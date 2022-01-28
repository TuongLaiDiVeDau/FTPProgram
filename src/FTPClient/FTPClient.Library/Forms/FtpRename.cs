using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FTPClient.Library.Forms
{
    public partial class FtpRename : Form
    {
        public FtpRename()
        {
            InitializeComponent();
        }

        private bool darkMode = false;

        public void EnableDarkMode(bool darkMode)
        {
            // TODO: Dark mode here.
        }

        public string OriginalName
        {
            get { return tbOriginal.Text; }
            set { tbOriginal.Text = value; }
        }

        public string RenameTo
        {
            get { return tbRenameTo.Text; }
            set { tbRenameTo.Text = value; }
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
