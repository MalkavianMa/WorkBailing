using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DW_YBBX.ZX_Business
{
    public partial class frmCqhlCx : Form
    {
        private RZYBHandle rzybHandle;
        
        //护理申请查询
        private YBMLModel Huli = new YBMLModel();
        //社保机构编号
        public string sbjgbh = string.Empty;
        //注册码
        public string zcm = string.Empty;
        //医院编号
        public string yybm = string.Empty;
        //usercode
        public string usercode = string.Empty;

        //护理查询
        private List<Hlcx> hlcx_ds;

        public frmCqhlCx(Form Parent)
        {
            InitializeComponent();
            this.MdiParent = Parent;
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

            Dictionary<string, string> result = new Dictionary<string, string>();
            rzybHandle = new RZYBHandle();
            string zcm = "211231I-970453-771964-4535";
            string yybm = "51080096";
            string Sbjg = txtSBjgbh.Text;
            string GrBH = txtGrbh.Text.Trim();

            rzybHandle = new RZYBHandle();
            rzybHandle.InitResolver(Sbjg, zcm, yybm, usercode); //初始化参数 ：社保机构编号、注册码、医院编码、usercode
            rzybHandle.AddInParas("p_grbh", GrBH);         //返回参数的格式 json  excel  txt
            rzybHandle.Handle("query_cqhl");                       // 传入方法名
           // result = rzybHandle.GetResultDict();
            result = rzybHandle.GetResultDict_Cx();

            foreach (string item in result.Keys)
            {
                // Console.WriteLine("键为：{0}，值为：{1}", item, result[item]);
                using (DataTable table = new DataTable())
                {
                    table.Columns.Add("护理种类");
                    table.Columns.Add("审核结果");
                    table.Columns.Add("审核意见");
                    table.Columns.Add("护理开始时间");

                    if (result.Count > 0)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            DataRow dr = table.NewRow();
                            dr["护理种类"] = result["hlzl"];
                            dr["审核结果"] = result["spbz"];
                            dr["审核意见"] = result["spyj"];
                            dr["护理开始时间"] = result["qsrq"];
                            table.Rows.Add(dr);
                        }
                    }

                    dataGridView1.DataSource = table;
                }
            }



            //hlcx_ds = rzybHandle.GetResult<List<Hlcx>>("hlcx_ds");

            //using (DataTable table = new DataTable())
            //{
            //    table.Columns.Add("护理种类");
            //    table.Columns.Add("审核结果");
            //    table.Columns.Add("审核意见");
            //    table.Columns.Add("护理开始时间");

            //    if (hlcx_ds.Count > 0)
            //    {
            //        for (int i = 0; i < hlcx_ds.Count; i++)
            //        {
            //            DataRow dr = table.NewRow();
            //            dr["护理种类"] = hlcx_ds[i].hlzl;
            //            dr["审核结果"] = hlcx_ds[i].spbz;
            //            dr["审核意见"] = hlcx_ds[i].spyj;
            //            dr["护理开始时间"] = hlcx_ds[i].qsrq;
            //            table.Rows.Add(dr);
            //        }
            //    }

            //    dataGridView1.DataSource = table;
            //}
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCqhlCx_Load(object sender, EventArgs e)
        {

        }
    }
}
