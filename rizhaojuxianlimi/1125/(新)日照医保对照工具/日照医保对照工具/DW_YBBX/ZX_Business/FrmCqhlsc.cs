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
    public partial class FrmCqhlsc : Form
    {

        private RZYBHandle rzybHandle;
        //社保机构编号
        public string sbjgbh = string.Empty;
        //注册码
        public string zcm = string.Empty;
        //医院编号
        public string yybm = string.Empty;
        //usercode
        public string usercode = string.Empty;
        public FrmCqhlsc(Form Parent)
        {
            InitializeComponent();
            this.MdiParent = Parent;
        }


        /// <summary>
        /// 页面赋值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCqhlsc_Load(object sender, EventArgs e)
        {
            ArrayList arr = new ArrayList();
            arr.Add(new DictionaryEntry("", ""));
            arr.Add(new DictionaryEntry("1", "医疗专护"));
            arr.Add(new DictionaryEntry("2", "机构医疗护理"));
            arr.Add(new DictionaryEntry("3", "居家医疗护理"));
            cmbHlzl.DataSource = arr;
            cmbHlzl.ValueMember = "Key";
            cmbHlzl.DisplayMember = "Value";
            cmbHlzl.SelectedIndex = 0;

            ArrayList arr1 = new ArrayList();
            arr1.Add(new DictionaryEntry("", ""));
            arr1.Add(new DictionaryEntry("1", "较大和完全依赖"));
            arr1.Add(new DictionaryEntry("2", "需部分帮助（夹菜、盛饭）"));
            arr1.Add(new DictionaryEntry("3", "全面自理"));
            cmbjspf.DataSource = arr1;
            cmbjspf.ValueMember = "Key";
            cmbjspf.DisplayMember = "Value";
            cmbjspf.SelectedIndex = 0;

            ArrayList arr2 = new ArrayList();
            arr2.Add(new DictionaryEntry("", ""));
            arr2.Add(new DictionaryEntry("1", "依赖"));
            arr2.Add(new DictionaryEntry("2", "自理"));
            //arr2.Add(new DictionaryEntry("3", "居家医疗护理"));
            cmbxzpf.DataSource = arr2;
            cmbxzpf.ValueMember = "Key";
            cmbxzpf.DisplayMember = "Value";
            cmbxzpf.SelectedIndex = 0;

            ArrayList arr3 = new ArrayList();
            arr3.Add(new DictionaryEntry("", ""));
            arr3.Add(new DictionaryEntry("1", "依赖"));
            arr3.Add(new DictionaryEntry("2", "自理（能独立完成洗脸、梳头、刷牙、剃须）"));
            //arr3.Add(new DictionaryEntry("3", "居家医疗护理"));
            cmbsxxspf.DataSource = arr3;
            cmbsxxspf.ValueMember = "Key";
            cmbsxxspf.DisplayMember = "Value";
            cmbsxxspf.SelectedIndex = 0;

            ArrayList arr4 = new ArrayList();
            arr4.Add(new DictionaryEntry("", ""));
            arr4.Add(new DictionaryEntry("1", "依赖"));
            arr4.Add(new DictionaryEntry("2", "需一半帮助"));
            arr4.Add(new DictionaryEntry("3", "自理（系开纽扣、开关拉链和穿鞋）"));
            cmbcypf.DataSource = arr4;
            cmbcypf.ValueMember = "Key";
            cmbcypf.DisplayMember = "Value";
            cmbcypf.SelectedIndex = 0;

            ArrayList arr5 = new ArrayList();
            arr5.Add(new DictionaryEntry("", ""));
            arr5.Add(new DictionaryEntry("1", "昏迷或失禁"));
            arr5.Add(new DictionaryEntry("2", "偶尔失禁（每周﹤1次）"));
            arr5.Add(new DictionaryEntry("3", "能控制"));
            cmbkzdbpf.DataSource = arr5;
            cmbkzdbpf.ValueMember = "Key";
            cmbkzdbpf.DisplayMember = "Value";
            cmbkzdbpf.SelectedIndex = 0;

            ArrayList arr6 = new ArrayList();
            arr6.Add(new DictionaryEntry("", ""));
            arr6.Add(new DictionaryEntry("1", "失禁或昏迷或需他人导尿"));
            arr6.Add(new DictionaryEntry("2", "偶尔失禁（﹤1次/24小时；﹥1次/周）"));
            arr6.Add(new DictionaryEntry("3", "能控制"));
            cmbkzxbpf.DataSource = arr6;
            cmbkzxbpf.ValueMember = "Key";
            cmbkzxbpf.DisplayMember = "Value";
            cmbkzxbpf.SelectedIndex = 0;

            ArrayList arr7 = new ArrayList();
            arr7.Add(new DictionaryEntry("", ""));
            arr7.Add(new DictionaryEntry("1", "依赖"));
            arr7.Add(new DictionaryEntry("2", "需部分帮助"));
            arr7.Add(new DictionaryEntry("3", "自理"));
            cmbrcpf.DataSource = arr7;
            cmbrcpf.ValueMember = "Key";
            cmbrcpf.DisplayMember = "Value";
            cmbrcpf.SelectedIndex = 0;

            ArrayList arr8 = new ArrayList();
            arr8.Add(new DictionaryEntry("", ""));
            arr8.Add(new DictionaryEntry("1", "完全依赖别人"));
            arr8.Add(new DictionaryEntry("2", "需大量帮助（2人），能坐"));
            arr8.Add(new DictionaryEntry("3", "需小量帮助（1人），或监护"));
            arr8.Add(new DictionaryEntry("4", "自理"));
            cmbzyzypf.DataSource = arr8;
            cmbzyzypf.ValueMember = "Key";
            cmbzyzypf.DisplayMember = "Value";
            cmbzyzypf.SelectedIndex = 0;

            ArrayList arr9 = new ArrayList();
            arr9.Add(new DictionaryEntry("", ""));
            arr9.Add(new DictionaryEntry("1", "不能走"));
            arr9.Add(new DictionaryEntry("2", "在轮椅上独立行动"));
            arr9.Add(new DictionaryEntry("3", "需1人帮助（体力或语言督导）"));
            arr9.Add(new DictionaryEntry("4", "独自步行（可用辅助器具）"));
            cmbxxpf.DataSource = arr9;
            cmbxxpf.ValueMember = "Key";
            cmbxxpf.DisplayMember = "Value";
            cmbxxpf.SelectedIndex = 0;

            ArrayList arr10 = new ArrayList();
            arr10.Add(new DictionaryEntry("", ""));
            arr10.Add(new DictionaryEntry("1", "不能"));
            arr10.Add(new DictionaryEntry("2", "需帮助"));
            arr10.Add(new DictionaryEntry("3", "自理"));
            //arr8.Add(new DictionaryEntry("4", "自理"));
            cmbsxltpf.DataSource = arr10;
            cmbsxltpf.ValueMember = "Key";
            cmbsxltpf.DisplayMember = "Value";
            cmbsxltpf.SelectedIndex = 0;


        }

        /// <summary>
        /// 长期护理申请上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            rzybHandle = new RZYBHandle();

            Dictionary<string, string> result = new Dictionary<string, string>();
            string zcm = "211231I-970453-771964-4535";
            string yybm = "51080096";
            

            rzybHandle = new RZYBHandle();
            rzybHandle.InitResolver(sbjgbh, zcm, yybm, usercode); //初始化参数 ：社保机构编号、注册码、医院编码、usercode
            rzybHandle.AddInParas("p_grbh", txtGrbh.Text);         //个人编号
            rzybHandle.AddInParas("p_hlzl", cmbHlzl.SelectedValue.ToString());         //护理种类
            rzybHandle.AddInParas("p_lxdh", txtDh.Text);         // 联系电话
            rzybHandle.AddInParas("p_jspf", cmbjspf.SelectedValue.ToString());         //进食评分
            rzybHandle.AddInParas("p_czpf", cmbxzpf.SelectedValue.ToString());         //洗澡评分
            rzybHandle.AddInParas("p_sxxspf", cmbsxxspf.SelectedValue.ToString());         //梳洗修饰评分
            rzybHandle.AddInParas("p_cypf", cmbcypf.SelectedValue.ToString());         //穿衣评分
            rzybHandle.AddInParas("p_kzdbpf", cmbkzdbpf.SelectedValue.ToString());         //控制大便评分
            rzybHandle.AddInParas("p_kzxbpf", cmbkzxbpf.SelectedValue.ToString());         //控制小便评分
            rzybHandle.AddInParas("p_rcpf", cmbrcpf.SelectedValue.ToString());         //如厕评分
            rzybHandle.AddInParas("p_zyzypf", cmbzyzypf.SelectedValue.ToString());         //床椅转移评分
            rzybHandle.AddInParas("p_xzpf", cmbxxpf.SelectedValue.ToString());         //行走评分
            rzybHandle.AddInParas("p_sxltpf", cmbsxltpf.SelectedValue.ToString());         //上下楼梯评分
            rzybHandle.AddInParas("p_bqmsjzd", richTextBox1.Text);         //病情描述及诊断
            //rzybHandle.AddInParas("p_dmbh", .SelectedValue.ToString());         //

            rzybHandle.Handle("add_cqhl");                       // 传入方法名
            MessageBox.Show("护理长期申请上传完成，请稍后在护理审核查询");
          
        }



       


        /// <summary>
        /// 清除框中数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            txtGrbh.Text = "";
            txtDh.Text = "";
            richTextBox1.Text = "";
        }

        /// <summary>
        /// 关闭页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
