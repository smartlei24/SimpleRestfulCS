using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace ASimpleHttPServer
{
    internal class EmployeesResouce :IResouce
    {
        private static JArray employeesArray;

        public EmployeesResouce()
        {
            var jsonFileContent = "";
            using (var reader =
                new StreamReader(
                    @"C:\Users\ll7i\Documents\GitHub\restfulCS\SimpleRestfulCS\ASimpleHttPServer\bin\Debug\employees.json")
            )
            {
                string line;
                while ((line = reader.ReadLine())!=null)
                    jsonFileContent += line;
            }
            employeesArray = JArray.Parse(jsonFileContent);
        }

        public string DealGet(string content, string token, string url)
        {
            var response = "";
            if (url.Split(new []{'/','\\'},StringSplitOptions.RemoveEmptyEntries).Length==1)
                return response;
            //列出单个员工的信息
            return response;
        }

        public string DealPost(string content, string token, string url)
        {
            var response = "";
            
            var newEmployee = JsonConvert.DeserializeObject(content) as Employee;
            return response;
        }

        public string DealPut(string content, string token, string url)
        {
            var response = "";
            return response;

        }

        public string DealDelect(string content, string token, string url)
        {
            var response = "";
            return response;
        }
    }
}
