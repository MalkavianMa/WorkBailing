using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using PayAPIInterfaceHandle.Dareway.JiNan;

namespace PayAPIInstance.Dareway.JNLX.Dialog
{
    public partial class PersonInfoDialog : Form
    {
        JNLXInterfaceModel model;
        /// <summary>
        /// 疾病编码
        /// </summary>
        public string strDiagnosCode = "";

        /// <summary>
        /// 疾病名称
        /// </summary>
        public string strDiagnosName = "";
        
        /// <summary>
        /// 医疗统筹类别
        /// </summary>
        public string strYltclb = "";

        /// <summary>
        /// 险种标识
        /// </summary>
        public string strXzbz = "";

        /// <summary>
        /// 免费用药诊断
        /// </summary>
        public string strYllb = "";

        /// <summary>
        /// 是否免费用药
        /// </summary>
        public bool isFreeDrug = false;

        private Dictionary<string, string> dicPerInfo;

        private bool isHaveDiagnos = false;

        /// <summary>
        /// 是否低保
        /// </summary>
        public string reDiBao = "";//是否低保

        ArrayList arrList = new ArrayList();



        /// <summary>
        /// 是否取消
        /// </summary>
        public bool isCancel = false;

        public PersonInfoDialog()
        {
            InitializeComponent();
        }

        public PersonInfoDialog(Dictionary<string, string> patInfo, JNLXInterfaceModel _model)
        {
            dicPerInfo = patInfo;
            model = _model;
            InitializeComponent();
            ShowInfo();
            //-----------------
            //---------------------------------------
            try
            {
                //此处需修改决定是否传录入的诊断名和编码
                //DataSet ds = JNDWInterfaceModel_MG.handelModel.getMzJzxx(model.outReimPara.PatInfo.OutPatId.ToString(), model.outReimPara.CommPara.TradeId.ToString());

                lblZdbm.Visible = true;
                lblZdmc.Visible = true;
                tbZdbm.Visible = true;
                tbZdmc.Visible = true;
                //tbZdbm.Text = ds.Tables[0].Rows[0]["DIAGNOSIS_CODE"].ToString();
                //tbZdmc.Text = ds.Tables[0].Rows[0]["DIAGNOSIS_NAME"].ToString();
                //cbxDiagnos.SelectedValue = ds.Tables[0].Rows[0]["DIAGNOSIS_CODE"].ToString();
                //if (cbxDiagnos.Text == "")
                //{
                //    cbxDiagnos.Text = ds.Tables[0].Rows[0]["DIAGNOSIS_NAME"].ToString();
                //}
            }
            catch (Exception ex)
            {
            }
            //---------------------------------
        }

        public PersonInfoDialog(Dictionary<string,string> patInfo)
        {
            dicPerInfo = patInfo;
            InitializeComponent();
            ShowInfo();
        }

