using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.VisualBasic.Devices;
using Hisingpower;
using System.Threading;
using RealtimeAnalysis;

namespace 线路参数导入程序
{
    public partial class Form1 : Form
    {


        System.Threading.Timer dataTimer;//数据timer



        string personS;
        string teamS;
        string unitS;
      
        string _Conn;
        public string PersonS
        {
            get { return Common.getCFG("person"); }
            set { personS = value; }
        }
        public string TeamS
        {
            get { return Common.getCFG("team"); }
            set { teamS = value; }
        }
        public string UnitS
        {
            get { return Common.getCFG("unit"); }
            set { unitS = value; }
        }
        public string Conn
        {
            get { return Common.getCFG("ConnectString"); }
            set { _Conn = value; }
        }

        string _IISpath;

        public string IISpath
        {
            get { return Common.getCFG("iispath"); }
            set { _IISpath = value; }
        }

        string _LuJing;

        public string LuJing
        {
            get { return Common.getCFG("LuJing"); }
            set { _LuJing = value; }
        }

        string _Chong;

        public string Chong
        {
            get { return Common.getCFG("chong"); }
            set { _Chong = value; }
        }

        string _Delete;

        public string Delete
        {
            get { return Common.getCFG("Delete"); }
            set { _Delete = value; }
        }

        string _Zi;

        public string Zi
        {
            get { return Common.getCFG("zi"); }
            set { _Zi = value; }
        }

        int _Timess;

        public int Timess
        {
            get { return Convert.ToInt32(Common.getCFG("timess")); }
            set { _Timess = value; }
        }


        int s = 0;
        public Form1()
        {
            InitializeComponent();

            //timer.Tick += new EventHandler(sss);



        }

        string _RemoveKey;

        public string RemoveKey
        {
            get { return Common.getCFG("RemoveKey"); }
            set { _RemoveKey = value; }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            dataTimer = new System.Threading.Timer(new TimerCallback(dataTimerEvent), null, 0, (Timess * 1000));
        }



        private void dataTimerEvent(object source)
        {
            string zi = Zi;
            try
            {
                dataTimer.Change(Timeout.Infinite, -1);
                NewMethod();

            }
            catch (System.Exception ex)
            {
                if (zi == "1")
                {
                    Common.WriteLog("图片地址导图程序", "图片地址导图程序出错！" + ex.Message.ToString(), Common.Now());
                }


            }
            finally
            {
                dataTimer.Change((Timess * 1000), (Timess * 1000));
            }

        }


        //统计 部分
        int p = 0;
        float total = 0;


