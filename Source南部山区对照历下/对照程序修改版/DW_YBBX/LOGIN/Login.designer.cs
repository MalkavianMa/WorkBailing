namespace DW_YBBX
{
    partial class Login
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmb_Hos = new System.Windows.Forms.ComboBox();
            this.textBox_CzyXm = new System.Windows.Forms.TextBox();
            this.label_CzyXm = new System.Windows.Forms.Label();
            this.text_Password = new System.Windows.Forms.TextBox();
            this.text_UserCode = new System.Windows.Forms.TextBox();
            this.label_Password = new System.Windows.Forms.Label();
            this.label_UserCode = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 35);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(126, 121);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmb_Hos);
            this.groupBox1.Controls.Add(this.textBox_CzyXm);
            this.groupBox1.Controls.Add(this.label_CzyXm);
            this.groupBox1.Controls.Add(this.text_Password);
            this.groupBox1.Controls.Add(this.text_UserCode);
            this.groupBox1.Controls.Add(this.label_Password);
            this.groupBox1.Controls.Add(this.label_UserCode);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(156, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(226, 155);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "操作员信息";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 14);
            this.label1.TabIndex = 7;
            this.label1.Text = "医院";
            // 
            // cmb_Hos
            // 
            this.cmb_Hos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Hos.FormattingEnabled = true;
            this.cmb_Hos.Location = new System.Drawing.Point(72, 54);
            this.cmb_Hos.Name = "cmb_Hos";
            this.cmb_Hos.Size = new System.Drawing.Size(133, 22);
            this.cmb_Hos.TabIndex = 6;
            this.cmb_Hos.TabStop = false;
            // 
            // textBox_CzyXm
            // 
            this.textBox_CzyXm.Location = new System.Drawing.Point(72, 85);
            this.textBox_CzyXm.Name = "textBox_CzyXm";
            this.textBox_CzyXm.ReadOnly = true;
            this.textBox_CzyXm.Size = new System.Drawing.Size(133, 23);
            this.textBox_CzyXm.TabIndex = 5;
            this.textBox_CzyXm.TabStop = false;
            // 
            // label_CzyXm
            // 
            this.label_CzyXm.AutoSize = true;
            this.label_CzyXm.Location = new System.Drawing.Point(17, 89);
            this.label_CzyXm.Name = "label_CzyXm";
            this.label_CzyXm.Size = new System.Drawing.Size(37, 14);
            this.label_CzyXm.TabIndex = 4;
            this.label_CzyXm.Text = "姓名";
            // 
            // text_Password
            // 
            this.text_Password.Location = new System.Drawing.Point(72, 120);
            this.text_Password.Name = "text_Password";
            this.text_Password.PasswordChar = '*';
            this.text_Password.Size = new System.Drawing.Size(133, 23);
            this.text_Password.TabIndex = 3;
            this.text_Password.TabStop = false;
            this.text_Password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.text_Password_KeyDown);
            // 
            // text_UserCode
            // 
            this.text_UserCode.Location = new System.Drawing.Point(72, 20);
            this.text_UserCode.Name = "text_UserCode";
            this.text_UserCode.Size = new System.Drawing.Size(133, 23);
            this.text_UserCode.TabIndex = 2;
            this.text_UserCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.text_UserCode_KeyDown);
            // 
            // label_Password
            // 
            this.label_Password.AutoSize = true;
            this.label_Password.Location = new System.Drawing.Point(17, 124);
            this.label_Password.Name = "label_Password";
            this.label_Password.Size = new System.Drawing.Size(37, 14);
            this.label_Password.TabIndex = 1;
            this.label_Password.Text = "密码";
            // 
            // label_UserCode
            // 
            this.label_UserCode.AutoSize = true;
            this.label_UserCode.Location = new System.Drawing.Point(17, 24);
            this.label_UserCode.Name = "label_UserCode";
            this.label_UserCode.Size = new System.Drawing.Size(37, 14);
            this.label_UserCode.TabIndex = 0;
            this.label_UserCode.Text = "工号";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.ForeColor = System.Drawing.Color.Blue;
            this.button1.Location = new System.Drawing.Point(77, 179);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(79, 36);
            this.button1.TabIndex = 2;
            this.button1.TabStop = false;
            this.button1.Text = "登录";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.ForeColor = System.Drawing.Color.Red;
            this.button2.Location = new System.Drawing.Point(237, 179);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(79, 36);
            this.button2.TabIndex = 3;
            this.button2.TabStop = false;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(394, 224);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "山东众阳医保接口（地纬）";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.Login_HelpButtonClicked);
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox text_Password;
        private System.Windows.Forms.TextBox text_UserCode;
        private System.Windows.Forms.Label label_Password;
        private System.Windows.Forms.Label label_UserCode;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox_CzyXm;
        private System.Windows.Forms.Label label_CzyXm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmb_Hos;

    }
}

