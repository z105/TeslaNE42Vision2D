using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.ToolBlock;
using Serilog;
using Stateless;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeslaNE42Vision2D.Entity;
using TeslaNE42Vision2D.Services.Camera;
using TeslaNE42Vision2D.Services.Vision;
using TeslaNE42Vision2D.Utils;

namespace TeslaNE42Vision2D.Services
{

    public enum MachineState
    {
        Preoperational,
        Error,
        Halted,
        Operational,
        Ready = 5,
        SingleExecution,
        ContinuousExecution,
    }


    public enum StateEvent
    {
        PrepareRecipe,
        UnprepareRecipe,
        StartSingleJob,
        StartContinuous,
        Halt,
        Reset,
        Stop,
        Abort,
        Error,
    }


    public class AppStateMachine
    {
        /// <summary>
        /// 日志事件 - 状态变化和操作信息通过此事件通知外部
        /// 订阅者：MainForm 日志列表、Serilog 日志记录
        /// </summary>
        public event Action<string> OnLogEvent;

        /// <summary>
        /// 检测结果事件 - 每次检测完成后通过此事件通知结果
        /// 订阅者：MainForm 检测结果显示、图像显示
        /// </summary>
        public event Action<DetectResultEntity> OnDetectResultEvent;

        /// <summary>
        /// 状态变化事件 - 状态转换完成后触发，通知源状态、目标状态和触发事件
        /// 订阅者：MainForm 状态显示、日志记录
        /// </summary>
        public event Action<MachineState, MachineState, StateEvent> OnStateChanged;

        /// <summary>
        /// 当前状态 - 状态机内部状态存储
        /// 通过状态机的 getter/setter 回调同步更新
        /// </summary>
        private MachineState _state = MachineState.Preoperational;

        /// <summary>
        /// 状态转换中标记 - 标识当前是否正在执行状态转换
        /// 用于防止转换过程中的重复触发
        /// </summary>
        private bool _isTransitioning;

        /// <summary>
        /// 错误触发器 - 带参数的错误事件触发器，携带错误消息
        /// 用于将错误信息传递到错误状态处理
        /// </summary>
        private StateMachine<MachineState, StateEvent>.TriggerWithParameters<string> _errorTrigger;

        /// <summary>
        /// 单步执行触发器 - 带参数的单步执行事件触发器
        /// 参数：触发源(Manual/Auto)、附加参数字典
        /// </summary>
        private StateMachine<MachineState, StateEvent>.TriggerWithParameters<string, Dictionary<string, object>> _startSingleJobTrigger;

        /// <summary>
        /// 连续执行触发器 - 带参数的连续执行事件触发器
        /// 参数：触发源(Manual/Auto)、附加参数字典
        /// </summary>
        private StateMachine<MachineState, StateEvent>.TriggerWithParameters<string, Dictionary<string, object>> _startContinuousJobTrigger;

        /// <summary>
        /// Stateless 状态机实例 - 管理状态转换逻辑
        /// 采用异步触发模式(FireAsync)，支持异步状态处理
        /// </summary>
        private readonly StateMachine<MachineState, StateEvent> _machine;

        /// <summary>
        /// 相机服务列表 - 由外部注入的实际相机或Mock相机
        /// 支持多相机同时拍照，图像结果保存在 DetectResultEntity.Images
        /// </summary>
        public List<ICameraService> Cameras { get; set; } = new List<ICameraService>();

        /// <summary>
        /// 视觉检测服务 - 调用 VisionPro SDK 执行视觉检测
        /// 返回像素坐标、条码、极性等信息
        /// </summary>
        public Dictionary<string, IVisionService> VisionServices { get; set; } = new Dictionary<string, IVisionService>();

        /// <summary>
        /// 九点标定服务 - 像素坐标到物理坐标的转换
        /// 通过九点标定矩阵实现坐标映射
        /// </summary>
        public Calibration.NinePointCalibrationVisionProService CalibrationService { get; set; }