        private void NewMethod()
        {



            int person=0;
            //this.btnImport.Enabled = false;
            //int person = Convert.ToInt32(PersonS);
            int team = 0;// Convert.ToInt32(TeamS);
            int unit = 0;// Convert.ToInt32(UnitS);
            string zi = Zi;
            string Connstr = Conn;
            string sqlPerson = " SELECT [Id]FROM [tbUser] where  [UserName]=N'" + PersonS + "'";
          
            try
            {
                using (SqlDataReader sdrp = GetReader(Connstr, sqlPerson, null, CommandType.Text))
                {
                    if (sdrp.HasRows)
                    {
                        while (sdrp.Read())
                        {
                            person = Convert.ToInt32(sdrp["Id"]);
                        }
                       

                    }
                }
                string sqlun = "SELECT  [DepartmentId] FROM [tbUserDepartment] where [UserId]=" + person;

                using (SqlDataReader sdrp = GetReader(Connstr, sqlun.ToString(), null, CommandType.Text))
                {
                    if (sdrp.HasRows)
                    {
                         while (sdrp.Read())
                        {
                        team = Convert.ToInt32(sdrp["DepartmentId"]);
                         }

                    }
                }
                string sqldan = "SELECT   [Id]  FROM  [tbDict]  where  [Name]=N'" + UnitS + "'";

                using (SqlDataReader sdrp = GetReader(Connstr, sqldan.ToString(), null, CommandType.Text))
                {
                    if (sdrp.HasRows)
                    {
                         while (sdrp.Read())
                        {
                            unit = Convert.ToInt32(sdrp["Id"]);
                        }

                    }
                }
            }

            catch (Exception ex)
            {

                Common.WriteLog("图片地址导图程序",   ex + "  导入失败！", Common.Now());

            }
          
        //      public DataTable GetUserInfo(int userId)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("select u.UserId,u.UserName,u.AddDate,r.RoleName,d.DepartmentName from tbUser u");
        //    strSql.Append(" left join tbUserRole ur on u.Id = ur.UserId");
        //    strSql.Append(" left join tbRole r on ur.RoleId = r.Id");
        //    strSql.Append(" left join tbUserDepartment ud on u.Id= ud.UserId");
        //    strSql.Append(" left join tbDepartment d on ud.DepartmentId = d.Id");
        //    strSql.Append(" where u.Id = @userId");
        //    return  (Connstr, CommandType.Text, strSql.ToString(), new SqlParameter("@userId", userId));
        //}
            try
            {
                //http://192.168.1.124/picsource/2017年第一轮巡视/220kV安林线/巡线员A/220kV安林线12号标示牌.jpg


                //  [5/13/2016 Administrator]
                result = new List<FileInfo>();
                List<FileInfo> allfiles = new List<FileInfo>();


                DirectoryInfo dirinfo = new DirectoryInfo(LuJing);
                DirectoryInfo[] directories = dirinfo.GetDirectories("*轮巡视", SearchOption.TopDirectoryOnly);
                for (int i = 0; i < directories.Length; i++)
                {
                    allfiles = GetFiles(directories[i]);
                    allfiles.AddRange(allfiles);
                }

                foreach (FileInfo FileNm in allfiles)
                {
                    //\\TESTSEVER119\source\影像库\2017年第一轮巡视\220kV安林线\巡线员A\220kV安林线12号标示牌.jpg
                    //string fullname = FileNm.FullName.Substring(FileNm.FullName.IndexOf("影像库") + 4).Replace(@"\", @"/"); ;
                    string filename = FileNm.Name;
                    try
                    {

                        string span = "";
                        if (filename.Contains("#"))
                        {
                            span = "#";
                            // filename.Replace("#", "号");
                        }
                        else if (filename.Contains("＃"))
                        {
                            span = "#";
                            //  filename.Replace("#", "号");
                        }
                        else if (!filename.Contains("#") && !filename.Contains("＃") && filename.Contains("号"))
                        {
                            span = "号";
                            //   filename.Replace("#", "号");
                        }


                        string iispath = "";
                        try
                        {
                            iispath = IISpath + FileNm.FullName.Replace(RemoveKey, "").Replace(@"\", @"/");
                        }
                        catch (System.Exception ex)
                        {
                            Common.WriteLog("图片地址导图程序", filename + ex + "  导入失败！", Common.Now());

                        }

                        string linename = filename.Substring(0, filename.IndexOf("线") + 1);
                        string towernum = filename.Substring(filename.IndexOf("线") + 1);
                        towernum = towernum.Substring(0, towernum.IndexOf(span));
                        string part = filename.Substring(filename.IndexOf(span) + 1).Split('.')[0];
                        string type = "标准化巡视";
                        string sql = "select [Id],towerno from (SELECT [Id],towerno,row_number() over(order by id asc) as rows  FROM [tbTower] W where lineid=(select Id from tbLine where linename=N'" + filename.Substring(0, filename.IndexOf("线") + 1) + "')) MM where rows=" + towernum + "";
                        SqlDataReader sdr = GetReader(Connstr, sql, null, CommandType.Text);
                        int towerid = 0;
                        string towerno = "";
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                bool flag = false;
                                if (Chong == "1")
                                {
                                    //跳过重复照片
                                    string sql0 = "SELECT Id FROM tbPicFile WHERE FileOSName = N'" + iispath + "'";
                                    if (ExeSca(Connstr, sql0, null) == null)
                                    {
                                        flag = true;
                                    }
                                    else
                                    {
                                        flag = false;
                                        if (zi == "1")
                                        {
                                            Common.WriteLog("图片地址导图程序", iispath + "  已存在，跳过！", Common.Now());
                                        }

                                    }
                                }
                                else
                                {
                                    //忽视重复照片
                                    flag = true;
                                }

                                if (flag)
                                {
                                    try
                                    {
                                        towerid = Convert.ToInt32(sdr["Id"]);
                                        towerno = sdr["towerno"].ToString();

                                        DateTime roundsdate = DateTime.Now;
                                        string sql20 = "SELECT [Id]FROM [tbDict] where [Name]=N'" + part + "'";
                                        string sql201 = "SELECT [Id]FROM [tbDict] where [Name]=N'" + type + "'";
                                        string sql3 = "INSERT INTO [tbProperty]([LineName],[VolLevel],[TowerNo],[Piccontent],[RoundsPerson],[RoundsDate],[Istrouble],[Isdefect],[Team],[Unit],[PhotoParts],[PhotoType])VALUES";
                                        sql3 += "(N'" + linename + "','" + linename.Substring(0, linename.IndexOf("V") + 1) + "',N'" + towerno + "',N'" + filename.Split('.')[0] + "'," + person + ",'" + roundsdate + "',0,0,'" + team + "','" + unit + "'," + ExeSca(Connstr, sql20, null).ToString() + "," + ExeSca(Connstr, sql201, null).ToString() + ");SELECT @@IDENTITY";
                                        object propid = ExeSca(Connstr, sql3, null);
                                        string sql1 = "INSERT INTO [tbPicFile]([FileOSName],[DisplayName],[AddDate],[AddUser],[Fkid],[Func])VALUES(N'" + iispath + "',N'" + filename + "','" + roundsdate + "'," + person + "," + towerid.ToString() + ",'picfile');SELECT @@IDENTITY";
                                        object picfileid = ExeSca(Connstr, sql1, null);
                                        string sql2 = "INSERT INTO [tbTowerPic]([TowerId],[PicFileId])VALUES(" + towerid.ToString() + "," + picfileid.ToString() + ")";
                                        ExeSQL(Connstr, sql2, null);

                                        string sql4 = "INSERT INTO [tbPicProp]([PicFileId],[PropId]) VALUES(" + picfileid.ToString() + "," + propid.ToString() + ")";
                                        ExeSQL(Connstr, sql4, null);
                                    }
                                    catch (Exception ex)
                                    {

                                        if (zi == "1")
                                        {
                                            Common.WriteLog("图片地址导图程序", filename + ex + "  导入失败！", Common.Now());
                                        }
                                    }
                                    if (zi == "1")
                                    {
                                        Common.WriteLog("图片地址导图程序", filename + "  导入成功！", Common.Now());
                                    }

                                }

                            }
                            sdr.Close();
                        }
                        else
                        {
                            sdr.Close();
                            if (zi == "1")
                            {
                                Common.WriteLog("图片地址导图程序", filename + "  导入失败！数据库无此数据。", Common.Now());
                            }

                        }


                    }
                    catch (System.Exception ex)
                    {
                        if (zi == "1")
                        {
                            Common.WriteLog("图片地址导图程序", filename + "  导入失败！", Common.Now());
                        }

                    }




                }



                //  [5/13/2016 Administrator]


            }
            catch (System.Exception ex)
            {
                if (zi == "1")
                {
                    Common.WriteLog("图片地址导图程序", ex.Message.ToString(), Common.Now());
                }

            }

        }


