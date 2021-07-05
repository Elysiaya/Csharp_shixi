using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Csharp_shixi
{
    public partial class WebManagement : Form
    {
        List<Web> webs = new List<Web> { };
        const string xml_path = @"..\..\InternetKeyListConfig.xml";
        public WebManagement()
        {
            InitializeComponent();
            this.textBox5.Text = "欢迎使用本系统\r\n";
        }

        private void WebManagement_Load(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"..\..\InternetKeyListConfig.xml");
            XmlNode xmlNode = doc.SelectSingleNode("InternetKeyListConfig");
            XmlNodeList xnl = xmlNode.ChildNodes;
            foreach (XmlNode item in xnl)
            {
                Web web = new Web();
                XmlElement xe = (XmlElement)item;


                web.keywords = xe.GetAttribute("keywords").ToString();
                web.username = xe.GetAttribute("username").ToString();
                web.webset = xe.GetAttribute("webset").ToString();
                web.chinesename = xe.GetAttribute("chinesename").ToString();

                webs.Add(web);
            }
            set_treeview();

        }

        void set_treeview()
        {
            TreeNode treeNode1 = new TreeNode("所有网站信息");
            this.treeView1.Nodes.Add(treeNode1);
            foreach (Web item in webs)
            {
                TreeNode treeNode = new TreeNode(item.chinesename);
                treeNode.Tag = item;


                treeNode1.Nodes.Add(treeNode);

                //三个子节点
                treeNode.Nodes.Add(item.username);
                treeNode.Nodes.Add(item.webset);
                treeNode.Nodes.Add(item.keywords);
            }
            treeView1.Nodes[0].Expand();
        }

        private void treeView1_Click(object sender, EventArgs e)
        {

        }
        Web web;
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.treeView1.SelectedNode == null) { return; }
            web = (Web)this.treeView1.SelectedNode.Tag;
            web.chinesename = this.textBox1.Text;
            web.keywords = this.textBox4.Text;
            web.username = this.textBox3.Text;
            web.webset = this.textBox2.Text;
            this.treeView1.Nodes.Clear();
            set_treeview();

            write_xmldoc();
            this.textBox5.Text += "更新节点" + web.chinesename + "\r\n";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Web web = new Web();
            web.chinesename = this.textBox1.Text;
            web.webset = this.textBox2.Text;
            web.username = this.textBox3.Text;
            web.keywords = this.textBox4.Text;
            int id = Array.IndexOf(webs.Select(a => a.chinesename).ToArray(), web.chinesename);
            if (id != -1)
            {
                DialogResult dialogResult = MessageBox.Show(String.Format("{0}已经在列表中了，是否添加相同的元素", web.chinesename), "确认新增", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.OK)
                {
                    webs.Add(web);
                }
                else
                {
                    return;
                }

            }
            else
            {
                webs.Add(web);
            }


            this.treeView1.Nodes.Clear();
            set_treeview();
            write_xmldoc();

            this.textBox5.Text += "添加元素" + web.chinesename + "\r\n";
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode treeNode = this.treeView1.SelectedNode;
            if (treeNode == null) { return; }
            if (treeNode.Nodes.Count == 3)
            {
                this.textBox1.Text = treeNode.Text;
                this.textBox2.Text = treeNode.Nodes[1].Text;
                this.textBox3.Text = treeNode.Nodes[0].Text;
                this.textBox4.Text = treeNode.Nodes[2].Text;
                this.groupBox3.Text = treeNode.Text;
            }
            this.textBox5.Text += "选择节点" + treeNode.Text + "\r\n";
        }
        /// <summary>
        /// checkBox改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.CheckState == CheckState.Checked)
            {
                this.textBox1.ReadOnly = false;
                this.textBox2.ReadOnly = false;
                this.textBox3.ReadOnly = false;
                this.textBox4.ReadOnly = false;

                this.textBox5.Text += "编辑已打开\r\n";
            }
            else if (this.checkBox1.CheckState == CheckState.Unchecked)
            {
                this.textBox1.ReadOnly = true;
                this.textBox2.ReadOnly = true;
                this.textBox3.ReadOnly = true;
                this.textBox4.ReadOnly = true;

                this.textBox5.Text += "编辑已关闭\r\n";
            }
        }
        /// <summary>
        /// 写入xml文件
        /// </summary>
        /// <param name="xml_path"></param>
        void write_xmldoc(string xml_path = xml_path)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.AppendChild(xmldoc.CreateXmlDeclaration("1.0", "UTF-8", null));
            XmlElement xmlElement = xmldoc.CreateElement("", "InternetKeyListConfig", "");
            xmldoc.AppendChild(xmlElement);
            foreach (Web item in webs)
            {
                //设置子节点
                XmlElement xmlElement1 = xmldoc.CreateElement("key");

                xmlElement1.SetAttribute("chinesename", item.chinesename);
                xmlElement1.SetAttribute("webset", item.webset);
                xmlElement1.SetAttribute("username", item.username);
                xmlElement1.SetAttribute("keywords", item.keywords);
                xmlElement.AppendChild(xmlElement1);
            }

            xmldoc.Save(xml_path);
        }
        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {

            using (var s = new System.IO.StreamWriter(@"..\..\网站信息.csv", false, Encoding.UTF8, 30))
            {
                s.WriteLine("网站名,网址,用户名,密码");
                foreach (var item in webs)
                {
                    s.WriteLine($"{item.chinesename},{item.webset},{item.username},{item.keywords}");
                }
                MessageBox.Show("导出excel完成");
            }
        }
    }

    /// <summary>
    /// web类，有四个字段
    /// </summary>
    class Web
    {
        public string keywords;
        public string username;
        public string webset;
        public string chinesename;
    }
}
