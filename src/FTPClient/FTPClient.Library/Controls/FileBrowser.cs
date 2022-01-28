using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FTPClient.Library.Controls
{
    public partial class FileBrowser : UserControl
    {
        public FileBrowser()
        {
            InitializeComponent();
            ShowLoadingUI(false);
        }

        #region Dark mode for control
        /// <summary>
        /// Gets or sets dark mode for control.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        [DisplayName("DarkMode")]
        [Description("Gets or sets dark mode for control.")]
        public bool DarkMode
        {
            get { return _darkMode; }
            set
            {
                _darkMode = value;
                // TODO: Dark mode here.
            }
        }

        private bool _darkMode = false;
        #endregion

        #region Current Directory path
        public string Path
        {
            get { return _path; }
            set { SetPath(value); }
        }
        private string _path = "/";

        private void SetPath(string path)
        {
            if (this.InvokeRequired)
                this.Invoke((MethodInvoker)delegate { SetPath(path); });
            else
            {
                if (path.Length == 0)
                {
                    _path = "/";
                    tbPath.Text = _path;
                }
                else
                {
                    _path = path;
                    tbPath.Text = _path;
                }

                if (TaskRequested != null)
                    TaskRequested(this, new FileBrowserTaskEventArgs()
                    {
                        TaskType = TaskType.Browse,
                        CurrentDirectoryPath = Path,
                        SourcePath = Path
                    });
            }
        }
        #endregion

        /// <summary>
        /// Fired when a task has requested.
        /// </summary>
        public event EventHandler<FileBrowserTaskEventArgs> TaskRequested;


        /// <summary>
        /// Refresh - Request load items info.
        /// </summary>
        public void RefreshView()
        {
            Path = Path;
        }

        /// <summary>
        /// Load items to ListView.
        /// </summary>
        /// <param name="list"></param>
        public void LoadView(List<ItemInfo> list)
        {
            if (listView.InvokeRequired)
                listView.Invoke((MethodInvoker)delegate { LoadView(list); });
            else
            {
                // Clear current list view
                listView.Items.Clear();

                try
                {
                    List<ItemInfo> list1 = new List<ItemInfo>();
                    list1.AddRange(list.Where(p => p.Type.ToString().ToLower() == "directory").OrderBy(s => s.Name).ToList());
                    list1.AddRange(list.Where(p => p.Type.ToString().ToLower() == "file").OrderBy(s => s.Name).ToList());

                    foreach (ItemInfo item in list1)
                    {
                        var itemRow = new ListViewItem(item.Name);
                        itemRow.SubItems.Add(item.Type.ToString());
                        itemRow.SubItems.Add(item.Size.HasValue ? item.SizeToString() : null);
                        itemRow.SubItems.Add(item.Date.HasValue ? item.Date.Value.ToString("dd/MM/yyyy hh:mm tt") : null);
                        itemRow.SubItems.Add(item.Permission);
                        listView.Items.Add(itemRow);
                    }
                }
                catch (Exception ex)
                {
                    // TODO: Exception here
                    Console.WriteLine(ex.ToString());
                }

                // Bring ListView to front
                ShowLoadingUI(false);
            }
        }

        public void ShowLoadingUI(bool show)
        {
            if (this.InvokeRequired)
                this.Invoke((MethodInvoker)delegate { ShowLoadingUI(show); });
            else
            {
                if (show)
                    loadingControls1.BringToFront();
                else
                    listView.BringToFront();
            }
        }

        private void pAddress_Click(object sender, EventArgs e)
        {
            tbPath.Focus();
        }

        private void tbPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // TODO: Load view here
                e.SuppressKeyPress = true;
                Path = tbPath.Text;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (Path.LastIndexOf('/') > -1)
                Path = Path.Remove(Path.LastIndexOf('/'));
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (TaskRequested != null)
                TaskRequested(this, new FileBrowserTaskEventArgs()
                {
                    TaskType = TaskType.FileUpload,
                    CurrentDirectoryPath = Path
                });
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
                if (listView.SelectedItems[0].SubItems[1].Text == "File")
                    if (TaskRequested != null)
                        TaskRequested(this, new FileBrowserTaskEventArgs()
                        {
                            TaskType = TaskType.FileDownload,
                            CurrentDirectoryPath = Path,
                            SourcePath = String.Format("{0}", listView.SelectedItems[0].Text),
                        });
        }

        private void btnNewFolder_Click(object sender, EventArgs e)
        {
            if (TaskRequested != null)
                TaskRequested(this, new FileBrowserTaskEventArgs()
                {
                    TaskType = TaskType.DirectoryCreate,
                    CurrentDirectoryPath = Path,
                });
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 1)
                if (TaskRequested != null)
                    TaskRequested(this, new FileBrowserTaskEventArgs()
                    {
                        TaskType = TaskType.Rename,
                        CurrentDirectoryPath = Path,
                        SourcePath = String.Format("{0}", listView.SelectedItems[0].Text),
                    });
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // TODO: 1 item only.
            if (listView.SelectedItems.Count == 1)
            {
                var type = (listView.SelectedItems[0].SubItems[1].Text == "File")
                    ? TaskType.FileDelete : TaskType.DirectoryDelete;

                if (TaskRequested != null)
                    TaskRequested(this, new FileBrowserTaskEventArgs()
                    {
                        TaskType = type,
                        CurrentDirectoryPath = Path,
                        SourcePath = String.Format("{0}", listView.SelectedItems[0].Text),
                    });
            }
        }

        private void btnShowLog_Click(object sender, EventArgs e)
        {
            if (TaskRequested != null)
                TaskRequested(this, new FileBrowserTaskEventArgs()
                {
                    TaskType = TaskType.ShowLog
                });
        }

        private void listView_ItemActivate(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                switch (listView.SelectedItems[0].SubItems[1].Text.ToLower())
                {
                    case "directory":
                        // Bring Loading UI to front.
                        ShowLoadingUI(true);
                        if (Path == "/")
                            Path += String.Format("{0}", listView.SelectedItems[0].Text);
                        else
                            Path += String.Format("/{0}", listView.SelectedItems[0].Text);
                        break;
                    case "file":
                        if (TaskRequested != null)
                            TaskRequested(this, new FileBrowserTaskEventArgs()
                            {
                                TaskType = TaskType.FileDownload,
                                CurrentDirectoryPath = Path,
                                SourcePath = String.Format("{0}", listView.SelectedItems[0].Text),
                            });
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
