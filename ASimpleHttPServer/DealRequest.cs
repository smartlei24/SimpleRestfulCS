using System;
using System.Net.Sockets;
using System.Text;

namespace SimpleHttpServer
{
    public class DealRequest
    {
        private string content="";

        byte[] bytes = new byte[1024];
        public string Sever { get; set; } = "Lave_server";
        public int StatusCode { get; set; }
        public Socket Handler { get; set; }

        //自定义状态枚举值
        public enum StautucodeString
        {
            OK = 200,
            PasswordWrong = 605,
            UserWrong=701,
            AllWrong=810
        }

        //构造函数
        public DealRequest(Socket client)
        {
            Handler = client;
            client.Receive(bytes);
            content += Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            Console.WriteLine("Recive a Http request.");

            //获取参数并判断
            string user = content.Split(new char[] {' ', '&'}, StringSplitOptions.RemoveEmptyEntries)[1];
            string password = content.Split(new char[] {' ', '&'}, StringSplitOptions.RemoveEmptyEntries)[2];
            if (user=="Lave_lei"&&password=="123456")
            {
                StatusCode = 200;
            }
            else if (password == "123456")
            {
                StatusCode = 701;
            }
            else if(user == "Lave_lei")
            {
                StatusCode = 605;
            }
            else
            {
                StatusCode = 810;
            }
        }

        //构造消息头
        private string BuildHeader()
        {
            StringBuilder header = new StringBuilder("HTTP/1.1");
            header.Append(" " + StatusCode + " " + ((StautucodeString)StatusCode) + "\r\n");
            header.Append("Content-Type: text/html;\r\ncharset=utf-8\r\n");
            header.Append("Connection: keep-alive\r\n");
            header.Append("Server:Lave_Sever\r\n");
            header.Append("Date:" + DateTime.Now.GetDateTimeFormats('r')[0] + "\r\n");
            header.Append("\r\n");
            return header.ToString();
        }

        //返回Response
        public bool SendResponse(string Content = "")
        {
            string buffString = BuildHeader() + Content;
            Console.WriteLine("Send A Response.");
            byte[] bufferBytes = Encoding.UTF8.GetBytes(buffString);
            Handler.Send(bufferBytes);
            Console.WriteLine(Encoding.UTF8.GetString(bufferBytes));
            Handler.Close();
            return true;
        }
    }
}