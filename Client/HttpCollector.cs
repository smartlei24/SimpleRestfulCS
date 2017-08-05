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
        private string requestUrl;
        private string method;
        private string ServerIP { get; }
        private int ServerPort { get; }
        public string Content { get; set; }
        public string ResponseMessage { get; set; }
        private Dictionary<string, string> headerDirectory = new Dictionary<string, string>();

        public HttpCollector(string method,string requestUrl,string serverIP= "127.0.0.1", int serverPort=6565)
        {
            this.method = method;
            this.requestUrl = requestUrl;
            ServerIP = serverIP;
            ServerPort = serverPort;
            requestSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            requestSocket.Connect(IPAddress.Parse(serverIP), serverPort);
        }

        private string BuildRequestHead(string content)
        {
            StringBuilder headBuilder = new StringBuilder(method +" ");
            headBuilder.Append(requestUrl);
            headBuilder.Append(" HTTP/1.1\r\n");
            headBuilder.Append("Host: 127.0.0.1:6565 \r\n");
            headBuilder.Append("Connection: keep-alive \r\n");
            headBuilder.Append("Accept-Language: zh-CN,zh;q=0.8 \r\n");
            headBuilder.Append("Content-Length: " + content.Length + "\r\n\r\n");
            headBuilder.Append(content);
            return headBuilder.ToString();
        }

        public void SendRequest(string content)
        {
            byte[] data = new byte[1024];
            data = Encoding.UTF8.GetBytes(BuildRequestHead(content));
            requestSocket.Send(data);
            byte[] responseBytes = new byte[1024];

            int receiveNum = 0;
            byte[] bytes = new byte[1023];
            int receviceCount = 0;

            while (true)
            {
                string headerText;
                string bodyText;
                receiveNum = requestSocket.Receive(bytes,bytes.Length,0);
                string message = Encoding.UTF8.GetString(bytes, 0, receiveNum);
                string[] httpSplit = message.Split(new[] {"\r\n\r\n"}, StringSplitOptions.RemoveEmptyEntries);
                headerText = httpSplit[0];
                bodyText = httpSplit.Length == 1 ? "" : httpSplit[1];

                if (receviceCount ==0)
                {
                    AddMessageToHeaderDirctory(headerText);
                }
                Content += bodyText;
                if (Convert.ToInt16(GetHeadValueFormDirctory("Content-Length")) == Content.Length)
                {
                    break;
                }
            }
        }

        //解析HttpHeader
        private void AddMessageToHeaderDirctory(string Header)
        {
            string[] splitHeader = Header.Split(
                new[] { "\r\n" },
                StringSplitOptions.RemoveEmptyEntries);
            string[] firstLineStrings = splitHeader[0].Split(
                new[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries);
            headerDirectory.Add("httpVersion", firstLineStrings[0]);
            headerDirectory.Add("StatusCode",firstLineStrings[1]);
            for (int i = 1; i < splitHeader.Length; i++)
            {
                string key = splitHeader[i].Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
                string keyValue = splitHeader[i].Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries)[1].ToString();
                headerDirectory.Add(key, keyValue);
            }
        }

        private string GetHeadValueFormDirctory(string key)
        {
            if (headerDirectory.ContainsKey(key))
                return headerDirectory[key];
            return "";
        }
    }
}
