using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Configuration;
using IReimCompanyInterface;
using RIClientClassLib;
//using RIClientCom;
//using IRCIDareWaySoftModel;



namespace DW_YBBX.ZX_Business
{
    /**
     * 需求his显示 编码，名称，产地，国药准字
     * 
     * 中心   编码，名称，产地，国药准字
     * 
     */
    public partial class catalog_correspond : Form
    {
        //测试库
        //public MSSQLHelpers SQLHelper = new MSSQLHelpers(ConfigurationManager.ConnectionStrings["connStr"].ToString());
        //正式库
        //MSSQLHelper SQLHelper = new MSSQLHelper("Data Source=172.16.170.8;Initial Catalog=comm;Persist Security Info=True;User ID=power;Password=m@ssunsoft009");
        public MSSQLHelpers SQLHelper = new MSSQLHelpers(ConfigurationManager.ConnectionStrings["connStr"].ToString());
        public string fylx = "2";
        public string dzlx = "";

        public catalog_correspond()
        {
            InitializeComponent();
            FrmLoad();

        }
        public catalog_correspond(Form Parent)
        {
            InitializeComponent();
            this.MdiParent = Parent;
            FrmLoad();
        }
        #region 初始化
        /// <summary>
        /// 初始化函数
        /// </summary>
        public void StructMethod()
        {
            //neusoft = new LYNeoSoftHandle();
            //neusoft.NeusoftInit();
        }
        #endregion

        #region 登录加载
        /// <summary>
        /// 初始化界面
        /// </summary>
        private void FrmLoad()
        {
            //费用类型
            ArrayList ArrXmlb = new ArrayList();
            ArrXmlb.Add(new DictionaryEntry("1", "药品"));
            ArrXmlb.Add(new DictionaryEntry("2", "医疗"));
            ArrXmlb.Add(new DictionaryEntry("3", "材料"));

            this.cmb_FYLX.DataSource = ArrXmlb;
            this.cmb_FYLX.ValueMember = "Key";
            this.cmb_FYLX.DisplayMember = "Value";
            this.cmb_FYLX.SelectedValue = "1";

            //对照类型
            ArrayList ArrZt = new ArrayList();
            //ArrZt.Add(new DictionaryEntry("0", "全部"));
            ArrZt.Add(new DictionaryEntry("1", "未对应"));
            ArrZt.Add(new DictionaryEntry("2", "已对应"));
            ArrZt.Add(new DictionaryEntry("3", "全部"));

            this.cmb_DZLX.DataSource = ArrZt;
            this.cmb_DZLX.DisplayMember = "Value";
            this.cmb_DZLX.ValueMember = "Key";
            this.cmb_DZLX.SelectedValue = "1";

            this.btn_Qxdz.Enabled = false;
            this.btn_DZSC.Enabled = false;
        }
        #endregion

        #region 查询更新显示his目录
        /// <summary>
        /// 更新显示his目录
        /// </summary>
        public void GX()
        {

            fylx = cmb_FYLX.SelectedValue.ToString(); //费用类型  1药品 2诊疗 3材料
            dzlx = cmb_DZLX.SelectedValue.ToString();//对照类型
            string hisxm = this.txt_hisxm.Text;
            DataTable dt = new DataTable();
            if (dzlx == "1")
            {
                dt = QueryNotMatch(fylx);
            }
            if (dzlx == "2")
            {
                dt = QueryMatchNotUpLoad(fylx);
            }
            if (dzlx == "3")
            {
                dt = QueryALL(fylx);
            }

        }
        #endregion
        
        #region 未对应编码的项目查询
        /// <summary>
        /// 未对应项目DataTable
        /// </summary>
        /// <returns></returns>
        public DataTable QueryNotMatch(string fylx)
        {
            DataTable dtResult = new DataTable();
            StringBuilder StrSql = new StringBuilder();
            if (fylx == "1")
            {
                StrSql.Append(" SELECT  '' AS 中心项目编码 ,'' AS 中心项目名称 ,MAX(A.DRUG_CODE) AS 编码 , ");
                StrSql.Append(" MAX(B.DRUG_NAME) AS 名称 ,MAX(B.DRUG_SPEC) AS 规格 ,MAX(e.MEASURE_UNIT_NAME) AS 单位 ,  ");
                StrSql.Append(" MAX(F.DRUG_FORM_NAME) AS 剂型  ,MAX('') AS 类别,'' AS 住院自付比例,'' AS 报销等级,MAX(A.RETAIL_PRICE) AS 价格,'' as 中心项目属性, MAX(G.PRODUC_AREA_NAME) as HIS产地, MAX(H.APPROVAL_NO) AS HIS国药,1 as 药品诊疗   ");
                StrSql.Append(" FROM    COMM.COMM.DRUG_PRICE_LIST AS A ");
                StrSql.Append(" LEFT JOIN COMM.dict.PRODUC_AREAS G ON G.PRODUC_AREA_ID = A.FIRM_ID ");
                StrSql.Append(" LEFT JOIN YP.DRUG.DRUG_STOCK H ON H.DRUG_PRICE_ID = A.DRUG_PRICE_ID ");
                StrSql.Append(" LEFT JOIN YP.DRUG.DRUGS AS B ON A.DRUG_ID = B.DRUG_ID ");
                StrSql.Append(" LEFT JOIN COMM.DICT.DRUG_CLASSES AS C ON C.DRUG_CLASS_ID = B.DRUG_CLASS_ID ");
                StrSql.Append(" LEFT JOIN COMM.DICT.MEASURE_UNITS AS E ON B.MEASURE_UNIT_ID=e.MEASURE_UNIT_ID ");
                StrSql.Append(" LEFT JOIN COMM.DICT.DRUG_FORMS AS F ON B.DRUG_FORM_ID=f.DRUG_FORM_ID ");
                StrSql.Append(" WHERE B.CHARGE_TYPE<>1 ");
                StrSql.Append(" AND A.DRUG_CODE NOT IN ( SELECT HIS_ITEM_CODE FROM   COMM.COMM.NETWORKING_ITEM_VS_HIS ");
                StrSql.Append(" WHERE   ITEM_PROP = '1' AND NETWORKING_PAT_CLASS_ID ='3' AND NETWORK_ITEM_CODE<>'' ) ");
                StrSql.Append(" AND A.DRUG_ID > 0AND A.DRUG_CODE <> '' AND B.FLAG_INVALID=0 ");
                StrSql.Append(" AND (B.DRUG_CODE LIKE '" + txt_hisxm.Text.Trim() + "%' OR B.DRUG_NAME LIKE '%" + txt_hisxm.Text.Trim() + "%' OR B.INPUT_CODE LIKE  '" + txt_hisxm.Text.Trim() + "%' OR H.APPROVAL_NO LIKE '%" + txt_hisxm.Text.Trim() + "%') ");
                StrSql.Append(" GROUP BY A.DRUG_CODE ,C.DRUG_CLASS_ID ");
                StrSql.Append(" ORDER BY C.DRUG_CLASS_ID ,A.DRUG_CODE ");
            }
            if (fylx == "2")
            {
                StrSql.Append(" SELECT  '' AS 中心项目编码 ,'' AS 中心项目名称 ,MAX(A.CHARGE_CODE) AS 编码 , ");
                StrSql.Append("  MAX(A.CHARGE_NAME) AS 名称 ,(CASE WHEN MAX(A.SPEC)='' THEN '/' ELSE MAX(A.SPEC) END) AS 规格,MAX(B.MEASURE_UNIT_NAME) AS 单位,MAX('') AS 自付比例,MAX('') AS 报销等级,MAX('') AS 类别,MAX(A.PRICE) AS 价格,'' as 中心项目属性,'' as 是否修改,2 as 药品诊疗  ");
                StrSql.Append(" FROM    COMM.COMM.CHARGE_PRICE AS A  LEFT JOIN COMM.DICT.MEASURE_UNITS AS B ON A.MEASURE_UNIT_ID = B.MEASURE_UNIT_ID ");
                StrSql.Append("WHERE  A.CHARGE_CODE NOT IN ( SELECT   HIS_ITEM_CODE FROM     COMM.COMM.NETWORKING_ITEM_VS_HIS ");
                StrSql.Append(" WHERE   ITEM_PROP = '2' AND NETWORKING_PAT_CLASS_ID ='3'  AND NETWORK_ITEM_CODE<>'' ) ");
                StrSql.Append(" AND A.CHARGE_ID > 0 AND A.CHARGE_CODE <> '' AND A.FLAG_INVALID=0");

                if (txt_hisxm.Text.Trim().ToString() != "")
                {
                    StrSql.Append(" AND (A.CHARGE_CODE LIKE '" + txt_hisxm.Text.Trim() + "%' OR A.CHARGE_NAME LIKE '%" + txt_hisxm.Text.Trim() + "%' OR a.INPUT_CODE LIKE '%" + txt_hisxm.Text.Trim() + "%') ");
                }
                StrSql.Append(" GROUP BY A.CHARGE_CODE ");
            }
            if (fylx == "3")
            {
                StrSql.Append(" SELECT '' AS 中心项目编码, ");
                StrSql.Append(" '' AS 中心项目名称, ");
                StrSql.Append(" MAX(A.DRUG_CODE)AS 编码, ");
                StrSql.Append(" MAX(B.DRUG_NAME) AS 名称, ");
                StrSql.Append(" MAX(B.DRUG_SPEC) AS 规格, ");
                StrSql.Append(" MAX(e.MEASURE_UNIT_NAME) AS 单位, ");
                StrSql.Append(" MAX(F.DRUG_FORM_NAME) AS 剂型, ");
                StrSql.Append(" MAX('') AS 类别, ");
                StrSql.Append(" '' AS 住院自付比例, ");
                StrSql.Append(" '' AS 报销等级, ");
                StrSql.Append(" MAX(A.RETAIL_PRICE) AS 价格, ");
                StrSql.Append(" '' AS 中心项目属性, ");
                StrSql.Append(" MAX(G.PRODUC_AREA_NAME) AS HIS产地, ");
                StrSql.Append(" MAX(H.APPROVAL_NO) AS HIS国药, ");
                StrSql.Append(" 3 AS 药品诊疗 ");
                StrSql.Append(" FROM   COMM.COMM.DRUG_PRICE_LIST AS A ");
                StrSql.Append(" LEFT JOIN COMM.dict.PRODUC_AREAS G ON G.PRODUC_AREA_ID = A.FIRM_ID ");
                StrSql.Append(" LEFT JOIN YP.DRUG.DRUG_STOCK H ON H.DRUG_PRICE_ID = A.DRUG_PRICE_ID ");
                StrSql.Append(" LEFT JOIN YP.DRUG.DRUGS AS B ON A.DRUG_ID = B.DRUG_ID ");
                StrSql.Append(" LEFT JOIN COMM.DICT.DRUG_CLASSES AS C ON C.DRUG_CLASS_ID = B.DRUG_CLASS_ID ");
                StrSql.Append(" LEFT JOIN COMM.DICT.MEASURE_UNITS AS E ON B.MEASURE_UNIT_ID = e.MEASURE_UNIT_ID ");
                StrSql.Append(" LEFT JOIN COMM.DICT.DRUG_FORMS AS F ON B.DRUG_FORM_ID = f.DRUG_FORM_ID ");
                StrSql.Append(" WHERE  B.CHARGE_TYPE = 1 ");
                StrSql.Append(" AND A.DRUG_CODE NOT IN(SELECT HIS_ITEM_CODE ");
                StrSql.Append(" FROM   COMM.COMM.NETWORKING_ITEM_VS_HIS ");
                StrSql.Append(" WHERE  ITEM_PROP = '1' ");
                StrSql.Append(" AND NETWORKING_PAT_CLASS_ID = '3' ");
                StrSql.Append(" AND NETWORK_ITEM_CODE <> '') ");
                StrSql.Append(" AND A.DRUG_ID > 0 ");
                StrSql.Append(" AND A.DRUG_CODE <> '' ");
                StrSql.Append(" AND B.FLAG_INVALID = 0 ");
                //StrSql.Append(" AND B.DRUG_CLASS_ID IN(24, 25, 28, 29, 30, 5, 7, 8) ");
                StrSql.Append(" AND B.DRUG_CLASS_ID not IN(1,2,3) ");
                StrSql.Append(" AND(B.DRUG_CODE LIKE '" + txt_hisxm.Text.Trim() + "%' ");
                StrSql.Append(" OR B.DRUG_NAME LIKE '%" + txt_hisxm.Text.Trim() + "%' ");
                StrSql.Append(" OR B.INPUT_CODE LIKE '" + txt_hisxm.Text.Trim() + "%' ");
                StrSql.Append(" OR H.APPROVAL_NO LIKE '%" + txt_hisxm.Text.Trim() + "%' ");
                StrSql.Append(" ) ");
                StrSql.Append(" GROUP BY A.DRUG_CODE, ");
                StrSql.Append(" C.DRUG_CLASS_ID ");
                StrSql.Append(" ORDER BY C.DRUG_CLASS_ID, ");
                StrSql.Append(" A.DRUG_CODE ");
            }


            DataSet ds = new DataSet();
            DataTable hisdata = new DataTable();
            ds = SQLHelper.ExecSqlReDs(StrSql.ToString());
            hisdata = ds.Tables[0];
            dgv_his.DataSource = hisdata;
            return hisdata;
        }
        #endregion


