using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using PayAPIUtilities.Config;
using PayAPIUtilities.Log;

using PayAPIInterface.ParaModel;

namespace PayAPIInstance.Dareway.JiNan.Dialog
{
    public partial class DiBaoJS : Form
    {
        public  MSSQLHelper sqlHelper = new MSSQLHelper("Data Source=172.22.29.6;Initial Catalog=COMM;Persist Security Info=True;User ID=power;Password=m@ssunsoft009");

        /// <summary>
        /// 医院ID
        /// </summary>
        public string hosid = "";

        public OutPayParameter outReimPara;
        //低保结算信息
        public Dictionary<string, string> dicSettleInfoDibao;
        public Dictionary<string, string> dicSettleInfo;

        public DiBaoJS(OutPayParameter _outReimPara, Dictionary<string, string> _dicSettleInfo, Dictionary<string, string> _dicSettleInfoDibao,string hosID)
        {
            InitializeComponent();
            outReimPara = _outReimPara;
            dicSettleInfo = _dicSettleInfo;
            dicSettleInfoDibao = _dicSettleInfoDibao;
            this.hosid = hosID;
        }

        private void DiBaoJS_Load(object sender, EventArgs e)
        {
            try
            {
                DK();

                //当姓名不一致时提示
                if (outReimPara.PatInfo.PatName != tbName.Text)
                {
                    if (MessageBox.Show(" 低保卡姓名为：【" + tbName.Text.ToString() + "】     HIS患者姓名为：【" + outReimPara.PatInfo.PatName + "】 是否继续 ", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    {
                        throw new Exception("姓名不一致，操作员取消操作！");
                    }
                }
                JS();
            }
            catch (Exception ex)
            {
                throw new Exception("低保结算失败:"+ex.Message);
            }
        }

        #region 低保
        /// <summary>
        /// 低保读卡
        /// </summary>
        private void DK()
        {
            int reInt;
            reInt = axSDACard1.Open(); //连接读卡器
            if (reInt != 0)
            {
                throw new Exception("读卡器Open失败!错误号:" + reInt);
            }

            reInt = axSDACard1.FindCard(); //寻卡
            if (reInt != 0)
            {
                throw new Exception("寻卡失败!错误号:" + reInt);
            }
            tbSerialNo.Text = axSDACard1.GetSerialNo(); //读取救助卡流水号（跟卡封面的流水号一致）
            tbName.Text = axSDACard1.GetName();  //读取居民姓名
            tbIDCardNo.Text = axSDACard1.GetIDCardNo(); //读取身份证号码
            tbInsuranceID.Text = axSDACard1.GetInsuranceID(); //读取医保卡号
            tbInsuranceType.Text = axSDACard1.GetInsuranceType(); //读取医保类型
            tbAidType.Text = axSDACard1.GetAidType(); //读取救助类型
            reInt = axSDACard1.Close(); //关闭读卡器
            if (reInt != 0)
            {
                throw new Exception("读卡器Close失败!错误号:" + reInt);
            }



            string strMC = "";
            switch (tbInsuranceType.Text)
            {
                case "01":
                    strMC = "门规";
                    break;
                case "02":
                    strMC = "非门规";
                    break;
                default:
                    strMC = "";
                    break;
            }
            tbInsuranceTypeMC.Text = strMC;


            //返回值：编码字符串，0001：低保，0010：低保边缘；0100：一级中度残疾；1000：二级重度残疾，支持组合类型，比如0011表示既是低保又是低保边缘
            strMC = "";
            string strS = tbAidType.Text;
            try
            {
                if (strS[3] == '1')
                {
                    if (strMC == "")
                    {
                        strMC = "低保";
                    }
                    else
                    {
                        strMC = strMC + "+低保";
                    }
                }
                if (strS[2] == '1')
                {
                    if (strMC == "")
                    {
                        strMC = "低保边缘";
                    }
                    else
                    {
                        strMC = strMC + "+低保边缘";
                    }
                }
                if (strS[1] == '1')
                {
                    if (strMC == "")
                    {
                        strMC = "一级中度残疾";
                    }
                    else
                    {
                        strMC = strMC + "+一级中度残疾";
                    }
                }
                if (strS[0] == '1')
                {
                    if (strMC == "")
                    {
                        strMC = "二级重度残疾";
                    }
                    else
                    {
                        strMC = strMC + "+二级重度残疾";
                    }
                }
                tbAidTypeMC.Text = strMC;
            }
            catch (Exception ex)
            {
            }

            //-----------------------------
            //--卡里没有的信息，从前置机数据库中取出来

            StringBuilder strSql = new StringBuilder();
            DataSet ds = new DataSet();
            string re = "";
            string errString;
            strSql.Append(" SELECT GENDER,SICARDNO,PHONENUMBER,SITYPE,AIDTYPE,ADDRESS,DISABLEDCARDNO,DISABLETYPE,PHOTO,ISDELETED FROM POWERSDA.CARDHOLDERINFO where AIDCARDNO='" + tbSerialNo.Text + "'   ");
            WebReferenceDiBao.DiBaoWebService WebServiceObj = new WebReferenceDiBao.DiBaoWebService();
            re = WebServiceObj.Query(strSql.ToString(), out errString, out ds);
            if (re != "0")
            {
                throw new Exception("调用WebService取低保数据失败:" + errString);
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                #region 图片显示注掉
                //--------------------------------------- 图片显示注掉
                //try
                //{
                //    byte[] File = (byte[])ds.Tables[0].Rows[0]["PHOTO"];
                //    Image photo = null;
                //    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(File))
                //    {
                //        ms.Write(File, 0, File.Length);
                //        photo = Image.FromStream(ms, true);
                //    }
                //    this.pictureBox1.Image = photo;
                //    if (File.Length == 0)
                //    {
                //        lblts.Visible = true;
                //    }
                //    else
                //    {
                //        lblts.Visible = false;
                //    }
                //}
                //catch (Exception ex)
                //{
                //}
                //-------------------------------------- 
                #endregion
                tbGENDER.Text = ds.Tables[0].Rows[0]["GENDER"].ToString() == "0" ? "女" : (ds.Tables[0].Rows[0]["GENDER"].ToString() == "1" ? "男" : "未知");//性别
                tbPHONENUMBER.Text = ds.Tables[0].Rows[0]["PHONENUMBER"].ToString(); //电话号码
                tbADDRESS.Text = ds.Tables[0].Rows[0]["ADDRESS"].ToString(); //现住址
                tbDISABLEDCARDNO.Text = ds.Tables[0].Rows[0]["DISABLEDCARDNO"].ToString(); //残疾证号
                tbDISABLETYPE.Text = ds.Tables[0].Rows[0]["DISABLETYPE"].ToString(); //残疾类型
                tbISDELETED.Text = ds.Tables[0].Rows[0]["ISDELETED"].ToString() == "1" ? "已删除" : ""; //删除标志
                tbInsuranceID.Text = ds.Tables[0].Rows[0]["SICARDNO"].ToString(); //医保卡号
                //-----------------------------------------
                tbInsuranceType.Text = ds.Tables[0].Rows[0]["SITYPE"].ToString();
                tbInsuranceTypeMC.Text = ds.Tables[0].Rows[0]["SITYPE"].ToString() == "1" ? "门规" : "非门规";
                //---------------------------------------------------
                tbAidType.Text = ds.Tables[0].Rows[0]["AIDTYPE"].ToString();
                //救助类型 1=低保 2=低保边缘 3=一级伤残 4=二级伤残 
                strMC = "";
                strS = tbAidType.Text;
                try
                {
                    string[] rr = strS.Split(new string[] { "||" }, System.StringSplitOptions.None);
                    for (int i = 0; i < rr.Length; i++)
                    {
                        if (rr[i] == "1")
                        {
                            if (strMC == "")
                            {
                                strMC = "低保";
                            }
                            else
                            {
                                strMC = strMC + "+低保";
                            }
                        }

                        if (rr[i] == "2")
                        {
                            if (strMC == "")
                            {
                                strMC = "低保边缘";
                            }
                            else
                            {
                                strMC = strMC + "+低保边缘";
                            }
                        }
                        if (rr[i] == "3")
                        {
                            if (strMC == "")
                            {
                                strMC = "一级伤残";
                            }
                            else
                            {
                                strMC = strMC + "+一级伤残";
                            }
                        }

                        if (rr[i] == "4")
                        {
                            if (strMC == "")
                            {
                                strMC = "二级伤残";
                            }
                            else
                            {
                                strMC = strMC + "+二级伤残";
                            }
                        }
                    }//for
                    tbAidTypeMC.Text = strMC;
                }
                catch (Exception ex)
                {
                }
            }

            //---------------------------------------

        }

        /// <summary>
        /// 低保结算
        /// </summary>
        private void JS()
        {
            decimal amount = 0;
            amount = Convert.ToDecimal(dicSettleInfo["brfdje"]) + Convert.ToDecimal(dicSettleInfo["ybfdje"]) + Convert.ToDecimal(dicSettleInfo["yljmje"]);       //本次医疗费用
            string strOrgId = PayAPIConfig.InstitutionDict[1].Memo;//调用方机构编号，保持跟交易明细视图中的机构编号一致；
            string strInvoiceId = outReimPara.CommPara.OutNetworkSettleId.ToString();//本次收费单ID
            string strIdCardNo = tbIDCardNo.Text;//当前居民身份证号码
            string strAidCardNo = tbSerialNo.Text;//当前救助卡编号
            float fTotalFee = float.Parse(amount.ToString());//本次收费总金额
            float fSiPayment = float.Parse((amount - (Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]))).ToString());//本次收费医保支付金额（非医保情况请置为0）
            float fSiBaseline = 0;//本次收费医保起付线金额（非医保情况请置为0）
            bool bSpecificOutpatient = outReimPara.RegInfo.NetType == "4" ? true : false; //是否门规医保 

            LogManager.Info("SdaCash 开始OUT_PAT_ID:" + outReimPara.PatInfo.OutPatId + "strOrgId:" + strOrgId + "strInvoiceId:" + strInvoiceId + "strIdCardNo:" + strIdCardNo + "strAidCardNo:" + strAidCardNo + "fTotalFee:" + fTotalFee + "fSiPayment:" + fSiPayment + "fSiBaseline:" + fSiBaseline + "bSpecificOutpatient:" + bSpecificOutpatient);

            int reInt;
            reInt = axSDACard1.SdaCash(strOrgId, strInvoiceId, strIdCardNo, strAidCardNo, fTotalFee, fSiPayment, fSiBaseline, bSpecificOutpatient);
            if (reInt != 0)
            {
                LogManager.Info("SdaCash 失败OUT_PAT_ID:" + outReimPara.PatInfo.OutPatId + "strOrgId:" + strOrgId + "strInvoiceId:" + strInvoiceId + "strIdCardNo:" + strIdCardNo + "strAidCardNo:" + strAidCardNo + "fTotalFee:" + fTotalFee + "fSiPayment:" + fSiPayment + "fSiBaseline:" + fSiBaseline + "bSpecificOutpatient:" + bSpecificOutpatient + "reInt:" + reInt);
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
            LogManager.Info("SdaCash 成功OUT_PAT_ID:" + outReimPara.PatInfo.OutPatId + "strOrgId:" + strOrgId + "strInvoiceId:" + strInvoiceId + "strIdCardNo:" + strIdCardNo + "strAidCardNo:" + strAidCardNo + "fTotalFee:" + fTotalFee + "fSiPayment:" + fSiPayment + "fSiBaseline:" + fSiBaseline + "bSpecificOutpatient:" + bSpecificOutpatient + "reInt:" + reInt);
            dicSettleInfoDibao.Clear();
            dicSettleInfoDibao.Add("Name", tbName.Text);
            dicSettleInfoDibao.Add("IDCardNo", tbIDCardNo.Text);
            dicSettleInfoDibao.Add("InsuranceID", tbInsuranceID.Text);
            dicSettleInfoDibao.Add("InsuranceType", tbInsuranceType.Text);
            dicSettleInfoDibao.Add("InsuranceTypeMC", tbInsuranceTypeMC.Text);
            dicSettleInfoDibao.Add("AidType", tbAidType.Text);
            dicSettleInfoDibao.Add("AidTypeMC", tbAidTypeMC.Text);
            dicSettleInfoDibao.Add("SerialNo", tbSerialNo.Text);
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
                string strSql = "INSERT INTO REPORT.dbo.db_JSB(hosid,OUT_PAT_ID, OUT_NETWORK_SETTLE_ID, InvoiceId, FLAG_INVALID, Name, GENDER, IDCardNo, InsuranceID, InsuranceType," +
                                "InsuranceTypeMC, AidType, AidTypeMC, SerialNo, AidCardNo,bSpecificOutpatient, TotalFee, SiPayment, SiBaseline, AidPercentage, AidPayment, HolderPayment, " +
                                "AidCardPayment, AidCardBalance, AidBaseline, CREATE_TIME)" +
                                " VALUES('" + hosid + "','" + outReimPara.PatInfo.OutPatId + "','" + outReimPara.CommPara.OutNetworkSettleId + "','" + outReimPara.CommPara.OutNetworkSettleId + "',0,'" + tbName.Text + "','" + tbGENDER.Text + "','" + tbIDCardNo.Text + "','" + tbInsuranceID.Text + "','" + tbInsuranceType.Text + "'," +
                                "'" + tbInsuranceTypeMC.Text + "','" + tbAidType.Text + "','" + tbAidTypeMC.Text + "','" + tbSerialNo.Text + "','" + tbAidCardNo.Text + "'," + (bSpecificOutpatient == true ? 1 : 0) + ",'" + tbTotalFee.Text + "','" + tbSiPayment.Text + "','" + tbSiBaseline.Text + "','" + tbAidPercentage.Text + "','" + tbAidPayment.Text + "','" + tbHolderPayment.Text + "'," +
                                "'" + tbAidCardPayment.Text + "','" + tbAidCardBalance.Text + "','" + tbAidBaseline.Text + "',getdate())";
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
            //-----------------------------

            string strOrgId = PayAPIConfig.InstitutionDict[1].Memo;//调用方机构编号，保持跟交易明细视图中的机构编号一致；
            string strInvoiceId = outReimPara.CommPara.OutNetworkSettleId.ToString();//上次收费的invoiceId
            string strIdCardNo = tbIDCardNo.Text;//当前居民身份证号码model.outReimPara.RegInfo.IdNo
            string strAidCardNo = tbAidCardNo.Text;//当前救助卡编号model.outReimPara.RegInfo.MemberNo
            float fTotalFee = -float.Parse(tbTotalFee.Text);//本次收费总金额
            float fSiPayment = float.Parse(tbSiPayment.Text);//本次收费医保支付金额（非医保情况请置为0）
            float fSiBaseline = 0;//本次收费医保起付线金额（非医保情况请置为0）
            bool bSpecificOutpatient = outReimPara.RegInfo.NetType == "4" ? true : false; //是否门规医保 是否门规医保 model.outReimPara.RegInfo.NetType == "01" ? true : false

            LogManager.Info("_SdaCash 开始OUT_PAT_ID:" + outReimPara.PatInfo.OutPatId + "strInvoiceId:" + strInvoiceId + "strIdCardNo:" + strIdCardNo + "strAidCardNo:" + strAidCardNo);
           

            int reInt;
            reInt = axSDACard1.SdaCash(strOrgId, strInvoiceId, strIdCardNo, strAidCardNo, fTotalFee, fSiPayment, fSiBaseline, bSpecificOutpatient);
            if (reInt != 0)
            {
                LogManager.Info("_SdaCash 失败OUT_PAT_ID:" + outReimPara.PatInfo.OutPatId + "strInvoiceId:" + strInvoiceId + "strIdCardNo:" + strIdCardNo + "strAidCardNo:" + strAidCardNo + "reInt:" + reInt);
                throw new Exception("_SdaCash失败!错误号:" + reInt);
            }
            string strSql = "UPDATE REPORT.dbo.db_JSB SET FLAG_INVALID=1 WHERE OUT_NETWORK_SETTLE_ID='" + outReimPara.CommPara.OutNetworkSettleId + "'";
            sqlHelper.ExecSqlReInt(strSql);
            dicSettleInfoDibao.Clear();
            this.Close();
        } 
        #endregion

        private void btn_xPhoto_Click(object sender, EventArgs e)
        {
            //frmPhoto photo = new frmPhoto(pictureBox1.Image);
            //photo.TopMost = true;
            //photo.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            QXJS();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DiBaoJS_Shown(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }
        //----------------------------------------------------------------
    }
}
