using FTPClient.Library;
using FTPClient.Library.Controls;
using FTPClient.Library.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTPClient
{
    public partial class MainForm : Form
    {
        private FtpTaskList ftpTaskList = null;
        private FtpLog ftpLog = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ftpTaskList = new FtpTaskList();
            ftpTaskList.TaskProgressChanged += FtpTaskList_TaskProgressChanged;
            ftpTaskList.TaskCompleted += FtpTaskList_TaskCompleted;

            ftpLog = new FtpLog(ftpTaskList);

            btnConnectChange_Click(btnConnectChange, EventArgs.Empty);
        }

        private void btnConnectChange_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                FtpConnect form = new FtpConnect(ftpTaskList);
                form.Top = this.Top + (this.Height - form.Height) / 2;
                form.Left = this.Left + (this.Width - form.Width) / 2;
                DialogResult dg = form.ShowDialog();

                if (dg == DialogResult.OK)
                {
                    lbServer.Text = String.Format("Server: {0}:{1}, Username: {2}", ftpTaskList.Host, ftpTaskList.Port, ftpTaskList.Username);

                    fileBrowser.Enabled = true;
                    fileBrowser.Path = "/";
                }

                form.Dispose();
            }));
        }

        private void btnShowLog_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
                this.Invoke((MethodInvoker)delegate { btnShowLog_Click(sender, e); });
            else
            {
                ftpLog.Location = new Point(this.Left + this.Width, this.Top);
                ftpLog.Show();
            }
        }

        private void FtpTaskList_TaskCompleted(object sender, EventArgs e)
        {
            fileBrowser.RefreshView();
            FtpTask task = sender as FtpTask;
            ftpLog.UpdateLog(task);

            lbStatus.Invoke(
                new Action(() =>
                {
                    var temp = ftpTaskList.GetRunningTaskCount();
                    if (temp[0] != 0)
                        lbStatus.Text = String.Format("{0} task{1} running, total {2}% completed.", Math.Round(temp[0], 0), temp[0] > 1 ? "s" : null, Math.Round(temp[1], 1));
                    else
                        lbStatus.Text = "All task completed. Check log for details.";
                })
                );
        }

        private void FtpTaskList_TaskProgressChanged(object sender, EventArgs e)
        {
            FtpTask task = sender as FtpTask;
            ftpLog.UpdateLog(task);

            lbStatus.Invoke(
                new Action(() =>
                    {
                        var temp = ftpTaskList.GetRunningTaskCount();
                        if (temp[0] != 0)
                            lbStatus.Text = String.Format("{0} task{1} running, total {2}% completed.", Math.Round(temp[0], 0), temp[0] > 1 ? "s" : null, Math.Round(temp[1], 1));
                        else
                            lbStatus.Text = "All task completed. Check log for details.";
                    })
                );
        }

        private void fileBrowser_TaskRequested(object sender, FileBrowserTaskEventArgs e)
        {
            Thread thread = new Thread(() => ExecuteTask(e));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void ExecuteTask(FileBrowserTaskEventArgs e)
        {
            switch (e.TaskType)
            {
                case TaskType.Browse:
                    fileBrowser.ShowLoadingUI(true);
                    var dataList = ftpTaskList.GetItemList(e.SourcePath);
                    fileBrowser.LoadView(dataList);
                    fileBrowser.ShowLoadingUI(false);
                    break;
                case TaskType.FileDownload:
                    string fileName = e.SourcePath.IndexOf("/") > -1 ? e.SourcePath.Substring(e.SourcePath.IndexOf("/")) : e.SourcePath;
                    SaveFileDialog saveDiag = new SaveFileDialog();
                    saveDiag.FileName = fileName;
                    saveDiag.Title = String.Format("Save \"{0}\"to", fileName);
                    saveDiag.Filter = "All files|*.*";
                    saveDiag.OverwritePrompt = true;
                    if (saveDiag.ShowDialog() == DialogResult.OK)
                        ftpTaskList.FileDownload(String.Format("{0}/{1}", e.CurrentDirectoryPath, e.SourcePath), saveDiag.FileName);
                    break;
                case TaskType.FileUpload:
                    OpenFileDialog openDiag = new OpenFileDialog();
                    openDiag.Title = "Choose file to upload";
                    openDiag.Filter = "All files|*.*";
                    openDiag.CheckFileExists = true;
                    if (openDiag.ShowDialog() == DialogResult.OK)
                        ftpTaskList.FileUpload(openDiag.FileName, String.Format("{0}/{1}", e.CurrentDirectoryPath, Path.GetFileName(openDiag.FileName)));
                    break;
                case TaskType.Rename:
                    FtpRename formRename = new FtpRename();
                    formRename.Top = this.Top + (this.Height - formRename.Height) / 2;
                    formRename.Left = this.Left + (this.Width - formRename.Width) / 2;
                    formRename.OriginalName = e.SourcePath;
                    if (formRename.ShowDialog() == DialogResult.OK)
                        ftpTaskList.RenameOrMove(
                            String.Format("{0}/{1}", e.CurrentDirectoryPath, formRename.OriginalName),
                            String.Format("{0}/{1}", e.CurrentDirectoryPath, formRename.RenameTo)
                            );
                    break;
                case TaskType.DirectoryCreate:
                    FtpDirectoryCreate formDirectoryCreate = new FtpDirectoryCreate();
                    formDirectoryCreate.Top = this.Top + (this.Height - formDirectoryCreate.Height) / 2;
                    formDirectoryCreate.Left = this.Left + (this.Width - formDirectoryCreate.Width) / 2;
                    if (formDirectoryCreate.ShowDialog() == DialogResult.OK)
                        ftpTaskList.DirectoryCreate(String.Format("{0}/{1}", e.CurrentDirectoryPath, formDirectoryCreate.FolderName));
                    break;
                case TaskType.FileDelete:
                case TaskType.DirectoryDelete:
                    DialogResult fileDelDiag = MessageBox.Show(
                        String.Format("Are you sure you want to delete \"{0}\"?\nThis cannot be undone!", e.SourcePath),
                        this.Text,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                        );
                    if (fileDelDiag == DialogResult.Yes)
                    {
                        if (e.TaskType == TaskType.FileDelete)
                            ftpTaskList.FileDelete(String.Format("{0}/{1}", e.CurrentDirectoryPath, e.SourcePath));
                        else
                            ftpTaskList.DirectoryDelete(String.Format("{0}/{1}", e.CurrentDirectoryPath, e.SourcePath));
                    }
                    break;
                default:
                    break;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
