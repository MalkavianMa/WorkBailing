using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAL;

namespace BLL
{
   public class BLLManager
    {
       DAL.DALServer dll = new DAL.DALServer();
       public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex,string conn)
       {
           return dll.GetListByPage(strWhere,orderby,startIndex,endIndex,conn);
       }
       public int GetRecordCount( string strWhere)
       {
           return dll.GetRecordCount(strWhere);
       }


       public static string GetStrJson(string strWhere, string orderby, int startIndex, int endIndex,string conn)//static有无
       {
           DAL.DALServer dll = new DAL.DALServer();//C#非静态的字段要求对象引用

           DataSet ds = dll. GetListByPage(strWhere, orderby, startIndex, endIndex,conn);
           int count =dll. GetRecordCount(strWhere);
           string strJson = ToJson.Dataset2Json(ds,count);
           return strJson;
           //throw new NotImplementedException();
       }

       public static int AddCount(string adminID, string firstname, string password, string workerID, string adminRightID)
       {
          // DAL.DALServer dll = new DAL.DALServer();//f非静态的字段要求对象引用
           return DAL.DALServer.GetAddCount(adminID,firstname,password,workerID,adminRightID);//无法使用实例引用来访问成员,请改用类型名来限定它
           //throw new NotImplementedException();
       }

       public static int UpdateCount(string adminID, string firstname, string password, string workerID, string adminRightID)
       {
           return DAL.DALServer.GetUpdateCount(adminID, firstname, password, workerID, adminRightID);
          // throw new NotImplementedException();
       }

       public static int DeleteCount(string adminID)
       {
           return DAL.DALServer.GetDeleteCount(adminID);
          // throw new NotImplementedException();
       }
    }
}
