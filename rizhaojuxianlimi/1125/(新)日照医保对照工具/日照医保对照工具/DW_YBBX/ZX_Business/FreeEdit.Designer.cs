﻿namespace DW_YBBX.ZX_Business
{
    partial class FreeEdit
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_hisxm = new System.Windows.Forms.TextBox();
            this.btn_His_CX = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(20, 105);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1021, 377);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 38;
            this.label2.Text = "医院项目编码";
            // 
            // txt_hisxm
            // 
            this.txt_hisxm.Location = new System.Drawing.Point(151, 37);
            this.txt_hisxm.Name = "txt_hisxm";
            this.txt_hisxm.Size = new System.Drawing.Size(126, 21);
            this.txt_hisxm.TabIndex = 37;
            // 
            // btn_His_CX
            // 
            this.btn_His_CX.Location = new System.Drawing.Point(318, 33);
            this.btn_His_CX.Name = "btn_His_CX";
            this.btn_His_CX.Size = new System.Drawing.Size(87, 27);
            this.btn_His_CX.TabIndex = 36;
            this.btn_His_CX.Text = "查询";
            this.btn_His_CX.UseVisualStyleBackColor = true;
            this.btn_His_CX.Click += new System.EventHandler(this.btn_His_CX_Click);
            // 
            // FreeEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1102, 526);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_hisxm);
            this.Controls.Add(this.btn_His_CX);
            this.Controls.Add(this.dataGridView1);
            this.Name = "FreeEdit";
            this.Text = "FreeEdit";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_hisxm;
        private System.Windows.Forms.Button btn_His_CX;
    }
}