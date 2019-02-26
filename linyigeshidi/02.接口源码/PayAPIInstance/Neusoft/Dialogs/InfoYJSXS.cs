using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PayAPIResolver.Neusoft;

namespace PayAPIInstance.Neusoft.Dialogs
{
    public partial class InfoYJSXS : Form
    {
        public InfoYJSXS()
        {
            InitializeComponent();
        }
        public DataTable dtTEST;
        private NeusoftResolver neusoftResolver;
        public InfoYJSXS(NeusoftResolver neusoftResolver)
        {
            InitializeComponent();
            this.neusoftResolver = neusoftResolver;
            
        }
        private void InfoYJSXS_Load(object sender, EventArgs e)
        {
            textBox1.Text = neusoftResolver.ListOutParas[47];
            textBox2.Text = neusoftResolver.ListOutParas[51];
            textBox3.Text = neusoftResolver.ListOutParas[294];
            textBox4.Text = neusoftResolver.ListOutParas[50];
            textBox5.Text = neusoftResolver.ListOutParas[53];
            textBox6.Text = neusoftResolver.ListOutParas[47];
            textBox7.Text = neusoftResolver.ListOutParas[287];
            textBox9.Text = neusoftResolver.ListOutParas[49];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmReturnInfo_XZ infoForm = new FrmReturnInfo_XZ(this.dtTEST);
            infoForm.ShowDialog();
        }
    }
}
