using System;
using System.Collections.Generic;
using System.Text;

namespace FTPClient.Library
{
    /// <summary>
    /// Task result
    /// </summary>
    public enum TaskResult
    {
        /// <summary>
        /// Task cancelled
        /// </summary>
        Cancelled = -2,
        /// <summary>
        /// Unknown result
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// Client is running...
        /// </summary>
        Running = 0,
        /// <summary>
        /// Task successful
        /// </summary>
        Successful = 1,
        /// <summary>
        /// Task failed
        /// </summary>
        Failed = 2,
    }

    /// <summary>
    /// Task type
    /// </summary>
    public enum TaskType
    {
        /// <summary>
        /// Unknown task
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// Get items info
        /// </summary>
        Browse = 0,
        /// <summary>
        /// Task: Download a file
        /// </summary>
        FileDownload = 1,
        /// <summary>
        /// Task: Upload file
        /// </summary>
        FileUpload = 2,
        /// <summary>
        /// Task: Rename file or folder
        /// </summary>
        Rename = 3,
        /// <summary>
        /// Task: Move file or folder
        /// </summary>
        Move = 4,
        /// <summary>
        /// Task: Delete file
        /// </summary>
        FileDelete = 5,
        /// <summary>
        /// Task: Create directory
        /// </summary>
        DirectoryCreate = 6,
        /// <summary>
        /// Task: Delete directory
        /// </summary>
        DirectoryDelete = 7,
        /// <summary>
        /// Show log result
        /// </summary>
        ShowLog = 8,
    }

    /// <summary>
    /// Item type
    /// </summary>
    public enum ItemType
    {
        /// <summary>
        /// Unknown items
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// This item is a file
        /// </summary>
        File = 0,
        /// <summary>
        /// This item is a directory
        /// </summary>
        Directory = 1,
    }

}
