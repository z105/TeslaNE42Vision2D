using System;
using System.Windows.Forms;

namespace TeslaNE42Vision2D.Views
{
    public partial class PasswordDialog : Form
    {
        public PasswordDialog()
        {
            InitializeComponent();
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                BtnOk_Click(sender, e);
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string Password => txtPassword.Text;

        public static bool Verify(string correctPassword, IWin32Window owner = null)
        {
            using (var dlg = new PasswordDialog())
            {
                if (dlg.ShowDialog(owner) == DialogResult.OK)
                    return dlg.Password == correctPassword;
                return false;
            }
        }
    }
}