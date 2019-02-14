using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DALServer
    {
        
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex,string conn)
        {
            //String sql = ConfigurationManager.ConnectionStrings["BookShopconnStr"].ToString();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.AdminID desc");
            }
            strSql.Append(")AS Row, T.*  from [vw_doctor_Daylist] T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
           // return SqlHelper.ExecuteDataSet(SqlHelper.Connstring, strSql.ToString(), null);
            ///SqlHelper s = new SqlHelper(conn);

            return SqlHelper.ExecuteDataSet(conn, strSql.ToString(), null);

            // throw new NotImplementedException();
        }

        /// <summary>
        /// 获取记录总数 
        /// </summary>
        /// <param name="strWhere">查询过滤条件字段</param>
        /// <returns></returns>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM vw_doctor_Daylist");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = SqlHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }



        public static int GetAddCount(string adminID, string firstname, string password, string workerID, string adminRightID)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into V_admin_MgPersonFiles (");
            sql.Append("[AdminName],[AdminPassword],[AdminRightName],[WorkerRealName])");
            sql.Append("values (");
            sql.Append("@AdminName,@AdminPassword,@AdminRightName,@WorkerRealName)");

            SqlParameter[] para = new SqlParameter[]
                        {
                            new SqlParameter("@AdminName",firstname),
                            new SqlParameter("@AdminPassword",password),
                            new SqlParameter("@AdminRightName",adminRightID),
                              new SqlParameter("@WorkerRealName",workerID),

                        };
            int count = SqlHelper.ExecuteNonQuery(SqlHelper.Connstring, CommandType.Text, sql.ToString(), para);
            return count;
            //  throw new NotImplementedException();
        }

        public static int GetUpdateCount(string adminID, string firstname, string password, string workerID, string adminRightID)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE  V_admin_MgPersonFiles  ");
            sql.Append("SET ");
            sql.Append(" AdminName=@AdminName,");
            sql.Append(" AdminPassword=@AdminPassword,");
            sql.Append(" AdminRightName=@AdminRightName,");
            sql.Append(" WorkerRealName=@WorkerRealName");
            sql.Append("  where  AdminID=@AdminID");
            //    sql.Append("[AdminName],[AdminPassword],[AdminRightName],[WorkerRealName])");
            //    sql.Append("values (");
            //    sql.Append("@AdminName,@AdminPassword,@AdminRightName,@WorkerRealName)");

            SqlParameter[] para = new SqlParameter[]
                        {
                            new SqlParameter("@AdminName",firstname),
                            new SqlParameter("@AdminPassword",password),
                            new SqlParameter("@AdminRightName",adminRightID),
                              new SqlParameter("@WorkerRealName",workerID),
                              new  SqlParameter("@AdminID",adminID),
                        };
            int count = SqlHelper.ExecuteNonQuery(SqlHelper.Connstring, CommandType.Text, sql.ToString(), para);
            return count;
            //  throw new NotImplementedException();
        }

        public static int GetDeleteCount(string adminID)
        {
            string sqlD = "delete from V_admin_MgPersonFiles where AdminID=@AdminID";
            SqlParameter[] para = new SqlParameter[]
                            {
                                new SqlParameter("@AdminID",adminID)
                            };
            int count = SqlHelper.ExecuteNonQuery(SqlHelper.Connstring, CommandType.Text, sqlD, para);
            return count;
            // throw new NotImplementedException();
        }
    }
}
