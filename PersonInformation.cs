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

namespace Csharp_shixi
{
    public partial class PersonInformation : Form
    {
        string[] info;
        string path = @"C:\Users\zerof\source\repos\Csharp_shixi\login.txt";
        public PersonInformation()
        {
            InitializeComponent();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void PersonInformation_Load(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(path);
            info = sr.ReadToEnd().Split(';');
            //用户名
            this.textBox1.Text = info[0];
            //密码
            this.textBox2.Text = info[1];
            //姓名
            this.textBox3.Text = info[2];
            //性别
            this.comboBox1.SelectedIndex = comboBox1.Items.IndexOf(info[3]);
            //出生日期
            this.dateTimePicker1.Value = Convert.ToDateTime(info[4]);
            //班级
            this.textBox6.Text = info[5];
            //学号
            this.textBox4.Text = info[6];
            //电话
            this.textBox8.Text = info[7];
            //备注
            this.textBox5.Text = info[8];
            //图片路径
            this.pictureBox3.Load(info[9]);
            sr.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter(path,false);
            List<string> vs = new List<string> { this.textBox1.Text, this.textBox2.Text , this.textBox3.Text ,
            this.comboBox1.SelectedItem.ToString(),this.dateTimePicker1.Value.ToString(),this.textBox6.Text,
            this.textBox4.Text,this.textBox8.Text,this.textBox5.Text,this.pictureBox3.ImageLocation};
            foreach (string item in vs)
            {
                sw.Write(item);
                sw.Write(';');
            }
            sw.Close();
            MessageBox.Show("保存成功");
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = ".";
            file.Filter = "所有文件(*.*)|*.*";
            file.ShowDialog();
            if (file.FileName != string.Empty)
            {
                try
                {
                    this.pictureBox3.Load(file.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.CheckState == CheckState.Checked)
            {
                this.textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                this.textBox2.UseSystemPasswordChar = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
