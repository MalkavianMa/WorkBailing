using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using PayAPIUtilities.Log;
using PayAPIUtilities.Config;
using PayAPIInterface.Model.Comm;


namespace PayAPIInterfaceHandle.Neusoft
{
    public class PosReturn
    {
        /// <summary>
        /// 返回码 00 表示成功，其它表示失败
        /// </summary>
        public string RespCode = "";
        /// <summary>
        /// 卡号	 20	卡号（屏蔽部分，保留前6后4）
        /// </summary>
        public string CardNo = "";
        /// <summary>
        /// 交易参考号
        /// </summary>
        public string Trace = "";
        /// <summary>
        /// 金额
        /// </summary>
        public string Amount = "";
        /// <summary>
        /// 错误说明
        /// </summary>
        public string Errdec = "";
        /// <summary>
        /// 交易类型
        /// </summary>
        public string Tran = "";

        /// <summary>
        /// pos流水号
        /// </summary>
        public string PosLsh = "";

        /// <summary>
        /// 授权号
        /// </summary>
        public string Sqh = "";

        /// <summary>
        /// 银行流水号
        /// </summary>
        public string Banklsh = "";

        /// <summary>
        /// 银行号
        /// </summary>
        public string BankCode = "";

        
    }


    public class LinYiPosHandle
    {

        [DllImport("sldll.dll")]
        public static extern int CardTrans(string strIn, StringBuilder strbOut);

        //输入参数列表
        public List<string> ListInParas = new List<string>();

        //入参
        public string StrInParas;

        /// <summary>
        /// 接收需要进行处理的字符串
        /// </summary>
        public string Output = "";

        /// <summary>
        /// 收银机号（最多8字节，不足右补空格）
        /// </summary>
        private static string _posid = "";

        /// <summary>
        /// 交易类型  
        /// </summary>
        public string Trans = "";

        /// <summary>
        /// 金额（12字节，没有小数点"."，精确到分，最后两位为小数位，不足左补0）
        /// </summary>
        public decimal Amount = 0;

        private string _strAmount = "";

        public PosReturn PosRe = new PosReturn();

        public OperatorInfo operatorInfo;

        /// <summary>
        /// 交易入参
        /// </summary>
        /// <param name="inpara"></param>
        /// <returns></returns>
        public PosReturn CardTrans(string inpara)
        {
            StringBuilder reStr = new StringBuilder();
            reStr.Length = 30000;
            int re = 0;
            try
            {
                //inpara = FormatInpara();
                re = CardTrans(inpara, reStr);
                LogManager.Info(reStr.ToString());
                if (re == 0)
                {
                    PosRe.RespCode = reStr.ToString().Substring(0, 6);
                    Output = reStr.ToString();
                    //PosRe.tran = GetSubString(270, 2);
                    //PosRe.errdec = GetSubString(6, 40);
                    //PosRe.trace = GetSubString(89, 12);
                    //PosRe.amount = GetSubString(131, 12);
                    //PosRe.card_no = GetSubString(64, 19);
                    //PosRe.posLsh = GetSubString(46, 6);
                    //PosRe.sqh = GetSubString(52, 6);
                    //PosRe.banklsh = GetSubString(159, 74);
                }
                else
                {
                    throw new Exception("业务失败");
                }
            }
            catch (System.Exception ex)
            {
                //throw ex;
                //2015.03.12修改如下
                PosRe.RespCode = ex.Message;
            }
            return PosRe;
        }

        /// <summary>
        /// 一个字符一个字符的截取解决汉字问题
        /// </summary>
        /// <param name="s"></param>
        /// <param name="indexOf"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string[] ChinesSubstring(string s, int indexOf, int length)
        {
            string[] retStr = new string[2];
            Regex reg = new Regex(@"[^\x00-\xff]");

            int ChinesAmont = 0; //该长度中存在的双字节个数
            string strTmp = "";
            string str = "";

            for (int i = 0; i < length; i++)
            {
                str = s.Substring(indexOf, 1);

                if (reg.IsMatch(str))
                {
                    //是双字节字符
                    --length;
                    ++ChinesAmont;
                }
                strTmp += str;

                indexOf++;
            }
            retStr[0] = indexOf.ToString();
            retStr[1] = strTmp;
            return retStr;
        }


