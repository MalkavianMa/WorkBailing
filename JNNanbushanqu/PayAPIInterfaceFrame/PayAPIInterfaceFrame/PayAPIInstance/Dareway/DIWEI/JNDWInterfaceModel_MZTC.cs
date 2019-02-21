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

namespace PayAPIInstance.Dareway.DIWEI
{
    /// <summary>
    /// 济南门诊大病类
    /// </summary>
    public class JNDWInterfaceModel_MZTC : IPayCompanyInterface
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
        /// 是否已经读取卡信息
        /// </summary>
        public bool IsOutReadCard = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public JNDWInterfaceModel_MZTC()
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
            outSettleMain.OperatorId = operatorInfo.UserSysId;
            outSettleMain.OutNetworkSettleId = Convert.ToDecimal(outReimPara.SettleInfo.OutNetworkSettleId);
            outSettleMain.SettleBackNo = "";
            outSettleMain.SettleType = "1";

            PayAPIInterface.Model.Comm.PayType payType;
            outReimPara.PayTypeList = new List<PayType>();
            payType = new PayAPIInterface.Model.Comm.PayType();
            payType.PayTypeId = 4;
            payType.PayTypeName = "医保";
            payType.PayAmount = Convert.ToDecimal(outSettleMain.MedAmountTotal);
            outReimPara.PayTypeList.Add(payType);

