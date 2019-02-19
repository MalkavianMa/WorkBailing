using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using DW_YBBX.ZX_Business;

namespace DW_YBBX.LOGIN
{
    public partial class Sel_SBJG : Form
    {
        string sbjgbh; //参保人所属的社保机构编号
        string yybm; //医院编码（医院在医保中心的编码）
        string UserCode_DW;//地纬的用户名
        string Password_DW;//地纬的密码
        string UserCode; //操作人工号
        string UserName; //操作人姓名
        string HOSPITAL_ID; //HOSPITAL_ID
        string HOSPITAL_NAME; //HOSPITAL_NAME
        string BXLX;
        public Sel_SBJG(string _UserCode, string _UserName, string _HOSPITAL_ID, string _HOSPITAL_NAME)
        {
            UserCode = _UserCode;
            UserName = _UserName;
            HOSPITAL_ID = _HOSPITAL_ID;
            HOSPITAL_NAME = _HOSPITAL_NAME;
            InitializeComponent();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (rbt_ZG.Checked == true)
            {
                sbjgbh = ConfigurationManager.AppSettings["sbjgbh_ZGYB"].ToString();//社保机构编号(职工)
                yybm = ConfigurationManager.AppSettings["yybm"].ToString(); //医院编码
                UserCode_DW = ConfigurationManager.AppSettings["UserCode_DW"].ToString();//医保登录用户名
                Password_DW = ConfigurationManager.AppSettings["Password_DW"].ToString();//医保登录密码
                BXLX = "职工";
            }
            if (rbt_JM.Checked == true)
            {
                sbjgbh = ConfigurationManager.AppSettings["sbjgbh_JMYB"].ToString(); //社保机构编号(居民)
                yybm = ConfigurationManager.AppSettings["yybm"].ToString(); //医院编码
                UserCode_DW = ConfigurationManager.AppSettings["UserCode_DW"].ToString();//医保登录用户名
                Password_DW = ConfigurationManager.AppSettings["Password_DW"].ToString();//医保登录密码    
                BXLX = "居民";
            }
            this.Hide();

            FrmsbjgSelect fsbjg = new FrmsbjgSelect();
            fsbjg.ShowDialog();

            sbjgbh = fsbjg.selSbjgBH;
            string networkPatclassID = fsbjg.fglyComtext;

            fsbjg.Close();
            MainForm frm = new MainForm(UserCode, UserName, HOSPITAL_ID, HOSPITAL_NAME, sbjgbh, yybm, UserCode_DW, Password_DW, networkPatclassID);
            frm.ShowDialog();
            this.Close();
            this.Dispose();
        }
    }
}
