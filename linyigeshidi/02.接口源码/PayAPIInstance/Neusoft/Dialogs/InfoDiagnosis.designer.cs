namespace PayAPIInstance.Neusoft.Dialogs
{
    partial class InfoDiagnosis
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
            this.label1 = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbDiagnosCode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnComplete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblIdent = new System.Windows.Forms.TextBox();
            this.lblBalance = new System.Windows.Forms.TextBox();
            this.lblType = new System.Windows.Forms.TextBox();
            this.lblTotal = new System.Windows.Forms.TextBox();
            this.lblPersonNo = new System.Windows.Forms.TextBox();
            this.lblICNo = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbOutStatus = new System.Windows.Forms.ComboBox();
            this.tbDignosis2 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tbDiagnosCode2 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tbDignosis1 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tbDiagnosCode1 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tbDignosis = new System.Windows.Forms.TextBox();
            this.dataGv = new System.Windows.Forms.DataGridView();
            this.btnDown = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGv)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "医疗类型:";
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(114, 23);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(156, 24);
            this.cbType.TabIndex = 1;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(297, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "第一诊断:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "诊断编码:";
            // 
            // tbDiagnosCode
            // 
            this.tbDiagnosCode.Location = new System.Drawing.Point(114, 53);
            this.tbDiagnosCode.Name = "tbDiagnosCode";
            this.tbDiagnosCode.ReadOnly = true;
            this.tbDiagnosCode.Size = new System.Drawing.Size(156, 26);
            this.tbDiagnosCode.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(52, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "IC卡号:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(297, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 16);
            this.label5.TabIndex = 7;
            this.label5.Text = "身份证号:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(36, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 16);
            this.label6.TabIndex = 8;
            this.label6.Text = "个人编号:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(68, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 16);
            this.label7.TabIndex = 9;
            this.label7.Text = "姓名:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(297, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 16);
            this.label8.TabIndex = 10;
            this.label8.Text = "人员类别:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 103);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(112, 16);
            this.label9.TabIndex = 11;
            this.label9.Text = "累计报销支付:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(329, 56);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 16);
            this.label10.TabIndex = 12;
            this.label10.Text = "余额:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnComplete
            // 
            this.btnComplete.Font = new System.Drawing.Font("宋体", 12F);
            this.btnComplete.Location = new System.Drawing.Point(164, 327);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(96, 26);
            this.btnComplete.TabIndex = 13;
            this.btnComplete.Text = "确定";
            this.btnComplete.UseVisualStyleBackColor = true;
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("宋体", 12F);
            this.btnCancel.Location = new System.Drawing.Point(338, 327);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 26);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblIdent);
            this.groupBox1.Controls.Add(this.lblBalance);
            this.groupBox1.Controls.Add(this.lblType);
            this.groupBox1.Controls.Add(this.lblTotal);
            this.groupBox1.Controls.Add(this.lblPersonNo);
            this.groupBox1.Controls.Add(this.lblICNo);
            this.groupBox1.Controls.Add(this.lblName);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 12F);
            this.groupBox1.Location = new System.Drawing.Point(19, 185);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(580, 136);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "个人信息";
            // 
            // lblIdent
            // 
            this.lblIdent.Location = new System.Drawing.Point(383, 76);
            this.lblIdent.Name = "lblIdent";
            this.lblIdent.ReadOnly = true;
            this.lblIdent.Size = new System.Drawing.Size(156, 26);
            this.lblIdent.TabIndex = 25;
            // 
            // lblBalance
            // 
            this.lblBalance.ForeColor = System.Drawing.Color.Red;
            this.lblBalance.Location = new System.Drawing.Point(383, 46);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.ReadOnly = true;
            this.lblBalance.Size = new System.Drawing.Size(156, 26);
            this.lblBalance.TabIndex = 24;
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(383, 17);
            this.lblType.Name = "lblType";
            this.lblType.ReadOnly = true;
            this.lblType.Size = new System.Drawing.Size(156, 26);
            this.lblType.TabIndex = 23;
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(117, 105);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.ReadOnly = true;
            this.lblTotal.Size = new System.Drawing.Size(156, 26);
            this.lblTotal.TabIndex = 22;
            // 
            // lblPersonNo
            // 
            this.lblPersonNo.Location = new System.Drawing.Point(117, 76);
            this.lblPersonNo.Name = "lblPersonNo";
            this.lblPersonNo.ReadOnly = true;
            this.lblPersonNo.Size = new System.Drawing.Size(156, 26);
            this.lblPersonNo.TabIndex = 21;
            // 
            // lblICNo
            // 
            this.lblICNo.Location = new System.Drawing.Point(117, 46);
            this.lblICNo.Name = "lblICNo";
            this.lblICNo.ReadOnly = true;
            this.lblICNo.Size = new System.Drawing.Size(156, 26);
            this.lblICNo.TabIndex = 20;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(117, 17);
            this.lblName.Name = "lblName";
            this.lblName.ReadOnly = true;
            this.lblName.Size = new System.Drawing.Size(156, 26);
            this.lblName.TabIndex = 19;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbOutStatus);
            this.groupBox2.Controls.Add(this.tbDignosis2);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.tbDiagnosCode2);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.tbDignosis1);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.tbDiagnosCode1);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.tbDignosis);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cbType);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.tbDiagnosCode);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 12F);
            this.groupBox2.Location = new System.Drawing.Point(19, 24);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(580, 155);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "诊断信息";
            // 
            // cbOutStatus
            // 
            this.cbOutStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOutStatus.FormattingEnabled = true;
            this.cbOutStatus.Location = new System.Drawing.Point(383, 24);
            this.cbOutStatus.Name = "cbOutStatus";
            this.cbOutStatus.Size = new System.Drawing.Size(156, 24);
            this.cbOutStatus.TabIndex = 8;
            // 
            // tbDignosis2
            // 
            this.tbDignosis2.Location = new System.Drawing.Point(383, 120);
            this.tbDignosis2.Name = "tbDignosis2";
            this.tbDignosis2.Size = new System.Drawing.Size(156, 26);
            this.tbDignosis2.TabIndex = 16;
            this.tbDignosis2.TextChanged += new System.EventHandler(this.tbDignosis2_TextChanged);
            this.tbDignosis2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDignosis2_KeyDown);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(297, 130);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(80, 16);
            this.label15.TabIndex = 15;
            this.label15.Text = "第三诊断:";
            // 
            // tbDiagnosCode2
            // 
            this.tbDiagnosCode2.Location = new System.Drawing.Point(114, 120);
            this.tbDiagnosCode2.Name = "tbDiagnosCode2";
            this.tbDiagnosCode2.Size = new System.Drawing.Size(156, 26);
            this.tbDiagnosCode2.TabIndex = 14;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(33, 127);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(80, 16);
            this.label14.TabIndex = 13;
            this.label14.Text = "诊断编码:";
            // 
            // tbDignosis1
            // 
            this.tbDignosis1.Location = new System.Drawing.Point(383, 85);
            this.tbDignosis1.Name = "tbDignosis1";
            this.tbDignosis1.Size = new System.Drawing.Size(156, 26);
            this.tbDignosis1.TabIndex = 12;
            this.tbDignosis1.TextChanged += new System.EventHandler(this.tbDignosis1_TextChanged);
            this.tbDignosis1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDignosis1_KeyDown);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(297, 95);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(80, 16);
            this.label13.TabIndex = 11;
            this.label13.Text = "第二诊断:";
            // 
            // tbDiagnosCode1
            // 
            this.tbDiagnosCode1.Location = new System.Drawing.Point(114, 85);
            this.tbDiagnosCode1.Name = "tbDiagnosCode1";
            this.tbDiagnosCode1.Size = new System.Drawing.Size(156, 26);
            this.tbDiagnosCode1.TabIndex = 17;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(33, 95);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 16);
            this.label12.TabIndex = 9;
            this.label12.Text = "诊断编码:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(297, 31);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 16);
            this.label11.TabIndex = 7;
            this.label11.Text = "出院状态:";
            // 
            // tbDignosis
            // 
            this.tbDignosis.Location = new System.Drawing.Point(383, 53);
            this.tbDignosis.Name = "tbDignosis";
            this.tbDignosis.Size = new System.Drawing.Size(156, 26);
            this.tbDignosis.TabIndex = 6;
            this.tbDignosis.TextChanged += new System.EventHandler(this.tbDignosis_TextChanged);
            this.tbDignosis.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDignosis_KeyDown);
            // 
            // dataGv
            // 
            this.dataGv.AllowUserToAddRows = false;
            this.dataGv.AllowUserToDeleteRows = false;
            this.dataGv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGv.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGv.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGv.ColumnHeadersVisible = false;
            this.dataGv.Location = new System.Drawing.Point(576, 79);
            this.dataGv.MultiSelect = false;
            this.dataGv.Name = "dataGv";
            this.dataGv.ReadOnly = true;
            this.dataGv.RowHeadersVisible = false;
            this.dataGv.RowTemplate.Height = 23;
            this.dataGv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGv.Size = new System.Drawing.Size(248, 203);
            this.dataGv.TabIndex = 7;
            this.dataGv.Visible = false;
            this.dataGv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGv_KeyDown);
            this.dataGv.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGv_MouseDoubleClick);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(503, 327);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(96, 26);
            this.btnDown.TabIndex = 24;
            this.btnDown.Text = "医保信息下载";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // InfoDiagnosis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(611, 356);
            this.ControlBox = false;
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.dataGv);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnComplete);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "InfoDiagnosis";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "个人信息及诊断";
            this.Load += new System.EventHandler(this.InfoDiagnosis_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDiagnosCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnComplete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbDignosis;
        private System.Windows.Forms.DataGridView dataGv;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cbOutStatus;
        private System.Windows.Forms.TextBox lblIdent;
        private System.Windows.Forms.TextBox lblBalance;
        private System.Windows.Forms.TextBox lblType;
        private System.Windows.Forms.TextBox lblTotal;
        private System.Windows.Forms.TextBox lblPersonNo;
        private System.Windows.Forms.TextBox lblICNo;
        private System.Windows.Forms.TextBox lblName;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.TextBox tbDignosis1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbDiagnosCode1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbDignosis2;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tbDiagnosCode2;
        private System.Windows.Forms.Label label14;
    }
}