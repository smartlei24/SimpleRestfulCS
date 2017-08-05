using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace ASimpleHttpServer
{
    static class EmployeeRoute
    {
        public static JArray employeesArray;

        private static string jsonFilePath = Application.StartupPath + "//employees.json";

        public static void LoadJsonFromFile()
        {
            var jsonFileContent = "";
            using (var reader =
                new StreamReader(jsonFilePath)
            )
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    jsonFileContent += line;
                }
            }
            employeesArray = JArray.Parse(jsonFileContent);
        }

        static public bool Sync()
        {
            var jsonFileContent = "";
            using (var reader =
                new StreamReader(jsonFilePath)
            )
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    jsonFileContent += line;
            }
            JArray ccc = JArray.Parse(jsonFileContent);
            try
            {
                using (StreamWriter writer = new StreamWriter(jsonFilePath, false, Encoding.UTF8))
                {
                    writer.Write(employeesArray.ToString());
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}