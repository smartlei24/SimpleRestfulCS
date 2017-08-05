using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework.Internal;

namespace ASimpleHttpServer
{

    internal class Server
    {
        private bool active;
        //初始化服务器IP和端口
        private Socket server;
        private string serverIP = "127.0.0.1";
        private int serverPort = 6565;

        public int ServerPort
        {
            get { return serverPort; }
            set { serverPort = value; }
        }

        //初始化
        public int Initialze()
        {
            ResouceRoute.InitializeResouceRoute();
            EmployeeRoute.LoadJsonFromFile();
            if (active)
                return 0;
            server = new Socket(SocketType.Stream, ProtocolType.Tcp);
            //开始监听
            server.Bind(new IPEndPoint(IPAddress.Parse(serverIP), serverPort));
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
                    Thread requestThread = new Thread(() => { ProcessRequst(client); }) {IsBackground = true};
                    requestThread.Start();
                }
            }) {IsBackground = true};
            activeSever.Start();
            //初始化成功返回1
            return 1;
        }

        private void ProcessRequst(Socket client)
        {
            //处理Request
            DealRequest request = new DealRequest(client);
        }

        ~Server()
        {
            if (EmployeeRoute.Sync())
            {
                Console.WriteLine("Successfully Sync");
            }
            else
            {
                Console.WriteLine("Faild Sync");
                Console.ReadKey();
            }
        }
    }
}