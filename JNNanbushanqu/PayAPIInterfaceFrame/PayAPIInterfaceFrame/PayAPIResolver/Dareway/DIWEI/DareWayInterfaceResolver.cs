using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using PayAPIUtilities.Log;
using PayAPIInterfaceHandle.Dareway.DW;
using PayAPIUtilities.Config;
using PayAPIInterface.ParaModel;
using PayAPIInterface.Model.In;
using PayAPIInterfaceHandle.Tools;

namespace PayAPIResolver.Dareway.DIWEI
{
    /// <summary>
    /// 淄博地维处理类
    /// </summary>
    public class DarewayInterfaceResolver
    {
        /// <summary>
        /// 接口代理
        /// </summary>
        public DarewayHandle seiproxy;


        private static RefCOM seiproxys;

        /// <summary>
        /// 解析结果
        /// </summary>
        private JObject joResult = new JObject();

        /// <summary>
        /// 入参字典
        /// </summary>
        private Dictionary<string, object> dictInPara = new Dictionary<string, object>();

        string hospitialidContinue = PayAPIConfig.Operator.HospitalId;

        string SaveInItemsErr = "";


        /// <summary>
        /// 构造函数
        /// </summary>
        public DarewayInterfaceResolver()
        {
            ReleaseComObj();
            DareWayInit();
        }


        #region 获取返回数据
        /// <summary>
        /// 获取返回字符串
        /// </summary>
        /// <param name="strPara"></param>
        /// <returns></returns>
        public string GetReturnString(string strPara)
        {
            return seiproxy.ExeFuncReStr("result_s", new object[] { strPara });
        }

        /// <summary>
        /// 获取返回日期
        /// </summary>
        /// <param name="strPara"></param>
        /// <returns></returns>
        public string GetReturnDate(string strPara)
        {
            return seiproxy.ExeFuncReStr("result_d", new object[] { strPara });
        }
        /// <summary>
        /// 获取返回数值
        /// </summary>
        /// <param name="strPara"></param>
        /// <returns></returns>
        public decimal GetReturnDec(string strPara)
        {
            return seiproxy.ExeFuncReDec("result_n", new object[] { strPara });
        }




