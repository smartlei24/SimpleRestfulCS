using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASimpleHttpServer
{
    interface IResouce
    {
        string DealGet(string content, string token, string url);
        string DealPost(string content, string token, string url);
        string DealPut(string content, string token, string url);
        string DealDelete(string content, string token, string url);
    }
}