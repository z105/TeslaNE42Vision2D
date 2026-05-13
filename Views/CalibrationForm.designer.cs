namespace TeslaNE42Vision2D.Views
{
    partial class CalibrationForm
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

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.dgvPoints = new System.Windows.Forms.DataGridView();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PhysicalX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PhysicalY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PixelX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PixelY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnClearRow = new System.Windows.Forms.Button();
            this.btnFillMock = new System.Windows.Forms.Button();
            this.btnCalibrate = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panelToolBlocks = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageMarks = new System.Windows.Forms.TabPage();
            this.cogToolBlockEditV2 = new Cognex.VisionPro.ToolBlock.CogToolBlockEditV2();
            this.tabPageCalib = new System.Windows.Forms.TabPage();
            this.cogCalibNPointToNPointEditV2 = new Cognex.VisionPro.CalibFix.CogCalibNPointToNPointEditV2();
            this.tableLayoutPanelMain.SuspendLayout();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPoints)).BeginInit();
            this.btnPanel.SuspendLayout();
            this.panelToolBlocks.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageMarks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogToolBlockEditV2)).BeginInit();
            this.tabPageCalib.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogCalibNPointToNPointEditV2)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.panelTop, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.panelToolBlocks, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 350F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(749, 558);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.dgvPoints);
            this.panelTop.Controls.Add(this.btnPanel);
            this.panelTop.Controls.Add(this.lblStatus);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTop.Location = new System.Drawing.Point(3, 3);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(743, 344);
            this.panelTop.TabIndex = 0;
            // 
            // dgvPoints
            // 
            this.dgvPoints.AllowUserToAddRows = false;
            this.dgvPoints.AllowUserToDeleteRows = false;
            this.dgvPoints.AllowUserToResizeRows = false;
            this.dgvPoints.BackgroundColor = System.Drawing.Color.White;
            this.dgvPoints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPoints.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Index,
            this.PhysicalX,
            this.PhysicalY,
            this.PixelX,
            this.PixelY});
            this.dgvPoints.GridColor = System.Drawing.Color.DarkGray;
            this.dgvPoints.Location = new System.Drawing.Point(17, 11);
            this.dgvPoints.MultiSelect = false;
            this.dgvPoints.Name = "dgvPoints";
            this.dgvPoints.RowHeadersVisible = false;
            this.dgvPoints.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPoints.Size = new System.Drawing.Size(660, 247);
            this.dgvPoints.TabIndex = 6;
            // 
            // Index
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Index.DefaultCellStyle = dataGridViewCellStyle6;
            this.Index.FillWeight = 40F;
            this.Index.HeaderText = "序号";
            this.Index.Name = "Index";
            this.Index.ReadOnly = true;
            // 
            // PhysicalX
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.LightCyan;
            this.PhysicalX.DefaultCellStyle = dataGridViewCellStyle7;
            this.PhysicalX.HeaderText = "物理坐标X";
            this.PhysicalX.Name = "PhysicalX";
            // 
            // PhysicalY
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.LightCyan;
            this.PhysicalY.DefaultCellStyle = dataGridViewCellStyle8;
            this.PhysicalY.HeaderText = "物理坐标Y";
            this.PhysicalY.Name = "PhysicalY";
            // 
            // PixelX
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.LightYellow;
            this.PixelX.DefaultCellStyle = dataGridViewCellStyle9;
            this.PixelX.HeaderText = "像素坐标X";
            this.PixelX.Name = "PixelX";
            // 
            // PixelY
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.LightYellow;
            this.PixelY.DefaultCellStyle = dataGridViewCellStyle10;
            this.PixelY.HeaderText = "像素坐标Y";
            this.PixelY.Name = "PixelY";
            // 
            // btnPanel
            // 
            this.btnPanel.Controls.Add(this.btnClearAll);
            this.btnPanel.Controls.Add(this.btnClearRow);
            this.btnPanel.Controls.Add(this.btnFillMock);
            this.btnPanel.Controls.Add(this.btnCalibrate);
            this.btnPanel.Controls.Add(this.btnSave);
            this.btnPanel.Controls.Add(this.btnLoad);
            this.btnPanel.Location = new System.Drawing.Point(17, 264);
            this.btnPanel.Name = "btnPanel";
            this.btnPanel.Size = new System.Drawing.Size(660, 45);
            this.btnPanel.TabIndex = 7;
            this.btnPanel.WrapContents = false;
            // 
            // btnClearAll
            // 
            this.btnClearAll.Location = new System.Drawing.Point(3, 3);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(110, 35);
            this.btnClearAll.TabIndex = 0;
            this.btnClearAll.Text = "清空所有数据";
            this.btnClearAll.UseVisualStyleBackColor = true;
            this.btnClearAll.Click += new System.EventHandler(this.BtnClearAll_Click);
            // 
            // btnClearRow
            // 
            this.btnClearRow.Location = new System.Drawing.Point(119, 3);
            this.btnClearRow.Name = "btnClearRow";
            this.btnClearRow.Size = new System.Drawing.Size(100, 35);
            this.btnClearRow.TabIndex = 1;
            this.btnClearRow.Text = "清空当前行";
            this.btnClearRow.UseVisualStyleBackColor = true;
            this.btnClearRow.Click += new System.EventHandler(this.BtnClearRow_Click);
            // 
            // btnFillMock
            // 
            this.btnFillMock.Location = new System.Drawing.Point(225, 3);
            this.btnFillMock.Name = "btnFillMock";
            this.btnFillMock.Size = new System.Drawing.Size(120, 35);
            this.btnFillMock.TabIndex = 2;
            this.btnFillMock.Text = "Mock填充当前行";
            this.btnFillMock.UseVisualStyleBackColor = true;
            this.btnFillMock.Click += new System.EventHandler(this.BtnFillMock_Click);
            // 
            // btnCalibrate
            // 
            this.btnCalibrate.Location = new System.Drawing.Point(351, 3);
            this.btnCalibrate.Name = "btnCalibrate";
            this.btnCalibrate.Size = new System.Drawing.Size(110, 35);
            this.btnCalibrate.TabIndex = 3;
            this.btnCalibrate.Text = "执行标定计算";
            this.btnCalibrate.UseVisualStyleBackColor = true;
            this.btnCalibrate.Click += new System.EventHandler(this.BtnCalibrate_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(467, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 35);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "保存数据";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(563, 3);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(90, 35);
            this.btnLoad.TabIndex = 5;
            this.btnLoad.Text = "加载数据";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblStatus.Location = new System.Drawing.Point(17, 314);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(660, 20);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "请录入九点标定数据，或使用Mock填充";
            // 
            // panelToolBlocks
            // 
            this.panelToolBlocks.Controls.Add(this.tabControl1);
            this.panelToolBlocks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelToolBlocks.Location = new System.Drawing.Point(3, 353);
            this.panelToolBlocks.Name = "panelToolBlocks";
            this.panelToolBlocks.Size = new System.Drawing.Size(743, 202);
            this.panelToolBlocks.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageMarks);
            this.tabControl1.Controls.Add(this.tabPageCalib);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(743, 202);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageMarks
            // 
            this.tabPageMarks.Controls.Add(this.cogToolBlockEditV2);
            this.tabPageMarks.Location = new System.Drawing.Point(4, 26);
            this.tabPageMarks.Name = "tabPageMarks";
            this.tabPageMarks.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMarks.Size = new System.Drawing.Size(735, 172);
            this.tabPageMarks.TabIndex = 0;
            this.tabPageMarks.Text = "Mark";
            this.tabPageMarks.UseVisualStyleBackColor = true;
            // 
            // cogToolBlockEditV2
            // 
            this.cogToolBlockEditV2.AllowDrop = true;
            this.cogToolBlockEditV2.ContextMenuCustomizer = null;
            this.cogToolBlockEditV2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogToolBlockEditV2.Location = new System.Drawing.Point(3, 3);
            this.cogToolBlockEditV2.Margin = new System.Windows.Forms.Padding(4);
            this.cogToolBlockEditV2.MinimumSize = new System.Drawing.Size(570, 0);
            this.cogToolBlockEditV2.Name = "cogToolBlockEditV2";
            this.cogToolBlockEditV2.ShowNodeToolTips = true;
            this.cogToolBlockEditV2.Size = new System.Drawing.Size(729, 166);
            this.cogToolBlockEditV2.SuspendElectricRuns = false;
            this.cogToolBlockEditV2.TabIndex = 0;
            // 
            // tabPageCalib
            // 
            this.tabPageCalib.Controls.Add(this.cogCalibNPointToNPointEditV2);
            this.tabPageCalib.Location = new System.Drawing.Point(4, 26);
            this.tabPageCalib.Name = "tabPageCalib";
            this.tabPageCalib.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCalib.Size = new System.Drawing.Size(735, 172);
            this.tabPageCalib.TabIndex = 1;
            this.tabPageCalib.Text = "CalbNP2NP";
            this.tabPageCalib.UseVisualStyleBackColor = true;
            // 
            // cogCalibNPointToNPointEditV2
            // 
            this.cogCalibNPointToNPointEditV2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogCalibNPointToNPointEditV2.Location = new System.Drawing.Point(3, 3);
            this.cogCalibNPointToNPointEditV2.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogCalibNPointToNPointEditV2.Name = "cogCalibNPointToNPointEditV2";
            this.cogCalibNPointToNPointEditV2.Size = new System.Drawing.Size(729, 166);
            this.cogCalibNPointToNPointEditV2.SuspendElectricRuns = false;
            this.cogCalibNPointToNPointEditV2.TabIndex = 0;
            // 
            // CalibrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 558);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "CalibrationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "九点标定";
            this.Load += new System.EventHandler(this.CalibrationForm_Load);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPoints)).EndInit();
            this.btnPanel.ResumeLayout(false);
            this.panelToolBlocks.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageMarks.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cogToolBlockEditV2)).EndInit();
            this.tabPageCalib.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cogCalibNPointToNPointEditV2)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.DataGridView dgvPoints;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn PhysicalX;
        private System.Windows.Forms.DataGridViewTextBoxColumn PhysicalY;
        private System.Windows.Forms.DataGridViewTextBoxColumn PixelX;
        private System.Windows.Forms.DataGridViewTextBoxColumn PixelY;
        private System.Windows.Forms.FlowLayoutPanel btnPanel;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Button btnClearRow;
        private System.Windows.Forms.Button btnFillMock;
        private System.Windows.Forms.Button btnCalibrate;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel panelToolBlocks;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageMarks;
        private Cognex.VisionPro.ToolBlock.CogToolBlockEditV2 cogToolBlockEditV2;
        private System.Windows.Forms.TabPage tabPageCalib;
        private Cognex.VisionPro.CalibFix.CogCalibNPointToNPointEditV2 cogCalibNPointToNPointEditV2;
    }
}