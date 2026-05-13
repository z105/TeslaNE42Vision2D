namespace TeslaNE42Vision2D.Views
{
    partial class DebugForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Button btnSnap;
        private System.Windows.Forms.Button btnDetect;
        private System.Windows.Forms.Button btnSendResult;
        private System.Windows.Forms.Button btnSnapAndDetect;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Panel displayPanel;
        private System.Windows.Forms.FlowLayoutPanel btnPanel;

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
            this.btnPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSnap = new System.Windows.Forms.Button();
            this.btnDetect = new System.Windows.Forms.Button();
            this.btnSnapAndDetect = new System.Windows.Forms.Button();
            this.btnSendResult = new System.Windows.Forms.Button();
            this.displayPanel = new System.Windows.Forms.Panel();
            this.lblResult = new System.Windows.Forms.Label();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();

            // btnPanel
            this.btnPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.btnPanel.Location = new System.Drawing.Point(0, 0);
            this.btnPanel.Name = "btnPanel";
            this.btnPanel.Padding = new System.Windows.Forms.Padding(5);
            this.btnPanel.Size = new System.Drawing.Size(884, 50);
            this.btnPanel.TabIndex = 0;

            // btnSnap
            this.btnSnap.Name = "btnSnap";
            this.btnSnap.Size = new System.Drawing.Size(100, 35);
            this.btnSnap.TabIndex = 0;
            this.btnSnap.Text = "拍照";
            this.btnSnap.UseVisualStyleBackColor = true;
            this.btnSnap.Click += new System.EventHandler(this.BtnSnap_Click);

            // btnDetect
            this.btnDetect.Name = "btnDetect";
            this.btnDetect.Size = new System.Drawing.Size(100, 35);
            this.btnDetect.TabIndex = 1;
            this.btnDetect.Text = "检测";
            this.btnDetect.UseVisualStyleBackColor = true;
            this.btnDetect.Click += new System.EventHandler(this.BtnDetect_Click);

            // btnSnapAndDetect
            this.btnSnapAndDetect.Name = "btnSnapAndDetect";
            this.btnSnapAndDetect.Size = new System.Drawing.Size(120, 35);
            this.btnSnapAndDetect.TabIndex = 2;
            this.btnSnapAndDetect.Text = "拍照+检测";
            this.btnSnapAndDetect.UseVisualStyleBackColor = true;
            this.btnSnapAndDetect.Click += new System.EventHandler(this.BtnSnapAndDetect_Click);

            // btnSendResult
            this.btnSendResult.Name = "btnSendResult";
            this.btnSendResult.Size = new System.Drawing.Size(140, 35);
            this.btnSendResult.TabIndex = 3;
            this.btnSendResult.Text = "发送结果到PLC";
            this.btnSendResult.UseVisualStyleBackColor = true;
            this.btnSendResult.Click += new System.EventHandler(this.BtnSendResult_Click);

            this.btnPanel.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.btnSnap, this.btnDetect, this.btnSnapAndDetect, this.btnSendResult
            });

            // displayPanel
            this.displayPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.displayPanel.Location = new System.Drawing.Point(10, 60);
            this.displayPanel.Name = "displayPanel";
            this.displayPanel.Size = new System.Drawing.Size(540, 480);
            this.displayPanel.TabIndex = 1;

            // lblResult
            this.lblResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblResult.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.lblResult.Location = new System.Drawing.Point(560, 60);
            this.lblResult.Name = "lblResult";
            this.lblResult.Padding = new System.Windows.Forms.Padding(5);
            this.lblResult.Size = new System.Drawing.Size(310, 120);
            this.lblResult.TabIndex = 2;
            this.lblResult.Text = "等待检测结果...";
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // rtbLog
            this.rtbLog.BackColor = System.Drawing.Color.Black;
            this.rtbLog.Font = new System.Drawing.Font("Consolas", 8F);
            this.rtbLog.ForeColor = System.Drawing.Color.LightGreen;
            this.rtbLog.Location = new System.Drawing.Point(560, 190);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbLog.Size = new System.Drawing.Size(310, 350);
            this.rtbLog.TabIndex = 3;

            // DebugForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 551);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.btnPanel, this.displayPanel, this.lblResult, this.rtbLog
            });
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.Name = "DebugForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "调试工具";

            this.ResumeLayout(false);
        }
    }
}