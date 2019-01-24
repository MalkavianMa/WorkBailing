using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

namespace DW_YBBX
{
    public class RZYBHandle
    {
        /// <summary>
        /// 接口对象
        /// </summary>
        //private WebReference.serviceproxy seiproxy;
        //private WebReference.pipInvokeService pipservice;
        private WebReference1.JwsServiceImplService pipservice;


        /// <summary>
        /// 解析结果
        /// </summary>
        private JObject joResult = new JObject();

        /// <summary>
        /// 是否重复获取参数
        /// </summary>
        private bool isInit = false;

        /// <summary>
        /// 输入列表
        /// </summary>
        public List<string> ListInput = new List<string>();

        /// <summary>
        /// 输入参数字典
        /// </summary>
        public Dictionary<string, object> DictInParas = new Dictionary<string, object>();

        /// <summary>
        /// 社保机构编号
        /// </summary>
        public string SBJGBH = "37110101";

        /// <summary>
        /// 注册码
        /// </summary>
        public string ZCM = "211231I-970453-771964-4535";

        /// <summary>
        /// 医院编码
        /// </summary>
        public string YYBM = "51080096";

        /// <summary>
        /// His交易号
        /// </summary>
        public string HISJYH = "";

        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName = "";

        /// <summary>
        /// 入参
        /// </summary>
        public string jsonParas = "";

        /// <summary>
        ///输出列表 
        /// </summary>
        public string[] ListOutput;

        /// <summary>
        /// 输出参数列表
        /// </summary>
        public string[] ListOutParas;

        /// <summary>
        /// 工号
        /// </summary>
        public string UserCode;

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="hisCode"></param>
        public void InitResolver( string _sbjgbh, string _zcm, string _yybm, string _usercode)
        {
            joResult = new JObject();
            UserCode = _usercode;
            SBJGBH = _sbjgbh;
            ZCM = _zcm;
            YYBM = _yybm;
            DictInParas.Clear();
        }

        /// <summary>
        /// 获取HIS交易号
        /// </summary> 
        /// <returns></returns>
        public string GetHisJYH()
        {
            string result = DateTime.Now.ToString("yyyyMMddhhmmssfff") + "-" + UserCode;
            return result;
        }

        /// <summary>
        /// 向参数字典添加参数
        /// </summary>
        /// <param name="paraName"></param>
        /// <param name="paraValue"></param>
        public void AddInParas(string paraName, object paraValue)
        {
            DictInParas.Add(paraName.ToString(), paraValue.ToString());
        }

        /// <summary>
        /// 业务处理
        /// </summary>
        /// <param name="sbjgbh"></param>
        /// <param name="zcm"></param>
        /// <param name="hisjyh"></param>
        /// <param name="method"></param>
        /// <param name="jsonPara"></param>
        /// <param name="yybm"></param>
        public void WanDaInvoke(String sbjgbh, String zcm, String hisjyh, String method, String jsonPara, String yybm)
        {
            joResult = new JObject();
            pipservice = new WebReference1.JwsServiceImplService();
            string result = pipservice.pipInvoke(sbjgbh, zcm, hisjyh, method, jsonPara, yybm);

            joResult = JObject.Parse(result);
        }

        /// <summary>
        /// 服务调用
        /// </summary>
        /// <returns></returns>
        public void Handle(string method,bool isCheckSuc = true)
        {
         
            if (method=="add_yyxm_info_all")
            {
                ZCM = "211231I-970453-771964-4535";
                YYBM = "51080096";
            }
            HISJYH = GetHisJYH();
            jsonParas = JsonConvert.SerializeObject(DictInParas);  //入参JSON序列化
            WanDaInvoke(SBJGBH, ZCM, HISJYH, method, jsonParas, YYBM);

            //获取结果
            string resultCode = GetResultStr("resultcode");
            string resultText = GetResultStr("resulttext");
            if (resultCode != "0")
            {
                //加入问询机制
                if (resultCode == "-6" && isCheckSuc)
                {
                    AskForSi(HISJYH,resultText);
                }
                else
                {
                    MessageBox.Show("服务调用失败,错误提示:" + resultText + "错误resultcode:" + resultCode);

                    throw new Exception("服务调用失败，错误提示：" + resultText);
                }
            }
        }

        /// <summary>
        /// 询问服务
        /// </summary>
        /// <param name="list"></param>
        public void AskForSi(string hisjyh,string orginalError)
        {
            HISJYH = GetHisJYH();
            string method = "ask_for_si";
            Dictionary<string, string> _dictInPara = new Dictionary<string, string>() { 
                {"p_hisjyh",hisjyh}
            };
            string jsonParas = JsonConvert.SerializeObject(_dictInPara, Formatting.None);
            WanDaInvoke(SBJGBH, ZCM, HISJYH, method, jsonParas, YYBM);
            //获取结果
            if (GetResultStr("resultcode") != "0")
            {
                //重新调用询问
                HISJYH = GetHisJYH();
                WanDaInvoke(SBJGBH, ZCM, HISJYH, method, jsonParas, YYBM);
                if (GetResultStr("resultcode") != "0")
                {
                    //返回原始错误
                    throw new Exception("服务调用失败，错误提示：" + orginalError);
                }
            }
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
        /// 获取泛型数据
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public T GetResult<T>(string para)
        {
            return joResult[para].ToObject<T>();
        }

        /// <summary>
        /// 获取医保目录结果
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public YBMLModel GetResult()
        {
            YBMLModel Result = new YBMLModel();
            if (joResult.Count > 0)
            {
                Result = joResult.ToObject<YBMLModel>();
            }
            return Result;
        }
        /// <summary>
        /// 获取中心疾病目录结果
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public YBJBModel GetResultZxjb()
        {
            YBJBModel Result = new YBJBModel();
            if (joResult.Count > 0)
            {
                Result = joResult.ToObject<YBJBModel>();
            }
            return Result;
        }

        /// <summary>
        /// 获取中心疾病目录结果
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public YYXMModel GetResultZxZXZ()
        {
            YYXMModel Result = new YYXMModel();
            if (joResult.Count > 0)
            {
                Result = joResult.ToObject<YYXMModel>();
            }
            return Result;
        }
        /// <summary>
        /// 获取字典结果
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetResultDict()
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();
            if (joResult.Count > 0)
            {
                Result = joResult.ToObject<Dictionary<string, string>>();
            }
            return Result;
        }

        /// <summary>
        /// 获取结果字典
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetResultDict_Cx()
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
