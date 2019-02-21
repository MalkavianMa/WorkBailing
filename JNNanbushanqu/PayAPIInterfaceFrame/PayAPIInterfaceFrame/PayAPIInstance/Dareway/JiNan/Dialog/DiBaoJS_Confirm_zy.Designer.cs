namespace PayAPIInstance.Dareway.JiNan.Dialog
{
    partial class DiBaoJS_Confirm_zy
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
            this.lblTs = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTs
            // 
            this.lblTs.AutoSize = true;
            this.lblTs.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblTs.Location = new System.Drawing.Point(12, 21);
            this.lblTs.Name = "lblTs";
            this.lblTs.Size = new System.Drawing.Size(503, 16);
            this.lblTs.TabIndex = 31;
            this.lblTs.Text = "医保结算完成，病人还需要负担000元，请选择是否进行低保结算?";
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.Location = new System.Drawing.Point(326, 99);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "否";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfirm.Location = new System.Drawing.Point(138, 99);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 30);
            this.btnConfirm.TabIndex = 29;
            this.btnConfirm.TabStop = false;
            this.btnConfirm.Text = "是";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // DiBaoJS_Confirm_zy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 141);
            this.ControlBox = false;
            this.Controls.Add(this.lblTs);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Name = "DiBaoJS_Confirm_zy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DiBaoJS_Confirm_zy";
            this.Load += new System.EventHandler(this.DiBaoJS_Confirm_zy_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTs;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnConfirm;
    }
}