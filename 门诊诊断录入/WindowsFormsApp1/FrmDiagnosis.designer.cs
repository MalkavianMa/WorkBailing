namespace WindowsFormsApp1
{
    partial class FrmDiagnosis
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
            this.label2 = new System.Windows.Forms.Label();
            this.dataGv = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbDignosis = new System.Windows.Forms.TextBox();
            this.tbDiagnosCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGv)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(20, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "拼音首字母或关键字检索";
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
            this.dataGv.Location = new System.Drawing.Point(156, 112);
            this.dataGv.MultiSelect = false;
            this.dataGv.Name = "dataGv";
            this.dataGv.ReadOnly = true;
            this.dataGv.RowHeadersVisible = false;
            this.dataGv.RowTemplate.Height = 23;
            this.dataGv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGv.Size = new System.Drawing.Size(248, 203);
            this.dataGv.TabIndex = 34;
            this.dataGv.Visible = false;
            this.dataGv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGv_KeyDown);
            this.dataGv.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGv_MouseDoubleClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(22, 178);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 36;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.tbDignosis);
            this.groupBox2.Controls.Add(this.tbDiagnosCode);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 12F);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(392, 94);
            this.groupBox2.TabIndex = 40;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "诊断信息";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(7, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 16);
            this.label3.TabIndex = 44;
            this.label3.Text = "疾病诊断编码";
            // 
            // tbDignosis
            // 
            this.tbDignosis.Location = new System.Drawing.Point(134, 22);
            this.tbDignosis.Name = "tbDignosis";
            this.tbDignosis.Size = new System.Drawing.Size(248, 26);
            this.tbDignosis.TabIndex = 42;
            this.tbDignosis.TextChanged += new System.EventHandler(this.tbDignosis_TextChanged);
            // 
            // tbDiagnosCode
            // 
            this.tbDiagnosCode.BackColor = System.Drawing.SystemColors.Control;
            this.tbDiagnosCode.Location = new System.Drawing.Point(134, 56);
            this.tbDiagnosCode.Name = "tbDiagnosCode";
            this.tbDiagnosCode.ReadOnly = true;
            this.tbDiagnosCode.Size = new System.Drawing.Size(248, 26);
            this.tbDiagnosCode.TabIndex = 43;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(7, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 16);
            this.label1.TabIndex = 41;
            this.label1.Text = "门诊疾病诊断";
            // 
            // FrmDiagnosis
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(548, 321);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGv);
            this.Controls.Add(this.label2);
            this.Name = "FrmDiagnosis";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "门诊疾病诊断录入";
            this.Activated += new System.EventHandler(this.FrmDiagnosis_Activated);
            this.Load += new System.EventHandler(this.FrmDiagnosis_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmDiagnosis_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGv)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGv;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDignosis;
        private System.Windows.Forms.TextBox tbDiagnosCode;
        private System.Windows.Forms.Label label1;
    }
}