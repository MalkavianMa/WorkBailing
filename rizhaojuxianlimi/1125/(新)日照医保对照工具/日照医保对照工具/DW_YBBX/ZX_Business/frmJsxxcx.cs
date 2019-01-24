using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace DW_YBBX.ZX_Business
{
    public partial class frmJsxxcx : Form
    {

        private RZYBHandle rzybHandle;

        //护理申请查询
        private YBMLModel JsModel = new YBMLModel();
        //社保机构编号
        public string sbjgbh = string.Empty;
        //注册码
        public string zcm = string.Empty;
        //医院编号
        public string yybm = string.Empty;
        //usercode
        public string usercode = string.Empty;

        //护理查询
        private List<jsxx> jsxx_ds;

        public frmJsxxcx(Form Parent)
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
            rzybHandle = new RZYBHandle();
            string zcm = "211231I-970453-771964-4535";
            string yybm = "51080096";
            string sbjg = txtSbjgbh.Text;
            string jslb = cmbJslb.SelectedValue.ToString();
            string rqlb = cmbRqlb.SelectedValue.ToString();
            string qsrq = dateTimePicker1.Value.ToString("yyyyMMdd");
            string zzrq = dateTimePicker2.Value.ToString("yyyyMMdd");
            rzybHandle = new RZYBHandle();
            rzybHandle.InitResolver(sbjg, zcm, yybm, usercode); //初始化参数 ：社保机构编号、注册码、医院编码、usercode
            rzybHandle.AddInParas("p_jslb", jslb);         //返回参数的格式 json  excel  txt
            rzybHandle.AddInParas("p_rqlb", rqlb);         //返回参数的格式 json  excel  txt
            rzybHandle.AddInParas("p_qsrq", qsrq);         //返回参数的格式 json  excel  txt
            rzybHandle.AddInParas("p_zzrq", zzrq);         //返回参数的格式 json  excel  txt
            rzybHandle.Handle("query_jsxx");                       // 传入方法名

            jsxx_ds = rzybHandle.GetResult<List<jsxx>>("jsxx_ds");

            using (DataTable table = new DataTable())
            {
                table.Columns.Add("个人编号");
                table.Columns.Add("姓名");
                table.Columns.Add("病人结算日期");
                table.Columns.Add("病人结算号");
                table.Columns.Add("医疗统筹类别");
                table.Columns.Add("医疗类别");
                table.Columns.Add("人群类别");
                table.Columns.Add("总金额");
                table.Columns.Add("医保负担金额");
                table.Columns.Add("个人账户支付");
                table.Columns.Add("现金支付");
                table.Columns.Add("结算标志");
                table.Columns.Add("本次统筹支付");
                table.Columns.Add("本次大额支付");
                table.Columns.Add("本次公务员补助支付");
                table.Columns.Add("本次商保大病理赔费用");
                table.Columns.Add("医疗机构减免金额");
                table.Columns.Add("扶贫人员民政救助金额");
                table.Columns.Add("扶贫特惠保险金额");
                table.Columns.Add("低保救助金额");
                table.Columns.Add("重特大疾病救助金额");
               

                if (jsxx_ds.Count > 0)
                {
                    for (int i = 0; i < jsxx_ds.Count; i++)
                    {
                        DataRow dr = table.NewRow();
                        dr["个人编号"] = jsxx_ds[i].grbh;
                        dr["姓名"] = jsxx_ds[i].xm;
                        dr["病人结算日期"] = jsxx_ds[i].brjsrq;
                        dr["病人结算号"] = jsxx_ds[i].jshid;
                        dr["医疗统筹类别"] = jsxx_ds[i].yltclb;
                        dr["医疗类别"] = jsxx_ds[i].yllb;
                        dr["人群类别"] = jsxx_ds[i].rqlb;
                        dr["总金额"] = jsxx_ds[i].zje;
                        dr["医保负担金额"] = jsxx_ds[i].ybfdje;
                        dr["个人账户支付"] = jsxx_ds[i].grzhzf;
                        dr["现金支付"] = jsxx_ds[i].xjzf;
                        dr["结算标志"] = jsxx_ds[i].jsbz;
                        dr["本次统筹支付"] = jsxx_ds[i].tczf;
                        dr["本次大额支付"] = jsxx_ds[i].dezf;
                        dr["本次公务员补助支付"] = jsxx_ds[i].gwybz;
                        dr["本次商保大病理赔费用"] = jsxx_ds[i].bcsbdblpzf;
                        dr["医疗机构减免金额"] = jsxx_ds[i].yljmje;
                        dr["扶贫人员民政救助金额"] = jsxx_ds[i].fprymzjzje;
                        dr["扶贫特惠保险金额"] = jsxx_ds[i].fpthbxje;
                        dr["低保救助金额"] = jsxx_ds[i].dbjzje;
                        dr["重特大疾病救助金额"] = jsxx_ds[i].ztdjbzf;

                        table.Rows.Add(dr);
                    }
                }

                dataGridView1.DataSource = table;
            }
        }

        /// <summary>
        /// 关闭页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 加载登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmJsxxcx_Load(object sender, EventArgs e)
        {
            ArrayList arr = new ArrayList();
            //arr.Add(new DictionaryEntry("", ""));
            arr.Add(new DictionaryEntry("1", "普通住院"));
            arr.Add(new DictionaryEntry("4", "门诊慢性病"));
            arr.Add(new DictionaryEntry("15", "尿毒症新结算办法"));
            arr.Add(new DictionaryEntry("6", "普通门诊"));
            arr.Add(new DictionaryEntry("16", "靶向药"));
            arr.Add(new DictionaryEntry("2", "长期护理"));
            cmbJslb.DataSource = arr;
            cmbJslb.ValueMember = "Key";
            cmbJslb.DisplayMember = "Value";
            cmbJslb.SelectedIndex = 0;

            ArrayList arr1 = new ArrayList();
            //arr1.Add(new DictionaryEntry("", ""));
            arr1.Add(new DictionaryEntry("A", "职工"));
            arr1.Add(new DictionaryEntry("B", "居民"));
            //arr1.Add(new DictionaryEntry("3", "居家医疗护理"));
            cmbRqlb.DataSource = arr1;
            cmbRqlb.ValueMember = "Key";
            cmbRqlb.DisplayMember = "Value";
            cmbRqlb.SelectedIndex = 0;
        }
    }
}
