using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using PayAPIInterfaceHandle.Neusoft;
using PayAPIResolver.Neusoft;
using PayAPIInterface;
using PayAPIInterface.Model.Comm;
using PayAPIInterface.ParaModel;
using PayAPIUtilities.Config;
using PayAPIClassLib.BLL;
using PayAPIClassLib.ParaModel;
using PayAPIInterface.Model.Out;
using PayAPIInterface.Model.In;
using PayAPIInstance.Neusoft.Dialogs;
using PayAPIResolver;
using System.Data;

namespace PayAPIInstance.Neusoft
{
    public class NeusoftModel : IPayCompanyInterface
    {

        NetworkPatInfo networkPatInfo = new NetworkPatInfo();
        OutNetworkRegisters outnetworkregisters = new OutNetworkRegisters();
        InNetworkRegisters inNetworkRegisters = new InNetworkRegisters();


        private bool isOut = false;                                //是否门诊

        private string strDiagnosName = "";                        //诊断名称
        private string strDiagnosCode = "";                        //诊断编号
        private string strTypeCode = "";                           //医疗类型编号
        private string strTypeName = "";                           //医疗类型名称

        private string patSerial = "";                             //流水号
        private string settleNo = "";                              //单据号
        private string settleTime = "";                            //结算时间
        private string OutTime = "";                               //出院时间

        private string OutReason = "0";                             //出院原因 
        public string TCqh = "";
        private bool isType = true;                             //是否是读卡功能
        OutPayParameter OutPayPara;
        InPayParameter InPayPara;
        public string isCard_ZF = "1";                            //是否使用卡支付  1.账户支付 0.账户不支付      

        public static bool isZYDJ = false;                         //住院登记 如果是住院登记时，个人信息及诊断界面内的出院诊断不显示，预结算时显示出院诊断
        public static List<InNetworkUpDetail> Gs_detailTable;

        public decimal Amount = 0;                                  //异地预结算传总费用
        /// <summary>
        /// 东软解析类
        /// </summary>
        private NeusoftResolver neusoftResolver;

        /// <summary>
        /// 东软函数处理
        /// </summary>
        private INeusoftHandle neusoftHandle = NeusoftHandleFactory.GetCommNeusoftHandle();


        public bool IsInit = false;

        /// <summary>
        /// 初始化东软
        /// </summary>
        public void InitNeusoft()
        {
            if (!IsInit)
            {
                neusoftResolver = new NeusoftResolver();
                StringBuilder output = new StringBuilder(5000);
                int result = neusoftHandle.Init(output);

                if (result != 0)
                {
                    throw new Exception("东软初始化失败，错误提示：" + output.ToString());
                }
                IsInit = true;
            }
        }
        /// <summary>
        /// 东软服务调用
        /// </summary>
        public string[] Handle()
        {

            string input = neusoftResolver.GetInputPara();
            string otherParam = "";
            string cardinfo = "";

            if (isType)  //读卡时  
            {
                otherParam = TCqh + "|||";  //cardInfo 参数返回空 otherParam 参数格式为   统筹区号|||

            }
            else  //其他操作
            {
                otherParam = TCqh + "|" + networkPatInfo.MedicalNo + "|" + networkPatInfo.IDNo + "|";   //otherParam的格式为  统筹区号|个人编号|身份证号|
                cardinfo = "|" + networkPatInfo.MedicalNo + "|";  // cardinfo参数的格式为    |卡号|
            }
            StringBuilder output = new StringBuilder(3000);
            int result = neusoftHandle.BusinessHandle(input, output);
            //int result = neusoftHandle.BusinessHandle_EX(input, otherParam, cardinfo, output);

            if (result != 0)
            {
                throw new Exception("东软调用失败，错误提示：" + output.ToString());
            }
            return neusoftResolver.ResolveOutput(output.ToString());
        }

        /// <summary>
        /// 签到
        /// </summary>
        public void StructMethod()
        {

            InitNeusoft();
            neusoftResolver = new NeusoftResolver();
            if (NeusoftResolver.BusinessCycleNum.Length == 0)
            {
                neusoftResolver.InitResolver("9100");
                var resultArr = Handle();
                NeusoftResolver.BusinessCycleNum = resultArr[0];
            }
        }

        /// <summary>
        /// 签退
        /// </summary>
        public void StructMethod_Out()
        {
            try
            {
                InitNeusoft();
                neusoftResolver = new NeusoftResolver();
                neusoftResolver.InitResolver("9110");
                var resultArr = Handle();

            }
            catch
            {
            }
            finally
            {

            }

        }
        /// <summary>
        /// 对账
        /// </summary>
        public void Str_DuiZhang()
        {
            try
            {
                InitNeusoft();
                neusoftResolver = new NeusoftResolver();
                neusoftResolver.InitResolver("1110");
                neusoftResolver.AddInParas(NeusoftResolver.BusinessCycleNum);               //业务周期号
                neusoftResolver.AddInParas("0.00");                                         //his医疗费总额
                neusoftResolver.AddInParas("0.00");                                         //his账户支付合计
                neusoftResolver.AddInParas("0.00");                                         //his现金支付合计
                neusoftResolver.AddInParas("0.00");                                         //his统筹基金支付合计
                neusoftResolver.AddInParas("0.00");                                         //his救助金支付合计
                neusoftResolver.AddInParas("0.00");                                         //his公务员不住合计
                neusoftResolver.AddInParas("0.00");                                         //his企业补充基金支付合计
                neusoftResolver.AddInParas("0.00");                                         //his其他基金支付合计
                var resultArr = Handle();

            }
            catch
            {
            }
            finally
            {

            }

        }

        /// <summary>
        /// 读取医保卡信息
        /// </summary>
        /// <returns></returns>
        public NetworkPatInfo NetworkReadCard()
        {
            InitNeusoft();
            StructMethod();
            neusoftResolver = new NeusoftResolver();
            neusoftResolver.InitResolver("2100");

            //if (isType == true)   //是读卡功能 跳转页面获取统筹区号的ID 
            //{
            //    InfoTongChou tongchou = new InfoTongChou(true, networkPatInfo, outnetworkregisters);
            //    tongchou.ShowDialog();
            //    TCqh = tongchou.TCTypeCode;
            //}

            var resultArr = Handle();
            networkPatInfo.MedicalNo = resultArr[0];                   //医保个人编号
            networkPatInfo.ICNo = resultArr[9] + "FY2002";             //社会保障卡卡号
            networkPatInfo.PatName = resultArr[3];                     //姓名
            networkPatInfo.Sex = resultArr[4] == "1" ? "男" : "女";    //性别
            networkPatInfo.IDNo = resultArr[2];                        //身份证号码
            networkPatInfo.MedicalType = resultArr[10];                //医疗人员类别
            networkPatInfo.ICAmount = Convert.ToDecimal(resultArr[34]);//账户余额
            networkPatInfo.CompanyName = resultArr[17];                //单位名称
            networkPatInfo.CompanyNo = resultArr[1];                   //单位编号
            outnetworkregisters.Memo1 = resultArr[14];                 //本地异地的标志
            return networkPatInfo;
        }

