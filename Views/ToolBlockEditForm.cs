using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.ToolBlock;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TeslaNE42Vision2D.Views
{
    public partial class ToolBlockEditForm : Form
    {
        private string path;

        private CogToolBlock cogToolBlock;

        public ToolBlockEditForm(CogToolBlock cogToolBlock, string toolBlockPath)
        {
            InitializeComponent();

            this.path = toolBlockPath;
            this.cogToolBlock = cogToolBlock;
            cogToolBlockEditV21.Subject = cogToolBlock;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                path = Path.Combine(AppContext.BaseDirectory, "toolblocks", path);
                CogSerializer.SaveObjectToFile(cogToolBlockEditV21.Subject, path);
                MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败！" + ex.Message);
            }

        }

        private void ToolBlockEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            cogToolBlockEditV21.Dispose();
            GC.Collect();
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDlg = new OpenFileDialog();
                openFileDlg.Filter = "图片文件|*.png;";
                if (openFileDlg.ShowDialog() == DialogResult.OK)
                {
                    //cogToolBlockEditV21.LoadImage(openFileDlg.FileName);
                    txtPolarityImagePath.Text = openFileDlg.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败！" + ex.Message);
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPolarityImagePath.Text))
            {
                MessageBox.Show("请选择图片！");
                return;
            }
            try
            {
                //CogImageFileTool cogImageFileTool = new CogImageFileTool();
                //cogImageFileTool.Operator.Open(txtPolarityImagePath.Text, CogImageFileModeConstants.Read);

                //cogImageFileTool.Run();
                //ICogImage cogImage = cogImageFileTool.OutputImage;

                ICogImage barcodeImage = new CogImage8Grey(new Bitmap(txtBarcodeImagePath.Text));
                ICogImage polarityImage = new CogImage8Grey(new Bitmap(txtPolarityImagePath.Text));

                if (cogToolBlock == null)
                {
                    return;
                }

                this.cogToolBlock.Inputs["ImageBarcode"].Value = barcodeImage;
                this.cogToolBlock.Inputs["ImagePolarity"].Value = polarityImage;
                this.cogToolBlock.Run();


            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败！" + ex.Message);
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                path = Path.Combine(AppContext.BaseDirectory, "toolblocks", path);
                CogSerializer.SaveObjectToFile(cogToolBlockEditV21.Subject, path);
                MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败！" + ex.Message);
            }
        }

        private void btnSelectBarcodeImage_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDlg = new OpenFileDialog();
                openFileDlg.Filter = "图片文件|*.png;";
                if (openFileDlg.ShowDialog() == DialogResult.OK)
                {
                    //cogToolBlockEditV21.LoadImage(openFileDlg.FileName);
                    txtBarcodeImagePath.Text = openFileDlg.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败！" + ex.Message);
            }
        }
    }
}