        /// <summary>
        /// 截取字符串 
        /// </summary>
        /// <param name="bankOutData"></param>
        /// <param name="curloacation"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public string GetSubString(string bankOutData,int curloacation, int len)
        {
            string rcvMsgBody = ChinesSubstring(bankOutData, 0, 285)[1];
            int doubleCharCount = GetChineseLength(rcvMsgBody.Substring(0, curloacation));
            return ChinesSubstring(rcvMsgBody, curloacation - doubleCharCount, len)[1];
        }



        /// <summary>
        /// 获取汉字个数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private int GetChineseLength(string input)
        {
            input = input.Trim();
            //Regex reg = new Regex(@"[\u4e00-\u9fa5，。；？~！：‘“”’【】（）《》￥……——、]");
            Regex reg = new Regex(@"[^\x00-\xff]");  //匹配双字节字符

            int length = 0;
            char[] chars = input.ToCharArray();
            foreach (char item in chars)
            {
                if (reg.IsMatch(item.ToString()))
                {
                    //length += 2;//汉字字符长度
                    length++;//汉字个数
                }
            }
            return length;
        }


        /// <summary>
        /// 封装入参函数
        /// </summary>
        /// <param name="trans">交易类型，扣款：s01</param>
        /// <param name="amount">交易金额</param>
        /// <param name="memo">辅助信息,撤销时传入原交易参考号</param>
        /// <param name="userCode">操作员编号</param>
        /// <param name="BillCode">his账单编号</param>
        /// <param name="BillTime">his交易时间，时间格式必须为 yyyyMMddHHmmss</param>
        /// <param name="bankCode">his分配银行的代码</param>
        /// <param name="hosCode">his医院代码</param>
        /// <param name="ChannelCode">渠道代码</param>
        /// <param name="jzh">就诊卡号</param>
        /// <returns></returns>
        public string FormatInpara(string trans, decimal amount, string memo, string userCode, string BillCode, string BillTime, string bankCode, string hosCode, string ChannelCode, string jzh)
        {
            //POS机号获取MAC地址的后8位
            _posid = GetMac();
            _posid = _posid.PadRight(10);
            //金额长度最大12位，最后2位是小数位，不足左补空格
            if ((amount * 100).ToString().Length > 12)
            {
                throw new Exception("传入的金额过大，超过了12位");
            }
            else
            {
                _strAmount = (amount * 100).ToString().PadLeft(12, '0');
            }
            memo = memo.PadRight(18);
            if (userCode.Length > 6)
            {
                userCode = operatorInfo.UserSysId.Substring(operatorInfo.UserSysId.Length - 6, 6);
                    //.UserCode.Substring(RIConfig.UserSysId.Length - 6, 6);
            }
            else
            {
                userCode = operatorInfo.UserSysId.PadRight(8);
            }
            if (BillCode.Length > 20)
            {
                BillCode = BillCode.Substring(BillCode.Length - 20, 20);
            }
            else
            {
                BillCode = BillCode.PadRight(20);
            }
            string serial = HosDealSerial();
            if (serial.Length > 20)
            {
                serial = serial.Substring(serial.Length - 20, 20);
            }
            else
            {
                serial = serial.PadRight(20);
            }
            if (hosCode.Length > 6)
            {
                hosCode = hosCode.Substring(hosCode.Length - 6, 6);
            }
            else
            {
                hosCode = hosCode.PadRight(6);
            }
            if (jzh.Length > 20)
            {
                jzh = jzh.Substring(jzh.Length - 20, 20);
            }
            else
            {
                jzh = jzh.PadRight(20);
            }
            string cardno = "";
            string yxq = "";
            if (trans == "V06" || trans == "R01")
            {
                cardno = cardno.PadRight(19);
                yxq = yxq.PadRight(4);
            }
            return trans + _strAmount + memo + _posid + userCode + BillCode + serial + BillTime + bankCode + hosCode + ChannelCode + jzh + cardno + yxq;
        }

