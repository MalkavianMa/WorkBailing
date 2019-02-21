using System;//JNDWInterfaceModel_MZGZY
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using PayAPIInterface;
using PayAPIInterface.Model.Comm;
using PayAPIInterface.ParaModel;
using PayAPIResolver.Dareway.DIWEI;
using PayAPIUtilities.Log;
using PayAPIInterface.Model.Out;
using PayAPIInstance.Dareway.DIWEI.Dialog;
using PayAPIInterface.Model.In;
using PayAPIUtilities.Config;
using PayAPIClassLib.BLL;

//namespace IRCIDareWaySoftModel.JiNan
namespace PayAPIInstance.Dareway.DIWEI
{
    /// <summary>
    /// 济南职工统筹住院 Source:南部山区 from:历下区
    /// </summary>
    public class JNDWInterfaceModel_MZGZY : IPayCompanyInterface
    {
        //医保个人信息 
        public NetworkPatInfo netPatInfo = new NetworkPatInfo();


        //低保结算信息
        public Dictionary<string, string> dicSettleInfoDibao = new Dictionary<string, string>();



        /// <summary>
        /// 当前操作员医院ID
        /// </summary>
        public string hosId = PayAPIConfig.Operator.HospitalId;

        /// <summary>
        /// 当前操作员姓名
        /// </summary>
        public string hosOperatorName = PayAPIConfig.Operator.UserName;

        /// <summary>
        /// 当前操作员usersysid
        /// </summary>
        public string hosOperatorSysid = PayAPIConfig.Operator.UserSysId;

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
        private static DarewayInterfaceResolver handelModel;
        //结算信息
        public Dictionary<string, string> dicSettleInfo = new Dictionary<string, string>();
        //患者信息
        public Dictionary<string, string> dicPatInfo = new Dictionary<string, string>();
        //疾病编码
        public string strDiagnosCode = "";

        public bool isInit = false;  //是否初始化

        public static string strPayTypeId = "4";  //固定支付方式
        /// <summary>
        ///  *使用账户类型;0 不使用,1银行卡,2 cpu 卡，3 联机卡
        /// </summary>
        public string P_syzhlx = "3";

        public string CARD_Y_N = "";//有卡读取或者无卡读取，0，为无卡，1为有卡
        /// <summary>
        /// 住院是否已经读取卡信息
        /// </summary>
        public bool IsInReadCard = false;

        public string bxlbjs = "";
        public string bxlbmcjs = "";






        /// <summary>
        /// 初始化
        /// </summary>
        public void InterfaceInit()
        {
            if (!isInit)
            {
                handelModel = new DarewayInterfaceResolver();
                isInit = true;
            }
        }

        #region 保存门诊结算数据 账户支付
        /// <summary>
        /// 保存门诊结算数据
        /// </summary>
        public void SaveOutSettleMain(string bz)
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
            outSettleMain.SettleNo = dicSettleInfo["mzzdlsh"];                    //医保中心交易流水号
            outSettleMain.Amount = Convert.ToDecimal(dicSettleInfo["zje"]);       //本次医疗费用
            outSettleMain.GetAmount = Convert.ToDecimal(dicSettleInfo["zje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]) - Convert.ToDecimal(dicSettleInfo["jmje"]);    //本次现金支出
            outSettleMain.MedAmountZhzf = Convert.ToDecimal(dicSettleInfo["grzhzf"]);//本次帐户支出
            outSettleMain.MedAmountTc = 0;  //本次统筹支出
            outSettleMain.MedAmountDb = 0;  //本次大病支出
            outSettleMain.MedAmountBz = Convert.ToDecimal(dicSettleInfo["jmje"]);  //本次大病支出
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
            outSettleMain.NetworkPatType = "0";
            outSettleMain.NetworkRefundTime = Convert.ToDateTime("2000-01-01");
            outSettleMain.NetworkSettleTime = DateTime.Now;
            outSettleMain.OperatorId = PayAPIConfig.Operator.UserSysId; //operatorInfo.UserSysId;
            outSettleMain.OutNetworkSettleId = Convert.ToDecimal(outReimPara.SettleInfo.OutNetworkSettleId);
            outSettleMain.SettleBackNo = "";

            outSettleMain.SettleType = (bz == "Deduct" ? "10" : (bz == "JZ" ? "9" : "1")); //自定义暂存款SettleType为10,急诊的SettleType为9
            outReimPara.SettleInfo = outSettleMain;

            PayAPIInterface.Model.Comm.PayType payType;
            outReimPara.PayTypeList = new List<PayType>();
            payType = new PayAPIInterface.Model.Comm.PayType();
            payType.PayTypeId = 4;
            payType.PayTypeName = "医保";
            payType.PayAmount = Convert.ToDecimal(outSettleMain.MedAmountTotal);
            outReimPara.PayTypeList.Add(payType);

            //payType = new PayAPIInterface.Model.Comm.PayType();
            //payType.PayTypeId = 5;
            //payType.PayTypeName = "医保卡";
            //payType.PayAmount = Convert.ToDecimal(outReimPara.SettleInfo.MedAmountZhzf);
            //outReimPara.PayTypeList.Add(payType);


        }
        #endregion

        #region 保存门诊结算数据
        /// <summary>
        /// 保存门诊结算数据
        /// </summary>
        public void SaveOutSettleMain()
        {
            string[] arr = new string[14];
            arr[0] = "病人结算号";
            arr[1] = "病人负担金额";
            arr[2] = "医保负担金额";
            arr[3] = "个人账户支付";
            arr[4] = "医疗补助金额";
            arr[5] = "医疗减免支付";
            arr[6] = "本次统筹支付";
            arr[7] = "本次大额支付";
            arr[8] = "暂缓支付";
            arr[9] = "累计统筹支付";
            arr[10] = "累计大额支付";
            arr[11] = "累计门诊额度";
            arr[12] = "个人自费累计";
            arr[13] = "其他统筹支付";



            #region 保存中心返回值参数列表
            //保存中心返回值参数列表
            try
            {
                int a = 0;
                OutNetworkSettleList outNetworkSettleList;
                foreach (var item in dicSettleInfo)
                {
                    outNetworkSettleList = new OutNetworkSettleList();
                    outNetworkSettleList.OutPatId = outReimPara.PatInfo.OutPatId;
                    outNetworkSettleList.OutNetworkSettleId = outReimPara.CommPara.OutNetworkSettleId;
                    outNetworkSettleList.ParaName = item.Key.ToString();
                    outNetworkSettleList.ParaValue = item.Value;
                    outNetworkSettleList.Memo = arr[a];
                    outReimPara.SettleParaList.Add(outNetworkSettleList);
                    a++;
                }
            }
            catch (Exception ex)
            {
                LogManager.Info("保存中心返回值参数列表 插入值 失败" + ex.Message);
            }
            #endregion

            OutNetworkSettleMain outSettleMain = new OutNetworkSettleMain();
            outSettleMain.OutPatId = outReimPara.PatInfo.OutPatId;
            outSettleMain.SettleNo = dicSettleInfo["brjsh"];//+ "|" + DateTime.Now.ToString("yyyyMMddHHmmss");                           //医保中心交易流水号
            outSettleMain.Amount = Convert.ToDecimal(dicSettleInfo["brfdje"]) + Convert.ToDecimal(dicSettleInfo["ybfdje"]) + Convert.ToDecimal(dicSettleInfo["yljmje"]);       //本次医疗费用
            outSettleMain.GetAmount = Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]);    //本次现金支出
            outSettleMain.MedAmountZhzf = Convert.ToDecimal(dicSettleInfo["grzhzf"]);//本次帐户支出
            outSettleMain.MedAmountTc = Convert.ToDecimal(dicSettleInfo["ybfdje"]);  //本次统筹支出
            outSettleMain.MedAmountDb = 0;  //本次大病支出
            outSettleMain.MedAmountBz = Convert.ToDecimal(dicSettleInfo["ylbzje"]);  //本次大病支出
            outSettleMain.MedAmountJm = Convert.ToDecimal(dicSettleInfo["yljmje"]);  //减免金额
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
            outSettleMain.NetworkPatType = "0";
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
            payType.PayAmount = Convert.ToDecimal(outSettleMain.MedAmountTotal);
            outReimPara.PayTypeList.Add(payType);

        }
        #endregion



