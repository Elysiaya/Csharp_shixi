using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Csharp_shixi
{
    public partial class NetworkCommunications : Form
    {
        string ipv4;
        string ipv6;
        int port = 8003;
        TcpListener server;
        public NetworkCommunications()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        private void NetworkCommunications_Load(object sender, EventArgs e)
        {


            //获取本机IP
            string name = Dns.GetHostName();
            IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
            foreach (var item in ipadrlist)
            {
                if (item.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipv4 = item.ToString();
                    this.textBox_localip.Text = ipv4;
                }
                if (item.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    ipv6 = item.ToString();
                    //this.textBox_localip.Text = ipv6;
                }
            }

            //添加发送ip
            this.treeView1.Nodes.Add(ipv4);
            this.textBox_localip.Text = ipv4;
            this.textBox_sentip.Text = ipv4;

            //开始监听

            //获得IP
            try
            {
                //创建ip对象
                IPAddress ip = IPAddress.Parse(ipv4);
                //创建端口号, EndPoint对象
                IPEndPoint point = new IPEndPoint(ip, port);
                //创建一个TcpListener对象
                server = new TcpListener(point);
                //开始监听
                server.Start();
                add_text_to_textbox("开始监听");
                //使用异步方法
                server.BeginAcceptTcpClient(Accept,server);


            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
            }
            finally
            {
                //server.Stop();
            }
        }
        /// <summary>
        /// 定义回调函数
        /// </summary>
        /// <param name="iar"></param>
        void Accept(IAsyncResult iar)
        {
            //还原传入的原始套接字
            TcpListener MyServer = (TcpListener)iar.AsyncState;
            //在原始套接字上调用EndAccept方法，返回新的套接字
            TcpClient tcpClient = MyServer.EndAcceptTcpClient(iar);

            string data = null;
            byte[] bytes = new Byte[256];
            NetworkStream networkStream = tcpClient.GetStream();
            while (networkStream.Read(bytes, 0, bytes.Length) != 0)
            {
                data = System.Text.Encoding.UTF8.GetString(bytes);
                add_text_to_textbox(data);
            }
            tcpClient.Close();
        }

        TcpClient client;
        NetworkStream stream;
        /// <summary>
        /// 连接按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (client == null)
            {
                IPAddress ip = IPAddress.Parse(this.textBox_sentip.Text);
                IPEndPoint iPEndPoint = new IPEndPoint(ip, port);

                try
                {
                    client = new TcpClient();
                    client.Connect(iPEndPoint);
                }
                catch (Exception a)
                {
                    MessageBox.Show(a.Message);
                }
            }
            sent(this.textBox2.Text);
        }
        void sent(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            stream = client.GetStream();
            stream.Write(bytes, 0, bytes.Length);
            this.textBox2.Text = "";
        }

        void add_text_to_textbox(string text)
        {
            this.textBox1.Text += text + "\r\n";
        }

    }
}
