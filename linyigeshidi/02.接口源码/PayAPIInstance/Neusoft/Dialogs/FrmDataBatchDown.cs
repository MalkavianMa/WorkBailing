using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections; 
using System.IO;
using System.Threading; 
using System.Data.SqlClient;
using System.Data.OleDb;
using PayAPIResolver.Neusoft;
using PayAPIInterfaceHandle.Neusoft;

namespace PayAPIInstance.Neusoft.Dialogs
{
    public partial class FrmDataBatchDown : Form
    {
        public string _hosID;
        public MSSQLHelper sqlHelper;
        public NeusoftResolver neusoft;
        public string txt_conn; //Txt数据源
        AutoCompleteStringCollection ac = new AutoCompleteStringCollection();

        /// <summary>
        /// 东软函数处理
        /// </summary>
        private INeusoftHandle neusoftHandle = NeusoftHandleFactory.GetCommNeusoftHandle();

        public FrmDataBatchDown(string hosID,string conn)
        {
            InitializeComponent();
            _hosID = hosID;
            sqlHelper = new MSSQLHelper(conn);
        }

        private void FrmDataBatchDown_Load(object sender, EventArgs e)
        {
            #region 填充combox控件 项目名称
            ArrayList arr = new ArrayList();
            arr.Add(new DictionaryEntry("01", "药品目录"));                             //01	药品目录	
            arr.Add(new DictionaryEntry("02", "诊疗项目信息"));                         //02	诊疗项目信息	
            arr.Add(new DictionaryEntry("03", "医疗服务设施信息"));                     //03	医疗服务设施信息	
            arr.Add(new DictionaryEntry("04", "费用类别信息"));                         //04	费用类别信息	
            arr.Add(new DictionaryEntry("05", "病种信息"));                             //05	病种信息	
            arr.Add(new DictionaryEntry("06", "住院和慢性病审批信息"));                 //06	住院和慢性病审批信息	开始时间从2012年开始上线
            arr.Add(new DictionaryEntry("07", "特检特治特药审批信息"));                 //07	特检特治特药审批信息	
            arr.Add(new DictionaryEntry("08", "费用明细"));                         //08	医院医师信息	
            arr.Add(new DictionaryEntry("09", "对照审批信息下载"));                     //09	对照审批信息下载	
            arr.Add(new DictionaryEntry("10", "门诊慢性病治疗方案明细下载"));           //10	门诊慢性病治疗方案明细下载	
            arr.Add(new DictionaryEntry("11", "定点医疗机构与统筹区对应关系下载"));     //11	定点医疗机构与统筹区对应关系下载	

            this.cmbProject.DataSource = arr;
            this.cmbProject.DisplayMember = "Value";
            this.cmbProject.ValueMember = "Key";
            this.cmbProject.SelectedIndex = 5;

            //不使用
            arr = new ArrayList();
            arr.Add(new DictionaryEntry("0", "未审核"));
            arr.Add(new DictionaryEntry("1", "审核通过"));
            arr.Add(new DictionaryEntry("2", "审核未通过"));
            //arr.Add(new DictionaryEntry("3", "全部"));

            this.cmbAuditFlag.DataSource = arr;
            this.cmbAuditFlag.DisplayMember = "Value";
            this.cmbAuditFlag.ValueMember = "Key";
            this.cmbAuditFlag.SelectedIndex = 1;
            #endregion

            #region 时间控件初始化
            this.dptStartDate.Value = Convert.ToDateTime(System.DateTime.Today.ToString("yyyy-MM-dd"));
            #endregion
        }

        #region DataGridView列设置
        /// <summary>
        ///  设置DataGridView的列
        /// </summary>
        /// <param name="projectID">项目的ID编号</param>
        private void setDataGridView(string projectID)
        {
            this.dgrvProjectInfo.Columns.Clear();
            switch (projectID)
            {
                case "01":
                    #region 药品目录
                    this.dgrvProjectInfo.Columns.Add("0 ", "药品编码");
                    this.dgrvProjectInfo.Columns.Add("1 ", "通用名称");
                    this.dgrvProjectInfo.Columns.Add("2 ", "通用名拼音码");
                    this.dgrvProjectInfo.Columns.Add("3 ", "通用名五笔码");
                    this.dgrvProjectInfo.Columns.Add("4 ", "通用名编码");
                    this.dgrvProjectInfo.Columns.Add("5 ", "开始时间");
                    this.dgrvProjectInfo.Columns.Add("6 ", "品名编码");
                    this.dgrvProjectInfo.Columns.Add("7 ", "商品名");
                    this.dgrvProjectInfo.Columns.Add("8 ", "商品名拼音码");
                    this.dgrvProjectInfo.Columns.Add("9 ", "商品名五笔码");
                    this.dgrvProjectInfo.Columns.Add("10", "英文名称");
                    this.dgrvProjectInfo.Columns.Add("11", "收费类别");
                    this.dgrvProjectInfo.Columns.Add("12", "处方药标志");
                    this.dgrvProjectInfo.Columns.Add("13", "收费项目等级");
                    this.dgrvProjectInfo.Columns.Add("14", "标注剂型");
                    this.dgrvProjectInfo.Columns.Add("15", "实际剂型");
                    this.dgrvProjectInfo.Columns.Add("16", "规格");
                    this.dgrvProjectInfo.Columns.Add("17", "单位");
                    this.dgrvProjectInfo.Columns.Add("18", "最高限价");
                    this.dgrvProjectInfo.Columns.Add("19", "门诊自付比例");
                    this.dgrvProjectInfo.Columns.Add("20", "住院自付比例");
                    this.dgrvProjectInfo.Columns.Add("21", "工伤自付比例");
                    this.dgrvProjectInfo.Columns.Add("22", "生育自付比例");
                    this.dgrvProjectInfo.Columns.Add("23", "普通离休自付比例");
                    this.dgrvProjectInfo.Columns.Add("24", "二乙自付比例");
                    this.dgrvProjectInfo.Columns.Add("25", "居民自付比例");
                    this.dgrvProjectInfo.Columns.Add("26", "学生自付比例");
                    this.dgrvProjectInfo.Columns.Add("27", "终止时间");
                    this.dgrvProjectInfo.Columns.Add("28", "产地");
                    this.dgrvProjectInfo.Columns.Add("29", "生产企业");
                    this.dgrvProjectInfo.Columns.Add("30", "批准文号");
                    this.dgrvProjectInfo.Columns.Add("31", "院内制剂标志");
                    this.dgrvProjectInfo.Columns.Add("32", "定点医疗机构编号");
                    this.dgrvProjectInfo.Columns.Add("33", "审批标志");
                    this.dgrvProjectInfo.Columns.Add("34", "有效标志");
                    this.dgrvProjectInfo.Columns.Add("35", "经办人");
                    this.dgrvProjectInfo.Columns.Add("36", "经办日期");
                    this.dgrvProjectInfo.Columns.Add("37", "限中心使用");
                    this.dgrvProjectInfo.Columns.Add("38", "限门诊使用");
                    this.dgrvProjectInfo.Columns.Add("39", "特药标志");
                    this.dgrvProjectInfo.Columns.Add("40", "招标价格");
                    this.dgrvProjectInfo.Columns.Add("41", "地市级离休自付比例");
                    #endregion
                    break;
                case "02":
                    #region 诊疗项目目录
                    this.dgrvProjectInfo.Columns.Add("0 ", "项目编码");
                    this.dgrvProjectInfo.Columns.Add("1 ", "开始时间");
                    this.dgrvProjectInfo.Columns.Add("2 ", "项目名称");
                    this.dgrvProjectInfo.Columns.Add("3 ", "费用类别");
                    this.dgrvProjectInfo.Columns.Add("4 ", "收费项目等级");
                    this.dgrvProjectInfo.Columns.Add("5 ", "拼音助记码");
                    this.dgrvProjectInfo.Columns.Add("6 ", "五笔助记码");
                    this.dgrvProjectInfo.Columns.Add("7 ", "门诊自付比例");
                    this.dgrvProjectInfo.Columns.Add("8 ", "住院自付比例");
                    this.dgrvProjectInfo.Columns.Add("9 ", "工伤自付比例");
                    this.dgrvProjectInfo.Columns.Add("10", "生育自付比例");
                    this.dgrvProjectInfo.Columns.Add("11", "普通离休自付比例");
                    this.dgrvProjectInfo.Columns.Add("12", "二乙自付比例");
                    this.dgrvProjectInfo.Columns.Add("13", "居民自付比例");
                    this.dgrvProjectInfo.Columns.Add("14", "学生自付比例");
                    this.dgrvProjectInfo.Columns.Add("15", "终止时间");
                    this.dgrvProjectInfo.Columns.Add("16", "规格");
                    this.dgrvProjectInfo.Columns.Add("17", "审批标志");
                    this.dgrvProjectInfo.Columns.Add("18", "经办人");
                    this.dgrvProjectInfo.Columns.Add("19", "经办日期");
                    this.dgrvProjectInfo.Columns.Add("20", "有效标志");
                    this.dgrvProjectInfo.Columns.Add("21", "限中心使用");
                    this.dgrvProjectInfo.Columns.Add("22", "省统一编码");
                    this.dgrvProjectInfo.Columns.Add("23", "国产进口标志");
                    this.dgrvProjectInfo.Columns.Add("24", "地市级离休自付比例");
                    this.dgrvProjectInfo.Columns.Add("25", "一级医院限价");
                    this.dgrvProjectInfo.Columns.Add("26", "二级医院限价");
                    this.dgrvProjectInfo.Columns.Add("27", "三级医院限价");
                    this.dgrvProjectInfo.Columns.Add("28", "医院等级");
                    this.dgrvProjectInfo.Columns.Add("29", "备注");
                    #endregion
                    break;
                case "03":
                    #region 费用类别
                    this.dgrvProjectInfo.Columns.Add("0 ", "费用类别编码");
                    this.dgrvProjectInfo.Columns.Add("1 ", "费用类别名称");
                    this.dgrvProjectInfo.Columns.Add("2 ", "收费大类编码");
                    this.dgrvProjectInfo.Columns.Add("3 ", "收费大类名称");
                    #endregion
                    break;
                case "04":
                    #region 病种目录
                    this.dgrvProjectInfo.Columns.Add("0", "疾病编码");
                    this.dgrvProjectInfo.Columns.Add("1", "疾病种类");
                    this.dgrvProjectInfo.Columns.Add("2", "病种分类");
                    this.dgrvProjectInfo.Columns.Add("3", "疾病名称");
                    this.dgrvProjectInfo.Columns.Add("4", "拼音助记码");
                    this.dgrvProjectInfo.Columns.Add("5", "五笔助记码");
                    this.dgrvProjectInfo.Columns.Add("6", "有效标志");
                    this.dgrvProjectInfo.Columns.Add("7", "经办人");
                    this.dgrvProjectInfo.Columns.Add("8", "经办时间");
                    this.dgrvProjectInfo.Columns.Add("9", "门诊慢性病限额");
                    this.dgrvProjectInfo.Columns.Add("10", "公务员慢性病限额");
                    this.dgrvProjectInfo.Columns.Add("11", "档次(徐州)");
                    this.dgrvProjectInfo.Columns.Add("12", "备注");
                    this.dgrvProjectInfo.Columns.Add("13", "工伤标识");
                    this.dgrvProjectInfo.Columns.Add("14", "生育标识");
                    #endregion
                    break;
                case "05":
                    #region 项目与一次性材料
                    this.dgrvProjectInfo.Columns.Add("0 ", "项目编码");
                    this.dgrvProjectInfo.Columns.Add("1 ", "项目名称");
                    this.dgrvProjectInfo.Columns.Add("2 ", "材料编码");
                    this.dgrvProjectInfo.Columns.Add("3 ", "材料名称");
                    this.dgrvProjectInfo.Columns.Add("4", "有效标志");
                    this.dgrvProjectInfo.Columns.Add("5 ", "开始日期");
                    this.dgrvProjectInfo.Columns.Add("6 ", "终止日期");
                    #endregion
                    break;
                case "06":
                    #region 项目对照信息
                    this.dgrvProjectInfo.Columns.Add("0", "唯一索引");
                    this.dgrvProjectInfo.Columns.Add("1", "中心项目编码");
                    this.dgrvProjectInfo.Columns.Add("2", "中心项目名称");
                    this.dgrvProjectInfo.Columns.Add("3", "收费类别");
                    this.dgrvProjectInfo.Columns.Add("4", "定点医疗机构编码");
                    this.dgrvProjectInfo.Columns.Add("5", "定点医疗机构项目编码");
                    this.dgrvProjectInfo.Columns.Add("6", "定点医疗机构项目名称");
                    this.dgrvProjectInfo.Columns.Add("7", "定点医疗机构药品剂型");
                    this.dgrvProjectInfo.Columns.Add("8", "收费项目种类");
                    this.dgrvProjectInfo.Columns.Add("9", "单位");
                    this.dgrvProjectInfo.Columns.Add("10", "规格");
                    this.dgrvProjectInfo.Columns.Add("11", "审核标志");
                    this.dgrvProjectInfo.Columns.Add("12", "审核时间");
                    this.dgrvProjectInfo.Columns.Add("13", "上传时间");
                    #endregion
                    break;
            }
        }
        #endregion


