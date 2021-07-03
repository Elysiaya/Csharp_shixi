using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Csharp_shixi
{
    public partial class Main_form : Form
    {
        string name;
        public Main_form(string name)
        {
            InitializeComponent();
            this.name = name;
            this.Text = String.Format("[{0}]的个人工作台", name);
        }

        private void Main_form_Load(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.toolStripLabel1.Text = name + "欢迎使用本系统" + "当前时间" + DateTime.Now.ToString();
            this.toolStripLabel2.Text = "当前操作";

        }
        Address address = null;
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (address == null)
            {
                address = new Address();
                address.MdiParent = this;
                address.Show();
            }
            else
            {
                MessageBox.Show("通信录窗口已打开，请不要重复打开");
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            PersonInformation personInformation = new PersonInformation();
            personInformation.MdiParent = this;
            personInformation.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            WebManagement webManagement = new WebManagement();
            webManagement.MdiParent = this;
            webManagement.Show();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            NetworkCommunications networkCommunications = new NetworkCommunications();
            networkCommunications.MdiParent = this;
            networkCommunications.Show();
        }
    }
}
