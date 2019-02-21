namespace PayAPIInstance.Dareway.JiNan.Dialog
{
    partial class frm_FYSH
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_FYSH));
            this.dGV_FYSH = new System.Windows.Forms.DataGridView();
            this.日期 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.项目名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.类别 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.项目编码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.中心编码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.项目规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单位 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单价 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.金额 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.自负比例 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_CancelUp = new System.Windows.Forms.Button();
            this.btn_YJS = new System.Windows.Forms.Button();
            this.btn_UPload = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.lblts1 = new System.Windows.Forms.Label();
            this.lblts = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_FYSH)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dGV_FYSH
            // 
            this.dGV_FYSH.AllowUserToAddRows = false;
            this.dGV_FYSH.AllowUserToDeleteRows = false;
            this.dGV_FYSH.AllowUserToOrderColumns = true;
            this.dGV_FYSH.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dGV_FYSH.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dGV_FYSH.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_FYSH.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.日期,
            this.项目名称,
            this.类别,
            this.项目编码,
            this.中心编码,
            this.项目规格,
            this.单位,
            this.单价,
            this.数量,
            this.金额,
            this.自负比例});
            this.dGV_FYSH.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dGV_FYSH.DefaultCellStyle = dataGridViewCellStyle6;
            this.dGV_FYSH.Location = new System.Drawing.Point(-2, 0);
            this.dGV_FYSH.Name = "dGV_FYSH";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dGV_FYSH.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dGV_FYSH.RowHeadersVisible = false;
            this.dGV_FYSH.RowTemplate.Height = 23;
            this.dGV_FYSH.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dGV_FYSH.ShowCellToolTips = false;
            this.dGV_FYSH.ShowEditingIcon = false;
            this.dGV_FYSH.Size = new System.Drawing.Size(1173, 397);
            this.dGV_FYSH.TabIndex = 12;
            this.dGV_FYSH.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dGV_FYSH_CellFormatting);
            // 
            // 日期
            // 
            this.日期.DataPropertyName = "CreateTime";
            this.日期.HeaderText = "日期";
            this.日期.Name = "日期";
            this.日期.Width = 130;
            // 
            // 项目名称
            // 
            this.项目名称.DataPropertyName = "ChargeName";
            this.项目名称.HeaderText = "项目名称";
            this.项目名称.Name = "项目名称";
            this.项目名称.ReadOnly = true;
            this.项目名称.Width = 250;
            // 
            // 类别
            // 
            this.类别.DataPropertyName = "Memo4";
            this.类别.HeaderText = "类别";
            this.类别.Name = "类别";
            this.类别.ReadOnly = true;
            this.类别.Width = 60;
            // 
            // 项目编码
            // 
            this.项目编码.DataPropertyName = "ChargeCode";
            this.项目编码.HeaderText = "项目编码";
            this.项目编码.Name = "项目编码";
            this.项目编码.ReadOnly = true;
            // 
            // 中心编码
            // 
            this.中心编码.DataPropertyName = "NetworkItemCode";
            this.中心编码.HeaderText = "中心编码";
            this.中心编码.Name = "中心编码";
            this.中心编码.ReadOnly = true;
            // 
            // 项目规格
            // 
            this.项目规格.DataPropertyName = "Spec";
            this.项目规格.HeaderText = "项目规格";
            this.项目规格.Name = "项目规格";
            this.项目规格.ReadOnly = true;
            this.项目规格.Width = 180;
            // 
            // 单位
            // 
            this.单位.DataPropertyName = "Unit";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.单位.DefaultCellStyle = dataGridViewCellStyle2;
            this.单位.HeaderText = "单位";
            this.单位.Name = "单位";
            this.单位.ReadOnly = true;
            this.单位.Width = 60;
            // 
            // 单价
            // 
            this.单价.DataPropertyName = "Price";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.单价.DefaultCellStyle = dataGridViewCellStyle3;
            this.单价.HeaderText = "单价";
            this.单价.Name = "单价";
            this.单价.ReadOnly = true;
            this.单价.Width = 90;
            // 
            // 数量
            // 
            this.数量.DataPropertyName = "Quantity";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.数量.DefaultCellStyle = dataGridViewCellStyle4;
            this.数量.HeaderText = "数量";
            this.数量.Name = "数量";
            this.数量.ReadOnly = true;
            this.数量.Width = 60;
            // 
            // 金额
            // 
            this.金额.DataPropertyName = "Amount";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = null;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.金额.DefaultCellStyle = dataGridViewCellStyle5;
            this.金额.HeaderText = "金额";
            this.金额.Name = "金额";
            this.金额.ReadOnly = true;
            // 
            // 自负比例
            // 
            this.自负比例.DataPropertyName = "SelfBurdenRatio";
            this.自负比例.HeaderText = "自负比例";
            this.自负比例.Name = "自负比例";
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.btn_CancelUp);
            this.groupBox1.Controls.Add(this.btn_YJS);
            this.groupBox1.Controls.Add(this.btn_UPload);
            this.groupBox1.Controls.Add(this.btn_Cancel);
            this.groupBox1.Controls.Add(this.lblts1);
            this.groupBox1.Controls.Add(this.lblts);
            this.groupBox1.Location = new System.Drawing.Point(-2, 450);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1173, 52);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            // 
            // btn_CancelUp
            // 
            this.btn_CancelUp.Location = new System.Drawing.Point(212, 12);
            this.btn_CancelUp.Name = "btn_CancelUp";
            this.btn_CancelUp.Size = new System.Drawing.Size(106, 34);
            this.btn_CancelUp.TabIndex = 7;
            this.btn_CancelUp.Text = "撤销已上传费用";
            this.btn_CancelUp.UseVisualStyleBackColor = true;
            this.btn_CancelUp.Visible = false;
            this.btn_CancelUp.Click += new System.EventHandler(this.btn_CancelUp_Click);
            // 
            // btn_YJS
            // 
            this.btn_YJS.Location = new System.Drawing.Point(114, 11);
            this.btn_YJS.Name = "btn_YJS";
            this.btn_YJS.Size = new System.Drawing.Size(82, 35);
            this.btn_YJS.TabIndex = 6;
            this.btn_YJS.Text = "结算";
            this.btn_YJS.UseVisualStyleBackColor = true;
            this.btn_YJS.Click += new System.EventHandler(this.btn_YJS_Click);
            // 
            // btn_UPload
            // 
            this.btn_UPload.Location = new System.Drawing.Point(14, 11);
            this.btn_UPload.Name = "btn_UPload";
            this.btn_UPload.Size = new System.Drawing.Size(83, 35);
            this.btn_UPload.TabIndex = 2;
            this.btn_UPload.Text = "上传费用";
            this.btn_UPload.UseVisualStyleBackColor = true;
            this.btn_UPload.Visible = false;
            this.btn_UPload.Click += new System.EventHandler(this.btn_UPload_Solo_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(335, 12);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(82, 35);
            this.btn_Cancel.TabIndex = 5;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // lblts1
            // 
            this.lblts1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblts1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblts1.Location = new System.Drawing.Point(959, 16);
            this.lblts1.Name = "lblts1";
            this.lblts1.Size = new System.Drawing.Size(214, 33);
            this.lblts1.TabIndex = 4;
            this.lblts1.Text = "共 0 条 已处理 0 条";
            this.lblts1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblts1.Visible = false;
            // 
            // lblts
            // 
            this.lblts.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblts.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblts.Location = new System.Drawing.Point(423, 14);
            this.lblts.Name = "lblts";
            this.lblts.Size = new System.Drawing.Size(553, 33);
            this.lblts.TabIndex = 3;
            this.lblts.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frm_FYSH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 604);
            this.Controls.Add(this.dGV_FYSH);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm_FYSH";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "未上传费用审核";
            this.Load += new System.EventHandler(this.frm_FYSH_Load);
            this.Shown += new System.EventHandler(this.frm_FYSH_Shown);
            this.Resize += new System.EventHandler(this.frm_FYSH_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_FYSH)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dGV_FYSH;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_UPload;
        public System.Windows.Forms.Label lblts1;
        public System.Windows.Forms.Label lblts;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button btn_YJS;
        private System.Windows.Forms.Button btn_CancelUp;
        private System.Windows.Forms.DataGridViewTextBoxColumn 日期;
        private System.Windows.Forms.DataGridViewTextBoxColumn 项目名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 类别;
        private System.Windows.Forms.DataGridViewTextBoxColumn 项目编码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 中心编码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 项目规格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单位;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单价;
        private System.Windows.Forms.DataGridViewTextBoxColumn 数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 金额;
        private System.Windows.Forms.DataGridViewTextBoxColumn 自负比例;
    }
}