        #region 保存住院结算数据 含低保卡、医保卡
        /// <summary>
        /// 保存住院结算数据
        /// </summary>
        public void SaveInSettleMain()
        {
            string[] arr = new string[14];


            arr[0] = "病人结算号";
            arr[1] = "病人负担金额";
            arr[2] = "医保负担金额";
            arr[3] = "医疗补助金额";
            arr[4] = "个人账户支付";
            arr[5] = "医疗减免金额";
            arr[6] = "医院负担金额";
            arr[7] = "超标床位费";
            arr[8] = "统筹支付";
            arr[9] = "大额支付";
            arr[10] = "二次报销金额";
            arr[11] = "卫计帮扶";
            arr[12] = "民政补助";
            arr[13] = "贫困人口补偿";



            #region 保存中心返回值参数列表
            //保存中心返回值参数列表
            #region 保存SettleList值
            try
            {
                int qwe = 0;
                InNetworkSettleList inNetworkSettleList = new InNetworkSettleList();
                foreach (var item in dicSettleInfo)
                {
                    inNetworkSettleList = new InNetworkSettleList();
                    inNetworkSettleList.PatInHosId = inReimPara.PatInfo.PatInHosId;
                    inNetworkSettleList.InNetworkSettleId = -1;
                    inNetworkSettleList.ParaName = item.Key;
                    inNetworkSettleList.ParaValue = item.Value.ToString();
                    inNetworkSettleList.Memo = arr[qwe];
                    inReimPara.SettleParaList.Add(inNetworkSettleList);
                    qwe++;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("保存农合中心返回值参数列表 插入值 失败" + ex.Message, ex);
            }
            #endregion
            InNetworkSettleMain inSettleMain = new InNetworkSettleMain();
            inSettleMain.PatInHosId = inReimPara.PatInfo.PatInHosId;
            inSettleMain.SettleNo = dicSettleInfo["brjsh"]; //+ "|" + DateTime.Now.ToString("yyyyMMddHHmmss");                              //医保中心交易流水号
            inSettleMain.Amount = Convert.ToDecimal(dicSettleInfo["brfdje"]) + Convert.ToDecimal(dicSettleInfo["ybfdje"]) + Convert.ToDecimal(dicSettleInfo["yyfdje"]);       //本次医疗费用
            inSettleMain.MedAmountZhzf = Convert.ToDecimal(dicSettleInfo["grzhzf"]);//本次帐户支出
                                                                                    //inSettleMain.MedAmountTc = Convert.ToDecimal(dicSettleInfo["ybfdje"]);  //本次统筹支出
            inSettleMain.MedAmountTc = Convert.ToDecimal(dicSettleInfo["tczf"]);  //本次统筹支出
            inSettleMain.MedAmountDb = dicSettleInfoDibao.Count > 0 ? Convert.ToDecimal(dicSettleInfoDibao["AidPayment"]) + Convert.ToDecimal(dicSettleInfoDibao["AidCardPayment"]) :   //低保支付
            inSettleMain.MedAmountBz = Convert.ToDecimal(dicSettleInfo["ylbzje"]);  //本次大病支出
            inSettleMain.MedAmountJm = Convert.ToDecimal(dicSettleInfo["yljmje"]);  //减免金额
            inSettleMain.GetAmount = Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]) - inSettleMain.MedAmountDb;    //本次现金支出
            inSettleMain.CreateTime = DateTime.Now;
            inSettleMain.InvoiceId = -1;
            inSettleMain.IsCash = true;
            inSettleMain.IsInvalid = false;
            inSettleMain.IsNeedRefund = false;
            inSettleMain.IsRefundDo = false;
            inSettleMain.IsSettle = true;
            inSettleMain.MedAmountTotal = Convert.ToDecimal(inSettleMain.Amount) - Convert.ToDecimal(inSettleMain.GetAmount);
            inSettleMain.NetworkingPatClassId = Convert.ToInt32(inReimPara.CommPara.NetworkPatClassId);
            inSettleMain.NetworkPatName = inReimPara.PatInfo.InPatName;
            inSettleMain.NetworkPatType = dicSettleInfoDibao.Count > 0 ? "低保" : ""; //低保病人结算;
            inSettleMain.SettleBackNo = inReimPara.PatInfo.PatInHosCode;
            inSettleMain.SettleType = dicSettleInfoDibao.Count > 0 ? "1" : "0"; //低保结算标志
            inSettleMain.NetworkRefundTime = Convert.ToDateTime("2000-01-01");
            inSettleMain.NetworkSettleTime = DateTime.Now;
            inSettleMain.OperatorId = PayAPIConfig.Operator.UserSysId;//operatorInfo.UserSysId;
                                                                      //根据his版本决定是否恢复
                                                                      //inReimPara.SettleInfo.InNetworkSettleMainExtend = new InNetworkSettleMainExtend
                                                                      //{
                                                                      //    Memo6 = dicSettleInfo["tczf"].ToString(),//统筹支付
                                                                      //    Memo1 = Convert.ToDecimal(dicSettleInfo["dezf"].ToString()),//大额支付
                                                                      //    Memo2 = Convert.ToDecimal(dicSettleInfo["ecbxje"].ToString()),//二次报销金额
                                                                      //    Memo3 = dicSettleInfo["wjbfje"].ToString(),//卫计帮扶
                                                                      //    Memo4 = dicSettleInfo["mzbzje"].ToString(),//民政补助
                                                                      //    Memo5 = dicSettleInfo["pkrkbcbxje"].ToString()//商业兜底
                                                                      //};
            inReimPara.SettleInfo = inSettleMain;

            //保存结算数据
            IInClientBLL inBLl = PayAPIClassLib.Factory.ClientBLLFactory.GetInClientBLLInstance();
            //获取联网结算ID 并重新组织数据
            inBLl.GetInSettleIdAndReorganizeData(inReimPara);
            inBLl.SaveInNetworkSettleMain(inSettleMain);
            inBLl.SaveInNetworkSettleList(inReimPara.SettleParaList);//保存settleLis结算数据


            PayAPIInterface.Model.Comm.PayType payType;
            inReimPara.PayTypeList = new List<PayType>();
            payType = new PayAPIInterface.Model.Comm.PayType();
            payType.PayTypeId = 4;
            payType.PayTypeName = "医保";
            payType.PayAmount = Convert.ToDecimal(inSettleMain.MedAmountTotal) - inReimPara.SettleInfo.MedAmountZhzf - inReimPara.SettleInfo.MedAmountDb;
            inReimPara.PayTypeList.Add(payType);

            PayAPIInterface.Model.Comm.PayType payType1;

            payType1 = new PayAPIInterface.Model.Comm.PayType();
            payType1.PayTypeId = 5;
            payType1.PayTypeName = "医保卡";
            payType1.PayAmount = Convert.ToDecimal(inSettleMain.MedAmountZhzf);
            inReimPara.PayTypeList.Add(payType1);


