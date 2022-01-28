using System;
using System.Windows.Forms;

namespace FTPServer
{
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
        }

        public RichTextBox RichTextBoxObj
        {
            get { return richTextBox1; }
        }

        public void AddToLog(string msg)
        {
            if (richTextBox1.InvokeRequired)
                richTextBox1.Invoke(new Action(() => { AddToLog(msg); }));
            else
            {
                if (!String.IsNullOrEmpty(msg))
                    richTextBox1.AppendText(msg);
            }
        }

        private void LogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
