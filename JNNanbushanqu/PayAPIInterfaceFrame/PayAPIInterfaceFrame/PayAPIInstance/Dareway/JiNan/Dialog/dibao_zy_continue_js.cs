using PayAPIInstance.tools;
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
    public partial class dibao_zy_continue_js : Form
    {
        /// <summary>
        /// 是否取消
        /// </summary>
        public bool isCancel = false;
        public InPayParameter inReimPara;
        public Dictionary<string, string> dicSettleInfo;
        public Dictionary<string, string> dicSettleInfoDibao;
        public string hosid;
        public string hosOperatorSysid;
        public string hosOperatorName;
        public DibaoSdaCash dsc = new DibaoSdaCash();

        public MSSQLHelper sqlHelper = new MSSQLHelper("Data Source=172.22.29.6;Initial Catalog=COMM;Persist Security Info=True;User ID=power;Password=m@ssunsoft009");


        public dibao_zy_continue_js()
        {
            InitializeComponent();

        }

        public dibao_zy_continue_js(InPayParameter inReimPara, Dictionary<string, string> dicSettleInfo, Dictionary<string, string> dicSettleInfoDibao, string hosid, string hosOperatorSysid, string hosOperatorName, DibaoSdaCash dsc)
        {
            InitializeComponent();
            this.dsc = dsc;
            // TODO: Complete member initialization
            this.inReimPara = inReimPara;
            this.dicSettleInfo = dicSettleInfo;
            this.dicSettleInfoDibao = dicSettleInfoDibao;
            this.hosid = hosid;
            this.hosOperatorSysid = hosOperatorSysid;
            this.hosOperatorName = hosOperatorName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            QXJS();
            isCancel = true;

            this.Close();
        }

        /// <summary>
        /// 低保结算
        /// </summary>
        private void JS()
        {
            decimal amount = 0;
            amount = Convert.ToDecimal(dicSettleInfo["brfdje"]) + Convert.ToDecimal(dicSettleInfo["ybfdje"]) + Convert.ToDecimal(dicSettleInfo["yljmje"]);       //本次医疗费用
            string strOrgId = PayAPIConfig.InstitutionDict[1].Memo;//调用方机构编号，保持跟交易明细视图中的机构编号一致；
            string strInvoiceId = inReimPara.CommPara.InNetworkSettleId.ToString();//本次收费单ID
            string strIdCardNo = dsc.tbIDCardNo;//当前居民身份证号码
            string strAidCardNo = dsc.tbSerialNo;//当前救助卡编号
            float fTotalFee = 0;
            float fSiPayment = 0;
            #region 根据实际情况决定是否需要判断  还是直接注掉
            if (float.Parse(tbYbzf1.Text == "" ? "0" : tbYbzf1.Text) + float.Parse(tbYbzf2.Text == "" ? "0" : tbYbzf2.Text) == 0)
            {
                fTotalFee = float.Parse(amount.ToString());//本次收费总金额
                fSiPayment = float.Parse(dsc.txt_YBZF == "" ? "0" : dsc.txt_YBZF);//本次收费医保支付金额（非医保情况请置为0）
            }
            else
            {
                fTotalFee = float.Parse(tbYbzf1.Text == "" ? "0" : tbYbzf1.Text) + float.Parse(tbYbzf2.Text == "" ? "0" : tbYbzf2.Text);//本次收费总金额
                fSiPayment = 0;//本次收费医保支付金额（非医保情况请置为0）
            }
            #endregion
            float fSiBaseline = float.Parse(tbSiBaseline.Text.ToString() == "" ? "0" : tbSiBaseline.Text.ToString());//本次收费医保起付线金额（非医保情况请置为0）

            bool bSpecificOutpatient = dsc.cbxMGyb == "" ? false : true;//inReimPara.RegInfo.NetType == "4" ? true : false; //是否门规医保 

            LogManager.Info("SdaCash 开始PatInHosId:" + inReimPara.RegInfo.PatInHosId + "strOrgId:" + strOrgId + "strInvoiceId:" + strInvoiceId + "strIdCardNo:" + strIdCardNo + "strAidCardNo:" + strAidCardNo + "fTotalFee:" + fTotalFee + "fSiPayment:" + fSiPayment + "fSiBaseline:" + fSiBaseline + "bSpecificOutpatient:" + bSpecificOutpatient);

            int reInt;
            reInt = axSDACard1.SdaCash(strOrgId, strInvoiceId, strIdCardNo, strAidCardNo, fTotalFee, fSiPayment, fSiBaseline, bSpecificOutpatient);
            if (reInt != 0)
            {
                LogManager.Info("SdaCash 失败PatInHosId:" + inReimPara.RegInfo.PatInHosId + "strOrgId:" + strOrgId + "strInvoiceId:" + strInvoiceId + "strIdCardNo:" + strIdCardNo + "strAidCardNo:" + strAidCardNo + "fTotalFee:" + fTotalFee + "fSiPayment:" + fSiPayment + "fSiBaseline:" + fSiBaseline + "bSpecificOutpatient:" + bSpecificOutpatient + "reInt:" + reInt);
                throw new Exception("SdaCash失败!错误号:" + reInt);
            }
            tbAidCardNo.Text = strAidCardNo;//救助卡号
            tbTotalFee.Text = axSDACard1.GetTotalFee().ToString();//本次消费总金额
            tbSiPayment.Text = axSDACard1.GetSiPayment().ToString();//本次消费医保总金额（非医保情况返回0）
            tbSiBaseline.Text = axSDACard1.GetSiBaseline().ToString();//本次消费医保起付线金额
            tbAidPercentage.Text = axSDACard1.GetAidPercentage().ToString();//救助比例（百分制，比如返回80，表示救助比例为80%）
            tbAidPayment.Text = axSDACard1.GetAidPayment().ToString();//本次消费大病救助垫付金额
            tbHolderPayment.Text = axSDACard1.GetHolderPayment().ToString();//本次消费自付金额
            tbAidCardPayment.Text = axSDACard1.GetAidCardPayment().ToString();//本次消费使用旧卡余额支付金额
            tbAidCardBalance.Text = axSDACard1.GetAidCardBalance().ToString();//本次消费结算后的旧卡余额
            tbAidBaseline.Text = axSDACard1.GetAidBaseline().ToString();//本次消费大病救助垫付基数
            LogManager.Info("SdaCash 成功OUT_PAT_ID:" + inReimPara.RegInfo.PatInHosId + "strOrgId:" + strOrgId + "strInvoiceId:" + strInvoiceId + "strIdCardNo:" + strIdCardNo + "strAidCardNo:" + strAidCardNo + "fTotalFee:" + fTotalFee + "fSiPayment:" + fSiPayment + "fSiBaseline:" + fSiBaseline + "bSpecificOutpatient:" + bSpecificOutpatient + "reInt:" + reInt);
            dicSettleInfoDibao.Clear();
            dicSettleInfoDibao.Add("Name", dsc.tbName);
            dicSettleInfoDibao.Add("IDCardNo", dsc.tbIDCardNo);
            dicSettleInfoDibao.Add("InsuranceID", dsc.tbInsuranceID);
            dicSettleInfoDibao.Add("InsuranceType", dsc.tbInsuranceType);
            dicSettleInfoDibao.Add("InsuranceTypeMC", dsc.tbInsuranceTypeMC);
            dicSettleInfoDibao.Add("AidType", dsc.tbAidType);
            dicSettleInfoDibao.Add("AidTypeMC", dsc.tbAidTypeMC);
            dicSettleInfoDibao.Add("SerialNo", dsc.tbSerialNo);
            dicSettleInfoDibao.Add("AidCardNo", tbAidCardNo.Text);
            dicSettleInfoDibao.Add("TotalFee", tbTotalFee.Text);
            dicSettleInfoDibao.Add("SiPayment", tbSiPayment.Text);
            dicSettleInfoDibao.Add("SiBaseline", tbSiBaseline.Text);
            dicSettleInfoDibao.Add("AidPercentage", tbAidPercentage.Text);
            dicSettleInfoDibao.Add("AidPayment", tbAidPayment.Text);
            dicSettleInfoDibao.Add("HolderPayment", tbHolderPayment.Text);
            dicSettleInfoDibao.Add("AidCardPayment", tbAidCardPayment.Text);
            dicSettleInfoDibao.Add("AidCardBalance", tbAidCardBalance.Text);
            dicSettleInfoDibao.Add("AidBaseline", tbAidBaseline.Text);

            try
            {
                //需要修改建表语句 添加PAT_IN_HOS_ID
                string strSql = "INSERT INTO REPORT.dbo.db_JSB_zy(PAT_IN_HOS_ID, settle_id,InvoiceId, FLAG_INVALID, Name, GENDER, IDCardNo, InsuranceID, InsuranceType," +
                              "InsuranceTypeMC, AidType, AidTypeMC, SerialNo, AidCardNo,bSpecificOutpatient, TotalFee, SiPayment, SiBaseline,MGBZbh,MGBZmc,YBZF1,YBZF2, AidPercentage, AidPayment, HolderPayment, " +
                              "AidCardPayment, AidCardBalance, AidBaseline, rq,czybh,czy)" +
                              " VALUES('" + inReimPara.RegInfo.PatInHosId + "','" + strOrgId + "','" + strInvoiceId + "',0,'" + dsc.tbName + "','" + dsc.tbGENDER + "','" + dsc.tbIDCardNo + "','" + dsc.tbInsuranceID + "','" + dsc.tbInsuranceType + "'," +
                              "'" + dsc.tbInsuranceTypeMC + "','" + dsc.tbAidType + "','" + dsc.tbAidTypeMC + "','" + dsc.tbSerialNo + "','" + tbAidCardNo.Text + "'," + (bSpecificOutpatient == true ? 1 : 0) + ",'" + tbTotalFee.Text + "','" + tbSiPayment.Text + "','" + tbSiBaseline.Text + "','" + dsc.cbxMGyb + "','" + dsc.cbxMGybText + "','" + tbYbzf1.Text + "','" + tbYbzf2.Text + "','" + tbAidPercentage.Text + "','" + tbAidPayment.Text + "','" + tbHolderPayment.Text + "'," +
                              "'" + tbAidCardPayment.Text + "','" + tbAidCardBalance.Text
                              + "','" + tbAidBaseline.Text + "',getdate(),'"
                              + hosOperatorSysid + "','" + hosOperatorName + "')";

                sqlHelper.ExecSqlReInt(strSql);
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                LogManager.Info("保存低保结算失败:" + ex.Message);
            }
        }

        private void QXJS()
        {
            DataSet ds = new DataSet();
            string strSql = "";
            strSql = @"SELECT * FROM REPORT.dbo.db_JSB_zy WHERE PAT_IN_HOS_ID='" + inReimPara.RegInfo.PatInHosId + "' AND FLAG_INVALID=0 ";
            ds = sqlHelper.ExecSqlReDs(strSql);
            if (ds.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("没有找到此人的住院低保结算记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return;
            }
            //-----------------------------
            try
            {


                string strOrgId = PayAPIConfig.InstitutionDict[1].Memo;//调用方机构编号，保持跟交易明细视图中的机构编号一致；
                string strInvoiceId = inReimPara.CommPara.InNetworkSettleId.ToString();//上次收费的invoiceId
                string strIdCardNo = dsc.tbIDCardNo;//当前居民身份证号码model.outReimPara.RegInfo.IdNo
                string strAidCardNo = tbAidCardNo.Text;//当前救助卡编号model.outReimPara.RegInfo.MemberNo
                float fTotalFee = -float.Parse(tbTotalFee.Text);//本次收费总金额
                float fSiPayment = float.Parse(tbSiPayment.Text);//本次收费医保支付金额（非医保情况请置为0）
                float fSiBaseline = float.Parse(tbSiBaseline.Text);//本次收费医保起付线金额（非医保情况请置为0）
                bool bSpecificOutpatient = dsc.cbxMGyb == "" ? false : true; //是否门规医保 model.outReimPara.RegInfo.NetPatType == "01" ? true : false
               //  dsc.cbxMGyb == "" ? false : true
                LogManager.Info("_SdaCash 开始PAT_IN_HOS_ID:" + inReimPara.RegInfo.PatInHosId + "strInvoiceId:" + strInvoiceId + "strIdCardNo:" + strIdCardNo + "strAidCardNo:" + strAidCardNo);

                int reInt;
                reInt = axSDACard1.SdaCash(strOrgId, strInvoiceId, strIdCardNo, strAidCardNo, fTotalFee, fSiPayment, fSiBaseline, bSpecificOutpatient);
                if (reInt != 0)
                {
                    LogManager.Info("_SdaCash 失败PAT_IN_HOS_ID:" + inReimPara.RegInfo.PatInHosId + "strInvoiceId:" + strInvoiceId + "strIdCardNo:" + strIdCardNo + "strAidCardNo:" + strAidCardNo + "reInt:" + reInt);
                    MessageBox.Show("_SdaCash失败!错误号:" + reInt, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                strSql = "UPDATE REPORT.dbo.db_JSB_zy SET FLAG_INVALID=1,czybh_qx='" + this.hosOperatorSysid + "',czy_qx='" + this.hosOperatorName + "',rq_qx=getdate() WHERE FLAG_INVALID=0 AND PAT_IN_HOS_ID='" + inReimPara.SettleInfo.PatInHosId + "' AND InvoiceId='" + inReimPara.CommPara.InNetworkSettleId.ToString() + "'";
                sqlHelper.ExecSqlReInt(strSql);

                ClearTxt();
                MessageBox.Show("撤销结算成功");
            }
            catch (Exception ex)
            {
                throw new Exception("撤销结算失败,请重新进行撤销结算" + ex.ToString());

            }
        }

        private void ClearTxt()
        {

            foreach (Control cl in this.groupBox2.Controls)
            {
                if (cl is TextBox)
                {
                    cl.Text = string.Empty;
                }
                else if (cl is ComboBox)
                {
                    ComboBox cob = cl as ComboBox;
                    cob.SelectedIndex = -1;
                }

            }


            //throw new NotImplementedException();
        }

        private void dibao_zy_continue_Load(object sender, EventArgs e)
        {
            isCancel = false;
            try
            {
                JS();
            }
            catch (Exception v)
            {

                throw new Exception("低保结算失败:" + v.ToString());
            }


        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
