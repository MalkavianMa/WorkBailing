using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using PayAPIInterfaceHandle.Dareway.JiNan;

namespace PayAPIInstance.Dareway.JiNan.Dialog
{
    public partial class PersonInfoDialog : Form
    {
       // JNDWInterfaceModel_MG model;
        //疾病编码
        public string strDiagnosCode = "";
        //疾病名称
        public string strDiagnosName = "";

        private Dictionary<string, string> dicPerInfo;

        private bool isHaveDiagnos = false;

        public string reDiBao = "";//是否低保

        ArrayList arrList = new ArrayList();

        //是否取消
        public bool isCancel = false;

        public PersonInfoDialog()
        {
            InitializeComponent();
        }

        #region 不用
        //public PersonInfoDialog(Dictionary<string, string> patInfo, JNDWInterfaceModel_MG _model)
        //{
        //    dicPerInfo = patInfo;
        //    //  model = _model;
        //    InitializeComponent();
        //    ShowInfo();
        //    //-----------------
        //    //---------------------------------------
        //    try
        //    {
        //        //此处需修改决定是否传录入的诊断名和编码
        //        //DataSet ds = JNDWInterfaceModel_MG.handelModel.getMzJzxx(model.outReimPara.PatInfo.OutPatId.ToString(), model.outReimPara.CommPara.TradeId.ToString());

        //        //lblZdbm.Visible = true;
        //        //lblZdmc.Visible = true;
        //        //tbZdbm.Visible = true;
        //        //tbZdmc.Visible = true;
        //        //tbZdbm.Text = ds.Tables[0].Rows[0]["DIAGNOSIS_CODE"].ToString();
        //        //tbZdmc.Text = ds.Tables[0].Rows[0]["DIAGNOSIS_NAME"].ToString();
        //        //cbxDiagnos.SelectedValue = ds.Tables[0].Rows[0]["DIAGNOSIS_CODE"].ToString();
        //        //if (cbxDiagnos.Text == "")
        //        //{
        //        //    cbxDiagnos.Text = ds.Tables[0].Rows[0]["DIAGNOSIS_NAME"].ToString();
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    //---------------------------------
        //}

        #endregion




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
            txtMemNo.Text = dicPerInfo["ylzbh"];
            txtIDNO.Text = dicPerInfo["shbzhm"];
            txtInvalid.Text = dicPerInfo["zfbz"];
            txtInvalidReason.Text = dicPerInfo["zfsm"];
            txtPersonType.Text = dicPerInfo["ylrylb"];
            txtCompany.Text = dicPerInfo["dwmc"];
            txtBalance.Text = dicPerInfo["ye"];
            txtIns.Text = dicPerInfo["sbjglx"] == "A" ? "职工" : (dicPerInfo["sbjglx"] == "B" ? "居民" : "");
            txtMzdbbz.Text = "";
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
            strDiagnosCode = isHaveDiagnos ? cbxDiagnos.SelectedValue.ToString() : "";
            strDiagnosName = cbxDiagnos.Text;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isCancel = true;
            this.Close();
        }

        private void PersonInfoDialog_Shown(object sender, EventArgs e)
        {
            //if (lblDiBao.Text == "" && arrList.Count==0)
            //{
            //    this.Close();//非低保，此界面自动关闭
            //}
        }
    }
}