        /// <summary>
        /// 门诊读卡
        /// </summary>
        /// <param name="para"></param>
        public void OutNetworkRegister(OutPayParameter para)
        {
            OutPayPara = para;
            InitNeusoft();
            StructMethod();
            ReadMedCard(true);

            //patSerial = "MZ" + para.CommPara.OutNetworkSettleId.ToString();
            patSerial = "MZ" + para.CommPara.OutNetworkSettleId.ToString() + DateTime.Now.ToString("HHmmss");
            settleNo = patSerial;

            OutReimRegister();

        }

        /// <summary>
        /// 读取患者信息
        /// </summary>
        public bool ReadMedCard(bool isOut)
        {
            isType = false;
            NetworkReadCard();

            InfoDiagnosis infoForm = new InfoDiagnosis(isOut, networkPatInfo, OutPayPara, InPayPara);

            if (InPayPara != null || (OutPayPara != null && OutPayPara.CommPara.NetworkType != "ZJJS"))
            {
                infoForm.ShowDialog();
                if (!infoForm.isOk)
                    throw new Exception("诊断录入操作取消");
            }
            isZYDJ = false;
            strDiagnosCode = infoForm.ReDiagnosCode;
            strDiagnosName = infoForm.ReDiagnosName;
            strTypeCode = infoForm.ReTypeCode;
            strTypeName = infoForm.ReTypeName;
            OutReason = infoForm.OutHosStatus;


            //诊间结算
            if (OutPayPara != null && OutPayPara.CommPara.NetworkType == "ZJJS")
            {
                strDiagnosCode = "";
                strDiagnosName = "";
                strTypeCode = "11";
                strTypeName = "普通门诊";
                OutReason = "";
            }

            string HisPatName = "";

            if (isOut == true) //isout 等于 true 代表门诊
            {
                HisPatName = OutPayPara.PatInfo.PatName;
                OutPayPara.RegInfo = new PayAPIInterface.Model.Out.OutNetworkRegisters
                {
                    NetPatName = networkPatInfo.PatName,
                    Balance = networkPatInfo.ICAmount,
                    CardNo = networkPatInfo.ICNo,
                    MemberNo = networkPatInfo.MedicalNo,
                    NetType = infoForm.ReTypeCode,
                    CantonCode = "1",
                    PatAddress = networkPatInfo.CompanyName,
                    CompanyName = networkPatInfo.CompanyName,
                    NetPatType = networkPatInfo.MedicalType,
                    IdNo = networkPatInfo.IDNo,
                    Memo1 = "",
                    Memo2 = "",
                    NetRegSerial = "",
                    OperatorId = PayAPIConfig.Operator.UserSysId,
                    OutNetworkSettleId = OutPayPara.CommPara.OutNetworkSettleId,
                    RegTimes = 0,
                    NetDiagnosCode = "",
                    NetDiagnosName = "",
                    IsInvalid = true,
                    IsReg = true,
                    OutPatId = OutPayPara.PatInfo.OutPatId,
                    PatSerial = ""
                };


            }
            else   //住院
            {
                HisPatName = InPayPara.PatInfo.InPatName;

                InPayPara.RegInfo.CardNo = networkPatInfo.ICNo;
                InPayPara.RegInfo.MemberNo = networkPatInfo.MedicalNo;
                InPayPara.RegInfo.CantonCode = "1";
                InPayPara.RegInfo.PatAddress = networkPatInfo.CompanyName;
                InPayPara.RegInfo.CompanyName = networkPatInfo.CompanyName;
                InPayPara.RegInfo.NetPatType = networkPatInfo.MedicalType;
                InPayPara.RegInfo.NetDiagnosCode = infoForm.ReDiagnosCode;
                InPayPara.RegInfo.NetDiagnosName = infoForm.ReDiagnosName;
                InPayPara.RegInfo.IdNo = networkPatInfo.IDNo;
                InPayPara.RegInfo.NetType = infoForm.ReTypeCode;
                InPayPara.RegInfo.NetPatName = networkPatInfo.PatName;
                InPayPara.RegInfo.PatClassID = "-1";
                InPayPara.RegInfo.PatInHosSerial = "";
                InPayPara.RegInfo.OperatorId = PayAPIConfig.Operator.UserSysId;
                //InPayPara.PatOutStatusID = Convert.ToInt32(infoForm.OutHosStatus);//出院状态
            }

            if (networkPatInfo.PatName != HisPatName)
            {
                throw new Exception("HIS登记姓名:" + HisPatName + ",医保读卡姓名：" + networkPatInfo.PatName + ",两者不一致，请核对！");
            }
            return true;
        }

        /// <summary>
        /// 门诊预结算
        /// </summary>
        /// <param name="para"></param>
        public void OutNetworkPreSettle(OutPayParameter para)
        {
            OutPayPara = para;

            InitNeusoft();
            OutReimUpItems();
            return;
        }

        /// <summary>
        /// 门诊结算
        /// </summary>
        /// <param name="para"></param>
        public void OutNetworkSettle(OutPayParameter para)
        {
            OutPayPara = para;
            //参数赋值
            InitNeusoft();
            OutTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            settleTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            //预结算，显示预结算信息
            //ReimSettle(0);
            //结算
            OutReason = "0";
            ReimSettle(1);
            SaveOutSettleMain();
            return;
        }
        /// <summary>
        /// 门诊挂号/登记
        /// </summary>
        /// <returns></returns>
        public string OutReimRegister()
        {
            isType = false;
            neusoftResolver.InitResolver("2210");
            neusoftResolver.AddInParas(patSerial);                                      //门诊流水号
            neusoftResolver.AddInParas(strTypeCode);                                    //医疗类别
            neusoftResolver.AddInParas(DateTime.Now.ToString("yyyyMMddHHmmss"));        //挂号日期
            neusoftResolver.AddInParas(strDiagnosCode);                                 //诊断疾病编码
            neusoftResolver.AddInParas(strDiagnosName);                                 //诊断疾病名称
            neusoftResolver.AddInParas("");                                             //病历信息
            neusoftResolver.AddInParas("");                                             //科室名称
            neusoftResolver.AddInParas("");                                             //床位号
            neusoftResolver.AddInParas("");                                             //医生代码
            neusoftResolver.AddInParas("");                                             //医生姓名
            neusoftResolver.AddInParas("");                                             //挂号费
            neusoftResolver.AddInParas("");                                             //检查费
            neusoftResolver.AddInParas(PayAPIConfig.Operator.UserName);                 //经办人
            neusoftResolver.AddInParas("");                                             //前次住院号
            neusoftResolver.AddInParas("");                                             //精神病院免起付线标志
            neusoftResolver.AddInParas("");                                             //分院编码
            neusoftResolver.AddInParas("");                                             //第一副诊断编码
            neusoftResolver.AddInParas("");                                             //第一副诊断名称
            neusoftResolver.AddInParas("");                                             //第二副诊断    
            neusoftResolver.AddInParas("");                                             //第二副诊断名称
            neusoftResolver.AddInParas("");                                             //档案号
            neusoftResolver.AddInParas("");                                             //身份证号
            neusoftResolver.AddInParas("");                                             //联系电话
            neusoftResolver.AddInParas("");                                             //备注 
            if (neusoftResolver.ListOutParas[14] == "1")                     //判断如果    异地人员标志  为1（异地） 则添加医院等级
            {
                neusoftResolver.AddInParas("");                                             //医院等级
            }
            else
            {
                neusoftResolver.AddInParas("");                                             //医院等级
            }
            neusoftResolver.AddInParas("");                                             //入院第三副诊断编码
            neusoftResolver.AddInParas("");                                             //入院第四副诊断编码
            neusoftResolver.AddInParas("");                                             //入院第五副诊断编码
            var resultArr = Handle();
            OutPayPara.RegInfo.PatSerial = patSerial;                                   //就诊流水号
            OutPayPara.RegInfo.Memo1 = neusoftResolver.HosSendSerial;                //交易流水号

            return "";

        }