        static List<FileInfo> result;
        //private static List<FileInfo> GetFiles(DirectoryInfo directory, string pattern)
        //{

        //    if (directory.Exists || pattern.Trim() != string.Empty)
        //    {
        //        try
        //        {
        //            foreach (FileInfo info in directory.GetFiles(pattern))
        //            {
        //                result.Add(info);
        //            }
        //        }
        //        catch (System.Exception ex)
        //        {

        //        }
        //        foreach (DirectoryInfo info in directory.GetDirectories())
        //        {
        //            GetFiles(info, pattern);
        //        }
        //    }
        //    return result;
        //}



        //执行增删改的sql语句（需改造或重载成可以执行存储过程的）
        private static List<FileInfo> GetFiles(DirectoryInfo directory)
        {

            if (directory.Exists)
            {
                try
                {
                    foreach (FileInfo info in directory.GetFiles())
                    {
                        result.Add(info);
                    }
                }
                catch (System.Exception ex)
                {
                    Common.WriteLog("图片地址导图程序", ex.Message.ToString(), Common.Now());
                }
                foreach (DirectoryInfo info in directory.GetDirectories())
                {
                    GetFiles(info);
                }
            }
            return result;
        }
        public static int ExeSQL(string Connstr, string sql, SqlParameter[] sps)
        {
            SqlConnection conn = new SqlConnection(Connstr);
            conn.Open();
            SqlCommand comm = new SqlCommand(sql, conn);
            if (sps != null)
                comm.Parameters.AddRange(sps);
            int i = comm.ExecuteNonQuery();
            conn.Close();
            return i;
        }
        //执行查询语句，返回第一行第一列的值
        public static object ExeSca(string Connstr, string sql, SqlParameter[] sps)
        {
            SqlConnection conn = new SqlConnection(Connstr);
            conn.Open();
            SqlCommand comm = new SqlCommand(sql, conn);
            if (sps != null)
                comm.Parameters.AddRange(sps);
            object ob = comm.ExecuteScalar();
            conn.Close();
            return ob;
        }
        //执行查询语句，返回结果集
        public static SqlDataReader GetReader(string Connstr, string sql, SqlParameter[] sps, CommandType ct)
        {
            SqlConnection conn = new SqlConnection(Connstr);
            conn.Open();
            SqlCommand comm = new SqlCommand(sql, conn);
            comm.CommandType = ct;
            if (sps != null)
                comm.Parameters.AddRange(sps);
            SqlDataReader sdr = comm.ExecuteReader(CommandBehavior.CloseConnection);
            return sdr;
        }





        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();


