using PayAPIInterface.ParaModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PayAPIInstance.Dareway.JiNan.Dialog
{
    public partial class DiBaoJS_Confirm_zy : Form
    {

        /// <summary>
        /// 医院ID
        /// </summary>
        public string hosid = "";
        public InPayParameter inReimPara;
        //低保结算信息
        public Dictionary<string, string> dicSettleInfoDibao;
        /// <summary>
        /// 结算信息
        /// </summary>
        public Dictionary<string, string> dicSettleInfo;
        //public DiBaoJS_Confirm_zy()
        //{
        //    InitializeComponent();
        //}

        public bool isCancel = false;

        /// <summary>
        /// 医院操作员姓名
        /// </summary>
        public string hosOperatorName = "";

        /// <summary>
        /// 医院操作员usersysid
        /// </summary>
        public string hosOperatorSysid = "";

        public DiBaoJS_Confirm_zy(InPayParameter _inReimPara, Dictionary<string, string> _dicSettleInfo, Dictionary<string, string> _dicSettleInfoDibao, string hosID, string hosOperatorName, string hosOperatorSysid)
        {
            isCancel = false;

            InitializeComponent();
            inReimPara = _inReimPara;
            dicSettleInfo = _dicSettleInfo;
            dicSettleInfoDibao = _dicSettleInfoDibao;
            this.hosid = hosID;
            this.hosOperatorName = hosOperatorName;
            this.hosOperatorSysid = hosOperatorSysid;
            lblTs.Text = "医保结算完成，病人还需要支付" + Math.Round(Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]), 2) + "元，请选择是否进行低保结算?";
        }


        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                //进入窗体前需先修改连接字符串
                DiBaoJS_zy diBaoJS = new DiBaoJS_zy(inReimPara, dicSettleInfo, dicSettleInfoDibao, hosid, hosOperatorSysid, hosOperatorName);
                diBaoJS.ShowDialog();
                if (diBaoJS.isCancel)
                {
                    isCancel = true;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                //是否throw 需撤销医保
                MessageBox.Show("低保结算失败！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //isCancel = true;
            this.Close();
        }

        private void DiBaoJS_Confirm_zy_Load(object sender, EventArgs e)
        {
            isCancel = false;
        }
    }
}