        /// <summary>
        /// 门诊费用明细数据上传-每次上传一条
        /// </summary>
        /// <returns></returns>
        public string OutReimUpItems()
        {
            isType = false;
            for (int i = 0; i < OutPayPara.Details.Count; i++)
            {
                if (Convert.ToDecimal(OutPayPara.Details[i].Quantity) > 0)
                {
                    OutPayPara.Details[i].Price = Convert.ToDecimal(OutPayPara.Details[i].Amount) / Convert.ToDecimal(OutPayPara.Details[i].Quantity);
                }
                else
                {
                    OutPayPara.Details[i].Price = Convert.ToDecimal(OutPayPara.Details[i].Amount);
                }

                if (string.IsNullOrEmpty(OutPayPara.Details[i].NetworkItemCode.ToString().Trim()))  //如果为空
                {

                    OutPayPara.Details[i].NetworkItemCode = "AAAA";
                    if (Convert.ToInt32(OutPayPara.Details[i].ChargeType) < 100)
                    {
                        OutPayPara.Details[i].NetworkItemProp = "1";//1药品、2诊疗项目
                    }
                    else
                    {
                        OutPayPara.Details[i].NetworkItemProp = "2";//1药品、2诊疗项目
                    }
                    OutPayPara.Details[i].NetworkItemClass = "91";//其他费用    
                }


                neusoftResolver.InitResolver("2310");
                neusoftResolver.AddInParas(patSerial);                                                                         //not null 住院流水号(门诊流水号)
                if (OutPayPara.Details[i].NetworkItemProp.ToString().Equals("1"))                                       //1 药品
                {
                    neusoftResolver.AddInParas(OutPayPara.Details[i].NetworkItemProp.ToString());                           //not null 项目类别 修改
                }
                if (OutPayPara.Details[i].NetworkItemProp.ToString().Equals("2"))                                       //2 诊疗
                {
                    neusoftResolver.AddInParas("2");
                }
                if (OutPayPara.Details[i].NetworkItemProp.ToString().Equals("3"))                                       //3 服务设施
                {
                    neusoftResolver.AddInParas("3");
                }

                neusoftResolver.AddInParas(OutPayPara.Details[i].NetworkItemClass.ToString());                       //not null 中心费用类别 修改
                neusoftResolver.AddInParas(OutPayPara.Details[i].AutoId.ToString());                                       //not null 处方号
                neusoftResolver.AddInParas(Convert.ToDateTime(OutPayPara.Details[i].CreateTime).ToString("yyyyMMddHHmmss")); //not null 处方日期                               
                neusoftResolver.AddInParas(OutPayPara.Details[i].ChargeCode.ToString());                                     //not null 医院收费项目内码
                neusoftResolver.AddInParas(OutPayPara.Details[i].NetworkItemCode.ToString());                               //收费项目中心编码
                if (OutPayPara.Details[i].ChargeName.ToString().Trim().Length > 25)                                      //not null 医院收费项目名称VARCHAR2(50)
                {
                    neusoftResolver.AddInParas(OutPayPara.Details[i].ChargeName.ToString().Trim().Substring(0, 25));     //大于25位截取前25位
                }
                else
                {
                    neusoftResolver.AddInParas(OutPayPara.Details[i].ChargeName.ToString().Trim());
                }

                neusoftResolver.AddInParas(OutPayPara.Details[i].Price.ToString());                                           //not null 单价
                neusoftResolver.AddInParas(OutPayPara.Details[i].Quantity.ToString());                                        //not null 数量
                neusoftResolver.AddInParas(OutPayPara.Details[i].DrugFormName.ToString());                                  //剂型
                neusoftResolver.AddInParas(OutPayPara.Details[i].Spec.ToString());                                             //规格
                neusoftResolver.AddInParas("");                                                                              //每次用量
                neusoftResolver.AddInParas("");                                                                                //使用频次
                neusoftResolver.AddInParas(OutPayPara.Details[i].DocCode.ToString());                                        //医生姓名 修改
                neusoftResolver.AddInParas(OutPayPara.Details[i].DocCode.ToString());                                        //处方医师
                neusoftResolver.AddInParas("");                                                                                 //用法
                neusoftResolver.AddInParas("");                                                                                //单位
                neusoftResolver.AddInParas("");                                       //科别名称 修改
                neusoftResolver.AddInParas("");                                                                                //执行天数
                neusoftResolver.AddInParas("1");                                                                                //草药单复方标志
                neusoftResolver.AddInParas(PayAPIConfig.Operator.UserName);                                                               //经办人
                neusoftResolver.AddInParas("");                                                                               //空
                neusoftResolver.AddInParas("");                                                                                //明细扣款金额
                neusoftResolver.AddInParas(OutPayPara.Details[i].Amount.ToString());                                         //金额
                neusoftResolver.AddInParas("");                                                                                 //自付比例
                neusoftResolver.AddInParas("");                                                                                //记账类别
                if (outnetworkregisters.Memo1 == "1")                                                                           //如果是异地输出项目等级
                {
                    neusoftResolver.AddInParas("");
                }
                else
                {
                    neusoftResolver.AddInParas("");
                }

                var resultArr = Handle();

                //返回值
                string str0 = resultArr[0];//超出治疗方案自付金额
                string str1 = resultArr[1];//金额
                string str2 = resultArr[2];//自理金额
                string str3 = resultArr[3];//自费金额
                string str4 = resultArr[4];//收费项目等级
                string str5 = resultArr[5];//全额自费标志  1自费 0非自费  

                string str11 = "";
                
                OutPayPara.Details[i].AmountSelf = Convert.ToDecimal(str2);
                OutPayPara.Details[i].AmountSelfBurdern = Convert.ToDecimal(str3);
                OutPayPara.Details[i].MedAmount = Convert.ToDecimal(str1) - Convert.ToDecimal(str2) - Convert.ToDecimal(str3);

                OutPayPara.Details[i].UploadBackSerial = neusoftResolver.ListOutput[1];
            }

            return "";
        }

        #region 门诊结算

        /// <summary>
        /// 结算
        /// </summary>
        /// <param name="type">0：预结算1：结算2：结算撤销</param>
        /// <returns></returns>
        public string ReimSettle(int type)
        {
            isType = false;
            switch (type)
            {
                case 0:
                    neusoftResolver.InitResolver("2420");                                 //预结算
                    SettleData();
                    var resultArr = Handle();
                    //显示预结算信息
                    break;
                case 1:
                    neusoftResolver.InitResolver("2410");                                     //结算
                    SettleData();
                    resultArr = Handle();
                    break;
                case 2:
                    neusoftResolver.InitResolver("2430");                                    //结算撤销
                    SettleData_CancelSettle();
                    resultArr = Handle();
                    break;
                default:
                    break;
            }

            return "";
        }

