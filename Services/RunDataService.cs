using Cognex.VisionPro.ToolBlock;
using System;
using System.Collections.Generic;
using System.Threading;
using TeslaNE42Vision2D.Entity;
using TeslaNE42Vision2D.Services.Calibration;
using TeslaNE42Vision2D.Services.Vision;

namespace TeslaNE42Vision2D.Services
{
    public class RunDataService
    {
        private static readonly Lazy<RunDataService> _lazy =
            new Lazy<RunDataService>(() => new RunDataService());

        public static RunDataService Instance => _lazy.Value;

        private RunDataService() { }

        public event Action<string> OnLogEvent;
        public event Action<DetectResultEntity> OnDetectResult;
        public event Action<bool> OnConnectionChanged;

        /// <summary>
        /// 状态机状态变化事件 - 通知界面实时更新状态显示
        /// 参数：源状态、目标状态、触发事件
        /// </summary>
        public event Action<MachineState, MachineState, StateEvent> OnStateChanged;

        /// <summary>
        /// 心跳状态变化事件 - 通知界面更新心跳显示
        /// 参数：心跳接收计数、是否超时
        /// </summary>
        public event Action<ulong, bool> OnHeartbeatStatusChanged;

        public ConfigService<AppConfig> AppConfigService { get; } = new ConfigService<AppConfig>("app");
        public ConfigService<CalibConfig> CalibConfigService { get; } = new ConfigService<CalibConfig>("calibration");
        public NinePointCalibrationVisionProService CalibrationService { get; } = new NinePointCalibrationVisionProService();

        public CogToolBlock CalcToolBlock { get; set; } = null;

        private ClientDevice _clientDevice;
        public ClientDevice ClientDevice
        {
            get => _clientDevice;
            set
            {
                _clientDevice = value;
                if (_clientDevice != null)
                {
                    _clientDevice.OnLogEvent += msg => OnLogEvent?.Invoke(msg);
                    _clientDevice.OnDetectResultEvent += result => OnDetectResult?.Invoke(result);
                    _clientDevice.OnConnectionChanged += connected => OnConnectionChanged?.Invoke(connected);
                    // 订阅状态机状态变化事件，转发到界面
                    _clientDevice.StateMachine.OnStateChanged += (from, to, evt) => OnStateChanged?.Invoke(from, to, evt);
                    // 订阅心跳状态变化事件，转发到界面
                    _clientDevice.OnHeartbeatStatusChanged += (count, timeout) => OnHeartbeatStatusChanged?.Invoke(count, timeout);
                }
            }
        }

        private long _okCount = 0;
        private long _ngCount = 0;

        public ulong OkCount => (ulong)Interlocked.Read(ref _okCount);
        public ulong NgCount => (ulong)Interlocked.Read(ref _ngCount);

        public void IncrementOk() => Interlocked.Increment(ref _okCount);
        public void IncrementNg() => Interlocked.Increment(ref _ngCount);

        public Dictionary<string, IVisionService> VisionServices { get; set; }

        public PositionType TempPosition { get; set; }

        public void ResetCounts()
        {
            Interlocked.Exchange(ref _okCount, 0);
            Interlocked.Exchange(ref _ngCount, 0);
        }

        public void Initialize()
        {
            AppConfigService.Load();
            //CalibConfigService.Load();
            //CalibrationService.LoadFromConfig(CalibConfigService.Config);

            string dbPath = AppConfigService.Config.DatabasePath;
            if (!dbPath.StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase))
                dbPath = "Data Source=" + dbPath;
            DatabaseService.Instance.Initialize(dbPath);
        }
    }
}
