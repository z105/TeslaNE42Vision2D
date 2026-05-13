using System;
using System.Windows.Forms;
using TeslaNE42Vision2D.Entity;

namespace TeslaNE42Vision2D.Views
{
    public partial class CameraConfigForm : Form
    {
        private CameraInfo _cameraInfo;
        private int _cameraIndex;

        public CameraInfo UpdatedCameraInfo { get; private set; }

        public CameraConfigForm(CameraInfo info, int index)
        {
            InitializeComponent();
            _cameraInfo = info ?? new CameraInfo { Index = index };
            _cameraIndex = index;
            LoadCameraInfo();
        }

        private void LoadCameraInfo()
        {
            txtIndex.Text = _cameraIndex.ToString();
            txtName.Text = _cameraInfo.Name ?? "";
            txtSn.Text = _cameraInfo.Sn ?? "";
            numExposurePolarity.Value = (decimal)_cameraInfo.ExposurePolarity;
            numExposureBarcode.Value = (decimal)_cameraInfo.ExposureBarcode;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            _cameraInfo.Name = txtName.Text.Trim();
            _cameraInfo.Sn = txtSn.Text.Trim();
            _cameraInfo.ExposurePolarity = (float)numExposurePolarity.Value;
            _cameraInfo.ExposureBarcode = (float)numExposureBarcode.Value;
            UpdatedCameraInfo = _cameraInfo;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}