        private void SettleData()//结算辅助方法
        {
            neusoftResolver.AddInParas(patSerial);             //not null 门诊流水号 
            neusoftResolver.AddInParas(settleNo);              //单据号 not null
            neusoftResolver.AddInParas(strTypeCode);           //医疗类别 not null
            neusoftResolver.AddInParas(settleTime);            //结算日期 not null
            neusoftResolver.AddInParas(OutTime);               //出院日期
            neusoftResolver.AddInParas(OutReason);             //出院原因
            neusoftResolver.AddInParas(strDiagnosCode);        //出院诊断疾病编码
            neusoftResolver.AddInParas(strDiagnosName);        //出院诊断疾病名称
            neusoftResolver.AddInParas("");                    //第一副诊断编码
            neusoftResolver.AddInParas("");                    //第一副诊断名称
            neusoftResolver.AddInParas("");                    //第二副诊断编码
            neusoftResolver.AddInParas("");                    //第二副诊断名称
            neusoftResolver.AddInParas("");                    //医嘱信息
            neusoftResolver.AddInParas("");                    //出院类别
            neusoftResolver.AddInParas("0");                   //报销类别 not null
            /*if (!isOut)
            {
                if (MessageBox.Show("是否使用账户支付", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    isCard_ZF = "1";
                }
                else
                {
                    isCard_ZF = "0";
                }
            }*/
            neusoftResolver.AddInParas(isCard_ZF);             //账户使用标志 not null 0：帐户不支付 1：账户支付
            neusoftResolver.AddInParas("0");                   //中途结算标志
            neusoftResolver.AddInParas(PayAPIConfig.Operator.UserName);  //经办人 not null 
            neusoftResolver.AddInParas("");                    //是否保存处方标志
            neusoftResolver.AddInParas("");                    //医生姓名 
            neusoftResolver.AddInParas("");                    //中心报销统筹扣款金额
            neusoftResolver.AddInParas("");                    //中心报销救助扣款金额   
            neusoftResolver.AddInParas("");                    //存档号
            neusoftResolver.AddInParas("");                    //公务报销标识（芜湖）
            neusoftResolver.AddInParas("");                    //生育中心报销比例
            neusoftResolver.AddInParas("");                    //生育胎儿数(省直)
            neusoftResolver.AddInParas("");                    //连续住院标志（宿州用）
            neusoftResolver.AddInParas("");                    //婴儿出生日期（省直）
            //neusoftResolver.AddInParas("");                    //外地已报销金额（淮北用）
            neusoftResolver.AddInParas("");                    //生产方式（省直）/死亡保险标志（淮北用）
            neusoftResolver.AddInParas("");                    //并发症标志（省直）
            neusoftResolver.AddInParas("");                    //结算项目（省直）
            neusoftResolver.AddInParas("");                    //特病标志（巢湖）
            /*neusoftResolver.AddInParas("");                    //单据数（蚌埠）
            neusoftResolver.AddInParas("");                    //生育病种信息（淮北）
            //------------------新增加-----------------------------------------------------------
            neusoftResolver.AddInParas("");                    //蚌埠中心报销自付增加比例
            if (outnetworkregisters.Memo1 == "1")           //如果是异地输出总费用
            {
                neusoftResolver.AddInParas(Amount);                    //总费用  异地
            }
            else
            {
                neusoftResolver.AddInParas("");                    //总费用 本地返回空
            }

            neusoftResolver.AddInParas("");                    //出院第三副诊断编码
            neusoftResolver.AddInParas("");                    //出院第四副诊断编码
            neusoftResolver.AddInParas("");                    //出院第五副诊断编码
            neusoftResolver.AddInParas("");*/
            //诊断附码

        }


        private void SettleData_CancelSettle()//撤销结算辅助方法
        {
            neusoftResolver.AddInParas(patSerial);             //not null 门诊流水号 
            neusoftResolver.AddInParas(settleNo);              //单据号 not null
            neusoftResolver.AddInParas(strTypeCode);           //医疗类别 not null
            neusoftResolver.AddInParas(settleTime);            //结算日期 not null
            neusoftResolver.AddInParas(OutTime);               //出院日期
            //neusoftResolver.AddInParas(OutReason);             //出院原因
            //neusoftResolver.AddInParas(strDiagnosCode);        //出院诊断疾病编码
            //neusoftResolver.AddInParas(strDiagnosName);        //出院诊断疾病名称
            neusoftResolver.AddInParas("");                     //出院原因
            neusoftResolver.AddInParas("");                     //出院诊断疾病编码
            neusoftResolver.AddInParas("");                     //出院诊断疾病名称
            neusoftResolver.AddInParas("");                    //第一副诊断编码
            neusoftResolver.AddInParas("");                    //第一副诊断名称
            neusoftResolver.AddInParas("");                    //第二副诊断编码
            neusoftResolver.AddInParas("");                    //第二副诊断名称
            neusoftResolver.AddInParas("");                    //医嘱信息
            neusoftResolver.AddInParas("");                    //出院类别
            neusoftResolver.AddInParas("0");                   //报销类别 not null
            neusoftResolver.AddInParas("1");                   //账户使用标志 not null 0：帐户不支付 1：账户支付
            neusoftResolver.AddInParas("0");                   //中途结算标志
            neusoftResolver.AddInParas(PayAPIConfig.Operator.UserName);  //经办人 not null 
            neusoftResolver.AddInParas("");                    //是否保存处方标志

            neusoftResolver.AddInParas("");                    //中心报销统筹扣款金额
            neusoftResolver.AddInParas("");                    //中心报销救助扣款金额   

        }