        /// <summary>
        /// 东软服务调用
        /// </summary>
        public string[] Handle()
        { 
            string input = neusoft.GetInputPara();
            string otherParam = "";
            string cardinfo = ""; 

             
            StringBuilder output = new StringBuilder(3000);
            int result = neusoftHandle.BusinessHandle(input, output);
            //int result = neusoftHandle.BusinessHandle_EX(input, otherParam, cardinfo, output);

            if (result != 0)
            {
                throw new Exception("东软调用失败，错误提示：" + output.ToString());
            }
            return neusoft.ResolveOutput(output.ToString());
        }

        // 下载 按钮事件
        private void btnQuery_Click(object sender, EventArgs e)
        {
            //this.cmbProject.Tag = this.cmbProject.SelectedValue;
            //this.setDataGridView(this.cmbProject.SelectedValue.ToString());
            try
            {
                //下载目录
                neusoft = new NeusoftResolver();//1601 1300
                neusoft.InitResolver("1300");
                neusoft.AddInParas(this.cmbProject.SelectedValue.ToString());//项目编码 
                /*项目编码 范围
                01	药品目录
                02	诊疗项目信息
                03	医疗服务设施信息
                04	费用类别信息
                05	病种信 息
                06	住院和慢性病审批信息
                07	特检特治特药审批信息
                08	医院医师信息
                09	对照审批信息下载
                10	门诊慢性病治疗方案明细下载
                11	定点医疗机构与统筹区对应关系下载
                12	外地医院信息*/
                DateTime dd = DateTime.Parse(dptStartDate.Text);//DateTime.Parse("2014-01-01");
                neusoft.AddInParas(dd.ToString("yyyyMMddHHmmss"));//开始日期
                neusoft.AddInParas("");//个人编号      
                var result = Handle();
                //MessageBox.Show(neusoft.ListOutParas[0].ToString());
                string filePathName = neusoft.ListOutParas[0].ToString();
                string fileName = ""; //.txt名
                string fileName_txt = ""; //_txt名
                //清空 dgrvProjectInfo 数据 

                dgrvProjectInfo.DataSource = null;
               // dgrvProjectInfo.Rows.Clear();

                //根据前台选择的项目类型编码,将文件名赋值 文件命名规则,由于原采用oledb模式,所以有下面分支语句
                switch (this.cmbProject.SelectedValue.ToString())
                {
                    case "01":      //01：YPML_下载数据开始日期；
                        fileName = "YPML_" + dd.ToString("yyyyMMddHHmmss");
                        break;
                    case "02":      //02：ZLXM_下载数据开始日期；
                        fileName = "ZLXM_" + dd.ToString("yyyyMMddHHmmss");
                        break;
                    case "03":      //03：FWSS_下载数据开始日期；
                        fileName = "FWSS_" + dd.ToString("yyyyMMddHHmmss");
                        break;
                    case "04":      //04：SFXMBM_下载数据开始日期；
                        fileName = "SFXMBM_" + dd.ToString("yyyyMMddHHmmss");
                        break;
                    case "05":      //05：BZML_下载数据开始日期；
                        fileName = "BZML_" + dd.ToString("yyyyMMddHHmmss");
                        break;
                    case "06":      //06：SPXX_下载数据开始日期；
                        fileName = "SPXX_" + dd.ToString("yyyyMMddHHmmss")+".txt";
                        fileName_txt = "SPXX_" + dd.ToString("yyyyMMddHHmmss") + "_txt";
                        break;
                    case "07":      //07：TJTZ_下载数据开始日期；
                        fileName = "TJTZ_" + dd.ToString("yyyyMMddHHmmss");
                        break;
                    case "08":      //08：YSXX_下载数据开始日期；
                        fileName = "FYMX_" + dd.ToString("yyyyMMddHHmmss");
                        fileName_txt = "SPXX_" + dd.ToString("yyyyMMddHHmmss") + "_txt";
                        break;
                    case "09":      //09：DZXX_下载数据开始日期；
                        fileName = "DZXX_" + dd.ToString("yyyyMMddHHmmss");
                        break;
                    case "10":      //10: MXBXX_下载数据开始日期；
                        fileName = "MXBXX_" + dd.ToString("yyyyMMddHHmmss");
                        break;
                    case "11":      //11: DDTCQ_下载数据开始日期；
                        fileName = "DDTCQ_" + dd.ToString("yyyyMMddHHmmss");
                        break;
                    case "12":      //12：WDYYXX_下载数据开始日期；
                        fileName = "WDYYXX_" + dd.ToString("yyyyMMddHHmmss");
                        break;
                }

                StreamReader sr = new StreamReader(filePathName, System.Text.Encoding.Default);
                //String text = sr.ReadToEnd(); //全部读取 
                DataTable kk = new DataTable();

                kk.Columns.Add(new DataColumn("SPBH", typeof(string)));//[SPBH] [nvarchar](18) NULL,
                kk.Columns.Add(new DataColumn("GRBH", typeof(string)));//[GRBH] [nvarchar](20) NOT NULL,
                kk.Columns.Add(new DataColumn("SPLB", typeof(string)));//[SPLB] [nvarchar](3) NULL,
                kk.Columns.Add(new DataColumn("KSSJ", typeof(string)));//[KSSJ] [varchar](100) NULL,
                kk.Columns.Add(new DataColumn("DWBH", typeof(string)));//[DWBH] [varchar](14) NULL,
                kk.Columns.Add(new DataColumn("DWMC", typeof(string)));//[DWMC] [varchar](100) NULL,
                kk.Columns.Add(new DataColumn("SFZH", typeof(string)));//[SFZH] [varchar](20) NULL,
                kk.Columns.Add(new DataColumn("XM", typeof(string)));//[XM] [varchar](20) NULL,
                kk.Columns.Add(new DataColumn("SPBZ", typeof(string)));//[SPBZ] [varchar](3) NULL,
                kk.Columns.Add(new DataColumn("SPJZYYBH", typeof(string)));//[SPJZYYBH] [varchar](20) NULL,
                kk.Columns.Add(new DataColumn("SPJZYYMC", typeof(string)));//[SPJZYYMC] [varchar](50) NULL,
                kk.Columns.Add(new DataColumn("BZBM", typeof(string)));//[BZBM] [varchar](20) NULL,
                kk.Columns.Add(new DataColumn("BZMC", typeof(string)));//[BZMC] [varchar](100) NULL,
                kk.Columns.Add(new DataColumn("YYJDYJ", typeof(string)));//[YYJDYJ] [varchar](100) NULL,
                kk.Columns.Add(new DataColumn("YYJDRQ", typeof(string)));//[YYJDRQ] [datetime] NULL,
                kk.Columns.Add(new DataColumn("SBRQ", typeof(string)));//[SBRQ] [datetime] NULL,
                kk.Columns.Add(new DataColumn("YBJGSPYJ", typeof(string)));//[YBJGSPYJ] [varchar](100) NULL,
                kk.Columns.Add(new DataColumn("SPRQ", typeof(string)));//[SPRQ] [datetime] NULL,
                kk.Columns.Add(new DataColumn("TCXE", typeof(string)));//[TCXE] [decimal](12, 2) NULL,
                kk.Columns.Add(new DataColumn("YYDJ", typeof(string)));//[YYDJ] [varchar](3) NULL,
                kk.Columns.Add(new DataColumn("ZZSJ", typeof(string)));//[ZZSJ] [datetime] NULL,
                kk.Columns.Add(new DataColumn("JDYSBH", typeof(string)));//[JDYSBH] [varchar](20) NULL,
                kk.Columns.Add(new DataColumn("JDYYBH", typeof(string)));//[JDYYBH] [varchar](20) NULL,
                kk.Columns.Add(new DataColumn("JDYYXM", typeof(string)));//[JDYYXM] [varchar](20) NULL,
                kk.Columns.Add(new DataColumn("JJZJDBH", typeof(string)));//[JJZJDBH] [varchar](20) NULL,
                kk.Columns.Add(new DataColumn("ZJZJDMC", typeof(string)));//[ZJZJDMC] [varchar](20) NULL,
                kk.Columns.Add(new DataColumn("DYSHR", typeof(string)));//[DYSHR] [varchar](20) NULL,
                kk.Columns.Add(new DataColumn("DESHR", typeof(string)));//[DESHR] [varchar](20) NULL,
                kk.Columns.Add(new DataColumn("ZZSHR", typeof(string)));//[ZZSHR] [varchar](20) NULL,
                kk.Columns.Add(new DataColumn("SBLY", typeof(string)));//[SBLY] [varchar](3) NULL,
                kk.Columns.Add(new DataColumn("SBJGBM", typeof(string)));//[SBJGBM] [varchar](20) NULL,
                kk.Columns.Add(new DataColumn("ZWLB", typeof(string)));//[ZWLB] [varchar](3) NULL,
                kk.Columns.Add(new DataColumn("ZJZJDYJ", typeof(string)));//[ZJZJDYJ] [varchar](100) NULL,
                kk.Columns.Add(new DataColumn("ZCYYBH", typeof(string)));//[ZCYYBH] [varchar](20) NULL,
                kk.Columns.Add(new DataColumn("ZCYYMC", typeof(string)));//[ZCYYMC] [varchar](50) NULL,
                kk.Columns.Add(new DataColumn("ZWCSLB", typeof(string)));//[ZWCSLB] [varchar](3) NULL,
                kk.Columns.Add(new DataColumn("ZWCSMC", typeof(string)));//[ZWCSMC] [varchar](100) NULL,
                kk.Columns.Add(new DataColumn("JDYYMC", typeof(string)));//[JDYYMC] [varchar](100) NULL,
                kk.Columns.Add(new DataColumn("SBJGMC", typeof(string)));//[SBJGMC] [varchar](100) NULL,
                kk.Columns.Add(new DataColumn("JBR", typeof(string)));//[JBR] [varchar](20) NULL,
                kk.Columns.Add(new DataColumn("JBRQ", typeof(string)));//[JBRQ] [datetime] NULL,
                kk.Columns.Add(new DataColumn("BZ", typeof(string)));//[BZ] [varchar](100) NULL

                DataRow newkkrow;
                string[] k2 = new string[42];
                while (!sr.EndOfStream)
                {
                    string lineText = sr.ReadLine();//逐行读取
                    //lineText.Split(k2,41,StringSplitOptions.None);
                    k2 = lineText.Split('\t');
                    newkkrow = kk.NewRow();
                    newkkrow["SPBH"] = k2[0];
                    newkkrow["GRBH"] = k2[1];
                    newkkrow["SPLB"] = k2[2];
                    newkkrow["KSSJ"] = k2[3];
                    newkkrow["DWBH"] = k2[4];
                    newkkrow["DWMC"] = k2[5];
                    newkkrow["SFZH"] = k2[6];
                    newkkrow["XM"] = k2[7];
                    newkkrow["SPBZ"] = k2[8];
                    newkkrow["SPJZYYBH"] = k2[9];
                    newkkrow["SPJZYYMC"] = k2[10];
                    newkkrow["BZBM"] = k2[11];
                    newkkrow["BZMC"] = k2[12];
                    newkkrow["YYJDYJ"] = k2[13];
                    newkkrow["YYJDRQ"] = k2[14];
                    newkkrow["SBRQ"] = k2[15];
                    newkkrow["YBJGSPYJ"] = k2[16];
                    newkkrow["SPRQ"] = k2[17];
                    newkkrow["TCXE"] = k2[18];
                    newkkrow["YYDJ"] = k2[19];
                    newkkrow["ZZSJ"] = k2[20];
                    newkkrow["JDYSBH"] = k2[21];
                    newkkrow["JDYYBH"] = k2[22];
                    newkkrow["JDYYXM"] = k2[23];
                    newkkrow["JJZJDBH"] = k2[24];
                    newkkrow["ZJZJDMC"] = k2[25];
                    newkkrow["DYSHR"] = k2[26];
                    newkkrow["DESHR"] = k2[27];
                    newkkrow["ZZSHR"] = k2[28];
                    newkkrow["SBLY"] = k2[29];
                    newkkrow["SBJGBM"] = k2[30];
                    newkkrow["ZWLB"] = k2[31];
                    newkkrow["ZJZJDYJ"] = k2[32];
                    newkkrow["ZCYYBH"] = k2[33];
                    newkkrow["ZCYYMC"] = k2[34];
                    newkkrow["ZWCSLB"] = k2[35];
                    newkkrow["ZWCSMC"] = k2[36];
                    newkkrow["JDYYMC"] = k2[37];
                    newkkrow["SBJGMC"] = k2[38];
                    newkkrow["JBR"] = k2[39];
                    newkkrow["JBRQ"] = k2[40];
                    newkkrow["BZ"] = k2[41];
                    kk.Rows.Add(newkkrow);
                }
                //填充dgrvProjectInfo
                dgrvProjectInfo.DataSource = kk;

                //原有
                //JSNeusoftHandle handleModel = new JSNeusoftHandle(\"1300\");
                //handleModel.NeusoftInit();
                //handleModel.AddListInParas(this.cmbProject.SelectedValue.ToString());
                //handleModel.AddListInParas(this.dptStartDate.Value.ToString("yyyyMMddHHmm"));

                //选择的不是全部
                //if (cmbAuditFlag.SelectedIndex != 3)
                //{
                //handleModel.AddListInParas(this.cmbAuditFlag.SelectedValue.ToString());
                //handleModel.AddListInParas("");
                //}

                //bool result = handleModel.NeusoftHandle();
                //if (result)
                //{
                //    string path = handleModel.ListOutParas[0];
                //    handleModel.TxtToDataGridView(path, this.dgrvProjectInfo);
                //}
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }








        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #region 保存数据到本地
        /// <summary>
        /// 保存下载的药品和诊疗目录到本地数据库，供费用目录对应时使用
        /// </summary>
        /// <param name="style">“药品”或者“诊疗”</param>
        /// <returns></returns>
 //       private int SavaProjectInfoToSQLite(string style)
 //       {
 //           int CountOfInserted = 0;
 //           SQLiteHelper sqliteDBhelper = new SQLiteHelper(RIConfig.DBFilePath);
 //           StringBuilder strSQL;
 //           StringBuilder strDBSql;

 //           switch (style)
 //           {
 //               case "药品":
 //                   sqliteDBhelper.BeginTran();
 //                   foreach (DataGridViewRow dgrvr in this.dgrvProjectInfo.Rows)
 //                   {
 //                       strSQL = new StringBuilder();
 //                       strSQL.Append("delete from network_drugs_dictionary where drug_class_id=2 and drug_id='" + dgrvr.Cells[0].Value + "';");
 //                       strSQL.Append(" insert into network_drugs_dictionary values(");
 //                       strSQL.Append("'" + dgrvr.Cells[0].Value + "',");//DRUG_ID
 //                       strSQL.Append("'" + dgrvr.Cells[0].Value.ToString().Replace("'", "") + "',");//DRUG_CODE
 //                       strSQL.Append("'" + dgrvr.Cells[7].Value.ToString().Replace("'", "") + "',");//DRUG_TRADE_NAME
 //                       strSQL.Append("'" + dgrvr.Cells[1].Value.ToString().Replace("'", "") + "',");//DRUG_COMM_NAME
 //                       strSQL.Append("'" + dgrvr.Cells[2].Value + "',");//DRUG_SPELLING
 //                       strSQL.Append("'" + dgrvr.Cells[16].Value + "',");//DRUG_SPEC
 //                       strSQL.Append("'" + dgrvr.Cells[15].Value + "',");//DRUG_FORM
 //                       strSQL.Append("'" + dgrvr.Cells[17].Value + "',");//DRUG_UNIT
 //                       strSQL.Append("'" + dgrvr.Cells[13].Value + "',");//DRUG_REIM_LEVEL
 //                       strSQL.Append("0,'" + dgrvr.Cells[11].Value.ToString().Trim() + "',2,");//,DRUG_STOP,DRUG_MOD_LASTTIME,DRUG_CLASS_ID     DRUG_MOD_LASTTIME 作为 收费类别
 //                       strSQL.Append("'" + dgrvr.Cells[29].Value.ToString().Replace("'", "") + "',");//DRUG_COMPANY
 //                       strSQL.Append("'" + dgrvr.Cells[18].Value + "')");//DRUG_PRICE
 //                       sqliteDBhelper.ExecSqlReInt(strSQL.ToString());
 //                       CountOfInserted++;
 //                       #region zan shi zuo fei
 //                       //strDBSql=new StringBuilder();
 //                       //strDBSql.Append("delete from REPORT.dbo.YB_XMML;");
 //                       //strDBSql.Append("INSERT INTO REPORT.dbo.YB_XMML VALUES ");
 //                       //strDBSql.Append("'" + dgrvr.Cells[0].Value + "',");//XMBM
 //                       //strDBSql.Append("'" + dgrvr.Cells[1].Value + "',");//XMMC
 //                       //strDBSql.Append("'" + dgrvr.Cells[2].Value + "',");//PYM
 //                       //strDBSql.Append("'" + dgrvr.Cells[3].Value + "',");//五笔码
 //                       //strDBSql.Append("'" + dgrvr.Cells[4].Value + "',");//通用名编码
 //                       //strDBSql.Append("'" + dgrvr.Cells[5].Value + "',");//开始时间
 //                       //strDBSql.Append("'" + dgrvr.Cells[6].Value + "',");//品名编码
 //                       //strDBSql.Append("'" + dgrvr.Cells[7].Value + "',");//商品名
 //                       //strDBSql.Append("'" + dgrvr.Cells[8].Value + "',");//商品名拼音码
 //                       //strDBSql.Append("'" + dgrvr.Cells[9].Value + "',");//商品名五笔码
 //                       //strDBSql.Append("'" + dgrvr.Cells[10].Value + "',");//英文名称
 //                       //strDBSql.Append("'" + dgrvr.Cells[11].Value + "',");//收费类别
 //                       //strDBSql.Append("'" + dgrvr.Cells[12].Value + "',");//处方药标志
 //                       //strDBSql.Append("'" + dgrvr.Cells[13].Value + "',");//收费项目等级
 //                       //strDBSql.Append("'" + dgrvr.Cells[14].Value + "',");//标注剂型
 //                       //strDBSql.Append("'" + dgrvr.Cells[15].Value + "',");//实际剂型
 //                       //strDBSql.Append("'" + dgrvr.Cells[16].Value + "',");//规格
 //                       //strDBSql.Append("'" + dgrvr.Cells[17].Value + "',");//单位
 //                       //strDBSql.Append("'" + dgrvr.Cells[18].Value + "',");//最高限价
 //                       //strDBSql.Append("'" + dgrvr.Cells[19].Value + "',");//门诊自付比例
 //                       //strDBSql.Append("'" + dgrvr.Cells[20].Value + "',");//住院自付比例
 //                       //strDBSql.Append("'" + dgrvr.Cells[21].Value + "',");//工伤自付比例
 //                       //strDBSql.Append("'" + dgrvr.Cells[22].Value + "',");//生育自付比例
 //                       //strDBSql.Append("'" + dgrvr.Cells[23].Value + "',");//普通离休自付比例
 //                       //strDBSql.Append("'" + dgrvr.Cells[24].Value + "',");//二乙自付比例
 //                       //strDBSql.Append("'" + dgrvr.Cells[25].Value + "',");//居民自付比例
 //                       //strDBSql.Append("'" + dgrvr.Cells[26].Value + "',");//学生自付比例
 //                       //strDBSql.Append("'" + dgrvr.Cells[27].Value + "',");//终止时间
 //                       //strDBSql.Append("'" + dgrvr.Cells[28].Value + "',");//产地
 //                       //strDBSql.Append("'" + dgrvr.Cells[29].Value + "',");//生产企业
 //                       //strDBSql.Append("'" + dgrvr.Cells[30].Value + "',");//批准文号
 //                       //strDBSql.Append("'" + dgrvr.Cells[31].Value + "',");//院内制剂标志
 //                       //strDBSql.Append("'" + dgrvr.Cells[32].Value + "',");//定点医疗机构编号
 //                       //strDBSql.Append("'" + dgrvr.Cells[33].Value + "',");//审批标志
 //                       //strDBSql.Append("'" + dgrvr.Cells[34].Value + "',");//有效标志
 //                       //strDBSql.Append("'" + dgrvr.Cells[35].Value + "',");//经办人
 //                       //strDBSql.Append("'" + dgrvr.Cells[36].Value + "',");//经办日期
 //                       //strDBSql.Append("'" + dgrvr.Cells[37].Value + "',");//限中心使用
 //                       //strDBSql.Append("'" + dgrvr.Cells[38].Value + "',");//限门诊使用
 //                       //strDBSql.Append("'" + dgrvr.Cells[39].Value + "',");//特药标志
 //                       //strDBSql.Append("'" + dgrvr.Cells[40].Value + "',");//招标价格
 //                       //strDBSql.Append("'" + dgrvr.Cells[41].Value + "',");//地市级离休自付比例
 //                       //strDBSql.Append("'',");//省统一编码
 //                       //strDBSql.Append("'',");//国产进口标志
 //                       //strDBSql.Append("'',");//一级医院限价
 //                       //strDBSql.Append("'',");//二级医院限价
 //                       //strDBSql.Append("'',");//三级医院限价
 //                       //strDBSql.Append("'',");//医院等级
 //                       //strDBSql.Append("'',");//备注
 //                       //strDBSql.Append("'1',");//药品标志
 //                       //strDBSql.Append("'"+DateTime.Now.ToString("yyyyMMddHHmmss")+"'");//下载时间
 //                       //sqlHelper.ExecSqlReInt(strDBSql.ToString());
	//#endregion
 //                   } 
 //                   sqliteDBhelper.CommitTran();
 //                   MessageBox.Show("成功下载" + CountOfInserted.ToString() + "数据!");
 //                   break;
 //               case "诊疗":
 //                   sqliteDBhelper.BeginTran();
 //                   foreach (DataGridViewRow dgrvr in this.dgrvProjectInfo.Rows)
 //                   {
 //                       strSQL = new StringBuilder();
 //                       strSQL.Append("delete from NETWORK_DIAGNOSIS_DICTIONARY where  DIAGNO_CLASS_ID=2 and DIAGNO_ID='" + dgrvr.Cells[0].Value + "';");
 //                       strSQL.Append(" insert into NETWORK_DIAGNOSIS_DICTIONARY values(");
 //                       strSQL.Append("'" + dgrvr.Cells[0].Value + "',");//DIAGNO_ID
 //                       strSQL.Append("'" + dgrvr.Cells[0].Value + "',");//DIAGNO_CODE
 //                       strSQL.Append("'" + dgrvr.Cells[2].Value.ToString().Replace("'","") + "',");//DIAGNO_NAME
 //                       strSQL.Append("'" + dgrvr.Cells[5].Value + "',");//DIAGNO_SPELLING
 //                       strSQL.Append("'" + dgrvr.Cells[3].Value + "',");//DIAGNO_CHARGE_STYPE
 //                       strSQL.Append("'',");//DIAGNO_UNIT
 //                       strSQL.Append("'" + dgrvr.Cells[4].Value + "',");//DIAGNO_REIM_LEVEL
 //                       strSQL.Append("'" + dgrvr.Cells[15].Value + "',");//DIAGNO_STOP
 //                       strSQL.Append("'" + dgrvr.Cells[25].Value + "',");//DIAGNO_HIGHEST_PRICE1
 //                       strSQL.Append("'" + dgrvr.Cells[26].Value + "',");//DIAGNO_HIGHEST_PRICE2
 //                       strSQL.Append("'" + dgrvr.Cells[27].Value + "',");//DIAGNO_HIGHEST_PRICE3
 //                       strSQL.Append("'" + dgrvr.Cells[19].Value + "',");//DIAGNO_MOD_LASTTIME
 //                       strSQL.Append("2)");//DIAGNO_CLASS_ID
 //                       sqliteDBhelper.ExecSqlReInt(strSQL.ToString());
 //                       CountOfInserted++;
 //                       #region zan shi zuo fei
 //                       //strDBSql = new StringBuilder();
 //                       //strDBSql.Append("delete from REPORT.dbo.YB_XMML;");
 //                       //strDBSql.Append("INSERT INTO REPORT.dbo.YB_XMML VALUES ");
 //                       //strDBSql.Append("'" + dgrvr.Cells[0].Value + "',");//XMBM
 //                       //strDBSql.Append("'" + dgrvr.Cells[2].Value + "',");//XMMC
 //                       //strDBSql.Append("'" + dgrvr.Cells[5].Value + "',");//PYM
 //                       //strDBSql.Append("'" + dgrvr.Cells[6].Value + "',");//五笔码
 //                       //strDBSql.Append("'',");//通用名编码
 //                       //strDBSql.Append("'" + dgrvr.Cells[1].Value + "',");//开始时间
 //                       //strDBSql.Append("'',");//品名编码
 //                       //strDBSql.Append("'',");//商品名
 //                       //strDBSql.Append("'',");//商品名拼音码
 //                       //strDBSql.Append("'',");//商品名五笔码
 //                       //strDBSql.Append("'',");//英文名称
 //                       //strDBSql.Append("'" + dgrvr.Cells[3].Value + "',");//收费类别
 //                       //strDBSql.Append("'',");//处方药标志
 //                       //strDBSql.Append("'" + dgrvr.Cells[4].Value + "',");//收费项目等级
 //                       //strDBSql.Append("'',");//标注剂型
 //                       //strDBSql.Append("'',");//实际剂型
 //                       //strDBSql.Append("'" + dgrvr.Cells[16].Value + "',");//规格
 //                       //strDBSql.Append("'',");//单位
 //                       //strDBSql.Append("'',");//最高限价
 //                       //strDBSql.Append("'" + dgrvr.Cells[7].Value + "',");//门诊自付比例
 //                       //strDBSql.Append("'" + dgrvr.Cells[8].Value + "',");//住院自付比例
 //                       //strDBSql.Append("'" + dgrvr.Cells[9].Value + "',");//工伤自付比例
 //                       //strDBSql.Append("'" + dgrvr.Cells[10].Value + "',");//生育自付比例
 //                       //strDBSql.Append("'" + dgrvr.Cells[11].Value + "',");//普通离休自付比例
 //                       //strDBSql.Append("'" + dgrvr.Cells[12].Value + "',");//二乙自付比例
 //                       //strDBSql.Append("'" + dgrvr.Cells[13].Value + "',");//居民自付比例
 //                       //strDBSql.Append("'" + dgrvr.Cells[14].Value + "',");//学生自付比例
 //                       //strDBSql.Append("'" + dgrvr.Cells[15].Value + "',");//终止时间
 //                       //strDBSql.Append("'',");//产地
 //                       //strDBSql.Append("'',");//生产企业
 //                       //strDBSql.Append("'',");//批准文号
 //                       //strDBSql.Append("'',");//院内制剂标志
 //                       //strDBSql.Append("'',");//定点医疗机构编号
 //                       //strDBSql.Append("'" + dgrvr.Cells[17].Value + "',");//审批标志
 //                       //strDBSql.Append("'" + dgrvr.Cells[20].Value + "',");//有效标志
 //                       //strDBSql.Append("'" + dgrvr.Cells[18].Value + "',");//经办人
 //                       //strDBSql.Append("'" + dgrvr.Cells[19].Value + "',");//经办日期
 //                       //strDBSql.Append("'" + dgrvr.Cells[21].Value + "',");//限中心使用
 //                       //strDBSql.Append("'',");//限门诊使用
 //                       //strDBSql.Append("'',");//特药标志
 //                       //strDBSql.Append("'',");//招标价格
 //                       //strDBSql.Append("'" + dgrvr.Cells[24].Value + "',");//地市级离休自付比例
 //                       //strDBSql.Append("'" + dgrvr.Cells[22].Value + "',");//省统一编码
 //                       //strDBSql.Append("'" + dgrvr.Cells[23].Value + "',");//国产进口标志
 //                       //strDBSql.Append("'" + dgrvr.Cells[25].Value + "',");//一级医院限价
 //                       //strDBSql.Append("'" + dgrvr.Cells[26].Value + "',");//二级医院限价
 //                       //strDBSql.Append("'" + dgrvr.Cells[27].Value + "',");//三级医院限价
 //                       //strDBSql.Append("'" + dgrvr.Cells[28].Value + "',");//医院等级
 //                       //strDBSql.Append("'" + dgrvr.Cells[29].Value + "',");//备注
 //                       //strDBSql.Append("'0',");//药品标志
 //                       //strDBSql.Append("'" + DateTime.Now.ToString("yyyyMMddHHmmss") + "'");//下载时间
 //                       //sqlHelper.ExecSqlReInt(strDBSql.ToString()); 
 //                       #endregion
 //                   }
 //                   sqliteDBhelper.CommitTran();
 //                   MessageBox.Show("成功插入" + CountOfInserted.ToString() + "数据!");
 //                   break;
 //               case "费用类别目录":
 //                   sqliteDBhelper.BeginTran();
 //                   foreach (DataGridViewRow dgrvr in this.dgrvProjectInfo.Rows)
 //                   {
 //                       strSQL = new StringBuilder();
 //                       strSQL.Append(" delete from network_charge_class where  CLASS_COMPANY='2' and CLASS_CODE='" + dgrvr.Cells[0].Value + "';");
 //                       strSQL.Append("  insert into network_charge_class values(");
 //                       strSQL.Append("'" + dgrvr.Cells[0].Value + "',");//CLASS_CODE
 //                       strSQL.Append("'" + dgrvr.Cells[1].Value.ToString().Replace("'","") + "',");//CLASS_NAME
 //                       strSQL.Append("'" + 2 + "',");//CLASS_COMPANY
 //                       strSQL.Append("'','','')");//MEMO1,MEMO2,MEMO3
 //                       sqliteDBhelper.ExecSqlReInt(strSQL.ToString());
 //                       CountOfInserted++;
 //                   }
 //                   sqliteDBhelper.CommitTran();
 //                   MessageBox.Show("成功插入" + CountOfInserted.ToString() + "数据!");
 //                   break;
 //               case "病种信息":
 //                   sqliteDBhelper.BeginTran();
 //                   foreach (DataGridViewRow dgrvr in dgrvProjectInfo.Rows)
 //                   {
 //                       strSQL = new StringBuilder();
 //                       strSQL.Append("delete from network_diagnosis where insurance_type='1' and diagnosis_code='" + dgrvr.Cells[0].Value.ToString().Trim() + "';");
 //                       strSQL.Append(" insert into network_diagnosis(diagnosis_code,diagnosis_name,input_code,full_code,flag_invalid,order_no,insurance_type) values(");
 //                       strSQL.Append("'" + dgrvr.Cells[0].Value.ToString().Trim() + "',"); //diagnosis_code
 //                       strSQL.Append("'" + dgrvr.Cells[3].Value.ToString().Trim() + "',"); //diagnosis_name
 //                       strSQL.Append("'" + dgrvr.Cells[4].Value.ToString().Trim() + "',"); //input_code
 //                       strSQL.Append("'',");                                               //FULL_CODE
 //                       strSQL.Append("'0',");                                              //FLAG_INALID
 //                       strSQL.Append("'" + (CountOfInserted + 1) + "',");                  //order_no
 //                       strSQL.Append("'1')");                                              //insurance_type
 //                       sqliteDBhelper.ExecSqlReInt(strSQL.ToString());
 //                       CountOfInserted++;
 //                   }
 //                   sqliteDBhelper.CommitTran();
 //                   MessageBox.Show("成功保存" + CountOfInserted.ToString() + "数据!");
 //                   break;
 //           }
 //           return CountOfInserted;
 //       }
        #endregion

        #region 保存数据到服务器
        /// <summary>
        /// 保存下载的药品和诊疗目录到服务器数据库，供费用目录对应时使用
        /// </summary>
        /// <param name="style">“药品”或者“诊疗”</param>
        /// <returns></returns>
        private int SavaProjectInfoToSever(string style)
        {
            int CountOfInserted = 0;
            StringBuilder strSQL;
            StringBuilder strDBSql;

            switch (style)
            {

                case "01":     //01	药品目录
                    foreach (DataGridViewRow dgrvr in this.dgrvProjectInfo.Rows)
                    {
                        sqlHelper.BeginTran();
                        CountOfInserted++;
                        #region 存储到服务器 药品中心目录 区分医院
                        strDBSql = new StringBuilder();
                        strDBSql.Append("delete from REPORT.dbo.NETWORK_DRUGS_DICTIONARY where D1='" + dgrvr.Cells[0].Value.ToString().Trim() + "' and HOSPITAL_ID='" + _hosID + "';");
                        strDBSql.Append("INSERT INTO REPORT.dbo.NETWORK_DRUGS_DICTIONARY VALUES (");
                        strDBSql.Append("'" + dgrvr.Cells[0].Value.ToString().Trim() + "',");//XMBM
                        strDBSql.Append("'" + dgrvr.Cells[1].Value.ToString().Trim() + "',");//XMMC
                        strDBSql.Append("'" + dgrvr.Cells[2].Value.ToString().Trim() + "',");//PYM
                        strDBSql.Append("'" + dgrvr.Cells[3].Value.ToString().Trim() + "',");//五笔码
                        strDBSql.Append("'" + dgrvr.Cells[4].Value.ToString().Trim() + "',");//通用名编码
                        strDBSql.Append("'" + dgrvr.Cells[5].Value.ToString().Trim() + "',");//开始时间
                        strDBSql.Append("'" + dgrvr.Cells[6].Value.ToString().Trim() + "',");//品名编码
                        strDBSql.Append("'" + dgrvr.Cells[7].Value.ToString().Trim() + "',");//商品名
                        strDBSql.Append("'" + dgrvr.Cells[8].Value.ToString().Trim() + "',");//商品名拼音码
                        strDBSql.Append("'" + dgrvr.Cells[9].Value.ToString().Trim() + "',");//商品名五笔码
                        strDBSql.Append("'" + dgrvr.Cells[10].Value.ToString().Trim() + "',");//英文名称
                        strDBSql.Append("'" + dgrvr.Cells[11].Value.ToString().Trim() + "',");//收费类别
                        strDBSql.Append("'" + dgrvr.Cells[12].Value.ToString().Trim() + "',");//处方药标志
                        strDBSql.Append("'" + dgrvr.Cells[13].Value.ToString().Trim() + "',");//收费项目等级
                        strDBSql.Append("'" + dgrvr.Cells[14].Value.ToString().Trim() + "',");//标注剂型
                        strDBSql.Append("'" + dgrvr.Cells[15].Value.ToString().Trim() + "',");//实际剂型
                        strDBSql.Append("'" + dgrvr.Cells[16].Value.ToString().Trim() + "',");//规格
                        strDBSql.Append("'" + dgrvr.Cells[17].Value.ToString().Trim() + "',");//单位
                        strDBSql.Append("'" + dgrvr.Cells[18].Value.ToString().Trim() + "',");//最高限价
                        strDBSql.Append("'" + dgrvr.Cells[19].Value.ToString().Trim() + "',");//门诊自付比例
                        strDBSql.Append("'" + dgrvr.Cells[20].Value.ToString().Trim() + "',");//住院自付比例
                        strDBSql.Append("'" + dgrvr.Cells[21].Value.ToString().Trim() + "',");//工伤自付比例
                        strDBSql.Append("'" + dgrvr.Cells[22].Value.ToString().Trim() + "',");//生育自付比例
                        strDBSql.Append("'" + dgrvr.Cells[23].Value.ToString().Trim() + "',");//普通离休自付比例
                        strDBSql.Append("'" + dgrvr.Cells[24].Value.ToString().Trim() + "',");//二乙自付比例
                        strDBSql.Append("'" + dgrvr.Cells[25].Value.ToString().Trim() + "',");//居民自付比例
                        strDBSql.Append("'" + dgrvr.Cells[26].Value.ToString().Trim() + "',");//学生自付比例
                        strDBSql.Append("'" + dgrvr.Cells[27].Value.ToString().Trim() + "',");//终止时间
                        strDBSql.Append("'" + dgrvr.Cells[28].Value.ToString().Trim() + "',");//产地
                        strDBSql.Append("'" + dgrvr.Cells[29].Value.ToString().Trim() + "',");//生产企业
                        strDBSql.Append("'" + dgrvr.Cells[30].Value.ToString().Trim() + "',");//批准文号
                        strDBSql.Append("'" + dgrvr.Cells[31].Value.ToString().Trim() + "',");//院内制剂标志
                        strDBSql.Append("'" + dgrvr.Cells[32].Value.ToString().Trim() + "',");//定点医疗机构编号
                        strDBSql.Append("'" + dgrvr.Cells[33].Value.ToString().Trim() + "',");//审批标志
                        strDBSql.Append("'" + dgrvr.Cells[34].Value.ToString().Trim() + "',");//有效标志
                        strDBSql.Append("'" + dgrvr.Cells[35].Value.ToString().Trim() + "',");//经办人
                        strDBSql.Append("'" + dgrvr.Cells[36].Value.ToString().Trim() + "',");//经办日期
                        strDBSql.Append("'" + dgrvr.Cells[37].Value.ToString().Trim() + "',");//限中心使用
                        strDBSql.Append("'" + dgrvr.Cells[38].Value.ToString().Trim() + "',");//限门诊使用
                        strDBSql.Append("'" + dgrvr.Cells[39].Value.ToString().Trim() + "',");//特药标志
                        strDBSql.Append("'" + dgrvr.Cells[40].Value.ToString().Trim() + "',");//招标价格
                        strDBSql.Append("'" + dgrvr.Cells[41].Value.ToString().Trim() + "',");//地市级离休自付比例
                        strDBSql.Append("'" + DateTime.Now.ToString("yyyy-MM-dd") + "',");//下载时间
                        strDBSql.Append("'" + _hosID + "')");//医院编码
                        //MessageBox.Show(strDBSql.ToString());
                        sqlHelper.ExecSqlReInt(strDBSql.ToString());
                        sqlHelper.CommitTran();
                        #endregion
                    }
                    MessageBox.Show("成功下载" + CountOfInserted.ToString() + "数据!");
                    break;
                case "02":      //02	诊疗项目信息
                    foreach (DataGridViewRow dgrvr in this.dgrvProjectInfo.Rows)
                    {
                        sqlHelper.BeginTran();
                        CountOfInserted++;
                        #region 存储到服务器 诊疗中心目录  区分医院
                        strDBSql = new StringBuilder();
                        strDBSql.Append("delete from REPORT.dbo.NETWORK_DIAGNOSIS_DICTIONARY where D1 ='" + dgrvr.Cells[0].Value.ToString().Trim() + "' and HOSPITAL_ID='" + _hosID + "';");
                        strDBSql.Append("INSERT INTO REPORT.dbo.NETWORK_DIAGNOSIS_DICTIONARY VALUES (");
                        strDBSql.Append("'" + dgrvr.Cells[0].Value.ToString().Trim() + "',");//XMBM
                        strDBSql.Append("'" + dgrvr.Cells[1].Value.ToString().Trim() + "',");//XMMC
                        strDBSql.Append("'" + dgrvr.Cells[2].Value.ToString().Trim() + "',");//PYM
                        strDBSql.Append("'" + dgrvr.Cells[3].Value.ToString().Trim() + "',");//五笔码
                        strDBSql.Append("'" + dgrvr.Cells[4].Value.ToString().Trim() + "',");//开始时间
                        strDBSql.Append("'" + dgrvr.Cells[5].Value.ToString().Trim() + "',");//收费类别
                        strDBSql.Append("'" + dgrvr.Cells[6].Value.ToString().Trim() + "',");//收费项目等级
                        strDBSql.Append("'" + dgrvr.Cells[7].Value.ToString().Trim() + "',");//规格
                        strDBSql.Append("'" + dgrvr.Cells[8].Value.ToString().Trim() + "',");//门诊自付比例
                        strDBSql.Append("'" + dgrvr.Cells[9].Value.ToString().Trim() + "',");//住院自付比例
                        strDBSql.Append("'" + dgrvr.Cells[10].Value.ToString().Trim() + "',");//工伤自付比例
                        strDBSql.Append("'" + dgrvr.Cells[11].Value.ToString().Trim() + "',");//生育自付比例
                        strDBSql.Append("'" + dgrvr.Cells[12].Value.ToString().Trim() + "',");//普通离休自付比例
                        strDBSql.Append("'" + dgrvr.Cells[13].Value.ToString().Trim() + "',");//二乙自付比例
                        strDBSql.Append("'" + dgrvr.Cells[14].Value.ToString().Trim() + "',");//居民自付比例
                        strDBSql.Append("'" + dgrvr.Cells[15].Value.ToString().Trim() + "',");//学生自付比例
                        strDBSql.Append("'" + dgrvr.Cells[16].Value.ToString().Trim() + "',");//终止时间
                        strDBSql.Append("'" + dgrvr.Cells[17].Value.ToString().Trim() + "',");//审批标志
                        strDBSql.Append("'" + dgrvr.Cells[28].Value.ToString().Trim() + "',");//有效标志
                        strDBSql.Append("'" + dgrvr.Cells[19].Value.ToString().Trim() + "',");//经办人
                        strDBSql.Append("'" + dgrvr.Cells[20].Value.ToString().Trim() + "',");//经办日期
                        strDBSql.Append("'" + dgrvr.Cells[21].Value.ToString().Trim() + "',");//限中心使用
                        strDBSql.Append("'" + dgrvr.Cells[22].Value.ToString().Trim() + "',");//地市级离休自付比例
                        strDBSql.Append("'" + dgrvr.Cells[23].Value.ToString().Trim() + "',");//省统一编码
                        strDBSql.Append("'" + dgrvr.Cells[24].Value.ToString().Trim() + "',");//国产进口标志
                        strDBSql.Append("'" + dgrvr.Cells[25].Value.ToString().Trim() + "',");//一级医院限价
                        strDBSql.Append("'" + dgrvr.Cells[26].Value.ToString().Trim() + "',");//二级医院限价
                        strDBSql.Append("'" + dgrvr.Cells[27].Value.ToString().Trim() + "',");//三级医院限价
                        strDBSql.Append("'" + dgrvr.Cells[28].Value.ToString().Trim() + "',");//医院等级
                        strDBSql.Append("'" + dgrvr.Cells[29].Value.ToString().Trim() + "',");//备注
                        strDBSql.Append("'" + DateTime.Now.ToString("yyyy-MM-dd") + "',");//下载时间
                        strDBSql.Append("'" + _hosID + "')");//医院编码
                        //MessageBox.Show(strDBSql.ToString());
                        sqlHelper.ExecSqlReInt(strDBSql.ToString());
                        #endregion
                        sqlHelper.CommitTran();
                    }
                    MessageBox.Show("成功插入" + CountOfInserted.ToString() + "数据!");
                    break;
                case "03":      //03	医疗服务设施信息
                    //SavaProjectInfoToSQLite("药品");
                    //SavaProjectInfoToSever("药品");
                    break;
                case "04":      //04	费用类别信息
                    foreach (DataGridViewRow dgrvr in this.dgrvProjectInfo.Rows)
                    {
                        sqlHelper.BeginTran();
                        strSQL = new StringBuilder();
                        //不区分医院
                        #region
                        strSQL.Append(" delete from REPORT.dbo.NETWORK_CHARGE_TYPE where D1='" + dgrvr.Cells[0].Value.ToString().Trim() + "';");
                        strSQL.Append("  INSERT INTO REPORT.dbo.NETWORK_CHARGE_TYPE VALUES(");
                        strSQL.Append("'" + dgrvr.Cells[0].Value.ToString().Trim() + "',");//费用类别编码
                        strSQL.Append("'" + dgrvr.Cells[1].Value.ToString().Replace("'", "").Trim() + "',");//费用类别名称
                        strSQL.Append("'" + dgrvr.Cells[2].Value.ToString().Trim() + "',");//收费大类编码
                        strSQL.Append("'" + dgrvr.Cells[3].Value.ToString().Trim() + "',");//收费大类名称
                        strSQL.Append("'" + DateTime.Now.ToString("yyyy-MM-dd") + "')");//下载时间
                        sqlHelper.ExecSqlReInt(strSQL.ToString());
                        CountOfInserted++;
                        sqlHelper.CommitTran();
                        #endregion
                    }
                    MessageBox.Show("成功插入" + CountOfInserted.ToString() + "数据!");
                    break;
                case "05":      //05	病种信息
                    foreach (DataGridViewRow dgrvr in dgrvProjectInfo.Rows)
                    {
                        sqlHelper.BeginTran();
                        strSQL = new StringBuilder();
                        //1是医保 101是农合
                        #region
                        strSQL.Append("delete from REPORT.dbo.NETWORK_DIAGNOSIS where INSURANCE_TYPE='1' and D1='" + dgrvr.Cells[0].Value.ToString().Trim() + "';");
                        strSQL.Append(" INSERT INTO REPORT.dbo.NETWORK_DIAGNOSIS VALUES(");
                        strSQL.Append("'" + dgrvr.Cells[0].Value.ToString().Trim() + "',"); //
                        strSQL.Append("'" + dgrvr.Cells[1].Value.ToString().Trim() + "',"); //
                        strSQL.Append("'" + dgrvr.Cells[2].Value.ToString().Trim() + "',"); //
                        strSQL.Append("'" + dgrvr.Cells[3].Value.ToString().Trim() + "',"); //
                        strSQL.Append("'" + dgrvr.Cells[4].Value.ToString().Trim() + "',");
                        strSQL.Append("'" + dgrvr.Cells[5].Value.ToString().Trim() + "',"); //
                        strSQL.Append("'" + dgrvr.Cells[6].Value.ToString().Trim() + "',"); //
                        strSQL.Append("'" + dgrvr.Cells[7].Value.ToString().Trim() + "',"); //
                        strSQL.Append("'" + dgrvr.Cells[8].Value.ToString().Trim() + "',"); //
                        strSQL.Append("'" + dgrvr.Cells[9].Value.ToString().Trim() + "',"); //
                        strSQL.Append("'" + dgrvr.Cells[10].Value.ToString().Trim() + "',"); //
                        strSQL.Append("'" + dgrvr.Cells[11].Value.ToString().Trim() + "',"); //
                        strSQL.Append("'" + dgrvr.Cells[12].Value.ToString().Trim() + "',"); //
                        strSQL.Append("'" + dgrvr.Cells[13].Value.ToString().Trim() + "',"); //
                        strSQL.Append("'" + dgrvr.Cells[14].Value.ToString().Trim() + "',"); //
                        strSQL.Append("'',"); //
                        strSQL.Append("'',"); //
                        //strSQL.Append("'" + dgrvr.Cells[17].Value.ToString().Trim() + "',"); //
                        strSQL.Append("'1',"); //
                        strSQL.Append("'" + DateTime.Now.ToString("yyyy-MM-dd") + "')"); //
                        sqlHelper.ExecSqlReInt(strSQL.ToString());
                        CountOfInserted++;
                        sqlHelper.CommitTran();
                        #endregion
                    }
                    MessageBox.Show("成功保存" + CountOfInserted.ToString() + "数据!");
                    break;
                case "06":      //06	住院和慢性病审批信息
                   foreach (DataGridViewRow dgrvr in this.dgrvProjectInfo.Rows)
                    {
                        //sqlHelper.BeginTran();
                        CountOfInserted++;
                        #region 存储到服务器 药品中心目录
                        strDBSql = new StringBuilder();
                        strDBSql.Append("delete from REPORT.dbo.Net_PAT_INFO where SPBH='" + dgrvr.Cells[0].Value.ToString().Trim() + "'");

                        strDBSql.Append(" INSERT INTO REPORT.dbo.Net_PAT_INFO VALUES (");
                        strDBSql.Append("'" + dgrvr.Cells[0].Value.ToString().Trim() + "',");//审批编号
                        strDBSql.Append("'" + dgrvr.Cells[1].Value.ToString().Trim() + "',");//个人编号
                        strDBSql.Append("'" + dgrvr.Cells[2].Value.ToString().Trim() + "',");//审批类别
                        strDBSql.Append("'" + dgrvr.Cells[3].Value.ToString().Trim() + "',");//开始时间
                        strDBSql.Append("'" + dgrvr.Cells[4].Value.ToString().Trim() + "',");//单位编号
                        strDBSql.Append("'" + dgrvr.Cells[5].Value.ToString().Trim() + "',");//单位名称
                        strDBSql.Append("'" + dgrvr.Cells[6].Value.ToString().Trim() + "',");//身份证号
                        strDBSql.Append("'" + dgrvr.Cells[7].Value.ToString().Trim() + "',");//姓名
                        strDBSql.Append("'" + dgrvr.Cells[8].Value.ToString().Trim() + "',");//审批标志
                        strDBSql.Append("'" + dgrvr.Cells[9].Value.ToString().Trim() + "',");//审批就诊医院编号
                        strDBSql.Append("'" + dgrvr.Cells[10].Value.ToString().Trim() + "',");//审批就诊医院名称
                        strDBSql.Append("'" + dgrvr.Cells[11].Value.ToString().Trim() + "',");//病种编码
                        strDBSql.Append("'" + dgrvr.Cells[12].Value.ToString().Trim() + "',");//病种名称
                        strDBSql.Append("'" + dgrvr.Cells[13].Value.ToString().Trim() + "',");//医院鉴定意见
                        strDBSql.Append("'" + dgrvr.Cells[14].Value.ToString().Trim() + "',");//医院鉴定日期
                        strDBSql.Append("'" + dgrvr.Cells[15].Value.ToString().Trim() + "',");//申报日期
                        strDBSql.Append("'" + dgrvr.Cells[16].Value.ToString().Trim() + "',");//医保机构审批意见
                        strDBSql.Append("'" + dgrvr.Cells[17].Value.ToString().Trim() + "',");//审批日期
                        strDBSql.Append("'" + dgrvr.Cells[18].Value.ToString().Trim() + "',");//统筹限额
                        strDBSql.Append("'" + dgrvr.Cells[19].Value.ToString().Trim() + "',");//医院等级
                        strDBSql.Append("'" + dgrvr.Cells[20].Value.ToString().Trim() + "',");//终止时间
                        strDBSql.Append("'" + dgrvr.Cells[21].Value.ToString().Trim() + "',");//鉴定医师编号
                        strDBSql.Append("'" + dgrvr.Cells[22].Value.ToString().Trim() + "',");//鉴定医院编号
                        strDBSql.Append("'" + dgrvr.Cells[23].Value.ToString().Trim() + "',");//鉴定医师姓名
                        strDBSql.Append("'" + dgrvr.Cells[24].Value.ToString().Trim() + "',");//专家组鉴定编号
                        strDBSql.Append("'" + dgrvr.Cells[25].Value.ToString().Trim() + "',");//专家组鉴定名称
                        strDBSql.Append("'" + dgrvr.Cells[26].Value.ToString().Trim() + "',");//第一审核人
                        strDBSql.Append("'" + dgrvr.Cells[27].Value.ToString().Trim() + "',");//第二审核人
                        strDBSql.Append("'" + dgrvr.Cells[28].Value.ToString().Trim() + "',");//最终审核人
                        strDBSql.Append("'" + dgrvr.Cells[29].Value.ToString().Trim() + "',");//申报来源
                        strDBSql.Append("'" + dgrvr.Cells[30].Value.ToString().Trim() + "',");//申报机构编码
                        strDBSql.Append("'" + dgrvr.Cells[31].Value.ToString().Trim() + "',");//转外类别
                        strDBSql.Append("'" + dgrvr.Cells[32].Value.ToString().Trim() + "',");//专家组鉴定意见
                        strDBSql.Append("'" + dgrvr.Cells[33].Value.ToString().Trim() + "',");//转出医院编号
                        strDBSql.Append("'" + dgrvr.Cells[34].Value.ToString().Trim() + "',");//转出医院名称
                        strDBSql.Append("'" + dgrvr.Cells[35].Value.ToString().Trim() + "',");//转外城市类别
                        strDBSql.Append("'" + dgrvr.Cells[36].Value.ToString().Trim() + "',");//转外城市名称
                        strDBSql.Append("'" + dgrvr.Cells[37].Value.ToString().Trim() + "',");//鉴定医院名称
                        strDBSql.Append("'" + dgrvr.Cells[38].Value.ToString().Trim() + "',");//申报机构名称
                        strDBSql.Append("'" + dgrvr.Cells[39].Value.ToString().Trim() + "',");//经办人
                        strDBSql.Append("'" + dgrvr.Cells[40].Value.ToString().Trim() + "',");//经办日期
                        strDBSql.Append("'" + dgrvr.Cells[41].Value.ToString().Trim() + "' )");//备注
                        //MessageBox.Show(strDBSql.ToString());
                        sqlHelper.ExecSqlReInt(strDBSql.ToString());
                        //sqlHelper.CommitTran();
                        #endregion
                    }
                    MessageBox.Show("成功保存" + CountOfInserted.ToString() + "数据!");
                    break;
                case "07":      //07	特检特治特药审批信息
                    //SavaProjectInfoToSQLite("药品");
                    //SavaProjectInfoToSever("药品");
                    break;
                case "08":      //08	医院医师信息
                    //SavaProjectInfoToSQLite("药品");
                    //SavaProjectInfoToSever("药品");
                    break;
                case "09":      //09	对照审批信息下载
                    //SavaProjectInfoToSQLite("药品");
                    //SavaProjectInfoToSever("药品");
                    break;
                case "10":      //10	门诊慢性病治疗方案明细下载
                    //SavaProjectInfoToSQLite("药品");
                    //SavaProjectInfoToSever("药品");
                    break;
                case "11":      //11	定点医疗机构与统筹区对应关系下载
                    //SavaProjectInfoToSQLite("药品");
                    //SavaProjectInfoToSever("药品");
                    break;
                case "12":      //12	外地医院信息
                    //SavaProjectInfoToSQLite("药品");
                    //SavaProjectInfoToSever("药品");
                    break;
            }
            return CountOfInserted;
        }
        #endregion
        //保存按钮事件,当前只写了慢性病患者信息,并且保存到服务器 
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.dgrvProjectInfo.Rows.Count == 0)
            {
                return;
            }
            try
            {
                switch (this.cmbProject.SelectedValue.ToString())
                {
                    case "01":     //01	药品目录
                        //SavaProjectInfoToSQLite("01");
                        //SavaProjectInfoToSever("01");
                        break;
                    case "02":      //02	诊疗项目信息
                        //SavaProjectInfoToSQLite("药品");
                        //SavaProjectInfoToSever("药品");
                        break;
                    case "03":      //03	医疗服务设施信息
                        //SavaProjectInfoToSQLite("药品");
                        //SavaProjectInfoToSever("药品");
                        break;
                    case "04":      //04	费用类别信息
                        //SavaProjectInfoToSQLite("药品");
                        //SavaProjectInfoToSever("药品");
                        break;
                    case "05":      //05	病种信息
                        //SavaProjectInfoToSQLite("药品");
                        //SavaProjectInfoToSever("药品");
                        break;
                    case "06":      //06	住院和慢性病审批信息
                        //SavaProjectInfoToSQLite("06");
                        SavaProjectInfoToSever("06");
                        break;
                    case "07":      //07	特检特治特药审批信息
                        //SavaProjectInfoToSQLite("药品");
                        //SavaProjectInfoToSever("药品");
                        break;
                    case "08":      //08	医院医师信息
                        //SavaProjectInfoToSQLite("药品");
                        //SavaProjectInfoToSever("药品");
                        break;
                    case "09":      //09	对照审批信息下载
                        //SavaProjectInfoToSQLite("药品");
                        //SavaProjectInfoToSever("药品");
                        break;
                    case "10":      //10	门诊慢性病治疗方案明细下载
                        //SavaProjectInfoToSQLite("药品");
                        //SavaProjectInfoToSever("药品");
                        break;
                    case "11":      //11	定点医疗机构与统筹区对应关系下载
                        //SavaProjectInfoToSQLite("药品");
                        //SavaProjectInfoToSever("药品");
                        break;
                    case "12":      //12	外地医院信息
                        //SavaProjectInfoToSQLite("药品");
                        //SavaProjectInfoToSever("药品");
                        break;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }
        
        /// <summary>
        /// 更新接口对照表
        /// </summary>
        private void WriteNewVs()
        {
            if (cmbAuditFlag.SelectedIndex != 1)
            {
                return;
            }
            StringBuilder strSql;
            sqlHelper.BeginTran();
            foreach (DataGridViewRow dr in dgrvProjectInfo.Rows)
            {
                try
                {
                    strSql = new StringBuilder();
                    strSql.Append("DELETE FROM comm.COMM.NETWORKING_ITEM_VS_HIS WHERE ");
                    strSql.Append(" HIS_ITEM_CODE='" + dr.Cells[5].Value.ToString().Trim() + "' AND HOSPITAL_ID='" + _hosID + "' AND NETWORKING_PAT_CLASS_ID=2;");
                    strSql.Append("INSERT comm.COMM.NETWORKING_ITEM_VS_HIS( NETWORKING_PAT_CLASS_ID ,ITEM_PROP ,HIS_ITEM_CODE ,HIS_ITEM_NAME ,NETWORK_ITEM_CODE ,NETWORK_ITEM_NAME ,SELF_BURDEN_RATIO ,MEMO ,START_TIME ,STOP_TIME ,TYPE_MEMO ,NETWORK_ITEM_PROP ,NETWORK_ITEM_CHARGE_CLASS ,HOSPITAL_ID) VALUES ");
                    strSql.Append(" ('2','0','" + dr.Cells[5].Value.ToString().Trim() + "','");
                    strSql.Append(dr.Cells[6].Value.ToString().Trim().Replace("'", "") + "','");
                    strSql.Append(dr.Cells[1].Value.ToString().Trim() + "','");
                    strSql.Append(dr.Cells[2].Value.ToString().Trim().Replace("'", "") + "','0','','");
                    strSql.Append(dr.Cells[12].Value.ToString().Trim() == "" ? "1990-01-01 00:00:00" : DateTime.ParseExact(dr.Cells[12].Value.ToString().Trim(), "yyyyMMddHHmmss", null).ToString());
                    strSql.Append("','");
                    strSql.Append(dr.Cells[13].Value.ToString().Trim() == "" ? "1990-01-01 00:00:00": DateTime.ParseExact(dr.Cells[13].Value.ToString().Trim(), "yyyyMMddHHmmss", null).ToString() + "','','" + dr.Cells[8].Value.ToString().Trim() + "','"+dr.Cells[3].Value.ToString().Trim()+"','" + _hosID + "')");
                    sqlHelper.ExecSqlReInt(strSql.ToString());
                }
                catch (System.Exception ex)
                {
                    sqlHelper.RollbackTran();
                    MessageBox.Show(ex.Message + "所有操作撤销.");
                    return;
                }
            }

            //分类 1药品 2诊疗
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append(" UPDATE COMM.COMM.NETWORKING_ITEM_VS_HIS SET ITEM_PROP = C.ITEMPROP ,SELF_BURDEN_RATIO = C.selfburdenratio ,TYPE_MEMO = C.typeMemo ");
            strSQL.Append(" FROM COMM.COMM.NETWORKING_ITEM_VS_HIS D ");
            strSQL.Append(" INNER JOIN ");

            strSQL.Append(" (SELECT ( CASE WHEN ( (A.CHARGE_TYPE) < 100 ) THEN '1' ");
            strSQL.Append(" ELSE '2' ");
            strSQL.Append(" END) AS ITEMPROP,B.AUTO_ID,");
            strSQL.Append("CAST(( ISNULL(E.住院自付比例, '0') ) AS FLOAT) * 100 AS selfburdenratio ,( (CASE WHEN CAST(( ISNULL(E.住院自付比例, '0') ) AS FLOAT) = 0 THEN '甲' WHEN CAST(( ISNULL(E.住院自付比例, '0') ) AS FLOAT) = 1 THEN '丙' ELSE '乙' END) ) AS typeMemo");
            strSQL.Append(" FROM comm.COMM.CHARGE_PRICE_ALL_VIEW A ");
            //strSQL.Append(" INNER JOIN comm.COMM.NETWORKING_ITEM_VS_HIS B ON B.NETWORK_ITEM_CODE =CAST(A.CHARGE_ID AS VARCHAR(30)) ");//农合
            strSQL.Append(" INNER JOIN comm.COMM.NETWORKING_ITEM_VS_HIS B ON B.HIS_ITEM_CODE =A.CHARGE_CODE ");                         //医保
            strSQL.Append(" AND B.HOSPITAL_ID='" + _hosID + "' AND NETWORKING_PAT_CLASS_ID=2 ");
            strSQL.Append(" LEFT JOIN REPORT.dbo.YB_XMML E ON E.XMBM = B.NETWORK_ITEM_CODE ");
            strSQL.Append(" WHERE HOSPITAL_ID='" + _hosID + "' AND FLAG_INVALID=0 ");//AND PRICE>=0 
            //strSQL.Append(" GROUP BY B.AUTO_ID ");
            strSQL.Append(" ) C ON C.AUTO_ID=D.AUTO_ID");
            strSQL.Append(" where HOSPITAL_ID='" + _hosID + "' AND NETWORKING_PAT_CLASS_ID=2 and D.ITEM_PROP=0");
            try
            {
                sqlHelper.ExecSqlReInt(strSQL.ToString());
            }
            catch (System.Exception ex)
            {
                sqlHelper.RollbackTran();
                MessageBox.Show(ex.Message + "所有操作撤销.");
                return;
            }
            sqlHelper.CommitTran();
            MessageBox.Show("更新完成");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SearchThisTable(false);
        }

        #region 若查看中心对照目录，显示查询操作区
        private void cmbProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DictionaryEntry dict = (DictionaryEntry)cmbProject.SelectedItem;
            if (dict.Key.ToString().Trim() != "06" && btnSearch.Visible)//中心对照目录下载
            {
               // groupBox1.Height -= tbSearchCode.Height;
                ChangeUIVisible(false);
            }
            else if (dict.Key.ToString().Trim() == "06"&&!btnSearch.Visible)
            {
                //groupBox1.Height += tbSearchCode.Height;
                ChangeUIVisible(true);
            }
        }

