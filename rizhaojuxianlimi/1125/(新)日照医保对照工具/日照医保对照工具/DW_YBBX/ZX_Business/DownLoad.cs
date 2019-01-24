using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Data.OleDb;
using System.Threading;

namespace DW_YBBX.ZX_Business
{
    public partial class DownLoad : Form
    {
        public MSSQLHelpers SQLHelper = new MSSQLHelpers(ConfigurationManager.ConnectionStrings["connStr"].ToString());
        private DW_Handle handelModel;

        private RZYBHandle rzybHandle;
        //是否已经下载
        private bool isDown = false;
        //药品类
        private YBMLModel DrugModel = new YBMLModel();
        //诊疗类
        private YBMLModel DiagModel = new YBMLModel();
        //服务设施类
        private YBMLModel FwssModel = new YBMLModel();
        //中心疾病类
        private YBJBModel YbjbModel = new YBJBModel();

        //中心对照好下载类
        private YYXMModel DzXzModel = new YYXMModel();

        //查询字典类
        private YBJBModel ybjbzd = new YBJBModel();
        //项目对照信息类
        private List<YYXM> yyxm_ds;

        //数据字典信息类
        private List<Cxzd> cxzd_ds;
        //待遇封锁
        private List<dyfs> fsxx_ds;
        //靶向药查询
        private List<Bxysp> spxx_ds;
        //社保机构编号
        public string sbjgbh = string.Empty;
        //注册码
        public string zcm = string.Empty;
        //医院编号
        public string yybm = string.Empty;
        //usercode
        public string usercode = string.Empty;
        //hosip
        public string hospitalId = string.Empty;
        public DownLoad()
        {
            InitializeComponent();
            sbjgbh = "37110101";
            zcm = "211231I-970453-771964-4535";
            yybm = "51080096";
            usercode = MainForm.UserCode;
            hospitalId = MainForm.HOSPITAL_ID;
            //this.ControlBox = true;  //设置可见得关闭按钮

        }
        #region 下载医师
        /// <summary>
        /// 下载医师
        /// </summary>
        /// <param name="sender"></para>
        /// <param name="e"></param>
        private void btn_DownYs_Click(object sender, EventArgs e)
        {
            handelModel = new DW_Handle();
            string sbjgbh = MainForm.sbjgbh;
            string yybm = MainForm.yybm;
            string filename = Environment.CurrentDirectory + @"\YBDLOAD\医院医师.xls";
            long filetype = 0;//0:表示excel文件,1:表示txt文件,2:表示csv文件，7:表示dbf2文件，8:表示dbf3文件
            long has_head = 1;//1:表示包含表头0:表示不包含表头（默认值为1）
            handelModel.Down_Ys(sbjgbh, yybm, filename, filetype, has_head);
            MessageBox.Show("下载医师目录成功！地址：" + Environment.CurrentDirectory + @"\YBDLOAD\医院医师.xls");
        }
        #endregion

        #region 下载核心目录
        /// <summary>
        /// 下载核心目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DownYBHXML_Click(object sender, EventArgs e)
        {
            rzybHandle = new RZYBHandle();
            //string sbjgbh = MainForm.sbjgbh;
            //string zcm = MainForm.sbjgbh;
            //string yybm = MainForm.yybm;
            //string usercode = MainForm.UserCode;
            string zcm = "211231I-970453-771964-4535";
            string yybm = "51080096";

            //药品
            rzybHandle.InitResolver(sbjgbh, zcm, yybm, usercode); //初始化参数 ：社保机构编号、注册码、医院编码、usercode
            rzybHandle.AddInParas("p_filetype", "json");         //返回参数的格式 json  excel  txt
            rzybHandle.AddInParas("p_ypbz", "1");                // 1:药品   0:诊疗项目   2:服务设施
            rzybHandle.Handle("query_ml");                       // 传入方法名
            DrugModel = rzybHandle.GetResult();


            //诊疗
            rzybHandle.InitResolver(sbjgbh, zcm, yybm, usercode); //初始化参数 ：社保机构编号、注册码、医院编码、usercode
            rzybHandle.AddInParas("p_filetype", "json");          //返回参数的格式 json  excel  txt
            rzybHandle.AddInParas("p_ypbz", "0");                 // 1:药品   0:诊疗项目   2:服务设施
            rzybHandle.Handle("query_ml");                        // 传入方法名
            DiagModel = rzybHandle.GetResult();

            //一次性材料
            rzybHandle.InitResolver(sbjgbh, zcm, yybm, usercode); //初始化参数 ：社保机构编号、注册码、医院编码、usercode
            rzybHandle.AddInParas("p_filetype", "json");          //返回参数的格式 json  excel  txt
            rzybHandle.AddInParas("p_ypbz", "2");                 // 1:药品   0:诊疗项目   2:服务设施
            rzybHandle.Handle("query_ml");                        // 传入方法名
            FwssModel = rzybHandle.GetResult();

            //补充库诊疗
            //rzybHandle.InitResolver(sbjgbh, zcm, yybm, usercode); //初始化参数 ：社保机构编号、注册码、医院编码、usercode
            //rzybHandle.AddInParas("p_filetype", "json");          //返回参数的格式 json  excel  txt
            //rzybHandle.AddInParas("p_ypbz", "3");                 // 1:药品   0:诊疗项目   2:服务设施
            //rzybHandle.Handle("query_ml");                        // 传入方法名
            //FwssModel = rzybHandle.GetResult();

            isDown = true;
            MessageBox.Show("下载核心目录成功，请保存数据！");
        }
        #endregion