        #region 已对应编码的目录查询
        /// <summary>
        /// 已对应未上传项目DataTable
        /// </summary>
        /// <returns></returns>
        public DataTable QueryMatchNotUpLoad(string fylx)
        {
            DataTable dtResult = new DataTable();
            StringBuilder StrSql = new StringBuilder();
            if (fylx == "1")
            {

               StrSql.Append(" SELECT ");
   //   StrSql.Append(" [ITEM_PROP]");
               StrSql.Append(" [HIS_ITEM_CODE] as 编码");
               StrSql.Append(" ,[HIS_ITEM_NAME] as 名称 ");
      StrSql.Append("  ,[NETWORK_ITEM_CODE] as  中心码");
    StrSql.Append("    ,[NETWORK_ITEM_NAME] as  中心名称");
     StrSql.Append("   ,[SELF_BURDEN_RATIO] ");
     StrSql.Append("   ,[MEMO]");
     StrSql.Append("   ,[START_TIME]  as  启用时间");
     StrSql.Append("   ,[STOP_TIME]  as  停用时间");
     StrSql.Append("   ,[TYPE_MEMO]");
  //   StrSql.Append("   ,[NETWORK_ITEM_PROP]");
      StrSql.Append("  ,[NETWORK_ITEM_CHARGE_CLASS]");
     StrSql.Append("   ,[HOSPITAL_ID]");
    StrSql.Append("    ,[NETWORK_ITEM_PRICE] as 联网价格");
   //  StrSql.Append("   ,[FLAG_DISABLED]");
 //    StrSql.Append("   ,[NETWORK_ITEM_FLAG_UP]");
   StrSql.Append(" FROM [COMM].[COMM].[NETWORKING_ITEM_VS_HIS] "); 
  
  StrSql.Append("  where  memo in('审批已通过','未审批') and  ITEM_PROP = '1'  ");
  StrSql.Append("  AND (HIS_ITEM_CODE LIKE '%" + txt_hisxm.Text.Trim() + "%' OR HIS_ITEM_NAME LIKE '%" + txt_hisxm.Text.Trim() + "%'  )  ORDER BY HIS_ITEM_CODE ");
              //  //StrSql.Append(" SELECT  MAX(A.NETWORK_ITEM_CODE) AS 中心项目编码 ,MAX(A.NETWORK_ITEM_NAME) AS 中心项目名称,MAX(B.DRUG_CODE) AS 编码 ,MAX(C.DRUG_NAME) AS 名称 ,A.MEMO AS 审批是否通过, ");
              //  //StrSql.Append(" MAX(B.DRUG_SPEC) AS 规格,MAX(e.MEASURE_UNIT_NAME) AS 单位, ");
              //  //StrSql.Append(" MAX(F.DRUG_FORM_NAME) AS 剂型,MAX(A.NETWORK_ITEM_CHARGE_CLASS) AS 类别,cast(MAX(A.SELF_BURDEN_RATIO) as varchar) AS 住院自付比例 ,MAX(A.TYPE_MEMO) AS 报销等级,MAX(B.RETAIL_PRICE) AS 价格,(case when MAX(A.NETWORK_ITEM_PROP)='1' then '药品' when MAX(A.NETWORK_ITEM_PROP)='2' then '诊疗' else '' end) as 中心项目属性,MAX(G.PRODUC_AREA_NAME) as HIS产地, MAX(H.APPROVAL_NO) AS HIS国药 ");
              //  //StrSql.Append(" FROM    COMM.COMM.NETWORKING_ITEM_VS_HIS AS A ");
              //  //StrSql.Append(" INNER JOIN COMM.COMM.DRUG_PRICE_LIST AS B ON A.HIS_ITEM_CODE = B.DRUG_CODE ");
              //  //StrSql.Append(" LEFT JOIN COMM.dict.PRODUC_AREAS G ON G.PRODUC_AREA_ID = B.FIRM_ID ");
              //  //StrSql.Append(" LEFT JOIN YP.DRUG.DRUG_STOCK H ON H.DRUG_PRICE_ID = B.DRUG_PRICE_ID ");
              //  //StrSql.Append(" LEFT JOIN YP.DRUG.DRUGS AS C ON B.DRUG_ID = C.DRUG_ID ");
              //  //StrSql.Append(" LEFT JOIN COMM.DICT.DRUG_CLASSES AS D ON C.DRUG_CLASS_ID = D.DRUG_CLASS_ID ");
              //  //StrSql.Append(" LEFT JOIN COMM.DICT.MEASURE_UNITS AS E ON c.MEASURE_UNIT_ID=e.MEASURE_UNIT_ID ");
              //  //StrSql.Append(" LEFT JOIN COMM.DICT.DRUG_FORMS AS F ON c.DRUG_FORM_ID=f.DRUG_FORM_ID ");
              //  //StrSql.Append(" WHERE C.CHARGE_TYPE<>1 ");
              //  //StrSql.Append(" AND A.ITEM_PROP='1' AND C.FLAG_INVALID=0 AND NETWORKING_PAT_CLASS_ID ='3' AND A.NETWORK_ITEM_CODE<>''  AND (A.HIS_ITEM_CODE LIKE '" + txt_hisxm.Text.Trim() + "%' OR A.HIS_ITEM_NAME LIKE '%" + txt_hisxm.Text.Trim() + "%' OR C.INPUT_CODE LIKE '%" + txt_hisxm.Text.Trim() + "%' ) ");
              //  //StrSql.Append(" GROUP BY A.HIS_ITEM_CODE ");
              //  //StrSql.Append(" ORDER BY A.HIS_ITEM_CODE ");
              //  StrSql.Append(" SELECT  MAX(A.NETWORK_ITEM_CODE) AS 中心项目编码 ,");
              //  StrSql.Append(" MAX(A.NETWORK_ITEM_NAME) AS 中心项目名称 ,");
              //  StrSql.Append(" MAX(A.HIS_ITEM_CODE) AS 编码 ,");
              //  StrSql.Append(" MAX(B.DRUG_CODE) AS 编码 ,");
              //  StrSql.Append(" MAX(C.DRUG_NAME) AS 名称 ,");
              //  StrSql.Append("MAX(A.MEMO) AS 审批是否通过 ,");
              //  StrSql.Append("MAX(A.NETWORK_ITEM_CHARGE_CLASS) AS 类别 ,");
              //  StrSql.Append("CAST(MAX(A.SELF_BURDEN_RATIO) AS VARCHAR) AS 自付比例 ,");
              //  StrSql.Append(" MAX(A.TYPE_MEMO) AS 报销等级 ,");
              //  StrSql.Append("MAX(B.RETAIL_PRICE) AS 价格 ,");
              //  StrSql.Append("MAX(B.DRUG_SPEC) AS 规格 ,");
              //  StrSql.Append("MAX(e.MEASURE_UNIT_NAME) AS 单位 ,");
              //  StrSql.Append("MAX(F.DRUG_FORM_NAME) AS 剂型 ,");
              //  StrSql.Append("( CASE WHEN MAX(A.NETWORK_ITEM_PROP) = '1' THEN '药品'");
              //  StrSql.Append("       WHEN MAX(A.NETWORK_ITEM_PROP) = '2' THEN '诊疗'");
              //  StrSql.Append("       ELSE ''");
              //  StrSql.Append("  END ) AS 中心项目属性");
              //  StrSql.Append("  FROM    COMM.COMM.NETWORKING_ITEM_VS_HIS A");
              //  StrSql.Append(" INNER JOIN COMM.COMM.DRUG_PRICE_LIST AS B ON SUBSTRING(A.HIS_ITEM_CODE,2, 50) = B.DRUG_CODE");
              //  StrSql.Append(" LEFT JOIN YP.DRUG.DRUGS AS C ON B.DRUG_ID = C.DRUG_ID");
              //  StrSql.Append(" LEFT JOIN COMM.dict.PRODUC_AREAS G ON G.PRODUC_AREA_ID = B.FIRM_ID");
              //  StrSql.Append(" LEFT JOIN COMM.DICT.MEASURE_UNITS AS E ON c.MEASURE_UNIT_ID = e.MEASURE_UNIT_ID");
              //  StrSql.Append(" LEFT JOIN COMM.DICT.DRUG_FORMS AS F ON c.DRUG_FORM_ID = f.DRUG_FORM_ID");
              //  StrSql.Append(" WHERE   C.CHARGE_TYPE <> 1");
              //  StrSql.Append(" AND A.ITEM_PROP = '1'");
              //  StrSql.Append(" AND C.FLAG_INVALID = 0");
              //  StrSql.Append(" AND A.NETWORKING_PAT_CLASS_ID = '3'");
              //  StrSql.Append(" AND A.NETWORK_ITEM_CODE <> ''");
              //  StrSql.Append(" AND (A.HIS_ITEM_CODE LIKE '" + txt_hisxm.Text.Trim() + "%' OR A.HIS_ITEM_NAME LIKE '%" + txt_hisxm.Text.Trim() + "%' OR C.INPUT_CODE LIKE '%" + txt_hisxm.Text.Trim() + "%' )");
              //  //StrSql.Append(" AND ( A.HIS_ITEM_CODE LIKE '%'");
              //  //StrSql.Append("     OR A.HIS_ITEM_NAME LIKE '%%'");
              //  //StrSql.Append("     OR C.INPUT_CODE LIKE '%%'");
              ////  StrSql.Append(" )");
              //  StrSql.Append(" GROUP BY A.HIS_ITEM_CODE");
              //  StrSql.Append(" ORDER BY A.HIS_ITEM_CODE");
            }
            if (fylx == "2")
            {
                StrSql.Append(" SELECT ");
                //   StrSql.Append(" [ITEM_PROP]");
                StrSql.Append(" [HIS_ITEM_CODE] as  编码");
                StrSql.Append(" ,[HIS_ITEM_NAME] as  名称");
                StrSql.Append("  ,[NETWORK_ITEM_CODE] as  中心码");
                StrSql.Append("    ,[NETWORK_ITEM_NAME] as  中心名称");
                StrSql.Append("   ,[SELF_BURDEN_RATIO] ");
                StrSql.Append("   ,[MEMO]");
                StrSql.Append("   ,[START_TIME]  as  启用时间");
                StrSql.Append("   ,[STOP_TIME]  as  停用时间");
                StrSql.Append("   ,[TYPE_MEMO]");
                //   StrSql.Append("   ,[NETWORK_ITEM_PROP]");
                StrSql.Append("  ,[NETWORK_ITEM_CHARGE_CLASS]");
                StrSql.Append("   ,[HOSPITAL_ID]");
                StrSql.Append("    ,[NETWORK_ITEM_PRICE] as 联网价格");
                //  StrSql.Append("   ,[FLAG_DISABLED]");
                //    StrSql.Append("   ,[NETWORK_ITEM_FLAG_UP]");
                StrSql.Append(" FROM [COMM].[COMM].[NETWORKING_ITEM_VS_HIS] ");

                StrSql.Append("  where  memo in('审批已通过','未审批')  ");
                StrSql.Append("  AND (HIS_ITEM_CODE LIKE '%" + txt_hisxm.Text.Trim() + "%' OR HIS_ITEM_NAME LIKE '%" + txt_hisxm.Text.Trim() + "%'  )  ORDER BY HIS_ITEM_CODE ");
                //旧
                //StrSql.Append(" SELECT  MAX(A.NETWORK_ITEM_CODE) AS 中心项目编码 ,MAX(A.NETWORK_ITEM_NAME) AS 中心项目名称 , ");
                //StrSql.Append(" MAX(A.HIS_ITEM_CODE) AS 编码 ,MAX(A.HIS_ITEM_NAME) AS 名称 ,A.MEMO AS 审批是否通过, ");
                //StrSql.Append(" (CASE WHEN MAX(B.SPEC)='' THEN '/' ELSE MAX(B.SPEC) END) AS 规格 ,MAX(C.MEASURE_UNIT_NAME) AS 单位 , ");
                //StrSql.Append("  cast(MAX(A.SELF_BURDEN_RATIO) as varchar) AS 自付比例 ,MAX(A.TYPE_MEMO) AS 报销等级,MAX(A.NETWORK_ITEM_CHARGE_CLASS) AS 类别,MAX(B.PRICE) AS 价格,(case when MAX(A.NETWORK_ITEM_PROP)='1' then '药品' when MAX(A.NETWORK_ITEM_PROP)='2' then '诊疗' else '' end) as 中心项目属性,'' as 是否修改  ");
                //StrSql.Append(" FROM    COMM.COMM.NETWORKING_ITEM_VS_HIS AS A ");
                //StrSql.Append("INNER JOIN COMM.COMM.CHARGE_PRICE AS B ON A.HIS_ITEM_CODE = B.CHARGE_CODE  LEFT JOIN COMM.DICT.MEASURE_UNITS AS C ON B.MEASURE_UNIT_ID = C.MEASURE_UNIT_ID ");
                //StrSql.Append(" WHERE  A.ITEM_PROP = '2' AND NETWORKING_PAT_CLASS_ID ='3' AND NETWORK_ITEM_CODE<>'' AND B.FLAG_INVALID=0    ");
                //if (txt_hisxm.Text.Trim().ToString() != "")
                //{
                //    StrSql.Append("  AND ( A.HIS_ITEM_CODE LIKE '" + txt_hisxm.Text.Trim() + "%' OR A.HIS_ITEM_NAME LIKE '%" + txt_hisxm.Text.Trim() + "%' OR B.INPUT_CODE LIKE '%" + txt_hisxm.Text.Trim() + "%' ) ");
                //}
                //StrSql.Append(" GROUP BY A.HIS_ITEM_CODE ORDER BY A.HIS_ITEM_CODE ");
            }

            if (fylx == "3")
            {
                //StrSql.Append(" SELECT  MAX(A.NETWORK_ITEM_CODE) AS 中心项目编码 ,MAX(A.NETWORK_ITEM_NAME) AS 中心项目名称,MAX(B.DRUG_CODE) AS 编码 ,MAX(C.DRUG_NAME) AS 名称 ,A.MEMO AS 审批是否通过, ");
                //StrSql.Append(" MAX(B.DRUG_SPEC) AS 规格,MAX(e.MEASURE_UNIT_NAME) AS 单位, ");
                //StrSql.Append(" MAX(F.DRUG_FORM_NAME) AS 剂型,MAX(A.NETWORK_ITEM_CHARGE_CLASS) AS 类别,cast(MAX(A.SELF_BURDEN_RATIO) as varchar) AS 住院自付比例 ,MAX(A.TYPE_MEMO) AS 报销等级,MAX(B.RETAIL_PRICE) AS 价格,(case when MAX(A.NETWORK_ITEM_PROP)='1' then '药品' when MAX(A.NETWORK_ITEM_PROP)='2' then '诊疗' else '' end) as 中心项目属性,MAX(G.PRODUC_AREA_NAME) as HIS产地, MAX(H.APPROVAL_NO) AS HIS国药 ");
                //StrSql.Append(" FROM    COMM.COMM.NETWORKING_ITEM_VS_HIS AS A ");
                //StrSql.Append(" INNER JOIN COMM.COMM.DRUG_PRICE_LIST AS B ON A.HIS_ITEM_CODE = B.DRUG_CODE ");
                //StrSql.Append(" LEFT JOIN COMM.dict.PRODUC_AREAS G ON G.PRODUC_AREA_ID = B.FIRM_ID ");
                //StrSql.Append(" LEFT JOIN YP.DRUG.DRUG_STOCK H ON H.DRUG_PRICE_ID = B.DRUG_PRICE_ID ");
                //StrSql.Append(" LEFT JOIN YP.DRUG.DRUGS AS C ON B.DRUG_ID = C.DRUG_ID ");
                //StrSql.Append(" LEFT JOIN COMM.DICT.DRUG_CLASSES AS D ON C.DRUG_CLASS_ID = D.DRUG_CLASS_ID ");
                //StrSql.Append(" LEFT JOIN COMM.DICT.MEASURE_UNITS AS E ON c.MEASURE_UNIT_ID=e.MEASURE_UNIT_ID ");
                //StrSql.Append(" LEFT JOIN COMM.DICT.DRUG_FORMS AS F ON c.DRUG_FORM_ID=f.DRUG_FORM_ID ");
                //StrSql.Append(" WHERE C.CHARGE_TYPE= 1 ");
                //StrSql.Append(" AND A.ITEM_PROP='1' AND C.FLAG_INVALID=0 AND NETWORKING_PAT_CLASS_ID ='3' AND A.NETWORK_ITEM_CODE<>''  AND (A.HIS_ITEM_CODE LIKE '" + txt_hisxm.Text.Trim() + "%' OR A.HIS_ITEM_NAME LIKE '%" + txt_hisxm.Text.Trim() + "%' OR C.INPUT_CODE LIKE '%" + txt_hisxm.Text.Trim() + "%' ) ");
                //StrSql.Append(" GROUP BY A.HIS_ITEM_CODE ");
                //StrSql.Append(" ORDER BY A.HIS_ITEM_CODE ");

                StrSql.Append(" SELECT ");
                //   StrSql.Append(" [ITEM_PROP]");
                StrSql.Append(" [HIS_ITEM_CODE] as 编码");
                StrSql.Append(" ,[HIS_ITEM_NAME] as 名称");
                StrSql.Append("  ,[NETWORK_ITEM_CODE] as  中心码");
                StrSql.Append("    ,[NETWORK_ITEM_NAME] as  中心名称");
                StrSql.Append("   ,[SELF_BURDEN_RATIO] ");
                StrSql.Append("   ,[MEMO]");
                StrSql.Append("   ,[START_TIME]  as  启用时间");
                StrSql.Append("   ,[STOP_TIME]  as  停用时间");
                StrSql.Append("   ,[TYPE_MEMO]");
                //   StrSql.Append("   ,[NETWORK_ITEM_PROP]");
                StrSql.Append("  ,[NETWORK_ITEM_CHARGE_CLASS]");
                StrSql.Append("   ,[HOSPITAL_ID]");
                StrSql.Append("    ,[NETWORK_ITEM_PRICE] as 联网价格");
                //  StrSql.Append("   ,[FLAG_DISABLED]");
                //    StrSql.Append("   ,[NETWORK_ITEM_FLAG_UP]");
                StrSql.Append(" FROM [COMM].[COMM].[NETWORKING_ITEM_VS_HIS] ");

                StrSql.Append("  where  memo in('审批已通过','未审批') and  ITEM_PROP = '1'  ");
                StrSql.Append("  AND (HIS_ITEM_CODE LIKE '%" + txt_hisxm.Text.Trim() + "%' OR HIS_ITEM_NAME LIKE '%" + txt_hisxm.Text.Trim() + "%'  )  ORDER BY HIS_ITEM_CODE ");
            }
            DataSet ds = new DataSet();
            DataTable hisdata = new DataTable();
            ds = SQLHelper.ExecSqlReDs(StrSql.ToString());
            hisdata = ds.Tables[0];
            dgv_his.DataSource = hisdata;
            return hisdata;
        }
        #endregion