        private void ChangeUIVisible(bool bShow)
        {
            btnGoOnSearch.Visible = btnSearch.Visible = tbSearchCode.Visible = tbSearchName.Visible = label4.Visible = label5.Visible = bShow;
            //临时的
            //btnSave.Visible = !bShow;
        } 
        #endregion

        private void button1_Click_2(object sender, EventArgs e)
        {
            SearchThisTable(true);
        }
        int nIndex = -1;//当前选中行的索引
        private void SearchThisTable(bool isGoOn)
        {
            //查找用户输入内容模糊查询到的完整名称和编码之集合
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT * FROM COMM.COMM.CHARGE_PRICE_ALL_VIEW WHERE CHARGE_CODE LIKE '%" + tbSearchCode.Text.Trim() + "%' OR CHARGE_NAME LIKE '%" + tbSearchName.Text.Trim() + "%'");
            DataTable dt = sqlHelper.ExecSqlReDs(strSQL.ToString()).Tables[0];
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("医院项目不存在查询信息");
                return;
            }
            string[] strCodes = new string[dt.Rows.Count],strNames= new string[dt.Rows.Count];
            for (int n = 0; n < dt.Rows.Count;n++ )
            {
                strCodes[n] = dt.Rows[n]["CHARGE_CODE"].ToString();
                strNames[n] = dt.Rows[n]["CHARGE_NAME"].ToString();
            }
            foreach (DataGridViewRow dr in dgrvProjectInfo.Rows)
            {
                if (isGoOn && dr.Index <= nIndex)
                {
                    continue;
                }
                if (ContainStr(strCodes, dr.Cells[5].Value.ToString().Trim()) || ContainStr(strNames, dr.Cells[6].Value.ToString().Trim()))
                {
                    dr.Selected = true;
                    nIndex = dr.Index;
                }
            }
        }
        /// <summary>
        /// 当前行内容是否属于查询内容的一种
        /// </summary>
        /// <param name="strSeaching"></param>
        /// <param name="strNow"></param>
        /// <returns></returns>
        private bool ContainStr(string[] strSeaching,string strNow)
        {
            foreach (string s in strSeaching)
            {
                if (strNow==s)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 查询服务器数据库中保存的已下载内容
        /// </summary>
        private void btnLoaded_Click(object sender, EventArgs e)
        {
            //this.cmbProject.Tag = this.cmbProject.SelectedValue;
            //this.setDataGridView(this.cmbProject.SelectedValue.ToString());
            //try
            //{
            //    dgrvProjectInfo.Rows.Clear();
            //    DictionaryEntry dict = (DictionaryEntry)cmbProject.SelectedItem;
            //    DataTable dtRe = null;
            //    StringBuilder strSQL = new StringBuilder();
            //    switch (dict.Key.ToString().Trim())
            //    {
            //        case "01"://药品目录
            //            strSQL.Append("SELECT  D1 ,D2 ,D3 ,D4 ,D5 , D6 , D7 , D8 , D9 , D10 , D11 , D12 , D13 , D14 , D15 , D16 , D17 , D18 , D19 , D20 , D21 , D22 , D23 , D24 , D25 , D26 , D27 , D28 , D29 , D30 , D31 , D32 , D33 , D34 , D35 , D36 , D37 , D38 , D39 , D40 , D41 , D42 FROM REPORT.dbo.NETWORK_DRUGS_DICTIONARY  where DownLoadTime<='" + Convert.ToDateTime(dptStartDate.Value).ToString("yyyy-MM-dd") + "' and HOSPITAL_ID='" + _hosID + "'");
            //            break;
            //        case "02"://诊疗项目信息
            //            strSQL.Append("SELECT  D1 , D2 , D3 , D4 , D5 , D6 , D7 , D8 , D9 , D10 , D11 , D12 , D13 , D14 , D15 , D16 , D17 , D18 , D19 , D20 , D21 , D22 , D23 , D24 , D25 , D26 , D27 , D28 , D29 , D30 FROM REPORT.dbo.NETWORK_DIAGNOSIS_DICTIONARY where DownloadTime<='" + Convert.ToDateTime(dptStartDate.Value).ToString("yyyy-MM-dd") + "' and HOSPITAL_ID='" + _hosID + "'");
            //            break;
            //        case "03"://费用类别信息
            //            strSQL.Append("SELECT  D1 ,D2 ,D3 ,D4 FROM REPORT.dbo.NETWORK_CHARGE_TYPE");
            //            break;
            //        case "04"://病种信息
            //            strSQL.Append("SELECT D1 , D2 , D3 , D4 , D5 , D6 , D7 , D8 , D9 , D10 , D11 , D12 , D13 , D14 , D15 FROM REPORT.dbo.NETWORK_DIAGNOSIS where INSURANCE_TYPE='1'");
            //            break;
            //        //case "05"://项目和一次性材料对应关系
            //        //    break;
            //        case "06"://项目对照信息
            //            strSQL.Append("SELECT  D1 , D2 , D3 , D4 , D5 , D6 , D7 , D8 , D9 , D10 , D11 , D12 , D13 , D14 FROM REPORT.dbo.NETWORK_CENTER_VS_HIS where hospital_id='" + _hosID + "'");
            //            break;
            //        default:
            //            break;
            //    }
            //    dtRe = sqlHelper.ExecSqlReDs(strSQL.ToString()).Tables[0];
            //    if (dtRe == null&&dtRe.Rows.Count==0)
            //    {
            //        MessageBox.Show("不存在已下载内容!");
            //        return;
            //    }
            //    foreach (DataRow dr in dtRe.Rows)
            //    {
            //        dgrvProjectInfo.Rows.Add(dr.ItemArray);
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

       
        
    }
}

