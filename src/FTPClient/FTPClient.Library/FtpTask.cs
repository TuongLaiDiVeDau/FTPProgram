using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace FTPClient.Library
{
    public class FtpTask
    {
        public FtpTask()
        {

        }

        ~FtpTask()
        {
            try
            {
                if (ftpWebRequest != null)
                {
                    ftpWebRequest.Abort();
                    ftpWebRequest = null;
                }
            }
            catch (Exception ex)
            {
                // TODO: Exception here.
                Console.WriteLine(ex.ToString());
            }
        }

        #region FTP Task server login info and configs
        /// <summary>
        /// Gets or sets FTP Task ID
        /// </summary>
        public int ID { get; set; } = 0;

        /// <summary>
        /// Gets or sets FTP Server host (not need to import ftp:// or sftp://).
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
        /// Get final host (host:port).
        /// </summary>
        /// <returns></returns>
        private string GetFinalHost()
        {
            return String.Format(
                "{0}{1}:{2}",
                "ftp://",
                Host,
                Port
                );
        }

        /// <summary>
        /// Gets or sets FTP Server port.
        /// </summary>
        public int Port { get; set; } = 21;

        /// <summary>
        /// Gets or sets username on FTP Server.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Private gets or sets password on FTP Server.
        /// </summary>
        public string Password
        {
            private get { return _password; }
            set { _password = value; }
        }
        private string _password = null;
        
        /// <summary>
        /// Gets or sets buffer size for task (default is 4 KB).
        /// </summary>
        public int BufferSize { get; set; } = 4096;
        #endregion

        #region FTP Task 
        /// <summary>
        /// Gets or sets path on local (PC).
        /// </summary>
        public string LocalPath { get; set; } = null;

        /// <summary>
        /// Gets or sets source path on server.
        /// </summary>
        public string ServerSourcePath { get; set; } = null;

        /// <summary>
        /// Gets or sets target path on server.
        /// </summary>
        public string ServerTargetPath { get; set; } = null;

        /// <summary>
        /// Gets or sets task type.
        /// </summary>
        public TaskType Type { get; set; } = TaskType.Unknown;
        #endregion

        #region FTP Task Progress
        /// <summary>
        /// Gets or sets FTP Task data.
        /// </summary>
        public Object Data { get; set; } = null;

        /// <summary>
        /// Gets or sets FTP Task result.
        /// </summary>
        public TaskResult Result { get; set; } = TaskResult.Unknown;

        /// <summary>
        /// Gets or sets FTP Task size total for file.
        /// </summary>
        public long SizeTotal { get; set; } = 0;

        /// <summary>
        /// Gets or sets FTP Task size completed for file.
        /// </summary>
        public long SizeCompleted { get; set; } = 0;

        /// <summary>
        /// Gets or sets FTP Task progress by percent for file.
        /// </summary>
        public double Progress
        {
            get
            {
                if (SizeCompleted == 0)
                    return 0;
                else 
                    return Math.Round((double)SizeCompleted / (double)SizeTotal * 100.0, 2);
            }
        }

        /// <summary>
        /// Gets or sets FTP Task message (response).
        /// </summary>
        public string Message { get; set; } = null;
        #endregion

        #region FTP Web Request, events and controls
        private FtpWebRequest ftpWebRequest = null;

        /// <summary>
        /// Fired when task progress changed.
        /// </summary>
        public event EventHandler TaskProgressChanged;

        /// <summary>
        /// Fired when task transfer completed or cancelled.
        /// </summary>
        public event EventHandler TaskCompleted;

        private void InitializeFTPWebRequest(string method, string source = null, string renameTo = null)
        {
            ftpWebRequest = (FtpWebRequest)WebRequest.Create(source);
            ftpWebRequest.UseBinary = true;
            ftpWebRequest.KeepAlive = true;
            ftpWebRequest.UsePassive = true;
            ftpWebRequest.Credentials = new NetworkCredential(Username, Password);
            if (renameTo != null)
                ftpWebRequest.RenameTo = renameTo;
            ftpWebRequest.Method = method;
        }

        /// <summary>
        /// Start FTP transfer task.
        /// </summary>
        public void Start()
        {
            if (Result != TaskResult.Unknown)
                throw new Exception("This task has ran. Create new task and try again.");

            switch (Type)
            {
                case TaskType.Browse:
                    FTPTask_Browse();
                    break;
                case TaskType.FileDownload:
                    FTPTask_FileDownload();
                    break;
                case TaskType.FileUpload:
                    FTPTask_FileUpload();
                    break;
                case TaskType.Move:
                case TaskType.Rename:
                    FTPTask_RenameOrMove();
                    break;
                case TaskType.FileDelete:
                    FTPTask_FileDelete();
                    break;
                case TaskType.DirectoryCreate:
                    FTPTask_DirectoryCreate();
                    break;
                case TaskType.DirectoryDelete:
                    FTPTask_DirectoryDelete();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Stop FTP transfer task.
        /// </summary>
        public void Stop()
        {
            Result = TaskResult.Cancelled;
            ftpWebRequest.Abort();
        }
        #endregion

        #region FTP Task background
        /// <summary>
        /// Get file size.
        /// </summary>
        /// <param name="source">Source path</param>
        /// <param name="online">File on local (false) or server (true)</param>
        /// <returns>Size of source file. -1 if failed.</returns>
        private long GetFileSize(string source, bool online)
        {
            long result = -1;

            try
            {
                if (online)
                {
                    FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(source);
                    ftpWebRequest.UseBinary = true;
                    ftpWebRequest.KeepAlive = true;
                    ftpWebRequest.UsePassive = true;
                    ftpWebRequest.Credentials = new NetworkCredential(Username, Password);
                    ftpWebRequest.Method = WebRequestMethods.Ftp.GetFileSize;

                    using (WebResponse resp = ftpWebRequest.GetResponse())
                        result = resp.ContentLength;
                }
                else
                {
                    FileInfo file = new FileInfo(source);
                    result = file.Length;
                }
            }
            catch (Exception ex)
            {
                // TODO: Exception here.
                Console.WriteLine(ex.ToString());
            }

            return result;
        }

        /// <summary>
        /// FTP Task background: Browse. Get all items info in FTP server folder.
        /// </summary>
        private void FTPTask_Browse()
        {
            try
            {
                // Set task is running
                Result = TaskResult.Running;

                InitializeFTPWebRequest(
                    WebRequestMethods.Ftp.ListDirectoryDetails,
                    GetFinalHost() + ServerSourcePath
                    );

                // Initialze a new List about Info Items.
                List<ItemInfo> itemList = new List<ItemInfo>();

                // Date and time
                string[] monthArr = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

                using (var response = ftpWebRequest.GetResponse())
                {
                    string[] itemArray = null;
                    using (var responseStream = response.GetResponseStream())
                    using (var reader = new StreamReader(responseStream))
                        itemArray = reader.ReadToEnd().Replace("\r", null).Split('\n');

                    foreach (string item in itemArray)
                    {
                        if (item == null) continue;
                        if (item.Length == 0) continue;

                        // Source: https://stackoverflow.com/questions/10079415/splitting-a-string-with-multiple-spaces
                        string[] temp = Regex.Split(item, @" +");

                        // Create new item
                        ItemInfo itemProp = new ItemInfo();

                        // Type of item (file/directory)
                        itemProp.Type = temp[0][0] == 'd' ? ItemType.Directory : ItemType.File;

                        // File/Directory permission
                        itemProp.Permission = temp[0].Substring(1);

                        // File size
                        if (itemProp.Type == ItemType.File)
                            itemProp.Size = Convert.ToInt64(temp[4]);

                        // File name
                        itemProp.Name = "";
                        for (int i = 8; i < temp.Length; i++)
                        {
                            if (i != 8)
                                itemProp.Name += " ";
                            itemProp.Name += temp[i];
                        }

                        // Date and time
                        int day = Convert.ToInt32(temp[6]);
                        int month = Array.FindIndex(monthArr, row => row.Contains(temp[5])) + 1;
                        if (temp[7].Contains(':'))
                        {
                            int hour = Convert.ToInt32(temp[7].Split(':')[0]);
                            int minute = Convert.ToInt32(temp[7].Split(':')[1]);
                            int year = DateTime.Now.Year;
                            itemProp.Date = new DateTime(year, month, day, hour, minute, 0);
                        }
                        else
                        {
                            int hour = 0;
                            int minute = 0;
                            int year = DateTime.Now.Year;
                            itemProp.Date = new DateTime(year, month, day, hour, minute, 0);
                        }

                        // Add this item to list
                        itemList.Add(itemProp);
                    }
                }

                // Return data and set task is successful.
                Data = itemList;
                Result = TaskResult.Successful;
            }
            catch (Exception ex)
            {
                // TODO: Exception here.
                Console.WriteLine(ex.ToString());

                // If any errors => task failed.
                if (Result != TaskResult.Cancelled)
                {
                    Result = TaskResult.Failed;
                    Message = ex.Message;
                }
            }

            // Call TaskCompleted event.
            if (TaskCompleted != null)
                TaskCompleted(this, EventArgs.Empty);
        }

        private void FTPTask_FileDownload()
        {
            try
            {
                // Set task is running
                Result = TaskResult.Running;

                // Get file size and set to size total.
                SizeTotal = GetFileSize(GetFinalHost() + ServerSourcePath, true);

                InitializeFTPWebRequest(
                    WebRequestMethods.Ftp.DownloadFile,
                    GetFinalHost() + ServerSourcePath
                    );

                using (var response = ftpWebRequest.GetResponse())
                {
                    using (Stream ftpStream = response.GetResponseStream())
                    using (FileStream fileStream = new FileStream(LocalPath, FileMode.Create))
                    {
                        byte[] buffer = new byte[BufferSize];
                        int read;

                        while ((read = ftpStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fileStream.Write(buffer, 0, read);

                            // 
                            SizeCompleted = fileStream.Position;
                            //
                            if (TaskProgressChanged != null)
                                TaskProgressChanged(this, EventArgs.Empty);
                        }
                    }
                }

                // Return data and set task is successful.
                Result = TaskResult.Successful;
            }
            catch (Exception ex)
            {
                // TODO: Exception here.
                Console.WriteLine(ex.ToString());

                // If any errors => task failed.
                if (Result != TaskResult.Cancelled)
                {
                    Result = TaskResult.Failed;
                    Message = ex.Message;
                }

            }

            // Call TaskCompleted event.
            if (TaskCompleted != null)
                TaskCompleted(this, EventArgs.Empty);
        }

        private void FTPTask_FileUpload()
        {
            try
            {
                // Set task is running
                Result = TaskResult.Running;

                // Get file size and set to size total.
                SizeTotal = GetFileSize(LocalPath, false);

                InitializeFTPWebRequest(
                    WebRequestMethods.Ftp.UploadFile,
                    GetFinalHost() + ServerSourcePath
                    );

                using (Stream fileStream = File.OpenRead(LocalPath))
                using (Stream ftpStream = ftpWebRequest.GetRequestStream())
                {
                    byte[] buffer = new byte[BufferSize];
                    int read;
                    while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ftpStream.Write(buffer, 0, read);

                        //
                        SizeCompleted = fileStream.Position;
                        // 
                        if (TaskProgressChanged != null)
                            TaskProgressChanged(this, EventArgs.Empty);
                    }
                }

                // Return data and set task is successful.
                Result = TaskResult.Successful;
            }
            catch (Exception ex)
            {
                // TODO: Exception here.
                Console.WriteLine(ex.ToString());

                // If any errors => task failed.
                if (Result != TaskResult.Cancelled)
                {
                    Result = TaskResult.Failed;
                    Message = ex.Message;
                }
            }

            // Call TaskCompleted event.
            if (TaskCompleted != null)
                TaskCompleted(this, EventArgs.Empty);
        }

        private void FTPTask_RenameOrMove()
        {
            try
            {
                // Set task is running
                Result = TaskResult.Running;

                InitializeFTPWebRequest(
                    WebRequestMethods.Ftp.Rename,
                    GetFinalHost() + ServerSourcePath,
                    ServerTargetPath
                    );

                // Execute command
                ftpWebRequest.GetResponse();

                // Return data and set task is successful.
                Result = TaskResult.Successful;
            }
            catch (Exception ex)
            {
                // TODO: Exception here.
                Console.WriteLine(ex.ToString());

                // If any errors => task failed.
                if (Result != TaskResult.Cancelled)
                {
                    Result = TaskResult.Failed;
                    Message = ex.Message;
                }
            }

            // Call TaskCompleted event.
            if (TaskCompleted != null)
                TaskCompleted(this, EventArgs.Empty);
        }

        private void FTPTask_FileDelete()
        {
            try
            {
                // Set task is running
                Result = TaskResult.Running;

                InitializeFTPWebRequest(
                    WebRequestMethods.Ftp.DeleteFile,
                    GetFinalHost() + ServerSourcePath
                    );

                // Execute command
                ftpWebRequest.GetResponse();

                // Return data and set task is successful.
                Result = TaskResult.Successful;
            }
            catch (Exception ex)
            {
                // TODO: Exception here.
                Console.WriteLine(ex.ToString());

                // If any errors => task failed.
                if (Result != TaskResult.Cancelled)
                {
                    Result = TaskResult.Failed;
                    Message = ex.Message;
                }
            }

            // Call TaskCompleted event.
            if (TaskCompleted != null)
                TaskCompleted(this, EventArgs.Empty);
        }

        private void FTPTask_DirectoryCreate()
        {
            try
            {
                // Set task is running
                Result = TaskResult.Running;

                InitializeFTPWebRequest(
                    WebRequestMethods.Ftp.MakeDirectory,
                    GetFinalHost() + ServerSourcePath
                    );

                // Execute command
                ftpWebRequest.GetResponse();

                // Return data and set task is successful.
                Result = TaskResult.Successful;
            }
            catch (Exception ex)
            {
                // TODO: Exception here.
                Console.WriteLine(ex.ToString());

                // If any errors => task failed.
                if (Result != TaskResult.Cancelled)
                {
                    Result = TaskResult.Failed;
                    Message = ex.Message;
                }

            }

            // Call TaskCompleted event.
            if (TaskCompleted != null)
                TaskCompleted(this, EventArgs.Empty);
        }

        private void FTPTask_DirectoryDelete()
        {
            try
            {
                // Set task is running
                Result = TaskResult.Running;

                InitializeFTPWebRequest(
                    WebRequestMethods.Ftp.RemoveDirectory,
                    GetFinalHost() + ServerSourcePath
                    );

                // Execute command
                ftpWebRequest.GetResponse();

                // Return data and set task is successful.
                Result = TaskResult.Successful;
            }
            catch (Exception ex)
            {
                // TODO: Exception here.
                Console.WriteLine(ex.ToString());

                // If any errors => task failed.
                if (Result != TaskResult.Cancelled)
                {
                    Result = TaskResult.Failed;
                    Message = ex.Message;
                }

            }

            // Call TaskCompleted event.
            if (TaskCompleted != null)
                TaskCompleted(this, EventArgs.Empty);
        }
        #endregion

    }
}
