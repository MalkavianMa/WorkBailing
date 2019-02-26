using PayAPIInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using PayAPIInterface.Model.Comm;
using PayAPIInterface.ParaModel;
using PayAPIUtilities.Log;
using System.Windows.Forms;
using PayAPIInterface.Model.Out;
using PayAPIInstance.JingQi.FuYang.Dialogs;
using PayAPIInterface.Model.In;
using PayAPIInterfaceHandle.AHFYCityWebReference;
using PayAPIClassLib.BLL;

namespace PayAPIInstance.JingQi.FuYang
{
    /// <summary>
    /// 阜阳农合接口
    /// </summary>  
    public class FYNHCityInterfaceModel : IPayCompanyInterface
    {
        #region 参数
        /// <summary>
        /// 门诊入参
        /// </summary>
        OutPayParameter outPayPara;

        /// <summary>
        /// 住院入参
        /// </summary>
        InPayParameter inPayPara;

        /// <summary>
        /// 联网患者信息
        /// </summary>
        NetPatInfo netPatInfo = new NetPatInfo();

        /// <summary>
        /// 数据库连接
        /// </summary>
        public MSSQLHelper sqlHelper = new MSSQLHelper(PubComm.ConnStr);

        /// <summary>
        /// 医院编码
        /// </summary>
        public string sHospitalCode = "5AB552AAF3DB47E055E06177CF51A5C4";       //医院在农合系统中存在的编号 长度20


        /// <summary>
        /// 区域编号
        /// </summary>DF
        public string sAreaCode = "";

        /// <summary>
        /// //逐条上传费用标志，只针对颍上农合          
        /// </summary>
        public bool OneByOneUploadFlag = false;

        /// <summary>
        /// 身份属性名称
        /// </summary>
        private string strIdeName = "";

        /// <summary>
        /// 出院状态
        /// </summary>
        public string outHosStatus = "";


        /// <summary>
        /// 用户名
        /// </summary>
        public string strUserName = "001";

        /// <summary>
        /// 密码
        /// </summary>
        private string strUserPwd = "001";

        public OperatorInfo operatorInfo
        {
            get
            {
                return PayAPIUtilities.Config.PayAPIConfig.Operator;
            }
        }

        /// <summary>
        /// 返回结果
        /// </summary>
        public string sResult = "";

        /// <summary>
        /// 信息
        /// </summary>
        public string sMessage = "";

        /// <summary>
        /// 总金额
        /// </summary>
        public string totalAmount = "0.00";


        /// <summary>
        /// 转诊单号
        /// </summary>
        public string ZZDH = "";

        /// <summary>
        /// 转诊类型
        /// </summary>
        public string ZZLX = "";

        /// <summary>
        /// 农合操作类
        /// </summary>
        IJQCenterWebServiceservice serr = new IJQCenterWebServiceservice();
        #endregion

        #region 读卡
        /// <summary>
        /// 读取农合卡
        /// </summary>
        /// <param name="nStatus">0为入院 1为出院</param>
        /// <returns></returns>
        public void ReadMedCard(int nStatus, bool isOut = false)
        {
            Dialogs.frmReadCard frmRC = new Dialogs.frmReadCard(netPatInfo, isOut, inPayPara, outPayPara);
            frmRC.nStatus = nStatus;
            frmRC.ShowDialog();

            sAreaCode = frmRC.sAreaCode;
            OneByOneUploadFlag = frmRC.ZTSCBZ.Checked;

            if (!frmRC.bIsValid)
            {
                throw new Exception("操作员取消操作");
            }
            netPatInfo = frmRC.netPatInfo;

            strIdeName = frmRC.strIdeName;
            ZZDH = frmRC.ZZDH;
            ZZLX = frmRC.ZZLX;

            if (inPayPara != null)
            {
                //住院患者读卡
                inPayPara.RegInfo.PatAddress = netPatInfo.patAddress;//地址
                inPayPara.RegInfo.CantonCode = netPatInfo.strRedeemNo;//补偿类型
                inPayPara.RegInfo.CardNo = netPatInfo.ICNo;
                //来院状态
                inPayPara.RegInfo.Memo1 = netPatInfo.strInHosId;//入院状态
                inPayPara.RegInfo.IdNo = netPatInfo.IDNo;            //身份证号
                //入院科室 要用中心的科室编码 名称
                inPayPara.PatInfo.InNetWorkDeptCode = netPatInfo.strInOfficeId;
                inPayPara.RegInfo.NetDiagnosCode = netPatInfo.strInDiagnoCode;
                inPayPara.RegInfo.NetDiagnosName = netPatInfo.strInDiagnoName;
                inPayPara.RegInfo.MemberNo = netPatInfo.strMemberNo;
                //治疗方式netPatInfo.medicalNo
                // inReimPara.Memo1 = netPatInfo.strTreatCode;
                inPayPara.RegInfo.Memo2 = frmRC.treatname;
                inPayPara.RegInfo.NetType = netPatInfo.strCureId;
                //家庭编码
                inPayPara.RegInfo.CompanyName = netPatInfo.strFamilySysno;
                //身份属性名称
                inPayPara.RegInfo.NetPatType = strIdeName;
                //出院信息
                inPayPara.RegInfo.OutDiagnoseCode = netPatInfo.strOutDiagnoCode;
                inPayPara.PatInfo.OutNetWorkDeptCode = netPatInfo.strOutOfficeId;
                inPayPara.RegInfo.OutDiagnoseName = frmRC.OutDiagnoseName;
                //inReimPara.InPatName = netPatInfo.patName;
                //inReimPara.Memo1 = netPatInfo.strProcreateNotice;///治疗方式
                inPayPara.RegInfo.Memo2 = netPatInfo.strTelNo;
            }

            if (outPayPara != null)
            {
                outPayPara.RegInfo.PatAddress = netPatInfo.patAddress;//地址
                outPayPara.RegInfo.CantonCode = netPatInfo.strRedeemNo;//补偿类型
                outPayPara.RegInfo.CardNo = netPatInfo.ICNo;
                //来院状态
                outPayPara.RegInfo.Memo1 = netPatInfo.strInHosId;//入院状态
                outPayPara.RegInfo.IdNo = netPatInfo.IDNo;            //身份证号
                //入院科室 要用中心的科室编码 名称 
                outPayPara.RegInfo.NetDiagnosCode = netPatInfo.strInDiagnoCode;
                outPayPara.RegInfo.NetDiagnosName = netPatInfo.strInDiagnoName;
                outPayPara.RegInfo.MemberNo = netPatInfo.strMemberNo;
                //治疗方式netPatInfo.medicalNo
                // inReimPara.Memo1 = netPatInfo.strTreatCode;
                outPayPara.RegInfo.Memo2 = frmRC.treatname;
                outPayPara.RegInfo.NetType = netPatInfo.strCureId;
                //家庭编码
                outPayPara.RegInfo.CompanyName = netPatInfo.strFamilySysno;
                //身份属性名称
                outPayPara.RegInfo.NetPatType = strIdeName;
                //inReimPara.InPatName = netPatInfo.patName;
                //inReimPara.Memo1 = netPatInfo.strProcreateNotice;///治疗方式
                outPayPara.RegInfo.Memo2 = netPatInfo.strTelNo;
            }

            frmRC.Dispose();
        }
        #endregion

        #region 门诊联网登记
        /// <summary>
        /// 门诊联网登记
        /// </summary>
        public void OutPatReimRegister()
        {
            string sInpatientID = "";
            string phone = "15863525437";

            int registerResult = serr.InpatientRegister(
                sAreaCode,                                            // sAreaCode	地区代码
                sHospitalCode,                                        //sHospitalCode	医疗机构编号
                outPayPara.PatInfo.OutPatId.ToString(),               //sInpatientCode	HIS住院号 
                netPatInfo.medicalNo,
                netPatInfo.medicalNo,                                        //"342128196109140699",//sCardCode	医疗卡号
                netPatInfo.ICNo,                                      //sPeopCode	人员编号
                netPatInfo.patName,                                   //sPeopName	姓名
                netPatInfo.sex,                                       //sSex	性别k
                netPatInfo.age.ToString(),                            //sAge	年龄
                netPatInfo.birthday.ToString("yyyy-MM-dd"),                       //sBirthDay	出生日期
                netPatInfo.IDNo,                                      //sIDCardNo	身份证号码
                netPatInfo.strOutDiagnoCode,                          //sDiagnoseCodeIn1	入院疾病诊断编码1
                netPatInfo.strOutOfficeName,                          //sDiagnoseNameIn1	入院疾病诊断名称1
                "",                                                   //sDiagnoseCodeIn2	入院疾病诊断编码2
                "",                                                   //sDiagnoseNameIn2	入院疾病诊断名称2
                "",                                                   //sDiagnoseCodeIn3	入院疾病诊断编码3
                "",                                                   //sDiagnoseNameIn3	入院疾病诊断名称3
                "",                                                   //sOperationCode1	手术编码1
                "",                                                   //sOperationName1	手术名称1
                "",                                                   //sOperationCode2	手术编码2
                "",                                                   //sOperationName2	手术名称2
                "",                                                   //sOperationCode3	手术编码3
                "",                                                   //sOperationName4	手术名称3
                "无",                                                 //sSectionOfficeName	HIS入院科室名称
                "无",                                                 //sSectionOfficeCode	中心入院科室编码
                netPatInfo.strCureId,                                 //sCureCode	就诊类型编码
                netPatInfo.strInHosId,                                //sInHospitalCode	入院状态编码 
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),         //sInHosptialDate	入院时间
                "0",                                                  //sBed	床号
                "",                                                   //sDoctorName	床位医生姓名
                "00",                                                 //sChangeCode	转诊类型编号
                "",                                                   //sChangeRCode	转诊单号
                "",                                                   //sCivilCode	民政通知书号
                "",                                                   //sBearCode	生育证号
                operatorInfo.UserName,//sOperatorName	操作人姓名 
                phone,                                                //电话
                "",                                                   //sObligate2	预留字段2
                "",                                                   //sObligate3	预留字段3
                "",                                                   //sObligate4	预留字段4
                "",                                                   //sObligate5	预留字段5
                ref sInpatientID,                                     //sInpatientID	就诊ID
                ref sMessage                                          //sMessage	错误/提示信息
            );

            if (registerResult == 0)
            {
                LogManager.Info(sMessage.ToString() + "InpatientRegister");
                throw new Exception(sMessage);
            }
            else
            {
                LogManager.Info(sInpatientID.ToString() + "+" + netPatInfo.patName + " InpatientRegister");
                //写的话往NetWorkBillNo中，读的话，从NetWorkRegNo中。
                outPayPara.RegInfo.NetRegSerial = sInpatientID.ToString();
                //防止重新登记后，联网登记流水号改变但是没更新
            }
            outPayPara.RegInfo.CantonCode = sAreaCode;
        }
        #endregion

        #region 门诊费用批量上传