        /// <summary>
        /// 保存门诊结算数据
        /// </summary>
        public void SaveOutSettleMain()
        {

            #region 保存农合中心返回值参数列表
            //保存农合中心返回值参数列表
            try
            {
                OutPayPara.SettleParaList = new List<PayAPIInterface.Model.Out.OutNetworkSettleList>();
                PayAPIInterface.Model.Out.OutNetworkSettleList outNetworkSettleList;
                outNetworkSettleList = new PayAPIInterface.Model.Out.OutNetworkSettleList();
                outNetworkSettleList.OutPatId = OutPayPara.PatInfo.OutPatId;
                outNetworkSettleList.OutNetworkSettleId = OutPayPara.CommPara.OutNetworkSettleId;
                outNetworkSettleList.ParaName = "S1";
                outNetworkSettleList.ParaValue = neusoftResolver.ListOutput[0];
                outNetworkSettleList.Memo = "";
                OutPayPara.SettleParaList.Add(outNetworkSettleList);

                outNetworkSettleList = new PayAPIInterface.Model.Out.OutNetworkSettleList();
                outNetworkSettleList.OutPatId = OutPayPara.PatInfo.OutPatId;
                outNetworkSettleList.OutNetworkSettleId = OutPayPara.CommPara.OutNetworkSettleId;
                outNetworkSettleList.ParaName = "S2";
                outNetworkSettleList.ParaValue = neusoftResolver.ListOutput[1];
                outNetworkSettleList.Memo = "";
                OutPayPara.SettleParaList.Add(outNetworkSettleList);

                for (int i = 0; i < neusoftResolver.ListOutParas.Length; i++)
                {
                    outNetworkSettleList = new PayAPIInterface.Model.Out.OutNetworkSettleList();
                    outNetworkSettleList.OutPatId = OutPayPara.PatInfo.OutPatId;
                    outNetworkSettleList.OutNetworkSettleId = OutPayPara.CommPara.OutNetworkSettleId;
                    outNetworkSettleList.ParaName = i.ToString();
                    outNetworkSettleList.ParaValue = neusoftResolver.ListOutParas[i].ToString();
                    outNetworkSettleList.Memo = "";
                    OutPayPara.SettleParaList.Add(outNetworkSettleList);
                }

            }
            catch (Exception ex)
            {
                //LogManager.RecordException("保存农合中心返回值参数列表 插入值 失败" + ex.Message, "@AnHuiInterfaceModel:门诊结算");
            }
            #endregion



            OutPayPara.SettleInfo = new PayAPIInterface.Model.Out.OutNetworkSettleMain();
            OutPayPara.SettleInfo.OutPatId = OutPayPara.PatInfo.OutPatId;
            OutPayPara.SettleInfo.SettleNo = patSerial;// neusoft.ListOutput[0];              //医保中心交易流水号
            OutPayPara.SettleInfo.Amount = Convert.ToDecimal(neusoftResolver.ListOutParas[47]);       //本次医疗费用
            OutPayPara.SettleInfo.GetAmount = Convert.ToDecimal(neusoftResolver.ListOutParas[50]);    //本次现金支出
            OutPayPara.SettleInfo.MedAmountZhzf = Convert.ToDecimal(neusoftResolver.ListOutParas[49]);//本次帐户支出
            OutPayPara.SettleInfo.MedAmountTc = Convert.ToDecimal(neusoftResolver.ListOutParas[51]);  //本次统筹支出
            OutPayPara.SettleInfo.MedAmountDb = 0;                                            //本次大病支出
            OutPayPara.SettleInfo.MedAmountJm = Convert.ToDecimal(neusoftResolver.ListOutParas[149]); //帐户余额
            OutPayPara.SettleInfo.CreateTime = DateTime.Now;
            OutPayPara.SettleInfo.InvoiceId = -1;
            OutPayPara.SettleInfo.IsCash = true;
            OutPayPara.SettleInfo.IsInvalid = false;
            OutPayPara.SettleInfo.IsNeedRefund = false;
            OutPayPara.SettleInfo.IsRefundDo = false;
            OutPayPara.SettleInfo.IsSettle = true;
            OutPayPara.SettleInfo.MedAmountTotal = Convert.ToDecimal(OutPayPara.SettleInfo.Amount) - Convert.ToDecimal(OutPayPara.SettleInfo.GetAmount);
            OutPayPara.SettleInfo.NetworkingPatClassId = Convert.ToInt32(OutPayPara.CommPara.NetworkPatClassId);
            OutPayPara.SettleInfo.NetworkPatName = networkPatInfo.PatName;
            OutPayPara.SettleInfo.NetworkPatType = networkPatInfo.MedicalType;// "0";
            OutPayPara.SettleInfo.NetworkRefundTime = Convert.ToDateTime("2000-01-01");
            OutPayPara.SettleInfo.NetworkSettleTime = DateTime.Now;
            OutPayPara.SettleInfo.OperatorId = PayAPIConfig.Operator.UserSysId;
            OutPayPara.SettleInfo.OutNetworkSettleId = Convert.ToDecimal(OutPayPara.CommPara.OutNetworkSettleId);
            OutPayPara.SettleInfo.SettleBackNo = NeusoftResolver.BusinessCycleNum;
            OutPayPara.SettleInfo.SettleType = strTypeCode;

            //门诊付费方式 本接口 4 医保 6农合
            PayAPIInterface.Model.Comm.PayType payType = new PayAPIInterface.Model.Comm.PayType();
            payType.PayTypeId = 4;
            payType.PayTypeName = "医保";
            payType.PayAmount = OutPayPara.SettleInfo.MedAmountTotal;
            OutPayPara.PayTypeList = new List<PayType>();
            OutPayPara.PayTypeList.Add(payType);

            if (OutPayPara.CommPara.NetworkType != "ZJJS")
            {
                try
                {
                    InfoPreSettle frmInfo = new InfoPreSettle(neusoftResolver.ListOutParas, networkPatInfo.ICAmount);
                    frmInfo.ShowDialog();
                    frmInfo.Dispose();
                }
                catch (System.Exception ex)
                {
                    //LogManager.RecordException("显示报销信息 失败" + ex.Message, "@SaveOutSettleMain:保存门诊结算");
                }
            }
        }

        /// <summary>
        /// 撤销门诊结算
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void CancelOutNetworkSettle(OutPayParameter para)
        {
            InitNeusoft();
            StructMethod();
            isOut = true;

            //门诊结算撤销 
            //参数赋值
            //strTypeCode = "11";
            //OutReason = "0";
            //patSerial = "MZ" + para.CommPara.OutNetworkSettleId.ToString();
            //settleNo = patSerial;
            patSerial = para.SettleInfo.SettleNo;
            settleNo = para.SettleInfo.SettleNo;
            OutTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            settleTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            strTypeCode = para.SettleInfo.SettleType;
            isType = false;
            NetworkReadCard();
            ReimSettle(2);


            return;
        }
        #endregion

        /// <summary>
        /// 住院登记
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void InNetworkRegister(InPayParameter para)
        {
            StructMethod();
            InPayPara = para;
            InPayPara.RegInfo.RegTimes = InPayPara.RegInfo.RegTimes + 1;

            patSerial = "ZY" + InPayPara.PatInfo.PatInHosId.ToString("0") + "_" + InPayPara.RegInfo.RegTimes.ToString();
            isOut = false;
            isType = false;
            isZYDJ = true;
            ReadMedCard(isOut);
            InReimRegister();
            return;
        }


        /// <summary>
        /// 住院登记
        /// </summary>
        /// <returns></returns>
        public void InReimRegister()
        {
            neusoftResolver.InitResolver("2210");
            neusoftResolver.AddInParas(patSerial);                                      //住院流水号
            neusoftResolver.AddInParas(strTypeCode);                                    //医疗类别
            neusoftResolver.AddInParas(DateTime.Now.ToString("yyyyMMddHHmmss"));        //挂号日期
            neusoftResolver.AddInParas(strDiagnosCode);                                 //诊断疾病编码
            neusoftResolver.AddInParas(strDiagnosName);                                 //诊断疾病名称
            neusoftResolver.AddInParas("");                                             //病历信息
            neusoftResolver.AddInParas("");                                             //科室名称
            neusoftResolver.AddInParas("");                                             //床位号
            neusoftResolver.AddInParas("");                                             //医生代码
            neusoftResolver.AddInParas("");                                             //医生姓名
            neusoftResolver.AddInParas("");                                             //挂号费
            neusoftResolver.AddInParas("");                                             //检查费
            neusoftResolver.AddInParas(PayAPIConfig.Operator.UserName);                 //经办人
            neusoftResolver.AddInParas("");                                             //前次住院号
            neusoftResolver.AddInParas("");                                             //精神病院免起付线标志
            neusoftResolver.AddInParas("");                                             //分院编码
            neusoftResolver.AddInParas("");                                             //第一副诊断编码
            neusoftResolver.AddInParas("");                                             //第一副诊断名称
            neusoftResolver.AddInParas("");                                             //第二副诊断    
            neusoftResolver.AddInParas("");                                             //第二副诊断名称
            neusoftResolver.AddInParas("");                                             //档案号
            neusoftResolver.AddInParas("");                                             //身份证号
            neusoftResolver.AddInParas("");                                             //联系电话
            neusoftResolver.AddInParas("");                                             //备注 
            if (neusoftResolver.ListOutParas[14] == "1")                     //判断如果    异地人员标志  为1（异地） 则添加医院等级
            {
                neusoftResolver.AddInParas("");                                             //医院等级
            }
            else
            {
                neusoftResolver.AddInParas("");                                             //医院等级
            }
            neusoftResolver.AddInParas("");                                             //入院第三副诊断编码
            neusoftResolver.AddInParas("");                                             //入院第四副诊断编码
            neusoftResolver.AddInParas("");                                             //入院第五副诊断编码
            isType = false;
            var resultArr = Handle();
            InPayPara.RegInfo.NetRegSerial = neusoftResolver.HosSendSerial;                //交易流水号
            InPayPara.RegInfo.Memo1 = neusoftResolver.HosSendSerial;
            InPayPara.RegInfo.Memo2 = "";
            InPayPara.RegInfo.PatInHosId = InPayPara.PatInfo.PatInHosId;
            InPayPara.RegInfo.PatInHosSerial = patSerial;
            InPayPara.RegInfo.IsReg = true;
            InPayPara.RegInfo.PatClassID = InPayPara.CommPara.NetworkPatClassId;

            InPayPara.NetworkPatInfo = networkPatInfo;
        }

