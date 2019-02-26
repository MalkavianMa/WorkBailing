using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using PayAPIInterface.Model.Comm;
using PayAPIInterface.ParaModel;

namespace PayAPIInstance.Neusoft.Dialogs
{
    public partial class InfoDiagnosis : Form
    {
        //服务器数据查询
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        private bool isOut = false;

        public bool isOk = false;//是否存在结果

        public string ReDiagnosName = "";//诊断名称
        public string ReDiagnosCode = "";//诊断编号
        public string ReDiagnosNameT = "";//
        public string ReDiagnosCodeT = "";//er
        public string ReDiagnosCode0 = ""; // san 
        public string ReDiagnosName0 = "";
        public string ReTypeCode = "";//医疗类型编号
        public string ReTypeName = "";//医疗类型名称
        public NetworkPatInfo netPatInfo;  //医保个人信息 
        public string OutHosStatus;//出院状态
        public string CardNo = "";
        public string mxbYxDate = ""; //慢性病审批最后时间
        public bool flagMxb = false;  //慢性病标志,默认为非慢性病

        string hosID = "1";

        public MSSQLHelper sqlHelper = new MSSQLHelper(PayAPIInstance.JingQi.FuYang.PubComm.ConnStr);

        /// <summary>
        /// 可修改初始化类型
        /// </summary>
        public string strDefaultSec = "普通住院";
        /// <summary>
        /// 可修改初始化诊断
        /// </summary>
        public string strDefaultOutDia = "";
        private bool bLoadingDia = false;//载入默认诊断时


        private OutPayParameter outPayPara;

        private InPayParameter inPayPara;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isOut">true门诊false住院</param>
        public InfoDiagnosis(bool IsOut, NetworkPatInfo patInfo, OutPayParameter _outPayPara, InPayParameter _inPayPara)
        {
            InitializeComponent();
            this.isOut = IsOut;
            this.netPatInfo = patInfo;

            outPayPara = _outPayPara;
            inPayPara = _inPayPara;
        }

