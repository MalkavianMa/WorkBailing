namespace DW_YBBX.ZX_Business
{
    partial class Frmaddfalg
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
            this.tbxthhisbm = new System.Windows.Forms.TextBox();
            this.tbxthzxbm = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxthzhmc = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbxthhisbm
            // 
            this.tbxthhisbm.Location = new System.Drawing.Point(114, 73);
            this.tbxthhisbm.Name = "tbxthhisbm";
            this.tbxthhisbm.Size = new System.Drawing.Size(100, 21);
            this.tbxthhisbm.TabIndex = 0;
            // 
            // tbxthzxbm
            // 
            this.tbxthzxbm.Location = new System.Drawing.Point(370, 73);
            this.tbxthzxbm.Name = "tbxthzxbm";
            this.tbxthzxbm.Size = new System.Drawing.Size(100, 21);
            this.tbxthzxbm.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "替换的HIS编码";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(280, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "替换的中心码";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "替换的中心名称";
            // 
            // tbxthzhmc
            // 
            this.tbxthzhmc.Location = new System.Drawing.Point(114, 151);
            this.tbxthzhmc.Name = "tbxthzhmc";
            this.tbxthzhmc.Size = new System.Drawing.Size(100, 21);
            this.tbxthzhmc.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(282, 151);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Frmaddfalg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 251);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbxthzhmc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbxthzxbm);
            this.Controls.Add(this.tbxthhisbm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frmaddfalg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "中心";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frmaddfalg_FormClosing);
            this.Load += new System.EventHandler(this.Frmaddfalg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxthhisbm;
        private System.Windows.Forms.TextBox tbxthzxbm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxthzhmc;
        private System.Windows.Forms.Button button1;
    }
}