        /// <summary>
        /// 撤销住院登记
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void CancelInNetworkRegister(InPayParameter para)
        {
            InitNeusoft();
            StructMethod();
            NetworkReadCard();
            InPayPara = para;

            //patSerial = "ZY" +     InPayPara.PatInfo.PatInHosId.ToString() + "_" + InPayPara.RegInfo.RegTimes.ToString();
            patSerial = InPayPara.RegInfo.PatInHosSerial.ToString();
            ReimCancelItems(patSerial);

            string sendSerial = InPayPara.RegInfo.Memo1;
            CancelReimRegister(sendSerial);
            return;
        }
        /// <summary>
        /// 费用明细数据撤销
        /// </summary>
        /// <returns></returns>
        public string ReimCancelItems(string strPatSerial)
        {
            neusoftResolver.InitResolver("2320");
            neusoftResolver.AddInParas(strPatSerial);              //not null 住院流水号(门诊流水号)
            neusoftResolver.AddInParas("");                        //被撤销交易流水号
            neusoftResolver.AddInParas(PayAPIConfig.Operator.UserName);        //not null 经办人
            isType = false;
            try
            {
                var resultArr = Handle();
            }
            catch
            {
            }
            finally
            {

            }
            return "";
        }

        /// <summary>
        /// 撤销挂号/登记
        /// </summary>
        /// <returns></returns>
        public string CancelReimRegister(string HosSendSerial)
        {

            neusoftResolver.InitResolver("2240");
            neusoftResolver.AddInParas(HosSendSerial);        //发送方(医疗机构)撤销交易流水号
            neusoftResolver.AddInParas(PayAPIConfig.Operator.UserName);   //经办员姓名
            isType = false;
            var resultArr = Handle();
            return "";
        }

        /// <summary>
        /// 住院预结算
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void InNetworkPreSettle(InPayParameter para)
        {
            InPayPara = para;

            if (InPayPara.SettleInfo != null && InPayPara.SettleInfo.MedAmountTotal != 0)
            {
                PayType _payType = new PayType();
                _payType.PayTypeId = 4;
                _payType.PayTypeName = "医保";
                _payType.PayAmount = InPayPara.SettleInfo.MedAmountTotal;
                InPayPara.PayTypeList = new List<PayType>();
                InPayPara.PayTypeList.Add(_payType);
                return;
            }

            InitNeusoft();
            StructMethod();
            isOut = false;
            patSerial = InPayPara.RegInfo.PatInHosSerial.ToString();// InPayPara.PatInfo.PatInHosId.ToString() + "_" + InPayPara.RegInfo.RegTimes.ToString();
            settleNo = "ZY" + InPayPara.PatInfo.PatInHosId.ToString("0");
            //settleNo = "ZY" + inReimPara.InDateTime.ToString("yyyyMMddHHmmss");
            OutTime = InPayPara.PatInfo.OutDateTime.ToString("yyyyMMddHHmmss");

            settleTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            ReadMedCard(isOut);
            ReimCancelItems(patSerial);
            InReimUpItems();
            ReimSettle(0);

            InPayPara.SettleInfo = new PayAPIInterface.Model.In.InNetworkSettleMain();

            InPayPara.SettleInfo.MedAmountTotal = Convert.ToDecimal(neusoftResolver.ListOutParas[47]) - Convert.ToDecimal(neusoftResolver.ListOutParas[50]);

            //IInClientBLL inBLl = PayAPIClassLib.Factory.ClientBLLFactory.GetInClientBLLInstance();
            ////获取联网结算ID 并重新组织数据
            //inBLl.GetInSettleIdAndReorganizeData(InPayPara);
            //inBLl.SaveInNetworkSettleMain(InPayPara.SettleInfo);
            //inBLl.UpdateInNetworkRegister(InPayPara.RegInfo);

            //门诊付费方式 本接口 4 医保 6农合
            PayType payType;
            para.PayTypeList = new List<PayType>();
            payType = new PayType();
            payType.PayTypeId = 4;
            payType.PayTypeName = "医保";
            payType.PayAmount = InPayPara.SettleInfo.MedAmountTotal;
            para.PayTypeList.Add(payType);

            //InfoYJSXS yjsxs = new InfoYJSXS(neusoftResolver);
            //yjsxs.ShowDialog();
            #region 显示返回的预结算信息
            string strRePreSettle = "";
            strRePreSettle += "本次医疗费用:" + neusoftResolver.ListOutParas[47] + "\n";
            strRePreSettle += "本次统筹支出(离休基金支出、工伤生育基金支出):" + neusoftResolver.ListOutParas[51] + "\n";
            strRePreSettle += "本次帐户支出:" + neusoftResolver.ListOutParas[49] + "\n";
            strRePreSettle += "本次现金支出:" + neusoftResolver.ListOutParas[50] + "\n";
            strRePreSettle += "本次公务员补助支出:" + neusoftResolver.ListOutParas[53] + "\n";
            strRePreSettle += "本次进入统筹计算费用:" + neusoftResolver.ListOutParas[44] + "\n";
            strRePreSettle += "本次自费金额（丙类）:" + neusoftResolver.ListOutParas[57] + "\n";
            strRePreSettle += "统筹分段自付:" + neusoftResolver.ListOutParas[69] + "\n";
            strRePreSettle += "乙类药品自理:" + neusoftResolver.ListOutParas[58] + "\n";
            strRePreSettle += "乙类特检:" + neusoftResolver.ListOutParas[60] + "\n";
            strRePreSettle += "乙类特治:" + neusoftResolver.ListOutParas[62] + "\n";
            strRePreSettle += "统筹分段1自付:" + neusoftResolver.ListOutParas[78] + "\n";
            strRePreSettle += "统筹分段2自付:" + neusoftResolver.ListOutParas[79] + "\n";
            strRePreSettle += "统筹分段3自付:" + neusoftResolver.ListOutParas[80] + "\n";
            strRePreSettle += "统筹分段4自付:" + neusoftResolver.ListOutParas[81] + "\n";
            strRePreSettle += "统筹分段5自付:" + neusoftResolver.ListOutParas[82] + "\n";
            strRePreSettle += "本次进入大病医疗金额：" + neusoftResolver.ListOutParas[294] + "\n";
            strRePreSettle += "本次大病支出金额：" + neusoftResolver.ListOutParas[287] + "\n";
            MessageBox.Show(strRePreSettle);
            #endregion
            return;
        }

