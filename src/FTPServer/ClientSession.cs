using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FTPServer
{
    public class ClientSession
    {
        // Main
        private TcpClient mainClient = null;
        private StreamReader mainReader = null;
        private StreamWriter mainWriter = null;

        // Data
        private DataConnectionState dataConnectionState = DataConnectionState.Unknown;
        private IPEndPoint dataEndPoint = null;

        // Passive
        private TcpClient dataClient = null;
        private TcpListener passiveTcpListener = null;

        // Option variables
        private int bufferSize = 8192;

        #region Temporary variable: Rename
        private string renameFrom = null;
        private string renameTo = null;

        private void ClearRenameField()
        {
            renameFrom = null;
            renameTo = null;
        }
        #endregion

        // User list
        // private List<FTPUser> user = null;
        private FTPUser currentUser = null;

        private string currentDirectoryPath = null;
        private string mainTransferType = null;

        public ClientSession(TcpClient client)
        {
            // Initialize TcpClient
            mainClient = client;
            mainReader = new StreamReader(client.GetStream());
            mainWriter = new StreamWriter(client.GetStream());
        }

        public void InvokeCommand(object obj)
        {
            mainWriter.WriteLine("220 Service Ready.");
            mainWriter.Flush();

            try
            {
                string input = null;
                while (!string.IsNullOrEmpty(input = mainReader.ReadLine()))
                {
                    // Ket qua tra ve cho client
                    string output = null;

                    // Chia cac lenh o dau cach dau tien
                    // VD: LIST /d => header la LIST, arguments la /d
                    string header = input.Split(' ', 2)[0].ToUpper();
                    string args = null;
                    try
                    {
                        args = input.Split(' ', 2)[1];
                    }
                    catch { }
                    // Neu chi co khoang trang hoac ko co gi => args null
                    if (String.IsNullOrWhiteSpace(args))
                        args = null;

                    if (output == null)
                    {
                        switch (header)
                        {
                            // Header thoat ket noi
                            case "QUIT":
                                output = "221 Closing connection. Goodbye!";
                                break;
                            // TODO: Header khoi tao phien moi (Dang thu nghiem - co the se bi xoa trong tuong lai)
                            case "REIN":
                                output = FuncReinitialize();
                                break;
                            // Header dang nhap voi user
                            case "USER":
                                output = CheckUser(args);
                                break;
                            // Header khai bao pass cua user
                            case "PASS":
                                output = CheckPassword(args);
                                break;
                            // Header xem thu muc hien tai
                            case "PWD":
                                output = String.Format("257 \"{0}\" is current directory.", currentDirectoryPath);
                                break;
                            // Header set kiểu truyền tập tin (binary, ascii, ...)
                            case "TYPE":
                                output = SetType(args);
                                break;
                            case "PASV":
                                output = SetPassive();
                                break;
                            case "PORT":
                                output = SetPort(args);
                                break;
                            // Header xem danh sach tap tin
                            case "LIST":
                                output = FuncList(args);
                                break;
                            // Header thay doi thu muc hien tai
                            case "CWD":
                                output = FuncChangeWorkingDirectory(args);
                                break;
                            // Header doi ten tap tin/thu muc
                            case "RNFR":
                                if (currentUser == null)
                                    output = "530 Not logged in";
                                else
                                {
                                    renameFrom = args;
                                    if (renameFrom == null)
                                    {
                                        output = "450 Your parameters is empty for rename from.";
                                        ClearRenameField();
                                    }
                                    output = "350 Requested file action pending further information.";
                                }
                                break;
                            case "RNTO":
                                if (currentUser == null)
                                    output = "530 Not logged in";
                                else
                                {
                                    renameTo = args;
                                    if (renameTo == null)
                                    {
                                        output = "450 Your parameters is empty for rename to. Cancelled action.";
                                        ClearRenameField();
                                    }
                                    output = FuncRename(renameFrom, renameTo);
                                }
                                break;
                            // Header tao thu muc
                            case "MKD":
                                output = FuncCreateDirectory(args);
                                break;
                            // Header xoa thu muc
                            case "RMD":
                                output = FuncRemoveDirectory(args);
                                break;
                            // Header xoa tap tin
                            case "DELE":
                                output = FuncDeleteFile(args);
                                break;
                            // Header lay kich co tap tin
                            case "SIZE":
                                output = FuncFileSize(args);
                                break;
                            // Header tai xuong file.
                            case "RETR":
                                output = FuncDownloadFile(args);
                                break;
                            // Header tai len file
                            case "STOR":
                                output = FuncUploadFile(args);
                                break;
                            // Khong co 1 trong cac header tren => khong ro lenh
                            default:
                                output = "502 Command not implemented";
                                break;
                        }
                    }

                    if (mainClient != null && mainClient.Connected)
                    {
                        mainWriter.WriteLine(output);
                        mainWriter.Flush();

                        // If found return with code 221, termiate TcpClient/FTP.
                        if (output.StartsWith("221"))
                            break;
                    }
                    else break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #region Login area
        private string CheckUser(string args)
        {
            // If already logged in, return error.
            if (currentUser != null)
                return "504 Command not implemented for that parameter";

            // If anonymous login, continue.
            if (args.ToLower() == "anonymous")
            {
                currentUser = new FTPUser();
                currentUser.Name = "anonymous";
                currentUser.CurrentDirectory = @"D:\FileZilla Server";
                return "331 Anonymous login, password required.";
            }

            // If user exist, require password.
            if (ConfigTrigger.CheckUserExist(args))
            {
                currentUser = ConfigTrigger.GetUser(args);
                return "331 Username found, password required.";
            }
            // Otherwise, return not logged in.
            else return "530 User not found, not logged in.";
        }

        private string CheckPassword(string args)
        {
            if (args == null)
                return "530 Password must not be empty, not logged in.";

            // If anonymous is accepted, continued
            if (currentUser.Name == "anonymous")
            {
                if (false)
                {
                    // TODO: Anonymous login.
                    // return "230 Logged in.";
                }
                else
                {
                    return "530 Anonymous cannot be logged in, not logged in.";
                }
            }

            // If correct password, return logged in.
            if (ConfigTrigger.CheckCorrectPassword(currentUser.Name, args))
            {
                currentDirectoryPath = "/";
                return "230 User logged in.";
            }
            // Otherwise, delete current user.
            else
            {
                currentUser = null;
                return "530 Incorrect password, not logged in.";
            }
        }

        private string FuncReinitialize()
        {
            currentDirectoryPath = "/";
            currentUser = null;
            passiveTcpListener = null;
            dataClient = null;

            return "220 Service ready for new user";
        }
        #endregion

        private string SetType(string args)
        {
            string response = "500 Unknown error";

            switch (args)
            {
                case "A":
                case "I":
                    mainTransferType = args;
                    response = "200 OK";
                    break;
                case "E":
                case "L":
                default:
                    response = "504 Command not implemented for that parameter.";
                    break;
            }

            return response;
        }

        private string SetPort(string args)
        {
            dataConnectionState = DataConnectionState.Active;

            string[] argsList = args.Split(',');

            byte[] ip = new byte[4];
            byte[] port = new byte[2];

            for (int i = 0; i < 6; i++)
            {
                // 1 -> 4
                if (i < 4)
                {
                    ip[i] = Convert.ToByte(argsList[i]);
                }
                // 5 -> 6
                else
                {
                    port[i - 4] = Convert.ToByte(argsList[i]);
                }
            }

            if (BitConverter.IsLittleEndian)
                Array.Reverse(port);

            dataEndPoint = new IPEndPoint(new IPAddress(ip), BitConverter.ToInt16(port, 0));

            return "200 Data connection established";
        }

        private string SetPassive()
        {
            dataConnectionState = DataConnectionState.Passive;
            IPAddress localIp = ((IPEndPoint)mainClient.Client.LocalEndPoint).Address;

            passiveTcpListener = new TcpListener(localIp, 0);
            passiveTcpListener.Start();

            IPEndPoint passiveListenerEndpoint = (IPEndPoint)passiveTcpListener.LocalEndpoint;

            byte[] address = passiveListenerEndpoint.Address.GetAddressBytes();
            short port = (short)passiveListenerEndpoint.Port;

            byte[] portArray = BitConverter.GetBytes(port);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(portArray);

            return string.Format("227 Entering Passive Mode ({0},{1},{2},{3},{4},{5})", address[0], address[1], address[2], address[3], portArray[0], portArray[1]);
        }

        #region Public Function for invoke command
        private string FuncList(string args)
        {
            if (currentUser == null)
                return "530 Not logged in";

            string finalPath = null;

            if (args != null)
            {
                // TODO: This can be done later.
                args = args.Replace("-a", null);

                // Processing path
                args = args.Replace("/", "\\");
                finalPath =
                    currentUser.CurrentDirectory +
                    (args.StartsWith("\\") ? null : currentDirectoryPath) +
                    args;
            }
            else finalPath = currentUser.CurrentDirectory + currentDirectoryPath;

            if (finalPath != null)
            {
                var task = new DataConnectionTask();
                task.Type = DataConnectionTaskType.List;
                task.Arguments = finalPath;

                OperationInitialize(task);

                return string.Format("150 Opening {0} mode data transfer for LIST", dataConnectionState);
            }

            return "450 Requested file action not taken";
        }

        private string FuncChangeWorkingDirectory(string args)
        {
            if (currentUser == null)
                return "530 Not logged in";

            string result;
            try
            {
                if (args == null)
                    throw new ArgumentNullException("Empty path. We can't understand new path.");

                string newPath = args;

                // Check if navigate from current directory path.
                // If true, convert it to a specific path.
                if (!newPath.StartsWith('/'))
                    newPath = currentDirectoryPath + '/' + newPath;

                // Convert to Windows Path Syntax
                string newWinPath = newPath.Replace('/', '\\');

                // Check if a vaild path. If true, set it to current directory path.
                if (Directory.Exists(currentUser.CurrentDirectory + newWinPath))
                {
                    currentDirectoryPath = newPath;
                    result = String.Format("250 Changed to \'{0}\'", newPath);
                }
                else
                {
                    result = String.Format("450 Hmm, check your path");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = String.Format("500 Hmm, we has encountered an error...");
            }

            PrintLogToConsole(String.Format("ChangeWorkingDirectory - \"{0}\" - Code {1}", args, result.Split(" ", 2)[0]));
            return result;
        }

        private string FuncRename(string from, string to)
        {
            string result;
            try
            {
                string pathFrom = currentUser.CurrentDirectory + currentDirectoryPath + '\\' + from;
                string pathTo = currentUser.CurrentDirectory + currentDirectoryPath + '\\' + to;
                if (File.Exists(pathFrom))
                {
                    if (!File.Exists(pathTo))
                        File.Move(pathFrom, pathTo);
                }
                else if (Directory.Exists(pathFrom))
                {
                    if (!Directory.Exists(pathTo))
                        Directory.Move(pathFrom, pathTo);
                }
                else throw new ArgumentException("Hmm, your entered file or folder name is incorrect (old file not exist, new name is exist).");


                result = String.Format("250 Changed name from \'{0}\' to \'{1}\'", from, to);
            }
            catch (ArgumentException exAE)
            {
                result = String.Format("450 {0}", exAE.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = String.Format("500 Hmm, we has encountered an error...");
            }

            PrintLogToConsole(String.Format("RenameDirectory - \"{0}\" -> \"{1}\" - Code {2}", from, to, result.Split(" ", 2)[0]));
            return result;
        }

        private string FuncCreateDirectory(string args)
        {
            if (currentUser == null)
                return "530 Not logged in";

            string result;
            try
            {
                string newPath = currentUser.CurrentDirectory + currentDirectoryPath + '/' + args;
                newPath = newPath.Replace('/', '\\');
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                    result = "250 Created directory.";
                }
                else result = String.Format("450 Hmm, directory has exist.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = String.Format("500 Hmm, we has encountered an error: {0}", ex.Message);
            }

            PrintLogToConsole(String.Format("CreateDirectory - \"{0}\" - Code {1}", args, result.Split(" ", 2)[0]));
            return result;
        }

        private string FuncRemoveDirectory(string args)
        {
            if (currentUser == null)
                return "530 Not logged in";

            string result;
            try
            {
                string delPath = currentUser.CurrentDirectory + currentDirectoryPath + '/' + args;
                delPath = delPath.Replace('/', '\\');
                if (Directory.Exists(delPath))
                {
                    Directory.Delete(delPath, true);
                    result = "250 Deleted directory.";
                }
                else result = String.Format("450 Hmm, directory does not exist.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = String.Format("500 Hmm, we has encountered an error: {0}", ex.Message);
            }

            PrintLogToConsole(String.Format("RemoveDirectory -  \"{0}\" - Code {1}", args, result.Split(" ", 2)[0]));
            return result;
        }

        private string FuncDeleteFile(string args)
        {
            if (currentUser == null)
                return "530 Not logged in";

            string result;
            try
            {
                string delPath = currentUser.CurrentDirectory + currentDirectoryPath + '/' + args;
                delPath = delPath.Replace('/', '\\');
                if (File.Exists(delPath))
                {
                    File.Delete(delPath);
                    result = "250 Deleted file.";
                }
                else result = String.Format("450 Hmm, files does not exist.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = String.Format("500 Hmm, we has encountered an error: {0}", ex.Message);
            }

            PrintLogToConsole(String.Format("DeleteFile -  \"{0}\" - Code {1}", args, result.Split(" ", 2)[0]));
            return result;
        }

        private string FuncFileSize(string args)
        {
            if (currentUser == null)
                return "530 Not logged in";

            args = args.Replace("/", "\\");
            string filePath = String.Format(
                "{0}{1}",
                currentUser.CurrentDirectory,
                args.StartsWith("\\") ? args : currentDirectoryPath + '/' + args
                );

            string result = null;

            if (File.Exists(filePath))
            {
                result = new FileInfo(filePath).Length.ToString();
            }

            result = result == null
                ? "550 Cannot get file size. File not found."
                : String.Format("213 {0}", result);

            PrintLogToConsole(String.Format("GetFileSize -  \"{0}\" - Code {1}", args, result.Split(" ", 2)[0]));
            return result;
        }

        private string FuncDownloadFile(string args)
        {
            if (currentUser == null)
                return "530 Not logged in";

            args = args.Replace("/", "\\");
            string finalPath = String.Format(
                "{0}{1}",
                currentUser.CurrentDirectory,
                args.StartsWith("\\") ? args : currentDirectoryPath + '/' + args
                );

            if (File.Exists(finalPath))
            {
                var task = new DataConnectionTask();
                task.Type = DataConnectionTaskType.Download;
                task.Arguments = finalPath;

                OperationInitialize(task);

                return string.Format("150 Opening {0} mode data transfer for LIST", dataConnectionState);
            }

            return "550 Cannot retrieve file. File Not Found";
        }

        private string FuncUploadFile(string args)
        {
            if (currentUser == null)
                return "530 Not logged in";

            args = args.Replace("/", "\\");
            string finalPath = String.Format(
                "{0}{1}",
                currentUser.CurrentDirectory,
                args.StartsWith("\\") ? args : currentDirectoryPath + '/' + args
                );

            if (Directory.Exists(Path.GetDirectoryName(finalPath)))
            {
                var task = new DataConnectionTask();
                task.Type = DataConnectionTaskType.Upload;
                task.Arguments = finalPath;

                OperationInitialize(task);

                return string.Format("150 Opening {0} mode data transfer for LIST", dataConnectionState);
            }

            return "550 Cannot retrieve file. File Not Found";
        }
        #endregion

        #region Background Task Core
        private void OperationInitialize(DataConnectionTask task)
        {
            if (dataConnectionState == DataConnectionState.Active)
            {
                dataClient = new TcpClient(dataEndPoint.AddressFamily);
                dataClient.BeginConnect(dataEndPoint.Address, dataEndPoint.Port, OperationDoWork, task);
            }
            else
            {
                passiveTcpListener.BeginAcceptTcpClient(OperationDoWork, task);
            }
        }

        private void OperationDoWork(IAsyncResult iasync)
        {
            if (dataConnectionState == DataConnectionState.Active)
            {
                dataClient.EndConnect(iasync);
            }
            else
            {
                dataClient = passiveTcpListener.EndAcceptTcpClient(iasync);
            }

            DataConnectionTask task = iasync.AsyncState as DataConnectionTask;

            string output = null;

            using (NetworkStream ns = dataClient.GetStream())
            {
                switch (task.Type)
                {
                    default:
                        break;
                    case DataConnectionTaskType.List:
                        output = FuncBackgroundList(ns, task.Arguments);
                        break;
                    case DataConnectionTaskType.Download:
                        output = FuncBackgroundDownloadFile(ns, task.Arguments);
                        break;
                    case DataConnectionTaskType.Upload:
                        output = FuncBackgroundUploadFile(ns, task.Arguments);
                        break;
                }
            }

            dataClient.Close();
            dataClient = null;

            mainWriter.WriteLine(output);
            mainWriter.Flush();
        }
        #endregion

        #region Background Task Func
        private string FuncBackgroundList(NetworkStream ns, string args)
        {
            string result;
            try
            {
                StreamWriter dataWriter = new StreamWriter(ns, Encoding.ASCII);
                // string lineSample = "{0}rw-r--r--  1 {1,5}     {2,10}     {3,8} {4,3} {5}";

                // List folder first
                IEnumerable<string> directories = Directory.EnumerateDirectories(args);
                foreach (string dir in directories)
                {
                    DirectoryInfo d = new DirectoryInfo(dir);

                    string date = d.LastWriteTime < DateTime.Now - TimeSpan.FromDays(180) ?
                        d.LastWriteTime.ToString("MMM dd  yyyy") :
                        d.LastWriteTime.ToString("MMM dd HH:mm");

                    string line = string.Format("drwxr-xr-x    1    -        -     {0,8} {1} {2}", "4096", date, d.Name);

                    dataWriter.WriteLine(line);
                    dataWriter.Flush();
                }

                // then, list files.
                IEnumerable<string> files = Directory.EnumerateFiles(args);
                foreach (string file in files)
                {
                    FileInfo f = new FileInfo(file);

                    string date = f.LastWriteTime < DateTime.Now - TimeSpan.FromDays(180) ?
                        f.LastWriteTime.ToString("MMM dd  yyyy") :
                        f.LastWriteTime.ToString("MMM dd HH:mm");

                    string line = string.Format("-rw-r--r--  1 {0,5}     {1,10}     {2,8} {3} {4}", "-", "-", f.Length, date, f.Name);

                    dataWriter.WriteLine(line);
                    dataWriter.Flush();
                }

                result = "226 Transfer complete";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = "450 Action error";
            }

            PrintLogToConsole(String.Format("Browse - \"{0}\" - Code {1}", args, result.Split(" ", 2)[0]));
            return result;
        }

        private string FuncBackgroundDownloadFile(NetworkStream ns, string args)
        {
            string result;
            try
            {
                long bytes = 0;

                using (var fs = new FileStream(args, FileMode.Open, FileAccess.Read))
                {
                    // Download using binary
                    if (mainTransferType == "I")
                    {
                        byte[] buffer = new byte[bufferSize];
                        int count = 0;

                        while ((count = fs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ns.Write(buffer, 0, count);
                            bytes += count;
                        }
                    }
                }

                result = "226 Successfully retrieved";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = "450 Unknown error while retrieving file.";
            }

            PrintLogToConsole(String.Format("Retrie - \"{0}\" - Code {1}", args, result.Split(" ", 2)[0]));
            return result;
        }

        private string FuncBackgroundUploadFile(NetworkStream ns, string args)
        {
            string result;
            try
            {
                long bytes = 0;

                using (var fs = new FileStream(args, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 4096, FileOptions.SequentialScan))
                {
                    // Upload using binary
                    if (mainTransferType == "I")
                    {
                        byte[] buffer = new byte[bufferSize];
                        int count = 0;

                        while ((count = ns.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fs.Write(buffer, 0, count);
                            bytes += count;
                        }
                    }
                }

                result = "226 Successfully stored";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = "450 Unknown error while retrieving file.";
            }

            PrintLogToConsole(String.Format("Store - \"{0}\" - Code {1}", args, result.Split(" ", 2)[0]));
            return result;
        }
        #endregion

        private void PrintLogToConsole(string msg)
        {
            var ip = (IPEndPoint)mainClient.Client.LocalEndPoint;
            string ipStr = String.Format("{0}:{1}", ip.Address, ip.Port);
            Console.WriteLine("{0} - {1} - {2}", DateTime.Now.ToString("dd/MM/yyyy - HH:mm"), ipStr, msg);
        }

    }
}