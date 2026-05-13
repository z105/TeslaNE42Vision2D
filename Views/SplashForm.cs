using System;
using System.Windows.Forms;

namespace TeslaNE42Vision2D.Views
{
    /// <summary>
    /// 加载等待窗口 - 在系统启动时显示，显示初始化进度
    /// </summary>
    public partial class SplashForm : Form
    {
        /// <summary>
        /// 更新加载状态信息
        /// </summary>
        public void UpdateStatus(string status)
        {
            if (this.IsDisposed) return;
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(UpdateStatus), status);
            }
            else
            {
                lblStatus.Text = status;
            }
        }

        /// <summary>
        /// 更新进度条
        /// </summary>
        public void UpdateProgress(int value, int maximum = 100)
        {
            if (this.IsDisposed) return;
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<int, int>(UpdateProgress), value, maximum);
            }
            else
            {
                progressBar.Maximum = maximum;
                progressBar.Value = Math.Min(value, maximum);
            }
        }

        public SplashForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // 禁用关闭按钮
            this.ControlBox = false;
        }
    }
}