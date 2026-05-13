namespace TeslaNE42Vision2D.Views
{
    partial class CameraConfigForm
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

        #region Windows 窗体设计器生成的代码

        private void InitializeComponent()
        {
            this.lblIndex = new System.Windows.Forms.Label();
            this.txtIndex = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblSn = new System.Windows.Forms.Label();
            this.txtSn = new System.Windows.Forms.TextBox();
            this.lblExposurePolarity = new System.Windows.Forms.Label();
            this.numExposurePolarity = new System.Windows.Forms.NumericUpDown();
            this.lblExposureBarcode = new System.Windows.Forms.Label();
            this.numExposureBarcode = new System.Windows.Forms.NumericUpDown();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numExposurePolarity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numExposureBarcode)).BeginInit();
            this.SuspendLayout();
            // 
            // lblIndex
            // 
            this.lblIndex.AutoSize = true;
            this.lblIndex.Location = new System.Drawing.Point(10, 12);
            this.lblIndex.Name = "lblIndex";
            this.lblIndex.Size = new System.Drawing.Size(53, 12);
            this.lblIndex.TabIndex = 0;
            this.lblIndex.Text = "相机索引";
            // 
            // txtIndex
            // 
            this.txtIndex.Location = new System.Drawing.Point(77, 10);
            this.txtIndex.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtIndex.Name = "txtIndex";
            this.txtIndex.ReadOnly = true;
            this.txtIndex.Size = new System.Drawing.Size(172, 21);
            this.txtIndex.TabIndex = 1;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(10, 36);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(53, 12);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "相机名称";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(77, 34);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(172, 21);
            this.txtName.TabIndex = 3;
            // 
            // lblSn
            // 
            this.lblSn.AutoSize = true;
            this.lblSn.Location = new System.Drawing.Point(10, 60);
            this.lblSn.Name = "lblSn";
            this.lblSn.Size = new System.Drawing.Size(41, 12);
            this.lblSn.TabIndex = 4;
            this.lblSn.Text = "序列号";
            // 
            // txtSn
            // 
            this.txtSn.Location = new System.Drawing.Point(77, 58);
            this.txtSn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSn.Name = "txtSn";
            this.txtSn.ReadOnly = true;
            this.txtSn.Size = new System.Drawing.Size(172, 21);
            this.txtSn.TabIndex = 5;
            // 
            // lblExposurePolarity
            // 
            this.lblExposurePolarity.AutoSize = true;
            this.lblExposurePolarity.Location = new System.Drawing.Point(10, 84);
            this.lblExposurePolarity.Name = "lblExposurePolarity";
            this.lblExposurePolarity.Size = new System.Drawing.Size(53, 12);
            this.lblExposurePolarity.TabIndex = 6;
            this.lblExposurePolarity.Text = "引导曝光";
            // 
            // numExposurePolarity
            // 
            this.numExposurePolarity.Location = new System.Drawing.Point(77, 82);
            this.numExposurePolarity.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numExposurePolarity.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numExposurePolarity.Name = "numExposurePolarity";
            this.numExposurePolarity.Size = new System.Drawing.Size(171, 21);
            this.numExposurePolarity.TabIndex = 7;
            // 
            // lblExposureBarcode
            // 
            this.lblExposureBarcode.AutoSize = true;
            this.lblExposureBarcode.Location = new System.Drawing.Point(10, 108);
            this.lblExposureBarcode.Name = "lblExposureBarcode";
            this.lblExposureBarcode.Size = new System.Drawing.Size(53, 12);
            this.lblExposureBarcode.TabIndex = 8;
            this.lblExposureBarcode.Text = "条码曝光";
            // 
            // numExposureBarcode
            // 
            this.numExposureBarcode.Location = new System.Drawing.Point(77, 106);
            this.numExposureBarcode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numExposureBarcode.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numExposureBarcode.Name = "numExposureBarcode";
            this.numExposureBarcode.Size = new System.Drawing.Size(171, 21);
            this.numExposureBarcode.TabIndex = 9;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(34, 144);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(86, 24);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(137, 144);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(86, 24);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // CameraConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 180);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.numExposureBarcode);
            this.Controls.Add(this.lblExposureBarcode);
            this.Controls.Add(this.numExposurePolarity);
            this.Controls.Add(this.lblExposurePolarity);
            this.Controls.Add(this.txtSn);
            this.Controls.Add(this.lblSn);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtIndex);
            this.Controls.Add(this.lblIndex);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CameraConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "相机配置";
            ((System.ComponentModel.ISupportInitialize)(this.numExposurePolarity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numExposureBarcode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblIndex;
        private System.Windows.Forms.TextBox txtIndex;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblSn;
        private System.Windows.Forms.TextBox txtSn;
        private System.Windows.Forms.Label lblExposurePolarity;
        private System.Windows.Forms.NumericUpDown numExposurePolarity;
        private System.Windows.Forms.Label lblExposureBarcode;
        private System.Windows.Forms.NumericUpDown numExposureBarcode;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}