        /// <summary>
        /// OK计数 - 线程安全的统计数据
        /// 使用 Interlocked 操作保证多线程安全
        /// </summary>
        private long _okCount = 0;

        /// <summary>
        /// NG计数 - 线程安全的统计数据
        /// 使用 Interlocked 操作保证多线程安全
        /// </summary>
        private long _ngCount = 0;

        /// <summary>
        /// 连续执行取消令牌 - 控制连续执行循环的终止
        /// 停止/中止操作通过取消此令牌中断执行循环
        /// </summary>
        private CancellationTokenSource _continuousCts;

        /// <summary>
        /// 单步执行触发源 - 用于日志记录区分触发来源
        /// </summary>
        private string _singleJobTriggerSource;

        /// <summary>
        /// 单步执行附加参数 - 可扩展参数字典
        /// </summary>
        private Dictionary<string, object> _singleJobParameters;

        /// <summary>
        /// 是否处于预运行状态
        /// </summary>
        public bool IsPreoperational => _machine.IsInState(MachineState.Preoperational);

        /// <summary>
        /// 是否处于错误状态
        /// </summary>
        public bool IsErrored => _machine.IsInState(MachineState.Error);

        /// <summary>
        /// 是否处于暂停状态
        /// </summary>
        public bool IsHalted => _machine.IsInState(MachineState.Halted);

        /// <summary>
        /// 是否处于运行就绪状态
        /// </summary>
        public bool IsOperational => _machine.IsInState(MachineState.Operational);

        /// <summary>
        /// 当前状态 - 供外部查询的状态值
        /// </summary>
        public MachineState State => _state;

        /// <summary>
        /// 是否正在执行状态转换
        /// </summary>
        public bool IsTransitioning => _isTransitioning;

        /// <summary>
        /// 构造函数 - 创建状态机实例并配置状态转换规则
        /// 状态机采用异步模式，所有状态转换通过 FireAsync 执行
        /// </summary>
        public AppStateMachine()
        {
            // 创建状态机，使用 getter/setter 回调管理内部状态
            // 这样可以在状态变化时同步更新 _state 字段
            _machine = new StateMachine<MachineState, StateEvent>(() => _state, s => _state = s);
            ConfigureStateMachine();
        }

