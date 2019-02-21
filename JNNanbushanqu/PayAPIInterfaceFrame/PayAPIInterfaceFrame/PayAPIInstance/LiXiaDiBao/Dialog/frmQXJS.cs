using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using PayAPIUtilities.Config;
using PayAPIUtilities.Log;

namespace PayAPIInstance.LiXiaDiBao.Dialog
{
    public partial class frmQXJS : Form
    {
        LiXiaDiBaoModel model;
        public frmQXJS(LiXiaDiBaoModel _model)
        {
            InitializeComponent();
            model = _model;
        }

        private void frmQXJS_Load(object sender, EventArgs e)
        {
            //------------------------退费时框架没有把登记OutPayPara.RegInfo信息没有取出来所以
            string strSql = "";
            DataSet ds = new DataSet();
            strSql = "SELECT MEMBER_NO,ID_NO FROM MZ.OUT.OUT_NETWORK_REGISTERS WHERE OUT_NETWORK_SETTLE_ID='" + model.outReimPara.SettleInfo.RelationId + "'";
            ds = model.sqlHelper.ExecSqlReDs(strSql);
            if (ds.Tables[0].Rows.Count < 1)
            {
                throw new Exception("没有找到联网登记信息！");
            }
            //-----------------------------

            string strOrgId = PayAPIConfig.InstitutionDict[1].Memo;//调用方机构编号，保持跟交易明细视图中的机构编号一致；
            string strInvoiceId = model.outReimPara.SettleInfo.SettleNo.ToString();//上次收费的invoiceId
            string strIdCardNo = ds.Tables[0].Rows[0]["ID_NO"].ToString();//当前居民身份证号码model.outReimPara.RegInfo.IdNo
            string strAidCardNo = ds.Tables[0].Rows[0]["MEMBER_NO"].ToString();//当前救助卡编号model.outReimPara.RegInfo.MemberNo
            float fTotalFee = -float.Parse(model.outReimPara.SettleInfo.Amount.ToString());//本次收费总金额
            float fSiPayment = 0;//本次收费医保支付金额（非医保情况请置为0）
            float fSiBaseline = 0;//本次收费医保起付线金额（非医保情况请置为0）
            bool bSpecificOutpatient = false; //是否门规医保 model.outReimPara.RegInfo.NetPatType == "01" ? true : false

            LogManager.Info("_SdaCash 开始OUT_PAT_ID:" + model.outReimPara.PatInfo.OutPatId + "strInvoiceId:" + strInvoiceId + "strIdCardNo:" + strIdCardNo + "strAidCardNo:" + strAidCardNo);

            int reInt;
            reInt = axSDACard1.SdaCash(strOrgId, strInvoiceId, strIdCardNo, strAidCardNo, fTotalFee, fSiPayment, fSiBaseline, bSpecificOutpatient);
            if (reInt != 0)
            {
                LogManager.Info("_SdaCash 失败OUT_PAT_ID:" + model.outReimPara.PatInfo.OutPatId + "strInvoiceId:" + strInvoiceId + "strIdCardNo:" + strIdCardNo + "strAidCardNo:" + strAidCardNo + "reInt:" + reInt);
                throw new Exception("_SdaCash失败!错误号:" + reInt);
            }

            strSql = "UPDATE REPORT.dbo.db_JSB SET FLAG_INVALID=1 WHERE OUT_NETWORK_SETTLE_ID='" + model.outReimPara.SettleInfo.RelationId + "'";
            model.sqlHelper.ExecSqlReInt(strSql);
        
            this.Close();
        }


        

        
        //-----------------------------------------------------
    }
}
