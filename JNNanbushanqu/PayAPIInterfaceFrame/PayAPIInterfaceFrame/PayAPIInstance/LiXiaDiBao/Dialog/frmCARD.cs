using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PayAPIInstance.LiXiaDiBao.Dialog
{
    public partial class frmCARD : Form
    {
        //是否取消
        public bool isCancel = true;
        LiXiaDiBaoModel model;
        public frmCARD(LiXiaDiBaoModel _model)
        {
            InitializeComponent();
            model = _model;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            model.networkPatInfo.MedicalNo = tbSerialNo.Text;
            model.networkPatInfo.IDNo = tbIDCardNo.Text;
            model.networkPatInfo.ICNo = tbInsuranceID.Text;
            model.networkPatInfo.PatName = tbName.Text;
            model.networkPatInfo.Sex = tbGENDER.Text; //性别名称
            model.networkPatInfo.CompanyNo = tbAidType.Text;
            model.networkPatInfo.CompanyName = tbAidTypeMC.Text;
            model.networkPatInfo.MedicalType = tbInsuranceType.Text;
            model.networkPatInfo.MedicalTypeName = tbInsuranceTypeMC.Text;
            
            isCancel = false;
            this.Close();
        }

        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            isCancel = true;
            this.Close();
        }

        private void frmCARD_Load(object sender, EventArgs e)
        {
            int reInt;
            SDACardOpen();
            FindCard();
            tbSerialNo.Text = axSDACard1.GetSerialNo(); //读取救助卡流水号（跟卡封面的流水号一致）
            tbName.Text = axSDACard1.GetName();  //读取居民姓名
            tbIDCardNo.Text = axSDACard1.GetIDCardNo(); //读取身份证号码
            tbInsuranceID.Text = axSDACard1.GetInsuranceID(); //读取医保卡号
            tbInsuranceType.Text = axSDACard1.GetInsuranceType(); //读取医保类型
            tbAidType.Text = axSDACard1.GetAidType(); //读取救助类型
            //axSDACard1.Beep(); //使读写器发出蜂鸣声
            SDACardClose();


            string strMC="";
            switch (tbInsuranceType.Text)
            {
                case "01":
                    strMC = "门规";
                    break;
                case "02":
                    strMC = "非门规";
                    break;
                default:
                    strMC = "";
                    break;
            }
            tbInsuranceTypeMC.Text = strMC;


            //返回值：编码字符串，0001：低保，0010：低保边缘；0100：一级中度残疾；1000：二级重度残疾，支持组合类型，比如0011表示既是低保又是低保边缘
            strMC = "";
            string strS = tbAidType.Text;
            try
            {
                if (strS[3] == '1')
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
                if (strS[2] == '1')
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
                if (strS[1] == '1')
                {
                    if (strMC == "")
                    {
                        strMC = "一级中度残疾";
                    }
                    else
                    {
                        strMC = "+一级中度残疾";
                    }
                }
                if (strS[0] == '1')
                {
                    if (strMC == "")
                    {
                        strMC = "二级重度残疾";
                    }
                    else
                    {
                        strMC = "+二级重度残疾";
                    }
                }
                tbAidTypeMC.Text = strMC;
            }
            catch (Exception ex)
            {
            }

            //-----------------------------
            //--卡里没有的信息，从前置机数据库中取出来
           
            StringBuilder strSql = new StringBuilder();
            DataSet ds = new DataSet();
            string re="";
            string errString;
            strSql.Append(" SELECT GENDER,SICARDNO,PHONENUMBER,SITYPE,AIDTYPE,ADDRESS,DISABLEDCARDNO,DISABLETYPE,PHOTO,ISDELETED FROM POWERSDA.CARDHOLDERINFO where AIDCARDNO='" + tbSerialNo.Text + "'   ");
            WebReferenceDiBao.DiBaoWebService WebServiceObj = new WebReferenceDiBao.DiBaoWebService();
            re = WebServiceObj.Query(strSql.ToString(), out errString, out ds);
            if (re != "0")
            {
                throw new Exception("调用WebService取低保数据失败:" + errString);
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                //---------------------------------------
                try
                {
                    byte[] File = (byte[])ds.Tables[0].Rows[0]["PHOTO"];
                    Image photo = null;
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(File))
                    {
                        ms.Write(File, 0, File.Length);
                        photo = Image.FromStream(ms, true);
                    }
                    this.pictureBox1.Image = photo;
                    if (File.Length == 0)
                    {
                        lblts.Visible = true;
                    }
                    else
                    {
                        lblts.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                }
                //--------------------------------------
                tbGENDER.Text = ds.Tables[0].Rows[0]["GENDER"].ToString() == "0" ? "女" : (ds.Tables[0].Rows[0]["GENDER"].ToString() == "1" ? "男" : "未知");//性别
                tbPHONENUMBER.Text = ds.Tables[0].Rows[0]["PHONENUMBER"].ToString(); //电话号码
                tbADDRESS.Text = ds.Tables[0].Rows[0]["ADDRESS"].ToString(); //现住址
                tbDISABLEDCARDNO.Text = ds.Tables[0].Rows[0]["DISABLEDCARDNO"].ToString(); //残疾证号
                tbDISABLETYPE.Text = ds.Tables[0].Rows[0]["DISABLETYPE"].ToString(); //残疾类型
                tbISDELETED.Text = ds.Tables[0].Rows[0]["ISDELETED"].ToString()=="1"?"已删除":""; //删除标志
                tbInsuranceID.Text = ds.Tables[0].Rows[0]["SICARDNO"].ToString(); //医保卡号
                //-----------------------------------------
                tbInsuranceType.Text = ds.Tables[0].Rows[0]["SITYPE"].ToString();
                tbInsuranceTypeMC.Text = ds.Tables[0].Rows[0]["SITYPE"].ToString()=="1"?"门规":"非门规";
                //---------------------------------------------------
                tbAidType.Text = ds.Tables[0].Rows[0]["AIDTYPE"].ToString();
                //救助类型 1=低保 2=低保边缘 3=一级伤残 4=二级伤残 
                strMC = "";
                strS = tbAidType.Text;
                try
                {
                    string[] rr = strS.Split(new string[] { "||" }, System.StringSplitOptions.None);
                    for (int i = 0; i < rr.Length; i++)
                    {
                        if (rr[i] == "1")
                        {
                            if (strMC=="")
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
                    tbAidTypeMC.Text = strMC;
                }
                catch (Exception ex)
                {
                }
            }
             
            //---------------------------------------
            
        }

        private void SDACardOpen()
        {
            int reInt = axSDACard1.Open(); //连接读卡器
            if (reInt != 0)
            {
                throw new Exception("Open失败!错误号:" + reInt);
            }
        }

        private void FindCard()
        {
            int reInt = axSDACard1.FindCard(); //寻卡
            if (reInt != 0)
            {
                throw new Exception("FindCard失败!错误号:" + reInt);
            }
        }

        private void SDACardClose()
        {
            int reInt = axSDACard1.Close(); //关闭读卡器
            if (reInt != 0)
            {
                throw new Exception("Close失败!错误号:" + reInt);
            }
        }

        private void btn_xPhoto_Click(object sender, EventArgs e)
        {
            frmPhoto photo = new frmPhoto(pictureBox1.Image);
            photo.TopMost = true;
            photo.ShowDialog();

        }

        private void frmCARD_Shown(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }
        //------------------------------
    }
}
