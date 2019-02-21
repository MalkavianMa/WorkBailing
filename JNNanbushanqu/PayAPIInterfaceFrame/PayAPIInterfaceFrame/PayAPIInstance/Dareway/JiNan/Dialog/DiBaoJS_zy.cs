using PayAPIInstance.tools;
using PayAPIInterface.ParaModel;
using PayAPIUtilities.Config;
using PayAPIUtilities.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PayAPIInstance.Dareway.JiNan.Dialog
{
    public partial class DiBaoJS_zy : Form
    {
        public MSSQLHelper sqlHelper = new MSSQLHelper("Data Source=172.22.29.6;Initial Catalog=COMM;Persist Security Info=True;User ID=power;Password=m@ssunsoft009");


        /// <summary>
        /// 是否取消
        /// </summary>
        public bool isCancel = false;
        /// <summary>
        /// 医院ID
        /// </summary>
        public string hosid = "";

        public InPayParameter inReimPara;
        //低保结算信息
        public Dictionary<string, string> dicSettleInfoDibao;

        /// <summary>
        /// 结算信息
        /// </summary>
        public Dictionary<string, string> dicSettleInfo;

        /// <summary>
        /// 操作员ID
        /// </summary>
        public string hosOperatorSysid = "";


        public DibaoSdaCash dsc = new DibaoSdaCash();
        /// <summary>
        /// 操作员姓名
        /// </summary>
        public string hosOperatorName = "";
        public DiBaoJS_zy(InPayParameter _inReimPara, Dictionary<string, string> _dicSettleInfo, Dictionary<string, string> _dicSettleInfoDibao, string hosID, string hosOperatorSysid, string hosOperatorName)
        {
            InitializeComponent();
            inReimPara = _inReimPara;//判断是否可以拿到PatInhosID;
            dicSettleInfo = _dicSettleInfo;
            dicSettleInfoDibao = _dicSettleInfoDibao;
            this.hosid = hosID;
            this.hosOperatorSysid = hosOperatorSysid;
            this.hosOperatorName = hosOperatorName;
        }

        private void DiBaoJS_zy_Load(object sender, EventArgs e)
        {
            try
            {

                DataSet ds = new DataSet();
                string strSql = "";
                //加载公费病种
                strSql = "SELECT bh,mc  FROM [REPORT].[dbo].[db_mgbz] order by bh";
                ds = sqlHelper.ExecSqlReDs(strSql);
                DataRow dr = ds.Tables[0].NewRow();
                dr[0] = "";
                dr[1] = "";
                ds.Tables[0].Rows.InsertAt(dr, 0);
                cmb_mgbz.DataSource = ds.Tables[0];
                cmb_mgbz.ValueMember = "bh";//值
                cmb_mgbz.DisplayMember = "mc";//显示字段
                cmb_mgbz.SelectedIndex = 0;

                cmb_mgbz.DropDownHeight = 600;

                ///------------------------------------
                isCancel = false;
                txtZYH.Text = inReimPara.PatInfo.PatInHosCode;
                decimal amount = 0;
                amount = Convert.ToDecimal(dicSettleInfo["brfdje"]) + Convert.ToDecimal(dicSettleInfo["ybfdje"]) + Convert.ToDecimal(dicSettleInfo["yljmje"]);       //本次医疗费用
                decimal getAmount = 0;
                getAmount = Convert.ToDecimal(dicSettleInfo["brfdje"]) - Convert.ToDecimal(dicSettleInfo["grzhzf"]);
                txt_FYZE.Text = amount.ToString();
                txtGRZF.Text = getAmount.ToString();
                txt_YBZF.Text = (amount - getAmount).ToString();
                DK();

                //当姓名不一致时提示
                if (inReimPara.PatInfo.InPatName != tbName.Text)
                {
                    if (MessageBox.Show(" 低保卡姓名为：【" + tbName.Text.ToString() + "】     HIS患者姓名为：【" + inReimPara.PatInfo.InPatName + "】 是否继续 ", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    {
                        throw new Exception("姓名不一致，操作员取消操作！");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("低保结算失败:" + ex.Message);
            }
        }


        #region 低保
        /// <summary>
        /// 低保读卡
        /// </summary>
        private void DK()
        {
            int reInt;
            reInt = axSDACard1.Open(); //连接读卡器
            if (reInt != 0)
            {
                throw new Exception("读卡器Open失败!错误号:" + reInt);
            }

            reInt = axSDACard1.FindCard(); //寻卡
            if (reInt != 0)
            {
                throw new Exception("寻卡失败!错误号:" + reInt);
            }
            tbSerialNo.Text = axSDACard1.GetSerialNo(); //读取救助卡流水号（跟卡封面的流水号一致）
            tbName.Text = axSDACard1.GetName();  //读取居民姓名
            tbIDCardNo.Text = axSDACard1.GetIDCardNo(); //读取身份证号码
            tbInsuranceID.Text = axSDACard1.GetInsuranceID(); //读取医保卡号
            tbInsuranceType.Text = axSDACard1.GetInsuranceType(); //读取医保类型
            tbAidType.Text = axSDACard1.GetAidType(); //读取救助类型
            reInt = axSDACard1.Close(); //关闭读卡器
            if (reInt != 0)
            {
                throw new Exception("读卡器Close失败!错误号:" + reInt);
            }



            string strMC = "";
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
                        strMC = strMC + "+低保";
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
                        strMC = strMC + "+低保边缘";
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
                        strMC = strMC + "+一级中度残疾";
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
                        strMC = strMC + "+二级重度残疾";
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
            string re = "";
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
                #region 图片显示注掉
                //--------------------------------------- 图片显示注掉
                //try
                //{
                //    byte[] File = (byte[])ds.Tables[0].Rows[0]["PHOTO"];
                //    Image photo = null;
                //    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(File))
                //    {
                //        ms.Write(File, 0, File.Length);
                //        photo = Image.FromStream(ms, true);
                //    }
                //    this.pictureBox1.Image = photo;
                //    if (File.Length == 0)
                //    {
                //        lblts.Visible = true;
                //    }
                //    else
                //    {
                //        lblts.Visible = false;
                //    }
                //}
                //catch (Exception ex)
                //{
                //}
                //-------------------------------------- 
                #endregion
                tbGENDER.Text = ds.Tables[0].Rows[0]["GENDER"].ToString() == "0" ? "女" : (ds.Tables[0].Rows[0]["GENDER"].ToString() == "1" ? "男" : "未知");//性别
                tbPHONENUMBER.Text = ds.Tables[0].Rows[0]["PHONENUMBER"].ToString(); //电话号码
                tbADDRESS.Text = ds.Tables[0].Rows[0]["ADDRESS"].ToString(); //现住址
                tbDISABLEDCARDNO.Text = ds.Tables[0].Rows[0]["DISABLEDCARDNO"].ToString(); //残疾证号
                tbDISABLETYPE.Text = ds.Tables[0].Rows[0]["DISABLETYPE"].ToString(); //残疾类型
                tbISDELETED.Text = ds.Tables[0].Rows[0]["ISDELETED"].ToString() == "1" ? "已删除" : ""; //删除标志
                tbInsuranceID.Text = ds.Tables[0].Rows[0]["SICARDNO"].ToString(); //医保卡号
                //-----------------------------------------
                tbInsuranceType.Text = ds.Tables[0].Rows[0]["SITYPE"].ToString();
                tbInsuranceTypeMC.Text = ds.Tables[0].Rows[0]["SITYPE"].ToString() == "1" ? "门规" : "非门规";
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
                            if (strMC == "")
                            {
                                strMC = "低保";
                            }
                            else
                            {
                                strMC = strMC + "+低保";
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
                                strMC = strMC + "+低保边缘";
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
                                strMC = strMC + "+一级伤残";
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
                                strMC = strMC + "+二级伤残";
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


        #endregion


        private void ClearTxt()
        {
            /*
            txt_PAT_IN_HOS_ID.Text = "";
            txt_XM.Text = "";
          
            txt_SFZH.Text = "";
            txt_RYSJ.Text = "";
            txt_CYSJ.Text = "";
            txt_CYZDMC.Text = "";
            txt_KS.Text = "";
            txt_YS.Text = "";
            txt_FB.Text = "";
            txt_FYZE.Text = "";
            txt_YBZF.Text = "";
            txtGRZF.Text = "";
            */
            foreach (Control cl in this.groupBox1.Controls)
            {
                if (cl is TextBox)
                {
                    cl.Text = string.Empty;
                }
                else if (cl is ComboBox)
                {
                    ComboBox cob = cl as ComboBox;
                    cob.SelectedIndex = -1;
                }
            }


            foreach (Control cl in this.groupBox3.Controls)
            {
                if (cl is TextBox)
                {
                    if (cl.Name != "txtZYH")
                    {
                        cl.Text = string.Empty;
                    }
                }
                else if (cl is ComboBox)
                {
                    ComboBox cob = cl as ComboBox;
                    cob.SelectedIndex = -1;
                }
            }
            //---------------------------------
            pictureBox1.Image = null;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            //当姓名不一致时提示
            if (inReimPara.PatInfo.InPatName != tbName.Text)
            {
                if (MessageBox.Show(" 低保卡姓名为：【" + tbName.Text.ToString() + "】     HIS患者姓名为：【" + inReimPara.PatInfo.InPatName + "】 是否继续 ", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                {
                    return;
                }
            }
            //DataSet ds = new DataSet();
            //string strSql = "";
            //strSql = "SELECT INVOICE_CODE,AMOUNT FROM ZY.[IN].IN_SETTLE_MAIN WHERE PAT_IN_HOS_ID='" + inReimPara.RegInfo.PatInHosId + "' AND FLAG_INVALID=0";
            //ds = sqlHelper.ExecSqlReDs(strSql);
            //if (ds.Tables[0].Rows.Count == 0)
            //{
            //    MessageBox.Show("没有找到此人出院发票信息,请确定是否出院！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    txtZYH.Focus();
            //    txtZYH.SelectAll();
            //    return;
            //}
            dsc = new DibaoSdaCash(tbIDCardNo.Text.Trim(), txt_YBZF.Text.Trim(), tbName.Text.Trim(), tbInsuranceID.Text.Trim(), tbInsuranceType.Text.Trim(), tbInsuranceTypeMC.Text.Trim(), tbAidType.Text.Trim(), tbAidTypeMC.Text.Trim(), tbSerialNo.Text.Trim(), tbGENDER.Text.Trim(),this.cmb_mgbz.SelectedValue.ToString(),this.cmb_mgbz.Text);
            dibao_zy_continue_js dzc = new dibao_zy_continue_js(inReimPara, dicSettleInfo, dicSettleInfoDibao, hosid, hosOperatorSysid, hosOperatorName,dsc);
            dzc.ShowDialog();
            if (dzc.isCancel)
            {
                this.isCancel = true;
            }
            this.Close();
        }



        private void cmb_mgbz_MouseEnter(object sender, EventArgs e)
        {
            ToolTip p = new ToolTip();
            p.ShowAlways = true;
            p.SetToolTip(cmb_mgbz, cmb_mgbz.Text);  //显示提示
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isCancel = true;
            this.Close();
            // throw new Exception("操作员取消了操作");
        }
    }
}
