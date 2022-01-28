using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTPClient.Library
{
    public class FtpTaskList
    {
        public FtpTaskList()
        {
            ftpTaskList = new List<FtpTask>();
        }

        ~FtpTaskList()
        {

        }

        #region Public variables
        /// <summary>
        /// Gets or sets FTP Host.
        /// </summary>
        public string Host
        {
            get { return _host; }
            set
            {
                if (value.ToLower().StartsWith("ftp://"))
                    _host = value.Replace("ftp://", "");
                else _host = value;
            }
        }
        private string _host = null;

        /// <summary>
        /// Gets or sets FTP Port.
        /// </summary>
        public int Port { get; set; } = 21;

        /// <summary>
        /// Gets or sets username to login to FTP.
        /// </summary>
        public string Username { get; set; } = null;

        /// <summary>
        /// Gets or sets password to login to FTP.
        /// </summary>
        public string Password { get; set; } = null;

        /// <summary>
        /// Gets or sets buffer size to increase or decrease task as much as task can. Default is 4 KB.
        /// </summary>
        public int BufferSize { get; set; } = 4096;
        #endregion

        #region Public events
        /// <summary>
        /// Fired when a task progress is changed.
        /// </summary>
        public event EventHandler TaskProgressChanged;

        /// <summary>
        /// Fired when a task is completed.
        /// </summary>
        public event EventHandler TaskCompleted;
        #endregion

        #region Control tasks
        /// <summary>
        /// Add task to task list and start.
        /// </summary>
        /// <param name="ftpTask"></param>
        private void AddAndStartTask(FtpTask ftpTask)
        {
            nextID++;
            ftpTask.ID = nextID;
            ftpTaskList.Add(ftpTask);
            ftpTask.Start();
        }
        int nextID = 0;

        /// <summary>
        /// Stop a task from ID.
        /// </summary>
        /// <param name="ID">ID which task to stop</param>
        public void StopTask(int ID)
        {
            try
            {
                ftpTaskList.Where(p => p.ID == ID).First().Stop();
            }
            catch (Exception ex)
            {
                // TODO: Exception here.
                Console.WriteLine(ex.ToString());
            }
        }

        public double[] GetRunningTaskCount()
        {
            try
            {
                double[] result = new double[2];

                List<FtpTask> ftp = ftpTaskList.Where(p => p.Result == TaskResult.Running).ToList();
                result[0] = ftp.Count;

                double sum = 0;
                int count = 0;
                foreach (FtpTask ftpTask in ftp)
                {
                    sum += ftpTask.Progress;
                    count++;
                }
                result[1] = sum / count;

                return result;
            }
            catch
            {
                return new double[] {0, 0};
            }
        }
        #endregion

        /// <summary>
        /// List of FTP Task (private).
        /// </summary>
        private List<FtpTask> ftpTaskList = null;

        /// <summary>
        /// Quickly create a FTP Task.
        /// </summary>
        /// <returns></returns>
        private FtpTask InitializeQuickFTPTask()
        {
            FtpTask ftpTask = new FtpTask();
            ftpTask.Host = Host;
            ftpTask.Port = Port;
            ftpTask.Username = Username;
            ftpTask.Password = Password;
            ftpTask.BufferSize = BufferSize;

            ftpTask.TaskCompleted += FtpTask_TaskCompleted;
            if (TaskProgressChanged != null)
                ftpTask.TaskProgressChanged += TaskProgressChanged;

            return ftpTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FtpTask_TaskCompleted(object sender, EventArgs e)
        {
            if (TaskCompleted != null)
                TaskCompleted(sender, e);

            FtpTask ftpTask = sender as FtpTask;
            ftpTaskList.Remove(ftpTask);
        }

        #region Task functions
        /// <summary>
        /// Download a file from server and save to computer.
        /// </summary>
        /// <param name="serverPath">Path from server</param>
        /// <param name="localPath">Path with file will save to local</param>
        public void FileDownload(string serverPath, string localPath)
        {
            FtpTask ftpTask = InitializeQuickFTPTask();
            ftpTask.Type = TaskType.FileDownload;
            ftpTask.ServerSourcePath = serverPath;
            ftpTask.LocalPath = localPath;
            AddAndStartTask(ftpTask);
        }

        /// <summary>
        /// Upload a file from computer to server.
        /// </summary>
        /// <param name="localPath">Path from local</param>
        /// <param name="serverPath">Path with file will save to server</param>
        public void FileUpload(string localPath, string serverPath)
        {
            FtpTask ftpTask = InitializeQuickFTPTask();
            ftpTask.Type = TaskType.FileUpload;
            ftpTask.ServerSourcePath = serverPath;
            ftpTask.LocalPath = localPath;
            AddAndStartTask(ftpTask);
        }

        /// <summary>
        /// Rename or move a file or directory on server. 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public void RenameOrMove(string source, string target)
        {
            FtpTask ftpTask = InitializeQuickFTPTask();
            ftpTask.Type = TaskType.Rename;
            ftpTask.ServerSourcePath = source;
            ftpTask.ServerTargetPath = target;
            AddAndStartTask(ftpTask);
        }

        /// <summary>
        /// Delete a file on server.
        /// </summary>
        /// <param name="serverPath">Path with file want to delete from</param>
        public void FileDelete(string serverPath)
        {
            FtpTask ftpTask = InitializeQuickFTPTask();
            ftpTask.Type = TaskType.FileDelete;
            ftpTask.ServerSourcePath = serverPath;
            AddAndStartTask(ftpTask);
        }

        /// <summary>
        /// Create a directory to server.
        /// </summary>
        /// <param name="serverPath">Path with directory want to create to</param>
        public void DirectoryCreate(string serverPath)
        {
            FtpTask ftpTask = InitializeQuickFTPTask();
            ftpTask.Type = TaskType.DirectoryCreate;
            ftpTask.ServerSourcePath = serverPath;
            AddAndStartTask(ftpTask);
        }

        /// <summary>
        /// Delete a directory from server.
        /// </summary>
        /// <param name="serverPath">Path with directory want to delete from</param>
        public void DirectoryDelete(string serverPath)
        {
            FtpTask ftpTask = InitializeQuickFTPTask();
            ftpTask.Type = TaskType.DirectoryDelete;
            ftpTask.ServerSourcePath = serverPath;
            AddAndStartTask(ftpTask);
        }

        /// <summary>
        /// Get list of file and directory 
        /// </summary>
        /// <param name="serverPath"></param>
        /// <returns>If successful, it will return a list items in serverPath. Otherwise, an exception will occured.</returns>
        public List<ItemInfo> GetItemList(string serverPath)
        {
            FtpTask ftpTask = new FtpTask();
            ftpTask.Host = Host;
            ftpTask.Port = Port;
            ftpTask.Username = Username;
            ftpTask.Password = Password;
            ftpTask.BufferSize = BufferSize;

            ftpTask.ID = ++nextID;
            ftpTask.Type = TaskType.Browse;
            ftpTask.ServerSourcePath = serverPath;
            ftpTask.Start();
            if (ftpTask.Result == TaskResult.Successful)
                return (List<ItemInfo>)ftpTask.Data;
            else throw new Exception(
                String.Format("Server has return with code {0}.", ftpTask.Result)
                );
        }
        #endregion

    }
}