        private void ShowInfo() 
        {
            txtName.Text = dicPerInfo["xm"];
            txtSex.Text = dicPerInfo["xb"] == "1" ? "男" : "女";
            txtMemNo.Text = dicPerInfo["p_kh"];
            txtIDNO.Text = dicPerInfo["sfzhm"];
            txtInvalid.Text = dicPerInfo["zfbz"];
            txtInvalidReason.Text = dicPerInfo["zfsm"];
            txtPersonType.Text = dicPerInfo["ylrylb"];
            txtCompany.Text = dicPerInfo["dwmc"];
            txtBalance.Text = dicPerInfo["zhye"];
            txtIns.Text = dicPerInfo["rqlb"] == "A" ? "职工" : (dicPerInfo["rqlb"] == "B" ? "居民" : "");
            txtMzdbbz.Text = dicPerInfo["mzdbbz"];
            string strDiagnos = dicPerInfo["mzdbjbs"];
            string[] arrDiagnos = strDiagnos.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            
            
			
            for (int i = 0; i < arrDiagnos.Length; i++)
            {
                if(arrDiagnos[i].Trim().Length!=0){
                    string[] arrDia = arrDiagnos[i].Split(new char[] { '#', 'm' }, StringSplitOptions.RemoveEmptyEntries);
                    arrList.Add(new DictionaryEntry(arrDia[1], arrDia[0]));
                }
            }
            if (arrList.Count > 0)
            {
                isHaveDiagnos = true;
                cbxDiagnos.DataSource = arrList;
                cbxDiagnos.DisplayMember = "Value";
                cbxDiagnos.ValueMember = "Key";
                cbxDiagnos.SelectedIndex = 0;
            }
            //----------------------
            StringBuilder strSql = new StringBuilder();
            DataSet ds = new DataSet();
            string re = "";
            string errString;
            //txtIDNO.Text = "370102200109063741";
            strSql.Append(" SELECT GENDER,SICARDNO,PHONENUMBER,SITYPE,AIDTYPE,ADDRESS,DISABLEDCARDNO,DISABLETYPE,PHOTO,ISDELETED FROM POWERSDA.CARDHOLDERINFO where IDENTITYCARDNO='" + txtIDNO.Text + "'   ");
            WebReferenceDiBao.DiBaoWebService WebServiceObj = new WebReferenceDiBao.DiBaoWebService();
            re = WebServiceObj.Query(strSql.ToString(), out errString, out ds);
            if (re == "0")//查询成功
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //救助类型 1=低保 2=低保边缘 3=一级伤残 4=二级伤残 
                    string strMC = "";
                    string strS = ds.Tables[0].Rows[0]["AIDTYPE"].ToString();
                    try
                    {
                        string[] rr = strS.Split(new string[] { "||" }, System.StringSplitOptions.None);
                        for (int i = 0; i < rr.Length; i++)
                        {
                            if (rr[i] == "1")
                            {
                                if (strMC == "")
                                {
                                    strMC = "低保";
                                }
                                else
                                {
                                    strMC = "+低保";
                                }
                            }

                            if (rr[i] == "2")
                            {
                                if (strMC == "")
                                {
                                    strMC = "低保边缘";
                                }
                                else
                                {
                                    strMC = "+低保边缘";
                                }
                            }
                            if (rr[i] == "3")
                            {
                                if (strMC == "")
                                {
                                    strMC = "一级伤残";
                                }
                                else
                                {
                                    strMC = "+一级伤残";
                                }
                            }

                            if (rr[i] == "4")
                            {
                                if (strMC == "")
                                {
                                    strMC = "二级伤残";
                                }
                                else
                                {
                                    strMC = "+二级伤残";
                                }
                            }
                        }//for
                        lblDiBao.Text = "低保类型：" + strMC;
                        lblDiBao.Visible = true;

                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            //--------------------------
            if (lblDiBao.Text != "")
            {
                reDiBao = "低保";
            }
            
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {

            strYltclb = this.cmbyltclb.SelectedValue.ToString();
            strYllb = this.cmbyllb.Text;//this.cmbyllb.SelectedValue.ToString();
            strXzbz = this.cboxzbz.SelectedValue.ToString();
            strDiagnosCode = isHaveDiagnos ? cbxDiagnos.SelectedValue.ToString() : "";
            strDiagnosName = cbxDiagnos.Text;
            this.Close();
        }

       

        private void PersonInfoDialog_Shown(object sender, EventArgs e)
        {
            if (lblDiBao.Text == "" && arrList.Count==0)
            {
              //  MessageBox.Show("Test");
               // this.Close();//非低保，此界面自动关闭
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isCancel = true;
            this.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void PersonInfoDialog_Load(object sender, EventArgs e)
        {
            ArrayList arr = new ArrayList();

            //0为仅获取人员基本信息，1为住院，4为门诊大病(特病)，6为普通门诊，不传时，
            //默认值为0,其他具体值调用数据字典接口获取，代码编号：YLTCLB
            arr.Add(new DictionaryEntry("0", "查询人员基本信息"));
            arr.Add(new DictionaryEntry("6", "普通门诊"));
            arr.Add(new DictionaryEntry("4", "门诊大病(特病)"));
            arr.Add(new DictionaryEntry("1", "住院"));
            cmbyltclb.DataSource = arr;
            cmbyltclb.ValueMember = "Key";
            cmbyltclb.DisplayMember = "Value";
            cmbyltclb.SelectedIndex = 0;
            cbxFreeDrug.Checked = false;


            ArrayList arr1 = new ArrayList();

            //医疗 C，工伤 D，生育 E， 可调用数据字典接口获取，代码编号：XZBZ
            //险种标识
            arr1.Add(new DictionaryEntry("C", "医疗"));
            arr1.Add(new DictionaryEntry("D", "工伤"));
            arr1.Add(new DictionaryEntry("E", "生育"));
            cboxzbz.DataSource = arr1;
            cboxzbz.ValueMember = "Key";
            cboxzbz.DisplayMember = "Value";
            cboxzbz.SelectedIndex = 0;
        }

        private void cbxFreeDrug_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxFreeDrug.Checked==true)
            {
                isFreeDrug = true;
            }
            else
            {
                isFreeDrug = false;
            }
        }
    }
}
