namespace PayAPIInstance.Dareway.DIWEI.Dialog
{
    partial class frmCARD
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbBxlb = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIDNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rad_wk = new System.Windows.Forms.RadioButton();
            this.rad_yk = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbBxlb);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtIDNo);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.rad_wk);
            this.groupBox1.Controls.Add(this.rad_yk);
            this.groupBox1.Location = new System.Drawing.Point(43, 23);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(411, 162);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "读卡类型";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // cmbBxlb
            // 
            this.cmbBxlb.FormattingEnabled = true;
            this.cmbBxlb.Items.AddRange(new object[] {
            "门诊统筹",
            "门诊门规"});
            this.cmbBxlb.Location = new System.Drawing.Point(146, 123);
            this.cmbBxlb.Name = "cmbBxlb";
            this.cmbBxlb.Size = new System.Drawing.Size(221, 20);
            this.cmbBxlb.TabIndex = 7;
            this.cmbBxlb.Visible = false;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(29, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 21);
            this.label3.TabIndex = 6;
            this.label3.Text = "报销类别";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label3.Visible = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(27, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 14);
            this.label2.TabIndex = 4;
            this.label2.Text = "（无卡需要填）";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // txtIDNo
            // 
            this.txtIDNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIDNo.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtIDNo.ForeColor = System.Drawing.Color.Blue;
            this.txtIDNo.Location = new System.Drawing.Point(146, 75);
            this.txtIDNo.Name = "txtIDNo";
            this.txtIDNo.ReadOnly = true;
            this.txtIDNo.Size = new System.Drawing.Size(221, 23);
            this.txtIDNo.TabIndex = 0;
            this.txtIDNo.TextChanged += new System.EventHandler(this.txtIDNo_TextChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(29, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "社会保障号码";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rad_wk
            // 
            this.rad_wk.AutoSize = true;
            this.rad_wk.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold);
            this.rad_wk.Location = new System.Drawing.Point(239, 20);
            this.rad_wk.Name = "rad_wk";
            this.rad_wk.Size = new System.Drawing.Size(64, 14);
            this.rad_wk.TabIndex = 1;
            this.rad_wk.Text = "无卡读取";
            this.rad_wk.UseVisualStyleBackColor = true;
            this.rad_wk.CheckedChanged += new System.EventHandler(this.rad_wk_CheckedChanged);
            // 
            // rad_yk
            // 
            this.rad_yk.AutoSize = true;
            this.rad_yk.Checked = true;
            this.rad_yk.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold);
            this.rad_yk.Location = new System.Drawing.Point(26, 20);
            this.rad_yk.Name = "rad_yk";
            this.rad_yk.Size = new System.Drawing.Size(64, 14);
            this.rad_yk.TabIndex = 0;
            this.rad_yk.TabStop = true;
            this.rad_yk.Text = "有卡读取";
            this.rad_yk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.Location = new System.Drawing.Point(313, 214);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfirm.Location = new System.Drawing.Point(86, 214);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 30);
            this.btnConfirm.TabIndex = 2;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // frmCARD
            // 
            this.AcceptButton = this.btnConfirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 270);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCARD";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "患者信息读取";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmCARD_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rad_wk;
        private System.Windows.Forms.RadioButton rad_yk;
        private System.Windows.Forms.TextBox txtIDNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbBxlb;
        private System.Windows.Forms.Label label3;
    }
}