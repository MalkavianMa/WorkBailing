using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using PayAPIResolver.Dareway.DIWEI;
using PayAPIInstance.Dareway.DIWEI.Dialog;
using PayAPIInterface;
using PayAPIInterface.Model.Comm;
using PayAPIUtilities.Config;
using PayAPIInterface.ParaModel;
using PayAPIInterface.Model.Out;
using PayAPIInterface.Model.In;
using PayAPIUtilities.Log;


namespace PayAPIInstance.Dareway.DIWEI
{
    /// <summary>
    /// 济南职工门规Source:南部山区 from:历下区
    /// </summary>
    public class JNDWInterfaceModel_ZGMG : IPayCompanyInterface
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
        /// 个人基本信息
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
        public static DarewayInterfaceResolver handelModel;
        //结算信息
        public Dictionary<string, string> dicSettleInfo = new Dictionary<string, string>();
        //患者信息
        public Dictionary<string, string> dicPatInfo = new Dictionary<string, string>();
        //疾病编码
        public string strDiagnosCode = "";

        public bool isInit = false;  //是否初始化

        public static string strPayTypeId = "6";  //固定支付方式
        /// <summary>
        ///  *使用账户类型;0 不使用,1银行卡,2 cpu 卡，3 联机卡
        /// </summary>
        public string P_syzhlx = "3";

        public string CARD_Y_N = "";//有卡读取或者无卡读取，0，为无卡，1为有卡
        /// <summary>
        /// 是否已经读取卡信息
        /// </summary>
        public bool IsOutReadCard = false;
        /// <summary>
        /// <summary>
        /// 构造函数
        /// </summary>
        public JNDWInterfaceModel_ZGMG()
        {
        }


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


