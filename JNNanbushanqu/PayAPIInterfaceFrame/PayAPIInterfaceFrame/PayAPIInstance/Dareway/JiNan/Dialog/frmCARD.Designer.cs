namespace PayAPIInstance.Dareway.JiNan.Dialog
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
            this.cbType = new System.Windows.Forms.ComboBox();
            this.lblzylb = new System.Windows.Forms.Label();
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
            this.groupBox1.Controls.Add(this.cbType);
            this.groupBox1.Controls.Add(this.lblzylb);
            this.groupBox1.Controls.Add(this.txtIDNo);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.rad_wk);
            this.groupBox1.Controls.Add(this.rad_yk);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(439, 139);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "读卡类型";
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold);
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(173, 104);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(239, 22);
            this.cbType.TabIndex = 5;
            this.cbType.Visible = false;
            // 
            // lblzylb
            // 
            this.lblzylb.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblzylb.Location = new System.Drawing.Point(78, 105);
            this.lblzylb.Name = "lblzylb";
            this.lblzylb.Size = new System.Drawing.Size(117, 21);
            this.lblzylb.TabIndex = 3;
            this.lblzylb.Text = "住院类别:";
            this.lblzylb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblzylb.Visible = false;
            // 
            // txtIDNo
            // 
            this.txtIDNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIDNo.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtIDNo.ForeColor = System.Drawing.Color.Blue;
            this.txtIDNo.Location = new System.Drawing.Point(173, 65);
            this.txtIDNo.Name = "txtIDNo";
            this.txtIDNo.ReadOnly = true;
            this.txtIDNo.Size = new System.Drawing.Size(239, 23);
            this.txtIDNo.TabIndex = 0;
            this.txtIDNo.Visible = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(7, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "个人编号（身份证号）:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Visible = false;
            // 
            // rad_wk
            // 
            this.rad_wk.AutoSize = true;
            this.rad_wk.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold);
            this.rad_wk.Location = new System.Drawing.Point(282, 29);
            this.rad_wk.Name = "rad_wk";
            this.rad_wk.Size = new System.Drawing.Size(85, 18);
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
            this.rad_yk.Location = new System.Drawing.Point(48, 29);
            this.rad_yk.Name = "rad_yk";
            this.rad_yk.Size = new System.Drawing.Size(85, 18);
            this.rad_yk.TabIndex = 0;
            this.rad_yk.TabStop = true;
            this.rad_yk.Text = "有卡读取";
            this.rad_yk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.Location = new System.Drawing.Point(294, 157);
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
            this.btnConfirm.Location = new System.Drawing.Point(70, 157);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 30);
            this.btnConfirm.TabIndex = 2;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // frmCARD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(463, 199);
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
        public System.Windows.Forms.Label lblzylb;
        public System.Windows.Forms.ComboBox cbType;
    }
}