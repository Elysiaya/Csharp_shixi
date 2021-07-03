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
    public partial class Address : Form
    {
        OleDbConnection oleDbConnection;
        List<string> list_relation = new List<string> { };
        List<Person> peoples = new List<Person> { };
        
        public Address()
        {
            InitializeComponent();
        }

        private void Address_Load(object sender, EventArgs e)
        {
            string connstring = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=..\..\个人通讯录.mdb";
            oleDbConnection = new OleDbConnection(connstring);
            oleDbConnection.Open();
            //从数据库中获取所有关系保存在list_relation中
            get_relation();
            //get_peopleslist();

            this.listView1.BeginUpdate();
            foreach (var relation in list_relation)
            {
                ListViewGroup listViewGroup = new ListViewGroup(relation);
                this.listView1.Groups.Add(listViewGroup);
                string sql2 = String.Format("select * from AddressBook where 关系=\"{0}\"", relation);
                OleDbCommand oleDb = new OleDbCommand(sql2, oleDbConnection);
                OleDbDataReader dr = oleDb.ExecuteReader();
                while (dr.Read())
                {
                    try
                    {
                        imageList1.Images.Add((string)dr["照片名称"], Image.FromFile(String.Format(@"..\..\Resources\个人通讯录图片集合\{0}", (string)dr["照片名称"])));
                    }
                    catch (Exception)
                    {

                    }
                    
                    ListViewItem listViewItem = new ListViewItem();

                    listViewItem.Tag = setperson(dr);
                    listViewItem.Text = ((Person)listViewItem.Tag).name;
                    listViewItem.Name = ((Person)listViewItem.Tag).name;
                    listViewItem.Group = listViewGroup;
                    listViewItem.ImageKey = ((Person)listViewItem.Tag).picture_name;

                    //将其添加到listview
                    this.listView1.Items.Add(listViewItem);
                }
            }
            this.listView1.EndUpdate();


        }

        Person setperson(OleDbDataReader dr)
        {
            Person person = new Person((int)dr[0]);
            person.name = (string)dr[1];
            person.sex = (string)dr[2];
            person.relation = (string)dr[3];
            person.company = (string)dr[4];
            person.phone = (string)dr[5];
            person.mail = (string)dr[6];
            person.picture_name = (string)dr[7];
            person.info = (string)dr[8];

            return person;
        }

        /// <summary>
        /// 打开数据库获取关系表的信息
        /// </summary>
        private void get_relation()
        {
            string sqlstring = "select * from Relation";
            OleDbCommand oleDbCommand = new OleDbCommand(sqlstring, oleDbConnection);
            OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
            while (oleDbDataReader.Read())
            {
                string relation = (string)oleDbDataReader[0];
                list_relation.Add(relation);
            }
            oleDbDataReader.Close();
        }
        /// <summary>
        /// 根据数据库获得peoples
        /// </summary>
        private void get_peopleslist()
        {
            string sql = "select * from AddressBook";
            OleDbCommand oleDbCommand = new OleDbCommand(sql, oleDbConnection);
            OleDbDataReader dr = oleDbCommand.ExecuteReader();
            while (dr.Read())
            {
                Person person = new Person((int)dr[0]);
                person.name = (string)dr[1];
                person.sex = (string)dr[2];
                person.relation = (string)dr[3];
                person.company = (string)dr[4];
                person.phone = (string)dr[5];
                person.mail = (string)dr[6];
                person.picture_name = (string)dr[7];
                person.info = (string)dr[8];

                peoples.Add(person);
            }
            dr.Close();
        }
        /// <summary>
        /// listview点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            set_tongxinlu(this.listView1.SelectedItems[0].Tag as Person);
        }
        /// <summary>
        /// 根据person对象绘制界面
        /// </summary>
        /// <param name="person"></param>
        private void set_tongxinlu(Person person)
        {
            this.label_name.Text = person.name;
            this.label_phone.Text = person.phone;
            this.label_sex.Text = person.sex;
            this.label_relation.Text = person.relation;
            this.label_company.Text = person.company;
            this.label_Email.Text = person.mail;
            this.beizhu.Text = person.info;
            this.pictureBox1.Image = imageList1.Images[person.picture_name];
        }
        /// <summary>
        /// 复选框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.CheckState == CheckState.Checked)
            {
                this.label_name.ReadOnly = false;
                this.label_phone.ReadOnly = false;
                this.label_Email.ReadOnly = false;
                this.label_company.ReadOnly = false;
                this.label_relation.ReadOnly = false;
                this.label_sex.ReadOnly = false;
                this.beizhu.ReadOnly = false;
                this.更新按钮.Enabled = true;
            }
            else
            {
                this.label_name.ReadOnly = true;
                this.label_phone.ReadOnly = true;
                this.label_Email.ReadOnly = true;
                this.label_company.ReadOnly = true;
                this.label_relation.ReadOnly = true;
                this.label_sex.ReadOnly = true;
                this.beizhu.ReadOnly = true;
                this.更新按钮.Enabled = false;
            }
        }
        /// <summary>
        /// 更新按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            int ID = ((Person)this.listView1.SelectedItems[0].Tag).id;
            string pn = ((Person)this.listView1.SelectedItems[0].Tag).picture_name;
            string sql = String.Format("update AddressBook set 姓名=\"{0}\",性别=\"{1}\",关系=\"{2}\",单位=\"{3}\",联系电话=\"{4}\",电子邮件=\"{5}\",备注=\"{6}\",照片名称=\"{7}\" where 编号={8}"
                ,label_name.Text,label_sex.Text,label_relation.Text,label_company.Text,label_phone.Text,label_Email.Text,beizhu.Text, pn, ID);
            ((Person)this.listView1.SelectedItems[0].Tag).name = label_name.Text;
            ((Person)this.listView1.SelectedItems[0].Tag).sex = label_sex.Text;
            ((Person)this.listView1.SelectedItems[0].Tag).relation = label_relation.Text;
            ((Person)this.listView1.SelectedItems[0].Tag).company = label_company.Text;
            ((Person)this.listView1.SelectedItems[0].Tag).phone = label_phone.Text;
            ((Person)this.listView1.SelectedItems[0].Tag).mail = label_Email.Text;
            ((Person)this.listView1.SelectedItems[0].Tag).info = beizhu.Text;


            OleDbCommand oleDbCommand = new OleDbCommand(sql, oleDbConnection);
            int x = oleDbCommand.ExecuteNonQuery();
            MessageBox.Show("更新数据成功"+"更新了"+x.ToString()+"行");


         
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            add_date add_Date = new add_date(oleDbConnection,list_relation,this.listView1,this.imageList1);
            add_Date.ShowDialog();
            add_Date.Text = "添加数据";

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = (this.listView1.SelectedItems[0].Tag as Person).id;
            string sql = "delete from AddressBook where 编号=" + ID.ToString();
            OleDbCommand oleDbCommand = new OleDbCommand(sql, oleDbConnection);
            int x = oleDbCommand.ExecuteNonQuery();
            MessageBox.Show("删除成功了" + x.ToString() + "行数据");
            listView1.Items.Remove(this.listView1.SelectedItems[0]);
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
                    string picture_name = file.FileName.Split('\\').Last();
                    this.pictureBox1.Load(file.FileName);
                    imageList1.Images.Add(picture_name, Image.FromFile(file.FileName));
                    (this.listView1.SelectedItems[0].Tag as Person).picture_name = picture_name;

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
