namespace PayAPIInstance.Neusoft.Dialogs
{
    partial class FrmReturnInfo_XZ
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvFeeDetail = new System.Windows.Forms.DataGridView();
            this.HIS编码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HIS名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单价 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.金额 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.医保编码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.自理金额 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.自费费用 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.等级 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.自付比例 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFeeDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvFeeDetail
            // 
            this.dgvFeeDetail.AllowUserToAddRows = false;
            this.dgvFeeDetail.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFeeDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvFeeDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFeeDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.HIS编码,
            this.HIS名称,
            this.数量,
            this.单价,
            this.金额,
            this.医保编码,
            this.自理金额,
            this.自费费用,
            this.等级,
            this.自付比例});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFeeDetail.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvFeeDetail.Location = new System.Drawing.Point(-68, 9);
            this.dgvFeeDetail.Name = "dgvFeeDetail";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFeeDetail.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvFeeDetail.RowHeadersWidth = 20;
            this.dgvFeeDetail.RowTemplate.Height = 23;
            this.dgvFeeDetail.Size = new System.Drawing.Size(899, 419);
            this.dgvFeeDetail.TabIndex = 3;
            // 
            // HIS编码
            // 
            this.HIS编码.DataPropertyName = "CHARGE_CODE";
            this.HIS编码.HeaderText = "HIS编码";
            this.HIS编码.Name = "HIS编码";
            this.HIS编码.Width = 60;
            // 
            // HIS名称
            // 
            this.HIS名称.DataPropertyName = "CHARGE_PRICE_NAME";
            this.HIS名称.HeaderText = "HIS名称";
            this.HIS名称.Name = "HIS名称";
            this.HIS名称.Width = 200;
            // 
            // 数量
            // 
            this.数量.DataPropertyName = "QUANTITY";
            this.数量.HeaderText = "数量";
            this.数量.Name = "数量";
            this.数量.Width = 60;
            // 
            // 单价
            // 
            this.单价.DataPropertyName = "PRICE";
            this.单价.HeaderText = "单价";
            this.单价.Name = "单价";
            this.单价.Width = 60;
            // 
            // 金额
            // 
            this.金额.DataPropertyName = "AMOUNT";
            this.金额.HeaderText = "金额";
            this.金额.Name = "金额";
            this.金额.Width = 70;
            // 
            // 医保编码
            // 
            this.医保编码.DataPropertyName = "NETWORK_ITEM_CODE";
            this.医保编码.HeaderText = "医保编码";
            this.医保编码.Name = "医保编码";
            this.医保编码.Width = 80;
            // 
            // 自理金额
            // 
            this.自理金额.DataPropertyName = "AMOUNT_SELF";
            this.自理金额.HeaderText = "自理金额";
            this.自理金额.Name = "自理金额";
            this.自理金额.Width = 80;
            // 
            // 自费费用
            // 
            this.自费费用.DataPropertyName = "AMOUNT_SELF_BURDERN";
            this.自费费用.HeaderText = "自费费用";
            this.自费费用.Name = "自费费用";
            // 
            // 等级
            // 
            this.等级.DataPropertyName = "MEMO_1";
            this.等级.HeaderText = "等级";
            this.等级.Name = "等级";
            this.等级.Width = 60;
            // 
            // 自付比例
            // 
            this.自付比例.DataPropertyName = "MEMO_2";
            this.自付比例.HeaderText = "自付比例";
            this.自付比例.Name = "自付比例";
            this.自付比例.Width = 60;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(288, 455);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(277, 41);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FrmReturnInfo_XZ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 508);
            this.Controls.Add(this.dgvFeeDetail);
            this.Controls.Add(this.btnOK);
            this.Name = "FrmReturnInfo_XZ";
            this.Text = "FrmReturnInfo_XZ";
            ((System.ComponentModel.ISupportInitialize)(this.dgvFeeDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvFeeDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn HIS编码;
        private System.Windows.Forms.DataGridViewTextBoxColumn HIS名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单价;
        private System.Windows.Forms.DataGridViewTextBoxColumn 金额;
        private System.Windows.Forms.DataGridViewTextBoxColumn 医保编码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 自理金额;
        private System.Windows.Forms.DataGridViewTextBoxColumn 自费费用;
        private System.Windows.Forms.DataGridViewTextBoxColumn 等级;
        private System.Windows.Forms.DataGridViewTextBoxColumn 自付比例;
        private System.Windows.Forms.Button btnOK;
    }
}