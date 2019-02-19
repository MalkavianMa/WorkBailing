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
    public partial class AddHisCode : Form
    {
        public MSSQLHelpers sqlHelper = new MSSQLHelpers(ConfigurationManager.ConnectionStrings["connStr"].ToString());

        /// <summary>
        /// 费用类别
        /// </summary>
        public string fylb = "";

        /// <summary>
        /// 项目类别
        /// </summary>
        public string xmlb = "";

        /// <summary>
        /// 类别
        /// </summary>
        public string memo = "";

        public AddHisCode()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 添加按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
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

            string networktypeprop = "";
            //if (cmbfylb.Text == "测试")
            //{
            //    fylb = "10004";
            //}
            string networkchargeClass = "";
            if (cmbxmlx.Text == "药品")
            {
                xmlb = "1";
                networktypeprop = "1";
                networkchargeClass = "1";
            }
            else if (cmbxmlx.Text == "诊疗")
            {
                xmlb = "2";
                networktypeprop = "2";
                networkchargeClass = "";
            }
            else if (cmbxmlx.Text == "材料")
            {
                xmlb = "1";
                networktypeprop = "1";
                networkchargeClass = "3";
            }
            if (txtzfbl.Text =="0")
            {
                memo = "甲";
            }
            else if (txtzfbl.Text != "0" && txtzfbl.Text != "100")
            {
                memo = "乙";
            }
            else if (txtzfbl.Text == "100")
            {
                memo = "丙";
            }


            if (string.IsNullOrEmpty(txtHiscode.Text)||string.IsNullOrEmpty(txtHisname.Text)||string.IsNullOrEmpty(txtZxCode.Text)||string.IsNullOrEmpty(MainForm.HOSPITAL_ID))
            {
                MessageBox.Show("不能添加或插入空值！");
                return;
            }
            //插入之前判断重复
            StringBuilder strquchong = new StringBuilder();
            strquchong.Append("select  * from   COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID=" + fylb + "  and  HIS_ITEM_CODE= '" + txtHiscode.Text + "' and HOSPITAL_ID= " + MainForm.HOSPITAL_ID);
            //WHERE NETWORKING_PAT_CLASS_ID='4' AND HIS_ITEM_CODE='110900001c'  and   HOSPITAL_ID ='8001'
            if (sqlHelper.ExecSqlReDs(strquchong.ToString()).Tables[0].Rows.Count>0)
            {
                MessageBox.Show("该类别的HIS编码已有一条对应关系，无法添加");
                return;
            }



            StringBuilder strAdd = new StringBuilder();
            strAdd.Append(" INSERT INTO COMM.COMM.NETWORKING_ITEM_VS_HIS ");
            strAdd.Append("( NETWORKING_PAT_CLASS_ID, ");
            strAdd.Append(" ITEM_PROP, ");
            strAdd.Append(" HIS_ITEM_CODE, ");
            strAdd.Append(" HIS_ITEM_NAME, ");
            strAdd.Append(" NETWORK_ITEM_CODE, ");
            strAdd.Append(" NETWORK_ITEM_NAME, ");
            strAdd.Append(" SELF_BURDEN_RATIO, ");
            strAdd.Append(" MEMO, ");
            strAdd.Append(" START_TIME, ");
            strAdd.Append(" STOP_TIME, ");
            strAdd.Append(" TYPE_MEMO, ");
            strAdd.Append(" NETWORK_ITEM_PROP, ");
            strAdd.Append(" NETWORK_ITEM_CHARGE_CLASS, ");
            strAdd.Append(" HOSPITAL_ID, ");
            strAdd.Append(" NETWORK_ITEM_PRICE, ");
            strAdd.Append(" FLAG_DISABLED, ");
            strAdd.Append(" NETWORK_ITEM_FLAG_UP ");
            strAdd.Append(" ) ");
            strAdd.Append(" VALUES( ");
            strAdd.Append(" '" + fylb + "', ");
            strAdd.Append(" '" + xmlb + "', ");
            strAdd.Append(" '" + txtHiscode.Text + "', ");
            strAdd.Append(" '" + txtHisname.Text + "', ");
            strAdd.Append(" '" + txtZxCode.Text + "', ");
            strAdd.Append(" '" + txtZxname.Text + "', ");
            strAdd.Append(" '" + txtzfbl.Text + "', ");
            strAdd.Append(" '" + memo + "', ");
            strAdd.Append(" '" + DateTime.Now + "', ");
            strAdd.Append(" '" + DateTime.Now + "', ");
            strAdd.Append(" '" + memo + "', ");
            strAdd.Append(" '" + networktypeprop + "', ");
            strAdd.Append(" '" + networkchargeClass + "', ");
            strAdd.Append(" '"+MainForm.HOSPITAL_ID+"', ");
            strAdd.Append(" '" + txtPrice.Text + "', ");
            strAdd.Append(" '0', ");
            strAdd.Append(" '1' ");
            strAdd.Append(" ) ");

            int suuccess = sqlHelper.ExecSqlReInt(strAdd.ToString());
            if (suuccess>0)
            {
                MessageBox.Show("添加成功");
            }
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void AddHisCode_Load(object sender, EventArgs e)
        {
            ArrayList arr1 = new ArrayList();
            //arr1.Add(new DictionaryEntry("", ""));
            //arr1.Add(new DictionaryEntry("7", "职工住院"));
            //arr1.Add(new DictionaryEntry("8", "居民住院"));
            //arr1.Add(new DictionaryEntry("10", "职工统筹"));
            //arr1.Add(new DictionaryEntry("11", "居民统筹"));
            //arr1.Add(new DictionaryEntry("12", "免费用药"));
            //arr1.Add(new DictionaryEntry("13", "职工普通门诊"));
            //arr1.Add(new DictionaryEntry("10004", "测试"));
            arr1.Add(new DictionaryEntry("2", "居民统筹"));
            arr1.Add(new DictionaryEntry("1", "居民门规"));
            arr1.Add(new DictionaryEntry("3", "普通门诊"));
            arr1.Add(new DictionaryEntry("4", "职工门规"));
            arr1.Add(new DictionaryEntry("5", "免费药品"));
            arr1.Add(new DictionaryEntry("6", "职工统筹"));

            cmbfylb.DataSource = arr1;
            cmbfylb.DisplayMember = "Value";
            cmbfylb.ValueMember = "Key";
            cmbfylb.SelectedIndex = 0;

            ArrayList arr2 = new ArrayList();
            arr2.Add(new DictionaryEntry("", ""));
            arr2.Add(new DictionaryEntry("1", "药品"));
            arr2.Add(new DictionaryEntry("2", "诊疗"));
            arr2.Add(new DictionaryEntry("3", "材料"));
            cmbxmlx.DataSource = arr2;
            cmbxmlx.DisplayMember = "Value";
            cmbxmlx.ValueMember = "Key";
            cmbxmlx.SelectedIndex = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ZX_Business.SelectDelete frm = new SelectDelete();
            frm.Show();
        }
    }
}