        #region 保存医保核心目录
        /// <summary>
        /// 保存医保核心目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveYBHXML_Click(object sender, EventArgs e)
        {
            int drugCount = 0;  //药品条数
            int ChargeCount = 0; //诊疗条数
            int Clcount = 0;    //材料条数
            int drugRate = 0;   //自付比例
            if (isDown)
            {
                #region 保存中心目录表
                StringBuilder ss = new StringBuilder();
                ss.Append(" TRUNCATE TABLE COMM.DICT.NETWORKING_CENTER_DRUG_DICT_BACK ");
                int a = SQLHelper.ExecSqlReInt(ss.ToString());
                StringBuilder ss1 = new StringBuilder();
                ss1.Append("TRUNCATE TABLE COMM.DICT.NETWORKING_CENTER_CHARGE_DICT_BACK");
                int a1 = SQLHelper.ExecSqlReInt(ss1.ToString());
                DateTime startTime = DateTime.Now; //定义一个开始时间

                using (DataTable table = new DataTable())
                {
                    #region 为数据表创建相对应的数据列
                    //为数据表创建相对应的数据列
                    table.Columns.Add("医保项目编码");
                    table.Columns.Add("医保项目名称");
                    table.Columns.Add("报销病人类别编号");
                    table.Columns.Add("英文名称");
                    table.Columns.Add("收费类别");
                    table.Columns.Add("处方药标志");
                    table.Columns.Add("目录等级");
                    table.Columns.Add("拼音助记码");
                    table.Columns.Add("五笔助记码");   // 可空
                    table.Columns.Add("药品剂量单位"); //可空

                    table.Columns.Add("最高限价");
                    table.Columns.Add("先付限价");    //可空
                    table.Columns.Add("门诊自付比例"); //可空
                    table.Columns.Add("住院自付比例"); //可空
                    table.Columns.Add("离休自付比例"); //可空
                    table.Columns.Add("工伤自负比例"); //可空
                    table.Columns.Add("生育自付比例"); //可空
                    table.Columns.Add("家庭子女学生自付比例");   //可空
                    table.Columns.Add("院内制剂标志");
                    table.Columns.Add("定点医疗机构编码");

                    table.Columns.Add("是否需要审批标志");
                    table.Columns.Add("最小医院等级");
                    table.Columns.Add("最小医师等级");
                    table.Columns.Add("自付部分进入统筹标志");
                    table.Columns.Add("自付部分进入救助金标志");
                    table.Columns.Add("是否招标药品");
                    table.Columns.Add("二次报销标志");
                    table.Columns.Add("剂型");
                    table.Columns.Add("每次用量");
                    table.Columns.Add("使用频次");

                    table.Columns.Add("用法");
                    table.Columns.Add("单位");
                    table.Columns.Add("规格");
                    table.Columns.Add("限定天数");
                    table.Columns.Add("商品名称");
                    table.Columns.Add("商品价格");
                    table.Columns.Add("商品拼音助记码");
                    table.Columns.Add("商品五笔助记码");
                    table.Columns.Add("药厂名称");  //生产企业
                    table.Columns.Add("国药准字");

                    table.Columns.Add("经办人");
                    table.Columns.Add("经办时间");
                    table.Columns.Add("起始日期");
                    table.Columns.Add("终止日期");
                    table.Columns.Add("备注");    //  等价于 返回参数说明
                    table.Columns.Add("自治药品编码");
                    table.Columns.Add("国家目录编码");
                    table.Columns.Add("参考价格");
                    table.Columns.Add("参考医院");
                    table.Columns.Add("使用范围");

                    table.Columns.Add("产地");
                    table.Columns.Add("可用标志");   //注销标志
                    table.Columns.Add("国家基本药品目录标志");
                    table.Columns.Add("国家基本药品目录基层卫生医疗机构用药标志");
                    table.Columns.Add("居民使用标志");
                    table.Columns.Add("药品标志");
                    table.Columns.Add("剂型码");
                    table.Columns.Add("单复方标志");
                    table.Columns.Add("门诊统筹是否可报");
                    table.Columns.Add("更新时间");
                    #endregion

                    #region 添加药品
                    List<YLXM> ylxm = new List<YLXM>();
                    ylxm = DrugModel.ylxm_ds;
                    //ylxm = FwssModel.ylxm_ds;
                    if (ylxm.Count > 0)
                    {

                        for (int i = 0; i < ylxm.Count; i++)
                        {
                            DataRow dr = table.NewRow(); //创建数据行
                            dr["医保项目编码"] = ylxm[i].ylxmbm;
                            dr["医保项目名称"] = ylxm[i].ylxmbzmc;
                            dr["报销病人类别编号"] = "3";
                            dr["英文名称"] = "";
                            dr["收费类别"] = ylxm[i].jsxmbh;
                            dr["处方药标志"] = "0";//ylxm[i].cfybz;//
                            dr["目录等级"] = ylxm[i].mldj;
                            dr["拼音助记码"] = ylxm[i].py;
                            dr["五笔助记码"] = "";
                            dr["药品剂量单位"] = ylxm[i].dw;

                            dr["最高限价"] = "";
                            dr["先付限价"] = "";
                            dr["门诊自付比例"] = "";
                            dr["住院自付比例"] = "";
                            dr["离休自付比例"] = "";
                            dr["工伤自负比例"] = "";
                            dr["生育自付比例"] = "";
                            dr["家庭子女学生自付比例"] = "";
                            dr["院内制剂标志"] = "0";//
                            dr["定点医疗机构编码"] = hospitalId;

                            dr["是否需要审批标志"] = "0";//
                            dr["最小医院等级"] = "";
                            dr["最小医师等级"] = "";
                            dr["自付部分进入统筹标志"] = "0";
                            dr["自付部分进入救助金标志"] = "0";
                            dr["是否招标药品"] = "0";
                            dr["二次报销标志"] = "0";
                            dr["剂型"] = "";
                            dr["每次用量"] = "";
                            dr["使用频次"] = "";

                            dr["用法"] = "";
                            dr["单位"] = ylxm[i].dw;
                            dr["规格"] = ylxm[i].gg;
                            dr["限定天数"] = "";
                            dr["商品名称"] = "";
                            dr["商品价格"] = "";
                            dr["商品拼音助记码"] = "";
                            dr["商品五笔助记码"] = "";
                            dr["药厂名称"] = ylxm[i].scqy;
                            dr["国药准字"] = "";

                            dr["经办人"] = "";
                            dr["经办时间"] = "";
                            dr["起始日期"] = ylxm[i].qsrq;
                            dr["终止日期"] = ylxm[i].zzrq;
                            dr["备注"] = "";
                            dr["自治药品编码"] = "";
                            dr["国家目录编码"] = "";
                            dr["参考价格"] = "";
                            dr["参考医院"] = "";
                            dr["使用范围"] = "";

                            dr["产地"] = "";
                            dr["可用标志"] = "0"; //ylxm[i].zxbz;
                            dr["国家基本药品目录标志"] = "0";
                            dr["国家基本药品目录基层卫生医疗机构用药标志"] = "0";
                            dr["居民使用标志"] = "0";
                            dr["药品标志"] = ylxm[i].ypbz;
                            dr["剂型码"] = ylxm[i].jxm;
                            dr["单复方标志"] = ylxm[i].dffbz;
                            dr["门诊统筹是否可报"] = ylxm[i].mztckfbx;
                            dr["更新时间"] = ylxm[i].gxsj;
                            table.Rows.Add(dr);  //将创建的数据行添加到table中
                            drugCount++; ;
                        }
                    }
                    #endregion

                    #region 添加材料
                    ylxm = new List<YLXM>();
                    ylxm = FwssModel.ylxm_ds;
                    //ylxm = FwssModel.ylxm_ds;
                    if (ylxm.Count > 0)
                    {

                        for (int i = 0; i < ylxm.Count; i++)
                        {
                            DataRow dr = table.NewRow(); //创建数据行
                            dr["医保项目编码"] = ylxm[i].ylxmbm;
                            dr["医保项目名称"] = ylxm[i].ylxmbzmc;
                            dr["报销病人类别编号"] = "3";
                            dr["英文名称"] = "";
                            dr["收费类别"] = ylxm[i].jsxmbh;
                            dr["处方药标志"] = "0";//ylxm[i].cfybz;//
                            dr["目录等级"] = ylxm[i].mldj;
                            dr["拼音助记码"] = ylxm[i].py;
                            dr["五笔助记码"] = "";
                            dr["药品剂量单位"] = ylxm[i].dw;


                            dr["最高限价"] = "";
                            dr["先付限价"] = "";
                            dr["门诊自付比例"] = "";
                            dr["住院自付比例"] = "";
                            dr["离休自付比例"] = "";
                            dr["工伤自负比例"] = "";
                            dr["生育自付比例"] = "";
                            dr["家庭子女学生自付比例"] = "";
                            dr["院内制剂标志"] = "0";//
                            dr["定点医疗机构编码"] = hospitalId;

                            dr["是否需要审批标志"] = "0";//
                            dr["最小医院等级"] = "";
                            dr["最小医师等级"] = "";
                            dr["自付部分进入统筹标志"] = "0";
                            dr["自付部分进入救助金标志"] = "0";
                            dr["是否招标药品"] = "0";
                            dr["二次报销标志"] = "0";
                            dr["剂型"] = "";
                            dr["每次用量"] = "";
                            dr["使用频次"] = "";

                            dr["用法"] = "";
                            dr["单位"] = ylxm[i].dw;
                            dr["规格"] = ylxm[i].gg;
                            dr["限定天数"] = "";
                            dr["商品名称"] = "";
                            dr["商品价格"] = "";
                            dr["商品拼音助记码"] = "";
                            dr["商品五笔助记码"] = "";
                            dr["药厂名称"] = ylxm[i].scqy;
                            dr["国药准字"] = "";

                            dr["经办人"] = "";
                            dr["经办时间"] = "";
                            dr["起始日期"] = ylxm[i].qsrq;
                            dr["终止日期"] = ylxm[i].zzrq;
                            dr["备注"] = "";
                            dr["自治药品编码"] = "";
                            dr["国家目录编码"] = "";
                            dr["参考价格"] = "";
                            dr["参考医院"] = "";
                            dr["使用范围"] = "";

                            dr["产地"] = "";
                            dr["可用标志"] = "0"; //ylxm[i].zxbz;
                            dr["国家基本药品目录标志"] = "0";
                            dr["国家基本药品目录基层卫生医疗机构用药标志"] = "0";
                            dr["居民使用标志"] = "0";


                            dr["药品标志"] = ylxm[i].ypbz;

                            dr["剂型码"] = ylxm[i].jxm;
                            if (string.IsNullOrEmpty(ylxm[i].dffbz))
                            {
                                ylxm[i].dffbz = "0";
                            }
                            dr["单复方标志"] = ylxm[i].dffbz;
                            if (string.IsNullOrEmpty(ylxm[i].mztckfbx))
                            {
                                ylxm[i].mztckfbx = "0";
                            }
                            dr["门诊统筹是否可报"] = ylxm[i].mztckfbx;
                            dr["更新时间"] = ylxm[i].gxsj;
                            table.Rows.Add(dr);  //将创建的数据行添加到table中
                            Clcount++; ;
                        }
                    }
                    #endregion

                    SqlBulkCopy bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["connStr"].ToString());
                    bulkCopy.DestinationTableName = "COMM.DICT.NETWORKING_CENTER_DRUG_DICT_BACK ";//设置数据库中对象的表名

                    #region 设置数据表table和数据库中表的列对应关系
                    //设置数据表table和数据库中表的列对应关系
                    bulkCopy.ColumnMappings.Add("医保项目编码", "NETWORK_ITEM_CODE");
                    bulkCopy.ColumnMappings.Add("医保项目名称", "NETWORK_ITEM_NAME");
                    bulkCopy.ColumnMappings.Add("报销病人类别编号", "NETWORKING_PAT_CLASS_ID");
                    bulkCopy.ColumnMappings.Add("英文名称", "DRUG_ENGLISH_NAME");
                    bulkCopy.ColumnMappings.Add("收费类别", "NETWORK_ITEM_CHARGE_CLASS");
                    bulkCopy.ColumnMappings.Add("处方药标志", "DRUG_FLAG_OTC");
                    bulkCopy.ColumnMappings.Add("目录等级", "TYPE_MEMO");
                    bulkCopy.ColumnMappings.Add("拼音助记码", "DRUG_INPUT_CODE_PY");
                    bulkCopy.ColumnMappings.Add("五笔助记码", "DRUG_INPUT_CODE_WB");
                    bulkCopy.ColumnMappings.Add("药品剂量单位", "DRUG_UNIT_DOSE");

                    bulkCopy.ColumnMappings.Add("最高限价", "DRUG_HIGHEST_PRICE");
                    bulkCopy.ColumnMappings.Add("先付限价", "DRUG_LIMIT_PRICE_FIRST");
                    bulkCopy.ColumnMappings.Add("门诊自付比例", "DRUG_MZ_SELF_BURRDEN_RATIO");
                    bulkCopy.ColumnMappings.Add("住院自付比例", "DRUG_ZY_SELF_BURRDEN_RATIO");
                    bulkCopy.ColumnMappings.Add("离休自付比例", "DRUG_LX_SELF_BURRDEN_RATIO");
                    bulkCopy.ColumnMappings.Add("工伤自负比例", "DRUG_GS_SELF_BURRDEN_RATIO");
                    bulkCopy.ColumnMappings.Add("生育自付比例", "DRUG_SY_SELF_BURRDEN_RATIO");
                    bulkCopy.ColumnMappings.Add("家庭子女学生自付比例", "DRUG_FAMLILY_SELF_BURRDEN_RATION");
                    bulkCopy.ColumnMappings.Add("院内制剂标志", "DRUG_IN_HOS_FALG");
                    bulkCopy.ColumnMappings.Add("定点医疗机构编码", "HOSPTIAL_ID");

                    bulkCopy.ColumnMappings.Add("是否需要审批标志", "DRUG_FLAG_APPROVAL");
                    bulkCopy.ColumnMappings.Add("最小医院等级", "DRUG_HOS_LOWEST_LEVEL");
                    bulkCopy.ColumnMappings.Add("最小医师等级", "DRUG_DOC_LOWEST_LEVEL");
                    bulkCopy.ColumnMappings.Add("自付部分进入统筹标志", "DRUG_FLAG_SELF_BURRDEN_IN_TC");
                    bulkCopy.ColumnMappings.Add("自付部分进入救助金标志", "DRUG_FLAG_SELF_BURRDEN_IN_JZ");
                    bulkCopy.ColumnMappings.Add("是否招标药品", "DRUG_FLAG_TENDER");
                    bulkCopy.ColumnMappings.Add("二次报销标志", "DRUG_FLAG_SEC_SETTLE");
                    bulkCopy.ColumnMappings.Add("剂型", "DRUG_DOSE");
                    bulkCopy.ColumnMappings.Add("每次用量", "DRUG_MG");
                    bulkCopy.ColumnMappings.Add("使用频次", "DRUG_USE_TIME");

                    bulkCopy.ColumnMappings.Add("用法", "DRUG_USAGE");
                    bulkCopy.ColumnMappings.Add("单位", "DRUG_UNIT");
                    bulkCopy.ColumnMappings.Add("规格", "DRUG_SPEC");
                    bulkCopy.ColumnMappings.Add("限定天数", "DRUG_LIMIT_DAY");
                    bulkCopy.ColumnMappings.Add("商品名称", "DRUG_TRADE_NAME");
                    bulkCopy.ColumnMappings.Add("商品价格", "DRUG_TRADE_PRICE");
                    bulkCopy.ColumnMappings.Add("商品拼音助记码", "DRUG_TRADE_INPUT_CODE_PY");
                    bulkCopy.ColumnMappings.Add("商品五笔助记码", "DRUG_TRADE_INPUT_CODE_WB");
                    bulkCopy.ColumnMappings.Add("药厂名称", "DRUG_COMPANY_NAME");
                    bulkCopy.ColumnMappings.Add("国药准字", "DRUG_COUNTRY_MEDICINE");

                    bulkCopy.ColumnMappings.Add("经办人", "DRUG_OPERATOR");
                    bulkCopy.ColumnMappings.Add("经办时间", "DRUG_OPERATE_TIME");
                    bulkCopy.ColumnMappings.Add("起始日期", "DRUG_START_TIME");
                    bulkCopy.ColumnMappings.Add("终止日期", "DRUG_END_TIME");
                    bulkCopy.ColumnMappings.Add("备注", "DRUG_BACKUP");
                    bulkCopy.ColumnMappings.Add("自治药品编码", "DRUG_PERSONAL_CODE");
                    bulkCopy.ColumnMappings.Add("国家目录编码", "DRUG_COUNTRY_MEUN_CODE");
                    bulkCopy.ColumnMappings.Add("参考价格", "DRUG_PRICE_REFERENCE");
                    bulkCopy.ColumnMappings.Add("参考医院", "DRUG_HOS_REFERENCE");
                    bulkCopy.ColumnMappings.Add("使用范围", "DRUG_USE_RANGE");

                    bulkCopy.ColumnMappings.Add("产地", "DRUG_OGRIN");
                    bulkCopy.ColumnMappings.Add("可用标志", "DRUG_FLAG_INVALID");
                    bulkCopy.ColumnMappings.Add("国家基本药品目录标志", "DRUG_FLAG_BASE_LIST");
                    bulkCopy.ColumnMappings.Add("国家基本药品目录基层卫生医疗机构用药标志", "DRUG_FLAG_BASE_AGENCY");
                    bulkCopy.ColumnMappings.Add("居民使用标志", "DRUG_FLAG_CITIZEN");
                    bulkCopy.ColumnMappings.Add("药品标志", "MEMO1");
                    bulkCopy.ColumnMappings.Add("剂型码", "MEMO2");
                    bulkCopy.ColumnMappings.Add("单复方标志", "MEMO3");
                    bulkCopy.ColumnMappings.Add("门诊统筹是否可报", "MEMO4");
                    bulkCopy.ColumnMappings.Add("更新时间", "MEMO5");
                    bulkCopy.WriteToServer(table);//将数据表table复制到数据库中
                    #endregion
                }

                using (DataTable table = new DataTable())
                {
                    #region 为数据表创建相对应的数据列
                    //为数据表创建相对应的数据列
                    table.Columns.Add("医保项目编码");
                    table.Columns.Add("医保项目名称");
                    table.Columns.Add("报销病人类别编号");
                    table.Columns.Add("起始日期");
                    table.Columns.Add("收费类别");
                    table.Columns.Add("目录等级");
                    table.Columns.Add("自付类别");
                    table.Columns.Add("需要审批标志");
                    table.Columns.Add("是否二次报销");
                    table.Columns.Add("自理部分进入统筹标志");

                    table.Columns.Add("自理部分进入救助金标志");
                    table.Columns.Add("特检特质标志");
                    table.Columns.Add("产地");
                    table.Columns.Add("最高价格");
                    table.Columns.Add("离休二乙最高价格");
                    table.Columns.Add("国产限价");
                    table.Columns.Add("门诊自付比例"); //可空
                    table.Columns.Add("住院自付比例"); //可空
                    table.Columns.Add("离休自付比例"); //可空
                    table.Columns.Add("工伤自付比例"); //可空

                    table.Columns.Add("生育自付比例"); //可空
                    table.Columns.Add("二乙自付比例");   //可空
                    table.Columns.Add("单位自付比例");   //可空
                    table.Columns.Add("家属子女学生自付比例");   //可空
                    table.Columns.Add("进口差价自付比例");
                    table.Columns.Add("医院绑定标志");
                    table.Columns.Add("定点医疗机构编码");
                    table.Columns.Add("生产厂家");
                    table.Columns.Add("拼音助记码");
                    table.Columns.Add("五笔助记码");   // 可空

                    table.Columns.Add("自定义编码");
                    table.Columns.Add("生产厂家1");
                    table.Columns.Add("经办人");
                    table.Columns.Add("经办时间");
                    table.Columns.Add("终止时间");
                    table.Columns.Add("备注");    //  等价于 返回参数说明
                    table.Columns.Add("限制使用范围");
                    table.Columns.Add("有效标志");   //注销标志
                    table.Columns.Add("国家基本药品目录");
                    table.Columns.Add("国家基本药品目录基层卫生医疗机构用药标志");

                    table.Columns.Add("居民使用标志");
                    table.Columns.Add("药品标志");
                    table.Columns.Add("剂型码");
                    table.Columns.Add("单复方标志");
                    table.Columns.Add("规格");
                    table.Columns.Add("单位");
                    table.Columns.Add("门诊统筹是否可报");
                    table.Columns.Add("更新时间");

                    #endregion

                    #region 添加诊疗项目
                    List<YLXM> ylxm = new List<YLXM>();
                    ylxm = DiagModel.ylxm_ds;
                    if (ylxm.Count > 0)
                    {
                        for (int i = 0; i < ylxm.Count; i++)
                        {
                            DataRow dr = table.NewRow(); //创建数据行
                            dr["医保项目编码"] = ylxm[i].ylxmbm;
                            dr["医保项目名称"] = ylxm[i].ylxmbzmc;
                            dr["报销病人类别编号"] = "3";
                            dr["起始日期"] = ylxm[i].qsrq;
                            dr["收费类别"] = ylxm[i].jsxmbh;
                            dr["目录等级"] = ylxm[i].mldj;
                            dr["自付类别"] = "";
                            dr["需要审批标志"] = "0";
                            dr["是否二次报销"] = "0";
                            dr["自理部分进入统筹标志"] = "0";

                            dr["自理部分进入救助金标志"] = "0";
                            dr["特检特质标志"] = "0";
                            dr["产地"] = "";
                            dr["最高价格"] = "";
                            dr["离休二乙最高价格"] = "";
                            dr["国产限价"] = "";
                            dr["门诊自付比例"] = "";
                            dr["住院自付比例"] = "";
                            dr["离休自付比例"] = "";
                            dr["工伤自付比例"] = "";

                            dr["生育自付比例"] = "";
                            dr["二乙自付比例"] = "";
                            dr["单位自付比例"] = "";
                            dr["家属子女学生自付比例"] = "";
                            dr["进口差价自付比例"] = "";
                            dr["医院绑定标志"] = "0";
                            dr["定点医疗机构编码"] = hospitalId;
                            dr["生产厂家"] = ylxm[i].scqy;
                            dr["拼音助记码"] = ylxm[i].py;
                            dr["五笔助记码"] = "";

                            dr["自定义编码"] = "";
                            dr["生产厂家1"] = ylxm[i].scqy;
                            dr["经办人"] = "";
                            dr["经办时间"] = "";
                            dr["终止时间"] = ylxm[i].zzrq;
                            dr["备注"] = ylxm[i].sm;
                            dr["限制使用范围"] = "";
                            dr["有效标志"] = ylxm[i].zxbz;
                            dr["国家基本药品目录"] = "";
                            dr["国家基本药品目录基层卫生医疗机构用药标志"] = "";

                            dr["居民使用标志"] = "0";
                            dr["药品标志"] = ylxm[i].ypbz;
                            dr["剂型码"] = ylxm[i].jxm;
                            dr["规格"] = ylxm[i].gg;
                            dr["单位"] = ylxm[i].dw;
                            //dr["单复方标志"] = ylxm[i].dffbz;
                            //dr["门诊统筹是否可报"] = ylxm[i].mztckfbx;
                            dr["更新时间"] = ylxm[i].gxsj;

                            //dr["规格"] = ylxm[i].gg;
                            //dr["单位"] = ylxm[i].dw;
                            //dr["处方药标志"] = ylxm[i].cfybz;
                            table.Rows.Add(dr);  //将创建的数据行添加到table中
                            ChargeCount++; ;
                        }
                    }
                    #endregion

                    SqlBulkCopy bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["connStr"].ToString());
                    bulkCopy.DestinationTableName = "COMM.DICT.NETWORKING_CENTER_CHARGE_DICT_BACK ";//设置数据库中对象的表名

                    #region 设置数据表table和数据库中表的列对应关系
                    //设置数据表table和数据库中表的列对应关系
                    bulkCopy.ColumnMappings.Add("医保项目编码", "NETWORK_ITEM_CODE");
                    bulkCopy.ColumnMappings.Add("医保项目名称", "NETWORK_ITEM_NAME");
                    bulkCopy.ColumnMappings.Add("报销病人类别编号", "NETWORKING_PAT_CLASS_ID");
                    bulkCopy.ColumnMappings.Add("起始日期", "CHARGE_START_TIME");
                    bulkCopy.ColumnMappings.Add("收费类别", "NETWORK_ITME_CHARGE_CLASS");
                    bulkCopy.ColumnMappings.Add("目录等级", "TYPE_MEMO");
                    bulkCopy.ColumnMappings.Add("自付类别", "CHARGE_SELF_CHARGE_CLASS");
                    bulkCopy.ColumnMappings.Add("需要审批标志", "CHARGE_FLAG_APPROVAL");
                    bulkCopy.ColumnMappings.Add("是否二次报销", "CHARGE_FLAG_SEC_SETTLE");
                    bulkCopy.ColumnMappings.Add("自理部分进入统筹标志", "CHARGE_FLAG_SELF_BURRDEN_IN_TC");

                    bulkCopy.ColumnMappings.Add("自理部分进入救助金标志", "CHARGE_FLAG_SELF_BURRDEN_IN_JZ");
                    bulkCopy.ColumnMappings.Add("特检特质标志", "CHARGE_FLAG_SPEICAL_CHARGE");
                    bulkCopy.ColumnMappings.Add("产地", "CHARGE_ORIGIN");
                    bulkCopy.ColumnMappings.Add("最高价格", "CHARGE_HIGHESET_PRICE");
                    bulkCopy.ColumnMappings.Add("离休二乙最高价格", "CHARGE_LIMIT_PRICE_FIRST");
                    bulkCopy.ColumnMappings.Add("国产限价", "CHARGE_COUNTRY_LIMIT_PRICE");
                    bulkCopy.ColumnMappings.Add("门诊自付比例", "CHARGE_MZ_SELF_BURRDEN_RATIO");
                    bulkCopy.ColumnMappings.Add("住院自付比例", "CHARGE_ZY_SELF_BURRDEN_RATIO");
                    bulkCopy.ColumnMappings.Add("离休自付比例", "CHARGE_LX_SELF_BURRDEN_RATIO");
                    bulkCopy.ColumnMappings.Add("工伤自付比例", "CHARGE_GS_SELF_BURRDEN_RATIO");

                    bulkCopy.ColumnMappings.Add("生育自付比例", "CHARGE_SY_SELF_BURRDEN_RATIO");
                    bulkCopy.ColumnMappings.Add("二乙自付比例", "CHARGE_SEC_SELF_BURRDEN_RATIO");
                    bulkCopy.ColumnMappings.Add("单位自付比例", "CHARGE_COMPANY_BURRDEN_RATIO");
                    bulkCopy.ColumnMappings.Add("家属子女学生自付比例", "CHARGE_FAMLILY_BURRDEN_RATIO");
                    bulkCopy.ColumnMappings.Add("进口差价自付比例", "CHARGE_OUT_IN_DIFF_PRICE_RATIO");
                    bulkCopy.ColumnMappings.Add("医院绑定标志", "CHARGE_FLAG_HOS_BINGDING");
                    bulkCopy.ColumnMappings.Add("定点医疗机构编码", "HOSPTIAL_ID");
                    bulkCopy.ColumnMappings.Add("生产厂家", "CHARGE_COMPANY");
                    bulkCopy.ColumnMappings.Add("拼音助记码", "CHARGE_INPUT_CODE_PY");
                    bulkCopy.ColumnMappings.Add("五笔助记码", "CHARGE_INPUT_CODE_WB");

                    bulkCopy.ColumnMappings.Add("自定义编码", "CHARGE_PERSONAL_CODE");
                    bulkCopy.ColumnMappings.Add("生产厂家", "CHARGE_MAUFACTURER");
                    bulkCopy.ColumnMappings.Add("经办人", "CHARGE_OPERATOR");
                    bulkCopy.ColumnMappings.Add("经办时间", "CHARGE_OPERATE_START_TIME");
                    bulkCopy.ColumnMappings.Add("终止时间", "CHARGE_OPERATE_END_TIME");
                    bulkCopy.ColumnMappings.Add("备注", "CHARGE_BACKUP");
                    bulkCopy.ColumnMappings.Add("限制使用范围", "CHARGE_BOUND");
                    bulkCopy.ColumnMappings.Add("有效标志", "CHARGE_FLAG_INVALID");
                    bulkCopy.ColumnMappings.Add("国家基本药品目录", "CHARGE_FLAG_BASE_LIST");
                    bulkCopy.ColumnMappings.Add("国家基本药品目录基层卫生医疗机构用药标志", "CHARGE_FLAG_BASE_MENU");

                    bulkCopy.ColumnMappings.Add("居民使用标志", "CHARGE_FLAG_CTIZEN");
                    bulkCopy.ColumnMappings.Add("药品标志", "MEMO1");
                    bulkCopy.ColumnMappings.Add("剂型码", "MEMO2");
                    bulkCopy.ColumnMappings.Add("规格", "MEMO3");
                    bulkCopy.ColumnMappings.Add("单位", "MEMO4");
                    bulkCopy.ColumnMappings.Add("更新时间", "MEMO5");
                    bulkCopy.WriteToServer(table);//将数据表table复制到数据库中
                    #endregion
                }
                #endregion

