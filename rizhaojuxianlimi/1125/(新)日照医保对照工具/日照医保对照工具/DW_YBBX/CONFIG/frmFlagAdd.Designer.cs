namespace DW_YBBX.CONFIG
{
    partial class frmFlagAdd
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxyyxmbm = new System.Windows.Forms.TextBox();
            this.tbxyyxmm = new System.Windows.Forms.TextBox();
            this.tbxCenterbm = new System.Windows.Forms.TextBox();
            this.cbotype = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tbxcenterName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "医院项目码";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 155);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "中心项目码";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "医院项目名";
            // 
            // tbxyyxmbm
            // 
            this.tbxyyxmbm.Location = new System.Drawing.Point(107, 6);
            this.tbxyyxmbm.Name = "tbxyyxmbm";
            this.tbxyyxmbm.Size = new System.Drawing.Size(100, 21);
            this.tbxyyxmbm.TabIndex = 3;
            // 
            // tbxyyxmm
            // 
            this.tbxyyxmm.Location = new System.Drawing.Point(107, 51);
            this.tbxyyxmm.Name = "tbxyyxmm";
            this.tbxyyxmm.Size = new System.Drawing.Size(100, 21);
            this.tbxyyxmm.TabIndex = 4;
            // 
            // tbxCenterbm
            // 
            this.tbxCenterbm.Location = new System.Drawing.Point(107, 152);
            this.tbxCenterbm.Name = "tbxCenterbm";
            this.tbxCenterbm.ReadOnly = true;
            this.tbxCenterbm.Size = new System.Drawing.Size(100, 21);
            this.tbxCenterbm.TabIndex = 5;
            // 
            // cbotype
            // 
            this.cbotype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbotype.FormattingEnabled = true;
            this.cbotype.Location = new System.Drawing.Point(86, 100);
            this.cbotype.Name = "cbotype";
            this.cbotype.Size = new System.Drawing.Size(121, 20);
            this.cbotype.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "项目类型";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(26, 242);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "添加";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("楷体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(263, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(317, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "此功能用于费用中已有作废医疗或材料项目的对应关系传输";
            // 
            // tbxcenterName
            // 
            this.tbxcenterName.Location = new System.Drawing.Point(107, 194);
            this.tbxcenterName.Name = "tbxcenterName";
            this.tbxcenterName.ReadOnly = true;
            this.tbxcenterName.Size = new System.Drawing.Size(100, 21);
            this.tbxcenterName.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 197);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "中心项目名称";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(352, 242);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(134, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "删除该作废项目的对应";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmFlagAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 296);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbxcenterName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbotype);
            this.Controls.Add(this.tbxCenterbm);
            this.Controls.Add(this.tbxyyxmm);
            this.Controls.Add(this.tbxyyxmbm);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmFlagAdd";
            this.Text = "作废项目添加";
            this.Load += new System.EventHandler(this.frmFlagAdd_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxyyxmbm;
        private System.Windows.Forms.TextBox tbxyyxmm;
        private System.Windows.Forms.TextBox tbxCenterbm;
        private System.Windows.Forms.ComboBox cbotype;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbxcenterName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button2;
    }
}