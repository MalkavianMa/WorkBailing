using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Config;
using System.Collections;
using DW_YBBX.ZX_Business;
using Malkavian.DButility;
using Malkavian;
namespace DW_YBBX
{
    public partial class Login : Form
    {
        public string UserCode, Password, UserName, HOSPITAL_ID, HOSPITAL_NAME;//登录用户名、密码
       //  public MSSQLHelpers SQLHelper = new MSSQLHelpers(ConfigurationManager.ConnectionStrings["connStr"].ToString());
        public MSSQLHelpers SQLHelper = new MSSQLHelpers(Common.getCFG("SQL","HIS", @"<SQL>6ABF6E75A50744805A7C3B5B5EC51FEEC4B2C77D3EE1415F05BBBB300E839C985D318EA87B743962991437C6A281B901ED1D44E323FC1B684B137FFAD7B38A7C38083C18E372EE4BAF6503EF009737C02D75E222526F10221EDB9CCD380C70F32135EFD99660208EED538E00651A61CA</SQL>", "0",Environment.CurrentDirectory + @"\AppConfig.xml"));

        string sbjgbh; //参保人所属的社保机构编号
        string yybm; //医院编码（医院在医保中心的编码）
        string UserCode_DW;//地纬的用户名
        string Password_DW;//地纬的密码
        string networkPatclassID = "";
        string strVersion = "SELECT TOP  1  [connection_version]     FROM [YB].[dbo].[compare_code_version]";
        decimal clientVersion = 0.3M;
        SqlHelper sqlVersion = new SqlHelper(ConfigurationManager.ConnectionStrings["connStr"].ToString());
        public Login()
        {
            decimal nowVesion = decimal.Parse(sqlVersion.ExecuteScalar(strVersion).ToString());
            if (clientVersion < nowVesion)
            {
                MessageBox.Show("当前对照工具版本:" + clientVersion + " 最新版本号:" + nowVesion + "请联系管理员!");
                System.Environment.Exit(0);
            }

            InitializeComponent();
            //skinEngine1.SkinFile = Application.StartupPath + @"\office2007.ssk"; //
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {




            var time = DateTime.Now;
            UserCode = text_UserCode.Text.ToString().Trim();
            Password = text_Password.Text.ToString().Trim();
            UserName = textBox_CzyXm.Text.ToString().Trim();
            HOSPITAL_ID = cmb_Hos.SelectedValue.ToString();
            HOSPITAL_NAME = cmb_Hos.Text;
            if (UserCode == "" || UserName == "")
            {
                MessageBox.Show("请正确输入用户名回车后再进行登录！");
                this.text_UserCode.Focus();
                return;
            }
            else if (Password == "")
            {
                MessageBox.Show("请输入密码后再进行登录！");
                this.text_Password.Focus();
                return;
            }
            else
            {
                string PassWord_JM = StringHelper.GetMD5String(Password);
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" SELECT DISTINCT a.USER_CODE,a.UESR_NAME,a.PASSWORD,a.HOSPITAL_ID,c.HOSPITAL_NAME FROM COMM.COMM.USERINFO_VIEW a ");
                strSql.Append(" LEFT JOIN COMM.COMM.ROLES_VS_SYS b ON b.ROLE_ID = a.ROLE_ID ");
                strSql.Append(" LEFT JOIN COMM.COMM.HOSPITALS c ON c.HOSPITAL_ID = a.HOSPITAL_ID ");
                strSql.Append(" WHERE b.SYS_ID IN ('1','3') AND a.FLAG_SYSINVALID=0 ");
                strSql.Append(" AND USER_CODE= '" + UserCode + "' AND a.HOSPITAL_ID='" + HOSPITAL_ID + "' ");//AND a.PASSWORD='" + PassWord_JM + "'

                DataTable dtRe = new DataTable();
                dtRe = SQLHelper.ExecSqlReDs(strSql.ToString()).Tables[0];
                if (dtRe.Rows.Count == 0)
                {
                    MessageBox.Show("用户不存在或无此登陆权限");
                    return;
                }
                else if (dtRe.Rows[0]["PASSWORD"].ToString().Trim() != PassWord_JM) //判断密码
                {
                    MessageBox.Show("密码不正确");
                    this.text_Password.Text = "";
                    text_Password.Focus();
                    return;
                }
                this.Hide();
                StringBuilder strSql2 = new StringBuilder();
                strSql2.Append(" SELECT TOP 1  INSTITUTION_CODE, INSTITUTION_NAME, INSTITUTION_USER_CODE, INSTITUTION_PASSWORD ");
                strSql2.Append(" FROM COMM.DICT.NETWORK_VS_INSTITUTION ");
                strSql2.Append(" WHERE HOSPITAL_ID='" + HOSPITAL_ID + "' ");
                dtRe = new DataTable();
                dtRe = SQLHelper.ExecSqlReDs(strSql2.ToString()).Tables[0];
                if (dtRe.Rows.Count == 0)
                {
                    MessageBox.Show("用户不存在或无此登陆权限");
                    return;
                }
                sbjgbh = ConfigurationManager.AppSettings["sbjgbh_JMYB"].ToString(); //社保机构编号(居民)
                yybm = dtRe.Rows[0]["INSTITUTION_CODE"].ToString(); //医院编码
                UserCode_DW = dtRe.Rows[0]["INSTITUTION_USER_CODE"].ToString();//医保登录用户名
                Password_DW = dtRe.Rows[0]["INSTITUTION_PASSWORD"].ToString();//医保登录密码 


                FrmsbjgSelect fsbjg = new FrmsbjgSelect();
                fsbjg.ShowDialog();

                sbjgbh = fsbjg.selSbjgBH;
                networkPatclassID = "";//fsbjg.fglyComtext;

                fsbjg.Close();
                //LOGIN.Sel_SBJG frm = new LOGIN.Sel_SBJG(this.text_UserCode.Text.ToString(), this.textBox_CzyXm.Text.ToString(), HOSPITAL_ID, HOSPITAL_NAME);
                MainForm frm = new MainForm(UserCode, UserName, HOSPITAL_ID, HOSPITAL_NAME, sbjgbh, yybm, UserCode_DW, Password_DW, networkPatclassID);
                frm.ShowDialog();
                this.Close();
                this.Dispose();
            }
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
        /// <summary>
        /// 输入工号回车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void text_UserCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" SELECT DISTINCT a.USER_CODE,a.UESR_NAME,a.PASSWORD,a.HOSPITAL_ID,c.HOSPITAL_NAME FROM COMM.COMM.USERINFO_VIEW a ");
                strSql.Append(" LEFT JOIN COMM.COMM.ROLES_VS_SYS b ON b.ROLE_ID = a.ROLE_ID ");
                strSql.Append(" LEFT JOIN COMM.COMM.HOSPITALS c ON c.HOSPITAL_ID = a.HOSPITAL_ID ");
                strSql.Append(" WHERE b.SYS_ID IN ('1','3') AND a.FLAG_SYSINVALID=0 ");
                strSql.Append(" AND USER_CODE= '" + this.text_UserCode.Text + "'  ");//AND a.PASSWORD='" + PassWord_JM + "'

                DataTable dtRe = new DataTable();
                dtRe = SQLHelper.ExecSqlReDs(strSql.ToString()).Tables[0];
                if (dtRe.Rows.Count == 0)
                {
                    MessageBox.Show("用户不存在或无此登陆权限!");
                    this.text_UserCode.Text = "";
                    this.textBox_CzyXm.Text = "";
                    return;
                }
                else
                {
                    ArrayList arraylist1 = new ArrayList();
                    for (int i = 0; i < dtRe.Rows.Count; i++)
                    {
                        arraylist1.Add(new DictionaryEntry(dtRe.Rows[i]["HOSPITAL_ID"].ToString(), dtRe.Rows[i]["HOSPITAL_NAME"].ToString()));
                    }
                    cmb_Hos.DataSource = arraylist1;
                    cmb_Hos.ValueMember = "Key";
                    cmb_Hos.DisplayMember = "Value";

                    this.textBox_CzyXm.Text = dtRe.Rows[0][1].ToString();
                    this.text_Password.Focus();
                }
            }
        }
        /// <summary>
        /// 输入密码回车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void text_Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.button1_Click(sender, e);//触发button事件 
            }
        }

        private void Login_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            AboutBox frm = new AboutBox();
            frm.ShowDialog();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