        /// <summary>
        /// 住院费用明细数据上传
        /// </summary>
        /// <returns></returns>
        public void InReimUpItems()
        {

            //如果费用明细里有未和农合对应的情况，则抛出异常终止操作

            string notMatchedCharge = "";
            InPayPara.Details = PayAPIUtilities.Tools.CommonTools.GetGroupList(InPayPara.Details);
            foreach (PayAPIInterface.Model.Comm.FeeDetail feeDetail in InPayPara.Details)
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

            for (int i = 0; i < InPayPara.Details.Count; i++)
            {
                /* 异地需要传总费用
                  Amount += InPayPara.Details[i].Amount;
                 */
                if (Convert.ToDecimal(InPayPara.Details[i].Amount) == 0)
                {
                    InPayPara.Details[i].UploadBackSerial = "";
                    continue;
                }

                if (Convert.ToDecimal(InPayPara.Details[i].Quantity) != 0)
                {
                    InPayPara.Details[i].Price = Convert.ToDecimal(InPayPara.Details[i].Amount) / Convert.ToDecimal(InPayPara.Details[i].Quantity);
                }
                else
                {
                    InPayPara.Details[i].Price = Convert.ToDecimal(InPayPara.Details[i].Amount);
                }

                if (string.IsNullOrEmpty(InPayPara.Details[i].NetworkItemCode.ToString().Trim()))
                {
                    //continue;
                    InPayPara.Details[i].NetworkItemCode = "AAAA";
                    if (Convert.ToInt32(InPayPara.Details[i].ChargeType) < 100)
                    {
                        InPayPara.Details[i].NetworkItemProp = "1";//1药品、2诊疗项目
                    }
                    else
                    {
                        InPayPara.Details[i].NetworkItemProp = "2";//1药品、2诊疗项目
                    }
                    InPayPara.Details[i].NetworkItemClass = "91";//其他费用    
                }

                neusoftResolver.InitResolver("2310");
                neusoftResolver.AddInParas(patSerial);                                                                          //not null 住院流水号(门诊流水号)
                if (InPayPara.Details[i].NetworkItemProp.ToString().Equals("1"))
                {
                    neusoftResolver.AddInParas(InPayPara.Details[i].NetworkItemProp.ToString());                               //not null 项目类别 1.药品 2.诊疗 3.服务设施
                }
                if (InPayPara.Details[i].NetworkItemProp.ToString().Equals("2"))
                {
                    neusoftResolver.AddInParas("2");
                }
                if (InPayPara.Details[i].NetworkItemProp.ToString().Equals("3"))
                {
                    neusoftResolver.AddInParas("3");
                }

                neusoftResolver.AddInParas(InPayPara.Details[i].NetworkItemClass.ToString());                         //not null 费用类别 修改
                string OrderNo = InPayPara.Details[i].AutoId.ToString();
                OrderNo += i.ToString();
                neusoftResolver.AddInParas(OrderNo);                                     //not null 处方号
                neusoftResolver.AddInParas(Convert.ToDateTime(InPayPara.Details[i].CreateTime).ToString("yyyyMMddHHmmss"));   //not null 处方日期
                neusoftResolver.AddInParas(InPayPara.Details[i].ChargeCode.ToString());                                     //not null 医院收费项目内码
                neusoftResolver.AddInParas(InPayPara.Details[i].NetworkItemCode.ToString());                               //收费项目中心编码
                if (InPayPara.Details[i].ChargeName.ToString().Trim().Length > 25)                                //not null 医院收费项目名称
                {
                    neusoftResolver.AddInParas(InPayPara.Details[i].ChargeName.ToString().Substring(0, 25));
                }
                else
                {
                    neusoftResolver.AddInParas(InPayPara.Details[i].ChargeName.ToString().Trim());
                }

                neusoftResolver.AddInParas(InPayPara.Details[i].Price.ToString());                                           //not null 单价
                neusoftResolver.AddInParas(InPayPara.Details[i].Quantity.ToString());                                        //not null 数量
                neusoftResolver.AddInParas(InPayPara.Details[i].DrugFormName.ToString());                                  //剂型
                neusoftResolver.AddInParas(InPayPara.Details[i].Spec.ToString());                                            //规格
                neusoftResolver.AddInParas("");                                                                                //每次用量
                neusoftResolver.AddInParas("");                                                                                //使用频次
                neusoftResolver.AddInParas(InPayPara.Details[i].DocCode.ToString());                                        //医生姓名 修改
                neusoftResolver.AddInParas(InPayPara.Details[i].DocCode.ToString());                                             //处方医师
                neusoftResolver.AddInParas("");                                                                                //用法
                neusoftResolver.AddInParas("");                                                                                //单位
                neusoftResolver.AddInParas(InPayPara.Details[i].DeptCode.ToString());                                       //科别名称 修改
                neusoftResolver.AddInParas("");                                                                                //执行天数
                neusoftResolver.AddInParas("1");                                                                               //草药单复方标志
                neusoftResolver.AddInParas(PayAPIConfig.Operator.UserSysId);                                                    //经办人
                neusoftResolver.AddInParas("");                                                                                //空
                neusoftResolver.AddInParas("");                                                                                //明细扣款金额
                neusoftResolver.AddInParas(Math.Round(Convert.ToDecimal(InPayPara.Details[i].Amount.ToString()), 2).ToString());   //金额 detailTable.Rows[i]["AMOUNT"].ToString()

                //自付比例 修改
                decimal selfRatio = Convert.ToDecimal(InPayPara.Details[i].SelfBurdenRatio.ToString());
                if (selfRatio > 1)
                {
                    selfRatio = selfRatio / 100;
                }

                neusoftResolver.AddInParas(selfRatio.ToString("0.00"));                                                        //自付比例
                neusoftResolver.AddInParas("");                                                                                //记账类别
                isType = false;
                var resultArr = Handle();

                //返回值
                string str1 = resultArr[1];//金额
                string str2 = resultArr[2];//自理金额
                string str3 = resultArr[3];//自费金额
                string str4 = resultArr[4];//收费项目等级
                string str5 = resultArr[5];//全额自费标志

                InPayPara.Details[i].AmountSelf = Convert.ToDecimal(str2);
                InPayPara.Details[i].AmountSelfBurdern = Convert.ToDecimal(str3);
                InPayPara.Details[i].MedAmount = Convert.ToDecimal(str1) - Convert.ToDecimal(str2) - Convert.ToDecimal(str3);
                InPayPara.Details[i].UploadBackSerial = neusoftResolver.ListOutput[0] + "";
                InPayPara.Details[i].Memo1 = str4;
                InPayPara.Details[i].Memo2 = str5;

                InPayPara.Details[i].NetworkSettleId = InPayPara.CommPara.InNetworkSettleId;
            }
            Gs_detailTable = InPayPara.Details;
        }

