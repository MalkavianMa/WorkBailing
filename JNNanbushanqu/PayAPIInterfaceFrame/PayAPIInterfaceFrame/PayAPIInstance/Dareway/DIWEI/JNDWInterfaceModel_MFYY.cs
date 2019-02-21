using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using PayAPIInterface;
using PayAPIInterface.Model.Comm;
using PayAPIInterface.ParaModel;
using PayAPIResolver.Dareway.DIWEI;
using PayAPIInterface.Model.Out;
using PayAPIUtilities.Log;
using PayAPIInterface.Model.In;
using PayAPIInstance.Dareway.DIWEI.Dialog;
using PayAPIUtilities.Config;
using PayAPIClassLib.BLL;

namespace PayAPIInstance.Dareway.DIWEI
{
    /// <summary>
    /// 职工和居民免费用药Source:南部山区 from历下区
    /// </summary>
    public class JNDWInterfaceModel_MFYY : IPayCompanyInterface
    {
        //医保个人信息 
        public NetworkPatInfo netPatInfo = new NetworkPatInfo();


        /// <summary>
        /// 个人基本信息
        /// </summary>
        Dictionary<string, string> patInfo = new Dictionary<string, string>();
        /// <summary>
        /// 门诊入参
        /// </summary>
        public OutPayParameter outReimPara;

        /// <summary>
        /// 当前操作员医院ID
        /// </summary>
        public string hosId = PayAPIConfig.Operator.HospitalId;
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


        //低保结算信息
        public Dictionary<string, string> dicSettleInfoDibao = new Dictionary<string, string>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public JNDWInterfaceModel_MFYY()
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


        #region 保存门诊结算数据 作废
        ///// <summary>
        ///// 保存门诊结算数据
        ///// </summary>
        //public void SaveOutSettleMain()
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
        //    outSettleMain.SettleNo = dicSettleInfo["brjsh"];                           //医保中心交易流水号
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
        //    LogManager.Debug("1免费用药医保支付" + payType.PayAmount);


        //}
        #endregion

        #region 保存门诊结算数据 免费用药 作废
        ///// <summary>
        ///// 保存门诊结算数据
        ///// </summary>
        //public void SaveOutSettleMain(string bz)
        //{
        //    string[] arr = new string[6];
        //    arr[0] = "病人结算号";
        //    arr[1] = "病人负担金额";
        //    arr[2] = "医保负担金额";
        //    arr[3] = "个人账户支付";
        //    arr[4] = "统筹支付";
        //    arr[5] = "账户支付";
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
        //            outNetworkSettleList.Memo = arr[a];
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
        //    outSettleMain.SettleNo = dicSettleInfo["brjsh"];                           //医保中心交易流水号
        //    outSettleMain.Amount = Convert.ToDecimal(dicSettleInfo["brfdje"]) + Convert.ToDecimal(dicSettleInfo["ybfdje"]);       //本次医疗费用
        //    outSettleMain.GetAmount = Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]);    //本次现金支出
        //    outSettleMain.MedAmountZhzf = Convert.ToDecimal(dicSettleInfo["grzhzf"]);//本次帐户支出
        //    outSettleMain.MedAmountTc = Convert.ToDecimal(dicSettleInfo["ybfdje"]);  //本次统筹支出
        //    outSettleMain.MedAmountDb = 0;  //本次大病支出
        //    outSettleMain.MedAmountBz = 0; //Convert.ToDecimal(dicSettleInfo["ylbzje"]);  //本次大病支出
        //    outSettleMain.MedAmountJm = Convert.ToDecimal(dicSettleInfo["zhzf"]);  //暂缓支付
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

        //    outSettleMain.SettleType = (bz == "Deduct" ? "10" : (bz == "JZ" ? "9" : "1")); //自定义暂存款SettleType为10,急诊的SettleType为9
        //    outReimPara.SettleInfo = outSettleMain;

