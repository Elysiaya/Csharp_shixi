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
    public partial class add_date : Form
    {
        OleDbConnection oleDbConnection;
        List<string> list_relation;
        ListView listView;
        ImageList ImageList;
        public add_date(OleDbConnection oleDbConnection,List<string> list_relation,ListView listView,ImageList imageList)
        {
            InitializeComponent();
            this.oleDbConnection = oleDbConnection;
            this.list_relation = list_relation;
            this.listView = listView;
            this.ImageList = imageList;
        }
        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 确认按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        string picture_name;
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.pictureBox1.ImageLocation != null)
            {
                picture_name = this.pictureBox1.ImageLocation.Split('\\').Last();
            }
            else
            {
                picture_name = "";
            }
            //picture_name = this.pictureBox1.ImageLocation;
            if (label_relation.Text == "") { label_relation.Text = "未设置"; }
            string values = String.Format("(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\")", label_name.Text, label_sex.Text, label_relation.Text, label_company.Text, label_phone.Text, label_Email.Text, picture_name, beizhu.Text);
            string sql = "insert into AddressBook (姓名,性别,关系,单位,联系电话,电子邮件,照片名称,备注) values " + values;
            if (!list_relation.Contains(label_relation.Text)) 
            {
                string sql2 = String.Format("insert into Relation values (\"{0}\")", label_relation.Text);
                OleDbCommand oleDbCommand2 = new OleDbCommand(sql2, oleDbConnection);
                oleDbCommand2.ExecuteNonQuery();
            }
            OleDbCommand oleDbCommand = new OleDbCommand(sql, oleDbConnection);
            int x = oleDbCommand.ExecuteNonQuery();
            MessageBox.Show(String.Format("添加数据成功，共添加了{0}行数据", x));
            
            ListViewItem listViewitem = new ListViewItem();
            listViewitem.Text = label_name.Text;
            int id = (int)(new OleDbCommand("select @@IDENTITY as res from AddressBook", oleDbConnection).ExecuteScalar());

            Person person = new Person(id);
            person.name = label_name.Text;
            person.sex = label_relation.Text;
            person.relation = label_name.Text;
            person.company = label_company.Text;
            person.picture_name = label_phone.Text;
            person.mail = label_Email.Text;
            person.picture_name = picture_name;
            person.info = beizhu.Text;

            listViewitem.Tag = person;
            listView.Items.Add(listViewitem);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = ".";
            file.Filter = "所有文件(*.*)|*.*";
            file.ShowDialog();
            if (file.FileName != string.Empty)
            {
                try
                {
                    picture_name = file.FileName.Split('\\').Last();
                    this.pictureBox1.Load(file.FileName);
                    ImageList.Images.Add(picture_name,Image.FromFile(file.FileName));

                    string pLocalFilePath = file.FileName;//要复制的文件路径
                    string pSaveFilePath = String.Format(@"..\..\Resources\个人通讯录图片集合\{0}", picture_name);
                    if (File.Exists(pLocalFilePath))//必须判断要复制的文件是否存在
                    {
                        File.Copy(pLocalFilePath, pSaveFilePath, true);//三个参数分别是源文件路径，存储路径，若存储路径有相同文件是否替换
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }


        }
    }
}