        #region 所有目录查询
        /// <summary>
        /// 所有项目DataTable
        /// </summary>
        /// <returns></returns>
        public DataTable QueryALL(string fylx)
        {
            DataTable dtResult = new DataTable();
            StringBuilder StrSql = new StringBuilder();
            if (fylx == "1")
            {
                StrSql.Append(" SELECT * FROM (");
                //--已对应

                StrSql.Append(" SELECT  MAX(A.NETWORK_ITEM_CODE) AS 中心项目编码 ,MAX(A.NETWORK_ITEM_NAME) AS 中心项目名称,MAX(B.DRUG_CODE) AS 编码 ,MAX(C.DRUG_NAME) AS 名称 , ");
                StrSql.Append(" MAX(B.DRUG_SPEC) AS 规格,MAX(e.MEASURE_UNIT_NAME) AS 单位, ");
                StrSql.Append(" MAX(F.DRUG_FORM_NAME) AS 剂型,MAX(A.NETWORK_ITEM_CHARGE_CLASS) AS 类别,cast(MAX(A.SELF_BURDEN_RATIO) as varchar) AS 住院自付比例 ,MAX(A.TYPE_MEMO) AS 报销等级,MAX(B.RETAIL_PRICE) AS 价格,(case when MAX(A.NETWORK_ITEM_PROP)='1' then '药品' when MAX(A.NETWORK_ITEM_PROP)='2' then '诊疗' else '' end) as 中心项目属性,MAX(G.PRODUC_AREA_NAME) as 产地  ");
                StrSql.Append(" FROM    COMM.COMM.NETWORKING_ITEM_VS_HIS AS A ");
                StrSql.Append(" INNER JOIN COMM.COMM.DRUG_PRICE_LIST AS B ON A.HIS_ITEM_CODE = B.DRUG_CODE ");
                StrSql.Append(" LEFT JOIN COMM.dict.PRODUC_AREAS G ON G.PRODUC_AREA_ID = B.FIRM_ID ");
                StrSql.Append(" LEFT JOIN YP.DRUG.DRUGS AS C ON B.DRUG_ID = C.DRUG_ID ");
                StrSql.Append(" LEFT JOIN COMM.DICT.DRUG_CLASSES AS D ON C.DRUG_CLASS_ID = D.DRUG_CLASS_ID ");
                StrSql.Append(" LEFT JOIN COMM.DICT.MEASURE_UNITS AS E ON c.MEASURE_UNIT_ID=e.MEASURE_UNIT_ID ");
                StrSql.Append(" LEFT JOIN COMM.DICT.DRUG_FORMS AS F ON c.DRUG_FORM_ID=f.DRUG_FORM_ID ");
                StrSql.Append(" WHERE C.CHARGE_TYPE<>1 ");

                StrSql.Append(" AND   A.ITEM_PROP='1' AND C.FLAG_INVALID=0 AND NETWORKING_PAT_CLASS_ID ='3'     AND (A.HIS_ITEM_CODE LIKE '" + txt_hisxm.Text.Trim() + "%' OR A.HIS_ITEM_NAME LIKE '%" + txt_hisxm.Text.Trim() + "%' OR C.INPUT_CODE LIKE '%" + txt_hisxm.Text.Trim() + "%' ) ");
                StrSql.Append(" GROUP BY A.HIS_ITEM_CODE ");

                StrSql.Append(" UNION ALL ");
                //--未对应
                StrSql.Append(" SELECT  '' AS 中心项目编码 ,'' AS 中心项目名称 ,MAX(A.DRUG_CODE) AS 编码 , ");
                StrSql.Append(" MAX(B.DRUG_NAME) AS 名称 ,MAX(B.DRUG_SPEC) AS 规格 ,MAX(e.MEASURE_UNIT_NAME) AS 单位 ,  ");
                StrSql.Append(" MAX(F.DRUG_FORM_NAME) AS 剂型  ,MAX('') AS 类别,'' AS 住院自付比例,'' AS 报销等级,MAX(A.RETAIL_PRICE) AS 价格,'' as 中心项目属性,MAX(G.PRODUC_AREA_NAME) as 产地     ");
                StrSql.Append(" FROM    COMM.COMM.DRUG_PRICE_LIST AS A ");
                StrSql.Append(" LEFT JOIN COMM.dict.PRODUC_AREAS G ON G.PRODUC_AREA_ID = A.FIRM_ID ");
                StrSql.Append(" LEFT JOIN YP.DRUG.DRUGS AS B ON A.DRUG_ID = B.DRUG_ID ");
                StrSql.Append(" LEFT JOIN COMM.DICT.DRUG_CLASSES AS C ON C.DRUG_CLASS_ID = B.DRUG_CLASS_ID ");
                StrSql.Append(" LEFT JOIN COMM.DICT.MEASURE_UNITS AS E ON B.MEASURE_UNIT_ID=e.MEASURE_UNIT_ID ");
                StrSql.Append(" LEFT JOIN COMM.DICT.DRUG_FORMS AS F ON B.DRUG_FORM_ID=f.DRUG_FORM_ID ");
                StrSql.Append(" WHERE B.CHARGE_TYPE<>1 ");
                StrSql.Append(" AND A.DRUG_CODE NOT IN ( SELECT HIS_ITEM_CODE FROM   COMM.COMM.NETWORKING_ITEM_VS_HIS ");
                StrSql.Append(" WHERE   ITEM_PROP = '1' AND NETWORKING_PAT_CLASS_ID ='3' ) ");
                StrSql.Append(" AND A.DRUG_ID > 0 AND A.DRUG_CODE <> '' AND B.FLAG_INVALID=0 ");
                StrSql.Append(" AND (B.DRUG_CODE LIKE '" + txt_hisxm.Text.Trim() + "%' OR B.DRUG_NAME LIKE '%" + txt_hisxm.Text.Trim() + "%' OR B.INPUT_CODE LIKE  '%" + txt_hisxm.Text.Trim() + "%' ) ");
                StrSql.Append(" GROUP BY A.DRUG_CODE ,C.DRUG_CLASS_ID ");

                StrSql.Append(" )aa ORDER BY 编码");
            }
            if (fylx == "2")
            {
                StrSql.Append(" SELECT * FROM (");
                //--已对应

                StrSql.Append(" SELECT  MAX(A.NETWORK_ITEM_CODE) AS 中心项目编码 ,MAX(A.NETWORK_ITEM_NAME) AS 中心项目名称 , ");
                StrSql.Append(" MAX(A.HIS_ITEM_CODE) AS 编码 ,MAX(A.HIS_ITEM_NAME) AS 名称 , ");
                StrSql.Append(" (CASE WHEN MAX(B.SPEC)='' THEN '/' ELSE MAX(B.SPEC) END) AS 规格 ,MAX(C.MEASURE_UNIT_NAME) AS 单位 , ");
                StrSql.Append("  cast(MAX(A.SELF_BURDEN_RATIO) as varchar) AS 自付比例 ,MAX(A.TYPE_MEMO) AS 报销等级,MAX(A.NETWORK_ITEM_CHARGE_CLASS) AS 类别,MAX(B.PRICE) AS 价格,(case when MAX(A.NETWORK_ITEM_PROP)='1' then '药品' when MAX(A.NETWORK_ITEM_PROP)='2' then '诊疗' else '' end) as 中心项目属性,'' as 是否修改  ");
                StrSql.Append(" FROM    COMM.COMM.NETWORKING_ITEM_VS_HIS AS A ");
                StrSql.Append("INNER JOIN COMM.COMM.CHARGE_PRICE AS B ON A.HIS_ITEM_CODE = B.CHARGE_CODE LEFT JOIN COMM.DICT.MEASURE_UNITS AS C ON B.MEASURE_UNIT_ID = C.MEASURE_UNIT_ID ");
                StrSql.Append(" WHERE   A.ITEM_PROP = '2' AND NETWORKING_PAT_CLASS_ID ='3' AND B.FLAG_INVALID=0    ");
                if (txt_hisxm.Text.Trim().ToString() != "")
                {
                    StrSql.Append("  AND ( A.HIS_ITEM_CODE LIKE '" + txt_hisxm.Text.Trim() + "%' OR A.HIS_ITEM_NAME LIKE '%" + txt_hisxm.Text.Trim() + "%' OR B.INPUT_CODE LIKE '%" + txt_hisxm.Text.Trim() + "%' ) ");
                }
                StrSql.Append(" GROUP BY A.HIS_ITEM_CODE");

                StrSql.Append(" UNION ALL ");
                //--未对应
                StrSql.Append(" SELECT  '' AS 中心项目编码 ,'' AS 中心项目名称 ,MAX(A.CHARGE_CODE) AS 编码 , ");
                StrSql.Append("  MAX(A.CHARGE_NAME) AS 名称 ,(CASE WHEN MAX(A.SPEC)='' THEN '/' ELSE MAX(A.SPEC) END) AS 规格,MAX(B.MEASURE_UNIT_NAME) AS 单位,MAX('') AS 自付比例,MAX('') AS 报销等级,MAX('') AS 类别,MAX(A.PRICE) AS 价格,'' as 中心项目属性,'' as 是否修改  ");
                StrSql.Append(" FROM    COMM.COMM.CHARGE_PRICE AS A  LEFT JOIN COMM.DICT.MEASURE_UNITS AS B ON A.MEASURE_UNIT_ID = B.MEASURE_UNIT_ID ");
                StrSql.Append(" WHERE A.CHARGE_CODE NOT IN ( SELECT   HIS_ITEM_CODE FROM     COMM.COMM.NETWORKING_ITEM_VS_HIS ");
                StrSql.Append(" WHERE   ITEM_PROP = '2' AND NETWORKING_PAT_CLASS_ID ='3'  ) ");
                StrSql.Append(" AND A.CHARGE_ID > 0 AND A.CHARGE_CODE <> '' AND A.FLAG_INVALID=0");

                if (txt_hisxm.Text.Trim().ToString() != "")
                {
                    StrSql.Append(" AND (A.CHARGE_CODE LIKE '" + txt_hisxm.Text.Trim() + "%' OR A.CHARGE_NAME LIKE '%" + txt_hisxm.Text.Trim() + "%' OR a.INPUT_CODE LIKE '%" + txt_hisxm.Text.Trim() + "%') ");
                }
                StrSql.Append(" GROUP BY A.CHARGE_CODE ");

                StrSql.Append(" )aa ORDER BY 编码");
            }

            if (fylx == "3")
            {
                StrSql.Append(" SELECT * FROM (");
                //--已对应

                StrSql.Append(" SELECT  MAX(A.NETWORK_ITEM_CODE) AS 中心项目编码 ,MAX(A.NETWORK_ITEM_NAME) AS 中心项目名称,MAX(B.DRUG_CODE) AS 编码 ,MAX(C.DRUG_NAME) AS 名称 , ");
                StrSql.Append(" MAX(B.DRUG_SPEC) AS 规格,MAX(e.MEASURE_UNIT_NAME) AS 单位, ");
                StrSql.Append(" MAX(F.DRUG_FORM_NAME) AS 剂型,MAX(A.NETWORK_ITEM_CHARGE_CLASS) AS 类别,cast(MAX(A.SELF_BURDEN_RATIO) as varchar) AS 住院自付比例 ,MAX(A.TYPE_MEMO) AS 报销等级,MAX(B.RETAIL_PRICE) AS 价格,(case when MAX(A.NETWORK_ITEM_PROP)='1' then '药品' when MAX(A.NETWORK_ITEM_PROP)='2' then '诊疗' else '' end) as 中心项目属性,MAX(G.PRODUC_AREA_NAME) as 产地  ");
                StrSql.Append(" FROM    COMM.COMM.NETWORKING_ITEM_VS_HIS AS A ");
                StrSql.Append(" INNER JOIN COMM.COMM.DRUG_PRICE_LIST AS B ON A.HIS_ITEM_CODE = B.DRUG_CODE ");
                StrSql.Append(" LEFT JOIN COMM.dict.PRODUC_AREAS G ON G.PRODUC_AREA_ID = B.FIRM_ID ");
                StrSql.Append(" LEFT JOIN YP.DRUG.DRUGS AS C ON B.DRUG_ID = C.DRUG_ID ");
                StrSql.Append(" LEFT JOIN COMM.DICT.DRUG_CLASSES AS D ON C.DRUG_CLASS_ID = D.DRUG_CLASS_ID ");
                StrSql.Append(" LEFT JOIN COMM.DICT.MEASURE_UNITS AS E ON c.MEASURE_UNIT_ID=e.MEASURE_UNIT_ID ");
                StrSql.Append(" LEFT JOIN COMM.DICT.DRUG_FORMS AS F ON c.DRUG_FORM_ID=f.DRUG_FORM_ID ");
                StrSql.Append(" WHERE C.CHARGE_TYPE=1 ");

                StrSql.Append(" AND   A.ITEM_PROP='1' AND C.FLAG_INVALID=0 AND NETWORKING_PAT_CLASS_ID ='3'     AND (A.HIS_ITEM_CODE LIKE '" + txt_hisxm.Text.Trim() + "%' OR A.HIS_ITEM_NAME LIKE '%" + txt_hisxm.Text.Trim() + "%' OR C.INPUT_CODE LIKE '%" + txt_hisxm.Text.Trim() + "%' ) ");
                StrSql.Append(" GROUP BY A.HIS_ITEM_CODE ");

                StrSql.Append(" UNION ALL ");
                //--未对应
                StrSql.Append(" SELECT  '' AS 中心项目编码 ,'' AS 中心项目名称 ,MAX(A.DRUG_CODE) AS 编码 , ");
                StrSql.Append(" MAX(B.DRUG_NAME) AS 名称 ,MAX(B.DRUG_SPEC) AS 规格 ,MAX(e.MEASURE_UNIT_NAME) AS 单位 ,  ");
                StrSql.Append(" MAX(F.DRUG_FORM_NAME) AS 剂型  ,MAX('') AS 类别,'' AS 住院自付比例,'' AS 报销等级,MAX(A.RETAIL_PRICE) AS 价格,'' as 中心项目属性,MAX(G.PRODUC_AREA_NAME) as 产地     ");
                StrSql.Append(" FROM    COMM.COMM.DRUG_PRICE_LIST AS A ");
                StrSql.Append(" LEFT JOIN COMM.dict.PRODUC_AREAS G ON G.PRODUC_AREA_ID = A.FIRM_ID ");
                StrSql.Append(" LEFT JOIN YP.DRUG.DRUGS AS B ON A.DRUG_ID = B.DRUG_ID ");
                StrSql.Append(" LEFT JOIN COMM.DICT.DRUG_CLASSES AS C ON C.DRUG_CLASS_ID = B.DRUG_CLASS_ID ");
                StrSql.Append(" LEFT JOIN COMM.DICT.MEASURE_UNITS AS E ON B.MEASURE_UNIT_ID=e.MEASURE_UNIT_ID ");
                StrSql.Append(" LEFT JOIN COMM.DICT.DRUG_FORMS AS F ON B.DRUG_FORM_ID=f.DRUG_FORM_ID ");
                StrSql.Append(" WHERE B.CHARGE_TYPE=1 ");
                StrSql.Append(" AND A.DRUG_CODE NOT IN ( SELECT HIS_ITEM_CODE FROM   COMM.COMM.NETWORKING_ITEM_VS_HIS ");
                StrSql.Append(" WHERE   ITEM_PROP = '1' AND NETWORKING_PAT_CLASS_ID ='3' ) ");
                StrSql.Append(" AND A.DRUG_ID > 0 AND A.DRUG_CODE <> '' AND B.FLAG_INVALID=0 ");
                StrSql.Append(" AND (B.DRUG_CODE LIKE '" + txt_hisxm.Text.Trim() + "%' OR B.DRUG_NAME LIKE '%" + txt_hisxm.Text.Trim() + "%' OR B.INPUT_CODE LIKE  '%" + txt_hisxm.Text.Trim() + "%' ) ");
                StrSql.Append(" GROUP BY A.DRUG_CODE ,C.DRUG_CLASS_ID ");

                StrSql.Append(" )aa ORDER BY 编码");
            }
            DataSet ds = new DataSet();
            DataTable hisdata = new DataTable();
            ds = SQLHelper.ExecSqlReDs(StrSql.ToString());
            hisdata = ds.Tables[0];
            dgv_his.DataSource = hisdata;
            return hisdata;
        }
        #endregion