        decimal MZTotalAmount = 0;
        /// <summary>
        /// 批量上传
        /// </summary>
        public void OutPatReimUpItems_batch()
        {
            MZTotalAmount = 0;
            List<OutNetworkUpDetail> details = outPayPara.Details;

            //  #region 如果费用明细里有未和农合对应的情况，则抛出异常终止操作 
            string notMatchedCharge = "";
            foreach (var item in details)
            {
                if (item.NetworkItemCode.ToString().Trim().Length == 0)
                {
                    notMatchedCharge += "编码:" + item.ChargeCode + "," + "名称:" + item.ChargeName + "；";
                }
            }
            if (notMatchedCharge.Trim().Length > 0)
            {
                if (MessageBox.Show("有以下项目未对应：\n" + notMatchedCharge + "\n 请联系农合办对照此项目后在办理出院手续。", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    throw new Exception("取消上传费用");
                }
            }

            // 上传明细时的基本信息 
            int EveryCount = 1000, n = 1;
            StringBuilder sInput = new StringBuilder();

            sInput.Append("<?xml version=\"1.0\" encoding=\"gb2312\" ?><JQWebService>");
            sInput.Append("<RowCount>" + details.Count + "</RowCount>");

            StringBuilder fyid = new StringBuilder();

            foreach (var item in details)
            {
                MZTotalAmount += item.Amount;

                fyid.Append(item.AutoId + ",");
                sInput.Append("<Row" + n + ">");
                if (item.Spec.ToString() == "") { item.Spec = "无"; }
                if (item.DrugFormName.ToString() == "") { item.DrugFormName = "无"; }
                if (item.NetworkItemCode.ToString() == "")
                {
                    //改为目录外药品
                    item.NetworkItemCode = "10300000954";
                    item.NetworkItemClass = "0";
                }
                //    名称	In/Out	字段	是否可为空	说明
                ///  sInput.Append("<sInpatientID>" + inReimPara.NetWorkRegNo + "</sInpatientID>");//就诊ID	In	sInpatientID		通过InpatientRegister取得的ID
                sInput.Append("<sCenterItemCode>" + item.NetworkItemCode + "</sCenterItemCode>");                                        //中心项目编码	In	sCenterItemCode		
                sInput.Append("<sItemKey>" + item.AutoId + "</sItemKey>");                                                                //HIS记帐关键字	In	sItemKey		
                sInput.Append("<sItemType>" + item.NetworkItemClass + "</sItemType>");                                            //费用类型	In	sItemType		0:西药;1:成药;2:草药:6:特殊检查;9:诊疗项目
                sInput.Append("<sReceiptName>" + item.ChargeName + "</sReceiptName>");                                                    //HIS发票项目名称	In	sReceiptName		
                sInput.Append("<sItemCode>" + item.ChargeCode + "</sItemCode>");                                                          //HIS药品/项目编码	In	sItemCode		
                sInput.Append("<sItemName>" + item.ChargeName + "</sItemName>");                                                          //HIS药品/项目名称	In	sItemName	
                sInput.Append("<sItemSpec>" + item.Spec + "</sItemSpec>");                                                                 //HIS药品/项目规格	In	sItemSpec		没有内容请传汉字”无”
                sInput.Append("<sItemDose>" + item.DrugFormName + "</sItemDose>");                                                       //HIS药品/项目剂型	In	sItemDose	是	没有内容请传汉字”无”
                sInput.Append("<sItemArea>" + "" + "</sItemArea>");                                                                         //HIS药品/项目产地	In	sItemArea	是	没有内容请传汉字”无”
                sInput.Append("<sItemProc>" + "" + "</sItemProc>");                                                                         //HIS药品/项目加工过程	In	sItemProc	是	没有内容请传汉字”无”
                sInput.Append("<sItemPart>" + "" + "</sItemPart>");                                                                         //HIS药品/项目入药部位	In	sItemPart	是	没有内容请传汉字”无”
                sInput.Append("<sIfCompound>" + "0" + "</sIfCompound>");                                                                    //是否复方	In	sIfCompound		0：单方1：复方2：非草药
                sInput.Append("<sTime>" + item.Quantity + "</sTime>");                                                                     //数量	In	sTime		整数
                sInput.Append("<sUnit>" + item.Unit + "</sUnit>");                                                                         //HIS项目单位	In	sUnit		
                sInput.Append("<sPrice>" + item.Price + "</sPrice>");                                                                      //单价	In	sPrice		格式0.000
                sInput.Append("<sSum>" + item.Amount + "</sSum>");                                                                         //总金额	In	sSum		格式0.00
                sInput.Append("<sSectionOfficeName>" + item.DeptCode + "</sSectionOfficeName>");                                          //HIS科室名称	In	sSectionOfficeName		
                sInput.Append("<sSectionOfficeCode>" + netPatInfo.strInOfficeId + "</sSectionOfficeCode>");                             //中心科室编码	In	sSectionOfficeCode		
                sInput.Append("<sDoctorName>" + item.DocCode + "</sDoctorName>");                                                         //医生名称	In	sDoctorName		
                sInput.Append("<sBed>" + "" + "</sBed>");                                                                                   //床号	In	sBed	是	
                sInput.Append("<sOperatorDate>" + item.CreateTime.ToString("yyyy-MM-dd hh:mm:ss") + "</sOperatorDate>");//记帐时间	In	sOperatorDate		格式：YYYY-MM-DD HH:MM:SS
                sInput.Append("<sInputName>" + item.DocCode + "</sInputName>");                                                                //记帐人姓名	In	sInputName		
                sInput.Append("</Row" + n + ">");

            }
            sInput.Append("</JQWebService>");


            int strRe = serr.InpatientFeeUpLoadAll(sAreaCode, outPayPara.RegInfo.NetRegSerial, sHospitalCode, sInput.ToString(), "", "", "", "", "", ref sResult, ref sMessage);

            if (strRe == 0)
            {
                LogManager.Info(sMessage.ToString() + " OutPatientFeeUpLoadAll");
                throw new Exception(sMessage);
            }
            else
            {
                LogManager.Info(sResult.ToString() + " OutPatientFeeUpLoadAll");
            }
        }
        #endregion

        #region 门诊病号单病种信息上传
        /// <summary>
        /// 单病种信息上传
        /// </summary>
        public void OutpatDiagnosisUpdate()
        {
            int registerResult = serr.InpatDiagnosisUpdate(
                //   名称	In/Out	字段	是否可为空	说明
                sAreaCode,                                   //地区代码	In	sAreaCode		见附表1 
                strUserName,                                /// RIConfig.UserCode,//用户名	In	sUserCode		当前用户在新农合系统的登录用户名
                strUserPwd,                                 //"",//密码	In	sUserPass		当前用户在新农合系统的登录用户口令
                sHospitalCode,                               //医疗机构编号	In	sHospitalCode		见附表2
                outPayPara.RegInfo.NetRegSerial,                    //就诊ID	In	sInpatientID		通过InpatientRegister取得的ID
                "",                                          //身高（CM）	In	sStature	是	
                "",                                          //体重（KG）	In	sWeight	是	
                netPatInfo.strProcreateNotice,               //治疗方式编码	In	sTreatCode		
                netPatInfo.strOutOfficeId,                   //单病种ICD编码	In	sIcdno		
                netPatInfo.strOutOfficeName,                 //单病种ICD名称	In	sIcdName		
                netPatInfo.strInOfficeId,                    //中心入院科室编码	In	sSectionOfficeCode		见附表S201-03
                netPatInfo.strCureId,                        //就诊类型编码	In	sCureCode		见附表S301-05
                netPatInfo.strInHosId,                       //入院状态编码	In	sInHospitalCode		见附表S301-02
                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),//入院时间	In	sInHosptialDate		格式：YYYY-MM-DD HH:MM:SS
                operatorInfo.UserName,//操作人姓名	In	sOperatorName		
                "",                                          //预留字段1	In	sObligate1	是	
                "",                                          //预留字段2	In	sObligate2	是	
                "",                                          //预留字段3	In	sObligate3	是	
                "",                                          //预留字段4	In	sObligate4	是	
                "",                                          //预留字段5	In	sObligate5	是	
                ref sMessage                                 //错误/提示信息	Out	sMessage		成功返回空/提示信息，否则返回错误提示信息

           );

            if (registerResult == 0)
            {
                LogManager.Info(sMessage.ToString() + "  sMessage" + "InpatDiagnosisUpdate失败");
                throw new Exception(sMessage);
            }
            else
            {
                LogManager.Info(sMessage.ToString() + " sMessage" + "InpatDiagnosisUpdate成功");
            }
        }
        #endregion