        /// <summary>
        /// 住院结算
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void InNetworkSettle(InPayParameter para)
        {
            InitNeusoft();

            InPayPara = para;

            if (InPayPara.SettleInfo != null && InPayPara.SettleInfo.MedAmountTotal != 0)
            {
                PayType _payType = new PayType();
                _payType.PayTypeId = 4;
                _payType.PayTypeName = "医保";
                _payType.PayAmount = InPayPara.SettleInfo.MedAmountTotal;
                InPayPara.PayTypeList = new List<PayType>();
                InPayPara.PayTypeList.Add(_payType);
                return;
            }

            isOut = false;
            patSerial = InPayPara.RegInfo.PatInHosSerial; //"ZY" + InPayPara.PatInfo.PatInHosId.ToString() + "_" + InPayPara.RegInfo.RegTimes.ToString();
            //settleNo = DateTime.Now.ToString("yyyyMMddHHmmss");
            settleNo = "ZY" + InPayPara.PatInfo.PatInHosId.ToString() + DateTime.Now.ToString("HHmmss");// +InPayPara.PatInfo.PatInHosId.ToString();
            OutTime = InPayPara.PatInfo.OutDateTime.ToString("yyyyMMddHHmmss");

            //OutReason = "0";
            settleTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            //ReadMedCard(isOut);
            //ReimCancelItems(patSerial);
            //InReimUpItems();
            ReimSettle(1);
            SaveInSettleMain();

            InPayPara.SettleInfo.MedAmountTotal = Convert.ToDecimal(neusoftResolver.ListOutParas[47]) - Convert.ToDecimal(neusoftResolver.ListOutParas[50]);
        }

        /// <summary>
        /// 保存住院结算数据
        /// </summary>
        public void SaveInSettleMain()
        {
            //保存农合中心返回值参数列表
            try
            {
                PayAPIInterface.Model.In.InNetworkSettleList inNetworkSettleList = new PayAPIInterface.Model.In.InNetworkSettleList();
                InPayPara.SettleParaList = new List<PayAPIInterface.Model.In.InNetworkSettleList>();
                inNetworkSettleList.PatInHosId = InPayPara.PatInfo.PatInHosId;
                inNetworkSettleList.InNetworkSettleId = -1;
                inNetworkSettleList.ParaName = "S1";
                inNetworkSettleList.ParaValue = neusoftResolver.ListOutput[0];
                inNetworkSettleList.Memo = "";
                InPayPara.SettleParaList.Add(inNetworkSettleList);

                inNetworkSettleList = new PayAPIInterface.Model.In.InNetworkSettleList();
                inNetworkSettleList.PatInHosId = InPayPara.PatInfo.PatInHosId;
                inNetworkSettleList.InNetworkSettleId = -1;
                inNetworkSettleList.ParaName = "S2";
                inNetworkSettleList.ParaValue = neusoftResolver.ListOutput[1];
                inNetworkSettleList.Memo = "";
                InPayPara.SettleParaList.Add(inNetworkSettleList);

                for (int i = 0; i < neusoftResolver.ListOutParas.Length; i++)
                {
                    inNetworkSettleList = new PayAPIInterface.Model.In.InNetworkSettleList();
                    inNetworkSettleList.PatInHosId = InPayPara.PatInfo.PatInHosId;
                    inNetworkSettleList.InNetworkSettleId = -1;
                    inNetworkSettleList.ParaName = i.ToString();
                    inNetworkSettleList.ParaValue = neusoftResolver.ListOutParas[i].ToString();
                    inNetworkSettleList.Memo = "";
                    InPayPara.SettleParaList.Add(inNetworkSettleList);
                }

            }

            catch (Exception ex)
            {
                //LogManager.RecordException("保存农合中心返回值参数列表 插入值 失败" + ex.Message, "@JSBCInterfaceModel:住院结算");
            }

            InPayPara.SettleInfo = new PayAPIInterface.Model.In.InNetworkSettleMain();
            InPayPara.SettleInfo.PatInHosId = InPayPara.PatInfo.PatInHosId;
            InPayPara.SettleInfo.SettleNo = settleNo;                                        //医保中心交易流水号
            InPayPara.SettleInfo.Amount = Convert.ToDecimal(neusoftResolver.ListOutParas[47]);       //本次医疗费用
            InPayPara.SettleInfo.GetAmount = Convert.ToDecimal(neusoftResolver.ListOutParas[50]);    //本次现金支出
            InPayPara.SettleInfo.MedAmountZhzf = Convert.ToDecimal(neusoftResolver.ListOutParas[49]);//本次帐户支出
            InPayPara.SettleInfo.MedAmountTc = Convert.ToDecimal(neusoftResolver.ListOutParas[51]);  //本次统筹支出
            InPayPara.SettleInfo.MedAmountDb = 0;                                                    //本次大病支出
            InPayPara.SettleInfo.MedAmountBz = Convert.ToDecimal(neusoftResolver.ListOutParas[52]);  //本次民政补助金额
            InPayPara.SettleInfo.MedAmountGwy = Convert.ToDecimal(neusoftResolver.ListOutParas[53]); //本次公务员补助
            InPayPara.SettleInfo.MedAmountJm = Convert.ToDecimal(neusoftResolver.ListOutParas[149]); //帐户余额
            InPayPara.SettleInfo.CreateTime = DateTime.Now;
            InPayPara.SettleInfo.InvoiceId = -1;
            InPayPara.SettleInfo.IsCash = true;
            InPayPara.SettleInfo.IsInvalid = false;
            InPayPara.SettleInfo.IsNeedRefund = false;
            InPayPara.SettleInfo.IsRefundDo = false;
            InPayPara.SettleInfo.IsSettle = true;
            InPayPara.SettleInfo.MedAmountTotal = Convert.ToDecimal(InPayPara.SettleInfo.Amount) - Convert.ToDecimal(InPayPara.SettleInfo.GetAmount);
            InPayPara.SettleInfo.NetworkingPatClassId = Convert.ToInt32(InPayPara.CommPara.NetworkPatClassId);
            InPayPara.SettleInfo.NetworkPatName = networkPatInfo.PatName;
            InPayPara.SettleInfo.NetworkPatType = networkPatInfo.MedicalType; // "0";
            InPayPara.SettleInfo.NetworkRefundTime = Convert.ToDateTime("2000-01-01");
            InPayPara.SettleInfo.NetworkSettleTime = DateTime.Now;
            InPayPara.SettleInfo.OperatorId = PayAPIConfig.Operator.UserSysId;
            InPayPara.SettleInfo.SettleBackNo = NeusoftResolver.BusinessCycleNum;
            InPayPara.SettleInfo.SettleType = strTypeCode;


            //门诊付费方式 本接口 4 医保 6农合
            PayAPIInterface.Model.Comm.PayType payType;
            InPayPara.PayTypeList = new List<PayType>();
            payType = new PayAPIInterface.Model.Comm.PayType();
            payType.PayTypeId = 4;
            payType.PayTypeName = "医保";
            payType.PayAmount = InPayPara.SettleInfo.MedAmountTotal;
            InPayPara.PayTypeList.Add(payType);
        }
        /// <summary>
        /// 撤销住院结算
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void CancelInNetworkSettle(InPayParameter para)
        {
            InitNeusoft();
            isOut = false;
            InPayPara = para;
            NetworkReadCard();
            patSerial = InPayPara.RegInfo.PatInHosSerial;//"ZY" + InPayPara.PatInfo.PatInHosId.ToString() + "_" + InPayPara.RegInfo.RegTimes.ToString();
            settleNo = InPayPara.SettleInfo.SettleNo;
            OutTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            settleTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            strTypeCode = para.RegInfo.NetType;
            //撤销结算
            ReimSettle(2);
            return;
        }
    }
}
