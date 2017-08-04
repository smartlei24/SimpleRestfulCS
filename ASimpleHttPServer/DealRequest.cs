using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using ASimpleHttPServer;

namespace ASimpleHttpServer
{
    public class DealRequest
    {
        //自定义状态枚举值
        public enum StautucodeString
        {
            OK = 200,
            PasswordWrong = 605,
            UserWrong = 701,
            AllWrong = 810
        }

        private Dictionary<string, string> headerDirectory;

        //构造函数
        public DealRequest(Socket client)
        {
            Handler = client;
            int receiveNum = 1;
            byte[] bytes = new byte[1024];
            string message = "";
            while (receiveNum != 0)
            {
                receiveNum = client.Receive(bytes);
                message += Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                Console.WriteLine("Recive a Http request.");
            }
            //将Request根据header和Content分割
            string httpHeader = message.Split(new[] {"\r\n\r\n"}, StringSplitOptions.RemoveEmptyEntries)[0];
            string httpContent = message.Split(new[] {"\r\n\r\n"}, StringSplitOptions.RemoveEmptyEntries)[1];

            AddMessageToHeaderDirctory(httpHeader);

            if (HeaderIsCorrect(httpContent))
            {
                string resouceUrl = GetHeadValueFormDirctory("requestUrl").Split(
                    new[] {'\\', '/'}, StringSplitOptions.RemoveEmptyEntries)[0];
                string resouceName = ResouceRoute.ResouceDictionary[resouceUrl];

                IResouce dealResouce = Activator.CreateInstance("ASimpleHttpServer", resouceName) as IResouce;

                if (dealResouce == null)
                {
                    Console.WriteLine("Have a Resouce is Null reference");
                    StatusCode = 600;
                    SendResponse("A Bad Resouce");
                    return;
                }

                string responContent = "";
                switch (GetHeadValueFormDirctory("method"))
                {
                    case "GET":
                        responContent = dealResouce.DealGet(
                            httpContent,
                            GetHeadValueFormDirctory("Token"),
                            GetHeadValueFormDirctory("url"));
                        break;
                    case "POST":
                        responContent = dealResouce.DealPost(
                            httpContent,
                            GetHeadValueFormDirctory("Token"),
                            GetHeadValueFormDirctory("url"));
                        break;
                    case "PUT":
                        responContent = dealResouce.DealPut(
                            httpContent,
                            GetHeadValueFormDirctory("Token"),
                            GetHeadValueFormDirctory("url"));
                        break;
                    case "DELECT":
                        responContent = dealResouce.DealDelect(
                            httpContent,
                            GetHeadValueFormDirctory("Token"),
                            GetHeadValueFormDirctory("url"));
                        break;
                }
                SendResponse(responContent);
            }
            else
            {
                SendResponse();
            }
        }


        public string Sever { get; set; }
        public int StatusCode { get; set; }
        public Socket Handler { get; set; }

        //判断头部是否争取 并设置返回的状态码
        private bool HeaderIsCorrect(string httpContent)
        {
            if (new[] {"GET", "POST", "PUT", "DELETE"}.Contains(GetHeadValueFormDirctory("method")))
            {
                StatusCode = 400;
                return false;
            }
            if (Convert.ToInt16(GetHeadValueFormDirctory("Content-Length")) != httpContent.Length)
            {
                StatusCode = 400;
                return false;
            }
            if (GetHeadValueFormDirctory("Accect").Equals("text/html,application/xhtml+xml,application/xml;"))
                StatusCode = 711;
            else if (string.IsNullOrWhiteSpace(GetHeadValueFormDirctory("requestUrl")))
                StatusCode = 400;
            StatusCode = 200;
            return true;
        }


        //构造消息头
        private string BuildHeader()
        {
            StringBuilder header = new StringBuilder("HTTP/1.1");
            header.Append(" " + StatusCode + " " + (StautucodeString) StatusCode + "\r\n");
            header.Append("Content-Type: text/html;charset=utf-8\r\n");
            header.Append("Connection: keep-alive\r\n");
            header.Append("Server:Lave_Sever\r\n");
            header.Append("Date:" + DateTime.Now.GetDateTimeFormats('r')[0] + "\r\n");
            header.Append("\r\n");
            return header.ToString();
        }

        //返回Response
        private void SendResponse(string Content = "")
        {
            string buffString = BuildHeader() + Content;
            Console.WriteLine("Send A Response.");
            byte[] bufferBytes = Encoding.UTF8.GetBytes(buffString);
            Handler.Send(bufferBytes);
            Console.WriteLine(Encoding.UTF8.GetString(bufferBytes));
            Handler.Close();
        }

        //解析HttpHeader
        private void AddMessageToHeaderDirctory(string Header)
        {
            string[] splitHeader = Header.Split(
                new[] {"\r\n"},
                StringSplitOptions.RemoveEmptyEntries);
            string[] firstLineStrings = splitHeader[0].Split(
                new[] {' '},
                StringSplitOptions.RemoveEmptyEntries);
            headerDirectory.Add("method", firstLineStrings[0]);
            headerDirectory.Add("requestUrl", firstLineStrings[1]);
            headerDirectory.Add("httpVersion", firstLineStrings[2]);
            for (int i = 1; i < splitHeader.Length; i++)
            {
                string key = splitHeader[i].Split(new[] {':', ' '}, StringSplitOptions.RemoveEmptyEntries)[0];
                string keyValue = splitHeader[i].Split(new[] {':', ' '}, StringSplitOptions.RemoveEmptyEntries)[1];
                headerDirectory.Add(key, keyValue);
            }
        }

        private string GetHeadValueFormDirctory(string key)
        {
            if (headerDirectory.ContainsKey(key))
                return headerDirectory[key];
            return null;
        }
    }
}