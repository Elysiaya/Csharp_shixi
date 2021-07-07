using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
namespace Csharp_shixi
{
    public partial class Login : Form
    {
        string username;
        string passwd;
        string name;
        int login_fail_num=3;
        public Login()
        {
            InitializeComponent();

            string conn_str = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=..\..\个人通讯录.mdb;Persist Security Info=True";
            OleDbConnection oleDbConnection = new OleDbConnection(conn_str);
            oleDbConnection.Open();
            string sql = "select username,password from personinfo";
            OleDbCommand oleDbCommand = new OleDbCommand(sql, oleDbConnection);
            OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
            oleDbDataReader.Read();
            username = (string)oleDbDataReader["username"];
            passwd = (string)oleDbDataReader["password"];
            oleDbDataReader.Close();
            oleDbConnection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            if (this.username_textbox.Text==username&&this.passwd_textbox.Text==passwd)
            {
                //登陆成功
                MessageBox.Show("登录成功");
                Form main_form = new Main_form(name);
                main_form.Show();
            }
            else
            {
                //登录失败
                if (login_fail_num != 0) 
                {
                    login_fail_num--;
                    MessageBox.Show(String.Format("用户名或密码不正确，请重新输入(还有{0}次机会)", login_fail_num)); 
                }
                else
                {
                    MessageBox.Show("错误次数已达3次，程序即将自动退出");
                    this.Close();
                }
            }
        }
    }
}
