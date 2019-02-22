using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using System.Threading;
using Malkavian;
namespace DW_YBBX
{
    class DW_Handle
    {
        /// <summary>
        /// 没有用继承
        /// </summary>
        public static RefCOM seiproxy;
        /// <summary>
        /// 构造函数
        /// </summary>
        public DW_Handle()
        {
            ReleaseComObj();
            DareWayInit();
        }

        //public static string HISDBStr = "Data Source=172.18.0.25;Initial Catalog=COMM;Persist Security Info=True;User ID=sa;Password=m@ssunsoft009";
        public MSSQLHelpers SqlHelper = new MSSQLHelpers(ConfigurationManager.ConnectionStrings["connStr"].ToString());


        public string zgtc_ID = ConfigurationManager.AppSettings["ZGTC_PATID"].ToString();
        public string ptmz_ID = ConfigurationManager.AppSettings["PTMZ_PATID"].ToString();
        public string zgmg_ID = ConfigurationManager.AppSettings["ZGMG_PATID"].ToString();
        public string zgzy_ID = ConfigurationManager.AppSettings["ZGZY_PATID"].ToString();
        public string jmmg_ID = ConfigurationManager.AppSettings["JMMG_PATID"].ToString();
        public string mfyy_ID = ConfigurationManager.AppSettings["MFYY_PATID"].ToString();
        public string jmzy_ID = ConfigurationManager.AppSettings["JMZY_PATID"].ToString();