        #region 已注释
        ////未对应
        //if (dzlx == "1")
        //{
        //    if (fylx == "2") //医疗
        //    {
        //        StringBuilder sqlStr = new StringBuilder();
        //        sqlStr.Append(" SELECT CHARGE_CODE 医院编码,CHARGE_NAME 医院名称,SPEC 规格,INPUT_CODE AS 拼音码,PRICE 单价 FROM COMM.COMM.CHARGE_PRICE WHERE CHARGE_CODE NOT IN ");
        //        sqlStr.Append(" (SELECT HIS_ITEM_CODE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID=1) AND FLAG_INVALID=0 AND ((PRICE=0 AND FLAG_MODIFY='1') OR PRICE!=0) AND FLAG_SUPER!='1'  ");
        //        if (hisxm != "")
        //        {
        //            sqlStr.Append(" AND (INPUT_CODE LIKE '" + hisxm + "%' ");
        //            sqlStr.Append(" OR CHARGE_NAME LIKE '" + hisxm + "%' OR CHARGE_CODE LIKE '" + hisxm + "%') ");
        //        }
        //        DataSet ds = new DataSet();
        //        DataTable hisdata = new DataTable();
        //        ds = SQLHelper.ExecSqlReDs(sqlStr.ToString());
        //        hisdata = ds.Tables[0];
        //        dgv_his.DataSource = hisdata;

        //    }
        //    else if (fylx == "1") //药品
        //    {
        //        //修改前
        //        StringBuilder sqlStr = new StringBuilder();
        //        sqlStr.Append(" SELECT MAX(A.DRUG_CODE) AS 医院编码,MAX(DRUG_NAME) AS 医院名称,MAX(A.DRUG_SPEC) AS 规格,MAX(d.DRUG_FORM_NAME) AS 剂型, SUBSTRING(MAX(A.INPUT_CODE),0,4)AS 拼音码,CONVERT(FLOAT,MAX(RETAIL_PRICE)) AS 单价 ");
        //        sqlStr.Append(" FROM YP.DRUG.DRUGS A LEFT JOIN COMM.COMM.DRUG_PRICE_LIST B ON A.DRUG_ID = B.DRUG_ID ");
        //        sqlStr.Append(" LEFT JOIN YP.DRUG.DRUG_SPEC C ON A.DRUG_ID = C.DRUG_ID ");
        //        sqlStr.Append(" LEFT JOIN COMM.DICT.DRUG_FORMS d ON a.DRUG_FORM_ID=d.DRUG_FORM_ID ");
        //        sqlStr.Append(" WHERE B.RETAIL_PRICE <> -1 AND REPLACE(A.DRUG_CODE,' ','') NOT IN (SELECT HIS_ITEM_CODE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORKING_PAT_CLASS_ID='1') AND a.FLAG_INVALID=0 ");

        //        if (hisxm != "")
        //        {
        //            sqlStr.Append(" AND (A.DRUG_CODE LIKE '" + hisxm + "%' OR ");
        //            sqlStr.Append(" DRUG_NAME LIKE '" + hisxm + "%' OR A.INPUT_CODE LIKE '" + hisxm + "%') ");
        //        }
        //        sqlStr.Append(" GROUP BY A.DRUG_ID ");

