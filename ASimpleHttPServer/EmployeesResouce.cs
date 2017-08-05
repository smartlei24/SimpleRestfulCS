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

namespace ASimpleHttpServer
{
    public class EmployeesResouce :IResouce
    {
        

        public string DealGet(string content, string token, string url)
        {
            var response = "";
            if (url.Split(new[] {'/', '\\'}, StringSplitOptions.RemoveEmptyEntries).Length == 1)
            {
                response = EmployeeRoute.employeesArray.ToString();
                return response;
            }

            string id = url.Split(new[] {'/', '\\'}, StringSplitOptions.RemoveEmptyEntries)[1];
            foreach (var employee in EmployeeRoute.employeesArray)
            {
                if (employee["Id"].ToString() == id)
                {
                    response = string.Join("", 
                        employee.ToString().Split(new []{"\r\n"},StringSplitOptions.RemoveEmptyEntries));
                }
                if (string.IsNullOrWhiteSpace(response))
                {
                    response = "Bad Query";
                }
            }
            return response;
        }

        public string DealPost(string content, string token, string url)
        {
            var response = "";
            Employee emp = JsonConvert.DeserializeObject<Employee>(content);
            foreach (var employee in EmployeeRoute.employeesArray)
            {
                if (Convert.ToInt32(employee["Id"]) == emp.Id)
                {
                    response = "This is a same Id";
                    return response;
                }
            }
            response = "Seccessfully Create";
            EmployeeRoute.employeesArray.Add(
                JObject.Parse(content));
            return response;
        }

        public string DealPut(string content, string token, string url)
        {
            var response = "Falid Modified";
            Employee emp = JsonConvert.DeserializeObject<Employee>(content);
            foreach (var employee in EmployeeRoute.employeesArray)
            {
                if (Convert.ToInt32(employee["Id"]) == emp.Id)
                {
                    EmployeeRoute.employeesArray.Remove(employee);
                    EmployeeRoute.employeesArray.Add(JObject.FromObject(emp));
                    response = "Successfully Modified";
                    break;
                }
            }
            return response;
        }

        public string DealDelete(string content, string token, string url)
        {
            var response = "Falid Deleted";
            Employee emp = JsonConvert.DeserializeObject<Employee>(content);
            foreach (var employee in EmployeeRoute.employeesArray)
            {
                if (Convert.ToInt32(employee["Id"]) == emp.Id)
                {
                    EmployeeRoute.employeesArray.Remove(employee);
                    response = "Successfully Deleted";
                    break;
                }
            }
            return response;
        }
    }
}
