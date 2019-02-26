using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Reflection;
using PayAPIInstance.Dareway.Neusoft.Dialog;
using PayAPIInterface;
using PayAPIInterface.Model.Comm;
using PayAPIUtilities.Config;
using PayAPIInterface.ParaModel;
using PayAPIInterface.Model.Out;
using PayAPIInterface.Model.In;
using PayAPIUtilities.Log;
using PayAPIInterfaceHandle.Neusoft;


namespace PayAPIInstance.Dareway.Neusoft
{
    public class NeusoftInterfaceModel : IPayCompanyInterface
    {
        #region 参数
        //兰陵// 
        // MSSQLHelper _mssqlHelpers = new MSSQLHelper("Data Source=11.104.0.183;Initial Catalog=comm;Persist Security Info=True;User ID=power;Password=massunsoft009");
        //MSSQLHelper _mssqlHelpers = new MSSQLHelper("Data Source=11.104.0.6;Initial Catalog=comm;Persist Security Info=True;User ID=power;Password=massunsoft009");

        //沂南总
        //  MSSQLHelper _mssqlHelpers = new MSSQLHelper("Data Source=172.12.12.102;Initial Catalog=comm;Persist Security Info=True;User ID=sa;Password=admin123");
        //开发i去
        MSSQLHelper _mssqlHelpers = new MSSQLHelper("Data Source=11.100.253.21;Initial Catalog=comm;Persist Security Info=True;User ID=power;Password=massunsoft009");
        //界湖
        //  MSSQLHelper _mssqlHelpers = new MSSQLHelper("Data Source=11.101.8.2;Initial Catalog=comm;Persist Security Info=True;User ID=power;Password=m@ssunsoft009");
        //费县192.168.1.99//11.105.9.53 massunsoft009
        //  MSSQLHelper _mssqlHelpers = new MSSQLHelper("Data Source=11.105.9.53;Initial Catalog=comm;Persist Security Info=True;User ID=power;Password=massunsoft009");


        public Boolean isRegisters = false;
        /// <summary>
        /// 医保个人信息
        /// </summary> 
        public NetworkPatInfo netPatInfo = new NetworkPatInfo();

        /// <summary>
        /// 住院入参
        /// </summary>
        public OutPayParameter outReimPara;
        //public OutPayParameter outReimPara=new OutPayParameter();


       



        /// <summary>
        /// 住院入参
        /// </summary>
        public InPayParameter inReimPara;
        /// <summary>
        ///  医保
        /// </summary>
        public LiYiNeusoftHandle _liYiNeusoftHandle = new LiYiNeusoftHandle();
        /// <summary>
        /// 门诊初始化结果
        /// </summary>
        public Dictionary<string, string> initMzRe = new Dictionary<string, string>();
        /// <summary>
        /// 住院初始化结果
        /// </summary>
        public Dictionary<string, string> initZyRe = new Dictionary<string, string>();

        /// <summary>
        /// 结算信息
        /// </summary>
        public Dictionary<string, string> dicSettleInfo = new Dictionary<string, string>();

        /// <summary>
        /// 患者信息
        /// </summary> 
        public Dictionary<string, string> dicPatInfo = new Dictionary<string, string>();

        /// <summary>
        /// 是否初始化
        /// </summary>
        public static bool isInit = false;

        /// <summary>
        /// 固定支付方式
        /// </summary>
        public static int payTypeId = 4;
        /// <summary>
        /// 是否住院
        /// </summary>
        public Boolean isOut = true;

        //操作人员
        // public OperatorInfo operatorInfo;
        public OperatorInfo operatorInfo = PayAPIConfig.Operator;

        //=new OperatorInfo();

        /// <summary>
        /// 银行标志失败，撤销交易单号
        /// </summary>
        public string bankRefundMarkNO = "";

        /// <summary>
        /// 银行标志失败，撤销交易流水号
        /// </summary>
        public string bankRefundSerial = "";

        //登记串
        public string registerStr = "";


        public string oldNewPatientMarkNetSerial = "";
        #endregion
        #region 医保签到函数
        /// <summary>
        /// 医保签到函数zuofei 
        /// </summary>
        public void StructMethod()
        {
            //_liYiNeusoftHandle = new LiYiNeusoftHandle();
            //string qd = "";
            //LogManager.Debug("医保签到：开始》》》");

            //qd = _liYiNeusoftHandle.GetImpString("9100", "1", "");
            //if (!isInit)
            //{
            //    _liYiNeusoftHandle = new LiYiNeusoftHandle();
            //    _liYiNeusoftHandle.InitNeu();

            //    isInit = true;
            //}
            //_liYiNeusoftHandle.BussinessHandle(qd);
            //LogManager.Debug("医保签到：" + DateTime.Now.ToString());

        }
        #endregion
        #region 参数修改赋值

        public string GetUSER_CODE(string userSysid)
        {
            string userCode = "";
            string sql = "  SELECT  *  FROM comm.COMM.USERS   WHERE   USER_ID IN (SELECT  USER_ID  FROM   comm.COMM.USERS_SYS  WHERE  USER_SYS_ID= '" + userSysid + "')";
            DataTable dt = _mssqlHelpers.ExecSqlReDs(sql).Tables[0];
            foreach (DataRow item in dt.Rows)
            {
                userCode = item["USER_CODE"].ToString();
            }

            return userCode;
        }


