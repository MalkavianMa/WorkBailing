using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DW_YBBX.ZX_Business;

namespace DW_YBBX.CONFIG
{
    public partial class frmFlagAdd : Form
    {
        public frmFlagAdd()
        {
            InitializeComponent();
        }
        public MSSQLHelpers SQLHelper = new MSSQLHelpers(ConfigurationManager.ConnectionStrings["connStr"].ToString());

        private void button1_Click(object sender, EventArgs e)
        {

            string strHisCode = this.tbxyyxmbm.Text.Trim();
            string strHisName = this.tbxyyxmm.Text.Trim();
            string strZXCode = this.tbxCenterbm.Text.Trim();
            string strZXName = this.tbxcenterName.Text.Trim();
            string strTypeMemoCgHIScode = "";
            string crcgName = "";
            //  SELECT * FROM		[COMM].[COMM].[NETWORKING_ITEM_VS_HIS] WHERE   [HIS_ITEM_CODE]=''
            StringBuilder SqlstringQc = new StringBuilder();
            SqlstringQc.Append("SELECT * FROM		[COMM].[COMM].[NETWORKING_ITEM_VS_HIS] WHERE   HIS_ITEM_CODE='" + strHisCode + "'");
            DataSet dtc = SQLHelper.ExecSqlReDs(SqlstringQc.ToString());
            if (dtc.Tables[0].Rows.Count >= 1)
            {
                MessageBox.Show("该编码已存在！");
                return;
            }



            StringBuilder Sqlstring = new StringBuilder();
         

            Sqlstring.Append("INSERT INTO COMM.COMM.NETWORKING_ITEM_VS_HIS");
            Sqlstring.Append("(NETWORKING_PAT_CLASS_ID,");
            Sqlstring.Append("ITEM_PROP,");
            Sqlstring.Append("HIS_ITEM_CODE,");
            Sqlstring.Append("HIS_ITEM_NAME,");
            Sqlstring.Append("NETWORK_ITEM_CODE,");
            Sqlstring.Append("NETWORK_ITEM_NAME,");
            Sqlstring.Append("SELF_BURDEN_RATIO,");
            Sqlstring.Append("MEMO,");
            Sqlstring.Append("START_TIME,");
            Sqlstring.Append("STOP_TIME,");
            Sqlstring.Append("TYPE_MEMO,");
            Sqlstring.Append("NETWORK_ITEM_PROP,");
            Sqlstring.Append("NETWORK_ITEM_CHARGE_CLASS,");
            Sqlstring.Append("HOSPITAL_ID,");
            Sqlstring.Append("NETWORK_ITEM_PRICE,");
            Sqlstring.Append("FLAG_DISABLED,");
            Sqlstring.Append("NETWORK_ITEM_FLAG_UP");
            Sqlstring.Append(")");
            Sqlstring.Append("VALUES( 3,");
            Sqlstring.Append(" '" + 9 + "',");
            Sqlstring.Append(" '" + strHisCode + "',");
            Sqlstring.Append(" '" + strHisName + "',");

            if (string.IsNullOrEmpty(this.cbotype.Text) || string.IsNullOrEmpty(strHisCode) || string.IsNullOrEmpty(strHisName))
            {
               // strHisCode = this.cbotype.SelectedValue.ToString();
                MessageBox.Show("请选择项目类型!医院项目码医院项目名必填!");
                return;
            }
            //此处篡改中心码
            if (this.cbotype.SelectedValue.ToString()=="1017-4")
            {
                strZXCode = "210485";
                strZXName = "自费诊疗项目";
                Sqlstring.Append(" '" + strZXCode + "',");
                //中心名无所谓
                Sqlstring.Append(" '" + strZXName + "',");
                strTypeMemoCgHIScode = "1017-4";
              //  crcgName = "锂测定（离子选择电极法）";
            }
            if (this.cbotype.SelectedValue.ToString() == "30401")
            {
                strZXCode = "206024";
                strZXName = "（新）自费一次性材料";
                Sqlstring.Append(" '" + strZXCode + "',");
                //中心名无所谓
                Sqlstring.Append(" '" + strZXName + "',");
                strTypeMemoCgHIScode = "30401";
             //   crcgName = "C高频手术电刀柄";
            }
            if (this.cbotype.SelectedValue.ToString() == "999")
            {
                Frmaddfalg fflag = new Frmaddfalg();
               
                fflag.ShowDialog();
                strZXCode = fflag.thzxbm;
                strZXName = fflag.thzxbmc;
                Sqlstring.Append(" '" + strZXCode + "',");
                //中心名无所谓
                Sqlstring.Append(" '" + strZXName + "',");
                strTypeMemoCgHIScode = fflag.thhisbm;

                
                
            }

            Sqlstring.Append(" '" + 100 + "',");
            Sqlstring.Append(" '审批已通过',"); //国药
            Sqlstring.Append(" GETDATE(),");
            Sqlstring.Append(" GETDATE(),");

            Sqlstring.Append(" '" + strTypeMemoCgHIScode + "',");//此处写入篡改的编码 对应his
            Sqlstring.Append(" '" + 0 + "',");
            //此处写入篡改的名称
            Sqlstring.Append("'" + crcgName + "',");
            Sqlstring.Append(" 1,");
            Sqlstring.Append(" '" + 0.0000 + "',");
            Sqlstring.Append(" 1, ");
            Sqlstring.Append(" 1 ");
            Sqlstring.Append(" ) ");


            int Num = SQLHelper.ExecSqlReInt(Sqlstring.ToString());
            if (Num > 0)
            {
                MessageBox.Show("添加成功！");
                return;
            }
        }

        private void frmFlagAdd_Load(object sender, EventArgs e)
        {
            ArrayList arraylist1 = new ArrayList();
            arraylist1.Add(new DictionaryEntry("2", ""));
            arraylist1.Add(new DictionaryEntry("1017-4", "医疗"));
            arraylist1.Add(new DictionaryEntry("30401", "材料"));
            arraylist1.Add(new DictionaryEntry("999", "自定义"));

            //arraylist1.Add(new DictionaryEntry("1".AppSettings["JMMG_PATID"].ToString(), "居民门规"));
            //arraylist1.Add(new DictionaryEntry(ConfigurationManager.AppSettings["PTMZ_PATID"].ToString(), "普通门诊"));
            //arraylist1.Add(new DictionaryEntry(ConfigurationManager.AppSettings["ZGMG_PATID"].ToString(), "职工门规"));
            //arraylist1.Add(new DictionaryEntry(ConfigurationManager.AppSettings["MFYY_PATID"].ToString(), "免费药品"));
            //arraylist1.Add(new DictionaryEntry(ConfigurationManager.AppSettings["ZGTC_PATID"].ToString(), "职工统筹"));




            this.cbotype.DataSource = arraylist1;
            this.cbotype.ValueMember = "Key";
            this.cbotype.DisplayMember = "Value";
            this.cbotype.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string strHisCode = this.tbxyyxmbm.Text.Trim();

            StringBuilder SqlstringQc = new StringBuilder();
            SqlstringQc.Append("DELETE    FROM		[COMM].[COMM].[NETWORKING_ITEM_VS_HIS] WHERE   HIS_ITEM_CODE='" + strHisCode + "'  AND     flag_disabled='1' ");
          //  DataSet dtc = SQLHelper.ExecSqlReDs(SqlstringQc.ToString());
            int Num = SQLHelper.ExecSqlReInt(SqlstringQc.ToString());

            if (Num >= 1)
            {
                MessageBox.Show("删除成功！");
                return;
            }
        }
    }
}
