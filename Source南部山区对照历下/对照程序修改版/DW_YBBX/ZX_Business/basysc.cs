using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

using HotelSystemORM.Unitl;



namespace DW_YBBX.ZX_Business
{
    public partial class basysc : Form
    {
        //登陆用户的 系统ID，姓名
        public string _hosId, _userId, _deptId, _dwuser, _dwpass, _hosName, _userName;
        public IniFiles ini = null;
        public basysc(string hosId, string userId, string deptId, string dwuser, string dwpass, string hosName, string userName)
        {
            _hosId = hosId;
            _userId = userId;
            _deptId = deptId;
            _dwuser = dwuser;
            _dwpass = dwpass;
            _hosName = hosName;
            _userName = userName;
            ini = new HotelSystemORM.Unitl.IniFiles(Application.StartupPath + @"\OrgConfig.INI");
            //hisCode = ini.IniReadValue("orgInfo", "hisCode");
            InitializeComponent();
        }

        public MSSQLHelpers SSS = new MSSQLHelpers(ConfigurationManager.AppSettings["HISDBStr"].ToString());
       
        private DW_Handle handelModel;
        public string hisCode = "";
        #region 地维登陆 获取用户口令
        /// <summary>
        /// 地维登陆
        /// </summary>
        public void DareWayInit()
        {
            //            int iRe = seiproxy.ExeFuncReInt("init", new object[] { usercode, hosNo, password });
            handelModel = new DW_Handle();

        }
        /// <summary>
        /// 住院初始化
        /// </summary>
        /// <returns></returns>
        public void ZyInit(string p_blh)
        {
            handelModel.ClearInPara();
            handelModel.SetInParaString("blh", p_blh);
            decimal iRe = handelModel.ExecService("init_zy");

            if (iRe != 0)
            {
                throw new Exception("住院初始化出错,医保返回提示：" + handelModel.ExeFuncReStr("get_errtext", null));
            }

        }
        #endregion


