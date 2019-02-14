using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL;
using System.Text;
using System.Configuration;

namespace WebApplication3
{
    /// <summary>
    /// SetAdmin 的摘要说明
    /// </summary>
    public class SetAdmin : IHttpHandler
    {
        BLL.BLLManager bll = new BLL.BLLManager();
        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "text/plain";
            string operation, firstname, password, workerID, adminRightID, adminID;
            operation = firstname = password = workerID = adminRightID = adminID = "";
            if (null != context.Request.QueryString["adminID"])
            {
                adminID = context.Request.QueryString["adminID"].ToString();
            }
            if (null != context.Request.QueryString["firstname"])
            {
                firstname = context.Request.QueryString["firstname"].ToString();
            }
            if (null != context.Request.QueryString["password"])
            {
                password = context.Request.QueryString["password"].ToString();

            }
            if (null != context.Request.QueryString["workerID"])
            {
                workerID = context.Request.QueryString["workerID"].ToString();

            }
            if (null != context.Request.QueryString["adminRightID"])
            {
                adminRightID = context.Request.QueryString["adminRightID"].ToString();

            }
            if (!string.IsNullOrEmpty(context.Request.QueryString["test"]))
            {
                operation = context.Request.QueryString["test"].ToString();

                switch (operation)
                {

                    case "add":
                        if (!string.IsNullOrEmpty(firstname) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(workerID) && !string.IsNullOrEmpty(adminRightID))//AdminName
                        {
                            int Count = BLL.BLLManager.AddCount(adminID, firstname, password, workerID, adminRightID);//静态字段访问
                            if (Count > 0)
                            {
                                context.Response.Write("T");//返回给前台页面,提示成功
                            }
                        }
                        break;
                    case "modify":
                        if (!string.IsNullOrEmpty(firstname) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(workerID) && !string.IsNullOrEmpty(adminRightID))//AdminName
                        {
                            int Count = BLL.BLLManager.UpdateCount(adminID, firstname, password, workerID, adminRightID);//静态字段访问
                            if (Count > 0)
                            {
                                context.Response.Write("T");//返回给前台页面,提示成功
                            }
                        }
                        break;
                    case "edit":
                        break;
                    case "delete":
                        if (!string.IsNullOrEmpty(adminID))//AdminName
                        {

                            int count = BLL.BLLManager.DeleteCount(adminID);

                            if (count > 0)
                            {
                                context.Response.Write("T");//返回给前台页面  

                            }

                        }
                        break;
                    default:
                        Query(context);
                        break;
                }
            }
            else
            {
                Query(context);
            }
            // context.Response.Write("Hello World");
        }

        #region 查询方法
        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="context"></param>
        public void Query(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            // context.Response.ContentType = "text/html";
            //string a = @"<script  type=" + "\"text/javascript\"" + @">alert(2)</script>";
            //===============================================================  
            //获取查询条件:【用户id,开始时间，结束时间，关键字】  
            string UserName, startTime, endTime, QuanXian, test;
            UserName = startTime = endTime = QuanXian = test = "";

            //  if (!string.IsNullOrEmpty(context.Request.QueryString["test"]))
            if (null != context.Request.QueryString["test"])
            {
                test = context.Request.QueryString["test"].ToString();
            }
            //获取前台传来的值  
            if (null != context.Request.QueryString["UserName"])
            {//获取前台传来的值  
                UserName = context.Request.QueryString["UserName"].ToString().Trim();// 取前台JS中 queryParams.UserName
            }
            if (null != context.Request.QueryString["StartTime"])
            {
                startTime = context.Request.QueryString["StartTime"].ToString().Trim();
            }
            if (null != context.Request.QueryString["EndTime"])
            {
                endTime = context.Request.QueryString["EndTime"].ToString().Trim();
            }
            if (null != context.Request.QueryString["QuanXian"])
            {
                QuanXian = context.Request.QueryString["QuanXian"].ToString().Trim();
            }

            //================================================================  
            //获取分页和排序信息：页大小，页码，排序方式，排序字段  
            int pageRows, page;
            pageRows = 10;
            page = 1;
            string order, sort, oderby; order = sort = oderby = "";
            if (null != context.Request.QueryString["rows"])
            {
                pageRows = int.Parse(context.Request.QueryString["rows"].ToString().Trim());

            }
            if (null != context.Request.QueryString["page"])
            {

                page = int.Parse(context.Request.QueryString["page"].ToString().Trim());

            }
            if (null != context.Request.QueryString["sort"])
            {

                order = context.Request.QueryString["sort"].ToString().Trim();

            }
            if (null != context.Request.QueryString["order"])
            {

                sort = context.Request.QueryString["order"].ToString().Trim();

            }


            //===================================================================  
            //组合查询语句：条件+排序  
            StringBuilder strWhere = new StringBuilder();
            if (UserName != "")
            {
                strWhere.AppendFormat(" WorkerRealName  like '%{0}%' and ", UserName);
            }
            if (QuanXian != "")
            {
                strWhere.AppendFormat(" AdminRightName like '%{0}%' and ", QuanXian);
            }
            if (startTime != "")
            {
                strWhere.AppendFormat(" ActiveDate >= '{0}' and ", startTime);
            }
            if (endTime != "")
            {
                strWhere.AppendFormat(" ActiveDate <= '{0}' and ", endTime);
            }

            //删除多余的and  
            int startindex = strWhere.ToString().LastIndexOf("and");//获取最后一个and的位置  
            if (startindex >= 0)
            {
                strWhere.Remove(startindex, 3);//删除多余的and关键字  
            }
            if (sort != "" && order != "")
            {
                //strWhere.AppendFormat(" order by {0} {1}", sort, order);//添加排序  
                oderby = order + " " + sort;
            }
           string conn=ConfigurationManager.ConnectionStrings["BookShopconnStr"].ToString();
            string strJson = BLL.BLLManager.GetStrJson(strWhere.ToString(), oderby, (page - 1) * pageRows + 1, page * pageRows,conn);
            // string strJson = bll.GetStrJson(strWhere.ToString(), oderby, (page - 1) * pageRows + 1, page * pageRows);//无法使用实例引用来访问成员请改用类型名来限定它
            // SqlHelper.ExecuteDataSet(sql, strWhere, parms);
            //DataSet ds = Bnotice.GetList(strWhere.ToString());  //调用不分页的getlist   
            //DataSet ds = GetListByPage(strWhere.ToString(), oderby, (page - 1) * pageRows + 1, page * pageRows);
            //int count = GetRecordCount(strWhere.ToString());//获取条数  
            //string strJson = ToJson.Dataset2Json(ds, count);//DataSet数据转化为Json数据  
            context.Response.Write(strJson);//返回给前台页面  
            //context.Request.QueryString["UserName"] = "";//集合是只读的
            context.Response.End();
            //   context.Request.QueryString.Clear();//集合是只读的

            //
        }
        #endregion
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}