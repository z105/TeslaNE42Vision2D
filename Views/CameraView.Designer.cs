namespace TeslaNE42Vision2D.Views
{
    partial class CameraView
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelToobar = new System.Windows.Forms.FlowLayoutPanel();
            this.lblCameraName = new System.Windows.Forms.Label();
            this.btnCapture = new System.Windows.Forms.Button();
            this.btnConfig = new System.Windows.Forms.Button();
            this.lblExposureInfo = new System.Windows.Forms.Label();
            this.panelImageView = new System.Windows.Forms.Panel();
            this.btnToolBlock = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelToobar.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelToobar, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelImageView, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(717, 378);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panelToobar
            // 
            this.panelToobar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.panelToobar.Controls.Add(this.lblCameraName);
            this.panelToobar.Controls.Add(this.btnCapture);
            this.panelToobar.Controls.Add(this.btnConfig);
            this.panelToobar.Controls.Add(this.btnToolBlock);
            this.panelToobar.Controls.Add(this.lblExposureInfo);
            this.panelToobar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelToobar.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panelToobar.Location = new System.Drawing.Point(5, 5);
            this.panelToobar.Name = "panelToobar";
            this.panelToobar.Padding = new System.Windows.Forms.Padding(5);
            this.panelToobar.Size = new System.Drawing.Size(114, 368);
            this.panelToobar.TabIndex = 1;
            // 
            // lblCameraName
            // 
            this.lblCameraName.AutoSize = true;
            this.lblCameraName.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.lblCameraName.ForeColor = System.Drawing.Color.White;
            this.lblCameraName.Location = new System.Drawing.Point(10, 15);
            this.lblCameraName.Margin = new System.Windows.Forms.Padding(5, 10, 0, 10);
            this.lblCameraName.Name = "lblCameraName";
            this.lblCameraName.Size = new System.Drawing.Size(50, 19);
            this.lblCameraName.TabIndex = 0;
            this.lblCameraName.Text = "相机 1";
            // 
            // btnCapture
            // 
            this.btnCapture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnCapture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCapture.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnCapture.ForeColor = System.Drawing.Color.White;
            this.btnCapture.Location = new System.Drawing.Point(10, 49);
            this.btnCapture.Margin = new System.Windows.Forms.Padding(5);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(92, 32);
            this.btnCapture.TabIndex = 1;
            this.btnCapture.Text = "拍照";
            this.btnCapture.UseVisualStyleBackColor = false;
            this.btnCapture.Click += new System.EventHandler(this.BtnCapture_Click);
            // 
            // btnConfig
            // 
            this.btnConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(120)))));
            this.btnConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfig.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnConfig.ForeColor = System.Drawing.Color.White;
            this.btnConfig.Location = new System.Drawing.Point(10, 91);
            this.btnConfig.Margin = new System.Windows.Forms.Padding(5);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(92, 32);
            this.btnConfig.TabIndex = 2;
            this.btnConfig.Text = "相机配置";
            this.btnConfig.UseVisualStyleBackColor = false;
            this.btnConfig.Click += new System.EventHandler(this.BtnConfig_Click);
            // 
            // lblExposureInfo
            // 
            this.lblExposureInfo.AutoSize = true;
            this.lblExposureInfo.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lblExposureInfo.ForeColor = System.Drawing.Color.LightGray;
            this.lblExposureInfo.Location = new System.Drawing.Point(10, 180);
            this.lblExposureInfo.Margin = new System.Windows.Forms.Padding(5, 10, 0, 0);
            this.lblExposureInfo.Name = "lblExposureInfo";
            this.lblExposureInfo.Size = new System.Drawing.Size(45, 16);
            this.lblExposureInfo.TabIndex = 3;
            this.lblExposureInfo.Text = "曝光: --";
            // 
            // panelImageView
            // 
            this.panelImageView.BackColor = System.Drawing.Color.Black;
            this.panelImageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImageView.Location = new System.Drawing.Point(125, 5);
            this.panelImageView.Name = "panelImageView";
            this.panelImageView.Size = new System.Drawing.Size(587, 368);
            this.panelImageView.TabIndex = 0;
            // 
            // btnToolBlock
            // 
            this.btnToolBlock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(120)))));
            this.btnToolBlock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToolBlock.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnToolBlock.ForeColor = System.Drawing.Color.White;
            this.btnToolBlock.Location = new System.Drawing.Point(10, 133);
            this.btnToolBlock.Margin = new System.Windows.Forms.Padding(5);
            this.btnToolBlock.Name = "btnToolBlock";
            this.btnToolBlock.Size = new System.Drawing.Size(92, 32);
            this.btnToolBlock.TabIndex = 4;
            this.btnToolBlock.Text = "ToolBlock";
            this.btnToolBlock.UseVisualStyleBackColor = false;
            this.btnToolBlock.Click += new System.EventHandler(this.btnToolBlock_Click);
            // 
            // CameraView
            // 
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CameraView";
            this.Size = new System.Drawing.Size(717, 378);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelToobar.ResumeLayout(false);
            this.panelToobar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelImageView;
        private System.Windows.Forms.FlowLayoutPanel panelToobar;
        private System.Windows.Forms.Label lblCameraName;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.Label lblExposureInfo;
        private System.Windows.Forms.Button btnToolBlock;
    }
}