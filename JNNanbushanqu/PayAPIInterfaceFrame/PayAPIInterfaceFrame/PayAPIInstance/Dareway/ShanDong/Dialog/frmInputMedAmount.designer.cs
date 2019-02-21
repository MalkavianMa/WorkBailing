namespace PayAPIInstance.Dareway.ShanDong.Dialog
{
    partial class frmInputMedAmount
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
            this.btnConfirm = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtZHZF = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGetAmount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConfirm
            // 
            this.btnConfirm.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfirm.Location = new System.Drawing.Point(151, 200);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 32);
            this.btnConfirm.TabIndex = 0;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtZHZF);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtGetAmount);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtAmount);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(352, 161);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // txtZHZF
            // 
            this.txtZHZF.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtZHZF.ForeColor = System.Drawing.Color.Blue;
            this.txtZHZF.Location = new System.Drawing.Point(156, 107);
            this.txtZHZF.Name = "txtZHZF";
            this.txtZHZF.Size = new System.Drawing.Size(116, 23);
            this.txtZHZF.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(79, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "账户支付";
            // 
            // txtGetAmount
            // 
            this.txtGetAmount.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtGetAmount.ForeColor = System.Drawing.Color.Blue;
            this.txtGetAmount.Location = new System.Drawing.Point(156, 68);
            this.txtGetAmount.Name = "txtGetAmount";
            this.txtGetAmount.Size = new System.Drawing.Size(116, 23);
            this.txtGetAmount.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(109, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "应收";
            // 
            // txtAmount
            // 
            this.txtAmount.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtAmount.ForeColor = System.Drawing.Color.Blue;
            this.txtAmount.Location = new System.Drawing.Point(156, 30);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(116, 23);
            this.txtAmount.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(79, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "费用总额";
            // 
            // frmInputMedAmount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 235);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnConfirm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInputMedAmount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "输入账户支付金额";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtZHZF;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtGetAmount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Label label1;
    }
}