using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class HttpCollector
    {
        private Socket requestSocket;
        private string requesturl;
        private string method;
        private string ServerIP { get; }
        private int ServerPort { get; }

        public string ResponseMessage { get; set; }

        public HttpCollector(string serverIP= "127.0.0.1", int serverPort=6565)
        {
            ServerIP = serverIP;
            ServerPort = serverPort;
            requestSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            requestSocket.Connect(IPAddress.Parse(serverIP), serverPort);
        }

        private string BuildRequestHead(string content)
        {
            StringBuilder headBuilder = new StringBuilder(method);
            headBuilder.Append(requesturl);
            headBuilder.Append(" HTTP/1.1\r\n");
            headBuilder.Append("Host: 127.0.0.1:6565 \r\n");
            headBuilder.Append("Connection: keep-alive \r\n");
            headBuilder.Append("Context-Length: \r\n");
            headBuilder.Append("Accept-Language: zh-CN,zh;q=0.8 \r\n");
            headBuilder.Append("content-Length: " + content.Length + "\r\n\r\n");
            headBuilder.Append(content);
            return headBuilder.ToString();
        }

        public void SendRequest(string content)
        {
            byte[] data = new byte[1024];
            data = Encoding.UTF8.GetBytes(BuildRequestHead(content));
            requestSocket.Send(data);
            byte[] responseBytes = new byte[1024];
            requestSocket.Receive(responseBytes);
            this.ResponseMessage = Encoding.UTF8.GetString(responseBytes);
        }
    }
}
