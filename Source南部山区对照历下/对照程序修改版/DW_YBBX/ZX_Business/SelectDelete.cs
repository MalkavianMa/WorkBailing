using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DW_YBBX.ZX_Business
{
    public partial class SelectDelete : Form
    {
        public MSSQLHelpers sqlHelper = new MSSQLHelpers(ConfigurationManager.ConnectionStrings["connStr"].ToString());
        string hosid = MainForm.HOSPITAL_ID;

        /// <summary>
        /// 费用类别
        /// </summary>
        public string fylb = "";

        /// <summary>
        /// 删除费用类别
        /// </summary>
        public string Upfylb = "";

        /// <summary>
        /// 医院编码
        /// </summary>
        public string Uphiscode = "";

        public SelectDelete()
        {
            InitializeComponent();
        }

        private void SelectDelete_Load(object sender, EventArgs e)
        {
            ArrayList arr1 = new ArrayList();

            arr1.Add(new DictionaryEntry("2", "居民统筹"));
            arr1.Add(new DictionaryEntry("1", "居民门规"));
            arr1.Add(new DictionaryEntry("3", "普通门诊"));
            arr1.Add(new DictionaryEntry("4", "职工门规"));
            arr1.Add(new DictionaryEntry("5", "免费药品"));
            arr1.Add(new DictionaryEntry("6", "职工统筹"));
            arr1.Add(new DictionaryEntry("7", "居民住院"));
            arr1.Add(new DictionaryEntry("kill", "全类别价格更新"));
            arr1.Add(new DictionaryEntry("8", "职工住院"));

            //arr1.Add(new DictionaryEntry("", ""));
            //arr1.Add(new DictionaryEntry("7", "职工住院"));
            //arr1.Add(new DictionaryEntry("8", "居民住院"));
            //arr1.Add(new DictionaryEntry("10", "职工统筹"));
            //arr1.Add(new DictionaryEntry("11", "居民统筹"));
            //arr1.Add(new DictionaryEntry("12", "免费用药"));
            //arr1.Add(new DictionaryEntry("13", "职工普通门诊"));
            //arr1.Add(new DictionaryEntry("10004", "测试"));
            cmbfylb.DataSource = arr1;
            cmbfylb.DisplayMember = "Value";
            cmbfylb.ValueMember = "Key";
            cmbfylb.SelectedIndex = 7;
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder strSselect = new StringBuilder();
            if (cmbfylb.Text == "全类别价格更新")
            {
                strSselect.Append(" SELECT NETWORKING_PAT_CLASS_ID AS 费用类别,HIS_ITEM_CODE AS 医院编码,HIS_ITEM_NAME AS 医院名称,NETWORK_ITEM_CODE AS 中心编码,NETWORK_ITEM_NAME AS 中心名称,SELF_BURDEN_RATIO AS 自付比例,NETWORK_ITEM_PRICE AS 价格,HOSPITAL_ID AS 医院编号 ");
                strSselect.Append(" FROM COMM.COMM.NETWORKING_ITEM_VS_HIS  WHERE (HOSPITAL_ID='" + hosid + "') AND  HIS_ITEM_CODE LIKE '%" + txtmcbm.Text + "%' OR HIS_ITEM_NAME LIKE '%" + txtmcbm.Text + "%' OR NETWORK_ITEM_CODE LIKE '%" + txtmcbm.Text + "%' ");

            }
            else
            {
                if (cmbfylb.Text == "居民统筹")
                {
                    fylb = "2";
                }
                else if (cmbfylb.Text == "居民门规")
                {
                    fylb = "1";
                }
                else if (cmbfylb.Text == "普通门诊")
                {
                    fylb = "3";
                }
                else if (cmbfylb.Text == "职工门规")
                {
                    fylb = "4";
                }
                else if (cmbfylb.Text == "免费药品")
                {
                    fylb = "5";
                }
                else if (cmbfylb.Text == "职工统筹")
                {
                    fylb = "6";
                }
                else if (cmbfylb.Text == "居民住院")
                {
                    fylb = "7";
                }
                else if (cmbfylb.Text == "职工住院")
                {
                    fylb = "8";
                }
                //else if (cmbfylb.Text == "居民住院")
                //{
                //    fylb = "7";
                //}


                //if (cmbfylb.Text == "测试")
                //{
                //    fylb = "10004";
                //}
                strSselect.Append(" SELECT 0 as ISCHECK,NETWORKING_PAT_CLASS_ID AS 费用类别,HIS_ITEM_CODE AS 医院编码,HIS_ITEM_NAME AS 医院名称,NETWORK_ITEM_CODE AS 中心编码,NETWORK_ITEM_NAME AS 中心名称,SELF_BURDEN_RATIO AS 自付比例,NETWORK_ITEM_PRICE AS 价格,HOSPITAL_ID AS 医院编码 ");

                strSselect.Append(" FROM COMM.COMM.NETWORKING_ITEM_VS_HIS  WHERE (HOSPITAL_ID='" + hosid + "') AND  NETWORKING_PAT_CLASS_ID='" + fylb + "' AND  (  HIS_ITEM_CODE LIKE '%" + txtmcbm.Text + "%' OR HIS_ITEM_NAME LIKE '%" + txtmcbm.Text + "%' OR NETWORK_ITEM_CODE LIKE '%" + txtmcbm.Text + "%') ");

            }
            DataSet ds = new DataSet();
            DataTable hisdata = new DataTable();
            ds = sqlHelper.ExecSqlReDs(strSselect.ToString());
            hisdata = ds.Tables[0];
            dataGridView1.DataSource = hisdata;
        }

        /// <summary>
        /// datagirdview单机事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_Click(object sender, EventArgs e)
        {
            //UpZfbl = dataGridView1.CurrentRow.Cells["自付比例"].Value.ToString();
            Uphiscode = dataGridView1.CurrentRow.Cells["医院编码"].Value.ToString();
            //Updylb = dataGridView1.CurrentRow.Cells["费用类别"].Value.ToString();
            Upfylb = dataGridView1.CurrentRow.Cells["费用类别"].Value.ToString();
            txtDeletehiscode.Text = Uphiscode;
        }

        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string hiscode = Uphiscode;
            string Deletefylb = Upfylb;

            if (string.IsNullOrEmpty(Uphiscode)||string.IsNullOrEmpty(Upfylb))
            {
                MessageBox.Show("请单击选中删除的数据");
                return;
            }
            try
            {
                StringBuilder strDelete = new StringBuilder();
                //strDelete.Append(" DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID='" + Deletefylb + "' AND HIS_ITEM_CODE='" + hiscode + "' ");
                strDelete.Append(" DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE   HIS_ITEM_CODE='" + hiscode + "' and HOSPITAL_ID = '" + MainForm.HOSPITAL_ID + "'");

                int deleteSuccess = sqlHelper.ExecSqlReInt(strDelete.ToString());
                if (deleteSuccess > 0)
                {
                    MessageBox.Show("删除成功");
                }
            }
            catch (Exception ex)
            {

                throw new Exception("删除出错，请重新操作"+ex.ToString());
            }

          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
