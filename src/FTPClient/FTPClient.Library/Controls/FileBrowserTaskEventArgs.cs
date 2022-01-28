using System;
using System.Collections.Generic;
using System.Text;

namespace FTPClient.Library.Controls
{
    public class FileBrowserTaskEventArgs
    {
        public TaskType TaskType { get; set; } = TaskType.Unknown;

        public string CurrentDirectoryPath { get; set; } = null;

        public string SourcePath { get; set; } = null;

    }
}