                #region 保存自付比例
                StringBuilder sss = new StringBuilder();
                sss.Append(" TRUNCATE TABLE Report.dbo.ZXML_ZFBL ");
                int aa = SQLHelper.ExecSqlReInt(sss.ToString());
                using (DataTable table2 = new DataTable())
                {
                    //为数据表创建相对应的数据列
                    table2.Columns.Add("医保项目编码");
                    table2.Columns.Add("起始日期");
                    table2.Columns.Add("终止日期");
                    table2.Columns.Add("首先自付比例");
                    table2.Columns.Add("限价");

                    #region 添加药品自付比例
                    List<ZFBL> zfbl = new List<ZFBL>();
                    zfbl = DrugModel.sxzfbl_ds;
                    if (zfbl.Count > 0)
                    {

                        for (int j = 0; j < zfbl.Count; j++)
                        {
                            DataRow dr = table2.NewRow();//创建数据行
                            dr["医保项目编码"] = zfbl[j].ylxmbm;
                            dr["起始日期"] = zfbl[j].qsrq;
                            dr["终止日期"] = zfbl[j].zzrq;
                            dr["首先自付比例"] = zfbl[j].sxzfbl;
                            dr["限价"] = zfbl[j].xj;
                            table2.Rows.Add(dr);//将创建的数据行添加到table中
                            drugRate++;
                        }
                    }
                    #endregion

                    #region zhenliaocailiao
                    //#region 添加诊疗自付比例
                    //zfbl = new List<ZFBL>();
                    //zfbl = DiagModel.sxzfbl_ds;
                    //if (zfbl.Count > 0)
                    //{

                    //    for (int j = 0; j < zfbl.Count; j++)
                    //    {

                    //        DataRow dr = table2.NewRow();//创建数据行
                    //        dr["医保项目编码"] = zfbl[j].ylxmbm;
                    //        dr["起始日期"] = zfbl[j].qsrq;
                    //        dr["终止日期"] = zfbl[j].zzrq;
                    //        dr["首先自付比例"] = zfbl[j].sxzfbl;
                    //        dr["限价"] = zfbl[j].xj;
                    //        table2.Rows.Add(dr);//将创建的数据行添加到table中
                    //        drugRate++;
                    //    }
                    //}
                    //#endregion

                    //#region 添加服务设施自付比例
                    //zfbl = new List<ZFBL>();
                    //zfbl = FwssModel.sxzfbl_ds;
                    //if (zfbl.Count > 0)
                    //{

                    //    for (int j = 0; j < zfbl.Count; j++)
                    //    {

                    //        DataRow dr = table2.NewRow();//创建数据行
                    //        dr["医保项目编码"] = zfbl[j].ylxmbm;
                    //        dr["起始日期"] = zfbl[j].qsrq;
                    //        dr["终止日期"] = zfbl[j].zzrq;
                    //        dr["首先自付比例"] = zfbl[j].sxzfbl;
                    //        dr["限价"] = zfbl[j].xj;
                    //        table2.Rows.Add(dr);//将创建的数据行添加到table中
                    //        drugRate++;
                    //    }
                    //}
                    //#endregion
                    #endregion

                    SqlBulkCopy bulkCopy2 = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["connStr"].ToString());
                    bulkCopy2.DestinationTableName = "Report.dbo.ZXML_ZFBL";//设置数据库中对象的表名
                    //设置数据表table和数据库中表的列对应关系
                    bulkCopy2.ColumnMappings.Add("医保项目编码", "医保项目编码");
                    bulkCopy2.ColumnMappings.Add("起始日期", "起始日期");
                    bulkCopy2.ColumnMappings.Add("终止日期", "终止日期");
                    bulkCopy2.ColumnMappings.Add("首先自付比例", "首先自付比例");
                    bulkCopy2.ColumnMappings.Add("限价", "限价"); ;
                    bulkCopy2.WriteToServer(table2);//将数据表table复制到数据库中
                }
                #endregion

