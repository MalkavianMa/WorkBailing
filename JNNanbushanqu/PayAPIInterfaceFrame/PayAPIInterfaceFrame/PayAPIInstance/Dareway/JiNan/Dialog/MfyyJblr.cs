using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DataGridSort;
using PayAPIInterface.Model.Out;

namespace PayAPIInstance.Dareway.JiNan.Dialog
{
    public partial class MfyyJblr : Form
    {
        /// <summary>
        /// 免费药物疾病编码
        /// </summary>
        public string MfyyJbbm = "";

        //是否取消
        public bool isCancel = true;

        JNDWInterfaceModel_MFYY model;

        public MfyyJblr()
        {
            InitializeComponent();
            
        }

        public MfyyJblr(string MfyySmS, JNDWInterfaceModel_MFYY _model)
        {
            InitializeComponent();
            richTextBox1.Text = MfyySmS;
            model = _model;
        }

        /// <summary>
        /// 确定按钮按钮选择疾病名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ok_Click(object sender, EventArgs e)
        {
            /*
            if (cmbJB.Text=="糖尿病")
            {
                MfyyJbbm = "E14.901 ,";
            }
            else if (cmbJB.Text == "原发性高血压")
            {
                MfyyJbbm = "I10  11 ,";
            }
            else if (cmbJB.Text == "冠心病")
            {
                MfyyJbbm = "I25.101 ,";
            }
            else
            {
                MessageBox.Show("请选择正确的疾病名称！");
            }*/

            if (cmbJB.Text == "")
            {
                MessageBox.Show("请选择正确的疾病名称！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cmbJB.Focus();
                return;
            }
            MfyyJbbm = cmbJB.SelectedValue.ToString() + " ,";
            isCancel = false;
            this.Close();
        }

        /// <summary>
        /// 关闭页面并退出结算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            isCancel = true;
            this.Close();
        }

        private void MfyyJblr_Load(object sender, EventArgs e)
        {
            //------加载疾病
            DataSet DS = new DataSet();
            string strSql = "SELECT '' AS jbbm,'' AS jbmc  UNION ALL SELECT jbbm,jbmc FROM REPORT.dbo.yb_MfyyJb";
            DS = JNDWInterfaceModel_MFYY.handelModel.sqlHelperHis.ExecSqlReDs(strSql);
            cmbJB.DataSource = DS.Tables[0];
            cmbJB.ValueMember = "jbbm";//值
            cmbJB.DisplayMember = "jbmc";//显示字段
            cmbJB.SelectedIndex = 0;
            DS.Tables.Clear();
            //-----------------------------
            for (int i = 0; i < model.outReimPara.Details.Count; i++)
            {
                if (model.outReimPara.Details[i].ChargeType < 100)
                {
                    model.outReimPara.Details[i].Memo4 = "药品";//药品
                }
                else
                {
                    model.outReimPara.Details[i].Memo4 = "医疗";//医疗
                }
            }
            dGV_FYSH.AutoGenerateColumns = false;//取消自动绑定列
            dGV_FYSH.DataSource = new BindingCollection<OutNetworkUpDetail>(model.outReimPara.Details);  //datagridview绑定List,点列表题排序
            dGV_FYSH.Refresh();
            //---------------------------------------
            try
            {
                DataSet ds = JNDWInterfaceModel_MFYY.handelModel.getMzJzxx(model.outReimPara.PatInfo.OutPatId.ToString(), model.outReimPara.CommPara.TradeId.ToString());
                tbZdbm.Text = ds.Tables[0].Rows[0]["DIAGNOSIS_CODE"].ToString();
                tbZdmc.Text = ds.Tables[0].Rows[0]["DIAGNOSIS_NAME"].ToString();
                cmbJB.SelectedValue = ds.Tables[0].Rows[0]["DIAGNOSIS_CODE"].ToString();
            }
            catch(Exception ex)
            {
            }
        }
        //------------------------------
    }
}
