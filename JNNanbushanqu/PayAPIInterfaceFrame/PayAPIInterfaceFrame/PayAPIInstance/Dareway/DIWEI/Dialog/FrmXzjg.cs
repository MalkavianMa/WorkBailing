using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PayAPIInstance.Dareway.DIWEI.Dialog
{
    public partial class FrmXzjg : Form
    {
        public string ZgBxfs = ""; 
        public FrmXzjg()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (comboBox1.Text== "普通职工门诊")
            {
                ZgBxfs = "普通职工门诊";
            }
            else if (comboBox1.Text == "普通职工统筹")

            {
                ZgBxfs = "普通职工统筹";
            }
             else
            {
                MessageBox.Show("请选择正确的职工报销方式");
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            throw new Exception("操作员取消操作");
        }
    }
}