        //public MSSQLHelpers SqlHelper = new MSSQLHelpers(HISDBStr);
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
        /// 获取返回数值
        /// </summary>
        /// <param name="strPara"></param>
        /// <returns></returns>
        public decimal GetReturnDec(string strPara)
        {
            return seiproxy.ExeFuncReDec("result_n", new object[] { strPara });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public string ExeFuncReStr(string p1, object[] p2)
        {
            return seiproxy.ExeFuncReStr(p1, p2);
        }

        /// <summary>
        /// 获取返回数值
        /// </summary>
        /// <param name="strPara"></param>
        /// <returns></returns>
        public DateTime GetReturnDate(string strPara)
        {
            return seiproxy.ExeFuncReDatetime("getvardatetime", new object[] { strPara });
        }
        #endregion
        #region 设置入参
        /// <summary>
        /// 清空入参
        /// </summary>
        public void ClearInPara()
        {

        }
        /// <summary>
        /// 释放COM对象
        /// </summary>
        public void ReleaseComObj()
        {
            try
            {
                if (seiproxy != null)
                {
                    //MessageBox.Show("1");
                    seiproxy.Dispose();
                    seiproxy = new RefCOM("embeded_interface");
                    //MessageBox.Show("2");
                }
            }
            catch (Exception desEx)
            {
                try
                {
                    MessageBox.Show("DisposeSelf::" + desEx.Message + "  " + desEx.StackTrace + " " + desEx.Source, "");
                }
                catch (Exception desEx1)
                {
                    MessageBox.Show("DisposeSelf_err::" + desEx1.Message + "  ", "");
                }
            }
        }
        /// <summary>
        ///设置字符串入参
        /// </summary>
        /// <param name="strPara"></param>
        /// <returns></returns>
        public decimal SetInParaString(string strPara, object strValue)
        {
            return seiproxy.ExeFuncReDec("putvarstring", new object[] { strPara, strValue });
        }
        /// <summary>
        /// 设置数值入参
        /// </summary>
        /// <param name="strPara"></param>
        /// <returns></returns>
        public decimal SetInParaDec(string strPara, object strValue)
        {
            return seiproxy.ExeFuncReDec("putvardec", new object[] { strPara, strValue });
        }

        /// <summary>
        /// 设置数值入参
        /// </summary>
        /// <param name="strPara"></param>
        /// <returns></returns>
        public decimal SetInParaDate(string strPara, object strValue)
        {
            return seiproxy.ExeFuncReDec("putvardatetime", new object[] { strPara, strValue });
        }

        /// <summary>
        /// 设置数值入参
        /// </summary>
        /// <param name="strPara"></param>
        /// <returns></returns>
        public decimal ExecService(string serviceName)
        {
            return seiproxy.ExeFuncReDec("request_service", new object[] { serviceName });
        }
        #endregion
        #region 地维登陆
        /// <summary>
        /// 地维登陆
        /// </summary>
        public void DareWayInit()
        {
            seiproxy = new RefCOM("embeded_interface");
            string hosNo = MainForm.yybm; //"011102"; //MainForm.yybm;
            string usercode = MainForm.UserCode_DW;//"0001"; //MainForm.UserCode_DW;
            string password = MainForm.Password_DW;//"1234"; //MainForm.Password_DW;
            int iRe = seiproxy.ExeFuncReInt("init", new object[] { usercode, hosNo, password });
            Common.WriteLog("地维初始化", "usercode:" + usercode + "hosNo:" + hosNo + "password;" + password, Common.Now());

            if (iRe != 0)
            {
                throw new Exception("登陆失败 连接医保服务器出错,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion
        #region 读医保卡，取得人员相关信息
        /// <summary>
        /// 读医保卡，取得人员相关信息。
        /// </summary>
        /// <param name="yltclb">0为取卡片基本信息，1为住院，4为门诊大病(特病)，6为普通门诊 </param>
        /// <param name="jymmbz">‘1’代表校验密码，‘0’代表不校验密码，(可选参数)</param>
        /// <param name="mzdebz">职工刷卡消费个人账户及居民消费门诊定额时，该参数应传入1，其他情况可不传入或传入为0</param>
        /// <param name="readertype">读卡器类型	（可选参数）</param>
        public Dictionary<string, string> ReadCard(string yltclb, string jymmbz, string readertype)
        {
            Dictionary<string, string> patinfo = new Dictionary<string, string>();
            ClearInPara();
            SetInParaString("yltclb", yltclb);
            SetInParaString("jymmbz", jymmbz);
            SetInParaString("readertype", readertype);
            decimal iRe = ExecService("read_card");
            if (iRe != 0)
            {
                throw new Exception("读卡失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
            patinfo.Add("sbjgbh", GetReturnString("sbjgbh"));         //社保机构编号
            patinfo.Add("rqlb", GetReturnString("rqlb"));               //人群类别
            patinfo.Add("kh", GetReturnString("kh"));               //*卡号
            patinfo.Add("xm", GetReturnString("xm"));               //姓名
            patinfo.Add("xb", GetReturnString("xb"));               //性别,1:男,2:女,9:不确定
            patinfo.Add("sfzhm", GetReturnString("sfzhm"));       //身份证号码
            patinfo.Add("zfbz", GetReturnString("zfbz"));           //灰白名单标志:0 代表灰名单,1 白名单
            patinfo.Add("zfsm", GetReturnString("zfsm"));           //灰名单原因(如果是白名单该值为空)
            patinfo.Add("dwmc", GetReturnString("dwmc"));           //单位名称
            patinfo.Add("ylrylb", GetReturnString("ylrylb"));       //人员类别(汉字)
            patinfo.Add("ydbz", GetReturnString("ydbz"));           //是否为异地人员 (1:是,0: 否)
            patinfo.Add("mzdbjbs", GetReturnString("mzdbjbs"));     //疾病编码
            //patinfo.Add("sbjbm", GetReturnString("sbjbm"));         //社保局编码,山东省直为379902
            patinfo.Add("zhzybz", GetReturnString("zhzybz"));       //有无15(医保参数制)天内的住院记录1:有 ,0 :无
            patinfo.Add("zhzysm", GetReturnString("zhzysm"));       //5(医保参数控制)天内的住院记录说明
            patinfo.Add("zcyymc", GetReturnString("zcyymc"));       //转出医院名称(如果zcyymc不为’’,则表示本次住院时候市内转院来的)
            patinfo.Add("ye", GetReturnDec("ye").ToString());       //账户余额
            patinfo.Add("grbh", GetReturnString("grbh"));       //获取持卡人的个人编号
            patinfo.Add("mzdbbz", GetReturnString("mzdbbz"));       //A门诊大病备注
            return patinfo;
        }
        #endregion
        #region 取得人员相关信息
        /// <summary>
        /// 取得人员相关信息
        /// </summary>
        /// <param name="p_grbh">社会保障号码或者身份证号</param>
        /// <param name="p_xm">姓名(该姓名必须和医保数据库中一致)</param>
        /// <param name="p_yltclb">*医疗统筹类别(1,住院，4 门诊大病,6 普通门诊)</param>
        /// <param name="p_sbjbm">*社保局编码</param>
        /// <returns></returns>

        public Dictionary<string, string> QueryBasicInfo(string p_grbh, string p_xm, string p_yltclb, string p_sbjbm)
        {
            Dictionary<string, string> patinfo = new Dictionary<string, string>();
            ClearInPara();
            SetInParaString("grbh", p_grbh);
            SetInParaString("xm", p_xm);
            SetInParaString("yltclb", p_yltclb);
            SetInParaString("sbjgbh", p_sbjbm);
            decimal iRe = ExecService("query_person_info");
            if (iRe != 0)
            {
                throw new Exception("无卡查询失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
            patinfo.Add("sbjgbh", p_sbjbm);         //社保机构编号
            patinfo.Add("rqlb", GetReturnString("rqlb"));               //人群类别
            patinfo.Add("kh", "");               //*卡号
            patinfo.Add("xm", GetReturnString("xm"));               //姓名
            patinfo.Add("xb", GetReturnString("xb"));               //性别,1:男,2:女,9:不确定
            patinfo.Add("sfzhm", GetReturnString("sfzhm"));       //身份证号码
            patinfo.Add("zfbz", GetReturnString("zfbz"));           //灰白名单标志:0 代表灰名单,1 白名单
            patinfo.Add("zfsm", GetReturnString("zfsm"));           //灰名单原因(如果是白名单该值为空)
            patinfo.Add("dwmc", GetReturnString("dwmc"));           //单位名称
            patinfo.Add("ylrylb", GetReturnString("ylrylb"));       //人员类别(汉字)
            patinfo.Add("ydbz", GetReturnString("ydbz"));           //是否为异地人员 (1:是,0: 否)
            patinfo.Add("mzdbjbs", GetReturnString("mzdbjbs"));     //疾病编码
            //patinfo.Add("sbjbm", GetReturnString("sbjbm"));         //社保局编码,山东省直为379902
            patinfo.Add("zhzybz", GetReturnString("zhzybz"));       //有无15(医保参数制)天内的住院记录1:有 ,0 :无
            patinfo.Add("zhzysm", GetReturnString("zhzysm"));       //5(医保参数控制)天内的住院记录说明
            patinfo.Add("zcyymc", GetReturnString("zcyymc"));       //转出医院名称(如果zcyymc不为’’,则表示本次住院时候市内转院来的)
            patinfo.Add("ye", "0");       //账户余额
            patinfo.Add("grbh", GetReturnString("grbh"));       //获取持卡人的个人编号
            patinfo.Add("mzdbbz", GetReturnString("mzdbbz"));       //A门诊大病备注
            return patinfo;
        }
        #endregion
        #region 门诊初始化
        /// <summary>
        /// 门诊初始化
        /// </summary>
        /// <param name="p_sbjbm">社保局编码</param>
        /// <param name="p_yltclb">医疗统筹类别  yltclb:4 门诊大病， 6普通门诊</param>
        /// <param name="xzbz">*险种标志  医疗 C ；工伤 D ；生育 E</param>
        /// <param name="p_grbh">社会保障号码</param>
        /// <param name="p_xm">姓名</param>
        /// <param name="p_xb">性别</param>
        /// <param name="p_jbbm">疾病编码(yltclb=’4’时，必须传递；yltclb=’6’时，xzbz=’C’传递’’,xzbz=’D’或xzbz=’E’，必须传递)</param>
        /// <param name="fyrq">*费用发生日期(精确到天)</param>
        /// <param name="kh">医保卡编号(读卡必须传递，不读卡传’’)</param>
        /// <param name="ysbm">医师编码</param>
        /// <param name="p_mzdebz">职工刷卡消费个人账户及居民消费门诊定额时，该参数应传入1，其他情况可不传入或传入为0</param>
        /// <param name="p_bcxm">补充项目信息（扩展使用）</param>
        public void InitMZ(string p_sbjbm, string p_yltclb, string p_xzbz, string p_grbh, string p_xm, string p_xb, string p_jbbm, DateTime p_fyrq, string p_ylzbh, string p_ysbm, string p_mzdebz)
        {
            ClearInPara();
            SetInParaString("sbjgbh", p_sbjbm);
            SetInParaString("yltclb", p_yltclb);
            SetInParaString("xzbz", p_xzbz);
            SetInParaString("grbh", p_grbh);
            SetInParaString("xm", p_xm);
            SetInParaString("xb", p_xb);
            SetInParaString("jbbm", p_jbbm);
            SetInParaDate("fyrq", p_fyrq);
            SetInParaString("kh", p_ylzbh);
            SetInParaString("ysbm", p_ysbm);
            SetInParaString("mzlx", "");
            SetInParaString("bxlb", "");
            SetInParaString("mzghbh", "");
            decimal iRe = ExecService("init_mz");
            if (iRe != 0)
            {
                throw new Exception("门诊初始化失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion
        #region 保存门诊费用明细
        /// <summary>
        /// 保存门诊费用明细
        /// </summary>
        /// <param name="itemsDt"></param>
        public void SaveOutItems(DataTable itemsDt)
        {

            for (int i = 0; i < itemsDt.Rows.Count; i++)
            {
                ClearInPara();
                SetInParaString("yyxmbm", itemsDt.Rows[i]["医院项目编码"].ToString()); //医院项目编码
                SetInParaString("yyxmmc", itemsDt.Rows[i]["医院项目名称"]);                     //医院项目名称
                SetInParaDec("dj", itemsDt.Rows[i]["单价"]);                                  //最小包装的单价
                SetInParaDec("sl", itemsDt.Rows[i]["数量"]);                               //大包装数量
                SetInParaDec("bzsl", "1");                                 //大包装的小包装数量
                SetInParaDec("zje", itemsDt.Rows[i]["金额"]);                                //总金额（zje=dj*sl*bzsl）
                SetInParaString("gg", itemsDt.Rows[i]["规格"]);                                //规格
                SetInParaDec("sxzfbl", "0");                                 //*首先自负比例
                SetInParaDate("fyfssj", itemsDt.Rows[i]["费用发生时间"]);
                SetInParaString("zxksbm", itemsDt.Rows[i]["执行科室编码"]);                          //*执行科室编码
                SetInParaString("kdksbm", itemsDt.Rows[i]["开单科室编码"]);                          //*开单科室编码    
                SetInParaString("sm", "");                                 //说明
                SetInParaString("yzlsh", "");                                 //医嘱流水号
                SetInParaString("sfryxm", "");                                 //*收费人员姓名
                decimal iRe = ExecService("put_fymx");
                if (iRe != 0)
                {
                    throw new Exception("保存费用明细出错 项目编码为:" + itemsDt.Rows[i]["医院项目编码"].ToString() + "  项目名称为;" + itemsDt.Rows[i]["医院项目名称"].ToString() + ",医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
                }
            }
        }
        #endregion
        #region 门诊结算
        public Dictionary<string, string> SettleMZ()
        {
            Dictionary<string, string> settleInfo = new Dictionary<string, string>();
            decimal iRe = ExecService("settle_mz");
            if (iRe != 0)
            {
                throw new Exception("门诊结算失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }

            settleInfo.Add("jshid", GetReturnString("jshid"));                         //医保系统的病人结算号
            settleInfo.Add("brfdje", GetReturnDec("brfdje").ToString());               //病人负担金额（包含账户支付）
            settleInfo.Add("yyfdje", GetReturnDec("yyfdje").ToString());               //医院负担金额
            settleInfo.Add("ybfdje", GetReturnDec("ybfdje").ToString());               //医保负担金额
            settleInfo.Add("ylbzje", GetReturnDec("ylbzje").ToString());               //医疗补助金额(优抚对象补助)

            settleInfo.Add("grzhzf", GetReturnDec("grzhzf").ToString());               //个人账户支付

            settleInfo.Add("rylb", GetReturnString("tjrylb").ToString());                //人员类别
            settleInfo.Add("zje", GetReturnDec("zje").ToString());                     //本次结算费用总额
            settleInfo.Add("zhzf", GetReturnDec("zhzf").ToString());                   //暂缓支付
            settleInfo.Add("tczf", GetReturnDec("tczf").ToString());                   //本次统筹支付
            settleInfo.Add("dezf", GetReturnDec("dezf").ToString());                   //本次大额支付
            settleInfo.Add("yljmje", GetReturnDec("yljmje").ToString());               //医疗减免支付
            settleInfo.Add("qttczf", GetReturnDec("qttczf").ToString());               //其他统筹支付
            settleInfo.Add("ljtczf", GetReturnDec("ljtczf").ToString());               //累计统筹支付
            settleInfo.Add("ljdezf", GetReturnDec("ljdezf").ToString());               //累计大额支付
            settleInfo.Add("ljmzed", GetReturnDec("ljmzed").ToString());               //门诊累计额度
            settleInfo.Add("ljgrzf", GetReturnDec("ljgrzf").ToString());               //个人自费累计
            return settleInfo;
        }
        #endregion
        #region 撤销门诊结算
        /// <summary>
        /// 撤销门诊结算
        /// </summary>
        /// <param name="p_brjsh">地纬结算系统的病人结算号</param>
        public void CancelMZSettle(string p_brjsh)
        {
            int iRe = seiproxy.ExeFuncReInt("destroy_mzmg", new object[] { p_brjsh });
            if (iRe != 0)
            {
                throw new Exception("撤销结算出错,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion
        #region 个帐初始化
        /// <summary>
        /// 个帐初始化
        /// </summary>
        public void InitGZ()
        {
            int iRe = seiproxy.ExeFuncReInt("init_gz", null);
            if (iRe != 0)
            {
                throw new Exception("个人账户消费初始化出错,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion
        #region 个人账户消费服务
        /// <summary>
        /// 个人账户消费服务
        /// </summary>
        /// <param name="p_sbjbm">*社保局编码</param>
        /// <param name="p_ylzbh">*医疗证编号</param>
        public Dictionary<string, string> SettleGZ(string p_sbjbm, string p_ylzbh)
        {
            Dictionary<string, string> settleInfo = new Dictionary<string, string>();
            int iRe = seiproxy.ExeFuncReInt("settle_gz", new object[] { p_sbjbm, p_ylzbh });

            if (iRe != 0)
            {
                throw new Exception("个人账户结算失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }

            settleInfo.Add("mzzdlsh", GetReturnString("mzzdlsh"));             //此次个人账户消费的流水号
            settleInfo.Add("zje", GetReturnDec("zje").ToString());             //本次消费总金额
            settleInfo.Add("grzhzf", GetReturnDec("grzhzf").ToString());       //个人账户支付
            settleInfo.Add("xj", GetReturnDec("xj").ToString());               //病人支付现金
            return settleInfo;
        }
        #endregion
        #region 扣除医保卡相应金额
        /// <summary>
        /// 个人账户消费服务
        /// </summary>
        /// <param name="amount">特定金额</param>
        public Dictionary<string, string> SettleGZ(decimal amount)
        {
            Dictionary<string, string> settleInfo = new Dictionary<string, string>();
            //初始化服务
            InitGZ();
            int iRe;

            iRe = seiproxy.ExeFuncReInt("readcard", new object[] { "6", "" });
            if (iRe != 0)
            {
                throw new Exception("读卡失败:" + seiproxy.ExeFuncReStr("get_errtext", null));
            }

            string p_sbjbm = GetReturnString("sbjbm");
            string p_ylzbh = GetReturnString("ylzbh");

            seiproxy.ExeFuncReObj("new_mzmg_item", null);
            seiproxy.ExeFuncReObj("set_mzmg_item_string", new object[] { "yyxmbm", "300300_SI" }); //医院项目编码
            seiproxy.ExeFuncReObj("set_mzmg_item_dec", new object[] { "dj", amount });                    //最小包装的单价
            seiproxy.ExeFuncReObj("set_mzmg_item_dec", new object[] { "sl", "1" });                 //大包装数量
            seiproxy.ExeFuncReObj("set_mzmg_item_dec", new object[] { "bzsl", 1 });                   //  大包装的小包装数量
            seiproxy.ExeFuncReObj("set_mzmg_item_dec", new object[] { "zje", amount });                  //总金额（zje=dj*sl*bzsl）
            seiproxy.ExeFuncReObj("set_mzmg_item_string", new object[] { "ksbm", "001" });           //科室编码
            seiproxy.ExeFuncReObj("set_mzmg_item_string", new object[] { "gg", "/" });                  //规格
            seiproxy.ExeFuncReObj("set_mzmg_item_string", new object[] { "zxksbm", "001" });            //*执行科室编码
            seiproxy.ExeFuncReObj("set_mzmg_item_string", new object[] { "kdksbm", "001" });            //*开单科室编码
            seiproxy.ExeFuncReObj("set_mzmg_item_dec", new object[] { "sxzfbl", 0 });    //*自付比例
            seiproxy.ExeFuncReObj("set_mzmg_item_string", new object[] { "yyxmmc", "" });       //医院项目名称
            iRe = seiproxy.ExeFuncReInt("save_mzmg_item", null);
            if (iRe != 0)
            {
                throw new Exception("保存费用明细失败");
            }

            iRe = seiproxy.ExeFuncReInt("settle_gz", new object[] { p_sbjbm, p_ylzbh });
            if (iRe != 0)
            {
                throw new Exception("个人账户结算失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
            settleInfo.Add("mzzdlsh", GetReturnString("mzzdlsh"));             //此次个人账户消费的流水号
            settleInfo.Add("zje", GetReturnDec("zje").ToString());             //本次消费总金额
            settleInfo.Add("grzhzf", GetReturnDec("grzhzf").ToString());       //个人账户支付
            settleInfo.Add("xj", GetReturnDec("xj").ToString());               //病人支付现金
            return settleInfo;
        }
        #endregion
        #region 撤销门诊结算，删除费用凭单。
        /// <summary>
        /// 撤销门诊结算，删除费用凭单。
        /// </summary>
        /// <param name="jshid">*结算号id</param>
        public void CancelSettleMZ(string jshid)
        {
            ClearInPara();
            SetInParaString("jshid", jshid);
            decimal iRe = ExecService("destroy_mz");
            if (iRe != 0)
            {
                throw new Exception("撤销结算出错,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion
        #region 无凭单消费个账扣款
        /// <summary>
        /// 个人账户消费服务
        /// </summary>
        /// <param name="brjsh">病人结算号</param>
        public Dictionary<string, string> GZKK(string brjsh)
        {
            Dictionary<string, string> settleInfo = new Dictionary<string, string>();
            seiproxy.ExeFuncReInt("Clear_item", null);
            seiproxy.ExeFuncReInt("set_item_s", new object[] { brjsh });

            int iRe = seiproxy.ExeFuncReInt("gzkk", null);

            if (iRe != 0)
            {
                throw new Exception("个人账户结算失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }

            settleInfo.Add("mzzdlsh", GetReturnString("mzzdlsh"));             //此次个人账户消费的流水号
            settleInfo.Add("zje", GetReturnDec("zje").ToString());             //本次消费总金额
            settleInfo.Add("grzhzf", GetReturnDec("grzhzf").ToString());       //个人账户支付
            return settleInfo;
        }
        #endregion
        #region 普通住院登记服务
        /// <summary>
        /// 普通住院登记服务
        /// </summary>
        /// <param name="p_blh">*病例号</param>
        /// <param name="p_sfzhm">*社会保障号码</param>
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
        public void SaveZYDJ(string p_blh, string p_sfzhm, string p_ylzbh, string p_xm, string p_xb,
                   string p_yltclb, string p_sbjbm, string p_syzhlx, string p_ksbm, string p_zyrq, string p_qzys,
                   string p_mzks, string p_zyfs, string p_xzbz, string p_bcxm, string p_lytclbmx, string p_ryzd)
        {
            ClearInPara();
            SetInParaString("blh", p_blh);
            SetInParaString("grbh", p_sfzhm);
            SetInParaString("kh", p_ylzbh);
            SetInParaString("xm", p_xm);
            SetInParaString("xb", p_xb);
            SetInParaString("yltclb", "1");
            SetInParaString("yltclbmx", p_lytclbmx);
            SetInParaString("sbjgbh", p_sbjbm);
            SetInParaString("ksbm", p_ksbm);
            SetInParaDate("zyrq", p_zyrq);
            SetInParaString("qzys", p_qzys);
            SetInParaString("mzks", p_mzks);
            SetInParaString("zyfs", "1");
            SetInParaString("xzbz", p_xzbz);
            SetInParaString("cw", "");
            SetInParaString("fj", "");
            SetInParaString("bqsm", "");
            SetInParaString("ryzd", p_ryzd);
            decimal iRe = this.ExecService("save_zydj");
            if (iRe != 0)
            {
                throw new Exception("医保登记失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
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
            ClearInPara();
            SetInParaString("blh", p_blh);
            decimal iRe = this.ExecService("init_zy");
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
        public void CancelZY(string p_blh)
        {
            InitZY(p_blh);
            ClearInPara();
            SetInParaString("blh", p_blh);
            decimal iRe = this.ExecService("destroy_zydj");
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
        public void SaveInItems(DataTable itemsDt, string vysbm, string vfsrq, string inHosCode)
        {
            decimal iRe = 0;

            StringBuilder codeStr = new StringBuilder();
            for (int i = 0; i < itemsDt.Rows.Count; i++)
            {
                ClearInPara();
                SetInParaString("yyxmbm", itemsDt.Rows[i]["NETWORK_ITEM_CODE"].ToString());
                if (itemsDt.Rows[i]["NETWORK_ITEM_CODE"].ToString() == "")
                {
                    codeStr.Append("" + itemsDt.Rows[i]["CHARGE_NAME"].ToString() + ",");
                }
                SetInParaString("yyxmmc", itemsDt.Rows[i]["NETWORK_ITEM_NAME"].ToString());
                SetInParaDec("dj", Convert.ToDecimal(itemsDt.Rows[i]["PRICE"].ToString()));
                SetInParaDec("sl", Convert.ToDecimal(itemsDt.Rows[i]["QUANTITY"].ToString()));
                SetInParaDec("bzsl", 1);
                SetInParaDec("zje", Convert.ToDecimal(itemsDt.Rows[i]["AMOUNT"].ToString()));
                SetInParaString("gg", itemsDt.Rows[i]["SPEC"].ToString());
                if (itemsDt.Rows[i]["SELF_BURDEN_RATIO"].ToString() == "")
                {
                    SetInParaDec("sxzfbl", 1);
                }
                else
                {
                    SetInParaDec("sxzfbl", Convert.ToDecimal(itemsDt.Rows[i]["SELF_BURDEN_RATIO"].ToString()));
                }
                SetInParaDate("fyfssj", Convert.ToDateTime(itemsDt.Rows[i]["BILL_TIME"]));
                SetInParaString("zxksbm", itemsDt.Rows[i]["BILL_DEPT"].ToString());
                SetInParaString("kdksbm", itemsDt.Rows[i]["BILL_DEPT"].ToString());
                SetInParaString("sm", "");
                SetInParaString("yzlsh", "");
                SetInParaString("sfryxm", "");

                iRe = ExecService("put_fymx");
                if (iRe != 0)
                {
                    throw new Exception("保存费用明细出错 项目编码为:" + itemsDt.Rows[i]["NETWORK_ITEM_CODE"].ToString() + "  项目名称为;" + itemsDt.Rows[i]["CHARGE_NAME"].ToString() + ",医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
                }
            }

            ClearInPara();
            SetInParaString("ysbm", vysbm);//
            SetInParaDate("date", vfsrq);
            iRe = ExecService("save_zy_script");
            if (iRe != 0)
            {
                throw new Exception("保存凭单出错," + codeStr + "医保编码为空,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }

            ClearInPara();
            SetInParaString("sbjgbh", MainForm.sbjgbh);
            SetInParaString("blh", inHosCode);
            iRe = ExecService("trans_zy_script");
            if (iRe != 0)
            {
                throw new Exception("上传凭单出错,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
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
            ClearInPara();
            SetInParaString("blh", p_blh);
            decimal iRe = this.ExecService("delete_all_fypd");
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
        /// <param name="p_jkyjsbz">HIS系统通过该参数来控制出院结算是否是预结算（jkyjsbz=1:预结算；jkyjsbz=0:正常结算。如果HIS系统没有传入该参数，系统默认为正常结算）</param>
        /// <param name="datetime">出院日期 </param>
        /// <param name="cyzd">出院诊断</param>
        /// <param name="cyfs">出院方式</param>
        /// <param name="zlfs">治疗方式</param>
        /// <param name="dkbz">读卡标志</param>
        /// <returns></returns>
        public Dictionary<string, string> SettleZY(string p_jkyjsbz, string p_blh, DateTime datetime, string cyzd, string cyfs, string zlfs, string dkbz)
        {
            Dictionary<string, string> settleInfo = new Dictionary<string, string>();

            InitZY(p_blh);
            ClearInPara();
            SetInParaDate("cyrq", datetime);
            SetInParaString("cyzd", cyzd);
            SetInParaString("cyfs", cyfs);
            SetInParaString("zlfs", zlfs);
            SetInParaString("dkbz", dkbz);

            decimal iRe = ExecService("settle_zy_init");

            if (iRe != 0)
            {
                throw new Exception("出院结算失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }

            ClearInPara();
            SetInParaString("jkyjsbz", p_jkyjsbz);
            iRe = ExecService("settle_zy");

            if (iRe != 0)
            {
                throw new Exception("出院结算失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }

            if (p_jkyjsbz == "0")
            {
                settleInfo.Add("jshid", GetReturnString("jshid"));                               //医保系统的病人结算号
                settleInfo.Add("fph", GetReturnString("fph").ToString());                        //发票号
                settleInfo.Add("brjsrq", GetReturnDate("brjsrq").ToString());                  //病人结算日期
                settleInfo.Add("qtjshid", GetReturnString("qtjshid").ToString());                //其他病人结算号
            }

            settleInfo.Add("brfdje", GetReturnDec("brfdje").ToString());                     //病人负担金额（包含账户支付）
            settleInfo.Add("ybfdje", GetReturnDec("ybfdje").ToString());                     //医保负担金额
            settleInfo.Add("ylbzje", GetReturnDec("ylbzje").ToString());                     //医疗补助金额(优抚对象补助)
            settleInfo.Add("grzhzf", GetReturnDec("grzhzf").ToString());                     //个人账户支付
            settleInfo.Add("dezf", GetReturnDec("dezf").ToString());                         //本次大额支付
            settleInfo.Add("desybx", GetReturnDec("desybx").ToString());                     //大额商业保险
            settleInfo.Add("gwybz", GetReturnDec("gwybz").ToString());                       //公务员补助
            settleInfo.Add("czlz", GetReturnDec("czlz").ToString());                         //财政列支
            settleInfo.Add("tczf", GetReturnDec("tczf").ToString());                         //统筹支付
            settleInfo.Add("qttczf", GetReturnDec("qttczf").ToString());                     //其他统筹支付
            settleInfo.Add("zje", GetReturnDec("zje").ToString());                        //总金额
            if (p_jkyjsbz == "0")
            {
                //打印结算单据
                string jshid = GetReturnString("jshid");
                SetInParaString("jshid", jshid);
                SetInParaString("djlx", "JSD");
                ExecService("print_dj");
            }
            return settleInfo;
        }
        #endregion
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
            ClearInPara();
            SetInParaString("blh", p_blh);
            decimal iRe = this.ExecService("destroy_cy");
            if (iRe != 0)
            {
                throw new Exception("撤销出院失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
            //撤销住院结算
            ClearInPara();
            SetInParaString("blh", p_blh);
            iRe = this.ExecService("destroy_zyjs");
            if (iRe != 0)
            {
                throw new Exception("撤销住院结算失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion
        #region 打印结算单据服务
        /// <summary>
        /// 打印结算单据服务
        /// </summary>
        /// <param name="jshid">*结算号id</param>
        /// <param name="djlx">**单据类型(‘FP’：发票必选，‘JSD’：结算单可选，‘GRZH’：打印个人账户可选)</param>
        public void PrintDJ(string jshid, string djlx)
        {
            ClearInPara();
            SetInParaString("jshid", jshid);
            SetInParaString("djlx", djlx);
            decimal iRe = ExecService("print_dj");
            if (iRe != 0)
            {
                throw new Exception("打印单据错误,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion
        #region 下载医院医师信息服务
        /// <summary>
        /// 下载医院医师信息服务
        /// </summary>
        /// <param name="yybm">*医院编码</param>
        /// <param name="filename">*目录文件的带路径名(目录文件的带路径名,比如‘C:\ys.txt’,为‘’，则不产生)</param>
        /// <param name="filetype">*文件类型(0:表示excel文件,1:表示txt文件,2:表示csv文件，7:表示dbf2文件，8:表示dbf3文件)</param>
        /// <param name="has_head">是否包含表头(1:表示包含表头0:表示不包含表头（默认值为1）)</param>
        public void Down_Ys(string sbjgbh, string yybm, string filename, long filetype, long has_head)
        {
            ClearInPara();
            SetInParaString("sbjgbh", sbjgbh);
            SetInParaString("yybm", yybm);
            SetInParaString("filename", filename);
            SetInParaDec("filetype", filetype);
            SetInParaDec("has_head", has_head);
            decimal iRe = ExecService("down_ys");
            if (iRe != 0)
            {
                throw new Exception("下载医院医师信息出错：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion
        #region 下载医院项目目录及对应医保核心端目录的相关信息服务
        /// <summary>
        /// 下载医院项目目录及对应医保核心端目录的相关信息服务
        /// </summary>
        /// <param name="sbjgbh">*社保机构编号</param>
        /// <param name="filename">*目录文件的带路径名(目录文件的带路径名,比如‘C:\yyxm.txt’,为‘’，则不产生)</param>
        /// <param name="filetype">*文件类型(0:表示excel文件,1:表示txt文件,2:表示csv文件，7:表示dbf2文件，8:表示dbf3文件)</param>
        /// <param name="has_head">是否包含表头(1:表示包含表头0:表示不包含表头（默认值为1）)</param>
        public void Down_Yyxm(string sbjgbh, string filename, long filetype, long has_head)
        {
            decimal iRe = seiproxy.ExeFuncReInt("down_yyxm", new object[] { sbjgbh, filename, filetype, true });
            if (iRe != 0)
            {
                throw new Exception("下载医院项目出错：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion


        #region 获取职工门规自负比例

        public void Down_Yyxm_ZFBL_ZGmg(string p_sbjbm, string p_yyxmbm, string p_rq)
        {

            string iRe = seiproxy.ExeFuncReStr("get_zfbl", new object[] { p_sbjbm, p_yyxmbm, p_rq });
            string memo = "";
            //bl = GetReturnString("zfbl");
            string zfbl = "100";
            if (iRe == "0" || iRe == "")
            {
                zfbl = "0";
            }
            else
            {
                //a = iRe.Substring(0, iRe.IndexOf("#"));
                var arr = iRe.Split('/');
                if (arr.Length > 1)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i].Trim().Length == 0)
                        {
                            continue;
                        }
                        var tmpArr = arr[i].Split('#');
                        //if (Convert.ToDecimal(tmpArr[0]) < Convert.ToDecimal(zfbl))
                        //{
                        //    zfbl = tmpArr[0];
                        //    memo = tmpArr[1];
                        //    if (memo.Length > 50)
                        //    {
                        //        memo.Substring(0, 45);
                        //    }
                        //}
                        decimal jtmp0 = 0;
                        if (decimal.TryParse(tmpArr[0], out jtmp0))
                        {

                        }
                        else
                        {
                            continue;
                        }
                        if (Convert.ToDecimal(tmpArr[0]) < Convert.ToDecimal(zfbl))
                        {
                            zfbl = tmpArr[0];
                            memo = tmpArr[1];
                            if (memo.Length > 25)
                            {
                                memo = memo.Substring(0, 25);
                            }
                        }
                    }
                }
                else
                {
                    var tmpArr = arr[0].Split('#');
                    zfbl = tmpArr[0];
                    if (tmpArr.Length > 1)
                    {
                        memo = tmpArr[1];
                    }

                }
            }

            if (memo.Length >= 20)
            {
                memo = memo.Substring(0, 20);
            }

            string strDelete = @"DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID ='4' AND HOSPITAL_ID='" + MainForm.HOSPITAL_ID + "' AND NETWORK_ITEM_CODE='" + p_yyxmbm + @"'";
            SqlHelper.ExecSqlReDs(strDelete.ToString());

            string strInsert = @" INSERT INTO COMM.COMM.NETWORKING_ITEM_VS_HIS	
            ( NETWORKING_PAT_CLASS_ID ,
            ITEM_PROP ,
          HIS_ITEM_CODE ,
          HIS_ITEM_NAME ,
          NETWORK_ITEM_CODE ,
          NETWORK_ITEM_NAME ,
          SELF_BURDEN_RATIO ,
          MEMO ,
          START_TIME ,
          STOP_TIME ,
          TYPE_MEMO ,
          NETWORK_ITEM_PROP ,
          NETWORK_ITEM_CHARGE_CLASS ,
          HOSPITAL_ID ,
          NETWORK_ITEM_PRICE ,
          FLAG_DISABLED ,
          NETWORK_ITEM_FLAG_UP
        )
SELECT " + "'4'" + @" ,
          ITEM_PROP ,
          HIS_ITEM_CODE ,
          HIS_ITEM_NAME ,
          NETWORK_ITEM_CODE ,
          NETWORK_ITEM_NAME ,
          '" + zfbl + @"',
          '" + memo + @"',
          START_TIME ,
          STOP_TIME ,
          TYPE_MEMO ,
          NETWORK_ITEM_PROP ,
          NETWORK_ITEM_CHARGE_CLASS ,
          '" + MainForm.HOSPITAL_ID + @"' ,
          NETWORK_ITEM_PRICE ,
          FLAG_DISABLED ,
          NETWORK_ITEM_FLAG_UP
          
          FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID=" + ptmz_ID + @" AND HOSPITAL_ID='" + MainForm.HOSPITAL_ID + @"' AND 
          NETWORK_ITEM_CODE='" + p_yyxmbm + @"'";
            SqlHelper.ExecSqlReDs(strInsert.ToString());


        }
        // string hosid, string p_xzbz, string p_sbjglx

        /// <summary>
        /// 获取中心自付比例center窗体用 校准自付比例用typeCheck=0
        /// </summary>
        /// <param name="p_sbjbm"></param>
        /// <param name="p_yyxmbm"></param>
        /// <param name="p_rq"></param>
        /// <param name="hosid"></param>
        /// <param name="p_xzbz"></param>
        /// <param name="p_sbjglx"></param>
        public void Down_Yyxm_ZFBL_ZGmg(string p_sbjbm, string p_yyxmbm, string p_rq, string hosid, string p_xzbz, string p_sbjglx,string typeCheck)
        {

            string iRe = seiproxy.ExeFuncReStr("get_zfbl", new object[] { p_sbjbm, p_yyxmbm, p_rq });
            string memo = "";
            //bl = GetReturnString("zfbl");
            string zfbl = "100";
            if (iRe == "0" || iRe == "")
            {
                zfbl = "0";
            }
            else
            {
                //a = iRe.Substring(0, iRe.IndexOf("#"));
                var arr = iRe.Split('/');
                if (arr.Length > 1)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i].Trim().Length == 0)
                        {
                            continue;
                        }
                        var tmpArr = arr[i].Split('#');
                        //if (Convert.ToDecimal(tmpArr[0]) < Convert.ToDecimal(zfbl))
                        //{
                        //    zfbl = tmpArr[0];
                        //    memo = tmpArr[1];
                        //    if (memo.Length > 50)
                        //    {
                        //        memo.Substring(0, 45);
                        //    }
                        //}
                        decimal jtmp0 = 0;
                        if (decimal.TryParse(tmpArr[0], out jtmp0))
                        {

                        }
                        else
                        {
                            continue;
                        }
                        if (Convert.ToDecimal(tmpArr[0]) < Convert.ToDecimal(zfbl))
                        {
                            zfbl = tmpArr[0];
                            memo = tmpArr[1];
                            if (memo.Length > 25)
                            {
                                memo = memo.Substring(0, 25);
                            }
                        }
                    }
                }
                else
                {
                    var tmpArr = arr[0].Split('#');
                    zfbl = tmpArr[0];
                    if (tmpArr.Length > 1)
                    {
                        memo = tmpArr[1];
                    }

                }
            }

            if (memo.Length >= 20)
            {
                memo = memo.Substring(0, 20);
            }

            #region 删除插入 职工门规
            string strDelete = @"DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID ='"+zgmg_ID+"' AND HOSPITAL_ID='" + MainForm.HOSPITAL_ID + "' AND NETWORK_ITEM_CODE='" + p_yyxmbm + @"'";
            SqlHelper.ExecSqlReDs(strDelete.ToString());

            string strInsert = @" INSERT INTO COMM.COMM.NETWORKING_ITEM_VS_HIS	
            ( NETWORKING_PAT_CLASS_ID ,
            ITEM_PROP ,
          HIS_ITEM_CODE ,
          HIS_ITEM_NAME ,
          NETWORK_ITEM_CODE ,
          NETWORK_ITEM_NAME ,
          SELF_BURDEN_RATIO ,
          MEMO ,
          START_TIME ,
          STOP_TIME ,
          TYPE_MEMO ,
          NETWORK_ITEM_PROP ,
          NETWORK_ITEM_CHARGE_CLASS ,
          HOSPITAL_ID ,
          NETWORK_ITEM_PRICE ,
          FLAG_DISABLED ,
          NETWORK_ITEM_FLAG_UP
        )
SELECT " + "'"+ zgmg_ID + "'" + @" ,
          ITEM_PROP ,
          HIS_ITEM_CODE ,
          HIS_ITEM_NAME ,
          NETWORK_ITEM_CODE ,
          NETWORK_ITEM_NAME ,
          '" + zfbl + @"',
          '" + memo + @"',
          START_TIME ,
          STOP_TIME ,
          TYPE_MEMO ,
          NETWORK_ITEM_PROP ,
          NETWORK_ITEM_CHARGE_CLASS ,
          '" + MainForm.HOSPITAL_ID + @"' ,
          NETWORK_ITEM_PRICE ,
          FLAG_DISABLED ,
          NETWORK_ITEM_FLAG_UP
          
          FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID=" + ptmz_ID + @" AND HOSPITAL_ID='" + MainForm.HOSPITAL_ID + @"' AND 
          NETWORK_ITEM_CODE='" + p_yyxmbm + @"'";
            SqlHelper.ExecSqlReDs(strInsert.ToString());



            #endregion
            #region 职工住院
            string strDeleteZY = @"DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID ='" + zgzy_ID + "' AND HOSPITAL_ID='" + MainForm.HOSPITAL_ID + "' AND NETWORK_ITEM_CODE='" + p_yyxmbm + @"'";
            SqlHelper.ExecSqlReDs(strDeleteZY.ToString());

            string strInsertZy = @" INSERT INTO COMM.COMM.NETWORKING_ITEM_VS_HIS	
            ( NETWORKING_PAT_CLASS_ID ,
            ITEM_PROP ,
          HIS_ITEM_CODE ,
          HIS_ITEM_NAME ,
          NETWORK_ITEM_CODE ,
          NETWORK_ITEM_NAME ,
          SELF_BURDEN_RATIO ,
          MEMO ,
          START_TIME ,
          STOP_TIME ,
          TYPE_MEMO ,
          NETWORK_ITEM_PROP ,
          NETWORK_ITEM_CHARGE_CLASS ,
          HOSPITAL_ID ,
          NETWORK_ITEM_PRICE ,
          FLAG_DISABLED ,
          NETWORK_ITEM_FLAG_UP
        )
SELECT " + "'" + zgzy_ID + "'" + @" ,
          ITEM_PROP ,
          HIS_ITEM_CODE ,
          HIS_ITEM_NAME ,
          NETWORK_ITEM_CODE ,
          NETWORK_ITEM_NAME ,
          '" + zfbl + @"',
          '" + memo + @"',
          START_TIME ,
          STOP_TIME ,
          TYPE_MEMO ,
          NETWORK_ITEM_PROP ,
          NETWORK_ITEM_CHARGE_CLASS ,
          '" + MainForm.HOSPITAL_ID + @"' ,
          NETWORK_ITEM_PRICE ,
          FLAG_DISABLED ,
          NETWORK_ITEM_FLAG_UP
          
          FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID=" + ptmz_ID + @" AND HOSPITAL_ID='" + MainForm.HOSPITAL_ID + @"' AND 
          NETWORK_ITEM_CODE='" + p_yyxmbm + @"'";
            SqlHelper.ExecSqlReDs(strInsertZy.ToString());
            #endregion

        }

        #endregion

        #region 获取居民门规自负比例

        public void Down_Yyxm_ZFBL_JMmg(string p_sbjbm, string p_yyxmbm, string p_rq)
        {

            string iRe = seiproxy.ExeFuncReStr("get_zfbl", new object[] { p_sbjbm, p_yyxmbm, p_rq });
            string memo = "";
            //bl = GetReturnString("zfbl");
            string zfbl = "100";
            if (iRe == "0" || iRe == "")
            {
                zfbl = "0";
            }
            else
            {
                //a = iRe.Substring(0, iRe.IndexOf("#"));
                var arr = iRe.Split('/');
                if (arr.Length > 1)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i].Trim().Length == 0)
                        {
                            continue;
                        }
                        var tmpArr = arr[i].Split('#');
                        decimal jtmp0 = 0;
                        if (decimal.TryParse(tmpArr[0], out jtmp0))
                        {

                        }
                        else
                        {
                            continue;
                        }
                        if (Convert.ToDecimal(tmpArr[0]) < Convert.ToDecimal(zfbl))
                        {
                            zfbl = tmpArr[0];
                            memo = tmpArr[1];
                            if (memo.Length > 25)
                            {
                                memo = memo.Substring(0, 25);
                            }
                        }
                    }
                }
                else
                {
                    var tmpArr = arr[0].Split('#');
                    zfbl = tmpArr[0];
                    if (tmpArr.Length > 1)
                    {
                        memo = tmpArr[1];
                    }

                }
            }

            if (memo.Length >= 20)
            {
                memo = memo.Substring(0, 20);
            }

            string strDelete = @"DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID ='" + jmmg_ID + "' AND HOSPITAL_ID='" + MainForm.HOSPITAL_ID + "' AND NETWORK_ITEM_CODE='" + p_yyxmbm + @"'";
            SqlHelper.ExecSqlReDs(strDelete.ToString());

            string strInsert = @" INSERT INTO COMM.COMM.NETWORKING_ITEM_VS_HIS	
            ( NETWORKING_PAT_CLASS_ID ,
            ITEM_PROP ,
          HIS_ITEM_CODE ,
          HIS_ITEM_NAME ,
          NETWORK_ITEM_CODE ,
          NETWORK_ITEM_NAME ,
          SELF_BURDEN_RATIO ,
          MEMO ,
          START_TIME ,
          STOP_TIME ,
          TYPE_MEMO ,
          NETWORK_ITEM_PROP ,
          NETWORK_ITEM_CHARGE_CLASS ,
          HOSPITAL_ID ,
          NETWORK_ITEM_PRICE ,
          FLAG_DISABLED ,
          NETWORK_ITEM_FLAG_UP
        )
SELECT " + jmmg_ID + @" ,
          ITEM_PROP ,
          HIS_ITEM_CODE ,
          HIS_ITEM_NAME ,
          NETWORK_ITEM_CODE ,
          NETWORK_ITEM_NAME ,
          '" + zfbl + @"',
          '" + memo + @"',
          START_TIME ,
          STOP_TIME ,
          TYPE_MEMO ,
          NETWORK_ITEM_PROP ,
          NETWORK_ITEM_CHARGE_CLASS ,
          '" + MainForm.HOSPITAL_ID + @"' ,
          NETWORK_ITEM_PRICE ,
          FLAG_DISABLED ,
          NETWORK_ITEM_FLAG_UP
          
          FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID=" + ptmz_ID + "  AND HOSPITAL_ID='" + MainForm.HOSPITAL_ID + @"' AND 
          NETWORK_ITEM_CODE='" + p_yyxmbm + @"'";
            SqlHelper.ExecSqlReDs(strInsert.ToString());

            //MessageBox.Show("更新居民门规自负比例成功！");
        }


        public void Down_Yyxm_ZFBL_JMmg(string p_sbjbm, string p_yyxmbm, string p_rq, string hosid, string p_xzbz, string p_sbjglx)
        {

            #region 综合获取自付比例
            string iRe = seiproxy.ExeFuncReStr("get_zfbl", new object[] { p_sbjbm, p_yyxmbm, p_rq });
            string memo = "";
            //bl = GetReturnString("zfbl");
            string zfbl = "100";
            if (iRe == "0" || iRe == "")
            {
                zfbl = "0";
            }
            else
            {
                //a = iRe.Substring(0, iRe.IndexOf("#"));
                var arr = iRe.Split('/');
                if (arr.Length > 1)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i].Trim().Length == 0)
                        {
                            continue;
                        }
                        var tmpArr = arr[i].Split('#');
                        decimal jtmp0 = 0;
                        if (decimal.TryParse(tmpArr[0], out jtmp0))
                        {

                        }
                        else
                        {
                            continue;
                        }
                        if (Convert.ToDecimal(tmpArr[0]) < Convert.ToDecimal(zfbl))
                        {
                            zfbl = tmpArr[0];
                            memo = tmpArr[1];
                            if (memo.Length > 25)
                            {
                                memo = memo.Substring(0, 25);
                            }
                        }
                    }
                }
                else
                {
                    var tmpArr = arr[0].Split('#');
                    zfbl = tmpArr[0];
                    if (tmpArr.Length > 1)
                    {
                        memo = tmpArr[1];
                    }

                }
            }

            if (memo.Length >= 20)
            {
                memo = memo.Substring(0, 20);
            }

            #region 先删除后插入方式更新 居民门规
            string strDelete = @"DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID ='" + jmmg_ID + "' AND HOSPITAL_ID='" + MainForm.HOSPITAL_ID + "' AND NETWORK_ITEM_CODE='" + p_yyxmbm + @"'";
            SqlHelper.ExecSqlReDs(strDelete.ToString());

            string strInsert = @" INSERT INTO COMM.COMM.NETWORKING_ITEM_VS_HIS	
            ( NETWORKING_PAT_CLASS_ID ,
            ITEM_PROP ,
          HIS_ITEM_CODE ,
          HIS_ITEM_NAME ,
          NETWORK_ITEM_CODE ,
          NETWORK_ITEM_NAME ,
          SELF_BURDEN_RATIO ,
          MEMO ,
          START_TIME ,
          STOP_TIME ,
          TYPE_MEMO ,
          NETWORK_ITEM_PROP ,
          NETWORK_ITEM_CHARGE_CLASS ,
          HOSPITAL_ID ,
          NETWORK_ITEM_PRICE ,
          FLAG_DISABLED ,
          NETWORK_ITEM_FLAG_UP
        )
SELECT " + jmmg_ID + @" ,
          ITEM_PROP ,
          HIS_ITEM_CODE ,
          HIS_ITEM_NAME ,
          NETWORK_ITEM_CODE ,
          NETWORK_ITEM_NAME ,
          '" + zfbl + @"',
          '" + memo + @"',
          START_TIME ,
          STOP_TIME ,
          TYPE_MEMO ,
          NETWORK_ITEM_PROP ,
          NETWORK_ITEM_CHARGE_CLASS ,
          '" + MainForm.HOSPITAL_ID + @"' ,
          NETWORK_ITEM_PRICE ,
          FLAG_DISABLED ,
          NETWORK_ITEM_FLAG_UP
          
          FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID=" + ptmz_ID + "  AND HOSPITAL_ID='" + MainForm.HOSPITAL_ID + @"' AND 
          NETWORK_ITEM_CODE='" + p_yyxmbm + @"'";
            SqlHelper.ExecSqlReDs(strInsert.ToString());
            #endregion

            #region 居民住院更新

            string strDeleteZY = @"DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID ='" + jmzy_ID + "' AND HOSPITAL_ID='" + MainForm.HOSPITAL_ID + "' AND NETWORK_ITEM_CODE='" + p_yyxmbm + @"'";
            SqlHelper.ExecSqlReDs(strDeleteZY.ToString());

            string strInsertZY = @" INSERT INTO COMM.COMM.NETWORKING_ITEM_VS_HIS	
            ( NETWORKING_PAT_CLASS_ID ,
            ITEM_PROP ,
          HIS_ITEM_CODE ,
          HIS_ITEM_NAME ,
          NETWORK_ITEM_CODE ,
          NETWORK_ITEM_NAME ,
          SELF_BURDEN_RATIO ,
          MEMO ,
          START_TIME ,
          STOP_TIME ,
          TYPE_MEMO ,
          NETWORK_ITEM_PROP ,
          NETWORK_ITEM_CHARGE_CLASS ,
          HOSPITAL_ID ,
          NETWORK_ITEM_PRICE ,
          FLAG_DISABLED ,
          NETWORK_ITEM_FLAG_UP
        )
SELECT " + jmzy_ID + @" ,
          ITEM_PROP ,
          HIS_ITEM_CODE ,
          HIS_ITEM_NAME ,
          NETWORK_ITEM_CODE ,
          NETWORK_ITEM_NAME ,
          '" + zfbl + @"',
          '" + memo + @"',
          START_TIME ,
          STOP_TIME ,
          TYPE_MEMO ,
          NETWORK_ITEM_PROP ,
          NETWORK_ITEM_CHARGE_CLASS ,
          '" + MainForm.HOSPITAL_ID + @"' ,
          NETWORK_ITEM_PRICE ,
          FLAG_DISABLED ,
          NETWORK_ITEM_FLAG_UP
          
          FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID=" + ptmz_ID + "  AND HOSPITAL_ID='" + MainForm.HOSPITAL_ID + @"' AND 
          NETWORK_ITEM_CODE='" + p_yyxmbm + @"'";
            SqlHelper.ExecSqlReDs(strInsertZY.ToString());

            #endregion



            #region 居民免费用药更新
            string strDeleteMF = @"DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID ='" + mfyy_ID + "' AND HOSPITAL_ID='" + MainForm.HOSPITAL_ID + "' AND NETWORK_ITEM_CODE='" + p_yyxmbm + @"'";
            SqlHelper.ExecSqlReDs(strDeleteMF.ToString());

            string strInsertMF = @" INSERT INTO COMM.COMM.NETWORKING_ITEM_VS_HIS	
            ( NETWORKING_PAT_CLASS_ID ,
            ITEM_PROP ,
          HIS_ITEM_CODE ,
          HIS_ITEM_NAME ,
          NETWORK_ITEM_CODE ,
          NETWORK_ITEM_NAME ,
          SELF_BURDEN_RATIO ,
          MEMO ,
          START_TIME ,
          STOP_TIME ,
          TYPE_MEMO ,
          NETWORK_ITEM_PROP ,
          NETWORK_ITEM_CHARGE_CLASS ,
          HOSPITAL_ID ,
          NETWORK_ITEM_PRICE ,
          FLAG_DISABLED ,
          NETWORK_ITEM_FLAG_UP
        )
SELECT " + mfyy_ID + @" ,
          ITEM_PROP ,
          HIS_ITEM_CODE ,
          HIS_ITEM_NAME ,
          NETWORK_ITEM_CODE ,
          NETWORK_ITEM_NAME ,
          '" + zfbl + @"',
          '" + memo + @"',
          START_TIME ,
          STOP_TIME ,
          TYPE_MEMO ,
          NETWORK_ITEM_PROP ,
          NETWORK_ITEM_CHARGE_CLASS ,
          '" + MainForm.HOSPITAL_ID + @"' ,
          NETWORK_ITEM_PRICE ,
          FLAG_DISABLED ,
          NETWORK_ITEM_FLAG_UP
          
          FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID=" + ptmz_ID + "  AND HOSPITAL_ID='" + MainForm.HOSPITAL_ID + @"' AND 
          NETWORK_ITEM_CODE='" + p_yyxmbm + @"'";
            SqlHelper.ExecSqlReDs(strInsertMF.ToString());
            #endregion

            #endregion




            //MessageBox.Show("更新居民门规自负比例成功！");
        }

        #endregion

        #region 自付比例下载 居民

        public void Down_Yyxm_ZFBLJm(string p_sbjbm, string p_yyxmbm, string p_rq, string bcxm, string jmtc_patid, string hosid, string p_xzbz, string p_sbjglx)
        {

            #region 获取自付比例综合
            // jmtc_patid = ConfigurationManager.AppSettings["JMTC_PATID"].ToString();
            string iRe = seiproxy.ExeFuncReStr("get_zfbl_mz", new object[] { p_sbjbm, p_yyxmbm, p_rq, bcxm });
            string memo = "";
            //bl = GetReturnString("zfbl");
            string zfbl = "100";
            if (iRe == "0" || iRe == "")
            {
                zfbl = "0";
            }
            else
            {
                //a = iRe.Substring(0, iRe.IndexOf("#"));
                var arr = iRe.Split('/');
                if (arr.Length > 1)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i].Trim().Length == 0)
                        {
                            continue;
                        }
                        var tmpArr = arr[i].Split('#');
                        if (Convert.ToDecimal(tmpArr[0]) < Convert.ToDecimal(zfbl))
                        {
                            zfbl = tmpArr[0];
                            memo = tmpArr[1];
                            if (memo.Length > 25)
                            {
                                memo = memo.Substring(0, 25);
                            }
                        }
                    }
                }
                else
                {
                    var tmpArr = arr[0].Split('#');
                    zfbl = tmpArr[0];
                    if (tmpArr.Length > 1)
                    {
                        memo = tmpArr[1];
                    }

                }
            }

            if (memo.Length >= 20)
            {
                memo = memo.Substring(0, 20);
            }
            #region 居民统筹
            string strDelete = @"DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID ='" + jmtc_patid + "' AND HOSPITAL_ID='" + hosid + "' AND NETWORK_ITEM_CODE='" + p_yyxmbm + @"'";
            SqlHelper.ExecSqlReDs(strDelete.ToString());

            string strInsert = @" INSERT INTO COMM.COMM.NETWORKING_ITEM_VS_HIS	
            ( NETWORKING_PAT_CLASS_ID ,
            ITEM_PROP ,
          HIS_ITEM_CODE ,
          HIS_ITEM_NAME ,
          NETWORK_ITEM_CODE ,
          NETWORK_ITEM_NAME ,
          SELF_BURDEN_RATIO ,
          MEMO ,
          START_TIME ,
          STOP_TIME ,
          TYPE_MEMO ,
          NETWORK_ITEM_PROP ,
          NETWORK_ITEM_CHARGE_CLASS ,
          HOSPITAL_ID ,
          NETWORK_ITEM_PRICE ,
          FLAG_DISABLED ,
          NETWORK_ITEM_FLAG_UP
        )
SELECT '" + jmtc_patid + @"' ,
          ITEM_PROP ,
          HIS_ITEM_CODE ,
          HIS_ITEM_NAME ,
          NETWORK_ITEM_CODE ,
          NETWORK_ITEM_NAME ,
          '" + zfbl + @"',
          '" + memo + @"',
          START_TIME ,
          STOP_TIME ,
          TYPE_MEMO ,
          NETWORK_ITEM_PROP ,
          NETWORK_ITEM_CHARGE_CLASS ,
          '" + hosid + @"' ,
          NETWORK_ITEM_PRICE ,
          FLAG_DISABLED ,
          NETWORK_ITEM_FLAG_UP
          
          FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID=" + ConfigurationManager.AppSettings["PTMZ_PATID"].ToString() + @" AND HOSPITAL_ID='" + hosid + @"' AND 
          NETWORK_ITEM_CODE='" + p_yyxmbm + @"'";
            SqlHelper.ExecSqlReDs(strInsert.ToString()); 
            #endregion
            #endregion

            //住院葛根素  天麻素
            //门诊的葡萄糖


        }


        /// <summary>
        /// 获取自付比例校准 循环
        /// </summary>
        /// <param name="p_sbjbm"></param>
        /// <param name="p_yyxmbm"></param>
        /// <param name="p_rq"></param>
        /// <param name="tc_patid"></param>
        /// <param name="hosid"></param>
        /// <param name="p_xzbz"></param>
        /// <param name="p_sbjglx"></param>
        private void GetZfCheck(string p_sbjbm, string p_yyxmbm, string p_rq, string tc_patid, string hosid, string p_xzbz, string p_sbjglx)
        {
            string reStr = seiproxy.ExeFuncReStr("get_zfbl_xzbz_sbjglx", new object[] { p_sbjbm, p_yyxmbm, p_rq, p_xzbz, p_sbjglx });

            decimal tSelf = 0;
            if (decimal.TryParse(reStr, out tSelf))
            {
                string sqlUpdate = "  UPDATE  COMM.COMM.NETWORKING_ITEM_VS_HIS  SET SELF_BURDEN_RATIO='" + reStr + "' WHERE  HOSPITAL_ID='" + hosid + "' AND  NETWORK_ITEM_CODE='" + p_yyxmbm + "'" + "   AND NETWORKING_PAT_CLASS_ID='" + tc_patid + "'";
                SqlHelper.ExecSqlReInt(sqlUpdate);
            }
            else
            {

            }
        }

        public void Down_Yyxm_ZFBLJm(string p_sbjbm, string p_yyxmbm, string p_rq, string bcxm, string jmtc_patid, string hosid)
        {

            // jmtc_patid = ConfigurationManager.AppSettings["JMTC_PATID"].ToString();
            string iRe = seiproxy.ExeFuncReStr("get_zfbl_mz", new object[] { p_sbjbm, p_yyxmbm, p_rq, bcxm });
            string memo = "";
            //bl = GetReturnString("zfbl");
            string zfbl = "100";
            if (iRe == "0" || iRe == "")
            {
                zfbl = "0";
            }
            else
            {
                //a = iRe.Substring(0, iRe.IndexOf("#"));
                var arr = iRe.Split('/');
                if (arr.Length > 1)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i].Trim().Length == 0)
                        {
                            continue;
                        }
                        var tmpArr = arr[i].Split('#');
                        if (Convert.ToDecimal(tmpArr[0]) < Convert.ToDecimal(zfbl))
                        {
                            zfbl = tmpArr[0];
                            memo = tmpArr[1];
                            if (memo.Length > 25)
                            {
                                memo = memo.Substring(0, 25);
                            }
                        }
                    }
                }
                else
                {
                    var tmpArr = arr[0].Split('#');
                    zfbl = tmpArr[0];
                    if (tmpArr.Length > 1)
                    {
                        memo = tmpArr[1];
                    }

                }
            }

            if (memo.Length >= 20)
            {
                memo = memo.Substring(0, 20);
            }
            string strDelete = @"DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID ='" + jmtc_patid + "' AND HOSPITAL_ID='" + hosid + "' AND NETWORK_ITEM_CODE='" + p_yyxmbm + @"'";
            SqlHelper.ExecSqlReDs(strDelete.ToString());

            string strInsert = @" INSERT INTO COMM.COMM.NETWORKING_ITEM_VS_HIS	
            ( NETWORKING_PAT_CLASS_ID ,
            ITEM_PROP ,
          HIS_ITEM_CODE ,
          HIS_ITEM_NAME ,
          NETWORK_ITEM_CODE ,
          NETWORK_ITEM_NAME ,
          SELF_BURDEN_RATIO ,
          MEMO ,
          START_TIME ,
          STOP_TIME ,
          TYPE_MEMO ,
          NETWORK_ITEM_PROP ,
          NETWORK_ITEM_CHARGE_CLASS ,
          HOSPITAL_ID ,
          NETWORK_ITEM_PRICE ,
          FLAG_DISABLED ,
          NETWORK_ITEM_FLAG_UP
        )
SELECT '" + jmtc_patid + @"' ,
          ITEM_PROP ,
          HIS_ITEM_CODE ,
          HIS_ITEM_NAME ,
          NETWORK_ITEM_CODE ,
          NETWORK_ITEM_NAME ,
          '" + zfbl + @"',
          '" + memo + @"',
          START_TIME ,
          STOP_TIME ,
          TYPE_MEMO ,
          NETWORK_ITEM_PROP ,
          NETWORK_ITEM_CHARGE_CLASS ,
          '" + hosid + @"' ,
          NETWORK_ITEM_PRICE ,
          FLAG_DISABLED ,
          NETWORK_ITEM_FLAG_UP
          
          FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID=" + ConfigurationManager.AppSettings["PTMZ_PATID"].ToString() + @" AND HOSPITAL_ID='" + hosid + @"' AND 
          NETWORK_ITEM_CODE='" + p_yyxmbm + @"'";
            SqlHelper.ExecSqlReDs(strInsert.ToString());

        }

        #endregion

        #region 自付比例下载 职工
        public void Down_Yyxm_ZFBL(string p_sbjbm, string p_yyxmbm, string p_rq, string bcxm)
        {

            string iRe = seiproxy.ExeFuncReStr("get_zfbl_mz", new object[] { p_sbjbm, p_yyxmbm, p_rq, bcxm });
            // string iRe = seiproxy.ExeFuncReStr("get_zfbl", new object[] { p_sbjbm, p_yyxmbm, p_rq, bcxm });
            //string iRe = seiproxy.ExeFuncReStr("get_zfbl", new object[] { p_sbjbm, p_yyxmbm, p_rq });

            string memo = "";
            //bl = GetReturnString("zfbl");
            string zfbl = "100";
            if (iRe == "0" || iRe == "")
            {
                zfbl = "0";
            }
            else
            {
                if (iRe.Contains("型糖尿病患者"))
                {
                    int n = 3;
                }
                //a = iRe.Substring(0, iRe.IndexOf("#"));
                var arr = iRe.Split('/');
                if (arr.Length > 1)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i].Trim().Length == 0)
                        {
                            continue;
                        }
                        var tmpArr = arr[i].Split('#');

                        decimal jtmp0 = 0;
                        if (decimal.TryParse(tmpArr[0], out jtmp0))
                        {

                        }
                        else
                        {
                            continue;
                        }
                        if (Convert.ToDecimal(tmpArr[0]) < Convert.ToDecimal(zfbl))
                        {
                            zfbl = tmpArr[0];
                            memo = tmpArr[1];
                            if (memo.Length > 25)
                            {
                                memo = memo.Substring(0, 25);

                            }
                        }
                    }
                }
                else
                {
                    var tmpArr = arr[0].Split('#');
                    zfbl = tmpArr[0];
                    if (tmpArr.Length > 1)
                    {
                        memo = tmpArr[1];
                    }

                }

            }
            if (memo.Length >= 20)
            {
                memo = memo.Substring(0, 20);
            }

            #region 职工统筹
            string strDelete = @"DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID ='" + zgtc_ID + "' AND HOSPITAL_ID='" + MainForm.HOSPITAL_ID + "' AND NETWORK_ITEM_CODE='" + p_yyxmbm + @"'";
            SqlHelper.ExecSqlReDs(strDelete.ToString());

            string strInsert = @" INSERT INTO COMM.COMM.NETWORKING_ITEM_VS_HIS	
            ( NETWORKING_PAT_CLASS_ID ,
            ITEM_PROP ,
          HIS_ITEM_CODE ,
          HIS_ITEM_NAME ,
          NETWORK_ITEM_CODE ,
          NETWORK_ITEM_NAME ,
          SELF_BURDEN_RATIO ,
          MEMO ,
          START_TIME ,
          STOP_TIME ,
          TYPE_MEMO ,
          NETWORK_ITEM_PROP ,
          NETWORK_ITEM_CHARGE_CLASS ,
          HOSPITAL_ID ,
          NETWORK_ITEM_PRICE ,
          FLAG_DISABLED ,
          NETWORK_ITEM_FLAG_UP
        )
SELECT   " + zgtc_ID + @"  ,
          ITEM_PROP ,
          HIS_ITEM_CODE ,
          HIS_ITEM_NAME ,
          NETWORK_ITEM_CODE ,
          NETWORK_ITEM_NAME ,
          '" + zfbl + @"',
          '" + memo + @"',
          START_TIME ,
          STOP_TIME ,
          TYPE_MEMO ,
          NETWORK_ITEM_PROP ,
          NETWORK_ITEM_CHARGE_CLASS ,
          '" + MainForm.HOSPITAL_ID + @"' ,
          NETWORK_ITEM_PRICE ,
          FLAG_DISABLED ,
          NETWORK_ITEM_FLAG_UP
          
          FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID=" + ptmz_ID + "  AND HOSPITAL_ID='" + MainForm.HOSPITAL_ID + @"'  AND 
          NETWORK_ITEM_CODE='" + p_yyxmbm + @"'";
            SqlHelper.ExecSqlReDs(strInsert.ToString()); 
            #endregion

            //MessageBox.Show("更新职工统筹自负比例成功！");
        }

        #endregion





        #region 下载核心目录和自付比例服务
        /// <summary>
        /// 下载核心目录和自付比例服务
        /// </summary>
        /// <param name="sbjgbh">*社保机构编号</param>
        /// <param name="filename">*目录文件的带路径名(目录文件的带路径名,比如‘C:\yyxm.txt’,为‘’，则不产生)</param>
        /// <param name="filename2">*自负比例文件的带路径名(自负比例的带路径名,比如‘C:\zfbl.txt’,为‘’，则不产生)</param>
        /// <param name="filetype">*文件类型(0:表示excel文件,1:表示txt文件,2:表示csv文件，7:表示dbf2文件，8:表示dbf3文件)</param>
        /// <param name="has_head">是否包含表头(1:表示包含表头0:表示不包含表头（默认值为1）)</param>
        public void Down_Ml(string sbjgbh, string filename, string filename2, long filetype, long has_head)
        {
            decimal iRe = seiproxy.ExeFuncReInt("down_ml", new object[] { sbjgbh, filename, filename2, filetype, true });
            if (iRe != 0)
            {
                throw new Exception("下载医院项目出错：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion
        #region 增加医院项目服务
        /// <summary>
        /// 增加医院项目服务
        /// </summary>
        /// <param name="dt"></param>
        string yyxmbm = "";
        string yyxmmc = "";
        string tishi = "";
        public MSSQLHelpers SQLHelper = new MSSQLHelpers(ConfigurationManager.ConnectionStrings["connStr"].ToString());
        public void add_yyxm_info_all(DataTable dt)
        {
            int iRe = 0;
            int errNum = 0;
            int upNum = 0;
            StringBuilder codeStr = new StringBuilder();
            string str = "";

            foreach (DataRow row in dt.Rows)
            {
                string p_yyxmbm = row["yyxmbm"].ToString(); //医院项目编码
                string p_yyxmmc = row["yyxmmc"].ToString(); //医院项目名称
                str += "医院项目编码:" + p_yyxmbm + ",医院项目名称:" + p_yyxmmc + "\n";
                string p_ypbz = row["ypbz"].ToString(); //药品标志（‘1’药品，‘0’诊疗，‘2’一次性材料）
                string type_p = row["yltype"].ToString();
                if (type_p.Equals("3"))
                {
                    p_ypbz = "2";
                }
                if (string.IsNullOrEmpty(type_p))
                {
                    p_ypbz = "0";
                }
                if (type_p.Equals("1"))
                {
                    p_ypbz = "1";
                }


                decimal p_dj = Convert.ToDecimal(row["dj"].ToString()); //最小包装规格的单价
                string p_zxgg = row["zxgg"].ToString(); //最小规格
                decimal p_bhsl = 1;//大包装包含小规格的数量
                string p_syz = row["syz"].ToString(); //适应症
                string p_jj = row["jj"].ToString();// 禁忌
                string p_scqy = row["scqy"].ToString(); //生产企业
                string p_zdgg = row["zdgg"].ToString();//大包装规格 
                string p_spm = row["spm"].ToString();//商品名
                string p_dw = row["dw"].ToString(); //单位
                string p_gmpbz = row["gmpbz"].ToString(); //是否GMP（‘1’ GMP，‘0’非GMP）
                string p_cfybz = row["cfybz"].ToString();//是否处方药（‘1’ 处方药，‘0’非处方药）

                Common.WriteLog("地维上传日志传输", "医院项目编码" + p_yyxmbm + "医院项目名称" + p_yyxmmc + "药品标志" + p_ypbz + "对照类别" + type_p, Common.Now());

                iRe = seiproxy.ExeFuncReInt("add_yyxm", new object[] { p_yyxmbm, p_yyxmmc, p_ypbz, p_dj, p_zxgg, p_bhsl, p_syz, p_jj, p_scqy, p_zdgg, p_spm, p_dw, p_gmpbz, p_cfybz, "", "", DateTime.Now });
                Common.WriteLog("日志传输结果", iRe.ToString(), Common.Now());
                if (iRe != 0)
                {
                    errNum++;

                    yyxmbm = row["yyxmbm"].ToString();
                    yyxmmc = row["yyxmmc"].ToString();
                    tishi = seiproxy.ExeFuncReStr("get_errtext", null);
                    Common.WriteLog("已进入错误日志插入", "医院项目编码：" + yyxmbm + "医院项目名称：" + yyxmmc + tishi, Common.Now());

                    SQLHelper.ExecSqlReDs("DELETE  from COMM.COMM.NETWORKING_ITEM_VS_HIS where HOSPITAL_ID = '" + MainForm.HOSPITAL_ID + "' and  HIS_ITEM_CODE='" + yyxmbm + "'and HIS_ITEM_NAME='" + yyxmmc + "'");
                    StringBuilder sqlStr = new StringBuilder();
                    if (type_p == "1")
                    {
                        sqlStr.Append(" INSERT INTO REPORT.dbo.upfail ");
                        sqlStr.Append(" (code ,name,memo) values");
                        sqlStr.Append(" ('" + yyxmbm + "' ,'" + yyxmmc + "','" + tishi + "')");
                    }
                    if (string.IsNullOrEmpty(type_p))
                    {
                        sqlStr.Append(" INSERT INTO REPORT.dbo.upfailzl ");
                        sqlStr.Append(" (code ,name,memo) values");
                        sqlStr.Append(" ('" + yyxmbm + "' ,'" + yyxmmc + "','" + tishi + "')");
                    }
                    if (type_p == "3")
                    {
                        sqlStr.Append(" INSERT INTO REPORT.dbo.upfailhc ");
                        sqlStr.Append(" (code ,name,memo) values");
                        sqlStr.Append(" ('" + yyxmbm + "' ,'" + yyxmmc + "','" + tishi + "')");
                    }
                    SQLHelper.ExecSqlReDs(sqlStr.ToString());
                    Thread fThread2 = new Thread(new ThreadStart(SleepT2));
                    fThread2.IsBackground = true;
                    fThread2.Start();

                    //throw new Exception("上传费用目录出错 项目编码为:" + row["yyxmbm"].ToString() + "  项目名称为;" + row["yyxmmc"].ToString() + ",医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
                }
                upNum++;
            }

            Thread fThread = new Thread(new ThreadStart(SleepT));
            fThread.IsBackground = true;
            fThread.Start();
            MessageBox.Show("上传完成！上传条数：" + upNum + "条,失败条数：" + errNum);
            MessageBox.Show(str);
        }

        public void SleepT2()
        {
            if (MessageBox.Show("上传费用目录出错 项目编码为:" + yyxmbm + "  项目名称为;" + yyxmmc + ",医保返回提示：" + tishi, "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                yyxmbm = "";
                yyxmmc = "";
                tishi = "";

            }
            else
            {
                yyxmbm = "";
                yyxmmc = "";
                tishi = "";
                throw new Exception("上传停止");
            }
        }
        public void SleepT()
        {
            MessageBox.Show("上传成功！");
        }

        #endregion
        #region 获取免费药品自负比例

        public void Down_Yyxm_ZFBL_mfyy(string p_sbjbm, string p_yyxmbm, string p_rq)
        {

            string iRe = seiproxy.ExeFuncReStr("get_zfbl", new object[] { p_sbjbm, p_yyxmbm, p_rq });
            string memo = "";
            //bl = GetReturnString("zfbl");
            string zfbl = "100";
            if (iRe == "0" || iRe == "")
            {
                zfbl = "0";
            }
            else
            {
                //a = iRe.Substring(0, iRe.IndexOf("#"));
                var arr = iRe.Split('/');
                if (arr.Length > 1)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i].Trim().Length == 0)
                        {
                            continue;
                        }
                        var tmpArr = arr[i].Split('#');
                        decimal jtmp0 = 0;
                        if (decimal.TryParse(tmpArr[0], out jtmp0))
                        {

                        }
                        else
                        {
                            continue;
                        }
                        if (Convert.ToDecimal(tmpArr[0]) < Convert.ToDecimal(zfbl))
                        {
                            zfbl = tmpArr[0];
                            memo = tmpArr[1];
                            if (memo.Length > 25)
                            {
                                memo = memo.Substring(0, 25);
                            }
                        }
                    }
                }
                else
                {
                    var tmpArr = arr[0].Split('#');
                    zfbl = tmpArr[0];
                    if (tmpArr.Length > 1)
                    {
                        memo = tmpArr[1];
                    }

                }
            }

            if (memo.Length >= 20)
            {
                memo = memo.Substring(0, 20);
            }

            string strDelete = @"DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID ='" + mfyy_ID + "' AND HOSPITAL_ID='" + MainForm.HOSPITAL_ID + "' AND NETWORK_ITEM_CODE='" + p_yyxmbm + @"'";
            SqlHelper.ExecSqlReDs(strDelete.ToString());

            string strInsert = @" INSERT INTO COMM.COMM.NETWORKING_ITEM_VS_HIS	
            ( NETWORKING_PAT_CLASS_ID ,
            ITEM_PROP ,
          HIS_ITEM_CODE ,
          HIS_ITEM_NAME ,
          NETWORK_ITEM_CODE ,
          NETWORK_ITEM_NAME ,
          SELF_BURDEN_RATIO ,
          MEMO ,
          START_TIME ,
          STOP_TIME ,
          TYPE_MEMO ,
          NETWORK_ITEM_PROP ,
          NETWORK_ITEM_CHARGE_CLASS ,
          HOSPITAL_ID ,
          NETWORK_ITEM_PRICE ,
          FLAG_DISABLED ,
          NETWORK_ITEM_FLAG_UP
        )
SELECT " + mfyy_ID + @" ,
          ITEM_PROP ,
          HIS_ITEM_CODE ,
          HIS_ITEM_NAME ,
          NETWORK_ITEM_CODE ,
          NETWORK_ITEM_NAME ,
          '" + zfbl + @"',
          '" + memo + @"',
          START_TIME ,
          STOP_TIME ,
          TYPE_MEMO ,
          NETWORK_ITEM_PROP ,
          NETWORK_ITEM_CHARGE_CLASS ,
          '" + MainForm.HOSPITAL_ID + @"' ,
          NETWORK_ITEM_PRICE ,
          FLAG_DISABLED ,
          NETWORK_ITEM_FLAG_UP
          
          FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID=" + ptmz_ID + "  AND HOSPITAL_ID='" + MainForm.HOSPITAL_ID + @"' AND 
          NETWORK_ITEM_CODE='" + p_yyxmbm + @"'";
            SqlHelper.ExecSqlReDs(strInsert.ToString());


        }

        #endregion
    }
}
