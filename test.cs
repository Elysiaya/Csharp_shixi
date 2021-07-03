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
    public partial class test : Form
    {
        public test()
        {
            InitializeComponent();
            ListViewGroup group = new ListViewGroup();
            group.Header = "123123";
            group.Name = "123123";
            listView1.Groups.Add(group);
            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Text = "4654564546";
            group.Items.Add(listViewItem);
        }

        private void test_Load(object sender, EventArgs e)
        {

        }
    }
}
