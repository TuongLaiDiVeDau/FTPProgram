using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FTPClient.Library.Controls
{
    public partial class LoadingControls : UserControl
    {
        public LoadingControls()
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

        private void LoadingControls_SizeChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(() => { LoadingControls_SizeChanged(sender, e); }));
            else
            {
                label1.Top = (this.Height - label1.Height) / 2;
                label1.Left = (this.Width - label1.Width) / 2;
            }
        }

        private void LoadingControls_Load(object sender, EventArgs e)
        {
            LoadingControls_SizeChanged(sender, e);
        }
    }
}
