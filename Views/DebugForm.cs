using Cognex.VisionPro;
using System;
using System.Windows.Forms;
using TeslaNE42Vision2D.Entity;
using TeslaNE42Vision2D.Services;
using TeslaNE42Vision2D.Services.Vision;
using TeslaNE42Vision2D.Utils;

namespace TeslaNE42Vision2D.Views
{
    public partial class DebugForm : Form
    {
        private CogRecordDisplay _cogDisplay;
        private ICogImage _lastImage;
        private DetectResultEntity _lastResult;

        public DebugForm()
        {
            InitializeComponent();
            InitializeCogDisplay();
        }

        private void InitializeCogDisplay()
        {
            try
            {
                _cogDisplay = new CogRecordDisplay
                {
                    Dock = DockStyle.Fill,
                };
                displayPanel.Controls.Add(_cogDisplay);
            }
            catch
            {
                var lbl = new Label
                {
                    Text = "VisionPro显示控件不可用",
                    Dock = DockStyle.Fill,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                };
                displayPanel.Controls.Add(lbl);
            }
        }

        private async void BtnSnap_Click(object sender, EventArgs e)
        {
            SetButtonsEnabled(false);
            try
            {
                var device = RunDataService.Instance.ClientDevice;
                if (device == null || device.StateMachine.Cameras.Count == 0)
                {
                    AppendLog("相机未初始化");
                    return;
                }

                var camera = device.StateMachine.Cameras[0];
                AppendLog("拍照中...");
                _lastImage = await System.Threading.Tasks.Task.Run(() => camera.Snap());
                AppendLog("拍照完成");

                if (_cogDisplay != null && _lastImage != null)
                {
                    _cogDisplay.Image = _lastImage;
                    _cogDisplay.Fit(true);
                }
            }
            catch (Exception ex)
            {
                AppendLog("拍照失败: " + ex.Message);
            }
            finally
            {
                SetButtonsEnabled(true);
            }
        }

        private async void BtnDetect_Click(object sender, EventArgs e)
        {
            if (_lastImage == null)
            {
                MessageBox.Show("请先拍照", "提示");
                return;
            }

            SetButtonsEnabled(false);
            try
            {
                var device = RunDataService.Instance.ClientDevice;
                var visionService = device?.StateMachine.VisionServices;
                if (visionService == null)
                {
                    AppendLog("视觉服务未初始化");
                    return;
                }

                AppendLog("检测中...");
                double px = 0, py = 0, angle = 0;
                string barcode = string.Empty, polarity = string.Empty;
                bool ok = false;

                await System.Threading.Tasks.Task.Run(() =>
                {
                    SnapAndInspectionInput inspectionInput = new SnapAndInspectionInput();
                    //ok = visionService.(inspectionInput);
                });

                _lastResult = new DetectResultEntity
                {
                    OkNg = ok,
                    PhysicalAngle = angle,
                };

                // 坐标转换
                var calibService = RunDataService.Instance.CalibrationService;
                if (ok && calibService != null && calibService.IsCalibrated)
                {
                    var (wx, wy) = calibService.Transform(px, py);
                    _lastResult.PhysicalX = wx;
                    _lastResult.PhysicalY = wy;
                }

                UpdateResultLabel();
                AppendLog($"检测结果: {(ok ? "OK" : "NG")} | 像素({px:F2},{py:F2}) | 物理({_lastResult.PhysicalX:F3},{_lastResult.PhysicalY:F3}) | 角度={angle:F3}");
                if (!string.IsNullOrEmpty(barcode)) AppendLog($"条码: {barcode}");
                if (!string.IsNullOrEmpty(polarity)) AppendLog($"极性: {polarity}");
            }
            catch (Exception ex)
            {
                AppendLog("检测失败: " + ex.Message);
            }
            finally
            {
                SetButtonsEnabled(true);
            }
        }

        private async void BtnSnapAndDetect_Click(object sender, EventArgs e)
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                this.Invoke((Action)(() => BtnSnap_Click(sender, e)));
            });
            BtnSnap_Click(sender, e);
        }

        private void BtnSendResult_Click(object sender, EventArgs e)
        {
            if (_lastResult == null)
            {
                MessageBox.Show("没有可发送的检测结果", "提示");
                return;
            }

            var device = RunDataService.Instance.ClientDevice;
            if (device == null || !device.IsConnected)
            {
                AppendLog("PLC 未连接");
                return;
            }

            bool sent = device.SendDetectResult(_lastResult);
            AppendLog(sent ? "检测结果已发送到 PLC" : "发送失败");
        }

        private void UpdateResultLabel()
        {
            if (_lastResult == null) return;
            lblResult.Text =
                $"判断: {(_lastResult.OkNg ? "OK ✓" : "NG ✗")}\n" +
                $"物理X: {_lastResult.PhysicalX:F3}\n" +
                $"物理Y: {_lastResult.PhysicalY:F3}\n" +
                $"角度: {_lastResult.PhysicalAngle:F3}";
            lblResult.ForeColor = _lastResult.OkNg ? System.Drawing.Color.DarkGreen : System.Drawing.Color.Red;
        }

        private void AppendLog(string msg)
        {
            string line = $"[{DateTime.Now:HH:mm:ss}] {msg}";
            if (rtbLog.InvokeRequired)
                rtbLog.Invoke((Action)(() => AppendLogDirect(line)));
            else
                AppendLogDirect(line);
        }

        private void AppendLogDirect(string line)
        {
            rtbLog.AppendText(line + "\n");
            rtbLog.ScrollToCaret();
        }

        private void SetButtonsEnabled(bool enabled)
        {
            void Set()
            {
                btnSnap.Enabled = enabled;
                btnDetect.Enabled = enabled;
                btnSnapAndDetect.Enabled = enabled;
                btnSendResult.Enabled = enabled;
            }

            if (this.InvokeRequired) this.Invoke((Action)Set);
            else Set();
        }
    }
}