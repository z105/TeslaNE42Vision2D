using System;
using System.Windows.Forms;
using TeslaNE42Vision2D.Services;
using TeslaNE42Vision2D.Services.Calibration;

namespace TeslaNE42Vision2D.Views
{
    public partial class CalibrationForm : Form
    {
        private const int PointCount = 9;
        private readonly NinePointCalibrationVisionProService _calibService;
        private readonly Random _random = new Random();

        public CalibrationForm()
        {
            InitializeComponent();

            _calibService = RunDataService.Instance.CalibrationService;

            // Add 9 rows to DataGridView
            for (int i = 0; i < PointCount; i++)
            {
                dgvPoints.Rows.Add(i + 1, "", "", "", "");
            }

            LoadExistingPoints();
        }

        private void LoadExistingPoints()
        {
            if (_calibService.Points.Count > 0)
            {
                for (int i = 0; i < Math.Min(_calibService.Points.Count, PointCount); i++)
                {
                    var p = _calibService.Points[i];
                    dgvPoints.Rows[i].Cells["PhysicalX"].Value = p.PhysicalX.ToString("F3");
                    dgvPoints.Rows[i].Cells["PhysicalY"].Value = p.PhysicalY.ToString("F3");
                    dgvPoints.Rows[i].Cells["PixelX"].Value = p.PixelX.ToString("F3");
                    dgvPoints.Rows[i].Cells["PixelY"].Value = p.PixelY.ToString("F3");
                }
                lblStatus.Text = $"已加载 {_calibService.Points.Count} 个标定点";
            }
        }

