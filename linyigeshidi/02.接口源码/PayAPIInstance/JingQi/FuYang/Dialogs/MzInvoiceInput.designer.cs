namespace PayAPIInstance.JingQi.FuYang.Dialogs
{
    partial class MzInvoiceInput
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
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.dgvInvoiceDetail = new System.Windows.Forms.DataGridView();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.医疗卡号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnFysc = new System.Windows.Forms.Button();
            this.tbAmount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbInvoiceCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnquery = new System.Windows.Forms.Button();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvoiceDetail)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.LightBlue;
            this.groupBox6.Controls.Add(this.dgvInvoiceDetail);
            this.groupBox6.Location = new System.Drawing.Point(6, 51);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(824, 293);
            this.groupBox6.TabIndex = 11;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "发票明细";
            // 
            // dgvInvoiceDetail
            // 
            this.dgvInvoiceDetail.AllowUserToAddRows = false;
            this.dgvInvoiceDetail.AllowUserToDeleteRows = false;
            this.dgvInvoiceDetail.AllowUserToResizeRows = false;
            this.dgvInvoiceDetail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvInvoiceDetail.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dgvInvoiceDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvInvoiceDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column10,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column9,
            this.Column5,
            this.Column6,
            this.Column7,
            this.医疗卡号});
            this.dgvInvoiceDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInvoiceDetail.Location = new System.Drawing.Point(3, 17);
            this.dgvInvoiceDetail.MultiSelect = false;
            this.dgvInvoiceDetail.Name = "dgvInvoiceDetail";
            this.dgvInvoiceDetail.ReadOnly = true;
            this.dgvInvoiceDetail.RowHeadersVisible = false;
            this.dgvInvoiceDetail.RowTemplate.Height = 23;
            this.dgvInvoiceDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvInvoiceDetail.Size = new System.Drawing.Size(818, 273);
            this.dgvInvoiceDetail.TabIndex = 1;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "项目名称";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            this.Column10.Width = 78;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "单价";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 54;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "数量";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 54;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "单位";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 54;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "金额";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.Width = 54;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "项目归属";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 78;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "开单科室";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 78;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "开单医生";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 78;
            // 
            // 医疗卡号
            // 
            this.医疗卡号.HeaderText = "记账时间";
            this.医疗卡号.Name = "医疗卡号";
            this.医疗卡号.ReadOnly = true;
            this.医疗卡号.Width = 78;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.LightBlue;
            this.groupBox1.Controls.Add(this.btnquery);
            this.groupBox1.Controls.Add(this.btnFysc);
            this.groupBox1.Controls.Add(this.tbAmount);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbInvoiceCode);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(6, 7);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(824, 46);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "发票信息";
            // 
            // btnFysc
            // 
            this.btnFysc.Location = new System.Drawing.Point(653, 12);
            this.btnFysc.Name = "btnFysc";
            this.btnFysc.Size = new System.Drawing.Size(90, 25);
            this.btnFysc.TabIndex = 4;
            this.btnFysc.Text = "上传";
            this.btnFysc.UseVisualStyleBackColor = true;
            this.btnFysc.Click += new System.EventHandler(this.btnFysc_Click);
            // 
            // tbAmount
            // 
            this.tbAmount.Location = new System.Drawing.Point(502, 14);
            this.tbAmount.Name = "tbAmount";
            this.tbAmount.ReadOnly = true;
            this.tbAmount.Size = new System.Drawing.Size(99, 21);
            this.tbAmount.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(448, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "总金额";
            // 
            // tbInvoiceCode
            // 
            this.tbInvoiceCode.Location = new System.Drawing.Point(160, 15);
            this.tbInvoiceCode.Name = "tbInvoiceCode";
            this.tbInvoiceCode.Size = new System.Drawing.Size(136, 21);
            this.tbInvoiceCode.TabIndex = 1;
            this.tbInvoiceCode.TextChanged += new System.EventHandler(this.tbInvoiceCode_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "请输入发票号:";
            // 
            // btnquery
            // 
            this.btnquery.Location = new System.Drawing.Point(318, 15);
            this.btnquery.Name = "btnquery";
            this.btnquery.Size = new System.Drawing.Size(90, 25);
            this.btnquery.TabIndex = 5;
            this.btnquery.Text = "查询";
            this.btnquery.UseVisualStyleBackColor = true;
            this.btnquery.Click += new System.EventHandler(this.btnquery_Click);
            // 
            // MzInvoiceInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 340);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox1);
            this.Name = "MzInvoiceInput";
            this.Text = "院前检查费用上传";
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvoiceDetail)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.DataGridView dgvInvoiceDetail;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnFysc;
        private System.Windows.Forms.TextBox tbAmount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbInvoiceCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn 医疗卡号;
        private System.Windows.Forms.Button btnquery;
    }
}