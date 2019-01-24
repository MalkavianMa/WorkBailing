using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DW_YBBX.ZX_Business
{
    public partial class FrmInvalidQuery : Form
    {
        public MSSQLHelpers SQLHelper = new MSSQLHelpers(ConfigurationManager.ConnectionStrings["connStr"].ToString());

        public FrmInvalidQuery()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Gzx();
        }

        private void Gzx()
        {
            StringBuilder StrSql = new StringBuilder();

            StrSql.Append(" SELECT ");
            //   StrSql.Append(" [ITEM_PROP]");
            StrSql.Append(" [HIS_ITEM_CODE]");
            StrSql.Append(" ,[HIS_ITEM_NAME]  AS  医院项目名称");
            StrSql.Append("  ,[NETWORK_ITEM_CODE] as  医保项目编码");
            StrSql.Append("    ,[NETWORK_ITEM_NAME] as  中心名称");
            StrSql.Append("   ,[SELF_BURDEN_RATIO] ");
            StrSql.Append("   ,[MEMO]");
            StrSql.Append("   ,[START_TIME]  as  启用时间");
            StrSql.Append("   ,[STOP_TIME]  as  停用时间");
            StrSql.Append("   ,[TYPE_MEMO]");
            //   StrSql.Append("   ,[NETWORK_ITEM_PROP]");
            StrSql.Append("  ,[NETWORK_ITEM_CHARGE_CLASS]");
            StrSql.Append("   ,[HOSPITAL_ID]");
            StrSql.Append("    ,[AUTO_ID] ");

            StrSql.Append("    ,[NETWORK_ITEM_PRICE] as 联网价格");
            //  StrSql.Append("   ,[FLAG_DISABLED]");
            //    StrSql.Append("   ,[NETWORK_ITEM_FLAG_UP]");
            StrSql.Append(" FROM [COMM].[COMM].[NETWORKING_ITEM_VS_HIS] ");

            StrSql.Append("  where  FLAG_DISABLED in('1')  ");
            StrSql.Append("  AND (HIS_ITEM_CODE LIKE '%" + txt_hisxm.Text + "%' OR HIS_ITEM_NAME LIKE '%" + txt_hisxm.Text + "%'  )  ORDER BY HIS_ITEM_CODE ");

            DataSet ds = new DataSet();
            DataTable zxdata = new DataTable();
            ds = SQLHelper.ExecSqlReDs(StrSql.ToString());
            zxdata = ds.Tables[0];
            dgv_his.DataSource = zxdata;
        }
    }
}
