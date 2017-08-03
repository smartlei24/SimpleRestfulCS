using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class LoginWindows : Form
    {
        public LoginWindows()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            //检查TextBox是否为空
            if (string.IsNullOrWhiteSpace(textBox3.Text)
                || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("不能为空哦");
                return;
            }

            HttpCollector requestSocket = new HttpCollector();
            requestSocket.SendRequest(textBox2+"&"+textBox3.Text);
            string message = requestSocket.ResponseMessage;

            string contentText = message.Split(new [] {"\r\n\r\n"}
            , StringSplitOptions.RemoveEmptyEntries)[1];
            //在状态栏显示Response中Content，并将整个报文打印在Textbox1中
            statusText.Text = contentText;
            textBox1.Text += message;
        }
    }
}