        //        DataSet ds = new DataSet();
        //        DataTable hisdata = new DataTable();
        //        ds = SQLHelper.ExecSqlReDs(sqlStr.ToString());
        //        hisdata = ds.Tables[0];
        //        dgv_his.DataSource = hisdata;
        //    }
        //}
        //else if (dzlx == "2") //已对照未上传
        //{
        //    if (fylx == "2") //医疗
        //    {
        //        StringBuilder sqlStr = new StringBuilder();
        //        sqlStr.Append(" SELECT a.HIS_ITEM_CODE 医院编码,a.HIS_ITEM_NAME 医院名称,a.NETWORK_ITEM_CODE 中心编码,a.NETWORK_ITEM_NAME 中心名称,  ");
        //        sqlStr.Append(" CASE WHEN b.收费项目等级='1' THEN '甲类' WHEN b.收费项目等级='2' THEN '乙类' WHEN b.收费项目等级='3' THEN '丙类' END 收费项目等级,NETWORK_ITEM_PROP AS 收费项目种类, ");
        //        sqlStr.Append(" CASE WHEN b.工伤使用标志='1' THEN '可用' ELSE '不可用' END 工伤使用标志,CASE WHEN b.生育使用标志='1' THEN '可用' ELSE '不可用' END 生育使用标志, ");
        //        sqlStr.Append(" CASE WHEN b.基本医疗使用标志='1' THEN    '可用' ELSE '不可用' END 基本医疗使用标志, ");
        //        sqlStr.Append(" CASE WHEN a.NETWORK_ITEM_FLAG_UP='0' THEN '未上传' ELSE '已上传' END AS 是否上传 ");
        //        sqlStr.Append(" FROM COMM.COMM.NETWORKING_ITEM_VS_HIS a ");
        //        sqlStr.Append(" LEFT JOIN YBDR.dbo.YB_ZLML b ON a.NETWORK_ITEM_CODE=b.诊疗项目编码 ");
        //        sqlStr.Append(" WHERE a.ITEM_PROP='2' AND a.NETWORKING_PAT_CLASS_ID='1' AND a.NETWORK_ITEM_FLAG_UP='0' ");
        //        if (hisxm != "")
        //        {
        //            sqlStr.Append(" AND ( "); //COMM.dbo.FUN_GETPY(HIS_ITEM_NAME) LIKE '" + hisxm + "%'
        //            sqlStr.Append("  HIS_ITEM_NAME LIKE '" + hisxm + "%' OR HIS_ITEM_CODE LIKE '" + hisxm + "%') ");//OR
        //        }
        //        DataSet ds = new DataSet();
        //        DataTable hisdata = new DataTable();
        //        ds = SQLHelper.ExecSqlReDs(sqlStr.ToString());
        //        hisdata = ds.Tables[0];
        //        dgv_his.DataSource = hisdata;

        //    }
        //    else if (fylx == "1") //药品
        //    {
        //        StringBuilder sqlStr = new StringBuilder();
        //        sqlStr.Append(" SELECT a.HIS_ITEM_CODE 医院编码,a.HIS_ITEM_NAME 医院名称,a.NETWORK_ITEM_CODE 中心编码,a.NETWORK_ITEM_NAME 中心名称,剂型 as 中心剂型, ");
        //        sqlStr.Append(" CASE WHEN b.收费项目等级='1' THEN '甲类' WHEN b.收费项目等级='2' THEN '乙类' WHEN b.收费项目等级='3' THEN '丙类' END 收费项目等级,收费项目种类,  ");
        //        sqlStr.Append(" CASE WHEN b.工伤使用标志='1' THEN '可用' ELSE '不可用' END 工伤使用标志,CASE WHEN b.生育使用标志='1' THEN '可用' ELSE '不可用' END 生育使用标志, ");
        //        sqlStr.Append(" CASE WHEN b.基本医疗使用标志='1' THEN    '可用' ELSE '不可用' END 基本医疗使用标志,CASE WHEN a.NETWORK_ITEM_FLAG_UP='0' THEN '未上传' ELSE '已上传' END AS 是否上传 ");
        //        sqlStr.Append(" FROM COMM.COMM.NETWORKING_ITEM_VS_HIS a ");
        //        sqlStr.Append(" LEFT JOIN (SELECT 药品编码,中文名称,b.SEC_VALUE as 剂型,收费项目等级,工伤使用标志,生育使用标志,基本医疗使用标志,'1' AS 收费项目种类 FROM YBDR.dbo.YB_YPML a ");
        //        sqlStr.Append(" LEFT JOIN YBDR.dbo.YB_SEC_LEVEL_CODE b ON a.剂型=b.SEC_CODE AND b.SEC_CLASS='剂型' ");
        //        sqlStr.Append(" UNION ALL ");
        //        sqlStr.Append(" SELECT 诊疗项目编码,项目名称,'' 剂型,收费项目等级,工伤使用标志,生育使用标志,基本医疗使用标志,'2' AS 收费项目种类 FROM YBDR.dbo.YB_ZLML WHERE 收费类别 IN ('28','94')) b ON a.NETWORK_ITEM_CODE=b.药品编码  ");
        //        sqlStr.Append(" WHERE a.ITEM_PROP='1' AND a.NETWORKING_PAT_CLASS_ID='1' AND a.NETWORK_ITEM_FLAG_UP='0' ");
        //        if (hisxm != "")
        //        {
        //            sqlStr.Append(" AND ( ");
        //            sqlStr.Append(" HIS_ITEM_NAME LIKE '" + hisxm + "%' OR HIS_ITEM_CODE LIKE '" + hisxm + "%') ");
        //        }
        //        DataSet ds = new DataSet();
        //        DataTable hisdata = new DataTable();
        //        ds = SQLHelper.ExecSqlReDs(sqlStr.ToString());
        //        hisdata = ds.Tables[0];
        //        dgv_his.DataSource = hisdata;
        //    }
        //}
        //else if (dzlx == "3") //已对照已上传
        //{
        //    if (fylx == "2") //医疗
        //    {
        //        StringBuilder sqlStr = new StringBuilder();
        //        sqlStr.Append(" SELECT a.HIS_ITEM_CODE 医院编码,a.HIS_ITEM_NAME 医院名称,a.NETWORK_ITEM_CODE 中心编码,a.NETWORK_ITEM_NAME 中心名称,  ");
        //        sqlStr.Append(" CASE WHEN b.收费项目等级='1' THEN '甲类' WHEN b.收费项目等级='2' THEN '乙类' WHEN b.收费项目等级='3' THEN '丙类' END 收费项目等级, ");
        //        sqlStr.Append(" CASE WHEN b.工伤使用标志='1' THEN '可用' ELSE '不可用' END 工伤使用标志,CASE WHEN b.生育使用标志='1' THEN '可用' ELSE '不可用' END 生育使用标志, ");
        //        sqlStr.Append(" CASE WHEN b.基本医疗使用标志='1' THEN '可用' ELSE '不可用' END 基本医疗使用标志, ");
        //        sqlStr.Append(" CASE WHEN a.NETWORK_ITEM_FLAG_UP='0' THEN '未上传' ELSE '已上传' END AS 是否上传 ");
        //        sqlStr.Append(" FROM COMM.COMM.NETWORKING_ITEM_VS_HIS a ");
        //        sqlStr.Append(" LEFT JOIN YBDR.dbo.YB_ZLML b ON a.NETWORK_ITEM_CODE=b.诊疗项目编码 ");
        //        sqlStr.Append(" WHERE a.ITEM_PROP='2' AND a.NETWORKING_PAT_CLASS_ID='1' AND a.NETWORK_ITEM_FLAG_UP='1' ");
        //        if (hisxm != "")
        //        {
        //            sqlStr.Append(" AND ( ");
        //            sqlStr.Append(" HIS_ITEM_NAME LIKE '" + hisxm + "%' OR HIS_ITEM_CODE LIKE '" + hisxm + "%') ");
        //        }
        //        DataSet ds = new DataSet();
        //        DataTable hisdata = new DataTable();
        //        ds = SQLHelper.ExecSqlReDs(sqlStr.ToString());
        //        hisdata = ds.Tables[0];
        //        dgv_his.DataSource = hisdata;

        //    }
        //    else if (fylx == "1") //药品
        //    {
        //        StringBuilder sqlStr = new StringBuilder();
        //        sqlStr.Append(" SELECT a.HIS_ITEM_CODE 医院编码,a.HIS_ITEM_NAME 医院名称,a.NETWORK_ITEM_CODE 中心编码,a.NETWORK_ITEM_NAME 中心名称,剂型 as 中心剂型, ");
        //        sqlStr.Append(" CASE WHEN b.收费项目等级='1' THEN '甲类' WHEN b.收费项目等级='2' THEN '乙类' WHEN b.收费项目等级='3' THEN '丙类' END 收费项目等级, ");
        //        sqlStr.Append(" CASE WHEN b.工伤使用标志='1' THEN '可用' ELSE '不可用' END 工伤使用标志,CASE WHEN b.生育使用标志='1' THEN '可用' ELSE '不可用' END 生育使用标志, ");
        //        sqlStr.Append(" CASE WHEN b.基本医疗使用标志='1' THEN    '可用' ELSE '不可用' END 基本医疗使用标志,CASE WHEN a.NETWORK_ITEM_FLAG_UP='0' THEN '未上传' ELSE '已上传' END AS 是否上传 ");
        //        sqlStr.Append(" FROM COMM.COMM.NETWORKING_ITEM_VS_HIS a ");
        //        sqlStr.Append(" LEFT JOIN (SELECT 药品编码,中文名称,b.SEC_VALUE as 剂型,收费项目等级,工伤使用标志,生育使用标志,基本医疗使用标志,'1' AS 收费项目种类 FROM YBDR.dbo.YB_YPML a ");
        //        sqlStr.Append(" LEFT JOIN YBDR.dbo.YB_SEC_LEVEL_CODE b ON a.剂型=b.SEC_CODE AND b.SEC_CLASS='剂型' ");
        //        sqlStr.Append(" UNION ALL ");
        //        sqlStr.Append(" SELECT 诊疗项目编码,项目名称,'' 剂型,收费项目等级,工伤使用标志,生育使用标志,基本医疗使用标志,'2' AS 收费项目种类 FROM YBDR.dbo.YB_ZLML WHERE 收费类别 IN ('28','94')) b ON a.NETWORK_ITEM_CODE=b.药品编码  ");
        //        sqlStr.Append(" WHERE a.ITEM_PROP='1' AND a.NETWORKING_PAT_CLASS_ID='1' AND a.NETWORK_ITEM_FLAG_UP='1' ");
        //        if (hisxm != "")
        //        {
        //            sqlStr.Append(" AND ( ");
        //            sqlStr.Append(" HIS_ITEM_NAME LIKE '" + hisxm + "%' OR HIS_ITEM_CODE LIKE '" + hisxm + "%') ");
        //        }
        //        DataSet ds = new DataSet();
        //        DataTable hisdata = new DataTable();
        //        ds = SQLHelper.ExecSqlReDs(sqlStr.ToString());
        //        hisdata = ds.Tables[0];
        //        dgv_his.DataSource = hisdata;
        //    }
        //}
        //}
        #endregion



        #region 诊疗
        //#region 未对应编码的项目查询
        ///// <summary>
        ///// 未对应项目DataTable
        ///// </summary>
        ///// <returns></returns>
        //public DataTable QueryNotMatch()
        //{
        //    DataTable dtResult = new DataTable();
        //    StringBuilder StrSql = new StringBuilder();
        //    StrSql.Append(" SELECT  '' AS 中心项目编码 ,'' AS 中心项目名称 ,MAX(A.CHARGE_CODE) AS 诊疗编码 , ");
        //    StrSql.Append("  MAX(A.CHARGE_NAME) AS 诊疗名称 ,(CASE WHEN MAX(A.SPEC)='' THEN '/' ELSE MAX(A.SPEC) END) AS 规格,MAX(B.MEASURE_UNIT_NAME) AS 单位,MAX('') AS 自付比例,MAX('') AS 报销等级,MAX('') AS 类别,MAX(A.PRICE) AS 价格,'' as 中心项目属性,'' as 是否修改  ");
        //    StrSql.Append(" FROM    COMM.COMM.CHARGE_PRICE AS A  LEFT JOIN COMM.DICT.MEASURE_UNITS AS B ON A.MEASURE_UNIT_ID = B.MEASURE_UNIT_ID ");
        //    StrSql.Append("WHERE  A.CHARGE_CODE NOT IN ( SELECT   HIS_ITEM_CODE FROM     COMM.COMM.NETWORKING_ITEM_VS_HIS ");
        //    StrSql.Append(" WHERE   ITEM_PROP = '2' AND NETWORKING_PAT_CLASS_ID ='3'  AND NETWORK_ITEM_CODE<>'' ) ");
        //    StrSql.Append(" AND A.CHARGE_ID > 0 AND A.CHARGE_CODE <> '' AND A.FLAG_INVALID=0");

        //    if (txt_hisxm.Text.Trim().ToString() != "")
        //    {
        //        StrSql.Append(" AND (A.CHARGE_CODE LIKE '" + txt_hisxm.Text.Trim() + "%' OR A.CHARGE_NAME LIKE '%" + txt_hisxm.Text.Trim() + "%' OR a.INPUT_CODE LIKE '%" + txt_hisxm.Text.Trim() + "%') ");
        //    }
        //    StrSql.Append(" GROUP BY A.CHARGE_CODE ");
        //    DataSet ds = new DataSet();
        //    DataTable hisdata = new DataTable();
        //    ds = SQLHelper.ExecSqlReDs(StrSql.ToString());
        //    hisdata = ds.Tables[0];
        //    dgv_his.DataSource = hisdata;
        //    return hisdata;
        //}
        //#endregion


