using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using PayAPIInterfaceHandle.Tools;
using PayAPIUtilities.Config;
using PayAPIInterface.Model.Comm;
using PayAPIUtilities.Log;
namespace PayAPIInterfaceHandle.Neusoft
{
    public class LiYiNeusoftHandle
    {
        #region 利用反射 获取对象
       // private RefCom LySiActiveX = new RefCom("LySiActiveX");
 
        private   RefCom LySiActiveX = new RefCom("LySiActive.LySiActiveX");
        //private RefCom LySiActiveX = new RefCom("clsid:7F1D82A9-C2E4-4ce6-B5BF-BD4382C148C1");
        //private RefCom LySiActiveX = new RefCom("7F1D82A9-C2E4-4ce6-B5BF-BD4382C148C1");

        #endregion

        #region 参数
        //兰陵// 
        //y//  MSSQLHelper _mssqlHelpers = new MSSQLHelper("Data Source=11.104.0.183;Initial Catalog=comm;Persist Security Info=True;User ID=power;Password=massunsoft009");
        // MSSQLHelper _mssqlHelpers = new MSSQLHelper("Data Source=11.104.0.6;Initial Catalog=comm;Persist Security Info=True;User ID=power;Password=massunsoft009");

        //沂南总
        // MSSQLHelper _mssqlHelpers = new MSSQLHelper("Data Source=172.12.12.102;Initial Catalog=comm;Persist Security Info=True;User ID=sa;Password=admin123");
        //k开发去
        MSSQLHelper _mssqlHelpers = new MSSQLHelper("Data Source=11.100.253.21;Initial Catalog=comm;Persist Security Info=True;User ID=power;Password=massunsoft009");
        //界湖
        //  MSSQLHelper _mssqlHelpers = new MSSQLHelper("Data Source=11.101.8.2;Initial Catalog=comm;Persist Security Info=True;User ID=power;Password=m@ssunsoft009");
        //费县        _mssqlHelpers
        //   MSSQLHelper _mssqlHelpers = new MSSQLHelper("Data Source=11.105.9.53;Initial Catalog=comm;Persist Security Info=True;User ID=power;Password=massunsoft009");

        public OperatorInfo operatorInfo = PayAPIConfig.Operator;//=new OperatorInfo();
           // =PayAPIConfig.Operator;
        //= PayAPIConfig.Operator;  //new OperatorInfo();



        /// <summary>
        ///     医疗机构编号
        /// </summary>
        public  string _yljgbh = PayAPIConfig.InstitutionDict[1].InstitutionCode.PadLeft(4, '0');

        /// <summary>
        ///     操作员编号
        /// </summary>
        public string _userCode =PayAPIConfig.Operator.UserSysId;//PayAPIConfig.Operator.UserSysId;
        /// <summary>
        ///     业务周期号
        /// </summary>
        private string _bussinessCode = "";

        /// <summary>
        ///     医院交易流水号
        /// </summary>
        private string _hosTransCode = "";

        /// <summary>
        ///     中心编码
        /// </summary>
        private string _centerCode = "";

        /// <summary>
        ///     是否联机
        /// </summary>
        private const string IsOnLine = "1";

        #endregion

        #region 方法

