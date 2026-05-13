namespace TeslaNE42Vision2D.Views
{
    partial class PasswordDialog
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblPrompt;

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
            this.lblPrompt = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // lblPrompt
            this.lblPrompt.Location = new System.Drawing.Point(20, 20);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(260, 20);
            this.lblPrompt.TabIndex = 0;
            this.lblPrompt.Text = "请输入管理员密码:";

            // txtPassword
            this.txtPassword.Location = new System.Drawing.Point(20, 50);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(240, 25);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtPassword_KeyDown);

            // btnOk
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(60, 85);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 30);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);

            // btnCancel
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(155, 85);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;

            // PasswordDialog
            this.AcceptButton = this.btnOk;
            this.CancelButton = this.btnCancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 141);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblPrompt, this.txtPassword, this.btnOk, this.btnCancel
            });
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "权限验证";

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}