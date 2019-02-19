using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Collections;

namespace DW_YBBX.ZX_Business
{
    public partial class Xgzfbl : Form
    {
        public MSSQLHelpers sqlHelper = new MSSQLHelpers(ConfigurationManager.ConnectionStrings["connStr"].ToString());
        string hosid = MainForm.HOSPITAL_ID;
        /// <summary>
        /// 费用类别
        /// </summary>
        public string fylb = "";

        /// <summary>
        /// 自付比例
        /// </summary>
        public string UpZfbl = "";
        /// <summary>
        /// 医院编码
        /// </summary>
        public string Uphiscode = "";
        /// <summary>
        /// 费用类别
        /// </summary>
        public string Updylb = "";
        public Xgzfbl()
        {
            InitializeComponent();
        }



        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder strSselect = new StringBuilder();
            if (cmbfylb.Text == "")
            {
                strSselect.Append(" SELECT NETWORKING_PAT_CLASS_ID AS 费用类别,HIS_ITEM_CODE AS 医院编码,HIS_ITEM_NAME AS 医院名称,NETWORK_ITEM_CODE AS 中心编码,NETWORK_ITEM_NAME AS 中心名称,SELF_BURDEN_RATIO AS 自付比例,NETWORK_ITEM_PRICE AS 价格,HOSPITAL_ID AS 医院编号 ");
                strSselect.Append(" FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE (HOSPITAL_ID='" + hosid + "') AND   HIS_ITEM_CODE LIKE '%" + txtmcbm.Text + "%' OR HIS_ITEM_NAME LIKE '%" + txtmcbm.Text + "%' OR NETWORK_ITEM_CODE LIKE '%" + txtmcbm.Text + "%' AND   HOSPITAL_ID  ");

            }
            else
            {


                //arraylist1.Add(new DictionaryEntry(ConfigurationManager.AppSettings["JMTC_PATID"].ToString(), "居民统筹"));
                //arraylist1.Add(new DictionaryEntry(ConfigurationManager.AppSettings["JMMG_PATID"].ToString(), "居民门规"));
                //arraylist1.Add(new DictionaryEntry(ConfigurationManager.AppSettings["PTMZ_PATID"].ToString(), "普通门诊"));
                //arraylist1.Add(new DictionaryEntry(ConfigurationManager.AppSettings["ZGMG_PATID"].ToString(), "职工门规"));
                //arraylist1.Add(new DictionaryEntry(ConfigurationManager.AppSettings["MFYY_PATID"].ToString(), "免费药品"));
                //arraylist1.Add(new DictionaryEntry(ConfigurationManager.AppSettings["ZGTC_PATID"].ToString(), "职工统筹"));
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
                //if (cmbfylb.Text == "测试")
                //{
                //    fylb = "10004";
                //}
                strSselect.Append(" SELECT 0 as ISCHECK,NETWORKING_PAT_CLASS_ID AS 费用类别,HIS_ITEM_CODE AS 医院编码,HIS_ITEM_NAME AS 医院名称,NETWORK_ITEM_CODE AS 中心编码,NETWORK_ITEM_NAME AS 中心名称,SELF_BURDEN_RATIO AS 自付比例,NETWORK_ITEM_PRICE AS 价格,HOSPITAL_ID AS 医院id ");

                strSselect.Append(" FROM COMM.COMM.NETWORKING_ITEM_VS_HIS  WHERE (HOSPITAL_ID='" + hosid + "') AND    NETWORKING_PAT_CLASS_ID='" + fylb + "' AND ( HIS_ITEM_CODE LIKE '%" + txtmcbm.Text + "%' OR HIS_ITEM_NAME LIKE '%" + txtmcbm.Text + "%' OR NETWORK_ITEM_CODE LIKE '%" + txtmcbm.Text + "%' )");

            }
            DataSet ds = new DataSet();
            DataTable hisdata = new DataTable();
            ds = sqlHelper.ExecSqlReDs(strSselect.ToString());
            hisdata = ds.Tables[0];
            dataGridView1.DataSource = hisdata;



        }


