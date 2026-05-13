namespace TeslaNE42Vision2D.Views
{
    partial class SplashForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Panel panelMain;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelMain = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();

            // panelMain
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.panelMain.Controls.Add(this.lblTitle);
            this.panelMain.Controls.Add(this.lblStatus);
            this.panelMain.Controls.Add(this.progressBar);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(400, 180);
            this.panelMain.TabIndex = 0;

            // lblTitle
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(360, 35);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "TeslaNE42 视觉引导系统";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblStatus
            this.lblStatus.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.lblStatus.Location = new System.Drawing.Point(20, 70);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(360, 30);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "正在初始化...";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // progressBar
            this.progressBar.Location = new System.Drawing.Point(40, 110);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(320, 30);
            this.progressBar.TabIndex = 2;
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;

            // SplashForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 180);
            this.Controls.Add(this.panelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SplashForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "加载中";
            this.ResumeLayout(false);
        }
    }
}