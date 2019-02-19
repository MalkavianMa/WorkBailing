using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Collections;

namespace DW_YBBX.ZX_Business
{
    public partial class FrmFourYBLY : Form
    {

        public string fglyComtext = "";
        public FrmFourYBLY()
        {
            InitializeComponent();
        }

        private void FrmFourYBLY_Load(object sender, EventArgs e)
        {

            ArrayList arraylist1 = new ArrayList();
            arraylist1.Add(new DictionaryEntry(ConfigurationManager.AppSettings["JMTC_PATID"].ToString(), "居民统筹"));
            arraylist1.Add(new DictionaryEntry(ConfigurationManager.AppSettings["JMMG_PATID"].ToString(), "居民门规"));
            arraylist1.Add(new DictionaryEntry(ConfigurationManager.AppSettings["ZGTC_PATID"].ToString(), "职工统筹"));
            arraylist1.Add(new DictionaryEntry(ConfigurationManager.AppSettings["ZGMG_PATID"].ToString(), "职工门规"));

            this.comboBox1.DataSource = arraylist1;
            this.comboBox1.ValueMember = "Key";
            this.comboBox1.DisplayMember = "Value";
            this.comboBox1.SelectedIndex = 0;
            fglyComtext = comboBox1.SelectedValue.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fglyComtext = comboBox1.SelectedValue.ToString();
            this.Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fglyComtext = comboBox1.SelectedValue.ToString();
        }
    }
}
