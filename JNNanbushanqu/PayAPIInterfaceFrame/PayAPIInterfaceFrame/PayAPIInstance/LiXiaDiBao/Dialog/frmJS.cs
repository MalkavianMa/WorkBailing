using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using PayAPIUtilities.Config;
using PayAPIUtilities.Log;

using PayAPIInterface.Model.Out;
using PayAPIInterface.Model.Comm;

namespace PayAPIInstance.LiXiaDiBao.Dialog
{
    public partial class frmJS : Form
    {

        public MSSQLHelper sqlHelper = new MSSQLHelper("Data Source=172.22.29.6;Initial Catalog=COMM;Persist Security Info=True;User ID=power;Password=m@ssunsoft009");
        LiXiaDiBaoModel model;

        public frmJS(LiXiaDiBaoModel _model)
        {
            InitializeComponent();
            model = _model;
        }


        

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmJS_Load(object sender, EventArgs e)
        {

            SDACardOpen();
            FindCard();

            decimal amount = 0;
            for (int i = 0; i < model.outReimPara.Details.Count; i++)
            {
                amount = amount + model.outReimPara.Details[i].Amount;
            }
            
            string strOrgId = PayAPIConfig.InstitutionDict[1].Memo;//调用方机构编号，保持跟交易明细视图中的机构编号一致；
            string strInvoiceId = model.outReimPara.CommPara.OutNetworkSettleId.ToString();//本次收费单ID
            string strIdCardNo = model.outReimPara.RegInfo.IdNo;//当前居民身份证号码
            string strAidCardNo = model.outReimPara.RegInfo.MemberNo;//当前救助卡编号
            float fTotalFee = float.Parse(amount.ToString());//本次收费总金额
            float fSiPayment = 0;//本次收费医保支付金额（非医保情况请置为0）
            float fSiBaseline = 0;//本次收费医保起付线金额（非医保情况请置为0）
            bool bSpecificOutpatient = false; //是否门规医保 model.outReimPara.RegInfo.NetPatType == "01" ? true : false

            LogManager.Info("SdaCash 开始OUT_PAT_ID:" + model.outReimPara.PatInfo.OutPatId + "strOrgId:" + strOrgId + "strInvoiceId:" + strInvoiceId + "strIdCardNo:" + strIdCardNo + "strAidCardNo:" + strAidCardNo + "fTotalFee:" + fTotalFee + "fSiPayment:" + fSiPayment + "fSiBaseline:" + fSiBaseline + "bSpecificOutpatient:" + bSpecificOutpatient);
            int reInt;
            reInt = axSDACard1.SdaCash(strOrgId, strInvoiceId, strIdCardNo, strAidCardNo, fTotalFee, fSiPayment, fSiBaseline, bSpecificOutpatient); 
            if (reInt != 0)
            {
                LogManager.Info("SdaCash 失败OUT_PAT_ID:" + model.outReimPara.PatInfo.OutPatId + "strOrgId:" + strOrgId + "strInvoiceId:" + strInvoiceId + "strIdCardNo:" + strIdCardNo + "strAidCardNo:" + strAidCardNo + "fTotalFee:" + fTotalFee + "fSiPayment:" + fSiPayment + "fSiBaseline:" + fSiBaseline + "bSpecificOutpatient:" + bSpecificOutpatient + "reInt:" + reInt);
                throw new Exception("SdaCash失败!错误号:" + reInt);
            }
            tbName.Text=model.outReimPara.RegInfo.NetPatName; //姓名
            tbIDCardNo.Text = model.outReimPara.RegInfo.IdNo;//身份证号
            tbAidCardNo.Text = model.outReimPara.RegInfo.MemberNo;//救助卡号
            tbTotalFee.Text = axSDACard1.GetTotalFee().ToString();//本次消费总金额
            tbSiPayment.Text = axSDACard1.GetSiPayment().ToString();//本次消费医保总金额（非医保情况返回0）
            tbSiBaseline.Text = axSDACard1.GetSiBaseline().ToString();//本次消费医保起付线金额
            tbAidPercentage.Text = axSDACard1.GetAidPercentage().ToString();//救助比例（百分制，比如返回80，表示救助比例为80%）
            tbAidPayment.Text = axSDACard1.GetAidPayment().ToString();//本次消费大病救助垫付金额
            tbHolderPayment.Text = axSDACard1.GetHolderPayment().ToString();//本次消费自付金额
            tbAidCardPayment.Text = axSDACard1.GetAidCardPayment().ToString();//本次消费使用旧卡余额支付金额
            tbAidCardBalance.Text = axSDACard1.GetAidCardBalance().ToString();//本次消费结算后的旧卡余额
            tbAidBaseline.Text = axSDACard1.GetAidBaseline().ToString();//本次消费大病救助垫付基数
            LogManager.Info("SdaCash 成功OUT_PAT_ID:" + model.outReimPara.PatInfo.OutPatId + "strOrgId:" + strOrgId + "strInvoiceId:" + strInvoiceId + "strIdCardNo:" + strIdCardNo + "strAidCardNo:" + strAidCardNo + "fTotalFee:" + fTotalFee + "fSiPayment:" + fSiPayment + "fSiBaseline:" + fSiBaseline + "bSpecificOutpatient:" + bSpecificOutpatient + "reInt:" + reInt);
            SDACardClose();

            try
            {
                string strSql = "INSERT INTO REPORT.dbo.db_JSB(OUT_PAT_ID, OUT_NETWORK_SETTLE_ID, InvoiceId, FLAG_INVALID, Name, GENDER, IDCardNo, InsuranceID, InsuranceType," +
                    "InsuranceTypeMC, AidType, AidTypeMC, SerialNo, AidCardNo,bSpecificOutpatient, TotalFee, SiPayment, SiBaseline, AidPercentage, AidPayment, HolderPayment, " +
                    "AidCardPayment, AidCardBalance, AidBaseline, CREATE_TIME)" +
                    " VALUES('" + model.outReimPara.PatInfo.OutPatId + "','" + model.outReimPara.CommPara.OutNetworkSettleId + "','" + model.outReimPara.CommPara.OutNetworkSettleId + "',0,'" + tbName.Text + "','" + model.networkPatInfo.Sex + "','" + tbIDCardNo.Text + "','" + model.networkPatInfo.ICNo + "','" + model.networkPatInfo.MedicalType + "'," +
                    "'" + model.networkPatInfo.MedicalTypeName + "','" + model.networkPatInfo.CompanyNo + "','" + model.networkPatInfo.CompanyName + "','" + model.networkPatInfo.MedicalNo + "','" + tbAidCardNo.Text + "'," + (bSpecificOutpatient == true ? 1 : 0) + ",'" + tbTotalFee.Text + "','" + tbSiPayment.Text + "','" + tbSiBaseline.Text + "','" + tbAidPercentage.Text + "','" + tbAidPayment.Text + "','" + tbHolderPayment.Text + "'," +
                    "'" + tbAidCardPayment.Text + "','" + tbAidCardBalance.Text + "','" + tbAidBaseline.Text + "',getdate())";
                sqlHelper.ExecSqlReInt(strSql);
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                LogManager.Info("保存低保结算失败:" + ex.Message);
                MessageBox.Show("保存低保结算失败:" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            SaveOutSettleMain();
        }

        #region 保存门诊结算数据
        /// <summary>
        /// 保存门诊结算数据
        /// </summary>
        public void SaveOutSettleMain()
        {

            #region 保存中心返回值参数列表
            //保存中心返回值参数列表
            try
            {

                model.outReimPara.SettleParaList = new List<PayAPIInterface.Model.Out.OutNetworkSettleList>();
                AddToSettleList_MZ("Name", tbName.Text, "姓名");
                AddToSettleList_MZ("IDCardNo", tbIDCardNo.Text, "身份证号");
                AddToSettleList_MZ("AidCardNo", tbAidCardNo.Text, "救助卡号");
                AddToSettleList_MZ("TotalFee", tbTotalFee.Text, "本次消费总金额");
                AddToSettleList_MZ("SiPayment", tbSiPayment.Text, "本次消费医保总金额");
                AddToSettleList_MZ("SiBaseline", tbSiBaseline.Text, "本次消费医保起付线金额");
                AddToSettleList_MZ("AidPercentage", tbAidPercentage.Text, "救助比例");
                AddToSettleList_MZ("AidPayment", tbAidPayment.Text, "本次消费大病救助垫付金额");
                AddToSettleList_MZ("HolderPayment", tbHolderPayment.Text, "本次消费自付金额");
                AddToSettleList_MZ("AidCardPayment", tbAidCardPayment.Text, "本次消费使用旧卡余额支付金额");
                AddToSettleList_MZ("AidCardBalance", tbAidCardBalance.Text, "本次消费结算后的旧卡余额");
                AddToSettleList_MZ("AidBaseline", tbAidBaseline.Text, "本次消费大病救助垫付基数");

            }
            catch (Exception ex)
            {
                LogManager.Info("保存中心返回值参数列表 插入值 失败" + ex.Message);
            }
            #endregion

            OutNetworkSettleMain outSettleMain = new OutNetworkSettleMain();
            outSettleMain.OutPatId = model.outReimPara.PatInfo.OutPatId;
            outSettleMain.SettleNo = model.outReimPara.CommPara.OutNetworkSettleId.ToString();                    
            outSettleMain.Amount = Convert.ToDecimal(tbTotalFee.Text); //本次医疗费用
            outSettleMain.GetAmount = Convert.ToDecimal(tbHolderPayment.Text);    //本次现金支出
            outSettleMain.MedAmountZhzf = Convert.ToDecimal(tbAidCardPayment.Text);//本次消费使用旧卡余额支付金额
            outSettleMain.MedAmountTc = Convert.ToDecimal(tbSiPayment.Text);  //本次消费医保总金额
            outSettleMain.MedAmountDb = Convert.ToDecimal(tbAidPayment.Text);  //本次消费大病救助垫付金额
            outSettleMain.MedAmountBz = Convert.ToDecimal(tbAidBaseline.Text);  //本次消费大病救助垫付基数
            outSettleMain.MedAmountJm = Convert.ToDecimal(tbAidCardBalance.Text);  //本次消费结算后的旧卡余额
            outSettleMain.CreateTime = DateTime.Now;
            outSettleMain.InvoiceId = -1;
            outSettleMain.IsCash = true;
            outSettleMain.IsInvalid = false;
            outSettleMain.IsNeedRefund = false;
            outSettleMain.IsRefundDo = false;
            outSettleMain.IsSettle = true;
            outSettleMain.MedAmountTotal = Convert.ToDecimal(outSettleMain.Amount) - Convert.ToDecimal(outSettleMain.GetAmount);
            outSettleMain.NetworkingPatClassId = Convert.ToInt32(model.outReimPara.SettleInfo.NetworkingPatClassId);
            outSettleMain.NetworkPatName = model.outReimPara.RegInfo.NetPatName;
            outSettleMain.NetworkPatType = "0";
            outSettleMain.NetworkRefundTime = Convert.ToDateTime("2000-01-01");
            outSettleMain.NetworkSettleTime = DateTime.Now;
            outSettleMain.OperatorId = PayAPIConfig.Operator.UserSysId; //operatorInfo.UserSysId;
            outSettleMain.OutNetworkSettleId = Convert.ToDecimal(model.outReimPara.SettleInfo.OutNetworkSettleId);
            outSettleMain.SettleBackNo = "";


            model.outReimPara.SettleInfo = outSettleMain;

            PayAPIInterface.Model.Comm.PayType payType;
            model.outReimPara.PayTypeList = new List<PayType>();
            payType = new PayAPIInterface.Model.Comm.PayType();
            payType.PayTypeId = 8;
            payType.PayTypeName = "低保";
            payType.PayAmount = Convert.ToDecimal(outSettleMain.MedAmountTotal);
            model.outReimPara.PayTypeList.Add(payType);

        }

        //----------------------------------------------------添加到out_network_settle_list
        public void AddToSettleList_MZ(string Name, string Value, string Memo)
        {
            PayAPIInterface.Model.Out.OutNetworkSettleList outNetworkSettleList;
            outNetworkSettleList = new PayAPIInterface.Model.Out.OutNetworkSettleList();
            outNetworkSettleList.OutPatId = model.outReimPara.PatInfo.OutPatId;
            outNetworkSettleList.OutNetworkSettleId = model.outReimPara.CommPara.OutNetworkSettleId;
            outNetworkSettleList.ParaName = Name;
            outNetworkSettleList.ParaValue = Value;
            outNetworkSettleList.Memo = Memo;
            model.outReimPara.SettleParaList.Add(outNetworkSettleList);
        }
        //----------------------------------------------------
        #endregion

        private void SDACardOpen()
        {
            int reInt = axSDACard1.Open(); //连接读卡器
            if (reInt != 0)
            {
                throw new Exception("Open失败!错误号:" + reInt);
            }
        }

        private void FindCard()
        {
            int reInt = axSDACard1.FindCard(); //寻卡
            if (reInt != 0)
            {
                throw new Exception("FindCard失败!错误号:" + reInt);
            }
        }

        private void SDACardClose()
        {
            int reInt = axSDACard1.Close(); //关闭读卡器
            if (reInt != 0)
            {
                throw new Exception("Close失败!错误号:" + reInt);
            }
        }
        //----------------------------------------------------------------------
    }
}
