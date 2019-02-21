using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using PayAPIInstance.Dareway.JiNan.Dialog;
using PayAPIInterface;
using PayAPIInterface.Model.Comm;
using PayAPIUtilities.Config;
using PayAPIInterface.ParaModel;
using PayAPIInterface.Model.Out;
using PayAPIInterface.Model.In;
using PayAPIUtilities.Log;
using PayAPIInterfaceHandle.Dareway.JiNan;
using System.Data;

namespace PayAPIInstance.Dareway.JiNan
{
    /// <summary>
    /// 职工和居民门诊统筹1
    /// </summary>
    public class JNDWInterfaceModel_MZTC : IPayCompanyInterface
    {
        //医保个人信息 
        public NetworkPatInfo netPatInfo = new NetworkPatInfo();
        /// <summary>
        /// 病人信息
        /// </summary>
        Dictionary<string, string> patInfo = new Dictionary<string, string>();
        /// <summary>
        /// 门诊入参
        /// </summary>
        public OutPayParameter outReimPara;

        /// <summary>
        /// 住院入参
        /// </summary>
        public InPayParameter inReimPara;

        public OperatorInfo operatorInfo;
        /// <summary>
        /// 业务处理
        /// </summary>
        public static DarewayHandle handelModel;
        //结算信息
        public Dictionary<string, string> dicSettleInfo = new Dictionary<string, string>();
        //低保结算信息
        public Dictionary<string, string> dicSettleInfoDibao = new Dictionary<string, string>();
        //患者信息
        public Dictionary<string, string> dicPatInfo = new Dictionary<string, string>();
        //疾病编码
        public string strDiagnosCode = "";

        public bool isInit = false;  //是否初始化

        /// <summary>
        ///  *使用账户类型;0 不使用,1银行卡,2 cpu 卡，3，济南医保卡
        /// </summary>
        public string P_syzhlx = "3";

        public string CARD_Y_N = "";//有卡读取或者无卡读取，0，为无卡，1为有卡



        /// <summary>
        /// 初始化
        /// </summary>
        public void InterfaceInit()
        {
            if (!isInit)
            {
                handelModel = new DarewayHandle();
                isInit = true;
            }
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
                OutNetworkSettleList outNetworkSettleList;
                foreach (var item in dicSettleInfo)
                {
                    outNetworkSettleList = new OutNetworkSettleList();
                    outNetworkSettleList.OutPatId = outReimPara.PatInfo.OutPatId;
                    outNetworkSettleList.OutNetworkSettleId = outReimPara.CommPara.OutNetworkSettleId;
                    outNetworkSettleList.ParaName = item.Key.ToString();
                    outNetworkSettleList.ParaValue = item.Value;
                    outNetworkSettleList.Memo = "";
                    outReimPara.SettleParaList.Add(outNetworkSettleList);
                }
            }
            catch (Exception ex)
            {
                LogManager.Info("保存中心返回值参数列表 插入值 失败" + ex.Message);
            }
            #endregion