        private void Xgzfbl_Load(object sender, EventArgs e)
        {
            ArrayList arr1 = new ArrayList();
            arr1.Add(new DictionaryEntry("2", "居民统筹"));
            arr1.Add(new DictionaryEntry("1", "居民门规"));
            arr1.Add(new DictionaryEntry("3", "普通门诊"));
            arr1.Add(new DictionaryEntry("4", "职工门规"));
            arr1.Add(new DictionaryEntry("5", "免费药品"));
            arr1.Add(new DictionaryEntry("6", "职工统筹"));
            arr1.Add(new DictionaryEntry("7", "居民住院"));
            arr1.Add(new DictionaryEntry("8", "职工住院"));

            // arr1.Add(new DictionaryEntry("13", "职工普通门诊"));
            //arr1.Add(new DictionaryEntry("10004", "测试"));
            cmbfylb.DataSource = arr1;
            cmbfylb.DisplayMember = "Value";
            cmbfylb.ValueMember = "Key";
            cmbfylb.SelectedIndex = 6;
        }

        /// <summary>
        /// 修改方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //string Zfblxg = dataGridView1.CurrentRow.Cells["自付比例"].Value.ToString();
                string hisCode = Uphiscode;
                string fylbxg = Updylb;
                string Yxgzfbl = txtYxgzfbl.Text;

                if (string.IsNullOrEmpty(hisCode) || string.IsNullOrEmpty(fylbxg) || string.IsNullOrEmpty(Yxgzfbl))
                {
                    MessageBox.Show("未选中要修改的数据，请单击选中！");
                    return;
                }

                StringBuilder strUpdate = new StringBuilder();
                strUpdate.Append("UPDATE COMM.COMM.NETWORKING_ITEM_VS_HIS SET SELF_BURDEN_RATIO='" + Yxgzfbl + "'  WHERE (HOSPITAL_ID='" + hosid + "') AND   NETWORKING_PAT_CLASS_ID='" + fylbxg + "' AND HIS_ITEM_CODE='" + hisCode + "' ");
                int success = sqlHelper.ExecSqlReInt(strUpdate.ToString());
                if (success > 0)
                {
                    MessageBox.Show("修改成功");
                }
            }
            catch (Exception)
            {

                throw new Exception("修改出错，请重新操作");
            }

        }

        /// <summary>
        /// 关闭页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //foreach (DataGridViewRow dgvRow in this.dataGridView1.Rows)
            //{
            //    if (checkBox1.Checked)
            //    {
            //        this.dataGridView1.Rows[dgvRow.Index].Cells["ISCHECK"].Value = 1;
            //    }
            //    else
            //    {
            //        //this.dataGridView1.Rows[dgvRow.Index].Cells["ISCHECK"].Value = 0;
            //    }
            //    //GetfpInfo();
            //}
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    if (i==Convert.ToInt32(dataGridView1.CurrentCell.Value.ToString()))
            //    {
            //        UpZfbl = dataGridView1.Rows[i].Cells["自付比例"].Value.ToString();
            //        Uphiscode = dataGridView1.Rows[i].Cells["医院编码"].Value.ToString();
            //        Updylb = dataGridView1.Rows[i].Cells["费用类别"].Value.ToString();
            //    }
            UpZfbl = dataGridView1.CurrentRow.Cells["自付比例"].Value.ToString();
            Uphiscode = dataGridView1.CurrentRow.Cells["医院编码"].Value.ToString();
            Updylb = dataGridView1.CurrentRow.Cells["费用类别"].Value.ToString();

            txtZfbl.Text = UpZfbl;

            //}

            //for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            //{
            //    for (int j = 0; j < dataGridView1.Columns.Count; j++)
            //    {
            //        UpZfbl = dataGridView1.Rows[i].Cells[j].Value.ToString();
            //    }

            //}
        }

        private void txtmcbm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == 13)
            {
                if (txtmcbm.Text == "")
                {
                    MessageBox.Show("请输入名称或者编码！！");
                    this.txtmcbm.Select();
                    return;
                }
                else
                {
                    this.button1.Select();
                }
            }
        }
    }
}