        private void Form1_Load(object sender, EventArgs e)
        {
            DataTable ds_info = get_maininfo_ds();
            dgv_main.DataSource = ds_info;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int up_num = 0;
            int invalid_num = 0;
            string now = DateTime.Now.ToString("HH:mm:ss");
            Dictionary<string, string> result = new Dictionary<string, string>();

            StringBuilder err_info = new StringBuilder();
            DataTable ds_info = get_maininfo();
            dgv_main.DataSource = ds_info;

            progressBar1.Value = 0;
            progressBar1.Visible = true;
            labinfo.Visible = true;
            DareWayInit();
            foreach (DataRow dr in ds_info.Rows)
            {
                ///住院初始化
                ZyInit(dr["病例号"].ToString());
                ///病历首页初始化
                zyInitCase(dr["病例号"].ToString());

                //此服务在文档中没有  地维没有回复
                deleteCase(dr["病例号"].ToString());//删除上传病历号
                //获取诊断和手术信息的数据集合
                DataTable tb_diagnosis = get_diagnosis(dr["病例号"].ToString());
                DataTable tb_SURGERIES = get_SURGERIES(dr["病例号"].ToString());

                if (tb_diagnosis.Rows.Count > 0)
                {
                    upDiagnosis(tb_diagnosis);
                }
                if (tb_SURGERIES.Rows.Count > 0)
                {
                    upOper(tb_SURGERIES);
                }
                handelModel.ClearInPara();
                string s = DateTime.Parse(dr["住院日期"].ToString()).ToShortDateString();
                //handelModel.SetInParaString("blh", dr["病例号"].ToString());
                handelModel.SetInParaDate("zyrq", DateTime.Parse(dr["住院日期"].ToString()).ToString("yyyy-MM-dd hh:mm:ss"));
                handelModel.SetInParaDate("cyrq", DateTime.Parse(dr["出院日期"].ToString()).ToString("yyyy-MM-dd hh:mm:ss"));
                handelModel.SetInParaDate("qzrq", DateTime.Parse(dr["确诊日期"].ToString()).ToString("yyyy-MM-dd hh:mm:ss"));
                handelModel.SetInParaString("ryks", dr["入院科室"].ToString());
                handelModel.SetInParaString("cyks", dr["出院科室"].ToString());
                handelModel.SetInParaString("xx", dr["血型"].ToString());
                handelModel.SetInParaString("rhxx", dr["RH血型"].ToString());
                handelModel.SetInParaString("lyfs", dr["离院方式"].ToString());
                handelModel.SetInParaString("cyxj", dr["出院小结"].ToString());
                handelModel.SetInParaString("zzysbm", dr["主治医师"].ToString());
                handelModel.SetInParaString("zyysbm", dr["住院医师"].ToString());
                handelModel.SetInParaString("lxr", dr["联系人"].ToString());
                handelModel.SetInParaString("lxrgx", dr["联系人关系"].ToString());
                handelModel.SetInParaString("lxrdh", dr["联系人电话"].ToString());
                handelModel.SetInParaString("brlxdh", dr["病人电话"].ToString());
                //handelModel.AddInPara("p_zrhsysbm", dr["责任护士"].ToString());
                handelModel.SetInParaString("kzrysbm", dr["科室主任"].ToString());
                handelModel.SetInParaString("zrysbm", dr["主任医师"].ToString());
                handelModel.SetInParaString("jxysbm", dr["进修医师"].ToString());
                handelModel.SetInParaString("sxysbm", dr["实习医师"].ToString());
                handelModel.SetInParaString("zcyybm", dr["转出医疗机构编码"].ToString());

                

                try
                {
                    if (tb_diagnosis.Rows.Count > 0)
                    {
                        //上传病案首页信息
                        decimal iRe = handelModel.ExecService("save_case");
                        if (iRe != 0)
                        {
                            throw new Exception("上传病案首页失败,医保返回提示：" + handelModel.ExeFuncReStr("get_errtext", null));
                        }

                        up_num += 1;

                        //写入本地记录
                        SSS.ExecSqlReInt("INSERT INTO REPORT.dbo.lszy_basc_dw( pat_in_hos_id ,bah ,cs ,ybscrq,hos_id)VALUES('" + dr["PAT_IN_HOS_ID"].ToString() + "','" + dr["病例号"].ToString() + "','1','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','" + _hosId + "')");
                    }
                    else
                    {
                        err_info.Append(dr["病例号"].ToString() + "\n");
                        err_info.Append("没有填写医保诊断信息  不予以上传首页信息" + "\n");
                        invalid_num += 1;
                    }

                    labinfo.Text = (up_num + invalid_num).ToString() + "/" + ds_info.Rows.Count.ToString();
                    progressBar1.Value = up_num + invalid_num;
                   // handelModel.InitHandle();
                }
                catch (Exception ex)
                {
                    err_info.Append(dr["病例号"].ToString() + "\n");
                    err_info.Append(ex + "\n");
                    invalid_num += 1;
                    labinfo.Text = (up_num + invalid_num).ToString() + "/" + ds_info.Rows.Count.ToString();
                    progressBar1.Value = up_num + invalid_num;
                    //handelModel.InitHandle();
                    //throw new Exception(ex.Message );
                }



            }
            if (err_info.Length == 0)
            {
                MessageBox.Show("传送完毕!\n上传成功" + up_num + "人次\n上传失败" + invalid_num + "人次");
                progressBar1.Value = 0;
                progressBar1.Visible = false;
                labinfo.Visible = false;
            }
            else
            {
                MessageBox.Show("传送完毕!\n上传成功" + up_num + "人次\n上传失败" + invalid_num + "人次");
                err_info.Replace("在 MED_UPLOAD_INFO.Form1.timer1_Tick(Object sender, EventArgs e) 位置 e:\\郑友峰\\事后报销程序源码\\MED_UPLOAD_INFO\\MED_UPLOAD_INFO\\Form1.cs:行号 81", "");
                err_info.Replace("在 IRCInterfaceHandle.ZIBO.ZBDareWayInterfaceHandle2016.Handle(String method, Boolean isCheckSuc) 位置 e:\\郑友峰\\事后报销程序源码\\MED_UPLOAD_INFO\\MED_UPLOAD_INFO\\ZBDareWayInterfaceHandle2016.cs:行号 195", "");
                err_info.Replace("在 MED_UPLOAD_INFO.Form1.timer1_Tick(Object sender, EventArgs e) 位置 e:\\郑友峰\\事后报销程序源码\\MED_UPLOAD_INFO\\MED_UPLOAD_INFO\\Form1.cs:行号 86", "");
                progressBar1.Value = 0;
                progressBar1.Visible = false;
                labinfo.Visible = false;

                rtb_log.Text = err_info.ToString();
            }
            

        }

        private void zyInitCase(string p_blh)
        {
            handelModel.ClearInPara();
            handelModel.SetInParaString("blh", p_blh);
            decimal iRe = handelModel.ExecService("init_case");

            if (iRe != 0)
            {
                throw new Exception("病案首页初始化出错,医保返回提示：" + handelModel.ExeFuncReStr("get_errtext", null));
            }
        }

