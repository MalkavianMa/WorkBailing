using PayAPIInstance.Dareway.JNLX.Dialog;
using PayAPIInterface;
using PayAPIInterface.Model.Comm;
using PayAPIInterface.Model.Out;
using PayAPIInterface.ParaModel;
using PayAPIUtilities.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace PayAPIInstance.Dareway.JNLX
{


    /// <summary>
    /// 济南历下社区地维接口源码
    /// </summary>
    public class JNLXInterfaceModel : IPayCompanyInterface
    {

        /// <summary>
        /// 医疗类别
        /// </summary>
        public string YllbStr = "";

        /// <summary>
        /// 险种标识
        /// </summary>
        public string xzbzStr = "";

        /// <summary>
        /// 免费用药诊断
        /// </summary>
        public string mfyyStr = "";
        /// <summary>
        /// 门诊慢病疾病编码
        /// </summary>
        public string MZMB_JBBM = "";

        /// <summary>
        /// 结算医疗统筹类别
        /// </summary>
        public string YltclbStr = "";
        /// <summary>
        /// 门诊入参
        /// </summary>
        public OutPayParameter outReimPara;

        /// <summary>
        /// 医保个人信息
        /// </summary> 
        public NetworkPatInfo netPatInfo = new NetworkPatInfo();

        /// <summary>
        /// 疾病编码
        /// </summary>
        public string strDiagnosCode = "";

        /// <summary>
        /// 结算信息
        /// </summary>
        public Dictionary<string, string> dicSettleInfo = new Dictionary<string, string>();

        /// <summary>
        /// 病人信息
        /// </summary>
        Dictionary<string, string> patInfo = new Dictionary<string, string>();
        /// <summary>
        /// 是否初始化
        /// </summary>
        public static bool isInit = false;

        /// <summary>
        /// 固定支付方式
        /// </summary>
        public static int payTypeId = 4;

        /// <summary>
        /// 业务处理
        /// </summary>
        private static PayAPIResolver.Dareway.JNLX.JNLXDarewayInterfaceResolver handelModel;

        /// <summary>
        /// 患者信息
        /// </summary> 
        public Dictionary<string, string> dicPatInfo = new Dictionary<string, string>();


        /// <summary>
        /// 医疗统筹类别（读卡）
        /// </summary>
        public static string strYlCtlb = "";
        /// <summary>
        /// 初始化
        /// </summary>
        public static void InterfaceInit()
        {
            if (!isInit)
            {
                //InterfaceCommInfoBll.InitCommInfo(PayAPIConfig.InstitutionDict[1].Memo);
                //readCardHandle = InterfaceCommInfoBll.GetReadCardInfoHandle();
                handelModel = new PayAPIResolver.Dareway.JNLX.JNLXDarewayInterfaceResolver();

                isInit = true;
            }
        }

        public NetworkPatInfo NetworkReadCard()
        {
            return ReadCardRePatInfo();
        }





        #region 读卡返回病号基本信息
        /// <summary>
        /// 读卡返回病号基本信息
        /// </summary>
        public static NetworkPatInfo ReadCardRePatInfo()
        {
            ////12108009618110811585101
            //ZBDWInterfaceModel n = new ZBDWInterfaceModel();
            //n.CancelOutReimSettle("12108009618110811585101");

            NetworkPatInfo networkPatInfo = new NetworkPatInfo();
            InterfaceInit();
            Dialog.frmSelectPerson_ZY frm = new Dialog.frmSelectPerson_ZY();
            frm.ShowDialog();
            strYlCtlb = frm.StrYltclb;
            if (frm.isHaveCard == 1)
            {

                Dictionary<string, string> patInfo = ReadCardInfo("", "", "C", "0");
                networkPatInfo.MedicalNo = patInfo["grbh"];                    //医保个人编号
                networkPatInfo.ICNo = patInfo["kh"];                           //卡号 阳煤地区该字段为空
                networkPatInfo.PatName = patInfo["xm"];                        //姓名
                networkPatInfo.Sex = patInfo["xb"] == "1" ? "男" : "女";       //性别
                networkPatInfo.IDNo = patInfo["sfzhm"];                        //身份证号码
                networkPatInfo.MedicalType = patInfo["ylrylb"];                //医疗人员类别
                networkPatInfo.ICAmount = Convert.ToDecimal(patInfo["zhye"]);  //账户余额
                networkPatInfo.CompanyName = patInfo["dwmc"];                  //单位名称
                networkPatInfo.CompanyName = patInfo["dwmc"];                    //单位编号
                return networkPatInfo;
            }
            if (frm.isHaveCard == 2)
            {

                Dictionary<string, string> patInfo = QueryAllCountry(frm.IDNO.Trim(), strYlCtlb, "C", "", "", "");
                networkPatInfo.MedicalNo = patInfo["grbh"];                    //医保个人编号
                networkPatInfo.ICNo = patInfo["kh"];                           //卡号 阳煤地区该字段为空
                networkPatInfo.PatName = patInfo["xm"];                        //姓名
                networkPatInfo.Sex = patInfo["xb"] == "1" ? "男" : "女";       //性别
                networkPatInfo.IDNo = patInfo["sfzhm"];                        //身份证号码
                networkPatInfo.MedicalType = patInfo["ylrylb"];                //医疗人员类别
                networkPatInfo.ICAmount = Convert.ToDecimal(patInfo["zhye"]);  //账户余额
                networkPatInfo.CompanyName = patInfo["dwmc"];                  //单位名称
                networkPatInfo.CompanyName = patInfo["dwmc"];                    //单位编号
                return networkPatInfo;

            }
            //  if (frm.isHaveCard==0)
            else
            {
                Dictionary<string, string> patInfo = QueryPersonInfo(frm.IDNO.Trim(), strYlCtlb, "C", "", "", "");
                handelModel.SBJGBH = patInfo["sbjgbh"];
                networkPatInfo.MedicalNo = patInfo["grbh"];                    //医保个人编号
                networkPatInfo.ICNo = "";// patInfo["kh"];                           //社会保障卡卡号
                networkPatInfo.PatName = patInfo["xm"];                        //姓名
                networkPatInfo.Sex = patInfo["xb"] == "1" ? "男" : "女";       //性别
                networkPatInfo.IDNo = patInfo["sfzhm"];                        //身份证号码
                networkPatInfo.MedicalType = patInfo["ylrylb"];                //医疗人员类别
                //networkPatInfo.ICAmount = Convert.ToDecimal(patInfo["zhye"]);  //账户余额
                networkPatInfo.CompanyName = patInfo["dwmc"];                  //单位名称
                networkPatInfo.CompanyName = patInfo["dwmc"];                    //单位编号
                networkPatInfo.Birthday = Convert.ToDateTime((patInfo["csrq"].ToString() == "" ? "" : patInfo["csrq"].ToString().Substring(0, 4) + "-" + patInfo["csrq"].ToString().Substring(4, 2) + "-" + patInfo["csrq"].ToString().Substring(6, 2)).ToString()); //出生日期
                return networkPatInfo;
            }
        }

        private static Dictionary<string, string> QueryAllCountry(string p1, string strYlCtlb, string p2, string p3, string p4, string p5)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            handelModel.InitHandle();
            handelModel.SBJGBH = "000000";

            //由地纬DLL控制读卡器时 ，全国异地读卡交易读卡获取人员基本信息。此时入参无参
            //业务处理
            handelModel.Handle("read_card_qgyd");

            result = handelModel.GetResultDict();
            return result;
            // throw new NotImplementedException();
        }


        /// <summary>
        /// 3.2.1获取人员基本信息（无卡）
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="strYlCtlb"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        /// <param name="p5"></param>
        /// <returns></returns>
        private static Dictionary<string, string> QueryPersonInfo(string idNumber, string strYlCtlb, string p3, string p4, string p5, string p_xzbz = "C")
        {

            Dictionary<string, string> result = new Dictionary<string, string>();
            handelModel.InitHandle();
            handelModel.SBJGBH = "000000";
            handelModel.AddInPara("p_grbh", idNumber);  ////*个人编号	社会保障号码或者身份证号  
            handelModel.AddInPara("p_xzbz", p_xzbz);////*险种标志	具体值调用数据字典接口获取，代码编号：XZBZ
            handelModel.AddInPara("p_xm", p_xzbz);////姓名	该姓名必须和医保数据库中一致
            handelModel.AddInPara("p_yltclb", strYlCtlb);////医疗统筹类别	0为仅获取人员基本信息，1为住院，4为门诊大病(特病)，6为普通门诊，不传时，默认值为0,其他具体值调用数据字典接口获取，代码编号：YLTCLB

            //业务处理
            handelModel.Handle("query_basic_info");

            result = handelModel.GetResultDict();
            return result;
            /// throw new NotImplementedException();
        }

        /// <summary>
        /// 3.2.2读卡获取人员基本信息
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p_yltclb"></param>
        /// <param name="p3"></param>
        /// <param name="ptmzMark"></param>
        /// <returns></returns>
        private static Dictionary<string, string> ReadCardInfo(string p1, string p_yltclb, string p3 = "C", string ptmzMark = "0")
        {
            //throw new NotImplementedException();

            Dictionary<string, string> result = new Dictionary<string, string>();
            handelModel.InitHandle();
            handelModel.SBJGBH = "000000";
            handelModel.AddInPara("p_kh", "");      //*若由地纬DLL控制读卡器，p_kh可不传；若由his控制读卡器，p_kh为必传。
            handelModel.AddInPara("p_xzbz", p3);      //*险种标志 具体值调用数据字典接口获取，代码编号：XZBZ
            handelModel.AddInPara("p_ewm", "");  //二维码	电子社保卡二维码号，传二维码号时p_kh传’’
            handelModel.AddInPara("p_yltclb", p_yltclb);  //医疗统筹类别 	0为仅获取人员基本信息，1为住院，4为门诊大病(特病)，6为普通门诊，不传时，默认值为0，其他具体值调用数据字典接口获取，代码编号：YLTCLB
            handelModel.AddInPara("p_kl", "");//口令由医保管理时，需传入口令；否则不传。（阳煤地区口令必传，如果口令没有修改过，则不会校验）
            handelModel.AddInPara("p_ptmzskbz", ptmzMark);//  普通门诊刷卡标志	纯个账消费传1，其他不传或传0            
            handelModel.AddInPara("p_shbzhm", "");////社会保障号码	由his控制读卡器时，跨省异地读卡必传
            handelModel.AddInPara("p_xm", "");////姓名	由his控制读卡器时，跨省异地读卡必传
            handelModel.AddInPara("p_sbm", "");////卡识别码	由his控制读卡器时，跨省异地读卡必传
            handelModel.AddInPara("p_gfbb", "");////卡规范版本	由his控制读卡器时，跨省异地读卡必传



            //业务处理
            handelModel.Handle("read_card");

            result = handelModel.GetResultDict();
            return result;
        }



        #endregion

        #region 门诊业务


        /// <summary>
        /// 门诊登记第一调用
        /// </summary>
        /// <param name="para"></param>
        public void OutNetworkRegister(OutPayParameter para)
        {
            outReimPara = para;

            OutNeworkReadCard();



        }

        /// <summary>
        /// 由门诊登记调用
        /// </summary>
        public void OutNeworkReadCard()
        {
            InterfaceInit();

            string quicklysfID = outReimPara.PatInfo.IDNo;
            frmSelectPerson_ZY frm = new frmSelectPerson_ZY(quicklysfID);
            frm.ShowDialog();
            strYlCtlb = frm.StrYltclb;
            if (frm.isHaveCard == 1)
            {

                Dictionary<string, string> patInfo = ReadCardInfo("", "", "C");
                dicPatInfo = patInfo;
                outReimPara.RegInfo.CardNo = patInfo["kh"];     // 卡号	阳煤地区该字段为空                   //医疗卡号
                //outReimPara.RegInfo.Memo1 //贫困人口标志	1：贫困人口，0：非贫困人口  + "|" +济南专用。’1’为优抚对象+ "|" +救助人员类别	具体值调用数据字典接口获取，代码编号：JZRYLB;                       
                outReimPara.RegInfo.Memo1 = patInfo["pkrkbz"] + "|" + patInfo["yfdxbz"] + "|" + patInfo["jzrylb"];                                //获取持卡人所在的社保机构编号
                outReimPara.RegInfo.CantonCode = patInfo["sbjgbh"];   //社保机构编号              //行政区号
                outReimPara.RegInfo.MemberNo = patInfo["grbh"];          //个人编号           //成员编码
                outReimPara.RegInfo.CompanyName = patInfo["rqlb"];  //  人群类别	A：职工，B：居民，其他具体值调用数据字典接口获取，代码编号：RQLB
                outReimPara.RegInfo.PatAddress = patInfo["dwmc"];                      //住址
                outReimPara.RegInfo.IdNo = patInfo["sfzhm"];                        //身份证号
                outReimPara.RegInfo.NetPatType = patInfo["ylrylb"];                //人员类别
                outReimPara.RegInfo.Balance = Convert.ToDecimal(patInfo["zhye"]); //账户余额 
                outReimPara.RegInfo.NetPatName = patInfo["xm"];                     //姓名  
                outReimPara.RegInfo.Memo2 = patInfo["ydbz"];//异地标志
                //outReimPara.RegInfo.NetType = "";

                if (!outReimPara.PatInfo.PatName.Equals(patInfo["xm"]))
                {
                    throw new Exception("his患者姓名" + outReimPara.PatInfo.PatName + "与医保登记姓名不一致，医保姓名：" + patInfo["xm"]);
                }

                //显示个人信息          
                Dialog.PersonInfoDialog perDialog = new Dialog.PersonInfoDialog(patInfo);
                perDialog.ShowDialog();
                if (perDialog.isCancel)
                {
                    throw new Exception("操作员取消操作");
                }
                MZMB_JBBM = perDialog.strDiagnosCode;
                YltclbStr = perDialog.strYltclb;
                outReimPara.RegInfo.NetType = YltclbStr;
                xzbzStr = perDialog.strXzbz;
                /// BxyjsStr = perDialog.BxyJs;
                ///BxyrqStr = perDialog.BxySpsj;
                ///
                if (perDialog.isFreeDrug == true)
                {
                    // mfyyStr = perDialog.strYllb;
                    switch (perDialog.strYllb)
                    {
                        case "糖尿病":
                            mfyyStr = "E14.901 ,";
                            outReimPara.RegInfo.NetDiagnosName = "糖尿病";
                            break;
                        case "原发性高血压":
                            mfyyStr = "I10  11 ,";
                            outReimPara.RegInfo.NetDiagnosName = "原发性高血压";
                            break;
                        case "冠心病":
                            mfyyStr = "I25.101 ,";
                            outReimPara.RegInfo.NetDiagnosName = "冠心病";
                            break;
                        default:
                            throw new Exception("免费用药请选择正确的疾病名称!");
                            break;
                    }
                    outReimPara.RegInfo.NetDiagnosCode = mfyyStr;
                }
                else
                {
                    outReimPara.RegInfo.NetDiagnosCode = MZMB_JBBM;
                    outReimPara.RegInfo.NetDiagnosName = perDialog.strDiagnosName;
                }


            }
            else
            {
                Dictionary<string, string> patInfo = QueryPersonInfo(frm.IDNO.Trim(), strYlCtlb, "", "", "", "C");
                dicPatInfo = patInfo;
                //outReimPara.RegInfo.CardNo = patInfo["kh"];                       //医疗卡号
                outReimPara.RegInfo.Memo1 = "";                                     //获取持卡人所在的社保机构编号
                outReimPara.RegInfo.CantonCode = patInfo["sbjgbh"];                 //行政区号
                outReimPara.RegInfo.MemberNo = patInfo["grbh"];                     //成员编码
                outReimPara.RegInfo.CompanyName = patInfo["dwmc"];                  //单位名称
                outReimPara.RegInfo.PatAddress = patInfo["dwmc"];                   //住址
                outReimPara.RegInfo.IdNo = patInfo["sfzhm"];                        //身份证号
                outReimPara.RegInfo.NetPatType = patInfo["ylrylb"];                 //人员类别
                //outReimPara.RegInfo.Balance = Convert.ToDecimal(patInfo["zhye"]);   //账户余额 
                outReimPara.RegInfo.NetPatName = patInfo["xm"];                     //姓名 
                //显示个人信息  
                if (!outReimPara.PatInfo.PatName.Equals(patInfo["xm"]))
                {
                    throw new Exception("his患者姓名" + outReimPara.PatInfo.PatName + "与医保登记姓名不一致，医保姓名：" + patInfo["xm"]);
                }
                Dialog.PersonInfoDialog perDialog = new Dialog.PersonInfoDialog(patInfo);
                perDialog.ShowDialog();
                if (perDialog.isCancel)
                {
                    throw new Exception("操作员取消操作");
                }
                MZMB_JBBM = perDialog.strDiagnosCode;
                YltclbStr = perDialog.strYltclb;
                //  YllbStr = perDialog.strYllb;
                // BxyjsStr = perDialog.BxyJs;
                /// BxyrqStr = perDialog.BxySpsj;
            }

            // throw new NotImplementedException();
        }


        /// <summary>
        /// 门诊预结算
        /// </summary>
        /// <param name="para"></param>
        public void OutNetworkPreSettle(OutPayParameter para)
        {
            outReimPara = para;
        }


        /// <summary>
        /// 门诊结算
        /// </summary>
        /// <param name="para"></param>
        public void OutNetworkSettle(OutPayParameter para)
        {
            outReimPara = para;

            //门诊结算 
            try
            {
                OutPatSettle();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("不允许消费账户"))
                {
                    MessageBox.Show("医保提示：不允许消费账户,将按自费收取");
                    //SaveOutSettleMainWithoutSettle();
                }
                else
                {
                    throw ex;
                }
            }
        }


        public void CancelOutNetworkSettle(OutPayParameter para)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region 保存门诊结算数据
        /// <summary>
        /// 保存门诊结算数据
        /// </summary>
        public void SaveOutSettleMain(int _payTypeId)
        {
            #region 保存农合中心返回值参数列表
            //保存农合中心返回值参数列表
            try
            {
                OutNetworkSettleList outNetworkSettleList;
                foreach (var item in dicSettleInfo)
                {
                    //if (dicSettleInfo["report"]=="report")

                    if (item.Key.ToString() == "report")
                    {
                        continue;
                    }
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
                LogManager.Error("保存农合中心返回值参数列表 插入值 失败" + ex.Message, ex);
            }
            #endregion

            OutNetworkSettleMain outSettleMain = new OutNetworkSettleMain();
            outSettleMain.SettleNo = dicSettleInfo["jshid"];                           //医保中心交易流水号
            outSettleMain.Amount = Convert.ToDecimal(dicSettleInfo["zje"]);            //本次医疗费用
            outSettleMain.GetAmount = Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]);    //本次现金支出
            outSettleMain.MedAmountZhzf = Convert.ToDecimal(dicSettleInfo["grzhzf"]);  //本次帐户支出
            outSettleMain.MedAmountTc = Convert.ToDecimal(dicSettleInfo["ybfdje"]);    //本次统筹支出
            outSettleMain.MedAmountDb = 0;                                             //本次大病支出
            outSettleMain.MedAmountBz = Convert.ToDecimal(dicSettleInfo["ylbzje"]);    //本次大病支出 
            outSettleMain.MedAmountTotal = Convert.ToDecimal(outSettleMain.Amount) - Convert.ToDecimal(outSettleMain.GetAmount);
            outSettleMain.NetworkingPatClassId = Convert.ToInt32(outReimPara.CommPara.NetworkPatClassId);
            outSettleMain.NetworkPatName = outReimPara.PatInfo.PatName;
            outSettleMain.NetworkPatType = "0";
            outSettleMain.OutNetworkSettleId = Convert.ToDecimal(outReimPara.CommPara.OutNetworkSettleId);
            outSettleMain.SettleBackNo = outReimPara.CommPara.OutNetworkSettleId.ToString("0");
            outSettleMain.SettleType = "1";

            outReimPara.SettleInfo = outSettleMain;

            //门诊付费方式 本接口 4 医保 6农合
            PayAPIInterface.Model.Comm.PayType payType;
            outReimPara.PayTypeList = new List<PayType>();
            payType = new PayAPIInterface.Model.Comm.PayType();
            payType.PayTypeId = _payTypeId;
            payType.PayTypeName = "医保";
            payType.PayAmount = outReimPara.SettleInfo.MedAmountTotal;
            outReimPara.PayTypeList.Add(payType);

        }
        #endregion

        #region 地纬门诊联网结算相关

        /// <summary>
        /// 门诊结算
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> OutReimSettle(bool isPreSettle = false)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            handelModel.InitHandle();

            string formatDate = DateTime.Now.ToString("yyyyMMdd");
            string formatDatetime = DateTime.Now.ToString("yyyyMMddhhmmss");
            handelModel.SBJGBH = dicPatInfo["sbjgbh"];
            string JBBM = "";
            string yltclb = YltclbStr;
            string yllb = YllbStr;
            string Yyxmbm = "";
            //门诊大病 地纬返回疾病编码
            if (YltclbStr == "4")
            {
                JBBM = MZMB_JBBM;
            }

            handelModel.AddInPara("p_blh", outReimPara.PatInfo.OutPatId.ToString("f0"));//*病历号	
            handelModel.AddInPara("p_yltclb", yltclb);                  //*医疗统筹类别	yltclb: 4 门诊大病，5 意外伤害，6普通门诊统筹 4：门诊慢性病，那么医疗类别只能传14、15；如果医疗统筹类别是6：普通门诊，那么医疗类别只能传11,54
            handelModel.AddInPara("p_yllb", yllb);                      //*11：普通门诊，54：计划生育，14：特殊疾病门诊，15：尿毒症新办法
            handelModel.AddInPara("p_grbh", dicPatInfo["grbh"]);        //*社会保障号码	
            handelModel.AddInPara("p_xm", dicPatInfo["xm"]);            //*姓名	参保病人的姓名
            handelModel.AddInPara("p_xb", dicPatInfo["xb"]);            //*性别	参保病人性别（1:男 2:女）
            handelModel.AddInPara("p_xzbz", "C");                       //*险种标志	医疗 C，工伤 D，生育 E
            //handelModel.AddInPara("p_fyrq", formatDate);                //*费用日期	
            //handelModel.AddInPara("p_ysbm", fixedOutDoc);                  //*医师编码	HIS必须传入一个非空的医师编码，并且保证医师有资格，HIS系统需要与地纬结算系统编码保持一致
            handelModel.AddInPara("p_jbbm", JBBM);                      //*疾病编码	yltclb=‘4’时，必须传递；yltclb=’5’或’6’时，xzbz=’C’传递’’,xzbz=’D’或xzbz=’E’，必须传递
            //handelModel.AddInPara("p_kh", dicPatInfo["kh"]);            //*卡号	读卡时若由地纬DLL控制读卡器，p_kh可不传；若由his控制读卡器，p_kh为必传；不读卡传’’。
            handelModel.AddInPara("p_qtzd", "");                        //其他诊断

            List<DarewayFeeMz> listFees = new List<DarewayFeeMz>();



            string notMatchedCharge = "";
            foreach (PayAPIInterface.Model.Comm.FeeDetail feeDetail in outReimPara.Details)
            {
                if (feeDetail.NetworkItemCode.ToString().Trim().Length == 0)
                {

                    notMatchedCharge += "编码:" + feeDetail.ChargeCode + "," + "名称:" + feeDetail.ChargeName + "；";
                }
            }

            if (notMatchedCharge.Trim().Length > 0)
            {
                if (MessageBox.Show("有以下项目未对应：\n" + notMatchedCharge + "\n是否继续？选择”是“将按自费项目进行收费报销。否则，取消本次收费报销！", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    throw new Exception("取消上传费用");
                }
            }





            foreach (var item in outReimPara.Details)
            {
                DarewayFeeMz fee = new DarewayFeeMz();

                //string sql = " SELECT  A.HIS_ITEM_CODE FROM  COMM.COMm.NETWORKING_ITEM_VS_HIS A LEFT JOIN COMM.COMM.CHARGE_PRICE_ALL_VIEW B ON SUBSTRING(A.HIS_ITEM_CODE,2, 50) = b.CHARGE_CODE WHERE B.FLAG_INVALID=0 AND CHARGE_ID='" + item.ChargeId.ToString("F0") + "' AND CHARGE_NAME='" + item.ChargeName + "' AND NETWORK_ITEM_CODE='" + item.NetworkItemCode + "' ";
                ////Yyxmbm = sqlHelperHis.ExecProc_ReStr(sql.ToString());
                //DataTable dtTable = sqlHelperHis.ExecSqlReDs(sql.ToString()).Tables[0];
                //if (dtTable.Rows.Count==0)
                //{
                //    throw new Exception("该项目编码：" + item.ChargeCode + "项目名称" + item.ChargeName + "为空，请检查是否对照上传");
                //}
                //Yyxmbm = dtTable.Rows[0][0].ToString();

                fee.yyxmbm = item.ChargeCode;
                fee.yyxmmc = item.ChargeName;

                if (string.IsNullOrEmpty(item.NetworkItemCode))
                {
                            MessageBox.Show("存在未对照" + "该项目编码：" + item.ChargeCode + "项目名称" + item.ChargeName + "");
                            throw new Exception("未对照操作被取消");
                }
                fee.ylxmbm = item.NetworkItemCode;
                fee.fyfssj = item.CreateTime.ToString("yyyyMMddhhmmss");
                fee.dj = item.Price;
                fee.sl = item.Quantity;
                //fee.bzsl = 1;
                fee.zje = item.Amount;
                fee.gg = item.Spec;
                fee.zxksbm = item.DeptCode;//fixedOutDept;
                fee.kdksbm = item.DeptCode;//fixedOutDept;
                fee.ysbm = item.DocCode;
                fee.ysmc = "";
                fee.gytj = "";
                fee.dcyl = "";
                fee.yypc = "";
                fee.yyts = "";
                fee.dffbz = "";
                fee.ybzdzf = "";

                //fee.sxzfbl = 0;

                listFees.Add(fee);
            }

            // MessageBox.Show(listFees.ToString());

            handelModel.AddInPara("p_fypd_ds", listFees);            //*数据集	*费用凭单信息

            //业务处理  settle_mz_pre
            string businessCode = isPreSettle ? "settle_mz_pre" : "settle_mz";

            handelModel.Handle(businessCode, true);
            //获取结果
            result = handelModel.GetResultDict();

            if (isPreSettle)
            {
                //Dialog.FrmSetteInfo frmSettleInfo = new Dialog.FrmSetteInfo(dicPatInfo, result);
                //frmSettleInfo.ShowDialog();
                //if (frmSettleInfo.isCancle)
                //{
                //    throw new Exception("操作员放弃结算！！！");
                //}
            }

            return result;
        }

        /// <summary>
        /// 门诊联网结算
        /// </summary>
        /// <param name="inPara">门诊接口入参</param>
        /// <returns></returns>
        public string OutPatSettle()
        {
            InterfaceInit();


            handelModel.SBJGBH = dicPatInfo["sbjgbh"];
            string Xm = dicPatInfo["xm"]; //姓名
            string Xb = dicPatInfo["xb"]; //性别
            string grbh = dicPatInfo["grbh"]; //性别

            //门诊支付方式
            int MZPayTypeId = 4;

        
           
                //门诊结算
                dicSettleInfo = OutReimSettle(false);

                //保存结算数据
                SaveOutSettleMain(MZPayTypeId);
                //打印结算单据
                try
                {

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }

          




            return "";
        }

        /// <summary>
        /// 撤销门诊结算
        /// </summary>
        /// <returns></returns>
        //public void CancelOutReimSettle(string p_blh, string p_jshid)
        public void CancelOutReimSettle(string p_jshid)
        {
            PayAPIResolver.Dareway.JNLX.JNLXDarewayInterfaceResolver handelModel = new PayAPIResolver.Dareway.JNLX.JNLXDarewayInterfaceResolver();
            Dictionary<string, string> result = new Dictionary<string, string>();
            //初始化
            handelModel.InitHandle();

            //handelModel.AddInPara("p_blh", p_blh);                    //*病历号	
            handelModel.AddInPara("p_jshid", p_jshid);                //*费用日期	

            //业务处理
            handelModel.Handle("destroy_mz", true);
            //获取结果
            result = handelModel.GetResultDict();
        }






        #endregion


        /// <summary>
        /// mz地纬格式费用
        /// </summary>
        public class DarewayFeeMz
        {

            ///// <summary>
            ///// 顺序号
            ///// </summary>
            //public string sxh
            //{
            //    get;
            //    set;
            //}

            ///// <summary>
            ///// 医嘱流水号
            ///// </summary>
            //public string yzlsh
            //{
            //    get;
            //    set;
            //}



            ///// <summary>
            /////医保项目编码
            ///// </summary>
            //public string ylxmmc
            //{
            //    get;
            //    set;
            //}

            /// <summary>
            /// 医院项目编码
            /// </summary>
            public string yyxmbm
            {
                get;
                set;
            }

            /// <summary>
            /// 医院项目名称
            /// </summary>
            public string yyxmmc
            {
                get;
                set;
            }
            /// <summary>
            ///医保项目编码
            /// </summary>
            public string ylxmbm
            {
                get;
                set;
            }
            /// <summary>
            /// 费用发生时间
            /// </summary>
            public string fyfssj
            {
                get;
                set;
            }

            /// <summary>
            /// 单价
            /// </summary>
            public decimal dj
            {
                get;
                set;
            }

            /// <summary>
            /// 数量
            /// </summary>
            public decimal sl
            {
                get;
                set;
            }

            /// <summary>
            /// 总金额
            /// </summary>
            public decimal zje
            {
                get;
                set;
            }

            /// <summary>
            /// 执行科室编码
            /// </summary>
            public string zxksbm
            {
                get;
                set;
            }

            /// <summary>
            /// 开单科室编码
            /// </summary>
            public string kdksbm
            {
                get;
                set;
            }

            /// <summary>
            /// 医师编码
            /// </summary>
            public string ysbm
            {
                get;
                set;
            }

            /// <summary>
            /// 医师名称
            /// </summary>
            public string ysmc
            {
                get;
                set;
            }

            /// <summary>
            /// 规格
            /// </summary>
            public string gg
            {
                get;
                set;
            }

            /// <summary>
            /// 单位
            /// </summary>
            public string dw
            {
                get;
                set;
            }

            /// <summary>
            /// 给药途径
            /// </summary>
            public string gytj
            {
                get;
                set;
            }

            /// <summary>
            /// 单次用量
            /// </summary>
            public string dcyl
            {
                get;
                set;
            }

            /// <summary>
            /// 用药频次
            /// </summary>
            public string yypc
            {
                get;
                set;
            }

            /// <summary>
            /// 用药天数
            /// </summary>
            public string yyts
            {
                get;
                set;
            }

            /// <summary>
            /// 中草药单复方标志
            /// 0：单方，1：复方
            /// 
            /// </summary>
            public string dffbz
            {
                get;
                set;
            }

            /// <summary>
            /// 医保指定自费
            /// 2：指定自费
            /// </summary>
            public string ybzdzf
            {
                get;
                set;
            }


            /// <summary>
            /// 费用区分标志
            /// </summary>
            public string fyqfbz
            {
                get;
                set;
            }

            ///// <summary>
            ///// 用药说明
            ///// </summary>
            //public string yysm
            //{
            //    get;
            //    set;
            //}



            ///// <summary>
            ///// 包装数量
            ///// </summary>
            //public decimal bzsl
            //{
            //    get;
            //    set;
            //}





            ///// <summary>
            ///// 首选自付比例
            ///// </summary>
            //public decimal sxzfbl
            //{
            //    get;
            //    set;
            //}



            /// <summary>
            /// 收费员姓名
            /// </summary>
            //public string sfryxm
            //{
            //    get
            //    {
            //        return PayAPIConfig.Operator.UserName;
            //    }
            //}
        }



        #region 住院业务

        public void InNetworkRegister(InPayParameter para)
        {
            throw new NotImplementedException();
        }


        public void InNetworkPreSettle(InPayParameter para)
        {
            throw new NotImplementedException();
        }


        public void InNetworkSettle(InPayParameter para)
        {
            throw new NotImplementedException();
        }


        public void CancelInNetworkSettle(InPayParameter para)
        {
            throw new NotImplementedException();
        }


        public void CancelInNetworkRegister(InPayParameter para)
        {
            throw new NotImplementedException();
        }








        #endregion
    }
}
