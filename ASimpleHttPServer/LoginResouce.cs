using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASimpleHttPServer
{
    class LoginResouce:IResouce
    {
        public string DealGet(string content, string token, string url)
        {
            return "Nothing to Get";
        }

        public string DealPost(string content, string token, string url)
        {
            string user = content.Split(new[] {'&'}, StringSplitOptions.RemoveEmptyEntries)[0];
            string passwd = content.Split(new[] {'&'}, StringSplitOptions.RemoveEmptyEntries)[1];
            if (user == "Lave_Lei" && passwd == "123456")
            {
                return "登陆成功\r\nToken:20170804";
            }
            if(user == "Lave_lei")
            {
                return "密码错误";
            }
            if(passwd == "123456")
            {
                return "用户名错误";
            }
            return "全错了";
        }

        public string DealPut(string content, string token, string url)
        {
            return "Can't to Update anything";
        }

        public string DealDelect(string content, string token, string url)
        {
            return "Can't to Delect anything";
        }
    }
}
