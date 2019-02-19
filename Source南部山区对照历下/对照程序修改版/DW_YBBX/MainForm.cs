using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace DW_YBBX
{
    public partial class MainForm : Form
    {
        public MSSQLHelpers SQLHelper = new MSSQLHelpers(ConfigurationManager.ConnectionStrings["connStr"].ToString());
        public const int CLOSE_SIZE = 13;
        public static string UserCode; //操作人工号
        public static string UserName; //操作人姓名
        public static string HOSPITAL_ID; //HOSPITAL_ID
        public static string HOSPITAL_NAME; //HOSPITAL_NAME
        public static string sbjgbh; //参保人所属的社保机构编号
        public static string yybm; //医院编码（医院在医保中心的编码）
        public static string UserCode_DW;//地纬的用户名
        public static string Password_DW;//地纬的密码
        public static string OPERATOR_ID;
        public static string NETWORKING_PAT_CLASS_ID; //NETWORKING_PAT_CLASS_ID
        string BXLX;
        #region 初始化
        public MainForm(string _UserCode, string _UserName, string _HOSPITAL_ID, string _HOSPITAL_NAME, string _sbjgbh, string _yybm, string _UserCode_DW, string _Password_DW, string networkPatclassID)
        {
         
            StringBuilder strSql = new StringBuilder();

            strSql.Append(" SELECT TOP 1 a.USER_SYS_ID,HOSPITAL_ID FROM COMM.COMM.USERINFO_VIEW a ");
            strSql.Append(" LEFT JOIN COMM.COMM.ROLES_VS_SYS b ON b.ROLE_ID = a.ROLE_ID ");
            strSql.Append(" WHERE b.SYS_ID='1' AND a.FLAG_SYSINVALID=0 ");
            strSql.Append(" AND USER_CODE= '" + _UserCode + "'  ");
            DataTable oper = SQLHelper.ExecSqlReDs(strSql.ToString()).Tables[0];
            OPERATOR_ID = oper.Rows[0]["USER_SYS_ID"].ToString();

            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append(" SELECT TOP 1 NETWORKING_PAT_CLASS_ID FROM COMM.DICT.NETWORKING_PAT_CLASS ");
            oper = SQLHelper.ExecSqlReDs(strSql2.ToString()).Tables[0];
           // NETWORKING_PAT_CLASS_ID = oper.Rows[0]["NETWORKING_PAT_CLASS_ID"].ToString();
            //networkPatclassID
            NETWORKING_PAT_CLASS_ID = networkPatclassID;

            HOSPITAL_ID = _HOSPITAL_ID;
            UserName = _UserName;
            
            HOSPITAL_NAME = _HOSPITAL_NAME;
            sbjgbh = _sbjgbh;
            yybm = _yybm;
            UserCode_DW = _UserCode_DW;
            Password_DW = _Password_DW;
            InitializeComponent();
        }
        #endregion
        #region 增加标签页
        private void Menu_Click(object sender, EventArgs e)
        {
            AddTabPage(((ToolStripDropDownItem)sender).Text.Trim());
        }
        #endregion
        #region 添加选项卡，动态的将调用的FORM添加到选项卡中的一个函数
        /// <summary>
        /// 添加选项卡，动态的将调用的FORM添加到选项卡中的一个函数
        /// </summary>
        /// <param name="strType"></param>
        private void AddTabPage(string strType)
        {
            this.tclContents.Visible = true;
            try
            {
                bool IsOpened = false;

                foreach (TabPage tp in tclContents.TabPages)
                {
                    if (tp.Text.Trim() == strType)
                    {
                        tclContents.SelectedTab = tp;

                        IsOpened = true;
                        break;
                    }
                }
                if (!IsOpened)
                {

                    TabPage tpage = new TabPage(strType);
                    tpage.ImageIndex = 0;
                    tclContents.TabPages.Add(tpage);
                    tclContents.SelectedTab = tpage;
                    tpage.Text = tpage.Text + "  ";

                    Panel p = new Panel();
                    p.Parent = tpage;
                    p.Dock = DockStyle.Fill; //这一句代码是为了将form填充满选项卡，不然会有部分控件位置不好看
                    Form frm = GetForm(strType);
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.TopLevel = false;
                    frm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                    frm.Parent = p;
                    p.Controls.Add(frm);
                    frm.Show();

                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("打开失败！ERR:" + ex.Message);
            }
        }
        #endregion
        #region 判断调用哪个FORM来进行显示
        /// <summary>
        /// 此函数是为了判断调用哪个FORM来进行显示，可以一次类推。
        /// </summary>
        /// <param name="strType"></param>
        /// <returns></returns>
        private Form GetForm(string strType)
        {
            Form frm = null;
            switch (strType)
            {
                case "目录下载":
                    frm = new ZX_Business.DownLoad();
                    break;
            }
            return frm;
        }
        #endregion
   
        #region 关闭标签事件
        /// <summary>
        /// 关闭标签事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tclContents_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                int x = e.X, y = e.Y;

                //计算关闭区域   
                Rectangle myTabRect = this.tclContents.GetTabRect(this.tclContents.SelectedIndex);

                myTabRect.Offset(myTabRect.Width - (CLOSE_SIZE + 3), 2);
                myTabRect.Width = CLOSE_SIZE;
                myTabRect.Height = CLOSE_SIZE;

                //如果鼠标在区域内就关闭选项卡   
                bool isClose = x > myTabRect.X && x < myTabRect.Right
                 && y > myTabRect.Y && y < myTabRect.Bottom;

                if (isClose == true)
                {
                    this.tclContents.TabPages.Remove(this.tclContents.SelectedTab);
                }
            }
        }
        #endregion
        #region 右上角帮助信息显示版本号
        /// <summary>
        /// 右上角帮助信息显示版本号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            AboutBox frm = new AboutBox();
            frm.ShowDialog();
        }
        #endregion
        #region 默认加载判断，根据权限显示界面
        /// <summary>
        /// 默认加载判断，根据权限显示界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text = "欢迎登录" + HOSPITAL_NAME + "医保报销接口系统！" ;
           // this.label_xx.Text = "当前报销类型：" + BXLX + "  操作员：" + UserName;
            this.label_xx.Text = "  操作员：" + UserName;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            目录下载ToolStripMenuItem.Visible = true;
            目录下载ToolStripMenuItem.Enabled = true;


        //    if (UserName != "管理人员")
        //    {
        //        #region 非管理员初始所有菜单不可见
        //        ToolStripMenuItem mnuItem;
        //        ToolStripMenuItem refMenuItem = new ToolStripMenuItem();
        //        foreach (ToolStripMenuItem ctrl in menuStrip1.Items)
        //        {
        //            ctrl.Visible = false;
        //            foreach (object subItem in ctrl.DropDownItems)
        //            {
        //                if (subItem.GetType() == refMenuItem.GetType())
        //                {
        //                    mnuItem = (ToolStripMenuItem)subItem;
        //                    mnuItem.Visible = false;
        //                }
        //            }
        //        }
        //        #endregion
        //        #region 根据分配的菜单显示菜单
        //        StringBuilder userinfo = new StringBuilder();
        //        userinfo.Append(" SELECT a.POPEDOM_NAME 菜单名 FROM YB.DBO.POPEDOM_ITEMS a ");
        //        userinfo.Append(" LEFT JOIN YB.dbo.User_Power c ON c.POPEDOM_ITEM_ID = a.POPEDOM_ITEM_ID AND c.User_Code='" + UserCode + "' ");
        //        userinfo.Append(" WHERE POPEDOM_NAME!='权限分配' AND c.User_Code='" + UserCode + "' ");
        //        DataTable Oper_Lev_info = SQLHelper.ExecSqlReDs(userinfo.ToString()).Tables[0];

        //        //遍历主菜单 
        //        foreach (ToolStripMenuItem ctrl in menuStrip1.Items)
        //        {
        //            foreach (DataRow dr in Oper_Lev_info.Rows)
        //            {
        //                if (ctrl.Text == dr[0].ToString().Trim())
        //                {
        //                    ctrl.Visible = true;
        //                    ctrl.Enabled = true;
                            
        //                    break;
        //                }
        //            }
        //            //遍历子菜单 
        //            foreach (object subItem in ctrl.DropDownItems)
        //            {
        //                foreach (DataRow dr in Oper_Lev_info.Rows)
        //                {
        //                    if (subItem.GetType() == refMenuItem.GetType())
        //                    {
        //                        mnuItem = (ToolStripMenuItem)subItem;
        //                        if (mnuItem.Text == dr[0].ToString().Trim())
        //                        {
        //                            mnuItem.Visible = true;
        //                            mnuItem.Enabled = true;
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        #endregion
        //    }
        }
        #endregion

        private void 自付比例修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZX_Business.Xgzfbl zfbl = new ZX_Business.Xgzfbl();
            zfbl.ShowDialog();
        }

        private void 添加项目ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZX_Business.AddHisCode add = new ZX_Business.AddHisCode();
            add.ShowDialog();
        }

        private void tclContents_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                Rectangle myTabRect = this.tclContents.GetTabRect(e.Index);
                //先添加TabPage属性   
                Font MyFont1 = new Font("宋体", 10, FontStyle.Bold); //新增字体格式

                e.Graphics.DrawString(this.tclContents.TabPages[e.Index].Text, MyFont1, SystemBrushes.ControlText, myTabRect.X + 2, myTabRect.Y + 2); //this.Font 

                //再画一个矩形框
                using (Pen p = new Pen(Color.Black))
                {
                    myTabRect.Offset(myTabRect.Width - (CLOSE_SIZE + 2), 2);
                    //myTabRect.Offset(myTabRect.Width - (CLOSE_SIZE + 3), 2);  //备份
                    myTabRect.Width = CLOSE_SIZE;
                    myTabRect.Height = CLOSE_SIZE;
                    e.Graphics.DrawRectangle(p, myTabRect);
                }

                //填充矩形框
                Color recColor = e.State == DrawItemState.Selected ? Color.DarkRed : Color.DarkGray;
                using (Brush b = new SolidBrush(recColor))
                {
                    e.Graphics.FillRectangle(b, myTabRect);
                }

                //画关闭符号
                using (Pen p = new Pen(Color.White))
                {
                    //画"/"线
                    Point p1 = new Point(myTabRect.X + 3, myTabRect.Y + 3);
                    Point p2 = new Point(myTabRect.X + myTabRect.Width - 3, myTabRect.Y + myTabRect.Height - 3);
                    e.Graphics.DrawLine(p, p1, p2);

                    //画"/"线
                    Point p3 = new Point(myTabRect.X + 3, myTabRect.Y + myTabRect.Height - 3);
                    Point p4 = new Point(myTabRect.X + myTabRect.Width - 3, myTabRect.Y + 3);
                    e.Graphics.DrawLine(p, p3, p4);
                }

                e.Graphics.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void 价格更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZX_Business.SelectDelete frm = new DW_YBBX.ZX_Business.SelectDelete();
            frm.Show();
        }

       
    }
}
