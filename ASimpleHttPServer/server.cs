using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SimpleHttpServer
{
    internal class Server
    {
        private bool active = false;

        //初始化服务器IP和端口
        private Socket server;
        private string serverIP = "127.0.0.1";
        public int ServerPort { get; } = 6565;

        //初始化
        public int Initialze()
        {
            if (active)
                return 0;
            server = new Socket(SocketType.Stream,ProtocolType.Tcp);
            //开始监听
            server.Bind(new IPEndPoint(IPAddress.Parse(serverIP), ServerPort));
            server.Listen(10);
            active = true;
            
            Thread activeSever = new Thread(() =>
            {
                while (active)
                {
                    Socket client = server.Accept();
                    //有新Request时在Console输出其IP
                    Console.WriteLine("A Client Connected in " +
                                      IPAddress.Parse(((IPEndPoint) client.RemoteEndPoint).Address.ToString()));
                    Thread requestThread = new Thread(() => { ProcessRequst(client); }) { IsBackground = true };
                    requestThread.Start();
                }
            }){IsBackground = true};
            activeSever.Start();
            //初始化成功返回1
            return 1;
        }

        private void ProcessRequst(Socket client)
        {
            
            //处理Request
            DealRequest request = new DealRequest(client);
            if (request.SendResponse() == 1)
            {
                client.Close();
            }
        }

    }


}