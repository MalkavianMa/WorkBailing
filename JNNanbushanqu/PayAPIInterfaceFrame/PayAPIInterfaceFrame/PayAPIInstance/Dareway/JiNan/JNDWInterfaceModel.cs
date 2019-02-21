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
    /// 职工门诊，职工和居民住院
    /// </summary>
    public class JNDWInterfaceModel : IPayCompanyInterface
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
        ///  *使用医保卡类型、（0:不使用医保卡 ,1银行卡,2 cpu 卡，3，济南医保卡，5，普通人员无卡住院登记。

        /// </summary>
        public string P_syzhlx = "3";

        public string p_yltclb = "1"; //住院类别 1:住院 2:家床

        public string CARD_Y_N = "";//有卡读取或者无卡读取，0，为无卡，1为有卡

        InNetworkSettleMain inSettleMain = new InNetworkSettleMain();
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
            outSettleMain.SettleNo = dicSettleInfo["mzzdlsh"];                    //医保中心交易流水号
            outSettleMain.Amount = Convert.ToDecimal(dicSettleInfo["zje"]);       //本次医疗费用
            outSettleMain.MedAmountZhzf = Convert.ToDecimal(dicSettleInfo["grzhzf"]);//本次帐户支出
            outSettleMain.MedAmountTc = 0;  //本次统筹支出
            outSettleMain.MedAmountDb = dicSettleInfoDibao.Count > 0 ? Convert.ToDecimal(dicSettleInfoDibao["AidPayment"]) + Convert.ToDecimal(dicSettleInfoDibao["AidCardPayment"]) : 0;  //低保支付
            outSettleMain.MedAmountBz = Convert.ToDecimal(dicSettleInfo["jmje"]);  //优抚对象减免金额
            outSettleMain.GetAmount =  Convert.ToDecimal(dicSettleInfo["xj"]) - outSettleMain.MedAmountDb;    //本次现金支出
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
 

            frmCARD frmCard = new frmCARD();
            frmCard.ShowDialog();
            if (frmCard.iscard == "1")
            {
                CARD_Y_N = "1";
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
            handelModel.InitMZ();

            handelModel.SaveOutItemsMZ(outReimPara.Details);

            //门诊结算
            dicSettleInfo = handelModel.SettleMZ(dicPatInfo["sbjbm"], dicPatInfo["ylzbh"]);
            //---------------------------------------------低保结算

            if (Convert.ToDecimal(dicSettleInfo["xj"])  > 0) //如果自负金额大于0弹出是否低保结算提示
            {

                if (outReimPara.RegInfo.Memo2 == "低保")
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
            inSettleMain.NetworkPatName = inReimPara.RegInfo.NetPatName;
            inSettleMain.NetworkPatType = inReimPara.RegInfo.NetPatType; // "0";
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

            //payType = new PayAPIInterface.Model.Comm.PayType();
            //payType.PayTypeId = 5;
            //payType.PayTypeName = "医保卡";
            //payType.PayAmount = Convert.ToDecimal(inSettleMain.MedAmountZhzf);
            //inReimPara.PayTypeList.Add(payType);

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
            //判断有卡无卡
            frmCARD frmCard = new frmCARD();
            frmCard.lblzylb.Visible = true;
            frmCard.cbType.Visible = true;
            frmCard.ShowDialog();
            if (frmCard.iscard == "1")
            {
                CARD_Y_N = "1";
                P_syzhlx = "3";
                patInfo = handelModel.ReadCardZY();
            }
            else if (frmCard.iscard == "0")
            {
                CARD_Y_N = "0";
                P_syzhlx = "5";
                patInfo = handelModel.QueryBasicInfo(frmCard.IDNo, "", "1", "");//*医疗统筹类别(1,住院，4 门规)
            }
            else
            {
                throw new Exception("操作员取消本次操作");
            }
            p_yltclb = frmCard.cbType.SelectedValue.ToString();
            dicPatInfo = patInfo;

            if (dicPatInfo["zhzybz"] == "1")
            {
                MessageBox.Show(dicPatInfo["zhzysm"]);
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
            inReimPara.RegInfo.Memo2 = CARD_Y_N; // 有卡，无卡
            inReimPara.RegInfo.NetPatName = patInfo["xm"]; //姓名
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
            inReimPara.RegInfo.PatInHosSerial = inReimPara.PatInfo.PatInHosCode;
            //当姓名不一致时提示
            if (inReimPara.PatInfo.InPatName != patInfo["xm"])
            {
                if (MessageBox.Show(" 医保卡姓名为：【" + patInfo["xm"].ToString() + "】     HIS患者姓名为：【" + inReimPara.PatInfo.InPatName + "】 是否继续 ", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                {
                    throw new Exception("姓名不一致，操作员取消操作！");
                }
            }

            //inReimPara.RegInfo.NetPatName = patInfo["xm"];                  //姓名

            //if (patInfo["ylrylb"].Contains("居民"))
            //    MessageBox.Show("提示：该患者人员类别属于居民，但住院信息中费别为职工，请取消操作并调整患者费别！");

            //显示个人信息
            PersonInfoDialog perDialog = new PersonInfoDialog(patInfo);
            perDialog.ShowDialog();
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
                                    p_yltclb,//   string            *住院类别 1:住院 2:家床
                                    dicPatInfo["sbjbm"],
                                    P_syzhlx,//(CARD_Y_N == "0" ? "0" : "3"),     //  *使用医保卡类型（0:不使用医保卡 ,1银行卡,2 cpu 卡，3，济南医保卡，5，普通人员无卡住院登记。） string
                                    handelModel.GetNetWorkDeptCode(inReimPara.PatInfo.InDeptCode.ToString()),               //需修改//varchar2(20)       *科室编码
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

            //-----------------------更新费别
            try
            {
                string Charge_class_id = "";
                if (inReimPara.RegInfo.NetPatType=="职工")
                {
                    Charge_class_id = "10002";
                }
                else
                {
                    Charge_class_id = "5";
                }
                change_chargeclass(Charge_class_id, inReimPara.PatInfo.PatInHosId.ToString());
            }
            catch (Exception ex)
            {
                PayAPIUtilities.Log.LogManager.Info("更新费别失败:PAT_IN_HOS_ID" + inReimPara.PatInfo.PatInHosId.ToString() + "\n\r" + ex.Message);
            }
            //--------------------------

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
        /// 住院费用上传
        /// </summary>
        public void InReimUpItems()
        {

            string notMatchedCharge = "";
            //InPayPara.Details = PayAPIUtilities.Tools.CommonTools.GetGroupList(InPayPara.Details); //传汇总后项目
            foreach (PayAPIInterface.Model.Comm.FeeDetail feeDetail in inReimPara.Details)
            {
                //feeDetail.NetworkItemCode = "963";
                if (feeDetail.NetworkItemCode.ToString().Trim().Length == 0)
                {
                    notMatchedCharge += "编码:" + feeDetail.ChargeCode + "," + "名称:" + feeDetail.ChargeName + "；";
                }

                feeDetail.Quantity = Convert.ToDecimal(Convert.ToDouble(feeDetail.Quantity));
                if (Convert.ToDecimal(feeDetail.Quantity) != 0)
                {
                    feeDetail.Price = Math.Round(Convert.ToDecimal(feeDetail.Amount) / Convert.ToDecimal(feeDetail.Quantity), 4);
                }
                else
                {
                    feeDetail.Price = Convert.ToDecimal(feeDetail.Amount);
                }
            }


            if (notMatchedCharge.Trim().Length > 0)
            {
                if (MessageBox.Show("有以下项目未对应：\n" + notMatchedCharge + "\n是否继续？选择”是“将继续进行报销。否则，取消本次报销！", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    throw new Exception("取消上传费用");
                }
            }

            for (int i = 0; i < inReimPara.Details.Count; i++)
            {
                if (inReimPara.Details[i].ChargeType < 100)
                {
                    inReimPara.Details[i].Memo4 = "药品";//药品
                }
                else
                {
                    inReimPara.Details[i].Memo4 = "医疗";//医疗
                }
            }

            //-------------------------------弹出费用审核界面-----------
            frm_FYSH frmFYSH = new frm_FYSH(this);
            frmFYSH.ShowDialog();

            if (frmFYSH.isOk == false)
            {
                throw new Exception("用户取消预结算");
            }
            //-----------------------------------------------------------

        }

        /// <summary>
        /// 删除住院费用
        /// </summary>
        public void DelAllInItems(string p_blh)
        {
            try
            {
                handelModel.InitZY(p_blh);
                handelModel.DelAllInItems(p_blh);
            }
            catch (Exception ex)
            {
                LogManager.Error("删除所有凭单失败！:" + ex.Message);
            }
            finally
            {

            }
            //-----------------------更新费用上传标志
            try
            {
                string strSql = "";
                strSql = "UPDATE ZY.[IN].IN_BILL_RECORD SET FLAG_NETWORK_UPLOAD='0' WHERE PAT_IN_HOS_ID='" + inReimPara.PatInfo.PatInHosId + "'";
                handelModel.sqlHelperHis.ExecSqlReInt(strSql);
            }
            catch (Exception ex)
            {
            }
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
                payType.PayAmount = inReimPara.SettleInfo.MedAmountTotal;
                inReimPara.PayTypeList.Add(payType);

                //payType = new PayAPIInterface.Model.Comm.PayType();
                //payType.PayTypeId = 5;
                //payType.PayTypeName = "医保卡";
                //payType.PayAmount = inReimPara.SettleInfo.MedAmountZhzf;
                //inReimPara.PayTypeList.Add(payType);

                return;
            }

            //StringBuilder str = new StringBuilder();
            //str.Append("SELECT MEMBER_NO FROM ZY.[IN].IN_NETWORK_REGISTERS WHERE PAT_IN_HOS_ID='"+para.PatInfo.PatInHosId.ToString("f0")+"'");
            //DataSet ds = JNDWInterfaceModel.handelModel.sqlHelperHis.ExecSqlReDs(str.ToString());
            //DataTable dt = ds.Tables[0];
            ////string MEMBERNO = dt.Rows[0]["MEMBER_NO"].ToString();
            ////if (inReimPara.RegInfo.MemberNo == null)
            //if (dt.Rows.Count>0)
            //{
            //    //throw new Exception("没有找到联网登记信息，请确认是否联网登记！");
            //}
            //else
            //{
            //    throw new Exception("没有找到联网登记信息，请确认是否联网登记！");
            //}

            if (inReimPara.RegInfo.MemberNo == null)
            {
                //throw new Exception("没有找到联网登记信息，请确认是否联网登记！");
            }

            InterfaceInit();
            handelModel.InitZY(inReimPara.RegInfo.PatInHosSerial);
            InReimUpItems();

            //----------------------------------------------------------------------
            #region
            //StringBuilder str = new StringBuilder();
            //str.Append("SELECT  ");
            ////str.Append("--TOP 1 ");
            //str.Append("IN_PAT_NAME AS 姓名,");
            //str.Append("SEX_NAME AS 性别,");
            //str.Append("d.ID_NO AS 身份证号,");
            //str.Append("d.PAT_IN_HOS_SERIAL AS 住院号,");
            //str.Append("PAT_AGAIN_IN_TIMES AS 住院次数, ");
            //str.Append("PAT_MAIN_DIAGNOSE_ID AS 诊断编码, ");
            //str.Append("MAIN_DIAG_NAME AS 诊断名称, ");
            //str.Append("b.DIAGNOSIS_CODE  as ICD疾病编码, ");
            //str.Append("PAT_IN_CHARGE_DOC_ID AS 经治医生编号, ");
            //str.Append("c.UESR_NAME AS 经治医生姓名, ");
            //str.Append("PAT_IN_TIME AS 入院时间, ");
            //str.Append("PAT_LEAVE_ORDER_LEAVE_TIME AS 出院时间, ");
            //str.Append("DATEDIFF(DAY, PAT_IN_TIME, PAT_LEAVE_ORDER_LEAVE_TIME) AS 天数, ");
            //str.Append("PAT_IN_TIME AS 确诊日期, ");
            //str.Append("DATEDIFF(DAY, PAT_IN_TIME, PAT_IN_TIME + 1) AS 确诊天数 ");
            //str.Append("FROM  ZY.[IN].PAT_ALL_INFO_VIEW a ");
            //str.Append("LEFT JOIN COMM.DICT.DIAGNOSIS b ON a.PAT_MAIN_DIAGNOSE_ID = b.DIAGNOSIS_ID ");
            //str.Append("LEFT JOIN COMM.COMM.USERINFO_VIEW c ON a.PAT_IN_CHARGE_DOC_ID = c.USER_SYS_ID ");
            //str.Append("LEFT JOIN ZY.[IN].IN_NETWORK_REGISTERS d ON a.PAT_IN_HOS_ID = d.PAT_IN_HOS_ID ");
            //str.Append("WHERE  a.PAT_IN_HOS_ID = '" + para.PatInfo.PatInHosId.ToString("f0") + "' ");

            //DataSet ds = JNDWInterfaceModel.handelModel.sqlHelperHis.ExecSqlReDs(str.ToString());
            //DataTable dt = ds.Tables[0];
            ////病历首页初始化
            //handelModel.BlsyCsh(para.PatInfo.PatInHosCode);
            ////比例首页上传参数保存
            //handelModel.BlsySc(dt);

            #endregion
            //----------------------------------------------------------------------------------------------------
            //if (MessageBox.Show("是否有医保卡？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            if (inReimPara.RegInfo.Memo2=="1")
            {
                dicSettleInfo = handelModel.SettleZY("");
            }
            else
            {
                dicSettleInfo = handelModel.SettleZY("wkzy");
            }

            SaveInSettleMain();
          

        }

        /// <summary>
        /// 住院结算
        /// </summary>
        /// <param name="inPara">住院接口入参</param>
        /// <returns></returns>
        public void InNetworkSettle(InPayParameter para)
        {

            inReimPara = para;
            double ybJE=0;//医保金额

            if (inReimPara.SettleInfo != null && inReimPara.SettleInfo.SettleNo != "")//inReimPara.SettleInfo.MedAmountTotal     inReimPara.SettleInfo.SettleNo
            {
                PayType payType = new PayType();
                payType.PayTypeId = 4;
                payType.PayTypeName = "医保";
                payType.PayAmount = inReimPara.SettleInfo.MedAmountTotal;
                inReimPara.PayTypeList = new List<PayType>();
                inReimPara.PayTypeList.Add(payType);

                ybJE = Math.Round(Convert.ToDouble(inReimPara.SettleInfo.Amount.ToString()), 2); 
            }

            else
            {
                SaveInSettleMain();
                ybJE = Math.Round(Convert.ToDouble(inSettleMain.Amount.ToString()), 2); 
            }

            //---------------------------------------------判断金额是否相等
            DataSet dst = new DataSet();
            string strS = "SELECT TOTAL_COSTS FROM ZY.[IN].PAT_IN_HOS_EXTENDED WHERE PAT_IN_HOS_ID='" + inReimPara.PatInfo.PatInHosId + "'";
            dst = JNDWInterfaceModel.handelModel.sqlHelperHis.ExecSqlReDs(strS);
            if (dst.Tables[0].Rows.Count > 0)
            {
                if (Math.Round(ybJE, 2) != Math.Round(Convert.ToDouble(dst.Tables[0].Rows[0]["TOTAL_COSTS"].ToString()), 2))
                {
                    throw new Exception("医保结算费用[" + ybJE.ToString() + "]与HIS发生的费用总额[" + Convert.ToDouble(dst.Tables[0].Rows[0]["TOTAL_COSTS"]).ToString() + "]不符！");
                }
            }
            //-------------------------------------------
        }





        /// <summary>
        /// 取消住院结算
        /// </summary>
        /// <param name="inPara">住院接口入参</param>
        /// <returns></returns>
        public void CancelInNetworkSettle(InPayParameter para)
        {
            inReimPara = para;

            string patInHosCode = inReimPara.RegInfo.PatInHosSerial;
            string settleNo = inReimPara.SettleInfo.SettleNo;

            InterfaceInit();

            handelModel.CancleZYSettle(patInHosCode, settleNo);



        }
        #endregion


        //----住院修改费别
        public void change_chargeclass(string Charge_class, string patInHosId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE COMM.DICT.IN_PATS SET CHARGE_CLASS_ID=" + Charge_class + ",PAT_IDCARD='" + netPatInfo.IDNo + "' WHERE IN_PAT_ID=(SELECT TOP 1 IN_PAT_ID FROM ZY.[IN].PAT_ALL_INFO_VIEW WHERE PAT_IN_HOS_ID=" + patInHosId + ")");
            strSql.Append("   UPDATE ZY.[IN].PAT_OUT_HOSPITAL SET CHARGE_CLASS_ID=" + Charge_class + "  WHERE PAT_IN_HOS_ID=" + patInHosId);
            strSql.Append("   UPDATE ZY.[IN].PAT_IN_HOSPITAL SET CHARGE_CLASS_ID=" + Charge_class + " WHERE PAT_IN_HOS_ID=" + patInHosId);
            JNDWInterfaceModel.handelModel.sqlHelperHis.ExecSqlReInt(strSql.ToString());
        }


    }
}
