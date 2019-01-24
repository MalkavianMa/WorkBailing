using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Configuration;
namespace DW_YBBX.ZX_Business
{
    public partial class FreeEdit : Form
    {
        public MSSQLHelpers SQLHelper = new MSSQLHelpers(ConfigurationManager.ConnectionStrings["connStr"].ToString());

        public FreeEdit()
        {
            InitializeComponent();
        }

        private void btn_His_CX_Click(object sender, EventArgs e)
        {
            gb();
        }

        private void gb()
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

            StrSql.Append("  where  memo in('审批已通过','未审批')  ");
            StrSql.Append("  AND (HIS_ITEM_CODE LIKE '%" + txt_hisxm.Text + "%' OR HIS_ITEM_NAME LIKE '%" + txt_hisxm.Text + "%'  )  ORDER BY HIS_ITEM_CODE ");

            DataSet ds = new DataSet();
            DataTable zxdata = new DataTable();
            ds = SQLHelper.ExecSqlReDs(StrSql.ToString());
            zxdata = ds.Tables[0];
            dataGridView1.DataSource = zxdata;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //string strZXName = dataGridView1.["医保项目名称"].Value.ToString();
            try
            {


                DataGridViewRow dgrZX = this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index];
                string autoID = dgrZX.Cells["AUTO_ID"].Value.ToString();
                string strZXName = dgrZX.Cells["医院项目名称"].Value.ToString();
                string strZXCode = dgrZX.Cells["医保项目编码"].Value.ToString();
                string strhiscode = dgrZX.Cells["HIS_ITEM_CODE"].Value.ToString();

                if (e.ColumnIndex==2||e.ColumnIndex==1)
                {
                    if (e.ColumnIndex==2)
                    {
                        StringBuilder strip = new StringBuilder();
                        strip.Append("update  COMM.COMM.NETWORKING_ITEM_VS_HIS set NETWORK_ITEM_CODE='" + strZXCode + "'   where  HIS_ITEM_CODE='" + strhiscode + "'  and  AUTO_ID='" + autoID + "'");
                        SQLHelper.ExecSqlReInt(strip.ToString());
                        gb();

                        frmTip ti = new frmTip();
                        ti.ShowDialog();

                    }
                    if (e.ColumnIndex==1)
                    {
                        StringBuilder strip = new StringBuilder();
                        strip.Append("update  COMM.COMM.NETWORKING_ITEM_VS_HIS set HIS_ITEM_NAME='" + strZXName + "'   where  HIS_ITEM_CODE='" + strhiscode + "'  and  AUTO_ID='" + autoID + "'");
                        SQLHelper.ExecSqlReInt(strip.ToString());
                        gb();
                        frmTip ti = new frmTip();
                        ti.ShowDialog();
                    }
                    return;
                }
                else
                {
                    MessageBox.Show("只可修改医院项目名称或医保项目编码！其他修改无效");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Environment.Exit(0);

                throw ex;
            }
            finally
            {
                gb();

            }
        }
    }
}