                StringBuilder sb = new StringBuilder();
                sb.Append(" TRUNCATE TABLE COMM.DICT.NETWORKING_CENTER_DRUG_DICT ");
                int b = SQLHelper.ExecSqlReInt(sb.ToString());

                StringBuilder str = new StringBuilder();
                str.Append("insert into comm.DICT.NETWORKING_CENTER_DRUG_DICT " +
                           "select * from COMM.DICT.NETWORKING_CENTER_DRUG_DICT_BACK ");
                int YPCount = SQLHelper.ExecSqlReInt(str.ToString());

                StringBuilder sb1 = new StringBuilder();
                sb1.Append("TRUNCATE TABLE COMM.DICT.NETWORKING_CENTER_CHARGE_DICT");
                int b1 = SQLHelper.ExecSqlReInt(sb1.ToString());

                StringBuilder str1 = new StringBuilder();
                str1.Append("insert into comm.DICT.NETWORKING_CENTER_CHARGE_DICT " +
                           "select * from COMM.DICT.NETWORKING_CENTER_CHARGE_DICT_BACK ");
                int ZLCount = SQLHelper.ExecSqlReInt(str1.ToString());

                TimeSpan ts = DateTime.Now - startTime;
                //MessageBox.Show("插入药品:" + drugCount + "条，插入比例：" + drugRate + "条");
                MessageBox.Show("插入药品:" + drugCount + "条，插入诊疗:" + ChargeCount + "条，插入材料:" + Clcount + "条，插入比例：" + drugRate + "条");
            }
            else
            {
                MessageBox.Show("请先下载医保核心目录！");
            }
        }
        #endregion

        #region 医院项目对照上报
        //上传医保对照关系
        private void btnUpload_Click(object sender, EventArgs e)
        {
            int num = 0;
            int num1 = 0;
            rzybHandle = new RZYBHandle();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            DateTime startTime = DateTime.Now; //定义一个开始时间
            StringBuilder sqlUpload = new StringBuilder();
            sqlUpload.Append("SELECT HIS_ITEM_CODE 医院项目编码,HIS_ITEM_NAME 医院项目名称,NETWORK_ITEM_CODE 医保项目编码,NETWORK_ITEM_NAME 医保项目名称," +
                             "ITEM_PROP 药品标志,memo,NETWORK_ITEM_PRICE 单价,'支'规格,'1' 数量,'' 单位,'' 剂型,START_TIME 起始日期 FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORK_ITEM_FLAG_UP <> 1  and memo!='未审批通过,请取消对照'");
            DataSet up_ds = SQLHelper.ExecSqlReDs(sqlUpload.ToString());
            if (up_ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in up_ds.Tables[0].Rows)
                {
                    string str = dr["MEMO"].ToString();
                    string[] s = str.Split('|');
                    rzybHandle.InitResolver(sbjgbh, zcm, yybm, usercode);
                    rzybHandle.AddInParas("p_yyxmbm", dr["医院项目编码"].ToString().Replace(" ", ""));
                    rzybHandle.AddInParas("p_yyxmmc", dr["医院项目名称"].ToString());
                    rzybHandle.AddInParas("p_ylxmbm", dr["医保项目编码"].ToString().Replace(" ", ""));
                    rzybHandle.AddInParas("p_ylxmmc", dr["医保项目名称"].ToString());
                    rzybHandle.AddInParas("p_ypbz", dr["药品标志"].ToString());
                    rzybHandle.AddInParas("p_gg", s[0].ToString());   //规格
                    //rzybHandle.AddInParas("p_dj", s[1].ToString());   //单价
                    rzybHandle.AddInParas("p_dj", dr["单价"].ToString());   //单价
                    //rzybHandle.AddInParas("p_dj", dr["规格"].ToString());   //规格
                    rzybHandle.AddInParas("p_bhsl", dr["数量"].ToString());
                    //rzybHandle.AddInParas("p_dw", s[1].ToString());  //单位
                    rzybHandle.AddInParas("p_dw", dr["单位"].ToString());  //单位
                    rzybHandle.AddInParas("p_jxm ", dr["剂型"].ToString());  //剂型
                    string qsrq = Convert.ToDateTime(dr["起始日期"].ToString()).ToString("yyyyMMdd");
                    rzybHandle.AddInParas("p_qsrq", qsrq);
                    rzybHandle.Handle("add_yyxm_info_all");

                    dict = rzybHandle.GetResultDict();
                    if (dict["spbz"] == "1")
                    {
                        sqlUpload = new StringBuilder();
                        sqlUpload.Append("UPDATE COMM.COMM.NETWORKING_ITEM_VS_HIS SET MEMO = '自动审批通过' ,NETWORK_ITEM_FLAG_UP = 1  WHERE HIS_ITEM_CODE = '" + dr["医院项目编码"] + "'");
                        num++;
                    }
                    else
                    {
                        sqlUpload = new StringBuilder();
                        sqlUpload.Append("UPDATE COMM.COMM.NETWORKING_ITEM_VS_HIS SET MEMO = '未审批通过,请取消对照' ,NETWORK_ITEM_FLAG_UP = 0  WHERE HIS_ITEM_CODE = '" + dr["医院项目编码"] + "'");
                        num1++;
                    }
                    SQLHelper.ExecSqlReInt(sqlUpload.ToString());
                }
                TimeSpan ts = DateTime.Now - startTime;
                MessageBox.Show("自动审批通过" + num + "条,未审批" + num1 + "条！");
            }
            else
            {
                MessageBox.Show("对照表无对照信息，请先添加对照信息！");
            }

        }
        #endregion

        #region 下载中心疾病目录
        /// <summary>
        /// 下载中心疾病目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXZJBML_Click(object sender, EventArgs e)
        {
            rzybHandle = new RZYBHandle();

            rzybHandle.InitResolver(sbjgbh, zcm, yybm, usercode);
            rzybHandle.AddInParas("p_filetype", "json");
            rzybHandle.Handle("query_si_sick");

            YbjbModel = rzybHandle.GetResultZxjb();

            DateTime startTime = DateTime.Now; //定义一个开始时间
            StringBuilder sql = new StringBuilder();
            sql.Append("TRUNCATE TABLE COMM.DICT.NETWORKING_DIAGNOSIS_DICT_BACK");
            SQLHelper.ExecSqlReInt(sql.ToString());

            using (DataTable table = new DataTable())
            {
                //为table创建数据列
                table.Columns.Add("联网费别ID");
                table.Columns.Add("中心疾病编码");
                table.Columns.Add("中心疾病名称");
                table.Columns.Add("疾病分类");
                table.Columns.Add("疾病类别");
                table.Columns.Add("拼音简码");
                table.Columns.Add("五笔编码");
                table.Columns.Add("作废标志");
                table.Columns.Add("操作员");
                table.Columns.Add("创建时间");
                table.Columns.Add("限额1");
                table.Columns.Add("限额2");
                table.Columns.Add("限额3");
                table.Columns.Add("标识1");
                table.Columns.Add("标识2");
                table.Columns.Add("标识3");
                table.Columns.Add("标识4");
                table.Columns.Add("备注1");
                table.Columns.Add("备注2");
                table.Columns.Add("备注3");
                table.Columns.Add("备注4");
                table.Columns.Add("排序字段");

                List<YBJB> list = new List<YBJB>();
                list = YbjbModel.ybjb_ds;

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        DataRow dr = table.NewRow();
                        dr["联网费别ID"] = 3;
                        dr["中心疾病编码"] = list[i].jbbm;
                        dr["中心疾病名称"] = list[i].jbmc;
                        dr["疾病分类"] = list[i].mzdblb;
                        dr["疾病类别"] = list[i].mzdblb;
                        dr["拼音简码"] = list[i].py;
                        dr["五笔编码"] = "";
                        dr["作废标志"] = 0;
                        dr["操作员"] = MainForm.UserName;
                        dr["创建时间"] = DateTime.Now;
                        dr["限额1"] = 0;
                        dr["限额2"] = 0;
                        dr["限额3"] = 0;
                        dr["标识1"] = 0;
                        dr["标识2"] = 0;
                        dr["标识3"] = 0;
                        dr["标识4"] = 0;
                        dr["备注1"] = list[i].bz;
                        dr["备注2"] = "";
                        dr["备注3"] = "";
                        dr["备注4"] = "";
                        dr["排序字段"] = i;
                        table.Rows.Add(dr);
                    }
                }
                SqlBulkCopy bulkCopy2 = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["connStr"].ToString());
                bulkCopy2.DestinationTableName = "COMM.DICT.NETWORKING_DIAGNOSIS_DICT_BACK";//设置数据库中对象的表名
                //设置数据表table和数据库中表的列对应关系
                bulkCopy2.ColumnMappings.Add("联网费别ID", "NETWORKING_PAT_CLASS_ID");
                bulkCopy2.ColumnMappings.Add("中心疾病编码", "CENTER_DIAGNOSIS_CODE");
                bulkCopy2.ColumnMappings.Add("中心疾病名称", "CENTER_DIAGNOSIS_NAME");
                bulkCopy2.ColumnMappings.Add("疾病分类", "DIAGNOSIS_TYPE");
                bulkCopy2.ColumnMappings.Add("疾病类别", "DIAGNOSIS_TYPE1");
                bulkCopy2.ColumnMappings.Add("拼音简码", "INPUT_CODE");
                bulkCopy2.ColumnMappings.Add("五笔编码", "WB_CODE");
                bulkCopy2.ColumnMappings.Add("作废标志", "FLAG_INVALID");
                bulkCopy2.ColumnMappings.Add("操作员", "OPERATOR");
                bulkCopy2.ColumnMappings.Add("创建时间", "CREATE_TIME");
                bulkCopy2.ColumnMappings.Add("限额1", "LIMIT_1");
                bulkCopy2.ColumnMappings.Add("限额2", "LIMIT_2");
                bulkCopy2.ColumnMappings.Add("限额3", "LIMIT_3");
                bulkCopy2.ColumnMappings.Add("标识1", "FLAG_1");
                bulkCopy2.ColumnMappings.Add("标识2", "FLAG_2");
                bulkCopy2.ColumnMappings.Add("标识3", "FLAG_3");
                bulkCopy2.ColumnMappings.Add("标识4", "FLAG_4");
                bulkCopy2.ColumnMappings.Add("备注1", "MEMO_1");
                bulkCopy2.ColumnMappings.Add("备注2", "MEMO_2");
                bulkCopy2.ColumnMappings.Add("备注3", "MEMO_3");
                bulkCopy2.ColumnMappings.Add("备注4", "MEMO_4");
                bulkCopy2.ColumnMappings.Add("排序字段", "ORDER_NO");
                bulkCopy2.WriteToServer(table);

                StringBuilder str = new StringBuilder();
                str.Append("TRUNCATE TABLE COMM.DICT.NETWORKING_DIAGNOSIS_DICT");
                SQLHelper.ExecSqlReInt(str.ToString());

                StringBuilder str1 = new StringBuilder();
                str1.Append("set identity_insert COMM.DICT.NETWORKING_DIAGNOSIS_DICT on ");
                int identityOn = SQLHelper.ExecSqlReInt(str1.ToString());
                StringBuilder str2 = new StringBuilder();
                str2.Append("insert into COMM.DICT.NETWORKING_DIAGNOSIS_DICT " +
                            "select [NETWORKING_PAT_CLASS_ID],[CENTER_DIAGNOSIS_CODE],[CENTER_DIAGNOSIS_NAME],[DIAGNOSIS_TYPE]," +
                            "[DIAGNOSIS_TYPE1],[INPUT_CODE],[WB_CODE],[FLAG_INVALID],[OPERATOR],[CREATE_TIME],[LIMIT_1],[LIMIT_2]," +
                            "[LIMIT_3],[FLAG_1],[FLAG_2],[FLAG_3],[FLAG_4],[MEMO_1],[MEMO_2],[MEMO_3],[MEMO_4],[ORDER_NO] " +
                            "from COMM.DICT.NETWORKING_DIAGNOSIS_DICT_BACK");
                int JBMLCount = SQLHelper.ExecSqlReInt(str2.ToString());

                TimeSpan ts = DateTime.Now - startTime;
                MessageBox.Show("中心疾病目录下载完毕，已保存，共" + list.Count + "！");
            }

        }
        #endregion

        #region 上传药品目录
        /// <summary>
        /// 上传药品目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            string hosId = MainForm.HOSPITAL_ID;
            //保存医保对照关系，默认NETWORKING_ITEM_CODE为0000
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append(" INSERT INTO COMM.COMM.NETWORKING_ITEM_VS_HIS");
            sqlStr.Append(" ( NETWORKING_PAT_CLASS_ID ,ITEM_PROP ,HIS_ITEM_CODE ,HIS_ITEM_NAME ,NETWORK_ITEM_CODE ,NETWORK_ITEM_NAME ");
            sqlStr.Append(" ,SELF_BURDEN_RATIO ,MEMO ,START_TIME ,STOP_TIME ,TYPE_MEMO ,NETWORK_ITEM_PROP ,NETWORK_ITEM_CHARGE_CLASS ,");
            sqlStr.Append(" HOSPITAL_ID ,NETWORK_ITEM_PRICE ,FLAG_DISABLED ,NETWORK_ITEM_FLAG_UP )");
            sqlStr.Append(" SELECT '" + MainForm.NETWORKING_PAT_CLASS_ID + "','1', MAX(A.DRUG_CODE) AS HIS_ITEM_CODE,MAX(DRUG_NAME) AS  HIS_ITEM_NAME,'0000','未对照','0','',GETDATE() ,GETDATE() ,'','1',");
            sqlStr.Append("  ''," + hosId + ",0,'0','0' ");
            sqlStr.Append(" FROM YP.DRUG.DRUGS A LEFT JOIN COMM.COMM.DRUG_PRICE_LIST B ON A.DRUG_ID = B.DRUG_ID ");
            sqlStr.Append(" LEFT JOIN YP.DRUG.DRUG_SPEC C ON A.DRUG_ID = C.DRUG_ID ");
            sqlStr.Append(" LEFT JOIN COMM.DICT.DRUG_FORMS d ON a.DRUG_FORM_ID=d.DRUG_FORM_ID ");
            sqlStr.Append(" ,yp.DRUG.DRUG_STOCK  e ");
            sqlStr.Append(" WHERE  A.DRUG_ID = e.DRUG_ID AND B.RETAIL_PRICE <> -1 AND REPLACE(A.DRUG_CODE,' ','') NOT IN (SELECT HIS_ITEM_CODE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID=1 AND HOSPITAL_ID='" + MainForm.HOSPITAL_ID + "' ) AND a.FLAG_INVALID=0 AND CHARGE_TYPE>10 AND E.HOSPITAL_ID='" + MainForm.HOSPITAL_ID + "'");
            sqlStr.Append(" GROUP BY A.DRUG_ID ");
            SQLHelper.ExecSqlReDs(sqlStr.ToString());

            //去除重复数据
            StringBuilder sqlStr1 = new StringBuilder();
            sqlStr1.Append(" DELETE  COMM.COMM.NETWORKING_ITEM_VS_HIS  WHERE  HIS_ITEM_CODE IN ( SELECT HIS_ITEM_CODE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS ");
            sqlStr1.Append(" WHERE HOSPITAL_ID ='" + hosId + "' ");
            sqlStr1.Append(" GROUP BY  HIS_ITEM_CODE having count(HIS_ITEM_CODE) > 1 ) and AUTO_ID not in ( select min(AUTO_ID) AS autoid ");
            sqlStr1.Append(" from COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE HOSPITAL_ID ='" + hosId + "'   group by HIS_ITEM_CODE having count(HIS_ITEM_CODE)>1) ");
            sqlStr1.Append(" AND HOSPITAL_ID ='" + hosId + "' ");
            SQLHelper.ExecSqlReDs(sqlStr1.ToString());

            //上传未对照数据至地纬
            StringBuilder yp = new StringBuilder();
            yp.Append(" SELECT 'YB_'+a.HIS_ITEM_CODE yyxmbm,a.HIS_ITEM_NAME yyxmmc,a.NETWORK_ITEM_CODE as ylxmbm,a.NETWORK_ITEM_NAME as ylxmmc,'1' AS ypbz,a.NETWORK_ITEM_PRICE AS dj,b.SPEC AS gg,1 AS bhsl,b.MEASURE_UNIT_NAME AS dw, ");
            yp.Append(" FROM COMM.COMM.NETWORKING_ITEM_VS_HIS a ");
            yp.Append(" LEFT JOIN (SELECT CHARGE_CODE,CHARGE_NAME,SPEC,MEASURE_UNIT_NAME FROM COMM.COMM.CHARGE_PRICE ");
            yp.Append(" UNION   ");
            yp.Append(" SELECT DRUG_CODE,DRUG_NAME,DRUG_SPEC,n.MEASURE_UNIT_NAME FROM YP.DRUG.DRUGS m ");
            yp.Append(" LEFT JOIN COMM.DICT.MEASURE_UNITS n ON n.MEASURE_UNIT_ID = m.MEASURE_UNIT_ID ");
            yp.Append(" ) b ON a.HIS_ITEM_CODE=b.CHARGE_CODE ");
            yp.Append(" WHERE NETWORKING_PAT_CLASS_ID='" + MainForm.NETWORKING_PAT_CLASS_ID + "' AND NETWORK_ITEM_FLAG_UP='0' AND a.HOSPITAL_ID='" + MainForm.HOSPITAL_ID + "' AND a.ITEM_PROP='1' ");

            DataSet up_ds = SQLHelper.ExecSqlReDs(yp.ToString());
            foreach (DataTable dt in up_ds.Tables)
            {
                handelModel = new DW_Handle();
                handelModel.add_yyxm_info_all(dt);
            }

        }
        #endregion 上传药品目录

        #region 上传医疗目录
        /// <summary>
        /// 上传医疗目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string hosId = MainForm.HOSPITAL_ID;
            //保存医保对照关系，默认NETWORKING_ITEM_CODE为0000
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append(" INSERT INTO COMM.COMM.NETWORKING_ITEM_VS_HIS");
            sqlStr.Append(" ( NETWORKING_PAT_CLASS_ID ,ITEM_PROP ,HIS_ITEM_CODE ,HIS_ITEM_NAME ,NETWORK_ITEM_CODE ,NETWORK_ITEM_NAME ");
            sqlStr.Append(" ,SELF_BURDEN_RATIO ,MEMO ,START_TIME ,STOP_TIME ,TYPE_MEMO ,NETWORK_ITEM_PROP ,NETWORK_ITEM_CHARGE_CLASS ,");
            sqlStr.Append(" HOSPITAL_ID ,NETWORK_ITEM_PRICE ,FLAG_DISABLED ,NETWORK_ITEM_FLAG_UP )");
            sqlStr.Append(" SELECT '" + MainForm.NETWORKING_PAT_CLASS_ID + "','2', AA.HIS_ITEM_CODE,AA.HIS_ITEM_NAME,'0000','未对照','0','',GETDATE() ,GETDATE() ,'','2',");
            sqlStr.Append("  ''," + hosId + ",0,'0','0' ");
            sqlStr.Append(" FROM (SELECT CHARGE_CODE HIS_ITEM_CODE,CHARGE_NAME HIS_ITEM_NAME,SPEC 规格,INPUT_CODE AS 拼音码,PRICE 单价 ");
            sqlStr.Append(" FROM COMM.COMM.CHARGE_PRICE WHERE CHARGE_CODE NOT IN  ");
            sqlStr.Append(" (SELECT HIS_ITEM_CODE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID='" + MainForm.NETWORKING_PAT_CLASS_ID + "' AND HOSPITAL_ID ='" + hosId + "') AND COMM.COMM.CHARGE_PRICE.HOSPITAL_ID='" + hosId + "' ");
            sqlStr.Append(" UNION SELECT MAX(A.DRUG_CODE) AS HIS_ITEM_CODE,MAX(DRUG_NAME) AS HIS_ITEM_NAME,MAX(A.DRUG_SPEC) AS 规格,MAX(A.INPUT_CODE) AS 拼音码,CONVERT(FLOAT,MAX(RETAIL_PRICE)) AS 单价 ");
            sqlStr.Append("  FROM YP.DRUG.DRUGS A LEFT JOIN COMM.COMM.DRUG_PRICE_LIST B ON A.DRUG_ID = B.DRUG_ID ");
            sqlStr.Append("  LEFT JOIN YP.DRUG.DRUG_SPEC C ON A.DRUG_ID = C.DRUG_ID ");
            sqlStr.Append(" ,yp.DRUG.DRUG_STOCK  e ");
            sqlStr.Append("  WHERE  A.DRUG_ID = e.DRUG_ID AND  B.RETAIL_PRICE <> -1  ");
            sqlStr.Append("  AND REPLACE(A.DRUG_CODE,' ','') NOT IN (SELECT HIS_ITEM_CODE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID='" + MainForm.NETWORKING_PAT_CLASS_ID + "' AND HOSPITAL_ID ='" + hosId + "') ");
            sqlStr.Append("  AND a.FLAG_INVALID=0 AND CHARGE_TYPE<5   AND e.HOSPITAL_ID ='" + hosId + "'  ");
            sqlStr.Append("  GROUP BY A.DRUG_ID )AA ");
            SQLHelper.ExecSqlReDs(sqlStr.ToString());

            //去除重复数据
            StringBuilder sqlStr1 = new StringBuilder();
            sqlStr1.Append(" DELETE  COMM.COMM.NETWORKING_ITEM_VS_HIS  WHERE  HIS_ITEM_CODE IN ( SELECT HIS_ITEM_CODE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS ");
            sqlStr1.Append(" WHERE HOSPITAL_ID ='" + hosId + "' ");
            sqlStr1.Append(" GROUP BY  HIS_ITEM_CODE having count(HIS_ITEM_CODE) > 1 ) and AUTO_ID not in ( select min(AUTO_ID) AS autoid ");
            sqlStr1.Append(" from COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE HOSPITAL_ID ='" + hosId + "'   group by HIS_ITEM_CODE having count(HIS_ITEM_CODE)>1) ");
            sqlStr1.Append(" AND HOSPITAL_ID ='" + hosId + "' ");
            SQLHelper.ExecSqlReDs(sqlStr1.ToString());


            //上传未对照数据至地纬
            StringBuilder yp = new StringBuilder();
            yp.Append(" SELECT 'YB_'+a.HIS_ITEM_CODE yyxmbm,a.HIS_ITEM_NAME yyxmmc,a.NETWORK_ITEM_CODE as ylxmbm,a.NETWORK_ITEM_NAME as ylxmmc,'1' AS ypbz,a.NETWORK_ITEM_PRICE AS dj,b.SPEC AS gg,1 AS bhsl,b.MEASURE_UNIT_NAME AS dw, ");
            yp.Append(" FROM COMM.COMM.NETWORKING_ITEM_VS_HIS a ");
            yp.Append(" LEFT JOIN (SELECT CHARGE_CODE,CHARGE_NAME,SPEC,MEASURE_UNIT_NAME FROM COMM.COMM.CHARGE_PRICE ");
            yp.Append(" UNION   ");
            yp.Append(" SELECT DRUG_CODE,DRUG_NAME,DRUG_SPEC,n.MEASURE_UNIT_NAME FROM YP.DRUG.DRUGS m ");
            yp.Append(" LEFT JOIN COMM.DICT.MEASURE_UNITS n ON n.MEASURE_UNIT_ID = m.MEASURE_UNIT_ID ");
            yp.Append(" ) b ON a.HIS_ITEM_CODE=b.CHARGE_CODE ");
            yp.Append(" WHERE NETWORKING_PAT_CLASS_ID='" + MainForm.NETWORKING_PAT_CLASS_ID + "' AND NETWORK_ITEM_FLAG_UP='0' AND a.HOSPITAL_ID='" + MainForm.HOSPITAL_ID + "' AND a.ITEM_PROP='2' ");
            DataSet up_ds = SQLHelper.ExecSqlReDs(yp.ToString());
            foreach (DataTable dt in up_ds.Tables)
            {
                handelModel = new DW_Handle();
                handelModel.add_yyxm_info_all(dt);
            }

        }
        #endregion

        #region 项目对照信息下载
        /// <summary>
        /// 项目对照信息下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DownYBDZGX_Click(object sender, EventArgs e)
        {
            string Zcm = "211231I-970453-771964-4535";
            string Yybm = "51080096";
            zcm = Zcm;
            yybm = Yybm;
            string search = "";
            if (tboSearch.Text != "" && tboSearch != null)
            {
                search = tboSearch.Text.ToString();
            }

            rzybHandle = new RZYBHandle();

            rzybHandle.InitResolver(sbjgbh, zcm, yybm, usercode); //初始化参数 ：社保机构编号、注册码、医院编码、usercode
            rzybHandle.AddInParas("p_filetype", "json");          //返回参数的格式 json  excel  txt
            rzybHandle.AddInParas("p_yyxmbm", search);            //医院项目编号
            rzybHandle.Handle("query_yyxm_info");                 //项目对照信息下载 接口名称

            yyxm_ds = rzybHandle.GetResult<List<YYXM>>("yyxm_ds");

            using (DataTable table = new DataTable())
            {
                table.Columns.Add("医院项目编码");
                table.Columns.Add("医院项目名称");
                table.Columns.Add("医保项目编码");
                table.Columns.Add("医疗项目名称");
                table.Columns.Add("药品标志");
                table.Columns.Add("收费类别");
                table.Columns.Add("规格");
                table.Columns.Add("单位");
                table.Columns.Add("剂型");
                table.Columns.Add("起始日期");
                table.Columns.Add("终止日期");
                table.Columns.Add("更新时间");
                table.Columns.Add("审批标志");
                table.Columns.Add("审批时间");
                if (yyxm_ds.Count > 0)
                {
                    for (int i = 0; i < yyxm_ds.Count; i++)
                    {
                        DataRow dr = table.NewRow();
                        dr["医院项目编码"] = yyxm_ds[i].yyxmbm;
                        dr["医院项目名称"] = yyxm_ds[i].yyxmmc;
                        dr["医保项目编码"] = yyxm_ds[i].ylxmbm;
                        dr["医疗项目名称"] = yyxm_ds[i].ylxmmc;
                        dr["药品标志"] = yyxm_ds[i].ypbz;
                        dr["收费类别"] = yyxm_ds[i].jsxmbh;
                        dr["规格"] = yyxm_ds[i].gg;
                        dr["单位"] = yyxm_ds[i].dw;
                        dr["剂型"] = yyxm_ds[i].jxm;
                        dr["起始日期"] = yyxm_ds[i].qsrq;
                        dr["终止日期"] = yyxm_ds[i].zzrq;
                        dr["更新时间"] = yyxm_ds[i].gxsj;
                        dr["审批标志"] = yyxm_ds[i].spbz;
                        dr["审批时间"] = yyxm_ds[i].spsj;
                        table.Rows.Add(dr);
                    }
                }

                dataGridView1.DataSource = table;
            }
        }
        #endregion

        #region 保存医保对照关系
        /// <summary>
        /// 保存医保对照关系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            handelModel = new DW_Handle();
            string sbjgbh = MainForm.sbjgbh;
            string filename = Environment.CurrentDirectory + @"\YBDLOAD\医院项目目录与核心端目录对照关系.txt";
            DateTime startTime = DateTime.Now; //定义一个开始时间
            //因为文件比较大，所有使用StreamReader的效率要比使用File.ReadLines高
            int n = 0;
            using (StreamReader sr = new StreamReader(filename, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    string readStr = sr.ReadLine();//读取一行数据

                    string[] strs = readStr.Split(new char[] { '\t', '"' });//将读取的字符串按"制表符/t“和””“分割成数组, StringSplitOptions.RemoveEmptyEntries
                    //医保中上传的HIS编码
                    string tem_code = strs[0];
                    if (tem_code == null || tem_code.IndexOf("YB_") == -1)
                    {
                        //医保有遗留数据，不包含YB_开头的不是HIS数据，剔除
                        continue;
                    }

                    if (strs.Length < 15)
                    {
                        n++;
                        continue;
                    }
                    //HIS编码
                    string HisItemCode = tem_code.Substring(3, tem_code.Length - 3);
                    //在地纬中对应的医保项目编码
                    string network_item_code = strs[4];
                    //药品标志
                    string isDrug = strs[10];
                    //自付比例
                    string selfCostRate = strs[2];
                    string reim_type = "";
                    if (Convert.ToDecimal(selfCostRate) == 1)
                    {
                        reim_type = "丙";
                    }
                    else if (Convert.ToDecimal(selfCostRate) == 0)
                    {
                        reim_type = "甲";
                    }
                    else
                    {
                        reim_type = "乙";
                    }
                    //更新对照关系表
                    StringBuilder sqlStr = new StringBuilder();
                    sqlStr.Append(" UPDATE COMM.COMM.NETWORKING_ITEM_VS_HIS ");
                    sqlStr.Append(" SET NETWORK_ITEM_CODE = '" + tem_code + "',");
                    sqlStr.Append(" NETWORK_ITEM_NAME = (SELECT TOP 1 医保项目名称 NETWORK_ITEM_NAME FROM YB.dbo.ZXML WHERE 医保项目编码 ='" + network_item_code + "'), ");
                    sqlStr.Append(" TYPE_MEMO = '" + reim_type + "',");
                    sqlStr.Append(" MEMO = '" + reim_type + "',");
                    sqlStr.Append(" SELF_BURDEN_RATIO = " + Convert.ToDecimal(selfCostRate) + ",");
                    sqlStr.Append(" NETWORK_ITEM_FLAG_UP = '1'");
                    sqlStr.Append(" WHERE  HIS_ITEM_CODE ='" + HisItemCode + "'");
                    sqlStr.Append(" AND  HOSPITAL_ID='" + MainForm.HOSPITAL_ID + "'");
                    SQLHelper.ExecSqlReDs(sqlStr.ToString());
                }
                TimeSpan ts = DateTime.Now - startTime;
                MessageBox.Show("更新成功！");
            }
        }
        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            string filename = Environment.CurrentDirectory + @"\药品变更\删除的药品.xlsx";
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1'";

            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                conn.Open();
                DataTable sheetsName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null); //得到所有sheet的名字
                string firstSheetName = sheetsName.Rows[0][2].ToString(); //得到第一个sheet的名字
                string sql = string.Format("SELECT * FROM [{0}]", firstSheetName); //查询字符串
                //string sql = string.Format("SELECT * FROM [{0}] WHERE [日期] is not null", firstSheetName); //查询字符串

                OleDbDataAdapter ada = new OleDbDataAdapter(sql, strConn);
                DataSet set = new DataSet();
                ada.Fill(set);
                DataTable dt = set.Tables[0];
                string today = DateTime.Now.ToString("yyyy-MM-dd");
                int n = 0;
                for (int i = 2; i < dt.Rows.Count; i++)
                {
                    string item_Code = dt.Rows[i][0].ToString();
                    string item_Name = dt.Rows[i][1].ToString();
                    string stop_date = dt.Rows[i][2].ToString();
                    StringBuilder sqlStr = new StringBuilder();
                    sqlStr.Append(" DELETE report.dbo.DELETE_NETWORKING_DRUG WHERE NETWORK_ITEM_CODE ='" + item_Code + "' AND CREATE_DATE='" + today + "'; ");
                    sqlStr.Append(" INSERT INTO report.dbo.DELETE_NETWORKING_DRUG");
                    sqlStr.Append(" ( NETWORK_ITEM_CODE ,NETWORK_ITEM_NAME ,FLAG_INVAILD ,CREATE_DATE ,NETWORK_STOP_DATE   )");
                    sqlStr.Append(" VALUES ('" + item_Code + "','" + item_Name + "','0','" + today + "','" + stop_date + "')");
                    SQLHelper.ExecSqlReDs(sqlStr.ToString());
                    n++;
                }
                MessageBox.Show("更新成功，更新条数：" + n);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 查询数据字典
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click_1(object sender, EventArgs e)
        {
            rzybHandle = new RZYBHandle();
            string zcm = "211231I-970453-771964-4535";
            string yybm = "51080096";
            string dmbh = txtxSjzd.Text.Trim();

            //string search = "";
            //if (tboSearch.Text != "" && tboSearch != null)
            //{
            //    search = tboSearch.Text.ToString();
            //}

            rzybHandle = new RZYBHandle();
            rzybHandle.InitResolver(sbjgbh, zcm, yybm, usercode); //初始化参数 ：社保机构编号、注册码、医院编码、usercode
            rzybHandle.AddInParas("p_dmbh", dmbh);         //返回参数的格式 json  excel  txt
            rzybHandle.Handle("query_si_code");                       // 传入方法名

            cxzd_ds = rzybHandle.GetResult<List<Cxzd>>("code_ds");

            using (DataTable table = new DataTable())
            {
                table.Columns.Add("代码编号");
                table.Columns.Add("字典名称");

                if (cxzd_ds.Count > 0)
                {
                    for (int i = 0; i < cxzd_ds.Count; i++)
                    {
                        DataRow dr = table.NewRow();
                        dr["代码编号"] = cxzd_ds[i].code;
                        dr["字典名称"] = cxzd_ds[i].content;

                        table.Rows.Add(dr);
                    }
                }

                dataGridView1.DataSource = table;
            }

        }

        /// <summary>
        /// 医疗待遇封锁信息查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            rzybHandle = new RZYBHandle();
            string zcm = "211231I-970453-771964-4535";
            string yybm = "51080096";
            string dmbh = txtyldyfsxxcx.Text.Trim();
            string Sbjg = txtSbjg.Text.Trim();
            rzybHandle = new RZYBHandle();
            rzybHandle.InitResolver(Sbjg, zcm, yybm, usercode); //初始化参数 ：社保机构编号、注册码、医院编码、usercode
            rzybHandle.AddInParas("p_grbh", dmbh);         //返回参数的格式 json  excel  txt
            rzybHandle.Handle("query_dyfs");                       // 传入方法名

            fsxx_ds = rzybHandle.GetResult<List<dyfs>>("fsxx_ds");

            using (DataTable table = new DataTable())
            {
                table.Columns.Add("封锁原因");
                table.Columns.Add("封锁类型");
                table.Columns.Add("起始日期");
                table.Columns.Add("终止日期");

                if (fsxx_ds.Count > 0)
                {
                    for (int i = 0; i < fsxx_ds.Count; i++)
                    {
                        DataRow dr = table.NewRow();
                        dr["封锁原因"] = fsxx_ds[i].fsyy;
                        dr["封锁类型"] = fsxx_ds[i].fslx;
                        dr["起始日期"] = fsxx_ds[i].qsrq;
                        dr["终止日期"] = fsxx_ds[i].zzrq;

                        table.Rows.Add(dr);
                    }
                }

                dataGridView1.DataSource = table;
            }
        }

        /// <summary>
        /// 查询靶向药信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button7_Click(object sender, EventArgs e)
        {
            rzybHandle = new RZYBHandle();
            string zcm = "211231I-970453-771964-4535";
            string yybm = "51080096";
            string dmbh = txtBxyspxx.Text.Trim();
            string Sbjg = txtSbjgbxy.Text.Trim();
            string Rq = dateTimePicker1.Value.ToString("yyyyMMdd");
            rzybHandle = new RZYBHandle();
            rzybHandle.InitResolver(Sbjg, zcm, yybm, usercode); //初始化参数 ：社保机构编号、注册码、医院编码、usercode
            rzybHandle.AddInParas("p_grbh", dmbh);         //个人编号
            rzybHandle.AddInParas("p_qsrq", Rq);         //起始日期
            rzybHandle.Handle("query_bxy_info");                       // 传入方法名

            spxx_ds = rzybHandle.GetResult<List<Bxysp>>("spxx_ds");

            using (DataTable table = new DataTable())
            {
                table.Columns.Add("审批医院编码");
                table.Columns.Add("审批医院名称");
                table.Columns.Add("审批药店编码");
                table.Columns.Add("审批药店名称");
                table.Columns.Add("审批起始日期");
                table.Columns.Add("审批终止日期");
                table.Columns.Add("审批药品编码");
                table.Columns.Add("审批药品名称");
                table.Columns.Add("医师编码");
                table.Columns.Add("医师姓名");

                if (spxx_ds.Count > 0)
                {
                    for (int i = 0; i < spxx_ds.Count; i++)
                    {
                        DataRow dr = table.NewRow();
                        dr["审批医院编码"] = spxx_ds[i].yybm;
                        dr["审批医院名称"] = spxx_ds[i].yymc;
                        dr["审批药店编码"] = spxx_ds[i].ydbm;
                        dr["审批药店名称"] = spxx_ds[i].ydmc;
                        dr["审批起始日期"] = spxx_ds[i].qsrq;
                        dr["审批终止日期"] = spxx_ds[i].zzrq;
                        dr["审批药品编码"] = spxx_ds[i].ylxmbm;
                        dr["审批药品名称"] = spxx_ds[i].ylxmmc;
                        dr["医师编码"] = spxx_ds[i].ysbh;
                        dr["医师姓名"] = spxx_ds[i].ysxm;

                        table.Rows.Add(dr);
                    }
                }

                dataGridView1.DataSource = table;
            }
        }



        /// <summary>
        /// 下载保存下来的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            string Zcm = "211231I-970453-771964-4535";
            string Yybm = "51080096";
            zcm = Zcm;
            yybm = Yybm;
            //string ypbz = "";
            string spbzz = "";
            rzybHandle = new RZYBHandle();
            rzybHandle.InitResolver(sbjgbh, zcm, yybm, usercode);
            rzybHandle.AddInParas("p_filetype", "json");
            rzybHandle.AddInParas("p_yyxmbm", "");
            rzybHandle.Handle("query_yyxm_info");

            DzXzModel = rzybHandle.GetResultZxZXZ();

            DateTime startTime = DateTime.Now; //定义一个开始时间
            StringBuilder sql = new StringBuilder();
            sql.Append("TRUNCATE TABLE COMM.COMM.NETWORKING_ITEM_VS_HIS_BACK");
            SQLHelper.ExecSqlReInt(sql.ToString());

            using (DataTable table = new DataTable())
            {
                //为table创建数据列
                table.Columns.Add("联网患者报销类型ID");
                table.Columns.Add("项目属性");
                table.Columns.Add("HIS项目代码");
                table.Columns.Add("HIS项目名称");
                table.Columns.Add("联网报销项目ID");
                table.Columns.Add("联网报销项目名称");
                table.Columns.Add("自付比例");
                table.Columns.Add("备注");
                table.Columns.Add("启用时间");
                table.Columns.Add("停用时间");
                table.Columns.Add("医保类型");
                table.Columns.Add("费用属性");
                table.Columns.Add("项目类别");
                table.Columns.Add("医院编号ID");
                table.Columns.Add("联网报销项目价格");
                table.Columns.Add("该项是否已禁用");
                table.Columns.Add("上传标志");


                List<YYXM> list = new List<YYXM>();
                list = DzXzModel.yyxm_ds;

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].spbz == "1")
                        {
                            spbzz = "审批已通过";
                        }
                        else if (list[i].spbz == "2")
                        {
                            spbzz = "审批未通过";
                        }
                        else if (list[i].spbz == "0")
                        {
                            spbzz = "未审批";
                        }
                        else
                        {
                            spbzz = list[i].spbz;
                        }
                        DataRow dr = table.NewRow();
                        dr["联网患者报销类型ID"] = 3;
                        dr["项目属性"] = list[i].ypbz;
                        dr["HIS项目代码"] = list[i].yyxmbm;
                        dr["HIS项目名称"] = list[i].yyxmmc;
                        dr["联网报销项目ID"] = list[i].ylxmbm;
                        dr["联网报销项目名称"] = list[i].ylxmmc;
                        dr["自付比例"] = "0";
                        dr["备注"] = spbzz;
                        if (string.IsNullOrEmpty(list[i].qsrq))
                        {
                            // MessageBox.Show("1");
                            list[i].qsrq = "20991106";
                        }
                        //DateTime a = DateTime.Now;//2099-11-06 17:10:33
                        DateTime qsrq = DateTime.ParseExact(list[i].qsrq, "yyyyMMdd", Thread.CurrentThread.CurrentCulture);// list[i].qsrq);
                        // bool b = DateTime.TryParseExact(list[i].qsrq, "yyyyMMdd", Thread.CurrentThread.CurrentCulture, 0, out a);
                        if (string.IsNullOrEmpty(list[i].zzrq))
                        {
                            list[i].zzrq = "2099-11-06 17:10:33";
                        }

                        dr["启用时间"] = qsrq;//(list[i].qsrq);
                        dr["停用时间"] = Convert.ToDateTime(list[i].zzrq);
                        dr["医保类型"] = "";
                        dr["费用属性"] = list[i].ypbz;
                        dr["项目类别"] = list[i].jsxmbh;
                        dr["医院编号ID"] = 1;
                        dr["联网报销项目价格"] = 0;
                        dr["该项是否已禁用"] = false;
                        dr["上传标志"] = 1;
                        table.Rows.Add(dr);
                    }
                }
                SqlBulkCopy bulkCopy2 = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["connStr"].ToString());
                bulkCopy2.DestinationTableName = "COMM.COMM.NETWORKING_ITEM_VS_HIS_BACK";//设置数据库中对象的表名
                //设置数据表table和数据库中表的列对应关系
                bulkCopy2.ColumnMappings.Add("联网患者报销类型ID", "NETWORKING_PAT_CLASS_ID");
                bulkCopy2.ColumnMappings.Add("项目属性", "ITEM_PROP");
                bulkCopy2.ColumnMappings.Add("HIS项目代码", "HIS_ITEM_CODE");
                bulkCopy2.ColumnMappings.Add("HIS项目名称", "HIS_ITEM_NAME");
                bulkCopy2.ColumnMappings.Add("联网报销项目ID", "NETWORK_ITEM_CODE");
                bulkCopy2.ColumnMappings.Add("联网报销项目名称", "NETWORK_ITEM_NAME");
                bulkCopy2.ColumnMappings.Add("自付比例", "SELF_BURDEN_RATIO");
                bulkCopy2.ColumnMappings.Add("备注", "MEMO");
                bulkCopy2.ColumnMappings.Add("启用时间", "START_TIME".ToString());
                bulkCopy2.ColumnMappings.Add("停用时间", "STOP_TIME".ToString());
                bulkCopy2.ColumnMappings.Add("医保类型", "TYPE_MEMO");
                bulkCopy2.ColumnMappings.Add("费用属性", "NETWORK_ITEM_PROP");
                bulkCopy2.ColumnMappings.Add("项目类别", "NETWORK_ITEM_CHARGE_CLASS");
                bulkCopy2.ColumnMappings.Add("医院编号ID", "HOSPITAL_ID");
                bulkCopy2.ColumnMappings.Add("联网报销项目价格", "NETWORK_ITEM_PRICE");
                bulkCopy2.ColumnMappings.Add("该项是否已禁用", "FLAG_DISABLED");
                bulkCopy2.ColumnMappings.Add("上传标志", "NETWORK_ITEM_FLAG_UP");

                bulkCopy2.WriteToServer(table);

                StringBuilder str = new StringBuilder();
                str.Append("TRUNCATE TABLE COMM.COMM.NETWORKING_ITEM_VS_HIS");
                SQLHelper.ExecSqlReInt(str.ToString());




                StringBuilder str1 = new StringBuilder();
                str1.Append("set identity_insert COMM.COMM.NETWORKING_ITEM_VS_HIS on ");
                int identityOn = SQLHelper.ExecSqlReInt(str1.ToString());
                StringBuilder str2 = new StringBuilder();
                str2.Append("insert into COMM.COMM.NETWORKING_ITEM_VS_HIS " +
                            "select [NETWORKING_PAT_CLASS_ID],[ITEM_PROP],[HIS_ITEM_CODE],[HIS_ITEM_NAME]," +
                            "[NETWORK_ITEM_CODE],[NETWORK_ITEM_NAME],[SELF_BURDEN_RATIO],[MEMO],[START_TIME],[STOP_TIME],[TYPE_MEMO],[NETWORK_ITEM_PROP]," +
                            "[NETWORK_ITEM_CHARGE_CLASS],[HOSPITAL_ID],[NETWORK_ITEM_PRICE],[FLAG_DISABLED],[NETWORK_ITEM_FLAG_UP] " +
                            "from COMM.COMM.NETWORKING_ITEM_VS_HIS_BACK");
                int JBMLCount = SQLHelper.ExecSqlReInt(str2.ToString());



                StringBuilder strqc1 = new StringBuilder();
                strqc1.Append("delete   FROM COMM.COMM.NETWORKING_ITEM_VS_HIS   where   memo  in ('未审批','审批未通过') and  flag_disabled='0'  ");
                SQLHelper.ExecSqlReInt(strqc1.ToString());

                StringBuilder strip = new StringBuilder();
                strip.Append("update  COMM.COMM.NETWORKING_ITEM_VS_HIS set item_prop=2   where  item_prop=0");
                SQLHelper.ExecSqlReInt(strip.ToString());

                StringBuilder strip2 = new StringBuilder();
                strip2.Append("update  COMM.COMM.NETWORKING_ITEM_VS_HIS set  his_item_name='L凝血酶时间测定(TT)',item_prop=0,network_item_name='L凝血酶时间测定(TT)'   WHERE NETWORK_ITEM_CODE='250203035Z00'");
                SQLHelper.ExecSqlReInt(strip2.ToString());
                //{ DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE   NETWORKING_PAT_CLASS_ID='3 '  AND  HIS_ITEM_CODE='1017-4' AND NETWORK_ITEM_CODE=00205894}
                //update  COMM.COMM.NETWORKING_ITEM_VS_HIS set  his_item_name='L凝血酶时间测定(TT)',item_prop=0,network_item_name='L凝血酶时间测定(TT)'   WHERE NETWORK_ITEM_CODE='250203035Z00'

                //StringBuilder strip3 = new StringBuilder();
                //strip3.Append("DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE   NETWORKING_PAT_CLASS_ID='3 '  AND  HIS_ITEM_CODE='1017-4' AND NETWORK_ITEM_CODE=00205894");
                //SQLHelper.ExecSqlReInt(strip3.ToString());

                string bb = " DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE   NETWORKING_PAT_CLASS_ID='3 '  AND  HIS_ITEM_CODE='1006' AND NETWORK_ITEM_CODE='00206156'  AND  HIS_ITEM_NAME='生化全项'	 DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE   NETWORKING_PAT_CLASS_ID='3 '  AND  HIS_ITEM_CODE='20110' AND NETWORK_ITEM_CODE='ZA12BY05160000'  AND  HIS_ITEM_NAME='Y速效救心丸'	 DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE   NETWORKING_PAT_CLASS_ID='3 '  AND  HIS_ITEM_CODE='40528-03' AND NETWORK_ITEM_CODE='202837'  AND  HIS_ITEM_NAME='抗链球菌溶血素O测定(ASO)'	 DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE   NETWORKING_PAT_CLASS_ID='3 '  AND  HIS_ITEM_CODE='40548' AND NETWORK_ITEM_CODE='00205894'  AND  HIS_ITEM_NAME='家庭病床建床费'	 DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE   NETWORKING_PAT_CLASS_ID='3 '  AND  HIS_ITEM_CODE='5002-233' AND NETWORK_ITEM_CODE='210102015z00'  AND  HIS_ITEM_NAME='L胸部正侧位'	 DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE   NETWORKING_PAT_CLASS_ID='3 '  AND  HIS_ITEM_CODE='B384' AND NETWORK_ITEM_CODE='ZFYP0000'  AND  HIS_ITEM_NAME='妇炎洁抑洗液'	 DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE   NETWORKING_PAT_CLASS_ID='3 '  AND  HIS_ITEM_CODE='5002-23' AND NETWORK_ITEM_CODE='2014030546'  AND  HIS_ITEM_NAME='胸部正侧位'	";
                string[] strs = bb.Split(new char[] { '\t', '\t' });
                foreach (var itdelete in strs)
                {
                    if (!string.IsNullOrEmpty(itdelete.ToString()))
                    {
                        StringBuilder strip4 = new StringBuilder();
                        strip4.Append(itdelete.ToString());
                       
                        SQLHelper.ExecSqlReInt(strip4.ToString());
                    }


                }


                TimeSpan ts = DateTime.Now - startTime;
                MessageBox.Show("三大目录下载完毕，已保存，共" + list.Count + "！请注意清除历史重复数据！");
            }
        }

        private void DownLoad_Load(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void button10_Click(object sender, EventArgs e)
        {
            string Zcm = "211231I-970453-771964-4535";
            string Yybm = "51080096";
            zcm = Zcm;
            yybm = Yybm;
            string search = "";
            if (tboSearch.Text != "" && tboSearch != null)
            {
                search = tboSearch.Text.ToString();
            }

            rzybHandle = new RZYBHandle();

            rzybHandle.InitResolver(sbjgbh, zcm, yybm, usercode); //初始化参数 ：社保机构编号、注册码、医院编码、usercode
            rzybHandle.AddInParas("p_filetype", "json");          //返回参数的格式 json  excel  txt
            rzybHandle.AddInParas("p_yyxmbm", search);            //医院项目编号
            rzybHandle.Handle("query_yyxm_info");                 //项目对照信息下载 接口名称

            yyxm_ds = rzybHandle.GetResult<List<YYXM>>("yyxm_ds");

            using (DataTable table = new DataTable())
            {
                table.Columns.Add("医院项目编码");
                table.Columns.Add("医院项目名称");
                table.Columns.Add("医保项目编码");
                table.Columns.Add("医疗项目名称");
                table.Columns.Add("药品标志");
                table.Columns.Add("收费类别");
                table.Columns.Add("规格");
                table.Columns.Add("单位");
                table.Columns.Add("剂型");
                table.Columns.Add("起始日期");
                table.Columns.Add("终止日期");
                table.Columns.Add("更新时间");
                table.Columns.Add("审批标志");
                table.Columns.Add("审批时间");
                if (yyxm_ds.Count > 0)
                {
                    for (int i = 0; i < yyxm_ds.Count; i++)
                    {
                        DataRow dr = table.NewRow();
                        dr["医院项目编码"] = yyxm_ds[i].yyxmbm;
                        dr["医院项目名称"] = yyxm_ds[i].yyxmmc;
                        dr["医保项目编码"] = yyxm_ds[i].ylxmbm;
                        dr["医疗项目名称"] = yyxm_ds[i].ylxmmc;
                        dr["药品标志"] = yyxm_ds[i].ypbz;
                        dr["收费类别"] = yyxm_ds[i].jsxmbh;
                        dr["规格"] = yyxm_ds[i].gg;
                        dr["单位"] = yyxm_ds[i].dw;
                        dr["剂型"] = yyxm_ds[i].jxm;
                        dr["起始日期"] = yyxm_ds[i].qsrq;
                        dr["终止日期"] = yyxm_ds[i].zzrq;
                        dr["更新时间"] = yyxm_ds[i].gxsj;
                        dr["审批标志"] = yyxm_ds[i].spbz;
                        dr["审批时间"] = yyxm_ds[i].spsj;
                        table.Rows.Add(dr);
                    }
                    if (File.Exists(@"D:\未审核项目对照软件专属请勿重名避免误删.xls"))
                    {
                        File.Delete(@"D:\未审核项目对照软件专属请勿重名避免误删.xls");
                        ExcelHelper.TableToExcel(table, @"D:\未审核项目对照软件专属请勿重名避免误删.xls");

                    }
                    else
                    {
                        ExcelHelper.TableToExcel(table, @"D:\未审核项目对照软件专属请勿重名避免误删.xls");

                    }

                    MessageBox.Show(@"导出D:\未审核项目对照软件专属请勿重名避免误删.xls成功");


                }




            }

       // 
        }


    }
}