            payType.PayTypeId = 5;
            payType.PayTypeName = "医保卡";
            payType.PayAmount = Convert.ToDecimal(outReimPara.SettleInfo.MedAmountZhzf);
            outReimPara.PayTypeList.Add(payType);
        }
        #endregion

        #region 保存门诊结算数据 账户支付
        /// <summary>
        /// 保存门诊结算数据
        /// </summary>
        public void SaveOutSettleMain_ZHZF()
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
            outSettleMain.SettleNo = dicSettleInfo["mzzdlsh"];                           //医保中心交易流水号
            outSettleMain.Amount = Convert.ToDecimal(dicSettleInfo["zje"]);       //本次医疗费用
            outSettleMain.GetAmount = Convert.ToDecimal(dicSettleInfo["zje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]);    //本次现金支出
            outSettleMain.MedAmountZhzf = Convert.ToDecimal(dicSettleInfo["grzhzf"]);//本次帐户支出
            outSettleMain.MedAmountTc = 0;  //本次统筹支出
            outSettleMain.MedAmountDb = 0;  //本次大病支出
            outSettleMain.MedAmountBz = 0;  //本次大病支出
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
            outSettleMain.OperatorId = operatorInfo.UserSysId;
            outSettleMain.OutNetworkSettleId = Convert.ToDecimal(outReimPara.SettleInfo.OutNetworkSettleId);
            outSettleMain.SettleBackNo = "";
            outSettleMain.SettleType = "2";

            PayAPIInterface.Model.Comm.PayType payType;
            outReimPara.PayTypeList = new List<PayType>();
            payType = new PayAPIInterface.Model.Comm.PayType();
            payType.PayTypeId = 4;
            payType.PayTypeName = "医保";
            payType.PayAmount = Convert.ToDecimal(outSettleMain.MedAmountTotal);
            outReimPara.PayTypeList.Add(payType);

            payType.PayTypeId = 5;
            payType.PayTypeName = "医保卡";
            payType.PayAmount = Convert.ToDecimal(outReimPara.SettleInfo.MedAmountZhzf);
            outReimPara.PayTypeList.Add(payType); ;
        }
        #endregion

        #region 保存住院结算数据
        /// <summary>
        /// 保存住院结算数据
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
            payType.PayAmount = Convert.ToDecimal(inSettleMain.MedAmountTotal);
            inReimPara.PayTypeList.Add(payType);

            payType.PayTypeId = 5;
            payType.PayTypeName = "医保卡";
            payType.PayAmount = Convert.ToDecimal(inSettleMain.MedAmountZhzf);
            inReimPara.PayTypeList.Add(payType);
        }
        #endregion

        #region 门诊业务

        /// <summary>
        /// 读卡
        /// </summary>
        /// <returns></returns>
        public NetworkPatInfo NetworkReadCard()
        {
            InterfaceInit();
            NetworkPatInfo networkPatInfo = new NetworkPatInfo();
            IsOutReadCard = false;
            frmCARD frmCard = new frmCARD();
            frmCard.ShowDialog();
            if (frmCard.iscard == "1")
            {
                CARD_Y_N = "1";
                P_syzhlx = "3";
                patInfo = handelModel.ReadCardMZ();
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
            IsOutReadCard = true;
            dicPatInfo = patInfo;

            networkPatInfo.MedicalNo = patInfo["ylzbh"];                   //医疗卡号
            networkPatInfo.PatName = patInfo["xm"];                        //姓名
            networkPatInfo.Sex = patInfo["xb"] == "1" ? "男" : "女";       //性别
            networkPatInfo.IDNo = patInfo["sfzhm"];                        //身份证号码
            networkPatInfo.MedicalTypeName = patInfo["ylrylb"];
            networkPatInfo.MedicalType = patInfo["ylrylb"];                //医疗人员类别
            networkPatInfo.ICAmount = Convert.ToDecimal(patInfo["ye"]);  //账户余额
            networkPatInfo.ICNo = patInfo["kh"];                           //社会保障卡卡号
            networkPatInfo.CompanyNo = patInfo["sbjgbh"];                    //单位编号
            networkPatInfo.CompanyName = patInfo["dwmc"];                  //单位名称
            networkPatInfo.Birthday = Convert.ToDateTime(patInfo["csrq"].Substring(0, 4) + "-" + patInfo["csrq"].Substring(4, 2) + "-" + patInfo["csrq"].Substring(6, 2));                    //出生日期
            networkPatInfo.MedicalType = patInfo["ylrylb"];
            return networkPatInfo;
        }

        /// <summary>
        /// 门诊登记
        /// </summary>
        /// <returns></returns>
        public void OutNetworkRegister(OutPayParameter para)
        {
            InterfaceInit();
            NetworkReadCard();
            //显示个人信息
            PersonInfoDialog perDialog = new PersonInfoDialog(patInfo);
            perDialog.ShowDialog();
            strDiagnosCode = perDialog.strDiagnosCode;
            if (perDialog.isCancel)
            {
                throw new Exception("取消操作");
            }
            outReimPara.RegInfo.CardNo = patInfo["ylzbh"];                   //医疗卡号
            outReimPara.RegInfo.NetPatType = patInfo["sbjglx"];              //卡序列号
            outReimPara.RegInfo.CantonCode = patInfo["sbjbm"];               //行政区号
            outReimPara.RegInfo.MemberNo = patInfo["shbzhm"];                //成员编码
            outReimPara.RegInfo.CompanyName = patInfo["dwmc"];               //单位名称
            outReimPara.RegInfo.PatAddress = patInfo["dwmc"];                //住址
            outReimPara.RegInfo.IdNo = patInfo["shbzhm"];                    //身份证号
            outReimPara.RegInfo.Balance = Convert.ToDecimal(patInfo["ye"]);  //账户余额
            outReimPara.RegInfo.NetPatName = patInfo["xm"];                  //姓名
            outReimPara.RegInfo.NetDiagnosCode = patInfo["mzdbjbs"];         //性别
            string rqlb = "";
            if (patInfo["sbjglx"] == "A")
            {
                rqlb = "职工";
            }
            else if (patInfo["sbjglx"] == "B")
            {
                rqlb = "居民";
            }

            outReimPara.RegInfo.NetPatType = rqlb;
            outReimPara.RegInfo.NetType = "6";
        }

        /// <summary>
        /// 门诊联网预结算
        /// </summary>
        /// <param name="inPara">门诊接口入参</param>
        /// <returns></returns>
        public void OutNetworkPreSettle(OutPayParameter outReimPara)
        {
            //InterfaceInit();
            //handelModel.InitJMMZ(outReimPara.RegInfo.CantonCode, "6", outReimPara.RegInfo.MemberNo, outReimPara.PatInfo.PatName,
            //                    "1", outReimPara.SettleInfo.OutNetworkSettleId.ToString(), DateTime.Now.ToString("yyyy-MM-dd"),//"001", 
            //                    operatorInfo.UserSysId, strDiagnosCode, P_syzhlx, outReimPara.RegInfo.CardNo, "C", "");
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
            if (outReimPara.RegInfo.NetPatName != dicPatInfo["xm"])
            {
                if (MessageBox.Show(" 医保登记姓名为：【" + dicPatInfo["xm"].ToString() + "】     患者姓名为：【" + outReimPara.RegInfo.NetPatName + "】 是否继续 ", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    throw new Exception("姓名不一致，操作员取消操作！");
                }
            }
            //门诊初始化
            //handelModel.InitJMMZ(outReimPara.RegInfo.CantonCode, "6", outReimPara.RegInfo.MemberNo, outReimPara.PatInfo.PatName,
            //                    dicPatInfo["xb"], outReimPara.SettleInfo.OutNetworkSettleId.ToString(), DateTime.Now.ToString("yyyy-MM-dd"),//"001", 
            //                    operatorInfo.UserSysId, strDiagnosCode, P_syzhlx, outReimPara.RegInfo.CardNo, "C", "");

            handelModel.InitJMMZ(outReimPara.RegInfo.CantonCode, "6", outReimPara.RegInfo.MemberNo, outReimPara.PatInfo.PatName,
                              dicPatInfo["xb"], outReimPara.CommPara.OutNetworkSettleId.ToString(), DateTime.Now.ToString("yyyy-MM-dd"),//"001", 
                              PayAPIConfig.Operator.UserSysId, strDiagnosCode, P_syzhlx, outReimPara.RegInfo.CardNo);


            handelModel.SaveOutItems(outReimPara);
            //门诊结算
            dicSettleInfo = handelModel.SettleMG(dicPatInfo["sbjglx"]);
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
            //撤销门诊大病结算
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
