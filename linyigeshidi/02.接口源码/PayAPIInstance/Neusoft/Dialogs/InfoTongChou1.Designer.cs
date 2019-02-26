namespace PayAPIInstance.Neusoft.Dialogs
{
    partial class InfoTongChou1
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
            this.dGV_ZD = new System.Windows.Forms.DataGridView();
            this.txtAreaName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_ZD)).BeginInit();
            this.SuspendLayout();
            // 
            // dGV_ZD
            // 
            this.dGV_ZD.AllowUserToAddRows = false;
            this.dGV_ZD.AllowUserToDeleteRows = false;
            this.dGV_ZD.AllowUserToOrderColumns = true;
            this.dGV_ZD.AllowUserToResizeRows = false;
            this.dGV_ZD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_ZD.Location = new System.Drawing.Point(57, 71);
            this.dGV_ZD.Name = "dGV_ZD";
            this.dGV_ZD.ReadOnly = true;
            this.dGV_ZD.RowHeadersVisible = false;
            this.dGV_ZD.RowTemplate.Height = 23;
            this.dGV_ZD.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dGV_ZD.Size = new System.Drawing.Size(240, 150);
            this.dGV_ZD.TabIndex = 32;
            this.dGV_ZD.Visible = false;
            // 
            // txtAreaName
            // 
            this.txtAreaName.Location = new System.Drawing.Point(164, 19);
            this.txtAreaName.Name = "txtAreaName";
            this.txtAreaName.Size = new System.Drawing.Size(137, 21);
            this.txtAreaName.TabIndex = 31;
            this.txtAreaName.TextChanged += new System.EventHandler(this.tbAreaName_TextChanged);
            this.txtAreaName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbAreaName_KeyDown);
            this.txtAreaName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbAreaName_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14F);
            this.label1.Location = new System.Drawing.Point(53, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 19);
            this.label1.TabIndex = 29;
            this.label1.Text = "统筹地区:";
            // 
            // btnOk
            // 
            this.btnOk.Font = new System.Drawing.Font("宋体", 12F);
            this.btnOk.Location = new System.Drawing.Point(114, 268);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(125, 28);
            this.btnOk.TabIndex = 30;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // InfoTongChou1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 331);
            this.Controls.Add(this.dGV_ZD);
            this.Controls.Add(this.txtAreaName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOk);
            this.Name = "InfoTongChou1";
            this.Text = "InfoTongChou1";
            ((System.ComponentModel.ISupportInitialize)(this.dGV_ZD)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dGV_ZD;
        private System.Windows.Forms.TextBox txtAreaName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOk;
    }
}