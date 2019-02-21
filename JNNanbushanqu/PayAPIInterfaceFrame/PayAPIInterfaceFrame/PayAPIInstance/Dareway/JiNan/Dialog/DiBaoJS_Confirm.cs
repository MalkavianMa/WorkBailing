using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using PayAPIInterface.ParaModel;

namespace PayAPIInstance.Dareway.JiNan.Dialog
{
    public partial class DiBaoJS_Confirm : Form
    {

        public string hosid = "";
        public OutPayParameter outReimPara;
        //低保结算信息
        public Dictionary<string, string> dicSettleInfoDibao;
        public Dictionary<string, string> dicSettleInfo;
        public DiBaoJS_Confirm(OutPayParameter _outReimPara, Dictionary<string, string> _dicSettleInfo, Dictionary<string, string> _dicSettleInfoDibao,string  hosID)
        {
            InitializeComponent();
            outReimPara = _outReimPara;
            dicSettleInfo = _dicSettleInfo;
            dicSettleInfoDibao = _dicSettleInfoDibao;
            this.hosid = hosID;
            lblTs.Text = "医保结算完成，病人还需要支付" + Math.Round(Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]), 2) + "元，请选择是否进行低保结算?";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                //进入窗体前需先修改连接字符串
                DiBaoJS diBaoJS = new DiBaoJS(outReimPara, dicSettleInfo, dicSettleInfoDibao,hosid);
                diBaoJS.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("低保结算失败！"+ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
        }

        private void DiBaoJS_Confirm_Load(object sender, EventArgs e)
        {

        }
    }
}
