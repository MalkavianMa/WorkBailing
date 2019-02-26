using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;
using System.Xml;
using PayAPIInterface.Model.Comm;
using PayAPIInterface.ParaModel;
using PayAPIInterfaceHandle.AHFYCityWebReference;

namespace PayAPIInstance.JingQi.FuYang.Dialogs
{
    public partial class frmReadCard : Form
    {
        #region 身份证读卡
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct IDCardData
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string Name; //姓名   
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
            public string Sex;   //性别
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string Nation; //名族
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
            public string Born; //出生日期
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 72)]
            public string Address; //住址
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 38)]
            public string IDCardNo; //身份证号
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string GrantDept; //发证机关
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
            public string UserLifeBegin; // 有效开始日期
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
            public string UserLifeEnd;  // 有效截止日期
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 38)]
            public string reserved; // 保留
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
            public string PhotoFileName; // 照片路径
        }

        /************************端口类API *************************/
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_GetCOMBaud", CharSet = CharSet.Ansi)]
        public static extern int Syn_GetCOMBaud(int iPort, ref uint puiBaudRate);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetCOMBaud", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetCOMBaud(int iPort, uint uiCurrBaud, uint uiSetBaud);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_OpenPort", CharSet = CharSet.Ansi)]
        public static extern int Syn_OpenPort(int iPort);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ClosePort", CharSet = CharSet.Ansi)]
        public static extern int Syn_ClosePort(int iPort);
        /**************************SAM类函数 **************************/
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetMaxRFByte", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetMaxRFByte(int iPort, byte ucByte, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ResetSAM", CharSet = CharSet.Ansi)]
        public static extern int Syn_ResetSAM(int iPort, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_GetSAMStatus", CharSet = CharSet.Ansi)]
        public static extern int Syn_GetSAMStatus(int iPort, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_GetSAMID", CharSet = CharSet.Ansi)]
        public static extern int Syn_GetSAMID(int iPort, ref byte pucSAMID, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_GetSAMIDToStr", CharSet = CharSet.Ansi)]
        public static extern int Syn_GetSAMIDToStr(int iPort, ref byte pcSAMID, int iIfOpen);
        /*************************身份证卡类函数 ***************************/
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_StartFindIDCard", CharSet = CharSet.Ansi)]
        public static extern int Syn_StartFindIDCard(int iPort, ref byte pucIIN, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SelectIDCard", CharSet = CharSet.Ansi)]
        public static extern int Syn_SelectIDCard(int iPort, ref byte pucSN, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ReadBaseMsg", CharSet = CharSet.Ansi)]
        public static extern int Syn_ReadBaseMsg(int iPort, ref byte pucCHMsg, ref uint puiCHMsgLen, ref byte pucPHMsg, ref uint puiPHMsgLen, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ReadBaseMsgToFile", CharSet = CharSet.Ansi)]
        public static extern int Syn_ReadBaseMsgToFile(int iPort, ref byte pcCHMsgFileName, ref uint puiCHMsgFileLen, ref byte pcPHMsgFileName, ref uint puiPHMsgFileLen, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ReadBaseFPMsg", CharSet = CharSet.Ansi)]
        public static extern int Syn_ReadBaseFPMsg(int iPort, ref byte pucCHMsg, ref uint puiCHMsgLen, ref byte pucPHMsg, ref uint puiPHMsgLen, ref byte pucFPMsg, ref uint puiFPMsgLen, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ReadBaseFPMsgToFile", CharSet = CharSet.Ansi)]
        public static extern int Syn_ReadBaseFPMsgToFile(int iPort, ref byte pcCHMsgFileName, ref uint puiCHMsgFileLen, ref byte pcPHMsgFileName, ref uint puiPHMsgFileLen, ref byte pcFPMsgFileName, ref uint puiFPMsgFileLen, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ReadNewAppMsg", CharSet = CharSet.Ansi)]
        public static extern int Syn_ReadNewAppMsg(int iPort, ref byte pucAppMsg, ref uint puiAppMsgLen, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_GetBmp", CharSet = CharSet.Ansi)]
        public static extern int Syn_GetBmp(int iPort, ref byte Wlt_File);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ReadMsg", CharSet = CharSet.Ansi)]
        public static extern int Syn_ReadMsg(int iPortID, int iIfOpen, ref IDCardData pIDCardData);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ReadFPMsg", CharSet = CharSet.Ansi)]
        public static extern int Syn_ReadFPMsg(int iPortID, int iIfOpen, ref IDCardData pIDCardData, ref byte cFPhotoname);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_FindReader", CharSet = CharSet.Ansi)]
        public static extern int Syn_FindReader();
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_FindUSBReader", CharSet = CharSet.Ansi)]
        public static extern int Syn_FindUSBReader();
        /***********************设置附加功能函数 ************************/
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetPhotoPath", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetPhotoPath(int iOption, ref byte cPhotoPath);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetPhotoType", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetPhotoType(int iType);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetPhotoName", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetPhotoName(int iType);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetSexType", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetSexType(int iType);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetNationType", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetNationType(int iType);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetBornType", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetBornType(int iType);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetUserLifeBType", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetUserLifeBType(int iType);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetUserLifeEType", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetUserLifeEType(int iType, int iOption);
        [DllImport("JQ_ReadCard.dll")]
        public static extern int Get_CardNo(string ls_qy, StringBuilder s_cardno, StringBuilder s_error);
        int m_iPort;
        #endregion

        #region 参数

        
        /// <summary>
        /// 数据库连接
        /// </summary>
        public MSSQLHelper sqlHelper = new MSSQLHelper(PubComm.ConnStr);

        /// <summary>
        /// 住院参数类
        /// </summary>
        public InPayParameter inReimPara = null;

        /// <summary>
        /// 门诊参数类
        /// </summary>
        public OutPayParameter outReimPara = null;

        /// <summary>
        /// 农合操作类
        /// </summary>
        IJQCenterWebServiceservice serr = new IJQCenterWebServiceservice();

        public NetPatInfo netPatInfo;
        /// <summary>
        /// 地区代码,根据操作员选择进行赋值
        /// </summary>
        public string sAreaCode = "";
        public string sHospitalCode = "5AB552AAF3DB47E055E06177CF51A5C4";   //医疗机构编号
        public string sMedicalCode = "";                                    //医疗证号
        public string sCardCode = "";                                       //医疗卡号

        public string sResult;                                              //农合系统返回值
        public string sMessage = "";                                        //农合系统返回状态信息
        int res;                                                            //农合执行语句扣返回值 1 成功 0失败
        public string strRedeemName = "";                                   //出院补偿类型名称
        public string tel = "";
        /// <summary>
        /// 入院为0 出院为1
        /// </summary>
        public int nStatus;
        /// <summary>
        /// 身份属性名称
        /// </summary>
        public string strIdeName = "";
        /// <summary>
        /// 点击确定返回true 取消返回false
        /// </summary>
        public bool bIsValid;

        public string OutDiagnoseName = "";

        public string treatname = "";
        /// <summary>
        /// 医疗证号 通过GetPersonInfo得到的值
        /// </summary>
        public string MedicalCode = "";
        /// <summary>
        /// 医疗卡号 通过GetPersonInfo得到的值
        /// </summary>
        public string CardCode = "";

        public string sResult1 = "";

        /// <summary>
        /// 转诊单号
        /// </summary>
        public string ZZDH = "";

        /// <summary>
        /// 转诊类型
        /// </summary>
        public string ZZLX = "";

        bool isOut = false;
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="model"></param>
        /// <param name="para"></param>
        /// <param name="patInfo"></param>
        /// <param name="_isOut"></param>
        public frmReadCard(NetPatInfo patInfo, bool _isOut = false, InPayParameter inReimPara = null, OutPayParameter outReimPara = null)
        {
            InitializeComponent();
            this.netPatInfo = patInfo;
            isOut = _isOut;
            this.outReimPara = outReimPara;
            this.inReimPara = inReimPara;
        }

        /// <summary>
        /// 展开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFolding_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 读卡操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadCard_Click(object sender, EventArgs e)
        {
            if (this.tbMedicalNo.Text.Trim().Length == 0)
            {
                readCard(sender, e);
            }

            sAreaCode = this.cbsAreaCode.SelectedValue.ToString();
            sMedicalCode = this.tbMedicalNo.Text.Trim();

            try
            {
                //获取家庭成员信息
                res = serr.GetPersonInfo(sAreaCode, sHospitalCode, sMedicalCode, "", "", "", "", "", "", ref sResult, ref sMessage);

                if (res == 0)
                {
                    throw new Exception("错误信息:" + sMessage);
                }

                XmlDocument ReResulst = new XmlDocument();
                string result = sResult;                                                           //进行医保业务
                ReResulst.LoadXml(result);

                int rowCount = int.Parse(ReResulst.SelectSingleNode("/JQWebService/RowCount").InnerText);

                StringBuilder strinfo = new StringBuilder();
                string smedicalcode = "";
                for (int i = 0; i < rowCount; i++)
                {
                    smedicalcode = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sMedicalCode").InnerText;
                    strinfo.Append("DELETE FROM REPORT.dbo.NH_MESSAGE WHERE  sMedicalCode = '" + smedicalcode + "'; ");
                };

                for (int i = 0; i < rowCount; i++)
                {
                    object[] row;
                    //个人编号,姓名,性别
                    row = new object[] {ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sMedicalCode").InnerText, ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sPeopCode").InnerText, ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sPeopName").InnerText, 
                         //家庭编码,出生日期, 身份证号, 地址
                      ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sSex").InnerText,ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sAge").InnerText,ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sBirthDay").InnerText,ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sIDCardNo").InnerText,
                      ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sAddress").InnerText,ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sCardCode").InnerText,ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sTelephone").InnerText,ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sPeopProp").InnerText,
                      ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sAddrCode").InnerText,ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sFamilyBalance").InnerText,ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sChronicName1").InnerText,ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sChronicName2").InnerText,
                      ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sChronicName3").InnerText};

                    this.dgvFamilyInfo.Rows.Add(row);

                    smedicalcode = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sMedicalCode").InnerText;
                    string sPeopCode = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sPeopCode").InnerText;
                    string sPeopName = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sPeopName").InnerText;
                    string sSex = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sSex").InnerText;
                    string sAge = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sAge").InnerText;
                    string sBirthDay = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sBirthDay").InnerText;
                    string sIDCardNo = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sIDCardNo").InnerText;
                    string sAddress = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sAddress").InnerText;
                    string sCardCode = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sCardCode").InnerText; ;
                    string sTelephone = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sTelephone").InnerText;
                    string sPeopProp = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sPeopProp").InnerText;
                    string sAddrCode = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sAddrCode").InnerText;
                    string sFamilyBalance = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sFamilyBalance").InnerText;
                    string sChronicName1 = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sChronicName1").InnerText;
                    string sChronicName2 = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sChronicName2").InnerText;
                    string sChronicName3 = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sChronicName3").InnerText;

                    string patInHosId = "-1";
                    string outPatId = "-1";

                    if (outReimPara != null)
                    {
                        outPatId = outReimPara.PatInfo.OutPatId.ToString();
                        patInHosId = outPatId;
                    }

                    if (inReimPara != null)
                    {
                        patInHosId = inReimPara.PatInfo.PatInHosId.ToString();
                    }

                    strinfo.Append("INSERT INTO REPORT.dbo.NH_MESSAGE ( sIDCardNo ,sMedicalCode,patinhosid,sPeopName,sAddress ) ");

                    strinfo.Append(" VALUES ('" + sIDCardNo + "','" + smedicalcode + "','" + patInHosId + "','" + sPeopName + "','" + sAddress + "') ");

                    // sqlHelper.ExecSqlReInt(strinfo.ToString());
                }

                try
                {
                    sqlHelper.ExecSqlReInt(strinfo.ToString());
                }
                catch (System.Exception ex)
                {
                }
                this.dgvFamilyInfo.Focus();
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmReadCard_Load(object sender, EventArgs e)
        {
            btnFolding.Text = "选填 >>";

            //选填界面隐藏
            //this.Height -= groupBox5.Height;
            //btnFolding.Location = new Point(btnFolding.Location.X, btnFolding.Location.Y - groupBox5.Height);
            //btnOK.Location = new Point(btnOK.Location.X, btnOK.Location.Y - groupBox5.Height);
            //btnCancel.Location = new Point(btnCancel.Location.X, btnCancel.Location.Y - groupBox5.Height);
            //groupBox5.Visible = false;
            //出入院界面选择
            //nStatus = model;
            dataGv.Visible = false;
            if (nStatus == 0)//入院
            {
                groupBox4.Visible = false;
                if (isOut)
                {
                    groupBox3.Visible = false;
                }
                else
                {
                    groupBox3.Visible = true;
                }
                this.Height -= groupBox4.Height;
                btnFolding.Location = new Point(btnFolding.Location.X, btnFolding.Location.Y - groupBox4.Height + 5);
                btnOK.Location = new Point(btnOK.Location.X, btnOK.Location.Y - groupBox4.Height + 5);
                btnCancel.Location = new Point(btnCancel.Location.X, btnCancel.Location.Y - groupBox4.Height + 5);

            }
            if (nStatus == 1)//出院  出入院区域都显示
            {
                btnFolding.Visible = false;
                groupBox4.Visible = true;

                ////////////////////////////////////////////////////////////////////////// 
                tbRegNo.Text = inReimPara.RegInfo.NetRegSerial;
                //////////////////////////////////////////////////////////////////////////
                //问题: 出院登记的时候，inReimPara.InDiagnoseCode 里面的数据为空。
                string strSql = "SELECT * FROM  REPORT.dbo.DIAGNOSIS WHERE DIAGNOSIS_CODE LIKE '%" + inReimPara.PatInfo.OutDiagnoseCode + "%'";

                DataSet dsLite = sqlHelper.ExecSqlReDs(strSql);

                if (dsLite.Tables[0].Rows.Count > 0)
                {
                    tbDianoCode.Text = dsLite.Tables[0].Rows[0]["DIAGNOSIS_CODE"].ToString().Trim();
                    tbIcdAllNo.Text = dsLite.Tables[0].Rows[0]["DIAGNOSIS_NAME"].ToString().Trim();
                    dataGv.Visible = false;
                }

                string strSql1 = "SELECT * FROM  REPORT.dbo.DIAGNOSIS  WHERE  DIAGNOSIS_CODE  LIKE '%" + inReimPara.PatInfo.InDiagnoseCode + "%'";
                DataSet dsLiteout = sqlHelper.ExecSqlReDs(strSql1);

                if (dsLiteout.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        tbIcdAllNo.Text = dsLiteout.Tables[0].Rows[0]["DIAGNOSIS_NAME"].ToString().Trim();
                        tbDianoCode.Text = dsLiteout.Tables[0].Rows[0]["DIAGNOSIS_CODE"].ToString().Trim();
                        tbOutDiagnoCode.Text = dsLiteout.Tables[0].Rows[0]["DIAGNOSIS_CODE"].ToString().Trim();
                        tbOutHosDIagno.Text = dsLiteout.Tables[0].Rows[0]["DIAGNOSIS_NAME"].ToString().Trim();
                        dataGv.Visible = false;
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
            }

            if (isOut)
            {
                groupBox4.Visible = true;
            }

            //载入 就诊类型 入院状态 出院状态 补偿类型
            ArrayList arr = new ArrayList();

            arr.Add(new DictionaryEntry("2", "住院"));
            //arr.Add(new DictionaryEntry("3", "体格检查"));
            // arr.Add(new DictionaryEntry("4", "正常分娩住院"));
            arr.Add(new DictionaryEntry("9", "其他"));
            arr.Add(new DictionaryEntry("1", "门诊"));
            arr.Add(new DictionaryEntry("39", "特殊病种住院补偿"));
            arr.Add(new DictionaryEntry("6", "住院生育补偿"));
            arr.Add(new DictionaryEntry("7", "不能确定他方责任"));
            arr.Add(new DictionaryEntry("B", "生育并发症"));
            arr.Add(new DictionaryEntry("E", "特殊病种大额门诊"));
            arr.Add(new DictionaryEntry("F", "单病种结算"));
            arr.Add(new DictionaryEntry("G", "恶性肿瘤住院"));
            arr.Add(new DictionaryEntry("H", "血液透析补偿"));
            arr.Add(new DictionaryEntry("I", "县定临床路径"));
            FillCBox(cbCureId, arr, 0);

            arr = new ArrayList();
            arr.Add(new DictionaryEntry("1", "危"));
            arr.Add(new DictionaryEntry("2", "急"));
            arr.Add(new DictionaryEntry("3", "一般"));
            arr.Add(new DictionaryEntry("9", "其他"));
            FillCBox(cbInHosId, arr, 2);

            arr = new ArrayList();
            arr.Add(new DictionaryEntry("1", "治愈"));
            arr.Add(new DictionaryEntry("2", "好转"));
            arr.Add(new DictionaryEntry("3", "未愈"));
            arr.Add(new DictionaryEntry("4", "死亡"));
            arr.Add(new DictionaryEntry("9", "其他"));
            FillCBox(cbOutHosId, arr, 0); 

            arr = new ArrayList();
            arr.Add(new DictionaryEntry("00", "无转诊"));
            arr.Add(new DictionaryEntry("01", "转入"));
            arr.Add(new DictionaryEntry("02", "转出")); 
            FillCBox(cmbZZLX, arr, 0); 

            arr = new ArrayList();

            string networkPatClassId = "";
            string deptCode = "";
            if (outReimPara != null)
            {
                networkPatClassId = outReimPara.CommPara.NetworkPatClassId;
            }
            if (inReimPara != null)
            {
                networkPatClassId = inReimPara.CommPara.NetworkPatClassId; ;
                deptCode = inReimPara.PatInfo.InDeptCode;
            }
            if (inReimPara != null)
            {
                string strSql1ml = "SELECT   PAT_IDCARD FROM    ZY.[IN].PAT_ALL_INFO_VIEW WHERE  PAT_IN_HOS_ID=" + inReimPara.PatInfo.PatInHosId + "";
                DataSet dsLiteoutml = sqlHelper.ExecSqlReDs(strSql1ml);
                if (dsLiteoutml.Tables[0].Rows.Count > 0)
                {
                    tbMedicalNo.Text = dsLiteoutml.Tables[0].Rows[0]["PAT_IDCARD"].ToString().Trim();
                }

            }
            string strQuer = "SELECT DISTINCT  AreaCode FROM REPORT.dbo.NH_Area WHERE  netpatclassid = '" + networkPatClassId + "'";

            string area = sqlHelper.ExecSqlReDs(strQuer).Tables[0].Rows[0]["AreaCode"].ToString();


            string strbclx = "SELECT sCode,sName FROM REPORT.dbo.NH_CaType WHERE sAreaCode = '" + area + "' ;";
            DataSet bclx = sqlHelper.ExecSqlReDs(strbclx);
            arr = new ArrayList();
            if (bclx.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < bclx.Tables[0].Rows.Count; i++)
                {
                    arr.Add(new DictionaryEntry(bclx.Tables[0].Rows[i]["sCode"], bclx.Tables[0].Rows[i]["sName"]));
                }
            }
            else
            {
                arr.Add(new DictionaryEntry("-1", ""));
            }
            FillCBox(cbredeemNo, arr, 1);

            //农合 地区编码
            arr = new ArrayList();
            arr.Add(new DictionaryEntry("341203", "颍东区"));
            arr.Add(new DictionaryEntry("341202", "颍州区"));
            arr.Add(new DictionaryEntry("341222", "太和县"));
            arr.Add(new DictionaryEntry("341200", "阜阳市"));
            arr.Add(new DictionaryEntry("341204", "颍泉区"));
            arr.Add(new DictionaryEntry("341221", "临泉县"));
            arr.Add(new DictionaryEntry("341225", "阜南县"));
            arr.Add(new DictionaryEntry("341226", "颍上县"));
            arr.Add(new DictionaryEntry("341282", "界首市"));
            FillCBox(cbsAreaCode, arr, 0);


            string strSql6 = "SELECT DISTINCT ZLFSBM,ZLFSMC FROM  REPORT.dbo.NHDBZ WHERE ICDBM LIKE  '%%'  AND SAREA_CODE='" + cbsAreaCode.SelectedValue.ToString() + "';";


            // strSql6 = "SELECT DISTINCT ZLFSBM,ZLFSMC FROM  REPORT.dbo.NHDBZ WHERE ICDBM LIKE  '%%'  AND SAREA_CODE='" + cbsAreaCode.SelectedValue.ToString() + "';";


            //string strSql6 = strSqlxxxx;
            DataSet dsListCurMethod = sqlHelper.ExecSqlReDs(strSql6);
            arr = new ArrayList();

            if (dsListCurMethod.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < dsListCurMethod.Tables[0].Rows.Count; i++)
                {
                    arr.Add(new DictionaryEntry(dsListCurMethod.Tables[0].Rows[i]["ZLFSBM"], dsListCurMethod.Tables[0].Rows[i]["ZLFSMC"]));
                }
            }
            else
            {
                arr.Add(new DictionaryEntry("-1", ""));
            }
            //  FillCBox(selCurMethod, arr, 4);

            ///获取中心科室编码
            string strSql7 = "SELECT NETWORK_DEPT_ID,HIS_DEPT_NAME  FROM comm.comm.NETWORKING_DEPT_VS_HIS  where  NETWORKING_PAT_CLASS_ID = '" + networkPatClassId + "' and HIS_DEPT_ID = '" + deptCode + "';";
            DataSet dsListDept = sqlHelper.ExecSqlReDs(strSql7);
            arr = new ArrayList();
            if (dsListDept.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < dsListDept.Tables[0].Rows.Count; i++)
                {
                    arr.Add(new DictionaryEntry(dsListDept.Tables[0].Rows[i]["NETWORK_DEPT_ID"], dsListDept.Tables[0].Rows[i]["HIS_DEPT_NAME"]));
                }
            }
            else
            {
                arr.Add(new DictionaryEntry("-1", ""));
            }
            FillCBox(cbInOfficeId, arr, 0);

            if (inReimPara != null)
            {
                //获取身份信息，前提是农合审核那里审核过，然后取审核后信息
                string strSql8 = "SELECT DISTINCT sIDCardNo FROM REPORT.dbo.NH_INFO WHERE sPeopName = '" + inReimPara.PatInfo.InPatName + "' AND patinhosid = '" + inReimPara.PatInfo.PatInHosId + "';";//createtime > '" + DateTime.Now.AddDays(-10) + "';";
                try
                {
                    tbMedicalNo.Text = sqlHelper.ExecSqlReDs(strSql8).Tables[0].Rows[0]["sIDCardNo"].ToString();
                }
                catch (System.Exception ex)
                {
                }
            }

            //设置默认值
            switch (networkPatClassId)
            {
                case "3":
                    cbsAreaCode.SelectedValue = "341282";
                    cbsAreaCode.SelectedText = "界首市";
                    cbredeemNo.SelectedValue = "1";
                    if (isOut)
                    {
                        cbCureId.SelectedValue = "1";
                        cbCureId.SelectedText = "门诊";
                        cbredeemNo.SelectedValue = "127";
                    }
                    break;
                case "8":
                    cbsAreaCode.SelectedValue = "341202";
                    cbsAreaCode.SelectedText = "颍州区";
                    cbredeemNo.SelectedValue = "1";
                    //cbredeemNo.SelectedText = "住院报补";
                    cbredeemNo.SelectedItem = "住院报补";
                    break;
                case "9":
                    cbsAreaCode.SelectedValue = "341203";
                    cbsAreaCode.SelectedText = "颍东区";
                    cbredeemNo.SelectedValue = "1";
                    //cbredeemNo.SelectedText = "住院报补";
                    cbredeemNo.SelectedItem = "住院报补";
                    break;
                case "4":
                    cbsAreaCode.SelectedValue = "341204";
                    cbsAreaCode.SelectedText = "颍泉区";
                    cbredeemNo.SelectedValue = "1";
                    //cbredeemNo.SelectedText = "住院报补";
                    cbredeemNo.SelectedItem = "住院报补";
                    break;
                case "5":
                    cbsAreaCode.SelectedValue = "341221";
                    cbsAreaCode.SelectedText = "临泉县";
                    cbredeemNo.SelectedValue = "1";
                    // cbredeemNo.SelectedText = "住院报补";
                    cbredeemNo.SelectedItem = "住院报补";
                    break;
                case "6":
                    cbsAreaCode.SelectedValue = "341225";
                    cbsAreaCode.SelectedText = "阜南县";
                    cbredeemNo.SelectedValue = "1";
                    // cbredeemNo.SelectedText = "住院报补";
                    cbredeemNo.SelectedItem = "住院报补";
                    break;
                case "7":
                    cbsAreaCode.SelectedValue = "341226";
                    cbsAreaCode.SelectedText = "颍上县";
                    // cbredeemNo.SelectedValue = "21";
                    // cbredeemNo.SelectedText = "普通住院";
                    //cbredeemNo.SelectedItem = "普通住院";
                    break;
                case "2":
                    cbsAreaCode.SelectedValue = "341222";
                    cbsAreaCode.SelectedText = "太和县";
                    cbredeemNo.SelectedValue = "1";
                    if (isOut)
                    {
                        cbCureId.SelectedValue = "1";
                        cbCureId.SelectedText = "门诊";
                        cbredeemNo.SelectedValue = "127";
                    }
                    break;
            }
        }

        /// <summary>
        /// 添加键值对到下拉控件,并设置默认项
        /// </summary>
        /// <param name="cbBox">控件</param>
        /// <param name="arr">键值对列表</param>
        /// <param name="nDefault">默认选项序列号</param>
        private void FillCBox(ComboBox cbBox, ArrayList arr, int nDefault)
        {
            /*      键值对列表例子
             * ArrayList arr = new ArrayList();
             * arr.Add(new DictionaryEntry("键", "值"));
             */
            cbBox.DataSource = arr;
            cbBox.DisplayMember = "Value";
            cbBox.ValueMember = "Key";
            cbBox.SelectedIndex = nDefault;
        }

        /// <summary>
        /// OK 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (tbName.Text.Trim() != "" && inReimPara != null && inReimPara.PatInfo.InPatName !=null && inReimPara.PatInfo.InPatName.Substring(inReimPara.PatInfo.InPatName.Length - 2, 2) != "之子" && inReimPara.PatInfo.InPatName.Substring(inReimPara.PatInfo.InPatName.Length - 2, 2) != "之女" && inReimPara.PatInfo.InPatName.Substring(inReimPara.PatInfo.InPatName.Length - 2, 2) != "长女" && inReimPara.PatInfo.InPatName.Substring(inReimPara.PatInfo.InPatName.Length - 2, 2) != "次女" && inReimPara.PatInfo.InPatName.Substring(inReimPara.PatInfo.InPatName.Length - 2, 2) != "长子" && inReimPara.PatInfo.InPatName.Substring(inReimPara.PatInfo.InPatName.Length - 2, 2) != "次子")
            {
                if (inReimPara.PatInfo.InPatName != tbName.Text.Trim())
                {
                    throw new Exception("选取家庭成员与住院病人姓名不一致");
                }
            }

            //门诊姓名判断
            if (tbName.Text.Trim() != "" && outReimPara != null && outReimPara.PatInfo.PatName.Substring(outReimPara.PatInfo.PatName.Length - 2, 2) != "之子" && outReimPara.PatInfo.PatName.Substring(outReimPara.PatInfo.PatName.Length - 2, 2) != "之女" && outReimPara.PatInfo.PatName.Substring(outReimPara.PatInfo.PatName.Length - 2, 2) != "长女" && outReimPara.PatInfo.PatName.Substring(outReimPara.PatInfo.PatName.Length - 2, 2) != "次女" && outReimPara.PatInfo.PatName.Substring(outReimPara.PatInfo.PatName.Length - 2, 2) != "长子" && outReimPara.PatInfo.PatName.Substring(outReimPara.PatInfo.PatName.Length - 2, 2) != "次子")
            {
                if (outReimPara.PatInfo.PatName != tbName.Text.Trim())
                {
                    throw new Exception("选取家庭成员与住院病人姓名不一致");
                }
            }

            netPatInfo.patName = tbName.Text.Trim();                                             //姓名 
            netPatInfo.strFamilySysno = tbFamilySysno.Text.Trim();                               //家庭编号
            netPatInfo.ICNo = tbMemberNO.Text.Trim();                                            //个人编号
            netPatInfo.medicalNo = tbMedicalNo.Text.Trim();                                      //医疗证号
            netPatInfo.IDNo = tbIdCardNo.Text.Trim();                                            //身份证号
            if (tbBirthDay.Text.Trim() != "")
            {
                netPatInfo.birthday = Convert.ToDateTime(tbBirthDay.Text.Trim());
            }
            netPatInfo.strMemberNo = tbMemberNO.Text.Trim();                                     //医疗卡号 
            netPatInfo.age = Convert.ToSingle(tbAge.Text.Trim() == "" ? "0" : tbAge.Text.Trim());//年龄 
            netPatInfo.patAddress = tbFamilyAddress.Text.Trim();                                 //家庭住址 
            netPatInfo.sex = tbSexId.Text.Trim();                                                //性别 
            DictionaryEntry dicEntry = (DictionaryEntry)cbCureId.SelectedItem;
            netPatInfo.strCureId = dicEntry.Key.ToString().Trim();                               //就诊类型
            dicEntry = (DictionaryEntry)cbInHosId.SelectedItem;                                  //来院状态
            netPatInfo.strInHosId = dicEntry.Key.ToString();                                     //来院状态 
            netPatInfo.strInDiagnoCode = tbDianoCode.Text.Trim();                                //诊断编码  
            netPatInfo.strInDiagnoName = tbIcdAllNo.Text.Trim(); //入院诊断  




            netPatInfo.strInOfficeId = cbInOfficeId.SelectedValue == null ? "" : cbInOfficeId.SelectedValue.ToString();
            netPatInfo.strInOfficeName = cbInOfficeId.SelectedText == null ? "" : cbInOfficeId.SelectedText.ToString();
            DictionaryEntry dicOutHos = (DictionaryEntry)cbredeemNo.SelectedItem;
            netPatInfo.strRedeemNo = dicOutHos.Key.ToString().Trim();  //补偿类型}  

            dicOutHos = (DictionaryEntry)cbOutHosId.SelectedItem;

            netPatInfo.strOutHosId = dicOutHos.Key.ToString().Trim();

            //netPatInfo.strOutOfficeId = tbSectionNo.Text.Trim();
            netPatInfo.strOutDiagnoCode = tbOutDiagnoCode.Text.Trim();
            // dicOutHos=(DictionaryEntry)cbOutOfficeId.SelectedItem;
            netPatInfo.strOutOfficeId = tbOutDiagnoCode.Text.Trim();
            netPatInfo.strOutOfficeName = tbOutHosDIagno.Text.Trim();

            netPatInfo.Canton = cbsAreaCode.SelectedValue.ToString();

            OutDiagnoseName = tbOutHosDIagno.Text.Trim();
            netPatInfo.strProcreateNotice = selCurMethod.SelectedValue == null ? "" : selCurMethod.SelectedValue.ToString();

            if (!isOut)
            {
                // netPatInfo.strCureId=dicOutHos.Value.ToString().Trim();
                // inReimPara.PersonClass = dicOutHos.Value.ToString();
                inReimPara.RegInfo.NetPatType = "1"; 
            }
            
            ZZDH = txtZZDH.Text.Trim();
            ZZLX = cmbZZLX.SelectedValue.ToString().Trim();

            bIsValid = true;
            this.Close();
        }

        /// <summary>
        /// 取消按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            netPatInfo = null;
            //netPatInfo.strRedeemNo = netPatInfo.strCureId = "";
            bIsValid = false;
            this.Close();
        }

        /// <summary>
        /// 医疗编号回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbMedicalNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.tbMedicalNo.Text.Trim().Length == 0)
                {
                    readCard(sender, e);
                }

                sAreaCode = this.cbsAreaCode.SelectedValue.ToString();
                sMedicalCode = this.tbMedicalNo.Text.Trim();

                try
                {
                    //获取家庭成员信息
                    res = serr.GetPersonInfo(sAreaCode, sHospitalCode, sMedicalCode, "", "", "", "", "", "", ref sResult, ref sMessage);

                    if (res == 0)
                    {
                        throw new Exception("错误信息:" + sMessage);
                    }

                    XmlDocument ReResulst = new XmlDocument();
                    string result = sResult;                                                           //进行医保业务
                    ReResulst.LoadXml(result);

                    int rowCount = int.Parse(ReResulst.SelectSingleNode("/JQWebService/RowCount").InnerText);


                    StringBuilder strinfo = new StringBuilder();
                    string smedicalcode = "";
                    for (int i = 0; i < rowCount; i++)
                    {
                        smedicalcode = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sMedicalCode").InnerText;
                        strinfo.Append("DELETE FROM REPORT.dbo.NH_MESSAGE WHERE  sMedicalCode = '" + smedicalcode + "'; ");
                    }
                    for (int i = 0; i < rowCount; i++)
                    {
                        object[] row;
                        //个人编号,姓名,性别
                        row = new object[] {ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sMedicalCode").InnerText, ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sPeopCode").InnerText, ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sPeopName").InnerText, 
                         //家庭编码,出生日期, 身份证号, 地址
                      ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sSex").InnerText,ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sAge").InnerText,ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sBirthDay").InnerText,ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sIDCardNo").InnerText,
                      ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sAddress").InnerText,ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sCardCode").InnerText,ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sTelephone").InnerText,ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sPeopProp").InnerText,
                      ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sAddrCode").InnerText,ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sFamilyBalance").InnerText,ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sChronicName1").InnerText,ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sChronicName2").InnerText,
                      ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sChronicName3").InnerText};

                        this.dgvFamilyInfo.Rows.Add(row);

                        smedicalcode = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sMedicalCode").InnerText;
                        string sPeopCode = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sPeopCode").InnerText;
                        string sPeopName = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sPeopName").InnerText;
                        string sSex = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sSex").InnerText;
                        string sAge = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sAge").InnerText;
                        string sBirthDay = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sBirthDay").InnerText;
                        string sIDCardNo = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sIDCardNo").InnerText;
                        string sAddress = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sAddress").InnerText;
                        string sCardCode = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sCardCode").InnerText; ;
                        string sTelephone = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sTelephone").InnerText;
                        string sPeopProp = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sPeopProp").InnerText;
                        string sAddrCode = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sAddrCode").InnerText;
                        string sFamilyBalance = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sFamilyBalance").InnerText;
                        string sChronicName1 = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sChronicName1").InnerText;
                        string sChronicName2 = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sChronicName2").InnerText;
                        string sChronicName3 = ReResulst.SelectSingleNode("/JQWebService/Row" + (i + 1).ToString() + "/sChronicName3").InnerText;

                        string patInHosId = "-1";
                        string outPatId = "-1";

                        if (outReimPara != null)
                        {
                            outPatId = outReimPara.PatInfo.OutPatId.ToString();
                            patInHosId = outPatId;
                        }

                        if (inReimPara != null)
                        {
                            patInHosId = inReimPara.PatInfo.PatInHosId.ToString();
                        }

                        strinfo.Append("INSERT INTO REPORT.dbo.NH_MESSAGE ( sIDCardNo ,sMedicalCode,patinhosid,sPeopName,sAddress) ");

                        strinfo.Append(" VALUES ('" + sIDCardNo + "','" + smedicalcode + "','" + patInHosId + "','" + sPeopName + "','" + sAddress + "') ");
                    }
                    try
                    {
                        sqlHelper.ExecSqlReInt(strinfo.ToString());
                    }
                    catch (System.Exception ex)
                    {
                    }
                    this.dgvFamilyInfo.Focus();
                }
                catch (System.Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        private TextBox tbShowDiagnos;
        private TextBox tbShowDiagnosCode;
        /// <summary>
        /// 显示病种选择
        /// </summary>
        /// <param name="tbDignosis">录入病种的textbox</param>
        /// <param name="tbDiagnosCode">显示编码的textbox</param>
        /// <param name="lblPoint">显示出现位置的定位坐标</param>
        /// <param name="gbShow">显示区域</param>
        private void ShowDignosisSelect(TextBox tbDignosis, TextBox tbDiagnosCode, Label lblPoint, GroupBox gbShow, int insurType)
        {
            //if (bLoadingDia)
            //    return;
            if (tbDignosis.Text.Trim() == "")
            {
                dataGv.Visible = false;
                tbDiagnosCode.Text = "";
                return;
            }
            #region 本地查询诊断
            //  NetworkQueryDALSQLite query = new NetworkQueryDALSQLite();
            string strSql2 = "";
            //if (cbCureId.Text == "单病种结算" || cbCureId.Text == "门诊")
            if (cbCureId.Text == "单病种结算")
            {
                strSql2 = "SELECT  DISTINCT  ICDBM,ICDMC FROM REPORT.dbo.NHDBZ  WHERE INPUT_CODE LIKE '%" + tbDignosis.Text.Trim() + "%'AND SAREA_CODE='" + cbsAreaCode.SelectedValue.ToString() + "';";
            }
            else
            {
                strSql2 = "SELECT  DISTINCT DIAGNOSIS_CODE,DIAGNOSIS_NAME FROM  REPORT.dbo.DIAGNOSIS WHERE INPUT_CODE LIKE '" + tbDignosis.Text.Trim() + "%'  OR DIAGNOSIS_NAME LIKE '%" + tbDignosis.Text.Trim() + "%';";
            }

            DataSet ds = sqlHelper.ExecSqlReDs(strSql2);

            //DataSet ds = query.QueryDignosis(tbDignosis.Text.Trim(), 2);
            if (ds.Tables[0].Rows.Count > 0)
                dataGv.Visible = true; // ml 注释
            ds.Tables[0].TableName = "Disgnos";
            dataGv.DataSource = ds;
            dataGv.DataMember = "Disgnos";
            #endregion
            tbShowDiagnos = tbDignosis;
            tbShowDiagnosCode = tbDiagnosCode;
            dataGv.Left = lblPoint.Left + gbShow.Left;
            // dataGv.Top = tbDignosis.Top + gbShow.Top + tbDignosis.Height;
            dataGv.Top = tbDignosis.Top + gbShow.Top - dataGv.Height;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbOutDiagnoCode_TextChanged(object sender, EventArgs e)
        {
            string strSql = "";
            if (cbCureId.Text == "单病种结算" || cbCureId.Text == "门诊")
            {
                strSql = "SELECT ZLFSBM,ZLFSMC FROM  REPORT.dbo.NHDBZ WHERE ICDBM='" + tbOutDiagnoCode.Text.Trim() + "'";
            }

            else if (cbredeemNo.Text == "慢特病门诊")
            {

                strSql = "SELECT DISTINCT DIAGNOSIS_ID AS ZLFSBM , DIAGNOSIS_NAME AS ZLFSMC   FROM REPORT..DIAGNOSIS  where  DIAGNOSIS_NAME='" + tbOutDiagnoCode.Text.Trim() + "'";

            }
            else
            {
                strSql = "SELECT distinct ZLFSBM,ZLFSMC FROM  REPORT.dbo.NHDBZ  WHERE SAREA_CODE  = '" + cbsAreaCode.SelectedValue.ToString() + "';";
            }

            DataSet dsListCurMethod = sqlHelper.ExecSqlReDs(strSql);
            ArrayList arr = new ArrayList();

            if (dsListCurMethod.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < dsListCurMethod.Tables[0].Rows.Count; i++)
                {
                    arr.Add(new DictionaryEntry(dsListCurMethod.Tables[0].Rows[i]["ZLFSBM"], dsListCurMethod.Tables[0].Rows[i]["ZLFSMC"]));
                }
            }
            else
            {
                arr.Add(new DictionaryEntry("-1", ""));
            }
            FillCBox(selCurMethod, arr, 0);
        }



        private void tbIcdAllNo_TextChanged(object sender, EventArgs e)
        {
            ShowDignosisSelect(tbIcdAllNo, tbDianoCode, label14, groupBox3, 2);
        }

        private void tbOutHosDIagno_TextChanged(object sender, EventArgs e)
        {
            ShowDignosisSelect(tbOutHosDIagno, tbOutDiagnoCode, label36, groupBox4, 2);
        }

        private void tbSecondIcdNo_TextChanged(object sender, EventArgs e)
        {
            //ShowDignosisSelect(tbSecondIcdNo, textBox27, label18, groupBox5, 2);
        }

        private void dataGv_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void dgvFamilyInfo_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvFamilyInfo.Rows.Count == 0 || dgvFamilyInfo.SelectedRows.Count == 0)
            {
                return;
            }
            //this.tbIdCardNo.Text = dgvFamilyInfo.SelectedRows[0].Cells[0].Value.ToString();      //个人编号
            tbMemberNO.Text = dgvFamilyInfo.SelectedRows[0].Cells[1].Value.ToString();      //个人编号
            tbName.Text = dgvFamilyInfo.SelectedRows[0].Cells[2].Value.ToString();      //姓名
            // tbCountryTeamCode.Text = dgvFamilyInfo.SelectedRows[0].Cells[2].Value.ToString();   //性别
            tbFamilySysno.Text = dgvFamilyInfo.SelectedRows[0].Cells[0].Value.ToString();//家庭编号
            tbSexId.Text = dgvFamilyInfo.SelectedRows[0].Cells[3].Value.ToString();      //性别
            tbIdCardNo.Text = dgvFamilyInfo.SelectedRows[0].Cells[6].Value.ToString();  //身份证号
            tbAge.Text = dgvFamilyInfo.SelectedRows[0].Cells[4].Value.ToString();           //年龄
            tbBirthDay.Text = dgvFamilyInfo.SelectedRows[0].Cells[5].Value.ToString();   //出生日期
            //tbFamilySysno.Text = dgvFamilyInfo.SelectedRows[0].Cells[9].Value.ToString();   //医疗证号
            tbFamilyAddress.Text = dgvFamilyInfo.SelectedRows[0].Cells[7].Value.ToString();   //地址
            // tbTel.Text = model.handleModel.dsRe.Tables[0].Rows[0]["tel"].ToString().Trim();
            //tbIdeName.Text = model.handleModel.dsRe.Tables[0].Rows[0]["ideName"].ToString().Trim(); 
            strIdeName = dgvFamilyInfo.SelectedRows[0].Cells[6].Value.ToString();
            netPatInfo.Canton = dgvFamilyInfo.SelectedRows[0].Cells[11].Value.ToString();
            netPatInfo.medicalType = dgvFamilyInfo.SelectedRows[0].Cells[10].Value.ToString();
            MedicalCode = dgvFamilyInfo.SelectedRows[0].Cells[8].Value.ToString();
            CardCode = dgvFamilyInfo.SelectedRows[0].Cells[0].Value.ToString();
            txt_CardCode.Text = dgvFamilyInfo.SelectedRows[0].Cells[8].Value.ToString();
            telno.Text = dgvFamilyInfo.SelectedRows[0].Cells[9].Value.ToString();// 电话
            netPatInfo.strTelNo = telno.Text;
        }

        private void tbtreatno_TextChanged(object sender, EventArgs e)
        {
            ShowDignosisSelect(tbtreatno, tbtreatname, label28, groupBox3, 102);
        }

        private void tbIcdAllNo_KeyDown(object sender, KeyEventArgs e)
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
                    tbShowDiagnosCode.Text = dataGv.SelectedRows[0].Cells[0].Value.ToString();
                    tbShowDiagnos.Text = dataGv.SelectedRows[0].Cells[1].Value.ToString();
                }
                dataGv.Visible = false;
            }
        }

        private void tbOutHosDIagno_KeyDown(object sender, KeyEventArgs e)
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
                    tbShowDiagnosCode.Text = dataGv.SelectedRows[0].Cells[0].Value.ToString();
                    tbShowDiagnos.Text = dataGv.SelectedRows[0].Cells[1].Value.ToString();
                }
                dataGv.Visible = false;
            }
        }

        private void dataGv_DoubleClick(object sender, EventArgs e)
        {
            if (dataGv.SelectedRows.Count != 1)
                return;
            tbShowDiagnosCode.Text = dataGv.SelectedRows[0].Cells[0].Value.ToString();
            tbShowDiagnos.Text = dataGv.SelectedRows[0].Cells[1].Value.ToString();

            dataGv.Visible = false;
            btnOK.Focus();
        }



        public void readCard(object sender, EventArgs e)
        {
            StringBuilder s_cardno = new StringBuilder(1024);
            StringBuilder s_error = new StringBuilder(1024);
            int re = Get_CardNo("340222", s_cardno, s_error);
            if (re == 0)
            {
                tbMedicalNo.Text = s_cardno.ToString();
            }
            else
            {
                MessageBox.Show("读卡失败 :" + s_error.ToString());
            }

        }

        private void dgvFamilyInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void tbOutDiagnoCode_TextChanged()
        {
        }

        private void tbOutOfficeId_TextChanged(object sender, EventArgs e)
        {
        }

        private void tbMedicalNo_TextChanged(object sender, EventArgs e)
        {
        }


        private void btnyqjcInput_Click(object sender, EventArgs e)
        {
            MzInvoiceInput xx = new MzInvoiceInput(inReimPara, netPatInfo, sAreaCode);
            xx.Show();
        }

        //删除上传费用 
        private void button1_Click(object sender, EventArgs e)
        {
            FYNHCityInterfaceModel mm = new FYNHCityInterfaceModel();
            mm.CancelItems(this.cbsAreaCode.SelectedValue.ToString(), inReimPara.RegInfo.NetRegSerial);
            MessageBox.Show("删除上传费用成功！");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FYNHCityInterfaceModel mm = new FYNHCityInterfaceModel();
            try
            {
                mm.CancelReimSettle(this.cbsAreaCode.SelectedValue.ToString(), inReimPara.SettleInfo.SettleNo, inReimPara.RegInfo.NetRegSerial);
            }
            catch (Exception)
            {

            }
            mm.CancelOutReigster(this.cbsAreaCode.SelectedValue.ToString(), inReimPara.RegInfo.NetRegSerial);
            MessageBox.Show("取消出院登记成功!");

        }

        #region 读卡事件
        private void hqsfzxx(object sender, EventArgs e)
        {
            IDCardData CardMsg = new IDCardData();
            int nRet, nPort;
            string stmp;
            byte[] cPath = new byte[255];
            byte[] pucIIN = new byte[4];
            byte[] pucSN = new byte[8];
            nPort = m_iPort;
            //if (pictureBox1.Image != null)
            //{
            //    pictureBox1.Image.Dispose();
            //    pictureBox1.Image = null;
            //}
            Syn_SetPhotoPath(0, ref cPath[0]);	//设置照片路径	iOption 路径选项	0=C:	1=当前路径	2=指定路径
            //cPhotoPath	绝对路径,仅在iOption=2时有效
            //iPhotoType = 0;
            Syn_SetPhotoType(0); //0 = bmp ,1 = jpg , 2 = base64 , 3 = WLT ,4 = 不生成
            Syn_SetPhotoName(2); // 生成照片文件名 0=tmp 1=姓名 2=身份证号 3=姓名_身份证号 

            Syn_SetSexType(1);	// 0=卡中存储的数据	1=解释之后的数据,男、女、未知
            Syn_SetNationType(1);// 0=卡中存储的数据	1=解释之后的数据 2=解释之后加"族"
            Syn_SetBornType(1);			// 0=YYYYMMDD,1=YYYY年MM月DD日,2=YYYY.MM.DD,3=YYYY-MM-DD,4=YYYY/MM/DD
            Syn_SetUserLifeBType(1);	// 0=YYYYMMDD,1=YYYY年MM月DD日,2=YYYY.MM.DD,3=YYYY-MM-DD,4=YYYY/MM/DD
            Syn_SetUserLifeEType(1, 1);	// 0=YYYYMMDD(不转换),1=YYYY年MM月DD日,2=YYYY.MM.DD,3=YYYY-MM-DD,4=YYYY/MM/DD,
            // 0=长期 不转换,	1=长期转换为 有效期开始+50年           
            if (Syn_OpenPort(nPort) == 0)
            {
                if (Syn_SetMaxRFByte(nPort, 80, 0) == 0)
                {
                    nRet = Syn_StartFindIDCard(nPort, ref pucIIN[0], 0);
                    nRet = Syn_SelectIDCard(nPort, ref pucSN[0], 0);
                    nRet = Syn_ReadMsg(nPort, 0, ref CardMsg);
                    if (nRet == 0)
                    {
                        //stmp = Convert.ToString(System.DateTime.Now) + "  姓名:" + CardMsg.Name;
                        //listBox1.Items.Add(stmp);
                        //stmp = Convert.ToString(System.DateTime.Now) + "  性别:" + CardMsg.Sex;
                        //listBox1.Items.Add(stmp);
                        //stmp = Convert.ToString(System.DateTime.Now) + "  民族:" + CardMsg.Nation;
                        //listBox1.Items.Add(stmp);
                        //stmp = Convert.ToString(System.DateTime.Now) + "  出生日期:" + CardMsg.Born;
                        //listBox1.Items.Add(stmp);
                        //stmp = Convert.ToString(System.DateTime.Now) + "  地址:" + CardMsg.Address;
                        //listBox1.Items.Add(stmp);
                        stmp = Convert.ToString(System.DateTime.Now) + "  身份证号:" + CardMsg.IDCardNo;
                        //listBox1.Items.Add(stmp);
                        //stmp = Convert.ToString(System.DateTime.Now) + "  发证机关:" + CardMsg.GrantDept;
                        //listBox1.Items.Add(stmp);
                        //stmp = Convert.ToString(System.DateTime.Now) + "  有效期开始:" + CardMsg.UserLifeBegin;
                        //listBox1.Items.Add(stmp);
                        //stmp = Convert.ToString(System.DateTime.Now) + "  有效期结束:" + CardMsg.UserLifeEnd;
                        //listBox1.Items.Add(stmp);
                        //stmp = Convert.ToString(System.DateTime.Now) + "  照片文件名:" + CardMsg.PhotoFileName;
                        //listBox1.Items.Add(stmp);
                        //if (iPhotoType == 0 || iPhotoType == 1)
                        //{
                        //    //pictureBox1.Image = Image.FromFile(CardMsg.PhotoFileName);
                        //}
                        tbMedicalNo.Text = CardMsg.IDCardNo;
                    }
                    else
                    {
                        stmp = Convert.ToString(System.DateTime.Now) + "  读取身份证信息错误";
                        //listBox1.Items.Add(stmp); 
                        MessageBox.Show(stmp);
                    }
                }
            }
            else
            {
                stmp = Convert.ToString(System.DateTime.Now) + "  打开端口失败";
                //  listBox1.Items.Add(stmp);
                MessageBox.Show(stmp);
            }
        }

        private void xzdkq(object sender, EventArgs e)
        {
            string stmp;
            int i, nRet;
            uint[] iBaud = new uint[1];
            i = Syn_FindReader();
            m_iPort = i;
            if (i > 0)
            {
                if (i > 1000)
                {
                    stmp = Convert.ToString(i);
                    stmp = Convert.ToString(System.DateTime.Now) + "  读卡器连接在USB " + stmp;
                    // MessageBox.Show(stmp);
                }
                else
                {
                    System.Threading.Thread.Sleep(200);
                    nRet = Syn_GetCOMBaud(i, ref iBaud[0]);
                    stmp = Convert.ToString(i);
                    stmp = Convert.ToString(System.DateTime.Now) + "  读卡器连接在COM " + stmp + ";当前波特率为 " + Convert.ToString(iBaud[0]);
                    //listBox1.Items.Add(stmp);
                    MessageBox.Show(stmp);
                }
            }
            else
            {
                stmp = Convert.ToString(System.DateTime.Now) + "  没有找到读卡器";
                //listBox1.Items.Add(stmp);
                MessageBox.Show(stmp);
            }
        }
        #endregion

        /// <summary>
        /// 治疗方式鼠标点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selCurMethod_MouseClick(object sender, MouseEventArgs e)
        {
            if (cbCureId.SelectedValue.ToString().Equals("F"))
            {
                string method = "SELECT ZLFSBM,ZLFSMC FROM REPORT.dbo.NHDBZ WHERE SAREA_CODE = '" + cbsAreaCode.SelectedValue.ToString() + "' AND  ICDBM like  '%" + tbOutDiagnoCode.Text.Trim() + "%' ;";
                DataSet ds1 = new DataSet();
                ds1 = sqlHelper.ExecSqlReDs(method);
                ArrayList arr = new ArrayList();
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    arr.Add(new DictionaryEntry(ds1.Tables[0].Rows[i]["ZLFSBM"], ds1.Tables[0].Rows[i]["ZLFSMC"]));
                }
                FillCBox(selCurMethod, arr, 0);
            }
            else
            {
            }
        }

        private void cbInOfficeId_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }


    }
}