        #region 门诊结算
        /// <summary>
        /// 对端门诊结算
        /// </summary> 
        public void OutPatReimSettle()
        {

            string sResult = "";
            int Re = serr.InpatientCalculate(sAreaCode, outPayPara.RegInfo.NetRegSerial, sHospitalCode, netPatInfo.strRedeemNo, outPayPara.RegInfo.NetRegSerial, MZTotalAmount.ToString(), operatorInfo.UserName, "", "", "", "", "", ref sResult, ref sMessage);

            if (Re == 0)
            {
                LogManager.Info(sMessage.ToString() + " InpatientCalculate");
                string xxx = sMessage;
                MessageBox.Show(xxx, "系统提示");
                CancelReimSettle();
                CancelOutReigster();
                throw new Exception(xxx);
            }

            LogManager.Info(sResult.ToString() + " InpatientCalculate");
            //////////////////////////////////////////////////////////////////////////
            //保存农合返回值   !!! 
            //  LogManager.RecordOperateLog(sResult, "农合结算返回值");
            XmlDocument ReResulst = new XmlDocument();
            string result = sResult;                                                                 //进行医保业务
            ReResulst.LoadXml(result);

            string BRFD = "", YYFD = "";

            BRFD = ReResulst.SelectSingleNode("/JQWebService/Row1/sPatientAssume").InnerText;
            YYFD = ReResulst.SelectSingleNode("/JQWebService/Row1/sHospialAssume").InnerText;
            InfoPreSettle info = new InfoPreSettle(inPayPara, true);
            info.Getcalinfo(BRFD, YYFD);
            try
            {
                info.Amount = ReResulst.SelectSingleNode("/JQWebService/Row1/sAllInCost").InnerText; //本次住院总医疗费用
                info.KBXFY = ReResulst.SelectSingleNode("/JQWebService/Row1/sAllApply").InnerText;   //可报销费用
                info.QFX = ReResulst.SelectSingleNode("/JQWebService/Row1/sBegin").InnerText;        //起付线
                info.JJZFJE = ReResulst.SelectSingleNode("/JQWebService/Row1/sFund").InnerText;      //基金支付金额
                info.ZHZF = ReResulst.SelectSingleNode("/JQWebService/Row1/sAccount").InnerText;     //帐户支付
                info.ZHYE = ReResulst.SelectSingleNode("/JQWebService/Row1/sAccountBegin").InnerText;//帐户余额
                info.ZFJE = ReResulst.SelectSingleNode("/JQWebService/Row1/sSelfCost").InnerText;    //自费金额
                info.ZJFJE = ReResulst.SelectSingleNode("/JQWebService/Row1/sOwnCost").InnerText;    //自己付金额 
                info.YQJCZE = ReResulst.SelectSingleNode("/JQWebService/Row1/sOutHCheck").InnerText; //院前检查总额
                info.YQJCBXJE = ReResulst.SelectSingleNode("/JQWebService/Row1/sOutCheck").InnerText;//院前检查报销总额
                info.MZJZ = ReResulst.SelectSingleNode("/JQWebService/Row1/sCivilComp").InnerText;//mingzheng
                info.DBBC = ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate1").InnerText;//大病补偿
                info.czdd = ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate4").InnerText;//财政兜底资金金额
                info.yibaling = ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate4").InnerText;//180fPoorSecondComp
            }
            catch (System.Exception ex)
            {
            }
            finally
            {
                info.ShowDialog();

                if (info.restr == "2")
                {
                    throw new Exception("撤销农合结算成功！");
                }

                if (info.restr == "1")
                {
                    throw new Exception("撤销农合结算成功,撤销农合出院登记成功！");
                }
            }

            OutNetworkSettleMain outSettleMain = new OutNetworkSettleMain();
            #region 解析正式收费数据
            outSettleMain.IsSettle = true;                                                                                            //结算标志，还有其他要修改的属性
            outSettleMain.OutPatId = outPayPara.PatInfo.OutPatId;
            decimal fpbc = 0;
            decimal dbbc = 0;
            decimal CZDD = 0;
            decimal noje = 0;
            decimal yibo = 0;
            outSettleMain.SettleNo = ReResulst.SelectSingleNode("/JQWebService/Row1/sCalculateCode").InnerText;                       //医保中心交易流水号 单据号
            outSettleMain.Amount = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sAllInCost").InnerText);          //医疗总费用       
            outSettleMain.MedAmountZhzf = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sAccount").InnerText);     //账户支付 
            outSettleMain.MedAmountTc = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sFund").InnerText);          //统筹支付 
            outSettleMain.MedAmountBz = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sAllApply").InnerText);      //可报销总费用 
            outSettleMain.MedAmountJm = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sBegin").InnerText); ;       //起伏线金额 
            outSettleMain.MedAmountGwy = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sHospialAssume").InnerText);//医院承担金额 
            outSettleMain.GetAmount = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sPatientAssume").InnerText);   //病人承担金额 
            outSettleMain.NetworkPatType = sAreaCode;
            outSettleMain.MedAmountDb = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sFund").InnerText);          //    区县   帐户支付后余额	sAccountAfter		格式：0.00
            fpbc = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sCivilComp").InnerText); // 民证
            yibo = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/fPoorSecondComp").InnerText);//180
            dbbc = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate1").InnerText);//大病补偿金额
            if (ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate5").InnerText == "")
            {
                noje = 0;
            }
            else
            {
                noje = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate5").InnerText);//农合预留
            }
            CZDD = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate4").InnerText);//政府兜底
            outSettleMain.MedAmountTotal = outSettleMain.MedAmountTc + outSettleMain.MedAmountGwy + fpbc + dbbc + CZDD + noje + yibo;
            outSettleMain.MedAmountTc = outSettleMain.MedAmountTotal;
            outSettleMain.GetAmount = outSettleMain.Amount - outSettleMain.MedAmountTotal;
            outSettleMain.CreateTime = DateTime.Now;
            outSettleMain.NetworkPatName = netPatInfo.patName;                      //户主姓名 
            outSettleMain.NetworkRefundTime = Convert.ToDateTime("2000-01-01");
            outSettleMain.NetworkSettleTime = DateTime.Now;
            outSettleMain.SettleBackNo = outPayPara.RegInfo.NetRegSerial;                  //住院登记流水号 中心住院号 唯一标识
            outSettleMain.SettleType = netPatInfo.medicalType;                     //补偿类型名称   

            outPayPara.SettleInfo = outSettleMain;
            #endregion


            //添加支付方式和相应的对应的报销金额  

            PayAPIInterface.Model.Comm.PayType payType = new PayAPIInterface.Model.Comm.PayType();
            payType.PayTypeId = 6;
            payType.PayTypeName = "农合";
            payType.PayAmount = outSettleMain.MedAmountTotal;
            outPayPara.PayTypeList = new List<PayType>();
            outPayPara.PayTypeList.Add(payType);


            string sCalculateCode = "";

            #region  //将结算数据全部保存到服务器
            string stra = "";
            try
            {

                //sAreaCode, inReimPara.NetWorkRegNo, sHospitalCode, netPatInfo.strRedeemNo, inReimPara.NetWorkRegNo, inReimPara.MedAmount.ToString(), operatorInfo.UserName,
                string sAreaCode1 = sAreaCode;//            地区代码	In	sAreaCode	
                string sInpatientID = outPayPara.RegInfo.NetRegSerial;//就诊ID	In	sInpatientID	
                //医疗机构编号	In	sHospitalCode	
                //补偿类别编码	In	sCalcCode	
                //发票号码	In	sReceiptCode	
                //HIS住院发生总费用	In	sAllInCost	
                //操作人姓名	In	sOperatorName	
                try
                {
                    sCalculateCode = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sCalculateCode").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sCalculateCode").InnerText);//结算单号	sCalculateCode		需要保存
                }
                catch (System.Exception ex)
                {
                    sCalculateCode = "";
                }
                string sAllInCost = "";
                try
                {
                    sAllInCost = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sAllInCost").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sAllInCost").InnerText);//医疗总费用	sAllInCost		格式：0.00
                }
                catch (System.Exception ex)
                {
                    sAllInCost = "";
                }
                string sAllApply = "";
                try
                {
                    sAllApply = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sAllApply").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sAllApply").InnerText);//可报销总费用	sAllApply		格式：0.00
                }
                catch (System.Exception ex)
                {
                    sAllApply = "";
                }
                string sBegin = "";
                try
                {
                    sBegin = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sBegin").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sBegin").InnerText);//起付线	sBegin		格式：0.00
                }
                catch (System.Exception ex)
                {
                    sBegin = "";
                }
                string sFund = "";
                try
                {
                    sFund = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sFund").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sFund").InnerText);//基金支付	sFund		格式：0.00:包含中医诊疗增补+院外检查+基本药物增补+民政补偿
                }
                catch (System.Exception ex)
                {
                    sFund = "";
                }
                string sAccount = "";
                try
                {
                    sAccount = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sAccount").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sAccount").InnerText);//帐户支付	sAccount		格式：0.00
                }
                catch (System.Exception ex)
                {
                    sAccount = "";
                }
                string sAccountBegin = "";
                try
                {
                    sAccountBegin = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sAccountBegin").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sAccountBegin").InnerText);//帐户支付前余额	sAccountBegin		格式：0.00
                }
                catch (System.Exception ex)
                {
                    sAccountBegin = "";
                }
                string sAccountAfter = "";
                try
                {
                    sAccountAfter = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sAccountAfter").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sAccountAfter").InnerText);//帐户支付后余额	sAccountAfter		格式：0.00
                }
                catch (System.Exception ex)
                {
                    sAccountAfter = "";
                }
                string sSumFund = "";
                try
                {
                    sSumFund = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sSumFund").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sSumFund").InnerText);//年度基金累计支付	sSumFund		格式：0.00包含本次支付
                }
                catch (System.Exception ex)
                {
                    sSumFund = "";
                }
                string sInHospialCount = "";
                try
                {
                    sInHospialCount = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sInHospialCount").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sInHospialCount").InnerText);//住院次数	sInHospialCount		格式：0.00包含本次住院
                }
                catch (System.Exception ex)
                {
                    sInHospialCount = "";
                }
                string sSelfCost = "";
                try
                {
                    sSelfCost = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sSelfCost").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sSelfCost").InnerText);//自费金额	sSelfCost		格式：0.00
                }
                catch (System.Exception ex)
                {
                    sSelfCost = "";
                }
                string sOwnCost = "";
                try
                {
                    sOwnCost = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sOwnCost").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sOwnCost").InnerText);//自付金额	sOwnCost		格式：0.00
                }
                catch (System.Exception ex)
                {
                    sOwnCost = "";
                }
                string sMedSum = "";
                try
                {
                    sMedSum = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sMedSum").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sMedSum").InnerText);//药品总金额	sMedSum		格式：0.00
                }
                catch (System.Exception ex)
                {
                    sMedSum = "";
                }
                string sMedAppSum = "";
                try
                {
                    sMedAppSum = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sMedAppSum").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sMedAppSum").InnerText);//药品可报销总金额	sMedAppSum		格式：0.00
                }
                catch (System.Exception ex)
                {
                    sMedAppSum = "";
                }
                string sCMeSum = "";
                try
                {
                    sCMeSum = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sCMeSum").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sCMeSum").InnerText);//中医药品/诊疗总金额	sCMeSum		格式：0.00
                }
                catch (System.Exception ex)
                {
                    sCMeSum = "";
                }
                string sCMeASum = "";
                try
                {
                    sCMeASum = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sCMeASum").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sCMeASum").InnerText);//中医药品/诊疗可报销总金额	sCMeASum		格式：0.00
                }
                catch (System.Exception ex)
                {
                    sCMeASum = "";
                }
                string sOutHCheck = "";
                try
                {
                    sOutHCheck = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sOutHCheck").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sOutHCheck").InnerText);//院外检查总金额	sOutHCheck		格式：0.00
                }
                catch (System.Exception ex)
                {
                    sOutHCheck = "";
                }
                string sOutCheckApp = "";
                try
                {
                    sOutCheckApp = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sOutCheckApp").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sOutCheckApp").InnerText);//院外检查可报销总金额	sOutCheckApp		格式：0.00
                }
                catch (System.Exception ex)
                {
                    sOutCheckApp = "";
                }
                string sOutCheck = "";
                try
                {
                    sOutCheck = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sOutCheck").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sOutCheck").InnerText);//院外检查报销总金额	sOutCheck		
                }
                catch (System.Exception ex)
                {
                    sOutCheck = "";
                }
                string sHostName = "";
                try
                {
                    sHostName = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sHostName").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sHostName").InnerText);//户主姓名	sHostName		
                }
                catch (System.Exception ex)
                {
                    sHostName = "";
                }
                string sYear = "";
                try
                {
                    sYear = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sYear").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sYear").InnerText);//业务年份	sYear		年份(整数),如2009
                }
                catch (System.Exception ex)
                {
                    sYear = "";
                }
                string sCalculateDate = "";
                try
                {
                    sCalculateDate = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sCalculateDate").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sCalculateDate").InnerText);//结算时间	sCalculateDate		格式:YYYY-MM-DD HH:MM:SS
                }
                catch (System.Exception ex)
                {
                    sCalculateDate = "";
                }
                string sTownName = "";
                try
                {
                    sTownName = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sTownName").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sTownName").InnerText);//乡镇名称	sTownName		
                }
                catch (System.Exception ex)
                {
                    sTownName = "";
                }
                string sVillageName = "";
                try
                {
                    sVillageName = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sVillageName").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sVillageName").InnerText);//村名称	sVillageName		
                }
                catch (System.Exception ex)
                {
                    sVillageName = "";
                }
                string sGroupName = "";
                try
                {
                    sGroupName = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sGroupName").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sGroupName").InnerText);//组名称	sGroupName		
                }
                catch (System.Exception ex)
                {
                    sGroupName = "";
                }
                string sMemo = "";
                try
                {
                    sMemo = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sMemo").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sMemo").InnerText);//备注	sMemo		如:保底补偿;已达封顶线等
                }
                catch (System.Exception ex)
                {
                    sMemo = "";
                }
                string sPatientAssume = "";
                try
                {
                    sPatientAssume = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sPatientAssume").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sPatientAssume").InnerText);//病人个人承担金额	sPatientAssume
                }
                catch (System.Exception ex)
                {
                    sPatientAssume = "";
                }
                string sHospialAssume = "";
                try
                {
                    sHospialAssume = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sHospialAssume").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sHospialAssume").InnerText);//医院承担金额	sHospialAssume
                }
                catch (System.Exception ex)
                {
                    sHospialAssume = "";
                }
                string sBasicMedSum = "";
                try
                {
                    sBasicMedSum = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sBasicMedSum").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sBasicMedSum").InnerText);//基本药物金额	sBasicMedSum		
                }
                catch (System.Exception ex)
                {
                    sBasicMedSum = "";
                }
                string sBasicMedComp = "";
                try
                {
                    sBasicMedComp = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sBasicMedComp").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sBasicMedComp").InnerText);//基本药物增补金额	sBasicMedComp		
                }
                catch (System.Exception ex)
                {
                    sBasicMedComp = "";
                }
                string sDisFixMoney = "";
                try
                {
                    sDisFixMoney = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sDisFixMoney").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sDisFixMoney").InnerText);//单病种费用定额	sDisFixMoney		
                }
                catch (System.Exception ex)
                {
                    sDisFixMoney = "";
                }
                string sCivilComp = "";
                try
                {
                    sCivilComp = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sCivilComp").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sCivilComp").InnerText);//民政救助补偿金额	sCivilComp		
                }
                catch (System.Exception ex)
                {
                    sCivilComp = "";
                }
                string sCMeComp = "";
                try
                {
                    sCMeComp = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sCMeComp").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sCMeComp").InnerText);//中医药品/诊疗增补金额	sCMeComp		
                }
                catch (System.Exception ex)
                {
                    sCMeComp = "";
                }
                string sIfBottom = "";
                try
                {
                    sIfBottom = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sIfBottom").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sIfBottom").InnerText);//是否保底补偿	sIfBottom		0:否 1:是
                }
                catch (System.Exception ex)
                {
                    sIfBottom = "";
                }


                string sFDJE1 = ""; string sRDJE1 = ""; string sBXBL1 = ""; string sBXJE1 = "";
                string sFDJE2 = ""; string sRDJE2 = ""; string sBXBL2 = ""; string sBXJE2 = "";
                string sObligate3 = ""; //string sObligate4 = ""; 
                string NetWorkBillNo = "";

                //if (ReResulst.SelectSingleNode("/JQWebService/RowCount").InnerText == "3")
                //  {
                try
                {
                    sFDJE1 = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row2/sFDJE").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row2/sFDJE").InnerText);//分段金额	sFDJE		格式:如0~100
                }
                catch (System.Exception ex)
                {
                    sFDJE1 = "";
                }
                try
                {
                    sRDJE1 = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row2/sRDJE").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row2/sRDJE").InnerText);//入段金额	sRDJE		格式：0.00
                }
                catch (System.Exception ex)
                {
                    sRDJE1 = "";
                }
                try
                {
                    sBXBL1 = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row2/sBXBL").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row2/sBXBL").InnerText);//报销比例	sBXBL		格式：0.00
                }
                catch (System.Exception ex)
                {
                    sBXBL1 = "";
                }
                try
                {
                    sBXJE1 = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row2/sBXJE").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row2/sBXJE").InnerText);//报销金额	sBXJE		格式：0.00
                }
                catch (System.Exception ex)
                {
                    sBXJE1 = "";
                }
                try
                {
                    sFDJE2 = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row3/sFDJE").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row3/sFDJE").InnerText);//分段金额	sFDJE		格式:如0~100
                }
                catch (System.Exception ex)
                {
                    sFDJE2 = "";
                }
                try
                {
                    sRDJE2 = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row3/sRDJE").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row3/sRDJE").InnerText);//入段金额	sRDJE		格式：0.00
                }
                catch (System.Exception ex)
                {
                    sRDJE2 = "";
                }
                try
                {
                    sBXBL2 = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row3/sBXBL").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row3/sBXBL").InnerText);//报销比例	sBXBL		格式：0.00
                }
                catch (System.Exception ex)
                {
                    sBXBL2 = "";
                }
                try
                {
                    sBXJE2 = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row3/sBXJE").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row3/sBXJE").InnerText);//报销金额	sBXJE		格式：0.00
                }
                catch (System.Exception ex)
                {
                    sBXJE2 = "";
                }


                //  }

                string sDBBC = "0";
                sDBBC = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate1").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate1").InnerText);//大病补偿	sIfBottom

                string sObligate4 = "0";  //门诊定义
                string sObligate5 = "0";  //2017-04-14加 农合预留
                sObligate4 = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate4").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate4").InnerText);//财政兜底资金金额  门诊取值
                sObligate5 = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate5").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate5").InnerText);//农合预留  门诊取值
                string yibalin = "0";
                yibalin = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/fPoorSecondComp").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/fPoorSecondComp").InnerText);//180  门诊取值
                string strSql = " insert REPORT.dbo.NH_InpatientCalculate values('" + sAreaCode + "','" + outPayPara.RegInfo.NetRegSerial + "','" + sHospitalCode + "','" + netPatInfo.strRedeemNo + "','" + outPayPara.RegInfo.NetRegSerial + "','" + MZTotalAmount + "','" + operatorInfo.UserName
                + "','" + sCalculateCode + "','" + sAllInCost + "','" + sAllApply + "','" + sBegin + "','" + sFund + "','" + sAccount + "','" + sAccountBegin + "','" + sAccountAfter
                + "','" + sSumFund + "','" + sInHospialCount + "','" + sSelfCost + "','" + sOwnCost + "','" + sMedSum + "','" + sMedAppSum + "','" + sCMeSum + "','" + sCMeASum
                + "','" + sOutHCheck + "','" + sOutCheckApp + "','" + sOutCheck + "','" + sHostName + "','" + sYear + "','" + sCalculateDate + "','" + sTownName + "','" + sVillageName
                + "','" + sGroupName + "','" + sMemo + "','" + sPatientAssume + "','" + sHospialAssume + "','" + sBasicMedSum + "','" + sBasicMedComp + "','" + sDisFixMoney
                + "','" + sCivilComp + "','" + sCMeComp + "','" + sIfBottom + "','" + sFDJE1 + "','" + sRDJE1 + "','" + sBXBL1 + "','" + sBXJE1
                + "','" + sFDJE2 + "','" + sRDJE2 + "','" + sBXBL2 + "','" + sBXJE2 + "','" + yibalin + "','" + sDBBC + "','" + sObligate3 + "','" + sObligate4 + "','" + sObligate5 + "','" + NetWorkBillNo + "','0 ')";   //门诊插入数据

                LogManager.Info(strSql + " 保存结算数据到本地" + outPayPara.RegInfo.NetRegSerial + "|" + outPayPara.RegInfo.OutPatId);
                stra = strSql;
                try
                {
                    sqlHelper.ExecSqlReInt(strSql);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("保存结算信息失败");
                }
            }
            catch
            {
                LogManager.Info(stra + " 保存结算信息");
                // inReimPara.TreatNo = inReimPara.NetWorkRegNo;
                //  inReimPara.NetWorkBillNo = sCalculateCode;
                // CancelReimSettle(inReimPara.Memo1, inReimPara.NetWorkRegNo, sCalculateCode);
                // CancelOutReigster(inReimPara.Memo1, inReimPara.NetWorkRegNo);
            }

            #endregion
        }
        #endregion

        #region 撤销门诊结算
        /// <summary>
        /// 出院结算撤销
        /// </summary> 
        public void CancelOutReimSettle()
        {
            //门诊结算撤销 
            string inpatientid = outPayPara.SettleInfo.SettleBackNo;
            string outNetworkNo = outPayPara.SettleInfo.SettleNo;
            string curAreaCode = outPayPara.SettleInfo.NetworkPatType;
            int Re = serr.InpatientCalculateCancel(curAreaCode, inpatientid, outNetworkNo, sHospitalCode, "费用变更", operatorInfo.UserName, "", "", "", "", "", ref sMessage);
            if (Re == 0)
            {
                LogManager.Info(sMessage.ToString() + inpatientid + "||" + outNetworkNo + " InpatientCalculateCancel");
            }
            LogManager.Info(sMessage.ToString() + inpatientid + "||" + outNetworkNo + " InpatientCalculateCancel");
        }
        #endregion

        #region 住院登记
        /// <summaribaba>
        /// 入院登记
        /// </summary>
        ///  
        public void InReimRegister()
        {
            DataSet ds = new DataSet();
            string sMedicalCode = "";
            string sCardCode = "";
            string sInpatientID = "";
            // string sPeopCode = "";
            //string getnhinfo = "SELECT top 1 sMedicalCode,sIDCardNo FROM REPORT.dbo.NH_MESSAGE WHERE patinhosid = '" + inReimPara.PatInHosId + "' AND sPeopName = '" + inReimPara.InPatName + "';";

            //try
            //{
            //    ds = sqlHelper.ExecSqlReDs(getnhinfo);
            //    sMedicalCode = ds.Tables[0].Rows[0]["sMedicalCode"].ToString();
            //    sCardCode = ds.Tables[0].Rows[0]["sIDCardNo"].ToString();
            //   // sPeopCode = ds.Tables[0].Rows[0]["sPeopCode"].ToString();

            //}
            //catch (System.Exception ex)
            //{

            //}
            switch (sAreaCode)
            {
                case "341204":
                    netPatInfo.strTelNo = "2780186";
                    break;
            }
            string rc = sAreaCode + "|" + sHospitalCode + inPayPara.PatInfo.PatInHosCode + "|" +
                     netPatInfo.medicalNo + "|" + netPatInfo.medicalNo + "|" + netPatInfo.ICNo + "|" +
                     netPatInfo.patName + "|" + netPatInfo.sex + "|" + netPatInfo.age.ToString() + "|" +
                     netPatInfo.birthday.ToString("yyyy-MM-dd") + "|" + netPatInfo.IDNo + "|" +
                     netPatInfo.strInDiagnoCode + "|" + netPatInfo.strInDiagnoName + "|" +
                     "" + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" +
                     "" + "|" + "" + "|" + "" + "|" + "" + "|" + inPayPara.PatInfo.InDeptName + "|" +
                     inPayPara.PatInfo.InNetWorkDeptCode + "|" + netPatInfo.strCureId + "|" +
                     netPatInfo.strInHosId + "|" + inPayPara.PatInfo.InDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "|" +
                     inPayPara.PatInfo.InBedNo + "|" + "" + "|" + "00" + "|" + "" + "|" + "" + "|" + "" + "|" +
                     operatorInfo.UserName + "|" + netPatInfo.strTelNo + "|" + "" + "|" + "" + "|" +
                      "" + "|" + "";   //登记入参

            int registerResult = serr.InpatientRegister(
                    sAreaCode,// sAreaCode	地区代码
                    sHospitalCode, //sHospitalCode	医疗机构编号
                    inPayPara.PatInfo.PatInHosCode, //sInpatientCode	HIS住院号
                                                    //netPatInfo.strMemberNo,//sMedicalCode	医疗证号 歌谱写死
                                                    // sMedicalCode,
                                                    // sCardCode,
                    netPatInfo.medicalNo,
                    netPatInfo.medicalNo,//马磊改
                                         //sCardCode,
                                         //sMedicalCode,

                    // sPeopCode,
                    netPatInfo.ICNo,//sPeopCode	人员编号
                    netPatInfo.patName,//sPeopName	姓名
                    netPatInfo.sex,//sSex	性别k
                    netPatInfo.age.ToString(),//sAge	年龄
                    netPatInfo.birthday.ToString("yyyy-MM-dd"),//sBirthDay	出生日期
                    netPatInfo.IDNo,//sIDCardNo	身份证号码
                    netPatInfo.strInDiagnoCode,//sDiagnoseCodeIn1	入院疾病诊断编码1
                    netPatInfo.strInDiagnoName,//sDiagnoseNameIn1	入院疾病诊断名称1
                    "",//sDiagnoseCodeIn2	入院疾病诊断编码2
                    "",//sDiagnoseNameIn2	入院疾病诊断名称2
                    "",//sDiagnoseCodeIn3	入院疾病诊断编码3
                    "",//sDiagnoseNameIn3	入院疾病诊断名称3
                    "",//sOperationCode1	手术编码1
                    "",//sOperationName1	手术名称1
                    "",//sOperationCode2	手术编码2
                    "",//sOperationName2	手术名称2
                    "",//sOperationCode3	手术编码3
                    "",//sOperationName4	手术名称3
                    inPayPara.PatInfo.InDeptName,//sSectionOfficeName	HIS入院科室名称
                    inPayPara.PatInfo.InNetWorkDeptCode,//sSectionOfficeCode	中心入院科室编码
                                                        // inReimPara.InDeptCode, //马磊
                                                        // "04.03",
                    netPatInfo.strCureId,//sCureCode	就诊类型编码
                    netPatInfo.strInHosId,//sInHospitalCode	入院状态编码

                    inPayPara.PatInfo.InDateTime.ToString("yyyy-MM-dd HH:mm:ss"),//sInHosptialDate	入院时间
                    inPayPara.PatInfo.InBedNo,//sBed	床号
                    "",//sDoctorName	床位医生姓名
                    "00",//sChangeCode	转诊类型编号
                    "",//sChangeRCode	转诊单号
                    "",//sCivilCode	民政通知书号
                    "",//sBearCode	生育证号
                    operatorInfo.UserName,//sOperatorName	操作人姓名
                                          //"",//sObligate1	预留字段1
                    netPatInfo.strTelNo,

                    "",//sObligate2	预留字段2
                    "",//sObligate3	预留字段3
                    "",//sObligate4	预留字段4
                    "",//sObligate5	预留字段5
                    ref sInpatientID,//sInpatientID	就诊ID
                    ref sMessage//sMessage	错误/提示信息
           );

            if (registerResult == 0)
            {
                throw new Exception(sMessage);
            }
            //农合住院登记流水号（18位） 唯一标识141107000066904
            //MessageBox.Show(sInpatientID);
            inPayPara.RegInfo.NetRegSerial = sInpatientID;
            inPayPara.RegInfo.Memo2 = sAreaCode;
        }
        #endregion

        #region 住院登记修改
        /// <summary>
        /// 修改入院登记信息
        /// </summary>
        public void InModifyReimRegist()
        {
            int registerResult = serr.InpatientRegisterModify(
                sAreaCode,                                            //地区代码	In	sAreaCode
                inPayPara.RegInfo.NetRegSerial,                              //就诊ID	In	sInpatientID
                sHospitalCode,                                        //医疗机构编号	In	sHospitalCode
                inPayPara.PatInfo.PatInHosCode,                                 //HIS住院号	In	sInpatientCode
                                                                                //inReimPara.CantonCode,//医疗证号	In	sMedicalCode
                                                                                //inReimPara.CardNo,//医疗卡号	In	sCardCode
                netPatInfo.medicalNo,
                netPatInfo.medicalNo,
                //inReimPara.MemberNo,//人员编号	In	sPeopCode
                //netPatInfo.medicalNo,
                netPatInfo.ICNo,
                inPayPara.PatInfo.InPatName,                                 //姓名	In	sPeopName
                netPatInfo.sex,                                       //性别	In	sSex
                netPatInfo.age.ToString(),                            //年龄	In	sAgek
                netPatInfo.birthday.ToString("yyyy-MM-dd"),           //出生日期	In	sBirthDay
                netPatInfo.IDNo,                                      //身份证号码	In	sIDCardNo
                inPayPara.RegInfo.NetDiagnosCode,                            //入院疾病诊断编码1	In	sDiagnoseCodeIn1
                inPayPara.RegInfo.NetDiagnosName,                            //入院疾病诊断名称1	In	sDiagnoseNameIn1
                "",                                                   //入院疾病诊断编码2	In	sDiagnoseCodeIn2
                "",                                                   //入院疾病诊断名称2	In	sDiagnoseNameIn2
                "",                                                   //入院疾病诊断编码3	In	sDiagnoseCodeIn3
                "",                                                   //入院疾病诊断名称3	In	sDiagnoseNameIn3
                "",                                                   //手术编码1	In	sOperationCode1
                "",                                                   //手术名称1	In	sOperationName1
                "",                                                   //手术编码2	In	sOperationCode2
                "",                                                   //手术名称2	In	sOperationName2
                "",                                                   //手术编码3	In	sOperationCode3
                "",                                                   //手术名称3	In	sOperationName4
                inPayPara.PatInfo.InDeptName,                                //HIS入院科室名称	In  sSectionOfficeName

                inPayPara.PatInfo.InNetWorkDeptCode,                         //中心入院科室编码	In	sSectionOfficeCode
                netPatInfo.strCureId,                                 //就诊类型编码	In	sCureCode
                netPatInfo.strInHosId,                                //入院状态编码	In	sInHospitalCode
                inPayPara.PatInfo.InDateTime.ToString("yyyy-MM-dd hh:mm:ss"),//入院时间	In	sInHosptialDate
                inPayPara.PatInfo.InBedNo,                                   //床号	In	sBed
                inPayPara.PatInfo.DoctorName,                                //床位医生姓名	In	sDoctorName
                "00",                                                 //转诊类型编号	In	sChangeCode
                "",                                                   //转诊单号	In	sChangeRCode
                "",                                                   //民政通知书号	In	sCivilCode
                "",                                                   //生育证号	In	sBearCode
                operatorInfo.UserName,                                 //操作人姓名	In	sOperatorName
                "",                                                   //预留字段1	In	sObligate1
                "",                                                   //预留字段2	In	sObligate2
                "",                                                   //预留字段3	In	sObligate3
                "",                                                   //预留字段4	In	sObligate4
                "",                                                   //预留字段5	In	sObligate5
                ref sMessage                                          //错误/提示信息	Out	sMessage

           );

            if (registerResult == 0)
            {
            }
        }
        #endregion

        #region 撤销对端入院登记
        /// <summary>
        /// 撤销住院登记 
        /// </summary>
        /// <param name="CenterNo">中心登记流水号</param>
        public void CancelInRegister()
        {
            /*
             * 当前登录用户名 当前登录用户密码  医疗机构编码  
             * 农合住院登记流水号 住院登记取消原因(直接空)  预留参数一  预留参数2  预留参数3  预留参数4  预留参数5
             */

            //   名称	In/Out	字段
            //地区代码	In	sAreaCode
            //就诊ID	In	sInpatientID
            //医疗机构编号	In	sHospitalCode
            //原因	In	sReason
            //操作人姓名	In	sOperatorName
            //预留字段1	In	sObligate1
            //预留字段2	In	sObligate2
            //预留字段3	In	sObligate3
            //预留字段4	In	sObligate4
            //预留字段5	In	sObligate5
            //错误/提示信息	Out	sMessage


            //int q = serr.InpatientRegisterCancel(
            //   "341282", "141028000005524", sHospitalCode, "登记错误", this.strUserName, "", "", "", "", "", ref sMessage);


            int strRe = serr.InpatientRegisterCancel(
                sAreaCode, inPayPara.RegInfo.NetRegSerial, sHospitalCode, "登记错误", this.strUserName, "", "", "", "", "", ref sMessage);

            if (strRe == 0)
            {
                throw new Exception(sMessage);
            }
        }
        #endregion

        #region 取消上传费用
        /// <summary>
        /// 住院所有明细取消
        /// </summary>
        /// <param name="CenterNo"></param>
        public void CancelItems()
        {
            /*
             * 住院就诊号(数字格式18位)  农合中心编码(12字节)
             */
            //this.handleModel.ExeMehodReObj("inpCancelFee", new object[] {EncryptionVsDecryption.NetBase64Code(inReimPara.NetWorkRegNo),strCenterNo });

            int Re = serr.InpatientFeeCancelAll(sAreaCode, inPayPara.RegInfo.NetRegSerial, sHospitalCode, "", "", "", "", "", ref sMessage);

            if (Re == 0)
            {
                throw new Exception(sMessage);
            }


        }
        #endregion

        #region 取消上传费用 带参数
        /// <summary>
        /// 住院所有明细取消
        /// </summary>
        /// <param name="CenterNo"></param>
        public void CancelItems(string sAreaCode, string sInpatientId)
        {
            /*
             * 住院就诊号(数字格式18位)  农合中心编码(12字节)
             */
            //this.handleModel.ExeMehodReObj("inpCancelFee", new object[] {EncryptionVsDecryption.NetBase64Code(inReimPara.NetWorkRegNo),strCenterNo });
            int Re = serr.InpatientFeeCancelAll(sAreaCode, sInpatientId, sHospitalCode, "", "", "", "", "", ref sMessage);

            if (Re == 0)
            {
                throw new Exception(sMessage);
            }

        }
        #endregion

        #region 取消HIS费用上传标志
        public void CancelHisItems(string patInHosId)
        {
            string sql = " UPDATE ZY.[IN].IN_BILL_RECORD SET FLAG_NETWORK_UPLOAD=0 WHERE PAT_IN_HOS_ID='" + patInHosId + "'";
            sqlHelper.ExecSqlReInt(sql);
        }
        #endregion

        #region 住院费用逐条上传
        /// <summary>
        /// 上传费用明细  单条上传
        /// </summary>
        public void InReimUpItems()
        {
            //DataTable detailTable = inBll.GetInReimItemsFromSqliteBySql(inReimPara, strSql.ToString());
            var details = PayAPIUtilities.Tools.CommonTools.GetGroupList(inPayPara.Details);

            #region 如果费用明细里有未和农合对应的情况，则抛出异常终止操作
            string notMatchedCharge = "";
            foreach (var item in details)
            {
                if (item.NetworkItemCode.ToString().Trim().Length == 0)
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
            }
            #endregion

            for (int i = 0; i < details.Count; i++)
            {
                //地区代码	In	sAreaCode	
                //就诊ID	In	sInpatientID	
                //医疗机构编号	In	sHospitalCode	
                //中心项目编码	In	sCenterItemCode	
                //HIS记帐关键字	In	sItemKey	
                //费用类型	In	sItemType	
                //HIS发票项目名称	In	sReceiptName	
                //HIS药品/项目编码	In	sItemCode	
                //HIS药品/项目名称	In	sItemName	
                //HIS药品/项目规格	In	sItemSpec	
                //HIS药品/项目剂型	In	sItemDose	是
                //HIS药品/项目产地	In	sItemArea	是
                //HIS药品/项目加工过程	In	sItemProc	是
                //HIS药品/项目入药部位	In	sItemPart	是
                //是否复方	In	sIfCompound	
                //数量	In	sTime	
                //HIS项目单位	In	sUnit	
                //单价	In	sPrice	
                //总金额	In	sSum	
                //HIS科室名称	In	sSectionOfficeName	
                //中心科室编码	In	sSectionOfficeCode	
                //医生名称	In	sDoctorName	
                //床号	In	sBed	是
                //记帐时间	In	sOperatorDate	
                //记帐人姓名	In	sInputName	
                //预留字段1	In	sObligate1	是
                //预留字段2	In	sObligate2	是
                //预留字段3	In	sObligate3	是
                //预留字段4	In	sObligate4	是
                //预留字段5	In	sObligate5	是
                //结果：规范格式	Out	sResult	
                //错误/提示信息	Out	sMessage	
                var detail = details[i];

                int uploadDetailId = serr.InpatientFeeUpLoad(sAreaCode,
                    inPayPara.RegInfo.NetRegSerial,
                    sHospitalCode,
                    detail.NetworkItemCode,
                    detail.AutoId.ToString(),
                    detail.NetworkItemClass,
                    detail.ChargeName,
                    detail.ChargeCode,
                    detail.ChargeName,
                    detail.Spec,
                    detail.DrugFormName,
                    "",
                    "",
                    "", "0",
                    detail.Quantity.ToString(),
                    string.IsNullOrEmpty(detail.Unit) == true ? "|" : detail.Unit,
                    detail.Price.ToString(),
                    detail.Amount.ToString(),
                    detail.DeptCode,
                    inPayPara.PatInfo.InNetWorkDeptCode,
                    detail.DocCode,
                    "",
                    detail.CreateTime.ToString("yyyy-MM-dd hh:mm:ss"),
                    detail.DocCode.ToString(),
                    "", "", "", "", "", ref sResult, ref sMessage
                    );

                if (uploadDetailId == 0)
                {
                    MessageBox.Show(detail.ChargeName.ToString());
                    throw new Exception(sMessage);
                }
                else
                {
                    string sql = "UPDATE ZY.[IN].IN_BILL_RECORD SET FLAG_NETWORK_UPLOAD=1 WHERE IN_BILL_ID='" + detail.AutoId + "'";
                    sqlHelper.ExecSqlReInt(sql);
                }

            }

        }
        #endregion

        #region 住院费用批量上传
        /// <summary>
        /// 批量上传
        /// </summary>
        public void InReimUpItems_batch()
        {
            //DataTable detailTable = inBll.GetInReimItemsFromSqliteBySql(inReimPara, strSql.ToString());
            var details = PayAPIUtilities.Tools.CommonTools.GetGroupList(inPayPara.Details);

            #region 如果费用明细里有未和农合对应的情况，则抛出异常终止操作
            string notMatchedCharge = "";
            foreach (var item in details)
            {
                if (item.NetworkItemCode.ToString().Trim().Length == 0)
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
            }
            #endregion

            // 上传明细时的基本信息

            int EveryCount = 1000, n = 0;

            StringBuilder sInput = new StringBuilder();

            sInput.Append("<?xml version=\"1.0\" encoding=\"gb2312\" ?><JQWebService>");

            //int scts = 0;
            //if (m != detailTable.Rows.Count / 1000)
            //{
            //    scts = 1000;
            //}
            //else
            //{
            //    scts = detailTable.Rows.Count - m * 1000;
            //}

            sInput.Append("<RowCount>" + details.Count + "</RowCount>");

            StringBuilder fyid = new StringBuilder();

            foreach (var dr in details)
            {
                fyid.Append(dr.AutoId + ",");
                sInput.Append("<Row" + n + ">");
                if (dr.Spec.ToString() == "") { dr.Spec = "无"; }
                if (dr.DrugFormName.ToString() == "") { dr.DrugFormName = "无"; }
                if (dr.DocCode.ToString() == "") { dr.DocCode = "系统生成"; }
                //if (dr["DRUG_FORM_NAME"].ToString() == "") { dr["DRUG_FORM_NAME"] = "无"; }

                //    名称	In/Out	字段	是否可为空	说明
                sInput.Append("<sInpatientID>" + inPayPara.RegInfo.NetRegSerial + "</sInpatientID>");//就诊ID	In	sInpatientID		通过InpatientRegister取得的ID
                sInput.Append("<sCenterItemCode>" + dr.NetworkItemCode + "</sCenterItemCode>"); //中心项目编码	In	sCenterItemCode		
                sInput.Append("<sItemKey>" + dr.AutoId + "</sItemKey>");//HIS记帐关键字	In	sItemKey		
                sInput.Append("<sItemType>" + dr.NetworkItemClass + "</sItemType>");//费用类型	In	sItemType		0:西药;1:成药;2:草药:6:特殊检查;9:诊疗项目
                sInput.Append("<sReceiptName>" + dr.ChargeName + "</sReceiptName>");//HIS发票项目名称	In	sReceiptName		
                sInput.Append("<sItemCode>" + dr.ChargeCode + "</sItemCode>");//HIS药品/项目编码	In	sItemCode		
                sInput.Append("<sItemName>" + dr.ChargeName + "</sItemName>");//HIS药品/项目名称	In	sItemName		
                sInput.Append("<sItemSpec>" + dr.Spec + "</sItemSpec>");//HIS药品/项目规格	In	sItemSpec		没有内容请传汉字”无”
                sInput.Append("<sItemDose>" + dr.DrugFormName + "</sItemDose>");//HIS药品/项目剂型	In	sItemDose
                sInput.Append("<sItemArea>" + "" + "</sItemArea>");//HIS药品/项目产地	In	sItemArea	是	没有内容请传汉字”无”
                sInput.Append("<sItemProc>" + "" + "</sItemProc>");//HIS药品/项目加工过程	In	sItemProc	是	没有内容请传汉字”无”
                sInput.Append("<sItemPart>" + "" + "</sItemPart>");//HIS药品/项目入药部位	In	sItemPart	是	没有内容请传汉字”无”
                sInput.Append("<sIfCompound>" + "0" + "</sIfCompound>");//是否复方	In	sIfCompound		0：单方1：复方2：非草药
                sInput.Append("<sTime>" + dr.Quantity + "</sTime>");//数量	In	sTime		整数
                sInput.Append("<sUnit>" + dr.Unit + "</sUnit>");//HIS项目单位	In	sUnit		
                sInput.Append("<sPrice>" + dr.Price + "</sPrice>");//单价	In	sPrice		格式0.000
                sInput.Append("<sSum>" + dr.Amount + "</sSum>");//总金额	In	sSum		格式0.00
                sInput.Append("<sSectionOfficeName>" + dr.DeptCode + "</sSectionOfficeName>");//HIS科室名称	In	sSectionOfficeName		
                sInput.Append("<sSectionOfficeCode>" + inPayPara.PatInfo.InNetWorkDeptCode + "</sSectionOfficeCode>");//中心科室编码	In	sSectionOfficeCode		
                sInput.Append("<sDoctorName>" + dr.DocCode + "</sDoctorName>");//医生名称	In	sDoctorName		
                sInput.Append("<sBed>" + "" + "</sBed>");//床号	In	sBed	是	
                sInput.Append("<sOperatorDate>" + Convert.ToDateTime(dr.CreateTime).ToString("yyyy-MM-dd hh:mm:ss") + "</sOperatorDate>");//记帐时间	In	sOperatorDate		格式：YYYY-MM-DD HH:MM:SS
                sInput.Append("<sInputName>" + dr.DocCode + "</sInputName>");//记帐人姓名	In	sInputName		
                sInput.Append("<sObligate1>" + "" + "</sObligate1>");//预留字段1	In	sObligate1	是	
                sInput.Append("<sObligate2>" + "" + "</sObligate2>");//预留字段2	In	sObligate2	是	
                sInput.Append("<sObligate3>" + "" + "</sObligate3>");//预留字段3	In	sObligate3	是	
                sInput.Append("<sObligate4>" + "" + "</sObligate4>");//预留字段4	In	sObligate4	是	
                sInput.Append("<sObligate5>" + "" + "</sObligate5>");//预留字段5	In	sObligate5	是	
                sInput.Append("</Row" + n + ">");


            }

            sInput.Append("</JQWebService>");

            //MessageBox.Show("kaishi");

            int strRe = serr.InpatientFeeUpLoadAll(sAreaCode, inPayPara.RegInfo.NetRegSerial, sHospitalCode, sInput.ToString(), "", "", "", "", "", ref sResult, ref sMessage);

            if (strRe == 0)
            {
                throw new Exception(sMessage);
            }
            else
            {
                try
                {
                    string sql = "UPDATE ZY.[IN].IN_BILL_RECORD SET FLAG_NETWORK_UPLOAD=1 WHERE PAT_IN_HOS_ID='" + inPayPara.PatInfo.PatInHosId + "' AND IN_BILL_ID IN(" + fyid.Append("0") + ")";
                    sqlHelper.ExecSqlReInt(sql);
                }
                catch { MessageBox.Show("更新本地上传信息失败"); }
            }

        }

        #endregion

        #region 对端出院预结算
        /// <summary>
        /// 住院费用 预/正式 结算
        /// </summary>
        /// <param name="isPre">是否预结算</param>
        public void InReimPreSettle()
        {
            //结算类型  38CBAED1C4EC7247860A12CA928730E0   141021000023311  341222

            string sql = "SELECT TOTAL_COSTS FROM ZY.[IN].PAT_ALL_INFO_VIEW WHERE PAT_IN_HOS_ID='" + inPayPara.PatInfo.PatInHosId + "'";
            totalAmount = sqlHelper.ExecSqlReDs(sql).Tables[0].Rows[0][0].ToString();
            MessageBox.Show("预结算");

            int Re = serr.InpatientTryCalculate(sAreaCode, inPayPara.RegInfo.NetRegSerial, sHospitalCode, netPatInfo.strRedeemNo, totalAmount, "", "", "", "", "", ref sResult, ref sMessage);

            if (Re == 0)
            {
                throw new Exception(sMessage);
            }

            XmlDocument ReResulst = new XmlDocument();
            string result = sResult;                                                           //进行医保业务
            ReResulst.LoadXml(result);


            if (ReResulst.SelectSingleNode("/JQWebService/RowCount").InnerText != "0")
            {

                InfoPreSettle info = new InfoPreSettle();

                info.Amount = ReResulst.SelectSingleNode("/JQWebService/Row1/sAllInCost").InnerText;//本次住院总医疗费用
                info.KBXFY = ReResulst.SelectSingleNode("/JQWebService/Row1/sAllApply").InnerText;//可报销费用
                info.QFX = ReResulst.SelectSingleNode("/JQWebService/Row1/sBegin").InnerText;//起付线
                info.JJZFJE = ReResulst.SelectSingleNode("/JQWebService/Row1/sFund").InnerText;//基金支付金额
                info.ZHZF = ReResulst.SelectSingleNode("/JQWebService/Row1/sAccount").InnerText;//帐户支付
                info.ZHYE = ReResulst.SelectSingleNode("/JQWebService/Row1/sAccountBegin").InnerText;//帐户余额
                info.ZFJE = ReResulst.SelectSingleNode("/JQWebService/Row1/sSelfCost").InnerText;//自费金额
                info.ZJFJE = ReResulst.SelectSingleNode("/JQWebService/Row1/sOwnCost").InnerText;//自己付金额
                //info.YQJCZE = ReResulst.SelectSingleNode("/JQWebService/Row1/sOwnCost").InnerText;//院前检查总额
                //info.YQJCBXJE = ReResulst.SelectSingleNode("/JQWebService/Row1/sOwnCost").InnerText;//院前检查总额


                info.ShowDialog();
            }
        }
        #endregion

        #region 对端出院结算
        /// <summary>
        /// 住院兑付
        /// </summary> 
        public void InReimSettle()
        {
            int Re = serr.InpatientCalculate(sAreaCode, inPayPara.RegInfo.NetRegSerial, sHospitalCode, netPatInfo.strRedeemNo, inPayPara.RegInfo.NetRegSerial, totalAmount, operatorInfo.UserName, "", "", "", "", "", ref sResult, ref sMessage);

            if (Re == 0)
            {
                string xxx = sMessage;
                CancelOutReigster(sAreaCode, inPayPara.RegInfo.NetRegSerial);
                throw new Exception(xxx);
            }

            XmlDocument ReResulst = new XmlDocument();
            string result = sResult;                                                           //进行医保业务
            ReResulst.LoadXml(result);

            //inReimPara.MedAmount = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sFund").InnerText);
            //inReimPara.NetWorkBillNo = ReResulst.SelectSingleNode("/JQWebService/Row1/sCalculateCode").InnerText;
            //inReimPara.Memo1 = sAreaCode;

            InfoPreSettle info = new InfoPreSettle(inPayPara);
            info.Amount = ReResulst.SelectSingleNode("/JQWebService/Row1/sAllInCost").InnerText;//本次住院总医疗费用
            info.KBXFY = ReResulst.SelectSingleNode("/JQWebService/Row1/sAllApply").InnerText;//可报销费用
            info.QFX = ReResulst.SelectSingleNode("/JQWebService/Row1/sBegin").InnerText;//起付线
            info.JJZFJE = ReResulst.SelectSingleNode("/JQWebService/Row1/sFund").InnerText;//基金支付金额
            info.ZHZF = ReResulst.SelectSingleNode("/JQWebService/Row1/sAccount").InnerText;//帐户支付
            info.ZHYE = ReResulst.SelectSingleNode("/JQWebService/Row1/sAccountBegin").InnerText;//帐户余额
            info.ZFJE = ReResulst.SelectSingleNode("/JQWebService/Row1/sSelfCost").InnerText;//自费金额
            info.ZJFJE = ReResulst.SelectSingleNode("/JQWebService/Row1/sOwnCost").InnerText;//自己付金额
            info.MZJZ = ReResulst.SelectSingleNode("/JQWebService/Row1/sCivilComp").InnerText;//民政救助
            info.DBBC = ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate1").InnerText;//大病补偿
            info.czdd = ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate4").InnerText;//财政兜底资金金额
            info.yibaling = ReResulst.SelectSingleNode("/JQWebService/Row1/fPoorSecondComp").InnerText;//180
            info.qitabc = ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate5").InnerText;//其他补偿
            if (sAreaCode == "341226")
            {

                info.sPatientAssume = ReResulst.SelectSingleNode("/JQWebService/Row1/sPatientAssume").InnerText;//病人个人承担金额
                info.sHospialAssume = ReResulst.SelectSingleNode("/JQWebService/Row1/sHospialAssume").InnerText;//医院承担金额
            }
            else
            {
                info.YQJCZE = ReResulst.SelectSingleNode("/JQWebService/Row1/sOutHCheck").InnerText;//院前检查总额
                info.YQJCBXJE = ReResulst.SelectSingleNode("/JQWebService/Row1/sOutCheck").InnerText;//院前检查报销总额

                info.sPatientAssume = ReResulst.SelectSingleNode("/JQWebService/Row1/sPatientAssume").InnerText;//病人个人承担金额
                info.sHospialAssume = ReResulst.SelectSingleNode("/JQWebService/Row1/sHospialAssume").InnerText;//医院承担金额
            }

            info.ShowDialog();

            if (info.restr == "2")
            {
                throw new Exception("撤销农合结算成功！");
            }

            if (info.restr == "1")
            {
                throw new Exception("撤销农合结算成功,撤销农合出院登记成功！");
            }

            InNetworkSettleMain inSettleMain = new InNetworkSettleMain();
            decimal mzjz = 0;
            decimal dbbc = 0;
            decimal CZDD = 0;
            decimal yibaba = 0;
            decimal qtbc = 0; // 其他补偿
            #region 解析正式收费数据
            inSettleMain.IsSettle = true;                               //结算标志，还有其他要修改的属性
            inSettleMain.PatInHosId = inPayPara.PatInfo.PatInHosId;
            inSettleMain.SettleNo = ReResulst.SelectSingleNode("/JQWebService/Row1/sCalculateCode").InnerText; ; //医保中心交易流水号 单据号
            inSettleMain.Amount = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sAllInCost").InnerText);      //医疗总费用                     

            inSettleMain.MedAmountZhzf = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sAccount").InnerText); //账户支付

            inSettleMain.MedAmountTc = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sFund").InnerText);    //统筹支付

            inSettleMain.MedAmountBz = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sAllApply").InnerText);  //可报销总费用

            inSettleMain.MedAmountJm = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sBegin").InnerText); ; //起伏线金额

            inSettleMain.MedAmountGwy = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sHospialAssume").InnerText);//医院承担金额

            inSettleMain.GetAmount = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sPatientAssume").InnerText);//病人承担金额

            mzjz = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sCivilComp").InnerText);//民政救助金额
            yibaba = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/fPoorSecondComp").InnerText);//180
            qtbc = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate5").InnerText);//其他补偿

            //if ((sAreaCode == "341226"))
            //{
            //    dbbc = 0;
            //}

            //else

            //{
            dbbc = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate1").InnerText);//大病补偿金额
            //}
            CZDD = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate4").InnerText);//大病补偿金额
            inSettleMain.NetworkPatType = sAreaCode;

            inSettleMain.MedAmountDb = Convert.ToDecimal(ReResulst.SelectSingleNode("/JQWebService/Row1/sFund").InnerText);//    区县   帐户支付后余额	sAccountAfter		格式：0.00

            inSettleMain.MedAmountTotal = inSettleMain.MedAmountTc + inSettleMain.MedAmountGwy + mzjz + dbbc + CZDD + yibaba + qtbc;

            inSettleMain.MedAmountTc = inSettleMain.MedAmountTotal;

            inSettleMain.GetAmount = inSettleMain.Amount - inSettleMain.MedAmountTotal;

            inSettleMain.CreateTime = DateTime.Now;

            inSettleMain.NetworkPatName = netPatInfo.patName;//户主姓名

            inSettleMain.NetworkRefundTime = Convert.ToDateTime("2000-01-01");
            inSettleMain.NetworkSettleTime = DateTime.Now;
            inSettleMain.SettleBackNo = inPayPara.RegInfo.NetRegSerial;        //住院登记流水号 中心住院号 唯一标识
            inSettleMain.SettleType = inPayPara.RegInfo.NetPatType;                     //补偿类型名称 

            inPayPara.SettleInfo = inSettleMain;

          
            #endregion

            string sCalculateCode = "";

            #region  //将结算数据全部保存到服务器
            try
            {

                //sAreaCode, inReimPara.NetWorkRegNo, sHospitalCode, netPatInfo.strRedeemNo, inReimPara.NetWorkRegNo, inReimPara.MedAmount.ToString(), operatorInfo.UserName,
                string sAreaCode1 = sAreaCode;//            地区代码	In	sAreaCode	
                string sInpatientID = inPayPara.RegInfo.NetRegSerial;//就诊ID	In	sInpatientID
                string NetWorkBillNo = inSettleMain.SettleNo;//结算流水号
                //医疗机构编号	In	sHospitalCode	
                //补偿类别编码	In	sCalcCode	
                //发票号码	In	sReceiptCode	
                //HIS住院发生总费用	In	sAllInCost	
                //操作人姓名	In	sOperatorName	
                string sOutCheckApp = "";
                string sOutCheck = "";
                string sDBBC = "0";
                string sObligate4 = "0";   //住院定义
                string sObligate5 = "0";   //2017-04-14加
                string bababa = "0"; // 180 2017-04-26 日加
                sCalculateCode = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sCalculateCode").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sCalculateCode").InnerText);//结算单号	sCalculateCode		需要保存
                string sAllInCost = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sAllInCost").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sAllInCost").InnerText);//医疗总费用	sAllInCost		格式：0.00
                string sAllApply = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sAllApply").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sAllApply").InnerText);//可报销总费用	sAllApply		格式：0.00
                string sBegin = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sBegin").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sBegin").InnerText);//起付线	sBegin		格式：0.00
                string sFund = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sFund").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sFund").InnerText);//基金支付	sFund		格式：0.00:包含中医诊疗增补+院外检查+基本药物增补+民政补偿
                string sAccount = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sAccount").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sAccount").InnerText);//帐户支付	sAccount		格式：0.00
                string sAccountBegin = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sAccountBegin").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sAccountBegin").InnerText);//帐户支付前余额	sAccountBegin		格式：0.00
                string sAccountAfter = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sAccountAfter").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sAccountAfter").InnerText);//帐户支付后余额	sAccountAfter		格式：0.00
                string sSumFund = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sSumFund").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sSumFund").InnerText);//年度基金累计支付	sSumFund		格式：0.00包含本次支付
                string sInHospialCount = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sInHospialCount").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sInHospialCount").InnerText);//住院次数	sInHospialCount		格式：0.00包含本次住院
                string sSelfCost = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sSelfCost").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sSelfCost").InnerText);//自费金额	sSelfCost		格式：0.00
                string sOwnCost = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sOwnCost").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sOwnCost").InnerText);//自付金额	sOwnCost		格式：0.00
                string sMedSum = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sMedSum").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sMedSum").InnerText);//药品总金额	sMedSum		格式：0.00
                string sMedAppSum = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sMedAppSum").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sMedAppSum").InnerText);//药品可报销总金额	sMedAppSum		格式：0.00
                string sCMeSum = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sCMeSum").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sCMeSum").InnerText);//中医药品/诊疗总金额	sCMeSum		格式：0.00
                string sCMeASum = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sCMeASum").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sCMeASum").InnerText);//中医药品/诊疗可报销总金额	sCMeASum		格式：0.00

                string sOutHCheck = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sOutHCheck").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sOutHCheck").InnerText);//院外检查总金额	sOutHCheck		格式：0.00
                if (sAreaCode == "341226")
                {
                    //sOutCheckApp = "0";
                    //sOutCheck = "0";
                    //sDBBC = "0";
                    sOutCheck = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sOutChecek").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sOutChecek").InnerText);//院外检查报销总金额	sOutCheck		


                }
                else
                {
                    sOutCheck = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sOutCheck").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sOutCheck").InnerText);//院外检查报销总金额	sOutCheck		
                }
                sOutCheckApp = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sOutCheckApp").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sOutCheckApp").InnerText);//院外检查可报销总金额	sOutCheckApp		格式：0.00
                sDBBC = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate1").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate1").InnerText);//大病补偿	sIfBottom

                string sHostName = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sHostName").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sHostName").InnerText);//户主姓名	sHostName		
                string sYear = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sYear").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sYear").InnerText);//业务年份	sYear		年份(整数),如2009
                string sCalculateDate = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sCalculateDate").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sCalculateDate").InnerText);//结算时间	sCalculateDate		格式:YYYY-MM-DD HH:MM:SS
                string sTownName = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sTownName").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sTownName").InnerText);//乡镇名称	sTownName		
                string sVillageName = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sVillageName").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sVillageName").InnerText);//村名称	sVillageName		
                string sGroupName = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sGroupName").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sGroupName").InnerText);//组名称	sGroupName		
                string sMemo = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sMemo").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sMemo").InnerText);//备注	sMemo		如:保底补偿;已达封顶线等
                string sPatientAssume = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sPatientAssume").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sPatientAssume").InnerText);//病人个人承担金额	sPatientAssume
                string sHospialAssume = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sHospialAssume").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sHospialAssume").InnerText);//医院承担金额	sHospialAssume
                string sBasicMedSum = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sBasicMedSum").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sBasicMedSum").InnerText);//基本药物金额	sBasicMedSum		
                string sBasicMedComp = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sBasicMedComp").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sBasicMedComp").InnerText);//基本药物增补金额	sBasicMedComp		
                string sDisFixMoney = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sDisFixMoney").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sDisFixMoney").InnerText);//单病种费用定额	sDisFixMoney		
                string sCivilComp = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sCivilComp").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sCivilComp").InnerText);//民政救助补偿金额	sCivilComp		
                string sCMeComp = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sCMeComp").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sCMeComp").InnerText);//中医药品/诊疗增补金额	sCMeComp		
                string sIfBottom = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sIfBottom").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sIfBottom").InnerText);//是否保底补偿	sIfBottom		0:否 1:是
                string sObligate3 = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate3").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate3").InnerText);//是否贫困人口	sObligate3		0：否 1：是

                sObligate4 = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate4").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate4").InnerText);//财政兜底资金金额	sObligate4  住院取值
                sObligate5 = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate5").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/sObligate5").InnerText);//农合预留  住院取值
                bababa = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row1/fPoorSecondComp").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row1/fPoorSecondComp").InnerText);//180  住院取值



                string sFDJE1 = ""; string sRDJE1 = ""; string sBXBL1 = ""; string sBXJE1 = "";
                string sFDJE2 = ""; string sRDJE2 = ""; string sBXBL2 = ""; string sBXJE2 = "";


                if (ReResulst.SelectSingleNode("/JQWebService/RowCount").InnerText == "3")
                {

                    sFDJE1 = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row2/sFDJE").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row2/sFDJE").InnerText);//分段金额	sFDJE		格式:如0~100
                    sRDJE1 = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row2/sRDJE").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row2/sRDJE").InnerText);//入段金额	sRDJE		格式：0.00
                    sBXBL1 = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row2/sBXBL").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row2/sBXBL").InnerText);//报销比例	sBXBL		格式：0.00
                    sBXJE1 = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row2/sBXJE").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row2/sBXJE").InnerText);//报销金额	sBXJE		格式：0.00
                    sFDJE2 = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row3/sFDJE").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row3/sFDJE").InnerText);//分段金额	sFDJE		格式:如0~100
                    sRDJE2 = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row3/sRDJE").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row3/sRDJE").InnerText);//入段金额	sRDJE		格式：0.00
                    sBXBL2 = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row3/sBXBL").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row3/sBXBL").InnerText);//报销比例	sBXBL		格式：0.00
                    sBXJE2 = string.IsNullOrEmpty(ReResulst.SelectSingleNode("/JQWebService/Row3/sBXJE").InnerText) == true ? "" : (ReResulst.SelectSingleNode("/JQWebService/Row3/sBXJE").InnerText);//报销金额	sBXJE		格式：0.00

                }


                // string strringsql = " UPDATE  REPORT.dbo.NH_InpatientCalculate   SET FLAG_INVALID=1 WHERE sInpatientID='" + inReimPara.NetWorkRegNo + "' ";
                string stringsql = " UPDATE  REPORT.dbo.NH_InpatientCalculate   SET FLAG_INVALID=1 WHERE sInpatientID='" + inPayPara.RegInfo.NetRegSerial + "' ";

                sqlHelper.ExecSqlReInt(stringsql);

                //住院插入数据
                string strSql = "insert REPORT.dbo.NH_InpatientCalculate values('" + sAreaCode + "','" + inPayPara.RegInfo.NetRegSerial + "','" + sHospitalCode + "','" + netPatInfo.strRedeemNo + "','" + inPayPara.RegInfo.NetRegSerial + "','" + inSettleMain.Amount.ToString() + "','" + operatorInfo.UserName
                + "','" + sCalculateCode + "','" + sAllInCost + "','" + sAllApply + "','" + sBegin + "','" + sFund + "','" + sAccount + "','" + sAccountBegin + "','" + sAccountAfter
                + "','" + sSumFund + "','" + sInHospialCount + "','" + sSelfCost + "','" + sOwnCost + "','" + sMedSum + "','" + sMedAppSum + "','" + sCMeSum + "','" + sCMeASum
                + "','" + sOutHCheck + "','" + sOutCheckApp + "','" + sOutCheck + "','" + sHostName + "','" + sYear + "','" + sCalculateDate + "','" + sTownName + "','" + sVillageName
                + "','" + sGroupName + "','" + sMemo + "','" + sPatientAssume + "','" + sHospialAssume + "','" + sBasicMedSum + "','" + sBasicMedComp + "','" + sDisFixMoney
                + "','" + sCivilComp + "','" + sCMeComp + "','" + sIfBottom + "','" + sFDJE1 + "','" + sRDJE1 + "','" + sBXBL1 + "','" + sBXJE1
                + "','" + sFDJE2 + "','" + sRDJE2 + "','" + sBXBL2 + "','" + sBXJE2 + "','" + bababa + "','" + sDBBC + "','" + sObligate3 + "','" + sObligate4 + "','" + sObligate5 + "','" + NetWorkBillNo + "','0')";

                LogManager.Info(strSql + "保存报补单数据" + inPayPara.RegInfo.NetRegSerial + "|" + inPayPara.PatInfo.PatInHosId.ToString());
                sqlHelper.ExecSqlReInt(strSql);


                PayAPIInterface.Model.Comm.PayType payType = new PayAPIInterface.Model.Comm.PayType();
                payType.PayTypeId = 6;
                payType.PayTypeName = "农合";
                payType.PayAmount = inSettleMain.MedAmountTotal;
                inPayPara.PayTypeList = new List<PayType>();
                inPayPara.PayTypeList.Add(payType);
            }
            catch
            {
                MessageBox.Show("保存结算信息失败");
                CancelReimSettle(inPayPara.RegInfo.Memo2, inPayPara.RegInfo.NetRegSerial, sCalculateCode);
                CancelOutReigster(inPayPara.RegInfo.Memo2, inPayPara.RegInfo.NetRegSerial);
                return;
            }

            #endregion


            //保存结算数据
            IInClientBLL inBLl = PayAPIClassLib.Factory.ClientBLLFactory.GetInClientBLLInstance();
            //获取联网结算ID 并重新组织数据
            inBLl.GetInSettleIdAndReorganizeData(inPayPara);
            inBLl.SaveInNetworkSettleMain(inSettleMain);
            inBLl.UpdateInNetworkRegister(inPayPara.RegInfo);
        }
        #endregion

        #region 对端出院登记
        /// <summary>
        /// 出院登记
        /// </summary>
        private void OutReimRegister()
        {
            string sql = "SELECT TOTAL_COSTS FROM ZY.[IN].PAT_ALL_INFO_VIEW WHERE PAT_IN_HOS_ID='" + inPayPara.PatInfo.PatInHosId + "'";
            totalAmount = sqlHelper.ExecSqlReDs(sql).Tables[0].Rows[0][0].ToString();

            //inReimPara.InNetWorkDeptCode
            // MessageBox.Show("000");
            int mmmm = serr.InpatientOutRegister(sAreaCode,
                inPayPara.RegInfo.NetRegSerial,
                sHospitalCode,
                inPayPara.RegInfo.OutDiagnoseCode,
                inPayPara.RegInfo.OutDiagnoseName,
                "", "", "", "",
                inPayPara.PatInfo.InDeptName, "04.03", "1",
                inPayPara.PatInfo.OutDateTime.ToString("yyyy-MM-dd hh:mm:ss"),
                operatorInfo.UserName,
                inPayPara.RegInfo.NetRegSerial,
                totalAmount, "", "", "", "", "", ref sMessage);

            //MessageBox.Show(sMessage);
            if (mmmm == 0)
            {
                string xx = sMessage;
                //MessageBox.Show(sMessage);
                CancelItems();
                CancelHisItems(inPayPara.PatInfo.PatInHosId.ToString());
                throw new Exception(xx);
            }
        }
        #endregion

        #region 撤销对端出院登记信息
        /// <summary>
        /// 出院登记撤销
        /// </summary>
        private void CancelOutReigster()
        {
            //int mmmm = serr.InpatientOutRegisterCancel("341222", "141023000020247", sHospitalCode, "费用信息不符", operatorInfo.UserName, "", "", "", "", "", ref sMessage);
            int mmmm = serr.InpatientOutRegisterCancel(sAreaCode, inPayPara.RegInfo.NetRegSerial, sHospitalCode, "费用信息不符", operatorInfo.UserName, "", "", "", "", "", ref sMessage);

            if (mmmm == 0)
            {
                throw new Exception(sMessage);
            }
        }
        #endregion

        #region 撤销对端出院登记信息 带参数
        /// <summary>
        /// 出院登记撤销
        /// </summary>
        public void CancelOutReigster(string areaCode, string sInpatientId)
        {
            int mmmm = serr.InpatientOutRegisterCancel(areaCode, sInpatientId, sHospitalCode, "费用信息不符", operatorInfo.UserName, "", "", "", "", "", ref sMessage);

            if (mmmm == 0)
            {
                throw new Exception(sMessage);
            }
        }
        #endregion

        #region 撤销对端结算信息
        /// <summary>
        /// 出院结算撤销
        /// </summary> 
        public void CancelReimSettle()
        {
            //141028000025145

            string strGetpatinetid = "SELECT MAX(NET_REG_SERIAL) AS NET_REG_SERIAL FROM ZY.[IN].IN_NETWORK_REGISTERS WHERE  FLAG_INVALID = 0 AND PAT_IN_HOS_ID = '" + inPayPara.PatInfo.PatInHosId + "';";
            DataSet dspaid = new DataSet();
            string inpatientid = sqlHelper.ExecSqlReDs(strGetpatinetid).Tables[0].Rows[0]["NET_REG_SERIAL"].ToString();
            inPayPara.RegInfo.NetRegSerial = inpatientid;

            int Re = serr.InpatientCalculateCancel(sAreaCode, inpatientid, inPayPara.SettleInfo.SettleNo, sHospitalCode, "费用变更", operatorInfo.UserName, "", "", "", "", "", ref sMessage);

            if (Re == 0)
            {
                throw new Exception(sMessage);
            }

            CancelOutReigster();
        }


        #endregion

        #region 撤销对端出院结算信息 带参数
        /// <summary>
        /// 出院结算撤销
        /// </summary>

        public void CancelReimSettle(string areaCode, string sInpatientReNo, string sCaculateCode)
        {

            int Re = serr.InpatientCalculateCancel(areaCode, sInpatientReNo, sCaculateCode, sHospitalCode, "费用变更", operatorInfo.UserName, "", "", "", "", "", ref sMessage);

            if (Re == 0)
            {
                throw new Exception(sMessage);
            }

        }
        #endregion

        #region 提交结算申请
        /// <summary>
        /// 提交结算申请
        /// </summary>
        public void SubmitApply()
        {
            //地区代码	In	sAreaCode	
            //就诊ID	In	sInpatientID	
            //医疗机构编号	In	sHospitalCode	
            //出院疾病诊断编码1	In	sDiagnoseCodeOut1	
            //出院疾病诊断名称1	In	sDiagnoseNameOut1	
            //出院疾病诊断编码2	In	sDiagnoseCodeOut2	是
            //出院疾病诊断名称2	In	sDiagnoseNameOut2	是
            //出院疾病诊断编码3	In	sDiagnoseCodeOut3	是
            //出院疾病诊断名称3	In	sDiagnoseNameOut3	是
            //补偿类别编码	In	sCalcCode	
            //操作人姓名	In	sOperatorName	
            //HIS住院发生总费用	In	sAllInCost	
            //预留字段1	In	sObligate1	是
            //预留字段2	In	sObligate2	是
            //预留字段3	In	sObligate3	是
            //预留字段4	In	sObligate4	是
            //预留字段5	In	sObligate5	是
            //错误/提示信息	Out	sMessage	
            //MessageBox.Show(inReimPara.NetWorkRegNo);
            int registerResult = serr.SubmitApply(
                        //  名称	In/Out	字段
                        sAreaCode,                //地区代码	In	sAreaCode
                        inPayPara.RegInfo.NetRegSerial,  //就诊ID	In	sInpatientID
                        sHospitalCode,            //医疗机构编号	In	sHospitalCode 
                        inPayPara.RegInfo.NetDiagnosCode,//入院疾病诊断编码1	In	sDiagnoseCodeIn1
                        inPayPara.RegInfo.NetDiagnosName,//入院疾病诊断名称1	In	sDiagnoseNameIn1 
                        "",                       //出院疾病诊断编码2	In	sDiagnoseCodeOut2	是
                        "",                       //出院疾病诊断名称2	In	sDiagnoseNameOut2	是
                        "",                       //出院疾病诊断编码3	In	sDiagnoseCodeOut3	是
                        "",                       //出院疾病诊断名称3	In	sDiagnoseNameOut3	是
                        netPatInfo.strRedeemNo,   //补偿类别编码	In	sCalcCode	
                        operatorInfo.UserName,     //操作人姓名	In	sOperatorName	
                        totalAmount,              //HIS住院发生总费用	In	sAllInCost	
                        "",                       //预留字段1	In	sObligate1	是
                        "",                       //预留字段2	In	sObligate2	是
                        "",                       //预留字段3	In	sObligate3	是
                        "",                       //预留字段4	In	sObligate4	是
                        "",                       //预留字段5	In	sObligate5	是
                        ref sMessage              //错误/提示信息	Out	sMessage	

                       );

            if (registerResult == 0)
            {
                throw new Exception(sMessage);
            }
        }
        #endregion

        #region 单病种信息上传
        /// <summary>
        /// 单病种信息上传
        /// </summary>
        public void InpatDiagnosisUpdate()
        {
            int registerResult = serr.InpatDiagnosisUpdate(                  //名称	In/Out	字段	是否可为空	说明
                sAreaCode,                                                   //地区代码	In	sAreaCode		见附表1
                operatorInfo.UserSysId,                                      //用户名	In	sUserCode		当前用户在新农合系统的登录用户名
                "",                                                          //密码	In	sUserPass		当前用户在新农合系统的登录用户口令
                sHospitalCode,                                               //医疗机构编号	In	sHospitalCode		见附表2
                inPayPara.RegInfo.NetRegSerial,                              //就诊ID	In	sInpatientID		通过InpatientRegister取得的ID
                "",                                                          //身高（CM）	In	sStature	是	
                "",                                                          //体重（KG）	In	sWeight	是	
                netPatInfo.strProcreateNotice,                               //治疗方式编码	In	sTreatCode		
                netPatInfo.strOutOfficeId,                                   //单病种ICD编码	In	sIcdno		
                netPatInfo.strOutOfficeName,                                 //单病种ICD名称	In	sIcdName		
                inPayPara.PatInfo.InNetWorkDeptCode,                         //中心入院科室编码	In	sSectionOfficeCode		见附表S201-03
                netPatInfo.strCureId,                                        //就诊类型编码	In	sCureCode		见附表S301-05
                netPatInfo.strInHosId,                                       //入院状态编码	In	sInHospitalCode		见附表S301-02
                inPayPara.PatInfo.InDateTime.ToString("yyyy-MM-dd hh:mm:ss"),//入院时间	In	sInHosptialDate		格式：YYYY-MM-DD HH:MM:SS
                operatorInfo.UserName,                                       //操作人姓名	In	sOperatorName		
                "",                                                          //预留字段1	In	sObligate1	是	
                "",                                                          //预留字段2	In	sObligate2	是	
                "",                                                          //预留字段3	In	sObligate3	是	
                "",                                                          //预留字段4	In	sObligate4	是	
                "",                                                          //预留字段5	In	sObligate5	是	
                ref sMessage                                                 //错误/提示信息	Out	sMessage		成功返回空/提示信息，否则返回错误提示信息
            );

            if (registerResult == 0)
            {
                throw new Exception(sMessage);
            }
        }
        #endregion

        #region 门诊业务
        /// <summary>
        /// 读取医保卡信息
        /// </summary>
        /// <returns></returns>
        public NetworkPatInfo NetworkReadCard()
        {
            NetworkPatInfo networkPatInfo = new NetworkPatInfo
            {
                MedicalNo = "",                    //医保个人编号
                ICNo = "",                         //社会保障卡卡号
                PatName = "",                      //姓名
                Sex = "",                          //性别
                IDNo = "",                         //身份证号码
                MedicalType = "",                  //医疗人员类别
                ICAmount = 0,                      //账户余额
                CompanyName = "",                  //单位名称
                CompanyNo = "",                    //单位编号 
            };
             
            return networkPatInfo; 
        }


        /// <summary>
        /// 门诊登记
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void OutNetworkRegister(OutPayParameter para)
        {
            outPayPara = para;
            ReadMedCard(-1, true);
            //门诊登记
            OutPatReimRegister();
        }

        /// <summary>
        /// 门诊预结算
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void OutNetworkPreSettle(OutPayParameter para)
        {
            outPayPara = para;
            //门诊上传费用
            OutPatReimUpItems_batch();
        }
        /// <summary>
        /// 门诊结算
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void OutNetworkSettle(OutPayParameter para)
        {
            outPayPara = para;
            //上传诊断编码
            OutpatDiagnosisUpdate();
            //门诊结算
            OutPatReimSettle();
        }
        /// <summary>
        /// 撤销门诊结算
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void CancelOutNetworkSettle(OutPayParameter para)
        {
            outPayPara = para;
            //撤销结算
            CancelOutReimSettle();
        }
        #endregion

        #region 住院业务
        /// <summary>
        /// 住院登记
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void InNetworkRegister(InPayParameter para)
        {
            inPayPara = para;
            ReadMedCard(0);
            inPayPara.RegInfo.RegTimes += 1;
            InReimRegister();
        }

        /// <summary>
        /// 撤销住院登记
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void CancelInNetworkRegister(InPayParameter para)
        {
            inPayPara = para;
            ReadMedCard(0);
            CancelInRegister();
        }
        /// <summary>
        /// 住院预结算
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void InNetworkPreSettle(InPayParameter para)
        { 

            inPayPara = para;

            if (inPayPara.SettleInfo != null && inPayPara.SettleInfo.MedAmountTotal != 0)
            {
                PayType payType = new PayType();
                payType.PayTypeId = 6;
                payType.PayTypeName = "农合";
                payType.PayAmount = inPayPara.SettleInfo.MedAmountTotal;
                inPayPara.PayTypeList = new List<PayType>();
                inPayPara.PayTypeList.Add(payType);
                return;
            }
            ReadMedCard(1);
            //CancelItems();
            //InReimUpItems();
            if (sAreaCode == "341226")
            {
                //MessageBox.Show("批量上传完成");
                if (OneByOneUploadFlag == true)
                {
                    InReimUpItems();
                }
                else
                {
                    InReimUpItems_batch();
                }
            }
            else
            {
                InModifyReimRegist();
                if (OneByOneUploadFlag == true)
                {
                    InReimUpItems();
                }
                else
                {
                    InReimUpItems_batch();
                }
            }

            if ((inPayPara.CommPara.NetworkPatClassId.ToString() != "2"))
            {
                SubmitApply();
            }

            if (netPatInfo.strRedeemNo == "113" || netPatInfo.strRedeemNo == "62" || netPatInfo.strRedeemNo == "209")
            {
                InpatDiagnosisUpdate();   //临床路径上传单病种信息
            }

            OutReimRegister();
            InReimSettle();

        }

        /// <summary>
        /// 住院结算
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void InNetworkSettle(InPayParameter para)
        {
            inPayPara = para;

            if (inPayPara.SettleInfo != null && inPayPara.SettleInfo.MedAmountTotal != 0)
            {
                PayType payType = new PayType();
                payType.PayTypeId = 6;
                payType.PayTypeName = "农合";
                payType.PayAmount = inPayPara.SettleInfo.MedAmountTotal;
                inPayPara.PayTypeList = new List<PayType>();
                inPayPara.PayTypeList.Add(payType);
                return;
            }
        }


        /// <summary>
        /// 撤销住院结算
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void CancelInNetworkSettle(InPayParameter para)
        {
            inPayPara = para;
            ReadMedCard(1);
            CancelReimSettle();
        }
        #endregion
    }
}
