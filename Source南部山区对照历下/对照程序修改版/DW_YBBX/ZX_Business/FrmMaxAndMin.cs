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
    public partial class FrmMaxAndMin : Form
    {
        public FrmMaxAndMin()
        {
            InitializeComponent();
        }
        public  string priceType = "";

        private void FrmMaxAndMin_Load(object sender, EventArgs e)
        {

        }

        private void FrmMaxAndMin_FormClosing(object sender, FormClosingEventArgs e)
        {
          //  priceType = "MIN";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            priceType = "MIN";
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            priceType = "MAX";
            this.Close();

        }
    }
}
