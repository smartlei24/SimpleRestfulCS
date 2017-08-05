using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace WindowsFormsApp1
{
    public partial class OpearatingEmployees : Form
    {
        public OpearatingEmployees()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            if (string.IsNullOrWhiteSpace(IdBox.Text))
            {
                label6.Text = "必须输入4位ID哦";
                return;
            }
            HttpCollector queryRequest = new HttpCollector("GET", "/users/"+IdBox.Text);
            queryRequest.SendRequest("");

            string responseContent = queryRequest.Content;

            if (responseContent=="Bad Query")
            {
                label6.Text = "查询结果为空";
                return;
            }

            Employee responceEmployee = JsonConvert.DeserializeObject<Employee>(responseContent) ;
            AgeBox.Text = responceEmployee.Age.ToString();
            NameBox.Text = responceEmployee.Name;
            PostionBox.Text = responceEmployee.Position;
            GenderBox.Text = responceEmployee.Gender;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            Employee modifiedEmployee = new Employee(
                Convert.ToInt16(IdBox.Text)
                , NameBox.Text
                , GenderBox.Text
                , PostionBox.Text
                , Convert.ToUInt16(AgeBox.Text));

            string requestContent = JsonConvert.SerializeObject(modifiedEmployee);
            HttpCollector modifiedRequest = new HttpCollector("PUT","/users/"+IdBox.Text);

            modifiedRequest.SendRequest(requestContent);
            string result =
                modifiedRequest.Content;
            label6.Text = result;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            Employee newEmployee = new Employee(
                Convert.ToInt16(IdBox.Text)
                , NameBox.Text
                , GenderBox.Text
                , PostionBox.Text
                , Convert.ToUInt16(AgeBox.Text));
            string requestContent = JsonConvert.SerializeObject(newEmployee);
            HttpCollector createRequest = new HttpCollector("POST","/users/"+IdBox.Text);
            createRequest.SendRequest(requestContent);

            string responseContent =
                createRequest.Content;
            label6.Text = responseContent;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            Employee deleteEmployee = new Employee(
                Convert.ToInt16(IdBox.Text)
                , NameBox.Text
                , GenderBox.Text
                , PostionBox.Text
                , Convert.ToUInt16(AgeBox.Text));
            string requestContent = JsonConvert.SerializeObject(deleteEmployee);
            HttpCollector deleteRequest = new HttpCollector("DELETE", "/users/" + IdBox.Text);
            deleteRequest.SendRequest(requestContent);

            string reponseContent =
                deleteRequest.Content;

            label6.Text = reponseContent;
        }

        [Test]
        public void test()
        {
            string a =
                "{\"Id\":9534,\"Name\":\"Rose\",\"Position\":\"UI设计师\",\"Age\":18,\"Gender\":\"女\"}";
            Employee bu = JsonConvert.DeserializeObject<Employee>(a);
            Console.WriteLine(bu.Id);
        }
    }
}
