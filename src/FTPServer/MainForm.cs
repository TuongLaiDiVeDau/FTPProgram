using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTPServer
{
    public partial class MainForm : Form
    {
        private LogForm logForm = null;

        public MainForm(LogForm logForm)
        {
            InitializeComponent();
            this.logForm = logForm;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(delegate ()
                {
                    MainForm_Shown(sender, e);
                }));
            else
            {
                // Load strings
                string strings = File.ReadAllText(ConfigTrigger.ConfigPath);

                // Convert to json
                ftpConfig = JsonSerializer.Deserialize<FTPConfig>(strings);

                // Load configs
                LoadConfigs();
            }
        }

        /// <summary>
        /// Fired when program is closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!cbAutoSave.Checked)
                if (!ftpConfig.Equals(ConfigTrigger.FTPConfig))
                {
                    switch (AskSaveConfig())
                    {
                        case DialogResult.Cancel:
                            e.Cancel = true;
                            return;
                        case DialogResult.Yes:
                            SaveConfig();
                            break;
                        default:
                            break;
                    }
                }

            if (FTPServerController.IsRunning)
            {
                btnStartStop_Click(btnStartStop, EventArgs.Empty);
            }

            if (e.Cancel == false)
            {
                Environment.Exit(0);
            }
        }

        // If this program is modifiying, prevent load config.
        private bool isModifying = false;
        // FTP Config
        private FTPConfig ftpConfig = null;
        // If this program has modified.
        private bool hasModified = false;

        private void LoadConfigs()
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(() => { LoadConfigs(); }));
            else
            {
                if (!isModifying)
                {
                    isModifying = true;

                    // Clear current items.
                    listView1.Items.Clear();

                    // Load config
                    tbServerPort.Text = ftpConfig.Port.ToString();

                    // Config: Load users
                    foreach (FTPUser user in ftpConfig.Users)
                    {
                        var temp = new ListViewItem();
                        temp.Text = "";
                        temp.SubItems.AddRange(new string[] { user.Name, user.CurrentDirectory });
                        listView1.Items.Add(temp);
                    }

                    // Config: Load autostarts
                    cbAutoStartAtProgram.Checked = ftpConfig.AutoStartAtOpen;

                    // Config: Auto save config
                    cbAutoSave.Checked = ftpConfig.ConfigAutoSave;

                    isModifying = false;

                    // If Auto Start at open was enabled, start them.
                    Task.Run(() =>
                    {
                        if (ftpConfig.AutoStartAtOpen)
                            btnStartStop_Click(btnStartStop, EventArgs.Empty);
                    });
                }
            }
        }

        /// <summary>
        /// Asks user for saving changes if user has modified config.
        /// </summary>
        /// <returns>Yes if no changes for user want to save. Otherwise No or Cancel.</returns>
        private DialogResult AskSaveConfig()
        {
            if (hasModified)
            {
                return MessageBox.Show(
                    "It looks like you has modified config but not saved.\nWe recommend you save config before running/exiting.\n\nDo you want to save config?",
                    this.Text,
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
            }
            else return DialogResult.Yes;
        }

        private void SaveConfig()
        {
            // Config: Users (already saved from other functions).

            // Config: Port
            ftpConfig.Port = Convert.ToInt32(tbServerPort.Text);

            // Config: Auto save
            ftpConfig.ConfigAutoSave = cbAutoSave.Checked;

            // Config: Auto start at open
            ftpConfig.AutoStartAtOpen = cbAutoStartAtProgram.Checked;

            // Save config to file.
            File.WriteAllText(ConfigTrigger.ConfigPath, JsonSerializer.Serialize(ftpConfig), Encoding.UTF8);

            // Reset HasModified
            hasModified = false;
        }

        /// <summary>
        /// Start/stop server event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartStop_Click(object sender, EventArgs e)
        {
            // Check if config changed. If not, continue.
            if (AskSaveConfig() != DialogResult.Yes)
                return;

            this.Invoke((MethodInvoker)delegate ()
            {
                Button btn = btnStartStop as Button;
                if (FTPServerController.IsRunning)
                {
                    ConfigTrigger.StopTrigger();
                    FTPServerController.Stop();
                    btn.Text = "Start Server";
                    startServerToolStripMenuItem.Text = "Start Server";
                    niSystemTray.ShowBalloonTip(7000, "FTP Server", "Server has stopped.", ToolTipIcon.Info);
                }
                else
                {
                    ConfigTrigger.InitializeTrigger();
                    FTPServerController.Start();
                    btn.Text = "Stop Server";
                    startServerToolStripMenuItem.Text = "Stop Server";
                    niSystemTray.ShowBalloonTip(7000, "FTP Server", "Server has started.", ToolTipIcon.Info);
                }
            });
        }

        /// <summary>
        /// Show about dialog event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAbout_Click(object sender, EventArgs e)
        {
            // TODO: Show about dialog.
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLog_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
                this.Invoke((MethodInvoker)delegate { btnLog_Click(sender, e); });
            else
            {
                logForm.Location = new Point(this.Left + this.Width, this.Top);
                logForm.Show();
            }
        }

        #region App Tray
        /// <summary>
        /// Show program (after hide) event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void niSystemTray_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        /// <summary>
        /// Show program (after hide) event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }
        #endregion

        #region App layout area
        /// <summary>
        /// Hide app event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHide_Click(object sender, EventArgs e)
        {
            this.Hide();
            niSystemTray.ShowBalloonTip(7000, "FTP Server - This app is hidden", "Double-click icon to open or right-click it for more options.", ToolTipIcon.Info);
        }

        /// <summary>
        /// Exit this app.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region ListView function area
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                try
                {
                    var form = new UserForm();
                    form.Top = this.Top + (this.Height - form.Height) / 2;
                    form.Left = this.Left + (this.Width - form.Width) / 2;

                    form.LoadUser(ftpConfig.Users.Where(p => p.Name == listView1.SelectedItems[0].SubItems[1].Text).First());
                    DialogResult dg = form.ShowDialog();
                    if (dg == DialogResult.OK)
                    {
                        // TODO: Edit a user.
                        FTPUser user = ftpConfig.Users.Where(p => p.Name == form.PreviousUser).First();
                        user.Name = form.User.Name;
                        user.Password = form.User.Password;
                        user.CurrentDirectory = form.User.CurrentDirectory;

                        // HasModified has true.
                        hasModified = true;

                        // Auto save if turned on
                        if (!isModifying && ftpConfig.ConfigAutoSave)
                            SaveConfig();

                        // Reload configs
                        LoadConfigs();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        String.Format("An error was encountered.\n\n{0}", ex.Message),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                        );
                }
            }
            // TODO: Edit a user.

                // HasModified has true.
                // hasModified = true;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                DialogResult dg = MessageBox.Show(
                    String.Format("Are you sure you want to delete \"{0}\"?\nThis cannot be undone!", listView1.SelectedItems[0].SubItems[1].Text),
                    this.Text,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dg == DialogResult.Yes)
                {
                    try
                    {
                        // Delete a user.
                        ftpConfig.Users.Remove(ftpConfig.Users.Where(p => p.Name == listView1.SelectedItems[0].SubItems[1].Text).First());

                        // HasModified has true.
                        hasModified = true;

                        // Auto save if turned on
                        if (!isModifying && ftpConfig.ConfigAutoSave)
                            SaveConfig();

                        // Reload configs
                        LoadConfigs();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            String.Format("An error was encountered.\n\n{0}", ex.Message),
                            this.Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                            );
                    }
                }
            }
            // TODO: Delete a user.

            // HasModified has true.
            // hasModified = true;
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            var form = new UserForm();
            form.Top = this.Top + (this.Height - form.Height) / 2;
            form.Left = this.Left + (this.Width - form.Width) / 2;

            DialogResult dg = form.ShowDialog();
            if (dg == DialogResult.OK)
            {
                // TODO: Add a user.
                ftpConfig.Users.Add(form.User);

                // HasModified has true.
                hasModified = true;

                // Auto save if turned on
                if (!isModifying && ftpConfig.ConfigAutoSave)
                    SaveConfig();

                // Reload configs
                LoadConfigs();
            }
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
                listView1.ContextMenuStrip = cmsUserArea;
            else listView1.ContextMenuStrip = null;
        }
        #endregion

        #region Server config
        /// <summary>
        /// Save config event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        /// <summary>
        /// Fired when user leave focus from Port TextBox. So, check if is vaild port.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbServerPort_Leave(object sender, EventArgs e)
        {
            try
            {
                int temp = int.Parse(tbServerPort.Text);
                if (temp > 0 && temp < 65536)
                    ftpConfig.Port = temp;
                else throw new Exception("Invaild port.");

                // HasModified has true.
                hasModified = true;

                if (!isModifying && ftpConfig.ConfigAutoSave)
                    SaveConfig();
            }
            catch
            {
                tbServerPort.Text = ftpConfig.Port.ToString();
            }
        }

        private void tbServerPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void cbAutoStartAtProgram_CheckedChanged(object sender, EventArgs e)
        {
            ftpConfig.AutoStartAtOpen = cbAutoStartAtProgram.Checked;
            if (!isModifying && ftpConfig.ConfigAutoSave)
                SaveConfig();
        }

        private void cbAutoSave_CheckedChanged(object sender, EventArgs e)
        {
            ftpConfig.ConfigAutoSave = cbAutoSave.Checked;
            if (!isModifying && ftpConfig.ConfigAutoSave)
                SaveConfig();
        }
        #endregion

    }
}
