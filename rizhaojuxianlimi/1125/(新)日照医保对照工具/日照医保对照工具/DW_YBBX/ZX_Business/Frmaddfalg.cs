using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace DW_YBBX.ZX_Business
{
    public partial class Frmaddfalg : Form
    {
        public MSSQLHelpers SQLHelper = new MSSQLHelpers(ConfigurationManager.ConnectionStrings["connStr"].ToString());
        public string thzxbm = "";
        public string thzxbmc = "";
        public string thhisbm = "";
        public bool canclesf = false;
        public Frmaddfalg()
        {
            InitializeComponent();
        }

        private void Frmaddfalg_Load(object sender, EventArgs e)
        {

            //StringBuilder StrSql = new StringBuilder();

            //StrSql.Append("");
            //DataSet ds = new DataSet();
            //DataTable hisdata = new DataTable();
            //ds = SQLHelper.ExecSqlReDs(StrSql.ToString());
            //hisdata = ds.Tables[0];
            //this.dataGridView1.DataSource = hisdata;
            //.DataSource = hisdata;
            //return hisdata;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            thzxbm = this.tbxthzxbm.Text;
            thzxbmc = this.tbxthzhmc.Text;
            thhisbm = this.tbxthhisbm.Text;



            if (string.IsNullOrEmpty(thzxbm)||string.IsNullOrEmpty(thzxbmc)||string.IsNullOrEmpty(thhisbm))
            {
                canclesf = false;
                MessageBox.Show("所填数据不能为空！");
                return;

              //  this.Close();
//
            }
            else
            {
                canclesf = true;
                this.Close();

            }
        }

        private void Frmaddfalg_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!canclesf)
            {
                MessageBox.Show("点击了自定义添加按钮！请务必完成添加，如不需要添加，也必须采用先添加后删除的方式！");
                e.Cancel = true; 
            }

        }
    }
}
