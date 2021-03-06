﻿using Malkavian.DButility;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DW_YBBX.ZX_Business
{
    public partial class FrmCeneter : Form
    {
        private DW_Handle handelModel;
        //  SqlHelper sqlcenterName = new SqlHelper(ConfigurationManager.ConnectionStrings["connStr"].ToString());
        public MSSQLHelpers SQLHelper = new MSSQLHelpers(ConfigurationManager.ConnectionStrings["connStr"].ToString());
        public string uploadHC = "";
        public string ptmz_ID = ConfigurationManager.AppSettings["PTMZ_PATID"].ToString();
        public FrmCeneter()
        {
            InitializeComponent();
            //var materialSkinManager = MaterialSkinManager.Instance;
            //materialSkinManager.AddFormToManage(this);
            //materialSkinManager.Theme = MaterialSkinManager.Themes.Dark;
            //materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.White);

            // materialRaisedButton7.BackColor = System.Drawing.Color.Red;
        }

        private void materialRaisedButton4_Click(object sender, EventArgs e)
        {
            #region 居民门诊统筹
            JmMzTCUP();


            #endregion

            #region 职工门诊统筹更新

            ZgMzTcUp();


            #endregion


            // MessageBox.Show("更新居民统筹自负比例成功！");

        }


        /// <summary>
        /// 职工门诊统筹
        /// </summary>
        private void ZgMzTcUp()
        {
            handelModel = new DW_Handle();
            string sbjgbh = "370100";
            DateTime rq = DateTime.Today;
            string sqlstr = @"SELECT NETWORK_ITEM_CODE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORK_ITEM_CODE  LIKE '%YB_%' AND NETWORK_ITEM_CODE NOT LIKE '0000%' AND NETWORKING_PAT_CLASS_ID=" + ptmz_ID + " And HOSPITAL_ID='" + MainForm.HOSPITAL_ID + "' ";
            //DataSet ds = SQLHelper.ExecSqlReDs(sqlstr.ToString());
            DataTable dt = SQLHelper.ExecSqlReDs(sqlstr.ToString()).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string yyxmbm = dt.Rows[i]["NETWORK_ITEM_CODE"].ToString();
                handelModel.Down_Yyxm_ZFBL(sbjgbh, yyxmbm, rq.ToString("yyyy-MM-dd"), "bcrylb#1|sbjglx#A|");
            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 居民门诊统筹更新 
        /// </summary>
        private void JmMzTCUP()
        {
            handelModel = new DW_Handle();
            string sbjgbh = MainForm.sbjgbh;

            if (sbjgbh == "370100")
            {
                MessageBox.Show("未选择居民经办机构，请重新登陆选择居民经办机构！");
                return;
            }
            DateTime rq = DateTime.Today;
            string jmtc_patid = ConfigurationManager.AppSettings["JMTC_PATID"].ToString();


            string sqlstr = @"SELECT NETWORK_ITEM_CODE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORK_ITEM_CODE  LIKE '%YB_%' AND NETWORK_ITEM_CODE NOT LIKE '0000%' AND NETWORKING_PAT_CLASS_ID=" + ptmz_ID + " And HOSPITAL_ID='" + MainForm.HOSPITAL_ID + "' ";
            //DataSet ds = SQLHelper.ExecSqlReDs(sqlstr.ToString());
            DataTable dt = SQLHelper.ExecSqlReDs(sqlstr.ToString()).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string yyxmbm = dt.Rows[i]["NETWORK_ITEM_CODE"].ToString();
                handelModel.Down_Yyxm_ZFBLJm(sbjgbh, yyxmbm, rq.ToString("yyyy-MM-dd"), "bcrylb#1|sbjglx#B|", jmtc_patid, MainForm.HOSPITAL_ID, "C", "B");
            }
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {

        }

        private void materialRaisedButton8_Click(object sender, EventArgs e)
        {

        }

        private void materialRaisedButton7_Click(object sender, EventArgs e)
        {

        }

        private void materialTabSelector1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.Text == "Pass")
            {
                this.materialRaisedButton8.Visible = true;
                this.materialRaisedButton1.Visible = true;
            }
        }

        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
            PreProcess();
            try
            {

                UpHisDrug();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                ///---等待的业务

                //   MessageBox.Show("下载核心目录成功！地址：" + Environment.CurrentDirectory + @"\YBDLOAD\核心目录.txt" + @"\YBDLOAD\核心目录自付比例.txt", "提示", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                // Application.DoEvents();

                EndProcessNews();
            }
        }

        private void UpHisDrug()
        {

            //  FrmFourYBLY fgybly = new FrmFourYBLY();
            //fgybly.ShowDialog();
            //string sqlDrug = " DELETE   from COMM.COMM.NETWORKING_ITEM_VS_HIS where   NETWORK_ITEM_CHARGE_CLASS=1 and HOSPITAL_ID = '" + MainForm.HOSPITAL_ID + "'";
            string sqlDrug2 = " DELETE  from REPORT.dbo.upfail";
            //SQLHelper.ExecSqlReDs(sqlDrug.ToString());
            SQLHelper.ExecSqlReDs(sqlDrug2.ToString());

            Text = "药品正在上传中，请稍等。。。。。";
            string networkPatclassID = "";//MainForm.NETWORKING_PAT_CLASS_ID;

            networkPatclassID = ConfigurationManager.AppSettings["PTMZ_PATID"].ToString();//MainForm.NETWORKING_PAT_CLASS_ID;
            string hosId = MainForm.HOSPITAL_ID;
            //fgybly.Close();

            FrmMaxAndMin fMA = new FrmMaxAndMin();
            fMA.ShowDialog();
            string fmaType = fMA.priceType;

            //保存医保对照关系，默认NETWORKING_ITEM_CODE为0000
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append(" INSERT INTO COMM.COMM.NETWORKING_ITEM_VS_HIS ");
            sqlStr.Append(" ( NETWORKING_PAT_CLASS_ID , ");
            sqlStr.Append(" ITEM_PROP , ");
            sqlStr.Append(" HIS_ITEM_CODE , ");
            sqlStr.Append(" HIS_ITEM_NAME , ");
            sqlStr.Append(" NETWORK_ITEM_CODE , ");
            sqlStr.Append(" NETWORK_ITEM_NAME , ");
            sqlStr.Append(" SELF_BURDEN_RATIO , ");
            sqlStr.Append(" MEMO , ");
            sqlStr.Append(" START_TIME , ");
            sqlStr.Append(" STOP_TIME , ");
            sqlStr.Append(" TYPE_MEMO , ");
            sqlStr.Append(" NETWORK_ITEM_PROP , ");
            sqlStr.Append(" NETWORK_ITEM_CHARGE_CLASS , ");
            sqlStr.Append(" HOSPITAL_ID ,");
            sqlStr.Append(" NETWORK_ITEM_PRICE , ");
            sqlStr.Append(" FLAG_DISABLED , ");
            sqlStr.Append(" NETWORK_ITEM_FLAG_UP ");
            sqlStr.Append(") ");



            //3 和1    要区分好 
            sqlStr.Append("SELECT    '" + networkPatclassID + "', ");
            sqlStr.Append("      '1', ");

            //sqlStr.Append("           CASE WHEN  aa.CHARGE_TYPE<10 THEN  '3' ELSE '1' END, ");
            sqlStr.Append("            aa.编码, ");
            sqlStr.Append("          AA.名称, ");
            sqlStr.Append("     '0000', ");
            sqlStr.Append("    '未对照', ");
            sqlStr.Append("   '0', ");
            sqlStr.Append("    '',  ");
            sqlStr.Append("   GETDATE(), ");
            sqlStr.Append("   GETDATE(), ");
            sqlStr.Append("   '',  ");
            sqlStr.Append("   '1', ");
            sqlStr.Append("   CASE WHEN  aa.CHARGE_TYPE<10 THEN  '3' ELSE '1' END,  ");
            sqlStr.Append("   '" + hosId + "', ");
            sqlStr.Append("  CASE  WHEN AA.价格 = -1 THEN 0 ELSE  AA.价格 END, ");
            sqlStr.Append("  '0', ");
            sqlStr.Append("  '0'  ");
            sqlStr.Append("   FROM ( SELECT    MAX(A.DRUG_CODE) AS 编码 , ");
            sqlStr.Append("   MAX(B.DRUG_NAME) AS 名称 , ");
            sqlStr.Append("  MAX(B.DRUG_SPEC) AS 规格 ,  ");
            sqlStr.Append("   MAX(e.MEASURE_UNIT_NAME) AS 单位 , ");
            sqlStr.Append("  MAX(F.DRUG_FORM_NAME) AS 剂型 , ");

            if (fmaType == "MAX")
            {
                sqlStr.Append("  MAX(A.RETAIL_PRICE) AS 价格 ,  ");

            }
            else
            {
                sqlStr.Append("  MIN(A.RETAIL_PRICE) AS 价格 ,  ");

            }
            sqlStr.Append("  MAX(G.PRODUC_AREA_NAME) AS HIS产地 ");
            sqlStr.Append("  ,MAX(b.CHARGE_TYPE) AS CHARGE_TYPE ");

            sqlStr.Append("  FROM COMM.COMM.DRUG_PRICE_LIST AS A  ");
            sqlStr.Append("  LEFT JOIN COMM.dict.PRODUC_AREAS G ON G.PRODUC_AREA_ID = A.FIRM_ID ");
            sqlStr.Append("  LEFT JOIN  YP.DRUG.PHARMACY_DRUG_STOCK  H ON H.DRUG_PRICE_ID = A.DRUG_PRICE_ID ");
            sqlStr.Append("  LEFT JOIN YP.DRUG.DRUGS AS B ON A.DRUG_ID = B.DRUG_ID  ");
            sqlStr.Append("  LEFT JOIN COMM.DICT.DRUG_CLASSES AS C ON C.DRUG_CLASS_ID = B.DRUG_CLASS_ID ");
            sqlStr.Append("  LEFT JOIN COMM.DICT.MEASURE_UNITS AS E ON B.MEASURE_UNIT_ID = e.MEASURE_UNIT_ID ");
            sqlStr.Append("   LEFT JOIN COMM.DICT.DRUG_FORMS AS F ON B.DRUG_FORM_ID = f.DRUG_FORM_ID ");
            sqlStr.Append("   WHERE  ");
            sqlStr.Append("   b.CHARGE_TYPE<100 ");
            sqlStr.Append("    AND A.FLAG_INVALID='0' AND  A.DRUG_CODE NOT IN ( ");
            sqlStr.Append("   SELECT HIS_ITEM_CODE  ");
            sqlStr.Append("   FROM COMM.COMM.NETWORKING_ITEM_VS_HIS ");
            sqlStr.Append("   WHERE ITEM_PROP in ('1') ");
            sqlStr.Append("    AND NETWORKING_PAT_CLASS_ID = '" + networkPatclassID + "' ");
            sqlStr.Append("    AND NETWORK_ITEM_CODE <> ''  and HOSPITAL_ID='" + hosId + "' ) ");
            sqlStr.Append("   AND A.DRUG_ID > 0  ");
            sqlStr.Append("   AND A.DRUG_CODE <> '' ");
            sqlStr.Append("   AND A.RETAIL_PRICE<> -1 ");
            sqlStr.Append("  AND HOSPITAL_ID = '" + hosId + "' ");
            sqlStr.Append(" AND B.FLAG_INVALID = 0 ");
            sqlStr.Append("  GROUP BY  A.DRUG_CODE,  ");
            sqlStr.Append("   C.DRUG_CLASS_ID  ");
            //正式需修改
            sqlStr.Append("   ) AA  ");

            //sqlStr.Append("   ) AA  where  AA.名称!='海螵蛸颗粒'");

            //sqlStr.Append(" SELECT top 1  '" + networkPatclassID + "', ");
            //sqlStr.Append(" '1', ");
            //sqlStr.Append(" aa.编码, ");
            //sqlStr.Append(" AA.名称, ");
            //sqlStr.Append(" '0000', ");
            //sqlStr.Append(" '未对照', ");
            //sqlStr.Append(" '0', ");
            //sqlStr.Append(" '', ");
            //sqlStr.Append(" GETDATE(), ");
            //sqlStr.Append(" GETDATE(), ");
            //sqlStr.Append(" '', ");
            //sqlStr.Append(" '1', ");
            //sqlStr.Append(" '', ");
            //sqlStr.Append(" " + hosId + ", ");
            //sqlStr.Append(" CASE  WHEN AA.价格 = -1 THEN 0 ELSE  AA.价格 END, ");
            //sqlStr.Append(" '0', ");
            //sqlStr.Append(" '0' ");
            //sqlStr.Append(" FROM ( SELECT    MAX(A.DRUG_CODE) AS 编码 , ");
            //sqlStr.Append(" MAX(B.DRUG_NAME) AS 名称 , ");
            //sqlStr.Append(" MAX(B.DRUG_SPEC) AS 规格 , ");
            //sqlStr.Append(" MAX(e.MEASURE_UNIT_NAME) AS 单位 , ");
            //sqlStr.Append(" MAX(F.DRUG_FORM_NAME) AS 剂型 , ");
            //sqlStr.Append(" MAX(A.RETAIL_PRICE) AS 价格 , ");
            //sqlStr.Append(" MAX(G.PRODUC_AREA_NAME) AS HIS产地 ");
            //sqlStr.Append(" FROM COMM.COMM.DRUG_PRICE_LIST AS A ");
            //sqlStr.Append(" LEFT JOIN COMM.dict.PRODUC_AREAS G ON G.PRODUC_AREA_ID = A.FIRM_ID ");
            //sqlStr.Append(" LEFT JOIN YP.DRUG.DRUG_STOCK H ON H.DRUG_PRICE_ID = A.DRUG_PRICE_ID ");
            //sqlStr.Append(" LEFT JOIN YP.DRUG.DRUGS AS B ON A.DRUG_ID = B.DRUG_ID ");
            //sqlStr.Append(" LEFT JOIN COMM.DICT.DRUG_CLASSES AS C ON C.DRUG_CLASS_ID = B.DRUG_CLASS_ID ");
            //sqlStr.Append(" LEFT JOIN COMM.DICT.MEASURE_UNITS AS E ON B.MEASURE_UNIT_ID = e.MEASURE_UNIT_ID ");
            //sqlStr.Append(" LEFT JOIN COMM.DICT.DRUG_FORMS AS F ON B.DRUG_FORM_ID = f.DRUG_FORM_ID ");
            //sqlStr.Append(" WHERE B.CHARGE_TYPE <> 1 ");
            //sqlStr.Append(" AND A.DRUG_CODE NOT IN ( ");
            //sqlStr.Append(" SELECT HIS_ITEM_CODE ");
            //sqlStr.Append(" FROM COMM.COMM.NETWORKING_ITEM_VS_HIS ");
            //sqlStr.Append(" WHERE ITEM_PROP = '1' ");
            //sqlStr.Append(" AND NETWORKING_PAT_CLASS_ID = '" + networkPatclassID + "' ");
            //sqlStr.Append(" AND HOSPITAL_ID = '" + MainForm.HOSPITAL_ID + "' ");
            //sqlStr.Append(" AND NETWORK_ITEM_CODE <> '' ) ");
            //sqlStr.Append(" AND A.DRUG_ID > 0 ");
            //sqlStr.Append(" AND A.DRUG_CODE <> '' ");
            //sqlStr.Append(" AND A.RETAIL_PRICE<> -1 ");
            //sqlStr.Append(" AND HOSPITAL_ID = '" + hosId + "' ");
            //sqlStr.Append(" AND B.FLAG_INVALID = 0 ");
            //sqlStr.Append(" GROUP BY  A.DRUG_CODE, ");
            //sqlStr.Append(" C.DRUG_CLASS_ID ");
            ////sqlStr.Append(" ) AA "); 原有 正式需恢复
            //sqlStr.Append(" ) AA where AA.名称 like  '%注射器%'");//测试

            SQLHelper.ExecSqlReDs(sqlStr.ToString());

            //去除重复数据
            StringBuilder sqlStr1 = new StringBuilder();
            sqlStr1.Append(" DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS ");
            sqlStr1.Append(" WHERE   HIS_ITEM_CODE IN ( SELECT   HIS_ITEM_CODE ");
            sqlStr1.Append(" FROM     COMM.COMM.NETWORKING_ITEM_VS_HIS ");
            sqlStr1.Append(" WHERE    HOSPITAL_ID = '" + hosId + "' ");
            sqlStr1.Append(" AND NETWORKING_PAT_CLASS_ID=" + networkPatclassID + " ");
            sqlStr1.Append(" AND NETWORK_ITEM_PROP=1 ");
            sqlStr1.Append(" AND NETWORK_ITEM_CODE='0000' ");
            sqlStr1.Append(" GROUP BY HIS_ITEM_CODE ");
            sqlStr1.Append(" HAVING COUNT(HIS_ITEM_CODE) > 1 ");
            sqlStr1.Append(" ) ");
            sqlStr1.Append(" AND HOSPITAL_ID = '" + hosId + "' ");
            sqlStr1.Append(" AND NETWORKING_PAT_CLASS_ID=" + networkPatclassID + " ");
            SQLHelper.ExecSqlReDs(sqlStr1.ToString());
            // DataTable dt2 = SQLHelper.ExecSqlReDs("select * from COMM.COMM.NETWORKING_ITEM_VS_HIS").Tables[0];
            //上传未对照数据至地纬
            StringBuilder yp = new StringBuilder();
            yp.Append("SELECT  'YB_' + a.HIS_ITEM_CODE yyxmbm ,a.NETWORK_ITEM_CHARGE_CLASS  as yltype,a.HIS_ITEM_NAME yyxmmc ,'1' AS ypbz ,a.NETWORK_ITEM_PRICE AS dj , b.SPEC AS zxgg ,1 AS bhsl ,");
            yp.Append("'001' mllb ,'' AS syz ,'' AS jj ,'' AS scqy ,b.SPEC AS zdgg ,'' spm ,b.MEASURE_UNIT_NAME AS dw ,'' AS gmpbz ,'' AS cfybz ,'01' AS mzjsxmbh ,'01' AS zyjsxmbh ,'' AS jxmc ,'' AS kcl FROM COMM.COMM.NETWORKING_ITEM_VS_HIS a INNER JOIN ( SELECT  d.CHARGE_CODE ,d.CHARGE_NAME ,d.SPEC ,d.MEASURE_UNIT_NAME FROM COMM.COMM.CHARGE_PRICE d UNION SELECT  DRUG_CODE ,DRUG_NAME ,DRUG_SPEC ,n.MEASURE_UNIT_NAME FROM YP.DRUG.DRUGS m LEFT JOIN COMM.DICT.MEASURE_UNITS n ON n.MEASURE_UNIT_ID = m.MEASURE_UNIT_ID WHERE m.CHARGE_TYPE!=1 ) b ON a.HIS_ITEM_CODE = b.CHARGE_CODE ");
            yp.Append(" WHERE NETWORKING_PAT_CLASS_ID = '" + networkPatclassID + "' AND NETWORK_ITEM_FLAG_UP = '0' AND a.HOSPITAL_ID = '" + MainForm.HOSPITAL_ID + "' AND a.NETWORK_ITEM_PROP=1  AND a.ITEM_PROP=1   AND  A.NETWORK_ITEM_CHARGE_CLASS='1'   ");
            DataSet up_ds = SQLHelper.ExecSqlReDs(yp.ToString());
            foreach (DataTable dt in up_ds.Tables)
            {
                handelModel = new DW_Handle();
                handelModel.add_yyxm_info_all(dt);
            }

            //MessageBox.Show("药品上传完成");
            Text = "药品上传完成";
        }


        /// <summary>
        /// 耗时业务完成,取消遮盖
        /// </summary>
        private void EndProcessNews()
        {
            lblts.Visible = false;
            panel2.Visible = false;
            //groupBox1.Visible = false;

            Application.DoEvents();
        }


        /// <summary>
        /// 耗时业务置顶遮盖防止误操作
        /// </summary>
        private void PreProcess()
        {
            //Thread fThread2 = new Thread(new ThreadStart(Sleepbtn_DownYBHXM1));

            //fThread2.IsBackground = false;
            //fThread2.Start();
            lblts.Location = ActiveForm.Location;

            lblts.Location = new Point(Convert.ToInt32(panel2.Width - lblts.Width) / 2, Convert.ToInt32(panel2.Height - lblts.Height) / 4);
            //lblts.Width = ActiveForm.Width / 2;
            lblts.Visible = true;
            panel2.Top = 0;// FrmCeneter.ActiveForm.Top;
            panel2.Left = (ActiveForm.Width / 2) - (ActiveForm.Width / 2);
            panel2.Size = ActiveForm.Size;
            panel2.Visible = true;
            panel2.Parent = this;

            //   groupBox1.Visible = true;
            Application.DoEvents();

            //Thread fThread = new Thread(new ThreadStart(Sleepbtn_DownYBHXM));
            //fThread.IsBackground = true;
            //fThread.Start();
        }



        private void materialRaisedButton1_Click_1(object sender, EventArgs e)
        {

            PreProcess();

            try
            {
                handelModel = new DW_Handle();
                string sbjgbh = MainForm.sbjgbh;
                string filename = Environment.CurrentDirectory + @"\YBDLOAD\核心目录.txt";
                string filename2 = Environment.CurrentDirectory + @"\YBDLOAD\核心目录自付比例.txt";
                long filetype = 1;//0:表示excel文件,1:表示txt文件,2:表示csv文件，7:表示dbf2文件，8:表示dbf3文件
                long has_head = 0;
                //由于该目录只获取名字 且地位不校验用户也不看 写死
                sbjgbh = ConfigurationManager.AppSettings["sbjgbh_ZGYB"];
                handelModel.Down_Ml(sbjgbh, filename, filename2, filetype, has_head);
                //Thread fThread = new Thread(new ThreadStart(Sleepbtn_DownYBHXM));
                //fThread.IsBackground = true;
                //fThread.Start();
                MessageBox.Show("下载核心目录成功！地址：" + Environment.CurrentDirectory + @"\YBDLOAD\核心目录.txt" + @"\YBDLOAD\核心目录自付比例.txt", "提示", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                EndProcessNews();
            }
        }

        public void Sleepbtn_DownYBHXM()
        {

            MessageBox.Show("下载核心目录成功！地址：" + Environment.CurrentDirectory + @"\YBDLOAD\核心目录.txt" + @"\YBDLOAD\核心目录自付比例.txt", "提示", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PreProcess();
            try
            {
                UpHisConsumptiveMaterial();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                EndProcessNews();
                //MessageBox.Show("123", "提示", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }
        }

        private void UpHisConsumptiveMaterial()
        {
            // if (!uploadHCbz&&!uploadHCxz)
            if (uploadHC.Length >= 2)
            {
                uploadHC = uploadHC.Substring(uploadHC.Length - 2, 2);
            }
            if (!uploadHC.Contains("12"))
            {
                uploadHC = "";

                MessageBox.Show("由于材料需要审核\n请确保已在地维客户端做过数据传送,并需要先运行按钮6下载医保对照关系,然后运行按钮7.才可以进行材料目录上传!");
                return;
            }
            string sqlDrug2 = " DELETE  from REPORT.dbo.upfailhc";
            SQLHelper.ExecSqlReDs(sqlDrug2.ToString());
            Text = "材料正在上传中，请稍等。。。。。";
            string networkPatclassID = "";//MainForm.NETWORKING_PAT_CLASS_ID;

            networkPatclassID = ConfigurationManager.AppSettings["PTMZ_PATID"].ToString();//MainForm.NETWORKING_PAT_CLASS_ID;
            string hosId = MainForm.HOSPITAL_ID;
            //fgybly.Close();

            //保存医保对照关系，默认NETWORKING_ITEM_CODE为0000
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append(" INSERT INTO COMM.COMM.NETWORKING_ITEM_VS_HIS ");
            sqlStr.Append(" ( NETWORKING_PAT_CLASS_ID , ");
            sqlStr.Append(" ITEM_PROP , ");
            sqlStr.Append(" HIS_ITEM_CODE , ");
            sqlStr.Append(" HIS_ITEM_NAME , ");
            sqlStr.Append(" NETWORK_ITEM_CODE , ");
            sqlStr.Append(" NETWORK_ITEM_NAME , ");
            sqlStr.Append(" SELF_BURDEN_RATIO , ");
            sqlStr.Append(" MEMO , ");
            sqlStr.Append(" START_TIME , ");
            sqlStr.Append(" STOP_TIME , ");
            sqlStr.Append(" TYPE_MEMO , ");
            sqlStr.Append(" NETWORK_ITEM_PROP , ");
            sqlStr.Append(" NETWORK_ITEM_CHARGE_CLASS , ");
            sqlStr.Append(" HOSPITAL_ID ,");
            sqlStr.Append(" NETWORK_ITEM_PRICE , ");
            sqlStr.Append(" FLAG_DISABLED , ");
            sqlStr.Append(" NETWORK_ITEM_FLAG_UP ");
            sqlStr.Append(") ");



            //3 和1    要区分好 
            sqlStr.Append("SELECT   '" + networkPatclassID + "', ");
            sqlStr.Append("  '1', ");

            //  sqlStr.Append("           CASE WHEN  aa.CHARGE_TYPE<10 THEN  '3' ELSE '1' END, ");
            sqlStr.Append("            aa.编码, ");
            sqlStr.Append("          AA.名称, ");
            sqlStr.Append("     '0000', ");
            sqlStr.Append("    '未对照', ");
            sqlStr.Append("   '0', ");
            sqlStr.Append("    '',  ");
            sqlStr.Append("   GETDATE(), ");
            sqlStr.Append("   GETDATE(), ");
            sqlStr.Append("   '',  ");
            sqlStr.Append("   '1', ");
            sqlStr.Append("   CASE WHEN  aa.CHARGE_TYPE<10 THEN  '3' ELSE '1' END,  ");
            sqlStr.Append("   '" + hosId + "', ");
            sqlStr.Append("  CASE  WHEN AA.价格 = -1 THEN 0 ELSE  AA.价格 END, ");
            sqlStr.Append("  '0', ");
            sqlStr.Append("  '0'  ");
            sqlStr.Append("   FROM ( SELECT    MAX(A.DRUG_CODE) AS 编码 , ");
            sqlStr.Append("   MAX(B.DRUG_NAME) AS 名称 , ");
            sqlStr.Append("  MAX(B.DRUG_SPEC) AS 规格 ,  ");
            sqlStr.Append("   MAX(e.MEASURE_UNIT_NAME) AS 单位 , ");
            sqlStr.Append("  MAX(F.DRUG_FORM_NAME) AS 剂型 , ");
            sqlStr.Append("  MAX(A.RETAIL_PRICE) AS 价格 ,  ");
            sqlStr.Append("  MAX(G.PRODUC_AREA_NAME) AS HIS产地 ");
            sqlStr.Append("  ,MAX(b.CHARGE_TYPE) AS CHARGE_TYPE ");

            sqlStr.Append("  FROM COMM.COMM.DRUG_PRICE_LIST AS A  ");
            sqlStr.Append("  LEFT JOIN COMM.dict.PRODUC_AREAS G ON G.PRODUC_AREA_ID = A.FIRM_ID ");
            sqlStr.Append("  LEFT JOIN YP.DRUG.PHARMACY_DRUG_STOCK H ON H.DRUG_PRICE_ID = A.DRUG_PRICE_ID ");
            sqlStr.Append("  LEFT JOIN YP.DRUG.DRUGS AS B ON A.DRUG_ID = B.DRUG_ID  ");
            sqlStr.Append("  LEFT JOIN COMM.DICT.DRUG_CLASSES AS C ON C.DRUG_CLASS_ID = B.DRUG_CLASS_ID ");
            sqlStr.Append("  LEFT JOIN COMM.DICT.MEASURE_UNITS AS E ON B.MEASURE_UNIT_ID = e.MEASURE_UNIT_ID ");
            sqlStr.Append("   LEFT JOIN COMM.DICT.DRUG_FORMS AS F ON B.DRUG_FORM_ID = f.DRUG_FORM_ID ");
            sqlStr.Append("   WHERE  ");
            sqlStr.Append("   b.CHARGE_TYPE<100 ");
            sqlStr.Append("   AND A.FLAG_INVALID='0' AND A.DRUG_CODE NOT IN ( ");
            sqlStr.Append("   SELECT HIS_ITEM_CODE  ");
            sqlStr.Append("   FROM COMM.COMM.NETWORKING_ITEM_VS_HIS ");
            sqlStr.Append("   WHERE ITEM_PROP in ('1') ");
            sqlStr.Append("    AND NETWORKING_PAT_CLASS_ID = '" + networkPatclassID + "' ");
            sqlStr.Append("    AND NETWORK_ITEM_CODE <> ''  and HOSPITAL_ID='" + hosId + "' ) ");
            sqlStr.Append("   AND A.DRUG_ID > 0  ");
            sqlStr.Append("   AND A.DRUG_CODE <> '' ");
            sqlStr.Append("   AND A.RETAIL_PRICE<> -1 ");
            sqlStr.Append("  AND HOSPITAL_ID = '" + hosId + "' ");
            sqlStr.Append(" AND B.FLAG_INVALID = 0 ");
            sqlStr.Append("  GROUP BY  A.DRUG_CODE,  ");
            sqlStr.Append("   C.DRUG_CLASS_ID  ");
            //正式需修改
            sqlStr.Append("   ) AA  ");

            //sqlStr.Append(" SELECT top 1  '" + networkPatclassID + "', ");
            //sqlStr.Append(" '1', ");
            //sqlStr.Append(" aa.编码, ");
            //sqlStr.Append(" AA.名称, ");
            //sqlStr.Append(" '0000', ");
            //sqlStr.Append(" '未对照', ");
            //sqlStr.Append(" '0', ");
            //sqlStr.Append(" '', ");
            //sqlStr.Append(" GETDATE(), ");
            //sqlStr.Append(" GETDATE(), ");
            //sqlStr.Append(" '', ");
            //sqlStr.Append(" '1', ");
            //sqlStr.Append(" '', ");
            //sqlStr.Append(" " + hosId + ", ");
            //sqlStr.Append(" CASE  WHEN AA.价格 = -1 THEN 0 ELSE  AA.价格 END, ");
            //sqlStr.Append(" '0', ");
            //sqlStr.Append(" '0' ");
            //sqlStr.Append(" FROM ( SELECT    MAX(A.DRUG_CODE) AS 编码 , ");
            //sqlStr.Append(" MAX(B.DRUG_NAME) AS 名称 , ");
            //sqlStr.Append(" MAX(B.DRUG_SPEC) AS 规格 , ");
            //sqlStr.Append(" MAX(e.MEASURE_UNIT_NAME) AS 单位 , ");
            //sqlStr.Append(" MAX(F.DRUG_FORM_NAME) AS 剂型 , ");
            //sqlStr.Append(" MAX(A.RETAIL_PRICE) AS 价格 , ");
            //sqlStr.Append(" MAX(G.PRODUC_AREA_NAME) AS HIS产地 ");
            //sqlStr.Append(" FROM COMM.COMM.DRUG_PRICE_LIST AS A ");
            //sqlStr.Append(" LEFT JOIN COMM.dict.PRODUC_AREAS G ON G.PRODUC_AREA_ID = A.FIRM_ID ");
            //sqlStr.Append(" LEFT JOIN YP.DRUG.DRUG_STOCK H ON H.DRUG_PRICE_ID = A.DRUG_PRICE_ID ");
            //sqlStr.Append(" LEFT JOIN YP.DRUG.DRUGS AS B ON A.DRUG_ID = B.DRUG_ID ");
            //sqlStr.Append(" LEFT JOIN COMM.DICT.DRUG_CLASSES AS C ON C.DRUG_CLASS_ID = B.DRUG_CLASS_ID ");
            //sqlStr.Append(" LEFT JOIN COMM.DICT.MEASURE_UNITS AS E ON B.MEASURE_UNIT_ID = e.MEASURE_UNIT_ID ");
            //sqlStr.Append(" LEFT JOIN COMM.DICT.DRUG_FORMS AS F ON B.DRUG_FORM_ID = f.DRUG_FORM_ID ");
            //sqlStr.Append(" WHERE B.CHARGE_TYPE <> 1 ");
            //sqlStr.Append(" AND A.DRUG_CODE NOT IN ( ");
            //sqlStr.Append(" SELECT HIS_ITEM_CODE ");
            //sqlStr.Append(" FROM COMM.COMM.NETWORKING_ITEM_VS_HIS ");
            //sqlStr.Append(" WHERE ITEM_PROP = '1' ");
            //sqlStr.Append(" AND NETWORKING_PAT_CLASS_ID = '" + networkPatclassID + "' ");
            //sqlStr.Append(" AND HOSPITAL_ID = '" + MainForm.HOSPITAL_ID + "' ");
            //sqlStr.Append(" AND NETWORK_ITEM_CODE <> '' ) ");
            //sqlStr.Append(" AND A.DRUG_ID > 0 ");
            //sqlStr.Append(" AND A.DRUG_CODE <> '' ");
            //sqlStr.Append(" AND A.RETAIL_PRICE<> -1 ");
            //sqlStr.Append(" AND HOSPITAL_ID = '" + hosId + "' ");
            //sqlStr.Append(" AND B.FLAG_INVALID = 0 ");
            //sqlStr.Append(" GROUP BY  A.DRUG_CODE, ");
            //sqlStr.Append(" C.DRUG_CLASS_ID ");
            ////sqlStr.Append(" ) AA "); 原有 正式需恢复
            //sqlStr.Append(" ) AA where AA.名称 like  '%注射器%'");//测试

            SQLHelper.ExecSqlReDs(sqlStr.ToString());

            //去除重复数据
            StringBuilder sqlStr1 = new StringBuilder();
            sqlStr1.Append(" DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS ");
            sqlStr1.Append(" WHERE   HIS_ITEM_CODE IN ( SELECT   HIS_ITEM_CODE ");
            sqlStr1.Append(" FROM     COMM.COMM.NETWORKING_ITEM_VS_HIS ");
            sqlStr1.Append(" WHERE    HOSPITAL_ID = '" + hosId + "' ");
            sqlStr1.Append(" AND NETWORKING_PAT_CLASS_ID=" + networkPatclassID + " ");
            sqlStr1.Append(" AND NETWORK_ITEM_PROP=1 ");
            sqlStr1.Append(" AND NETWORK_ITEM_CODE='0000' ");
            sqlStr1.Append(" GROUP BY HIS_ITEM_CODE ");
            sqlStr1.Append(" HAVING COUNT(HIS_ITEM_CODE) > 1 ");
            sqlStr1.Append(" ) ");
            sqlStr1.Append(" AND HOSPITAL_ID = '" + hosId + "' ");
            sqlStr1.Append(" AND NETWORKING_PAT_CLASS_ID=" + networkPatclassID + " ");
            SQLHelper.ExecSqlReDs(sqlStr1.ToString());
            // DataTable dt2 = SQLHelper.ExecSqlReDs("select * from COMM.COMM.NETWORKING_ITEM_VS_HIS").Tables[0];
            //上传未对照数据至地纬
            StringBuilder yp = new StringBuilder();
            yp.Append("SELECT  'YB_' + a.HIS_ITEM_CODE yyxmbm ,a.NETWORK_ITEM_CHARGE_CLASS  as yltype,a.HIS_ITEM_NAME yyxmmc ,'1' AS ypbz ,a.NETWORK_ITEM_PRICE AS dj , b.SPEC AS zxgg ,1 AS bhsl ,");
            yp.Append("'001' mllb ,'' AS syz ,'' AS jj ,'' AS scqy ,b.SPEC AS zdgg ,'' spm ,b.MEASURE_UNIT_NAME AS dw ,'' AS gmpbz ,'' AS cfybz ,'01' AS mzjsxmbh ,'01' AS zyjsxmbh ,'' AS jxmc ,'' AS kcl FROM COMM.COMM.NETWORKING_ITEM_VS_HIS a INNER JOIN ( SELECT  d.CHARGE_CODE ,d.CHARGE_NAME ,d.SPEC ,d.MEASURE_UNIT_NAME FROM COMM.COMM.CHARGE_PRICE d UNION SELECT  DRUG_CODE ,DRUG_NAME ,DRUG_SPEC ,n.MEASURE_UNIT_NAME FROM YP.DRUG.DRUGS m LEFT JOIN COMM.DICT.MEASURE_UNITS n ON n.MEASURE_UNIT_ID = m.MEASURE_UNIT_ID WHERE m.CHARGE_TYPE!=1 ) b ON a.HIS_ITEM_CODE = b.CHARGE_CODE ");
            yp.Append(" WHERE NETWORKING_PAT_CLASS_ID = '" + networkPatclassID + "' AND NETWORK_ITEM_FLAG_UP = '0' AND a.HOSPITAL_ID = '" + MainForm.HOSPITAL_ID + "' AND a.NETWORK_ITEM_PROP=1  AND a.ITEM_PROP=1  AND  a.HIS_ITEM_CODE!='0000' AND  A.NETWORK_ITEM_CHARGE_CLASS='3'  ");
            DataSet up_ds = SQLHelper.ExecSqlReDs(yp.ToString());
            foreach (DataTable dt in up_ds.Tables)
            {
                handelModel = new DW_Handle();
                handelModel.add_yyxm_info_all(dt);
            }
            uploadHC = "";
            //MessageBox.Show("药品上传完成");
            Text = "材料上传完成";
        }

        private void materialRaisedButton10_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Test");
        }

        private void materialRaisedButton6_Click(object sender, EventArgs e)
        {
            PreProcess();
            try
            {
                UpHisMedical();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                EndProcessNews();
            }
        }

        private void UpHisMedical()
        {
            Text = "诊疗正在上传中，请稍等。。。。。";
            string networkPatclassID = "";//MainForm.NETWORKING_PAT_CLASS_ID;
            //string sqlMedical = " DELETE   from COMM.COMM.NETWORKING_ITEM_VS_HIS where   NETWORK_ITEM_CHARGE_CLASS='' and HOSPITAL_ID = '" + MainForm.HOSPITAL_ID + "'";
            //SQLHelper.ExecSqlReDs(sqlMedical.ToString());
            string sqlDrug2 = " DELETE  from REPORT.dbo.upfailzl";
            SQLHelper.ExecSqlReDs(sqlDrug2.ToString());

            networkPatclassID = ConfigurationManager.AppSettings["PTMZ_PATID"].ToString();//MainForm.NETWORKING_PAT_CLASS_ID;
            string hosId = MainForm.HOSPITAL_ID;

            //保存医保对照关系，默认NETWORKING_ITEM_CODE为0000
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append(" INSERT INTO COMM.COMM.NETWORKING_ITEM_VS_HIS ");
            sqlStr.Append(" ( NETWORKING_PAT_CLASS_ID , ");
            sqlStr.Append(" ITEM_PROP , ");
            sqlStr.Append(" HIS_ITEM_CODE , ");
            sqlStr.Append(" HIS_ITEM_NAME , ");
            sqlStr.Append(" NETWORK_ITEM_CODE , ");
            sqlStr.Append(" NETWORK_ITEM_NAME , ");
            sqlStr.Append(" SELF_BURDEN_RATIO , ");
            sqlStr.Append(" MEMO , ");
            sqlStr.Append(" START_TIME , ");
            sqlStr.Append(" STOP_TIME , ");
            sqlStr.Append(" TYPE_MEMO , ");
            sqlStr.Append(" NETWORK_ITEM_PROP , ");
            sqlStr.Append(" NETWORK_ITEM_CHARGE_CLASS , ");
            sqlStr.Append(" HOSPITAL_ID , ");
            sqlStr.Append(" NETWORK_ITEM_PRICE , ");
            sqlStr.Append(" FLAG_DISABLED , ");
            sqlStr.Append(" NETWORK_ITEM_FLAG_UP ");
            sqlStr.Append(" ) ");
            //正式需恢复
            // sqlStr.Append(" SELECT  ' " + networkPatclassID + "' , ");
            //仅测试用
            sqlStr.Append(" SELECT    ' " + networkPatclassID + "' , ");
            sqlStr.Append(" '2' , ");
            sqlStr.Append(" aa.编码 , ");
            sqlStr.Append(" AA.名称 , ");
            sqlStr.Append(" '0000' , ");
            sqlStr.Append(" '未对照' , ");
            sqlStr.Append(" '0' , ");
            sqlStr.Append(" '' , ");
            sqlStr.Append(" GETDATE() , ");
            sqlStr.Append(" GETDATE() , ");
            sqlStr.Append(" '' , ");
            sqlStr.Append(" '2' , ");
            sqlStr.Append(" '' , ");
            sqlStr.Append(" " + hosId + " , ");
            sqlStr.Append(" CASE  WHEN AA.价格 = -1 THEN 0 ELSE  AA.价格 END, ");
            sqlStr.Append(" '0' , ");
            sqlStr.Append(" '0' ");
            sqlStr.Append(" FROM ( SELECT MAX(A.CHARGE_CODE) AS 编码 , ");
            sqlStr.Append(" MAX(A.CHARGE_NAME) AS 名称 , ");
            sqlStr.Append(" ( CASE WHEN MAX(A.SPEC) = '' THEN '/' ");
            sqlStr.Append(" ELSE MAX(A.SPEC) ");
            sqlStr.Append(" END ) AS 规格 , ");
            sqlStr.Append(" MAX(B.MEASURE_UNIT_NAME) AS 单位 , ");
            sqlStr.Append(" MAX(A.PRICE) AS 价格 ");
            sqlStr.Append(" FROM COMM.COMM.CHARGE_PRICE AS A ");
            sqlStr.Append(" LEFT JOIN COMM.DICT.MEASURE_UNITS AS B ON A.MEASURE_UNIT_ID = B.MEASURE_UNIT_ID ");
            sqlStr.Append(" WHERE  A.FLAG_INVALID='0' AND A.CHARGE_CODE NOT IN ( ");
            sqlStr.Append(" SELECT  HIS_ITEM_CODE ");
            sqlStr.Append(" FROM COMM.COMM.NETWORKING_ITEM_VS_HIS ");
            sqlStr.Append(" WHERE 1 = 1 ");
            sqlStr.Append(" AND NETWORKING_PAT_CLASS_ID = '" + networkPatclassID + "' ");
            sqlStr.Append(" AND HOSPITAL_ID = '" + hosId + "' ");
            sqlStr.Append(" AND NETWORK_ITEM_CODE <> '' ) ");
            sqlStr.Append(" AND A.CHARGE_ID > 0 ");
            sqlStr.Append(" AND A.CHARGE_CODE <> '' ");
            sqlStr.Append(" AND A.FLAG_INVALID = 0 ");
            sqlStr.Append(" AND A.HOSPITAL_ID ='" + hosId + "' ");
            sqlStr.Append(" GROUP BY  A.CHARGE_CODE ");
            //正式需恢复
            sqlStr.Append(" ) AA ");
            //仅测试用
            //  sqlStr.Append(" ) AA   WHERE AA.名称 LIKE '%床位费%'");
            //sqlStr.Append(" ) AA   WHERE AA.名称  in ('血清肌钙蛋白T测定','人免疫缺陷病毒抗体测定（Anti-HIV）') ");
            //sqlStr.Append(" ) AA   WHERE AA.名称  in ('血清总胆固醇测定','血清甘油三酯测定') ");

            // in ('血清肌钙蛋白T测定','人免疫缺陷病毒抗体测定（Anti-HIV）')
            SQLHelper.ExecSqlReDs(sqlStr.ToString());

            //去除重复数据
            StringBuilder sqlStr1 = new StringBuilder();
            //sqlStr1.Append(" DELETE  COMM.COMM.NETWORKING_ITEM_VS_HIS  WHERE  HIS_ITEM_CODE IN ( SELECT HIS_ITEM_CODE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS ");
            //sqlStr1.Append(" WHERE HOSPITAL_ID ='" + hosId + "' ");
            //sqlStr1.Append(" GROUP BY  HIS_ITEM_CODE having count(HIS_ITEM_CODE) > 1 ) and AUTO_ID not in ( select min(AUTO_ID) AS autoid ");
            //sqlStr1.Append(" from COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE HOSPITAL_ID ='" + hosId + "'   group by HIS_ITEM_CODE having count(HIS_ITEM_CODE)>1) ");
            //sqlStr1.Append(" AND HOSPITAL_ID ='" + hosId + "' ");
            sqlStr1.Append(" DELETE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS ");
            sqlStr1.Append(" WHERE   HIS_ITEM_CODE IN ( SELECT   HIS_ITEM_CODE ");
            sqlStr1.Append(" FROM     COMM.COMM.NETWORKING_ITEM_VS_HIS ");
            sqlStr1.Append(" WHERE    HOSPITAL_ID = '" + hosId + "' ");
            sqlStr1.Append(" AND NETWORKING_PAT_CLASS_ID=" + ptmz_ID + " ");
            sqlStr1.Append(" AND NETWORK_ITEM_PROP=2 ");
            sqlStr1.Append(" AND NETWORK_ITEM_CODE='0000' ");
            sqlStr1.Append(" GROUP BY HIS_ITEM_CODE ");
            sqlStr1.Append(" HAVING COUNT(HIS_ITEM_CODE) > 1 ");
            sqlStr1.Append(" ) ");
            sqlStr1.Append(" AND HOSPITAL_ID = '" + hosId + "' ");
            sqlStr1.Append(" AND NETWORKING_PAT_CLASS_ID=" + ptmz_ID + " ");
            SQLHelper.ExecSqlReDs(sqlStr1.ToString());


            //上传未对照数据至地纬
            StringBuilder yp = new StringBuilder();
            yp.Append(" SELECT 'YB_'+a.HIS_ITEM_CODE yyxmbm,a.HIS_ITEM_NAME yyxmmc,a.NETWORK_ITEM_CHARGE_CLASS  as yltype,'0' AS ypbz,a.NETWORK_ITEM_PRICE AS dj,b.SPEC AS zxgg,1 AS bhsl,'002' mllb, ");
            yp.Append(" '' AS syz,'' AS jj,'' AS scqy,b.SPEC AS zdgg,'' spm,b.MEASURE_UNIT_NAME AS dw,'' AS gmpbz,'' AS cfybz,'04' AS mzjsxmbh,'04' AS zyjsxmbh,'' AS jxmc,'' AS kcl ");
            yp.Append(" FROM COMM.COMM.NETWORKING_ITEM_VS_HIS a ");
            yp.Append(" LEFT JOIN (SELECT CHARGE_CODE,CHARGE_NAME,SPEC,MEASURE_UNIT_NAME FROM COMM.COMM.CHARGE_PRICE ");
            yp.Append(" UNION   ");
            yp.Append(" SELECT DRUG_CODE,DRUG_NAME,DRUG_SPEC,n.MEASURE_UNIT_NAME FROM YP.DRUG.DRUGS m ");
            yp.Append(" LEFT JOIN COMM.DICT.MEASURE_UNITS n ON n.MEASURE_UNIT_ID = m.MEASURE_UNIT_ID ");
            yp.Append(" ) b ON a.HIS_ITEM_CODE=b.CHARGE_CODE ");
            yp.Append(" WHERE NETWORKING_PAT_CLASS_ID='" + ptmz_ID + "' AND NETWORK_ITEM_FLAG_UP='0' AND a.HOSPITAL_ID='" + MainForm.HOSPITAL_ID + "' AND a.NETWORK_ITEM_PROP='2'  AND a.ITEM_PROP='2'  ");
            DataSet up_ds = SQLHelper.ExecSqlReDs(yp.ToString());
            foreach (DataTable dt in up_ds.Tables)
            {
                handelModel = new DW_Handle();
                handelModel.add_yyxm_info_all(dt);
            }
            //MessageBox.Show("计价项目上传完成");
            Text = "诊疗上传完成";
        }

        private void materialRaisedButton3_Click(object sender, EventArgs e)
        {
            PreProcess();
            try
            {
                handelModel = new DW_Handle();
                string sbjgbh = MainForm.sbjgbh;
                string filename = Environment.CurrentDirectory + @"\YBDLOAD\医院项目目录与核心端目录对照关系.txt";
                long filetype = 1;//0:表示excel文件,1:表示txt文件,2:表示csv文件，7:表示dbf2文件，8:表示dbf3文件
                long has_head = 0;//1:表示包含表头0:表示不包含表头（默认值为1）
                handelModel.Down_Yyxm(sbjgbh, filename, filetype, has_head);
                GC.KeepAlive(handelModel);

                GC.Collect();
                uploadHC += "1";
                Thread fThread = new Thread(new ThreadStart(Sleep6));
                fThread.IsBackground = true;
                fThread.Start();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                EndProcessNews();
            }
        }
        public void Sleep6()
        {
            MessageBox.Show("下载医院项目目录与核心端目录对照关系成功！地址：" + Environment.CurrentDirectory + @"\YBDLOAD\医院项目目录与核心端目录对照关系.txt");
        }
        private void FrmCeneter_Load(object sender, EventArgs e)
        {
            //string strSql = "";
            //DataSet DS = new DataSet();
            //strSql = "select NETWORKING_PAT_CLASS_NAME,NETWORKING_PAT_CLASS_ID from COMM.DICT.NETWORKING_PAT_CLASS where NETWORKING_PAT_CLASS_ID in (" + ConfigurationManager.AppSettings["NetWorkingPatCalssId"] + ") ORDER BY NETWORKING_PAT_CLASS_ID";
            //DS = SQLHelper.ExecSqlReDs(strSql);
            //comboBox1.DataSource = DS.Tables[0];
            //comboBox1.ValueMember = "NETWORKING_PAT_CLASS_ID";//值
            //comboBox1.DisplayMember = "NETWORKING_PAT_CLASS_NAME";//显示字段
            //comboBox1.SelectedIndex = 0;
            //DS.Tables.Clear();
        }

        private void materialRaisedButton8_Click_1(object sender, EventArgs e)
        {
            PreProcess();

            try
            {
                int n = 0;
                int m = 0;
                int drugCount = 0;
                int drugRate = 0;
                string filename = Environment.CurrentDirectory + @"\YBDLOAD\核心目录.txt";
                string filename2 = Environment.CurrentDirectory + @"\YBDLOAD\核心目录自付比例.txt";
                if (File.Exists(filename) && File.Exists(filename2))
                {
                    #region 保存中心目录表
                    StringBuilder ss = new StringBuilder();

                    if (MainForm.sbjgbh == "370100")//济南市医保办
                    {
                        ss.Append(" TRUNCATE TABLE REPORT.dbo.ZXML ");
                    }
                    #region 作废
                    //if (MainForm.sbjgbh == "370112")//社保机构编号(历城居民)
                    //{
                    //    ss.Append(" TRUNCATE TABLE REPORT.dbo.ZXML_LCYB ");


                    //}
                    //if (MainForm.sbjgbh == "370191")//社保机构编号(南山区居民)
                    //{
                    //    ss.Append(" TRUNCATE TABLE REPORT.dbo.ZXML_NSYB ");


                    //}
                    // ss.Append(" TRUNCATE TABLE REPORT.dbo.ZXML "); 
                    #endregion
                    int a = SQLHelper.ExecSqlReInt(ss.ToString());
                    DateTime startTime = DateTime.Now; //定义一个开始时间
                    using (DataTable table = new DataTable())
                    {
                        //为数据表创建相对应的数据列
                        table.Columns.Add("医保项目编码");
                        table.Columns.Add("医保项目名称");
                        table.Columns.Add("适应症");
                        table.Columns.Add("禁忌");
                        table.Columns.Add("规格");
                        table.Columns.Add("单位");
                        table.Columns.Add("物价基准最高价");
                        table.Columns.Add("剂型码");
                        table.Columns.Add("注销标志");
                        table.Columns.Add("生产企业");
                        table.Columns.Add("产地码");
                        table.Columns.Add("是否处方药");
                        table.Columns.Add("GMP标志");
                        table.Columns.Add("包装单位");
                        table.Columns.Add("最小规格");
                        table.Columns.Add("大包装包含小包装数量");
                        table.Columns.Add("更新时间");
                        //因为文件比较大，所有使用StreamReader的效率要比使用File.ReadLines高
                        using (StreamReader sr = new StreamReader(filename, Encoding.Default))
                        {
                            while (!sr.EndOfStream)
                            {
                                DataRow dr = table.NewRow();//创建数据行
                                string readStr = sr.ReadLine();//读取一行数据
                                string[] strs = readStr.Split(new char[] { '\t', '"' });//将读取的字符串按"制表符/t“和””“分割成数组, StringSplitOptions.RemoveEmptyEntries
                                if (strs.Length < 17)
                                {
                                    n++;
                                    continue;
                                }

                                //往对应的 行中添加数据
                                dr["医保项目编码"] = strs[0];
                                dr["医保项目名称"] = strs[1];
                                dr["适应症"] = strs[2];
                                dr["禁忌"] = strs[3];
                                dr["规格"] = strs[4];
                                dr["单位"] = strs[5];
                                dr["物价基准最高价"] = strs[6];
                                dr["剂型码"] = strs[7];
                                dr["注销标志"] = strs[8];
                                dr["生产企业"] = strs[9];
                                dr["产地码"] = strs[10];
                                dr["是否处方药"] = strs[11];
                                dr["GMP标志"] = strs[12];
                                dr["包装单位"] = strs[13];
                                dr["最小规格"] = strs[14];
                                dr["大包装包含小包装数量"] = strs[15];
                                dr["更新时间"] = strs[16];
                                table.Rows.Add(dr);//将创建的数据行添加到table中
                                drugCount++;
                            }
                        }
                        SqlBulkCopy bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["connStr"].ToString());



                        bulkCopy.DestinationTableName = "REPORT.dbo.ZXML";//设置数据库中对象的表名

                        #region 作废
                        //if (MainForm.sbjgbh == "370112")//社保机构编号(历城居民)
                        //{
                        //    bulkCopy.DestinationTableName = "REPORT.dbo.ZXML_LCYB";


                        //}
                        //if (MainForm.sbjgbh == "370191")//社保机构编号(南山区居民)
                        //{
                        //    bulkCopy.DestinationTableName = "REPORT.dbo.ZXML_NSYB";



                        //} 
                        #endregion


                        //设置数据表table和数据库中表的列对应关系
                        bulkCopy.ColumnMappings.Add("医保项目编码", "医保项目编码");
                        bulkCopy.ColumnMappings.Add("医保项目名称", "医保项目名称");

                        bulkCopy.ColumnMappings.Add("适应症", "适应症");
                        bulkCopy.ColumnMappings.Add("禁忌", "禁忌");
                        bulkCopy.ColumnMappings.Add("规格", "规格");
                        bulkCopy.ColumnMappings.Add("单位", "单位");
                        bulkCopy.ColumnMappings.Add("物价基准最高价", "物价基准最高价");
                        bulkCopy.ColumnMappings.Add("剂型码", "剂型码");
                        bulkCopy.ColumnMappings.Add("注销标志", "注销标志");
                        bulkCopy.ColumnMappings.Add("生产企业", "生产企业");
                        bulkCopy.ColumnMappings.Add("产地码", "产地码");
                        bulkCopy.ColumnMappings.Add("是否处方药", "是否处方药");
                        bulkCopy.ColumnMappings.Add("GMP标志", "GMP标志");
                        bulkCopy.ColumnMappings.Add("最小规格", "最小规格");
                        bulkCopy.ColumnMappings.Add("大包装包含小包装数量", "大包装包含小包装数量");
                        bulkCopy.ColumnMappings.Add("更新时间", "更新时间");
                        bulkCopy.WriteToServer(table);//将数据表table复制到数据库中
                    }

                    #endregion
                    #region 保存自付比例
                    StringBuilder sss = new StringBuilder();





                    if (MainForm.sbjgbh == "370100")//济南市医保办
                    {
                        sss.Append(" TRUNCATE TABLE REPORT.dbo.ZXML_ZFBL ");
                    }

                    #region 作废
                    //if (MainForm.sbjgbh == "370112")//社保机构编号(历城居民)
                    //{
                    //    // ss.Append(" TRUNCATE TABLE REPORT.dbo.ZXML_LCYB ");
                    //    sss.Append(" TRUNCATE TABLE REPORT.dbo.ZXML_ZFBL_LC ");



                    //}
                    //if (MainForm.sbjgbh == "370191")//社保机构编号(南山区居民)
                    //{
                    //    sss.Append(" TRUNCATE TABLE REPORT.dbo.ZXML_ZFBL_NS ");

                    //    // ss.Append(" TRUNCATE TABLE REPORT.dbo.ZXML_NSYB ");


                    //} 
                    #endregion


                    int aa = SQLHelper.ExecSqlReInt(sss.ToString());
                    using (DataTable table2 = new DataTable())
                    {
                        //为数据表创建相对应的数据列
                        table2.Columns.Add("医保项目编码");
                        table2.Columns.Add("起始日期");
                        table2.Columns.Add("终止日期");
                        table2.Columns.Add("自付比例");
                        table2.Columns.Add("险种标志");
                        table2.Columns.Add("自付比例说明");
                        table2.Columns.Add("社保机构类型");
                        table2.Columns.Add("限价");

                        //因为文件比较大，所有使用StreamReader的效率要比使用File.ReadLines高
                        using (StreamReader sr = new StreamReader(filename2, Encoding.Default))
                        {
                            while (!sr.EndOfStream)
                            {
                                DataRow dr = table2.NewRow();//创建数据行
                                string readStr = sr.ReadLine();//读取一行数据
                                string[] strs = readStr.Split(new char[] { '\t', '"' });//将读取的字符串按"制表符/t“和””“分割成数组, StringSplitOptions.RemoveEmptyEntries
                                if (strs.Length < 8)
                                {
                                    m++;
                                    continue;
                                }

                                //往对应的 行中添加数据
                                dr["医保项目编码"] = strs[0];
                                dr["起始日期"] = strs[1];
                                dr["终止日期"] = strs[2];
                                dr["自付比例"] = strs[3];
                                dr["险种标志"] = strs[4];
                                dr["自付比例说明"] = strs[5];
                                dr["社保机构类型"] = strs[6];
                                dr["限价"] = strs[7];

                                table2.Rows.Add(dr);//将创建的数据行添加到table中
                                drugRate++;
                            }
                        }

                        SqlBulkCopy bulkCopy2 = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["connStr"].ToString());
                        bulkCopy2.DestinationTableName = "REPORT.dbo.ZXML_ZFBL";//设置数据库中对象的表名

                        #region 作废
                        //if (MainForm.sbjgbh == "370112")//社保机构编号(历城居民)
                        //{
                        //    bulkCopy2.DestinationTableName = "REPORT.dbo.ZXML_ZFBL_LC";


                        //}
                        //if (MainForm.sbjgbh == "370191")//社保机构编号(南山区居民)
                        //{
                        //    bulkCopy2.DestinationTableName = "REPORT.dbo.ZXML_ZFBL_NS";



                        //} 
                        #endregion



                        //设置数据表table和数据库中表的列对应关系
                        bulkCopy2.ColumnMappings.Add("医保项目编码", "医保项目编码");
                        bulkCopy2.ColumnMappings.Add("起始日期", "起始日期");
                        bulkCopy2.ColumnMappings.Add("终止日期", "终止日期");
                        bulkCopy2.ColumnMappings.Add("自付比例", "自付比例");
                        bulkCopy2.ColumnMappings.Add("险种标志", "险种标志");
                        bulkCopy2.ColumnMappings.Add("自付比例说明", "自付比例说明");

                        bulkCopy2.ColumnMappings.Add("社保机构类型", "社保机构类型");
                        bulkCopy2.ColumnMappings.Add("限价", "限价");

                        bulkCopy2.WriteToServer(table2);//将数据表table复制到数据库中
                    }
                    #endregion
                    TimeSpan ts = DateTime.Now - startTime;
                    MessageBox.Show("插入药品:" + drugCount + "条,忽略药品：" + n + "条，插入比例：" + drugRate + "条，忽略：" + m + "条");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                EndProcessNews();
            }
        }

        private void materialRaisedButton5_Click(object sender, EventArgs e)
        {
            try
            {
                // MessageBox.Show("正在保存数据！");
                //  handelModel = new DW_Handle();
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
                        //医保类别
                        string ybtype = "";
                        if (strs.Length > 18)
                        {
                            ybtype = strs[18];
                        }
                        int a = strs[3].IndexOf("终止日期");
                        string endtime = strs[3].Substring(strs[3].IndexOf("终止日期") + 5, 4);
                        string currentime = (DateTime.Now).ToString("yyyy");
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
                        string nameCenterYB = "未检索到中心名称";

                        //if (network_item_code=="215000001609JZ")
                        //{
                        //    MessageBox.Show("1");
                        //}
                        //   MessageBox.Show("正在修改！");
                        //更新对照关系表
                        if (Convert.ToInt32(endtime) > Convert.ToInt32(currentime))
                        {
                            if (ybtype == "" || ybtype == "A")
                            {
                                //  DataTable dt2 = SQLHelper.ExecSqlReDs("SELECT TOP 1 COALESCE(医保项目名称,' ') NETWORK_ITEM_NAME FROM REPORT.dbo.ZXML WHERE 医保项目编码 ='" + network_item_code + "'").Tables[0];
                                StringBuilder sqlStr = new StringBuilder();
                                sqlStr.Append(" UPDATE COMM.COMM.NETWORKING_ITEM_VS_HIS ");
                                sqlStr.Append(" SET NETWORK_ITEM_CODE = '" + tem_code + "',");

                                if (MainForm.sbjgbh == "370100")//济南市医保办
                                {
                                    nameCenterYB = "未检索到中心名称";
                                    if (!string.IsNullOrEmpty(QueryJN(network_item_code)))
                                    {
                                        nameCenterYB = QueryJN(network_item_code);
                                    }
                                    if (!string.IsNullOrEmpty(QueryLC(network_item_code)))
                                    {
                                        nameCenterYB = QueryLC(network_item_code);
                                    }
                                    if (!string.IsNullOrEmpty(QueryNS(network_item_code)))
                                    {
                                        nameCenterYB = QueryNS(network_item_code);
                                    }
                                    if (string.IsNullOrEmpty(nameCenterYB))
                                    {
                                        nameCenterYB = "未检索到中心名称";
                                    }
                                    sqlStr.Append(" NETWORK_ITEM_NAME ='" + nameCenterYB + "', ");
                                }

                                if (MainForm.sbjgbh == "370112")//社保机构编号(历城居民)
                                {
                                    // ss.Append(" TRUNCATE TABLE REPORT.dbo.ZXML_LCYB ");
                                    // sqlStr.Append(" NETWORK_ITEM_NAME = (SELECT TOP 1 COALESCE(医保项目名称,' ') NETWORK_ITEM_NAME FROM REPORT.dbo.ZXML_LCYB  WHERE 医保项目编码 ='" + network_item_code + "'), ");
                                    nameCenterYB = "未检索到中心名称";
                                    if (!string.IsNullOrEmpty(QueryJN(network_item_code)))
                                    {
                                        nameCenterYB = QueryJN(network_item_code);
                                    }
                                    if (!string.IsNullOrEmpty(QueryLC(network_item_code)))
                                    {
                                        nameCenterYB = QueryLC(network_item_code);
                                    }
                                    if (!string.IsNullOrEmpty(QueryNS(network_item_code)))
                                    {
                                        nameCenterYB = QueryNS(network_item_code);
                                    }
                                    if (string.IsNullOrEmpty(nameCenterYB))
                                    {
                                        nameCenterYB = "未检索到中心名称";
                                    }
                                    sqlStr.Append(" NETWORK_ITEM_NAME ='" + nameCenterYB + "', ");


                                }
                                if (MainForm.sbjgbh == "370191")//社保机构编号(南山区居民)
                                {
                                    //  sqlStr.Append(" NETWORK_ITEM_NAME = (SELECT TOP 1 COALESCE(医保项目名称,' ') NETWORK_ITEM_NAME FROM REPORT.dbo.ZXML_NSYB  WHERE 医保项目编码 ='" + network_item_code + "'), ");

                                    // ss.Append(" TRUNCATE TABLE REPORT.dbo.ZXML_NSYB ");
                                    nameCenterYB = "未检索到中心名称";
                                    if (!string.IsNullOrEmpty(QueryJN(network_item_code)))
                                    {
                                        nameCenterYB = QueryJN(network_item_code);
                                    }
                                    if (!string.IsNullOrEmpty(QueryLC(network_item_code)))
                                    {
                                        nameCenterYB = QueryLC(network_item_code);
                                    }
                                    if (!string.IsNullOrEmpty(QueryNS(network_item_code)))
                                    {
                                        nameCenterYB = QueryNS(network_item_code);
                                    }
                                    if (string.IsNullOrEmpty(nameCenterYB))
                                    {
                                        nameCenterYB = "未检索到中心名称";
                                    }
                                    sqlStr.Append(" NETWORK_ITEM_NAME ='" + nameCenterYB + "', ");


                                }

                                sqlStr.Append(" TYPE_MEMO = '" + reim_type + "',");
                                sqlStr.Append(" MEMO = '" + reim_type + "',");
                                sqlStr.Append(" SELF_BURDEN_RATIO = " + Convert.ToDecimal(selfCostRate) + ",");
                                sqlStr.Append(" NETWORK_ITEM_FLAG_UP = '1'");
                                sqlStr.Append(" WHERE  HIS_ITEM_CODE ='" + HisItemCode + "'");
                                sqlStr.Append(" AND  HOSPITAL_ID='" + MainForm.HOSPITAL_ID + "'");
                                sqlStr.Append(" AND  NETWORKING_PAT_CLASS_ID=" + ptmz_ID + "  ");
                                SQLHelper.ExecSqlReDs(sqlStr.ToString());
                            }
                        }
                    }
                    //  MessageBox.Show("修改完成！");
                    uploadHC += "2";



                    TimeSpan ts = DateTime.Now - startTime;
                    Thread fThread = new Thread(new ThreadStart(SleepT));
                    fThread.IsBackground = true;
                    fThread.Start();

                    //MessageBox.Show("更新成功！");

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message); ;
            }
        }



        public void SleepT()
        {
            MessageBox.Show("更新成功！");
        }


        public string QueryJN(string network_item_code)
        {
            string sqljnOnlyone = "";
            string sqljn = "SELECT TOP 1 COALESCE(医保项目名称,' ') NETWORK_ITEM_NAME FROM REPORT.dbo.ZXML  WHERE 医保项目编码 ='" + network_item_code + "'";
            // sqljnOnlyone = sqlcenterName.ExecuteScalar(sqljn).ToString();
            DataTable dtRe1 = new DataTable();
            dtRe1 = SQLHelper.ExecSqlReDs(sqljn).Tables[0];
            //  sqljnOnlyone = dtRe1.Rows[0][0].ToString();
            foreach (DataRow item in dtRe1.Rows)
            {
                sqljnOnlyone = item[0].ToString();
            }

            return sqljnOnlyone;
        }


        public string QueryLC(string network_item_code)
        {
            string sqlLCOnlyone = "";
            string sqlLC = "SELECT TOP 1 COALESCE(医保项目名称,' ') NETWORK_ITEM_NAME FROM REPORT.dbo.ZXML_LCYB  WHERE 医保项目编码 ='" + network_item_code + "'";
            //sqlLCOnlyone = sqlcenterName.ExecuteScalar(sqlLC).ToString();
            DataTable dtRe1 = new DataTable();
            dtRe1 = SQLHelper.ExecSqlReDs(sqlLC).Tables[0];
            //  sqlLCOnlyone = dtRe1.Rows[0][0].ToString();

            foreach (DataRow item in dtRe1.Rows)
            {
                sqlLCOnlyone = item[0].ToString();
            }
            return sqlLCOnlyone;
        }

        public string QueryNS(string network_item_code)
        {
            string sqlNsOnlyone = "";
            string sqlNS = "SELECT TOP 1 COALESCE(医保项目名称,' ') NETWORK_ITEM_NAME FROM REPORT.dbo.ZXML_NSYB  WHERE 医保项目编码 ='" + network_item_code + "'";
            // sqlNsOnlyone = sqlcenterName.ExecuteScalar(sqlNS).ToString();
            DataTable dtRe1 = new DataTable();
            dtRe1 = SQLHelper.ExecSqlReDs(sqlNS).Tables[0];
            //sqlNsOnlyone=dtRe1.Rows[0][0].ToString();
            foreach (DataRow item in dtRe1.Rows)
            {
                sqlNsOnlyone = item[0].ToString();
            }
            return sqlNsOnlyone;
        }

        private void materialRaisedButton12_Click(object sender, EventArgs e)
        {
            #region 居民住院、门规、免费用药更新
            JmZyMgUP();
            #endregion

            #region 职工住院、门规、

            ZgZyMgUP();

            #endregion
        }


        /// <summary>
        /// 职工住院、门规、
        /// </summary>
        private void ZgZyMgUP()
        {
            #region 职工住院
            handelModel = new DW_Handle();
            string sbjgbh = "370100";
            DateTime rq = DateTime.Today;
            string sqlstr3 = @"SELECT NETWORK_ITEM_CODE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORK_ITEM_CODE  LIKE '%YB_%' AND NETWORK_ITEM_CODE NOT LIKE '0000%' AND NETWORKING_PAT_CLASS_ID=" + ptmz_ID + " And HOSPITAL_ID='" + MainForm.HOSPITAL_ID + "' ";
            //DataSet ds = SQLHelper.ExecSqlReDs(sqlstr.ToString());
            DataTable dt3 = SQLHelper.ExecSqlReDs(sqlstr3.ToString()).Tables[0];

            for (int i = 0; i < dt3.Rows.Count; i++)
            {
                string yyxmbm = dt3.Rows[i]["NETWORK_ITEM_CODE"].ToString();
                handelModel.Down_Yyxm_ZFBL_ZGmg(sbjgbh, yyxmbm, rq.ToString("yyyy-MM-dd"),MainForm.HOSPITAL_ID,"C","A","");
            } 
            #endregion

        }

        /// <summary>
        /// 居民住院、门规更新
        /// </summary>
        private void JmZyMgUP()
        {
            string sbjgbh = MainForm.sbjgbh;

            if (sbjgbh == "370100")
            {
                MessageBox.Show("未选择居民经办机构，请重新登陆选择居民经办机构！");
                return;
            }
            string sqlstr = @"SELECT NETWORK_ITEM_CODE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORK_ITEM_CODE  LIKE '%YB_%' AND NETWORK_ITEM_CODE NOT LIKE '0000%' AND NETWORKING_PAT_CLASS_ID=" + ptmz_ID + " And HOSPITAL_ID='" + MainForm.HOSPITAL_ID + "' ";

            // string sqlstr2 = @"SELECT NETWORK_ITEM_CODE FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WHERE NETWORK_ITEM_CODE  LIKE '%YB_%' AND NETWORK_ITEM_CODE NOT LIKE '0000%' AND NETWORKING_PAT_CLASS_ID=" + ptmz_ID + @" And HOSPITAL_ID='" + MainForm.HOSPITAL_ID + "' ";
            //DataSet ds = SQLHelper.ExecSqlReDs(sqlstr.ToString());
            DataTable dt2 = SQLHelper.ExecSqlReDs(sqlstr.ToString()).Tables[0];
            DateTime rq = DateTime.Today;
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                string yyxmbm = dt2.Rows[i]["NETWORK_ITEM_CODE"].ToString();
                handelModel.Down_Yyxm_ZFBL_JMmg(sbjgbh, yyxmbm, rq.ToString("yyyy-MM-dd"), MainForm.HOSPITAL_ID, "C", "B");
            }
        }

        private void materialRaisedButton16_Click(object sender, EventArgs e)
        {

        }

        private void materialRaisedButton11_Click(object sender, EventArgs e)
        {

        }
    }
}
