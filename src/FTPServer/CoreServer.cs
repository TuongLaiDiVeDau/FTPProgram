using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FTPServer
{
    public class CoreServer
    {
        private TcpListener tcp = null;
        private bool isRunning = false;

        public CoreServer(IPAddress ipAddress, int port = 21)
        {
            tcp = new TcpListener(ipAddress, port);
        }

        public void Start()
        {
            isRunning = true;
            tcp.Start();
            tcp.BeginAcceptTcpClient(BeginAcceptTcpClient, tcp);
        }

        public void Stop()
        {
            isRunning = false;
            tcp.Stop();
        }

        public void BeginAcceptTcpClient(IAsyncResult result)
        {
            if (isRunning)
            {
                TcpClient client = tcp.EndAcceptTcpClient(result);
                tcp.BeginAcceptTcpClient(BeginAcceptTcpClient, tcp);


                ClientSession cnn = new ClientSession(client);

                ThreadPool.QueueUserWorkItem(cnn.InvokeCommand, client);
            }
        }
    }
}
