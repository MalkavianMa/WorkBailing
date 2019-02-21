using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PayAPIInstance.Dareway.ZIBO.Dialog
{
    public partial class FrmSetteInfo : Form
    {
        private Dictionary<string, string> patInfo;
        private Dictionary<string, string> setteInfo;

        /// <summary>
        /// 是否撤销
        /// </summary>
        public bool isCancle = false;

        /// <summary>
        /// 
        /// </summary>
        public decimal RE_GRZHZF = 0;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_patInfo"></param>
        /// <param name="_setteInfo"></param>
        /// <param name="canChangeGRZH">是否可修改个人账户支付</param>
        public FrmSetteInfo(Dictionary<string, string> _patInfo, Dictionary<string, string> _setteInfo, bool _canChangeGRZH = false)
        {
            InitializeComponent();
            patInfo = _patInfo;
            setteInfo = _setteInfo;
            RE_GRZHZF = Convert.ToDecimal(_setteInfo["grzhzf"]);
            if (_canChangeGRZH)
            {
                text_GRZHZF.ReadOnly = false;
                text_GRZHZF.TextChanged += text_GRZHZF_TextChanged; 
            }
        }

        /// <summary>
        /// 个人账户支付
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void text_GRZHZF_TextChanged(object sender, EventArgs e)
        {
            string _grzhzf = text_GRZHZF.Text;
            decimal d_grzhzf = 0;
            if (Decimal.TryParse(_grzhzf, out d_grzhzf))
            {
                setteInfo["grzhzf"] = d_grzhzf.ToString();

                text_BJXJ.Text = (Convert.ToDecimal(setteInfo["brfdje"]) - Convert.ToDecimal(setteInfo["grzhzf"])).ToString();//补缴现金
                text_XFHZHYE.Text = (Convert.ToDecimal(patInfo["zhye"]) - Convert.ToDecimal(setteInfo["grzhzf"])).ToString();                              //消费后账户余额
                text_GRZHZF.Text = setteInfo["grzhzf"].ToString();                             //个人账户支付
            }
            else 
            { 
                MessageBox.Show("请输入正确的数字！！");
                setteInfo["grzhzf"] = RE_GRZHZF.ToString();
                text_GRZHZF.Text = setteInfo["grzhzf"];
            }
        }

        /// <summary>
        /// 加载函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {

            text_XM.Text = patInfo["xm"];                                                  //姓名
            text_XB.Text = patInfo["xb"] == "1" ? "男" : "女";                             //性别
            text_SFZHM.Text = patInfo["sfzhm"];                                            //身份证号码
            text_YBKH.Text = patInfo["kh"];                                                //医保卡号
            text_DWMC.Text = patInfo["dwmc"];                                              //单位名称
            text_JBJG.Text = patInfo["sbjgbh"];                                            //经办机构
            text_KYE.Text = patInfo["zhye"];                                               //卡余额
            text_BNYNRTCFW.Text = setteInfo["bnynrtcfw"].ToString();                       //本年进入统筹额度
            text_BCNRTCFW.Text = setteInfo["bcnrtcfw"].ToString();                         //本次进入统筹额度
            text_BCZFY.Text = setteInfo["zje"].ToString();                                 //本次总费用
            text_BCQFQ.Text = setteInfo["bcqfx"].ToString();                               //本次起付钱
            text_TCZF.Text = setteInfo["tczf"].ToString();                                 //统筹支付
            text_DEZF.Text = setteInfo["dezf"].ToString();                                 //大额支付
            text_QTTCZF.Text = setteInfo["qttczf"].ToString();                             //其他统筹支付
            if (setteInfo.ContainsKey("zhzf"))
            {
                text_ZHZF.Text = setteInfo["zhzf"].ToString();                             //暂缓（灰名单）支付 
            }
            text_GRFDZE.Text = setteInfo["brfdje"].ToString();                             //个人负担总额
            text_YLBZJE.Text = setteInfo["ylbzje"].ToString();                             //医疗补助金额
            text_YLJMJE.Text = "";                                                         //医疗减免金额
            text_BJXJ.Text = (Convert.ToDecimal(setteInfo["brfdje"]) - Convert.ToDecimal(setteInfo["grzhzf"])).ToString();//补缴现金

            if (setteInfo.ContainsKey("zhye"))
            {
                text_XFHZHYE.Text = setteInfo["zhye"].ToString();                          //消费后账户余额 
            }
            text_GRZHZF.Text = setteInfo["grzhzf"].ToString();                             //个人账户支付
        }

        /// <summary>
        /// 收费记账
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 放弃结算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            isCancle = true;
            this.Close();
        }
    }
}
