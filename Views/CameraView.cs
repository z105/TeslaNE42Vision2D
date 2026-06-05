using Cognex.VisionPro;
using System;
using System.Linq;
using System.Windows.Forms;
using TeslaNE42Vision2D.Entity;
using TeslaNE42Vision2D.Services;
using TeslaNE42Vision2D.Services.Camera;
using TeslaNE42Vision2D.Services.Vision;

namespace TeslaNE42Vision2D.Views
{
    public partial class CameraView : UserControl
    {
        private CogRecordDisplay _cogDisplay;
        private CameraInfo _cameraInfo;
        public CameraInfo CameraInfo { get => _cameraInfo; }

        private ICameraService _cameraService;
        private int _cameraIndex;

        public event Action<int> OnCaptureRequested;

        public CameraView()
        {
            InitializeComponent();
            InitializeCogDisplay();
            lblExposureInfo.Visible = false;
        }

        private void InitializeCogDisplay()
        {
            try
            {
                _cogDisplay = new CogRecordDisplay
                {
                    Dock = System.Windows.Forms.DockStyle.Fill,
                    BackColor = System.Drawing.Color.Black,
                };
                panelImageView.Controls.Add(_cogDisplay);
            }
            catch (Exception ex)
            {
                var placeholder = new System.Windows.Forms.Label
                {
                    Text = "图像显示区域\n(VisionPro 未加载)",
                    Dock = System.Windows.Forms.DockStyle.Fill,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    ForeColor = System.Drawing.Color.Gray,
                    BackColor = System.Drawing.Color.Black,
                };
                panelImageView.Controls.Add(placeholder);
            }
        }

        public void SetCameraInfo(CameraInfo info)
        {
            _cameraInfo = info;
            UpdateDisplay();
        }

        public void SetCameraService(ICameraService service)
        {
            _cameraService = service;
        }

        public void SetCameraIndex(int index)
        {
            _cameraIndex = index;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (_cameraInfo != null)
            {
                lblCameraName.Text = string.IsNullOrEmpty(_cameraInfo.Name)
                    ? $"相机 {_cameraIndex + 1}"
                    : _cameraInfo.Name;
                //lblExposureInfo.Text = $"曝光: 极性={_cameraInfo.ExposurePolarity:F1}, 条码={_cameraInfo.ExposureBarcode:F1}";
            }
            else
            {
                lblCameraName.Text = $"相机 {_cameraIndex + 1}";
                lblExposureInfo.Text = "曝光: --";
            }
        }

        public void DisplayImage(ICogImage image)
        {
            if (_cogDisplay != null)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => DisplayImage(image)));
                    return;
                }
                _cogDisplay.Image = image;
                _cogDisplay.Fit(false);
            }
        }

        public void DisplayRecord(ICogRecord record)
        {
            if (_cogDisplay != null)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => DisplayRecord(record)));
                    return;
                }
                _cogDisplay.Record = record;
                _cogDisplay.MaintainImageRegion = true;
                //_cogDisplay.AutoFit = true;
                _cogDisplay.Fit(false);
            }
        }

        private void BtnCapture_Click(object sender, EventArgs e)
        {
            if (_cameraService != null)
            {
                try
                {
                    var image = _cameraService.Snap();
                    if (image != null)
                    {
                        DisplayImage(image);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(
                        $"拍照失败: {ex.Message}",
                        "错误",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            else
            {
                OnCaptureRequested?.Invoke(_cameraIndex);
            }
        }

        private void BtnConfig_Click(object sender, EventArgs e)
        {
            using (var form = new CameraConfigForm(_cameraInfo, _cameraIndex))
            {
                if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    _cameraInfo = form.UpdatedCameraInfo;
                    var config = RunDataService.Instance.AppConfigService.Config.Cameras.FirstOrDefault(x => x.Index == _cameraIndex);
                    if (config != null)
                    {
                        config.Name = _cameraInfo.Name;
                        config.Sn = _cameraInfo.Sn;
                        config.ExposureBarcode = _cameraInfo.ExposureBarcode;
                        config.ExposurePolarity = _cameraInfo.ExposurePolarity;
                        RunDataService.Instance.AppConfigService.Save();
                    }
                    UpdateDisplay();
                }
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (_cogDisplay != null)
            {
                try { _cogDisplay.Dispose(); } catch { }
                _cogDisplay = null;
            }
            base.OnHandleDestroyed(e);
        }

        private void btnToolBlock_Click(object sender, EventArgs e)
        {
            IVisionService visionService = RunDataService.Instance.VisionServices[_cameraInfo.Name];
            var form = new ToolBlockEditForm(visionService.CogToolBlock, _cameraInfo.ToolBlockPath);
            form.ShowDialog();
   
        }
    }
}