        private int _nDefalt = -1;
        private int _nDefalt2 = 0;
        private void InfoDiagnosis_Load(object sender, EventArgs e)
        {

            flagMxb = ismxb(netPatInfo.MedicalNo);
            if (isOut)
            {
                tbDiagnosCode1.Visible = false;
                tbDiagnosCode2.Visible = false;
                tbDignosis1.Visible = false;
                tbDignosis2.Visible = false;
                label12.Visible = false;
                label13.Visible = false;
                label14.Visible = false;
                label15.Visible = false;
            }


            if (!flagMxb || !isOut)
            {
                //患者不是慢性病 
                DataSet ds = new DataSet();

                string strsql = "";

                #region 本地获取医疗类别
                if (isOut)
                {
                    strsql = "SELECT NET_TYPE_CODE TYPE_CODE,NET_TYPE_NAME TYPE_NAME FROM COMM.DICT.NETWORKING_NET_TYPE WHERE NETWORKING_PAT_CLASS_ID=1 AND TYPE_FLAG=0";
                    ds = sqlHelper.ExecSqlReDs(strsql);
                    // ds = query.QueryType("1", "0");
                }
                else
                {
                    strsql = "SELECT NET_TYPE_CODE TYPE_CODE,NET_TYPE_NAME TYPE_NAME FROM COMM.DICT.NETWORKING_NET_TYPE WHERE NETWORKING_PAT_CLASS_ID=1 AND TYPE_FLAG=1";
                    ds = sqlHelper.ExecSqlReDs(strsql);
                }
                DataTable dtType = ds.Tables[0];
                #endregion

                #region 中心获取医疗类别列表
                //CommReimBLL bll = new CommReimBLL();
                //DataTable dtType = bll.GetYB_Type_Dict(isOut ? "1" : "0", isOut ? "0" : "1"); 
                #endregion

                ArrayList al = new ArrayList();
                for (int i = 0; i < dtType.Rows.Count; i++)
                {
                    al.Add(new DictionaryEntry(dtType.Rows[i]["TYPE_CODE"].ToString(), dtType.Rows[i]["TYPE_NAME"].ToString()));
                    if (dtType.Rows[i]["TYPE_NAME"].ToString().Trim() == strDefaultSec)
                        _nDefalt = i;
                    if (dtType.Rows[i]["TYPE_NAME"].ToString().Trim() == "普通住院")
                        _nDefalt2 = i;
                }
                cbType.ValueMember = "Key";
                cbType.DisplayMember = "Value";
                cbType.DataSource = al;
                if (cbType.Items.Count > 0)
                {
                    if (_nDefalt != -1)
                        cbType.SelectedIndex = _nDefalt;
                    else
                        cbType.SelectedIndex = _nDefalt2;
                }
                if (netPatInfo != null)
                {
                    lblName.Text = netPatInfo.PatName;
                    lblICNo.Text = netPatInfo.ICNo;
                    lblIdent.Text = netPatInfo.IDNo;
                    lblPersonNo.Text = netPatInfo.MedicalNo;
                    lblBalance.Text = netPatInfo.ICAmount.ToString();
                    lblType.Text = netPatInfo.MedicalType;

                    //tbDiagnosCode.Text = netPatInfo.strInDiagnoCode;//自动为诊断赋值
                    //tbDignosis.Text = netPatInfo.strInDiagnoName;   //自动为诊断赋值

                    if (inPayPara != null && inPayPara.PatInfo != null)
                    {
                        tbDiagnosCode.Text = inPayPara.PatInfo.InDiagnoseCode;//自动为诊断赋值
                        tbDignosis.Text = inPayPara.PatInfo.InDiagnoseName;   //自动为诊断赋值 
                    }
                }
                FillOutCb();

                if (strDefaultOutDia != "")
                {
                    //NetworkQueryDALSQLite queryLite = new NetworkQueryDALSQLite();
                    //DataSet dsLite = queryLite.QueryDignosisByName(strDefaultOutDia, 1); 


                    strsql = "SELECT DIAGNOSIS_CODE,DIAGNOSIS_NAME FROM COMM.DICT.DIAGNOSIS WHERE FLAG_INVALID=0  AND INPUT_CODE LIKE '%" + strDefaultOutDia + "%'";
                    DataSet dsLite = sqlHelper.ExecSqlReDs(strsql);


                    if (dsLite.Tables[0].Rows.Count > 0)
                    {
                        bLoadingDia = true;
                        tbDiagnosCode.Text = dsLite.Tables[0].Rows[0]["DIAGNOSIS_CODE"].ToString().Trim();
                        tbDignosis.Text = dsLite.Tables[0].Rows[0]["DIAGNOSIS_NAME"].ToString().Trim();
                    }
                    bLoadingDia = false;
                }
                if (tbDignosis.Text.Trim() != "")
                    btnComplete.Focus();
                else
                    tbDignosis.Focus();
            }
            else
            {   //患者是慢性病患者处理
                #region 中心获取医疗类别列表
                #endregion

                //填充医保类别
                ArrayList al = new ArrayList();
                al.Add(new DictionaryEntry("13", "门诊慢性病"));
                al.Add(new DictionaryEntry("11", "普通门诊"));
                al.Add(new DictionaryEntry("21", "普通住院"));
                al.Add(new DictionaryEntry("91", "生育住院"));
                al.Add(new DictionaryEntry("93", "计生技术"));

                cbType.ValueMember = "Key";
                cbType.DisplayMember = "Value";
                cbType.DataSource = al;
                cbType.SelectedIndex = 0;

                //填充患者相关信息
                if (netPatInfo != null)
                {
                    lblName.Text = netPatInfo.PatName;
                    lblICNo.Text = netPatInfo.ICNo;
                    lblIdent.Text = netPatInfo.IDNo;
                    lblPersonNo.Text = netPatInfo.MedicalNo;
                    lblBalance.Text = netPatInfo.ICAmount.ToString();
                    lblType.Text = netPatInfo.MedicalType;
                }

                FillOutCb();

                if (ReDiagnosCode != "")
                {

                    //NetworkQueryDALSQLite queryLite = new NetworkQueryDALSQLite();
                    //DataSet dsLite = queryLite.QueryDignosisByName(ReDiagnosCode, 1);
                    //if (dsLite.Tables[0].Rows.Count > 0)
                    //{
                    //    bLoadingDia = true;
                    //    tbDiagnosCode.Text = ReDiagnosCode.Trim();
                    //    tbDignosis.Text = dsLite.Tables[0].Rows[0]["DIAGNOSIS_NAME"].ToString().Trim(); ;
                    //}

                    tbDiagnosCode.Text = dt.Rows[0]["BZBM"].ToString().Trim();
                    tbDignosis.Text = dt.Rows[0]["BZMC"].ToString().Trim();

                    bLoadingDia = false;
                }
                if (tbDignosis.Text.Trim() != "")
                    btnComplete.Focus();
                else
                    tbDignosis.Focus();
            }
        }



