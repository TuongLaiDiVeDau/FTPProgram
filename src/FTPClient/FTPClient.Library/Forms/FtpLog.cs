using FTPClient.Library;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace FTPClient
{
    public partial class FtpLog : Form
    {
        private FtpTaskList ftpTaskList = null;

        public FtpLog(FtpTaskList ftpTaskList)
        {
            InitializeComponent();
            this.ftpTaskList = ftpTaskList;
        }

        public void UpdateLog(FtpTask task)
        {
            if (this.InvokeRequired)
                this.Invoke((MethodInvoker)delegate { UpdateLog(task); });
            else
            {
                try
                {
                    ListViewItem lvItem = null;

                    int index = -1;
                    var temp = listView.FindItemWithText(task.ID.ToString());
                    if (temp != null)
                        index = listView.Items.IndexOf(temp);

                    // If exist in ListView, call them.
                    if (index > -1)
                    {
                        lvItem = listView.Items[index];
                    }
                    // Otherwise, create them and add it to ListView.
                    else
                    {
                        lvItem = new ListViewItem();
                        listView.Items.Add(lvItem);
                    }

                    lvItem.SubItems.Clear();
                    lvItem.Text = task.ID.ToString();
                    lvItem.SubItems.AddRange(new string[]
                    {
                        task.Type.ToString(),
                        task.Result == TaskResult.Running ? String.Format("{0}% completed", task.Progress) : task.Result.ToString(),
                        // TODO: Need change 2 lines here.
                        task.ServerSourcePath == null ? "" : task.ServerSourcePath,
                        task.LocalPath == null ? "" : task.LocalPath,
                        task.Message == null ? "" : task.Message
                    });
                }
                catch (Exception ex)
                {
                    // TODO: Exception here
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void LogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void copyMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 1)
            {
                String temp = listView.SelectedItems[0].SubItems[5].Text;
                if (!String.IsNullOrEmpty(temp))
                    Clipboard.SetText(temp);
            }
        }

        private void copyLocalPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 1)
                Clipboard.SetText(listView.SelectedItems[0].SubItems[4].Text);
        }

        private void openLocalPathInWindowsExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 1)
                Process.Start("explorer.exe", "/select,\"" + listView.SelectedItems[0].SubItems[4].Text + "\"");
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
                listView.ContextMenuStrip = contextMenuStrip1;
            else
                listView.ContextMenuStrip = null;
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 1)
                ftpTaskList.StopTask(Convert.ToInt32(listView.SelectedItems[0].Text));
        }
    }
}