        private void BtnClearAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvPoints.Rows)
            {
                row.Cells["PhysicalX"].Value = "";
                row.Cells["PhysicalY"].Value = "";
                row.Cells["PixelX"].Value = "";
                row.Cells["PixelY"].Value = "";
            }
            _calibService.ClearPoints();
            lblStatus.Text = "已清空所有数据";
            lblStatus.ForeColor = System.Drawing.Color.DarkBlue;
        }

        private void BtnClearRow_Click(object sender, EventArgs e)
        {
            if (dgvPoints.CurrentRow == null)
            {
                MessageBox.Show("请先选择一行", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int rowIndex = dgvPoints.CurrentRow.Index;
            dgvPoints.Rows[rowIndex].Cells["PhysicalX"].Value = "";
            dgvPoints.Rows[rowIndex].Cells["PhysicalY"].Value = "";
            dgvPoints.Rows[rowIndex].Cells["PixelX"].Value = "";
            dgvPoints.Rows[rowIndex].Cells["PixelY"].Value = "";

            lblStatus.Text = $"已清空第 {rowIndex + 1} 行数据";
            lblStatus.ForeColor = System.Drawing.Color.DarkBlue;
        }

        private void BtnFillMock_Click(object sender, EventArgs e)
        {
            if (dgvPoints.CurrentRow == null)
            {
                MessageBox.Show("请先选择一行", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int rowIndex = dgvPoints.CurrentRow.Index;

            // Generate mock data based on row index to simulate a realistic calibration grid
            // Physical coordinates: grid pattern starting at (100, 100) with 50mm spacing
            int row = rowIndex / 3;
            int col = rowIndex % 3;
            double physicalX = 100 + col * 50 + _random.NextDouble() * 0.5 - 0.25;
            double physicalY = 100 + row * 50 + _random.NextDouble() * 0.5 - 0.25;

            // Pixel coordinates: corresponding pixel positions starting at (500, 500) with 200px spacing
            double pixelX = 500 + col * 200 + _random.NextDouble() * 2 - 1;
            double pixelY = 500 + row * 200 + _random.NextDouble() * 2 - 1;

            dgvPoints.Rows[rowIndex].Cells["PhysicalX"].Value = physicalX.ToString("F3");
            dgvPoints.Rows[rowIndex].Cells["PhysicalY"].Value = physicalY.ToString("F3");
            dgvPoints.Rows[rowIndex].Cells["PixelX"].Value = pixelX.ToString("F3");
            dgvPoints.Rows[rowIndex].Cells["PixelY"].Value = pixelY.ToString("F3");

            lblStatus.Text = $"已向第 {rowIndex + 1} 行填入Mock数据";
            lblStatus.ForeColor = System.Drawing.Color.Green;
        }

        private void BtnCalibrate_Click(object sender, EventArgs e)
        {
            _calibService.ClearPoints();

            int validCount = 0;
            foreach (DataGridViewRow row in dgvPoints.Rows)
            {
                string physXStr = row.Cells["PhysicalX"].Value?.ToString()?.Trim() ?? "";
                string physYStr = row.Cells["PhysicalY"].Value?.ToString()?.Trim() ?? "";
                string pixXStr = row.Cells["PixelX"].Value?.ToString()?.Trim() ?? "";
                string pixYStr = row.Cells["PixelY"].Value?.ToString()?.Trim() ?? "";

                if (double.TryParse(physXStr, out double physX) &&
                    double.TryParse(physYStr, out double physY) &&
                    double.TryParse(pixXStr, out double pixX) &&
                    double.TryParse(pixYStr, out double pixY))
                {
                    _calibService.AddPoint(pixX, pixY, physX, physY);
                    validCount++;
                }
            }

            if (validCount < 3)
            {
                MessageBox.Show("有效标定点不足，至少需要3个完整点", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblStatus.Text = "标定失败：有效点不足";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            bool success = _calibService.Calibrate();
            if (success)
            {
                lblStatus.Text = $"标定成功！使用了 {validCount} 个点";
                lblStatus.ForeColor = System.Drawing.Color.Green;
                MessageBox.Show($"标定计算成功！共使用 {validCount} 个点", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                lblStatus.Text = "标定计算失败，请检查数据";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show("标定计算失败，请检查数据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Collect current data before saving
            _calibService.ClearPoints();
            foreach (DataGridViewRow row in dgvPoints.Rows)
            {
                string physXStr = row.Cells["PhysicalX"].Value?.ToString()?.Trim() ?? "";
                string physYStr = row.Cells["PhysicalY"].Value?.ToString()?.Trim() ?? "";
                string pixXStr = row.Cells["PixelX"].Value?.ToString()?.Trim() ?? "";
                string pixYStr = row.Cells["PixelY"].Value?.ToString()?.Trim() ?? "";

                if (double.TryParse(physXStr, out double physX) &&
                    double.TryParse(physYStr, out double physY) &&
                    double.TryParse(pixXStr, out double pixX) &&
                    double.TryParse(pixYStr, out double pixY))
                {
                    _calibService.AddPoint(pixX, pixY, physX, physY);
                }
            }

            RunDataService.Instance.CalibConfigService.Config = _calibService.ToConfig();
            RunDataService.Instance.CalibConfigService.Save();
            lblStatus.Text = "标定数据已保存";
            lblStatus.ForeColor = System.Drawing.Color.Green;
            MessageBox.Show("标定数据已保存", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            RunDataService.Instance.CalibConfigService.Load();
            _calibService.LoadFromConfig(RunDataService.Instance.CalibConfigService.Config);

            // Clear grid first
            foreach (DataGridViewRow row in dgvPoints.Rows)
            {
                row.Cells["PhysicalX"].Value = "";
                row.Cells["PhysicalY"].Value = "";
                row.Cells["PixelX"].Value = "";
                row.Cells["PixelY"].Value = "";
            }

            LoadExistingPoints();
            lblStatus.Text = "标定数据已加载";
            lblStatus.ForeColor = System.Drawing.Color.Green;
            MessageBox.Show("标定数据已加载", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CalibrationForm_Load(object sender, EventArgs e)
        {
            
            cogCalibNPointToNPointEditV2.Subject = _calibService.CalibTool;
            cogToolBlockEditV2.Subject = _calibService.FindMarkBlock;

        }
    }
}