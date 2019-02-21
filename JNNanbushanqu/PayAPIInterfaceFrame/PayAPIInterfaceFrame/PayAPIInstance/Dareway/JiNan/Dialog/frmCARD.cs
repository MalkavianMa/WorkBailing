using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace PayAPIInstance.Dareway.JiNan.Dialog
{
    public partial class frmCARD : Form
    {
        public string IDNo = "";//身份证号
        public string iscard = "";//有卡无卡
        public frmCARD()
        {
            InitializeComponent();
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
            this.Close();
        }

        private void rad_wk_CheckedChanged(object sender, EventArgs e)
        {
            if (rad_wk.Checked == true)
            {
                txtIDNo.ReadOnly = false;
                txtIDNo.Visible = true;
                label1.Visible = true;
                txtIDNo.Focus();
            }
            else
            {
                txtIDNo.ReadOnly = true;
                txtIDNo.Visible = false;
                label1.Visible = false;
                txtIDNo.Text = "";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCARD_Load(object sender, EventArgs e)
        {
            //填充医保类别
            ArrayList al = new ArrayList();
            al.Add(new DictionaryEntry("1", "住院"));
            al.Add(new DictionaryEntry("2", "家床"));

            cbType.ValueMember = "Key";
            cbType.DisplayMember = "Value";
            cbType.DataSource = al;
            cbType.SelectedIndex = 0;
        }
    }
}
