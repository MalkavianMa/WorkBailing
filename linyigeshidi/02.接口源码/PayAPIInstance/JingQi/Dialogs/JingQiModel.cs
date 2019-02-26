using System;
using System.Collections.Generic;
using System.Text;
using PayAPIInterface;
using PayAPIInterface.ParaModel;
using PayAPIInterface.Model.Comm;
using PayAPIInstance.JingQi;
using PayAPIUtilities.Config;
using System.Windows.Forms;
using System.Collections;
using JingQi;


namespace PayAPIInstance.JingQi.Dialogs
{
    class JingQiModel : IPayCompanyInterface
    {
        string[] JS;
        public bool DengJiLeiXing;
        Form2 form2;
        int InpatCount = 0;//住院次数
        InPayParameter InPayPara = new InPayParameter();
        // public List<string> SA = new List<string>();
        NetworkPatInfo networkPatInfo = new NetworkPatInfo();
        Form1 form1;
        //就诊号晶奇唯一标识符
        StringBuilder JZH;
        //创建晶奇dll对象
        public JingQiHandle jingQiHandle = new JingQiHandle();
        //病人信息
        StringBuilder BRXX;
        //字符串分割保存在a3数组里 
        string[] a3;

        /// <summary>
        /// 晶奇初始化
        /// </summary>
        public void init()
        {
            int a = jingQiHandle.Init();
            //判断初始化是否成功 0为成功 -1为失败
            if (a != 0)
            {
                throw new Exception("接口初始化失败");
            }

        }
        /// <summary>
        /// 获取晶奇唯一就诊号
        /// </summary>
        public void getReccode()
        {


            StringBuilder DataBuffer = new StringBuilder(3000);
            int getReccode = jingQiHandle.GetReccode(DataBuffer);
            if (getReccode != 0)
            {
                throw new Exception("接口调用失败,错误提示：" + DataBuffer.ToString());
            }
            else
            {
                //获取病人身份证标识
                JZH = DataBuffer;

            }

        }
        /// <summary>
        /// 获取患者信息
        /// </summary>
        public void getPersonInfo()
        {


            //获取病人身份信息
            StringBuilder DataBuffer1 = new StringBuilder(3000);

            StringBuilder CardID = new StringBuilder(3000);
            CardID.Append("");



            //第一个参数为医生手动输入
            int getpersonInfo = jingQiHandle.getPersonInfo("", DataBuffer1);

            if (getpersonInfo != 0)
            {
                throw new Exception("接口调用失败,错误提示：" + DataBuffer1.ToString());
            }

            //获取病人信息，在窗体展示
            BRXX = DataBuffer1;


            //  textBox_XM.Text = "栾洪众";
            //分割字符串
            a3 = BRXX.ToString().Split('|');
            form1 = new Form1(a3);
            form1.ShowDialog();//开启窗体





            //StringBuilder DataBuffer3 = new StringBuilder(3000);
            //int a = jingQiHandle.getChronicName(a3[1], "", "", "", "", "", DataBuffer3);

            //if (a != 0)
            //{
            //}
            //else
            //{
            //JBMC = DataBuffer3.ToString().Split('|');
            //ArrayList arr2 = new ArrayList();
            //for (int i = 0; i < JBMC.Length; i++)
            //{
            //    arr2.Add(new DictionaryEntry(i, JBMC[i]));
            //}
            //comboBox2.DataSource = arr2;
            //comboBox2.DisplayMember = "Value";
            //comboBox2.ValueMember = "Key";
            //cbType.SelectedIndex = 0; //默认空

            //}



            //  MessageBox.Show(a3[0]);


        }
        /// <summary>
        /// 门诊，住院读卡
        /// </summary>
        /// <returns></returns>
        public NetworkPatInfo NetworkReadCard()
        {
            init();
            getReccode();
            getPersonInfo();
            networkPatInfo.MedicalNo = JZH.ToString();                   //医保个人编号 //新农合就诊号
            networkPatInfo.ICNo = a3[1];                        //新农合个人编号01
            networkPatInfo.PatName = a3[3];                    //姓名
            networkPatInfo.Sex = a3[4];                         //性别
            networkPatInfo.IDNo = a3[6];                        //身份证号码
            networkPatInfo.MedicalType = "新农合";                //医疗人员类别
            networkPatInfo.ICAmount = Convert.ToDecimal(0);    //账户余额
            networkPatInfo.CompanyName = "";                   //单位名称
            networkPatInfo.CompanyNo = "";
            //单位编号
            return networkPatInfo;



        }