        /// <summary>
        /// 第一步：获取已经归档的病人的病案首页
        /// </summary>
        /// <returns></returns>
        private DataTable get_maininfo()
        {
            StringBuilder str_info_s =new StringBuilder();

            str_info_s.Append("SELECT  a.MEDICAL_RECORD_CODE AS 病例号 ,");
            str_info_s.Append("        a.PAT_IN_WAY AS 入院途径 ,");
            str_info_s.Append("        CONVERT(DATETIME,a.PAT_IN_TIME,120) AS 住院日期 ,");
            str_info_s.Append("        CONVERT(DATETIME,a.PAT_OUT_TIME,120) 出院日期 ,");
            str_info_s.Append("        isnull((case when b.DIAGNOSE_TIME='0001-01-01'then CONVERT(DATETIME,a.PAT_IN_TIME,120) else CONVERT(DATETIME,b.DIAGNOSE_TIME,120) end), CONVERT(DATETIME,a.PAT_IN_TIME,120)) AS 确诊日期 ,");
            str_info_s.Append("        a.PAT_IN_DEPT AS 入院科室 ,");
            str_info_s.Append("        a.PAT_OUT_DEPT AS 出院科室 ,");
            str_info_s.Append("        b.BLOOD_TYPE AS 血型 ,");
            str_info_s.Append("        b.RH AS RH血型 ,");
            str_info_s.Append("        '' AS 离院方式 ,");
            str_info_s.Append("        '医嘱出院' AS 出院小结 ,");
            str_info_s.Append("        ISNULL(c.USER_CODE,'') AS 主治医师 ,");
            str_info_s.Append("        ISNULL(d.USER_CODE,'') AS 住院医师 ,");
            str_info_s.Append("        a.RELATIVE_NAME AS 联系人 ,");
            str_info_s.Append("        a.RELATION AS 联系人关系 ,");
            str_info_s.Append("        (CASE WHEN a.RELATIVE_PHONE = '' AND a.ADDRESS_PHONE='' THEN '05338592008' WHEN a.ADDRESS_PHONE=''  THEN a.RELATIVE_PHONE ELSE a.ADDRESS_PHONE END) AS 联系人电话 ,");
            str_info_s.Append("        (CASE WHEN a.RELATIVE_PHONE = '' AND a.ADDRESS_PHONE='' THEN '05338592008' WHEN a.ADDRESS_PHONE=''  THEN a.RELATIVE_PHONE ELSE a.ADDRESS_PHONE END) AS 病人电话 ,");
            str_info_s.Append("        ISNULL(e.USER_CODE,'') AS 责任护士 ,");
            str_info_s.Append("        ISNULL(f.USER_CODE,'') AS 科室主任 ,");
            str_info_s.Append("        ISNULL(g.USER_CODE,'') AS 主任医师 ,");
            str_info_s.Append("        ISNULL(h.USER_CODE,'') AS 进修医师 ,");
            str_info_s.Append("        ISNULL(j.USER_CODE,'') AS 实习医师 ,");
            str_info_s.Append("        '' AS 转出医疗机构编码,A.PAT_IN_HOS_ID ");
            str_info_s.Append("FROM    MedicalRecord.MEDICAL.MEDICAL_RECORD_BASE_MSG a");
            //str_info_s.Append("        INNER JOIN MedicalRecord.MEDICAL.MEDICAL_RECORD_CHECK k ON a.MEDICAL_RECORD_CODE = k.MEDICAL_CODE");
            str_info_s.Append("        LEFT JOIN MedicalRecord.MEDICAL.MEDICAL_RECORD_DIAGNOSIS b ON a.MEDICAL_RECORD_CODE = b.MEDICAL_CODE AND a.ID = b.BASE_MSG_ID");
            str_info_s.Append("        LEFT JOIN COMM.COMM.users c ON b.MAIN_DOCTOR = c.[USER_ID]");
            str_info_s.Append("        LEFT JOIN COMM.COMM.users d ON b.PAT_IN_DOCTOR = d.[USER_ID]");
            str_info_s.Append("        LEFT JOIN COMM.COMM.users e ON b.RESP_NURSE = e.[USER_ID]");
            str_info_s.Append("        LEFT JOIN COMM.COMM.users f ON b.DEPT_HEAD = f.[USER_ID]");
            str_info_s.Append("        LEFT JOIN COMM.COMM.users g ON b.DEPUTY_HEAD = g.[USER_ID]");
            str_info_s.Append("        LEFT JOIN COMM.COMM.users h ON b.ADVANCED_DOCTOR = h.[USER_ID]");
            str_info_s.Append("        LEFT JOIN COMM.COMM.users j ON b.STUDY_DOCTOR = j.[USER_ID]");
            if (comboBox1.Text == "已上传")
            {
                str_info_s.Append("		   WHERE CONVERT(VARCHAR(10),CONVERT(DATETIME,a.pat_out_time,120),120)>=CONVERT(VARCHAR(10),CONVERT(DATETIME,'" + dtp_start.Value.ToString("yyyy-MM-dd") + " 00:00:00 ',120),120) and CONVERT(VARCHAR(10),CONVERT(DATETIME,a.pat_out_time,120),120)<=CONVERT(VARCHAR(10),CONVERT(DATETIME,'" + dtp_stop.Value.ToString("yyyy-MM-dd") + " 23:59:59',120),120) and a.pat_in_hos_id in(select pat_in_hos_id from report.dbo.lszy_basc_dw) and a.HOSPITAL_ID='" + _hosId + "' ");

            }
            else
            {
                str_info_s.Append("		   WHERE CONVERT(VARCHAR(10),CONVERT(DATETIME,a.pat_out_time,120),120)>=CONVERT(VARCHAR(10),CONVERT(DATETIME,'" + dtp_start.Value.ToString("yyyy-MM-dd") + " 00:00:00 ',120),120) and CONVERT(VARCHAR(10),CONVERT(DATETIME,a.pat_out_time,120),120)<=CONVERT(VARCHAR(10),CONVERT(DATETIME,'" + dtp_stop.Value.ToString("yyyy-MM-dd") + " 23:59:59',120),120) and a.pat_in_hos_id not in(select pat_in_hos_id from report.dbo.lszy_basc_dw) and a.HOSPITAL_ID='" + _hosId + "' ");

            }
            //str_info_s.Append("		   WHERE CONVERT(VARCHAR(10),CONVERT(DATETIME,k.CHECK_DATE,120),120)>=CONVERT(VARCHAR(10),'" + dtp_start.Value + "',120) and CONVERT(VARCHAR(10),CONVERT(DATETIME,k.CHECK_DATE,120),120)<=CONVERT(VARCHAR(10),'" + dtp_stop.Value + "',120) and a.pat_in_hos_id not in(select pat_in_hos_id from report.dbo.lszy_basc_dw) ");

            if (txt_zyh.Text != "")
            {
                str_info_s.Append("     AND A.MEDICAL_RECORD_CODE='" + txt_zyh.Text.Trim() + "'");
            }
            else
            {

            }

            string str_info = str_info_s.ToString();
            DataSet ds = SSS.ExecSqlReDs(str_info);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return ds.Tables[0];
            }
        }

