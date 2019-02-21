using System;
using System.Collections.Generic;
using System.Text;
using PayAPIUtilities.Log;
using PayAPIUtilities.Config;
using System.Data;

using PayAPIInterface.Model.Out;
using PayAPIInterface.Model.In;


namespace PayAPIInterfaceHandle.Dareway.ShanDong
{
    public class DarewayHandle
    {
        public MSSQLHelper sqlHelperHis = new MSSQLHelper("Data Source=172.22.29.6;Initial Catalog=COMM;Persist Security Info=True;User ID=power;Password=m@ssunsoft009");
        //
        public static RefCOM seiproxy = new RefCOM("seiproxy");
        /// <summary>
        /// 构造函数
        /// </summary>
        public DarewayHandle()
        {
            ReleaseComObj();
            DareWayInit();
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
                    seiproxy.DisposeSelf();
                    seiproxy = new RefCOM("seiproxy");
                    //MessageBox.Show("2");
                }
            }
            catch (Exception desEx)
            {
                try
                {
                    LogManager.Error("DisposeSelf::" + desEx.Message + "  " + desEx.StackTrace + " " + desEx.Source);
                }
                catch (Exception desEx1)
                {
                    LogManager.Error("DisposeSelf_err::" + desEx1.Message + "  ");
                }
            }
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

