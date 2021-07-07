using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
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
        OleDbConnection oleDbConnection;
        public PersonInformation()
        {
            InitializeComponent();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void PersonInformation_Load(object sender, EventArgs e)
        {

            string conn_str = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=..\..\个人通讯录.mdb;Persist Security Info=True";
            oleDbConnection = new OleDbConnection(conn_str);
            oleDbConnection.Open();
            string sql = "select * from personinfo";
            OleDbCommand oleDbCommand = new OleDbCommand(sql, oleDbConnection);
            OleDbDataReader dr = oleDbCommand.ExecuteReader();
            dr.Read();

            //用户名
            this.textBox1.Text = (string)dr[1];
            //密码
            this.textBox2.Text = (string)dr[2];
            //姓名
            this.textBox3.Text = (string)dr[3];
            //性别
            this.comboBox1.SelectedIndex = comboBox1.Items.IndexOf((string)dr[4]);
            //出生日期
            this.dateTimePicker1.Value = Convert.ToDateTime((string)dr[5]);
            //班级
            this.textBox6.Text = (string)dr[6];
            //学号
            this.textBox4.Text = (string)dr[7];
            //电话
            this.textBox8.Text = (string)dr[8];
            //备注
            this.textBox5.Text = (string)dr[9];
            //图片

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = $"update personinfo set [username]=\'{this.textBox1.Text}\',[password]=\'{this.textBox2.Text}\',[姓名]=\'{this.textBox3.Text}\',[性别]=\'{this.comboBox1.SelectedItem.ToString()}\',[出生日期]=\"{this.dateTimePicker1.Value.ToString()}\",[班级]=\'{this.textBox6.Text}\',[学号]=\'{this.textBox4.Text}\',[电话]=\'{this.textBox8.Text}\',[备注]=\'{this.textBox5.Text}\' where [ID]=1;";
            OleDbCommand oleDbCommand = new OleDbCommand(sql, oleDbConnection);
            int x = oleDbCommand.ExecuteNonQuery();
            MessageBox.Show("更新成功");
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
                    Image image = Image.FromFile(file.FileName);
                    this.pictureBox3.Image = image;
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