            if (dicSettleInfoDibao.Count > 0)
            {
                PayAPIInterface.Model.Comm.PayType payType2;

                payType2 = new PayAPIInterface.Model.Comm.PayType();
                payType2.PayTypeId = 8;
                payType2.PayTypeName = "低保";
                payType2.PayAmount = Convert.ToDecimal(inReimPara.SettleInfo.MedAmountDb);
                inReimPara.PayTypeList.Add(payType2);
            }
            #endregion

        }
        #endregion

        #region 保存住院结算数据 作废
        /// <summary>
        /// 保存门诊结算数据
        /// </summary>
        //public void SaveInSettleMain()
        //{

        //    #region 保存中心返回值参数列表
        //    //保存中心返回值参数列表
        //    try
        //    {
        //        InNetworkSettleList inNetworkSettleList;
        //        foreach (var item in dicSettleInfo)
        //        {
        //            inNetworkSettleList = new InNetworkSettleList();
        //            inNetworkSettleList.PatInHosId = inReimPara.PatInfo.PatInHosId;
        //            inNetworkSettleList.InNetworkSettleId = -1; //inReimPara.CommPara.InNetworkSettleId;
        //            inNetworkSettleList.ParaName = item.Key.ToString();
        //            inNetworkSettleList.ParaValue = item.Value;
        //            inNetworkSettleList.Memo = "";
        //            inReimPara.SettleParaList.Add(inNetworkSettleList);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.Info("保存农合中心返回值参数列表 插入值 失败" + ex.Message);
        //    }
        //    #endregion

        //    InNetworkSettleMain inSettleMain = new InNetworkSettleMain();
        //    inSettleMain.PatInHosId = inReimPara.PatInfo.PatInHosId;
        //    inSettleMain.SettleNo = dicSettleInfo["brjsh"]; //+ "|" + DateTime.Now.ToString("yyyyMMddHHmmss");                              //医保中心交易流水号
        //    inSettleMain.Amount = Convert.ToDecimal(dicSettleInfo["brfdje"]) + Convert.ToDecimal(dicSettleInfo["ybfdje"]) + Convert.ToDecimal(dicSettleInfo["yljmje"]);       //本次医疗费用
        //    inSettleMain.GetAmount = Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]);    //本次现金支出
        //    inSettleMain.MedAmountZhzf = Convert.ToDecimal(dicSettleInfo["grzhzf"]);//本次帐户支出
        //    inSettleMain.MedAmountTc = Convert.ToDecimal(dicSettleInfo["ybfdje"]);  //本次统筹支出
        //    inSettleMain.MedAmountDb = 0;  //本次大病支出
        //    inSettleMain.MedAmountBz = Convert.ToDecimal(dicSettleInfo["ylbzje"]);  //本次大病支出
        //    inSettleMain.MedAmountJm = Convert.ToDecimal(dicSettleInfo["yljmje"]);  //减免金额
        //    inSettleMain.CreateTime = DateTime.Now;
        //    inSettleMain.InvoiceId = -1;
        //    inSettleMain.IsCash = true;
        //    inSettleMain.IsInvalid = false;
        //    inSettleMain.IsNeedRefund = false;
        //    inSettleMain.IsRefundDo = false;
        //    inSettleMain.IsSettle = true;
        //    inSettleMain.MedAmountTotal = Convert.ToDecimal(inSettleMain.Amount) - Convert.ToDecimal(inSettleMain.GetAmount);
        //    inSettleMain.NetworkingPatClassId = Convert.ToInt32(inReimPara.SettleInfo.NetworkingPatClassId);
        //    inSettleMain.NetworkPatName = netPatInfo.PatName;
        //    inSettleMain.NetworkPatType = "0";
        //    inSettleMain.NetworkRefundTime = Convert.ToDateTime("2000-01-01");
        //    inSettleMain.NetworkSettleTime = DateTime.Now;
        //    inSettleMain.OperatorId = PayAPIConfig.Operator.UserSysId;//operatorInfo.UserSysId;
        //    inSettleMain.SettleBackNo = "";
        //    inSettleMain.SettleType = "1";

        //    inReimPara.SettleInfo = inSettleMain;


        //    PayAPIInterface.Model.Comm.PayType payType;
        //    inReimPara.PayTypeList = new List<PayType>();
        //    payType = new PayAPIInterface.Model.Comm.PayType();
        //    payType.PayTypeId = 4;
        //    payType.PayTypeName = "医保";
        //    payType.PayAmount = Convert.ToDecimal(inSettleMain.MedAmountTotal);
        //    inReimPara.PayTypeList.Add(payType);

        //    payType = new PayAPIInterface.Model.Comm.PayType();
        //    payType.PayTypeId = 5;
        //    payType.PayTypeName = "医保卡";
        //    payType.PayAmount = Convert.ToDecimal(inSettleMain.MedAmountZhzf);
        //    inReimPara.PayTypeList.Add(payType);

        //}
        #endregion

        #region 门诊联网操作
        /// <summary>
        /// 读卡
        /// </summary>
        /// <returns></returns>
        public NetworkPatInfo NetworkReadCard()
        {
            //此段强制退费 正式需注释

            //  chexiaoqiang();





            InterfaceInit();
            NetworkPatInfo networkPatInfo = new NetworkPatInfo();

            P_syzhlx = "0";
            //strPayTypeId = "5";
            IsInReadCard = false;
            frmCARD frmCard = new frmCARD();
            frmCard.ShowDialog();
            bxlbjs = frmCard.Bxlb;
            bxlbmcjs = frmCard.Bxlbmc;

            //if (bxlbmcjs == "门诊门规")
            //{
            //    if (frmCard.iscard == "1")
            //    {
            //        CARD_Y_N = "1";
            //        //P_syzhlx = "3";
            //        patInfo = handelModel.ReadCardMG();
            //    }
            //    else if (frmCard.iscard == "0")
            //    {
            //        CARD_Y_N = "0";
            //        //P_syzhlx = "5";
            //        patInfo = handelModel.QueryBasicInfo(frmCard.IDNo, "", "4", "");//*医疗统筹类别(1,住院，4 门规)
            //    }
            //    else
            //    {
            //        throw new Exception("操作员取消本次操作");
            //    }
            //    IsInReadCard = true;
            //}
            //else if(bxlbmcjs == "门诊统筹" || bxlbmcjs == "")
            //{
            if (frmCard.iscard == "1")
            {
                CARD_Y_N = "1";
                patInfo = handelModel.ReadCardMZ();
            }
            else if (frmCard.iscard == "0")
            {
                CARD_Y_N = "0";
                patInfo = handelModel.QueryBasicInfo(frmCard.IDNo, "", "6", "");//*医疗统筹类别(1,住院，4 门规)
            }
            else
            {
                throw new Exception("操作员取消本次操作");
            }
            IsInReadCard = true;
            //}


            dicPatInfo = patInfo;
            networkPatInfo.MedicalNo = patInfo["ylzbh"];                   //医疗卡号
            networkPatInfo.PatName = patInfo["xm"];                        //姓名
            networkPatInfo.Sex = patInfo["xb"] == "1" ? "男" : "女";       //性别
            networkPatInfo.IDNo = patInfo["shbzhm"];//patInfo["sfzhm"];                        //身份证号码shbzhm
            networkPatInfo.MedicalTypeName = patInfo["ylrylb"];
            networkPatInfo.MedicalType = patInfo["ylrylb"];                //医疗人员类别
            networkPatInfo.ICAmount = Convert.ToDecimal(patInfo["ye"]);  //账户余额
            networkPatInfo.ICNo = "";//patInfo["kh"];                           //社会保障卡卡号
            networkPatInfo.CompanyNo = ""; //patInfo["sbjgbh"];                    //单位编号sbjbm
            networkPatInfo.CompanyName = patInfo["dwmc"];                  //单位名称
            //networkPatInfo.Birthday = Convert.ToDateTime(patInfo["csrq"].Substring(0, 4) + "-" + patInfo["csrq"].Substring(4, 2) + "-" + patInfo["csrq"].Substring(6, 2));                    //出生日期
            networkPatInfo.MedicalType = patInfo["ylrylb"];
            return networkPatInfo;

        }


        /// <summary>
        /// 临时强制退费方法 需注释掉
        /// </summary>
        //private void chexiaoqiang()
        //{
        //    #region 撤销结算时获取住院号


        //    InterfaceInit();
        //    //100002-001
        //    handelModel.CancleZYSettle("100002", "100002-001");//, inReimPara.SettleInfo.InNetworkSettleId.ToString());

        //    #endregion


        //}
        /// <summary>
        /// 门诊登记
        /// </summary>
        /// <param name="para"></param>
        public void OutNetworkRegister(OutPayParameter para)
        {
            throw new Exception("该类别为住院类业务");
            NetworkReadCard();
            //显示个人信息
            JiNan.Dialog.PersonInfoDialog perDialog = new JiNan.Dialog.PersonInfoDialog(patInfo);
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
            throw new Exception("该类别为住院类业务");

            outReimPara = para;
            //InterfaceInit();
            //handelModel.InitMZMG(para.RegInfo.CantonCode, "6", para.RegInfo.MemberNo, para.PatInfo.PatName,
            //                    patInfo["xb"], para.SettleInfo.OutNetworkSettleId.ToString(), DateTime.Now.ToString("yyyy-MM-dd"),//"001",
            //                    operatorInfo.UserSysId, strDiagnosCode, P_syzhlx, para.RegInfo.CardNo, "C", "");

            //门诊初始化
            //handelModel.InitMZ();
        }

        /// <summary>
        /// 门诊联网结算
        /// </summary>
        /// <param name="inPara">门诊接口入参</param>
        /// <returns></returns>
        public void OutNetworkSettle(OutPayParameter para)
        {
            throw new Exception("该类别为住院类业务");

            try
            {
                InterfaceInit();
                outReimPara = para;

                //当姓名不一致时提示
                if (outReimPara.PatInfo.PatName != dicPatInfo["xm"])
                {
                    if (MessageBox.Show(" 医保登记姓名为：【" + dicPatInfo["xm"].ToString() + "】     患者姓名为：【" + outReimPara.PatInfo.PatName + "】 是否继续 ", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        throw new Exception("姓名不一致，操作员取消操作！");
                    }
                }
                // if (bxlbmcjs == "门诊门规")
                // {
                //     //门诊初始化
                //     handelModel.InitMZMG(outReimPara.RegInfo.CantonCode, "4", outReimPara.RegInfo.MemberNo, outReimPara.PatInfo.PatName,
                //                         dicPatInfo["xb"], outReimPara.CommPara.OutNetworkSettleId.ToString(), DateTime.Now.ToString("yyyy-MM-dd"),//"001", 
                //                         PayAPIConfig.Operator.UserSysId, strDiagnosCode, P_syzhlx, outReimPara.RegInfo.CardNo, "C", "");
                // }  
                //  else if(bxlbmcjs == "门诊统筹" && bxlbmcjs == "")
                //{
                //门诊初始化
                //handelModel.InitZGMZ(outReimPara.RegInfo.CantonCode, "6", outReimPara.RegInfo.MemberNo, outReimPara.PatInfo.PatName,
                //                        dicPatInfo["xb"], outReimPara.CommPara.OutNetworkSettleId.ToString(), DateTime.Now.ToString("yyyy-MM-dd"),//"001", 
                //                        PayAPIConfig.Operator.UserSysId, strDiagnosCode, P_syzhlx, outReimPara.RegInfo.CardNo, "C", "");
                //} 
                string notMatchedCharge = "";
                foreach (var item in outReimPara.Details)
                {
                    if (item.NetworkItemCode.ToString().Trim().Length == 0 || "0000".Equals(item.NetworkItemCode))
                    {
                        notMatchedCharge += "编码:" + item.ChargeCode + "," + "名称:" + item.ChargeName + "；";
                    }
                }
                if (notMatchedCharge.Trim().Length > 0)
                {
                    throw new Exception("有以下项目未对应：\n" + notMatchedCharge + "取消上传费用");


                }
                handelModel.InitZGMZ(outReimPara.RegInfo.CantonCode, "6", outReimPara.PatInfo.PatName, dicPatInfo["xb"],
                                     outReimPara.RegInfo.MemberNo, outReimPara.CommPara.OutNetworkSettleId.ToString(),
                                     DateTime.Now.ToString("yyyy-MM-dd"), PayAPIConfig.Operator.UserSysId, "", "0", "6");
                //上传门诊费用
                handelModel.SaveOutItems(outReimPara);
                //门诊结算
                dicSettleInfo = handelModel.SettleMG(dicPatInfo["sbjglx"]);
                //保存门诊结算明细
                SaveOutSettleMain();

                GC.KeepAlive(handelModel);
                GC.Collect();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
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


            //普通职工统筹
            while (true)
            {
                try
                {
                    NetworkReadCard();
                    string ylzbh = "";//outReimPara.RegInfo.CardNo;
                    string syzhlx = "5";

                    MSSQLHelper sqlhelper = new MSSQLHelper("Data Source=172.22.25.6;Initial Catalog=COMM;User ID=power;Password=m@ssuns0ft009");

                    string bh1 = outReimPara.SettleInfo.RelationId.ToString("0"); //outReimPara.CommPara.OutNetworkSettleId.ToString();
                    string bh2 = outReimPara.SettleInfo.NetworkPatName;
                    //select * from MZ.OUT.OUT_NETWORK_REGISTERS   where   OUT_NETWORK_SETTLE_ID  ='12' and  NET_PAT_NAME='刘洪英'

                    string sqlChr = "select * from MZ.OUT.OUT_NETWORK_REGISTERS   where   OUT_NETWORK_SETTLE_ID  =" + bh1 + " and  NET_PAT_NAME=  '" + bh2 + "'";
                    //  string bh3=outReimPara.SettleInfo.

                    if (sqlhelper.ExecSqlReDs(sqlChr).Tables[0].Rows.Count < 1)
                    {
                        throw new Exception("退费关联ID异常，无法撤销结算 ！");
                    }

                    string regInfoCardNo = "";
                    string regInfoCantonCode = "";
                    string regInfoMemberNo = "";
                    DataTable dtc = sqlhelper.ExecSqlReDs(sqlChr).Tables[0];
                    foreach (DataRow item in dtc.Rows)
                    {
                        regInfoCantonCode = item["CANTON_CODE"].ToString();
                        regInfoCardNo = item["CARD_NO"].ToString();
                        regInfoMemberNo = item["MEMBER_NO"].ToString();
                    }

                    if (string.IsNullOrEmpty(regInfoCantonCode) || string.IsNullOrEmpty(regInfoMemberNo))
                    {
                        throw new Exception("接口 提示 登记表统筹区号参数或个人编号参数为空  无法撤销结算");
                    }


                    handelModel.InitZGMZ(regInfoCantonCode, "6", outReimPara.PatInfo.PatName, dicPatInfo["xb"],
                          regInfoMemberNo, outReimPara.CommPara.OutNetworkSettleId.ToString(),
                           DateTime.Now.ToString("yyyy-MM-dd"), PayAPIConfig.Operator.UserSysId, "", "0", "6");
                    LogManager.Debug("开始撤销职工门诊结算，流水号：" + settleNo);
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


        #region 住院联网操作
        /// <summary>
        /// 住院读卡
        /// </summary>
        /// <returns></returns>
        public void InNetWorkReadCard(InPayParameter para)
        {
            Dictionary<string, string> patInfo = new Dictionary<string, string>();

            string quciclyNum = "";
            quciclyNum = para.PatInfo.IDNo;
            //判断有卡无卡
            frmCARD frmCard = new frmCARD(quciclyNum);
            frmCard.ShowDialog();
            if (frmCard.iscard == "1")
            {
                CARD_Y_N = "1";
                patInfo = handelModel.ReadCardZY();
            }
            else if (frmCard.iscard == "0")
            {
                CARD_Y_N = "0";
                patInfo = handelModel.QueryBasicInfo(frmCard.IDNo, "", "1", "");//*医疗统筹类别(1,住院，4 门规)
            }
            else
            {
                throw new Exception("操作员取消本次操作");
            }

            dicPatInfo = patInfo;

            if (dicPatInfo["zhzybz"] == "1")
            {
                MessageBox.Show(dicPatInfo["zhzysm"]);
            }
            if (patInfo["ylrylb"].Contains("居民"))
            {
                throw new Exception("提示：该患者人员类别属于居民，但住院信息中费别为职工，请取消操作并调整患者费别！");
            }
            //显示个人信息
            JiNan.Dialog.PersonInfoDialog perDialog = new JiNan.Dialog.PersonInfoDialog(patInfo);
            perDialog.ShowDialog();
            strDiagnosCode = perDialog.strDiagnosCode;
            if (perDialog.isCancel)
            {
                throw new Exception("取消操作");
            }
            inReimPara.RegInfo.CardNo = patInfo["ylzbh"];                   //医疗卡号
            inReimPara.RegInfo.NetRegSerial = patInfo["sbjglx"];              //卡序列号
            inReimPara.RegInfo.CantonCode = patInfo["sbjbm"];              //行政区号
            inReimPara.RegInfo.MemberNo = patInfo["ylzbh"];                //成员编码
            inReimPara.RegInfo.CompanyName = patInfo["dwmc"];               //单位名称
            inReimPara.RegInfo.PatAddress = patInfo["dwmc"];                   //住址
            inReimPara.RegInfo.IdNo = patInfo["shbzhm"];                    //身份证号
            inReimPara.RegInfo.NetPatType = patInfo["ylrylb"];             //人员类别 
            inReimPara.RegInfo.Memo1 = patInfo["xb"] == "1" ? "男" : "女";                         //性别
            inReimPara.RegInfo.NetPatName = patInfo["xm"]; //姓名
            inReimPara.RegInfo.Memo2 = perDialog.reDiBao;//是否低保
            string rqlb = "";
            if (dicPatInfo["sbjglx"] == "A")
            {
                rqlb = "职工";
            }
            else if (dicPatInfo["sbjglx"] == "B")
            {
                rqlb = "居民";
            }
            inReimPara.RegInfo.NetPatType = rqlb;                                              //人员
            inReimPara.RegInfo.NetType = "1";
            //当姓名不一致时提示
            if (inReimPara.RegInfo.NetPatName != patInfo["xm"])
            {
                if (MessageBox.Show(" 医保登记姓名为：【" + patInfo["xm"].ToString() + "】     患者姓名为：【" + inReimPara.RegInfo.NetPatName + "】 是否继续 ", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    throw new Exception("姓名不一致，操作员取消操作！");
                }
            }

            //inReimPara.RegInfo.NetPatName = patInfo["xm"];                  //姓名

     

         
        }

        /// <summary>
        /// 住院登记
        /// </summary>
        /// <param name="inPara">住院接口入参</param>
        /// <returns></returns>
        public void InNetworkRegister(InPayParameter para)
        {
            InterfaceInit();
            inReimPara = para;
            InNetWorkReadCard(inReimPara);
            try
            {
                handelModel.SaveZYDJ(inReimPara.PatInfo.PatInHosCode,
                                    dicPatInfo["shbzhm"],
                                    dicPatInfo["ylzbh"],
                                    dicPatInfo["xm"],
                                    dicPatInfo["xb"],
                                    "1",//   string            *住院类别 1:住院 2:家床
                                    dicPatInfo["sbjbm"],
                                    "3",//(CARD_Y_N == "0" ? "0" : "3"),     //  *使用医保卡类型（0:不使用医保卡 ,1银行卡,2 cpu 卡，3，济南医保卡） string
                                    inReimPara.PatInfo.InDeptCode.ToString(),               //需修改//varchar2(20)       *科室编码
                                    inReimPara.PatInfo.InDateTime.ToString("yyyy-MM-dd"),//datetime           *住院日期
                                    "",//string              确诊医师
                                    "",//  varchar2(20)        门诊科室
                                    "1",
                                    "C",
                                    "");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.InnerException.Message);
                throw ex;
            }
            IsInReadCard = false;
        }

        /// <summary>
        /// 取消住院登记
        /// </summary>
        /// <param name="inPara">住院接口入参</param>
        /// <returns></returns>
        public void CancelInNetworkRegister(InPayParameter para)
        {
            InterfaceInit();
            inReimPara = para;
            handelModel.DelAllInItems(inReimPara.PatInfo.PatInHosCode);
            handelModel.CancelZY(inReimPara.PatInfo.PatInHosCode);
            isInit = false;

        }

        /// <summary> 
        /// 住院预结算
        /// </summary>
        /// <param name="inPara">住院接口入参</param>
        /// <returns></returns>
        public void InNetworkPreSettle(InPayParameter para)
        {


            inReimPara = para;

            if (inReimPara.SettleInfo != null && inReimPara.SettleInfo.MedAmountTotal != 0)
            {
                PayAPIInterface.Model.Comm.PayType payType;
                inReimPara.PayTypeList = new List<PayType>();
                payType = new PayAPIInterface.Model.Comm.PayType();
                payType.PayTypeId = 4;
                payType.PayTypeName = "医保";
                payType.PayAmount = inReimPara.SettleInfo.MedAmountTotal - inReimPara.SettleInfo.MedAmountZhzf;
                inReimPara.PayTypeList.Add(payType);
                PayAPIInterface.Model.Comm.PayType payType1;
                payType1 = new PayAPIInterface.Model.Comm.PayType();
                payType1.PayTypeId = 5;
                payType1.PayTypeName = "医保卡";
                payType1.PayAmount = inReimPara.SettleInfo.MedAmountZhzf;
                inReimPara.PayTypeList.Add(payType1);
                LogManager.Debug("检测到结算表里有值跳过医保厂商结算 易宝支付：" + payType.PayAmount + " 医保卡支付:" + payType1.PayAmount + "");

                return;
            }

            InterfaceInit();

            Form2 a = new Form2();
            a.Details = inReimPara.Details;

            a.ShowDialog();
            inReimPara.Details = a.Details;
            if (a.ispreCancle)
            {
                LogManager.Debug("用户取消了操作，进程将关闭");

                MessageBox.Show("用户取消了操作，进程将关闭");
                System.Environment.Exit(0);
            }

            string[] arr = new string[14];
            arr[0] = "病人结算号";
            arr[1] = "病人负担金额";
            arr[2] = "医保负担金额";
            arr[3] = "医疗补助金额";
            arr[4] = "个人账户支付";
            arr[5] = "医疗减免金额";
            arr[6] = "医院负担金额";
            arr[7] = "超标床位费";
            arr[8] = "统筹支付";
            arr[9] = "大额支付";
            arr[10] = "二次报销金额";
            arr[11] = "卫计帮扶";
            arr[12] = "民政补助";
            arr[13] = "贫困人口补偿";






            string notMatchedCharge = "";
            foreach (var item in inReimPara.Details)
            {
                if (item.NetworkItemCode.ToString().Trim().Length == 0)
                {
                    notMatchedCharge += "编码:" + item.ChargeCode + "," + "名称:" + item.ChargeName + "；";
                }
            }
            if (notMatchedCharge.Trim().Length > 0)
            {
                throw new Exception("有以下项目未对应：\n" + notMatchedCharge + "取消上传费用");

                //MessageBox.Show("有以下项目未对应：\n" + notMatchedCharge + "\n不能收费报销！", "提示", MessageBoxButtons.OK);

                //throw new Exception("项目未对照");

            }

            handelModel.InitZY(inReimPara.PatInfo.PatInHosCode);

            handelModel.DelAllInItems(inReimPara.PatInfo.PatInHosCode);
            if (inReimPara.Details.Count > 0)
            {
                handelModel.SaveInItems(inReimPara.Details,
                    //"001", // 医师编码
                                         inReimPara.PatInfo.DoctorCode,
                                         inReimPara.PatInfo.OutDateTime.ToString("yyyy-MM-dd"),  //费用发生日期
                                         inReimPara.PatInfo.PatInHosCode
                                         );

                Formsfyybk yibaoka = new Formsfyybk();
                yibaoka.ShowDialog();
                if (yibaoka.sfyybk)
                //if (MessageBox.Show("是否有医保卡？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    yibaoka.Close();
                    dicSettleInfo = handelModel.SettleZY("");


                    //---------------------------------------------低保结算
                    #region 低保结算

                    if (Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]) > 0) //如果自负金额大于0弹出是否低保结算提示
                    {

                        if (inReimPara.RegInfo.Memo2 == "低保")
                        {
                            dicSettleInfoDibao.Clear();
                            PayAPIInstance.Dareway.JiNan.Dialog.DiBaoJS_Confirm_zy diBaoJS;
                            try
                            {
                                diBaoJS = new PayAPIInstance.Dareway.JiNan.Dialog.DiBaoJS_Confirm_zy(inReimPara, dicSettleInfo, dicSettleInfoDibao, hosId, hosOperatorName, hosOperatorSysid);
                                diBaoJS.ShowDialog();
                            }
                            catch (Exception ex)
                            {
                                while (true)
                                {
                                    try
                                    {
                                        CancelReasonYB(inReimPara.PatInfo.PatInHosCode, dicSettleInfo["brjsh"]);

                                        break;
                                    }
                                    catch (Exception ex1)
                                    {
                                        if (MessageBox.Show("撤销医保住院结算失败 错误提示" + ex1.Message + "  是否重新撤销", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                                        {
                                            throw new Exception("请手动在地维中撤销出院结算,避免重复结算,操作员放弃了重新撤销医保结算,请查验低保是否结算");
                                        }
                                        //throw;
                                    }
                                }
                                throw new Exception("低保结算错误:" + ex.ToString());

                            }


                            if (diBaoJS.isCancel)
                            {
                                while (true)
                                {
                                    try
                                    {
                                        CancelReasonYB(inReimPara.PatInfo.PatInHosCode, dicSettleInfo["brjsh"]);

                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        if (MessageBox.Show("撤销医保住院结算失败 错误提示" + ex.Message + "  是否重新撤销", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                                        {
                                            throw new Exception("请手动在地维中撤销出院结算,避免重复结算,操作员放弃了重新撤销医保结算,低保已撤销");
                                        }
                                        //throw;
                                    }
                                }
                                throw new Exception("操作员取消了操作,低保以及医保结算已经撤销出院");
                            }
                        }

                    }
                    #endregion

                    SaveInSettleMain();
                    #region 作废
                    //SaveInSettleMain();
                    //保存住院结算数据
                    //#region 保存中心返回值参数列表
                    ////保存中心返回值参数列表
                    //#region 保存SettleList值
                    //try
                    //{
                    //    int abc = 0;
                    //    InNetworkSettleList inNetworkSettleList = new InNetworkSettleList();
                    //    foreach (var item in dicSettleInfo)
                    //    {
                    //        inNetworkSettleList = new InNetworkSettleList();
                    //        inNetworkSettleList.PatInHosId = inReimPara.PatInfo.PatInHosId;
                    //        inNetworkSettleList.InNetworkSettleId = -1;
                    //        inNetworkSettleList.ParaName = item.Key;
                    //        inNetworkSettleList.ParaValue = item.Value.ToString();
                    //        inNetworkSettleList.Memo = arr[abc];
                    //        inReimPara.SettleParaList.Add(inNetworkSettleList);
                    //        abc++;
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    LogManager.Error("保存农合中心返回值参数列表 插入值 失败" + ex.Message, ex);
                    //}
                    //#endregion
                    //InNetworkSettleMain inSettleMain = new InNetworkSettleMain();
                    //inSettleMain.PatInHosId = inReimPara.PatInfo.PatInHosId;
                    //inSettleMain.SettleNo = dicSettleInfo["brjsh"] + "|" + DateTime.Now.ToString("yyyyMMddHHmmss");                              //医保中心交易流水号
                    //inSettleMain.Amount = Convert.ToDecimal(dicSettleInfo["brfdje"]) + Convert.ToDecimal(dicSettleInfo["ybfdje"]) + Convert.ToDecimal(dicSettleInfo["yljmje"]);       //本次医疗费用
                    //inSettleMain.GetAmount = Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]);    //本次现金支出
                    //inSettleMain.MedAmountZhzf = Convert.ToDecimal(dicSettleInfo["grzhzf"]);//本次帐户支出
                    //inSettleMain.MedAmountTc = Convert.ToDecimal(dicSettleInfo["ybfdje"]);  //本次统筹支出
                    //inSettleMain.MedAmountDb = 0;  //本次大病支出
                    //inSettleMain.MedAmountBz = Convert.ToDecimal(dicSettleInfo["ylbzje"]);  //本次大病支出
                    //inSettleMain.MedAmountJm = Convert.ToDecimal(dicSettleInfo["yljmje"]);  //减免金额
                    //inSettleMain.MedAmountTotal = Convert.ToDecimal(inSettleMain.Amount) - Convert.ToDecimal(inSettleMain.GetAmount);
                    //inSettleMain.NetworkingPatClassId = Convert.ToInt32(inReimPara.CommPara.NetworkPatClassId);
                    //inSettleMain.NetworkPatName = inReimPara.PatInfo.InPatName;
                    //inSettleMain.NetworkPatType = "0";
                    //inSettleMain.SettleBackNo = inReimPara.PatInfo.PatInHosCode;
                    //inSettleMain.SettleType = "1";
                    //inReimPara.SettleInfo = inSettleMain;

                    ////保存结算数据
                    //IInClientBLL inBLl = PayAPIClassLib.Factory.ClientBLLFactory.GetInClientBLLInstance();
                    ////获取联网结算ID 并重新组织数据
                    //inBLl.GetInSettleIdAndReorganizeData(inReimPara);
                    //inBLl.SaveInNetworkSettleMain(inSettleMain);
                    //inBLl.SaveInNetworkSettleList(inReimPara.SettleParaList);//保存settleLis结算数据
                    ////门诊付费方式 本接口 4 医保 6农合
                    ////PayAPIInterface.Model.Comm.PayType payType;
                    ////inReimPara.PayTypeList = new List<PayType>();
                    ////payType = new PayAPIInterface.Model.Comm.PayType();
                    ////payType.PayTypeId = 4;
                    ////payType.PayTypeName = "医保";
                    ////payType.PayAmount = inReimPara.SettleInfo.MedAmountTotal - inSettleMain.MedAmountDb;
                    ////inReimPara.PayTypeList.Add(payType);

                    //PayAPIInterface.Model.Comm.PayType payType;
                    //inReimPara.PayTypeList = new List<PayType>();
                    //payType = new PayAPIInterface.Model.Comm.PayType();
                    //payType.PayTypeId = 4;
                    //payType.PayTypeName = "医保";
                    //payType.PayAmount = inReimPara.SettleInfo.MedAmountTotal - inReimPara.SettleInfo.MedAmountZhzf;
                    //inReimPara.PayTypeList.Add(payType);

                    //PayAPIInterface.Model.Comm.PayType payType1;
                    //payType1 = new PayAPIInterface.Model.Comm.PayType();
                    //payType1.PayTypeId = 5;
                    //payType1.PayTypeName = "医保卡";
                    //payType1.PayAmount = inReimPara.SettleInfo.MedAmountZhzf;
                    //inReimPara.PayTypeList.Add(payType1);
                    //#endregion 
                    #endregion
                }
                else
                {
                    yibaoka.Close();
                    dicSettleInfo = handelModel.SettleZY("wkzy");

                    //---------------------------------------------低保结算
                    #region 低保结算

                    if (Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]) > 0) //如果自负金额大于0弹出是否低保结算提示
                    {

                        if (inReimPara.RegInfo.Memo2 == "低保")
                        {
                            dicSettleInfoDibao.Clear();
                            PayAPIInstance.Dareway.JiNan.Dialog.DiBaoJS_Confirm_zy diBaoJS;
                            try
                            {
                                diBaoJS = new PayAPIInstance.Dareway.JiNan.Dialog.DiBaoJS_Confirm_zy(inReimPara, dicSettleInfo, dicSettleInfoDibao, hosId, hosOperatorName, hosOperatorSysid);
                                diBaoJS.ShowDialog();
                            }
                            catch (Exception ex)
                            {
                                while (true)
                                {
                                    try
                                    {
                                        CancelReasonYB(inReimPara.PatInfo.PatInHosCode, dicSettleInfo["brjsh"]);

                                        break;
                                    }
                                    catch (Exception ex1)
                                    {
                                        if (MessageBox.Show("撤销医保住院结算失败 错误提示" + ex1.Message + "  是否重新撤销", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                                        {
                                            throw new Exception("请手动在地维中撤销出院结算,避免重复结算,操作员放弃了重新撤销医保结算,请查验低保是否结算");
                                        }
                                        //throw;
                                    }
                                }
                                throw new Exception("低保结算错误:" + ex.ToString());

                            }


                            if (diBaoJS.isCancel)
                            {
                                while (true)
                                {
                                    try
                                    {
                                        CancelReasonYB(inReimPara.PatInfo.PatInHosCode, dicSettleInfo["brjsh"]);

                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        if (MessageBox.Show("撤销医保住院结算失败 错误提示" + ex.Message + "  是否重新撤销", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                                        {
                                            throw new Exception("请手动在地维中撤销出院结算,避免重复结算,操作员放弃了重新撤销医保结算,低保已撤销");
                                        }
                                        //throw;
                                    }
                                }
                                throw new Exception("操作员取消了操作,低保以及医保结算已经撤销出院");
                            }
                        }

                    }
                    #endregion

                    SaveInSettleMain();
                    #region 作废
                    //保存住院结算数据
                    //#region 保存中心返回值参数列表
                    ////保存中心返回值参数列表
                    //#region 保存SettleList值
                    //try
                    //{
                    //    int abc = 0;
                    //    InNetworkSettleList inNetworkSettleList = new InNetworkSettleList();
                    //    foreach (var item in dicSettleInfo)
                    //    {
                    //        inNetworkSettleList = new InNetworkSettleList();
                    //        inNetworkSettleList.PatInHosId = inReimPara.PatInfo.PatInHosId;
                    //        inNetworkSettleList.InNetworkSettleId = -1;
                    //        inNetworkSettleList.ParaName = item.Key;
                    //        inNetworkSettleList.ParaValue = item.Value.ToString();
                    //        inNetworkSettleList.Memo = arr[abc];
                    //        inReimPara.SettleParaList.Add(inNetworkSettleList);
                    //        abc++;
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    LogManager.Error("保存农合中心返回值参数列表 插入值 失败" + ex.Message, ex);
                    //}
                    //#endregion
                    //InNetworkSettleMain inSettleMain = new InNetworkSettleMain();
                    //inSettleMain.PatInHosId = inReimPara.PatInfo.PatInHosId;
                    //inSettleMain.SettleNo = dicSettleInfo["brjsh"] + "|" + DateTime.Now.ToString("yyyyMMddHHmmss");                              //医保中心交易流水号
                    //inSettleMain.Amount = Convert.ToDecimal(dicSettleInfo["brfdje"]) + Convert.ToDecimal(dicSettleInfo["ybfdje"]) + Convert.ToDecimal(dicSettleInfo["yljmje"]);       //本次医疗费用
                    //inSettleMain.GetAmount = Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]);    //本次现金支出
                    //inSettleMain.MedAmountZhzf = Convert.ToDecimal(dicSettleInfo["grzhzf"]);//本次帐户支出
                    //inSettleMain.MedAmountTc = Convert.ToDecimal(dicSettleInfo["ybfdje"]);  //本次统筹支出
                    //inSettleMain.MedAmountDb = 0;  //本次大病支出
                    //inSettleMain.MedAmountBz = Convert.ToDecimal(dicSettleInfo["ylbzje"]);  //本次大病支出
                    //inSettleMain.MedAmountJm = Convert.ToDecimal(dicSettleInfo["yljmje"]);  //减免金额
                    //inSettleMain.MedAmountTotal = Convert.ToDecimal(inSettleMain.Amount) - Convert.ToDecimal(inSettleMain.GetAmount);
                    //inSettleMain.NetworkingPatClassId = Convert.ToInt32(inReimPara.CommPara.NetworkPatClassId);
                    //inSettleMain.NetworkPatName = inReimPara.PatInfo.InPatName;
                    //inSettleMain.NetworkPatType = "0";
                    //inSettleMain.SettleBackNo = inReimPara.PatInfo.PatInHosCode;
                    //inSettleMain.SettleType = "1";
                    //inReimPara.SettleInfo = inSettleMain;

                    ////保存结算数据
                    //IInClientBLL inBLl = PayAPIClassLib.Factory.ClientBLLFactory.GetInClientBLLInstance();
                    ////获取联网结算ID 并重新组织数据
                    //inBLl.GetInSettleIdAndReorganizeData(inReimPara);
                    //inBLl.SaveInNetworkSettleMain(inSettleMain);
                    //inBLl.SaveInNetworkSettleList(inReimPara.SettleParaList);//保存settleLis结算数据


                    //PayAPIInterface.Model.Comm.PayType payType;
                    //inReimPara.PayTypeList = new List<PayType>();
                    //payType = new PayAPIInterface.Model.Comm.PayType();
                    //payType.PayTypeId = 4;
                    //payType.PayTypeName = "医保";
                    //payType.PayAmount = inReimPara.SettleInfo.MedAmountTotal - inReimPara.SettleInfo.MedAmountZhzf;
                    //inReimPara.PayTypeList.Add(payType);

                    //PayAPIInterface.Model.Comm.PayType payType1;
                    //payType1 = new PayAPIInterface.Model.Comm.PayType();
                    //payType1.PayTypeId = 5;
                    //payType1.PayTypeName = "医保卡";
                    //payType1.PayAmount = inReimPara.SettleInfo.MedAmountZhzf;
                    //inReimPara.PayTypeList.Add(payType1);
                    //#endregion 
                    #endregion
                }


            }




        }

        /// <summary>
        /// 住院结算
        /// </summary>
        /// <param name="inPara">住院接口入参</param>
        /// <returns></returns>
        public void InNetworkSettle(InPayParameter para)
        {

            inReimPara = para;
            try
            {
                string sql = "select TOP 1 SETTLE_NO from ZY.[IN].IN_NETWORK_SETTLE_MAIN WHERE PAT_IN_HOS_ID=" + inReimPara.PatInfo.PatInHosId.ToString("0") + " AND SETTLE_NO!='' ORDER BY CREATE_TIME DESC";
                MSSQLHelper sqlhelper = new MSSQLHelper("Data Source=172.22.25.6;Initial Catalog=COMM;User ID=power;Password=m@ssuns0ft009");
                DataRowCollection drc = sqlhelper.ExecSqlReDs(sql).Tables[0].Rows;
                if (drc.Count > 0)
                {
                    string SETTLE_NO = drc[0][0].ToString();
                    // inReimPara.SettleInfo.MedAmountTotal = Convert.ToDecimal(Amount);
                    inReimPara.SettleInfo.SettleNo = SETTLE_NO;
                }

                if (inReimPara.SettleInfo != null && inReimPara.SettleInfo.SettleNo != "")//inReimPara.SettleInfo.MedAmountTotal     inReimPara.SettleInfo.SettleNo
                {
                    PayAPIInterface.Model.Comm.PayType payType;
                    inReimPara.PayTypeList = new List<PayType>();
                    payType = new PayAPIInterface.Model.Comm.PayType();
                    payType.PayTypeId = 4;
                    payType.PayTypeName = "医保";
                    payType.PayAmount = inReimPara.SettleInfo.MedAmountTotal - inReimPara.SettleInfo.MedAmountZhzf;
                    inReimPara.PayTypeList.Add(payType);

                    PayAPIInterface.Model.Comm.PayType payType1;
                    payType1 = new PayAPIInterface.Model.Comm.PayType();
                    payType1.PayTypeId = 5;
                    payType1.PayTypeName = "医保卡";
                    payType1.PayAmount = inReimPara.SettleInfo.MedAmountZhzf;
                    inReimPara.PayTypeList.Add(payType1);
                    LogManager.Debug("正式结算检测到结算表里有值跳过医保厂商结算 易宝支付：" + payType.PayAmount + " 医保卡支付:" + payType1.PayAmount + "");
                    return;
                }

                else
                {
                    throw new Exception("结算失败：找不到该患者的联网结算记录");
                }
            }
            catch (Exception ex2)
            {

                throw new  Exception(ex2.ToString());
            }




        }


        /// <summary>
        /// 由于低保结算没有完成造成的需要撤销医保结算
        /// </summary>
        /// <param name="patInHosCode"></param>
        /// <param name="settleNo"></param>
        public void CancelReasonYB(string patInHosCode, string settleNo)
        {
            InterfaceInit();

            handelModel.CancleZYSettle(patInHosCode, settleNo);//, inReimPara.SettleInfo.InNetworkSettleId.ToString());

        }


        /// <summary>
        /// 取消住院结算
        /// </summary>
        /// <param name="inPara">住院接口入参</param>
        /// <returns></returns>
        public void CancelInNetworkSettle(InPayParameter para)
        {

            inReimPara = para;

            //撤销低保结算
            #region 撤销低保结算
            if (inReimPara.SettleInfo.NetworkPatType.ToString() == "低保")
            {
                while (true)
                {
                    try
                    {
                        //修改连接字符串
                        PayAPIInstance.Dareway.JiNan.Dialog.DiBaoQxjsZy frmqxjs = new PayAPIInstance.Dareway.JiNan.Dialog.DiBaoQxjsZy(inReimPara, hosId, hosOperatorName, hosOperatorSysid);
                        frmqxjs.TopMost = true;
                        frmqxjs.ShowDialog();
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (MessageBox.Show("撤销住院低保结算失败 错误提示" + ex.Message + "  是否重新撤销", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            throw new Exception("操作员取消撤销结算");
                        }
                    }
                }
            }
            #endregion


            #region  撤销结算时获取住院号 可尝试是否可以注掉 原南部山区是由于inReimPara.SettleInfo.SettleBackNo 框架无法获取使用此段SQL来获取 SettleBackNo



            string sql = "SELECT SETTLE_BACK_NO  FROM  ZY.[IN].IN_NETWORK_SETTLE_MAIN WHERE SETTLE_BACK_NO!='' AND  SETTLE_NO='" + inReimPara.SettleInfo.SettleNo + "'";
            // MSSQLHelper sqlhelper = new MSSQLHelper("Data Source=172.18.0.25;Initial Catalog=COMM;User ID=sa;Password=m@ssunsoft009");
            MSSQLHelper sqlhelper = new MSSQLHelper("Data Source=172.22.25.6;Initial Catalog=COMM;User ID=power;Password=m@ssuns0ft009");


            DataRowCollection drc = sqlhelper.ExecSqlReDs(sql).Tables[0].Rows;
            if (drc.Count > 0)
            {
                string SETTLE_BACK_NO = drc[0][0].ToString();
                // inReimPara.SettleInfo.MedAmountTotal = Convert.ToDecimal(Amount);
                inReimPara.SettleInfo.SettleBackNo = SETTLE_BACK_NO;
            }
            #endregion
            string patInHosCode = inReimPara.SettleInfo.SettleBackNo;
            string settleNo = inReimPara.SettleInfo.SettleNo;

            #region 撤销医保结算
            while (true)
            {
                try
                {
                    InterfaceInit();

                    handelModel.CancleZYSettle(patInHosCode, settleNo);//, inReimPara.SettleInfo.InNetworkSettleId.ToString());
                    break;

                }
                catch (Exception ex)
                {
                    if (MessageBox.Show("撤销医保出院结算失败 错误提示" + ex.Message + "  是否重新撤销", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        throw new Exception("操作员取消撤销结算");
                    }
                }
            }  
            #endregion     //}
        }
        #endregion


        //public string HISDBStr = "Data Source=172.16.169.8;Initial Catalog=COMM;Persist Security Info=True;User ID=power;Password=m@ssunsoft009";


        #region 判断是否是急诊科费用
        private Boolean isJZ(string OUT_PAT_ID) //判断是否是急诊科费用
        {
            //if (OUT_PAT_ID == "")
            //{
            //    return false;
            //}
            //IRCIDareWaySoftModel.Tools.MSSQLHelper sqlhelper = new IRCIDareWaySoftModel.Tools.MSSQLHelper(HISDBStr);
            //DataTable dtinfo;

            ////-----------
            //string sql;
            //sql=" SELECT  * FROM   REPORT.DBO.YB_NETTPYE_LIMIT ";
            //dtinfo = sqlhelper.ExecSqlReDs(sql).Tables[0];
            //string deptIds = "";
            //int beginHour = 0;
            //int endHour = 0;
            //if (dtinfo.Rows.Count > 0)
            //{
            //    deptIds = dtinfo.Rows[0]["DEPT_IDS"].ToString();
            //    beginHour = Convert.ToInt16(dtinfo.Rows[0]["BTIME"]);
            //    endHour = Convert.ToInt16(dtinfo.Rows[0]["ETIME"]);
            //}
            ////判断是否是在夜间区间内
            //int curHour = System.DateTime.Now.Hour;

            //if (curHour >= beginHour || curHour <= endHour)
            //{
            //    return true;
            //}



            ////判断是否是急诊科费用

            //sql = "SELECT * FROM MZ.OUT.OUT_ORDER_CHARGE_TMP WHERE PAT_ID='" + OUT_PAT_ID + "' AND BILL_DEPT_ID IN(" + deptIds + ")   ";
            //dtinfo.Clear();
            //dtinfo = sqlhelper.ExecSqlReDs(sql).Tables[0];

            //if (dtinfo.Rows.Count > 0)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            //-----------------

            return false;

        }
        #endregion



    }
}