        //#region 已对应编码的目录查询
        ///// <summary>
        ///// 已对应未上传项目DataTable
        ///// </summary>
        ///// <returns></returns>
        //public DataTable QueryMatchNotUpLoad()
        //{
        //    DataTable dtResult = new DataTable();
        //    StringBuilder StrSql = new StringBuilder();
        //    StrSql.Append(" SELECT  MAX(A.NETWORK_ITEM_CODE) AS 中心项目编码 ,MAX(A.NETWORK_ITEM_NAME) AS 中心项目名称 , ");
        //    StrSql.Append(" MAX(A.HIS_ITEM_CODE) AS 诊疗编码 ,MAX(A.HIS_ITEM_NAME) AS 诊疗名称 , ");
        //    StrSql.Append(" (CASE WHEN MAX(B.SPEC)='' THEN '/' ELSE MAX(B.SPEC) END) AS 规格 ,MAX(C.MEASURE_UNIT_NAME) AS 单位 , ");
        //    StrSql.Append("  cast(MAX(A.SELF_BURDEN_RATIO) as varchar) AS 自付比例 ,MAX(A.TYPE_MEMO) AS 报销等级,MAX(A.NETWORK_ITEM_CHARGE_CLASS) AS 类别,MAX(B.PRICE) AS 价格,(case when MAX(A.NETWORK_ITEM_PROP)='1' then '药品' when MAX(A.NETWORK_ITEM_PROP)='2' then '诊疗' else '' end) as 中心项目属性,'' as 是否修改  ");
        //    StrSql.Append(" FROM    COMM.COMM.NETWORKING_ITEM_VS_HIS AS A ");
        //    StrSql.Append("INNER JOIN COMM.COMM.CHARGE_PRICE AS B ON A.HIS_ITEM_CODE = B.CHARGE_CODE  LEFT JOIN COMM.DICT.MEASURE_UNITS AS C ON B.MEASURE_UNIT_ID = C.MEASURE_UNIT_ID ");
        //    StrSql.Append(" WHERE  A.ITEM_PROP = '2' AND NETWORKING_PAT_CLASS_ID ='3' AND NETWORK_ITEM_CODE<>'' AND B.FLAG_INVALID=0    ");
        //    if (txt_hisxm.Text.Trim().ToString() != "")
        //    {
        //        StrSql.Append("  AND ( A.HIS_ITEM_CODE LIKE '" + txt_hisxm.Text.Trim() + "%' OR A.HIS_ITEM_NAME LIKE '%" + txt_hisxm.Text.Trim() + "%' OR B.INPUT_CODE LIKE '%" + txt_hisxm.Text.Trim() + "%' ) ");
        //    }
        //    StrSql.Append(" GROUP BY A.HIS_ITEM_CODE ORDER BY A.HIS_ITEM_CODE ");
        //    DataSet ds = new DataSet();
        //    DataTable hisdata = new DataTable();
        //    ds = SQLHelper.ExecSqlReDs(StrSql.ToString());
        //    hisdata = ds.Tables[0];
        //    dgv_his.DataSource = hisdata;
        //    return hisdata;
        //}
        //#endregion


        //#region 所有目录查询
        ///// <summary>
        ///// 所有项目DataTable
        ///// </summary>
        ///// <returns></returns>
        //public DataTable QueryALL()
        //{


        //    DataTable dtResult = new DataTable();
        //    StringBuilder StrSql = new StringBuilder();
        //    StrSql.Append(" SELECT * FROM (");
        //    //--已对应

        //    StrSql.Append(" SELECT  MAX(A.NETWORK_ITEM_CODE) AS 中心项目编码 ,MAX(A.NETWORK_ITEM_NAME) AS 中心项目名称 , ");
        //    StrSql.Append(" MAX(A.HIS_ITEM_CODE) AS 诊疗编码 ,MAX(A.HIS_ITEM_NAME) AS 诊疗名称 , ");
        //    StrSql.Append(" (CASE WHEN MAX(B.SPEC)='' THEN '/' ELSE MAX(B.SPEC) END) AS 规格 ,MAX(C.MEASURE_UNIT_NAME) AS 单位 , ");
        //    StrSql.Append("  cast(MAX(A.SELF_BURDEN_RATIO) as varchar) AS 自付比例 ,MAX(A.TYPE_MEMO) AS 报销等级,MAX(A.NETWORK_ITEM_CHARGE_CLASS) AS 类别,MAX(B.PRICE) AS 价格,(case when MAX(A.NETWORK_ITEM_PROP)='1' then '药品' when MAX(A.NETWORK_ITEM_PROP)='2' then '诊疗' else '' end) as 中心项目属性,'' as 是否修改  ");
        //    StrSql.Append(" FROM    COMM.COMM.NETWORKING_ITEM_VS_HIS AS A ");
        //    StrSql.Append("INNER JOIN COMM.COMM.CHARGE_PRICE AS B ON A.HIS_ITEM_CODE = B.CHARGE_CODE LEFT JOIN COMM.DICT.MEASURE_UNITS AS C ON B.MEASURE_UNIT_ID = C.MEASURE_UNIT_ID ");
        //    StrSql.Append(" WHERE   A.ITEM_PROP = '2' AND NETWORKING_PAT_CLASS_ID ='3' AND B.FLAG_INVALID=0    ");
        //    if (txt_hisxm.Text.Trim().ToString() != "")
        //    {
        //        StrSql.Append("  AND ( A.HIS_ITEM_CODE LIKE '" + txt_hisxm.Text.Trim() + "%' OR A.HIS_ITEM_NAME LIKE '%" + txt_hisxm.Text.Trim() + "%' OR B.INPUT_CODE LIKE '%" + txt_hisxm.Text.Trim() + "%' ) ");
        //    }
        //    StrSql.Append(" GROUP BY A.HIS_ITEM_CODE");

        //    StrSql.Append(" UNION ALL ");
        //    //--未对应
        //    StrSql.Append(" SELECT  '' AS 中心项目编码 ,'' AS 中心项目名称 ,MAX(A.CHARGE_CODE) AS 诊疗编码 , ");
        //    StrSql.Append("  MAX(A.CHARGE_NAME) AS 诊疗名称 ,(CASE WHEN MAX(A.SPEC)='' THEN '/' ELSE MAX(A.SPEC) END) AS 规格,MAX(B.MEASURE_UNIT_NAME) AS 单位,MAX('') AS 自付比例,MAX('') AS 报销等级,MAX('') AS 类别,MAX(A.PRICE) AS 价格,'' as 中心项目属性,'' as 是否修改  ");
        //    StrSql.Append(" FROM    COMM.COMM.CHARGE_PRICE AS A  LEFT JOIN COMM.DICT.MEASURE_UNITS AS B ON A.MEASURE_UNIT_ID = B.MEASURE_UNIT_ID ");
        //    StrSql.Append(" WHERE A.CHARGE_CODE NOT IN ( SELECT   HIS_ITEM_CODE FROM     COMM.COMM.NETWORKING_ITEM_VS_HIS ");
        //    StrSql.Append(" WHERE   ITEM_PROP = '2' AND NETWORKING_PAT_CLASS_ID ='3'  ) ");
        //    StrSql.Append(" AND A.CHARGE_ID > 0 AND A.CHARGE_CODE <> '' AND A.FLAG_INVALID=0");

        //    if (txt_hisxm.Text.Trim().ToString() != "")
        //    {
        //        StrSql.Append(" AND (A.CHARGE_CODE LIKE '" + txt_hisxm.Text.Trim() + "%' OR A.CHARGE_NAME LIKE '%" + txt_hisxm.Text.Trim() + "%' OR a.INPUT_CODE LIKE '%" + txt_hisxm.Text.Trim() + "%') ");
        //    }
        //    StrSql.Append(" GROUP BY A.CHARGE_CODE ");

        //    StrSql.Append(" )aa ORDER BY 诊疗编码");
        //    DataSet ds = new DataSet();
        //    DataTable hisdata = new DataTable();
        //    ds = SQLHelper.ExecSqlReDs(StrSql.ToString());
        //    hisdata = ds.Tables[0];
        //    dgv_his.DataSource = hisdata;
        //    return hisdata;
        //}
        //#endregion
        //#endregion



        //#endregion
        #endregion
        #region HIS目录查询
        /// <summary>
        /// HIS目录查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_His_CX_Click(object sender, EventArgs e)
        {
            GX();
        }
        #endregion

