using PayAPIUtilities.Config;
using PayAPIUtilities.Log;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using cmhs5;

namespace PayAPIResolver.Dareway.JNLX
{
    /// <summary>
    /// 济南历下地维处理类
    /// </summary>
    public class JNLXDarewayInterfaceResolver
    {


        mhs5 mhs = new mhs5();
        mhs5down mhsd = new mhs5down();


        /// <summary>
        ///目标社保机构编码 
        /// </summary>
        public string SBJGBH = "";

        /// <summary>
        /// 登录用户隶属机构编码
        /// </summary>
        public string dwJgBm = "";

        /// <summary>
        /// 医院地维用户名
        /// </summary>
        public string dwUser = "";

        /// <summary>
        /// 医院地维用户密码
        /// </summary>
        public string dwPassword = "";

        /// <summary>
        /// 地维返回令牌
        /// </summary>
        public string dwtoken = "";

        /// <summary>
        /// 是否重复获取参数
        /// </summary>
        private bool isInit = false;
        /// <summary>
        /// 构造取地维用户名密码  注册码
        /// </summary>
        public JNLXDarewayInterfaceResolver()
        {
            GetInPara();
        }
        /// <summary>
        /// 解析结果
        /// </summary>
        private JObject joResult = new JObject();


        /// <summary>
        /// 入参字典
        /// </summary>
        private Dictionary<string, object> dictInPara = new Dictionary<string, object>();



        /// <summary>
        /// 获取地维用户名密码
        /// </summary>
        public void GetInPara()
        {
            if (!isInit)
            {

                dwJgBm = PayAPIConfig.InstitutionDict[1].InstitutionCode;
                dwUser = PayAPIConfig.InstitutionDict[1].InstitutionUserCode;
                dwPassword = PayAPIConfig.InstitutionDict[1].InstitutionPassword;
                DareWayInit(dwUser, dwPassword);

            }
            isInit = true;

        }



        #region 地维登陆 获取用户口令

        public void DareWayInit(string usercode, string password)
        {

            JObject loginResult = JObject.Parse(mhs.login(usercode, password));

            if (loginResult["resultcode"].ToString() != "0")
            {
                dwtoken = "";

                throw new Exception("mhs.login错误,resultcode:" + loginResult["resultcode "].ToString());
            }
            else
            {
                dwtoken = loginResult["resulttext"].ToString();
            }
            // loginResult["resulttext"].ToString();

        }

        /// <summary>
        /// 地维登陆
        /// </summary>
        //public void DareWayInit()
        //{
        //    seiproxy = new DarewayHandle();

        //    string hosNo = PayAPIConfig.InstitutionDict[1].InstitutionCode;
        //    string usercode = PayAPIConfig.InstitutionDict[1].InstitutionUserCode;
        //    string password = PayAPIConfig.InstitutionDict[1].InstitutionPassword;

        //    JObject loginResult = DarewayLogin(usercode, password);

        //    if (loginResult["resultcode"].ToString() != "0")
        //    {
        //        throw new Exception("登陆失败 连接医保服务器出错,医保返回提示：" + loginResult["resulttext"].ToString());
        //    }

        //    userKey = loginResult["resulttext"].ToString();
        //}
        #endregion


        /// <summary>
        /// 地纬业务处理函数
        /// </summary>
        /// <param name="sbjgbh"></param>
        /// <param name="userKey"></param>
        /// <param name="hisjyh"></param>
        /// <param name="method"></param>
        /// <param name="jsonPara"></param>
        private void DarwayInvoke(string sbjgbh, string zcm, string hisjyh, string method, string jsonPara, string yybm)
        {
            LogManager.Info("地纬调用开始***============================================*** ");
            LogManager.Info("业务处理入参---sbjgbh:" + sbjgbh + "zcm:" + zcm + " hisjyh: " + hisjyh + " method: " + method + " \r\n" + "入参: " + jsonPara);

            //string result = pipservice.pipInvoke(sbjgbh, zcm, hisjyh, method, jsonPara, yybm);
            string result = mhs.mhs5invoke(sbjgbh, zcm, hisjyh, method, jsonPara);

            LogManager.Info("业务处理出参---" + " hisjyh: " + hisjyh + " zcm: " + zcm + " method: " + method + "\r\n" + "出参:  " + result);
            LogManager.Info("地纬调用结束***=============================================***");
            joResult = JObject.Parse(result);
        }

