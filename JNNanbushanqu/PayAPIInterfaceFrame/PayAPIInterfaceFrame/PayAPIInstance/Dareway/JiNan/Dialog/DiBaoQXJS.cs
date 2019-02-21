using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using PayAPIUtilities.Config;
using PayAPIUtilities.Log;

namespace PayAPIInstance.Dareway.JiNan.Dialog
{
    public partial class DiBaoQXJS : Form
    {
        public MSSQLHelper sqlHelper = new MSSQLHelper("Data Source=172.22.29.6;Initial Catalog=COMM;Persist Security Info=True;User ID=power;Password=m@ssunsoft009");
        decimal OUT_NETWORK_SETTLE_ID;
        public DiBaoQXJS(decimal _OUT_NETWORK_SETTLE_ID)
        {
            InitializeComponent();
            OUT_NETWORK_SETTLE_ID = _OUT_NETWORK_SETTLE_ID;
        }

        private void DiBaoQXJS_Load(object sender, EventArgs e)
        {
            //------------------------退费时框架没有把登记OutPayPara.RegInfo信息没有取出来所以
            string strSql = "";
            DataSet ds = new DataSet();
            strSql = "SELECT InvoiceId,SerialNo,IDCardNo,TotalFee,SiPayment,SiBaseline,OUT_PAT_ID,FLAG_INVALID,bSpecificOutpatient FROM REPORT.dbo.db_JSB WHERE OUT_NETWORK_SETTLE_ID='" + OUT_NETWORK_SETTLE_ID + "'";
            ds = sqlHelper.ExecSqlReDs(strSql);
            if (ds.Tables[0].Rows.Count < 1)
            {
                throw new Exception("没有低保结算信息！");
            }
            if (Convert.ToBoolean(ds.Tables[0].Rows[0]["FLAG_INVALID"].ToString()) == true)
            {
                this.Close();
                return;
            }

            //-----------------------------

            string strOrgId = PayAPIConfig.InstitutionDict[1].Memo;//调用方机构编号，保持跟交易明细视图中的机构编号一致；
            string strInvoiceId = ds.Tables[0].Rows[0]["invoiceId"].ToString();//上次收费的invoiceId
            string strIdCardNo = ds.Tables[0].Rows[0]["IDCardNo"].ToString();//当前居民身份证号码
            string strAidCardNo = ds.Tables[0].Rows[0]["SerialNo"].ToString();//当前救助卡编号
            float fTotalFee = -float.Parse(ds.Tables[0].Rows[0]["TotalFee"].ToString());//本次收费总金额
            float fSiPayment = float.Parse(ds.Tables[0].Rows[0]["SiPayment"].ToString());//本次收费医保支付金额（非医保情况请置为0）
            float fSiBaseline = float.Parse(ds.Tables[0].Rows[0]["SiBaseline"].ToString());//本次收费医保起付线金额（非医保情况请置为0）
            bool bSpecificOutpatient =Convert.ToBoolean(ds.Tables[0].Rows[0]["bSpecificOutpatient"].ToString()); //是否门规医保 model.outReimPara.RegInfo.NetPatType == "01" ? true : false

            LogManager.Info("_SdaCash 开始OUT_PAT_ID:" + ds.Tables[0].Rows[0]["OUT_PAT_ID"].ToString() + "strInvoiceId:" + strInvoiceId + "strIdCardNo:" + strIdCardNo + "strAidCardNo:" + strAidCardNo);

            int reInt;
            reInt = axSDACard1.SdaCash(strOrgId, strInvoiceId, strIdCardNo, strAidCardNo, fTotalFee, fSiPayment, fSiBaseline, bSpecificOutpatient);
            if (reInt != 0)
            {
                LogManager.Info("_SdaCash 失败OUT_PAT_ID:" + ds.Tables[0].Rows[0]["OUT_PAT_ID"].ToString() + "strInvoiceId:" + strInvoiceId + "strIdCardNo:" + strIdCardNo + "strAidCardNo:" + strAidCardNo + "reInt:" + reInt);
                throw new Exception("_SdaCash失败!错误号:" + reInt);
            }

            strSql = "UPDATE REPORT.dbo.db_JSB SET FLAG_INVALID=1 WHERE OUT_NETWORK_SETTLE_ID='" + OUT_NETWORK_SETTLE_ID + "'";
            sqlHelper.ExecSqlReInt(strSql);

            this.Close();
        }


        

        
        
        //-----------------------------------------------------
    }
}
