using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FTPServer
{
    public class ControlWriter : TextWriter
    {
        private RichTextBox rtb;

        public ControlWriter(RichTextBox rtb)
        {
            this.rtb = rtb;
        }

        public override void Write(char value)
        {
            AppendToRTB(value.ToString());
        }

        public override void Write(string value)
        {
            AppendToRTB(value);
        }

        private void AppendToRTB(string value)
        {
            if (rtb.InvokeRequired)
                rtb.Invoke(new Action(() => { Write(value); }));
            else
            {
                rtb.AppendText(value);
            }
        }

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}
