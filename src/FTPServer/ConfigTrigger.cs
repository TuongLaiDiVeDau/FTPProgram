using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace FTPServer
{
    public static class ConfigTrigger
    {
        #region User config path
        private static string configPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\config.json";

        public static string ConfigPath
        {
            get { return configPath; }
        }
        #endregion

        #region Config variable
        private static FTPConfig ftpConfig = null;

        public static FTPConfig FTPConfig
        {
            get { return ftpConfig; }
            set { ftpConfig = value; }
        }

        /// <summary>
        /// Fired if config file has been modified.
        /// </summary>
        public static event EventHandler ConfigModified;
        #endregion

        #region File changed trigger
        private static FileSystemWatcher fsw = null;

        /// <summary>
        /// Initialize trigger for file modify detection.
        /// </summary>
        public static void InitializeTrigger()
        {
            LoadConfig();

            if (fsw != null)
                StopTrigger();

            fsw = new FileSystemWatcher(Path.GetDirectoryName(configPath));
            fsw.NotifyFilter = NotifyFilters.LastWrite;
            fsw.Created += Fsw_Created;
            fsw.Changed += Fsw_Changed;
            fsw.Deleted += Fsw_Deleted;

            fsw.Filter = "config.json";
            fsw.IncludeSubdirectories = false;
            fsw.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Stop trigger from file modify detection.
        /// </summary>
        public static void StopTrigger()
        {
            try
            {
                fsw.Dispose();
                fsw = null;
            }
            catch (Exception)
            {

            }
        }

        private static void Fsw_Deleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("{0} - Config was modified. Reloading...", DateTime.Now.ToString("dd/MM/yyyy - HH:mm"));
            LoadConfig();
        }

        private static void Fsw_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            Console.WriteLine("{0} - Config was modified. Reloading...", DateTime.Now.ToString("dd/MM/yyyy - HH:mm"));
            LoadConfig();
        }

        private static void Fsw_Created(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("{0} - Config was modified. Reloading...", DateTime.Now.ToString("dd/MM/yyyy - HH:mm"));
            LoadConfig();
        }
        #endregion

        #region Function: Username and password detection
        public static FTPUser GetUser(string user)
        {
            if (FTPConfig == null)
                return null;

            if (FTPConfig.Users == null)
                return null;

            FTPUser user1 = null;
            try
            {
                user1 = FTPConfig.Users.Where(p => p.Name == user).First();
            }
            catch
            {
                user1 = null;
            }

            return user1;
        }

        public static bool CheckUserExist(string user)
        {
            if (FTPConfig == null)
                return false;

            if (FTPConfig.Users == null)
                return false;

            try
            {
                FTPUser match = FTPConfig.Users.Where(p => p.Name == user).First();
                if (match.Name == user)
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckCorrectPassword(string user, string pass)
        {
            if (FTPConfig == null)
                return false;

            if (FTPConfig.Users == null)
                return false;

            try
            {
                FTPUser match = FTPConfig.Users.Where(p => p.Name == user).First();
                if (match.Name == user)
                    if (match.Password == pass)
                        return true;

                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// Load config from file.
        /// </summary>
        private static void LoadConfig()
        {
            try
            {
                // Load config
                // If file is not exist, return nothing
                if (!File.Exists(configPath))
                {
                    FTPConfig.Users = null;
                    return;
                }

                // Load strings
                string strings = File.ReadAllText(configPath);

                // Convert to json
                // Load config
                ftpConfig = JsonSerializer.Deserialize<FTPConfig>(strings);

                // Fire an event
                if (ConfigModified != null)
                    ConfigModified(new object(), EventArgs.Empty);

                // Output: Loaded config
                Console.WriteLine("{0} - Config was successfully loaded.", DateTime.Now.ToString("dd/MM/yyyy - HH:mm"));
            }
            catch (Exception)
            {
                Console.WriteLine("{0} - Error while reloading config. Reloading...", DateTime.Now.ToString("dd/MM/yyyy - HH:mm"));
                LoadConfig();
            }
        }
    }
}