        /// <summary>
        /// 初始化HANDLE
        /// </summary>
        public void InitHandle()
        {
            joResult = new JObject();
            dictInPara.Clear();
        }

        /// <summary>
        /// 增加入参
        /// </summary>
        /// <param name="paraName">参数名称</param>
        /// <param name="paraValue">参数值</param>
        public void AddInPara(string paraName, object paraValue)
        {
            dictInPara.Add(paraName, paraValue);
        }


        /// <summary>
        /// 清除入参
        /// </summary>
        public void ClearInPara()
        {
            dictInPara.Clear();
        }


        /// <summary>
        /// 地纬获取HIS交易流水号
        /// </summary> 
        /// <returns></returns>
        private string DarwayGetHisjyh()
        {
            string result = DateTime.Now.ToString("yyyyMMddhhmmssfff") + "-" + PayAPIConfig.Operator.UserSysId;
            //string result = seiproxy.ExeFuncReStr("hisjyh", null);
            return result;
        }

        /// <summary>
        /// 业务处理 
        /// </summary>
        public void Handle(string method, bool isCheckSuc = false)
        {
            //检测是否认证
            //InterfaceCommInfoBll.GetCertified();

            string HisJYH = DarwayGetHisjyh();
            //JSON入参
            string jsonPara = JsonConvert.SerializeObject(dictInPara, Formatting.None);
            //获取注册码和医院编码
            GetInPara();

            //DarwayInvoke("37110205", ZCM, HisJYH, method, jsonPara, YYBM);  //测试靶向药患者
            //DarwayInvoke("37110101", ZCM, HisJYH, method, jsonPara, YYBM);  //测试靶向药患者
            //地纬处理
            //因网络暂时写死实现撤销登记
            //SBJGBH = "37112205";
            DarwayInvoke(SBJGBH, dwtoken, HisJYH, method, jsonPara, "YYBM");
            //获取结果
            string resultCode = GetResultStr("resultcode");
            string reusltText = GetResultStr("resulttext");
            if (resultCode != "0")
            {
                //加入问询机制
                if (resultCode == "-6" && isCheckSuc)
                {
                    AskForSi(HisJYH, reusltText);
                }
                else
                {
                    throw new Exception(reusltText);
                }
            }
        }

        /// <summary>
        /// 问询机制
        /// </summary>
        private void AskForSi(string hisjyh, string orginalError)
        {
            string HisJYH = DarwayGetHisjyh();
            string method = "ask_for_si";
            Dictionary<string, string> _dictInPara = new Dictionary<string, string>() {
                {"p_hisjyh",hisjyh}
            };
            string jsonPara = JsonConvert.SerializeObject(_dictInPara, Formatting.None);
            //获取注册码和医院编码
            GetInPara();
            //地纬处理
            DarwayInvoke(SBJGBH, dwtoken, HisJYH, method, jsonPara, "YYBM");
            //获取结果
            if (GetResultStr("resultcode") != "0")
            {
                //获取注册码和医院编码
                GetInPara();
                //地纬处理
                DarwayInvoke("SBJGBH", " ZCM", HisJYH, method, jsonPara, "YYBM");
                if (GetResultStr("resultcode") != "0")
                {
                    //返回原始错误
                    throw new Exception(orginalError);
                }
            }
            //其他情况下则调用正常
        }

        /// <summary>
        /// 获取字符串结果
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public string GetResultStr(string para)
        {
            return joResult[para].ToString();
        }

        /// <summary>
        /// 获取结果
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public decimal GetResultDec(string para)
        {
            return joResult[para].ToObject<decimal>();
        }

        /// <summary>
        /// 获取泛型数据
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public T GetResult<T>(string para)
        {
            return joResult[para].ToObject<T>();
        }

        /// <summary>
        /// 获取结果字典
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetResultDict()
        {
            Dictionary<string, string> dictResult = new Dictionary<string, string>();
            if (joResult.Count > 0)
            {
                dictResult = joResult.ToObject<Dictionary<string, string>>();
            }
            return dictResult;
        }


    }
}