        /// <summary>
        /// 第二步：获取某个病人病案首页的诊断数据集
        /// </summary>
        /// <param name="pat_in_hos_code">住院号</param>
        /// <returns></returns>
        private DataTable get_diagnosis(string pat_in_hos_code)
        {
            StringBuilder str_diagnosis_s = new StringBuilder();

            //str_diagnosis_s.Append("SELECT  id AS sxh ,");
            //str_diagnosis_s.Append("        ( CASE WHEN a.IS_MAIN_DIAGNOSIS = 1 THEN 12 ELSE 13 END ) jbzdlb ,");
            //str_diagnosis_s.Append("        a.SICKNESS_CODE AS jbbm,");
            //str_diagnosis_s.Append("        a.PAT_IN_STATUS AS rybq,");
            //str_diagnosis_s.Append("        '' AS zdms ");
            //str_diagnosis_s.Append("FROM    MedicalRecord.MEDICAL.MEDICAL_RECORD_DIAGNOSIS_DETAIL a");
            //str_diagnosis_s.Append("		WHERE   a.MEDICAL_RECORD_CODE = '"+ pat_in_hos_code + "'	");

            str_diagnosis_s.Append("SELECT top 1  cast(ORDER_NO as varchar(50)) AS sxh ,");
            str_diagnosis_s.Append("        ( CASE WHEN a.ORDER_NO = 0 THEN '12' ELSE '13' END ) jbzdlb ,");
            str_diagnosis_s.Append("        a.DIAGNOSIS_CODE AS jbbm,");
            str_diagnosis_s.Append("        '1' AS rybq,");
            str_diagnosis_s.Append("        '' AS zdms ");
            str_diagnosis_s.Append("FROM    MedicalRecord.MEDICAL.NETWORKING_DIAGNOSIS_MSG a");
            str_diagnosis_s.Append("		WHERE   a.MEDICAL_RECORD_CODE = '" + pat_in_hos_code + "' ORDER BY  a.PAT_IN_COUNT desc	");

            string str_diagnosis = str_diagnosis_s.ToString();
            DataSet ds_disgnosis = SSS.ExecSqlReDs(str_diagnosis);
            return ds_disgnosis.Tables[0];
        }