        private void ConfigureStateMachine()
        {
            // 设置带参数的触发器，用于传递触发源和附加参数
            _startSingleJobTrigger = _machine.SetTriggerParameters<string, Dictionary<string, object>>(StateEvent.StartSingleJob);
            _startContinuousJobTrigger = _machine.SetTriggerParameters<string, Dictionary<string, object>>(StateEvent.StartContinuous);
            _errorTrigger = _machine.SetTriggerParameters<string>(StateEvent.Error);

            // 注册状态转换回调 - 转换开始和转换完成时执行
            _machine.OnTransitioned(TransitionedAction);
            _machine.OnTransitionCompleted(TransitionCompletedAction);

            // ===== 预运行状态配置 =====
            // 初始状态，等待配方准备完成
            _machine.Configure(MachineState.Preoperational)
                .PermitIf(StateEvent.PrepareRecipe, MachineState.Operational, () => true)  // 准备配方后进入运行就绪
                .Permit(StateEvent.Halt, MachineState.Halted)                              // 可手动暂停
                .Permit(StateEvent.Error, MachineState.Error)                              // 可进入错误状态
                .OnExit(TransitionStartedAction)                                           // 状态转换开始标记
                .OnExitFrom(StateEvent.PrepareRecipe, OnPrepareRecipe);                    // 准备配方完成处理

            // ===== 暂停状态配置 =====
            // 软件被暂停后的状态，只能复位恢复
            _machine.Configure(MachineState.Halted)
                .Permit(StateEvent.Reset, MachineState.Preoperational)    // 复位回到预运行
                .Permit(StateEvent.Error, MachineState.Error)             // 可进入错误状态
                .OnExit(TransitionStartedAction);

            // ===== 错误状态配置 =====
            // 发生异常时进入，需要人工复位
            _machine.Configure(MachineState.Error)
                .Permit(StateEvent.Reset, MachineState.Preoperational)                    // 复位回到预运行
                .OnExit(TransitionStartedAction)
                .OnExit(OnResetBlockInfo)                                                 // 复位时清空统计
                .OnEntryFrom(_errorTrigger, (message, trigger) => OnErrorEntry(message)); // 记录错误信息

            // ===== 运行就绪状态配置 =====
            // 配方已准备完成，可执行检测任务
            // 包含三个子状态：Ready、SingleExecution、ContinuousExecution
            _machine.Configure(MachineState.Operational)
                .Permit(StateEvent.UnprepareRecipe, MachineState.Preoperational)  // 卸载配方回到预运行
                .Permit(StateEvent.Halt, MachineState.Halted)                     // 可手动暂停
                .Permit(StateEvent.Error, MachineState.Error)                     // 可进入错误状态
                .OnEntry(OnOperationalEntry)                                       // 进入时记录日志
                .InitialTransition(MachineState.Ready)                             // 默认进入就绪子状态
                .OnExit(TransitionStartedAction)
                .OnExit(OnUnprepareRecipe);                                        // 退出时取消连续执行

            // ===== 就绪子状态配置 =====
            // 等待触发单步或连续执行
            _machine.Configure(MachineState.Ready)
                .SubstateOf(MachineState.Operational)                                                  // 作为 Operational 的子状态
                .Permit(StateEvent.StartSingleJob, MachineState.SingleExecution)                      // 可触发单步执行
                .Permit(StateEvent.StartContinuous, MachineState.ContinuousExecution)                 // 可触发连续执行
                .OnExit(TransitionStartedAction)
                .OnExitFrom(_startContinuousJobTrigger, OnStartContinuousExecution)                  // 连续执行开始处理
                .OnExitFrom(_startSingleJobTrigger, SaveSingleJobParameters);                         // 保存单步执行参数

            // ===== 单步执行子状态配置 =====
            // 执行一次检测后自动返回就绪
            _machine.Configure(MachineState.SingleExecution)
                .SubstateOf(MachineState.Operational)
                .Permit(StateEvent.Abort, MachineState.Ready)          // 中止返回就绪
                .Permit(StateEvent.Stop, MachineState.Ready)           // 停止返回就绪
                .OnEntry(TransitionStartedAction)
                .OnEntryAsync(OnSingleExecutionEntry)                  // 进入时执行检测
                .OnExit(TransitionStartedAction)
                .OnExitFrom(StateEvent.Stop, OnStop)                   // 停止处理
                .OnExitFrom(StateEvent.Abort, OnAbort);                 // 中止处理

            // ===== 连续执行子状态配置 =====
            // 循环执行直到收到停止/中止信号
            _machine.Configure(MachineState.ContinuousExecution)
                .SubstateOf(MachineState.Operational)
                .Permit(StateEvent.Abort, MachineState.Ready)          // 中止返回就绪
                .Permit(StateEvent.Stop, MachineState.Ready)           // 停止返回就绪
                .OnExit(TransitionStartedAction)
                .OnExitFrom(StateEvent.Stop, OnStop)                   // 停止处理
                .OnExitFrom(StateEvent.Abort, OnAbort);                // 中止处理
        }

        /// <summary>
        /// 触发状态转换 - 通用触发方法，用于触发不带参数的状态事件
        /// 使用 Task.Run 在后台线程触发，避免阻塞 UI 线程
        /// </summary>
        /// <param name="evt">要触发的事件类型</param>
        /// <returns>是否成功触发，若当前状态不允许该事件则返回 false</returns>
        public bool Trigger(StateEvent evt)
        {
            if (_machine.CanFire(evt))
            {
                // 在后台线程触发状态转换，避免 UI 线程被阻塞
                Task.Run(() => _machine.FireAsync(evt));
                return true;
            }
            WriteLog($"当前状态 {_state} 无法触发事件 {evt}");
            return false;
        }

