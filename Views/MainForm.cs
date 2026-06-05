using Cognex.VisionPro;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TeslaNE42Vision2D.Entity;
using TeslaNE42Vision2D.Services;
using TeslaNE42Vision2D.Services.Camera;
using TeslaNE42Vision2D.Services.Vision;
using TeslaNE42Vision2D.Utils;

// 引入状态机枚举类型，便于直接使用 MachineState 和 StateEvent

namespace TeslaNE42Vision2D.Views
{
    public partial class MainForm : Form
    {
        private List<CameraView> _cameraViews = new List<CameraView>();
        private List<ICameraService> _cameras;

        private System.Windows.Forms.Timer _statusTimer;
        private const int MaxLogLines = 200;

        public List<ICameraService> Cameras
        {
            set => _cameras = value;
        }

        public MainForm()
        {
            InitializeComponent();
            BindEvents();
            SetupDynamicLayout();
            InitPositionControl();
        }

        private void InitPositionControl()
        {
            // 只读
            comboBoxPosition.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxPosition.Items.Clear();
            comboBoxPosition.Items.Add("left");
            comboBoxPosition.Items.Add("right");
            comboBoxPosition.Items.Add("all");

            comboBoxPosition.SelectedItem = "all";
        }

        private void SetupDynamicLayout()
        {
            _statusTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _statusTimer.Tick += StatusTimer_Tick;
            _statusTimer.Start();

            this.SizeChanged += (s, e) =>
            {
                int h = this.ClientSize.Height - 55 - 22;
                displayPanel.Height = h;
                displayPanel.Width = this.ClientSize.Width - 440;
                rightPanel.Location = new Point(this.ClientSize.Width - 440, 55);
                rightPanel.Height = h;
                rightPanel.Width = 440;
                logGroup.Size = new Size(rightPanel.Width - 20, rightPanel.Height - 200);
            };
        }

        public void RebuildDisplayGrid()
        {
            if (displayPanel == null) return;

            foreach (var view in _cameraViews)
            {
                try { view.Dispose(); } catch { }
            }
            _cameraViews.Clear();
            displayPanel.Controls.Clear();
            displayPanel.ColumnStyles.Clear();
            displayPanel.RowStyles.Clear();

            int count = RunDataService.Instance.AppConfigService.Config.CameraCount;
            if (count <= 0) count = 1;

            int cols = count <= 1 ? 1 : 2;
            int rows = (int)Math.Ceiling(count / (double)cols);

            displayPanel.ColumnCount = cols;
            for (int c = 0; c < cols; c++)
            {
                displayPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / cols));
            }

