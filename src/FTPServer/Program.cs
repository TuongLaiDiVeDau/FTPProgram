using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace FTPServer
{
    internal class Program
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [STAThread]
        static void Main(string[] args)
        {
            bool createdNew;
            using (Mutex mutex = new Mutex(true, "FTPServer", out createdNew))
            {
                if (createdNew)
                {
                    Application.SetHighDpiMode(HighDpiMode.SystemAware);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    // Source: http://csharphelper.com/blog/2018/08/redirect-console-window-output-to-a-textbox-in-c/
                    LogForm logForm = new LogForm();
                    ControlWriter cw = new ControlWriter(logForm.RichTextBoxObj);
                    Console.SetOut(cw);

                    // Start application
                    Application.Run(new MainForm(logForm));
                }
                else
                {
                    Process current = Process.GetCurrentProcess();
                    foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                    {
                        if (process.Id != current.Id)
                        {
                            SetForegroundWindow(process.MainWindowHandle);
                            break;
                        }
                    }
                }
            }

        }
    }
}