        #region 地维登陆
        /// <summary>
        /// 地维登陆
        /// </summary>
        public void DareWayInit()
        {
            string hosNo = PayAPIConfig.InstitutionDict[2].InstitutionCode;
            string usercode = PayAPIConfig.InstitutionDict[2].InstitutionUserCode;
            string password = PayAPIConfig.InstitutionDict[2].InstitutionPassword;


            string pStr = "gzrybh#" + usercode + "|yybm#" + hosNo + "|passwd#" + password + "|syzhlx#0|";
            int iRe = seiproxy.ExeFuncReInt("initialize", new object[] { pStr });
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
        /// <param name="p_yltclb">医疗统筹类别 0 取卡片基本信息，1 住院，4 门诊大病，6 普通门诊 </param>
        /// <param name="p_bcxm">补充项目，用于以后功能扩展，目前一律传递’’</param>
        public Dictionary<string, string> ReadCard(string p_yltclb, string p_bcxm)
        {
            Dictionary<string, string> patinfo = new Dictionary<string, string>();
            int iRe = 0;
            try
            {
                iRe = seiproxy.ExeFuncReInt("readcard", new object[] { p_yltclb, p_bcxm });
            }
            catch (Exception ex)
            {
                LogManager.Error(ex.Message + "  " + ex.InnerException + "  " + ex.Source);
                throw;
            }

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
            patinfo.Add("mzdbbz", "");                              //疾病编码
            patinfo.Add("sbjbm", GetReturnString("sbjbm"));         //社保局编码,山东省直为379902
            patinfo.Add("zhzybz", GetReturnString("zhzybz"));       //有无15(医保参数制)天内的住院记录1:有 ,0 :无
            patinfo.Add("zhzysm", GetReturnString("zhzysm"));       //5(医保参数控制)天内的住院记录说明
            patinfo.Add("zcyymc", GetReturnString("zcyymc"));       //转出医院名称(如果zcyymc不为’’,则表示本次住院时候市内转院来的)
            patinfo.Add("ye", GetReturnDec("ye").ToString());       //账户余额
            patinfo.Add("zccyrq", GetReturnString("zccyrq"));       //转出医院出院日期
            patinfo.Add("sbjglx", GetReturnString("sbjglx"));  //机构类型(A：职工，B：居民)
            patinfo.Add("csrq", GetReturnString("csrq"));       //出生日期
            patinfo.Add("cbdsbh", GetReturnString("cbdsbh"));       //参保地市编号
            //patinfo.Add("cbdsmc", GetReturnString("cbdsmc"));       //参保地市名称
            return patinfo;
        }
        #endregion

        #region 取得人员相关信息
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
            int iRe = seiproxy.ExeFuncReInt("query_basic_info", new object[] { p_grbh, p_xm, p_yltclb, p_sbjbm });
            if (iRe != 0)
            {
                throw new Exception("读卡失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }
            patinfo.Add("ylzbh", p_grbh);                               //医保卡号
            patinfo.Add("xm", GetReturnString("xm"));               //姓名
            patinfo.Add("xb", GetReturnString("xb"));               //性别,1:男,2:女,9:不确定
            patinfo.Add("shbzhm", GetReturnString("shbzhm"));       //社会保障号码
            patinfo.Add("zfbz", GetReturnString("zfbz"));           //灰白名单标志:0 代表灰名单,1 白名单
            patinfo.Add("zfsm", GetReturnString("zfsm"));           //灰名单原因(如果是白名单该值为空)
            patinfo.Add("dwmc", GetReturnString("dwmc"));           //单位名称
            patinfo.Add("ylrylb", GetReturnString("ylrylb"));       //人员类别(汉字)
            patinfo.Add("ydbz", GetReturnString("ydbz"));           //是否为异地人员 (1:是,0: 否)
            patinfo.Add("mzdbjbs", GetReturnString("mzdbjbs"));     //疾病编码
            patinfo.Add("sbjbm", p_sbjbm);                          //社保局编码,山东省直为379902
            patinfo.Add("zhzybz", GetReturnString("zhzybz"));       //有无15(医保参数制)天内的住院记录1:有 ,0 :无
            patinfo.Add("zhzysm", GetReturnString("zhzysm"));       //5(医保参数控制)天内的住院记录说明
            patinfo.Add("zcyymc", GetReturnString("zcyymc"));       //转出医院名称(如果zcyymc不为’’,则表示本次住院时候市内转院来的)
            patinfo.Add("ye", "0");                                 //账户余额
            patinfo.Add("sbjglx", GetReturnString("sbjglx"));       //转出医院出院日期
            patinfo.Add("cbdsbh", GetReturnString("cbdsbh"));       //参保地市编号
            patinfo.Add("cbjgmc", GetReturnString("cbjgmc"));       //参保地市名称
            return patinfo;
        }
        #endregion

        #region 门诊初始化
        /// <summary>
        /// 门诊初始化
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
        public void InitMZ(string p_sbjbm, string p_yltclb, string p_grbh, string p_xm, string p_xb, string p_zylsh, string p_fyrq, string p_ysbm, string p_jbbm, string p_syzhlx, string p_ylzbh, string p_xzbz, string p_bcxm)
        {
            int iRe = seiproxy.ExeFuncReInt("init_mzmg", new object[] { p_sbjbm, p_yltclb, p_grbh, p_xm, p_xb, p_zylsh, p_fyrq, p_ysbm, p_jbbm, p_syzhlx, p_ylzbh, p_xzbz, p_bcxm });
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
        public void SaveOutItems(List<OutNetworkUpDetail> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                seiproxy.ExeFuncReObj("new_mzmg_item", null);
                seiproxy.ExeFuncReObj("set_mzmg_item_string", new object[] { "yyxmbm", items[i].NetworkItemCode }); //医院项目编码
                seiproxy.ExeFuncReObj("set_mzmg_item_dec", new object[] { "dj", items[i].Price });                    //最小包装的单价
                seiproxy.ExeFuncReObj("set_mzmg_item_dec", new object[] { "sl", items[i].Quantity });                 //大包装数量
                seiproxy.ExeFuncReObj("set_mzmg_item_dec", new object[] { "bzsl", 1 });                   //  大包装的小包装数量
                seiproxy.ExeFuncReObj("set_mzmg_item_dec", new object[] { "zje", items[i].Amount });             //总金额（zje=dj*sl*bzsl）
                seiproxy.ExeFuncReObj("set_mzmg_item_string", new object[] { "ksbm", items[i].DeptCode });           //科室编码
                seiproxy.ExeFuncReObj("set_mzmg_item_string", new object[] { "gg", items[i].Spec });                  //规格
                seiproxy.ExeFuncReObj("set_mzmg_item_string", new object[] { "zxksbm", "001" });            //*执行科室编码
                seiproxy.ExeFuncReObj("set_mzmg_item_string", new object[] { "kdksbm", "001" });            //*开单科室编码
                //seiproxy.ExeFuncReObj("set_mzmg_item_dec", new object[] { "jyzfbl", 0});    //*自付比例 items[i].SelfBurdenRatio
                seiproxy.ExeFuncReObj("set_mzmg_item_dec", new object[] { "jyzfbl", GetSelfBurdenRatio("27",items[i].ChargeCode) });    
                seiproxy.ExeFuncReObj("set_mzmg_item_string", new object[] { "yyxmmc", items[i].ChargeName });       //医院项目名称
                int iRe = seiproxy.ExeFuncReInt("save_mzmg_item", null);
                if (iRe != 0)
                {
                    throw new Exception("保存费用明细出错 项目编码为:" + items[i].ChargeCode + "  项目名称为;" + items[i].ChargeName + ",医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
                }
                items[i].UploadBackSerial =  "";
            }
        }
        #endregion

        #region 门诊结算
        /// <summary>
        /// 门诊结算
        /// </summary>
        /// <param name="isPrint">是否打印</param>
        /// <returns></returns>
        public Dictionary<string, string> SettleMZ(bool isPrint = true)
        {
            Dictionary<string, string> settleInfo = new Dictionary<string, string>();
            int iRe = seiproxy.ExeFuncReInt("settle_mzmg", null);

            if (iRe != 0)
            {
                throw new Exception("门诊结算失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }

            settleInfo.Add("brjsh", GetReturnString("brjsh"));                         //医保系统的病人结算号
            settleInfo.Add("brfdje", GetReturnDec("brfdje").ToString());               //病人负担金额（包含账户支付）
            settleInfo.Add("ybfdje", GetReturnDec("ybfdje").ToString());               //医保负担金额
            settleInfo.Add("ylbzje", GetReturnDec("ylbzje").ToString());               //医疗补助金额(优抚对象补助)
            settleInfo.Add("yyfdje", GetReturnDec("yyfdje").ToString());               //医院负担金额
            settleInfo.Add("grzhzf", GetReturnDec("grzhzf").ToString());               //个人账户支付

            settleInfo.Add("rylb", GetReturnString("rylb").ToString());                //人员类别
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

            //打印结算单据
            if (isPrint)
            {
                string brjsh = GetReturnString("brjsh");
                try
                {
                    seiproxy.ExeFuncReInt("printdj", new object[] { brjsh, "FP" });
                }
                catch (System.Exception ex)
                {
                    LogManager.Error("地维打印单据异常\n"+ex.Message.ToString());
                    ReleaseComObj();
                    DareWayInit();
                }

            }
            return settleInfo;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void PrintDJ(string brjsh, string FPLX)
        {
            try
            {
                seiproxy.ExeFuncReInt("printdj", new object[] { brjsh, FPLX });
            }
            catch (System.Exception ex)
            {
                LogManager.Error("地维打印单据异常\n"+ex.Message.ToString());
                ReleaseComObj();
                DareWayInit();
            }
        }

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

            int iRe = seiproxy.ExeFuncReInt("save_zydj",
                            new object[] {  p_blh,   p_shbzhm,   p_ylzbh,   p_xm,   p_xb,
                                            p_yltclb,   p_sbjbm,   p_syzhlx,   p_ksbm,   p_zyrq,   p_qzys,
                                            p_mzks, p_zyfs, p_xzbz, p_bcxm});
            if (iRe != 0)
            {
                throw new Exception("个人账户退费服务失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
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

        public void SaveInItems(List<InNetworkUpDetail> items, string vysbm, string zyh)
        {
            int iRe = 0;

            string dateCur = Convert.ToDateTime(items[0].CreateTime).ToString("yyyy-MM-dd");

            for (int i = 0; i < items.Count; i++)
            {
                if (dateCur != Convert.ToDateTime(items[i].CreateTime).ToString("yyyy-MM-dd"))
                {
                    iRe = seiproxy.ExeFuncReInt("save_zy_script", new object[] { vysbm, dateCur });
                    if (iRe != 0)
                    {
                        throw new Exception("保存凭单出错,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
                    }
                    dateCur = Convert.ToDateTime(items[i].CreateTime).ToString("yyyy-MM-dd");

                    InitZY(zyh);
                }

                seiproxy.ExeFuncReObj("new_zy_item", null);
                seiproxy.ExeFuncReObj("set_zy_item_string", new object[] { "yyxmbm", items[i].NetworkItemCode }); //医院项目编码
                seiproxy.ExeFuncReObj("set_zy_item_dec", new object[] { "dj", items[i].Price });                    //最小包装的单价
                seiproxy.ExeFuncReObj("set_zy_item_dec", new object[] { "sl", items[i].Quantity });                 //大包装数量
                seiproxy.ExeFuncReObj("set_zy_item_dec", new object[] { "bzsl", "1" });                   //  大包装的小包装数量
                seiproxy.ExeFuncReObj("set_zy_item_dec", new object[] { "zje", items[i].Amount });                  //总金额（zje=dj*sl*bzsl）
                seiproxy.ExeFuncReObj("set_zy_item_string", new object[] { "ksbm", items[i].DeptCode });           //科室编码
                seiproxy.ExeFuncReObj("set_zy_item_string", new object[] { "gg", items[i].Spec });                  //规格
                seiproxy.ExeFuncReObj("set_zy_item_string", new object[] { "zxksbm", "001" });            //*执行科室编码
                seiproxy.ExeFuncReObj("set_zy_item_string", new object[] { "kdksbm", "001" });            //*开单科室编码
                //seiproxy.ExeFuncReObj("set_zy_item_dec", new object[] { "jyzfbl",0 });    //*自付比例 items[i].SelfBurdenRatio 
                seiproxy.ExeFuncReObj("set_zy_item_dec", new object[] { "jyzfbl", GetSelfBurdenRatio("27", items[i].ChargeCode) }); 
                iRe = seiproxy.ExeFuncReInt("save_zy_item", null);
                if (iRe != 0)
                {
                    throw new Exception("保存费用明细出错 项目编码为:" + items[i].ChargeCode.ToString() + "  项目名称为;" + items[i].ChargeName.ToString() + ",医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
                }
            }

            iRe = seiproxy.ExeFuncReInt("save_zy_script", new object[] { vysbm, dateCur });
            if (iRe != 0)
            {
                throw new Exception("保存凭单出错,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
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
            int iRe = seiproxy.ExeFuncReInt("destroy_all_fypd", null);
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
        public Dictionary<string, string> SettleZY()
        {
            Dictionary<string, string> settleInfo = new Dictionary<string, string>();
            int iRe = seiproxy.ExeFuncReInt("settle_zy", null);

            if (iRe != 0)
            {
                throw new Exception("出院结算失败,医保返回提示：" + seiproxy.ExeFuncReStr("get_errtext", null));
            }

            settleInfo.Add("brjsh", GetReturnString("brjsh"));                               //医保系统的病人结算号
            settleInfo.Add("brfdje", GetReturnDec("brfdje").ToString());                     //病人负担金额（包含账户支付）
            settleInfo.Add("ybfdje", GetReturnDec("ybfdje").ToString());                     //医保负担金额
            settleInfo.Add("ylbzje", GetReturnDec("ylbzje").ToString());                     //医疗补助金额(优抚对象补助)
            settleInfo.Add("grzhzf", GetReturnDec("grzhzf").ToString());                     //个人账户支付
            settleInfo.Add("yyfdje", GetReturnDec("yyfdje").ToString());                     //医院负担金额

            settleInfo.Add("fph", GetReturnString("fph").ToString());                        //发票号
            settleInfo.Add("brjsrq", GetReturnDate("brjsrq").ToString());                  //病人结算日期

            //try
            //{
            //    settleInfo.Add("cgxjje", GetReturnDec("cgxjje").ToString());                     //超过限价金额
            //}
            //catch(Exception ex)
            //{
            //    LogManager.Error(ex.Message);
                settleInfo.Add("cgxjje", "0"); 
            //}

            //打印结算单据
            string brjsh = GetReturnString("brjsh");
            try
            {
                seiproxy.ExeFuncReInt("printdj", new object[] { brjsh, "FP" });
                seiproxy.ExeFuncReInt("printdj", new object[] { brjsh, "JSD" });
            }
            catch
            {
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

        /// <summary>
        /// 获取自付比例
        /// </summary>
        /// <param name="docCode"></param>
        /// <returns></returns>
        public string GetSelfBurdenRatio(string Networking_pat_class_id, string HisItmeCode)
        {

            string zfbl = "";

            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT MIN(SELF_BURDEN_RATIO) FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE  NETWORKING_PAT_CLASS_ID='" + Networking_pat_class_id + "' AND HIS_ITEM_CODE='" + HisItmeCode + "'");

            DataTable dt = sqlHelperHis.ExecSqlReDs(strSql.ToString()).Tables[0];
            if (dt.Rows.Count > 0)
            {
                zfbl = dt.Rows[0][0].ToString();
            }
            else
            {
                zfbl = "0";
            }
            return zfbl;
        }
    }
}
