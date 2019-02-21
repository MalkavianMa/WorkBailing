namespace PayAPIInstance.Dareway.JNLX.Dialog
{
    partial class frmSelectPerson_ZY
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
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbXzlb = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comSbjgh = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIDNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dtView = new System.Windows.Forms.DataGridView();
            this.chkIsYWSH = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton4);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(36, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(523, 73);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton4.Location = new System.Drawing.Point(269, 31);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(161, 18);
            this.radioButton4.TabIndex = 5;
            this.radioButton4.Text = "有卡读取(全国异地)";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.Visible = false;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton2.Location = new System.Drawing.Point(140, 31);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(115, 18);
            this.radioButton2.TabIndex = 3;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "无卡读取信息";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.Click += new System.EventHandler(this.radioButton2_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton1.Location = new System.Drawing.Point(13, 31);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(115, 18);
            this.radioButton1.TabIndex = 2;
            this.radioButton1.Text = "有卡读取信息";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbXzlb);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.comSbjgh);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtName);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtIDNo);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(36, 81);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(523, 146);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // cmbXzlb
            // 
            this.cmbXzlb.FormattingEnabled = true;
            this.cmbXzlb.Location = new System.Drawing.Point(157, 69);
            this.cmbXzlb.Name = "cmbXzlb";
            this.cmbXzlb.Size = new System.Drawing.Size(221, 20);
            this.cmbXzlb.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(84, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 14);
            this.label4.TabIndex = 6;
            this.label4.Text = "险种标识";
            // 
            // comSbjgh
            // 
            this.comSbjgh.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comSbjgh.ForeColor = System.Drawing.Color.Blue;
            this.comSbjgh.FormattingEnabled = true;
            this.comSbjgh.Location = new System.Drawing.Point(52, 152);
            this.comSbjgh.Name = "comSbjgh";
            this.comSbjgh.Size = new System.Drawing.Size(221, 22);
            this.comSbjgh.TabIndex = 5;
            this.comSbjgh.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(-31, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "社保局编码";
            this.label3.Visible = false;
            // 
            // txtName
            // 
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtName.ForeColor = System.Drawing.Color.Blue;
            this.txtName.Location = new System.Drawing.Point(157, 106);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(221, 23);
            this.txtName.TabIndex = 3;
            this.txtName.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(114, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "姓名";
            this.label2.Visible = false;
            // 
            // txtIDNo
            // 
            this.txtIDNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIDNo.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtIDNo.ForeColor = System.Drawing.Color.Blue;
            this.txtIDNo.Location = new System.Drawing.Point(157, 29);
            this.txtIDNo.Name = "txtIDNo";
            this.txtIDNo.Size = new System.Drawing.Size(221, 23);
            this.txtIDNo.TabIndex = 1;
            this.txtIDNo.TextChanged += new System.EventHandler(this.txtIDNo_TextChanged);
            this.txtIDNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtIDNo_KeyDown);
            this.txtIDNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtIDNo_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(54, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "社会保障号码";
            // 
            // btnConfirm
            // 
            this.btnConfirm.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfirm.Location = new System.Drawing.Point(94, 261);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 30);
            this.btnConfirm.TabIndex = 4;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.Location = new System.Drawing.Point(305, 261);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dtView
            // 
            this.dtView.AllowUserToAddRows = false;
            this.dtView.AllowUserToDeleteRows = false;
            this.dtView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtView.Location = new System.Drawing.Point(399, 233);
            this.dtView.Name = "dtView";
            this.dtView.ReadOnly = true;
            this.dtView.RowTemplate.Height = 23;
            this.dtView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtView.Size = new System.Drawing.Size(377, 155);
            this.dtView.TabIndex = 6;
            this.dtView.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtView_CellContentDoubleClick);
            this.dtView.DoubleClick += new System.EventHandler(this.dtView_DoubleClick);
            // 
            // chkIsYWSH
            // 
            this.chkIsYWSH.AutoSize = true;
            this.chkIsYWSH.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkIsYWSH.Location = new System.Drawing.Point(36, 233);
            this.chkIsYWSH.Name = "chkIsYWSH";
            this.chkIsYWSH.Size = new System.Drawing.Size(129, 20);
            this.chkIsYWSH.TabIndex = 7;
            this.chkIsYWSH.Text = "是否意外伤害";
            this.chkIsYWSH.UseVisualStyleBackColor = true;
            this.chkIsYWSH.Visible = false;
            this.chkIsYWSH.CheckedChanged += new System.EventHandler(this.chkIsYWSH_CheckedChanged);
            // 
            // frmSelectPerson_ZY
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 307);
            this.ControlBox = false;
            this.Controls.Add(this.dtView);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkIsYWSH);
            this.Name = "frmSelectPerson_ZY";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择对话框";
            this.Load += new System.EventHandler(this.frmSelectPerson_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtIDNo;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dtView;
        private System.Windows.Forms.CheckBox chkIsYWSH;
        private System.Windows.Forms.ComboBox comSbjgh;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbXzlb;
        private System.Windows.Forms.RadioButton radioButton4;
    }
}