namespace DW_YBBX
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.中心业务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.目录下载ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.医保上传ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据维护ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.目录对照ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.特殊处理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.自付比例修改ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加项目ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.门诊业务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.门诊报销ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.门诊报销查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.住院业务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.住院报销ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.报销汇总查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.报销明细查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.权限分配ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tclContents = new System.Windows.Forms.TabControl();
            this.label_xx = new System.Windows.Forms.Label();
            this.价格更新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.menuStrip1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.中心业务ToolStripMenuItem,
            this.数据维护ToolStripMenuItem,
            this.门诊业务ToolStripMenuItem,
            this.住院业务ToolStripMenuItem,
            this.权限分配ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1039, 30);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "主菜单";
            // 
            // 中心业务ToolStripMenuItem
            // 
            this.中心业务ToolStripMenuItem.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.中心业务ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.目录下载ToolStripMenuItem,
            this.医保上传ToolStripMenuItem});
            this.中心业务ToolStripMenuItem.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.中心业务ToolStripMenuItem.Name = "中心业务ToolStripMenuItem";
            this.中心业务ToolStripMenuItem.Size = new System.Drawing.Size(86, 26);
            this.中心业务ToolStripMenuItem.Text = "中心业务";
            // 
            // 目录下载ToolStripMenuItem
            // 
            this.目录下载ToolStripMenuItem.Name = "目录下载ToolStripMenuItem";
            this.目录下载ToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
            this.目录下载ToolStripMenuItem.Text = "目录下载";
            this.目录下载ToolStripMenuItem.Click += new System.EventHandler(this.Menu_Click);
            // 
            // 医保上传ToolStripMenuItem
            // 
            this.医保上传ToolStripMenuItem.Name = "医保上传ToolStripMenuItem";
            this.医保上传ToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
            this.医保上传ToolStripMenuItem.Text = "医保上传";
            this.医保上传ToolStripMenuItem.Visible = false;
            this.医保上传ToolStripMenuItem.Click += new System.EventHandler(this.Menu_Click);
            // 
            // 数据维护ToolStripMenuItem
            // 
            this.数据维护ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.目录对照ToolStripMenuItem,
            this.特殊处理ToolStripMenuItem,
            this.自付比例修改ToolStripMenuItem,
            this.添加项目ToolStripMenuItem,
            this.价格更新ToolStripMenuItem});
            this.数据维护ToolStripMenuItem.Name = "数据维护ToolStripMenuItem";
            this.数据维护ToolStripMenuItem.Size = new System.Drawing.Size(86, 26);
            this.数据维护ToolStripMenuItem.Text = "数据维护";
            // 
            // 目录对照ToolStripMenuItem
            // 
            this.目录对照ToolStripMenuItem.Name = "目录对照ToolStripMenuItem";
            this.目录对照ToolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.目录对照ToolStripMenuItem.Text = "目录对照";
            this.目录对照ToolStripMenuItem.Visible = false;
            this.目录对照ToolStripMenuItem.Click += new System.EventHandler(this.Menu_Click);
            // 
            // 特殊处理ToolStripMenuItem
            // 
            this.特殊处理ToolStripMenuItem.Name = "特殊处理ToolStripMenuItem";
            this.特殊处理ToolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.特殊处理ToolStripMenuItem.Text = "特殊处理";
            this.特殊处理ToolStripMenuItem.Visible = false;
            this.特殊处理ToolStripMenuItem.Click += new System.EventHandler(this.Menu_Click);
            // 
            // 自付比例修改ToolStripMenuItem
            // 
            this.自付比例修改ToolStripMenuItem.Name = "自付比例修改ToolStripMenuItem";
            this.自付比例修改ToolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.自付比例修改ToolStripMenuItem.Text = "自付比例修改";
            this.自付比例修改ToolStripMenuItem.Click += new System.EventHandler(this.自付比例修改ToolStripMenuItem_Click);
            // 
            // 添加项目ToolStripMenuItem
            // 
            this.添加项目ToolStripMenuItem.Name = "添加项目ToolStripMenuItem";
            this.添加项目ToolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.添加项目ToolStripMenuItem.Text = "添加项目";
            this.添加项目ToolStripMenuItem.Visible = false;
            this.添加项目ToolStripMenuItem.Click += new System.EventHandler(this.添加项目ToolStripMenuItem_Click);
            // 
            // 门诊业务ToolStripMenuItem
            // 
            this.门诊业务ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.门诊报销ToolStripMenuItem,
            this.门诊报销查询ToolStripMenuItem});
            this.门诊业务ToolStripMenuItem.Name = "门诊业务ToolStripMenuItem";
            this.门诊业务ToolStripMenuItem.Size = new System.Drawing.Size(86, 26);
            this.门诊业务ToolStripMenuItem.Text = "门诊业务";
            this.门诊业务ToolStripMenuItem.Visible = false;
            // 
            // 门诊报销ToolStripMenuItem
            // 
            this.门诊报销ToolStripMenuItem.Name = "门诊报销ToolStripMenuItem";
            this.门诊报销ToolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.门诊报销ToolStripMenuItem.Text = "门诊报销";
            this.门诊报销ToolStripMenuItem.Click += new System.EventHandler(this.Menu_Click);
            // 
            // 门诊报销查询ToolStripMenuItem
            // 
            this.门诊报销查询ToolStripMenuItem.Name = "门诊报销查询ToolStripMenuItem";
            this.门诊报销查询ToolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.门诊报销查询ToolStripMenuItem.Text = "门诊报销查询";
            this.门诊报销查询ToolStripMenuItem.Click += new System.EventHandler(this.Menu_Click);
            // 
            // 住院业务ToolStripMenuItem
            // 
            this.住院业务ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.住院报销ToolStripMenuItem,
            this.报销汇总查询ToolStripMenuItem,
            this.报销明细查询ToolStripMenuItem});
            this.住院业务ToolStripMenuItem.Name = "住院业务ToolStripMenuItem";
            this.住院业务ToolStripMenuItem.Size = new System.Drawing.Size(86, 26);
            this.住院业务ToolStripMenuItem.Text = "住院业务";
            this.住院业务ToolStripMenuItem.Visible = false;
            // 
            // 住院报销ToolStripMenuItem
            // 
            this.住院报销ToolStripMenuItem.Name = "住院报销ToolStripMenuItem";
            this.住院报销ToolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.住院报销ToolStripMenuItem.Text = "住院报销";
            this.住院报销ToolStripMenuItem.Click += new System.EventHandler(this.Menu_Click);
            // 
            // 报销汇总查询ToolStripMenuItem
            // 
            this.报销汇总查询ToolStripMenuItem.Name = "报销汇总查询ToolStripMenuItem";
            this.报销汇总查询ToolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.报销汇总查询ToolStripMenuItem.Text = "报销汇总查询";
            // 
            // 报销明细查询ToolStripMenuItem
            // 
            this.报销明细查询ToolStripMenuItem.Name = "报销明细查询ToolStripMenuItem";
            this.报销明细查询ToolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.报销明细查询ToolStripMenuItem.Text = "报销明细查询";
            // 
            // 权限分配ToolStripMenuItem
            // 
            this.权限分配ToolStripMenuItem.Name = "权限分配ToolStripMenuItem";
            this.权限分配ToolStripMenuItem.Size = new System.Drawing.Size(86, 26);
            this.权限分配ToolStripMenuItem.Text = "权限分配";
            this.权限分配ToolStripMenuItem.Visible = false;
            this.权限分配ToolStripMenuItem.Click += new System.EventHandler(this.Menu_Click);
            // 
            // tclContents
            // 
            this.tclContents.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tclContents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tclContents.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tclContents.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tclContents.ItemSize = new System.Drawing.Size(100, 25);
            this.tclContents.Location = new System.Drawing.Point(0, 30);
            this.tclContents.Margin = new System.Windows.Forms.Padding(10);
            this.tclContents.Multiline = true;
            this.tclContents.Name = "tclContents";
            this.tclContents.Padding = new System.Drawing.Point(10, 10);
            this.tclContents.SelectedIndex = 0;
            this.tclContents.Size = new System.Drawing.Size(1039, 329);
            this.tclContents.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tclContents.TabIndex = 2;
            this.tclContents.Visible = false;
            this.tclContents.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tclContents_DrawItem);
            this.tclContents.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tclContents_MouseDown);
            // 
            // label_xx
            // 
            this.label_xx.AutoSize = true;
            this.label_xx.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_xx.ForeColor = System.Drawing.Color.DarkBlue;
            this.label_xx.Location = new System.Drawing.Point(661, 4);
            this.label_xx.Name = "label_xx";
            this.label_xx.Size = new System.Drawing.Size(54, 19);
            this.label_xx.TabIndex = 3;
            this.label_xx.Text = "shijian";
            // 
            // 价格更新ToolStripMenuItem
            // 
            this.价格更新ToolStripMenuItem.Name = "价格更新ToolStripMenuItem";
            this.价格更新ToolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.价格更新ToolStripMenuItem.Text = "价格更新";
            this.价格更新ToolStripMenuItem.Click += new System.EventHandler(this.价格更新ToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1039, 359);
            this.Controls.Add(this.label_xx);
            this.Controls.Add(this.tclContents);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "山东众阳核三版医保接口（地纬）";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.MainForm_HelpButtonClicked);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 中心业务ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 数据维护ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 门诊业务ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 住院业务ToolStripMenuItem;
        private System.Windows.Forms.TabControl tclContents;
        private System.Windows.Forms.ToolStripMenuItem 目录对照ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 特殊处理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 门诊报销ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 门诊报销查询ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 目录下载ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 住院报销ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 报销汇总查询ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 报销明细查询ToolStripMenuItem;
        private System.Windows.Forms.Label label_xx;
        private System.Windows.Forms.ToolStripMenuItem 权限分配ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 医保上传ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 自付比例修改ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加项目ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 价格更新ToolStripMenuItem;
    }
}