        /// <summary>
        /// 触发单步执行 - 手动模式下执行一次检测
        /// 触发源标记为 "Manual"，用于日志记录区分
        /// 使用 Task.Run 在后台线程触发，避免阻塞 UI 线程
        /// </summary>
        /// <returns>是否成功触发</returns>
        public bool TriggerSingleJob(string[] stringParams, double[] doubleParams)
        {
            if (_machine.CanFire(StateEvent.StartSingleJob))
            {
                // 在后台线程触发状态转换，避免 UI 线程被阻塞
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("StringCommandParameters", stringParams);
                parameters.Add("LRealCommandParameters", doubleParams);
                Task.Run(() => _machine.FireAsync(_startSingleJobTrigger, "Manual", parameters));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 触发连续执行 - 自动模式下持续执行检测
        /// 触发源标记为 "Auto"，用于日志记录区分
        /// 使用 Task.Run 在后台线程触发，避免阻塞 UI 线程
        /// </summary>
        /// <returns>是否成功触发</returns>
        public bool TriggerContinuous()
        {
            if (_machine.CanFire(StateEvent.StartContinuous))
            {
                // 在后台线程触发状态转换，避免 UI 线程被阻塞
                Task.Run(() => _machine.FireAsync(_startContinuousJobTrigger, "Auto", new Dictionary<string, object>()));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 触发错误状态 - 将软件转入错误状态并记录错误信息
        /// 使用 Task.Run 在后台线程触发，避免阻塞 UI 线程
        /// </summary>
        /// <param name="message">错误描述信息</param>
        /// <returns>是否成功触发</returns>
        public bool TriggerError(string message)
        {
            if (_machine.CanFire(StateEvent.Error))
            {
                // 在后台线程触发状态转换，避免 UI 线程被阻塞
                Task.Run(() => _machine.FireAsync(_errorTrigger, message));
                return true;
            }
            return false;
        }

        #region State Actions - 状态动作处理

        /// <summary>
        /// 准备配方完成处理 - 配方加载成功后执行
        /// </summary>
        private void OnPrepareRecipe()
        {
            WriteLog("准备配方完成");
        }

        /// <summary>
        /// 进入运行就绪状态处理 - 记录状态变化日志
        /// </summary>
        private void OnOperationalEntry()
        {
            WriteLog("进入 Operational 状态");
        }

        /// <summary>
        /// 取消准备配方处理 - 退出运行就绪状态时执行
        /// 同时取消正在进行的连续执行任务
        /// </summary>
        private void OnUnprepareRecipe()
        {
            WriteLog("取消准备配方");
            _continuousCts?.Cancel();
        }

        /// <summary>
        /// 停止执行处理 - 正常结束检测任务
        /// 取消连续执行循环，等待当前检测完成
        /// </summary>
        private void OnStop()
        {
            WriteLog("停止执行");
            _continuousCts?.Cancel();
        }

        /// <summary>
        /// 中止执行处理 - 异常情况下紧急中断检测任务
        /// 立即取消连续执行循环
        /// </summary>
        private void OnAbort()
        {
            WriteLog("中止执行");
            _continuousCts?.Cancel();
        }

        /// <summary>
        /// 进入错误状态处理 - 记录错误信息
        /// </summary>
        /// <param name="message">错误描述</param>
        private void OnErrorEntry(string message)
        {
            WriteLog($"进入错误状态: {message}");
        }

        /// <summary>
        /// 复位状态信息处理 - 清空统计数据
        /// 使用 Interlocked.Exchange 确保线程安全
        /// </summary>
        private void OnResetBlockInfo()
        {
            WriteLog("重置状态信息");
            Interlocked.Exchange(ref _okCount, 0);
            Interlocked.Exchange(ref _ngCount, 0);
        }

        /// <summary>
        /// 开始连续执行处理 - 启动连续执行循环
        /// 创建取消令牌，后台运行检测循环
        /// </summary>
        /// <param name="triggerSource">触发源（Manual/Auto）</param>
        /// <param name="parameters">附加参数（可扩展）</param>
        private void OnStartContinuousExecution(string triggerSource, Dictionary<string, object> parameters)
        {
            WriteLog($"开始连续执行: {triggerSource}");
            _continuousCts = new CancellationTokenSource();
            _ = RunContinuousAsync(_continuousCts.Token);
        }

        /// <summary>
        /// 保存单步执行参数 - 在 Ready 状态退出时保存触发参数
        /// </summary>
        /// <param name="triggerSource">触发源（Manual/Auto）</param>
        /// <param name="parameters">附加参数（可扩展）</param>
        private void SaveSingleJobParameters(string triggerSource, Dictionary<string, object> parameters)
        {
            _singleJobTriggerSource = triggerSource;
            _singleJobParameters = parameters;
        }

        /// <summary>
        /// 单步执行入口 - 进入 SingleExecution 状态时执行
        /// 执行一次检测后自动返回就绪状态
        /// </summary>
        private async Task OnSingleExecutionEntry()
        {
            WriteLog($"开始单步执行: {_singleJobTriggerSource ?? "Unknown"}");
            await ExecuteOneDetectionAsync();
            // 执行完成后自动触发 Stop 事件返回就绪状态
            if (_machine.CanFire(StateEvent.Stop))
                await _machine.FireAsync(StateEvent.Stop);
        }

        /// <summary>
        /// 连续执行循环 - 循环执行检测直到收到取消信号
        /// 每次检测间隔 100ms，避免过度占用资源
        /// </summary>
        /// <param name="token">取消令牌</param>
        private async Task RunContinuousAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await ExecuteOneDetectionAsync();
                // 检测间隔 100ms，使用 ContinueWith 处理取消异常
                if (!token.IsCancellationRequested)
                    await Task.Delay(100, token).ContinueWith(_ => { });
            }
        }

        /// <summary>
        /// 执行一次检测 - 检测流程的核心方法
        /// 流程步骤：
        /// 1. 所有相机拍照获取图像
        /// 2. 视觉检测获取像素坐标、条码、极性
        /// 3. 坐标转换（像素→物理坐标）
        /// 4. 更新 OK/NG 统计计数
        /// 5. 保存图像到本地
        /// 6. 写入数据库记录
        /// 7. 通知 UI 更新显示
        /// </summary>
        private async Task ExecuteOneDetectionAsync()
        {
            DetectResultEntity result = new DetectResultEntity();
            try
            {

                List<SnapAndInspectionInput> inputs = new List<SnapAndInspectionInput>();

                string[] stringParameters = _singleJobParameters["StringCommandParameters"] as string[];
                double[] doubleParameters = _singleJobParameters["LRealCommandParameters"] as double[];

                PositionType positionType = PositionType.All;
                if (stringParameters[2] == "10")
                {
                    positionType = PositionType.Left;
                }
                else if(stringParameters[2] == "01")
                {
                    positionType = PositionType.Right;
                }
                else if (stringParameters[2] == "11")
                {
                    positionType = PositionType.All;
                }
                else
                {
                    positionType = PositionType.All;
                    //throw new Exception("error position string" + stringParameters[2]);
                }

                string posStr = GetPositionTypeStr(positionType);

                List<CameraInfo> cameraList = new List<CameraInfo>();
                if(positionType == PositionType.Left)
                {
                    cameraList = RunDataService.Instance.AppConfigService.Config.Cameras.Where(x => x.Position == "left").ToList();
                }
                else if(positionType == PositionType.Right)
                {
                    cameraList = RunDataService.Instance.AppConfigService.Config.Cameras.Where(x => x.Position == "right").ToList();
                }
                else
                {
                    cameraList = RunDataService.Instance.AppConfigService.Config.Cameras;
                }


                foreach (var camera in cameraList)
                {
                    
                    var cameraConfig = RunDataService.Instance.AppConfigService.Config.Cameras.FirstOrDefault(x => x.Name == camera.Name);
                    SnapAndInspectionInput input = new SnapAndInspectionInput();
                    input.CameraName = camera.Name;
                    input.CameraIndex = cameraConfig.Index;
                    input.CameraService = Cameras.FirstOrDefault(x => x.Name == camera.Name);
                    input.State = Convert.ToInt32(doubleParameters[0]);

                    inputs.Add(input);
                }


                // 为每个输入创建一个任务
                var tasks = inputs.Select(input => Task.Run(() => SnapAndDetect(input)));

                // 等待所有任务完成
                List<InspectionOutput> inspectionOutputs = (await Task.WhenAll(tasks)).ToList();

           

                result.InspectionOutputs = inspectionOutputs;

                SetInputs(RunDataService.Instance.CalcToolBlock, positionType, inspectionOutputs);
                RunDataService.Instance.CalcToolBlock.Inputs["PosNum"].Value = (int)doubleParameters[0];
                RunDataService.Instance.CalcToolBlock.Run();

                double physicalX = 0, physicalY = 0, physicalAngle = 0;
                try { physicalX = (double)RunDataService.Instance.CalcToolBlock.Outputs["X"].Value; } catch { }
                try { physicalY = (double)RunDataService.Instance.CalcToolBlock.Outputs["Y"].Value; } catch { }
                try { physicalAngle = (double)RunDataService.Instance.CalcToolBlock.Outputs["Q"].Value; } catch { }
                int resultCode = (int)RunDataService.Instance.CalcToolBlock.Outputs["Result"].Value;
                result.PhysicalX = physicalX;
                result.PhysicalY = physicalY;
                result.PhysicalAngle = physicalAngle;


                bool inspOk = resultCode == 1 ? true : false;

                //  更新统计计数 

                if (inspOk)
                    Interlocked.Increment(ref _okCount);
                else
                    Interlocked.Increment(ref _ngCount);

                result.OkNg = inspOk;
                result.OkCount = (ulong)Interlocked.Read(ref _okCount);
                result.NgCount = (ulong)Interlocked.Read(ref _ngCount);

                // ===== 步骤5: 保存图像 =====
                // 图像保存到本地文件夹，按 OK/NG 分类，文件名使用时间戳
                var config = RunDataService.Instance.AppConfigService.Config;
                //if (images.Length > 0 && images[0] != null)
                //{
                //    string imagePath = await Task.Run(() =>
                //        ImageSaveService.SaveImage(images[0], inspOk ? "OK" : "NG", config.ImageSavePath));

                //    // ===== 步骤6: 写入数据库 =====
                //    // 使用 FreeSql 写入 SQLite/SQL Server 数据库
                //    DatabaseService.Instance.InsertRecord(new Entity.DetectRecord
                //    {
                //        DetectTime = DateTime.Now,
                //        OkNg = inspOk,
                //        PhysicalX = result.PhysicalX,
                //        PhysicalY = result.PhysicalY,
                //        PhysicalAngle = result.PhysicalAngle,
                //        Barcode = barcode,
                //        Polarity = polarity,
                //        ImagePath = imagePath,
                //    });
                //}

                // ===== 步骤7: 通知 UI =====
                // 通过事件通知主界面更新检测结果和图像显示
                OnDetectResultEvent?.Invoke(result);
                LogResult(result);
                RunDataService.Instance.ClientDevice.SendDetectResult(result);
                WriteLog($"检测完成: {(inspOk ? "OK" : "NG")} | X={result.PhysicalX:F3} Y={result.PhysicalY:F3} Angle={result.PhysicalAngle:F3} Result={resultCode}");

                 //RunDataService.Instance.ClientDevice.SendDetectResult(result);
            }
            catch (Exception ex)
            {
                // 异常处理：记录日志并触发错误状态
                LogHelper.Error("检测执行异常", ex);
                WriteLog($"检测异常: {ex.Message}");
                TriggerError(ex.Message);
            }
        }

        private PositionType GetPositionType(string posStr)
        {
            if(posStr == "left")
            {
                return PositionType.Left;
            }

            if(posStr == "right")
            {
                return PositionType.Right;
            }

            return PositionType.All;
        }

        private string GetPositionTypeStr(PositionType type)
        {
            if(type == PositionType.Left )
            {
                return "left";
            }

            if(type == PositionType.Right )
            {
                return "right";
            }

            return "all";
        }

        private void LogResult(DetectResultEntity result)
        {
            var inspectionOutputs = result.InspectionOutputs.OrderBy(x => x.Index);
            foreach(var output in inspectionOutputs)
            {
                // barcode、polarity、像素坐标等信息
                if (output != null)
                {
                    WriteLog($"相机 {output.Name} 条码: {string.Join(", ", output.BarcodeList)}");
                    WriteLog($"相机 {output.Name} 极性: {string.Join(", ", output.PolarityList)}");
                    WriteLog($"相机 {output.Name} 坐标: X={string.Join(",", output.X.Select(x => x.ToString("f3")))}");
                    WriteLog($"相机 {output.Name} 坐标: Y={string.Join(",", output.Y.Select(y => y.ToString("f3")))}");
                }
            }

            WriteLog($"Calc: X={result.PhysicalX:F3} Y={result.PhysicalY:F3} Angle={result.PhysicalAngle:F2}");
        }

        /// <summary>
        /// positionType 为 Left 时，设置输入为 Camera1、Camera2、Camera3 的坐标值, 为 Right 时，设置输入为 Camera4、Camera5、Camera6 的坐标值
        /// </summary>
        /// <param name="toolBlock"></param>
        /// <param name="positionType"></param>
        /// <param name="name"></param>
        /// <param name="list"></param>
        private void SetInputs(CogToolBlock toolBlock, PositionType positionType, List<InspectionOutput> list)
        {
            List<string> leftCameraNames = RunDataService.Instance.AppConfigService.Config.Cameras
                .Where(x => x.Position == "left")
                .Select(x => x.Name)
                .ToList();

            // right
            List<string> rightCameraNames = RunDataService.Instance.AppConfigService.Config.Cameras
                .Where(x => x.Position == "right")
                .Select(x => x.Name)
                .ToList();

            // all 
            List<string> allCameraNames = RunDataService.Instance.AppConfigService.Config.Cameras
                .Select(x => x.Name)
                .ToList();

            //clear 
            foreach (var cameraName in allCameraNames)
            {
                var config = RunDataService.Instance.AppConfigService.Config.Cameras.FirstOrDefault(x => x.Name == cameraName);
                var output = list.FirstOrDefault(x => x.Name == cameraName);
                if (config != null)
                {
                    toolBlock.Inputs[$"{config.VpNameX}"].Value = new double[] { 0,0,0,0};
                    toolBlock.Inputs[$"{config.VpNameY}"].Value = new double[] { 0, 0, 0, 0 };
                }
            }

            if (positionType == PositionType.Left)
            {
                foreach (var cameraName in leftCameraNames)
                {
                    var config = RunDataService.Instance.AppConfigService.Config.Cameras.FirstOrDefault(x => x.Name == cameraName);
                    var output = list.FirstOrDefault(x => x.Name == cameraName);
                    if (config != null)
                    {
                        toolBlock.Inputs[$"{config.VpNameX}"].Value = output.X;
                        toolBlock.Inputs[$"{config.VpNameY}"].Value = output.Y;
                    }
                }
            }

            if(positionType == PositionType.Right)
            {
                foreach (var cameraName in rightCameraNames)
                {
                    var config = RunDataService.Instance.AppConfigService.Config.Cameras.FirstOrDefault(x => x.Name == cameraName);
                    var output = list.FirstOrDefault(x => x.Name == cameraName);
                    if (config != null)
                    {
                        toolBlock.Inputs[$"{config.VpNameX}"].Value = output.X;
                        toolBlock.Inputs[$"{config.VpNameY}"].Value = output.Y;
                    }
                }
            }

            if(positionType ==  PositionType.All)
            {
                foreach (var cameraName in allCameraNames)
                {
                    var config = RunDataService.Instance.AppConfigService.Config.Cameras.FirstOrDefault(x => x.Name == cameraName);
                    var output = list.FirstOrDefault(x => x.Name == cameraName);
                    if (config != null)
                    {
                        toolBlock.Inputs[$"{config.VpNameX}"].Value = output.X;
                        toolBlock.Inputs[$"{config.VpNameY}"].Value = output.Y;
                    }
                }
            }


        }

        private string GetImagePath(string camera, string type)
        {
            string today = DateTime.Now.ToString("yyyyMMdd");
            string dir = RunDataService.Instance.AppConfigService.Config.ImageSavePath;
            string fullDir = Path.Combine(dir, today, type, camera);
            if(Directory.Exists(fullDir) == false)
            {
                Directory.CreateDirectory(fullDir);
            }
            string name = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}_{Guid.NewGuid().ToString().Replace("-", "")}.png";
            
            return Path.Combine(fullDir, name) ;
        }

        /// <summary>
        /// 线程中存图
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageName"></param>
        private void SaveOriginImage(ICogImage image, string fullPath)
        {
            Task.Run(() =>
            {

                try
                {
                    // 1. 入参校验
                    if (image == null) return;

                    // 3. 使用CogImageFileTool保存
                    using (var imageFileTool = new CogImageFileTool())
                    {
                        imageFileTool.InputImage = image; 
                        imageFileTool.Operator.Open(fullPath, CogImageFileModeConstants.Write);
                        imageFileTool.Run(); // 执行保存
                    }



                }
                catch (Exception ex)
                {
                    Log.Error($"保存图片失败：{fullPath} {ex}");
                }
            });

        }

        private InspectionOutput SnapAndDetect(SnapAndInspectionInput input)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Transition Callbacks - 状态转换回调

        /// <summary>
        /// 状态转换动作 - 状态转换开始时执行
        /// 记录转换信息：源状态 → 目标状态 [触发事件]
        /// </summary>
        /// <param name="t">转换信息对象</param>
        private void TransitionedAction(StateMachine<MachineState, StateEvent>.Transition t)
        {
            WriteLog($"状态转换: {t.Source} -> {t.Destination} [{t.Trigger}]");
        }

        /// <summary>
        /// 状态转换完成动作 - 状态转换完成后执行
        /// 清除转换中标记，触发 OnStateChanged 事件通知外部
        /// </summary>
        /// <param name="t">转换信息对象</param>
        private void TransitionCompletedAction(StateMachine<MachineState, StateEvent>.Transition t)
        {
            _isTransitioning = false;
            // 通知外部状态变化，供 UI 显示状态
            OnStateChanged?.Invoke(t.Source, t.Destination, t.Trigger);
            WriteLog($"状态切换完成: {t.Source} -> {t.Destination}");
        }

        /// <summary>
        /// 状态转换开始动作 - 设置转换中标记
        /// 用于防止转换过程中的重复触发
        /// </summary>
        /// <param name="t">转换信息对象</param>
        private void TransitionStartedAction(StateMachine<MachineState, StateEvent>.Transition t)
        {
            _isTransitioning = true;
        }

        #endregion

        /// <summary>
        /// 写入日志 - 同时输出到 Serilog 日志和 OnLogEvent 事件
        /// Serilog 用于文件日志记录，OnLogEvent 用于界面日志显示
        /// </summary>
        /// <param name="message">日志消息</param>
        private void WriteLog(string message)
        {
            //LogHelper.Info(message);
            OnLogEvent?.Invoke(message);
        }
    }
}