            LuJing = folderBrowserDialog1.SelectedPath;

            // folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.Desktop;

            //// 设置当前选择的路径
            //folderBrowserDialog1.SelectedPath = "C:";

            //// 允许在对话框中包括一个新建目录的按钮
            //folderBrowserDialog1.ShowNewFolderButton = true;

            //// 设置对话框的说明信息
            //folderBrowserDialog1.Description = "请选择输出目录";

            //if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            //{

            //    // 在此添加代码,选择的路径为 folderBrowserDialog1.SelectedPath


            //    folderBrowserDialog1.SelectedPath = System.Environment.CurrentDirectory.ToString() + @"/Data";//默认是程序所在目录的Data文件夹
            //}
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string Connstr = Conn;
            string delete = "";
            if (delete == "1")
            {
                //清空历史照片
                string sql = "DELETE FROM tbPicFile";
                ExeSQL(Connstr, sql, null);
                sql = "DELETE FROM tbPicProp";
                ExeSQL(Connstr, sql, null);
                sql = "DELETE FROM tbProperty";
                ExeSQL(Connstr, sql, null);
                sql = "DELETE FROM tbTowerPic";
                ExeSQL(Connstr, sql, null);
            }
            dataTimer = new System.Threading.Timer(new TimerCallback(dataTimerEvent), null, 0, (Timess * 1000));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (new CloseForm(this)).ShowDialog() == DialogResult.OK ? false : true;
        }


    }
}
