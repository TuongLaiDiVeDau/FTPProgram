using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FTPServer
{
    public partial class UserForm : Form
    {
        public UserForm()
        {
            InitializeComponent();
        }

        private FTPUser user = null;
        public FTPUser User
        {
            get { return user; }
        }

        private string preUser = null;
        public string PreviousUser
        {
            get { return preUser; }
        }

        private bool isEdit = false;
        public bool IsEdit
        {
            get { return isEdit; }
        }

        public void LoadUser(FTPUser user)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(() => { LoadUser(user); }));
            else
            {
                this.user = user;
                this.preUser = user.Name;

                tbUser.Text = user.Name;
                tbPass.Text = user.Password;
                tbHomeDir.Text = user.CurrentDirectory;

                isEdit = true;
                this.Text = String.Format("FTP Server - Edit a user ({0})", this.PreviousUser);
                this.label4.Text = "Edit a user";
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                if (tbPass.UseSystemPasswordChar)
                {
                    tbPass.UseSystemPasswordChar = false;
                    btnShow.Text = "Hide";
                }
                else
                {
                    tbPass.UseSystemPasswordChar = true;
                    btnShow.Text = "Show";
                }
            }));
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult diag = folderBrowserDialog1.ShowDialog();

            if (diag == DialogResult.OK)
                tbHomeDir.Text = folderBrowserDialog1.SelectedPath;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!IsEdit)
            {
                user = new FTPUser();
            }

            user.Name = tbUser.Text;
            user.Password = tbPass.Text;
            user.CurrentDirectory = tbHomeDir.Text;

            DialogResult = DialogResult.OK;
        }
    }
}
