using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms; 
using PayAPIInterface.ParaModel;

namespace PayAPIInstance.JingQi.FuYang.Dialogs
{
    public partial class InfoPreSettle : Form
    { 
         
        public InPayParameter inReimPara;//住院患者信息
        PersonInfo pi = new PersonInfo();
        /// <summary>
        /// 医疗总费用
        /// </summary>
        public string Amount = "";
        /// <summary>
        /// 基金支付金额
        /// </summary>
        public string JJZFJE = "";
        /// <summary>
        /// 身份证号
        /// </summary>
        public string ICNo = "";
        /// <summary>
        /// 可报销费用
        /// </summary>
        public string KBXFY = "";
        /// <summary>
        /// 起伏线
        /// </summary>
        public string QFX = "";
        /// <summary>
        /// 自费金额
        /// </summary>
        public string ZFJE = "";
        /// <summary>
        /// 自付金额
        /// </summary>
        public string ZJFJE = "";
        /// <summary>
        /// 帐户支付
        /// </summary>
        public string ZHZF = "";

        /// <summary>
        /// 帐户余额
        /// </summary>
        public string ZHYE = "";

        /// <summary>
        /// 民政救助
        /// </summary>
        public string MZJZ = "";
        public string DBBC = "";  //大病补偿
        public string czdd = "";  //财政兜底
        public string yibaling = ""; //180
        public string qitabc = ""; //180
        /// <summary>
        /// 是否单病种补偿
        /// </summary>
        public string DBZBC = "";
        /// <summary>
        /// 单病种费用定额
        /// </summary>
        public string DBZDE = "";
        /// <summary>
        /// 单病种医院垫付
        /// </summary>
        public string DBZDF = "";
        /// <summary>
        /// 单病种个人自付
        /// </summary>
        public string DBZGRZF = "";
        /// <summary>
        /// 高额材料限价超额费
        /// </summary>
        public string GECLXJ = "";
        /// <summary>
        /// 补偿类型名称
        /// </summary>
        public string BCLXMC = "";

        /// <summary>
        /// 院前检查金额
        /// </summary>
        public string YQJCZE = "";
        public string BRFD = "";
        public string YYFD = "";
        /// <summary>
        /// 院前检查报销金额
        /// </summary>
        public string YQJCBXJE = "";



        /// <summary>
        /// 病人负担
        /// </summary>
        public string sPatientAssume = "";

        /// <summary>
        /// 医院负担
        /// </summary>
        public string sHospialAssume = "";


        public string restr = "";

        public InfoPreSettle()
        {
            InitializeComponent();
        }

        public  InfoPreSettle(InPayParameter inpara , bool isOut = false ) {
            InitializeComponent();
            inReimPara = inpara;
            if (isOut)
            {
                button1.Visible = false;
            }
        }

        public void Getcalinfo(string spatientassum, string ahospitalsaaum) // ml
        {
            sPatientAssume = spatientassum;
            sHospialAssume = ahospitalsaaum;
        }


        private void InfoPreSettle_Load(object sender, EventArgs e)
        {
            tbAmountTotal.Text = Amount;//总费用
            tbjjzfje.Text = JJZFJE;
            tbzjfje.Text = ZJFJE;
            tbzfje.Text = ZFJE;

            tbkbxfy.Text = KBXFY;

            tbqfx.Text = QFX;
            tbzhye.Text = ZHYE;
            tbzhzf.Text = ZHZF;
            tbkbxfy.Text = KBXFY;
            txtyqjcbx.Text=YQJCBXJE;
            txtyqjcze.Text=YQJCZE;
            txtbrcd.Text = sPatientAssume;
            txtyycd.Text = sHospialAssume;
            txt_dbbc.Text = DBBC;
            txt_czdd.Text = czdd;
            txt_mzjz.Text = MZJZ;
            text180.Text = yibaling;
            textBox1.Text = qitabc;

            if (DBZBC == "是" || DBZBC == "1")
            {
                //tbDBZBC.Text = DBZBC;
                //tbDBZDE.Text = DBZDE;
                //tbDBZDF.Text = DBZDF;
                //tbDBZGRZF.Text = DBZGRZF;
                //tbGECLXJ.Text = GECLXJ;
            }
            else//不是单病种 隐藏单病种信息
            {
                //groupBox2.Visible = false;
                //this.Height -= groupBox2.Height;
            }
            btnClose.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        { 
            this.Close();
            FYNHCityInterfaceModel mm = new FYNHCityInterfaceModel();
            mm.CancelReimSettle(inReimPara.RegInfo.Memo2, inReimPara.RegInfo.NetRegSerial, inReimPara.SettleInfo.SettleNo);
            restr = "2";
            mm.CancelOutReigster(inReimPara.RegInfo.Memo2, inReimPara.RegInfo.NetRegSerial);
            restr = "1";
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

       
    }
}
