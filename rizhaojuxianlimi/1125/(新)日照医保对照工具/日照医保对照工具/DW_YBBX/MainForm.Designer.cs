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
            this.项目对照ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.重复数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.自定义修改ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.作废数据添加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.门诊统筹定点签约ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据维护ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.目录对照ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.特殊处理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.门诊业务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.门诊报销ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.门诊报销查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.住院业务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.住院报销ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.报销汇总查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.报销明细查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.权限分配ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据对照ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.项目对照ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.护理申请ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.长期护理申请上传ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.长期护理申请查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.结算信息查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.结算信息查询ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tclContents = new System.Windows.Forms.TabControl();
            this.label_xx = new System.Windows.Forms.Label();
            this.作废数据对应关系汇总ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.权限分配ToolStripMenuItem,
            this.数据对照ToolStripMenuItem,
            this.护理申请ToolStripMenuItem,
            this.结算信息查询ToolStripMenuItem});
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
            this.医保上传ToolStripMenuItem,
            this.项目对照ToolStripMenuItem1,
            this.重复数据ToolStripMenuItem,
            this.自定义修改ToolStripMenuItem,
            this.作废数据添加ToolStripMenuItem,
            this.门诊统筹定点签约ToolStripMenuItem,
            this.作废数据对应关系汇总ToolStripMenuItem});
            this.中心业务ToolStripMenuItem.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.中心业务ToolStripMenuItem.Name = "中心业务ToolStripMenuItem";
            this.中心业务ToolStripMenuItem.Size = new System.Drawing.Size(86, 26);
            this.中心业务ToolStripMenuItem.Text = "中心业务";
            // 
            // 目录下载ToolStripMenuItem
            // 
            this.目录下载ToolStripMenuItem.Name = "目录下载ToolStripMenuItem";
            this.目录下载ToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.目录下载ToolStripMenuItem.Text = "目录下载";
            this.目录下载ToolStripMenuItem.Click += new System.EventHandler(this.Menu_Click);
            // 
            // 医保上传ToolStripMenuItem
            // 
            this.医保上传ToolStripMenuItem.Name = "医保上传ToolStripMenuItem";
            this.医保上传ToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.医保上传ToolStripMenuItem.Text = "医保上传";
            this.医保上传ToolStripMenuItem.Visible = false;
            this.医保上传ToolStripMenuItem.Click += new System.EventHandler(this.Menu_Click);
            // 
            // 项目对照ToolStripMenuItem1
            // 
            this.项目对照ToolStripMenuItem1.Name = "项目对照ToolStripMenuItem1";
            this.项目对照ToolStripMenuItem1.Size = new System.Drawing.Size(240, 26);
            this.项目对照ToolStripMenuItem1.Text = "项目对照";
            this.项目对照ToolStripMenuItem1.Click += new System.EventHandler(this.项目对照ToolStripMenuItem1_Click);
            // 
            // 重复数据ToolStripMenuItem
            // 
            this.重复数据ToolStripMenuItem.Name = "重复数据ToolStripMenuItem";
            this.重复数据ToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.重复数据ToolStripMenuItem.Text = "重复数据";
            this.重复数据ToolStripMenuItem.Click += new System.EventHandler(this.重复数据ToolStripMenuItem_Click);
            // 
            // 自定义修改ToolStripMenuItem
            // 
            this.自定义修改ToolStripMenuItem.Name = "自定义修改ToolStripMenuItem";
            this.自定义修改ToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.自定义修改ToolStripMenuItem.Text = "自定义修改";
            this.自定义修改ToolStripMenuItem.Click += new System.EventHandler(this.自定义修改ToolStripMenuItem_Click);
            // 
            // 作废数据添加ToolStripMenuItem
            // 
            this.作废数据添加ToolStripMenuItem.Name = "作废数据添加ToolStripMenuItem";
            this.作废数据添加ToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.作废数据添加ToolStripMenuItem.Text = "作废数据添加";
            this.作废数据添加ToolStripMenuItem.Click += new System.EventHandler(this.作废数据添加ToolStripMenuItem_Click);
            // 
            // 门诊统筹定点签约ToolStripMenuItem
            // 
            this.门诊统筹定点签约ToolStripMenuItem.Name = "门诊统筹定点签约ToolStripMenuItem";
            this.门诊统筹定点签约ToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.门诊统筹定点签约ToolStripMenuItem.Text = "门诊统筹定点签约";
            this.门诊统筹定点签约ToolStripMenuItem.Click += new System.EventHandler(this.门诊统筹定点签约ToolStripMenuItem_Click);
            // 
            // 数据维护ToolStripMenuItem
            // 
            this.数据维护ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.目录对照ToolStripMenuItem,
            this.特殊处理ToolStripMenuItem});
            this.数据维护ToolStripMenuItem.Name = "数据维护ToolStripMenuItem";
            this.数据维护ToolStripMenuItem.Size = new System.Drawing.Size(86, 26);
            this.数据维护ToolStripMenuItem.Text = "数据维护";
            this.数据维护ToolStripMenuItem.Visible = false;
            // 
            // 目录对照ToolStripMenuItem
            // 
            this.目录对照ToolStripMenuItem.Name = "目录对照ToolStripMenuItem";
            this.目录对照ToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
            this.目录对照ToolStripMenuItem.Text = "目录对照";
            this.目录对照ToolStripMenuItem.Visible = false;
            this.目录对照ToolStripMenuItem.Click += new System.EventHandler(this.Menu_Click);
            // 
            // 特殊处理ToolStripMenuItem
            // 
            this.特殊处理ToolStripMenuItem.Name = "特殊处理ToolStripMenuItem";
            this.特殊处理ToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
            this.特殊处理ToolStripMenuItem.Text = "特殊处理";
            this.特殊处理ToolStripMenuItem.Visible = false;
            this.特殊处理ToolStripMenuItem.Click += new System.EventHandler(this.Menu_Click);
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
            // 数据对照ToolStripMenuItem
            // 
            this.数据对照ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.项目对照ToolStripMenuItem});
            this.数据对照ToolStripMenuItem.Name = "数据对照ToolStripMenuItem";
            this.数据对照ToolStripMenuItem.Size = new System.Drawing.Size(86, 26);
            this.数据对照ToolStripMenuItem.Text = "数据对照";
            // 
            // 项目对照ToolStripMenuItem
            // 
            this.项目对照ToolStripMenuItem.Name = "项目对照ToolStripMenuItem";
            this.项目对照ToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.项目对照ToolStripMenuItem.Text = "项目对照";
            this.项目对照ToolStripMenuItem.Click += new System.EventHandler(this.项目对照ToolStripMenuItem_Click);
            // 
            // 护理申请ToolStripMenuItem
            // 
            this.护理申请ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.长期护理申请上传ToolStripMenuItem,
            this.长期护理申请查询ToolStripMenuItem});
            this.护理申请ToolStripMenuItem.Name = "护理申请ToolStripMenuItem";
            this.护理申请ToolStripMenuItem.Size = new System.Drawing.Size(86, 26);
            this.护理申请ToolStripMenuItem.Text = "护理申请";
            this.护理申请ToolStripMenuItem.Visible = false;
            // 
            // 长期护理申请上传ToolStripMenuItem
            // 
            this.长期护理申请上传ToolStripMenuItem.Name = "长期护理申请上传ToolStripMenuItem";
            this.长期护理申请上传ToolStripMenuItem.Size = new System.Drawing.Size(208, 26);
            this.长期护理申请上传ToolStripMenuItem.Text = "长期护理申请上传";
            this.长期护理申请上传ToolStripMenuItem.Click += new System.EventHandler(this.长期护理申请上传ToolStripMenuItem_Click);
            // 
            // 长期护理申请查询ToolStripMenuItem
            // 
            this.长期护理申请查询ToolStripMenuItem.Name = "长期护理申请查询ToolStripMenuItem";
            this.长期护理申请查询ToolStripMenuItem.Size = new System.Drawing.Size(208, 26);
            this.长期护理申请查询ToolStripMenuItem.Text = "长期护理申请查询";
            this.长期护理申请查询ToolStripMenuItem.Click += new System.EventHandler(this.长期护理申请查询ToolStripMenuItem_Click);
            // 
            // 结算信息查询ToolStripMenuItem
            // 
            this.结算信息查询ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.结算信息查询ToolStripMenuItem1});
            this.结算信息查询ToolStripMenuItem.Name = "结算信息查询ToolStripMenuItem";
            this.结算信息查询ToolStripMenuItem.Size = new System.Drawing.Size(118, 26);
            this.结算信息查询ToolStripMenuItem.Text = "结算信息查询";
            this.结算信息查询ToolStripMenuItem.Click += new System.EventHandler(this.结算信息查询ToolStripMenuItem_Click);
            // 
            // 结算信息查询ToolStripMenuItem1
            // 
            this.结算信息查询ToolStripMenuItem1.Name = "结算信息查询ToolStripMenuItem1";
            this.结算信息查询ToolStripMenuItem1.Size = new System.Drawing.Size(176, 26);
            this.结算信息查询ToolStripMenuItem1.Text = "结算信息查询";
            this.结算信息查询ToolStripMenuItem1.Click += new System.EventHandler(this.结算信息查询ToolStripMenuItem1_Click);
            // 
            // tclContents
            // 
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
            this.tclContents.TabIndex = 0;
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
            // 作废数据对应关系汇总ToolStripMenuItem
            // 
            this.作废数据对应关系汇总ToolStripMenuItem.Name = "作废数据对应关系汇总ToolStripMenuItem";
            this.作废数据对应关系汇总ToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.作废数据对应关系汇总ToolStripMenuItem.Text = "作废数据对应关系汇总";
            this.作废数据对应关系汇总ToolStripMenuItem.Click += new System.EventHandler(this.作废数据对应关系汇总ToolStripMenuItem_Click);
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
        private System.Windows.Forms.ToolStripMenuItem 数据对照ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 项目对照ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 护理申请ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 长期护理申请上传ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 长期护理申请查询ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 结算信息查询ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 结算信息查询ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 项目对照ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 重复数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 自定义修改ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 作废数据添加ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 门诊统筹定点签约ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 作废数据对应关系汇总ToolStripMenuItem;
    }
}

