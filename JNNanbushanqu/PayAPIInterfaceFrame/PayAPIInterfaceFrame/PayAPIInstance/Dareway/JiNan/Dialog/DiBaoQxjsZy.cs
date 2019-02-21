using PayAPIInterface.ParaModel;
using PayAPIUtilities.Config;
using PayAPIUtilities.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PayAPIInstance.Dareway.JiNan.Dialog
{
    public partial class DiBaoQxjsZy : Form
    {
        /// <summary>
        /// 住院入参
        /// </summary>
        public InPayParameter inReimPara;

        public MSSQLHelper sqlHelper = new MSSQLHelper("Data Source=172.22.29.6;Initial Catalog=COMM;Persist Security Info=True;User ID=power;Password=m@ssunsoft009");

        public string hosId;
        public string hosOperatorName;
        public string hosOperatorSysid;
        public DiBaoQxjsZy(InPayParameter _inReimPara, string _hosId, string _hosOperatorName, string _hosOperatorSysid)
        {
            InitializeComponent();
            this.inReimPara = _inReimPara;
            this.hosId = _hosId;
            this.hosOperatorName = _hosOperatorName;
            this.hosOperatorSysid = _hosOperatorSysid;
        }

        private void DiBaoQxjsZy_Load(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            string strSql = "";
            //对于由于医保逆结算报错导致低保二次逆结算的遗留数据(上次低保已退费成功)
            strSql = @"SELECT * FROM REPORT.dbo.db_JSB_zy WHERE PAT_IN_HOS_ID='" + inReimPara.SettleInfo.PatInHosId + "' AND InvoiceId='" + inReimPara.SettleInfo.InNetworkSettleId+ "' ";// "' AND FLAG_INVALID=0 ";
            ds = sqlHelper.ExecSqlReDs(strSql);
            if (ds.Tables[0].Rows.Count == 0)
            {
                // MessageBox.Show("没有找到此人的住院低保结算记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                throw new Exception("没有找到此人的住院低保结算记录");
            }
            #region 对于由于医保逆结算报错导致低保二次逆结算的遗留数据(上次低保已退费成功),需重点测试InvoiceId对应的InNetworkSettleId是否可靠
            if (Convert.ToBoolean(ds.Tables[0].Rows[0]["FLAG_INVALID"].ToString()) == true)
            {
                this.Close();
                return;
            }
            #endregion
            //-----------------------------
            try
            {


                string strOrgId = PayAPIConfig.InstitutionDict[1].Memo;//调用方机构编号，保持跟交易明细视图中的机构编号一致；
                string strInvoiceId = ds.Tables[0].Rows[0]["invoiceId"].ToString();//上次收费的invoiceId
                string strIdCardNo = ds.Tables[0].Rows[0]["IDCardNo"].ToString();//当前居民身份证号码
                string strAidCardNo = ds.Tables[0].Rows[0]["SerialNo"].ToString();//当前救助卡编号
                float fTotalFee = -float.Parse(ds.Tables[0].Rows[0]["TotalFee"].ToString());//本次收费总金额
                float fSiPayment = float.Parse(ds.Tables[0].Rows[0]["SiPayment"].ToString());//本次收费医保支付金额（非医保情况请置为0）
                float fSiBaseline = float.Parse(ds.Tables[0].Rows[0]["SiBaseline"].ToString());//本次收费医保起付线金额（非医保情况请置为0）
                bool bSpecificOutpatient = Convert.ToBoolean(ds.Tables[0].Rows[0]["bSpecificOutpatient"].ToString()); //是否门规医保 model.outReimPara.RegInfo.NetPatType == "01" ? true : false


                LogManager.Info("_SdaCash 开始PAT_IN_HOS_ID:" + inReimPara.SettleInfo.PatInHosId + "strInvoiceId:" + strInvoiceId + "strIdCardNo:" + strIdCardNo + "strAidCardNo:" + strAidCardNo);

                int reInt;
                reInt = axSDACard1.SdaCash(strOrgId, strInvoiceId, strIdCardNo, strAidCardNo, fTotalFee, fSiPayment, fSiBaseline, bSpecificOutpatient);
                if (reInt != 0)
                {
                    LogManager.Info("_SdaCash 失败PAT_IN_HOS_ID:" + inReimPara.SettleInfo.PatInHosId + "strInvoiceId:" + strInvoiceId + "strIdCardNo:" + strIdCardNo + "strAidCardNo:" + strAidCardNo + "reInt:" + reInt);
                    MessageBox.Show("_SdaCash失败!错误号:" + reInt, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                strSql = "UPDATE REPORT.dbo.db_JSB_zy SET FLAG_INVALID=1,czybh_qx='" + hosOperatorSysid + "',czy_qx='" + hosOperatorName+ "',rq_qx=getdate() WHERE FLAG_INVALID=0 AND PAT_IN_HOS_ID='" + inReimPara.SettleInfo.PatInHosId + "' AND InvoiceId='" + strInvoiceId + "'";
               sqlHelper.ExecSqlReInt(strSql);

                MessageBox.Show("撤销结算成功");
            }
            catch (Exception ex)
            {
               throw new Exception("撤销结算失败,请重新进行撤销结算"+ex.ToString());
            }

        }
    }
}