            OutNetworkSettleMain outSettleMain = new OutNetworkSettleMain();
            outSettleMain.OutPatId = outReimPara.PatInfo.OutPatId;
            outSettleMain.SettleNo = dicSettleInfo["brjsh"];                           //医保中心交易流水号
            outSettleMain.Amount = Convert.ToDecimal(dicSettleInfo["brfdje"]) + Convert.ToDecimal(dicSettleInfo["ybfdje"]) + Convert.ToDecimal(dicSettleInfo["yljmje"]);       //本次医疗费用
            outSettleMain.MedAmountZhzf = Convert.ToDecimal(dicSettleInfo["grzhzf"]);//本次帐户支出
            outSettleMain.MedAmountTc = Convert.ToDecimal(dicSettleInfo["ybfdje"]);  //本次统筹支出
            outSettleMain.MedAmountDb = dicSettleInfoDibao.Count > 0 ? Convert.ToDecimal(dicSettleInfoDibao["AidPayment"]) + Convert.ToDecimal(dicSettleInfoDibao["AidCardPayment"]) : 0;  //低保支付
            outSettleMain.MedAmountBz = Convert.ToDecimal(dicSettleInfo["ylbzje"]);  //本次补助支出
            outSettleMain.MedAmountJm = Convert.ToDecimal(dicSettleInfo["yljmje"]);  //减免金额
            outSettleMain.GetAmount = Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]) - outSettleMain.MedAmountDb;    //本次现金支出
            outSettleMain.CreateTime = DateTime.Now;
            outSettleMain.InvoiceId = -1;
            outSettleMain.IsCash = true;
            outSettleMain.IsInvalid = false;
            outSettleMain.IsNeedRefund = false;
            outSettleMain.IsRefundDo = false;
            outSettleMain.IsSettle = true;
            outSettleMain.MedAmountTotal = Convert.ToDecimal(outSettleMain.Amount) - Convert.ToDecimal(outSettleMain.GetAmount);
            outSettleMain.NetworkingPatClassId = Convert.ToInt32(outReimPara.SettleInfo.NetworkingPatClassId);
            outSettleMain.NetworkPatName = dicPatInfo["xm"];
            outSettleMain.NetworkPatType = dicSettleInfoDibao.Count > 0?"低保":""; //低保病人结算
            outSettleMain.SettleType = dicSettleInfoDibao.Count > 0?"1":"0"; //低保结算标志
            outSettleMain.NetworkRefundTime = Convert.ToDateTime("2000-01-01");
            outSettleMain.NetworkSettleTime = DateTime.Now;
            outSettleMain.OperatorId = PayAPIConfig.Operator.UserSysId; //operatorInfo.UserSysId;
            outSettleMain.OutNetworkSettleId = Convert.ToDecimal(outReimPara.SettleInfo.OutNetworkSettleId);
            outSettleMain.SettleBackNo = "";


            outReimPara.SettleInfo = outSettleMain;

            PayAPIInterface.Model.Comm.PayType payType;
            outReimPara.PayTypeList = new List<PayType>();
            payType = new PayAPIInterface.Model.Comm.PayType();
            payType.PayTypeId = 4;
            payType.PayTypeName = "医保";
            payType.PayAmount = Convert.ToDecimal(outSettleMain.MedAmountTotal)-Convert.ToDecimal(outReimPara.SettleInfo.MedAmountDb);
            outReimPara.PayTypeList.Add(payType);

            if (dicSettleInfoDibao.Count > 0)
            {
                payType = new PayAPIInterface.Model.Comm.PayType();
                payType.PayTypeId = 8;
                payType.PayTypeName = "低保";
                payType.PayAmount = Convert.ToDecimal(outReimPara.SettleInfo.MedAmountDb);
                outReimPara.PayTypeList.Add(payType);
            }

        }
        #endregion

        #region 门诊联网操作
        /// <summary>
        /// 读卡
        /// </summary>
        /// <returns></returns>
        public NetworkPatInfo NetworkReadCard()
        {
            InterfaceInit();
            NetworkPatInfo networkPatInfo = new NetworkPatInfo();




            frmCARD frmCard = new frmCARD();
            frmCard.ShowDialog();
            if (frmCard.iscard == "1")
            {
                CARD_Y_N = "1";
                P_syzhlx = "3";
                try  
                {
                    patInfo = handelModel.ReadCardMZ();
                }
                catch (Exception ex)
                {
                    patInfo = handelModel.ReadCardMZ();
                }
            }
            else if (frmCard.iscard == "0")
            {
                CARD_Y_N = "0";
                P_syzhlx = "5";
                patInfo = handelModel.QueryBasicInfo(frmCard.IDNo, "", "6", "");//*医疗统筹类别(1,住院，4 门规)
            }
            else
            {
                throw new Exception("操作员取消本次操作");
            }

            dicPatInfo = patInfo;
            networkPatInfo.MedicalNo = patInfo["ylzbh"];                   //医疗卡号
            networkPatInfo.PatName = patInfo["xm"];                        //姓名
            networkPatInfo.Sex = patInfo["xb"] == "1" ? "男" : "女";       //性别
            networkPatInfo.IDNo = patInfo["shbzhm"];//patInfo["sfzhm"];                        //身份证号码shbzhm
            networkPatInfo.MedicalTypeName = patInfo["ylrylb"];
            networkPatInfo.MedicalType = patInfo["ylrylb"];                //医疗人员类别
            networkPatInfo.ICAmount = Convert.ToDecimal(patInfo["ye"]);  //账户余额
            networkPatInfo.ICNo = "";//patInfo["kh"];                           //社会保障卡卡号
            networkPatInfo.CompanyNo = patInfo["sbjbm"];                    //单位编号sbjbm
            networkPatInfo.CompanyName = patInfo["dwmc"];                  //单位名称
            //networkPatInfo.Birthday = Convert.ToDateTime(patInfo["csrq"].Substring(0, 4) + "-" + patInfo["csrq"].Substring(4, 2) + "-" + patInfo["csrq"].Substring(6, 2));                    //出生日期
            networkPatInfo.MedicalType = patInfo["ylrylb"];
            return networkPatInfo;

        }
        /// <summary>
        /// 门诊登记
        /// </summary>
        /// <param name="para"></param>
        public void OutNetworkRegister(OutPayParameter para)
        {

            NetworkReadCard();
            //显示个人信息
            PersonInfoDialog perDialog = new PersonInfoDialog(patInfo);
            perDialog.ShowDialog();
            strDiagnosCode = perDialog.strDiagnosCode;
            if (perDialog.isCancel)
            {
                throw new Exception("取消操作");
            }

            para.RegInfo.CardNo = patInfo["ylzbh"];                   //医疗卡号
            para.RegInfo.NetRegSerial = patInfo["sbjglx"];              //卡序列号
            para.RegInfo.CantonCode = patInfo["sbjbm"];              //行政区号
            para.RegInfo.MemberNo = patInfo["shbzhm"];                //成员编码
            para.RegInfo.CompanyName = patInfo["dwmc"];               //单位名称
            para.RegInfo.PatAddress = patInfo["dwmc"];                   //住址
            para.RegInfo.IdNo = patInfo["shbzhm"];                    //身份证号
            para.RegInfo.NetPatType = patInfo["ylrylb"];             //人员类别
            para.RegInfo.Balance = Convert.ToDecimal(patInfo["ye"]);//账户余额
            para.RegInfo.NetPatName = patInfo["xm"];                  //姓名
            para.RegInfo.Memo1 = patInfo["xb"] == "1" ? "男" : "女";                         //性别
            para.RegInfo.Memo2 = perDialog.reDiBao;  //是否低保
            string rqlb = "";
            if (patInfo["sbjglx"] == "A")
            {
                rqlb = "职工";
            }
            else if (patInfo["sbjglx"] == "B")
            {
                rqlb = "居民";
            }

            para.RegInfo.NetPatType = rqlb;
            para.RegInfo.NetType = "6";

        }

        /// <summary>
        /// 门诊联网预结算
        /// </summary>
        /// <param name="inPara">门诊接口入参</param>
        /// <returns></returns>
        public void OutNetworkPreSettle(OutPayParameter para)
        {
            outReimPara = para;
            //outReimPara.CommPara.NetworkPatClassId;
            //判断费用限额
            string reStr = handelModel.MZfysh(outReimPara.RegInfo.IdNo, outReimPara.CommPara.NetworkPatClassId, outReimPara.CommPara.TradeId.ToString());
            if (reStr != "1")
            {
                throw new Exception(reStr);
            }
        }

        /// <summary>
        /// 门诊联网结算
        /// </summary>
        /// <param name="inPara">门诊接口入参</param>
        /// <returns></returns>
        public void OutNetworkSettle(OutPayParameter para)
        {
            InterfaceInit();
            outReimPara = para;

            //当姓名不一致时提示
            if (outReimPara.PatInfo.PatName != dicPatInfo["xm"])
            {
                if (MessageBox.Show(" 医保卡姓名为：【" + dicPatInfo["xm"].ToString() + "】     HIS患者姓名为：【" + outReimPara.PatInfo.PatName + "】 是否继续 ", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                {
                    throw new Exception("姓名不一致，操作员取消操作！");
                }
            }


            //门诊初始化
            handelModel.InitJMMZ(outReimPara.RegInfo.CantonCode, "6", outReimPara.RegInfo.MemberNo, outReimPara.PatInfo.PatName,
                                dicPatInfo["xb"], outReimPara.CommPara.OutNetworkSettleId.ToString(), DateTime.Now.ToString("yyyy-MM-dd"),//"001", 
                                handelModel.GetNetWorkDocCode(handelModel.getMzysbh(outReimPara.PatInfo.OutPatId.ToString(), outReimPara.CommPara.TradeId.ToString())), strDiagnosCode, P_syzhlx, outReimPara.RegInfo.CardNo, "C", "");

            //上传门诊费用
            handelModel.SaveOutItems(outReimPara.Details);
            //门诊结算
            dicSettleInfo = handelModel.SettleMG(dicPatInfo["sbjglx"]);

            //---------------------------------------------低保结算

            if (Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]) > 0) //如果自负金额大于0弹出是否低保结算提示
            {

                if (outReimPara.RegInfo.Memo2=="低保")
                {
                    dicSettleInfoDibao.Clear();
                    DiBaoJS_Confirm diBaoJS = new DiBaoJS_Confirm(outReimPara, dicSettleInfo, dicSettleInfoDibao);
                    diBaoJS.ShowDialog();
                }
            }
            //------------------------------------------------
  

            //保存门诊结算明细
            SaveOutSettleMain();

            GC.KeepAlive(handelModel);
            GC.Collect();
        }

        /// <summary>
        /// 撤销门诊结算
        /// </summary>
        /// <param name="inPara">门诊接口入参</param>
        /// <returns></returns>
        public void CancelOutNetworkSettle(OutPayParameter para)
        {
            InterfaceInit();
            outReimPara = para;

            if (outReimPara.SettleInfo == null)
                throw new Exception("本地结算信息不存在");
            string settleNo = outReimPara.SettleInfo.SettleNo.ToString();

            //撤销低保结算
            if (outReimPara.SettleInfo.NetworkPatType.ToString() == "低保")
            {
                while (true)
                {
                    try
                    {
                        DiBaoQXJS frmqxjs = new DiBaoQXJS(outReimPara.SettleInfo.RelationId);
                        frmqxjs.TopMost = true;
                        frmqxjs.ShowDialog();
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (MessageBox.Show("撤销门诊结算失败 错误提示" + ex.Message + "  是否重新撤销", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            throw new Exception("操作员取消撤销结算");
                        }
                    }
                }
            }

            //撤销医保结算
            while (true)
            {
                try
                {
                    handelModel.CancelMGSettle(settleNo);
                    break;
                }
                catch (Exception ex)
                {
                    if (MessageBox.Show("撤销门诊结算失败 错误提示" + ex.Message + "  是否重新撤销", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        throw new Exception("操作员取消撤销结算");
                    }
                }
            }



            GC.KeepAlive(handelModel);
            GC.Collect();
        }

        #endregion

        #region 住院(不需要)
        public void CancelInNetworkRegister(InPayParameter para)
        {
            throw new NotImplementedException();
        }

        public void CancelInNetworkSettle(InPayParameter para)
        {
            throw new NotImplementedException();
        }


        public void InNetworkPreSettle(InPayParameter para)
        {
            throw new NotImplementedException();
        }

        public void InNetworkRegister(InPayParameter para)
        {
            throw new NotImplementedException();
        }

        public void InNetworkSettle(InPayParameter para)
        {
            throw new NotImplementedException();
        }
        #endregion



    }
}
