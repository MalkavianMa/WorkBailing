using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PayAPIInterfaceHandle.Tools;

namespace PayAPIInstance.Dareway.DIWEI.Dialog
{
    /// <summary>
    /// 人员选择
    /// </summary>
    public partial class frmSelectPerson : Form
    {

        public string HISDBStr = "Data Source=10.1.50.73;Initial Catalog=COMM;Persist Security Info=True;User ID=power;Password=m@ssunsoft009";

        //姓名
        public string patName = "";
        //社保局编码
        public string strSBJBM = "";
        //身份证号
        public string IDNO = "";
        //是否有卡
        public bool isHaveCard = true;
        //是否取消
        public bool isCancel = false;
        //是否意外伤害
        public bool isYWSH = false;

        public frmSelectPerson()
        {
            InitializeComponent();
            dtView.Visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isYF">是否是特殊医保</param>
        public frmSelectPerson(bool isTS)
        {
            InitializeComponent();

            dtView.Visible = false;

        }

        private void txtIDNo_TextChanged(object sender, EventArgs e)
        {
            if (txtIDNo.Focused == false)
            {
                dtView.Visible = false;
                return;
            }


            if (txtIDNo.Text != "")
            {
                FillDtgv(txtIDNo.Text);
            }
            else
            {
                dtView.Visible = false;
                return;
            }
        }


        private void FillDtgv(string InputCode)
        {
            MSSQLHelper mssqlHelper = new MSSQLHelper(HISDBStr);
            StringBuilder strSql = new StringBuilder();
            DataSet ds = new DataSet();

            strSql.Append(" SELECT A.INPUT_CODE 助记码 ,OUT_PAT_NAME 姓名,A.PAT_IDCARD 身份证号 ,B.COMPANY_NAME 单位名称,OUT_PAT_CODE 就诊卡 FROM COMM.DICT.OUT_PATS A ");
            strSql.Append(" LEFT JOIN COMM.DICT.COMPANIES B ON A.COMPANY_ID=B.COMPANY_ID ");
            strSql.Append(" WHERE CHARGE_CLASS_ID='10016'  ");
            strSql.Append(" AND ( A.INPUT_CODE LIKE '%" + InputCode + "%' OR OUT_PAT_CODE LIKE '%" + InputCode + "%') ");

            ds = mssqlHelper.ExecSqlReDs(strSql.ToString());

            if (ds.Tables[0].Rows.Count > 0)
            {
                dtView.Visible = true;
            }
            else
            {
                dtView.Visible = false;
                return;
            }
            //ds.Tables[0].TableName = "Person";
            //dtView.DataSource = ds;
            
            //dtView.DataMember = "Person";
            dtView.DataSource = ds.Tables[0];
        }

        //private void txtIDNo_KeyDown(object sender, KeyEventArgs e)
        //{
        //    int n = -1;
        //    if (dtView.Rows.Count > 0)
        //        n = dtView.SelectedRows[0].Index;
        //    if (e.KeyCode == Keys.Down && n < dtView.Rows.Count - 1)
        //    {
        //        dtView.Rows[n + 1].Selected = true;
        //        dtView.FirstDisplayedScrollingRowIndex = n + 1;
        //    }
        //    if (e.KeyCode == Keys.Up && n > 0)
        //    {
        //        dtView.Rows[n - 1].Selected = true;
        //        dtView.FirstDisplayedScrollingRowIndex = n - 1;
        //    }
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        if (dtView.SelectedRows.Count > 0)
        //        {
        //            patName = dtView.SelectedRows[0].Cells[1].Value.ToString();
        //            IDNO = dtView.SelectedRows[0].Cells[2].Value.ToString();
        //            txtName.Text = patName;
        //            txtIDNo.Text = IDNO;

        //            dtView.Visible = false;
        //            btnConfirm.Focus();
        //        }
        //    }
        //}
        private void txtIDNo_KeyDown(object sender, KeyEventArgs e)
        {
            //上下箭头移动datagridview当前行

            if (e.KeyCode == Keys.Down)
            {
                System.Windows.Forms.SendKeys.Send("{END}");  //发送end键把光标挪到最后

                if (this.dtView.Visible == true)
                {
                    BindingManagerBase bm = dtView.BindingContext[dtView.DataSource];
                    bm.Position += 1;
                }
            }

            if (e.KeyCode == Keys.Up)
            {
                System.Windows.Forms.SendKeys.Send("{END}");
                if (this.dtView.Visible == true)
                {
                    BindingManagerBase bm = dtView.BindingContext[dtView.DataSource];
                    bm.Position -= 1;
                }
            }
        }
        private void dtView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dtView_DoubleClick(object sender, EventArgs e)
        {
            patName = dtView.SelectedRows[0].Cells[1].Value.ToString();
            IDNO = dtView.SelectedRows[0].Cells[2].Value.ToString();
            //strSBJBM = dtView.SelectedRows[0].Cells[2].Value.ToString();
            txtName.Text = patName;
            txtIDNo.Text = IDNO;

            dtView.Visible = false;
            btnConfirm.Focus();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            isCancel = false;

            IDNO = txtIDNo.Text;

            patName = txtName.Text;
            if (IDNO.Trim().Length == 0)
            {
                MessageBox.Show("身份证号不能为空，请重新检索或输入");
                txtIDNo.Focus();
                return;
            }


            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isCancel = true;
            this.Close();
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            txtIDNo.SelectAll();
            txtIDNo.Focus();
        }

        private void txtIDNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                dtView_DoubleClick(this, e);
            }
        }

        private void frmSelectPerson_Load(object sender, EventArgs e)
        {

        }

 
    }
}