        //    PayAPIInterface.Model.Comm.PayType payType;
        //    outReimPara.PayTypeList = new List<PayType>();
        //    payType = new PayAPIInterface.Model.Comm.PayType();
        //    payType.PayTypeId = 4;
        //    payType.PayTypeName = "医保";
        //    payType.PayAmount = Convert.ToDecimal(outSettleMain.MedAmountTotal);
        //    outReimPara.PayTypeList.Add(payType);
        //    LogManager.Debug("免费用药医保支付" + payType.PayAmount);
        //    //payType = new PayAPIInterface.Model.Comm.PayType();
        //    //payType.PayTypeId = 5;
        //    //payType.PayTypeName = "医保卡";
        //    //payType.PayAmount = Convert.ToDecimal(outReimPara.SettleInfo.MedAmountZhzf);
        //    //outReimPara.PayTypeList.Add(payType);


        //}
        #endregion

        #region 保存门诊结算数据 免费用药 含低保
        /// <summary>
        /// 保存门诊结算数据
        /// </summary>
        public void SaveOutSettleMain()
        {
            string[] arr = new string[6];
            arr[0] = "病人结算号";
            arr[1] = "病人负担金额";
            arr[2] = "医保负担金额";
            arr[3] = "个人账户支付";
            arr[4] = "统筹支付";
            arr[5] = "账户支付";
            #region 保存中心返回值参数列表
            //保存中心返回值参数列表
            try
            {
                OutNetworkSettleList outNetworkSettleList;
                int a = 0;
                foreach (var item in dicSettleInfo)
                {
                    outNetworkSettleList = new OutNetworkSettleList();
                    outNetworkSettleList.OutPatId = outReimPara.PatInfo.OutPatId;
                    outNetworkSettleList.OutNetworkSettleId = outReimPara.CommPara.OutNetworkSettleId;
                    outNetworkSettleList.ParaName = item.Key.ToString();
                    outNetworkSettleList.ParaValue = item.Value;
                    if (a<6)
                    {
                        outNetworkSettleList.Memo = arr[a];

                    }
                    else
                    {
                        outNetworkSettleList.Memo = "索引出界请查询文档";
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
            outSettleMain.OutPatId = outReimPara.PatInfo.OutPatId;
            outSettleMain.SettleNo = dicSettleInfo["brjsh"];                           //医保中心交易流水号
            outSettleMain.Amount = Convert.ToDecimal(dicSettleInfo["brfdje"]) + Convert.ToDecimal(dicSettleInfo["ybfdje"]);       //本次医疗费用
            outSettleMain.MedAmountZhzf = Convert.ToDecimal(dicSettleInfo["grzhzf"]);//本次帐户支出
            outSettleMain.MedAmountTc = Convert.ToDecimal(dicSettleInfo["ybfdje"]);  //本次统筹支出
            outSettleMain.MedAmountDb = dicSettleInfoDibao.Count > 0 ? Convert.ToDecimal(dicSettleInfoDibao["AidPayment"]) + Convert.ToDecimal(dicSettleInfoDibao["AidCardPayment"]) : 0;  //低保支付
            outSettleMain.MedAmountBz = 0; //Convert.ToDecimal(dicSettleInfo["ylbzje"]);  //本次大病支出
            outSettleMain.MedAmountJm = Convert.ToDecimal(dicSettleInfo["zhzf"]);  //暂缓支付
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
            payType.PayAmount = Convert.ToDecimal(outSettleMain.MedAmountTotal) - Convert.ToDecimal(outReimPara.SettleInfo.MedAmountDb);
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

            P_syzhlx = "3";
            //strPayTypeId = "5";
            IsInReadCard = false;
            frmCARD frmCard = new frmCARD();
            frmCard.ShowDialog();
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


        public NetworkPatInfo NetworkReadCard(OutPayParameter para)
        {
            InterfaceInit();
            NetworkPatInfo networkPatInfo = new NetworkPatInfo();

            P_syzhlx = "3";
            //strPayTypeId = "5";
            IsInReadCard = false;
            string quickIDnumber = "";

            if (para != null)
            {
                quickIDnumber = para.PatInfo.IDNo;
            }
            frmCARD frmCard = new frmCARD(quickIDnumber);
            frmCard.ShowDialog();
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
        /// 门诊登记
        /// </summary>
        /// <param name="para"></param>
        public void OutNetworkRegister(OutPayParameter para)
        {

            NetworkReadCard(para);
            //显示个人信息
            PayAPIInstance.Dareway.JiNan.Dialog.PersonInfoDialog perDialog = new JiNan.Dialog.PersonInfoDialog(patInfo);
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
            try
            {
                Dictionary<string, string> MFYYInfo = new Dictionary<string, string>();

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
                //获取免费用药余额
                MFYYInfo = handelModel.HqMfyyXx(outReimPara.RegInfo.CantonCode, outReimPara.RegInfo.IdNo, outReimPara.RegInfo.NetRegSerial);

                string MFYYSM = MFYYInfo["mfyysm"];
                //获取疾病信息
                Dialog.MfyyJblr frm = new MfyyJblr(MFYYSM);
                frm.ShowDialog();
                string Mfyybm = frm.MfyyJbbm;

                string ylzbh = "";
                string syzhlx = "5";
                if (CARD_Y_N == "1")
                {
                    ylzbh = outReimPara.RegInfo.CardNo;
                    syzhlx = "0";
                }
                //门诊初始化
                handelModel.InitMFYY(outReimPara.RegInfo.CantonCode, "6", outReimPara.RegInfo.MemberNo, outReimPara.PatInfo.PatName,
                                    dicPatInfo["xb"], outReimPara.CommPara.OutNetworkSettleId.ToString(), DateTime.Now.ToString("yyyy-MM-dd"),//"001", 
                                    PayAPIConfig.Operator.UserSysId, Mfyybm, syzhlx, ylzbh);


                //handelModel.SaveOutItemsMZ(outReimPara.Details);
                handelModel.SaveOutItems(outReimPara);
                //门诊结算
                dicSettleInfo = handelModel.SettleMFYY(dicPatInfo["sbjglx"]);
                //---------------------------------------------低保结算

                if (Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]) > 0) //如果自负金额大于0弹出是否低保结算提示
                {

                    if (outReimPara.RegInfo.Memo2 == "低保")
                    {
                        dicSettleInfoDibao.Clear();
                        PayAPIInstance.Dareway.JiNan.Dialog.DiBaoJS_Confirm diBaoJS = new PayAPIInstance.Dareway.JiNan.Dialog.DiBaoJS_Confirm(outReimPara, dicSettleInfo, dicSettleInfoDibao,hosId);
                        diBaoJS.ShowDialog();
                    }
                }
                //------------------------------------------------


                //保存门诊结算明细
               // SaveOutSettleMain("MZ");
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



            //撤销低保结算
            if (outReimPara.SettleInfo.NetworkPatType.ToString() == "低保")
            {
                while (true)
                {
                    try
                    {
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

            //撤销医保结算
            //撤销免费用药结算
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



        #region 住院业务(无用）

        /// <summary>
        /// 住院读卡
        /// </summary>
        /// <returns></returns>
        public void InNetWorkReadCard(InPayParameter para)
        {
            throw new Exception("该类别无住院业务");
        }

        /// <summary>
        /// 住院登记
        /// </summary>
        /// <param name="inPara">住院接口入参</param>
        /// <returns></returns>
        public void InNetworkRegister(InPayParameter para)
        {
            throw new Exception("该类别无住院业务");

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
            throw new Exception("该类别无住院业务");

        }

        /// <summary>
        /// 住院结算
        /// </summary>
        /// <param name="inPara">住院接口入参</param>
        /// <returns></returns>
        public void InNetworkSettle(InPayParameter para)
        {
            throw new Exception("该类别无住院业务");

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
