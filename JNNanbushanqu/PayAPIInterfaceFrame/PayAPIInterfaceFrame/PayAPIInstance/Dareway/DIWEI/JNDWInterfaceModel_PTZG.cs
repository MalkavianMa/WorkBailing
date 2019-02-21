using System;
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
    /// 济南职工普通门诊 Source:南部山区 from:历下区
    /// </summary>
    public class JNDWInterfaceModel_PTMZ : IPayCompanyInterface
    {
        //医保个人信息 
        public NetworkPatInfo netPatInfo = new NetworkPatInfo();

        /// <summary>
        /// 当前操作员医院ID
        /// </summary>
        public string hosId = PayAPIConfig.Operator.HospitalId;

        /// <summary>
        /// 病人信息
        /// </summary>
        Dictionary<string, string> patInfo = new Dictionary<string, string>();
        /// <summary>
        /// 门诊入参
        /// </summary>
        public OutPayParameter outReimPara;

        //低保结算信息
        public Dictionary<string, string> dicSettleInfoDibao = new Dictionary<string, string>();

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
        /// <summary>
        /// 构造函数
        /// </summary>
        public JNDWInterfaceModel_PTMZ()
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

        #region 保存门诊结算数据 账户支付 作废
        /// <summary>
        /// 保存门诊结算数据
        /// </summary>
        public void SaveOutSettleMain(string bz)
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
                int a = 0;
                OutNetworkSettleList outNetworkSettleList;
                foreach (var item in dicSettleInfo)
                {
                    outNetworkSettleList = new OutNetworkSettleList();
                    outNetworkSettleList.OutPatId = outReimPara.PatInfo.OutPatId;
                    outNetworkSettleList.OutNetworkSettleId = outReimPara.CommPara.OutNetworkSettleId;
                    outNetworkSettleList.ParaName = item.Key.ToString();
                    outNetworkSettleList.ParaValue = item.Value;
                    if (a < 6)
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


        #region 保存门诊结算数据 含低保

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
                int a = 0;
                OutNetworkSettleList outNetworkSettleList;
                foreach (var item in dicSettleInfo)
                {
                    outNetworkSettleList = new OutNetworkSettleList();
                    outNetworkSettleList.OutPatId = outReimPara.PatInfo.OutPatId;
                    outNetworkSettleList.OutNetworkSettleId = outReimPara.CommPara.OutNetworkSettleId;
                    outNetworkSettleList.ParaName = item.Key.ToString();
                    outNetworkSettleList.ParaValue = item.Value;
                    if (a < 6)
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
                InNetworkSettleList inNetworkSettleList;
                foreach (var item in dicSettleInfo)
                {
                    inNetworkSettleList = new InNetworkSettleList();
                    inNetworkSettleList.PatInHosId = inReimPara.PatInfo.PatInHosId;
                    inNetworkSettleList.InNetworkSettleId = -1; //inReimPara.CommPara.InNetworkSettleId;
                    inNetworkSettleList.ParaName = item.Key.ToString();
                    inNetworkSettleList.ParaValue = item.Value;
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
            inSettleMain.SettleNo = dicSettleInfo["brjsh"]; //+ "|" + DateTime.Now.ToString("yyyyMMddHHmmss");                              //医保中心交易流水号
            inSettleMain.Amount = Convert.ToDecimal(dicSettleInfo["brfdje"]) + Convert.ToDecimal(dicSettleInfo["ybfdje"]) + Convert.ToDecimal(dicSettleInfo["yljmje"]);       //本次医疗费用
            inSettleMain.GetAmount = Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]);    //本次现金支出
            inSettleMain.MedAmountZhzf = Convert.ToDecimal(dicSettleInfo["grzhzf"]);//本次帐户支出
            inSettleMain.MedAmountTc = Convert.ToDecimal(dicSettleInfo["ybfdje"]);  //本次统筹支出
            inSettleMain.MedAmountDb = 0;  //本次大病支出
            inSettleMain.MedAmountBz = Convert.ToDecimal(dicSettleInfo["ylbzje"]);  //本次大病支出
            inSettleMain.MedAmountJm = Convert.ToDecimal(dicSettleInfo["yljmje"]);  //减免金额
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
            inSettleMain.OperatorId = PayAPIConfig.Operator.UserSysId;//operatorInfo.UserSysId;
            inSettleMain.SettleBackNo = "";
            inSettleMain.SettleType = "1";

            inReimPara.SettleInfo = inSettleMain;


            PayAPIInterface.Model.Comm.PayType payType;
            inReimPara.PayTypeList = new List<PayType>();
            payType = new PayAPIInterface.Model.Comm.PayType();
            payType.PayTypeId = 4;
            payType.PayTypeName = "医保";
            payType.PayAmount = Convert.ToDecimal(inSettleMain.MedAmountTotal);
            inReimPara.PayTypeList.Add(payType);

            payType = new PayAPIInterface.Model.Comm.PayType();
            payType.PayTypeId = 5;
            payType.PayTypeName = "医保卡";
            payType.PayAmount = Convert.ToDecimal(inSettleMain.MedAmountZhzf);
            inReimPara.PayTypeList.Add(payType);

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
        /// <summary>
        /// 门诊登记
        /// </summary>
        /// <param name="para"></param>
        public void OutNetworkRegister(OutPayParameter para)
        {

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

                //门诊初始化
                handelModel.InitMZ();

                handelModel.SaveOutItemsMZ(outReimPara.Details);

                //门诊结算
                dicSettleInfo = handelModel.SettleMZ(dicPatInfo["sbjbm"], dicPatInfo["ylzbh"]);


                //---------------------------------------------低保结算

                if (Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]) > 0) //如果自负金额大于0弹出是否低保结算提示
                {

                    if (outReimPara.RegInfo.Memo2 == "低保")
                    {
                        dicSettleInfoDibao.Clear();
                        JiNan.Dialog.DiBaoJS_Confirm diBaoJS = new JiNan.Dialog.DiBaoJS_Confirm(outReimPara, dicSettleInfo, dicSettleInfoDibao,hosId);
                        diBaoJS.ShowDialog();
                    }
                }
                //------------------------------------------------
                //保存门诊结算明细
                //SaveOutSettleMain("MZ");
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

            //撤销普通门诊
            while (true)
            {
                try
                {
                    dicPatInfo = handelModel.ReadCardMZ();
                    handelModel.InitMZ();
                    //门诊退费 
                    handelModel.CancelSettleMZ(settleNo, dicPatInfo["sbjbm"], dicPatInfo["ylzbh"]);

                    break;
                }
                catch (Exception ex)
                {
                    if (MessageBox.Show("撤销个人账户结算失败 错误提示" + ex.Message + "  是否重新撤销", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
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
            throw new Exception("该类别是普通门诊");

            //Dictionary<string, string> patInfo = new Dictionary<string, string>();
            ////判断有卡无卡
            //frmCARD frmCard = new frmCARD();
            //frmCard.ShowDialog();
            //if (frmCard.iscard == "1")
            //{
            //    CARD_Y_N = "1";
            //    patInfo = handelModel.ReadCardZY();
            //}
            //else if (frmCard.iscard == "0")
            //{
            //    CARD_Y_N = "0";
            //    patInfo = handelModel.QueryBasicInfo(frmCard.IDNo, "", "1", "");//*医疗统筹类别(1,住院，4 门规)
            //}
            //else
            //{
            //    throw new Exception("操作员取消本次操作");
            //}

            //dicPatInfo = patInfo;

            //if (dicPatInfo["zhzybz"] == "1")
            //{
            //    MessageBox.Show(dicPatInfo["zhzysm"]);
            //}

            //inReimPara.RegInfo.CardNo = patInfo["ylzbh"];                   //医疗卡号
            //inReimPara.RegInfo.NetRegSerial = patInfo["sbjglx"];              //卡序列号
            //inReimPara.RegInfo.CantonCode = patInfo["sbjbm"];              //行政区号
            //inReimPara.RegInfo.MemberNo = patInfo["ylzbh"];                //成员编码
            //inReimPara.RegInfo.CompanyName = patInfo["dwmc"];               //单位名称
            //inReimPara.RegInfo.PatAddress = patInfo["dwmc"];                   //住址
            //inReimPara.RegInfo.IdNo = patInfo["shbzhm"];                    //身份证号
            //inReimPara.RegInfo.NetPatType = patInfo["ylrylb"];             //人员类别 
            //inReimPara.RegInfo.Memo1 = patInfo["xb"] == "1" ? "男" : "女";                         //性别
            //inReimPara.RegInfo.NetPatName = patInfo["xm"]; //姓名
            //string rqlb = "";
            //if (dicPatInfo["sbjglx"] == "A")
            //{
            //    rqlb = "职工";
            //}
            //else if (dicPatInfo["sbjglx"] == "B")
            //{
            //    rqlb = "居民";
            //}
            //inReimPara.RegInfo.NetPatType = rqlb;                                              //人员
            //inReimPara.RegInfo.NetType = "1";
            ////当姓名不一致时提示
            //if (inReimPara.RegInfo.NetPatName != patInfo["xm"])
            //{
            //    if (MessageBox.Show(" 医保登记姓名为：【" + patInfo["xm"].ToString() + "】     患者姓名为：【" + inReimPara.RegInfo.NetPatName + "】 是否继续 ", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
            //    {
            //        throw new Exception("姓名不一致，操作员取消操作！");
            //    }
            //}

            ////inReimPara.RegInfo.NetPatName = patInfo["xm"];                  //姓名

            //if (patInfo["ylrylb"].Contains("居民"))
            //    MessageBox.Show("提示：该患者人员类别属于居民，但住院信息中费别为职工，请取消操作并调整患者费别！");

            ////显示个人信息
            //PersonInfoDialog perDialog = new PersonInfoDialog(patInfo);
            //perDialog.ShowDialog();
        }

        /// <summary>
        /// 住院登记
        /// </summary>
        /// <param name="inPara">住院接口入参</param>
        /// <returns></returns>
        public void InNetworkRegister(InPayParameter para)
        {
            throw new Exception("该类别是普通门诊");

            //InterfaceInit();
            //inReimPara = para;
            //InNetWorkReadCard(inReimPara);
            //try
            //{
            //    handelModel.SaveZYDJ(inReimPara.PatInfo.PatInHosCode,
            //                        dicPatInfo["shbzhm"],
            //                        dicPatInfo["ylzbh"],
            //                        dicPatInfo["xm"],
            //                        dicPatInfo["xb"],
            //                        "1",//   string            *住院类别 1:住院 2:家床
            //                        dicPatInfo["sbjbm"],
            //                        "3",//(CARD_Y_N == "0" ? "0" : "3"),     //  *使用医保卡类型（0:不使用医保卡 ,1银行卡,2 cpu 卡，3，济南医保卡） string
            //                        inReimPara.PatInfo.InDeptCode.ToString(),               //需修改//varchar2(20)       *科室编码
            //                        inReimPara.PatInfo.InDateTime.ToString("yyyy-MM-dd"),//datetime           *住院日期
            //                        "",//string              确诊医师
            //                        "",//  varchar2(20)        门诊科室
            //                        "1",
            //                        "C",
            //                        "");

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    MessageBox.Show(ex.InnerException.Message);
            //    throw ex;
            //}
            //IsInReadCard = false;
        }

        /// <summary>
        /// 取消住院登记
        /// </summary>
        /// <param name="inPara">住院接口入参</param>
        /// <returns></returns>
        public void CancelInNetworkRegister(InPayParameter para)
        {
            throw new Exception("该类别是普通门诊");

            //InterfaceInit();
            //inReimPara = para;
            //handelModel.DelAllInItems(inReimPara.PatInfo.PatInHosCode);
            //handelModel.CancelZY(inReimPara.PatInfo.PatInHosCode);
            //isInit = false;

        }

        /// <summary> 
        /// 住院预结算
        /// </summary>
        /// <param name="inPara">住院接口入参</param>
        /// <returns></returns>
        public void InNetworkPreSettle(InPayParameter para)
        {

            throw new Exception("该类别是普通门诊");

            //inReimPara = para;

            //if (inReimPara.SettleInfo != null && inReimPara.SettleInfo.MedAmountTotal != 0)
            //{
            //    PayAPIInterface.Model.Comm.PayType payType;
            //    inReimPara.PayTypeList = new List<PayType>();
            //    payType = new PayAPIInterface.Model.Comm.PayType();
            //    payType.PayTypeId = 4;
            //    payType.PayTypeName = "医保";
            //    payType.PayAmount = inReimPara.SettleInfo.MedAmountTotal;
            //    inReimPara.PayTypeList.Add(payType);

            //    payType = new PayAPIInterface.Model.Comm.PayType();
            //    payType.PayTypeId = 5;
            //    payType.PayTypeName = "医保卡";
            //    payType.PayAmount = inReimPara.SettleInfo.MedAmountZhzf;
            //    inReimPara.PayTypeList.Add(payType);

            //    return;
            //}

            //InterfaceInit();

            //string notMatchedCharge = "";
            //foreach (var item in inReimPara.Details)
            //{
            //    if (item.NetworkItemCode.ToString().Trim().Length == 0)
            //    {
            //        notMatchedCharge += "编码:" + item.ChargeCode + "," + "名称:" + item.ChargeName + "；";
            //    }
            //}
            //if (notMatchedCharge.Trim().Length > 0)
            //{
            //    if (MessageBox.Show("有以下项目未对应：\n" + notMatchedCharge + "\n是否继续？选择”是“将按自费项目进行收费报销。否则，取消本次收费报销！", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
            //    {
            //        throw new Exception("取消上传费用");
            //    }

            //    //MessageBox.Show("有以下项目未对应：\n" + notMatchedCharge + "\n不能收费报销！", "提示", MessageBoxButtons.OK);

            //    //throw new Exception("项目未对照");

            //}

            //handelModel.InitZY(inReimPara.PatInfo.PatInHosCode);

            //handelModel.DelAllInItems(inReimPara.PatInfo.PatInHosCode);
            //if (inReimPara.Details.Count > 0)
            //{
            //    handelModel.SaveInItems(inReimPara.Details,
            //        //"001", // 医师编码
            //                             inReimPara.PatInfo.DoctorCode,
            //                             inReimPara.PatInfo.OutDateTime.ToString("yyyy-MM-dd"),  //费用发生日期
            //                             inReimPara.PatInfo.PatInHosCode
            //                             );


            //    if (MessageBox.Show("是否有医保卡？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //    {
            //        dicSettleInfo = handelModel.SettleZY("");

            //        //SaveInSettleMain();
            //        //保存住院结算数据
            //        #region 保存中心返回值参数列表
            //        //保存中心返回值参数列表
            //        #region 保存SettleList值
            //        try
            //        {
            //            InNetworkSettleList inNetworkSettleList = new InNetworkSettleList();
            //            foreach (var item in dicSettleInfo)
            //            {
            //                inNetworkSettleList = new InNetworkSettleList();
            //                inNetworkSettleList.PatInHosId = inReimPara.PatInfo.PatInHosId;
            //                inNetworkSettleList.InNetworkSettleId = -1;
            //                inNetworkSettleList.ParaName = item.Key;
            //                inNetworkSettleList.ParaValue = item.Value.ToString();
            //                inNetworkSettleList.Memo = "";
            //                inReimPara.SettleParaList.Add(inNetworkSettleList);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            LogManager.Error("保存农合中心返回值参数列表 插入值 失败" + ex.Message, ex);
            //        }
            //        #endregion
            //        InNetworkSettleMain inSettleMain = new InNetworkSettleMain();
            //        inSettleMain.PatInHosId = inReimPara.PatInfo.PatInHosId;
            //        inSettleMain.SettleNo = dicSettleInfo["brjsh"] + "|" + DateTime.Now.ToString("yyyyMMddHHmmss");                              //医保中心交易流水号
            //        inSettleMain.Amount = Convert.ToDecimal(dicSettleInfo["brfdje"]) + Convert.ToDecimal(dicSettleInfo["ybfdje"]) + Convert.ToDecimal(dicSettleInfo["yljmje"]);       //本次医疗费用
            //        inSettleMain.GetAmount = Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]);    //本次现金支出
            //        inSettleMain.MedAmountZhzf = Convert.ToDecimal(dicSettleInfo["grzhzf"]);//本次帐户支出
            //        inSettleMain.MedAmountTc = Convert.ToDecimal(dicSettleInfo["ybfdje"]);  //本次统筹支出
            //        inSettleMain.MedAmountDb = 0;  //本次大病支出
            //        inSettleMain.MedAmountBz = Convert.ToDecimal(dicSettleInfo["ylbzje"]);  //本次大病支出
            //        inSettleMain.MedAmountJm = Convert.ToDecimal(dicSettleInfo["yljmje"]);  //减免金额
            //        inSettleMain.MedAmountTotal = Convert.ToDecimal(inSettleMain.Amount) - Convert.ToDecimal(inSettleMain.GetAmount);
            //        inSettleMain.NetworkingPatClassId = Convert.ToInt32(inReimPara.CommPara.NetworkPatClassId);
            //        inSettleMain.NetworkPatName = inReimPara.PatInfo.InPatName;
            //        inSettleMain.NetworkPatType = "0";
            //        inSettleMain.SettleBackNo = inReimPara.PatInfo.PatInHosCode;
            //        inSettleMain.SettleType = "1";
            //        inReimPara.SettleInfo = inSettleMain;

            //        //保存结算数据
            //        IInClientBLL inBLl = PayAPIClassLib.Factory.ClientBLLFactory.GetInClientBLLInstance();
            //        //获取联网结算ID 并重新组织数据
            //        inBLl.GetInSettleIdAndReorganizeData(inReimPara);
            //        inBLl.SaveInNetworkSettleMain(inSettleMain);
            //        inBLl.SaveInNetworkSettleList(inReimPara.SettleParaList);//保存settleLis结算数据
            //        //门诊付费方式 本接口 4 医保 6农合
            //        //PayAPIInterface.Model.Comm.PayType payType;
            //        //inReimPara.PayTypeList = new List<PayType>();
            //        //payType = new PayAPIInterface.Model.Comm.PayType();
            //        //payType.PayTypeId = 4;
            //        //payType.PayTypeName = "医保";
            //        //payType.PayAmount = inReimPara.SettleInfo.MedAmountTotal - inSettleMain.MedAmountDb;
            //        //inReimPara.PayTypeList.Add(payType);

            //        PayAPIInterface.Model.Comm.PayType payType;
            //        inReimPara.PayTypeList = new List<PayType>();
            //        payType = new PayAPIInterface.Model.Comm.PayType();
            //        payType.PayTypeId = 4;
            //        payType.PayTypeName = "医保";
            //        payType.PayAmount = Convert.ToDecimal(inSettleMain.MedAmountTotal);
            //        inReimPara.PayTypeList.Add(payType);

            //        payType = new PayAPIInterface.Model.Comm.PayType();
            //        payType.PayTypeId = 5;
            //        payType.PayTypeName = "医保卡";
            //        payType.PayAmount = Convert.ToDecimal(inSettleMain.MedAmountZhzf);
            //        inReimPara.PayTypeList.Add(payType);
            //        #endregion
            //    }
            //    else
            //    {
            //        dicSettleInfo = handelModel.SettleZY("wkzy");
            //        //SaveInSettleMain();
            //        //保存住院结算数据
            //        #region 保存中心返回值参数列表
            //        //保存中心返回值参数列表
            //        #region 保存SettleList值
            //        try
            //        {
            //            InNetworkSettleList inNetworkSettleList = new InNetworkSettleList();
            //            foreach (var item in dicSettleInfo)
            //            {
            //                inNetworkSettleList = new InNetworkSettleList();
            //                inNetworkSettleList.PatInHosId = inReimPara.PatInfo.PatInHosId;
            //                inNetworkSettleList.InNetworkSettleId = -1;
            //                inNetworkSettleList.ParaName = item.Key;
            //                inNetworkSettleList.ParaValue = item.Value.ToString();
            //                inNetworkSettleList.Memo = "";
            //                inReimPara.SettleParaList.Add(inNetworkSettleList);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            LogManager.Error("保存农合中心返回值参数列表 插入值 失败" + ex.Message, ex);
            //        }
            //        #endregion
            //        InNetworkSettleMain inSettleMain = new InNetworkSettleMain();
            //        inSettleMain.PatInHosId = inReimPara.PatInfo.PatInHosId;
            //        inSettleMain.SettleNo = dicSettleInfo["brjsh"] + "|" + DateTime.Now.ToString("yyyyMMddHHmmss");                              //医保中心交易流水号
            //        inSettleMain.Amount = Convert.ToDecimal(dicSettleInfo["brfdje"]) + Convert.ToDecimal(dicSettleInfo["ybfdje"]) + Convert.ToDecimal(dicSettleInfo["yljmje"]);       //本次医疗费用
            //        inSettleMain.GetAmount = Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]);    //本次现金支出
            //        inSettleMain.MedAmountZhzf = Convert.ToDecimal(dicSettleInfo["grzhzf"]);//本次帐户支出
            //        inSettleMain.MedAmountTc = Convert.ToDecimal(dicSettleInfo["ybfdje"]);  //本次统筹支出
            //        inSettleMain.MedAmountDb = 0;  //本次大病支出
            //        inSettleMain.MedAmountBz = Convert.ToDecimal(dicSettleInfo["ylbzje"]);  //本次大病支出
            //        inSettleMain.MedAmountJm = Convert.ToDecimal(dicSettleInfo["yljmje"]);  //减免金额
            //        inSettleMain.MedAmountTotal = Convert.ToDecimal(inSettleMain.Amount) - Convert.ToDecimal(inSettleMain.GetAmount);
            //        inSettleMain.NetworkingPatClassId = Convert.ToInt32(inReimPara.CommPara.NetworkPatClassId);
            //        inSettleMain.NetworkPatName = inReimPara.PatInfo.InPatName;
            //        inSettleMain.NetworkPatType = "0";
            //        inSettleMain.SettleBackNo = inReimPara.PatInfo.PatInHosCode;
            //        inSettleMain.SettleType = "1";
            //        inReimPara.SettleInfo = inSettleMain;

            //        //保存结算数据
            //        IInClientBLL inBLl = PayAPIClassLib.Factory.ClientBLLFactory.GetInClientBLLInstance();
            //        //获取联网结算ID 并重新组织数据
            //        inBLl.GetInSettleIdAndReorganizeData(inReimPara);
            //        inBLl.SaveInNetworkSettleMain(inSettleMain);
            //        inBLl.SaveInNetworkSettleList(inReimPara.SettleParaList);//保存settleLis结算数据


            //        PayAPIInterface.Model.Comm.PayType payType;
            //        inReimPara.PayTypeList = new List<PayType>();
            //        payType = new PayAPIInterface.Model.Comm.PayType();
            //        payType.PayTypeId = 4;
            //        payType.PayTypeName = "医保";
            //        payType.PayAmount = Convert.ToDecimal(inSettleMain.MedAmountTotal);
            //        inReimPara.PayTypeList.Add(payType);

            //        payType = new PayAPIInterface.Model.Comm.PayType();
            //        payType.PayTypeId = 5;
            //        payType.PayTypeName = "医保卡";
            //        payType.PayAmount = Convert.ToDecimal(inSettleMain.MedAmountZhzf);
            //        inReimPara.PayTypeList.Add(payType);
            //        #endregion
            //    }


            //}




        }

        /// <summary>
        /// 住院结算
        /// </summary>
        /// <param name="inPara">住院接口入参</param>
        /// <returns></returns>
        public void InNetworkSettle(InPayParameter para)
        {
            throw new Exception("该类别是普通门诊");

            //inReimPara = para;
            //try
            //{
            //    string sql = "select TOP 1 SETTLE_NO from ZY.[IN].IN_NETWORK_SETTLE_MAIN WHERE PAT_IN_HOS_ID=" + inReimPara.PatInfo.PatInHosId.ToString("0") + " AND SETTLE_NO!='' ORDER BY CREATE_TIME DESC";
            //    MSSQLHelper sqlhelper = new MSSQLHelper("Data Source=172.18.0.25;Initial Catalog=COMM;User ID=sa;Password=m@ssunsoft009");
            //    DataRowCollection drc = sqlhelper.ExecSqlReDs(sql).Tables[0].Rows;
            //    if (drc.Count > 0)
            //    {
            //        string SETTLE_NO = drc[0][0].ToString();
            //        // inReimPara.SettleInfo.MedAmountTotal = Convert.ToDecimal(Amount);
            //        inReimPara.SettleInfo.SettleNo = SETTLE_NO;
            //    }

            //    if (inReimPara.SettleInfo != null && inReimPara.SettleInfo.SettleNo != "")//inReimPara.SettleInfo.MedAmountTotal     inReimPara.SettleInfo.SettleNo
            //    {
            //        PayType payType = new PayType();
            //        payType.PayTypeId = 4;
            //        payType.PayTypeName = "医保";
            //        payType.PayAmount = inReimPara.SettleInfo.MedAmountTotal;
            //        inReimPara.PayTypeList = new List<PayType>();
            //        inReimPara.PayTypeList.Add(payType);
            //        return;
            //    }

            //    else
            //    {
            //        throw new Exception("结算失败：找不到该患者的联网结算记录");
            //    }
            //}
            //catch (Exception)
            //{

            //    throw;
            //}




        }

        /// <summary>
        /// 取消住院结算
        /// </summary>
        /// <param name="inPara">住院接口入参</param>
        /// <returns></returns>
        public void CancelInNetworkSettle(InPayParameter para)
        {
            //inReimPara = para;
            ////if (MessageBox.Show("请确保医保是否已经撤销，已经撤销则点【是】，未撤销则点【否】！", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
            ////{
            throw new Exception("该类别是普通门诊");
            ////}
            ////else
            ////{
            //#region 撤销结算时获取住院号
            //string sql = "SELECT SETTLE_BACK_NO  FROM  ZY.[IN].IN_NETWORK_SETTLE_MAIN WHERE SETTLE_BACK_NO!='' AND  SETTLE_NO='" + inReimPara.SettleInfo.SettleNo + "'";
            //MSSQLHelper sqlhelper = new MSSQLHelper("Data Source=172.18.0.25;Initial Catalog=COMM;User ID=sa;Password=m@ssunsoft009");
            //DataRowCollection drc = sqlhelper.ExecSqlReDs(sql).Tables[0].Rows;
            //if (drc.Count > 0)
            //{
            //    string SETTLE_BACK_NO = drc[0][0].ToString();
            //    // inReimPara.SettleInfo.MedAmountTotal = Convert.ToDecimal(Amount);
            //    inReimPara.SettleInfo.SettleBackNo = SETTLE_BACK_NO;
            //}
            //#endregion
            //string patInHosCode = inReimPara.SettleInfo.SettleBackNo;
            //string settleNo = inReimPara.SettleInfo.SettleNo;

            //InterfaceInit();

            //handelModel.CancleZYSettle(patInHosCode, settleNo);//, inReimPara.SettleInfo.InNetworkSettleId.ToString());

            ////}


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
