using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PayAPIInstance.Dareway.DIWEI.Dialog
{
    public partial class frmCARD : Form
    {
        public string IDNo = "";//身份证号
        public string iscard = "";//有卡无卡
        public string Bxlb = ""; //报销类别
        public string Bxlbmc = ""; //报销类别名称
        private string quickIDnumber;
        public frmCARD()
        {
            InitializeComponent();
        }

        public frmCARD(string quickIDnumber)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            this.txtIDNo.Text = quickIDnumber.Trim();

        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (rad_wk.Checked == true)
            {
              
                IDNo = txtIDNo.Text.Trim();
                iscard = "0";


            }
            else
            {
                iscard = "1";
             }

            if (cmbBxlb.Text== "门诊统筹")
            {
                Bxlb = "6";
                Bxlbmc = "门诊统筹";
            }
            else if (cmbBxlb.Text == "门诊门规")
            {
                Bxlb = "4";
                Bxlbmc = "门诊门规";
            }
         



            this.Close();
        }

        private void rad_wk_CheckedChanged(object sender, EventArgs e)
        {
            if (rad_wk.Checked == true)
            {
                txtIDNo.ReadOnly = false;
                txtIDNo.Focus();//201703112237xinjia
            }
            else
            {
                txtIDNo.ReadOnly = true;
                txtIDNo.Text = "";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtIDNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmCARD_Load(object sender, EventArgs e)
        {

        }
    }
}
