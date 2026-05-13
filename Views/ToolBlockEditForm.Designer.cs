namespace TeslaNE42Vision2D.Views
{
    partial class ToolBlockEditForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExecute = new System.Windows.Forms.Button();
            this.btnSelectImage = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtImagePath = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cogToolBlockEditV21 = new Cognex.VisionPro.ToolBlock.CogToolBlockEditV2();
            this.panelResult = new System.Windows.Forms.Panel();
            this.tabControlResult = new System.Windows.Forms.TabControl();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogToolBlockEditV21)).BeginInit();
            this.panelResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1083, 478);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnExecute);
            this.panel1.Controls.Add(this.btnSelectImage);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtImagePath);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 431);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1077, 44);
            this.panel1.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(726, 14);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(81, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(636, 14);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(84, 23);
            this.btnExecute.TabIndex = 4;
            this.btnExecute.Text = "执行";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // btnSelectImage
            // 
            this.btnSelectImage.Location = new System.Drawing.Point(471, 14);
            this.btnSelectImage.Name = "btnSelectImage";
            this.btnSelectImage.Size = new System.Drawing.Size(93, 23);
            this.btnSelectImage.TabIndex = 3;
            this.btnSelectImage.Text = "选择图像";
            this.btnSelectImage.UseVisualStyleBackColor = true;
            this.btnSelectImage.Click += new System.EventHandler(this.btnSelectImage_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "图像路径:";
            // 
            // txtImagePath
            // 
            this.txtImagePath.Location = new System.Drawing.Point(84, 14);
            this.txtImagePath.Name = "txtImagePath";
            this.txtImagePath.Size = new System.Drawing.Size(381, 21);
            this.txtImagePath.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel2.Controls.Add(this.cogToolBlockEditV21, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panelResult, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1077, 422);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // cogToolBlockEditV21
            // 
            this.cogToolBlockEditV21.AllowDrop = true;
            this.cogToolBlockEditV21.ContextMenuCustomizer = null;
            this.cogToolBlockEditV21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogToolBlockEditV21.Location = new System.Drawing.Point(3, 3);
            this.cogToolBlockEditV21.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogToolBlockEditV21.Name = "cogToolBlockEditV21";
            this.cogToolBlockEditV21.ShowNodeToolTips = true;
            this.cogToolBlockEditV21.Size = new System.Drawing.Size(1071, 416);
            this.cogToolBlockEditV21.SuspendElectricRuns = false;
            this.cogToolBlockEditV21.TabIndex = 2;
            // 
            // panelResult
            // 
            this.panelResult.Controls.Add(this.tabControlResult);
            this.panelResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelResult.Location = new System.Drawing.Point(1080, 3);
            this.panelResult.Name = "panelResult";
            this.panelResult.Size = new System.Drawing.Size(1, 416);
            this.panelResult.TabIndex = 3;
            // 
            // tabControlResult
            // 
            this.tabControlResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlResult.Location = new System.Drawing.Point(0, 0);
            this.tabControlResult.Name = "tabControlResult";
            this.tabControlResult.SelectedIndex = 0;
            this.tabControlResult.Size = new System.Drawing.Size(1, 416);
            this.tabControlResult.TabIndex = 0;
            // 
            // ToolBlockEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1083, 478);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ToolBlockEditForm";
            this.Text = "编辑 ToolBlock";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ToolBlockEditForm_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cogToolBlockEditV21)).EndInit();
            this.panelResult.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtImagePath;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Cognex.VisionPro.ToolBlock.CogToolBlockEditV2 cogToolBlockEditV21;
        private System.Windows.Forms.Panel panelResult;
        private System.Windows.Forms.TabControl tabControlResult;
        private System.Windows.Forms.Button btnSelectImage;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Button btnSave;
    }
}