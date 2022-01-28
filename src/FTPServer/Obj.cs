using System.Collections.Generic;

namespace FTPServer
{
    public class FTPUser
    {
        public string CurrentDirectory { get; set; } = null;
        public string Name { get; set; } = null;
        public string Password { get; set; } = null;
    }

    public class FTPConfig
    {
        public bool AutoStartAtOpen { get; set; } = false;
        public bool ConfigAutoSave { get; set; } = false;
        public int Port { get; set; } = 21;
        public List<FTPUser> Users { get; set; }
    }
}
