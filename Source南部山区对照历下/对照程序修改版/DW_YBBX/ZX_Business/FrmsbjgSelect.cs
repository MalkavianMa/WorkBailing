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
    public partial class FrmsbjgSelect : Form
    {
        public string selSbjgBH = "";
        public string fglyComtext = "";

        public FrmsbjgSelect()
        {
            InitializeComponent();

            //this.radioButton1.Checked = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            //if (radioButton2.Checked)
            //{
            //    radioButton1.Checked = false;
            //    selSbjgBH = ConfigurationManager.AppSettings["sbjgbh_ZGYB"].ToString();
               
               
            //}
           
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //if (radioButton1.Checked)
            //{
            //    radioButton2.Checked = false;
            //    selSbjgBH = ConfigurationManager.AppSettings["sbjgbh_JMYB"].ToString();
                
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fglyComtext = comboBox1.SelectedValue.ToString();
            selSbjgBH = comboBox2.SelectedValue.ToString();

            this.Hide();
        }

        private void FrmsbjgSelect_Load(object sender, EventArgs e)
        {
            ArrayList arraylist1 = new ArrayList();
            arraylist1.Add(new DictionaryEntry(ConfigurationManager.AppSettings["JMTC_PATID"].ToString(), "居民统筹"));
            arraylist1.Add(new DictionaryEntry(ConfigurationManager.AppSettings["JMMG_PATID"].ToString(), "居民门规"));
            arraylist1.Add(new DictionaryEntry(ConfigurationManager.AppSettings["PTMZ_PATID"].ToString(), "普通门诊"));
            arraylist1.Add(new DictionaryEntry(ConfigurationManager.AppSettings["ZGMG_PATID"].ToString(), "职工门规"));
            arraylist1.Add(new DictionaryEntry(ConfigurationManager.AppSettings["MFYY_PATID"].ToString(), "免费药品"));
            arraylist1.Add(new DictionaryEntry(ConfigurationManager.AppSettings["ZGTC_PATID"].ToString(), "职工统筹"));




            this.comboBox1.DataSource = arraylist1;
            this.comboBox1.ValueMember = "Key";
            this.comboBox1.DisplayMember = "Value";
            this.comboBox1.SelectedIndex = 0;

            ArrayList arraylist2 = new ArrayList();
            arraylist2.Add(new DictionaryEntry(ConfigurationManager.AppSettings["sbjgbh_ZGYB"].ToString(), "济南市医保办"));
            arraylist2.Add(new DictionaryEntry(ConfigurationManager.AppSettings["sbjgbh_JMYB"].ToString(), "济南市历城居民医保办"));
            arraylist2.Add(new DictionaryEntry(ConfigurationManager.AppSettings["sbjgbh_JMYBNS"].ToString(), "济南市南山区医保办"));

            this.comboBox2.DataSource = arraylist2;
            this.comboBox2.ValueMember = "Key";
            this.comboBox2.DisplayMember = "Value";
            this.comboBox2.SelectedIndex = 2;

            selSbjgBH = comboBox2.SelectedValue.ToString();
            fglyComtext = comboBox1.SelectedValue.ToString();
        }
    }
}
