namespace PayAPIInstance.Dareway.JiNan.Dialog
{
    partial class MfyyJblr
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbJB = new System.Windows.Forms.ComboBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.dGV_FYSH = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.tbZdbm = new System.Windows.Forms.TextBox();
            this.tbZdmc = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
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
            ((System.ComponentModel.ISupportInitialize)(this.dGV_FYSH)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBox1.Location = new System.Drawing.Point(5, 242);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(1144, 148);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(547, 424);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "医保免费用药疾病：";
            // 
            // cmbJB
            // 
            this.cmbJB.DropDownHeight = 206;
            this.cmbJB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbJB.DropDownWidth = 143;
            this.cmbJB.FormattingEnabled = true;
            this.cmbJB.IntegralHeight = false;
            this.cmbJB.Items.AddRange(new object[] {
            "糖尿病",
            "原发性高血压",
            "冠心病"});
            this.cmbJB.Location = new System.Drawing.Point(674, 419);
            this.cmbJB.Name = "cmbJB";
            this.cmbJB.Size = new System.Drawing.Size(162, 20);
            this.cmbJB.TabIndex = 3;
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(872, 412);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 38);
            this.btn_ok.TabIndex = 4;
            this.btn_ok.Text = "确定";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(1020, 413);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 38);
            this.btn_cancel.TabIndex = 5;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
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
            this.dGV_FYSH.Location = new System.Drawing.Point(5, 9);
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
            this.dGV_FYSH.Size = new System.Drawing.Size(1144, 227);
            this.dGV_FYSH.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(7, 422);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 14);
            this.label2.TabIndex = 14;
            this.label2.Text = "HIS门诊诊断编码：";
            // 
            // tbZdbm
            // 
            this.tbZdbm.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tbZdbm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbZdbm.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbZdbm.ForeColor = System.Drawing.Color.Blue;
            this.tbZdbm.Location = new System.Drawing.Point(126, 418);
            this.tbZdbm.Name = "tbZdbm";
            this.tbZdbm.ReadOnly = true;
            this.tbZdbm.Size = new System.Drawing.Size(87, 23);
            this.tbZdbm.TabIndex = 25;
            // 
            // tbZdmc
            // 
            this.tbZdmc.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tbZdmc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbZdmc.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbZdmc.ForeColor = System.Drawing.Color.Blue;
            this.tbZdmc.Location = new System.Drawing.Point(346, 418);
            this.tbZdmc.Name = "tbZdmc";
            this.tbZdmc.ReadOnly = true;
            this.tbZdmc.Size = new System.Drawing.Size(153, 23);
            this.tbZdmc.TabIndex = 27;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(230, 424);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 14);
            this.label3.TabIndex = 26;
            this.label3.Text = "HIS门诊诊断名称：";
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
            // MfyyJblr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1153, 473);
            this.ControlBox = false;
            this.Controls.Add(this.tbZdmc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbZdbm);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dGV_FYSH);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.cmbJB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox1);
            this.Name = "MfyyJblr";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "免费药物病种选择";
            this.Load += new System.EventHandler(this.MfyyJblr_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_FYSH)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbJB;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.DataGridView dGV_FYSH;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbZdbm;
        private System.Windows.Forms.TextBox tbZdmc;
        private System.Windows.Forms.Label label3;
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