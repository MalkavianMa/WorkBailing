using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using PayAPIInstance.Dareway.ShanDong.Dialog;
using PayAPIInterface;
using PayAPIInterface.Model.Comm;
using PayAPIUtilities.Config;
using PayAPIInterface.ParaModel;
using PayAPIInterface.Model.Out;
using PayAPIInterface.Model.In;
using PayAPIUtilities.Log;
using PayAPIInterfaceHandle.Dareway.ShanDong;
using System.Data;

namespace PayAPIInstance.Dareway.ShanDong
{
    public class DarewayModel : IPayCompanyInterface
    {
        public MSSQLHelper sqlHelperHis = new MSSQLHelper("Data Source=172.22.29.6;Initial Catalog=COMM;Persist Security Info=True;User ID=power;Password=m@ssunsoft009");
        //业务处理
        public DarewayHandle handelModel;
        //结算信息
        public Dictionary<string, string> dicSettleInfo = new Dictionary<string, string>();
        //患者信息
        Dictionary<string, string> patInfo = new Dictionary<string, string>();
        public bool isInit = false;  //是否初始化
        public bool isOut = false;                                //是否门诊

        public NetworkPatInfo networkPatInfo = new NetworkPatInfo();
        OutNetworkRegisters outnetworkregisters = new OutNetworkRegisters();
        InNetworkRegisters inNetworkRegisters = new InNetworkRegisters();

        public OutPayParameter OutPayPara = new OutPayParameter();
        public InPayParameter InPayPara = new InPayParameter();   

        

        public string strDiagnosCode = "";//疾病编码
        public string strDiagnosName = "";//疾病名称
        public string p_yltclb = "0"; //医疗统筹类别
        public string p_sbjbm = "379902"; //社保局编码,山东省直为379902
        /// <summary>
        ///  *使用账户类型;0 不使用,1银行卡,2 cpu 卡，3 联机卡
        /// </summary>
        public string p_syzhlx = "3";

        /// <summary>
        /// 初始化
        /// </summary>
        public void InterfaceInit()
        {
            //if (!isInit) 
            //{
            handelModel = new DarewayHandle();
            isInit = true;
            //}
        }

