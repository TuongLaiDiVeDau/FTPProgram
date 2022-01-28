using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FTPClient.Library.Forms
{
    public partial class FtpDirectoryCreate : Form
    {
        public FtpDirectoryCreate()
        {
            InitializeComponent();
        }

        private bool _darkMode = false;

        public bool DarkMode
        {
            get { return _darkMode; }
            set
            {
                _darkMode = value;
                // TODO: Dark mode here.
            }
        }

        public string FolderName
        {
            get { return tbFolderName.Text; }
            set { tbFolderName.Text = value; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
