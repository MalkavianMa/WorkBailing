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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        public MSSQLHelpers SQLHelper = new MSSQLHelpers(ConfigurationManager.ConnectionStrings["connStr"].ToString());

        private void button1_Click(object sender, EventArgs e)
        {

            //去除重复数据
            GX();
        }

        public List<string> yjqch = new List<string>();
        private void GX()
        {
            StringBuilder sqlStr1 = new StringBuilder();
            sqlStr1.Append("   select     *  FROM COMM.COMM.NETWORKING_ITEM_VS_HIS ");
            sqlStr1.Append("  WHERE   HIS_ITEM_CODE IN ( SELECT   HIS_ITEM_CODE ");
            sqlStr1.Append("  FROM     COMM.COMM.NETWORKING_ITEM_VS_HIS ");
            sqlStr1.Append("  WHERE    HOSPITAL_ID = '1' ");
            sqlStr1.Append("  AND NETWORKING_PAT_CLASS_ID='3' ");

            sqlStr1.Append("   GROUP BY HIS_ITEM_CODE ");
            sqlStr1.Append("   HAVING COUNT(HIS_ITEM_CODE) > 1 ");
            sqlStr1.Append("  ) ");
            sqlStr1.Append("AND HOSPITAL_ID = '1' ");
            sqlStr1.Append("   AND NETWORKING_PAT_CLASS_ID='3';");

            DataSet ds = new DataSet();
            DataTable hisdata = new DataTable();
            ds = SQLHelper.ExecSqlReDs(sqlStr1.ToString());
            hisdata = ds.Tables[0];
            dgv_his.DataSource = hisdata;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            DataGridViewRow dgrHis = this.dgv_his.Rows[this.dgv_his.CurrentRow.Index];
            string strHisNAME = dgrHis.Cells["HIS_ITEM_NAME"].Value.ToString();
            string STRHISCODE = dgrHis.Cells["HIS_ITEM_CODE"].Value.ToString();
            string strnetworkCode = dgrHis.Cells["NETWORK_ITEM_CODE"].Value.ToString();
            StringBuilder Sqlstring = new StringBuilder();
            ///  Sqlstring.Append(" DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE AUTO_ID='" + strHisID + "' AND NETWORKING_PAT_CLASS_ID='3 '  AND  HIS_ITEM_CODE='" + STRHISCODE + "' AND   ");
            //Sqlstring.Append("  SELECT * FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE   NETWORKING_PAT_CLASS_ID='3 '  AND  HIS_ITEM_CODE='" + STRHISCODE + "' AND NETWORK_ITEM_CODE='" + strnetworkCode + "'  AND  HIS_ITEM_NAME='" + strHisNAME + "'");

            ////Sqlstring.Append(" SELECT * FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE   NETWORKING_PAT_CLASS_ID='3 '  AND  HIS_ITEM_CODE='" + STRHISCODE + "' AND NETWORK_ITEM_CODE=" + strnetworkCode);
            //DataSet dt = SQLHelper.ExecSqlReDs(Sqlstring.ToString());

            Sqlstring.Append(" DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE   NETWORKING_PAT_CLASS_ID='3 '  AND  HIS_ITEM_CODE='" + STRHISCODE + "' AND NETWORK_ITEM_CODE='" + strnetworkCode + "'  AND  HIS_ITEM_NAME='" + strHisNAME + "'");
            yjqch.Add(Sqlstring.ToString());
            int Num = SQLHelper.ExecSqlReInt(Sqlstring.ToString());
            if (Num > 0)
            {
                MessageBox.Show("取消对照成功！");
                this.dgv_his.Rows.RemoveAt(this.dgv_his.CurrentRow.Index);
                GX();
                return;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GX();

        }
        string chq = "";
        private void button3_Click(object sender, EventArgs e)
        {
            chq = "";
            if (yjqch.Count > 0)
            {
                //循环删除
                foreach (var item in yjqch)
                {
                    chq += item + "\t";
                    this.richTextBox1.Text += item;
                }

                // int Num = SQLHelper.ExecSqlReInt(Sqlstring.ToString());

            }
            string bb = " DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE   NETWORKING_PAT_CLASS_ID='3 '  AND  HIS_ITEM_CODE='1006' AND NETWORK_ITEM_CODE='00206156'  AND  HIS_ITEM_NAME='生化全项'	 DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE   NETWORKING_PAT_CLASS_ID='3 '  AND  HIS_ITEM_CODE='20110' AND NETWORK_ITEM_CODE='ZA12BY05160000'  AND  HIS_ITEM_NAME='Y速效救心丸'	 DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE   NETWORKING_PAT_CLASS_ID='3 '  AND  HIS_ITEM_CODE='40528-03' AND NETWORK_ITEM_CODE='202837'  AND  HIS_ITEM_NAME='抗链球菌溶血素O测定(ASO)'	 DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE   NETWORKING_PAT_CLASS_ID='3 '  AND  HIS_ITEM_CODE='40548' AND NETWORK_ITEM_CODE='00205894'  AND  HIS_ITEM_NAME='家庭病床建床费'	 DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE   NETWORKING_PAT_CLASS_ID='3 '  AND  HIS_ITEM_CODE='5002-233' AND NETWORK_ITEM_CODE='210102015z00'  AND  HIS_ITEM_NAME='L胸部正侧位'	 DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE   NETWORKING_PAT_CLASS_ID='3 '  AND  HIS_ITEM_CODE='B384' AND NETWORK_ITEM_CODE='ZFYP0000'  AND  HIS_ITEM_NAME='妇炎洁抑洗液'	 DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE   NETWORKING_PAT_CLASS_ID='3 '  AND  HIS_ITEM_CODE='5002-23' AND NETWORK_ITEM_CODE='2014030546'  AND  HIS_ITEM_NAME='胸部正侧位'	";
            string[] strs = bb.Split(new char[] { '\t', '\t' });
        }
    }
}
