using System.Net;

namespace FTPServer
{
    public static class FTPServerController
    {
        private static CoreServer ftp = null;

        public static bool IsRunning
        {
            get { return ftp != null; }
        }

        public static void Start()
        {
            if (ftp != null)
            {
                Stop();
            }

            ftp = new CoreServer(IPAddress.Any, ConfigTrigger.FTPConfig.Port);
            ftp.Start();
        }

        public static void Stop()
        {
            if (ftp != null)
            {
                ftp.Stop();
                ftp = null;
            }
        }
    }
}
