namespace TeslaNE42Vision2D.Views
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnSingleRun;
        private System.Windows.Forms.Button btnCalibration;
        private System.Windows.Forms.Button btnDebug;
        private System.Windows.Forms.Button btnConfig;

        private System.Windows.Forms.TableLayoutPanel displayPanel;
        private System.Windows.Forms.Panel rightPanel;

        private System.Windows.Forms.Label lblOkCount;
        private System.Windows.Forms.Label lblNgCount;
        private System.Windows.Forms.Label lblNgRate;
        private System.Windows.Forms.Label lblLastResult;

        private System.Windows.Forms.ListBox lbLog;

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStateStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblPlcStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblHeartbeatStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblDiskStatus;

        private System.Windows.Forms.Button btnClearLog;

        private System.Windows.Forms.GroupBox resultGroup;
        private System.Windows.Forms.GroupBox logGroup;

        private System.Windows.Forms.FlowLayoutPanel topPanel;

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
            this.topPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSingleRun = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnCalibration = new System.Windows.Forms.Button();
            this.btnDebug = new System.Windows.Forms.Button();
            this.btnConfig = new System.Windows.Forms.Button();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.btnCalcToolBLock = new System.Windows.Forms.Button();
            this.displayPanel = new System.Windows.Forms.TableLayoutPanel();
            this.rightPanel = new System.Windows.Forms.Panel();
            this.resultGroup = new System.Windows.Forms.GroupBox();
            this.lblLastResult = new System.Windows.Forms.Label();
            this.lblOkCount = new System.Windows.Forms.Label();
            this.lblNgCount = new System.Windows.Forms.Label();
            this.lblNgRate = new System.Windows.Forms.Label();
            this.logGroup = new System.Windows.Forms.GroupBox();
            this.lbLog = new System.Windows.Forms.ListBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStateStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblPlcStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblHeartbeatStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblDiskStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.comboBoxPosition = new System.Windows.Forms.ComboBox();
            this.topPanel.SuspendLayout();
            this.rightPanel.SuspendLayout();
            this.resultGroup.SuspendLayout();
            this.logGroup.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.topPanel.Controls.Add(this.btnSingleRun);
            this.topPanel.Controls.Add(this.btnStop);
            this.topPanel.Controls.Add(this.btnReset);
            this.topPanel.Controls.Add(this.btnCalibration);
            this.topPanel.Controls.Add(this.btnDebug);
            this.topPanel.Controls.Add(this.btnConfig);
            this.topPanel.Controls.Add(this.btnClearLog);
            this.topPanel.Controls.Add(this.btnCalcToolBLock);
            this.topPanel.Controls.Add(this.comboBoxPosition);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Padding = new System.Windows.Forms.Padding(8, 10, 8, 0);
            this.topPanel.Size = new System.Drawing.Size(1280, 55);
            this.topPanel.TabIndex = 0;
            // 
            // btnSingleRun
            // 
            this.btnSingleRun.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(153)))), ((int)(((byte)(76)))));
            this.btnSingleRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSingleRun.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnSingleRun.ForeColor = System.Drawing.Color.White;
            this.btnSingleRun.Location = new System.Drawing.Point(12, 10);
            this.btnSingleRun.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnSingleRun.Name = "btnSingleRun";
            this.btnSingleRun.Size = new System.Drawing.Size(110, 34);
            this.btnSingleRun.TabIndex = 2;
            this.btnSingleRun.Text = "单步运行";
            this.btnSingleRun.UseVisualStyleBackColor = false;
            this.btnSingleRun.Click += new System.EventHandler(this.BtnSingleRun_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.Location = new System.Drawing.Point(130, 10);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(110, 34);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(0)))));
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(248, 10);
            this.btnReset.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(110, 34);
            this.btnReset.TabIndex = 4;
            this.btnReset.Text = "复位";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // btnCalibration
            // 
            this.btnCalibration.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(120)))));
            this.btnCalibration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalibration.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnCalibration.ForeColor = System.Drawing.Color.White;
            this.btnCalibration.Location = new System.Drawing.Point(366, 10);
            this.btnCalibration.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnCalibration.Name = "btnCalibration";
            this.btnCalibration.Size = new System.Drawing.Size(110, 34);
            this.btnCalibration.TabIndex = 6;
            this.btnCalibration.Text = "九点标定";
            this.btnCalibration.UseVisualStyleBackColor = false;
            this.btnCalibration.Click += new System.EventHandler(this.BtnCalibration_Click);
            // 
            // btnDebug
            // 
            this.btnDebug.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(120)))));
            this.btnDebug.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDebug.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnDebug.ForeColor = System.Drawing.Color.White;
            this.btnDebug.Location = new System.Drawing.Point(484, 10);
            this.btnDebug.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Size = new System.Drawing.Size(110, 34);
            this.btnDebug.TabIndex = 7;
            this.btnDebug.Text = "调试工具";
            this.btnDebug.UseVisualStyleBackColor = false;
            this.btnDebug.Click += new System.EventHandler(this.BtnDebug_Click);
            // 
            // btnConfig
            // 
            this.btnConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(120)))));
            this.btnConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfig.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnConfig.ForeColor = System.Drawing.Color.White;
            this.btnConfig.Location = new System.Drawing.Point(602, 10);
            this.btnConfig.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(110, 34);
            this.btnConfig.TabIndex = 8;
            this.btnConfig.Text = "系统配置";
            this.btnConfig.UseVisualStyleBackColor = false;
            this.btnConfig.Click += new System.EventHandler(this.BtnConfig_Click);
            // 
            // btnClearLog
            // 
            this.btnClearLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnClearLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearLog.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnClearLog.ForeColor = System.Drawing.Color.White;
            this.btnClearLog.Location = new System.Drawing.Point(720, 10);
            this.btnClearLog.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(80, 34);
            this.btnClearLog.TabIndex = 9;
            this.btnClearLog.Text = "清空日志";
            this.btnClearLog.UseVisualStyleBackColor = false;
            this.btnClearLog.Click += new System.EventHandler(this.BtnClearLog_Click);
            // 
            // btnCalcToolBLock
            // 
            this.btnCalcToolBLock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(120)))));
            this.btnCalcToolBLock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalcToolBLock.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnCalcToolBLock.ForeColor = System.Drawing.Color.White;
            this.btnCalcToolBLock.Location = new System.Drawing.Point(808, 10);
            this.btnCalcToolBLock.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnCalcToolBLock.Name = "btnCalcToolBLock";
            this.btnCalcToolBLock.Size = new System.Drawing.Size(110, 34);
            this.btnCalcToolBLock.TabIndex = 10;
            this.btnCalcToolBLock.Text = "Calc ToolBlock";
            this.btnCalcToolBLock.UseVisualStyleBackColor = false;
            this.btnCalcToolBLock.Click += new System.EventHandler(this.btnCalcToolBLock_Click);
            // 
            // displayPanel
            // 
            this.displayPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.displayPanel.BackColor = System.Drawing.Color.Black;
            this.displayPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.displayPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 840F));
            this.displayPanel.Location = new System.Drawing.Point(0, 55);
            this.displayPanel.Name = "displayPanel";
            this.displayPanel.Size = new System.Drawing.Size(840, 723);
            this.displayPanel.TabIndex = 1;
            // 
            // rightPanel
            // 
            this.rightPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rightPanel.BackColor = System.Drawing.Color.White;
            this.rightPanel.Controls.Add(this.resultGroup);
            this.rightPanel.Controls.Add(this.logGroup);
            this.rightPanel.Location = new System.Drawing.Point(840, 55);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Size = new System.Drawing.Size(440, 723);
            this.rightPanel.TabIndex = 2;
            // 
            // resultGroup
            // 
            this.resultGroup.Controls.Add(this.lblLastResult);
            this.resultGroup.Controls.Add(this.lblOkCount);
            this.resultGroup.Controls.Add(this.lblNgCount);
            this.resultGroup.Controls.Add(this.lblNgRate);
            this.resultGroup.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.resultGroup.Location = new System.Drawing.Point(10, 10);
            this.resultGroup.Name = "resultGroup";
            this.resultGroup.Size = new System.Drawing.Size(410, 160);
            this.resultGroup.TabIndex = 0;
            this.resultGroup.TabStop = false;
            this.resultGroup.Text = "检测结果";
            // 
            // lblLastResult
            // 
            this.lblLastResult.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Bold);
            this.lblLastResult.ForeColor = System.Drawing.Color.DarkGray;
            this.lblLastResult.Location = new System.Drawing.Point(10, 25);
            this.lblLastResult.Name = "lblLastResult";
            this.lblLastResult.Size = new System.Drawing.Size(390, 40);
            this.lblLastResult.TabIndex = 0;
            this.lblLastResult.Text = "---";
            this.lblLastResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOkCount
            // 
            this.lblOkCount.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblOkCount.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblOkCount.Location = new System.Drawing.Point(10, 75);
            this.lblOkCount.Name = "lblOkCount";
            this.lblOkCount.Size = new System.Drawing.Size(180, 30);
            this.lblOkCount.TabIndex = 1;
            this.lblOkCount.Text = "OK: 0";
            // 
            // lblNgCount
            // 
            this.lblNgCount.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lblNgCount.ForeColor = System.Drawing.Color.DarkRed;
            this.lblNgCount.Location = new System.Drawing.Point(10, 110);
            this.lblNgCount.Name = "lblNgCount";
            this.lblNgCount.Size = new System.Drawing.Size(180, 30);
            this.lblNgCount.TabIndex = 2;
            this.lblNgCount.Text = "NG: 0";
            // 
            // lblNgRate
            // 
            this.lblNgRate.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblNgRate.ForeColor = System.Drawing.Color.DarkRed;
            this.lblNgRate.Location = new System.Drawing.Point(200, 110);
            this.lblNgRate.Name = "lblNgRate";
            this.lblNgRate.Size = new System.Drawing.Size(200, 30);
            this.lblNgRate.TabIndex = 3;
            this.lblNgRate.Text = "NG率: 0.00%";
            // 
            // logGroup
            // 
            this.logGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logGroup.Controls.Add(this.lbLog);
            this.logGroup.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.logGroup.Location = new System.Drawing.Point(10, 180);
            this.logGroup.Name = "logGroup";
            this.logGroup.Size = new System.Drawing.Size(410, 533);
            this.logGroup.TabIndex = 1;
            this.logGroup.TabStop = false;
            this.logGroup.Text = "运行日志";
            // 
            // lbLog
            // 
            this.lbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbLog.Font = new System.Drawing.Font("Consolas", 8F);
            this.lbLog.HorizontalScrollbar = true;
            this.lbLog.Location = new System.Drawing.Point(3, 19);
            this.lbLog.Name = "lbLog";
            this.lbLog.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lbLog.Size = new System.Drawing.Size(404, 511);
            this.lbLog.TabIndex = 0;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStateStatus,
            this.lblPlcStatus,
            this.lblHeartbeatStatus,
            this.lblDiskStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 774);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1280, 26);
            this.statusStrip.TabIndex = 3;
            // 
            // lblStateStatus
            // 
            this.lblStateStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblStateStatus.Name = "lblStateStatus";
            this.lblStateStatus.Size = new System.Drawing.Size(79, 21);
            this.lblStateStatus.Text = "状态: 初始化";
            // 
            // lblPlcStatus
            // 
            this.lblPlcStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblPlcStatus.Name = "lblPlcStatus";
            this.lblPlcStatus.Size = new System.Drawing.Size(76, 21);
            this.lblPlcStatus.Text = "PLC: 未连接";
            // 
            // lblHeartbeatStatus
            // 
            this.lblHeartbeatStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblHeartbeatStatus.Name = "lblHeartbeatStatus";
            this.lblHeartbeatStatus.Size = new System.Drawing.Size(78, 21);
            this.lblHeartbeatStatus.Text = "心跳: 0 正常";
            // 
            // lblDiskStatus
            // 
            this.lblDiskStatus.Name = "lblDiskStatus";
            this.lblDiskStatus.Size = new System.Drawing.Size(49, 21);
            this.lblDiskStatus.Text = "磁盘: --";
            // 
            // comboBoxPosition
            // 
            this.comboBoxPosition.FormattingEnabled = true;
            this.comboBoxPosition.Location = new System.Drawing.Point(925, 13);
            this.comboBoxPosition.Name = "comboBoxPosition";
            this.comboBoxPosition.Size = new System.Drawing.Size(104, 25);
            this.comboBoxPosition.TabIndex = 11;
            this.comboBoxPosition.SelectedIndexChanged += new System.EventHandler(this.comboBoxPosition_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 800);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.rightPanel);
            this.Controls.Add(this.displayPanel);
            this.Controls.Add(this.topPanel);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.MinimumSize = new System.Drawing.Size(1000, 650);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TeslaNE42 视觉引导系统";
            this.topPanel.ResumeLayout(false);
            this.rightPanel.ResumeLayout(false);
            this.resultGroup.ResumeLayout(false);
            this.logGroup.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnCalcToolBLock;
        private System.Windows.Forms.ComboBox comboBoxPosition;
    }
}