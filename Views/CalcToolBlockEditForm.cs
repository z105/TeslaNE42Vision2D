using Cognex.VisionPro;
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
    public partial class CalcToolBlockEditForm : Form
    {
        private string path;

        private CogToolBlock cogToolBlock;

        public CalcToolBlockEditForm(CogToolBlock cogToolBlock, string toolBlockPath)
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
    }
}
