using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FTPClient.Library.Forms
{
    public partial class FtpConnect : Form
    {
        FtpTaskList ftpTask = null;

        public FtpConnect(FtpTaskList ftpTask)
        {
            InitializeComponent();
            this.ftpTask = ftpTask;
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

        public string Host
        {
            get { return tbHost.Text; }
        }

        public string Username
        {
            get { return tbUser.Text; }
        }

        public string Password
        {
            get { return tbPass.Text; }
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
                this.Invoke((MethodInvoker)delegate { btnConnect_Click(sender, e); });
            else
            {
                lbStatus.Text = "Logging in...";
                bw1.RunWorkerAsync();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void LoginControlEnabled(bool value)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(() => { LoginControlEnabled(value); }));
            else
            {
                tbHost.Enabled = value;
                tbUser.Enabled = value;
                tbPass.Enabled = value;
                btnConnect.Enabled = value;
            }
        }

        private void FtpConnect_Shown(object sender, EventArgs e)
        {
            tbHost.Focus();
        }

        private void bw1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string[] addressSplitted = tbHost.Text.Replace("ftp://", "").Split(":");
                string host = addressSplitted[0];
                int port = -1;
                if (addressSplitted.Length > 1)
                    port = Convert.ToInt32(addressSplitted[1]);

                LoginControlEnabled(false);

                FtpTask ftpTask = new FtpTask();
                ftpTask.Host = host;
                if (port > -1)
                    ftpTask.Port = port;
                ftpTask.Username = tbUser.Text;
                ftpTask.Password = tbPass.Text;
                ftpTask.Type = TaskType.Browse;
                ftpTask.ServerSourcePath = "/";
                ftpTask.Start();

                if (ftpTask.Result == TaskResult.Successful)
                    e.Result = true;
                else e.Result = false;
            }
            catch (Exception ex)
            {
                // TODO: Exception here.
                Console.WriteLine(ex.ToString());
            }
        }

        private void bw1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((bool)e.Result == true)
            {
                lbStatus.Text = "Successfully login!";

                string[] addressSplitted = tbHost.Text.Replace("ftp://", "").Split(":");
                ftpTask.Host = addressSplitted[0];
                if (addressSplitted.Length > 1)
                    ftpTask.Port = Convert.ToInt32(addressSplitted[1]);
                ftpTask.Username = tbUser.Text;
                ftpTask.Password = tbPass.Text;

                DialogResult = DialogResult.OK;
            }
            else
            {
                lbStatus.Text = "Failed while logging you in!\nCheck your login information and try again.\nIf everything is correct, check your internet connection.";
                LoginControlEnabled(true);
            }
        }

        private void FtpConnect_Load(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(() => FtpConnect_Load(sender, e)));
            else
            {
                if (!String.IsNullOrEmpty(ftpTask.Host))
                    tbHost.Text = String.Format("{0}:{1}", ftpTask.Host, ftpTask.Port);
                tbUser.Text = ftpTask.Username;
                tbPass.Text = ftpTask.Password;
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnConnect_Click(btnConnect, EventArgs.Empty);
            }
        }
    }
}