        #endregion
        #region 地维登陆 获取用户口令和释放object
        /// <summary>
        /// 地维登陆
        /// </summary>
        public void DareWayInit()
        {
            if (seiproxy != null)
            {
                seiproxy.SeiProxy.Dispose();
            }
            seiproxy = new DarewayHandle();

            LogManager.Info("地维初始化登陆传入德usersysID" + PayAPIConfig.Operator.UserSysId + "地维初始化登陆传入" + PayAPIConfig.Operator.DeptId + ",,," + PayAPIConfig.Operator.UserName);
          //  string binhg = PayAPIConfig.InstitutionDict[8].InstitutionCode;
            string hosNo = PayAPIConfig.InstitutionDict[1].InstitutionCode;
            string usercode = PayAPIConfig.InstitutionDict[1].InstitutionUserCode;
            string password = PayAPIConfig.InstitutionDict[1].InstitutionPassword;
            //string hosNo = "011102";
            //string usercode = "0001";
            //string password = "1234";

            int iRe = seiproxy.ExeFuncReInt("init", new object[] { usercode, hosNo, password });
            if (iRe != 0)
            {
                throw new Exception("登陆失败 连接医保服务器出错,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }

        /// <summary>
        /// 释放object
        /// </summary>
        public void ReleaseComObj()
        {
            //seiproxy.ReleaseComObj();
            try
            {
                if (seiproxys != null)
                {
                    //MessageBox.Show("1");
                    seiproxys.Dispose();
                    //SeiProxy = new RefCOM("embeded_interface");
                    //MessageBox.Show("2");
                }
            }
            catch (Exception desEx)
            {
                try
                {
                    LogManager.Info("DisposeSelf::" + desEx.Message + "  " + desEx.StackTrace + " " + desEx.Source);
                }
                catch (Exception desEx1)
                {
                    LogManager.Info("DisposeSelf_err::" + desEx1.Message + "  ");
                }
            }
        }
        #endregion

        #region 读医保卡，取得人员相关信息
        /// <summary>
        /// 读医保卡，取得人员相关信息。
        /// </summary>
        /// <param name="p_yltclb">医疗统筹类别 0 取卡片基本信息，1 住院，4 门诊大病，6 普通门诊 </param>
        /// <param name="p_bcxm">补充项目，用于以后功能扩展，目前一律传递’’</param>
        public Dictionary<string, string> ReadCard(string p_yltclb, string p_bcxm)
        {
            Dictionary<string, string> patinfo = new Dictionary<string, string>();
            int iRe = seiproxy.ExeFuncReInt("readcard", new object[] { p_yltclb, p_bcxm });
            if (iRe != 0)
            {
                throw new Exception("读卡失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
            patinfo.Add("ylzbh", GetReturnString("ylzbh"));         //医保卡号
            patinfo.Add("xm", GetReturnString("xm"));               //姓名
            patinfo.Add("xb", GetReturnString("xb"));               //性别,1:男,2:女,9:不确定
            patinfo.Add("shbzhm", GetReturnString("shbzhm"));       //社会保障号码
            patinfo.Add("zfbz", GetReturnString("zfbz"));           //灰白名单标志:0 代表灰名单,1 白名单
            patinfo.Add("zfsm", GetReturnString("zfsm"));           //灰名单原因(如果是白名单该值为空)
            patinfo.Add("dwmc", GetReturnString("dwmc"));           //单位名称
            patinfo.Add("ylrylb", GetReturnString("ylrylb"));       //人员类别(汉字)
            patinfo.Add("ydbz", GetReturnString("ydbz"));           //是否为异地人员 (1:是,0: 否)
            patinfo.Add("mzdbjbs", GetReturnString("mzdbjbs"));     //疾病编码
            patinfo.Add("sbjbm", GetReturnString("sbjbm"));         //社保局编码,山东省直为379902
            patinfo.Add("zhzybz", GetReturnString("zhzybz"));       //有无15(医保参数制)天内的住院记录1:有 ,0 :无
            patinfo.Add("zhzysm", GetReturnString("zhzysm"));       //5(医保参数控制)天内的住院记录说明
            patinfo.Add("zcyymc", GetReturnString("zcyymc"));       //转出医院名称(如果zcyymc不为’’,则表示本次住院时候市内转院来的)
            patinfo.Add("ye", GetReturnDec("ye").ToString());       //账户余额
            patinfo.Add("sbjglx", GetReturnString("sbjglx"));       //转出医院出院日期
            return patinfo;
        }
        #endregion

        #region 门诊读医保卡，取得人员相关信息
        /// <summary>
        /// 门诊读医保卡，取得人员相关信息。
        /// </summary>
        /// <param name="p_yltclb">医疗统筹类别 0 取卡片基本信息，1 住院，4 门诊大病，6 普通门诊 </param>
        /// <param name="p_bcxm">补充项目，用于以后功能扩展，目前一律传递’’</param>
        public Dictionary<string, string> ReadCardMZ()
        {

            LogManager.Debug("读取医保卡》》");
            Dictionary<string, string> patinfo = new Dictionary<string, string>();
            decimal iRe = seiproxy.ExeFuncReInt("readcardmz", null);
            if (iRe != 0)
            {
                throw new Exception("读卡失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
            //--------------------------------------第一次读卡失败，自动重新读卡
            try
            {
                patinfo.Add("ylzbh", GetReturnString("ylzbh"));         //医保卡号
            }
            catch (Exception)
            {
                ReleaseComObj();
                DareWayInit();

                iRe = seiproxy.ExeFuncReInt("readcardmz", null);

                if (iRe != 0)
                {
                    throw new Exception("读卡失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
                }
                patinfo.Add("ylzbh", GetReturnString("ylzbh"));         //医保卡号

            }
            //-----------------------------------------------
            //patinfo.Add("ylzbh", GetReturnString("ylzbh"));         //医保卡号
            patinfo.Add("xm", GetReturnString("xm"));               //姓名
            patinfo.Add("xb", GetReturnString("xb"));               //性别,1:男,2:女,9:不确定
            patinfo.Add("shbzhm", GetReturnString("shbzhm"));       //社会保障号码
            patinfo.Add("zfbz", "");           //灰白名单标志:0 代表灰名单,1 白名单
            patinfo.Add("zfsm", "");           //灰名单原因(如果是白名单该值为空)
            patinfo.Add("dwmc", GetReturnString("dwmc"));           //单位名称
            patinfo.Add("ylrylb", GetReturnString("ylrylb"));       //人员类别(汉字)
            patinfo.Add("ydbz", GetReturnString("ydbz"));           //是否为异地人员 (1:是,0: 否)
            patinfo.Add("mzdbjbs", "");     //疾病编码
            patinfo.Add("sbjbm", GetReturnString("sbjbm"));         //社保局编码,山东省直为370100
            patinfo.Add("zhzybz", "");                              //有无15(医保参数制)天内的住院记录1:有 ,0 :无
            patinfo.Add("zhzysm", "");                              //5(医保参数控制)天内的住院记录说明
            patinfo.Add("zcyymc", "");                              //转出医院名称(如果zcyymc不为’’,则表示本次住院时候市内转院来的)
            patinfo.Add("yfdxbz", GetReturnString("yfdxbz"));       // 优抚对象标志,’1’为优抚对象
            patinfo.Add("yfdxlb", GetReturnString("yfdxlb"));       // 优抚对象人员类别(汉字说明)
            patinfo.Add("ye", GetReturnDec("ye").ToString());       //账户余额
            patinfo.Add("sbjglx", GetReturnString("sbjglx"));       //(‘A’ 城镇职工人员,’B’ 城镇居民人员)
            patinfo.Add("mzjslx", GetReturnString("mzjslx"));       //’1’ 普通门诊模式,’2’消费个人账户模式

            patinfo.Add("bcrylb", GetReturnString("bcrylb"));       //	补充人员类别，该值用于判断参保人是否为补充医疗人员和保健人群(‘A‘ 补充医疗人员，’B’ 保健人群)
            patinfo.Add("mzddbz", GetReturnString("mzddbz"));       //	门诊定点标志，该值用于判断当前定点是否是参保人的门诊统筹签约定点，如果是返回1，否则返回0
            patinfo.Add("mzddsm", GetReturnString("mzddsm"));       // 门诊定点说明

            return patinfo;
        }
        #endregion

        #region 门规读医保卡，取得人员相关信息
        /// <summary>
        /// 门规读医保卡，取得人员相关信息。
        /// </summary>
        public Dictionary<string, string> ReadCardMG()
        {
            Dictionary<string, string> patinfo = new Dictionary<string, string>();
            int iRe = seiproxy.ExeFuncReInt("readcardmg", null);
            if (iRe != 0)
            {
                throw new Exception("读卡失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
            //--------------------------------------第一次读卡失败，自动重新读卡
            try
            {
                patinfo.Add("ylzbh", GetReturnString("ylzbh"));         //医保卡号
            }
            catch (Exception)
            {
                ReleaseComObj();
                DareWayInit();

                iRe = seiproxy.ExeFuncReInt("readcardmg", null);

                if (iRe != 0)
                {
                    throw new Exception("读卡失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
                }
                patinfo.Add("ylzbh", GetReturnString("ylzbh"));         //医保卡号

            }
            //--------------------------------------
            //patinfo.Add("ylzbh", GetReturnString("ylzbh"));         //医保卡号
            patinfo.Add("xm", GetReturnString("xm"));               //姓名
            patinfo.Add("xb", GetReturnString("xb"));               //性别,1:男,2:女,9:不确定
            patinfo.Add("shbzhm", GetReturnString("shbzhm"));       //社会保障号码
            patinfo.Add("zfbz", GetReturnString("zfbz"));           //灰白名单标志:0 代表灰名单,1 白名单
            patinfo.Add("zfsm", GetReturnString("zfsm"));           //灰名单原因(如果是白名单该值为空)
            patinfo.Add("dwmc", GetReturnString("dwmc"));           //单位名称
            patinfo.Add("ylrylb", GetReturnString("ylrylb"));       //人员类别(汉字)
            patinfo.Add("ydbz", GetReturnString("ydbz"));           //是否为异地人员 (1:是,0: 否)
            patinfo.Add("mzdbjbs", GetReturnString("mzdbjbs"));     //疾病编码
            patinfo.Add("sbjbm", GetReturnString("sbjbm"));         //社保局编码,山东省直为370100
            //patinfo.Add("zhzybz", "");                              //有无15(医保参数制)天内的住院记录1:有 ,0 :无
            //patinfo.Add("zhzysm", "");                              //5(医保参数控制)天内的住院记录说明
            //patinfo.Add("zcyymc", "");                              //转出医院名称(如果zcyymc不为’’,则表示本次住院时候市内转院来的)
            patinfo.Add("yfdxbz", GetReturnString("yfdxbz"));       // 优抚对象标志,’1’为优抚对象
            patinfo.Add("yfdxlb", GetReturnString("yfdxlb"));       // 优抚对象人员类别(汉字说明)
            patinfo.Add("ye", GetReturnDec("ye").ToString());       //账户余额
            patinfo.Add("sbjglx", GetReturnString("sbjglx"));       //(‘A’ 城镇职工人员,’B’ 城镇居民人员)
            //patinfo.Add("mzjslx", "");                              //’1’ 普通门诊模式,’2’消费个人账户模式
            return patinfo;
        }
        #endregion

        #region 住院读医保卡，取得人员相关信息
        /// <summary>
        /// 住院读医保卡，取得人员相关信息。
        /// </summary>
        public Dictionary<string, string> ReadCardZY()
        {
            Dictionary<string, string> patinfo = new Dictionary<string, string>();
            int iRe = seiproxy.ExeFuncReInt("readcardzy", null);
            if (iRe != 0)
            {
                throw new Exception("读卡失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
            patinfo.Add("ylzbh", GetReturnString("ylzbh"));         //医保卡号
            patinfo.Add("xm", GetReturnString("xm"));               //姓名
            patinfo.Add("xb", GetReturnString("xb"));               //性别,1:男,2:女,9:不确定
            patinfo.Add("shbzhm", GetReturnString("shbzhm"));       //社会保障号码
            patinfo.Add("zfbz", GetReturnString("zfbz"));           //灰白名单标志:0 代表灰名单,1 白名单
            patinfo.Add("zfsm", GetReturnString("zfsm"));           //灰名单原因(如果是白名单该值为空)
            patinfo.Add("dwmc", GetReturnString("dwmc"));           //单位名称
            patinfo.Add("ylrylb", GetReturnString("ylrylb"));       //人员类别(汉字)
            patinfo.Add("ydbz", GetReturnString("ydbz"));           //是否为异地人员 (1:是,0: 否)
            patinfo.Add("mzdbjbs", "");     //疾病编码
            patinfo.Add("sbjbm", GetReturnString("sbjbm"));         //社保局编码,山东省直为370100
            patinfo.Add("zhzybz", GetReturnString("zhzybz"));       //有无15(医保参数制)天内的住院记录1:有 ,0 :无
            patinfo.Add("zhzysm", GetReturnString("zhzysm"));       //5(医保参数控制)天内的住院记录说明
            patinfo.Add("zcyymc", GetReturnString("zcyymc"));       //转出医院名称(如果zcyymc不为’’,则表示本次住院时候市内转院来的)
            patinfo.Add("yfdxbz", GetReturnString("yfdxbz"));       // 优抚对象标志,’1’为优抚对象
            patinfo.Add("yfdxlb", GetReturnString("yfdxlb"));       // 优抚对象人员类别(汉字说明)
            patinfo.Add("ye", GetReturnDec("ye").ToString());       //账户余额
            patinfo.Add("sbjglx", GetReturnString("sbjglx"));       //(‘A’ 城镇职工人员,’B’ 城镇居民人员)
            patinfo.Add("mzjslx", "");                              //’1’ 普通门诊模式,’2’消费个人账户模式
            return patinfo;
        }
        #endregion

        #region 取得人员相关信息 无卡
        /// <summary>
        /// 取得人员相关信息
        /// </summary>
        /// <param name="p_grbh">社会保障号码或者身份证号</param>
        /// <param name="p_xm">姓名(该姓名必须和医保数据库中一致)</param>
        /// <param name="p_yltclb">*医疗统筹类别(1 住院，4 门规)</param>
        /// <param name="p_sbjbm">*社保局编码</param>
        /// <returns></returns>

        public Dictionary<string, string> QueryBasicInfo(string p_grbh, string p_xm, string p_yltclb, string p_sbjbm)
        {
            Dictionary<string, string> patinfo = new Dictionary<string, string>();
            //  p_sbjbm = "370191";
            int iRe = seiproxy.ExeFuncReInt("query_basic_info", new object[] { p_grbh, p_yltclb, p_sbjbm });
            if (iRe != 0)
            {
                throw new Exception("读卡失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
            patinfo.Add("ylzbh", p_grbh);                           //医保卡号
            patinfo.Add("xm", GetReturnString("xm"));               //姓名
            patinfo.Add("xb", GetReturnString("xb"));               //性别,1:男,2:女,9:不确定
            patinfo.Add("shbzhm", p_grbh);                          //社会保障号码
            patinfo.Add("zfbz", GetReturnString("zfbz"));           //灰白名单标志:0 代表灰名单,1 白名单
            patinfo.Add("zfsm", GetReturnString("zfsm"));           //灰名单原因(如果是白名单该值为空)
            patinfo.Add("dwmc", GetReturnString("dwmc"));           //单位名称
            patinfo.Add("ylrylb", GetReturnString("ylrylb"));       //人员类别(汉字)
            patinfo.Add("ydbz", GetReturnString("ydbz"));           //是否为异地人员 (1:是,0: 否)
            patinfo.Add("mzdbjbs", GetReturnString("mzdbjbs"));     //疾病编码
            //patinfo.Add("sbjbm", p_sbjbm);
            patinfo.Add("sbjbm", GetReturnString("sbjbm")); //社保局编码,济南为370100
            patinfo.Add("zhzybz", GetReturnString("zhzybz"));       //有无15(医保参数制)天内的住院记录1:有 ,0 :无
            patinfo.Add("zhzysm", GetReturnString("zhzysm"));       //5(医保参数控制)天内的住院记录说明
            patinfo.Add("zcyymc", GetReturnString("zcyymc"));       //转出医院名称(如果zcyymc不为’’,则表示本次住院时候市内转院来的)
            // patinfo.Add("ye", GetReturnDec("ye").ToString());                                 //账户余额

            patinfo.Add("ye", "0");                                 //账户余额
            patinfo.Add("sbjglx", GetReturnString("sbjglx"));       //社保结构类型,标识持卡人的身份(‘A’ 城镇职工人员,’B’ 城镇居民人员)
            return patinfo;
        }
        #endregion

        #region 门规初始化

        public void InitMZMG(string p_sbjbm, string p_zylsh, string p_xm, string p_xb, string p_grbh, string p_ylzbh, string p_fyrq, string p_ysbm, string p_jbbm, string p_syzhlx)
        {
            int iRe = seiproxy.ExeFuncReInt("init_mg", new object[] { p_sbjbm, p_zylsh, p_xm, p_xb, p_grbh, p_ylzbh, p_fyrq, p_ysbm, p_jbbm, p_syzhlx });
            if (iRe != 0)
            {
                throw new Exception("门诊初始化失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion

        #region 保存门规费用明细
        /// <summary>
        /// 保存门规费用明细
        /// </summary>
        /// <param name="itemsDt"></param>
        public void SaveOutItems(OutPayParameter outPara)
        {
            LogManager.Debug("费用明细上传》》");
            foreach (var fee in outPara.Details)
            {
                LogManager.Debug("上传参数医院项目编码：" + fee.NetworkItemCode + "最小包装的单价:" + fee.Price + "总金额:" + fee.Amount + "科室编码：" + fee.DeptCode + "执行科室编码：" + fee.DeptCode + "自付比例：" + fee.SelfBurdenRatio + "医院项目名称:" + fee.ChargeName);

                seiproxy.ExeFuncReObj("new_mg_item", null);
                seiproxy.ExeFuncReObj("set_mg_item_string", new object[] { "yyxmbm", fee.NetworkItemCode }); //医院项目编码
                seiproxy.ExeFuncReObj("set_mg_item_dec", new object[] { "dj", fee.Price });                    //最小包装的单价
                seiproxy.ExeFuncReObj("set_mg_item_dec", new object[] { "sl", fee.Quantity });                 //大包装数量
                seiproxy.ExeFuncReObj("set_mg_item_dec", new object[] { "bzsl", "1" });                   //  大包装的小包装数量
                seiproxy.ExeFuncReObj("set_mg_item_dec", new object[] { "zje", fee.Amount });                  //总金额（zje=dj*sl*bzsl）
                seiproxy.ExeFuncReObj("set_mg_item_string", new object[] { "ksbm", fee.DeptCode });           //科室编码
                seiproxy.ExeFuncReObj("set_mg_item_string", new object[] { "gg", fee.Spec });                  //规格
                seiproxy.ExeFuncReObj("set_mg_item_string", new object[] { "zxksbm", fee.DeptCode });            //*执行科室编码
                //if (fee.SelfBurdenRatio > 1 && fee.NetworkItemProp=="1")
                //{
                //    seiproxy.ExeFuncReObj("set_mg_item_dec", new object[] { "jyzfbl", Convert.ToString(Convert.ToDecimal(fee.SelfBurdenRatio) / 100) });    //*自付比例
                //}
                //else
                //{
                //    seiproxy.ExeFuncReObj("set_mg_item_dec", new object[] { "jyzfbl", fee.SelfBurdenRatio });    //*自付比例
                //}
                //string a = Convert.ToString(Convert.ToInt32(fee.SelfBurdenRatio));
                seiproxy.ExeFuncReObj("set_mg_item_dec", new object[] { "jyzfbl", fee.SelfBurdenRatio });    //*自付比例
                //seiproxy.ExeFuncReObj("set_mg_item_dec", new object[] { "jyzfbl", 10 });  
                seiproxy.ExeFuncReObj("set_mg_item_string", new object[] { "yyxmmc", fee.ChargeName });       //医院项目名称

                LogManager.Debug("医院项目编码" + fee.NetworkItemCode + "最小包装的单价" + fee.Price + " 大包装数量" + fee.Quantity + " 总金额 " + fee.Amount + " 自付比例" + fee.SelfBurdenRatio);

                int iRe = seiproxy.ExeFuncReInt("save_mg_item", null);
                if (iRe != 0)
                {
                    throw new Exception("保存费用明细出错 项目编码为:" + fee.ChargeCode.ToString() + "  项目名称为;" + fee.ChargeName.ToString() + ",医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
                }
            }
        }
        #endregion


        #region 门规结算
        /// <summary>
        /// 门规结算
        /// </summary>
        /// <param name="isPrint">是否打印</param>
        /// <returns></returns>
        public Dictionary<string, string> SettleMG(string sbjglx, bool isPrint = true)
        {
            Dictionary<string, string> settleInfo = new Dictionary<string, string>();
            int iRe = seiproxy.ExeFuncReInt("settle_mg", new object[] { sbjglx });

            if (iRe != 0)
            {
                throw new Exception("门诊结算失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }




            settleInfo.Add("brjsh", GetReturnString("brjsh"));                         //医保系统的病人结算号
            settleInfo.Add("brfdje", GetReturnDec("brfdje").ToString());               //病人负担金额（包含账户支付）
            settleInfo.Add("ybfdje", GetReturnDec("ybfdje").ToString());               //医保负担金额
            settleInfo.Add("grzhzf", GetReturnDec("grzhzf").ToString());               //个人账户支付
            settleInfo.Add("ylbzje", GetReturnDec("ylbzje").ToString());               //医疗补助金额(优抚对象补助)
            settleInfo.Add("yljmje", GetReturnDec("yljmje").ToString());               //医疗减免支付

            settleInfo.Add("tczf", GetReturnDec("tczf").ToString());               //本次统筹支付
            settleInfo.Add("dezf", GetReturnDec("dezf").ToString());               //本次大额支付
            settleInfo.Add("zhzf", GetReturnDec("zhzf").ToString());               //暂缓支付
            settleInfo.Add("ljtczf", GetReturnDec("ljtczf").ToString());               //累计统筹支付
            settleInfo.Add("ljdezf", GetReturnDec("ljdezf").ToString());               //累计大额支付
            settleInfo.Add("ljmzed", GetReturnDec("ljmzed").ToString());               //累计门诊额度
            settleInfo.Add("ljgrzf", GetReturnDec("ljgrzf").ToString());               //个人自费累计
            settleInfo.Add("qttczf", GetReturnDec("qttczf").ToString());               //其他统筹支付

            LogManager.Debug("结算流水 号：" + GetReturnString("brjsh"));

            //打印结算单据
            if (isPrint)
            {
                try
                {
                    string brjsh = GetReturnString("brjsh");
                    LogManager.Debug("结算流水 号打印：" + brjsh);
                    seiproxy.ExeFuncReInt("printdj", new object[] { brjsh, "FP" });
                }
                catch (Exception ex)
                {
                    LogManager.Info(ex.Message + "异常调用：" + ex.TargetSite + "||调用堆栈" + ex.StackTrace + "||source" + ex.Source);
                }
            }
            return settleInfo;
        }
        #endregion

        #region 撤销门规结算
        /// <summary>
        /// 撤销门规结算
        /// </summary>
        /// <param name="p_brjsh">地纬结算系统的病人结算号</param>
        public void CancelMGSettle(string p_brjsh)
        {
            int iRe = seiproxy.ExeFuncReInt("destroy_mgjs", new object[] { p_brjsh });
            if (iRe != 0)
            {
                throw new Exception("撤销结算出错,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion

        #region 门诊初始化
        /// <summary>
        /// 门诊初始化
        /// </summary>
        public void InitMZ()
        {
            int iRe = seiproxy.ExeFuncReInt("init_mz", null);
            if (iRe != 0)
            {
                throw new Exception("个人账户消费初始化出错,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion

        #region 保存门诊费用明细
        /// <summary>
        /// 保存门诊费用明细
        /// </summary>
        /// <param name="itemsDt"></param>
        //public void SaveOutItemsMZ(OutPayParameter para)
        //{
        //   foreach(var fee in para.Details)
        //    {
        //        seiproxy.ExeFuncReObj("new_mz_item", null);
        //        seiproxy.ExeFuncReObj("set_mz_item_string", new object[] { "yyxmbm", fee.ChargeCode }); //医院项目编码
        //        seiproxy.ExeFuncReObj("set_mz_item_dec", new object[] { "dj", fee.Price });                    //最小包装的单价
        //        seiproxy.ExeFuncReObj("set_mz_item_dec", new object[] { "sl", fee.Quantity });                 //大包装数量
        //        seiproxy.ExeFuncReObj("set_mz_item_dec", new object[] { "bzsl", "1" });                   //  大包装的小包装数量
        //        seiproxy.ExeFuncReObj("set_mz_item_dec", new object[] { "zje", fee.Amount });                  //总金额（zje=dj*sl*bzsl）
        //        seiproxy.ExeFuncReObj("set_mz_item_string", new object[] { "ksbm", fee.DeptCode });                //科室编码
        //        seiproxy.ExeFuncReObj("set_mz_item_string", new object[] { "gg", fee.Spec });                  //规格
        //        seiproxy.ExeFuncReObj("set_mz_item_string", new object[] { "zxksbm", fee.DeptCode});            //*执行科室编码
        //        seiproxy.ExeFuncReObj("set_mz_item_string", new object[] { "Kdksbm", fee.DeptCode });            //*执行科室编码
        //        seiproxy.ExeFuncReObj("set_mz_item_dec", new object[] { "Sxzfbl", fee.SelfBurdenRatio });               //*自付比例
        //        seiproxy.ExeFuncReObj("set_mz_item_string", new object[] { "yyxmmc", fee.ChargeName});       //医院项目名称
        //        int iRe = seiproxy.ExeFuncReInt("save_mz_item", null);
        //        if (iRe != 0)
        //        {
        //            throw new Exception("保存费用明细出错 项目编码为:" + fee.ChargeCode.ToString() + "  项目名称为;" + fee.ChargeName.ToString() + ",医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
        //        }
        //    }
        //}

        public void SaveOutItemsMZ(List<PayAPIInterface.Model.Out.OutNetworkUpDetail> itemsDt)
        {
            for (int i = 0; i < itemsDt.Count; i++)
            {
                seiproxy.ExeFuncReObj("new_mz_item", null);
                seiproxy.ExeFuncReObj("set_mz_item_string", new object[] { "yyxmbm", itemsDt[i].NetworkItemCode }); //医院项目编码
                seiproxy.ExeFuncReObj("set_mz_item_dec", new object[] { "dj", itemsDt[i].Price });                    //最小包装的单价
                seiproxy.ExeFuncReObj("set_mz_item_dec", new object[] { "sl", itemsDt[i].Quantity });                 //大包装数量
                seiproxy.ExeFuncReObj("set_mz_item_dec", new object[] { "bzsl", "1" });                   //  大包装的小包装数量
                seiproxy.ExeFuncReObj("set_mz_item_dec", new object[] { "zje", itemsDt[i].Amount });                  //总金额（zje=dj*sl*bzsl）
                seiproxy.ExeFuncReObj("set_mz_item_string", new object[] { "ksbm", itemsDt[i].DeptCode });                //科室编码
                seiproxy.ExeFuncReObj("set_mz_item_string", new object[] { "gg", itemsDt[i].Spec });                  //规格
                seiproxy.ExeFuncReObj("set_mz_item_string", new object[] { "zxksbm", itemsDt[i].DeptCode });            //*执行科室编码
                seiproxy.ExeFuncReObj("set_mz_item_string", new object[] { "Kdksbm", itemsDt[i].DeptCode });            //*执行科室编码
                seiproxy.ExeFuncReObj("set_mz_item_dec", new object[] { "Sxzfbl", itemsDt[i].SelfBurdenRatio });               //*自付比例
                seiproxy.ExeFuncReObj("set_mz_item_string", new object[] { "yyxmmc", itemsDt[i].ChargeName });       //医院项目名称

                LogManager.Debug("医院项目编码" + itemsDt[i].NetworkItemCode + "最小包装的单价" + itemsDt[i].Price + " 大包装数量" + itemsDt[i].Quantity + " 总金额 " + itemsDt[i].Amount + " 自付比例" + itemsDt[i].SelfBurdenRatio);

                int iRe = seiproxy.ExeFuncReInt("save_mz_item", null);
                if (iRe != 0)
                {
                    throw new Exception("保存费用明细出错 项目编码为:" + itemsDt[i].ChargeCode.ToString() + "  项目名称为;" + itemsDt[i].ChargeName.ToString() + ",医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
                }
            }
        }
        #endregion


        #region 免费用药结算
        /// <summary>
        /// 免费用药结算
        /// </summary>
        /// <param name="isPrint">是否打印</param>
        /// <returns></returns>
        public Dictionary<string, string> SettleMFYY(string sbjglx, bool isPrint = true)
        {
            Dictionary<string, string> settleInfo = new Dictionary<string, string>();
            int iRe = seiproxy.ExeFuncReInt("settle_mfyy", new object[] { sbjglx });

            if (iRe != 0)
            {
                throw new Exception("门诊结算失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }

            settleInfo.Add("brjsh", GetReturnString("brjsh"));                         //医保系统的病人结算号
            settleInfo.Add("brfdje", GetReturnDec("brfdje").ToString());               //病人负担金额（包含账户支付）
            settleInfo.Add("ybfdje", GetReturnDec("ybfdje").ToString());               //医保负担金额
            settleInfo.Add("grzhzf", GetReturnDec("grzhzf").ToString());               //个人账户支付
            settleInfo.Add("tczf", GetReturnDec("tczf").ToString());               //医疗补助金额(优抚对象补助)
            settleInfo.Add("zhzf", GetReturnDec("zhzf").ToString());               //医疗减免支付
            LogManager.Debug("免费用药病人结算号：" + GetReturnString("brjsh") + "");


            //打印结算单据
            if (isPrint)
            {
                try
                {
                    string brjsh = GetReturnString("brjsh");
                    seiproxy.ExeFuncReInt("printdj", new object[] { brjsh, "FP" });
                }
                catch (Exception ex)
                {
                    LogManager.Info(ex.Message + "异常调用：" + ex.TargetSite + "||调用堆栈" + ex.StackTrace + "||source" + ex.Source);
                }
            }
            return settleInfo;
        }
        #endregion

        #region 免费用药初始化
        /// <summary>
        /// 免费用药初始化
        /// </summary>
        /// <param name="p_sbjbm">社保局编码</param>
        /// <param name="p_yltclb">医疗统筹类别  yltclb:4 门诊大病， 6普通门诊</param>
        /// <param name="p_grbh">社会保障号码</param>
        /// <param name="p_xm">姓名</param>
        /// <param name="p_xb">性别</param>
        /// <param name="p_zylsh">病历号</param>
        /// <param name="p_fyrq">费用录入日期(精确到天)</param>
        /// <param name="p_ysbm">医师编码（HIS必须传入一个非空的医师编码，并且保证医师有资格，HIS系统需要与地纬结算系统编码保持一致）</param>
        /// <param name="p_jbbm">疾病编码(yltclb=’4’时，必须传递；yltclb=’6’时，xzbz=’C’传递’’,xzbz=’D’或xzbz=’E’，必须传递)</param>
        /// <param name="p_syzhlx">使用账户类型;0 不使用,1银行卡,2 cpu 卡，3 济南医保卡</param>
        /// <param name="p_ylzbh">医保卡编号(读卡必须传递，不读卡传’’)</param>
        /// <param name="p_xzbz">险种标志 （医疗 C）（ 工伤 D）（ 生育 E）</param>
        /// <param name="p_bcxm">补充项目信息（扩展使用）</param>
        public void InitMFYY(string p_sbjbm, string p_yltclb, string p_grbh, string p_xm, string p_xb, string p_zylsh, string p_fyrq, string p_ysbm, string p_jbbm, string p_syzhlx, string p_ylzbh)
        {
            int iRe = seiproxy.ExeFuncReInt("init_jmmz", new object[] { p_sbjbm, p_zylsh, p_xm, p_xb, p_grbh, p_ylzbh, p_fyrq, p_ysbm, p_jbbm, p_syzhlx, p_yltclb });
            if (iRe != 0)
            {
                throw new Exception("门诊初始化失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion

        #region 获取职工居民最近一次免费用药信息
        /// <summary>
        /// 获取职工居民最近一次免费用药信息
        /// </summary>
        /// <param name="p_sbjbm">社保局编码</param>
        /// <param name="p_yltclb">身份证号码  yltclb:4 门诊大病， 6普通门诊</param>
        /// <param name="p_grbh">社保机构类型</param>

        public Dictionary<string, string> HqMfyyXx(string p_sbjbm, string p_shbzhm, string p_sbjglx)
        {
            Dictionary<string, string> settleInfo = new Dictionary<string, string>();
            int iRe = seiproxy.ExeFuncReInt("query_mfyy_info", new object[] { p_sbjbm, p_shbzhm, p_sbjglx });
            if (iRe != 0)
            {
                throw new Exception("获取职工居民最近一次免费用药信息失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
            LogManager.Debug("医保系统的病人结算号:" + GetReturnString("mfyysm") + "");
            settleInfo.Add("mfyysm", GetReturnString("mfyysm"));                         //医保系统的病人结算号
            settleInfo.Add("mfyyye", GetReturnDec("mfyyye").ToString());               //病人负担金额（包含账户支付）


            return settleInfo;
        }
        #endregion

        #region 无卡 免费用药初始化
        /// <summary>
        /// 无卡 免费用药初始化
        /// </summary>
        /// <param name="p_sbjbm">社保局编码</param>

        /// <param name="p_grbh">身份证号码</param>
        /// <param name="p_xm">姓名</param>
        /// <param name="p_xb">性别</param>
        /// <param name="p_zylsh">病历号</param>
        /// <param name="p_fyrq">费用录入日期(精确到天)</param>
        /// <param name="p_ysbm">医师编码（HIS必须传入一个非空的医师编码，并且保证医师有资格，HIS系统需要与地纬结算系统编码保持一致）</param>
        /// <param name="p_jbbm">疾病编码(yltclb=’4’时，必须传递；yltclb=’6’时，xzbz=’C’传递’’,xzbz=’D’或xzbz=’E’，必须传递)</param>
        /// <param name="p_syzhlx">使用账户类型;0 不使用,1银行卡,2 cpu 卡，3 济南医保卡</param>
        /// <param name="p_ylzbh">医保卡编号(读卡必须传递，不读卡传’’)</param>

        public void InitMZMGMfyy(string p_sbjbm, string p_grbh, string p_xm, string p_xb, string p_zylsh, string p_fyrq, string p_ysbm, string p_jbbm, string p_syzhlx, string p_ylzbh)
        {
            int iRe = seiproxy.ExeFuncReInt("init_mg", new object[] { p_sbjbm, p_zylsh, p_xm, p_xb, p_grbh, p_ylzbh, p_fyrq, p_ysbm, p_jbbm, p_syzhlx });
            if (iRe != 0)
            {
                throw new Exception("门诊初始化失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion


        #region 门诊结算
        /// <summary>
        /// 门诊结算
        /// </summary>
        /// <param name="p_sbjbm">*社保局编码</param>
        /// <param name="p_ylzbh">*医疗证编号</param>
        public Dictionary<string, string> SettleMZ(string p_sbjbm, string p_ylzbh)
        {
            Dictionary<string, string> settleInfo = new Dictionary<string, string>();
            int iRe = seiproxy.ExeFuncReInt("settle_mz", new object[] { p_sbjbm, p_ylzbh });

            if (iRe != 0)
            {
                throw new Exception("个人账户结算失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }

            settleInfo.Add("mzzdlsh", GetReturnString("mzzdlsh"));             //此次个人账户消费的流水号
            settleInfo.Add("zje", GetReturnDec("zje").ToString());             //本次消费总金额
            settleInfo.Add("grzhzf", GetReturnDec("grzhzf").ToString());       //个人账户支付
            settleInfo.Add("xj", GetReturnDec("xj").ToString());               //病人支付现金
            settleInfo.Add("jmje", GetReturnDec("jmje").ToString());           //优抚对象减免金额
            LogManager.Debug("此次个人账户消费的流水号:" + GetReturnString("mzzdlsh"));
            return settleInfo;
        }
        #endregion

        #region 急诊结算
        /// <summary>
        /// 急诊结算
        /// </summary>
        /// <param name="p_sbjbm">*社保局编码</param>
        /// <param name="p_ylzbh">*医疗证编号</param>
        public Dictionary<string, string> SettleJZ(string p_sbjbm, string p_ylzbh)
        {
            Dictionary<string, string> settleInfo = new Dictionary<string, string>();
            int iRe = seiproxy.ExeFuncReInt("settle_jz", new object[] { p_sbjbm, p_ylzbh });

            if (iRe != 0)
            {
                throw new Exception("急诊门诊结算失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }

            settleInfo.Add("mzzdlsh", GetReturnString("mzzdlsh"));             //此次个人账户消费的流水号
            settleInfo.Add("zje", GetReturnDec("zje").ToString());             //本次消费总金额
            settleInfo.Add("grzhzf", GetReturnDec("grzhzf").ToString());       //个人账户支付
            settleInfo.Add("xj", GetReturnDec("xj").ToString());               //病人支付现金
            settleInfo.Add("jmje", GetReturnDec("jmje").ToString());           //优抚对象减免金额
            return settleInfo;
        }
        #endregion



        #region 个人账户退费服务
        /// <summary>
        /// 个人账户退费服务
        /// </summary>
        /// <param name="p_mzzdlsh">*地纬结算系统的消费流水号</param>
        /// <param name="p_sbjbm">*社保局编码</param>
        /// <param name="p_ylzbh">*社保局编码</param>
        public void CancelSettleMZ(string p_mzzdlsh, string p_sbjbm, string p_ylzbh)
        {
            LogManager.Debug("撤销消费流水号：" + p_mzzdlsh + "社保局编码:" + p_sbjbm + "医疗证号：" + p_ylzbh);

            int iRe = seiproxy.ExeFuncReInt("destroy_mz", new object[] { p_mzzdlsh, p_sbjbm, p_ylzbh });
            if (iRe != 0)
            {
                throw new Exception("个人账户退费服务失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
            //Dictionary<string, string> settleInfo = new Dictionary<string, string>();
            //settleInfo.Add("cxlsh", GetReturnString("cxlsh"));                 //退费流水号
            //settleInfo.Add("zje", GetReturnDec("zje").ToString());             //本次退费总金额
            //settleInfo.Add("grzhzf", GetReturnDec("grzhzf").ToString());       //退还个人账户金额
            //settleInfo.Add("xj", GetReturnDec("xj").ToString());               //退还现金
        }
        #endregion

        #region 急诊退费服务
        /// <summary>
        /// 急诊退费服务
        /// </summary>
        /// <param name="p_mzzdlsh">*地纬结算系统的消费流水号</param>
        /// <param name="p_sbjbm">*社保局编码</param>
        /// <param name="p_ylzbh">*社保局编码</param>
        public void CancelSettleJZ(string p_mzzdlsh, string p_sbjbm, string p_ylzbh)
        {

            int iRe = seiproxy.ExeFuncReInt("destroy_jz", new object[] { p_mzzdlsh, p_sbjbm, p_ylzbh });

            if (iRe != 0)
            {
                throw new Exception("急诊退费服务失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }

        }
        #endregion

        #region 特殊门诊初始化
        /// <summary>
        /// 特殊门诊初始化
        /// </summary>
        public void InitTSMZ()
        {
            int iRe = seiproxy.ExeFuncReInt("init_tsmz", null);
            if (iRe != 0)
            {
                throw new Exception("个人账户消费初始化出错,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion

        #region 保存特殊门诊费用明细
        /// <summary>
        /// 保存特殊门诊费用明细
        /// </summary>
        /// <param name="itemsDt"></param>
        public void SaveOutItemsTSMZ(DataTable itemsDt)
        {
            for (int i = 0; i < itemsDt.Rows.Count; i++)
            {
                seiproxy.ExeFuncReObj("new_tsrymz_item", null);
                seiproxy.ExeFuncReObj("set_tsrymz_item_string", new object[] { "yyxmbm", itemsDt.Rows[i]["NETWORKING_ITEM_CODE"] }); //医院项目编码
                seiproxy.ExeFuncReObj("set_tsrymz_item_dec", new object[] { "dj", itemsDt.Rows[i]["PRICE"] });                    //最小包装的单价
                seiproxy.ExeFuncReObj("set_tsrymz_item_dec", new object[] { "sl", itemsDt.Rows[i]["QUANTITY"] });                 //大包装数量
                seiproxy.ExeFuncReObj("set_tsrymz_item_dec", new object[] { "bzsl", itemsDt.Rows[i]["BZSL"] });                   //  大包装的小包装数量
                seiproxy.ExeFuncReObj("set_tsrymz_item_dec", new object[] { "zje", itemsDt.Rows[i]["AMOUNT"] });                  //总金额（zje=dj*sl*bzsl）
                seiproxy.ExeFuncReObj("set_tsrymz_item_string", new object[] { "ksbm", itemsDt.Rows[i]["KSBM"] });                //科室编码
                seiproxy.ExeFuncReObj("set_tsrymz_item_string", new object[] { "gg", itemsDt.Rows[i]["SPEC"] });                  //规格
                seiproxy.ExeFuncReObj("set_tsrymz_item_string", new object[] { "zxksbm", itemsDt.Rows[i]["ZXKSBM"] });            //*执行科室编码
                seiproxy.ExeFuncReObj("set_tsrymz_item_string", new object[] { "Kdksbm", itemsDt.Rows[i]["KDKSBM"] });            //*执行科室编码
                seiproxy.ExeFuncReObj("set_tsrymz_item_dec", new object[] { "Jyzfbl", itemsDt.Rows[i]["SXZFBL"] });               //*自付比例
                seiproxy.ExeFuncReObj("set_tsrymz_item_string", new object[] { "yyxmmc", itemsDt.Rows[i]["CHARGE_NAME"] });       //医院项目名称
                int iRe = seiproxy.ExeFuncReInt("save_tsrymz_item", null);
                if (iRe != 0)
                {
                    throw new Exception("保存费用明细出错 项目编码为:" + itemsDt.Rows[i]["CHARGE_CODE"].ToString() + "  项目名称为;" + itemsDt.Rows[i]["CHARGE_NAME"].ToString() + ",医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
                }
            }
        }
        #endregion

        #region 特殊门诊结算
        /// <summary>
        /// 特殊门诊结算
        /// </summary>
        /// <param name="p_grbh">个人编号</param>
        /// <param name="p_xm">姓名</param>
        /// <param name="p_sbjbm">社保局编码,济南为370100</param>
        /// <param name="p_ksbm">科室编码</param>
        /// <param name="p_syzhlx">使用帐户类型 ,特殊人员为’0’</param>
        /// <returns></returns>
        public Dictionary<string, string> SettleTSMZ(string p_grbh, string p_xm, string p_sbjbm, string p_ksbm, string p_syzhlx)
        {
            Dictionary<string, string> settleInfo = new Dictionary<string, string>();
            int iRe = seiproxy.ExeFuncReInt("settle_tsry_mzjs", new object[] { p_grbh, p_xm, p_sbjbm, p_ksbm, p_syzhlx });

            if (iRe != 0)
            {
                throw new Exception("个人账户结算失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }

            settleInfo.Add("brjsh", GetReturnString("brjsh"));                 //医保系统的病人结算号
            settleInfo.Add("brfdje", GetReturnDec("brfdje").ToString());       //病人负担金额
            settleInfo.Add("ybfdje", GetReturnDec("ybfdje").ToString());       //医保负担金额
            settleInfo.Add("grzhzf", GetReturnDec("grzhzf").ToString());       //扣除个人帐户
            //打印单据
            PrintDJ(GetReturnString("brjsh"), "FP");
            return settleInfo;
        }
        #endregion

        #region 撤销特殊人员门诊结算服务
        /// <summary>
        /// 撤销特殊人员门诊结算服务
        /// </summary>
        /// <param name="p_mzzdlsh">*地纬结算系统的消费流水号</param>
        public void CancelSettleTSMZ(string p_mzzdlsh)
        {
            int iRe = seiproxy.ExeFuncReInt("destroy_tsry_mzjs", new object[] { p_mzzdlsh });

            if (iRe != 0)
            {
                throw new Exception("撤销特殊人员门诊结算服务失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }

        }
        #endregion

        #region 居民门诊初始化
        /// <summary>
        /// 居民门诊初始化
        /// </summary>
        /// <param name="p_sbjbm">社保局编码</param>
        /// <param name="p_yltclb">医疗统筹类别  yltclb:4 门诊大病， 6普通门诊</param>
        /// <param name="p_grbh">社会保障号码</param>
        /// <param name="p_xm">姓名</param>
        /// <param name="p_xb">性别</param>
        /// <param name="p_zylsh">病历号</param>
        /// <param name="p_fyrq">费用录入日期(精确到天)</param>
        /// <param name="p_ysbm">医师编码（HIS必须传入一个非空的医师编码，并且保证医师有资格，HIS系统需要与地纬结算系统编码保持一致）</param>
        /// <param name="p_jbbm">疾病编码(yltclb=’4’时，必须传递；yltclb=’6’时，xzbz=’C’传递’’,xzbz=’D’或xzbz=’E’，必须传递)</param>
        /// <param name="p_syzhlx">使用账户类型;0 不使用,1银行卡,2 cpu 卡，3 济南医保卡</param>
        /// <param name="p_ylzbh">医保卡编号(读卡必须传递，不读卡传’’)</param>
        /// <param name="p_xzbz">险种标志 （医疗 C）（ 工伤 D）（ 生育 E）</param>
        /// <param name="p_bcxm">补充项目信息（扩展使用）</param>
        public void InitJMMZ(string p_sbjbm, string p_yltclb, string p_grbh, string p_xm, string p_xb, string p_zylsh, string p_fyrq, string p_ysbm, string p_jbbm, string p_syzhlx, string p_ylzbh)
        {
            int iRe = seiproxy.ExeFuncReInt("init_jmmz", new object[] { p_sbjbm, p_zylsh, p_xm, p_xb, p_grbh, p_ylzbh, p_fyrq, p_ysbm, p_jbbm, p_syzhlx, p_yltclb });
            if (iRe != 0)
            {
                throw new Exception("门诊初始化失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion

        #region 职工门诊初始化
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_sbjbm"></param>
        /// <param name="p_zylsh"></param>
        /// <param name="p_xm"></param>
        /// <param name="p_xb"></param>
        /// <param name="p_grbh"></param>
        /// <param name="p_ylzbh"></param>
        /// <param name="p_fyrq"></param>
        /// <param name="p_ysbm"></param>
        /// <param name="p_jbbm"></param>
        /// <param name="p_syzhlx"></param>
        /// <param name="p_yltclb"></param>
        public void InitZGMZ(string p_sbjbm, string p_zylsh, string p_xm, string p_xb, string p_grbh, string p_ylzbh, string p_fyrq, string p_ysbm, string p_jbbm, string p_syzhlx, string p_yltclb)
        {
            int iRe = seiproxy.ExeFuncReInt("init_jmmz", new object[] { p_sbjbm, p_zylsh, p_xm, p_xb, p_grbh, p_ylzbh, p_fyrq, p_ysbm, p_jbbm, p_syzhlx, p_yltclb });
            if (iRe != 0)
            {
                throw new Exception("门诊初始化失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion

        #region 普通住院登记服务
        /// <summary>
        /// 普通住院登记服务
        /// </summary>
        /// <param name="p_blh">*病例号</param>
        /// <param name="p_shbzhm">*社会保障号码</param>
        /// <param name="p_ylzbh">*医疗证编号（读卡必须传入，无卡传空</param>
        /// <param name="p_xm">*姓名</param>
        /// <param name="p_xb">性别 1:男 2:女</param>
        /// <param name="p_yltclb">*住院类别 1:住院 2:家床</param>
        /// <param name="p_sbjbm">*社保局编码</param>
        /// <param name="p_syzhlx"> *使用医保卡类型：（0 不使用医保卡 ,1银行卡,2 cpu 卡，3 联机卡）</param>
        /// <param name="p_ksbm">*科室编码</param>
        /// <param name="p_zyrq">*住院日期</param>
        /// <param name="p_qzys">确诊医师</param>
        /// <param name="p_mzks">门诊科室</param>
        /// <param name="p_zyfs">*住院方式（‘1’普通住院，‘6’市内转院 ）</param>
        /// <param name="p_xzbz">*险种标志 医疗 C 工伤 D 生育 E</param>
        /// <param name="p_bcxm"> 补充项目信息（扩展使用）</param>
        public void SaveZYDJ(string p_blh, string p_shbzhm, string p_ylzbh, string p_xm, string p_xb,
            string p_yltclb, string p_sbjbm, string p_syzhlx, string p_ksbm, string p_zyrq, string p_qzys,
            string p_mzks, string p_zyfs, string p_xzbz, string p_bcxm)
        {

            int iRe = 0;
            //MessageBox.Show(p_syzhlx);
            if (p_syzhlx == "0")
            {
                iRe = seiproxy.ExeFuncReInt("save_zyxx_zyfs",
                            new object[] { p_blh, p_shbzhm, "", p_xm, p_xb, p_yltclb, p_sbjbm, p_syzhlx, p_ksbm, p_zyrq, p_qzys, p_mzks, p_zyfs, p_xzbz });
            }
            else
            {
                if (p_xzbz == "D")
                {
                    iRe = seiproxy.ExeFuncReInt("save_zyxx_zyfs",
                            new object[] { p_blh, p_shbzhm, p_ylzbh, p_xm, p_xb, p_yltclb, p_sbjbm, p_syzhlx, p_ksbm, p_zyrq, p_qzys, p_mzks, p_zyfs, p_xzbz });

                }
                else
                {
                    iRe = seiproxy.ExeFuncReInt("save_zyxx",
                                new object[] {  p_blh,   p_shbzhm,   p_ylzbh,   p_xm,   p_xb,
                                            p_yltclb,   p_sbjbm,   p_syzhlx,   p_ksbm,   p_zyrq,   p_qzys,
                                            p_mzks});
                }
            }
            if (iRe != 0)
            {
                throw new Exception("登记失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion

        #region 住院初始化服务
        /// <summary>
        /// 住院初始化服务
        /// </summary>
        /// <param name="p_blh">*病历号</param>
        public void InitZY(string p_blh)
        {
            int iRe = seiproxy.ExeFuncReInt("init_zy", new object[] { p_blh });
            if (iRe != 0)
            {
                throw new Exception("住院初始化出错,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion

        #region 撤销住院登记服务
        /// <summary>
        /// 撤销住院登记服务
        /// </summary>
        /// <param name="blh">*病历号</param>
        public void CancelZY(string blh)
        {

            InitZY(blh);
            int iRe = seiproxy.ExeFuncReInt("destroy_zydj", null);
            if (iRe != 0)
            {
                throw new Exception("住院初始化出错,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion



        #region 保存住院费用明细
        /// <summary>
        /// 保存住院费用明细
        /// </summary>
        /// <param name="itemsDt">费用table</param>
        /// <param name="vysbm">医生编码</param>
        /// <param name="vfsrq">发生日期</param>
        /// 



        public void SaveInItems(List<InNetworkUpDetail> Details, string vysbm, string vfsrq, string zyh)
        {
            //Details = PayAPIUtilities.Tools.CommonTools.GetGroupList(Details);
            int iRe = 0;
            string dateCur = Convert.ToDateTime(Details[0].CreateTime).ToString("yyyy-MM-dd");


            for (int i = 0; i < Details.Count; i++)
            {


                if (hospitialidContinue.Equals("8001"))
                {
                    if (Details[i].NetworkItemCode == "YB_009" && Details[i].ChargeName == "一次性中单")
                    {
                        continue;
                    }

                    if (Details[i].NetworkItemCode == "YB_008" && Details[i].ChargeName == "一次性床罩")
                    {
                        continue;
                    }
                }
                if (Details[i].Quantity == 0 || Details[i].Amount == 0)
                {
                    continue;
                }
                if (dateCur != Convert.ToDateTime(Details[i].CreateTime).ToString("yyyy-MM-dd"))
                {
                    iRe = seiproxy.ExeFuncReInt("save_zy_script", new object[] { vysbm, dateCur });
                    if (iRe != 0)
                    {
                        // throw new Exception("保存凭单出错,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
                        SaveInItemsErr += "保存凭单出错,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null);

                    }
                    dateCur = Convert.ToDateTime(Details[i].CreateTime).ToString("yyyy-MM-dd");

                    InitZY(zyh);
                }

                seiproxy.ExeFuncReObj("new_zy_item", null);
                seiproxy.ExeFuncReObj("set_zy_item_string", new object[] { "yyxmbm", Details[i].NetworkItemCode }); //医院项目编码
                seiproxy.ExeFuncReObj("set_zy_item_dec", new object[] { "dj", Details[i].Price });                    //最小包装的单价
                seiproxy.ExeFuncReObj("set_zy_item_dec", new object[] { "sl", Details[i].Quantity });                 //大包装数量
                seiproxy.ExeFuncReObj("set_zy_item_dec", new object[] { "bzsl", "1" });                   //  大包装的小包装数量
                seiproxy.ExeFuncReObj("set_zy_item_dec", new object[] { "zje", Details[i].Amount });                  //总金额（zje=dj*sl*bzsl）
                seiproxy.ExeFuncReObj("set_zy_item_string", new object[] { "ksbm", Details[i].DeptCode });           //科室编码
                seiproxy.ExeFuncReObj("set_zy_item_string", new object[] { "gg", Details[i].Spec });                  //规格
                seiproxy.ExeFuncReObj("set_zy_item_string", new object[] { "zxksbm", Details[i].DeptCode });            //*执行科室编码
                seiproxy.ExeFuncReObj("set_zy_item_string", new object[] { "kdksbm", Details[i].DeptCode });            //*开单科室编码
                seiproxy.ExeFuncReObj("set_zy_item_dec", new object[] { "jyzfbl", Details[i].SelfBurdenRatio });    //*自付比例
                seiproxy.ExeFuncReObj("set_zy_item_string", new object[] { "yyxmmc", Details[i].ChargeName });            //*医院项目名称
                LogManager.Debug("医院项目编码" + Details[i].NetworkItemCode + "最小包装的单价" + Details[i].Price + " 大包装数量" + Details[i].Quantity + " 总金额 " + Details[i].Amount + " 自付比例" + Details[i].SelfBurdenRatio);
                iRe = seiproxy.ExeFuncReInt("save_zy_item", null);
                if (iRe != 0)
                {
                    SaveInItemsErr += "保存费用明细出错 项目编码为:" + Details[i].ChargeCode.ToString() + "  项目名称为;" + Details[i].ChargeName.ToString() + ",医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null);
                    // throw new Exception("保存费用明细出错 项目编码为:" + Details[i].ChargeCode.ToString() + "  项目名称为;" + Details[i].ChargeName.ToString() + ",医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
                }

            }
            iRe = seiproxy.ExeFuncReInt("save_zy_script", new object[] { vysbm, dateCur });
            //iRe = seiproxy.ExeFuncReInt("save_zy_script", new object[] { vysbm, vfsrq });
            if (iRe != 0)
            {
                SaveInItemsErr += "保存凭单出错,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null);
                // throw new Exception("保存凭单出错,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
            if (!string.IsNullOrEmpty(SaveInItemsErr))
            {
                throw new Exception(SaveInItemsErr);
            }
        }
        #endregion

        #region 删除住院费用信息服务
        /// <summary>
        /// 删除住院费用信息服务 在调用此方法之前要先调用init_zy(string p_blh)方法
        /// </summary>
        /// <param name="p_blh">*病历号</param>
        public void DelAllInItems(string p_blh)
        {
            InitZY(p_blh);
            int iRe = seiproxy.ExeFuncReInt("destroy_all_zypd", new object[] { p_blh });
            if (iRe != 0)
            {
                throw new Exception("删除住院费用出错,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion

        #region 出院结算服务
        /// <summary>
        /// 出院结算服务
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> SettleZY(string strPara)
        {
            Dictionary<string, string> settleInfo = new Dictionary<string, string>();
            int iRe = 0;
            if (strPara == "wkzy") //普通人员无卡住院
            {
                iRe = seiproxy.ExeFuncReInt("settle_wkzy", null);
            }
            else
            {
                iRe = seiproxy.ExeFuncReInt("settle_zy", null);
            }

            if (iRe != 0)
            {
                throw new Exception("出院结算失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }

            settleInfo.Add("brjsh", GetReturnString("brjsh"));                               //医保系统的病人结算号
            settleInfo.Add("brfdje", GetReturnDec("brfdje").ToString());                     //病人负担金额（包含账户支付）
            settleInfo.Add("ybfdje", GetReturnDec("ybfdje").ToString());                     //医保负担金额
            settleInfo.Add("ylbzje", GetReturnDec("ylbzje").ToString());                     //医疗补助金额(优抚对象补助)
            settleInfo.Add("grzhzf", GetReturnDec("grzhzf").ToString());                     //个人账户支付
            settleInfo.Add("yljmje", GetReturnDec("yljmje").ToString());                    //优抚对象减免金额
            settleInfo.Add("yyfdje", GetReturnDec("yyfdje").ToString());                    //医院负担金额（该费用包含减免费用）
            settleInfo.Add("cbcwf", GetReturnDec("cbcwf").ToString());                      //医院负担金额（该费用包含减免费用） 
            settleInfo.Add("tczf", GetReturnDec("tczf").ToString());                      //统筹支付新增开始
            settleInfo.Add("dezf", GetReturnDec("dezf").ToString());                      //大额支付
            settleInfo.Add("ecbxje", GetReturnDec("ecbxje").ToString());                      //二次报销金额
            settleInfo.Add("wjbfje", GetReturnDec("wjbfje").ToString());                      //卫计帮扶
            settleInfo.Add("mzbzje", GetReturnDec("mzbzje").ToString());                      //民政补助
            settleInfo.Add("pkrkbcbxje", GetReturnDec("pkrkbcbxje").ToString());

            LogManager.Debug("医保系统的病人预结算号:" + GetReturnString("brjsh") + "");
            //打印结算单据
            string brjsh = GetReturnString("brjsh");
            try
            {
                //打印发票
                seiproxy.ExeFuncReInt("printdj", new object[] { brjsh, "FP" });
                //打印打印结算单
                seiproxy.ExeFuncReInt("printdj", new object[] { brjsh, "JSD" });

            }
            catch { }

            return settleInfo;
        }
        #endregion

        //#region 出院结算服务
        ///// <summary>
        ///// 出院结算服务
        ///// </summary>
        ///// <returns></returns>
        //public Dictionary<string, string> SettleZYJm(string strPara)
        //{
        //    Dictionary<string, string> settleInfo = new Dictionary<string, string>();
        //    int iRe = 0;
        //    if (strPara == "wkzy") //普通人员无卡住院
        //    {
        //        iRe = seiproxy.ExeFuncReInt("settle_wkzy", null);

        //        if (iRe != 0)
        //        {
        //            throw new Exception("出院结算失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
        //        }

        //        settleInfo.Add("brjsh", GetReturnString("brjsh"));                               //医保系统的病人结算号
        //        settleInfo.Add("brfdje", GetReturnDec("brfdje").ToString());                     //病人负担金额（包含账户支付）
        //        settleInfo.Add("ybfdje", GetReturnDec("ybfdje").ToString());                     //医保负担金额
        //        settleInfo.Add("ylbzje", GetReturnDec("ylbzje").ToString());                     //医疗补助金额(优抚对象补助)
        //        settleInfo.Add("grzhzf", GetReturnDec("grzhzf").ToString());                     //个人账户支付
        //        settleInfo.Add("yljmje", GetReturnDec("yljmje").ToString());                     //优抚对象减免金额
        //        settleInfo.Add("yyfdje", GetReturnDec("yyfdje").ToString());                     //医院负担金额（该费用包含减免费用）
        //        settleInfo.Add("cbcwf", GetReturnDec("cbcwf").ToString());                       //医院负担金额（该费用包含减免费用） 
        //        //settleInfo.Add("wjbfje", GetReturnDec("wjbfje").ToString());                     //危急帮扶金额
        //        //settleInfo.Add("mzbzje", GetReturnDec("mzbzje").ToString());                     //民政补助金额 
        //        //settleInfo.Add("pkrkbcbxje", GetReturnDec("pkrkbcbxje").ToString());             //商业兜底
        //    }
        //    else
        //    {
        //        iRe = seiproxy.ExeFuncReInt("settle_zy", null);


        //        if (iRe != 0)
        //        {
        //            throw new Exception("出院结算失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
        //        }

        //        settleInfo.Add("brjsh", GetReturnString("brjsh"));                               //医保系统的病人结算号
        //        settleInfo.Add("brfdje", GetReturnDec("brfdje").ToString());                     //病人负担金额（包含账户支付）
        //        settleInfo.Add("ybfdje", GetReturnDec("ybfdje").ToString());                     //医保负担金额
        //        settleInfo.Add("ylbzje", GetReturnDec("ylbzje").ToString());                     //医疗补助金额(优抚对象补助)
        //        settleInfo.Add("grzhzf", GetReturnDec("grzhzf").ToString());                     //个人账户支付
        //        settleInfo.Add("yljmje", GetReturnDec("yljmje").ToString());                     //优抚对象减免金额
        //        settleInfo.Add("yyfdje", GetReturnDec("yyfdje").ToString());                     //医院负担金额（该费用包含减免费用）
        //        settleInfo.Add("cbcwf", GetReturnDec("cbcwf").ToString());                       //医院负担金额（该费用包含减免费用） 
        //        settleInfo.Add("wjbfje", GetReturnDec("wjbfje").ToString());                     //危急帮扶金额
        //        settleInfo.Add("mzbzje", GetReturnDec("mzbzje").ToString());                     //民政补助金额 
        //        settleInfo.Add("pkrkbcbxje", GetReturnDec("pkrkbcbxje").ToString());             //商业兜底
        //    }



        //    //打印结算单据
        //    string brjsh = GetReturnString("brjsh");
        //    try
        //    {
        //        //打印发票
        //        seiproxy.ExeFuncReInt("printdj", new object[] { brjsh, "FP" });
        //        //打印打印结算单
        //        seiproxy.ExeFuncReInt("printdj", new object[] { brjsh, "JSD" });

        //    }
        //    catch { }

        //    return settleInfo;
        //}
        //#endregion


        #region 撤销出院服务
        /// <summary>
        /// 撤销出院服务
        /// </summary>
        /// <param name="p_blh">*病历号</param>
        /// <param name="p_brjsh">*病人结算号</param>
        public void CancleZYSettle(string p_blh, string p_brjsh)
        {
            InitZY(p_blh);
            //撤销出院
            int iRe = seiproxy.ExeFuncReInt("destroy_cy", null);
            if (iRe != 0)
            {
                throw new Exception("撤销出院失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
            //撤销住院结算
            iRe = seiproxy.ExeFuncReInt("destroy_zyjs", new object[] { p_brjsh });
            if (iRe != 0)
            {
                throw new Exception("撤销住院结算失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion

        #region 打印单据
        /// <summary>
        /// 打印单据
        /// </summary>
        /// <param name="vbrjsh">医保系统中的病人结算号</param>
        /// <param name="vdjlx">打印的单据类型（‘FP’: 打印发票（必选）‘JSD’:打印结算单（可选））</param>
        public void PrintDJ(string vbrjsh, string vdjlx)
        {
            try
            {
                seiproxy.ExeFuncReInt("printdj", new object[] { vbrjsh, vdjlx });
            }
            catch (Exception ex)
            {
                LogManager.Info(ex.Message + "异常调用：" + ex.TargetSite + "||调用堆栈" + ex.StackTrace + "||source" + ex.Source);
            }
        }
        #endregion


        #region 病历首页初始化
        /// <summary>
        /// 病历首页初始化
        /// </summary>
        /// <param name="vbrjsh">医保系统中的病人结算号</param>
        /// <param name="vdjlx">打印的单据类型（‘FP’: 打印发票（必选）‘JSD’:打印结算单（可选））</param>
        public void BlsyCsh(string blh)
        {
            int iRe = seiproxy.ExeFuncReInt("init_case", new object[] { blh });
            if (iRe != 0)
            {
                throw new Exception("病历首页初始化,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }

        #endregion



        //#region 保存病历首页内容
        ///// <summary>
        ///// 保存病历首页内容
        ///// </summary>

        //public void BlsyBc(InPayParameter InPara)
        //{

        //    foreach (var fee in InPara.Details)
        //    {
        //        seiproxy.ExeFuncReObj("set_case_dec", new object[] { "CCUTS", });                    //CCUTS天数
        //        seiproxy.ExeFuncReObj("set_case_dec", new object[] { "ICUTS", });                    //ICUTS天数
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "BMYXM", });                 //编码员姓名
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "BAZL", });                  //病案质量
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "BLZD1", });                 //病理诊断1
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "BLZD1ICD", });              //病理诊断1ICD
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "BLZD2", });                 //病理诊断2
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "BLZD2ICD", });              //病理诊断2ICD
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "LRWBBZ", });                //病历首页录完标志
        //        seiproxy.ExeFuncReObj("set_case_dec", new object[] { "BWTS", });                     //病危天数
        //        seiproxy.ExeFuncReObj("set_case_dec", new object[] { "BZTS", });                     //病重天数
        //        seiproxy.ExeFuncReObj("set_case_datetime", new object[] { "CSRQ", });                //出生日期
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "CYBS", });                  //出院病室(床位)
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "CYJZYS", });                //出院经治医生
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "CYJZYSBM", });              //出院经治医生编码
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "CYKS", });                  //出院科室
        //        seiproxy.ExeFuncReObj("set_case_datetime", new object[] { "CYRQ", });                //出院日期
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "CYZD1", });                 //出院诊断1
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "CYZD1ICD", });              //出院诊断1ICD
        //        seiproxy.ExeFuncReObj("set_case_dec", new object[] { "CYZD1ZLTS", });                //出院诊断1治疗天数 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "CYZD1ZLXG", });             //出院诊断1治疗效果 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "CYZD2", });                 //出院诊断2
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "CYZD2ICD", });              //出院诊断2ICD
        //        seiproxy.ExeFuncReObj("set_case_dec", new object[] { "CYZD2ZLTS", });                //出院诊断2治疗天数 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "CYZD2ZLXG", });             //出院诊断2治疗效果
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "DZ", });                    //地址
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSCZBM1", });               //第1次手术操作编码
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSCZLX1", });               //第1次手术操作类型
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSCZMC1", });               //第1次手术操作名称 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSMBFS1", });               //第1次手术麻醉方法
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSYHDJ1", });               //第1次手术切口愈合等级 
        //        seiproxy.ExeFuncReObj("set_case_datetime", new object[] { "SSRQ1", });               //第1次手术日期  
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSCZBM2", });               //第二次手术操作编码
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSCZLX2", });               //第二次手术操作类型
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSCZMC2", });               //第二次手术操作名称 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSMBFS2", });               //第二次手术麻醉方法
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSYHDJ2", });               //第二次手术切口愈合等级 
        //        seiproxy.ExeFuncReObj("set_case_datetime", new object[] { "SSRQ2", });               //第二次手术日期
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSCZBM3", });               //第3次手术操作编码
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSCZLX3", });               //第3次手术操作类型
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSCZMC3", });               //第3次手术操作名称 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSMBFS3", });               //第3次手术麻醉方法
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSYHDJ3", });               //第3次手术切口愈合等级 
        //        seiproxy.ExeFuncReObj("set_case_datetime", new object[] { "SSRQ3", });               //第3次手术日期  
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSCZBM4", });               //第4次手术操作编码
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSCZLX4", });               //第4次手术操作类型
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSCZMC4", });               //第4次手术操作名称 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSMBFS4", });               //第4次手术麻醉方法
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSYHDJ4", });               //第4次手术切口愈合等级 
        //        seiproxy.ExeFuncReObj("set_case_datetime", new object[] { "SSRQ4", });               //第4次手术日期  
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "DH", });                    //电话 
        //        seiproxy.ExeFuncReObj("set_case_dec", new object[] { "EJHLTS", });                   //二级护理天数 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "FSSHFH", });                //放射与术后诊断符合 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "GMYW1", });                 //过敏药物1 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "GMYW2", });                 //过敏药物2 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "JXYS", });                  //进修医师 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "KZRXM", });                 //科主任姓名 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "LXRDZ", });                    //联系人地址 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "LXRDH", });                    //联系人电话 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "LXRGX", });                    //联系人关系 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "LXRXM", });                    //联系人姓名 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "LCBLFH", });                    //临床与病理诊断符合 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "MZZD1", });                    //门急诊诊断1 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "MZZD2", });                    //门急诊诊断2 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "MZCYFH", });                    //门诊与出院诊断符合 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "MZZD1BM", });                    //门诊诊断1编码 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "MZZD2BM", });                    //门诊诊断2编码 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "MZZDYS", });                    //门诊诊断医生 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "MZZDYSBM", });                    //门诊诊断医生编码 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "NL", });                    //年龄 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "QTBFZ", });                    //其他并发症             
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "QTBFZICD", });                    //其他并发症ICD 
        //        seiproxy.ExeFuncReObj("set_case_datetime", new object[] { "QTBFZRQ", });                    //其他并发症发生日期     
        //        seiproxy.ExeFuncReObj("set_case_dec", new object[] { "QTBFZZLTS", });                    //其他并发症治疗天数 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "QTBFZZLXG", });                    //其他并发症治疗效果
        //        seiproxy.ExeFuncReObj("set_case_dec", new object[] { "QJCGCS", });                    //抢救成功次数 
        //        seiproxy.ExeFuncReObj("set_case_dec", new object[] { "QJTS", });                    //抢救天数     
        //        seiproxy.ExeFuncReObj("set_case_datetime", new object[] { "QZRQ", });                    //确诊日期 
        //        seiproxy.ExeFuncReObj("set_case_dec", new object[] { "QZTS", });                    //确诊天数   
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "RYBS", });                    //入院病室(床位)
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "RYFS", });                    //入院方式
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "RYJZYS", });                    //入院经治医生
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "RYJZYSBM", });                    //入院经治医生编码
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "RYKS", });                    //入院科室
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "RYQK", });                    //入院情况
        //        seiproxy.ExeFuncReObj("set_case_datetime", new object[] { "RYRQ", });                    //入院日期
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "RYCYFH", });                    //入院与出院诊断符合
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "RYZD1", });                    //入院诊断1
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "RYZD1ICD", });                    //入院诊断1ICD
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "RYZD2", });                    //入院诊断2
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "RYZD2ICD", });                    //入院诊断2ICD
        //        seiproxy.ExeFuncReObj("set_case_datetime", new object[] { "SCRQ", });                    //上传日期
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SHBZHM", });                    //社会保障号码
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SFZHM", });                    //身份证号码
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SJBZ", });                    //尸检标志
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SXYS", });                    //实习医师
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSYS1", });                    //手术医生1
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSYS2", });                    //手术医生2
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSYS3", });                    //手术医生3
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SSYS4", });                    //手术医生4
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SQSHFH", });                    //术前与术后诊断符合 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SWYY", });                    //死亡原因               
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "SWYYICD", });                    //死亡原因ICD  
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "ZDMC", });                    //损伤/中毒名称 
        //        seiproxy.ExeFuncReObj("set_case_dec", new object[] { "THTS", });                    //特护天数  
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "WZQJYY", });                    //危症抢救原因 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "XB", });                    //性别 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "XM", });                    //姓名 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "XX", });                    //血型 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "YJSSXYS", });                    //研究生实习医师 
        //        seiproxy.ExeFuncReObj("set_case_dec", new object[] { "YJHLTS", });                    //一级护理天数           
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "YZBM", });                    //邮政编码 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "YNGR", });                    //院内感染 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "YNGRICD", });                    //院内感染ICD    
        //        seiproxy.ExeFuncReObj("set_case_datetime", new object[] { "YNGRRQ", });                    //院内感染发生日期       
        //        seiproxy.ExeFuncReObj("set_case_dec", new object[] { "YNGRZLTS", });                    //院内感染治疗天数       
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "YNGRZLXG", });                    //院内感染治疗效果       
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "ZKHS", });                    //质控护士     
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "ZKYSBM", });                    //质控医师编码 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "ZKYS", });                    //质控医师姓名 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "ZRYS", });                    //主任医生姓名 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "ZRYSBM", });                    //主任医师编码 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "ZZYSBM", });                    //主治医生编码 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "ZYCS", });                    //住院次数 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "ZYTS", });                    //住院天数               
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "ZYYSBMDH", });                    //住院医生编码 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "ZYYS", });                    //住院医生姓名 
        //        seiproxy.ExeFuncReObj("set_case_string", new object[] { "ZZYS", });                    //住址医生姓名 
        //        seiproxy.ExeFuncReObj("set_case_dec", new object[] { "ZKCS", });                    //转科次数               


        //        int iRe = seiproxy.ExeFuncReInt("save_case", null);
        //        if (iRe != 0)
        //        {
        //            throw new Exception("保存病历首页内容,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
        //        }

        //    }


        //}

        //#endregion

    }
}
