namespace RIClientTestDemo
{
    partial class TestDemo
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
            this.txtWebApiAddress = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btn_Init = new System.Windows.Forms.Button();
            this.txtUserSysID = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chkDebugMode = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgrv_SelectName = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DGRV_AUTOID = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOutRefund = new System.Windows.Forms.Button();
            this.txt_QueryContent = new System.Windows.Forms.TextBox();
            this.cmb_QuerySelection = new System.Windows.Forms.ComboBox();
            this.txtOutPatCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txt_OutPatName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbChargeClassId = new System.Windows.Forms.ComboBox();
            this.btnOutSettle = new System.Windows.Forms.Button();
            this.btnReadCard = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnCancelInRegister = new System.Windows.Forms.Button();
            this.btnInRegister = new System.Windows.Forms.Button();
            this.btnInSettle = new System.Windows.Forms.Button();
            this.btnCancelInSettle = new System.Windows.Forms.Button();
            this.btnInPreSettle = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Cmb_In_Times = new System.Windows.Forms.ComboBox();
            this.tbSettleNo = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.tbPatInHosCode = new System.Windows.Forms.TextBox();
            this.txt_PatInChargeClassID = new System.Windows.Forms.TextBox();
            this.txt_pat_in_name = new System.Windows.Forms.TextBox();
            this.ucPayInterfaceTest = new PayAPIClientCom.UCPayInterface();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgrv_SelectName)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGRV_AUTOID)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtWebApiAddress
            // 
            this.txtWebApiAddress.Location = new System.Drawing.Point(339, 181);
            this.txtWebApiAddress.Name = "txtWebApiAddress";
            this.txtWebApiAddress.Size = new System.Drawing.Size(388, 21);
            this.txtWebApiAddress.TabIndex = 31;
            this.txtWebApiAddress.Text = "http://localhost:81/";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(262, 184);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 12);
            this.label11.TabIndex = 30;
            this.label11.Text = "HIS服务地址";
            // 
            // btn_Init
            // 
            this.btn_Init.Location = new System.Drawing.Point(785, 189);
            this.btn_Init.Name = "btn_Init";
            this.btn_Init.Size = new System.Drawing.Size(75, 62);
            this.btn_Init.TabIndex = 32;
            this.btn_Init.Text = "初始化控件";
            this.btn_Init.UseVisualStyleBackColor = true;
            this.btn_Init.Click += new System.EventHandler(this.btn_Init_Click);
            // 
            // txtUserSysID
            // 
            this.txtUserSysID.Location = new System.Drawing.Point(339, 238);
            this.txtUserSysID.Name = "txtUserSysID";
            this.txtUserSysID.Size = new System.Drawing.Size(388, 21);
            this.txtUserSysID.TabIndex = 34;
            this.txtUserSysID.Text = "123";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F);
            this.label9.Location = new System.Drawing.Point(264, 239);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 33;
            this.label9.Text = "系统用户ID";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1165, 413);
            this.tabControl1.TabIndex = 35;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chkDebugMode);
            this.tabPage1.Controls.Add(this.btn_Init);
            this.tabPage1.Controls.Add(this.txtUserSysID);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.txtWebApiAddress);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1157, 388);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "初始化控件";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chkDebugMode
            // 
            this.chkDebugMode.AutoSize = true;
            this.chkDebugMode.Location = new System.Drawing.Point(339, 298);
            this.chkDebugMode.Name = "chkDebugMode";
            this.chkDebugMode.Size = new System.Drawing.Size(96, 16);
            this.chkDebugMode.TabIndex = 36;
            this.chkDebugMode.Text = "是否调试模式";
            this.chkDebugMode.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(801, 465);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 62);
            this.button1.TabIndex = 35;
            this.button1.Text = "测试门诊";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgrv_SelectName);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1157, 388);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "门诊业务";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgrv_SelectName
            // 
            this.dgrv_SelectName.AllowUserToAddRows = false;
            this.dgrv_SelectName.AllowUserToDeleteRows = false;
            this.dgrv_SelectName.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgrv_SelectName.Location = new System.Drawing.Point(1150, 74);
            this.dgrv_SelectName.MultiSelect = false;
            this.dgrv_SelectName.Name = "dgrv_SelectName";
            this.dgrv_SelectName.ReadOnly = true;
            this.dgrv_SelectName.RowTemplate.Height = 23;
            this.dgrv_SelectName.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgrv_SelectName.Size = new System.Drawing.Size(460, 199);
            this.dgrv_SelectName.TabIndex = 37;
            this.dgrv_SelectName.Visible = false;
            this.dgrv_SelectName.DoubleClick += new System.EventHandler(this.dgrv_SelectName_DoubleClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.DGRV_AUTOID);
            this.groupBox2.Location = new System.Drawing.Point(3, 74);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1144, 184);
            this.groupBox2.TabIndex = 38;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "费用明细";
            // 
            // DGRV_AUTOID
            // 
            this.DGRV_AUTOID.AllowUserToAddRows = false;
            this.DGRV_AUTOID.AllowUserToDeleteRows = false;
            this.DGRV_AUTOID.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.DGRV_AUTOID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGRV_AUTOID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGRV_AUTOID.Location = new System.Drawing.Point(3, 17);
            this.DGRV_AUTOID.Name = "DGRV_AUTOID";
            this.DGRV_AUTOID.RowHeadersVisible = false;
            this.DGRV_AUTOID.RowTemplate.Height = 23;
            this.DGRV_AUTOID.Size = new System.Drawing.Size(1138, 164);
            this.DGRV_AUTOID.TabIndex = 22;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnOutRefund);
            this.groupBox1.Controls.Add(this.txt_QueryContent);
            this.groupBox1.Controls.Add(this.cmb_QuerySelection);
            this.groupBox1.Controls.Add(this.txtOutPatCode);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.txt_OutPatName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbChargeClassId);
            this.groupBox1.Controls.Add(this.btnOutSettle);
            this.groupBox1.Controls.Add(this.btnReadCard);
            this.groupBox1.Location = new System.Drawing.Point(3, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1141, 62);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "人员信息";
            // 
            // btnOutRefund
            // 
            this.btnOutRefund.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOutRefund.Location = new System.Drawing.Point(1029, 23);
            this.btnOutRefund.Name = "btnOutRefund";
            this.btnOutRefund.Size = new System.Drawing.Size(80, 23);
            this.btnOutRefund.TabIndex = 32;
            this.btnOutRefund.Text = "门诊退费";
            this.btnOutRefund.UseVisualStyleBackColor = true;
            this.btnOutRefund.Click += new System.EventHandler(this.btnOutRefund_Click);
            // 
            // txt_QueryContent
            // 
            this.txt_QueryContent.Location = new System.Drawing.Point(105, 27);
            this.txt_QueryContent.Name = "txt_QueryContent";
            this.txt_QueryContent.Size = new System.Drawing.Size(126, 21);
            this.txt_QueryContent.TabIndex = 31;
            this.txt_QueryContent.TextChanged += new System.EventHandler(this.txt_QueryContent_TextChanged);
            this.txt_QueryContent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_QueryContent_KeyDown);
            this.txt_QueryContent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_QueryContent_KeyPress);
            // 
            // cmb_QuerySelection
            // 
            this.cmb_QuerySelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_QuerySelection.FormattingEnabled = true;
            this.cmb_QuerySelection.Items.AddRange(new object[] {
            "姓名",
            "就诊号",
            "OUT_PAT_ID"});
            this.cmb_QuerySelection.Location = new System.Drawing.Point(7, 27);
            this.cmb_QuerySelection.Name = "cmb_QuerySelection";
            this.cmb_QuerySelection.Size = new System.Drawing.Size(92, 20);
            this.cmb_QuerySelection.TabIndex = 30;
            // 
            // txtOutPatCode
            // 
            this.txtOutPatCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOutPatCode.Location = new System.Drawing.Point(273, 25);
            this.txtOutPatCode.Name = "txtOutPatCode";
            this.txtOutPatCode.Size = new System.Drawing.Size(136, 21);
            this.txtOutPatCode.TabIndex = 6;
            this.txtOutPatCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtOutPatCode_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(237, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 14);
            this.label1.TabIndex = 5;
            this.label1.Text = "卡号";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("宋体", 10.5F);
            this.label15.Location = new System.Drawing.Point(415, 27);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(35, 14);
            this.label15.TabIndex = 20;
            this.label15.Text = "姓名";
            // 
            // txt_OutPatName
            // 
            this.txt_OutPatName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_OutPatName.Location = new System.Drawing.Point(452, 25);
            this.txt_OutPatName.Name = "txt_OutPatName";
            this.txt_OutPatName.ReadOnly = true;
            this.txt_OutPatName.Size = new System.Drawing.Size(89, 21);
            this.txt_OutPatName.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(547, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 14);
            this.label3.TabIndex = 9;
            this.label3.Text = "费别";
            // 
            // cmbChargeClassId
            // 
            this.cmbChargeClassId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChargeClassId.FormattingEnabled = true;
            this.cmbChargeClassId.Location = new System.Drawing.Point(588, 23);
            this.cmbChargeClassId.Name = "cmbChargeClassId";
            this.cmbChargeClassId.Size = new System.Drawing.Size(106, 20);
            this.cmbChargeClassId.TabIndex = 23;
            // 
            // btnOutSettle
            // 
            this.btnOutSettle.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOutSettle.Location = new System.Drawing.Point(943, 23);
            this.btnOutSettle.Name = "btnOutSettle";
            this.btnOutSettle.Size = new System.Drawing.Size(80, 23);
            this.btnOutSettle.TabIndex = 4;
            this.btnOutSettle.Text = "门诊结算";
            this.btnOutSettle.UseVisualStyleBackColor = true;
            this.btnOutSettle.Click += new System.EventHandler(this.btnOutSettle_Click);
            // 
            // btnReadCard
            // 
            this.btnReadCard.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReadCard.Location = new System.Drawing.Point(853, 23);
            this.btnReadCard.Name = "btnReadCard";
            this.btnReadCard.Size = new System.Drawing.Size(84, 23);
            this.btnReadCard.TabIndex = 2;
            this.btnReadCard.Text = "读卡";
            this.btnReadCard.UseVisualStyleBackColor = true;
            this.btnReadCard.Click += new System.EventHandler(this.btnReadCard_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox7);
            this.tabPage3.Controls.Add(this.groupBox6);
            this.tabPage3.Location = new System.Drawing.Point(4, 21);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1157, 388);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "住院业务";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btnCancelInRegister);
            this.groupBox7.Controls.Add(this.btnInRegister);
            this.groupBox7.Controls.Add(this.btnInSettle);
            this.groupBox7.Controls.Add(this.btnCancelInSettle);
            this.groupBox7.Controls.Add(this.btnInPreSettle);
            this.groupBox7.Location = new System.Drawing.Point(10, 114);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(1144, 260);
            this.groupBox7.TabIndex = 38;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "住院业务";
            // 
            // btnCancelInRegister
            // 
            this.btnCancelInRegister.Location = new System.Drawing.Point(170, 35);
            this.btnCancelInRegister.Name = "btnCancelInRegister";
            this.btnCancelInRegister.Size = new System.Drawing.Size(107, 29);
            this.btnCancelInRegister.TabIndex = 15;
            this.btnCancelInRegister.Text = "取消登记";
            this.btnCancelInRegister.UseVisualStyleBackColor = true;
            this.btnCancelInRegister.Click += new System.EventHandler(this.btnCancelInRegister_Click);
            // 
            // btnInRegister
            // 
            this.btnInRegister.Location = new System.Drawing.Point(18, 35);
            this.btnInRegister.Name = "btnInRegister";
            this.btnInRegister.Size = new System.Drawing.Size(107, 29);
            this.btnInRegister.TabIndex = 14;
            this.btnInRegister.Text = "住院登记";
            this.btnInRegister.UseVisualStyleBackColor = true;
            this.btnInRegister.Click += new System.EventHandler(this.btnInRegister_Click);
            // 
            // btnInSettle
            // 
            this.btnInSettle.Location = new System.Drawing.Point(477, 35);
            this.btnInSettle.Name = "btnInSettle";
            this.btnInSettle.Size = new System.Drawing.Size(107, 29);
            this.btnInSettle.TabIndex = 20;
            this.btnInSettle.Text = "住院结算";
            this.btnInSettle.UseVisualStyleBackColor = true;
            this.btnInSettle.Click += new System.EventHandler(this.btnInSettle_Click);
            // 
            // btnCancelInSettle
            // 
            this.btnCancelInSettle.Location = new System.Drawing.Point(621, 35);
            this.btnCancelInSettle.Name = "btnCancelInSettle";
            this.btnCancelInSettle.Size = new System.Drawing.Size(107, 29);
            this.btnCancelInSettle.TabIndex = 21;
            this.btnCancelInSettle.Text = "取消结算";
            this.btnCancelInSettle.UseVisualStyleBackColor = true;
            this.btnCancelInSettle.Click += new System.EventHandler(this.btnCancelInSettle_Click);
            // 
            // btnInPreSettle
            // 
            this.btnInPreSettle.Location = new System.Drawing.Point(324, 35);
            this.btnInPreSettle.Name = "btnInPreSettle";
            this.btnInPreSettle.Size = new System.Drawing.Size(107, 29);
            this.btnInPreSettle.TabIndex = 22;
            this.btnInPreSettle.Text = "住院预结算";
            this.btnInPreSettle.UseVisualStyleBackColor = true;
            this.btnInPreSettle.Click += new System.EventHandler(this.btnInPreSettle_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.Cmb_In_Times);
            this.groupBox6.Controls.Add(this.tbSettleNo);
            this.groupBox6.Controls.Add(this.label14);
            this.groupBox6.Controls.Add(this.label16);
            this.groupBox6.Controls.Add(this.label17);
            this.groupBox6.Controls.Add(this.tbPatInHosCode);
            this.groupBox6.Controls.Add(this.txt_PatInChargeClassID);
            this.groupBox6.Controls.Add(this.txt_pat_in_name);
            this.groupBox6.Location = new System.Drawing.Point(6, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(1145, 102);
            this.groupBox6.TabIndex = 37;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "个人信息";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 17;
            this.label6.Text = "患者住院号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(816, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 35;
            this.label2.Text = "结算流水号";
            // 
            // Cmb_In_Times
            // 
            this.Cmb_In_Times.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cmb_In_Times.FormattingEnabled = true;
            this.Cmb_In_Times.Location = new System.Drawing.Point(625, 23);
            this.Cmb_In_Times.Name = "Cmb_In_Times";
            this.Cmb_In_Times.Size = new System.Drawing.Size(166, 20);
            this.Cmb_In_Times.TabIndex = 25;
            // 
            // tbSettleNo
            // 
            this.tbSettleNo.Location = new System.Drawing.Point(909, 23);
            this.tbSettleNo.Name = "tbSettleNo";
            this.tbSettleNo.Size = new System.Drawing.Size(163, 21);
            this.tbSettleNo.TabIndex = 34;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(535, 26);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 26;
            this.label14.Text = "住院次数";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(17, 64);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 12);
            this.label16.TabIndex = 28;
            this.label16.Text = "病人姓名";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(300, 26);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(29, 12);
            this.label17.TabIndex = 29;
            this.label17.Text = "费别";
            // 
            // tbPatInHosCode
            // 
            this.tbPatInHosCode.Location = new System.Drawing.Point(97, 22);
            this.tbPatInHosCode.Name = "tbPatInHosCode";
            this.tbPatInHosCode.Size = new System.Drawing.Size(166, 21);
            this.tbPatInHosCode.TabIndex = 19;
            this.tbPatInHosCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbPatInHosCode_KeyDown);
            // 
            // txt_PatInChargeClassID
            // 
            this.txt_PatInChargeClassID.Location = new System.Drawing.Point(353, 22);
            this.txt_PatInChargeClassID.Name = "txt_PatInChargeClassID";
            this.txt_PatInChargeClassID.ReadOnly = true;
            this.txt_PatInChargeClassID.Size = new System.Drawing.Size(166, 21);
            this.txt_PatInChargeClassID.TabIndex = 30;
            // 
            // txt_pat_in_name
            // 
            this.txt_pat_in_name.Location = new System.Drawing.Point(97, 61);
            this.txt_pat_in_name.Name = "txt_pat_in_name";
            this.txt_pat_in_name.ReadOnly = true;
            this.txt_pat_in_name.Size = new System.Drawing.Size(166, 21);
            this.txt_pat_in_name.TabIndex = 27;
            // 
            // ucPayInterfaceTest
            // 
            this.ucPayInterfaceTest.Location = new System.Drawing.Point(527, 12);
            this.ucPayInterfaceTest.Name = "ucPayInterfaceTest";
            this.ucPayInterfaceTest.Size = new System.Drawing.Size(150, 78);
            this.ucPayInterfaceTest.TabIndex = 0;
            // 
            // TestDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1189, 431);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.ucPayInterfaceTest);
            this.Controls.Add(this.button1);
            this.Name = "TestDemo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "测试";
            this.Load += new System.EventHandler(this.TestDemo_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgrv_SelectName)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGRV_AUTOID)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PayAPIClientCom.UCPayInterface ucPayInterfaceTest;
        private System.Windows.Forms.TextBox txtWebApiAddress;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btn_Init;
        private System.Windows.Forms.TextBox txtUserSysID;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnCancelInRegister;
        private System.Windows.Forms.Button btnInRegister;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox Cmb_In_Times;
        private System.Windows.Forms.TextBox tbSettleNo;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox tbPatInHosCode;
        private System.Windows.Forms.TextBox txt_PatInChargeClassID;
        private System.Windows.Forms.TextBox txt_pat_in_name;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txt_QueryContent;
        private System.Windows.Forms.ComboBox cmb_QuerySelection;
        private System.Windows.Forms.TextBox txtOutPatCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txt_OutPatName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbChargeClassId;
        private System.Windows.Forms.Button btnOutSettle;
        private System.Windows.Forms.Button btnReadCard;
        private System.Windows.Forms.DataGridView dgrv_SelectName;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button btnInSettle;
        private System.Windows.Forms.Button btnCancelInSettle;
        private System.Windows.Forms.Button btnInPreSettle;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView DGRV_AUTOID;
        private System.Windows.Forms.Button btnOutRefund;
        private System.Windows.Forms.CheckBox chkDebugMode;
    }
}

