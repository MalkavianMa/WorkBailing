using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace PayAPIInstance.Dareway.JNLX.Dialog
{
    public partial class frmSelectPerson_ZY : Form
    {

        public string DWDBStr = "";

        //姓名
        public string patName = "";
        //社保局编码
        public string strSBJBM = "";
        //身份证号
        public string IDNO = "";
        //是否有卡
        //public bool isHaveCard = true;
        /// <summary>
        /// 是否异地:0 无卡，1有卡，2全国异地有卡读取
        /// </summary>
        public int isHaveCard = 0;
        //是否取消
        public bool isCancel = false;
        //是否意外伤害
        public bool isYWSH = false;

        //医疗统筹类别
        public string StrYltclb="";

        public frmSelectPerson_ZY()
        {
            InitializeComponent();
            //initComboxSbjgbh();
            dtView.Visible = false;

        }

        public frmSelectPerson_ZY(string  zysfid)
        {
            InitializeComponent();
            //initComboxSbjgbh();
            this.txtIDNo.Text = zysfid.Trim();
            dtView.Visible = false;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isYF">是否是特殊医保</param>
        public frmSelectPerson_ZY(bool isTS)
        {
            InitializeComponent();
            //initComboxSbjgbh();
            dtView.Visible = false;
            chkIsYWSH.Visible = true;
        }

        /// <summary>
        /// 身份证号change 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtIDNo_TextChanged(object sender, EventArgs e)
        {
            ////if (txtIDNo.Text.Trim().Length > 0)
            ////{
            ////    radioButton2.Focus();
            ////}
        }

        /// <summary>
        /// 加载社保机构
        /// </summary>
        public void initComboxSbjgbh()
        {
         
        }

        private void FillDtgv(string InputCode)
        {
        }

        private void txtIDNo_KeyDown(object sender, KeyEventArgs e)
        {
            int n = -1;
            if (dtView.Rows.Count > 0)
                n = dtView.SelectedRows[0].Index;
            if (e.KeyCode == Keys.Down && n < dtView.Rows.Count - 1)
            {
                dtView.Rows[n + 1].Selected = true;
                dtView.FirstDisplayedScrollingRowIndex = n + 1;
            }
            if (e.KeyCode == Keys.Up && n > 0)
            {
                dtView.Rows[n - 1].Selected = true;
                dtView.FirstDisplayedScrollingRowIndex = n - 1;
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (dtView.SelectedRows.Count > 0)
                {
                    patName = dtView.SelectedRows[0].Cells[0].Value.ToString();
                    IDNO = dtView.SelectedRows[0].Cells[1].Value.ToString();
                    strSBJBM = dtView.SelectedRows[0].Cells[2].Value.ToString();
                    txtName.Text = patName;
                    txtIDNo.Text = IDNO;
                    comSbjgh.SelectedValue = strSBJBM;
                    dtView.Visible = false;
                    btnConfirm.Focus();
                }
            }
        }

         

        private void dtView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dtView_DoubleClick(object sender, EventArgs e)
        {
            patName = dtView.SelectedRows[0].Cells[0].Value.ToString();
            IDNO = dtView.SelectedRows[0].Cells[1].Value.ToString();
            strSBJBM = dtView.SelectedRows[0].Cells[2].Value.ToString();
            txtName.Text = patName;
            txtIDNo.Text = IDNO;
            comSbjgh.SelectedValue = strSBJBM;
            dtView.Visible = false;
            btnConfirm.Focus();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            isCancel =false;
            //isHaveCard = radioButton1.Checked ? true : false;
            //if (!isHaveCard)
            //{
            //    IDNO = txtIDNo.Text;
            //    //strSBJBM = comSbjgh.SelectedValue.ToString();
            //    patName = txtName.Text;
            //    if (IDNO.Trim().Length == 0)
            //    {
            //        MessageBox.Show("身份证号不能为空，请重新检索或输入");
            //        txtIDNo.Focus();
            //        return;
            //    }
            //}:0 无卡，1有卡，2全国异地
            if (radioButton2.Checked)
            {
                isHaveCard = 0;//无卡
            }
            //if (radioButton3.Checked)
            //{
            //    isHaveCard = 1;//有卡
            //}
            if (radioButton4.Checked)
            {
                isHaveCard = 2;//全国异地有卡读取
            }
            if (isHaveCard == 0 || isHaveCard == 1)
            {
                IDNO = txtIDNo.Text;
                //strSBJBM = comSbjgh.SelectedValue.ToString();
                patName = txtName.Text;
                if (IDNO.Trim().Length == 0)
                {
                    MessageBox.Show("身份证号不能为空，请重新检索或输入");
                    txtIDNo.Focus();
                    return;
                }
            }
            StrYltclb = cmbXzlb.SelectedValue.ToString();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isCancel = true;
            this.Close();
           // throw new Exception("用户取消了操作");
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            txtIDNo.SelectAll();
            txtIDNo.Focus();
        }

        private void chkIsYWSH_CheckedChanged(object sender, EventArgs e)
        {
            isYWSH = chkIsYWSH.Checked;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtIDNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == 13)
            {
                if (txtIDNo.Text.Trim().Length > 0)
                {
                    this.btnConfirm.Focus();
                }
               
            }
        }

        /// <summary>
        /// 加载医疗统筹标志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSelectPerson_Load(object sender, EventArgs e)
        {
            ArrayList arr = new ArrayList();

            //医疗 C，工伤 D，生育 E，可调用数据字典接口获取，代码编号：XZBZ
            //险种标识
            arr.Add(new DictionaryEntry("C", "医疗"));
            arr.Add(new DictionaryEntry("D", "工伤"));
            arr.Add(new DictionaryEntry("E", "生育"));            
           // arr.Add(new DictionaryEntry("1", "住院"));
            cmbXzlb.DataSource = arr;
            cmbXzlb.ValueMember = "Key";
            cmbXzlb.DisplayMember = "Value";
            cmbXzlb.SelectedIndex = 0;
        }

    }
}
