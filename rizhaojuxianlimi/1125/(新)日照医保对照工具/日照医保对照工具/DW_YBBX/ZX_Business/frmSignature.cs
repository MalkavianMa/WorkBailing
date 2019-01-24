using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DW_YBBX.ZX_Business
{
    public partial class frmSignature : Form
    {
        public frmSignature()
        {
            InitializeComponent();
        }
        private RZYBHandle rzybHandle;
        //社保机构编号
        public string sbjgbh = string.Empty;
        //注册码
        public string zcm = string.Empty;
        //医院编号
        public string yybm = string.Empty;
        //usercode
        public string usercode = string.Empty;
        private void button1_Click(object sender, EventArgs e)
        {
          //  rzybHandle = new RZYBHandle();

            Dictionary<string, string> result = new Dictionary<string, string>();
            string zcm = "211231I-970453-771964-4535";
            string yybm = "51080096";

            Dictionary<string, string> patInfo = QueryPersonInfo(txtGrbh.Text.Trim(), "0", "C");
            sbjgbh = patInfo["sbjgbh"];


            rzybHandle = new RZYBHandle();
            rzybHandle.InitResolver(sbjgbh, zcm, yybm, usercode); //初始化参数 ：社保机构编号、注册码、医院编码、usercode
            rzybHandle.AddInParas("p_grbh", txtGrbh.Text.Trim());         //个人编号
                    //病情描述及诊断
            //rzybHandle.AddInParas("p_dmbh", .SelectedValue.ToString());         //

            rzybHandle.Handle("add_mztc");                       // 传入方法名
            MessageBox.Show("签约成功!个人编号:" + txtGrbh.Text);
          

        }


        /// <summary>
        /// 无卡人员读取信息
        /// </summary>
        /// <param name="p_grbh"></param>
        /// <param name="p_xm"></param>
        /// <param name="p_yltclb"></param>
        /// <param name="p_xzbz"></param>
        /// <returns></returns>
        public static Dictionary<string, string> QueryPersonInfo(string p_grbh, string p_yltclb, string p_xzbz)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            SignatureYB fs = new SignatureYB();
            fs.rzybHandle = new RZYBHandle();
            fs.sbjgbh = "000000";
            fs.zcm = "211231I-970453-771964-4535";
            fs.yybm = "51080096";
            fs.rzybHandle.InitResolver(fs.sbjgbh, fs.zcm, fs.yybm, fs.usercode);
           //();
            //handelModel.SBJGBH = "000000";
            fs.rzybHandle.AddInParas("p_grbh", p_grbh);      //*个人编号
            fs.rzybHandle.AddInParas("p_yltclb", "0");  //医疗统筹类别
            fs.rzybHandle.AddInParas("p_xzbz", "C");      //险种标志
            //业务处理
            fs.rzybHandle.Handle("query_basic_info");

            result = fs.rzybHandle.GetResultDict();
            return result;
        }
    }
}