        #region 保存门诊结算数据  含低保
        private void SaveOutSettleMain()
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
                    if (a > 13)
                    {
                        outNetworkSettleList.Memo = "索引出界,请查询文档";

                    }
                    else
                    {
                        outNetworkSettleList.Memo = arr[a];
                    }
                    a++;
                    outReimPara.SettleParaList.Add(outNetworkSettleList);
                }
            }
            catch (Exception ex)
            {
                LogManager.Info("保存中心返回值参数列表 插入值 失败" + ex.Message);
            }
            #endregion
            OutNetworkSettleMain outSettleMain = new OutNetworkSettleMain();
            outSettleMain.OutPatId = outReimPara.RegInfo.OutPatId;
            outSettleMain.SettleNo = dicSettleInfo["brjsh"];                           //医保中心交易流水号
            outSettleMain.Amount = Convert.ToDecimal(dicSettleInfo["brfdje"]) + Convert.ToDecimal(dicSettleInfo["ybfdje"]) + Convert.ToDecimal(dicSettleInfo["yljmje"]);       //本次医疗费用
            outSettleMain.MedAmountZhzf = Convert.ToDecimal(dicSettleInfo["grzhzf"]);//本次帐户支出
            outSettleMain.MedAmountTc = Convert.ToDecimal(dicSettleInfo["ybfdje"]);  //本次统筹支出
            outSettleMain.MedAmountDb = dicSettleInfoDibao.Count > 0 ? Convert.ToDecimal(dicSettleInfoDibao["AidPayment"]) + Convert.ToDecimal(dicSettleInfoDibao["AidCardPayment"]) : 0;  //低保支付
            outSettleMain.MedAmountBz = Convert.ToDecimal(dicSettleInfo["ylbzje"]);  //补助金额
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
            outSettleMain.NetworkPatType = dicSettleInfoDibao.Count > 0 ? "低保" : ""; //低保病人结算
            outSettleMain.SettleType = dicSettleInfoDibao.Count > 0 ? "1" : "0"; //低保结算标志
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
            payType.PayAmount = outReimPara.SettleInfo.MedAmountTotal - Convert.ToDecimal(outReimPara.SettleInfo.MedAmountDb);
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

        #region 保存门诊结算数据 作废
        /// <summary>
        /// 保存门诊结算数据
        /// </summary>
        //public void SaveOutSettleMain()
        //{

        //    string[] arr = new string[14];
        //    arr[0] = "病人结算号";
        //    arr[1] = "病人负担金额";
        //    arr[2] = "医保负担金额";
        //    arr[3] = "个人账户支付";
        //    arr[4] = "医疗补助金额";
        //    arr[5] = "医疗减免支付";
        //    arr[6] = "本次统筹支付";
        //    arr[7] = "本次大额支付";
        //    arr[8] = "暂缓支付";
        //    arr[9] = "累计统筹支付";
        //    arr[10] = "累计大额支付";
        //    arr[11] = "累计门诊额度";
        //    arr[12] = "个人自费累计";
        //    arr[13] = "其他统筹支付";




        //    #region 保存中心返回值参数列表
        //    //保存中心返回值参数列表
        //    try
        //    {
        //        int a = 0;
        //        OutNetworkSettleList outNetworkSettleList;
        //        foreach (var item in dicSettleInfo)
        //        {
        //            outNetworkSettleList = new OutNetworkSettleList();
        //            outNetworkSettleList.OutPatId = outReimPara.PatInfo.OutPatId;
        //            outNetworkSettleList.OutNetworkSettleId = outReimPara.CommPara.OutNetworkSettleId;
        //            outNetworkSettleList.ParaName = item.Key.ToString();
        //            outNetworkSettleList.ParaValue = item.Value;
        //            if (a>13)
        //            {
        //                outNetworkSettleList.Memo = "索引出界,请查询文档";

        //            }
        //            else
        //            {
        //                outNetworkSettleList.Memo = arr[a];
        //            }
        //            outReimPara.SettleParaList.Add(outNetworkSettleList);
        //            a++;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.Info("保存中心返回值参数列表 插入值 失败" + ex.Message);
        //    }
        //    #endregion

        //    OutNetworkSettleMain outSettleMain = new OutNetworkSettleMain();
        //    outSettleMain.OutPatId = outReimPara.PatInfo.OutPatId;
        //    outSettleMain.SettleNo = dicSettleInfo["brjsh"];//+ "|" + DateTime.Now.ToString("yyyyMMddHHmmss");                           //医保中心交易流水号
        //    outSettleMain.Amount = Convert.ToDecimal(dicSettleInfo["brfdje"]) + Convert.ToDecimal(dicSettleInfo["ybfdje"]) + Convert.ToDecimal(dicSettleInfo["yljmje"]);       //本次医疗费用
        //    outSettleMain.GetAmount = Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]);    //本次现金支出
        //    outSettleMain.MedAmountZhzf = Convert.ToDecimal(dicSettleInfo["grzhzf"]);//本次帐户支出
        //    outSettleMain.MedAmountTc = Convert.ToDecimal(dicSettleInfo["ybfdje"]);  //本次统筹支出
        //    outSettleMain.MedAmountDb = 0;  //本次大病支出
        //    outSettleMain.MedAmountBz = Convert.ToDecimal(dicSettleInfo["ylbzje"]);  //本次大病支出
        //    outSettleMain.MedAmountJm = Convert.ToDecimal(dicSettleInfo["yljmje"]);  //减免金额
        //    outSettleMain.CreateTime = DateTime.Now;
        //    outSettleMain.InvoiceId = -1;
        //    outSettleMain.IsCash = true;
        //    outSettleMain.IsInvalid = false;
        //    outSettleMain.IsNeedRefund = false;
        //    outSettleMain.IsRefundDo = false;
        //    outSettleMain.IsSettle = true;
        //    outSettleMain.MedAmountTotal = Convert.ToDecimal(outSettleMain.Amount) - Convert.ToDecimal(outSettleMain.GetAmount);
        //    outSettleMain.NetworkingPatClassId = Convert.ToInt32(outReimPara.SettleInfo.NetworkingPatClassId);
        //    outSettleMain.NetworkPatName = dicPatInfo["xm"];
        //    outSettleMain.NetworkPatType = "0";
        //    outSettleMain.NetworkRefundTime = Convert.ToDateTime("2000-01-01");
        //    outSettleMain.NetworkSettleTime = DateTime.Now;
        //    outSettleMain.OperatorId = PayAPIConfig.Operator.UserSysId; //operatorInfo.UserSysId;
        //    outSettleMain.OutNetworkSettleId = Convert.ToDecimal(outReimPara.SettleInfo.OutNetworkSettleId);
        //    outSettleMain.SettleBackNo = "";


        //    outReimPara.SettleInfo = outSettleMain;

        //    PayAPIInterface.Model.Comm.PayType payType;
        //    outReimPara.PayTypeList = new List<PayType>();
        //    payType = new PayAPIInterface.Model.Comm.PayType();
        //    payType.PayTypeId = 4;
        //    payType.PayTypeName = "医保";
        //    payType.PayAmount = Convert.ToDecimal(outSettleMain.MedAmountTotal);
        //    outReimPara.PayTypeList.Add(payType);

        //}
        #endregion

        #region 保存门诊结算数据 账户支付 作废
        /// <summary>
        /// 保存门诊结算数据
        /// </summary>
        //public void SaveOutSettleMain_ZHZF()
        //{

        //    #region 保存中心返回值参数列表
        //    //保存中心返回值参数列表
        //    try
        //    {
        //        OutNetworkSettleList outNetworkSettleList;
        //        foreach (var item in dicSettleInfo)
        //        {
        //            outNetworkSettleList = new OutNetworkSettleList();
        //            outNetworkSettleList.OutPatId = outReimPara.PatInfo.OutPatId;
        //            outNetworkSettleList.OutNetworkSettleId = outReimPara.CommPara.OutNetworkSettleId;
        //            outNetworkSettleList.ParaName = item.Key.ToString();
        //            outNetworkSettleList.ParaValue = item.Value;
        //            outNetworkSettleList.Memo = "";
        //            outReimPara.SettleParaList.Add(outNetworkSettleList);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.Info("保存中心返回值参数列表 插入值 失败" + ex.Message);
        //    }
        //    #endregion

        //    OutNetworkSettleMain outSettleMain = new OutNetworkSettleMain();
        //    outSettleMain.OutPatId = outReimPara.PatInfo.OutPatId;
        //    outSettleMain.SettleNo = dicSettleInfo["mzzdlsh"];                           //医保中心交易流水号
        //    outSettleMain.Amount = Convert.ToDecimal(dicSettleInfo["zje"]);       //本次医疗费用
        //    outSettleMain.GetAmount = Convert.ToDecimal(dicSettleInfo["zje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]);    //本次现金支出
        //    outSettleMain.MedAmountZhzf = Convert.ToDecimal(dicSettleInfo["grzhzf"]);//本次帐户支出
        //    outSettleMain.MedAmountTc = 0;  //本次统筹支出
        //    outSettleMain.MedAmountDb = 0;  //本次大病支出
        //    outSettleMain.MedAmountBz = 0;  //本次大病支出
        //    outSettleMain.CreateTime = DateTime.Now;
        //    outSettleMain.InvoiceId = -1;
        //    outSettleMain.IsCash = true;
        //    outSettleMain.IsInvalid = false;
        //    outSettleMain.IsNeedRefund = false;
        //    outSettleMain.IsRefundDo = false;
        //    outSettleMain.IsSettle = true;
        //    outSettleMain.MedAmountTotal = Convert.ToDecimal(outSettleMain.Amount) - Convert.ToDecimal(outSettleMain.GetAmount);
        //    outSettleMain.NetworkingPatClassId = Convert.ToInt32(outReimPara.SettleInfo.NetworkingPatClassId);
        //    outSettleMain.NetworkPatName = dicPatInfo["xm"];
        //    outSettleMain.NetworkPatType = "0";
        //    outSettleMain.NetworkRefundTime = Convert.ToDateTime("2000-01-01");
        //    outSettleMain.NetworkSettleTime = DateTime.Now;
        //    outSettleMain.OperatorId = operatorInfo.UserSysId;
        //    outSettleMain.OutNetworkSettleId = Convert.ToDecimal(outReimPara.SettleInfo.OutNetworkSettleId);
        //    outSettleMain.SettleBackNo = "";
        //    outSettleMain.SettleType = "2";

        //    PayAPIInterface.Model.Comm.PayType payType;
        //    outReimPara.PayTypeList = new List<PayType>();
        //    payType = new PayAPIInterface.Model.Comm.PayType();
        //    payType.PayTypeId = 4;
        //    payType.PayTypeName = "医保";
        //    payType.PayAmount = Convert.ToDecimal(outReimPara.SettleInfo.MedAmountZhzf) + Convert.ToDecimal(outSettleMain.MedAmountTotal);
        //    outReimPara.PayTypeList.Add(payType);

        //    payType.PayTypeId = 5;
        //    payType.PayTypeName = "医保卡";
        //    payType.PayAmount = Convert.ToDecimal(outReimPara.SettleInfo.MedAmountZhzf);
        //    outReimPara.PayTypeList.Add(payType);


        //}
        #endregion

        #region 保存住院结算数据
        /// <summary>
        /// 保存门诊结算数据
        /// </summary>
        public void SaveInSettleMain()
        {

            #region 保存中心返回值参数列表
            //保存中心返回值参数列表
            try
            {
                InNetworkSettleList inNetworkSettleList = new InNetworkSettleList();
                foreach (var item in dicSettleInfo)
                {
                    inNetworkSettleList = new InNetworkSettleList();
                    inNetworkSettleList.PatInHosId = inReimPara.PatInfo.PatInHosId;
                    inNetworkSettleList.InNetworkSettleId = -1;
                    inNetworkSettleList.ParaName = item.Key;
                    inNetworkSettleList.ParaValue = item.Value.ToString();
                    inNetworkSettleList.Memo = "";
                    inReimPara.SettleParaList.Add(inNetworkSettleList);
                }
            }
            catch (Exception ex)
            {
                LogManager.Info("保存农合中心返回值参数列表 插入值 失败" + ex.Message);
            }
            #endregion

            InNetworkSettleMain inSettleMain = new InNetworkSettleMain();
            inSettleMain.PatInHosId = inReimPara.PatInfo.PatInHosId;
            inSettleMain.SettleNo = dicSettleInfo["brjsh"];                              //医保中心交易流水号
            inSettleMain.Amount = Convert.ToDecimal(dicSettleInfo["brfdje"]) + Convert.ToDecimal(dicSettleInfo["ybfdje"]) + Convert.ToDecimal(dicSettleInfo["ylbzje"]);       //本次医疗费用
            inSettleMain.GetAmount = Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]);    //本次现金支出
            inSettleMain.MedAmountZhzf = Convert.ToDecimal(dicSettleInfo["grzhzf"]);//本次帐户支出
            inSettleMain.MedAmountTc = Convert.ToDecimal(dicSettleInfo["ybfdje"]);  //本次统筹支出
            inSettleMain.MedAmountDb = 0;  //本次大病支出
            inSettleMain.MedAmountBz = Convert.ToDecimal(dicSettleInfo["ylbzje"]);  //本次大病支出
            inSettleMain.CreateTime = DateTime.Now;
            inSettleMain.InvoiceId = -1;
            inSettleMain.IsCash = true;
            inSettleMain.IsInvalid = false;
            inSettleMain.IsNeedRefund = false;
            inSettleMain.IsRefundDo = false;
            inSettleMain.IsSettle = true;
            inSettleMain.MedAmountTotal = Convert.ToDecimal(inSettleMain.Amount) - Convert.ToDecimal(inSettleMain.GetAmount);
            inSettleMain.NetworkingPatClassId = Convert.ToInt32(inReimPara.SettleInfo.NetworkingPatClassId);
            inSettleMain.NetworkPatName = netPatInfo.PatName;
            inSettleMain.NetworkPatType = "0";
            inSettleMain.NetworkRefundTime = Convert.ToDateTime("2000-01-01");
            inSettleMain.NetworkSettleTime = DateTime.Now;
            inSettleMain.OperatorId = operatorInfo.UserSysId;
            inSettleMain.SettleBackNo = "";
            inSettleMain.SettleType = "1";


            PayAPIInterface.Model.Comm.PayType payType;
            inReimPara.PayTypeList = new List<PayType>();
            payType = new PayAPIInterface.Model.Comm.PayType();
            payType.PayTypeId = 4;
            payType.PayTypeName = "医保";
            payType.PayAmount = inReimPara.SettleInfo.MedAmountTotal;
            inReimPara.PayTypeList.Add(payType);

            payType.PayTypeId = 5;
            payType.PayTypeName = "医保卡";
            payType.PayAmount = inReimPara.SettleInfo.MedAmountZhzf;
            inReimPara.PayTypeList.Add(payType);
        }
        #endregion

        /// <summary>
        /// 读卡
        /// </summary>
        /// <returns></returns>
        public NetworkPatInfo NetworkReadCard()
        {
            InterfaceInit();
            NetworkPatInfo networkPatInfo = new NetworkPatInfo();

            IsOutReadCard = false;

            //frmCARD frmCard = new Dialog.frmCARD();

            string quickIDnumber = "";

            if (inReimPara != null && outReimPara != null)
            {
                quickIDnumber = inReimPara.PatInfo.IDNo;

            }
            else
            {
                quickIDnumber = inReimPara == null ? outReimPara.PatInfo.IDNo : inReimPara.PatInfo.IDNo;
            }


            frmCARD frmCard = new frmCARD(quickIDnumber);
            frmCard.ShowDialog();
            if (frmCard.iscard == "1")
            {
                CARD_Y_N = "1";
                P_syzhlx = "3";
                patInfo = handelModel.ReadCardMG();
            }
            else if (frmCard.iscard == "0")
            {
                CARD_Y_N = "0";
                P_syzhlx = "5";
                patInfo = handelModel.QueryBasicInfo(frmCard.IDNo, "", "4", "");//*医疗统筹类别(1,住院，4 门规)
            }
            else
            {
                throw new Exception("操作员取消本次操作");
            }
            IsOutReadCard = true;
            dicPatInfo = patInfo;

            networkPatInfo.MedicalNo = patInfo["ylzbh"];                   //医疗卡号
            networkPatInfo.PatName = patInfo["xm"];                        //姓名
            networkPatInfo.Sex = patInfo["xb"] == "1" ? "男" : "女";       //性别
            networkPatInfo.IDNo = patInfo["shbzhm"];                        //身份证号码
            networkPatInfo.MedicalTypeName = patInfo["ylrylb"];
            networkPatInfo.MedicalType = patInfo["ylrylb"];                //医疗人员类别
            networkPatInfo.ICAmount = Convert.ToDecimal(patInfo["ye"]);  //账户余额
            networkPatInfo.ICNo = "";                           //社会保障卡卡号
            networkPatInfo.CompanyNo = "";                    //单位编号
            networkPatInfo.CompanyName = patInfo["dwmc"];                  //单位名称
            //networkPatInfo.Birthday = Convert.ToDateTime(patInfo["csrq"].Substring(0, 4) + "-" + patInfo["csrq"].Substring(4, 2) + "-" + patInfo["csrq"].Substring(6, 2));                    //出生日期
            networkPatInfo.MedicalType = patInfo["ylrylb"];
            return networkPatInfo;
        }

        #region 门诊联网操作
        /// <summary>
        /// 门诊登记
        /// </summary>
        /// <returns></returns>
        public void OutNetworkRegister(OutPayParameter para)
        {
            InterfaceInit();
            NetworkReadCard();
            //显示个人信息
            JiNan.Dialog.PersonInfoDialog perDialog = new JiNan.Dialog.PersonInfoDialog(patInfo);
            perDialog.ShowDialog();
            strDiagnosCode = perDialog.strDiagnosCode;
            if (perDialog.isCancel)
            {
                throw new Exception("取消操作");
            }
            //outReimPara.RegInfo.CardNo = patInfo["shbzhm"];                   //医疗卡号
            //outReimPara.RegInfo.NetPatType = patInfo["ylzbh"];              //卡序列号
            //outReimPara.RegInfo.CantonCode = patInfo["sbjbm"];              //行政区号
            //outReimPara.RegInfo.MemberNo = patInfo["shbzhm"];                //成员编码
            //outReimPara.RegInfo.CompanyName = patInfo["dwmc"];               //单位名称
            //outReimPara.RegInfo.PatAddress = patInfo["dwmc"];                   //住址
            //outReimPara.RegInfo.IdNo = patInfo["shbzhm"];                    //身份证号
            //outReimPara.RegInfo.Balance = Convert.ToDecimal(patInfo["ye"]);//账户余额
            //outReimPara.RegInfo.NetPatName = patInfo["xm"];                  //姓名
            //outReimPara.RegInfo.NetDiagnosCode = patInfo["mzdbjbs"];                         //

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
            para.RegInfo.NetDiagnosCode = patInfo["mzdbjbs"];                         //性别
            string rqlb = "";
            if (patInfo["sbjglx"] == "A")
            {
                rqlb = "职工";
            }
            else if (patInfo["sbjglx"] == "B")
            {
                rqlb = "居民";
            }

            para.RegInfo.NetDiagnosName = perDialog.strDiagnosName;
            para.RegInfo.NetPatType = rqlb;
            para.RegInfo.NetType = "4";///

        }

        /// <summary>
        /// 门诊联网预结算
        /// </summary>
        /// <returns></returns>
        public void OutNetworkPreSettle(OutPayParameter para)
        {
            //InterfaceInit();
            ////MessageBox.Show("INIT1");
            //handelModel.InitMZMG(outReimPara.RegInfo.CantonCode, "4", outReimPara.RegInfo.MemberNo, outReimPara.RegInfo.NetPatName,
            //                    "1", outReimPara.RegInfo.OutNetworkSettleId.ToString(), DateTime.Now.ToString("yyyy-MM-dd"),//"001", 
            //                    outReimPara.RegInfo.OperatorId,strDiagnosCode, P_syzhlx, outReimPara.RegInfo.CardNo, "C", "");


            outReimPara = para;
        }

        /// <summary>
        /// 门诊联网结算
        /// </summary>
        /// <param name="inPara">门诊接口入参</param>
        /// <returns></returns>
        public void OutNetworkSettle(OutPayParameter para)
        {
            try
            {
                InterfaceInit();
                outReimPara = para;

                //当姓名不一致时提示
                if (outReimPara.RegInfo.NetPatName != dicPatInfo["xm"])
                {
                    if (MessageBox.Show(" 医保登记姓名为：【" + dicPatInfo["xm"].ToString() + "】     患者姓名为：【" + outReimPara.RegInfo.NetPatName + "】 是否继续 ", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        throw new Exception("姓名不一致，操作员取消操作！");
                    }
                }
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
                    if (MessageBox.Show("有以下项目未对应：\n" + notMatchedCharge + "\n是否继续？选择”是“将按自费项目进行收费报销。否则，取消本次收费报销！", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        throw new Exception("取消上传费用");
                    }


                }//Malkavian1225
                //门诊初始化
                //handelModel.InitMZMG(outReimPara.RegInfo.CantonCode, "4", outReimPara.PatInfo.PatName, dicPatInfo["xb"],
                //                     outReimPara.RegInfo.MemberNo, para.RegInfo.CardNo,//outReimPara.CommPara.OutNetworkSettleId.ToString(),
                //                     DateTime.Now.ToString("yyyy-MM-dd"), PayAPIConfig.Operator.UserSysId, strDiagnosCode, P_syzhlx);
                handelModel.InitMZMG(outReimPara.RegInfo.CantonCode, outReimPara.CommPara.OutNetworkSettleId.ToString(), outReimPara.PatInfo.PatName, dicPatInfo["xb"],
                           outReimPara.RegInfo.MemberNo, para.RegInfo.CardNo,//outReimPara.CommPara.OutNetworkSettleId.ToString(),
                           DateTime.Now.ToString("yyyy-MM-dd"), PayAPIConfig.Operator.UserSysId, strDiagnosCode, P_syzhlx);
                //删除结算数据
                //DelReimSettleMainSQLite(outReimPara.RegInfo.OutPatId);

                handelModel.SaveOutItems(outReimPara);
                //门诊结算
                dicSettleInfo = handelModel.SettleMG(dicPatInfo["sbjglx"]);

                //---------------------------------------------低保结算

                if (Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]) > 0) //如果自负金额大于0弹出是否低保结算提示
                {

                    if (outReimPara.RegInfo.Memo2 == "低保")
                    {
                        dicSettleInfoDibao.Clear();
                        PayAPIInstance.Dareway.JiNan.Dialog.DiBaoJS_Confirm diBaoJS = new PayAPIInstance.Dareway.JiNan.Dialog.DiBaoJS_Confirm(outReimPara, dicSettleInfo, dicSettleInfoDibao, hosId);
                        diBaoJS.ShowDialog();
                    }
                }
                //------------------------------------------------
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
            //OutNetWorkReadCard(inPara);
            //撤销医保结算
            if (para.SettleInfo == null)
                throw new Exception("本地结算信息不存在");
            string settleNo = outReimPara.SettleInfo.SettleNo.ToString();

            //撤销低保结算
            #region 撤销低保结算
            if (outReimPara.SettleInfo.NetworkPatType.ToString() == "低保")
            {
                while (true)
                {
                    try
                    {
                        //修改连接字符串
                        PayAPIInstance.Dareway.JiNan.Dialog.DiBaoQXJS frmqxjs = new PayAPIInstance.Dareway.JiNan.Dialog.DiBaoQXJS(outReimPara.SettleInfo.RelationId);
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
            #endregion


            //撤销门诊大病结算
            while (true)
            {
                try
                {
           //         NetworkReadCard();
           //         //门诊初始化
           //         //handelModel.InitMZMG(outReimPara.RegInfo.CantonCode, outReimPara.CommPara.OutNetworkSettleId.ToString(), outReimPara.PatInfo.PatName, dicPatInfo["xb"],
           //         //                     outReimPara.RegInfo.MemberNo, outReimPara.CommPara.OutNetworkSettleId.ToString(),
           //         //                     DateTime.Now.ToString("yyyy-MM-dd"), PayAPIConfig.Operator.UserSysId, strDiagnosCode, P_syzhlx);

           //         handelModel.InitMZMG(outReimPara.RegInfo.CantonCode, outReimPara.CommPara.OutNetworkSettleId.ToString(), outReimPara.PatInfo.PatName, dicPatInfo["xb"],
           //outReimPara.RegInfo.MemberNo, para.RegInfo.CardNo,//outReimPara.CommPara.OutNetworkSettleId.ToString(),
           //DateTime.Now.ToString("yyyy-MM-dd"), PayAPIConfig.Operator.UserSysId, strDiagnosCode, P_syzhlx);
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

        #region 住院联网操作（无用）

        /// <summary>
        /// 住院读卡
        /// </summary>
        /// <returns></returns>
        public void InNetWorkReadCard(InPayParameter para)
        {

        }

        /// <summary>
        /// 住院登记
        /// </summary>
        /// <param name="inPara">住院接口入参</param>
        /// <returns></returns>
        public void InNetworkRegister(InPayParameter para)
        {
        }

        /// <summary>
        /// 取消住院登记
        /// </summary>
        /// <param name="inPara">住院接口入参</param>
        /// <returns></returns>
        public void CancelInNetworkRegister(InPayParameter para)
        {
        }

        /// <summary> 
        /// 住院预结算
        /// </summary>
        /// <param name="inPara">住院接口入参</param>
        /// <returns></returns>
        public void InNetworkPreSettle(InPayParameter para)
        {

        }

        /// <summary>
        /// 住院结算
        /// </summary>
        /// <param name="inPara">住院接口入参</param>
        /// <returns></returns>
        public void InNetworkSettle(InPayParameter para)
        {

        }

        /// <summary>
        /// 取消住院结算
        /// </summary>
        /// <param name="inPara">住院接口入参</param>
        /// <returns></returns>
        public void CancelInNetworkSettle(InPayParameter para)
        {

        }
        #endregion
    }
}
