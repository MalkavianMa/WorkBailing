using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PayAPIInstance.Neusoft
{
    public partial class InfoPreSettle : Form
    {
        private string[] strOutParas;
        private decimal dLastBalance = 0;

        public InfoPreSettle(string[] strparas,decimal lastBalance)
        {
            InitializeComponent();
            this.strOutParas = strparas;
            this.dLastBalance = lastBalance;
        }

        private void InfoPreSettle_Load(object sender, EventArgs e)
        {
            tbAmountTotal.Text = strOutParas[47].Trim();//总费用
            tbAmount.Text = strOutParas[50].Trim();//现金支付
            tbMedAmount.Text = (Convert.ToDecimal(tbAmountTotal.Text) - Convert.ToDecimal(tbAmount.Text)).ToString();//报销总金额
            tbAmountTC.Text = strOutParas[51].Trim();
            tbAmountGWY.Text = strOutParas[53].Trim();
            tbPayAmount.Text = strOutParas[49].Trim();
            tbBalance.Text = strOutParas[149].Trim();
            tbLastBalance.Text = dLastBalance.ToString();
            tbAmountBZ.Text = strOutParas[52].Trim();
            btnClose.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
