namespace TeslaNE42Vision2D.Views
{
    partial class ConfigForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblIp;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblMachineId;
        private System.Windows.Forms.TextBox txtMachineId;
        private System.Windows.Forms.Label lblCameraCount;
        private System.Windows.Forms.TextBox txtCameraCount;
        private System.Windows.Forms.Label lblUseMock;
        private System.Windows.Forms.CheckBox chkUseMock;
        private System.Windows.Forms.Label lblMockImageFolder;
        private System.Windows.Forms.TextBox txtMockImageFolder;
        private System.Windows.Forms.Label lblImageSavePath;
        private System.Windows.Forms.TextBox txtImageSavePath;
        private System.Windows.Forms.Label lblVisionProToolBlockPath;
        private System.Windows.Forms.TextBox txtVisionProToolBlockPath;
        private System.Windows.Forms.Label lblDatabasePath;
        private System.Windows.Forms.TextBox txtDatabasePath;
        private System.Windows.Forms.Label lblAdminPassword;
        private System.Windows.Forms.TextBox txtAdminPassword;
        private System.Windows.Forms.Label lblImageRetainDays;
        private System.Windows.Forms.TextBox txtImageRetainDays;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

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
            this.lblIp = new System.Windows.Forms.Label();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lblMachineId = new System.Windows.Forms.Label();
            this.txtMachineId = new System.Windows.Forms.TextBox();
            this.lblCameraCount = new System.Windows.Forms.Label();
            this.txtCameraCount = new System.Windows.Forms.TextBox();
            this.lblUseMock = new System.Windows.Forms.Label();
            this.chkUseMock = new System.Windows.Forms.CheckBox();
            this.lblMockImageFolder = new System.Windows.Forms.Label();
            this.txtMockImageFolder = new System.Windows.Forms.TextBox();
            this.lblImageSavePath = new System.Windows.Forms.Label();
            this.txtImageSavePath = new System.Windows.Forms.TextBox();
            this.lblVisionProToolBlockPath = new System.Windows.Forms.Label();
            this.txtVisionProToolBlockPath = new System.Windows.Forms.TextBox();
            this.lblDatabasePath = new System.Windows.Forms.Label();
            this.txtDatabasePath = new System.Windows.Forms.TextBox();
            this.lblAdminPassword = new System.Windows.Forms.Label();
            this.txtAdminPassword = new System.Windows.Forms.TextBox();
            this.lblImageRetainDays = new System.Windows.Forms.Label();
            this.txtImageRetainDays = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblIp
            // 
            this.lblIp.AutoSize = true;
            this.lblIp.Location = new System.Drawing.Point(10, 12);
            this.lblIp.Name = "lblIp";
            this.lblIp.Size = new System.Drawing.Size(77, 12);
            this.lblIp.TabIndex = 0;
            this.lblIp.Text = "PLC IP 地址:";
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(154, 10);
            this.txtIp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(292, 21);
            this.txtIp.TabIndex = 1;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(10, 36);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(59, 12);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "PLC 端口:";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(154, 34);
            this.txtPort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(292, 21);
            this.txtPort.TabIndex = 3;
            // 
            // lblMachineId
            // 
            this.lblMachineId.AutoSize = true;
            this.lblMachineId.Location = new System.Drawing.Point(10, 60);
            this.lblMachineId.Name = "lblMachineId";
            this.lblMachineId.Size = new System.Drawing.Size(53, 12);
            this.lblMachineId.TabIndex = 4;
            this.lblMachineId.Text = "机器 ID:";
            // 
            // txtMachineId
            // 
            this.txtMachineId.Location = new System.Drawing.Point(154, 58);
            this.txtMachineId.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtMachineId.Name = "txtMachineId";
            this.txtMachineId.Size = new System.Drawing.Size(292, 21);
            this.txtMachineId.TabIndex = 5;
            // 
            // lblCameraCount
            // 
            this.lblCameraCount.AutoSize = true;
            this.lblCameraCount.Location = new System.Drawing.Point(10, 84);
            this.lblCameraCount.Name = "lblCameraCount";
            this.lblCameraCount.Size = new System.Drawing.Size(59, 12);
            this.lblCameraCount.TabIndex = 6;
            this.lblCameraCount.Text = "相机数量:";
            // 
            // txtCameraCount
            // 
            this.txtCameraCount.Location = new System.Drawing.Point(154, 82);
            this.txtCameraCount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCameraCount.Name = "txtCameraCount";
            this.txtCameraCount.Size = new System.Drawing.Size(292, 21);
            this.txtCameraCount.TabIndex = 7;
            // 
            // lblUseMock
            // 
            this.lblUseMock.AutoSize = true;
            this.lblUseMock.Location = new System.Drawing.Point(10, 108);
            this.lblUseMock.Name = "lblUseMock";
            this.lblUseMock.Size = new System.Drawing.Size(59, 12);
            this.lblUseMock.TabIndex = 8;
            this.lblUseMock.Text = "模拟模式:";
            // 
            // chkUseMock
            // 
            this.chkUseMock.Location = new System.Drawing.Point(154, 108);
            this.chkUseMock.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkUseMock.Name = "chkUseMock";
            this.chkUseMock.Size = new System.Drawing.Size(86, 16);
            this.chkUseMock.TabIndex = 9;
            this.chkUseMock.Text = "启用模拟";
            this.chkUseMock.UseVisualStyleBackColor = true;
            // 
            // lblMockImageFolder
            // 
            this.lblMockImageFolder.AutoSize = true;
            this.lblMockImageFolder.Location = new System.Drawing.Point(10, 132);
            this.lblMockImageFolder.Name = "lblMockImageFolder";
            this.lblMockImageFolder.Size = new System.Drawing.Size(83, 12);
            this.lblMockImageFolder.TabIndex = 10;
            this.lblMockImageFolder.Text = "模拟图片目录:";
            // 
            // txtMockImageFolder
            // 
            this.txtMockImageFolder.Location = new System.Drawing.Point(154, 130);
            this.txtMockImageFolder.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtMockImageFolder.Name = "txtMockImageFolder";
            this.txtMockImageFolder.Size = new System.Drawing.Size(292, 21);
            this.txtMockImageFolder.TabIndex = 11;
            // 
            // lblImageSavePath
            // 
            this.lblImageSavePath.AutoSize = true;
            this.lblImageSavePath.Location = new System.Drawing.Point(10, 156);
            this.lblImageSavePath.Name = "lblImageSavePath";
            this.lblImageSavePath.Size = new System.Drawing.Size(83, 12);
            this.lblImageSavePath.TabIndex = 12;
            this.lblImageSavePath.Text = "图像保存路径:";
            // 
            // txtImageSavePath
            // 
            this.txtImageSavePath.Location = new System.Drawing.Point(154, 154);
            this.txtImageSavePath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtImageSavePath.Name = "txtImageSavePath";
            this.txtImageSavePath.Size = new System.Drawing.Size(292, 21);
            this.txtImageSavePath.TabIndex = 13;
            // 
            // lblVisionProToolBlockPath
            // 
            this.lblVisionProToolBlockPath.AutoSize = true;
            this.lblVisionProToolBlockPath.Location = new System.Drawing.Point(10, 180);
            this.lblVisionProToolBlockPath.Name = "lblVisionProToolBlockPath";
            this.lblVisionProToolBlockPath.Size = new System.Drawing.Size(95, 12);
            this.lblVisionProToolBlockPath.TabIndex = 14;
            this.lblVisionProToolBlockPath.Text = "ToolBlock 路径:";
            // 
            // txtVisionProToolBlockPath
            // 
            this.txtVisionProToolBlockPath.Location = new System.Drawing.Point(154, 178);
            this.txtVisionProToolBlockPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtVisionProToolBlockPath.Name = "txtVisionProToolBlockPath";
            this.txtVisionProToolBlockPath.Size = new System.Drawing.Size(292, 21);
            this.txtVisionProToolBlockPath.TabIndex = 15;
            // 
            // lblDatabasePath
            // 
            this.lblDatabasePath.AutoSize = true;
            this.lblDatabasePath.Location = new System.Drawing.Point(10, 204);
            this.lblDatabasePath.Name = "lblDatabasePath";
            this.lblDatabasePath.Size = new System.Drawing.Size(83, 12);
            this.lblDatabasePath.TabIndex = 16;
            this.lblDatabasePath.Text = "数据库连接串:";
            // 
            // txtDatabasePath
            // 
            this.txtDatabasePath.Location = new System.Drawing.Point(154, 202);
            this.txtDatabasePath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtDatabasePath.Name = "txtDatabasePath";
            this.txtDatabasePath.Size = new System.Drawing.Size(292, 21);
            this.txtDatabasePath.TabIndex = 17;
            // 
            // lblAdminPassword
            // 
            this.lblAdminPassword.AutoSize = true;
            this.lblAdminPassword.Location = new System.Drawing.Point(10, 228);
            this.lblAdminPassword.Name = "lblAdminPassword";
            this.lblAdminPassword.Size = new System.Drawing.Size(71, 12);
            this.lblAdminPassword.TabIndex = 18;
            this.lblAdminPassword.Text = "管理员密码:";
            // 
            // txtAdminPassword
            // 
            this.txtAdminPassword.Location = new System.Drawing.Point(154, 226);
            this.txtAdminPassword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtAdminPassword.Name = "txtAdminPassword";
            this.txtAdminPassword.PasswordChar = '*';
            this.txtAdminPassword.Size = new System.Drawing.Size(292, 21);
            this.txtAdminPassword.TabIndex = 19;
            // 
            // lblImageRetainDays
            // 
            this.lblImageRetainDays.AutoSize = true;
            this.lblImageRetainDays.Location = new System.Drawing.Point(10, 252);
            this.lblImageRetainDays.Name = "lblImageRetainDays";
            this.lblImageRetainDays.Size = new System.Drawing.Size(83, 12);
            this.lblImageRetainDays.TabIndex = 20;
            this.lblImageRetainDays.Text = "图像保留天数:";
            // 
            // txtImageRetainDays
            // 
            this.txtImageRetainDays.Location = new System.Drawing.Point(154, 250);
            this.txtImageRetainDays.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtImageRetainDays.Name = "txtImageRetainDays";
            this.txtImageRetainDays.Size = new System.Drawing.Size(292, 21);
            this.txtImageRetainDays.TabIndex = 21;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(212, 310);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(69, 24);
            this.btnSave.TabIndex = 22;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(126, 310);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 24);
            this.btnCancel.TabIndex = 23;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // ConfigForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(489, 369);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtImageRetainDays);
            this.Controls.Add(this.lblImageRetainDays);
            this.Controls.Add(this.txtAdminPassword);
            this.Controls.Add(this.lblAdminPassword);
            this.Controls.Add(this.txtDatabasePath);
            this.Controls.Add(this.lblDatabasePath);
            this.Controls.Add(this.txtVisionProToolBlockPath);
            this.Controls.Add(this.lblVisionProToolBlockPath);
            this.Controls.Add(this.txtImageSavePath);
            this.Controls.Add(this.lblImageSavePath);
            this.Controls.Add(this.txtMockImageFolder);
            this.Controls.Add(this.lblMockImageFolder);
            this.Controls.Add(this.chkUseMock);
            this.Controls.Add(this.lblUseMock);
            this.Controls.Add(this.txtCameraCount);
            this.Controls.Add(this.lblCameraCount);
            this.Controls.Add(this.txtMachineId);
            this.Controls.Add(this.lblMachineId);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.txtIp);
            this.Controls.Add(this.lblIp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "系统配置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}