        /// <summary>
        /// 第三步：获取某个病人的病案首页手术信息数据集
        /// </summary>
        /// <param name="pat_in_hos_code">住院号</param>
        /// <returns></returns>
        private DataTable get_SURGERIES(string pat_in_hos_code)
        {
            StringBuilder str_SURGERIES_s = new StringBuilder();

            str_SURGERIES_s.Append("SELECT  top 1 CAST(row_number()over(order by a.id asc) AS VARCHAR(10))  AS sxh ,");
            str_SURGERIES_s.Append("        a.OPERATION_CODE AS ssbm ,");
            str_SURGERIES_s.Append("        b.USER_CODE AS ssysbm ,");
            str_SURGERIES_s.Append("        convert(varchar(19),replace(a.OPERATION_DATE,'  ',' '),120) AS czrq ,");
            str_SURGERIES_s.Append("        '无' AS ssjl ,");
            str_SURGERIES_s.Append("        a.OPERATION_LEVEL AS ssjb ,");
            str_SURGERIES_s.Append("        CASE a.CURE_LEVEL WHEN '0' THEN '0' WHEN '1' THEN 'I' WHEN '2' THEN 'II' WHEN '3' THEN 'III' ELSE '0' end  AS qklb ,");
            str_SURGERIES_s.Append("        CASE a.HEAL_LEVEL WHEN '1' THEN '1' WHEN '2' THEN '2' WHEN '3' THEN '3' WHEN '4' THEN '9' ELSE '9' END  AS yhdj ,");
            str_SURGERIES_s.Append("        a.HOCUS_METHOD AS mzfs ,");
            str_SURGERIES_s.Append("        c.USER_CODE AS mzysbm ");
            str_SURGERIES_s.Append("FROM    MedicalRecord.MEDICAL.MEDICAL_RECORD_OPERATION_MSG a ");
            str_SURGERIES_s.Append("        LEFT JOIN COMM.COMM.USERS b ON a.OPERATOR = b.[USER_ID] ");
            str_SURGERIES_s.Append("        LEFT JOIN COMM.COMM.USERS c ON a.HOCUS_DOCTOR = C.[USER_ID] ");
            str_SURGERIES_s.Append("WHERE   MEDICAL_RECORD_CODE = '" + pat_in_hos_code + "'  AND OPERATION_CODE<>'' ORDER BY  a.PAT_IN_COUNT desc ");

            string str_SURGERIES = str_SURGERIES_s.ToString();
            DataSet ds_SURGERIES = SSS.ExecSqlReDs(str_SURGERIES);
            return ds_SURGERIES.Tables[0];
        }

        private void dgv_main_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dgrmain = this.dgv_main.Rows[this.dgv_main.CurrentRow.Index];
 