        /// <summary>
        /// 初始化接口环境
        /// </summary>
        /// <returns></returns>
        public void InitNeu()
        {
            try
            {
                //_userCode = operatorInfo.UserSysId;
                _userCode = GetUSER_CODE(_userCode);
                //_userCode = operatorInfo.UserSysId;
               // var pErrMsg = new StringBuilder(1024);
                StringBuilder pErrMsg = new StringBuilder(3000);

                object[] objArr = { pErrMsg };
                bool[] refArr = { true };
                //var ret = LySiActiveX.ExeFuncReIntFrm("INIT", objArr, refArr);

                //var ret = LySiActiveX.ExeFuncReInt("INIT", objArr);
              var ret = LySiActiveX.ExeFuncReInt("INIT", objArr, refArr);
               // var ret = LySiActiveX.ExeFuncReInt("INIT", objArr);
                var outMsg = objArr[0].ToString();
                if (ret < 0)
                {
                    LogManager.Info("医保初始化异常"+ret+"msg:"+outMsg);
                    throw new Exception(ret + "msg:" + outMsg);
                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception("医保初始化错误,ErrMsg:" + ex.Message);
            }
        }

        /// <summary>
        ///     交易
        /// </summary>
        /// <param name="inputData"></param>
        public string[] BussinessHandle(string inputData)
        {
            try
            {
                //inputData = "2450^1321100105^01000^0105-00001000-20180701396^0105-20180704102409-4.0^0000^ZY1152025.011|20180704102352|21|20180704102400|20180704102401||I10.x00|高血压病|1||杨传军|msun^1^";
                var outData = new StringBuilder(2000);
               // object[] objArr = { "2430^1321100105^01000^0105-00001000-20180701396^0105-20180703105039-444^0000^MZ14204783|14204783.0|20180703105031|杨传军|0|msun|||||^1^ ", outData };

                object[] objArr = { inputData, outData };
                //2430^1321100105^01000^0105-00001000-20180701396^0105-20180703105039-444^0000^MZ14204783|14204783|20180703105031|杨传军|0|msun|||||^1^
                //object[] objArr = { inputData };
                LogManager.Debug("<<<医保入参："+inputData);
                bool[] refArr = { false, true };
              var ret = LySiActiveX.ExeFuncReInt("BUSINESS_HANDLE", objArr, refArr);
             //  var ret = LySiActiveX.ExeFuncReInt("BUSINESS_HANDLE", objArr);
               /// var ret = LySiActiveX.ExeFuncReInt("INIT", new object[] { outData });
               // ExeFuncReDec("request_service", new object[] { "init" });
                var outMsg = objArr[1].ToString();
                LogManager.Debug(">>>医保出参" + outMsg);
                if (ret < 0)
                {
                    LogManager.Info("医保返回：" + ret + "err：" + outMsg);
                    throw new Exception(outMsg);
                }
                //if (outMsg.Split('^')[2].Split('|').Length>0)
                //{
                //    //LogManager.Debug(">>输出参数:" + outMsg.Split('^')[2].Split('|')[0].ToString());
                //}
                return outMsg.Split('^')[2].Split('|');
            }
            catch (Exception ex)
            {//0105-00001000-20180701396
                LogManager.Info("医保交易失败，Errmsg",ex);
                throw new Exception("医保交易失败,ErrMsg:" + ex.Message);
            }
        }


        /// <summary>
        /// 撤销处方专用交易，出现无视异常
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public string[] ZZCHEBussinessHandle(string inputData)
        {
            try
            {
                var outData = new StringBuilder(2000);
                // object[] objArr = { "2430^1321100105^01000^0105-00001000-20180701396^0105-20180703105039-444^0000^MZ14204783|14204783.0|20180703105031|杨传军|0|msun|||||^1^ ", outData };

                object[] objArr = { inputData, outData };
                //2430^1321100105^01000^0105-00001000-20180701396^0105-20180703105039-444^0000^MZ14204783|14204783|20180703105031|杨传军|0|msun|||||^1^
                //object[] objArr = { inputData };
                LogManager.Debug("<<<医保入参：" + inputData);
                bool[] refArr = { false, true };
                var ret = LySiActiveX.ExeFuncReInt("BUSINESS_HANDLE", objArr, refArr);
                //  var ret = LySiActiveX.ExeFuncReInt("BUSINESS_HANDLE", objArr);
                /// var ret = LySiActiveX.ExeFuncReInt("INIT", new object[] { outData });
                // ExeFuncReDec("request_service", new object[] { "init" });
                var outMsg = objArr[1].ToString();
               // LogManager.Debug(">>>医保出参" + outMsg);
                if (ret < 0)
                {
                    //LogManager.Info("医保返回：" + ret + "err：" + outMsg);
                   // throw new Exception(outMsg);
                }
                if (outMsg.Split('^')[2].Split('|').Length > 0)
                {
                    LogManager.Debug(">>撤销处方》》");
                   // LogManager.Debug(">>输出参数:" + outMsg.Split('^')[2].Split('|')[0].ToString());
                }
                return outMsg.Split('^')[2].Split('|');
            }
            catch (Exception ex)
            {//0105-00001000-20180701396
               // LogManager.Info("医保交易失败，Errmsg", ex);
              //  throw new Exception("医保交易失败,ErrMsg:" + ex.Message);
                string[] buEX = { ""};
                return buEX;
            }
        }
        #endregion

        #region 交易

        /// <summary>
        ///     读卡
        /// </summary>
        /// <param name="inputData">交易串</param>
        /// <returns></returns>
        public Dictionary<string, string> ReadCard(string inputData)
        {
            var retArr = BussinessHandle(inputData);
            var personInfoDic = new Dictionary<string, string> { };

        if(inputData.Substring(0,4).ToString()=="1400")
            {
                personInfoDic = new Dictionary<string, string>
            {
                {"个人编号", retArr[0]},
                {"单位编号", retArr[1]},
                {"身份证号", retArr[2]},
                {"姓名", retArr[3]},
                {"性别", retArr[4]},
                {"民族", retArr[5]},
                {"出生日期", retArr[6]},
                {"经办日期", retArr[7]},
                {"社会保障卡卡号", retArr[8]},
                {"人员类别", retArr[9]},
                {"医疗证号", retArr[10]},
                {"人员状态", retArr[11]},
                {"参保状态", retArr[12]},
                {"异地人员标志", retArr[13]},
                {"所属区号", retArr[14]},
                {"基金类型", retArr[15]},
                {"年度", retArr[16]},
                {"在院状态", retArr[17]},
                {"帐户余额", retArr[18]},
                {"本年医疗费累计", retArr[19]},
                {"本年帐户支出累计", retArr[20]},
                {"本年统筹支出累计", retArr[21]},
                {"本年救助金支出累计", retArr[22]},
                {"公务员补助支出累计", retArr[23]},
                {"财政补助支出累计", retArr[24]},
                {"离休统筹支出累计", retArr[25]},
                {"意外伤害统筹支出", retArr[26]},
                {"居民大病支出累计", retArr[27]},
                {"起付标准累计", retArr[28]},
                {"本年住院次数", retArr[29]},
                {"门诊统筹支付累计", retArr[30]},
                {"住院统筹支付累计", retArr[31]},
                {"单位名称", retArr[32]},             
                {"精准扶贫标志", retArr[38]},//0非精准扶贫
                {"民政人员标志", retArr[39]}//0非民政低保

                
            };
            }
            else
            {
                personInfoDic = new Dictionary<string, string>
            {
                {"个人编号", retArr[0]},
                {"单位编号", retArr[1]},
                {"身份证号", retArr[2]},
                {"姓名", retArr[3]},
                {"性别", retArr[4]},
                {"民族", retArr[5]},
                {"出生日期", retArr[6]},
                {"经办日期", retArr[7]},
                {"社会保障卡卡号", retArr[8]},
                {"人员类别", retArr[9]},
                {"医疗证号", retArr[10]},
                {"人员状态", retArr[11]},
                {"参保状态", retArr[12]},
                {"异地人员标志", retArr[13]},
                {"所属区号", retArr[14]},
                {"基金类型", retArr[15]},
                {"年度", retArr[16]},
                {"在院状态", retArr[17]},
                {"帐户余额", retArr[18]},
                {"本年医疗费累计", retArr[19]},
                {"本年帐户支出累计", retArr[20]},
                {"本年统筹支出累计", retArr[21]},
                {"本年救助金支出累计", retArr[22]},
                {"公务员补助支出累计", retArr[23]},
                {"财政补助支出累计", retArr[24]},
                {"离休统筹支出累计", retArr[25]},
                {"意外伤害统筹支出", retArr[26]},
                {"居民大病支出累计", retArr[27]},
                {"起付标准累计", retArr[28]},
                {"本年住院次数", retArr[29]},
                {"门诊统筹支付累计", retArr[30]},
                {"住院统筹支付累计", retArr[31]},
                {"单位名称", retArr[32]},             
                {"精准扶贫标志", retArr[36]},//0非精准扶贫
                {"民政人员标志", retArr[37]}

                
            };
            }
          
            return personInfoDic;
        }

        /// <summary>
        ///     费用上传
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public Dictionary<string, string> UploadOutFee(string inputData)
        {
            var retArr = BussinessHandle(inputData);
            var outFeeDic = new Dictionary<string, string>
            {
                {"金额", retArr[0]},
                {"自理金额", retArr[1]},
                {"自费金额", retArr[2]},
                {"超限价自付金额", retArr[3]},
                {"收费类别", retArr[4]},
                {"收费项目等级", retArr[5]},
                {"全额自费标志", retArr[6]},
                {"自理比例", retArr[7]},
                {"限价", retArr[8]},
                {"说明信息", retArr[9]}
            };
            return outFeeDic;
        }

        /// <summary>
        ///     预结算
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public Dictionary<string, string> PreSettle(string inputData)
        {
            var retArr = BussinessHandle(inputData);
            var preSettleDic = new Dictionary<string, string>
            {
                {"定点医疗机构编号", retArr[0]},
                {"个人编号", retArr[1]},
                {"门诊(住院)流水号", retArr[2]},
                {"单据号", retArr[3]},
                {"交易类型", retArr[4]},
                {"医院交易流水号", retArr[5]},
                {"中心交易流水号", retArr[6]},
                {"医疗费总额", retArr[7]},
                {"本次帐户支付", retArr[8]},
                {"统筹支付金额", retArr[9]},
                {"救助金支付", retArr[10]},
                {"公务员补助支付", retArr[11]},
                {"企业补充基金支付", retArr[12]},
                {"本次现金支付", retArr[13]},
                {"自费费用", retArr[14]},
                {"乙类自理费用", retArr[15]},
                {"超限价自付费用", retArr[16]},
                {"起付标准自付", retArr[17]},
                {"进入统筹费用", retArr[18]},
                {"统筹分段自付", retArr[19]},
                {"进入救助金费用", retArr[20]},
                {"救助金自付", retArr[21]},
                {"超大额封顶线自付", retArr[22]},
                {"符合基本医疗费用", retArr[23]},
                {"住院次数", retArr[24]},
                {"大病支付费用", retArr[25]},
                {"符合大病费用", retArr[26]},
                {"意外伤害支付费用", retArr[27]},
                {"转诊先自付", retArr[28]},
                {"财政支出", retArr[29]},
                {"离休统筹支出", retArr[30]},
                {"门诊统筹支出", retArr[31]},
                {"住院统筹支出", retArr[32]}, 
                {"交易索引号", retArr[33]},
                {"参考号", retArr[34]},
                {"Pos机交易编号", retArr[35]},
                {"原交易日期", retArr[36]},
                {"银行交易成功标志", retArr[37]},
                {"银行账号", retArr[38]},
                {"民政救助支出",retArr[39]},
                {"民政特大救助支出",retArr[40]},
                {"商业补充保险支出",retArr[41]},
                {"医疗机构减免",retArr[42]}
            };
            return preSettleDic;
        }

        /// <summary>
        ///     结算
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public Dictionary<string, string> OffSettle(string inputData)
        {
            var retArr = BussinessHandle(inputData);
            var offSettleDic = new Dictionary<string, string>
            {
                {"定点医疗机构编号", retArr[0]},
                {"个人编号", retArr[1]},
                {"门诊(住院)流水号", retArr[2]},
                {"单据号", retArr[3]},
                {"交易类型", retArr[4]},
                {"医院交易流水号", retArr[5]},
                {"中心交易流水号", retArr[6]},
                {"医疗费总额", retArr[7]},
                {"本次帐户支付", retArr[8]},
                {"统筹支付金额", retArr[9]},
                {"救助金支付", retArr[10]},
                {"公务员补助支付", retArr[11]},
                {"企业补充基金支付", retArr[12]},
                {"本次现金支付", retArr[13]},
                {"自费费用", retArr[14]},
                {"乙类自理费用", retArr[15]},
                {"超限价自付费用", retArr[16]},
                {"起付标准自付", retArr[17]},
                {"进入统筹费用", retArr[18]},
                {"统筹分段自付", retArr[19]},
                {"进入救助金费用", retArr[20]},
                {"救助金自付", retArr[21]},
                {"超大额封顶线自付", retArr[22]},
                {"符合基本医疗费用", retArr[23]},
                {"住院次数", retArr[24]},
                {"大病支付费用", retArr[25]},
                {"符合大病费用", retArr[26]},
                {"意外伤害支付费用", retArr[27]},
                {"转诊先自付", retArr[28]},
                {"财政支出", retArr[29]},
                {"离休统筹支出", retArr[30]},
                {"门诊统筹支出", retArr[31]},
                {"住院统筹支出", retArr[32]},
                {"交易索引号", retArr[33]},
                {"参考号", retArr[34]},
                {"Pos机交易编号", retArr[35]},
                {"原交易日期", retArr[36]},
                {"银行交易成功标志", retArr[37]},
                {"银行账号", retArr[38]},
                {"民政救助支出",retArr[39]},
                 {"民政特大救助支出",retArr[40]},
                {"商业补充保险支出",retArr[41]},
                {"医疗机构减免",retArr[42]}

            };
            return offSettleDic;
        }

        /// <summary>
        ///  银行交易
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public Dictionary<string, string> BankTrans(string inputData)
        {
            var retArr = BussinessHandle(inputData);
            LinYiPosHandle liyiLinYiPosHandle = new LinYiPosHandle();
            return liyiLinYiPosHandle.GetBankDic(retArr[0]);
        }

        /// <summary>
        ///  明细对账
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public Dictionary<string, string> AccountDic(string inputData)
        {
            var retArr = BussinessHandle(inputData);
            var accFeeDic = new Dictionary<string, string>
            {
                {"费用总额", retArr[0]},
                {"超出治疗方案总额", retArr[1]},
                {"自理费用总额", retArr[2]},
                {"自费费用总额", retArr[3]}
            };
            return accFeeDic;
        }

        #endregion

        #region 工具

        /// <summary>
        /// 获取交易头文件
        /// </summary>
        /// <param name="byCode">交易编码</param>
        /// <param name="patId"></param>
        /// <param name="byOutData">入参</param>
        /// <returns></returns>
        public string GetImpString(string byCode, string patId, string byOutData,bool isMz,string zyNetSerial)
        {
            var timeStr = DateTime.Now.ToString("yyyyMMddHHmmss");
            var patIdSec = patId.PadLeft(3, '0');
            _bussinessCode = GetBusNo();
           if (isMz)
           {
               _hosTransCode = _yljgbh.Substring(_yljgbh.Length - 4, 4) + "-" + timeStr + "-" + patIdSec.Substring(patIdSec.Length - 3, 3);

           }
           else
           {
               if (string.IsNullOrEmpty(zyNetSerial))//新病人
               {
                                  _hosTransCode = _yljgbh.Substring(_yljgbh.Length - 4, 4) + "-" + timeStr + "-" + patIdSec.Substring(patIdSec.Length - 3, 3);

               }
               else//老病人
               {
                   _hosTransCode = _yljgbh.Substring(0, 4) + "-" + timeStr + "-" + patIdSec.Substring(patIdSec.Length - 3, 3);

               }

           }
            _centerCode = "0000";
            _userCode = GetUSER_CODE(PayAPIConfig.Operator.UserSysId);//PayAPIConfig.InstitutionDict[1].UserId;
            var listImp = new List<string>
            {
                byCode,
                _yljgbh,
                _userCode,
                _bussinessCode,
                _hosTransCode,
                _centerCode,
                byOutData,
                IsOnLine,
                ""
            };
            return String.Join("^", listImp.ToArray());
        }

        /// <summary>
        ///     获取业务周期号
        /// </summary>
        /// <returns></returns>
        public string GetBusNo()
        {

            //var busNo = "";
            //var sql = " SELECT [操作员工号],[业务周期号] FROM [YBDR].[dbo].[YB_YWZQH] WHERE 签退='0' AND 操作员工号='" + operatorInfo.UserSysId + "'";
            //var dsBus = _mssqlHelpers.ExecSqlReDs(sql);
            //if (dsBus.Tables[0].Rows.Count > 0)
            //{
            //    busNo = dsBus.Tables[0].Rows[0]["业务周期号"].ToString();
            //}
            //else
            //{
            //    throw new Exception("请签到!!!");
            //}
            //return busNo;
            var busNo = "";
            string sqluserid = "select  * from   comm.COMM.USERS where   USER_ID in(SELECT USER_ID  FROM [comm].[COMM].[USERS_SYS]  WHERE  USER_SYS_ID='" + operatorInfo.UserSysId + "')";
            // select  * from   comm.COMM.USERS where   USER_ID in(SELECT USER_ID  FROM [comm].[COMM].[USERS_SYS]  WHERE  USER_SYS_ID='100073288')

            string use_code = "";
            DataTable dt = _mssqlHelpers.ExecSqlReDs(sqluserid).Tables[0];
            foreach (DataRow item in dt.Rows)
            {
                use_code = item["USER_CODE"].ToString();
            }
            if (string.IsNullOrEmpty(use_code))
            {
                throw new  Exception("当前登录HIS用户usercode为空！！");
            }
            var sql = " SELECT [操作员工号],[业务周期号] FROM [YBDR].[dbo].[YB_YWZQH] WHERE 签退='0' AND 操作员工号='" + use_code + "'";
            var dsBus = _mssqlHelpers.ExecSqlReDs(sql);
            if (dsBus.Tables[0].Rows.Count > 0)
            {
                busNo = dsBus.Tables[0].Rows[0]["业务周期号"].ToString();
            }
            else
            {
                throw new Exception("请签到!!!");
            }
            return busNo;
        }

        /// <summary>
        ///     获取费用总额
        /// </summary>
        /// <returns></returns>
        public decimal GetAmountByPatInHosId(string patInHosId)
        {
            decimal amount = 0;
            var sql = " SELECT ISNULL(SUM(amount),'0')amount FROM ZY.[IN].IN_BILL_RECORD WHERE PAT_IN_HOS_ID='" +
                      patInHosId + "' ";
            var ds = _mssqlHelpers.ExecSqlReDs(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                amount = Convert.ToDecimal(ds.Tables[0].Rows[0]["amount"]);
            }
            return amount;
        }

        /// <summary>
        ///  获取登记信息
        /// </summary>
        /// <param name="netSettleId">OUT_NETWORK_SETTLE_ID</param>
        /// <returns></returns>
        public DataTable GetOutRegInfoByNetSettId(string netSettleId)
        {
            string sql = "SELECT NET_REG_SERIAL,MEMO_1 FROM MZ.OUT.OUT_NETWORK_REGISTERS WHERE OUT_NETWORK_SETTLE_ID='" + netSettleId + "'";
            return _mssqlHelpers.ExecSqlReDs(sql).Tables[0];
        }

        /// <summary>
        ///  获取登记信息
        /// </summary>
        /// <param name="netSettleId">IN_NETWORK_SETTLE_ID</param>
        /// <returns></returns>
        public DataTable GetInRegInfoByNetSettId(string netSettleId)
        {
            //string sql = "SELECT NET_REG_SERIAL,MEMO_1 FROM ZY.[IN].IN_NETWORK_REGISTERS WHERE IN_NETWORK_SETTLE_ID='" + netSettleId + "'";//1.38没有IN_NETWORK_SETTLE_ID
           string sql = "SELECT a.NET_REG_SERIAL,a.MEMO_1 FROM ZY.[IN].IN_NETWORK_REGISTERS a LEFT JOIN zy.[IN].IN_NETWORK_SETTLE_MAIN b ON a.PAT_IN_HOS_ID=b.PAT_IN_HOS_ID WHERE a.FLAG_INVALID=0 AND b.FLAG_INVALID=0 and b.IN_NETWORK_SETTLE_ID='" + netSettleId + "'";
            return _mssqlHelpers.ExecSqlReDs(sql).Tables[0];
        }

        /// <summary>
        /// 获取门诊银行结算信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetBankInfoForMz(string netSettldId, string outPatId)
        {// string sql = "  SELECT * FROM MZ.OUT.OUT_NETWORK_SETTLE_LIST WHERE OUT_NETWORK_SETTLE_ID='" + netSettldId + "' AND OUT_PAT_ID='" + outPatId + "' AND PARA_NAME IN ('交易索引号','参考号','Pos机交易编号','原交易日期','银行交易成功标志','银行账号') ";
            string sql = "  SELECT * FROM MZ.OUT.OUT_NETWORK_SETTLE_LIST WHERE OUT_NETWORK_SETTLE_ID='" + netSettldId + "' AND OUT_PAT_ID='" + outPatId + "'";

            return _mssqlHelpers.ExecSqlReDs(sql).Tables[0];
        }

        /// <summary>
        /// 获取住院银行结算信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetBankInfoForZy(string netSettldId, string patInHosId)
        {
            string sql = "  SELECT * FROM ZY.[IN].IN_NETWORK_SETTLE_LIST WHERE IN_NETWORK_SETTLE_ID='" + netSettldId + "' AND PAT_IN_HOS_ID='" + patInHosId + "' AND PARA_NAME IN ('交易索引号','参考号','Pos机交易编号','原交易日期','银行交易成功标志','银行账号') ";
            return _mssqlHelpers.ExecSqlReDs(sql).Tables[0];
        }

        /// <summary>
        ///   保存银行交易信息
        /// </summary>
        /// <param name="patId"></param>
        /// <param name="netSettleId"></param>
        /// <param name="bankInfo"></param>
        /// <param name="mark"></param>
        public void SaveBankTransInfo(string patId, string netSettleId, Dictionary<string, string> bankInfo, string mark)
        {
            StringBuilder strSql = new StringBuilder();
            foreach (var bif in bankInfo)
            {
                strSql.Append(
                    "INSERT INTO YBDR.dbo.CEL_BANKTRANS_RECORD( PAT_ID ,NET_SETTLE_ID , PARA_NAME ,PARA_VALUE,MARK)  VALUES  ('")
                    .Append(patId)
                    .Append("','")
                    .Append(netSettleId)
                    .Append("','")
                    .Append(bif.Key)
                    .Append("','")
                    .Append(bif.Value)
                    .Append("',' ")
                    .Append(mark)
                    .Append("') ");
            }
            _mssqlHelpers.ExecSqlReInt(strSql.ToString());
        }

        /// <summary>
        ///  查询诊断信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public DataSet QueryDignosis(string input, string bztype)
        {
          //  string sql = " SELECT TOP 100 CENTER_DIAGNOSIS_CODE AS 编码,CENTER_DIAGNOSIS_NAME AS 名称 FROM COMM.DICT.NETWORKING_DIAGNOSIS_DICT WHERE INPUT_CODE LIKE '" + input + "%'";
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT 病种编码,病种名称,CASE WHEN 病种类别='01' THEN '普通病种' WHEN 病种类别='02' THEN '门诊慢性病' WHEN 病种类别='03' THEN '特殊病种' WHEN 病种类别='01' THEN '' WHEN 病种类别='04' THEN '生育病种' WHEN 病种类别='09' THEN '其他病种' END 病种类别 FROM YBDR.dbo.YB_BZML  ");
            strSql.Append(" WHERE 病种编码 LIKE '" + input + "%' OR 病种名称 LIKE  '" + input + "%' OR 拼音助记码 LIKE  '" + input + "%' ");
            if (bztype == "03")
            {
                strSql.Append(" AND 病种类别<>'" + bztype + "' ");
            }
            else
            {
                strSql.Append(" AND 病种类别='" + bztype + "' ");
            }
            return _mssqlHelpers.ExecSqlReDs(strSql.ToString());
        }

        /// <summary>
        ///  查询二级代码字典
        /// </summary>
        /// <returns></returns>
        public DataTable GetSecLevelCodeDs()
        {
            string sql = "SELECT SEC_CODE,SEC_VALUE,SEC_CLASS FROM YBDR.dbo.YB_SEC_LEVEL_CODE ";
            return _mssqlHelpers.ExecSqlReDs(sql).Tables[0];
        }
        #endregion

#region 参数修改赋值

        public   string GetUSER_CODE(string userSysid)
	{
        string userCode = "";
        string sql = "  SELECT  *  FROM comm.COMM.USERS   WHERE   USER_ID IN (SELECT  USER_ID  FROM   comm.COMM.USERS_SYS  WHERE  USER_SYS_ID= '" + userSysid + "')";
        DataTable dt = _mssqlHelpers.ExecSqlReDs(sql).Tables[0];
        foreach (DataRow item in dt.Rows)
        {
            userCode = item["USER_CODE"].ToString();
        }
        if (string.IsNullOrEmpty(userCode))
        {
            throw new Exception("获取操作员编号为空，请联系管理员！");
        }

        return userCode;
	}


	#endregion
    
    }
}