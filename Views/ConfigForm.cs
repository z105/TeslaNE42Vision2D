using System;
using System.Windows.Forms;
using TeslaNE42Vision2D.Services;

namespace TeslaNE42Vision2D.Views
{
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();
            LoadConfig();
        }

        private void LoadConfig()
        {
            var config = RunDataService.Instance.AppConfigService.Config;
            txtIp.Text = config.Ip;
            txtPort.Text = config.Port.ToString();
            txtMachineId.Text = config.MachineID;
            txtCameraCount.Text = config.CameraCount.ToString();
            chkUseMock.Checked = config.UseMock;
            txtMockImageFolder.Text = config.MockImageFolder;
            txtImageSavePath.Text = config.ImageSavePath;
            txtVisionProToolBlockPath.Text = config.CalcToolBlockPath;
            txtDatabasePath.Text = config.DatabasePath;
            txtAdminPassword.Text = config.AdminPassword;
            txtImageRetainDays.Text = config.ImageRetainDays.ToString();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!PasswordDialog.Verify(RunDataService.Instance.AppConfigService.Config.AdminPassword, this))
            {
                MessageBox.Show("密码错误，无法保存", "权限验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtPort.Text, out int port) || port <= 0 || port > 65535)
            {
                MessageBox.Show("端口号无效（1-65535）", "输入错误");
                txtPort.Focus();
                return;
            }

            if (!int.TryParse(txtCameraCount.Text, out int camCount) || camCount < 1 || camCount > 8)
            {
                MessageBox.Show("相机数量无效（1-8）", "输入错误");
                txtCameraCount.Focus();
                return;
            }

            if (!int.TryParse(txtImageRetainDays.Text, out int retainDays) || retainDays < 1)
            {
                MessageBox.Show("图像保留天数无效（≥1）", "输入错误");
                txtImageRetainDays.Focus();
                return;
            }

            var config = RunDataService.Instance.AppConfigService.Config;
            config.Ip = txtIp.Text.Trim();
            config.Port = port;
            config.MachineID = txtMachineId.Text.Trim();
            config.CameraCount = camCount;
            config.UseMock = chkUseMock.Checked;
            config.MockImageFolder = txtMockImageFolder.Text.Trim();
            config.ImageSavePath = txtImageSavePath.Text.Trim();
            config.CalcToolBlockPath = txtVisionProToolBlockPath.Text.Trim();
            config.DatabasePath = txtDatabasePath.Text.Trim();
            config.AdminPassword = txtAdminPassword.Text;
            config.ImageRetainDays = retainDays;

            RunDataService.Instance.AppConfigService.Save();
            MessageBox.Show("配置已保存，部分设置重启后生效", "成功");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}