            DataTable ds_diagnosis = get_diagnosis_ds(dgrmain.Cells["病例号"].Value.ToString());
            DataTable ds_operation = get_SURGERIES_ds(dgrmain.Cells["病例号"].Value.ToString());
            dgv_diagnosis.DataSource = ds_diagnosis;
            dgv_operation.DataSource = ds_operation;
        }

        private void dgv_main_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void dgv_main_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Up" || e.KeyCode.ToString() == "Down")
            {
                DataGridViewRow dgrmain = this.dgv_main.Rows[this.dgv_main.CurrentRow.Index];

                DataTable ds_diagnosis = get_diagnosis_ds(dgrmain.Cells["病例号"].Value.ToString());
                DataTable ds_operation = get_SURGERIES_ds(dgrmain.Cells["病例号"].Value.ToString());
                dgv_diagnosis.DataSource = ds_diagnosis;
                dgv_operation.DataSource = ds_operation;

            }

        }


        /// <summary>
        /// 获取已经归档的病人的病案首页
        /// </summary>
        /// <returns></returns>
        private DataTable get_maininfo_ds()
        {
            StringBuilder str_info_s = new StringBuilder();

            str_info_s.Append("SELECT  a.MEDICAL_RECORD_CODE AS 病例号 ,");
            str_info_s.Append("        a.SICK_NAME AS 病人姓名 ,");
            str_info_s.Append("        a.PAT_IN_WAY AS 入院途径 ,");
            str_info_s.Append("        CONVERT(DATETIME,a.PAT_IN_TIME,120) AS 住院日期 ,");
            str_info_s.Append("        CONVERT(DATETIME,a.PAT_OUT_TIME,120) 出院日期 ,");
            str_info_s.Append("        isnull((case when b.DIAGNOSE_TIME='0001-01-01'then CONVERT(DATETIME,a.PAT_IN_TIME,120) else CONVERT(DATETIME,b.DIAGNOSE_TIME,120) end), CONVERT(DATETIME,a.PAT_IN_TIME,120)) AS 确诊日期 ,");
            str_info_s.Append("        a.PAT_IN_DEPT AS 入院科室 ,");
            str_info_s.Append("        a.PAT_OUT_DEPT AS 出院科室 ,");
            str_info_s.Append("        b.BLOOD_TYPE AS 血型 ,");
            str_info_s.Append("        b.RH AS RH血型 ,");
            str_info_s.Append("        '' AS 离院方式 ,");
            str_info_s.Append("        '' AS 出院小结 ,");
            str_info_s.Append("        ISNULL(c.USER_CODE,'') AS 主治医师 ,");
            str_info_s.Append("        ISNULL(d.USER_CODE,'') AS 住院医师 ,");
            str_info_s.Append("        a.RELATIVE_NAME AS 联系人 ,");
            str_info_s.Append("        a.RELATION AS 联系人关系 ,");
            str_info_s.Append("        a.RELATIVE_PHONE AS 联系人电话 ,");
            str_info_s.Append("        a.ADDRESS_PHONE AS 病人电话 ,");
            str_info_s.Append("        ISNULL(e.USER_CODE,'') AS 责任护士 ,");
            str_info_s.Append("        ISNULL(f.USER_CODE,'') AS 科室主任 ,");
            str_info_s.Append("        ISNULL(g.USER_CODE,'') AS 主任医师 ,");
            str_info_s.Append("        ISNULL(h.USER_CODE,'') AS 进修医师 ,");
            str_info_s.Append("        ISNULL(j.USER_CODE,'') AS 实习医师 ,");
            str_info_s.Append("        '' AS 转出医疗机构编码 ");
            str_info_s.Append("FROM    MedicalRecord.MEDICAL.MEDICAL_RECORD_BASE_MSG a");
            //str_info_s.Append("        INNER JOIN MedicalRecord.MEDICAL.MEDICAL_RECORD_CHECK k ON a.MEDICAL_RECORD_CODE = k.MEDICAL_CODE");
            str_info_s.Append("        LEFT JOIN MedicalRecord.MEDICAL.MEDICAL_RECORD_DIAGNOSIS b ON a.MEDICAL_RECORD_CODE = b.MEDICAL_CODE AND a.ID = b.BASE_MSG_ID");
            str_info_s.Append("        LEFT JOIN COMM.COMM.users c ON b.MAIN_DOCTOR = c.[USER_ID]");
            str_info_s.Append("        LEFT JOIN COMM.COMM.users d ON b.PAT_IN_DOCTOR = d.[USER_ID]");
            str_info_s.Append("        LEFT JOIN COMM.COMM.users e ON b.RESP_NURSE = e.[USER_ID]");
            str_info_s.Append("        LEFT JOIN COMM.COMM.users f ON b.DEPT_HEAD = f.[USER_ID]");
            str_info_s.Append("        LEFT JOIN COMM.COMM.users g ON b.DEPUTY_HEAD = g.[USER_ID]");
            str_info_s.Append("        LEFT JOIN COMM.COMM.users h ON b.ADVANCED_DOCTOR = h.[USER_ID]");
            str_info_s.Append("        LEFT JOIN COMM.COMM.users j ON b.STUDY_DOCTOR = j.[USER_ID]");
            str_info_s.Append("        LEFT JOIN ZY.[IN].PAT_OUT_HOSPITAL k ON  k.PAT_IN_HOS_ID = a.PAT_IN_HOS_ID ");
            if (comboBox1.Text == "已上传")
            {
                str_info_s.Append("		   WHERE CONVERT(VARCHAR(10),CONVERT(DATETIME,a.pat_out_time,120),120)>=CONVERT(VARCHAR(10),CONVERT(DATETIME,'" + dtp_start.Value.ToString("yyyy-MM-dd") + " 00:00:00',120),120) and CONVERT(VARCHAR(10),CONVERT(DATETIME,a.pat_out_time,120),120)<=CONVERT(VARCHAR(10),CONVERT(DATETIME,'" + dtp_stop.Value.ToString("yyyy-MM-dd") + " 23:59:59',120),120) and a.pat_in_hos_id in(select pat_in_hos_id from report.dbo.lszy_basc_dw) and a.HOSPITAL_ID='"+_hosId+"' ");

            }
            else
            {
                str_info_s.Append("		   WHERE CONVERT(VARCHAR(10),CONVERT(DATETIME,a.pat_out_time,120),120)>=CONVERT(VARCHAR(10),CONVERT(DATETIME,'" + dtp_start.Value.ToString("yyyy-MM-dd") + " 00:00:00',120),120) and CONVERT(VARCHAR(10),CONVERT(DATETIME,a.pat_out_time,120),120)<=CONVERT(VARCHAR(10),CONVERT(DATETIME,'" + dtp_stop.Value.ToString("yyyy-MM-dd") + " 23:59:59',120),120) and a.pat_in_hos_id not in(select pat_in_hos_id from report.dbo.lszy_basc_dw) and a.HOSPITAL_ID='"+_hosId+"' ");

            }
            //str_info_s.Append("		   WHERE CONVERT(VARCHAR(10),CONVERT(DATETIME,k.CHECK_DATE,120),120)>=CONVERT(VARCHAR(10),'" + dtp_start.Value + "',120) and CONVERT(VARCHAR(10),CONVERT(DATETIME,k.CHECK_DATE,120),120)<=CONVERT(VARCHAR(10),'" + dtp_stop.Value + "',120) and a.pat_in_hos_id not in(select pat_in_hos_id from report.dbo.lszy_basc_dw) ");
            str_info_s.Append(" AND k.OPERATOR_DEPT_ID IN ( SELECT  DEPT_ID FROM    COMM.COMM.USERS_SYS WHERE   USER_ID = '" + _userId + "' ) ");
            if (txt_zyh.Text != "")
            {
                str_info_s.Append("     AND A.MEDICAL_RECORD_CODE='" + txt_zyh.Text.Trim() + "'");
            }
            else
            {

            }
            string str_info = str_info_s.ToString();
            DataSet ds = SSS.ExecSqlReDs(str_info);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return ds.Tables[0];
            }
        }
        /// <summary>
        /// 获取某个病人病案首页的诊断数据集
        /// </summary>
        /// <param name="pat_in_hos_code">住院号</param>
        /// <returns></returns>
        private DataTable get_diagnosis_ds(string pat_in_hos_code)
        {
            StringBuilder str_diagnosis_s = new StringBuilder();

            //str_diagnosis_s.Append("SELECT  a.SICKNESS_CODE AS 疾病编码 ,");
            //str_diagnosis_s.Append("        a.PAT_OUT_DIAGNOSIS_NAME AS 疾病名称 ,");
            //str_diagnosis_s.Append("        id AS 顺序号,");
            //str_diagnosis_s.Append("        ( CASE WHEN a.IS_MAIN_DIAGNOSIS = 1 THEN 12 ELSE 13 END ) 疾病诊断类别,");
            //str_diagnosis_s.Append("        a.PAT_IN_STATUS AS 入院病情,");
            //str_diagnosis_s.Append("        '' AS 诊断说明 ");
            //str_diagnosis_s.Append("FROM    MedicalRecord.MEDICAL.MEDICAL_RECORD_DIAGNOSIS_DETAIL a");
            //str_diagnosis_s.Append("		WHERE   a.MEDICAL_RECORD_CODE = '" + pat_in_hos_code + "'	");

            str_diagnosis_s.Append("SELECT top 1  a.DIAGNOSIS_CODE AS 疾病编码 ,");
            str_diagnosis_s.Append("        a.DIAGNOSIS_NAME AS 疾病名称 ,");
            str_diagnosis_s.Append("        id AS 顺序号,");
            str_diagnosis_s.Append("        ( CASE WHEN a.order_no = 0 THEN '12' ELSE '13' END ) 疾病诊断类别,");
            str_diagnosis_s.Append("        '' AS 入院病情,");
            str_diagnosis_s.Append("        '' AS 诊断说明 ");
            str_diagnosis_s.Append("FROM    MedicalRecord.MEDICAL.NETWORKING_DIAGNOSIS_MSG a");
            str_diagnosis_s.Append("		WHERE   a.MEDICAL_RECORD_CODE = '" + pat_in_hos_code + "' ORDER BY  a.PAT_IN_COUNT desc	");

            string str_diagnosis = str_diagnosis_s.ToString();
            DataSet ds_disgnosis = SSS.ExecSqlReDs(str_diagnosis);
            return ds_disgnosis.Tables[0];
        }

        /// <summary>
        /// 获取某个病人的病案首页手术信息数据集
        /// </summary>
        /// <param name="pat_in_hos_code">住院号</param>
        /// <returns></returns>
        private DataTable get_SURGERIES_ds(string pat_in_hos_code)
        {
            StringBuilder str_SURGERIES_s = new StringBuilder();

            str_SURGERIES_s.Append("SELECT top 1 a.OPERATION_CODE AS 手术编码 ,");
            str_SURGERIES_s.Append("        a.OPERATION_NAME AS 手术名称 ,");
            str_SURGERIES_s.Append("        a.id AS 顺序号 ,");
            str_SURGERIES_s.Append("        b.USER_CODE AS 手术医生编码 ,");
            str_SURGERIES_s.Append("        CONVERT(DATETIME,replace(a.OPERATION_DATE,'  ',' '),120) AS 操作日期 ,");
            str_SURGERIES_s.Append("        '无' AS 手术记录 ,");
            str_SURGERIES_s.Append("        a.OPERATION_LEVEL AS 手术级别 ,");
            str_SURGERIES_s.Append("        CASE a.CURE_LEVEL WHEN '0' THEN '0' WHEN '1' THEN 'I' WHEN '2' THEN 'II' WHEN '3' THEN 'III' ELSE '0' end  AS 切口类别 ,");
            str_SURGERIES_s.Append("        CASE a.HEAL_LEVEL WHEN '1' THEN '1' WHEN '2' THEN '2' WHEN '3' THEN '3' WHEN '4' THEN '9' ELSE '9' END  AS 愈合等级 ,");
            str_SURGERIES_s.Append("        a.HOCUS_METHOD AS 麻醉方式 ,");
            str_SURGERIES_s.Append("        c.USER_CODE AS 麻醉医师编码 ");
            str_SURGERIES_s.Append("FROM    MedicalRecord.MEDICAL.MEDICAL_RECORD_OPERATION_MSG a ");
            str_SURGERIES_s.Append("        LEFT JOIN COMM.COMM.USERS b ON a.OPERATOR = b.[USER_ID] ");
            str_SURGERIES_s.Append("        LEFT JOIN COMM.COMM.USERS c ON a.HOCUS_DOCTOR = C.[USER_ID] ");
            str_SURGERIES_s.Append("WHERE   MEDICAL_RECORD_CODE = '" + pat_in_hos_code + "'  AND OPERATION_CODE<>'' ORDER BY  a.PAT_IN_COUNT desc ");

            string str_SURGERIES = str_SURGERIES_s.ToString();
            DataSet ds_SURGERIES = SSS.ExecSqlReDs(str_SURGERIES);
            return ds_SURGERIES.Tables[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable ds_info = get_maininfo_ds();
            dgv_main.DataSource = ds_info;
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1_Tick(sender,e);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dtp_start_ValueChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 第四步：上传保存病例首页信息
        /// </summary>
        //private void med_upload()
        //{
        //    handelModel.Handle("save_case",true);
        //}

        #region 上传疾病诊断编码
        /// <summary>
        /// 上传疾病诊断编码
        /// </summary>
        /// <param name="Diagnosis"></param>
        public void upDiagnosis(DataTable jbzds)
        {
            foreach (DataRow jbzd in jbzds.Rows)
            {
                handelModel.ClearInPara();
                handelModel.SetInParaString("sxh", jbzd["sxh"]);
                handelModel.SetInParaString("jbzdlb", jbzd["jbzdlb"]);
                handelModel.SetInParaString("jbbm", jbzd["jbbm"]);
                //handelModel.SetInParaString("jbbm", "I10.02");
                handelModel.SetInParaString("rybq", jbzd["rybq"]);
                handelModel.SetInParaString("zdms", jbzd["zdms"]);
                decimal iRe = handelModel.ExecService("put_jbzd");
                if (iRe != 0)
                {
                    throw new Exception("上传病案首页失败：" + handelModel.ExeFuncReStr("get_errtext", null));
                }
            }
        }
        #endregion
        #region 手术信息上传
        /// <summary>
        /// 手术信息上传
        /// </summary>
        /// <param name="ssxxs"></param>
        public void upOper(DataTable ssxxs)
        {
            foreach (DataRow ssxx in ssxxs.Rows)
            {
                handelModel.ClearInPara();
                handelModel.SetInParaString("sxh", ssxx["sxh"]);
                handelModel.SetInParaString("ssbm", "16*" + ssxx["ssbm"]);
                handelModel.SetInParaString("ssysbm", ssxx["ssysbm"]);
                handelModel.SetInParaDate("czrq", DateTime.Parse(ssxx["czrq"].ToString()).ToString("yyyy-MM-dd hh:mm:ss"));
                handelModel.SetInParaString("ssjl", ssxx["ssjl"]);
                handelModel.SetInParaString("ssjb", ssxx["ssjb"]);
                handelModel.SetInParaString("qklb", ssxx["qklb"]);
                handelModel.SetInParaString("yhdj", ssxx["yhdj"]);
                handelModel.SetInParaString("mzfs", ssxx["mzfs"]);
                handelModel.SetInParaString("mzysbm", ssxx["mzysbm"]);
                decimal iRe = handelModel.ExecService("put_ssxx");
                if (iRe != 0)
                {
                    throw new Exception("上传病案首页失败：" + handelModel.ExeFuncReStr("get_errtext", null));
                }
            }
        }
        #endregion
        #region 病历删除
        public void deleteCase(string zyh)
        {
            handelModel.ClearInPara();
            handelModel.SetInParaString("zyh", zyh);
            decimal iRe = handelModel.ExecService("delete_case");
        }
        #endregion
    }
}