            displayPanel.RowCount = rows;
            for (int r = 0; r < rows; r++)
            {
                displayPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / rows));
            }

            var cameraConfigs = RunDataService.Instance.AppConfigService.Config.Cameras;

            for (int i = 0; i < count; i++)
            {
                CameraInfo cameraInfo = null;
                ICameraService cameraService = null;

                if (cameraConfigs != null && i < cameraConfigs.Count)
                {
                    cameraInfo = cameraConfigs[i];
                }

                if (_cameras != null && i < _cameras.Count)
                {
                    cameraService = _cameras[i];
                }

                var cameraView = new CameraView();
                cameraView.Dock = DockStyle.Fill;
                cameraView.Margin = new Padding(1);
                cameraView.SetCameraInfo(cameraInfo);
                cameraView.SetCameraService(cameraService);
                cameraView.SetCameraIndex(i);

                displayPanel.Controls.Add(cameraView, i % cols, i / cols);
                _cameraViews.Add(cameraView);
            }
        }

        private void BindEvents()
        {
            RunDataService.Instance.OnLogEvent += msg => AppendLog(msg);
            RunDataService.Instance.OnDetectResult += OnDetectResult;
            RunDataService.Instance.OnConnectionChanged += OnConnectionChanged;
            // 订阅状态机状态变化事件，实时更新界面状态显示
            RunDataService.Instance.OnStateChanged += OnStateChanged;
            // 订阅心跳状态变化事件
            RunDataService.Instance.OnHeartbeatStatusChanged += OnHeartbeatStatusChanged;
        }

        /// <summary>
        /// 状态机状态变化处理 - 实时更新界面状态显示
        /// 不同状态使用不同颜色显示，便于用户快速识别当前状态
        /// </summary>
        /// <param name="from">源状态</param>
        /// <param name="to">目标状态</param>
        /// <param name="evt">触发事件</param>
        private void OnStateChanged(MachineState from, MachineState to, StateEvent evt)
        {
            this.SafeInvoke(() =>
            {
                UpdateStateDisplay(to);
                AppendLog($"状态变化: {from} -> {to} [{evt}]");
            });
        }

        /// <summary>
        /// 更新状态显示 - 根据状态设置不同的显示颜色
        /// Preoperational: 灰色（初始/预运行）
        /// Operational/Ready: 绿色（正常运行）
        /// SingleExecution/ContinuousExecution: 蓝色（执行中）
        /// Error: 红色（错误）
        /// Halted: 黄色（暂停）
        /// </summary>
        /// <param name="state">当前状态</param>
        private void UpdateStateDisplay(MachineState state)
        {
            lblStateStatus.Text = $"状态: {GetStateDisplayName(state)}";

            // 根据状态设置颜色
            switch (state)
            {
                case MachineState.Preoperational:
                    lblStateStatus.ForeColor = Color.Gray;
                    break;
                case MachineState.Operational:
                case MachineState.Ready:
                    lblStateStatus.ForeColor = Color.DarkGreen;
                    break;
                case MachineState.SingleExecution:
                case MachineState.ContinuousExecution:
                    lblStateStatus.ForeColor = Color.DodgerBlue;
                    break;
                case MachineState.Error:
                    lblStateStatus.ForeColor = Color.Red;
                    break;
                case MachineState.Halted:
                    lblStateStatus.ForeColor = ColorOrange;
                    break;
                default:
                    lblStateStatus.ForeColor = SystemColors.ControlText;
                    break;
            }
        }

        /// <summary>
        /// 获取状态的中文显示名称
        /// </summary>
        /// <param name="state">状态枚举值</param>
        /// <returns>状态的中文显示名称</returns>
        private string GetStateDisplayName(MachineState state)
        {
            switch (state)
            {
                case MachineState.Preoperational:
                    return "预运行";
                case MachineState.Operational:
                    return "运行就绪";
                case MachineState.Ready:
                    return "就绪";
                case MachineState.SingleExecution:
                    return "单步执行";
                case MachineState.ContinuousExecution:
                    return "连续执行";
                case MachineState.Error:
                    return "错误";
                case MachineState.Halted:
                    return "暂停";
                default:
                    return state.ToString();
            }
        }

        // 橙色常量（WinForms 没有 Color.Orange）
        private static readonly Color ColorOrange = Color.FromArgb(255, 165, 0);

        private void OnDetectResult(DetectResultEntity result)
        {
            this.SafeInvoke(() =>
            {
                foreach( CameraView cameraView in _cameraViews)
                {
                    if(cameraView != null)
                    {
                        cameraView.DisplayRecord(null);
                        cameraView.DisplayImage(null);
                    }
                }

                if (result.InspectionOutputs != null)
                {
                    foreach (var output in result.InspectionOutputs)
                    {
                        if(output != null && output.ImagePolarity != null)
                        {
                            var view = _cameraViews.FirstOrDefault(x => x.CameraInfo.Name == output.Name);
                            if (view != null)
                            {
                                // view.DisplayImage(output.ImagePolarity);
                                view.DisplayRecord(output.CogRecord);
                            }
                        }
                    }
                  
                }

                lblLastResult.Text = result.OkNg ? "OK" : "NG";
                lblLastResult.ForeColor = result.OkNg ? Color.DarkGreen : Color.Red;
                lblOkCount.Text = $"OK: {result.OkCount}";
                lblNgCount.Text = $"NG: {result.NgCount}";
                ulong total = result.OkCount + result.NgCount;
                double ngRate = total > 0 ? (double)result.NgCount / total * 100 : 0;
                lblNgRate.Text = $"NG率: {ngRate:F2}%";
            });
        }

        private void OnConnectionChanged(bool connected)
        {
            this.SafeInvoke(() =>
            {
                lblPlcStatus.Text = connected ? "PLC: 已连接" : "PLC: 未连接 (重连中...)";
                lblPlcStatus.ForeColor = connected ? Color.DarkGreen : Color.Red;
            });
        }

        private void OnHeartbeatStatusChanged(ulong count, bool timeout)
        {
            this.SafeInvoke(() =>
            {
                lblHeartbeatStatus.Text = $"心跳: {count} {(timeout ? "超时" : "正常")}";
                lblHeartbeatStatus.ForeColor = timeout ? Color.Red : Color.DarkGreen;
            });
        }

        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            var device = RunDataService.Instance.ClientDevice;
            var state = device?.State;

            // 更新状态显示（使用统一的状态显示方法）
            if (state.HasValue)
            {
                UpdateStateDisplay(state.Value);
            }
            else
            {
                lblStateStatus.Text = "状态: 未初始化";
                lblStateStatus.ForeColor = Color.Gray;
            }

            // 更新磁盘空间显示
            try
            {
                double freeGb = DiskHelper.GetAvailableFreeSpaceGB(
                    RunDataService.Instance.AppConfigService.Config.ImageSavePath);
                lblDiskStatus.Text = $"磁盘: {freeGb:F1} GB";
                lblDiskStatus.ForeColor = freeGb < 5 ? Color.Red : SystemColors.ControlText;
            }
            catch { }
        }

        private void BtnAuto_Click(object sender, EventArgs e)
        {
            var sm = RunDataService.Instance.ClientDevice?.StateMachine;
            if (sm == null) return;
            bool ok = sm.TriggerContinuous();
            AppendLog(ok ? "已触发连续运行" : "当前状态无法触发连续运行");
        }

        private void BtnSingleRun_Click(object sender, EventArgs e)
        {
            var sm = RunDataService.Instance.ClientDevice?.StateMachine;
            if (sm == null) return;
            string posStr = "";
            if(RunDataService.Instance.TempPosition == PositionType.Left)
            {
                posStr = "10";
            }
            else if (RunDataService.Instance.TempPosition == PositionType.Right)
            {
                posStr = "01";
            }
            else
            {
                posStr = "11";
            }
            string[] strings = new string[10];
            double[] doubles = new double[10];
            strings[2] = posStr;
            //double[] doubles = new double[] { RunDataService.Instance.TempPosition };
            bool ok = sm.TriggerSingleJob(strings, doubles);
            AppendLog(ok ? "已触发单步运行" : "当前状态无法触发单步运行");
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            var sm = RunDataService.Instance.ClientDevice?.StateMachine;
            sm?.Trigger(Services.StateEvent.Stop);
            AppendLog("已发送停止指令");
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            var sm = RunDataService.Instance.ClientDevice?.StateMachine;
            sm?.Trigger(Services.StateEvent.Reset);
            AppendLog("已发送复位指令");
        }

        private void BtnCalibration_Click(object sender, EventArgs e)
        {
            using (var form = new CalibrationForm())
                form.ShowDialog(this);
        }

        private void BtnDebug_Click(object sender, EventArgs e)
        {
            var form = new DebugForm();
            form.Show(this);
        }

        private void BtnConfig_Click(object sender, EventArgs e)
        {
            using (var form = new ConfigForm())
                form.ShowDialog(this);
        }

        private void BtnClearLog_Click(object sender, EventArgs e)
        {
            lbLog.Items.Clear();
            AppendLog("日志已清空");
        }

        private void AppendLog(string message)
        {
            string line = $"[{DateTime.Now:HH:mm:ss.fff}] {message}";
            this.SafeInvoke(() =>
            {
                lbLog.Items.Add(line);
                if (lbLog.Items.Count > MaxLogLines)
                    lbLog.Items.RemoveAt(0);
                lbLog.TopIndex = lbLog.Items.Count - 1;
            });
            //LogHelper.Info(message);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RebuildDisplayGrid();
            AppendLog("系统已启动");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("确定退出？", "退出确认", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }
            _statusTimer?.Stop();
            RunDataService.Instance.ClientDevice?.Disconnect();
            DatabaseService.Instance.Dispose();

            base.OnFormClosing(e);
        }

        private void btnCalcToolBLock_Click(object sender, EventArgs e)
        {
            string path = RunDataService.Instance.AppConfigService.Config.CalcToolBlockPath;
            var form = new CalcToolBlockEditForm(RunDataService.Instance.CalcToolBlock, path);
            form.ShowDialog();
        }

        private void comboBoxPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = comboBoxPosition.SelectedItem.ToString();
            if(selectedValue == "left")
            {
                RunDataService.Instance.TempPosition = PositionType.Left;
            }
            else if(selectedValue == "right")
            {
                RunDataService.Instance.TempPosition = PositionType.Right;
            }
            else
            {
                RunDataService.Instance.TempPosition = PositionType.All;
            }
        }
    }

    internal static class ControlExtensions
    {
        public static void SafeInvoke(this Control control, Action action)
        {
            if (control.IsDisposed) return;
            if (control.InvokeRequired)
                control.Invoke(action);
            else
                action();
        }
    }
}