using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PayAPIInstance.Dareway.DIWEI.Dialog
{
    public partial class MfyyJblr : Form
    {
        /// <summary>
        /// 免费药物疾病编码
        /// </summary>
        public string MfyyJbbm = "";

        public MfyyJblr()
        {
            InitializeComponent();
        }

        public MfyyJblr(string MfyySmS)
        {
            InitializeComponent();
            richTextBox1.Text = MfyySmS;
        }

        /// <summary>
        /// 确定按钮按钮选择疾病名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text=="糖尿病")
            {
                MfyyJbbm = "E14.901 ,";
            }
            else if (comboBox1.Text == "原发性高血压")
            {
                MfyyJbbm = "I10  11 ,";
            }
            else if (comboBox1.Text == "冠心病")
            {
                MfyyJbbm = "I25.101 ,";
            }
            else
            {
                MessageBox.Show("请选择正确的疾病名称！");
            }
            this.Close();
        }

        /// <summary>
        /// 关闭页面并退出结算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            throw new Exception("操作员取消选择免费药物疾病名称并退出！！");
        }
    }
}
