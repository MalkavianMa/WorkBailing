using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PayAPIInstance.Dareway.ShanDong.Dialog
{
    public partial class frmInputMedAmount : Form
    {
        /// <summary>
        /// 账户支付金额
        /// </summary>
        public decimal MedAmountZhzf = 0;

        /// <summary>
        /// 病人负担金额
        /// </summary>
        private decimal BRFDJE = 0;


        /// <summary>
        /// 
        /// </summary>
        public frmInputMedAmount()
        {
            InitializeComponent();
        }

        public frmInputMedAmount(decimal amount,decimal brfdje)
        {
            InitializeComponent();
            BRFDJE = brfdje;
            txtAmount.Text = amount.ToString();
            txtGetAmount.Text = brfdje.ToString();
            txtZHZF.Text = "0";
            txtZHZF.Focus();
            txtZHZF.SelectAll();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string zhzf = txtZHZF.Text.Trim();

            if (!decimal.TryParse(zhzf, out MedAmountZhzf))
            {
                MessageBox.Show("请输入正确的数字格式，请核对！！！");
                txtZHZF.Focus();
                txtZHZF.SelectAll();
                return;
            }

            if (MedAmountZhzf > BRFDJE)
            {
                MessageBox.Show("输入的金额大于病人负担金额，请核对！！！");
                return;
            }
            this.Close();
        }
    }
}
