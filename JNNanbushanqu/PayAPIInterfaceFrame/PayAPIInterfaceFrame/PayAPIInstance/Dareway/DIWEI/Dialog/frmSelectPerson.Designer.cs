namespace PayAPIInstance.Dareway.DIWEI.Dialog
{
    partial class frmSelectPerson
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIDNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dtView = new System.Windows.Forms.DataGridView();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtName);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtIDNo);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(35, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(447, 130);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // txtName
            // 
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtName.ForeColor = System.Drawing.Color.Blue;
            this.txtName.Location = new System.Drawing.Point(157, 80);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(221, 23);
            this.txtName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(116, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "姓名";
            // 
            // txtIDNo
            // 
            this.txtIDNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIDNo.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtIDNo.ForeColor = System.Drawing.Color.Blue;
            this.txtIDNo.Location = new System.Drawing.Point(157, 27);
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
            this.label1.Location = new System.Drawing.Point(55, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "社会保障号码";
            // 
            // btnConfirm
            // 
            this.btnConfirm.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfirm.Location = new System.Drawing.Point(113, 260);
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
            this.btnCancel.Location = new System.Drawing.Point(338, 260);
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
            this.dtView.AllowUserToOrderColumns = true;
            this.dtView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dtView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtView.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtView.DefaultCellStyle = dataGridViewCellStyle2;
            this.dtView.Location = new System.Drawing.Point(72, 83);
            this.dtView.Name = "dtView";
            this.dtView.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dtView.RowHeadersVisible = false;
            this.dtView.RowTemplate.Height = 23;
            this.dtView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtView.ShowCellToolTips = false;
            this.dtView.ShowEditingIcon = false;
            this.dtView.Size = new System.Drawing.Size(404, 174);
            this.dtView.TabIndex = 42;
            this.dtView.Visible = false;
            this.dtView.DoubleClick += new System.EventHandler(this.dtView_DoubleClick);
            // 
            // frmSelectPerson
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 302);
            this.ControlBox = false;
            this.Controls.Add(this.dtView);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmSelectPerson";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择对话框";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmSelectPerson_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtIDNo;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dtView;

    }
}