        /// <summary>
        /// 填充出院状态
        /// </summary>
        private void FillOutCb()
        {
            //出院状态
            ArrayList arr2 = new ArrayList();
            arr2.Add(new DictionaryEntry("0", "正常治愈出院"));
            arr2.Add(new DictionaryEntry("1", "未治愈出院"));
            arr2.Add(new DictionaryEntry("2", "医院要求转院"));
            arr2.Add(new DictionaryEntry("3", "病人要求转院"));
            arr2.Add(new DictionaryEntry("4", "院内相关疾病转科治疗"));
            arr2.Add(new DictionaryEntry("5", "院内无关疾病专科治疗"));
            arr2.Add(new DictionaryEntry("6", "死亡"));
            cbOutStatus.DataSource = arr2;
            cbOutStatus.DisplayMember = "Value";
            cbOutStatus.ValueMember = "Key";
            cbOutStatus.SelectedIndex = 0;
            //如果是门诊则隐藏
            if (isOut)
            {
                label11.Visible = false;
                cbOutStatus.Visible = false;
            }
        }

        private void tbDignosis_KeyDown(object sender, KeyEventArgs e)
        {
            int n = -1;
            if (dataGv.Rows.Count > 0)
                n = dataGv.SelectedRows[0].Index;
            if (e.KeyCode == Keys.Down && n < dataGv.Rows.Count - 1)
            {
                dataGv.Rows[n + 1].Selected = true;
                dataGv.FirstDisplayedScrollingRowIndex = n + 1;
            }
            if (e.KeyCode == Keys.Up && n > 0)
            {
                dataGv.Rows[n - 1].Selected = true;
                dataGv.FirstDisplayedScrollingRowIndex = n - 1;
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (dataGv.SelectedRows.Count > 0)
                {
                    tbDiagnosCode.Text = dataGv.SelectedRows[0].Cells[0].Value.ToString();
                    tbDignosis.Text = dataGv.SelectedRows[0].Cells[1].Value.ToString();
                }
                dataGv.Visible = false;
                btnComplete.Focus();
            }
        }

        private void tbDignosis_TextChanged(object sender, EventArgs e)
        {
            if (bLoadingDia)
                return;
            if (tbDignosis.Text.Trim() == "")
            {
                dataGv.Visible = false;
                tbDiagnosCode.Text = "";
                return;
            }
            #region 本地查询诊断

            //NetworkQueryDALSQLite query = new NetworkQueryDALSQLite();
            //DataSet ds = query.QueryDignosis(tbDignosis.Text.Trim(), 1);

            string strsql = "SELECT CENTER_DIAGNOSIS_CODE AS DIAGNOSIS_CODE, CENTER_DIAGNOSIS_NAME AS DIAGNOSIS_NAME  FROM comm.DICT.NETWORKING_DIAGNOSIS_DICT WHERE NETWORKING_PAT_CLASS_ID=1 AND  INPUT_CODE LIKE  '%" + tbDignosis.Text.Trim() + "%'";
            // string strsql = "SELECT DIAGNOSIS_CODE,DIAGNOSIS_NAME FROM COMM.DICT.DIAGNOSIS WHERE FLAG_INVALID=0  AND INPUT_CODE LIKE '%" + tbDignosis.Text.Trim() + "%'";


            DataSet ds = sqlHelper.ExecSqlReDs(strsql);

            if (ds.Tables[0].Rows.Count > 0)
                dataGv.Visible = true;
            ds.Tables[0].TableName = "Disgnos";
            dataGv.DataSource = ds;
            dataGv.DataMember = "Disgnos";
            #endregion

            #region 从中心查询疾病诊断信息
            //CommReimBLL bll = new CommReimBLL();
            //DataTable dt = bll.GetDiagnosisInfo(tbDiagnosCode.Text, "1");
            //if (dt.Rows.Count > 0)
            //    dataGv.Visible = true;
            //dt.TableName = "Diagnos";
            //dataGv.DataSource = dt;
            //dataGv.DataMember = "Diagnos"; 
            #endregion

            dataGv.Left = label2.Left + groupBox2.Left;
            dataGv.Top = tbDignosis.Top + groupBox2.Top + tbDignosis.Height;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isOk = false;
            ReDiagnosName = ReDiagnosCode = ReTypeCode = ReTypeName = "";
            this.Close();
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            //if (dataGv.SelectedRows.Count != 1)
            //    return;
            if (((!isOut) || (isOut && cbType.Text.Trim() == "工伤门诊")) && tbDiagnosCode.Text.Trim().Length == 0)
            {
                MessageBox.Show("请填写诊断!");
                return;
            }
            ReDiagnosCode = tbDiagnosCode.Text.Trim();
            ReDiagnosName = tbDignosis.Text.Trim();
            DictionaryEntry de = (DictionaryEntry)cbType.SelectedItem;
            ReTypeCode = de.Key.ToString();
            ReTypeName = de.Value.ToString();
            de = (DictionaryEntry)cbOutStatus.SelectedItem;
            OutHosStatus = de.Key.ToString();
            isOk = true;
            this.Close();
        }