        #region 中心目录查询
        /// <summary>
        /// 中心目录查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ZXCX_Click(object sender, EventArgs e)
        {
            string zxxm = this.txt_zxxm.Text.Trim(); //中心项目
            fylx = cmb_FYLX.SelectedValue.ToString(); //费用类型

            StringBuilder sqlStr = new StringBuilder();
            if (fylx == "1")
            {
              //  sqlStr.Append("SELECT 中心编码,中心名称,住院自付比例,报销等级,中心收费类别,国药准字,产地,剂型,中心项目属性,拼音码,价格 FROM (");
              //  sqlStr.Append("SELECT  NETWORK_ITEM_CODE AS 中心编码 ,NETWORK_ITEM_NAME AS 中心名称 , DRUG_ZY_SELF_BURRDEN_RATIO AS 住院自付比例,");
              //  sqlStr.Append(" TYPE_MEMO AS 报销等级 ,NETWORK_ITEM_CHARGE_CLASS AS 中心收费类别,DRUG_COUNTRY_MEDICINE  AS 国药准字 ,DRUG_OGRIN AS 产地 ,DRUG_DOSE as 剂型 ,'药品' as 中心项目属性,DRUG_INPUT_CODE_PY AS 拼音码,DRUG_HIGHEST_PRICE AS 价格 ");
              //  sqlStr.Append(" FROM COMM.DICT.NETWORKING_CENTER_DRUG_DICT WHERE NETWORKING_PAT_CLASS_ID ='2' AND (DRUG_INPUT_CODE_PY LIKE '%" + zxxm + "%' OR NETWORK_ITEM_NAME LIKE '%" + zxxm + "%' OR NETWORK_ITEM_CODE LIKE '%" + zxxm + "%'OR DRUG_COUNTRY_MEDICINE LIKE '%" + zxxm + "%' OR DRUG_TRADE_INPUT_CODE_PY LIKE '" + zxxm + "%' OR TYPE_MEMO LIKE '" + zxxm + "')");
              //  sqlStr.Append(") aa ORDER BY 中心项目属性 ASC,charindex('" + zxxm + "',拼音码),charindex('" + zxxm + "',中心名称),LEN(拼音码)");

              //  sqlStr.Append("SELECT yyxmbm AS 中心编码,");
              //  sqlStr.Append("yyxmmc AS 中心名称,");
              //  sqlStr.Append("zfbl AS 自付比例,");
              //  sqlStr.Append("jxm AS 剂型,");
              //  sqlStr.Append("sm AS 自付比例说明,");
              //  // sqlStr.Append("pyqt AS 拼音全拼,");
              //  sqlStr.Append("pyjp AS 拼音简拼");
              //  sqlStr.Append(" FROM   REPORT.dbo.yyxmCs");
              //  sqlStr.Append(" WHERE   yyxmbm LIKE '%" + zxxm + "%'");
              //  sqlStr.Append(" OR yyxmmc LIKE '%" + zxxm + "%'");
              //  sqlStr.Append(" OR pyqt LIKE '%" + zxxm + "%'");
              //  sqlStr.Append(" OR pyjp LIKE '%" + zxxm + "%'");

              //  sqlStr.Append(" SELECT  NETWORK_ITEM_CODE 医保项目编码, ");
              //  sqlStr.Append(" NETWORK_ITEM_NAME 医保项目名称, ");
              //  //sqlStr.Append(" --b.医保项目编码 ,");
              //  sqlStr.Append(" ISNULL(b.首先自付比例,'0') 自付比例,");
              //  sqlStr.Append(" A.TYPE_MEMO 报销等级 ");
              //  //sqlStr.Append(" --a.MEMO2 剂型码 ");
              //  //sqlStr.Append(" --,A.NETWORK_ITEM_CHARGE_CLASS 收费类别 ");
              //  //sqlStr.Append(" --,A.MEMO1 药品材料区分标志 ");
              //  //sqlStr.Append(" -- ISNULL(b.医保项目编码,'') ");
              // // sqlStr.Append(" --,c.mc 剂型码 ");
              //  sqlStr.Append(" FROM    COMM.DICT.NETWORKING_CENTER_DRUG_DICT A ");
              //  sqlStr.Append(" LEFT JOIN Report.dbo.ZXML_ZFBL b ON b.医保项目编码 = A.NETWORK_ITEM_CODE ");
              ////  sqlStr.Append(" --LEFT JOIN Report.dbo.sjzd c ON C.bh=a.MEMO2 ");
              //  sqlStr.Append(" WHERE ");
              //  sqlStr.Append(" A.NETWORK_ITEM_CODE LIKE '%" + zxxm + "%'");
              //  sqlStr.Append(" OR ");
              //  sqlStr.Append(" A.NETWORK_ITEM_NAME LIKE '%" + zxxm + "%'");
              // // sqlStr.Append(" --and c.bz=4 ");
              //  sqlStr.Append(" ORDER  BY NETWORK_ITEM_CODE ");

                sqlStr.Append(" SELECT  NETWORK_ITEM_CODE 医保项目编码, ");
                sqlStr.Append(" NETWORK_ITEM_NAME 医保项目名称, ");

                sqlStr.Append(" ISNULL(b.首先自付比例,'0') 自付比例,");
                sqlStr.Append(" A.TYPE_MEMO 报销等级 ");
                sqlStr.Append(" ,MAX(a.MEMO2) AS  中心剂型马, J.zxjxma AS  中心剂型,MAX(A.DRUG_START_TIME)   as 起始日期,MAX(DRUG_END_TIME) AS 终止日期 ");
                sqlStr.Append(" FROM    COMM.DICT.NETWORKING_CENTER_DRUG_DICT A ");
                sqlStr.Append(" LEFT JOIN Report.dbo.ZXML_ZFBL b ON b.医保项目编码 = A.NETWORK_ITEM_CODE ");
                sqlStr.Append("  LEFT  JOIN  Report.dbo.NETWORK_DW_JXM   J ON  a.MEMO2=J.zxjxm ");

                sqlStr.Append(" WHERE ");
                sqlStr.Append(" A.NETWORK_ITEM_CODE LIKE '%" + zxxm + "%'");
                sqlStr.Append(" OR ");
                sqlStr.Append(" A.NETWORK_ITEM_NAME LIKE '%" + zxxm + "%'");

                //sqlStr.Append(" ORDER  BY NETWORK_ITEM_CODE ");
                sqlStr.Append(" group BY  NETWORK_ITEM_CODE,NETWORK_ITEM_NAME,b.首先自付比例,A.TYPE_MEMO,J.zxjxma ");

            }



            if (fylx == "2")
            {
                //sqlStr.Append("SELECT 中心编码,中心名称,住院自付比例,报销等级,中心收费类别,剂型,中心项目属性,拼音码,价格 FROM (");
                //sqlStr.Append(" SELECT   NETWORK_ITEM_CODE AS 中心编码 ,NETWORK_ITEM_NAME AS 中心名称 , CHARGE_ZY_SELF_BURRDEN_RATIO AS 住院自付比例,");
                //sqlStr.Append(" TYPE_MEMO AS 报销等级 ,NETWORK_ITME_CHARGE_CLASS AS 中心收费类别 ,'' as 剂型,'诊疗' as 中心项目属性,CHARGE_INPUT_CODE_PY AS 拼音码, CASE WHEN CHARGE_HIGHESET_PRICE='' OR CHARGE_HIGHESET_PRICE IS NULL THEN '0' end  AS 价格 ");
                //sqlStr.Append(" FROM COMM.DICT.NETWORKING_CENTER_CHARGE_DICT WHERE NETWORKING_PAT_CLASS_ID ='2' AND (CHARGE_INPUT_CODE_PY LIKE '%" + zxxm + "%' OR NETWORK_ITEM_NAME LIKE '%" + zxxm + "%' OR NETWORK_ITEM_CODE LIKE '%" + zxxm + "%' OR CHARGE_INPUT_CODE_PY LIKE '%" + zxxm + "%')");
                //sqlStr.Append(") aa ORDER BY 中心项目属性 ASC,charindex('" + zxxm + "',拼音码),charindex('" + zxxm + "',中心名称),LEN(拼音码)");

                //sqlStr.Append("SELECT yyxmbm AS 中心编码,");
                //sqlStr.Append("yyxmmc AS 中心名称,");
                //sqlStr.Append("zfbl AS 自付比例,");
                //sqlStr.Append("jxm AS 剂型,");
                //sqlStr.Append("sm AS 自付比例说明,");
                ////sqlStr.Append("pyqt AS 拼音全拼,");
                //sqlStr.Append("pyjp AS 拼音简拼");
                //sqlStr.Append(" FROM   REPORT.dbo.yyxmCs");
                //sqlStr.Append(" WHERE   yyxmbm LIKE '%" + zxxm + "%'");
                //sqlStr.Append(" OR yyxmmc LIKE '%" + zxxm + "%'");
                //sqlStr.Append(" OR pyqt LIKE '%" + zxxm + "%'");
                //sqlStr.Append(" OR pyjp LIKE '%" + zxxm + "%'");

                //旧备份
                sqlStr.Append(" SELECT  NETWORK_ITEM_CODE 医保项目编码, ");
                sqlStr.Append(" NETWORK_ITEM_NAME 医保项目名称, ");
                // sqlStr.Append(" --b.医保项目编码 ,");
                sqlStr.Append(" ISNULL(b.首先自付比例,'0') 自付比例,");
                sqlStr.Append(" A.TYPE_MEMO 报销等级,MAX(A.MEMO4) AS 中心剂型,MAX(A.CHARGE_OPERATE_START_TIME) AS  起始时间, MAX(A.CHARGE_OPERATE_END_TIME) AS 终止时间 ");
                // sqlStr.Append(" --a.MEMO2 剂型码 ");
                // sqlStr.Append(" --,A.NETWORK_ITEM_CHARGE_CLASS 收费类别 ");
                // sqlStr.Append(" --,A.MEMO1 药品材料区分标志 ");
                // sqlStr.Append(" -- ISNULL(b.医保项目编码,'') ");
                //  sqlStr.Append(" --,c.mc 剂型码 ");
                sqlStr.Append(" FROM    COMM.DICT.NETWORKING_CENTER_CHARGE_DICT A ");
                sqlStr.Append(" LEFT JOIN Report.dbo.ZXML_ZFBL b ON b.医保项目编码 = A.NETWORK_ITEM_CODE ");
                //  sqlStr.Append(" --LEFT JOIN Report.dbo.sjzd c ON C.bh=a.MEMO2 ");
                sqlStr.Append(" WHERE ");
                sqlStr.Append(" A.NETWORK_ITEM_CODE LIKE '%" + zxxm + "%'");
                sqlStr.Append(" OR ");
                sqlStr.Append(" A.NETWORK_ITEM_NAME LIKE '%" + zxxm + "%'");
                //   sqlStr.Append(" --and c.bz=4 ");
                //sqlStr.Append(" ORDER  BY NETWORK_ITEM_CODE ");
                sqlStr.Append(" group BY  NETWORK_ITEM_CODE,NETWORK_ITEM_NAME,b.首先自付比例,A.TYPE_MEMO ");

                //新
                //sqlStr.Append(" SELECT  NETWORK_ITEM_CODE 医保项目编码, ");
                //sqlStr.Append(" NETWORK_ITEM_NAME 医保项目名称, ");

                //sqlStr.Append(" ISNULL(b.首先自付比例,'0') 自付比例,");
                //sqlStr.Append(" A.TYPE_MEMO 报销等级 ");

                //sqlStr.Append(" FROM    COMM.DICT.NETWORKING_CENTER_DRUG_DICT A ");
                //sqlStr.Append(" LEFT JOIN Report.dbo.ZXML_ZFBL b ON b.医保项目编码 = A.NETWORK_ITEM_CODE ");

                //sqlStr.Append(" WHERE ");
                //sqlStr.Append(" A.NETWORK_ITEM_CODE LIKE '%" + zxxm + "%'");
                //sqlStr.Append(" OR ");
                //sqlStr.Append(" A.NETWORK_ITEM_NAME LIKE '%" + zxxm + "%'");

                ////sqlStr.Append(" ORDER  BY NETWORK_ITEM_CODE ");
                //sqlStr.Append(" group BY  NETWORK_ITEM_CODE,NETWORK_ITEM_NAME,b.首先自付比例,A.TYPE_MEMO ");
            }
            if (fylx == "3")
            {
                //sqlStr.Append("SELECT 中心编码,中心名称,住院自付比例,报销等级,中心收费类别,剂型,中心项目属性,拼音码,价格 FROM (");
                //sqlStr.Append(" SELECT   NETWORK_ITEM_CODE AS 中心编码 ,NETWORK_ITEM_NAME AS 中心名称 , CHARGE_ZY_SELF_BURRDEN_RATIO AS 住院自付比例,");
                //sqlStr.Append(" TYPE_MEMO AS 报销等级 ,NETWORK_ITME_CHARGE_CLASS AS 中心收费类别 ,'' as 剂型,'诊疗' as 中心项目属性,CHARGE_INPUT_CODE_PY AS 拼音码, CASE WHEN CHARGE_HIGHESET_PRICE='' OR CHARGE_HIGHESET_PRICE IS NULL THEN '0' end  AS 价格 ");
                //sqlStr.Append(" FROM COMM.DICT.NETWORKING_CENTER_CHARGE_DICT WHERE NETWORKING_PAT_CLASS_ID ='2' AND (CHARGE_INPUT_CODE_PY LIKE '%" + zxxm + "%' OR NETWORK_ITEM_NAME LIKE '%" + zxxm + "%' OR NETWORK_ITEM_CODE LIKE '%" + zxxm + "%' OR CHARGE_INPUT_CODE_PY LIKE '%" + zxxm + "%')");
                //sqlStr.Append(") aa ORDER BY 中心项目属性 ASC,charindex('" + zxxm + "',拼音码),charindex('" + zxxm + "',中心名称),LEN(拼音码)");

                //sqlStr.Append("SELECT yyxmbm AS 中心编码,");
                //sqlStr.Append("yyxmmc AS 中心名称,");
                //sqlStr.Append("zfbl AS 自付比例,");
                //sqlStr.Append("jxm AS 剂型,");
                //sqlStr.Append("sm AS 自付比例说明,");
                ////sqlStr.Append("pyqt AS 拼音全拼,");
                //sqlStr.Append("pyjp AS 拼音简拼");
                //sqlStr.Append(" FROM   REPORT.dbo.yyxmCs");
                //sqlStr.Append(" WHERE   yyxmbm LIKE '%" + zxxm + "%'");
                //sqlStr.Append(" OR yyxmmc LIKE '%" + zxxm + "%'");
                //sqlStr.Append(" OR pyqt LIKE '%" + zxxm + "%'");
                //sqlStr.Append(" OR pyjp LIKE '%" + zxxm + "%'");

                sqlStr.Append(" SELECT  NETWORK_ITEM_CODE 医保项目编码, ");
                sqlStr.Append(" NETWORK_ITEM_NAME 医保项目名称, ");
               // sqlStr.Append(" --b.医保项目编码 ,");
                sqlStr.Append(" ISNULL(b.首先自付比例,'0') 自付比例,");
                sqlStr.Append(" A.TYPE_MEMO 报销等级 ");
               // sqlStr.Append(" --a.MEMO2 剂型码 ");
               // sqlStr.Append(" --,A.NETWORK_ITEM_CHARGE_CLASS 收费类别 ");
               // sqlStr.Append(" --,A.MEMO1 药品材料区分标志 ");
               // sqlStr.Append(" -- ISNULL(b.医保项目编码,'') ");
               // sqlStr.Append(" --,c.mc 剂型码 ");
                sqlStr.Append(" FROM    COMM.DICT.NETWORKING_CENTER_DRUG_DICT A ");
                sqlStr.Append(" LEFT JOIN Report.dbo.ZXML_ZFBL b ON b.医保项目编码 = A.NETWORK_ITEM_CODE ");
               // sqlStr.Append(" --LEFT JOIN Report.dbo.sjzd c ON C.bh=a.MEMO2 ");
                sqlStr.Append(" WHERE ");
                sqlStr.Append(" A.NETWORK_ITEM_CODE LIKE '%" + zxxm + "%'");
                sqlStr.Append(" OR ");
                sqlStr.Append(" A.NETWORK_ITEM_NAME LIKE '%" + zxxm + "%'");
              //  sqlStr.Append(" --and c.bz=4 ");
                //sqlStr.Append(" ORDER  BY NETWORK_ITEM_CODE ");
                sqlStr.Append(" group BY  NETWORK_ITEM_CODE,NETWORK_ITEM_NAME,b.首先自付比例,A.TYPE_MEMO ");
            }
            DataSet ds = new DataSet();
            DataTable zxdata = new DataTable();
            ds = SQLHelper.ExecSqlReDs(sqlStr.ToString());
            zxdata = ds.Tables[0];
            dgv_zx.DataSource = zxdata;
        }
        #endregion



