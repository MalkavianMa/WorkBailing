using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PayAPIInstance.Neusoft.Dialogs
{
    public partial class FrmReturnInfo_XZ : Form
    {

        private DataTable dtFeeDetail;
        public FrmReturnInfo_XZ(DataTable dtDetail)
        {
            InitializeComponent();
            dtFeeDetail = dtDetail;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void FrmReturnInfo_XZ_Load(object sender, EventArgs e)
        {
            this.dgvFeeDetail.AutoGenerateColumns = false;
            if (this.dtFeeDetail != null)
            {
                this.dgvFeeDetail.DataSource = this.dtFeeDetail;

                this.dgvFeeDetail.Refresh();
            }
        }

        private void dgvFeeDetail_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < this.dgvFeeDetail.Rows.Count; i++)
            {
                try
                {
                    if (this.dgvFeeDetail.Rows[i].Cells[5].Value.ToString() == "" || this.dgvFeeDetail.Rows[i].Cells[8].Value.ToString() == "3" || this.dgvFeeDetail.Rows[i].Cells[8].Value.ToString() == "5")
                    {
                        this.dgvFeeDetail.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    }

                    if (this.dgvFeeDetail.Rows[i].Cells[8].Value.ToString() == "1")
                    {
                        this.dgvFeeDetail.Rows[i].DefaultCellStyle.BackColor = Color.Blue;
                    }
                    if (this.dgvFeeDetail.Rows[i].Cells[8].Value.ToString() == "2")
                    {
                        this.dgvFeeDetail.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }


        }
    }
}