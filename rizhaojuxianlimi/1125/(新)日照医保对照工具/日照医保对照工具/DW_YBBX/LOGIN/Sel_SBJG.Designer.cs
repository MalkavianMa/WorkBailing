namespace DW_YBBX.LOGIN
{
    partial class Sel_SBJG
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
            this.rbt_ZG = new System.Windows.Forms.RadioButton();
            this.rbt_JM = new System.Windows.Forms.RadioButton();
            this.btn_ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rbt_ZG
            // 
            this.rbt_ZG.AutoSize = true;
            this.rbt_ZG.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbt_ZG.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rbt_ZG.Location = new System.Drawing.Point(49, 27);
            this.rbt_ZG.Name = "rbt_ZG";
            this.rbt_ZG.Size = new System.Drawing.Size(100, 46);
            this.rbt_ZG.TabIndex = 0;
            this.rbt_ZG.Text = "职工";
            this.rbt_ZG.UseVisualStyleBackColor = true;
            // 
            // rbt_JM
            // 
            this.rbt_JM.AutoSize = true;
            this.rbt_JM.Checked = true;
            this.rbt_JM.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbt_JM.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rbt_JM.Location = new System.Drawing.Point(204, 27);
            this.rbt_JM.Name = "rbt_JM";
            this.rbt_JM.Size = new System.Drawing.Size(100, 46);
            this.rbt_JM.TabIndex = 1;
            this.rbt_JM.TabStop = true;
            this.rbt_JM.Text = "居民";
            this.rbt_JM.UseVisualStyleBackColor = true;
            // 
            // btn_ok
            // 
            this.btn_ok.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_ok.ForeColor = System.Drawing.Color.Blue;
            this.btn_ok.Location = new System.Drawing.Point(111, 104);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(110, 47);
            this.btn_ok.TabIndex = 2;
            this.btn_ok.Text = "确定";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // Sel_SBJG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(340, 180);
            this.ControlBox = false;
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.rbt_JM);
            this.Controls.Add(this.rbt_ZG);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Sel_SBJG";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择社保机构";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbt_ZG;
        private System.Windows.Forms.RadioButton rbt_JM;
        private System.Windows.Forms.Button btn_ok;
    }
}