        #region his项目检索回车事件
        /// <summary>
        /// his项目检索回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_hisxm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.btn_His_CX_Click(sender, e);//触发button事件 
            }
        }
        #endregion
        #region 中心项目检索回车事件
        /// <summary>
        /// 中心项目检索回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_zxxm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.btn_ZXCX_Click(sender, e);//触发button事件 ‘
            }
        }
        #endregion
        #region 是否显示取消对照
        /// <summary>
        /// 是否显示取消对照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb_DZLX_SelectedValueChanged(object sender, EventArgs e)
        {


            if (cmb_DZLX.SelectedValue.ToString() == "1")
            {
                this.btn_Qxdz.Enabled = false;
                this.btn_DZSC.Enabled = false;
                this.btn_DZ.Enabled = true;
            }
            else if (cmb_DZLX.SelectedValue.ToString() == "2")
            {
                this.btn_Qxdz.Enabled = true;
                this.btn_DZ.Enabled = false;
                this.btn_DZSC.Enabled = true;
            }
            else
            {
                this.btn_Qxdz.Enabled = true;
                this.btn_DZ.Enabled = false;
                this.btn_DZSC.Enabled = false;
            }
        }
        #endregion
        #region 项目对照
        /// <summary>
        /// 项目对照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //中心项目编码  
        //Network_item_code
        //中心项目名称  
        //Network_item_name
        //中心收费类别 
        //Network_item_charge_class
        //中心项目类别 
        //Item_prop
        //定点医疗机构项目编码
        //his_item_code
        //定点医疗机构项目名称
        //his_item_name
        //定点医疗机构项目价格
        //国药准字 
        private void btn_DZ_Click(object sender, EventArgs e)
        { 
            string strZXCode = ""; //中心项目编码
            string strZXName = ""; //中心项目名称
            string NETWORK_ITEM_CHARGE_CLASS ="";//中心收费分类
            string NETWORK_ITEM_PROP = "";//药品诊疗
            string strHisName = "";//his名称
            string strHisCode = "";//his编码
            string Medicine = "";//国药
            string leibie = "";
            string NETWORK_ITEM_PRICE = "";//价格
            string Zfblzx = ""; //中心字符比例
            string TypeMemo = "";
            string GuiGe = ""; //规格
            string DanWei = "";// 单位

            if (this.dgv_zx.Rows.Count == 0)
            {
                MessageBox.Show("请选择中心项目！");
                return;
            }
            if (this.dgv_his.Rows.Count == 0)
            {
                MessageBox.Show("请选择HIS项目！");
                return;
            }
           

            DataGridViewRow dgrZX = this.dgv_zx.Rows[this.dgv_zx.CurrentRow.Index];
            strZXCode = dgrZX.Cells["医保项目编码"].Value.ToString();
            strZXName = dgrZX.Cells["医保项目名称"].Value.ToString();
            //if (fylx.Equals("1")) {
            //    Medicine = dgrZX.Cells["国药准字"].Value.ToString();             
            //}
            if (dgrZX.Cells["自付比例"].Value.ToString()=="0")
            {
                TypeMemo = "甲";
            }
            else if (dgrZX.Cells["自付比例"].Value.ToString() == "100")
            {
                TypeMemo = "自费";
            }
            else
            {
                TypeMemo = "乙";    //报销等级
            }

            //leibie = dgrZX.Cells["报销等级"].Value.ToString();//报销等级
            fylx = cmb_FYLX.SelectedValue.ToString(); //费用类型
           
            //NETWORK_ITEM_CHARGE_CLASS = dgrZX.Cells["中心收费类别"].Value.ToString();
            Zfblzx= dgrZX.Cells["自付比例"].Value.ToString();

            DataGridViewRow dgrHis = this.dgv_his.Rows[this.dgv_his.CurrentRow.Index];
            
            strHisCode = dgrHis.Cells["编码"].Value.ToString();
            // NETWORK_ITEM_PROP = dgrHis.Cells["药品诊疗"].Value.ToString() == "药品" ? "1" : "2"; //
            //NETWORK_ITEM_PROP = dgrHis.Cells["药品诊疗"].Value.ToString();
            NETWORK_ITEM_PRICE = dgrHis.Cells["价格"].Value.ToString();
            GuiGe = dgrHis.Cells["规格"].Value.ToString();
            DanWei = dgrHis.Cells["单位"].Value.ToString();

            if (dgrHis.Cells["药品诊疗"].Value.ToString()=="1")
            {
               // strHisName ="Y"+ dgrHis.Cells["名称"].Value.ToString();
                strHisName =  dgrHis.Cells["名称"].Value.ToString();

                NETWORK_ITEM_PROP = "1";
            }
            else if (dgrHis.Cells["药品诊疗"].Value.ToString()=="2")
            {
                strHisName = dgrHis.Cells["名称"].Value.ToString();

               // strHisName = "L" + dgrHis.Cells["名称"].Value.ToString();
                NETWORK_ITEM_PROP = "2";
            }
            else if (dgrHis.Cells["药品诊疗"].Value.ToString() == "3")
            {
                strHisName = dgrHis.Cells["名称"].Value.ToString();

                //strHisName = "C" + dgrHis.Cells["名称"].Value.ToString();
                NETWORK_ITEM_PROP = "1";
            }
            StringBuilder Sqlstringqc = new StringBuilder();

            Sqlstringqc.Append(" SELECT * FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE   NETWORKING_PAT_CLASS_ID='3 '  AND  HIS_ITEM_CODE='" + strHisCode + "'");
            DataSet dtc = SQLHelper.ExecSqlReDs(Sqlstringqc.ToString());
            if (dtc.Tables[0].Rows.Count>=1)
            {
                MessageBox.Show("该编码已存在！");
                return;
            }
          //  DataTable dtc1 = new DataTable();
           

            StringBuilder Sqlstring = new StringBuilder();        

            Sqlstring.Append("INSERT INTO COMM.COMM.NETWORKING_ITEM_VS_HIS");
            Sqlstring.Append("(NETWORKING_PAT_CLASS_ID,");
            Sqlstring.Append("ITEM_PROP,");
            Sqlstring.Append("HIS_ITEM_CODE,");
            Sqlstring.Append("HIS_ITEM_NAME,");
            Sqlstring.Append("NETWORK_ITEM_CODE,");
            Sqlstring.Append("NETWORK_ITEM_NAME,");
            Sqlstring.Append("SELF_BURDEN_RATIO,");
            Sqlstring.Append("MEMO,");
            Sqlstring.Append("START_TIME,");
            Sqlstring.Append("STOP_TIME,");
            Sqlstring.Append("TYPE_MEMO,");
            Sqlstring.Append("NETWORK_ITEM_PROP,");
            Sqlstring.Append("NETWORK_ITEM_CHARGE_CLASS,");
            Sqlstring.Append("HOSPITAL_ID,");
            Sqlstring.Append("NETWORK_ITEM_PRICE,");
            Sqlstring.Append("FLAG_DISABLED,");
            Sqlstring.Append("NETWORK_ITEM_FLAG_UP");
            Sqlstring.Append(")");
            Sqlstring.Append("VALUES( 3,");
            Sqlstring.Append(" '" + NETWORK_ITEM_PROP + "',");
            Sqlstring.Append(" '" + strHisCode + "',");
            Sqlstring.Append(" '" + strHisName + "',");
            Sqlstring.Append(" '" + strZXCode + "',");
            Sqlstring.Append(" '" + strZXName + "',");
            Sqlstring.Append(" '" + Zfblzx + "',");
            Sqlstring.Append(" '" + GuiGe + "|"+ DanWei +"',"); //国药
            Sqlstring.Append(" GETDATE(),");
            Sqlstring.Append(" GETDATE(),");
            Sqlstring.Append(" '"+TypeMemo+"',");
            Sqlstring.Append(" '" + NETWORK_ITEM_PROP + "',");
            Sqlstring.Append("'',");
            Sqlstring.Append(" 1,");
            Sqlstring.Append(" '" + NETWORK_ITEM_PRICE + "',");
            Sqlstring.Append(" 0, ");
            Sqlstring.Append(" 0 ");
            Sqlstring.Append(" ) ");

            int Num = SQLHelper.ExecSqlReInt(Sqlstring.ToString());
            if (Num > 0)
            {
                MessageBox.Show("对照成功！");
                dgv_his.Rows.RemoveAt(dgv_his.CurrentRow.Index);
                return;
            }
        }
        #endregion


        #region 取消对照
        /// <summary>
        /// 取消对照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Qxdz_Click(object sender, EventArgs e)
        {
            string strHisName = "";
            string strHisCode = "";

            DataGridViewRow dgrHis = this.dgv_his.Rows[this.dgv_his.CurrentRow.Index];
          strHisName = dgrHis.Cells["名称"].Value.ToString();
            strHisCode = dgrHis.Cells["编码"].Value.ToString();
            string m_szstr = "select NETWORK_ITEM_FLAG_UP from COMM.COMM.NETWORKING_ITEM_VS_HIS where HIS_ITEM_CODE = '" + strHisCode + "' AND NETWORKING_PAT_CLASS_ID='3'";
            DataSet ds = SQLHelper.ExecSqlReDs(m_szstr);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            //string m_szTstr = dt.Rows[0]["NETWORK_ITEM_FLAG_UP"].ToString();
            //if (m_szTstr.Equals("1")) {
            //    MessageBox.Show("该项目已经上传请先撤销上传");
            //    return;
            //}
            StringBuilder Sqlstring = new StringBuilder();
            Sqlstring.Append(" DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE HIS_ITEM_CODE='" + strHisCode + "' AND NETWORKING_PAT_CLASS_ID='3 ' ");

            int Num = SQLHelper.ExecSqlReInt(Sqlstring.ToString());
            if (Num > 0)
            {
                MessageBox.Show("取消对照成功！");
                this.dgv_his.Rows.RemoveAt(this.dgv_his.CurrentRow.Index);
                return;
            }
        }
        #endregion


        #region 对照上传  未用 屏蔽
        /// <summary>
        /// 对照上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DZSC_Click(object sender, EventArgs e)
        {
            //fylx = cmb_FYLX.SelectedValue.ToString(); //费用类型;
            //DataTable detailTable = dgv_his.DataSource as DataTable;
            //StringBuilder strERR = new StringBuilder();
            //StructMethod();
            //for (int i = 0; i < detailTable.Rows.Count; i++)
            //{
            //    neusoft = new LYNeoSoftHandle("3130"); //4.4.3	对照信息批量上传(3130)    
            //    neusoft.AddListInParas(detailTable.Rows[i]["收费项目种类"].ToString());//项目类别	VARCHAR2(3)	NOT NULL	1药品、2诊疗项目、3服务设施
            //    neusoft.AddListInParas(detailTable.Rows[i]["中心编码"].ToString());//三大目录中心编码	VARCHAR2(20)	NOT NULL	项目中心编码
            //    neusoft.AddListInParas(detailTable.Rows[i]["医院编码"].ToString());//医院项目内码	VARCHAR2(20)	NOT NULL
            //    neusoft.AddListInParas(detailTable.Rows[i]["医院名称"].ToString());//医院项目名称	VARCHAR2(50)	NOT NULL
            //    neusoft.AddListInParas(""); //定点医疗机构药品剂型	VARCHAR2(20)
            //    neusoft.AddListInParas(""); //单位	VARCHAR2(10)
            //    neusoft.AddListInParas(""); //规格	VARCHAR2(14)
            //    neusoft.AddListInParas(""); //医院端价格	VARCHAR2(16)		2位小数
            //    neusoft.AddListInParas(""); //医院端产地	VARCHAR2(50)
            //    neusoft.AddListInParas(""); //对照经办人	VARCHAR2(20)
            //    neusoft.AddListInParas(DateTime.Now.ToString("yyyyMMddHHmmss"));//对照操作时间	VARCHAR2(14)		YYYYMMDDHH24MISS 
            //    neusoft.AddListInParas("20111001000000");//开始日期	VARCHAR2(14)		YYYYMMDDHH24MISS
            //    neusoft.AddListInParas("20991001000000");//终止日期	VARCHAR2(14)		YYYYMMDDHH24MIS 20000101000000

            //    try
            //    {
            //        neusoft.NeusoftHandle();//东软上传费用对照关系
            //        StringBuilder gxbd = new StringBuilder();
            //        gxbd.Append(" UPDATE COMM.COMM.NETWORKING_ITEM_VS_HIS SET NETWORK_ITEM_FLAG_UP='1' WHERE NETWORKING_PAT_CLASS_ID='1' AND HIS_ITEM_CODE='" + detailTable.Rows[i]["医院编码"].ToString() + "' ");
            //        int h = SQLHelper.ExecSqlReInt(gxbd.ToString());
            //    }
            //    catch (Exception ex)
            //    {
            //        strERR.Append(ex.ToString());
            //        continue;
            //    }
            //}
            //if (strERR.Length > 0)
            //{
            //    MessageBox.Show("部分费用上传失败!" + strERR.ToString());
            //}
            //MessageBox.Show("上传完成！");
        }
        #endregion

        private void dgv_his_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //txt_zxxm.Text = dgv_his.SelectedRows[0].Cells["拼音码"].Value.ToString();
        }

        private void txt_zxxm_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_hisxm_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgv_zx_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void catalog_correspond_Load(object sender, EventArgs e)
        {

        }

        private void catalog_correspond_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
        }

        private void cmb_DZLX_SelectedIndexChanged(object sender, EventArgs e)
        {
            GX();
        }
    }
}