        /// <summary>
        /// 获取MAC地址
        /// </summary>
        /// <returns></returns>
        private static string GetMac()
        {
            List<string> listMac = GetMacByNetworkInterface();
            List<string> listReMac = new List<string>();
            foreach (var li in listMac)
            {

                if (!li.Substring(li.Length - 2, 2).Equals(li.Substring(li.Length - 4, 2)))
                {
                    listReMac.Add(li);
                }
            }
            if (listReMac.Count > 0)
            {
                if (listReMac[0].Length < 8)
                {
                    throw new Exception("获取POS机代码失败,MAC长度无效。");
                }
                return listReMac[0].Substring(listReMac[0].Length - 8, 8);
            }
            else
            {
                throw new Exception("获取POS机代码失败，获取MAC失败。");
            }
        }

        private static NetworkInterface[] NetCardInfo()
        {
            return NetworkInterface.GetAllNetworkInterfaces();
        }

        private static List<string> GetMacByNetworkInterface()
        {
            try
            {
                List<string> macs = new List<string>();
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface ni in interfaces)
                {
                    if (ni.NetworkInterfaceType.ToString().ToLower().Equals("ethernet"))
                    {
                        macs.Add(ni.GetPhysicalAddress().ToString());
                    }
                }
                return macs;
            }
            catch (System.Exception ex)
            {
                throw new Exception("获取POS机代码失败，获取MAC失败\n错误描述:" + ex.Message.ToString());
            }
        }
      

        /// <summary>
        /// 生成交易流水号
        /// </summary>
        /// <returns></returns>
        public string HosDealSerial()
        {
            return string.Format("{0:yyyyMMddHHmmssffff}", System.DateTime.Now).Substring(0, 14) + operatorInfo.UserSysId;
        }

        /// <summary>
        /// 向入参参数列表中添加参数，长度大于参数长度时截取右边参数长度
        /// </summary>
        /// <param name="objInParas">传入参数值</param>
        /// <param name="length">参数长度</param>
        /// <param name="paddingchar">补齐内容</param>
        /// <param name="LOR">传入L或R，R代码右对齐，L代表左对齐</param>
        public void AddListInParas(string objInParas, int length, char[] paddingchar, string LOR)
        {
            if (objInParas.Length > length)
            {
                objInParas = objInParas.Substring(objInParas.Length - length, length);
            }
            else
            {
                if (LOR == "R")
                {
                    objInParas = objInParas.PadLeft(length, paddingchar[0]);
                }
                else
                {
                    objInParas = objInParas.PadRight(length, paddingchar[0]);
                }
            }
            ListInParas.Add(objInParas);
        }

        /// <summary>
        /// 输入字符串
        /// </summary>
        /// <returns></returns>
        public string CommInput()
        {
            StrInParas = String.Join("", ListInParas.ToArray());
            return StrInParas;
        }

        /// <summary>
        ///  解析银行出参
        /// </summary>
        /// <param name="bankOutStr"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetBankDic(string bankOutStr)
        {
            var bankTransDic = new Dictionary<string, string>
            {
                {"返回码", GetSubString(bankOutStr,0,6)},
                {"返回码含义",GetSubString(bankOutStr,6,40)},
                {"POS流水号",GetSubString(bankOutStr,46,6)},
                {"授权号", GetSubString(bankOutStr,52,6)},
                {"批次号", GetSubString(bankOutStr,58,6)},
                {"卡号",GetSubString(bankOutStr,64,19)},
                {"有效期", GetSubString(bankOutStr,83,4)},
                {"银行号", GetSubString(bankOutStr,87,2)},
                {"参考号",GetSubString(bankOutStr,89,12)},
                {"终端号", GetSubString(bankOutStr,101,15)},
                {"商户号",GetSubString(bankOutStr,116,15)},
                {"交易金额",GetSubString(bankOutStr,131,12)},
                {"交易索引号", GetSubString(bankOutStr,143,16)},
                {"自定义域",GetSubString(bankOutStr,159,74)},
                {"发卡行代码", GetSubString(bankOutStr,233,8)},
                {"银行主机日期",GetSubString(bankOutStr,241,8)},
                {"银行主机时间", GetSubString(bankOutStr,249,6)},
                {"订单号", GetSubString(bankOutStr,255,15)},
                {"交易代码",GetSubString(bankOutStr,270,2)},
                {"卡片余额", GetSubString(bankOutStr,272,13)}
            };
            return bankTransDic;
        }
    }
}