        private void dataGv_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dataGv.SelectedRows.Count != 1)
                return;
            if (tbDignosis.Text != "" && tbDignosis1.Text == "" && tbDignosis2.Text == "")
            {
                tbDiagnosCode.Text = dataGv.SelectedRows[0].Cells[0].Value.ToString();
                tbDignosis.Text = dataGv.SelectedRows[0].Cells[1].Value.ToString();
            }
            if (tbDignosis1.Text != "" && tbDignosis.Text != "" && tbDignosis2.Text == "")
            {
                tbDiagnosCode1.Text = dataGv.SelectedRows[0].Cells[0].Value.ToString();
                tbDignosis1.Text = dataGv.SelectedRows[0].Cells[1].Value.ToString();
            }
            if (tbDignosis1.Text != "" && tbDignosis.Text != "" && tbDignosis2.Text != "")
            {
                tbDiagnosCode2.Text = dataGv.SelectedRows[0].Cells[0].Value.ToString();
                tbDignosis2.Text = dataGv.SelectedRows[0].Cells[1].Value.ToString();
            }
            ReDiagnosCode = tbDiagnosCode.Text.Trim();
            ReDiagnosName = tbDignosis.Text.Trim();
            ReDiagnosCodeT = tbDiagnosCode1.Text.Trim();
            ReDiagnosNameT = tbDignosis1.Text.Trim();
            ReDiagnosName0 = tbDignosis2.Text.Trim();
            ReDiagnosCode0 = tbDiagnosCode2.Text.Trim();
            DictionaryEntry de = (DictionaryEntry)cbType.SelectedItem;
            ReTypeCode = de.Key.ToString();
            ReTypeName = de.Value.ToString();
            dataGv.Visible = false;
            btnComplete.Focus();
        }

        private void dataGv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (dataGv.SelectedRows.Count != 1)
                    return;

                if (tbDignosis.Text == "")
                {
                    tbDiagnosCode.Text = dataGv.SelectedRows[0].Cells[0].Value.ToString();
                    tbDignosis.Text = dataGv.SelectedRows[0].Cells[1].Value.ToString();
                }
                if (tbDignosis1.Text == "" && tbDignosis.Text != "" && tbDignosis2.Text == "")
                {
                    tbDiagnosCode1.Text = dataGv.SelectedRows[0].Cells[0].Value.ToString();
                    tbDignosis1.Text = dataGv.SelectedRows[0].Cells[1].Value.ToString();
                }
                if (tbDignosis1.Text != "" && tbDignosis.Text != "" && tbDignosis2.Text == "")
                {
                    tbDiagnosCode2.Text = dataGv.SelectedRows[0].Cells[0].Value.ToString();
                    tbDignosis2.Text = dataGv.SelectedRows[0].Cells[1].Value.ToString();
                }

                ReDiagnosCode = tbDiagnosCode.Text.Trim();
                ReDiagnosName = tbDignosis.Text.Trim();
                ReDiagnosCodeT = tbDiagnosCode1.Text.Trim();
                ReDiagnosNameT = tbDignosis1.Text.Trim();
                ReDiagnosName0 = tbDignosis2.Text.Trim();
                ReDiagnosCode0 = tbDiagnosCode2.Text.Trim();
                DictionaryEntry de = (DictionaryEntry)cbType.SelectedItem;
                ReTypeCode = de.Key.ToString();
                ReTypeName = de.Value.ToString();
                dataGv.Visible = false;
                btnComplete.Focus();
            }
        }

        //医保下载按钮事件
        #region 医保下载按钮事件
        private void btnDown_Click(object sender, EventArgs e)
        {
            string conn = PayAPIInstance.JingQi.FuYang.PubComm.ConnStr;
            FrmDataBatchDown NetworkDown = new FrmDataBatchDown(hosID, conn);
            NetworkDown.ShowDialog();

        }
        #endregion

        //查询传入患者个人编号,判断是否慢性病患者
        private bool ismxb(string Grbh)
        {
            string strSQL = "";
            strSQL = "SELECT GRBH,BZBM,BZMC,SPJZYYBH FROM REPORT.dbo.Net_PAT_INFO WHERE  SPLB='13' AND SPBZ='1' AND SPJZYYBH='34122004' AND GRBH='" + Grbh + "'";
            //原带有审批类别,开发文档15为慢性病,但医院给的测试卡 类别为13,导致语句失败,检索不到患者
            //strSQL = "SELECT GRBH,BZBM,BZBM FROM REPORT.dbo.Net_PAT_INFO WHERE  SPLB='15' AND SPBZ='1' AND GRBH='"+Grbh+"'";

            ds = sqlHelper.ExecSqlReDs(strSQL);
            dt = ds.Tables[0]; 
            if (dt.Rows.Count > 0)
            {
                //是慢性病患者
                ReDiagnosCode = dt.Rows[0]["BZBM"].ToString().Trim();
                return true;
            }
            else
            {
                return false;
            }

        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        ////////// 添加第二诊断
        private void tbDignosis1_KeyDown(object sender, KeyEventArgs e)
        {
            int n = -1;
            if (dataGv.Rows.Count > 0)
                n = dataGv.SelectedRows[0].Index;
            if (e.KeyCode == Keys.Down && n < dataGv.Rows.Count - 1)
            {
                dataGv.Rows[n + 1].Selected = true;
                dataGv.FirstDisplayedScrollingRowIndex = n + 1;
            }
            if (e.KeyCode == Keys.Up && n > 0)
            {
                dataGv.Rows[n - 1].Selected = true;
                dataGv.FirstDisplayedScrollingRowIndex = n - 1;
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (dataGv.SelectedRows.Count > 0)
                {
                    tbDiagnosCode1.Text = dataGv.SelectedRows[0].Cells[0].Value.ToString();
                    tbDignosis1.Text = dataGv.SelectedRows[0].Cells[1].Value.ToString();
                }
                dataGv.Visible = false;
                btnComplete.Focus();
            }
        }
        /// <summary>
        /// ////////// 马磊添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbDignosis1_TextChanged(object sender, EventArgs e)
        {
            if (bLoadingDia)
                return;
            if (tbDignosis1.Text.Trim() == "")
            {
                dataGv.Visible = false;
                tbDiagnosCode1.Text = "";
                return;
            }
            #region 本地查询诊断

            //NetworkQueryDALSQLite query = new NetworkQueryDALSQLite();
            //DataSet ds = query.QueryDignosis(tbDignosis.Text.Trim(), 1);


            string strsql = "SELECT CENTER_DIAGNOSIS_CODE AS DIAGNOSIS_CODE, CENTER_DIAGNOSIS_NAME AS DIAGNOSIS_NAME  FROM comm.DICT.NETWORKING_DIAGNOSIS_DICT WHERE NETWORKING_PAT_CLASS_ID=1 AND  INPUT_CODE LIKE '%" + tbDignosis1.Text.Trim() + "%'";
            // string strsql = "SELECT DIAGNOSIS_CODE,DIAGNOSIS_NAME FROM COMM.DICT.DIAGNOSIS WHERE FLAG_INVALID=0  AND INPUT_CODE LIKE '%" + tbDignosis1.Text.Trim() + "%'";
            DataSet ds = sqlHelper.ExecSqlReDs(strsql);

            if (ds.Tables[0].Rows.Count > 0)
                dataGv.Visible = true;
            ds.Tables[0].TableName = "Disgnos";
            dataGv.DataSource = ds;
            dataGv.DataMember = "Disgnos";
            #endregion

            #region 从中心查询疾病诊断信息
            //CommReimBLL bll = new CommReimBLL();
            //DataTable dt = bll.GetDiagnosisInfo(tbDiagnosCode.Text, "1");
            //if (dt.Rows.Count > 0)
            //    dataGv.Visible = true;
            //dt.TableName = "Diagnos";
            //dataGv.DataSource = dt;
            //dataGv.DataMember = "Diagnos"; 
            #endregion

            dataGv.Left = label2.Left + groupBox2.Left;
            dataGv.Top = tbDignosis1.Top + groupBox2.Top + tbDignosis1.Height;
        }
        ////////// 添加第三诊断
        private void tbDignosis2_KeyDown(object sender, KeyEventArgs e)
        {
            int n = -1;
            if (dataGv.Rows.Count > 0)
                n = dataGv.SelectedRows[0].Index;
            if (e.KeyCode == Keys.Down && n < dataGv.Rows.Count - 1)
            {
                dataGv.Rows[n + 1].Selected = true;
                dataGv.FirstDisplayedScrollingRowIndex = n + 1;
            }
            if (e.KeyCode == Keys.Up && n > 0)
            {
                dataGv.Rows[n - 1].Selected = true;
                dataGv.FirstDisplayedScrollingRowIndex = n - 1;
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (dataGv.SelectedRows.Count > 0)
                {
                    tbDiagnosCode2.Text = dataGv.SelectedRows[0].Cells[0].Value.ToString();
                    tbDignosis2.Text = dataGv.SelectedRows[0].Cells[1].Value.ToString();
                }
                dataGv.Visible = false;
                btnComplete.Focus();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbDignosis2_TextChanged(object sender, EventArgs e)
        {
            if (bLoadingDia)
                return;
            if (tbDignosis2.Text.Trim() == "")
            {
                dataGv.Visible = false;
                tbDiagnosCode2.Text = "";
                return;
            }
            #region 本地查询诊断

            //NetworkQueryDALSQLite query = new NetworkQueryDALSQLite();
            //DataSet ds = query.QueryDignosis(tbDignosis.Text.Trim(), 1);

            string strsql = "SELECT CENTER_DIAGNOSIS_CODE AS DIAGNOSIS_CODE, CENTER_DIAGNOSIS_NAME AS DIAGNOSIS_NAME  FROM comm.DICT.NETWORKING_DIAGNOSIS_DICT WHERE NETWORKING_PAT_CLASS_ID=1 AND  INPUT_CODE LIKE '%" + tbDignosis2.Text.Trim() + "%'";
            // string strsql = "SELECT DIAGNOSIS_CODE,DIAGNOSIS_NAME FROM COMM.DICT.DIAGNOSIS WHERE FLAG_INVALID=0  AND INPUT_CODE LIKE '%" + tbDignosis2.Text.Trim() + "%'";

            DataSet ds = sqlHelper.ExecSqlReDs(strsql);

            if (ds.Tables[0].Rows.Count > 0)
                dataGv.Visible = true;
            ds.Tables[0].TableName = "Disgnos";
            dataGv.DataSource = ds;
            dataGv.DataMember = "Disgnos";
            #endregion

            #region 从中心查询疾病诊断信息
            //CommReimBLL bll = new CommReimBLL();
            //DataTable dt = bll.GetDiagnosisInfo(tbDiagnosCode.Text, "1");
            //if (dt.Rows.Count > 0)
            //    dataGv.Visible = true;
            //dt.TableName = "Diagnos";
            //dataGv.DataSource = dt;
            //dataGv.DataMember = "Diagnos"; 
            #endregion

            dataGv.Left = label2.Left + groupBox2.Left;
            dataGv.Top = tbDignosis2.Top + groupBox2.Top + tbDignosis2.Height;
        }

    }
}