        #region 住院
        /// <summary>
        /// 撤销住院登记
        /// </summary>
        /// <param name="para"></param>
        public void CancelInNetworkRegister(InPayParameter para)
        {
            init();
            DengJiLeiXing = false;
            //InNetworkRegister(InPayPara);
            getPersonInfo();
            inputreg();
           
        }
        /// <summary>
        /// 上传住院费用
        /// </summary>
        public void InReimUpItems()
        {

            //如果费用明细里有未和农合对应的情况，则抛出异常终止操作

            string notMatchedCharge = "";
            
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
                if (string.IsNullOrEmpty(InPayPara.Details[i].NetworkItemCode.ToString().Trim()))
                {
                    //continue;
                    InPayPara.Details[i].NetworkItemCode = "";
                    if (Convert.ToInt32(InPayPara.Details[i].ChargeType) < 100)
                    {
                        InPayPara.Details[i].NetworkItemProp = "0";//1药品、2诊疗项目
                    }
                    else
                    {
                        InPayPara.Details[i].NetworkItemProp = "9";//1药品、2诊疗项目
                    }
                    InPayPara.Details[i].NetworkItemClass = "91";//其他费用    
                }

                //出错信息或信息提示
                StringBuilder ErrorMsg = new StringBuilder(3000);


                string ZXBM = InPayPara.Details[i].NetworkItemCode.ToString();
                string ZXMC = InPayPara.Details[i].NetworkItemCode.ToString();
                string YYBM = InPayPara.Details[i].ChargeCode.ToString();
                string YYMC = InPayPara.Details[i].ChargeName.ToString();
                string GG = InPayPara.Details[i].Spec.ToString();
                string JX = InPayPara.Details[i].DrugFormName.ToString();
                double DJ = Convert.ToDouble(InPayPara.Details[i].Amount.ToString())/Convert.ToDouble(InPayPara.Details[i].Quantity.ToString()) ;//= Convert.ToDouble(InPayPara.Details[i].Price.ToString())单价屏蔽
                double SL = Convert.ToDouble(InPayPara.Details[i].Quantity.ToString());
                double JE = Convert.ToDouble(InPayPara.Details[i].Amount.ToString());
                double YCYL = 1;
                string PC = "";
                string YF = "";
                string DJR = InPayPara.PatInfo.DoctorName;
                string DJRQ = Convert.ToString(DateTime.Now.ToString());
                //费用类别（0 西药1成药 2草药 6特殊诊疗材料 9 诊疗项目）此处应该有判断????
                int FYLB = Convert.ToInt32(InPayPara.Details[i].NetworkItemProp.ToString());
                string YYCFH = "";
                string YSMC = InPayPara.PatInfo.DoctorName;
                string SFFF = "1";
                string  sa  = InPayPara.RegInfo.MemberNo;
                int a1 = jingQiHandle.writeFeeDetail(sa, ZXBM, ZXMC, YYBM, YYMC, GG, JX, DJ, SL, JE, YCYL, PC, YF, DJR, DJRQ, FYLB, YYCFH, YSMC, SFFF, ErrorMsg);
                if (a1 != 0)
                {
                    throw new Exception("费用上传失败,错误提示：" + ErrorMsg.ToString());
                }
                







            }
            MessageBox.Show("上传费用成功");
        }
        /// <summary>
        /// 预结算
        /// </summary>
        public void  expensecalc() 
        {

                form2 = new Form2(InPayPara);
                form2.ShowDialog();
                string RecCode = InPayPara.RegInfo.MemberNo;//就诊号
                int InpatType = 2;//就诊类型
                int ExpenseType = Convert.ToInt32(form2.Jslx);//结算类型
                String Operator = PayAPIConfig.Operator.UserName;//登记人
                String ExpenseDate = DateTime.Now.ToString();
                String RegDate = InPayPara.RegInfo.CreateTime.ToString();//入院时间
                String LeaveDate = DateTime.Now.ToString();
                String DiseaseNo1 = InPayPara.RegInfo.NetDiagnosName;//入院主诊断
                String LDiseaseNo1 = form2.Cyzdmc;//出院主诊断
                String BillNo = "12345";//医院单据号
                Double HomePay = 0;//递减金额
                int CalcType = 1;





                StringBuilder DataBuffer = new StringBuilder(3000);
                int a = jingQiHandle.expenseCalc(RecCode, InpatType, ExpenseType, Operator, ExpenseDate, RegDate, LeaveDate, DiseaseNo1, LDiseaseNo1, BillNo, HomePay, CalcType, DataBuffer);
                if (a != 0)
                {
                    throw new Exception("接口调用失败:" + DataBuffer.ToString());
                }

              //  MessageBox.Show(DataBuffer.ToString());
                 JS =  DataBuffer.ToString().Split('|');
                #region 显示返回的预结算信息

                string strRePreSettle = "";
                strRePreSettle += "医疗费总额:" + JS[0] + "\n";
                strRePreSettle += "基金支付:" + JS[1] + "\n";
                strRePreSettle += "现金支付:" + JS[2] + "\n";
                strRePreSettle += "个人支付:" + JS[3] + "\n";
                strRePreSettle += "起付金额:" + JS[4] + "\n";
                strRePreSettle += "帐户支付:" + JS[5] + "\n";
                strRePreSettle += "本本年度基金累计支付:" + JS[6] + "\n";
                strRePreSettle += "本次支付前帐户余额:" + JS[7] + "\n";
                strRePreSettle += "可报销总金额:" + JS[8] + "\n";
                strRePreSettle += "本次支付后帐户余额:" + JS[9] + "\n";
                strRePreSettle += "自费金额:" + JS[10] + "\n";
                strRePreSettle += "自付比例金额:" + JS[11] + "\n";
                strRePreSettle += "中心单据号(报补单号):" + JS[12] + "\n";
                strRePreSettle += "民政救助补偿金额:" + JS[24] + "\n";
                strRePreSettle += "大病保险补偿金额:" + JS[29] + "\n";
                strRePreSettle += "财政兜底补偿金额:" + JS[33] + "\n";
                strRePreSettle += "大病保险补偿金额:" + JS[35] + "\n";
               
                MessageBox.Show(strRePreSettle);
                #endregion



                //住院结算申请
                StringBuilder DataBuffer3 = new StringBuilder(3000);
                int a3 = jingQiHandle.expensereq(InPayPara.RegInfo.MemberNo, Convert.ToInt32(form2.Jslx), DataBuffer3);
                if (a3 != 0)
                {
                    throw new Exception("接口调用失败:" + DataBuffer3.ToString());
                }
                int RegType = 3;//登记类型
               //string RecCode = networkPatInfo.MedicalNo;//就诊号
                string PersonNo = networkPatInfo.ICNo;//个人编号
                //string DiseaseNo1 = form1.JBMC;//入院主诊断
                string DiseaseNo2 = "";//入院次诊断
                string DiseaseNo3 = "";//入院三诊断
               // string RegDate = DateTime.Now.ToString();//入院日期
                string InpatOperator = PayAPIConfig.Operator.UserName;//登记人
              //  string LeaveDate = "";//出院日期
                string LeaveOperator = PayAPIConfig.Operator.UserName;//出院登记人
                string department = InPayPara.PatInfo.InDeptName;//住院科室
                string marriage = "123456789";//电话号码
                int transfer = 0;//是否转院
                string transferNO = "";//当transfer＝1，不能为空
               // string LDiseaseNo1 = "";//出院主诊断
                string LdiseaseNo2 = "";//
                string LdiseaseNo3 = "";//
                string Disease1 = "";//诊断编码
                string Disease2 = "";//
                string Disease3 = "";//
                string LDisease1 = form2.Cyzdbm;//出院诊断编码
                string LDisease2 = "";//
                string LDisease3 = "";//
                string LReason = "康复";//
                string InHosNO = Convert.ToString(InPayPara.PatInfo.InPatId);//
                string BedNO = InPayPara.PatInfo.BedNo;//床位号
                StringBuilder ErrorMsg = new StringBuilder(3000);
                int a1 = jingQiHandle.inpatReg(RegType, RecCode, PersonNo, DiseaseNo1, DiseaseNo2, DiseaseNo3, RegDate, InpatOperator, LeaveDate, LeaveOperator, department, marriage, transfer, transferNO, LDiseaseNo1, LdiseaseNo2, LdiseaseNo3, Disease1, Disease2, Disease3, LDisease1, LDisease2, LDisease3, LReason, InHosNO, BedNO, InpatCount, ErrorMsg);
                if (a1 != 0)
                {
                    throw new Exception("接口调用失败：" + ErrorMsg.ToString());
                }
        }
        /// <summary>
        /// 晶奇登记方法
        /// </summary>
        public void inputreg()
        {
            if (DengJiLeiXing)
            {
                int RegType = Convert.ToInt32(form1.DJLX);//登记类型

                string RecCode = networkPatInfo.MedicalNo;//就诊号
                string PersonNo = networkPatInfo.ICNo;//个人编号
                string DiseaseNo1 = form1.JBMC;//入院主诊断
                string DiseaseNo2 = "";//入院次诊断
                string DiseaseNo3 = "";//入院三诊断
                string RegDate = DateTime.Now.ToString();//入院日期
                string InpatOperator = PayAPIConfig.Operator.UserName;//登记人
                string LeaveDate = "";//出院日期
                string LeaveOperator = "";//出院登记人
                string department = InPayPara.PatInfo.InDeptName;//住院科室
                string marriage = a3[7];//电话号码
                int transfer = 0;//是否转院
                string transferNO = "";//当transfer＝1，不能为空
                string LDiseaseNo1 = "";//出院主诊断
                string LdiseaseNo2 = "";//
                string LdiseaseNo3 = "";//
                string Disease1 = form1.JBBM;//诊断编码
                string Disease2 = "";//
                string Disease3 = "";//
                string LDisease1 = "";//出院诊断编码
                string LDisease2 = "";//
                string LDisease3 = "";//
                string LReason = "";//
                string InHosNO = Convert.ToString(InPayPara.PatInfo.InPatId);//
                string BedNO = InPayPara.PatInfo.BedNo;//床位号
                StringBuilder ErrorMsg = new StringBuilder(3000);
                int a = jingQiHandle.inpatReg(RegType, RecCode, PersonNo, DiseaseNo1, DiseaseNo2, DiseaseNo3, RegDate, InpatOperator, LeaveDate, LeaveOperator, department, marriage, transfer, transferNO, LDiseaseNo1, LdiseaseNo2, LdiseaseNo3, Disease1, Disease2, Disease3, LDisease1, LDisease2, LDisease3, LReason, InHosNO, BedNO, InpatCount, ErrorMsg);
                if (a!=0)
                {
                    throw new Exception("接口调用失败：" + ErrorMsg.ToString());
                }
                MessageBox.Show("住院次数:" + InpatCount );
            }
            else 
            {

                int RegType = Convert.ToInt32(form1.DJLX);//登记类型
                string RecCode =  InPayPara.RegInfo.MemberNo;//就诊号
                string PersonNo =  networkPatInfo.ICNo;//个人编号
                string DiseaseNo1 = form1.JBMC;//入院主诊断
                string DiseaseNo2 = "";//入院次诊断
                string DiseaseNo3 = "";//入院三诊断
                string RegDate = DateTime.Now.ToString();//入院日期
                string InpatOperator = PayAPIConfig.Operator.UserName;//登记人
                string LeaveDate = "";//出院日期
                string LeaveOperator = PayAPIConfig.Operator.UserName;//出院登记人
                string department = "急诊科";//InPayPara.PatInfo.InDeptName;//住院科室
                string marriage = a3[7];//电话号码
                int transfer = 0;//是否转院
                string transferNO = "";//当transfer＝1，不能为空
                string LDiseaseNo1 = "";//出院主诊断
                string LdiseaseNo2 = "";//
                string LdiseaseNo3 = "";//
                string Disease1 = form1.JBBM;//诊断编码
                string Disease2 = "";//
                string Disease3 = "";//
                string LDisease1 = "";//出院诊断编码
                string LDisease2 = "";//
                string LDisease3 = "";//
                string LReason = "";//
                string InHosNO =  Convert.ToString(InPayPara.PatInfo.InPatId);//
                string BedNO =  InPayPara.PatInfo.BedNo;//床位号
                StringBuilder ErrorMsg = new StringBuilder(3000);
                int a = jingQiHandle.inpatReg(RegType, RecCode, PersonNo, DiseaseNo1, DiseaseNo2, DiseaseNo3, RegDate, InpatOperator, LeaveDate, LeaveOperator, department, marriage, transfer, transferNO, LDiseaseNo1, LdiseaseNo2, LdiseaseNo3, Disease1, Disease2, Disease3, LDisease1, LDisease2, LDisease3, LReason, InHosNO, BedNO, InpatCount, ErrorMsg);
                if (a!=0) 
                {
                    throw new Exception("接口调用失败：" + ErrorMsg.ToString());
                }
            }
            
           
        }
        /// <summary>
        /// 住院预结算包括住院上传费用
        /// </summary>
        /// <param name="para"></param>
        public void InNetworkPreSettle(InPayParameter para)

            
        {

           InPayPara = para;
              init();
            StringBuilder b1 =new StringBuilder(3000);
            //删除费用上传
            int a = jingQiHandle.cancelFee(InPayPara.RegInfo.MemberNo, 1, b1);
            
            //上传费用
            InReimUpItems();
            //预结算
            expensecalc(); 


            
        }
        /// <summary>
        /// 联网住院登记
        /// </summary>
        /// <param name="para"></param>
        public void InNetworkRegister(InPayParameter para)
        {
            DengJiLeiXing = true;
            //1712767
            InPayPara = para;
            NetworkReadCard();
            inputreg();
            InPayPara.RegInfo.NetDiagnosName = form1.JBMC;//疾病名称
            InPayPara.RegInfo.NetDiagnosCode = form1.JBBM;//疾病编码
            InPayPara.RegInfo.CardNo = networkPatInfo.ICNo; 
            InPayPara.RegInfo.MemberNo = networkPatInfo.MedicalNo;//新农合唯一就诊号
            InPayPara.RegInfo.CantonCode = "1";
            InPayPara.RegInfo.PatAddress = networkPatInfo.CompanyName;
            InPayPara.RegInfo.CompanyName = networkPatInfo.CompanyName;
            InPayPara.RegInfo.NetPatType = networkPatInfo.MedicalType;
            InPayPara.RegInfo.IdNo = networkPatInfo.IDNo;
            //联网类型 住院门诊 生育住院
            InPayPara.RegInfo.NetType = form1.DJLX;
            InPayPara.RegInfo.NetPatName = networkPatInfo.PatName;
            InPayPara.RegInfo.PatClassID = "10002";
            //住院登记流水号
            InPayPara.RegInfo.PatInHosSerial = "";
            InPayPara.RegInfo.OperatorId = PayAPIConfig.Operator.UserSysId;
            //
            InPayPara.RegInfo.PatInHosId = InPayPara.PatInfo.PatInHosId;
            InPayPara.RegInfo.CreateTime = DateTime.Now;
            InPayPara.RegInfo.NetRegSerial = "";
            InPayPara.RegInfo.RegTimes = 1;
            InPayPara.RegInfo.Memo1 = "";
            InPayPara.RegInfo.Memo2 = "";
            InPayPara.RegInfo.OutDiagnoseCode = "";
            InPayPara.RegInfo.OutDiagnoseName = "";
            InPayPara.RegInfo.Memo2 = "";
          //  InPayPara.RegInfo.InNetworkSettleId = 12;
            


            //InPayPara.PatOutStatusID = Convert.ToInt32(infoForm.OutHosStatus);//出院状态

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
               

                for (int i = 0; i < JS.Length; i++)
                {
                    inNetworkSettleList = new PayAPIInterface.Model.In.InNetworkSettleList();
                    inNetworkSettleList.PatInHosId = InPayPara.PatInfo.PatInHosId;
                    inNetworkSettleList.InNetworkSettleId = -1;
                    inNetworkSettleList.ParaName = i.ToString();
                    inNetworkSettleList.ParaValue = JS[i];
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
            InPayPara.SettleInfo.SettleNo = InPayPara.RegInfo.MemberNo;                                        //医保中心交易流水号//农合就诊证号
            InPayPara.SettleInfo.Amount = Convert.ToDecimal(JS[0].ToString());       //本次医疗费用
            InPayPara.SettleInfo.GetAmount = Convert.ToDecimal(JS[2].ToString());    //本次现金支出
            InPayPara.SettleInfo.MedAmountZhzf = Convert.ToDecimal(JS[5].ToString());//本次帐户支出
            InPayPara.SettleInfo.MedAmountTc = Convert.ToDecimal(JS[1].ToString());  //本次统筹支出
            InPayPara.SettleInfo.MedAmountDb = 0;                                            //本次大病支出
            InPayPara.SettleInfo.MedAmountBz = Convert.ToDecimal(JS[24].ToString());  //本次民政补助金额
            InPayPara.SettleInfo.MedAmountGwy = Convert.ToDecimal(0); //本次公务员补助
            InPayPara.SettleInfo.CreateTime = DateTime.Now;
            InPayPara.SettleInfo.InvoiceId = -1;
            InPayPara.SettleInfo.IsCash = true;
            InPayPara.SettleInfo.IsInvalid = false;
            InPayPara.SettleInfo.IsNeedRefund = false;
            InPayPara.SettleInfo.IsRefundDo = false;
            InPayPara.SettleInfo.IsSettle = true;     //总费用                                                         //基金统筹                            //民政救助                           //   大病保险                          //180补偿金额
            InPayPara.SettleInfo.MedAmountTotal = Convert.ToDecimal(InPayPara.SettleInfo.Amount) - Convert.ToDecimal(InPayPara.SettleInfo.GetAmount) - Convert.ToDecimal(JS[24].ToString()) - Convert.ToDecimal(JS[29].ToString()) - Convert.ToDecimal(JS[33].ToString()) - Convert.ToDecimal(JS[35].ToString());
            InPayPara.SettleInfo.NetworkingPatClassId = Convert.ToInt32("10002");
            InPayPara.SettleInfo.NetworkPatName = networkPatInfo.PatName;
            InPayPara.SettleInfo.NetworkPatType = "0";
            InPayPara.SettleInfo.NetworkRefundTime = Convert.ToDateTime("2000-01-01");
            InPayPara.SettleInfo.NetworkSettleTime = DateTime.Now;
            InPayPara.SettleInfo.OperatorId = PayAPIConfig.Operator.UserSysId;
            InPayPara.SettleInfo.SettleBackNo = "";
            InPayPara.SettleInfo.SettleType = JS[12];//中心单据号，报补单号
            InPayPara.SettleInfo.NetworkPatName = "";

            
      
            //门诊付费方式 本接口 4 医保 5  农合
            PayAPIInterface.Model.Comm.PayType payType;
            InPayPara.PayTypeList = new List<PayType>();
         


            payType = new PayAPIInterface.Model.Comm.PayType();
            payType.PayTypeId = 5;
           // payType.PayAmount = Convert.ToDecimal(neusoftResolver.ListOutParas[49]);
            InPayPara.PayTypeList.Add(payType);

        }

        /// <summary>
        /// 住院结算
        /// </summary>
        /// <param name="para"></param>
        public void InNetworkSettle(InPayParameter para)
        {
              InPayPara =para;

              if (MessageBox.Show("请确认是否已经审批", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
              {
                  throw new Exception("取消结算，如果要结算请重新进行下预结算");
              }
            string RecCode = InPayPara.RegInfo.MemberNo;//就诊号
            int InpatType = 2;//就诊类型
            int ExpenseType = Convert.ToInt32(form2.Jslx);//结算类型
            String Operator = PayAPIConfig.Operator.UserName;//登记人
            String ExpenseDate = DateTime.Now.ToString();
            String RegDate = InPayPara.RegInfo.CreateTime.ToString();//入院时间
            String LeaveDate = DateTime.Now.ToString();
            String DiseaseNo1 = InPayPara.RegInfo.NetDiagnosName;//入院主诊断
            String LDiseaseNo1 = form2.Cyzdmc;//出院主诊断
            String BillNo = Convert.ToString( InPayPara.RegInfo.PatInHosId);//医院单据号
            Double HomePay = 0;//递减金额
            int CalcType = 2;





            StringBuilder DataBuffer1 = new StringBuilder(3000);
            int a1 = jingQiHandle.expenseCalc(RecCode, InpatType, ExpenseType, Operator, ExpenseDate, RegDate, LeaveDate, DiseaseNo1, LDiseaseNo1, BillNo, HomePay, CalcType, DataBuffer1);
            if (a1 != 0)
            {
                throw new Exception("接口调用失败:" + DataBuffer1.ToString());
            }
            JS = DataBuffer1.ToString().Split('|');

            MessageBox.Show("报补单号:"+JS[12]);
            SaveInSettleMain();
           
        }
        /// <summary>
        /// 撤销住院结算
        /// </summary>
        /// <param name="para"></param>
        public void CancelInNetworkSettle(InPayParameter para)
        {
            InPayPara = para;
            StringBuilder DataBuffer1 = new StringBuilder(3000);
            int a = jingQiHandle.cancelSettleFee(para.RegInfo.MemberNo, InPayPara.SettleInfo.SettleType, 1, PayAPIConfig.Operator.UserName, DataBuffer1);
            if (a != 0)
            {
                throw new Exception("接口调用失败:" + DataBuffer1.ToString());
            }
        }
        #endregion


     

        public void CancelOutNetworkSettle(OutPayParameter para)
        {
            throw new NotImplementedException();
        }





        public void OutNetworkPreSettle(OutPayParameter para)
        {
            throw new NotImplementedException();
        }

        public void OutNetworkRegister(OutPayParameter para)
        {
            throw new NotImplementedException();
        }

        public void OutNetworkSettle(OutPayParameter para)
        {
            throw new NotImplementedException();
        }
    }
}