        #endregion
        #region 构造函数及东软处理类初始化
        public NeusoftInterfaceModel()
        {
            InterfaceInit();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InterfaceInit()
        {
            if (!isInit)
            {
                _liYiNeusoftHandle = new LiYiNeusoftHandle();
                _liYiNeusoftHandle.InitNeu();

                isInit = true;
            }
        }
        #endregion


        #region 临时撤销用的参数拼接
        public string linsche()
        {

            var chex = new List<string>();

            //outPara.NetRegSerial ="MZ"+ outPara.NetworkSettleId;
            // outReimPara.RegInfo.NetRegSerial = "MZ";
            //outReimPara.RegInfo.NetRegSerial.Length;
            //门诊流水号
            // mzRegList.Add(outReimPara.RegInfo.PatSerial);
            //住院流水号
            chex.Add("MZ_347.0");

            //单据号
            chex.Add("14204762.0");
            //结算日期
            //mzRegList.Add(DateTime.Now.ToString("yyyyMMddHHmmss"));
            chex.Add("");

            //经办人
            chex.Add(operatorInfo.UserName);
            //是否保存处方标志
            chex.Add("0");
            //开发商标志
            chex.Add("msun");
            //交易编号
            chex.Add("");
            //社保交易编号
            chex.Add("");
            //Pos机器交易编号
            chex.Add("");
            //银行交易编号
            chex.Add("");
            //备用
            chex.Add("");

            LogManager.Debug("时间" + DateTime.Now.ToString());

            return string.Join("|", chex.ToArray());


        }

        /// <summary>
        /// 冲正拼接
        /// </summary>
        /// <returns></returns>
        public string linsche111()
        {

            var chex = new List<string>();

            //outPara.NetRegSerial ="MZ"+ outPara.NetworkSettleId;
            // outReimPara.RegInfo.NetRegSerial = "MZ";
            //outReimPara.RegInfo.NetRegSerial.Length;
            //门诊流水号
            // mzRegList.Add(outReimPara.RegInfo.PatSerial);
            //冲正业务交易编码
            chex.Add("2410");
            // chex.Add("2431");

            //2431
            //0105-20180702152404-4.0
            //被冲正交易发送方交易流水号
            chex.Add("0105-20180702143138-4.0");
            //chex.Add("0105-20180702152404-4.0");


            LogManager.Debug("时间" + DateTime.Now.ToString());

            return string.Join("|", chex.ToArray());


        }

        #endregion
        #region //读卡操作
        /// <summary>
        /// 读卡返回病号基本信息
        /// </summary>
        public NetworkPatInfo NetworkReadCard()
        {
            InterfaceInit();
            //StructMethod();
            //#region 利用读卡进行调试

            ////注掉的部分是由于调试异常，无法正常退费,使用下面的代码进行了强制退费//不建议使用冲正交易
            ////银行退款
            //var celBankTranStr = GetBankTransMzStr(outReimPara);
            //if (celBankTranStr.Length > 0)
            //{
            //   // var celBankImpStr = _liYiNeusoftHandle.GetImpString("2900", outReimPara.PatInfo.OutPatId.ToString(),
            //   //celBankTranStr);
            //    var celBankImpStr = _liYiNeusoftHandle.GetImpString("2900", "21030",
            // celBankTranStr);
            //    try
            //    {
            //        var celBankRet = _liYiNeusoftHandle.BankTrans(celBankImpStr);
            //       //_liYiNeusoftHandle.SaveBankTransInfo(outReimPara.PatInfo.OutPatId.ToString(), outReimPara.RegInfo.OutNetworkSettleId.ToString(), celBankRet, "MZ");
            //        _liYiNeusoftHandle.SaveBankTransInfo("21030", "14615919", celBankRet, "MZ");

            //        if (celBankRet["返回码"] != "000000")
            //        {
            //            throw new Exception(celBankRet["返回码含义"]);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        DialogResult dr = MessageBox.Show("银行撤销失败,是否继续？点击 是 将继续撤销医保结算,银行交易请手工进行撤销。点击 否 将终止本次撤销。",
            //            "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            //        if (dr == DialogResult.No)
            //        {
            //            throw new Exception("银行撤销交易失败." + ex.Message);
            //        }
            //    }
            //}
            ////冲正
            ////  var regImpStr11 = _liYiNeusoftHandle.GetImpString("2421", "102606444", linsche111());

            //// var regRet11 = _liYiNeusoftHandle.BussinessHandle(regImpStr11);
            ////102606443
            //// var regImpStr = _liYiNeusoftHandle.GetImpString("2430", "102606444", linsche());

            //// var regRet = _liYiNeusoftHandle.BussinessHandle(regImpStr);
            /////下面是结算用的格式
            /////var mzSetStr = GetMzSettleStr(outReimPara);
            ///// var setImpStr = _liYiNeusoftHandle.GetImpString(outReimPara.RegInfo.Memo1 == "true" ? "2410" : "2411", outReimPara.PatInfo.OutPatId.ToString(), mzSetStr);

            ////以上为调试异常临时撤销费用使用

            ////WinReadCard winReadCard = new WinReadCard("OUT") { OutPara = outReimPara };
            //#endregion

            WinReadCard winReadCard = new WinReadCard("OUT");// OutPara =outReimPara };


            winReadCard.ShowDialog();
            if (winReadCard.DialogResult == System.Windows.Forms.DialogResult.Cancel)
            {
                throw new Exception("用户取消操作!");
            }

            var patInfo = winReadCard.PersonInfoDic;
            NetworkPatInfo networkPatInfo = new NetworkPatInfo();
            networkPatInfo.MedicalNo = patInfo["医疗证号"];                    //医保个人编号
            networkPatInfo.ICNo = patInfo["社会保障卡卡号"];                           //社会保障卡卡号
            networkPatInfo.PatName = patInfo["姓名"];                        //姓名
            networkPatInfo.Sex = patInfo["性别"] == "1" ? "男" : "女";       //性别
            networkPatInfo.IDNo = patInfo["身份证号"];                        //身份证号码
            networkPatInfo.MedicalType = patInfo["人员类别"];                //医疗人员类别
            networkPatInfo.ICAmount = Convert.ToDecimal(patInfo["帐户余额"]);  //账户余额
            networkPatInfo.CompanyName = patInfo["单位名称"];                  //单位名称
            networkPatInfo.CompanyName = patInfo["单位名称"];                    //单位编号
            networkPatInfo.Birthday = Convert.ToDateTime(patInfo["身份证号"].Substring(6, 4) + "-" + patInfo["身份证号"].Substring(10, 2) + "-" + patInfo["身份证号"].Substring(12, 2));                    //出生日期
            return networkPatInfo;

        }
        #endregion
        #region 门诊业务
        /// <summary>
        /// 门诊读卡
        /// </summary>
        /// <returns></returns>
        public void OutNeworkReadCard()
        {
            InterfaceInit();
            // StructMethod();
            //102606443
            LogManager.Debug("初始化成功>>>门诊读卡开始");
            WinReadCard winReadCard = new WinReadCard("OUT") { OutPara = outReimPara };
            winReadCard.ShowDialog();
            if (winReadCard.DialogResult == System.Windows.Forms.DialogResult.Cancel)
            {
                throw new Exception("用户取消操作!");
            }
            var patInfo = winReadCard.PersonInfoDic;
            outReimPara.RegInfo.CardNo = patInfo["社会保障卡卡号"];                   //医疗卡号null
            outReimPara.RegInfo.CantonCode = patInfo["所属区号"];              //社保机构编码
            outReimPara.RegInfo.MemberNo = patInfo["个人编号"];                //成员编码
            outReimPara.RegInfo.CompanyName = patInfo["单位编号"];               //单位名称
            outReimPara.RegInfo.PatAddress = patInfo["单位名称"];                   //住址
            outReimPara.RegInfo.CardNo = patInfo["医疗证号"];
            outReimPara.RegInfo.IdNo = patInfo["身份证号"];                    //身份证号
            outReimPara.RegInfo.NetPatType = patInfo["人员类别"];             //人员类别
            outReimPara.RegInfo.Memo1 = patInfo["有无卡"];
            //当姓名不一致时提示
            // if (outReimPara.RegInfo.NetPatName != patInfo["姓名"])
            if (outReimPara.PatInfo.PatName.Trim() != patInfo["姓名"].Trim())
            {
                MessageBox.Show(" 医保登记姓名为：【" + patInfo["姓名"].ToString() + "】     患者姓名为：" + outReimPara.PatInfo.PatName.Trim());
                throw new Exception("姓名不一致，操作员取消操作！");
                //if (MessageBox.Show(" 医保登记姓名为：【" + patInfo["姓名"].ToString() + "】     患者姓名为：【" + outReimPara.PatInfo.PatName.Trim() + "】 是否继续 ", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                //{

                //    throw new Exception("姓名不一致，操作员取消操作！");
                //}
            }
            if (patInfo["医疗类别"] != "11" && patInfo["医疗类别"] != "14")
            {

            }
            outReimPara.RegInfo.NetPatName = patInfo["姓名"];                  //姓名
            outReimPara.RegInfo.Memo2 = patInfo["精准扶贫标志"] + "|" + patInfo["民政人员标志"];//patInfo["医疗类别"];               //医疗统筹类别
            outReimPara.RegInfo.NetType = patInfo["医疗类别"]; //医疗统筹类别
            outReimPara.RegInfo.NetDiagnosCode = patInfo["诊断编码"]; //诊断编码
            outReimPara.RegInfo.NetDiagnosName = patInfo["诊断名称"];  //诊断名称
            outReimPara.RegInfo.Balance = Convert.ToDecimal(patInfo["帐户余额"]);
            //outReimPara.RegInfo.NetRegSerial = "MZ_" + DateTime.Now.ToString("yyyyMMddHHmmssff") +"_"+ outReimPara.PatInfo.OutPatId;
            // outReimPara.RegInfo.NetRegSerial = "MZ_" +DateTime.Now.ToString("ff")+ outReimPara.CommPara.TradeId; //outReimPara.PatInfo.OutPatId;
            //int  BH= outReimPara.RegInfo.NetRegSerial.Length;
            outReimPara.RegInfo.NetRegSerial = "MZ" + outReimPara.CommPara.OutNetworkSettleId.ToString();
            bankRefundSerial = outReimPara.RegInfo.NetRegSerial;

        }


        /// <summary>
        /// 退费强制读卡，获取人员卡片类别
        /// </summary>
        public void OutNeworkReadCardRefundMoney()
        {
            InterfaceInit();
            //StructMethod();
            //102606443

            WinReadCard winReadCard = new WinReadCard("OUT") { OutPara = outReimPara };
            winReadCard.ShowDialog();
            if (winReadCard.DialogResult == System.Windows.Forms.DialogResult.Cancel)
            {
                throw new Exception("用户取消操作!");
            }
            var patInfo = winReadCard.PersonInfoDic;
            outReimPara.RegInfo.CardNo = patInfo["社会保障卡卡号"];                   //医疗卡号null
            outReimPara.RegInfo.CantonCode = patInfo["所属区号"];              //社保机构编码
            outReimPara.RegInfo.MemberNo = patInfo["个人编号"];                //成员编码
            outReimPara.RegInfo.CompanyName = patInfo["单位编号"];               //单位名称
            outReimPara.RegInfo.PatAddress = patInfo["单位名称"];                   //住址
            outReimPara.RegInfo.IdNo = patInfo["身份证号"];                    //身份证号
            outReimPara.RegInfo.NetPatType = patInfo["人员类别"];             //人员类别
            outReimPara.RegInfo.Memo1 = patInfo["有无卡"];
            //当姓名不一致时提示
            // if (outReimPara.RegInfo.NetPatName != patInfo["姓名"])
            if (outReimPara.PatInfo.PatName.Trim() != patInfo["姓名"].Trim())
            {
                MessageBox.Show(" 医保登记姓名为：【" + patInfo["姓名"].ToString() + "】     患者姓名为：" + outReimPara.PatInfo.PatName.Trim());
                throw new Exception("姓名不一致，操作员取消操作！");
                //if (MessageBox.Show(" 医保登记姓名为：【" + patInfo["姓名"].ToString() + "】     患者姓名为：【" + outReimPara.PatInfo.PatName.Trim() + "】 是否继续 ", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                //{

                //    throw new Exception("姓名不一致，操作员取消操作！");
                //}
            }
            if (patInfo["医疗类别"] != "11" && patInfo["医疗类别"] != "14")
            {
                outReimPara.RegInfo.NetDiagnosCode = patInfo["诊断编码"]; //诊断编码
                outReimPara.RegInfo.NetDiagnosName = patInfo["诊断名称"];  //诊断名称
            }

        }

        /// <summary>
        /// 门诊登记(点击门诊结算首先调用)
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void OutNetworkRegister(OutPayParameter para)
        {

            outReimPara = para;
            OutNeworkReadCard();
            //门诊登记
            var mzRegList = new List<string>();

            //outPara.NetRegSerial ="MZ"+ outPara.NetworkSettleId;
            // outReimPara.RegInfo.NetRegSerial = "MZ";
            //outReimPara.RegInfo.NetRegSerial.Length;
            //门诊流水号
            // mzRegList.Add(outReimPara.RegInfo.PatSerial);
            mzRegList.Add(outReimPara.RegInfo.NetRegSerial);

            //医疗类别
            /// mzRegList.Add(outReimPara.RegInfo.Memo2);
            mzRegList.Add(outReimPara.RegInfo.NetType);

            //挂号日期
            mzRegList.Add(DateTime.Now.ToString("yyyyMMddHHmmss"));
            //诊断疾病编码
            mzRegList.Add(outReimPara.RegInfo.NetDiagnosCode);
            //诊断疾病名称
            mzRegList.Add(outReimPara.RegInfo.NetDiagnosName);
            //科室名称
            mzRegList.Add("");
            //床位号
            mzRegList.Add("");
            //医生代码
            mzRegList.Add("");
            //医生姓名
            mzRegList.Add("");
            //挂号费
            mzRegList.Add("0");
            //检查费
            mzRegList.Add("0");

            LogManager.Debug("时间" + DateTime.Now.ToString() + "门诊登记流水号：" + outReimPara.RegInfo.NetRegSerial);
            //经办人
            //mzRegList.Add(GetUSER_CODE(operatorInfo.UserSysId));
            // mzRegList.Add("00001000");
            // mzRegList.Add("刘焕乾");//(operatorInfo.UserName);
            mzRegList.Add(operatorInfo.UserName);
            //   mzRegList.Add(GetUSER_CODE(operatorInfo.UserSysId));
            //mzRegList.Add(operatorInfo.UserSysId);

            //卡号 
            mzRegList.Add(outReimPara.RegInfo.CardNo);
            //个人编号
            mzRegList.Add(outReimPara.RegInfo.MemberNo);
            registerStr = string.Join("|", mzRegList.ToArray());


        }
        /// <summary>
        /// 门诊预结算 门诊登记后执行 第二调用
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void OutNetworkPreSettle(OutPayParameter para)
        {
            outReimPara = para;
            //门诊上传费用 
        }
        /// <summary>
        /// 门诊结算 门诊预结算后执行 第三调用
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void OutNetworkSettle(OutPayParameter para)
        {
            outReimPara = para;
            //门诊登记
            //  OutNetworkRegister(outReimPara);
            //StructMethod();
            var regImpStr = _liYiNeusoftHandle.GetImpString(outReimPara.RegInfo.Memo1 == "true" ? "2210" : "2211", outReimPara.PatInfo.OutPatId.ToString(), registerStr, true, "");
            var regRet = _liYiNeusoftHandle.BussinessHandle(regImpStr);
            LogManager.Debug("门诊登记结束……");
            //上传费用
            UploadOutFee(outReimPara);
            //门诊结算
            var mzSetStr = GetMzSettleStr(outReimPara);
            var setImpStr = _liYiNeusoftHandle.GetImpString(outReimPara.RegInfo.Memo1 == "true" ? "2410" : "2411", outReimPara.PatInfo.OutPatId.ToString(), mzSetStr, true, "");
            dicSettleInfo = _liYiNeusoftHandle.OffSettle(setImpStr);

            if (dicSettleInfo["银行交易成功标志"] == "0"||string.IsNullOrEmpty(dicSettleInfo["银行交易成功标志"]))
            {
               
                //保存结算结果
                SaveOutSettleMain();
            }
            else
            {
                MessageBox.Show("交易失败！银行交易成功标志：" + dicSettleInfo["银行交易成功标志"]+"交易过程中发生异常,执行退费,过程中请勿关闭");
                // var celSetStr = GetCancelSettleStr(outReimPara);
                LogManager.Debug("交易失败！银行交易成功标志：" + dicSettleInfo["银行交易成功标志"] + "交易过程中发生异常,执行退费,过程中请勿关闭");
                var celSetStrBr = GetCancelSettleStrRefund(outReimPara);

                var celSetImpStr = _liYiNeusoftHandle.GetImpString(outReimPara.RegInfo.Memo1 == "true" ? "2430" : "2431", outReimPara.PatInfo.OutPatId.ToString(), celSetStrBr, true, "");
                var celSetRet = _liYiNeusoftHandle.BussinessHandle(celSetImpStr);
                throw new Exception("银行扣款失败,已撤销本次结算。请重新尝试!");
            }

        }
        /// <summary>
        /// 撤销门诊结算
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void CancelOutNetworkSettle(OutPayParameter para)
        {
            outReimPara = para;
            LogManager.Debug("撤销结算开始");
            DialogResult re = MessageBox.Show("即将开始执行撤销结算,请确认医保卡是否插入!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (re == DialogResult.OK)
            {
                //OutNeworkReadCardRefundMoney();
                if (outReimPara.SettleInfo == null)
                    throw new Exception("本地结算信息不存在");
                try
                {
                    //Int64 outPatIdVInt = (Int64)outReimPara.SettleInfo.OutPatId;

                    //    DataTable dtCard = _mssqlHelpers.ExecSqlReDs();
                    var celSetStr = GetCancelSettleStr(outReimPara);

                    if (outReimPara.RegInfo.Memo1 == "false")  //  if (bankDt.Rows.Count == 0)
                    {
                        //无卡费用撤销结算
                        //throw new Exception("获取银行交易信息失败，请检查参数");
                        //撤销结算
                        var celSetImpStr = _liYiNeusoftHandle.GetImpString(outReimPara.RegInfo.Memo1 == "true" ? "2430" : "2431", outReimPara.SettleInfo.OutPatId.ToString("0"), celSetStr, true, "");
                        var celSetRet = _liYiNeusoftHandle.BussinessHandle(celSetImpStr);
                        LogManager.Debug("无卡人员社保撤销结算结束...");
                    }
                    else
                    {

                        //银行退款
                        var celBankTranStr = GetBankTransMzStr(outReimPara);
                        if (celBankTranStr.Length > 0)
                        {
                            //outReimPara.SettleInfo.OutPatId.ToString("0")
                            // var celBankImpStr = _liYiNeusoftHandle.GetImpString("2900", outReimPara.PatInfo.OutPatId.ToString(),
                            //celBankTranStr);
                            var celBankImpStr = _liYiNeusoftHandle.GetImpString("2900", outReimPara.SettleInfo.OutPatId.ToString("0"),
                        celBankTranStr, true, "");
                            try
                            {
                                var celBankRet = _liYiNeusoftHandle.BankTrans(celBankImpStr);
                                // _liYiNeusoftHandle.SaveBankTransInfo(outReimPara.PatInfo.OutPatId.ToString(), outReimPara.RegInfo.OutNetworkSettleId.ToString(), celBankRet, "MZ");
                                _liYiNeusoftHandle.SaveBankTransInfo(outReimPara.SettleInfo.OutPatId.ToString("0"), outReimPara.SettleInfo.RelationId.ToString("0"), celBankRet, "MZ");

                                if (celBankRet["返回码"] != "000000")
                                {
                                    throw new Exception(celBankRet["返回码含义"]);
                                    
                                }
                                LogManager.Info("银行撤销成功》》");
                            }
                            catch (Exception ex)
                            {
                                DialogResult dr = MessageBox.Show("银行撤销失败,是否继续？点击 是 将继续撤销医保结算,银行交易请手工进行撤销。点击 否 将终止本次撤销。",
                                    "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (dr == DialogResult.No)
                                {
                                    LogManager.Error("银行撤销交易失败" + ex.Message);
                                    throw new Exception("银行撤销交易失败." + ex.Message);

                                }
                                LogManager.Debug("点击了是....银行撤销失败,是否继续？点击 是");
                            }
                        }
                        //撤销结算
                        celSetStr = GetCancelSettleStr(outReimPara);
                        //outReimPara.SettleInfo.OutPatId.ToString("0")
                        //var celSetImpStr = _liYiNeusoftHandle.GetImpString(outReimPara.RegInfo.Memo1 == "true" ? "2430" : "2431", outReimPara.PatInfo.OutPatId.ToString(), celSetStr);
                        var celSetImpStr = _liYiNeusoftHandle.GetImpString(outReimPara.RegInfo.Memo1 == "true" ? "2430" : "2431", outReimPara.SettleInfo.OutPatId.ToString("0"), celSetStr, true, "");

                        var celSetRet = _liYiNeusoftHandle.BussinessHandle(celSetImpStr);
                        LogManager.Debug("有卡人员社保撤销结算结束...");
                    }

                }
                catch (Exception ex)
                {
                    LogManager.Info(ex.Message);
                    throw new Exception("医保撤销失败!" + ex.Message);
                }
            }
            //--
            else
            {

                throw new Exception("用户取消了操作！");
            }

        }
        /// <summary>
        ///  上传门诊费用
        /// </summary>
        /// <param name="outPara"></param>
        public void UploadOutFee(OutPayParameter outPara)
        {
            string notMatchedCharge = "";
            foreach (var item in outReimPara.Details)
            {
                if (string.IsNullOrEmpty(item.NetworkItemCode.ToString()))
                {
                    notMatchedCharge += "编码:" + item.ChargeCode + "," + "名称:" + item.ChargeName + "；";
                }
            }
            if (notMatchedCharge.Trim().Length > 0)
            {
                DialogResult diag = MessageBox.Show("有以下项目未对应：\n" + notMatchedCharge + ",是否继续上传。继续上传将按自费进行处理!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (diag == DialogResult.No)
                {
                    throw new Exception("有以下项目未对应：\n" + notMatchedCharge + ",取消上传！");
                }
            }
            foreach (var item in outReimPara.Details)
            {
                if (string.IsNullOrEmpty(item.NetworkItemCode.ToString().Trim()))
                {
                    //continue;
                    item.NetworkItemCode = "0";
                    item.Memo1 = "0";
                    if (Convert.ToInt32(item.Memo1) < 100)
                    {
                        item.NetworkItemProp = "1";//1药品、2诊疗项目
                    }
                    else
                    {
                        item.NetworkItemProp = "2";//1药品、2诊疗项目
                    }
                    //服务设施为3没传
                    item.NetworkItemClass = "91";//其他费用    
                }

                var chargeList = new List<string>();
                //就诊流水号
                chargeList.Add(outPara.RegInfo.NetRegSerial);
                //收费项目种类
                chargeList.Add(item.NetworkItemProp.ToString());
                //收费类别
                chargeList.Add(item.NetworkItemClass.ToString());
                //处方号
                //
                chargeList.Add(item.AutoId.ToString());
                //处方日期
                chargeList.Add(Convert.ToDateTime(item.CreateTime).ToString("yyyyMMddHHmmss"));
                //医院收费项目内码
                chargeList.Add(item.ChargeCode.ToString());
                //收费项目中心编码
                chargeList.Add(item.NetworkItemCode.ToString());
                //医院收费项目名称
                chargeList.Add(item.ChargeName.ToString());
                //单价
                chargeList.Add(item.Price.ToString());
                //数量
                chargeList.Add(item.Quantity.ToString());
                //金额
                chargeList.Add(item.Amount.ToString());
                //剂型 二级代码，非药品传999 暂时为传 
                chargeList.Add(item.NetworkItemProp.ToString() == "1" ? "0" : "999");
                //规格
                chargeList.Add(string.IsNullOrEmpty(item.Spec.ToString()) ? "/" : item.Spec.ToString());
                //每次用量
                chargeList.Add("1");
                //使用频次
                chargeList.Add("1");
                //医师代码
                chargeList.Add(item.DocCode.ToString());
                //医师姓名
               // chargeList.Add(item.DocCode.ToString());
                chargeList.Add(item.DocCode.ToString());

                //用法
                chargeList.Add("1");
                //单位
                chargeList.Add(item.Unit.ToString());
                //科室编号
                chargeList.Add(item.DeptCode.ToString());
                //科室名称
                chargeList.Add(item.DeptCode.ToString());
                //执行天数
                chargeList.Add("0");
                //经办人
                chargeList.Add(operatorInfo.UserName);
                //药品剂量单位
                chargeList.Add(item.Unit.ToString());
                //上传
                var impStr = _liYiNeusoftHandle.GetImpString("2310", outPara.PatInfo.OutPatId.ToString(),
                    string.Join("|", chargeList.ToArray()), true, "");
                var outFeeDic = _liYiNeusoftHandle.UploadOutFee(impStr);
                item.AmountSelf = decimal.Parse(outFeeDic["自理金额"]);
                item.AmountSelfBurdern = decimal.Parse(outFeeDic["自费金额"]);
                if (!string.IsNullOrEmpty(outFeeDic["说明信息"].ToString()))
                {
                    LogManager.Debug("医保返回自费原因:"+outFeeDic["说明信息"].ToString());
                }
            }
        }
        /// <summary>
        ///  结算交易串
        /// </summary>
        /// <param name="outPara"></param>
        /// <returns></returns>
        private string GetMzSettleStr(OutPayParameter outPara)
        {
            var mzSettleList = new List<string>();
            //住院流水号(门诊流水号)
            mzSettleList.Add(outPara.RegInfo.NetRegSerial);
            bankRefundSerial = outPara.RegInfo.NetRegSerial;
            //单据号
            mzSettleList.Add(outPara.RegInfo.OutNetworkSettleId.ToString());
            bankRefundMarkNO = outPara.RegInfo.OutNetworkSettleId.ToString();
            //医疗类别
            // mzSettleList.Add(outPara.RegInfo.Memo2);
            mzSettleList.Add(outPara.RegInfo.NetType);

            //结算日期
            mzSettleList.Add(DateTime.Now.ToString("yyyyMMddHHmmss"));
            //出院日期
            mzSettleList.Add(DateTime.Now.ToString("yyyyMMddHHmmss"));
            //出院原因
            mzSettleList.Add("");
            //出院诊断疾病编码
            mzSettleList.Add("");
            //出院诊断疾病名称
            mzSettleList.Add("");
            //账户使用标志
            mzSettleList.Add("1");
            LogManager.Debug("结算交易串单据号：" + outPara.RegInfo.OutNetworkSettleId.ToString());
            //中途结算标志
            mzSettleList.Add("");
            //经办人
            mzSettleList.Add(operatorInfo.UserName);
            //开发商标志
            mzSettleList.Add("msun");
            return string.Join("|", mzSettleList.ToArray());
        }
        /// <summary>
        ///  撤销结算交易串
        /// </summary>
        /// <param name="outPara"></param>
        /// <returns></returns>
        private string GetCancelSettleStr(OutPayParameter outPara)
        {
            var mzCancelSettleList = new List<string>();
            //住院流水号(门诊流水号)
            //mzCancelSettleList.Add(outPara.RegInfo.NetRegSerial);
            string regInfoNetRegSerial = "";
            string sqlSerial = "select TOP 1  * from MZ.OUT.OUT_NETWORK_REGISTERS where OUT_PAT_ID='" + outPara.SettleInfo.OutPatId + "' and  OUT_NETWORK_SETTLE_ID='" + outPara.SettleInfo.RelationId + "'" + " ORDER BY CREATE_TIME DESC ";
            DataTable dSerial = _mssqlHelpers.ExecSqlReDs(sqlSerial).Tables[0];
            if (dSerial.Rows.Count != 1)
            {
                throw new Exception("获取退费联网流水号异常！OutPatId：" + outPara.SettleInfo.OutPatId);
            }
            foreach (DataRow item in dSerial.Rows)
            {
                regInfoNetRegSerial = item["NET_REG_SERIAL"].ToString();
                outReimPara.RegInfo.Memo1 = item["MEMO_1"].ToString();

            }
            if (string.IsNullOrEmpty(regInfoNetRegSerial) || string.IsNullOrEmpty(outReimPara.RegInfo.Memo1))
            {
                throw new Exception("获取退费联网流水号为空异常！或检测不到人员卡类别！OutPatId：" + outPara.SettleInfo.OutPatId);
            }
            mzCancelSettleList.Add(regInfoNetRegSerial);

            //单据号
            mzCancelSettleList.Add(outPara.SettleInfo.SettleNo);
            //  mzCancelSettleList.Add("14204783.0");


            //  mzCancelSettleList.Add(outPara.RegInfo.OutNetworkSettleId.ToString());
            //结算日期
            mzCancelSettleList.Add(DateTime.Now.ToString("yyyyMMddHHmmss"));
            //经办人
            mzCancelSettleList.Add(operatorInfo.UserName);
            //是否保存处方标志
            mzCancelSettleList.Add("0");
            //开发商标志
            mzCancelSettleList.Add("msun");
            //20160720 add 备用字段
            //交易编号
            mzCancelSettleList.Add("");
            //社保交易编号
            mzCancelSettleList.Add("");
            //Pos机器交易编号
            mzCancelSettleList.Add("");
            //银行交易编号
            mzCancelSettleList.Add("");
            //备用
            mzCancelSettleList.Add("");
            return string.Join("|", mzCancelSettleList.ToArray());
        }



        /// <summary>
        /// 银行标志失败，撤销交易串拼接
        /// </summary>
        /// <param name="outPara"></param>
        /// <returns></returns>
        private string GetCancelSettleStrRefund(OutPayParameter outPara)
        {
            var mzCancelSettleList = new List<string>();
            //住院流水号(门诊流水号)
            //mzCancelSettleList.Add(outPara.RegInfo.NetRegSerial);
            string regInfoNetRegSerial = "";
            //string sqlSerial = "select TOP 1  * from MZ.OUT.OUT_NETWORK_REGISTERS where OUT_PAT_ID='" + outPara.SettleInfo.OutPatId + "' and  OUT_NETWORK_SETTLE_ID='" + outPara.SettleInfo.RelationId + "'" + " ORDER BY CREATE_TIME DESC ";
            //DataTable dSerial = _mssqlHelpers.ExecSqlReDs(sqlSerial).Tables[0];
            //if (dSerial.Rows.Count != 1)
            //{
            //    throw new Exception("获取退费联网流水号异常！OutPatId：" + outPara.SettleInfo.OutPatId);
            //}
            //foreach (DataRow item in dSerial.Rows)
            //{
            regInfoNetRegSerial = bankRefundSerial;// = item["NET_REG_SERIAL"].ToString();
            // outReimPara.RegInfo.Memo1 = item["MEMO_1"].ToString();

            //}
            if (string.IsNullOrEmpty(regInfoNetRegSerial) || string.IsNullOrEmpty(outReimPara.RegInfo.Memo1))
            {
                throw new Exception("获取退费联网流水号为空异常！或检测不到人员卡类别！OutPatId：" + outPara.SettleInfo.OutPatId);
            }
            mzCancelSettleList.Add(regInfoNetRegSerial);

            //单据号
            //mzCancelSettleList.Add(outPara.SettleInfo.SettleNo);
            mzCancelSettleList.Add(bankRefundMarkNO);

            //  mzCancelSettleList.Add("14204783.0");


            //  mzCancelSettleList.Add(outPara.RegInfo.OutNetworkSettleId.ToString());
            //结算日期
            mzCancelSettleList.Add(DateTime.Now.ToString("yyyyMMddHHmmss"));
            //经办人
            mzCancelSettleList.Add(operatorInfo.UserName);
            //是否保存处方标志
            mzCancelSettleList.Add("0");
            //开发商标志
            mzCancelSettleList.Add("msun");
            //20160720 add 备用字段
            //交易编号
            mzCancelSettleList.Add("");
            //社保交易编号
            mzCancelSettleList.Add("");
            //Pos机器交易编号
            mzCancelSettleList.Add("");
            //银行交易编号
            mzCancelSettleList.Add("");
            //备用
            mzCancelSettleList.Add("");
            return string.Join("|", mzCancelSettleList.ToArray());
        }

        /// <summary>
        ///  银行交易 参数缺失
        /// </summary>
        private string GetBankTransMzStr(OutPayParameter outPara)
        {
            LinYiPosHandle handle = new LinYiPosHandle();
            string outPatId = outReimPara.SettleInfo.OutPatId.ToString("0");//outReimPara.PatInfo.OutPatId.ToString();
            string networkSettleId = outReimPara.SettleInfo.RelationId.ToString("0");//outReimPara.RegInfo.OutNetworkSettleId.ToString();

            // string outPatId = "21030";//outReimPara.SettleInfo.OutPatId.ToString("0");//outReimPara.PatInfo.OutPatId.ToString();
            // string networkSettleId = "14615919";//outReimPara.SettleInfo.RelationId.ToString("0");//outReimPara.RegInfo.OutNetworkSettleId.ToString();

            DataTable bankDt = _liYiNeusoftHandle.GetBankInfoForMz(networkSettleId, outPatId);

            // LinYiPosHandle handle = new LinYiPosHandle();
            // string outPatId = outPara.PatInfo.OutPatId.ToString();//"102606444"; //
            // string networkSettleId = outPara.RegInfo.OutNetworkSettleId.ToString();// "14204762";//
            // //获取银行交易信息14204762
            // DataTable bankDt = _liYiNeusoftHandle.GetBankInfoForMz(networkSettleId, outPatId);
            //// DataTable bankDt = _liYiNeusoftHandle.GetBankInfoForMz("14204762", outPatId);

            //交易索引号 参考号 Pos机交易编号 原交易日期 银行交易成功标志 银行账号 

            if (bankDt.Rows.Count < 1)
            {
                throw new Exception("获取银行交易信息失败，请检查参数");
            }
            var bjysyh = bankDt.Select("PARA_NAME='交易索引号'")[0]["PARA_VALUE"].ToString();
            var bckh = bankDt.Select("PARA_NAME='参考号'")[0]["PARA_VALUE"].ToString();
            var bposjybh = bankDt.Select("PARA_NAME='Pos机交易编号'")[0]["PARA_VALUE"].ToString();
            var byjyrq = bankDt.Select("PARA_NAME='原交易日期'")[0]["PARA_VALUE"].ToString();
            var byhzh = bankDt.Select("PARA_NAME='银行账号'")[0]["PARA_VALUE"].ToString();
            var byhjybz = bankDt.Select("PARA_NAME='银行交易成功标志'")[0]["PARA_VALUE"].ToString();
            if (bjysyh != "" && bckh != "" && bposjybh != "" && byhzh != "" && byhjybz == "0")
            {
                LogManager.Info("撤销银行交易<<<");
                //撤销银行交易
                var insertTime = byjyrq;
                var bankAmount = Convert.ToString(Convert.ToInt32(Convert.ToDecimal(outPara.SettleInfo.MedAmountZhzf) * 100));
                //var bankAmount = Convert.ToString(Convert.ToInt32(Convert.ToDecimal(52.70) * 100));

                handle.AddListInParas(insertTime != DateTime.Now.ToString("yyyyMMdd") ? "09" : "02", 2,
                    new char[] { (' ') },
                    "L");
                handle.AddListInParas(bankAmount, 12, new char[] { ('0') }, "R"); //此处为报销金额
                //handle.AddListInParas("45", 12, new char[] { ('0') }, "R"); //此处为报销金额

                handle.AddListInParas(bposjybh, 6, new char[] { (' ') }, "L"); //POS 流水号
                handle.AddListInParas("", 10, new char[] { (' ') }, "L");
                handle.AddListInParas("", 10, new char[] { (' ') }, "L");
                handle.AddListInParas(bckh, 15, new char[] { (' ') }, "L"); // 参考号 
                handle.AddListInParas("", 6, new char[] { (' ') }, "L");
                handle.AddListInParas(insertTime, 8, new char[] { (' ') }, "L");
                // 区分新旧卡 
                handle.AddListInParas("P", 1, new char[] { (' ') }, "L");
                handle.AddListInParas(bjysyh, 76, new char[] { (' ') }, "L"); // 交易索引号 
                //handle.AddListInParas(netPatInfo.ICNo, 37, new char[] { (' ') }, "L");
                handle.AddListInParas(byhzh, 37, new char[] { (' ') }, "L"); //银行卡号
                handle.AddListInParas("", 104, new char[] { (' ') }, "L"); //交易流水号
                handle.AddListInParas("", 2, new char[] { (' ') }, "L");
                handle.AddListInParas("", 15, new char[] { (' ') }, "L");
                handle.AddListInParas("", 6, new char[] { (' ') }, "L");
                handle.AddListInParas("", 3, new char[] { (' ') }, "L");
                handle.AddListInParas("", 20, new char[] { (' ') }, "L");
                handle.AddListInParas("", 30, new char[] { (' ') }, "L");
                handle.AddListInParas("", 15, new char[] { (' ') }, "L");
            }
            return handle.CommInput();
        }
        #endregion
        #region 住院业务
        /// <summary>
        /// 住院读卡
        /// </summary>
        /// <returns></returns>
        public void InNetWorkReadCard()
        {
            WinReadCard winReadCard = new WinReadCard("IN") { InPara = inReimPara };
            winReadCard.ShowDialog();
            if (winReadCard.DialogResult == System.Windows.Forms.DialogResult.Cancel)
            {
                throw new Exception("用户取消操作!");
            }
            var patInfo = winReadCard.PersonInfoDic;
            inReimPara.RegInfo.CardNo = patInfo["社会保障卡卡号"];                   //医疗卡号
            inReimPara.RegInfo.Memo1 = patInfo["有无卡"];                   //卡序列号

            inReimPara.RegInfo.CantonCode = patInfo["所属区号"];              //社保机构编码
            inReimPara.RegInfo.MemberNo = patInfo["个人编号"];                //成员编码
            inReimPara.RegInfo.CompanyName = patInfo["单位编号"];               //单位名称
            inReimPara.RegInfo.PatAddress = patInfo["单位名称"];                   //住址
            inReimPara.RegInfo.IdNo = patInfo["身份证号"];                    //身份证号
            inReimPara.RegInfo.NetPatType = patInfo["人员类别"];             //人员类别
            //inReimPara.Sex = patInfo["性别"];
            //当姓名不一致时提示
            //  if (inReimPara.RegInfo.NetPatName.Trim() != patInfo["姓名"])
            if (inReimPara.PatInfo.InPatName.Trim() != patInfo["姓名"].Trim())
            {
                //if (MessageBox.Show(" 医保登记姓名为：【" + patInfo["姓名"].ToString() + "】     患者姓名为：【" + inReimPara.PatInfo.InPatName + "】 是否继续 ", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                //{
                //    throw new Exception("姓名不一致，操作员取消操作！");
                //}
                MessageBox.Show(" 医保登记姓名为：【" + patInfo["姓名"].ToString() + "】     患者姓名为：【" + inReimPara.PatInfo.InPatName + "】");
                throw new Exception(" 医保登记姓名为：【" + patInfo["姓名"].ToString() + "】     患者姓名为：【" + inReimPara.PatInfo.InPatName + "】");

            }
            inReimPara.RegInfo.NetPatName = patInfo["姓名"];                  //姓名
            // inReimPara.RegInfo.Memo2 = patInfo["医疗类别"];               //医疗统筹类别
            inReimPara.RegInfo.NetType = patInfo["医疗类别"];               //医疗统筹类别
            inReimPara.RegInfo.Memo2 = patInfo["精准扶贫标志"] + "|" + patInfo["民政人员标志"];
            //if ( patInfo["精准扶贫标志"]=="1")
            //{
            //    frmPoorTip frmpoor = new frmPoorTip();
            //    frmpoor.ShowDialog();
            //}
          
            //  frmpoor.Close();


            inReimPara.RegInfo.NetDiagnosCode = patInfo["诊断编码"]; //诊断编码
            inReimPara.RegInfo.NetDiagnosName = patInfo["诊断名称"];  //诊断名称
            inReimPara.RegInfo.PatInHosSerial = "ZY" + inReimPara.PatInfo.PatInHosId + DateTime.Now.ToString("ff");
            inReimPara.RegInfo.NetRegSerial = inReimPara.RegInfo.PatInHosSerial;
        }
        /// <summary>
        /// 住院登记
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void InNetworkRegister(InPayParameter para)
        {
            inReimPara = para;
            InNetWorkReadCard();
            //住院登记
            var inRegStr = GetInRegistStr(inReimPara);
            var regImpStr = _liYiNeusoftHandle.GetImpString(inReimPara.RegInfo.Memo1 == "true" ? "2210" : "2211", inReimPara.RegInfo.PatInHosId.ToString(), inRegStr, false, inReimPara.RegInfo.NetRegSerial);
            var regRet = _liYiNeusoftHandle.BussinessHandle(regImpStr);
        }

        /// <summary>
        /// 撤销住院登记
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void CancelInNetworkRegister(InPayParameter para)
        {
            inReimPara = para;
            DeleteAllFee(inReimPara);

            //新病人
            if (string.IsNullOrEmpty(inReimPara.RegInfo.NetRegSerial))
            {
                var inCelRegStr = inReimPara.RegInfo.PatInHosSerial + "|" + operatorInfo.UserName;

                var celregImpStr = _liYiNeusoftHandle.GetImpString("2240", inReimPara.PatInfo.PatInHosId.ToString(), inCelRegStr, false, inReimPara.RegInfo.NetRegSerial);
                var regRet = _liYiNeusoftHandle.BussinessHandle(celregImpStr);
            }
            //老病人
            else
            {
                var inCelRegStr = inReimPara.RegInfo.NetRegSerial + "|" + operatorInfo.UserName;

                var celregImpStr = _liYiNeusoftHandle.GetImpString("2240", inReimPara.PatInfo.PatInHosId.ToString(), inCelRegStr, false, inReimPara.RegInfo.NetRegSerial);
                var regRet = _liYiNeusoftHandle.BussinessHandle(celregImpStr);
            }

        }
        /// <summary>
        /// 住院预结算
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void InNetworkPreSettle(InPayParameter para)
        {
            inReimPara = para;
            ///MessageBox.Show("登记信息" + inReimPara.RegInfo.Memo1 +"住院ID"+ inReimPara.RegInfo.PatInHosId+inReimPara.RegInfo.InNetworkRegisterId+"姓名"+inReimPara.RegInfo.NetPatName+"流水号"+inReimPara.RegInfo.NetRegSerial);

            if (inReimPara.SettleInfo != null && inReimPara.SettleInfo.MedAmountTotal != 0)
            {
                LogManager.Debug("监测到接口结算表存在结算数据,跳过东软结算.医保农合支付总额:" + inReimPara.SettleInfo.MedAmountTotal + "住院ID" + inReimPara.RegInfo.PatInHosId + "姓名" + inReimPara.RegInfo.NetPatName);

                //付费方式 本接口 4 医保 6农合
                PayAPIInterface.Model.Comm.PayType payType1;
                PayAPIInterface.Model.Comm.PayType payTypeZg1;
                inReimPara.PayTypeList = new List<PayType>();

                payType1 = new PayAPIInterface.Model.Comm.PayType();


                payTypeZg1 = new PayAPIInterface.Model.Comm.PayType();
                //payType.PayTypeId = payTypeId;
                //payType.PayTypeName = "医保";

                payType1.PayTypeId = 4;
                payType1.PayTypeName = "医保";

                payType1.PayAmount = inReimPara.SettleInfo.Amount - inReimPara.SettleInfo.GetAmount - inReimPara.SettleInfo.MedAmountZhzf;  // - Convert.ToDecimal(setRet["本次现金支付"]) - Convert.ToDecimal(setRet["本次帐户支付"]);// Convert.ToDecimal(dicSettleInfo["统筹支付金额"]);//Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"]);


                payTypeZg1.PayTypeId = 5;
                payTypeZg1.PayTypeName = "医保卡";

                payTypeZg1.PayAmount = inReimPara.SettleInfo.MedAmountZhzf;//Convert.ToDecimal(setRet["本次帐户支付"]);// Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["统筹支付金额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"]);


                inReimPara.PayTypeList.Add(payType1);
                inReimPara.PayTypeList.Add(payTypeZg1);

                #region 显示返回的预结算信息

                string strRePreSettle1 = "";
                //
                //strRePreSettle += "1.本次医疗总费用：" + Convert.ToDecimal(setRet["医疗费总额"]) + "\n";
                //strRePreSettle += "2.本次个人自费金额：" + Convert.ToDecimal(setRet["自费费用"]) + "\n";
                //strRePreSettle += "5.本次进入统筹金额：" + Convert.ToDecimal(setRet["进入统筹费用"]) + "\n";
                //strRePreSettle += "42.本次账户支出金额：" + Convert.ToDecimal(setRet["本次帐户支付"]) + "\n";
                //strRePreSettle += "43.个人现金支付金额：" + Convert.ToDecimal(setRet["本次现金支付"]) + "\n";
                //strRePreSettle += "44.乙类项目自理金额：" + Convert.ToDecimal(setRet["乙类自理费用"]) + "\n";
                //strRePreSettle += "45.救助金支出金额：" + Convert.ToDecimal(setRet["救助金支付"]) + "\n";
                //strRePreSettle += "46.公务员补助支出金额：" + Convert.ToDecimal(setRet["公务员补助支付"]) + "\n";
                //strRePreSettle += "47.企业补充基金支付：" + Convert.ToDecimal(setRet["企业补充基金支付"]) + "\n";
                //strRePreSettle += "48.统筹支付金额：" + Convert.ToDecimal(setRet["统筹支付金额"]);
                strRePreSettle1 += "1.本次医疗总费用：" + inReimPara.SettleInfo.Amount + "\n";
              //  strRePreSettle += "2.本次个人自费金额：" + inReimPara.SettleInfo. + "\n";
             //   strRePreSettle += "5.本次进入统筹金额：" +  + "\n";
                strRePreSettle1 += "42.本次账户支出金额：" +inReimPara.SettleInfo.MedAmountZhzf + "\n";
                strRePreSettle1 += "43.个人现金支付金额：" + inReimPara.SettleInfo.GetAmount + "\n";
             //   strRePreSettle += "44.乙类项目自理金额：" + Convert.ToDecimal(setRet["乙类自理费用"]) + "\n";
              //  strRePreSettle += "45.救助金支出金额：" + Convert.ToDecimal(setRet["救助金支付"]) + "\n";
                strRePreSettle1 += "46.公务员补助支出金额：" +inReimPara.SettleInfo.MedAmountGwy + "\n";
              //  strRePreSettle += "47.企业补充基金支付：" + Convert.ToDecimal(setRet["企业补充基金支付"]) + "\n";
                strRePreSettle1 += "48.统筹支付金额：" + inReimPara.SettleInfo.MedAmountTc + "\n";
                strRePreSettle1 += "49.医疗机构减免：" + inReimPara.SettleInfo.MedAmountJm + "\n";
                strRePreSettle1 += "50.商业补充保险支出：" + inReimPara.SettleInfo.AmountPos + "\n";
                strRePreSettle1 += "51.民政支出：" +inReimPara.SettleInfo.MedAmountBz;
                //      {"民政救助支出",retArr[39]},
                //     {"民政特大救助支出",retArr[40]},
                //

                MessageBox.Show(strRePreSettle1);
                return;

                #endregion

                //    //PayType _payType = new PayType();
            //    //_payType.PayTypeId = 4;
            //    //_payType.PayTypeName = "医保";
            //    //_payType.PayAmount = inReimPara.SettleInfo.MedAmountTotal - inReimPara.SettleInfo.MedAmountJm;
            //    //inReimPara.PayTypeList = new List<PayType>();
            //    //inReimPara.PayTypeList.Add(_payType);

            //    //_payType.PayTypeId = 19;
            //    //_payType.PayTypeName = "医院减免";
            //    //_payType.PayAmount = inReimPara.SettleInfo.MedAmountJm;
            //    //inReimPara.PayTypeList = new List<PayType>();
            //    //inReimPara.PayTypeList.Add(_payType);

            //    //付费方式 本接口 4 医保 6农合
            //   // PayAPIInterface.Model.Comm.PayType payType;
            //    inReimPara.PayTypeList = new List<PayType>();
            //    payType = new PayAPIInterface.Model.Comm.PayType();
            //    //payType.PayTypeId = payTypeId;
            //    //payType.PayTypeName = "医保";


            //    payType.PayTypeId = 5;
            //    payType.PayTypeName = "医保卡";
            //    payType.PayAmount = inReimPara.SettleInfo.MedAmountZhzf;//InPayPara.SettleInfo.MedAmountTotal - InPayPara.SettleInfo.MedAmountJm;
            //  //  payType.PayAmount = Convert.ToDecimal(dicSettleInfo["本次帐户支付"]);// Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["统筹支付金额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"]);


            //    payType.PayTypeId = 4;
            //    payType.PayTypeName = "医保";

            //    payType.PayAmount = inReimPara.SettleInfo.MedAmountTotal;
            //        //Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"]); //- Convert.ToDecimal(dicSettleInfo["本次帐户支付"]);// Convert.ToDecimal(dicSettleInfo["统筹支付金额"]);//Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"]);

            //    inReimPara.PayTypeList.Add(payType);
            //    return;
            }
            
            
            if (string.IsNullOrEmpty(inReimPara.RegInfo.Memo1))
            {
                throw new Exception("获取联网登记信息卡片类别失败，请检查登记信息" + inReimPara.RegInfo.Memo1+inReimPara.RegInfo.PatInHosId);
            }
            //删除费用
            DeleteAllFee(inReimPara);
            //上传费用
            UploadInFee(inReimPara);
            //预结算
            var inSetStr = GetInSettleStr(inReimPara);

            var setImpStr = _liYiNeusoftHandle.GetImpString(inReimPara.RegInfo.Memo1 == "true" ? "2450" : "2420", inReimPara.PatInfo.PatInHosId.ToString(), inSetStr, false, oldNewPatientMarkNetSerial);
            var setRet = _liYiNeusoftHandle.PreSettle(setImpStr);


            //付费方式 本接口 4 医保 6农合
            PayAPIInterface.Model.Comm.PayType payType;
             PayAPIInterface.Model.Comm.PayType payTypeZg;
            inReimPara.PayTypeList = new List<PayType>();

            payType = new PayAPIInterface.Model.Comm.PayType();


            payTypeZg = new PayAPIInterface.Model.Comm.PayType();
            //payType.PayTypeId = payTypeId;
            //payType.PayTypeName = "医保";

            if (string.IsNullOrEmpty(setRet["救助金支付"]))
            {
                setRet["救助金支付"] = "0";
            }
            if (string.IsNullOrEmpty(setRet["公务员补助支付"]))
            {
                setRet["公务员补助支付"] = "0";
            }
            if (string.IsNullOrEmpty(setRet["企业补充基金支付"]))
            {
                setRet["企业补充基金支付"] = "0";
            }
            if (string.IsNullOrEmpty(setRet["医疗机构减免"]))
            {
                setRet["医疗机构减免"] = "0";
            }
            if (string.IsNullOrEmpty(setRet["商业补充保险支出"]))
            {
                setRet["商业补充保险支出"] = "0";
            }
            if (string.IsNullOrEmpty(setRet["民政救助支出"]))
            {
                setRet["民政救助支出"] = "0";
            }
            if (string.IsNullOrEmpty(setRet["民政特大救助支出"]))
            {
                setRet["民政特大救助支出"] = "0";
            }
            LogManager.Debug("开始返回信息给页面>>>>");
            payType.PayTypeId = 4;
            payType.PayTypeName = "医保";

            payType.PayAmount = Convert.ToDecimal(setRet["医疗费总额"]) - Convert.ToDecimal(setRet["本次现金支付"]) - Convert.ToDecimal(setRet["本次帐户支付"]);// Convert.ToDecimal(dicSettleInfo["统筹支付金额"]);//Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"]);


            payTypeZg.PayTypeId = 5;
            payTypeZg.PayTypeName = "医保卡";

            payTypeZg.PayAmount = Convert.ToDecimal(setRet["本次帐户支付"]);// Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["统筹支付金额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"]);


            inReimPara.PayTypeList.Add(payType);
            inReimPara.PayTypeList.Add(payTypeZg);
            //if (inReimPara.RegInfo.Memo1 == "true")
            //{
            //    // payType.PayAmount = Convert.ToDecimal(setRet["本次帐户支付"]);
            //    payType.PayAmount = Convert.ToDecimal(setRet["医疗费总额"]) - Convert.ToDecimal(setRet["本次现金支付"]) - Convert.ToDecimal(setRet["本次帐户支付"]);

            //}
            //else
            //{
            //    payType.PayAmount = Convert.ToDecimal(setRet["医疗费总额"]) - Convert.ToDecimal(setRet["本次现金支付"]) - Convert.ToDecimal(setRet["本次帐户支付"]);
            //}

           // inReimPara.PayTypeList.Add(payType);

            #region 显示返回的预结算信息

            string strRePreSettle = "";
            //
            //strRePreSettle += "1.本次医疗总费用：" + Convert.ToDecimal(setRet["医疗费总额"]) + "\n";
            //strRePreSettle += "2.本次个人自费金额：" + Convert.ToDecimal(setRet["自费费用"]) + "\n";
            //strRePreSettle += "5.本次进入统筹金额：" + Convert.ToDecimal(setRet["进入统筹费用"]) + "\n";
            //strRePreSettle += "42.本次账户支出金额：" + Convert.ToDecimal(setRet["本次帐户支付"]) + "\n";
            //strRePreSettle += "43.个人现金支付金额：" + Convert.ToDecimal(setRet["本次现金支付"]) + "\n";
            //strRePreSettle += "44.乙类项目自理金额：" + Convert.ToDecimal(setRet["乙类自理费用"]) + "\n";
            //strRePreSettle += "45.救助金支出金额：" + Convert.ToDecimal(setRet["救助金支付"]) + "\n";
            //strRePreSettle += "46.公务员补助支出金额：" + Convert.ToDecimal(setRet["公务员补助支付"]) + "\n";
            //strRePreSettle += "47.企业补充基金支付：" + Convert.ToDecimal(setRet["企业补充基金支付"]) + "\n";
            //strRePreSettle += "48.统筹支付金额：" + Convert.ToDecimal(setRet["统筹支付金额"]);
            strRePreSettle += "1.本次医疗总费用：" + Convert.ToDecimal(setRet["医疗费总额"]) + "\n";
            strRePreSettle += "2.本次个人自费金额：" + Convert.ToDecimal(setRet["自费费用"]) + "\n";
            strRePreSettle += "5.本次进入统筹金额：" + Convert.ToDecimal(setRet["进入统筹费用"]) + "\n";
            strRePreSettle += "42.本次账户支出金额：" + Convert.ToDecimal(setRet["本次帐户支付"]) + "\n";
            strRePreSettle += "43.个人现金支付金额：" + Convert.ToDecimal(setRet["本次现金支付"]) + "\n";
            strRePreSettle += "44.乙类项目自理金额：" + Convert.ToDecimal(setRet["乙类自理费用"]) + "\n";
            strRePreSettle += "45.救助金支出金额：" + Convert.ToDecimal(setRet["救助金支付"]) + "\n";
            strRePreSettle += "46.公务员补助支出金额：" + Convert.ToDecimal(setRet["公务员补助支付"]) + "\n";
            strRePreSettle += "47.企业补充基金支付：" + Convert.ToDecimal(setRet["企业补充基金支付"]) + "\n";
            strRePreSettle += "48.统筹支付金额：" + Convert.ToDecimal(setRet["统筹支付金额"]) + "\n";
            strRePreSettle += "49.医疗机构减免：" + Convert.ToDecimal(setRet["医疗机构减免"]) + "\n";
            strRePreSettle += "50.商业补充保险支出：" + Convert.ToDecimal(setRet["商业补充保险支出"])+"\n";
            strRePreSettle += "51.民政支出：" + Convert.ToDecimal(setRet["民政救助支出"]) + Convert.ToDecimal(setRet["民政特大救助支出"]);
            //      {"民政救助支出",retArr[39]},
            //     {"民政特大救助支出",retArr[40]},
            //

            MessageBox.Show(strRePreSettle);
            LogManager.Debug("返回结束.");
            #endregion


            ////门诊付费方式 本接口 4 医保 6农合
            //PayAPIInterface.Model.Comm.PayType payType;
            //inReimPara.PayTypeList = new List<PayType>();
            //outReimPara.PayTypeList = new List<PayType>();
            //payType = new PayAPIInterface.Model.Comm.PayType();
            //payType.PayTypeId = 4;
            //payType.PayTypeName = "医保";
            //payType.PayAmount = Convert.ToDecimal(setRet["医疗费总额"]) - Convert.ToDecimal(setRet["本次现金支付"]) - Convert.ToDecimal(setRet["本次帐户支付"]);
            //outReimPara.PayTypeList.Add(payType);

            ////门诊付费方式 本接口 4 医保 6农合
            //payType = new PayAPIInterface.Model.Comm.PayType();
            //payType.PayTypeId = 5;
            //payType.PayTypeName = "医保卡";
            //payType.PayAmount = Convert.ToDecimal(setRet["本次帐户支付"]);
            //inReimPara.PayTypeList.Add(payType);
        }
        /// <summary>
        /// 住院结算
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void InNetworkSettle(InPayParameter para)
        {
            inReimPara = para;

            if (inReimPara.SettleInfo != null && inReimPara.SettleInfo.MedAmountTotal != 0)
            {
                LogManager.Debug("监测到接口结算表存在结算数据,跳过东软结算.医保农合支付总额:" + inReimPara.SettleInfo.MedAmountTotal + "住院ID" + inReimPara.SettleInfo.PatInHosId + "姓名" + inReimPara.RegInfo.NetPatName);
                //付费方式 本接口 4 医保 6农合
                PayAPIInterface.Model.Comm.PayType payType2;
                PayAPIInterface.Model.Comm.PayType payTypeZg2;
                inReimPara.PayTypeList = new List<PayType>();

                payType2 = new PayAPIInterface.Model.Comm.PayType();


                payTypeZg2 = new PayAPIInterface.Model.Comm.PayType();
                //payType.PayTypeId = payTypeId;
                //payType.PayTypeName = "医保";

                payType2.PayTypeId = 4;
                payType2.PayTypeName = "医保";

                payType2.PayAmount = inReimPara.SettleInfo.Amount - inReimPara.SettleInfo.GetAmount - inReimPara.SettleInfo.MedAmountZhzf; //Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"]) - Convert.ToDecimal(dicSettleInfo["本次帐户支付"]);// Convert.ToDecimal(dicSettleInfo["统筹支付金额"]);//Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"]);


                payTypeZg2.PayTypeId = 5;
                payTypeZg2.PayTypeName = "医保卡";

                payTypeZg2.PayAmount = inReimPara.SettleInfo.MedAmountZhzf; //Convert.ToDecimal(dicSettleInfo["本次帐户支付"]);// Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["统筹支付金额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"]);


                inReimPara.PayTypeList.Add(payType2);
                inReimPara.PayTypeList.Add(payTypeZg2); 
                
                return;
            }

            //查询医保端费用总额和本地费用总额是否一致
            if (string.IsNullOrEmpty(inReimPara.RegInfo.NetRegSerial))//新病人
            {
                var acImpStr = _liYiNeusoftHandle.GetImpString("1210", inReimPara.PatInfo.PatInHosId.ToString(), inReimPara.RegInfo.PatInHosSerial, false, "");
                decimal ybAmount = Convert.ToDecimal(_liYiNeusoftHandle.AccountDic(acImpStr)["费用总额"]);
                decimal hisAmount = _liYiNeusoftHandle.GetAmountByPatInHosId(inReimPara.PatInfo.PatInHosId.ToString());

                if (ybAmount != hisAmount)
                {
                    decimal yb_his_divid = ybAmount - hisAmount;
                    if (yb_his_divid < 0.3m && yb_his_divid > 0)
                    {
                        LogManager.Debug("HIS金额与医保金额不一致,医保总额:" + ybAmount + ",HIS总额:" + hisAmount);
                    }
                    else
                    {
                        throw new Exception("HIS金额与医保金额不一致,医保总额:" + ybAmount + ",HIS总额:" + hisAmount + ".请重试或联系管理员!");
                    }
                }
            }
            else//老病人
            {
                var acImpStr = _liYiNeusoftHandle.GetImpString("1210", inReimPara.PatInfo.PatInHosId.ToString(), inReimPara.RegInfo.NetRegSerial, false, inReimPara.RegInfo.NetRegSerial);
                decimal ybAmount = Convert.ToDecimal(_liYiNeusoftHandle.AccountDic(acImpStr)["费用总额"]);
                decimal hisAmount = _liYiNeusoftHandle.GetAmountByPatInHosId(inReimPara.PatInfo.PatInHosId.ToString());
                if (ybAmount != hisAmount)
                {

                    decimal yb_his_divid = ybAmount - hisAmount;
                    if (yb_his_divid < 0.3m && yb_his_divid>0)
                    {
                        LogManager.Debug("HIS金额与医保金额不一致,医保总额:" + ybAmount + ",HIS总额:" + hisAmount);
                    }
                    else
                    {
                        throw new Exception("HIS金额与医保金额不一致,医保总额:" + ybAmount + ",HIS总额:" + hisAmount + ".请重试或联系管理员!");
                    }
                
                }
            }


            //进行结算
            var inSetStr = GetInSettleStr(inReimPara);
            var setImpStr = _liYiNeusoftHandle.GetImpString(inReimPara.RegInfo.Memo1 == "true" ? "2410" : "2411", inReimPara.PatInfo.PatInHosId.ToString(), inSetStr, false, inReimPara.RegInfo.NetRegSerial);
            dicSettleInfo = _liYiNeusoftHandle.OffSettle(setImpStr);
            //如果银行扣款成功，则保存结算信息  否则 撤销本次结算
            if (dicSettleInfo["银行交易成功标志"] == "0")
            {
                SaveInSettleMain();

            }
            else
            {
                var celSetStr = GetCancelInSettleStr(inReimPara);
                var celSetImpStr = _liYiNeusoftHandle.GetImpString(inReimPara.RegInfo.Memo1 == "true" ? "2430" : "2431", inReimPara.PatInfo.PatInHosId.ToString(), celSetStr, false, inReimPara.RegInfo.NetRegSerial);
                var celSetRet = _liYiNeusoftHandle.BussinessHandle(celSetImpStr);
                LogManager.Debug("银行扣款失败，已撤销本次医保结算。请重新尝试!");
                throw new Exception("银行扣款失败，已撤销本次医保结算。请重新尝试!");
            }

        }
        /// <summary>
        /// 撤销住院结算
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void CancelInNetworkSettle(InPayParameter para)
        {
            inReimPara = para;
            InterfaceInit();
            DialogResult re = MessageBox.Show("即将开始执行撤销结算,请确认医保卡是否插入!", "提示", MessageBoxButtons.OKCancel,
                 MessageBoxIcon.Information);
            if (re == DialogResult.OK)
            {
                if (inReimPara.SettleInfo == null)
                    throw new Exception("本地结算信息不存在");
                if (string.IsNullOrEmpty(inReimPara.RegInfo.Memo1))
                {
                    throw new Exception("查询HIS登记的有无卡信息失败，请检查");
                }
                try
                {
                    if (inReimPara.RegInfo.Memo1 == "false")
                    {
                        var celSetStr = GetCancelInSettleStr(inReimPara);
                        var celSetImpStr = _liYiNeusoftHandle.GetImpString(inReimPara.RegInfo.Memo1 == "true" ? "2430" : "2431", inReimPara.PatInfo.PatInHosId.ToString(), celSetStr, false, inReimPara.RegInfo.NetRegSerial);
                        var celSetRet = _liYiNeusoftHandle.BussinessHandle(celSetImpStr);
                        LogManager.Info("医保撤销成功");

                    }
                    else
                    {   //银行退款
                        var celBankTranStr = GetBankTransZyStr(inReimPara);
                        if (celBankTranStr.Length > 0)
                        {
                            var celBankImpStr = _liYiNeusoftHandle.GetImpString("2900", inReimPara.PatInfo.PatInHosId.ToString(),
                            celBankTranStr, false, inReimPara.RegInfo.NetRegSerial);
                            try
                            {
                                var celBankRet = _liYiNeusoftHandle.BankTrans(celBankImpStr);
                                _liYiNeusoftHandle.SaveBankTransInfo(inReimPara.PatInfo.PatInHosId.ToString(), inReimPara.SettleInfo.InNetworkSettleId.ToString(), celBankRet, "ZY");
                                if (celBankRet["返回码"] != "000000")
                                {
                                    LogManager.Debug(celBankRet["返回码含义"]);

                                    throw new Exception(celBankRet["返回码含义"]);
                                }
                                LogManager.Info("银行退款成功");
                            }
                            catch (Exception ex)
                            {
                                DialogResult dr = MessageBox.Show("银行撤销失败,是否继续？点击 是 将继续撤销医保结算,银行交易请手工进行撤销。点击 否 将终止本次撤销。",
                                    "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (dr == DialogResult.No)
                                {
                                    LogManager.Debug("银行撤销交易失败." + ex.Message);

                                    throw new Exception("银行撤销交易失败." + ex.Message);
                                }
                            }
                        }

                        var celSetStr = GetCancelInSettleStr(inReimPara);
                        var celSetImpStr = _liYiNeusoftHandle.GetImpString(inReimPara.RegInfo.Memo1 == "true" ? "2430" : "2431", inReimPara.PatInfo.PatInHosId.ToString(), celSetStr, false, inReimPara.RegInfo.NetRegSerial);
                        var celSetRet = _liYiNeusoftHandle.BussinessHandle(celSetImpStr);
                        LogManager.Info("医保撤销成功");

                    }
                }
                catch (Exception ex)
                {
                    LogManager.Info(ex.Message);
                    throw new Exception("撤销失败!" + ex.Message);
                }
            }
            else
            {
                throw new Exception("用户取消了操作!");

            }
        }
        /// <summary>
        ///  获取住院登记交易串
        /// </summary>
        /// <param name="inPara"></param>
        /// <returns></returns>
        private string GetInRegistStr(InPayParameter inPara)
        {
            //住院登记
            var inRegList = new List<string>();
            //住院流水号
            if (inPara.RegInfo.PatInHosSerial == null)
            {
                throw new Exception("HIS联网登记流水号为空，请检查登记");
            }
            inRegList.Add(inPara.RegInfo.PatInHosSerial);
            LogManager.Debug("》》住院登记流水号：" + inPara.RegInfo.PatInHosSerial + ">>");
            //医疗类别
            //   inRegList.Add(inPara.RegInfo.Memo2);
            inRegList.Add(inPara.RegInfo.NetType);

            //挂号日期
            //   inRegList.Add(DateTime.Now.ToString("yyyyMMddHHmmss"));
            inRegList.Add(inPara.PatInfo.InDateTime.ToString("yyyyMMddHHmmss"));

            //诊断疾病编码
            inRegList.Add(inPara.RegInfo.NetDiagnosCode);
            //诊断疾病名称
            inRegList.Add(inPara.RegInfo.NetDiagnosName);
            //科室名称
            inRegList.Add(inPara.PatInfo.InDeptName);
            //床位号
            inRegList.Add(inPara.PatInfo.InBedNo);
            //医生代码
            inRegList.Add(inPara.PatInfo.DoctorCode);
            //医生姓名
            inRegList.Add(inPara.PatInfo.DoctorName);
            //挂号费
            inRegList.Add("0");
            //检查费
            inRegList.Add("0");
            //经办人
            inRegList.Add(operatorInfo.UserName);
            //卡号 
            inRegList.Add(inPara.RegInfo.CardNo);
            //个人编号
            inRegList.Add(inPara.RegInfo.MemberNo);
            return string.Join("|", inRegList.ToArray());
        }
        /// <summary>
        /// 删除费用
        /// </summary>
        /// <param name="inPara"></param>
        private void DeleteAllFee(InPayParameter inPara)
        {//老病人
            if (!string.IsNullOrEmpty(inReimPara.RegInfo.NetRegSerial))
            {
                var delFeeStr = inReimPara.RegInfo.NetRegSerial + "||" + operatorInfo.UserName;
                var delImpStr = _liYiNeusoftHandle.GetImpString("2320", inPara.PatInfo.PatInHosId.ToString(), delFeeStr, false, inReimPara.RegInfo.NetRegSerial);
                var delRet = _liYiNeusoftHandle.ZZCHEBussinessHandle(delImpStr);
            }
            //新病人
            else
            {
                var delFeeStr = inReimPara.RegInfo.PatInHosSerial + "||" + operatorInfo.UserName;
                var delImpStr = _liYiNeusoftHandle.GetImpString("2320", inPara.PatInfo.PatInHosId.ToString(), delFeeStr, false, inReimPara.RegInfo.NetRegSerial);
                var delRet = _liYiNeusoftHandle.ZZCHEBussinessHandle(delImpStr);
            }
        }
        /// <summary>
        ///  上传住院费用
        /// </summary>
        /// <param name="inPara"></param>
        private void UploadInFee(InPayParameter inPara)
        {

            inPara.Details = PayAPIUtilities.Tools.CommonTools.GetGroupList(inPara.Details);
            //费用串 允许同时上传多条处方明细，多条处方间以$分隔
            //var chargeStr = new StringBuilder();
            string notMatchedCharge = "";
            foreach (var item in inPara.Details)
            {
                if (string.IsNullOrEmpty(item.NetworkItemCode.ToString()))
                {
                    notMatchedCharge += "编码:" + item.ChargeCode + "," + "名称:" + item.ChargeName + "；";
                }
            }
            if (notMatchedCharge.Trim().Length > 0)
            {
                DialogResult diag = MessageBox.Show("有以下项目未对应：\n" + notMatchedCharge + ",是否继续上传。继续上传将按自费进行处理!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (diag == DialogResult.No)
                {
                    throw new Exception("有以下项目未对应：\n" + notMatchedCharge + ",取消上传！");
                }
            }
            if (inPara.Details.Count > 0)
            {
                foreach (var chargeRow in inPara.Details)
                {
                    if (chargeRow.Quantity > 0)
                    {
                        if (string.IsNullOrEmpty(chargeRow.NetworkItemCode.ToString().Trim()))
                        {
                            //continue;
                            chargeRow.NetworkItemCode = "0";
                            int chargeT = 0;
                            if (int.TryParse(chargeRow.ChargeType.ToString(), out chargeT))
                            {
                                if (Convert.ToInt32(chargeRow.ChargeType.ToString()) < 100)
                                {
                                    chargeRow.NetworkItemProp = "1";//1药品、2诊疗项目
                                }
                                else
                                {
                                    chargeRow.NetworkItemProp = "2";//1药品、2诊疗项目
                                }
                                chargeRow.NetworkItemClass = "91";//其他费用    
                            }
                            else
                            {
                                chargeRow.NetworkItemProp = "1";//1药品、2诊疗项目
                                chargeRow.NetworkItemClass = "91";//其他费用    

                            }

                        }

                        var chargeList = new List<string>();
                        //就诊流水号
                        //    chargeList.Add(inPara.RegInfo.PatInHosSerial.ToString());
                        //新病人
                        if (string.IsNullOrEmpty(inPara.RegInfo.NetRegSerial))
                        {
                            chargeList.Add(inPara.RegInfo.PatInHosSerial.ToString());

                        }
                        else//老病人
                        {
                            chargeList.Add(inPara.RegInfo.NetRegSerial);

                        }

                        //收费项目种类
                        chargeList.Add(chargeRow.NetworkItemProp.ToString());
                        //收费类别
                        chargeList.Add(chargeRow.NetworkItemClass.ToString());
                        //处方号
                        // chargeList.Add(chargeRow.OrderId.ToString()+DateTime.Now.ToString("ss"));
                        chargeList.Add(chargeRow.AutoId.ToString());

                        //处方日期
                        chargeList.Add(Convert.ToDateTime(chargeRow.CreateTime).ToString("yyyyMMddHHmmss"));
                        //医院收费项目内码
                        chargeList.Add(chargeRow.ChargeCode.ToString());
                        //收费项目中心编码
                        chargeList.Add(chargeRow.NetworkItemCode.ToString());
                        //医院收费项目名称
                        chargeList.Add(chargeRow.ChargeName.ToString());
                        //单价
                        decimal chargeDJ = chargeRow.Amount / chargeRow.Quantity;
                        chargeDJ = Math.Round(chargeDJ, 6);
                        chargeList.Add(chargeDJ.ToString());
                        //数量
                        chargeList.Add(chargeRow.Quantity.ToString());
                        //金额
                        //double chargeAmount = Math.Round(double.Parse(chargeRow.Price.ToString()) * double.Parse(chargeRow.Quantity.ToString()), 2);
                        //chargeList.Add(String.Format("{0:F}", chargeAmount));
                        //  double chargeAmount = Math.Round(double.Parse(chargeRow.Price.ToString()),2 )* double.Parse(chargeRow.Quantity.ToString());
                        //chargeList.Add(String.Format("{0:F}", chargeAmount));
                        //  chargeList.Add(chargeAmount.ToString());
                        chargeList.Add(chargeRow.Amount.ToString());
                        //剂型 二级代码，非药品传999 暂时为传 
                        chargeList.Add(chargeRow.NetworkItemProp.ToString() == "1" ? "0" : "999");
                        //规格
                        chargeList.Add(string.IsNullOrEmpty(chargeRow.Spec.ToString()) ? "/" : chargeRow.Spec.ToString());
                        //每次用量
                        chargeList.Add("0");
                        //使用频次
                        chargeList.Add("0");
                        //医师代码
                        chargeList.Add(chargeRow.DocCode.ToString());
                        //医师姓名
                        chargeList.Add(chargeRow.DocCode.ToString());
                        //用法
                        chargeList.Add("");
                        //单位
                        chargeList.Add("");
                        //科室编号
                        chargeList.Add(chargeRow.DeptCode.ToString());
                        //科室名称
                        chargeList.Add(chargeRow.DeptCode.ToString());
                        //执行天数
                        chargeList.Add("0");
                        //经办人
                        chargeList.Add(operatorInfo.UserName);
                        //药品剂量单位
                        chargeList.Add(chargeRow.Unit.ToString());
                        //上传
                        var impStr = _liYiNeusoftHandle.GetImpString("2310", inPara.PatInfo.PatInHosId.ToString(),
                            string.Join("|", chargeList.ToArray()), false, inReimPara.RegInfo.NetRegSerial);
                        var outFeeDic = _liYiNeusoftHandle.UploadOutFee(impStr);
                        chargeRow.AmountSelf = decimal.Parse(outFeeDic["自理金额"]);
                        chargeRow.AmountSelfBurdern = decimal.Parse(outFeeDic["自费金额"]);
                    }
                }
            }
        


        }
        /// <summary>
        ///  结算交易串
        /// </summary>
        /// <param name="inPara"></param>
        /// <returns></returns>
        private string GetInSettleStr(InPayParameter inPara)
        {
            //取单据号 唯一
            inReimPara.SettleInfo.SettleNo = DateTime.Now.ToString("yyyyMMddHHmmss");
            var inSettleList = new List<string>();
            //住院流水号(门诊流水号)
            //inSettleList.Add(inPara.RegInfo.PatInHosSerial.ToString());
            //老病人
            if (!string.IsNullOrEmpty(inPara.RegInfo.NetRegSerial))
            {
                inSettleList.Add(inPara.RegInfo.NetRegSerial);
                oldNewPatientMarkNetSerial = inPara.RegInfo.NetRegSerial;
            }
            else//新病人
            {
                inSettleList.Add(inPara.RegInfo.PatInHosSerial.ToString());
                oldNewPatientMarkNetSerial = "";
            }
            //单据号
            //            inSettleList.Add(inPara.SettleInfo.SettleNo);
            inSettleList.Add(inPara.SettleInfo.SettleNo);

            LogManager.Debug("住院流水号>>:" + inPara.RegInfo.PatInHosSerial.ToString() + "单据号：》》" + inPara.SettleInfo.SettleNo);
            //医疗类别
            //inSettleList.Add(inPara.RegInfo.Memo2);
            inSettleList.Add(inPara.RegInfo.NetType);

            //结算日期
            inSettleList.Add(DateTime.Now.ToString("yyyyMMddHHmmss"));
            //出院日期
            inSettleList.Add(inPara.PatInfo.OutDateTime.ToString("yyyyMMddHHmmss"));
            //  inSettleList.Add(DateTime.Now.ToString("yyyyMMddHHmmss"));
            //出院原因
            inSettleList.Add("");
            //出院诊断疾病编码
            //inSettleList.Add(inPara.RegInfo.OutDiagnoseCode);
            inSettleList.Add(inPara.RegInfo.NetDiagnosCode);

            //出院诊断疾病名称
            //   inSettleList.Add(inPara.RegInfo.NetDiagnosName);
            inSettleList.Add(inPara.RegInfo.NetDiagnosName);

            //账户使用标志
            inSettleList.Add("1");
            //中途结算标志
            //     inSettleList.Add(inPara.SettleInfo.IsSettle ? "1" : "0");
            inSettleList.Add("");

            //经办人
            inSettleList.Add(operatorInfo.UserName);
            //开发商标志
            inSettleList.Add("msun");
            return string.Join("|", inSettleList.ToArray());
        }
        /// <summary>
        ///  撤销结算交易串
        /// </summary>
        /// <param name="inPara"></param>
        /// <returns></returns>
        private string GetCancelInSettleStr(InPayParameter inPara)
        {
            var inCancelSettleList = new List<string>();
            //住院流水号(门诊流水号)
            //inCancelSettleList.Add(inPara.PatInfo.PatInHosId.ToString());
            if (string.IsNullOrEmpty(inPara.RegInfo.NetRegSerial))//新病人
            {
                inCancelSettleList.Add(inPara.RegInfo.PatInHosSerial);

            }
            else//老病人
            {
                inCancelSettleList.Add(inPara.RegInfo.NetRegSerial);

            }

            //单据号
            inCancelSettleList.Add(inPara.SettleInfo.SettleNo);
            //结算日期
            inCancelSettleList.Add(DateTime.Now.ToString("yyyyMMddHHmmss"));
            //经办人
            inCancelSettleList.Add(operatorInfo.UserName);
            //是否保存处方标志
            inCancelSettleList.Add("0");
            //开发商标志
            inCancelSettleList.Add("msun");
            //20160720 add 备用字段
            //交易编号
            inCancelSettleList.Add("");
            //社保交易编号
            inCancelSettleList.Add("");
            //Pos机器交易编号
            inCancelSettleList.Add("");
            //银行交易编号
            inCancelSettleList.Add("");
            //备用
            inCancelSettleList.Add("");
            return string.Join("|", inCancelSettleList.ToArray());
        }
        /// <summary>
        ///  撤销银行交易
        /// </summary>
        /// <param name="dt"></param>
        private string GetBankTransZyStr(InPayParameter inPara)
        {
            LinYiPosHandle handle = new LinYiPosHandle();
            string patInHosId = inPara.PatInfo.PatInHosId.ToString();
            string networkSettleId = inPara.SettleInfo.InNetworkSettleId.ToString();
            //获取银行交易信息
            DataTable bankDt = _liYiNeusoftHandle.GetBankInfoForZy(networkSettleId, patInHosId);
            //交易索引号 参考号 Pos机交易编号 原交易日期 银行交易成功标志 银行账号  
            var bjysyh = bankDt.Select("PARA_NAME='交易索引号'")[0]["PARA_VALUE"].ToString();
            var bckh = bankDt.Select("PARA_NAME='参考号'")[0]["PARA_VALUE"].ToString();
            var bposjybh = bankDt.Select("PARA_NAME='Pos机交易编号'")[0]["PARA_VALUE"].ToString();
            var byjyrq = bankDt.Select("PARA_NAME='原交易日期'")[0]["PARA_VALUE"].ToString();
            var byhzh = bankDt.Select("PARA_NAME='银行账号'")[0]["PARA_VALUE"].ToString();
            var byhjybz = bankDt.Select("PARA_NAME='银行交易成功标志'")[0]["PARA_VALUE"].ToString();
            if (bjysyh != "" && bckh != "" && bposjybh != "" && byhzh != "" && byhjybz == "0")
            {
                //撤销银行交易
                var insertTime = byjyrq;
                var bankAmount = Convert.ToString(Convert.ToInt32(Convert.ToDecimal(inPara.SettleInfo.MedAmountZhzf) * 100));

                handle.AddListInParas(insertTime != DateTime.Now.ToString("yyyyMMdd") ? "09" : "02", 2, new char[] { (' ') },
                    "L");
                handle.AddListInParas(bankAmount, 12, new char[] { ('0') }, "R");//此处为报销金额
                handle.AddListInParas(bposjybh, 6, new char[] { (' ') }, "L");//POS 流水号
                handle.AddListInParas("", 10, new char[] { (' ') }, "L");
                handle.AddListInParas("", 10, new char[] { (' ') }, "L");
                handle.AddListInParas(bckh, 15, new char[] { (' ') }, "L");   // 参考号 
                handle.AddListInParas("", 6, new char[] { (' ') }, "L");
                handle.AddListInParas(insertTime, 8, new char[] { (' ') }, "L");
                // 区分新旧卡 
                handle.AddListInParas("P", 1, new char[] { (' ') }, "L");
                handle.AddListInParas(bjysyh, 76, new char[] { (' ') }, "L"); // 交易索引号 
                //handle.AddListInParas(netPatInfo.ICNo, 37, new char[] { (' ') }, "L");
                handle.AddListInParas(byhzh, 37, new char[] { (' ') }, "L");    //银行卡号
                handle.AddListInParas("", 104, new char[] { (' ') }, "L");      //交易流水号
                handle.AddListInParas("", 2, new char[] { (' ') }, "L");
                handle.AddListInParas("", 15, new char[] { (' ') }, "L");
                handle.AddListInParas("", 6, new char[] { (' ') }, "L");
                handle.AddListInParas("", 3, new char[] { (' ') }, "L");
                handle.AddListInParas("", 20, new char[] { (' ') }, "L");
                handle.AddListInParas("", 30, new char[] { (' ') }, "L");
                handle.AddListInParas("", 15, new char[] { (' ') }, "L");
            }
            return handle.CommInput();
        }
        #endregion

        #region 保存门诊结算数据
        /// <summary>
        /// 保存门诊结算数据
        /// </summary>
        public void SaveOutSettleMain()
        {
            #region 保存农合中心返回值参数列表
            //保存农合中心返回值参数列表
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
                LogManager.Error("保存农合中心返回值参数列表 插入值 失败" + ex.Message, ex);
            }
            #endregion
            OutNetworkSettleMain outSettleMain = new OutNetworkSettleMain();

            //Int64 settlNoV = (Int64)Convert.ToDecimal(dicSettleInfo["单据号"].ToString());

            outSettleMain.SettleNo = dicSettleInfo["单据号"];
            outSettleMain.Amount = Convert.ToDecimal(dicSettleInfo["医疗费总额"]);
            outSettleMain.GetAmount = Convert.ToDecimal(dicSettleInfo["本次现金支付"]);
            outSettleMain.MedAmountZhzf = Convert.ToDecimal(dicSettleInfo["本次帐户支付"]);
            outSettleMain.MedAmountTc = Convert.ToDecimal(dicSettleInfo["统筹支付金额"]);
            outSettleMain.MedAmountDb = Convert.ToDecimal(dicSettleInfo["大病支付费用"]);
            // outSettleMain.MedAmountBz = Convert.ToDecimal(dicSettleInfo["救助金支付"]);
            // outSettleMain.MedAmountBz = Convert.ToDecimal(dicSettleInfo["救助金支付"]);
            outSettleMain.MedAmountBz = Convert.ToDecimal(dicSettleInfo["民政救助支出"]) + Convert.ToDecimal(dicSettleInfo["民政特大救助支出"]);       //救助金支出金额

            outSettleMain.MedAmountGwy = Convert.ToDecimal(dicSettleInfo["公务员补助支付"]);
            // outSettleMain.MedAmountJm = Convert.ToDecimal(dicSettleInfo["财政支出"]);
            outSettleMain.MedAmountJm = Convert.ToDecimal(dicSettleInfo["医疗机构减免"]);
            outSettleMain.AmountPos = Convert.ToDecimal(dicSettleInfo["商业补充保险支出"]);
                     //   outReimPara.SettleInfo.MedAmountTotal = Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"])// - Convert.ToDecimal(dicSettleInfo["本次帐户支付"]);
            outSettleMain.MedAmountTotal = Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"]);// - Convert.ToDecimal(dicSettleInfo["本次帐户支付"]);

            outSettleMain.CreateTime = DateTime.Now;
            outSettleMain.InvoiceId = -1;
            outSettleMain.IsCash = true;
            outSettleMain.IsInvalid = false;
            outSettleMain.IsNeedRefund = false;
            outSettleMain.IsRefundDo = false;
            outSettleMain.IsSettle = true;
            outSettleMain.OperatorId = operatorInfo.UserSysId;
            outSettleMain.NetworkingPatClassId = Convert.ToInt32(outReimPara.CommPara.NetworkPatClassId);
            outSettleMain.NetworkPatName = outReimPara.PatInfo.PatName;
            //  outSettleMain.NetworkPatType = "0";
            outSettleMain.NetworkPatType = outReimPara.RegInfo.NetPatType;//outReimPara.CommPara.NetworkPatClassId;// "2";

            outSettleMain.OutNetworkSettleId = Convert.ToDecimal(outReimPara.CommPara.OutNetworkSettleId);
            outSettleMain.SettleNo = dicSettleInfo["单据号"];//dicSettleInfo["单据号"];                    // 单据号
            outSettleMain.SettleBackNo = _liYiNeusoftHandle.GetBusNo();         // 医院流水号 对账使用
            outSettleMain.SettleType = "1";

            outReimPara.SettleInfo = outSettleMain;

            PayAPIInterface.Model.Comm.PayType payType;
            PayAPIInterface.Model.Comm.PayType payTypeZg;

            if (isOut)
            {
                //门诊付费方式 本接口 4 医保 6农合
                outReimPara.PayTypeList = new List<PayType>();

                payType = new PayAPIInterface.Model.Comm.PayType();
                payTypeZg = new PayAPIInterface.Model.Comm.PayType();


                payType.PayTypeId = 4;
                payType.PayTypeName = "医保";
                payType.PayAmount = Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"]) - Convert.ToDecimal(dicSettleInfo["本次帐户支付"]); //outReimPara.SettleInfo.MedAmountTotal;// outReimPara.SettleInfo.MedAmountTotal;
                LogManager.Debug("医保支付:" + payType.PayAmount);

                // payType.PayTypeId = 5;
                // payType.PayTypeName = "医保卡";
                //  outReimPara.SettleInfo.MedAmountTotal = Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"]) - Convert.ToDecimal(dicSettleInfo["本次帐户支付"]);
                // outReimPara.SettleInfo.MedAmountTotal = outSettleMain.MedAmountZhzf + outSettleMain.MedAmountTc + outSettleMain.MedAmountDb + outSettleMain.MedAmountBz + outSettleMain.MedAmountGwy + outSettleMain.MedAmountJm + outSettleMain.AmountPos;




                payTypeZg.PayTypeId = 5;
                payTypeZg.PayTypeName = "医保卡";

                payTypeZg.PayAmount = outSettleMain.MedAmountZhzf;
                LogManager.Debug("医保卡支付:" + payTypeZg.PayAmount);

            

                outReimPara.PayTypeList.Add(payType);
                outReimPara.PayTypeList.Add(payTypeZg);
                LogManager.Debug("门诊结算医保接口已全部结束");

            }
            else
            {
                //门诊付费方式 本接口 4 医保 6农合

                payType = new PayAPIInterface.Model.Comm.PayType();
                payTypeZg = new PayAPIInterface.Model.Comm.PayType();


                payType.PayTypeId = 4;
                payType.PayTypeName = "医保";
              //  outReimPara.SettleInfo.MedAmountTotal = Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"]) - Convert.ToDecimal(dicSettleInfo["本次帐户支付"]);
             //   PayAPIInterface.Model.Comm.PayType payTypeZg;
                payType.PayAmount = Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"]) - Convert.ToDecimal(dicSettleInfo["本次帐户支付"]); //outReimPara.SettleInfo.MedAmountTotal;// outReimPara.SettleInfo.MedAmountTotal;
                LogManager.Debug("医保支付:" + payType.PayAmount);
                //payType.PayTypeId = 5;
                //payType.PayTypeName = "医保卡";
               // payType.PayAmount = outSettleMain.MedAmountZhzf;

                payTypeZg.PayTypeId = 5;
                payTypeZg.PayTypeName = "医保卡";

                payTypeZg.PayAmount = Convert.ToDecimal(dicSettleInfo["本次帐户支付"]);// Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["统筹支付金额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"]);
                LogManager.Debug("医保卡支付:" + payType.PayAmount);

          
                //  outReimPara.SettleInfo.MedAmountTotal = Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"]) - Convert.ToDecimal(dicSettleInfo["本次帐户支付"]);
                // outSettleMain.MedAmountZhzf + outSettleMain.MedAmountTc + outSettleMain.MedAmountDb + outSettleMain.MedAmountBz + outSettleMain.MedAmountGwy + outSettleMain.MedAmountJm;
               // outReimPara.SettleInfo.MedAmountTotal = outSettleMain.MedAmountZhzf + outSettleMain.MedAmountTc + outSettleMain.MedAmountDb + outSettleMain.MedAmountBz + outSettleMain.MedAmountGwy + outSettleMain.MedAmountJm + outSettleMain.AmountPos;

                outReimPara.PayTypeList.Add(payType);
                outReimPara.PayTypeList.Add(payTypeZg);
                LogManager.Debug("end............");
            }


        }
        #endregion
        #region 保存住院结算数据
        /// <summary>
        /// 保存住院结算数据
        /// </summary>
        public void SaveInSettleMain()
        {
            LogManager.Debug("住院结算数据保存开始>>>");

            #region 保存中心返回值参数列表
            //保存中心返回值参数列表
            try
            {

                InNetworkSettleList inNetworkSettleList = new InNetworkSettleList();
                LogManager.Debug("住院Settlelist保存>>>");
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
                LogManager.Error("保存农合中心返回值参数列表 插入值 失败" + ex.Message, ex);
            }
            #endregion

            InNetworkSettleMain inSettleMain = new InNetworkSettleMain();
            inSettleMain.PatInHosId = inReimPara.PatInfo.PatInHosId;
            //inSettleMain.SettleNo = dicSettleInfo["中心交易流水号"];                        //医保中心交易流水号
            inSettleMain.SettleNo = dicSettleInfo["单据号"];                                    //医保中心交易流水号
            inSettleMain.Amount = Convert.ToDecimal(dicSettleInfo["医疗费总额"]);             //本次医疗费用
            inSettleMain.GetAmount = Convert.ToDecimal(dicSettleInfo["本次现金支付"]);       //本次现金支出
            inSettleMain.MedAmountZhzf = Convert.ToDecimal(dicSettleInfo["本次帐户支付"]);     //本次帐户支出
            inSettleMain.MedAmountTc = Convert.ToDecimal(dicSettleInfo["统筹支付金额"]);       //本次统筹支出

            // {"民政救助支出",retArr[39]},
            //   {"民政特大救助支出",retArr[40]},
            inSettleMain.MedAmountBz = Convert.ToDecimal(dicSettleInfo["民政救助支出"]) + Convert.ToDecimal(dicSettleInfo["民政特大救助支出"]);       //救助金支出金额
            inSettleMain.MedAmountGwy = Convert.ToDecimal(dicSettleInfo["公务员补助支付"]);      //公务员补助
            inSettleMain.MedAmountDb = Convert.ToDecimal(dicSettleInfo["大病支付费用"]);        //大病支付费用
            //inSettleMain.MedAmountJm = Convert.ToDecimal(dicSettleInfo["财政支出"]);            //财政支出
            inSettleMain.MedAmountJm = Convert.ToDecimal(dicSettleInfo["医疗机构减免"]);            //财政支出
            inSettleMain.AmountPos = Convert.ToDecimal(dicSettleInfo["商业补充保险支出"]);

            inSettleMain.MedAmountTotal = Convert.ToDecimal(inSettleMain.Amount) - Convert.ToDecimal(inSettleMain.GetAmount);// -Convert.ToDecimal(inSettleMain.MedAmountZhzf);
            inSettleMain.NetworkingPatClassId = Convert.ToInt32(inReimPara.CommPara.NetworkPatClassId);
            inSettleMain.CreateTime = DateTime.Now;
            inSettleMain.NetworkSettleTime = DateTime.Now;
            inSettleMain.InvoiceId = -1;
            inSettleMain.IsCash = true;
            inSettleMain.IsInvalid = false;
            inSettleMain.IsNeedRefund = false;
            inSettleMain.IsRefundDo = false;
            inSettleMain.NetworkPatName = inReimPara.PatInfo.InPatName;
            inSettleMain.OperatorId = operatorInfo.UserSysId;
            inSettleMain.NetworkPatType = inReimPara.RegInfo.NetPatType;// "0";
            inSettleMain.SettleBackNo = _liYiNeusoftHandle.GetBusNo();
            inSettleMain.SettleType = "1";



            inReimPara.SettleInfo = inSettleMain;
            //付费方式 本接口 4 医保 6农合
            //PayAPIInterface.Model.Comm.PayType payType;
            //inReimPara.PayTypeList = new List<PayType>();
            //payType = new PayAPIInterface.Model.Comm.PayType();
            //payType.PayTypeId = payTypeId;
            //payType.PayTypeName = "医保";
            //payType.PayAmount = Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"]) - Convert.ToDecimal(dicSettleInfo["本次帐户支付"]);
            //inReimPara.PayTypeList.Add(payType);

            LogManager.Debug("住院结算数据保存结束");
            //付费方式 本接口 4 医保 6农合
            PayAPIInterface.Model.Comm.PayType payType;
            PayAPIInterface.Model.Comm.PayType payTypeZg;
            inReimPara.PayTypeList = new List<PayType>();

            payType = new PayAPIInterface.Model.Comm.PayType();


            payTypeZg = new PayAPIInterface.Model.Comm.PayType();
            //payType.PayTypeId = payTypeId;
            //payType.PayTypeName = "医保";

            payType.PayTypeId = 4;
            payType.PayTypeName = "医保";

            payType.PayAmount = Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"]) - Convert.ToDecimal(dicSettleInfo["本次帐户支付"]);// Convert.ToDecimal(dicSettleInfo["统筹支付金额"]);//Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"]);

            LogManager.Debug("医保支付"+payType.PayAmount);
            payTypeZg.PayTypeId = 5;
            payTypeZg.PayTypeName = "医保卡";

            payTypeZg.PayAmount = Convert.ToDecimal(dicSettleInfo["本次帐户支付"]);// Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["统筹支付金额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"]);

            LogManager.Debug("医保卡支付" + payTypeZg.PayAmount);

            inReimPara.PayTypeList.Add(payType);
            inReimPara.PayTypeList.Add(payTypeZg);
            //PayAPIInterface.Model.Comm.PayType payType;

            LogManager.Debug("住院结算部分接口调用结束...");

            ////付费方式 本接口 4 医保 6农合
            //inReimPara.PayTypeList = new List<PayType>();
            //payType = new PayAPIInterface.Model.Comm.PayType();
            ////payType.PayTypeId = payTypeId;
            ////payType.PayTypeName = "医保";

            //payType.PayTypeId = 5;
            //payType.PayTypeName = "医保卡";

            //payType.PayAmount = Convert.ToDecimal(dicSettleInfo["本次帐户支付"]);// Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["统筹支付金额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"]);


            //payType.PayTypeId = 4;
            //payType.PayTypeName = "医保";

            //payType.PayAmount = Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"])- Convert.ToDecimal(dicSettleInfo["本次帐户支付"]);// Convert.ToDecimal(dicSettleInfo["统筹支付金额"]);//Convert.ToDecimal(dicSettleInfo["医疗费总额"]) - Convert.ToDecimal(dicSettleInfo["本次现金支付"]);

            //inReimPara.PayTypeList.Add(payType);
            ////门诊付费方式 本接口 4 医保 6农合
            //payType = new PayAPIInterface.Model.Comm.PayType();
            //payType.PayTypeId = 5;
            //payType.PayTypeName = "医保卡";
            //payType.PayAmount = Convert.ToDecimal(dicSettleInfo["本次帐户支付"]);
            //inReimPara.PayTypeList.Add(payType);
        }
        #endregion
    }
}