        /// <summary>
        /// 框架调用的读卡
        /// </summary>
        /// <returns></returns>
        public NetworkPatInfo NetworkReadCard()
        {

            InterfaceInit();
            patInfo = handelModel.ReadCard("0", "");  
            networkPatInfo.MedicalNo = patInfo["shbzhm"];  //个人编号
            networkPatInfo.ICNo = patInfo["ylzbh"];     //医疗卡号
            networkPatInfo.PatName = patInfo["xm"];  //姓名
            networkPatInfo.Sex = patInfo["xb"] == "1" ? "男" : "女";  //性别
            networkPatInfo.IDNo = "";        //身份证号码
            networkPatInfo.Birthday = DateTime.ParseExact(patInfo["csrq"], "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);//出生日期
            networkPatInfo.Age = DateTime.Now.Year - networkPatInfo.Birthday.Year;//年龄
            networkPatInfo.MedicalType = patInfo["sbjglx"];       //医疗人员类别//人员属性
            networkPatInfo.ICAmount =Convert.ToDecimal(patInfo["ye"]);                                            //账户余额
            networkPatInfo.CompanyName = patInfo["dwmc"];//单位名称
            networkPatInfo.CompanyNo = "";                                         //单位编号
            return networkPatInfo;
        }

        /// <summary>
        /// 自定义的根据p_yltclb进行读卡
        /// </summary>
        /// <param name="p_yltclb"></param>
        /// <returns></returns>
        public NetworkPatInfo ReadCard()
        {

            
         //弹出窗口选择统筹类别
            Dialog.frmCARD frmCard = new Dialog.frmCARD(this);
            frmCard.ShowDialog();
            if (!frmCard.isOk)
                throw new Exception("操作取消");
            p_yltclb = frmCard.p_yltclb;
            InterfaceInit();
            patInfo = handelModel.ReadCard(p_yltclb, "");
            networkPatInfo.MedicalNo = patInfo["shbzhm"];  //个人编号
            networkPatInfo.ICNo = patInfo["ylzbh"];     //医疗卡号
            networkPatInfo.PatName = patInfo["xm"];  //姓名
            networkPatInfo.Sex = patInfo["xb"] == "1" ? "男" : "女";  //性别
            networkPatInfo.IDNo = "";        //身份证号码
            networkPatInfo.Birthday = DateTime.ParseExact(patInfo["csrq"], "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);//出生日期
            networkPatInfo.Age = DateTime.Now.Year - networkPatInfo.Birthday.Year;//年龄
            networkPatInfo.MedicalType = patInfo["sbjglx"];       //医疗人员类别//人员属性
            networkPatInfo.ICAmount = Convert.ToDecimal(patInfo["ye"]);                                            //账户余额
            networkPatInfo.CompanyName = patInfo["dwmc"];//单位名称
            networkPatInfo.CompanyNo = "";        //单位编号
            return networkPatInfo;
        }

        public void OutNetworkRegister(OutPayParameter para)
        {
            OutPayPara = para;
            isOut = true;
            ReadCard(); //读卡
            Dialog.PersonInfoDialog perDialog = new Dialog.PersonInfoDialog(patInfo);
            perDialog.ShowDialog();
            if (perDialog.isCancel)
            {
                throw new Exception("取消操作");
            }
            strDiagnosCode = perDialog.strDiagnosCode;
            strDiagnosName = perDialog.strDiagnosName;
            string HisPatName= OutPayPara.PatInfo.PatName;
            OutPayPara.RegInfo = new PayAPIInterface.Model.Out.OutNetworkRegisters
            {
                NetPatName = networkPatInfo.PatName,
                Balance = networkPatInfo.ICAmount,
                CardNo = networkPatInfo.ICNo,
                MemberNo = networkPatInfo.MedicalNo,
                NetType = p_yltclb,
                CantonCode = patInfo["cbdsbh"],
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
                NetDiagnosCode = strDiagnosCode,
                NetDiagnosName = strDiagnosName,
                IsInvalid = true,
                IsReg = true,
                OutPatId = OutPayPara.PatInfo.OutPatId,
                PatSerial = ""
            };
            if (networkPatInfo.PatName.Trim() != HisPatName.Trim())
            {
                throw new Exception("HIS登记姓名:" + HisPatName.Trim() + ",医保读卡姓名：" + networkPatInfo.PatName.Trim() + ",两者不一致，请核对！");
            }

        }

        public void OutNetworkPreSettle(OutPayParameter para)
        {
            OutPayPara = para;
            InterfaceInit();
            handelModel.InitMZ(p_sbjbm, OutPayPara.RegInfo.NetType, OutPayPara.RegInfo.MemberNo, OutPayPara.RegInfo.NetPatName,
                                patInfo["xb"], OutPayPara.CommPara.OutNetworkSettleId.ToString(), DateTime.Now.ToString("yyyy-MM-dd"),
                                getMzysbh(), strDiagnosCode, p_syzhlx, OutPayPara.RegInfo.CardNo, "C", "");  //4 门诊大病，5 意外伤害，6普通门诊
            handelModel.SaveOutItems(OutPayPara.Details);
        }

        public void OutNetworkSettle(OutPayParameter para)
        {
            OutPayPara = para;
            //门诊结算
            dicSettleInfo = handelModel.SettleMZ(false);
            //保存门诊结算明细
            SaveOutSettleMain();

            handelModel.PrintDJ(dicSettleInfo["brjsh"], "FP");
            isInit = false;
            GC.Collect();
            //释放对象
            handelModel.ReleaseComObj();
        }

        public void SaveOutSettleMain()
        {

            #region 保存中心返回值参数列表
            //保存中心返回值参数列表
            try
            {
                OutPayPara.SettleParaList = new List<PayAPIInterface.Model.Out.OutNetworkSettleList>();
                PayAPIInterface.Model.Out.OutNetworkSettleList outNetworkSettleList;
                foreach (var item in dicSettleInfo)
                {
                    outNetworkSettleList = new PayAPIInterface.Model.Out.OutNetworkSettleList();
                    outNetworkSettleList.OutPatId = OutPayPara.PatInfo.OutPatId;
                    outNetworkSettleList.OutNetworkSettleId = OutPayPara.CommPara.OutNetworkSettleId;
                    outNetworkSettleList.ParaName = item.Key.ToString();
                    outNetworkSettleList.ParaValue = item.Value.ToString();
                    outNetworkSettleList.Memo = "";
                    OutPayPara.SettleParaList.Add(outNetworkSettleList);
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("门诊结算保存中心返回值参数列表 插入值 失败" + ex.Message);
            }
            #endregion

            OutPayPara.SettleInfo = new PayAPIInterface.Model.Out.OutNetworkSettleMain();
            OutPayPara.SettleInfo.OutPatId = OutPayPara.PatInfo.OutPatId;
            OutPayPara.SettleInfo.SettleNo = dicSettleInfo["brjsh"];
            OutPayPara.SettleInfo.Amount = Convert.ToDecimal(dicSettleInfo["zje"]);       //本次医疗费用
            OutPayPara.SettleInfo.GetAmount = Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]);    //本次现金支出
            OutPayPara.SettleInfo.MedAmountZhzf = Convert.ToDecimal(dicSettleInfo["grzhzf"]);//本次帐户支出
            OutPayPara.SettleInfo.MedAmountTc = Convert.ToDecimal(dicSettleInfo["ybfdje"]);  //本次统筹支出
            OutPayPara.SettleInfo.MedAmountDb = Convert.ToDecimal(dicSettleInfo["dezf"]); //本次大额支出
            OutPayPara.SettleInfo.MedAmountBz = Convert.ToDecimal(dicSettleInfo["ylbzje"]);  //医疗补助支付
            OutPayPara.SettleInfo.MedAmountGwy = 0; //公务员补助
            OutPayPara.SettleInfo.MedAmountJm = Convert.ToDecimal(dicSettleInfo["yljmje"]); //医疗减免支付
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
            OutPayPara.SettleInfo.SettleBackNo = OutPayPara.RegInfo.NetPatType;
            OutPayPara.SettleInfo.SettleType = OutPayPara.RegInfo.NetType;

            //门诊付费方式 本接口 4 医保 6农合
            PayAPIInterface.Model.Comm.PayType payType = new PayAPIInterface.Model.Comm.PayType();
            payType.PayTypeId = 4;
            payType.PayTypeName = "医保";
            payType.PayAmount = Convert.ToDecimal(OutPayPara.SettleInfo.MedAmountTotal) ;
            OutPayPara.PayTypeList = new List<PayType>();
            OutPayPara.PayTypeList.Add(payType);

        }


        public void CancelOutNetworkSettle(OutPayParameter para)
        {
            OutPayPara = para;
            InterfaceInit();

            //撤销医保结算
            
            while (true)
            {
                try
                {
                    handelModel.CancelMZSettle(OutPayPara.SettleInfo.SettleNo);
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


            isInit = false;
            GC.Collect();
            //释放对象
            handelModel.ReleaseComObj();
        }


        public void InNetworkRegister(InPayParameter para)
        {
            InPayPara = para;
            isOut = false;
            ReadCard(); //读卡
            Dialog.PersonInfoDialog perDialog = new Dialog.PersonInfoDialog(patInfo);
            perDialog.ShowDialog();
            if (perDialog.isCancel)
            {
                throw new Exception("取消操作");
            }

            string HisPatName = InPayPara.PatInfo.InPatName;
            InPayPara.RegInfo.Memo1 = networkPatInfo.ICAmount.ToString();
            InPayPara.RegInfo.CardNo = networkPatInfo.ICNo;
            InPayPara.RegInfo.MemberNo = networkPatInfo.MedicalNo;
            InPayPara.RegInfo.CantonCode = patInfo["cbdsbh"];
            InPayPara.RegInfo.PatAddress = networkPatInfo.CompanyName;
            InPayPara.RegInfo.CompanyName = networkPatInfo.CompanyName;
            InPayPara.RegInfo.NetPatType = networkPatInfo.MedicalType;
            InPayPara.RegInfo.NetDiagnosCode = "";
            InPayPara.RegInfo.NetDiagnosName = "";
            InPayPara.RegInfo.IdNo = networkPatInfo.IDNo;
            InPayPara.RegInfo.NetType = p_yltclb;
            InPayPara.RegInfo.NetPatName = networkPatInfo.PatName;
            InPayPara.RegInfo.PatClassID = "-1";
            InPayPara.RegInfo.PatInHosSerial = InPayPara.PatInfo.PatInHosCode;
            InPayPara.RegInfo.OperatorId = PayAPIConfig.Operator.UserSysId;

            if (networkPatInfo.PatName.Trim() != HisPatName.Trim())
            {
                throw new Exception("HIS登记姓名:" + HisPatName.Trim() + ",医保读卡姓名：" + networkPatInfo.PatName.Trim() + ",两者不一致，请核对！");
            }


            try
            {
                handelModel.SaveZYDJ(InPayPara.PatInfo.PatInHosCode,
                                    patInfo["shbzhm"],
                                    patInfo["ylzbh"],
                                    patInfo["xm"],
                                    patInfo["xb"],
                                    p_yltclb,
                                    patInfo["sbjbm"],
                                    p_syzhlx,
                                    InPayPara.PatInfo.InDeptCode.ToString(),   //需修改//varchar2(20)       *科室编码
                                    InPayPara.PatInfo.InDateTime.ToString("yyyy-MM-dd"),
                                    "",
                                    "",
                                    "1", //*住院方式（‘1’普通住院，‘6’市内转院）
                                    "C",
                                    ""
                                    );
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.InnerException.Message);
                throw ex;
            }
            isInit = false;
        }

        public void CancelInNetworkRegister(InPayParameter para)
        {
            InPayPara = para;
            InterfaceInit();
            handelModel.CancelZY(InPayPara.PatInfo.PatInHosCode);
            isInit = false;
        }



        public void InNetworkPreSettle(InPayParameter para)
        {
            InPayPara = para;


            //在 住院预结算 和 住院结算的时候 在函数最开始   加入一下判断
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



            if (InPayPara.RegInfo.MemberNo == null)
            {
                throw new Exception("没有找到联网登记信息，请确认是否联网登记！");
            }

            InterfaceInit();
            InReimUpItems();
            dicSettleInfo = handelModel.SettleZY();

            SaveInSettleMain();

            
            isInit = false;
        }

        /// <summary>
        /// 删除费用
        /// </summary>
        public void ReimCancelItems()
        {
            try
            {
                InterfaceInit();
                handelModel.DelAllInItems(InPayPara.PatInfo.PatInHosCode);
                isInit = false;
            }
            catch (Exception ex)
            {
                LogManager.Error("删除所有凭单失败！:" + ex.Message);
            }
            finally
            {

            }
        }

        /// <summary>
        /// 住院费用上传
        /// </summary>
        public void InReimUpItems()
        {

            string notMatchedCharge = "";
            //InPayPara.Details = PayAPIUtilities.Tools.CommonTools.GetGroupList(InPayPara.Details); //传汇总后项目
            foreach (PayAPIInterface.Model.Comm.FeeDetail feeDetail in InPayPara.Details)
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

            for (int i = 0; i < InPayPara.Details.Count; i++)
            {
                if (InPayPara.Details[i].ChargeType < 100)
                {
                    InPayPara.Details[i].Memo4 = "药品";//药品
                    //InPayPara.Details[i].NetworkItemCode = "963";
                }
                else
                {
                    InPayPara.Details[i].Memo4 = "医疗";//医疗
                    //InPayPara.Details[i].NetworkItemCode = "5455";
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

        public void SaveInSettleMain()
        {
            try
            {
   
                InNetworkSettleList inNetworkSettleList = new InNetworkSettleList();
                foreach (var item in dicSettleInfo)
                {
                    inNetworkSettleList = new InNetworkSettleList();
                    inNetworkSettleList.PatInHosId = InPayPara.PatInfo.PatInHosId;
                    inNetworkSettleList.InNetworkSettleId = -1;
                    inNetworkSettleList.ParaName = item.Key;
                    inNetworkSettleList.ParaValue = item.Value.ToString();
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
            InPayPara.SettleInfo.SettleNo = dicSettleInfo["brjsh"];                                       //医保中心交易流水号
            InPayPara.SettleInfo.Amount = Convert.ToDecimal(dicSettleInfo["brfdje"]) + Convert.ToDecimal(dicSettleInfo["ybfdje"]) + Convert.ToDecimal(dicSettleInfo["ylbzje"]) + Convert.ToDecimal(dicSettleInfo["yyfdje"]);       //本次医疗费用
            InPayPara.SettleInfo.GetAmount = Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]);    //本次现金支出
            InPayPara.SettleInfo.MedAmountZhzf = Convert.ToDecimal(dicSettleInfo["grzhzf"]);//本次帐户支出
            InPayPara.SettleInfo.MedAmountTc = Convert.ToDecimal(dicSettleInfo["ybfdje"]);  //本次统筹支出
            InPayPara.SettleInfo.MedAmountDb =0;   //大病救助
            InPayPara.SettleInfo.MedAmountBz = Convert.ToDecimal(dicSettleInfo["ylbzje"]);  //医疗补助金额(优抚对象补助)
            InPayPara.SettleInfo.MedAmountGwy =0;  //公务员补助
            InPayPara.SettleInfo.MedAmountJm = Convert.ToDecimal(dicSettleInfo["yyfdje"]); //医院负担金额
            InPayPara.SettleInfo.CreateTime = DateTime.Now;
            InPayPara.SettleInfo.InvoiceId = -1;
            InPayPara.SettleInfo.IsCash = true;
            InPayPara.SettleInfo.IsInvalid = false;
            InPayPara.SettleInfo.IsNeedRefund = false;
            InPayPara.SettleInfo.IsRefundDo = false;
            InPayPara.SettleInfo.IsSettle = true;
            InPayPara.SettleInfo.MedAmountTotal = Convert.ToDecimal(InPayPara.SettleInfo.Amount) - Convert.ToDecimal(InPayPara.SettleInfo.GetAmount);
            InPayPara.SettleInfo.NetworkingPatClassId = Convert.ToInt32(InPayPara.CommPara.NetworkPatClassId);
            InPayPara.SettleInfo.NetworkPatName = InPayPara.RegInfo.NetPatName;
            InPayPara.SettleInfo.NetworkPatType = InPayPara.RegInfo.NetPatType; // "0";
            InPayPara.SettleInfo.NetworkRefundTime = Convert.ToDateTime("2000-01-01");
            InPayPara.SettleInfo.NetworkSettleTime = DateTime.Now;
            InPayPara.SettleInfo.OperatorId = PayAPIConfig.Operator.UserSysId;
            InPayPara.SettleInfo.SettleBackNo = InPayPara.RegInfo.NetPatType;
            InPayPara.SettleInfo.SettleType = InPayPara.RegInfo.NetType;

            InPayPara.RegInfo.OutDiagnoseCode = "";//出院诊断编号
            InPayPara.RegInfo.OutDiagnoseName = "";//出院诊断名称

            //门诊付费方式 本接口 4 医保 6农合
            PayAPIInterface.Model.Comm.PayType payType;
            InPayPara.PayTypeList = new List<PayType>();
            payType = new PayAPIInterface.Model.Comm.PayType();
            payType.PayTypeId = 4;
            payType.PayTypeName = "医保";
            payType.PayAmount = InPayPara.SettleInfo.MedAmountTotal;
            InPayPara.PayTypeList.Add(payType);
        }

        public void InNetworkSettle(InPayParameter para)
        {
            InPayPara = para;
            //在 住院预结算 和 住院结算的时候 在函数最开始   加入一下判断
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
            else
            {
                SaveInSettleMain();
            }
        }


        public void CancelInNetworkSettle(InPayParameter para)
        {
            InterfaceInit();
            //初始化
            InPayPara = para;
            handelModel.CancleZYSettle(InPayPara.RegInfo.PatInHosSerial, InPayPara.SettleInfo.SettleNo);
        }

        private string getMzysbh()
        {
            DataSet ds = new DataSet();
            string strSql = "";
            //------------------------------------------------------------------------------------------------
            try
            {
                string strTradeID = "";
                strSql = "SELECT * FROM MZ.OUT.TRADE_ORDER WHERE TRADE_ID='" + OutPayPara.CommPara.TradeId + "'";
                ds = sqlHelperHis.ExecSqlReDs(strSql);
                if (ds.Tables[0].Rows[0]["TRADE_BACK_ID"].ToString() != "-1")//余额发票订单需要找到原始订单号
                {
                    do
                    {
                        strSql = "SELECT * FROM MZ.OUT.TRADE_ORDER WHERE TRADE_ID='" + ds.Tables[0].Rows[0]["TRADE_BACK_ID"].ToString() + "'";
                        ds = sqlHelperHis.ExecSqlReDs(strSql);
                    } while (ds.Tables[0].Rows[0]["TRADE_BACK_ID"].ToString() != "-1");
                    strTradeID = ds.Tables[0].Rows[0]["TRADE_ID"].ToString();//原始订单号


                }
                else //正常收款订单
                {
                    strTradeID = OutPayPara.CommPara.TradeId.ToString();
                }
                strSql = @"SELECT BILL_DEPT_ID,b.DEPT_CODE,b.DEPT_NAME ,a.DOC_SYS_ID,c.USER_CODE AS DocCode,c.UESR_NAME AS DocName,f.DIAGNOSIS_CODE,f.DIAGNOSIS_NAME FROM MZ.OUT.TRADE_OUT_ORDER_CHARGE_TMP a 
                           LEFT JOIN COMM.COMM.DEPTS b ON a.BILL_DEPT_ID=b.DEPT_ID 
                           LEFT JOIN COMM.COMM.USERINFO_VIEW c ON c.USER_SYS_ID=a.DOC_SYS_ID
                           LEFT JOIN MZ.OUT.REGISTERS d ON a.REGISTER_ID=d.REGISTER_ID AND a.PAT_ID=d.OUT_PAT_ID 
                           LEFT JOIN MZ.OUT.OUT_PAT_DIAGNOSE e ON d.REGISTER_ID=e.REGISTER_ID 
                           LEFT JOIN COMM.DICT.DIAGNOSIS f ON e.DIAGNOSE_ID_1=f.DIAGNOSIS_ID 
                           WHERE PAT_ID='" + OutPayPara.PatInfo.OutPatId + "' AND a.TRADE_ID='" + strTradeID + "'  ORDER BY f.DIAGNOSIS_CODE DESC, a.CREATE_TIME";

                ds = sqlHelperHis.ExecSqlReDs(strSql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0]["DocCode"].ToString();

                }
                else
                {
                    return "001";
                }
                
            }
            catch (Exception ex)
            {
                return "001";
            }
        }


    }
}
