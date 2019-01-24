﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;

public class MSSQLHelpers
{
    #region 私有变量和常用操作和属性
    private SqlConnection conn;         //SQL连接
    private SqlTransaction sqlTran;     //事务处理
    private bool isInTran = false;        //标识是否处于事务中
    private string connStr;             //连接字符串
    private bool isConnCloseWhenEveryRun = true; //连接是否在每一次执行相关方法后自动关闭 

    /// <summary>
    /// 获取或者设置连接是否在每一次执行相关方法后自动关闭,
    /// 如果不自动关闭需要手动调用Close方法关闭链接
    /// </summary>
    public bool IsConnCloseWhenEveryRun
    {
        get { return isConnCloseWhenEveryRun; }
        set { this.isConnCloseWhenEveryRun = value; }
    }

    /// <summary>
    /// 获取链接字符串 
    /// </summary>
    public string ConnectionString
    {
        get { return this.connStr; }
    }


    /// <summary>
    /// 默认构造函数 用于初始化数据库连接，使用该方法初始化时，
    /// 每次执行非事务数据库相关的方法或者结束事物后都会关闭连接
    /// </summary>
    /// <param name="connstring">数据库连接字符串</param>
    public MSSQLHelpers(string connstring)
    {
        this.connStr = connstring;
        conn = new SqlConnection(this.connStr);
        this.isConnCloseWhenEveryRun = true;
    }

    /// <summary>
    /// 默认构造函数 用于初始化数据库连接，使用该方法初始化时，
    /// 每次执行非事务数据库相关的方法或者结束事物后都会关闭连接
    /// </summary>
    /// <param name="connstring">数据库连接字符串</param>
    /// <param name="isConnCloseWhenEveryRun"> 获取或者设置连接是否在每一次执行相关方法后自动关闭,
    /// 如果不自动关闭需要手动调用Close方法关闭链接</param>
    public MSSQLHelpers(string connstring, bool isConnCloseWhenEveryRun)
    {
        this.connStr = connstring;
        conn = new SqlConnection(this.connStr);
        this.isConnCloseWhenEveryRun = isConnCloseWhenEveryRun;
    }

    /// <summary>
    /// 打开数据库连接
    /// </summary>
    public void Open()
    {
        if (this.conn.State == ConnectionState.Closed)
        {
            
                this.conn.Open();
           
            
        }
        if (this.conn.State == ConnectionState.Broken)
        {
            
                this.conn.Close();
                this.conn.Open();
           
            
        }
    }


    /// <summary>
    /// 关闭数据库连接
    /// </summary>
    public void Close()
    {
        if (this.conn.State == ConnectionState.Closed)
        {
            return;
        }
        try
        {
            this.conn.Close();
        }
        catch
        {

        }
    }
    #endregion

    #region 执行语句

    /// <summary>
    /// 执行sql语句返回受影响的行数
    /// </summary>
    /// <param name="sqlStr">执行的sql语句</param>
    /// <returns>受影响的行数</returns>
    public int ExecSqlReInt(string sqlStr)
    {
        this.Open();
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = this.conn;
            if (this.isInTran)
            {
                cmd.Transaction = this.sqlTran;
            }
            cmd.CommandText = sqlStr;
            int i = cmd.ExecuteNonQuery();
            cmd.Dispose();
            if ((!this.isInTran) && this.isConnCloseWhenEveryRun)
            {
                this.Close();
            }
            return i;
        }
        catch (Exception ex)
        {
            if ((!this.isInTran) && this.isConnCloseWhenEveryRun)
            {
                this.Close();
            }
            throw new Exception(ex.Message + "执行sql时错误：" + sqlStr);
        }

    }

    /// <summary>
    /// 执行带参数的sql语句返回受影响的行数
    /// </summary>
    /// <param name="sqlStr">执行的sql语句</param>
    /// <param name="para">参数列表</param>
    /// <returns>受影响的行数</returns>
    //public int ExecSqlReInt(string sqlStr, IList<IDataParameter> para)
    //{
    //    IDataParameter[] datas = para.ToArray<IDataParameter>();
    //    return this.ExecSqlReInt(sqlStr, datas);
    //}

    /// <summary>
    /// 执行带参数的sql语句返回受影响的行数
    /// </summary>
    /// <param name="sqlStr">执行的sql语句</param>
    /// <param name="para">参数列表</param>
    /// <returns>受影响的行数</returns>
    public int ExecSqlReInt(string sqlStr, IDataParameter[] para)
    {
        this.Open();
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = this.conn;
            if (isInTran)
            {
                cmd.Transaction = sqlTran;
            }
            cmd.CommandText = sqlStr;
            if (para != null)
            {
                foreach (SqlParameter paramete in para)
                {
                    cmd.Parameters.Add(paramete);
                }
            }
            int i = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            if ((!this.isInTran) && this.isConnCloseWhenEveryRun)
            {
                this.Close();
            }
            return i;
        }
        catch (Exception ex)
        {
            if ((!this.isInTran) && this.isConnCloseWhenEveryRun)
            {
                this.Close();
            }
            StringBuilder errstr = new StringBuilder();
            foreach (SqlParameter parateme in para)
            {
                errstr.Append(parateme.ParameterName);
                errstr.Append(":");
                errstr.Append(parateme.Value.ToString());
            }
            throw new Exception(ex.Message + "执行sql时错误：" + sqlStr + ";" + errstr.ToString());
        }

    }
    #endregion

    #region DataReader
    /// <summary>
    /// 执行sql语句返回Reader执行该方法时，数据库连接不会关闭如果IsConnCloseWhenEveryRun 为true并且没有执行事务则
    /// 在关闭dr的时候数据库连接自动关闭。在提交或者回滚事物之前必须关闭dr
    /// </summary>
    /// <param name="commandBehavior">前置条件：必须将改数据连接类的IsConnCloseWhenEveryRun 设置为false或者在事物过程中执行语句时
    /// 该项起作用，否则commandBehavior 一直为CloseConnection 即关闭dr时自动关闭链接 查询结果对数据库连接影响的说明</param>
    /// <param name="sqlstr">执行的sql语句</param>
    /// <returns>返回的reader</returns>
    public SqlDataReader ExecSqlReDr(CommandBehavior commandBehavior, string sqlstr)
    {
        this.Open();
        SqlDataReader dr = null;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = this.conn;
            if (isInTran)
            {
                cmd.Transaction = sqlTran;
            }
            cmd.CommandText = sqlstr;

            if ((!this.isInTran) && this.isConnCloseWhenEveryRun)
            {
                commandBehavior = CommandBehavior.CloseConnection;
            }
            dr = cmd.ExecuteReader(commandBehavior);
            cmd.Parameters.Clear();
            cmd.Dispose();
        }
        catch (Exception ex)
        {
            if (dr != null && (!dr.IsClosed))
            {
                dr.Close();
                if ((!this.isInTran) && this.isConnCloseWhenEveryRun)
                {
                    this.Close();
                }
            }
            throw new Exception(ex.Message + "获取dr时错误：" + sqlstr);
        }
        return dr;
    }

    /// <summary>
    /// 执行sql语句返回Reader执行该方法时，数据库连接不会关闭如果IsConnCloseWhenEveryRun 为true并且没有执行事务则
    /// 在关闭dr的时候数据库连接自动关闭。在提交或者回滚事物之前必须关闭dr
    /// </summary>
    /// <param name="sqlstr">执行的sql语句</param>
    /// <returns>返回的reader</returns>
    public SqlDataReader ExecSqlReDr(string sqlstr)
    {
        CommandBehavior commandBehavior = CommandBehavior.Default;
        this.Open();
        SqlDataReader dr = null;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = this.conn;
            if (isInTran)
            {
                cmd.Transaction = sqlTran;
            }
            cmd.CommandText = sqlstr;

            if ((!this.isInTran) && this.isConnCloseWhenEveryRun)
            {
                commandBehavior = CommandBehavior.CloseConnection;
            }
            dr = cmd.ExecuteReader(commandBehavior);
            cmd.Parameters.Clear();
            cmd.Dispose();
        }
        catch (Exception ex)
        {
            if (dr != null && (!dr.IsClosed))
            {
                dr.Close();
                if ((!this.isInTran) && this.isConnCloseWhenEveryRun)
                {
                    this.Close();
                }
            }
            throw new Exception(ex.Message + "获取dr时错误：" + sqlstr);
        }


        return dr;
    }

    /// <summary>
    /// 执行sql语句返回Reader执行该方法时，数据库连接不会关闭如果IsConnCloseWhenEveryRun 为true并且没有执行事务则
    /// 在关闭dr的时候数据库连接自动关闭。在提交或者回滚事物之前必须关闭dr
    /// </summary>
    /// <param name="commandBehavior">前置条件：必须将改数据连接类的IsConnCloseWhenEveryRun 设置为false或者在事物过程中执行语句时
    /// 该项起作用，否则commandBehavior 一直为CloseConnection 即关闭dr时自动关闭链接 查询结果对数据库连接影响的说明</param>
    /// <param name="sqlstr">执行的sql语句</param>
    /// <param name="para">参数列表</param>
    /// <returns>返回的SqlDataReader</returns>
    public SqlDataReader ExecSqlReDr(CommandBehavior commandBehavior, string sqlstr, IDataParameter[] para)
    {
        this.Open();
        SqlDataReader dr = null;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = this.conn;
            if (isInTran)
            {
                cmd.Transaction = sqlTran;
            }

            cmd.CommandText = sqlstr;
            if (para != null)
            {
                foreach (SqlParameter parateme in para)
                {
                    cmd.Parameters.Add(parateme);
                }
            }

            if ((!this.isInTran) && this.isConnCloseWhenEveryRun)
            {
                commandBehavior = CommandBehavior.CloseConnection;
            }
            dr = cmd.ExecuteReader(commandBehavior);
            cmd.Parameters.Clear();
            cmd.Dispose();
        }
        catch (Exception ex)
        {
            if (dr != null && (!dr.IsClosed))
            {
                dr.Close();
                if ((!this.isInTran) && this.isConnCloseWhenEveryRun)
                {
                    this.Close();
                }
            }
            StringBuilder errstr = new StringBuilder();
            foreach (SqlParameter parateme in para)
            {
                errstr.Append(parateme.ParameterName);
                errstr.Append(":");
                errstr.Append(parateme.Value.ToString());
            }
            throw new Exception(ex.Message + "获取dr时错误：" + sqlstr + ";" + errstr.ToString());
        }

        return dr;

    }

    /// <summary>
    /// 执行sql语句返回Reader执行该方法时，数据库连接不会关闭如果IsConnCloseWhenEveryRun 为true并且没有执行事务则
    /// 在关闭dr的时候数据库连接自动关闭。在提交或者回滚事物之前必须关闭dr
    /// </summary>
    /// <param name="sqlStr">执行的sql语句</param>
    /// <param name="para">参数列表</param>
    /// <returns>返回的SqlDataReader</returns>
    //public SqlDataReader ExecSqlReDr(string sqlStr, IList<IDataParameter> para)
    //{
        
    //    IDataParameter[] datas = para.ToArray<IDataParameter>();
    //    return this.ExecSqlReDr(sqlStr, datas);
    //}

    /// <summary>
    /// 执行sql语句返回Reader执行该方法时，数据库连接不会关闭如果IsConnCloseWhenEveryRun 为true并且没有执行事务则
    /// 在关闭dr的时候数据库连接自动关闭。在提交或者回滚事物之前必须关闭dr
    /// </summary>
    /// <param name="sqlstr">执行的sql语句</param>
    /// <param name="para">参数列表</param>
    /// <returns>返回的SqlDataReader</returns>
    public SqlDataReader ExecSqlReDr(string sqlstr, IDataParameter[] para)
    {
        CommandBehavior commandBehavior = CommandBehavior.Default;
        this.Open();
        SqlDataReader dr = null;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = this.conn;
            if (isInTran)
            {
                cmd.Transaction = sqlTran;
            }

            cmd.CommandText = sqlstr;
            if (para != null)
            {
                foreach (SqlParameter parateme in para)
                {
                    cmd.Parameters.Add(parateme);
                }
            }
            if ((!this.isInTran) && this.isConnCloseWhenEveryRun)
            {
                commandBehavior = CommandBehavior.CloseConnection;
            }
            dr = cmd.ExecuteReader(commandBehavior);
            cmd.Parameters.Clear();
            cmd.Dispose();
        }
        catch (Exception ex)
        {
            if (dr != null && (!dr.IsClosed))
            {
                dr.Close();
                if ((!this.isInTran) && this.isConnCloseWhenEveryRun)
                {
                    this.Close();
                }
            }
            StringBuilder errstr = new StringBuilder();
            foreach (SqlParameter parateme in para)
            {
                errstr.Append(parateme.ParameterName);
                errstr.Append(":");
                errstr.Append(parateme.Value.ToString());
            }
            throw new Exception(ex.Message + "获取dr时错误：" + sqlstr + ";" + errstr.ToString());
        }

        return dr;

    }

    #endregion

    #region DataSet



    /// <summary>
    /// 执行sql语句返回数据集
    /// </summary>
    /// <param name="sqlStr">执行sql语句</param>
    /// <returns>返回的数据集</returns>
    public DataSet ExecSqlReDs(string sqlStr)
    {
        this.Open();
        try
        {
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = this.conn;
            if (isInTran)
            {
                cmd.Transaction = sqlTran;
            }
            cmd.CommandText = sqlStr;
            DataSet ds = new DataSet();
            using (SqlDataAdapter ad = new SqlDataAdapter())
            {
                ad.SelectCommand = cmd;
                ad.Fill(ds);
            }
            cmd.Parameters.Clear();
            cmd.Dispose();
            if ((!this.isInTran) && this.isConnCloseWhenEveryRun)
            {
                this.Close();
            }
            return ds;
        }
        catch (Exception ex)
        {
            if ((!this.isInTran) && this.isConnCloseWhenEveryRun)
            {
                this.Close();
            }
            throw new Exception(ex.Message + "获取ds时错误：" + sqlStr);
        }


    }

    /// <summary>
    /// 执行带参数sql语句返回数据集
    /// </summary>
    /// <param name="sqlStr">执行的sql语句</param>
    /// <param name="para">参数列表</param>
    /// <returns>返回的数据集</returns>
    //public DataSet ExecSqlReDs(string sqlStr, IList<IDataParameter> para)
    //{
    //    IDataParameter[] datas = para.ToArray<IDataParameter>();
    //    return this.ExecSqlReDs(sqlStr, datas);
    //}


    /// <summary>
    /// 执行带参数sql语句返回数据集
    /// </summary>
    /// <param name="sqlStr">执行的sql语句</param>
    /// <param name="para">参数列表</param>
    /// <returns>返回的数据集</returns>
    public DataSet ExecSqlReDs(string sqlStr, IDataParameter[] para)
    {
        this.Open();
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = this.conn;
            if (isInTran)
            {
                cmd.Transaction = sqlTran;
            }

            cmd.CommandText = sqlStr;
            if (para != null)
            {
                foreach (SqlParameter parateme in para)
                {
                    cmd.Parameters.Add(parateme);
                }
            }
            DataSet ds = new DataSet();
            using (SqlDataAdapter ad = new SqlDataAdapter())
            {
                ad.SelectCommand = cmd;
                ad.Fill(ds);
            }
            cmd.Parameters.Clear();
            cmd.Dispose();
            if ((!this.isInTran) && this.isConnCloseWhenEveryRun)
            {
                this.Close();
            }
            return ds;
        }
        catch (Exception ex)
        {
            if ((!this.isInTran) && this.isConnCloseWhenEveryRun)
            {
                this.Close();
            }
            StringBuilder errstr = new StringBuilder();
            foreach (SqlParameter parateme in para)
            {
                errstr.Append(parateme.ParameterName);
                errstr.Append(":");
                errstr.Append(parateme.Value.ToString());
            }
            throw new Exception(ex.Message + "获取ds时错误：" + sqlStr + ";" + errstr.ToString());
        }

    }

    #endregion

    /// <summary>
    /// 析构方法
    /// </summary>
    ~MSSQLHelpers()
    {
        this.Dispose();
    }

    /// <summary>
    /// 释放占用的资源
    /// </summary>
    /// <param name="dis"></param>
    protected void Dispose(bool dis)
    {
        if (dis)
        {
            if (this.conn != null)
            {
                this.Close();
                this.conn.Dispose();
            }

            if (this.sqlTran != null)
            {
                this.sqlTran.Dispose();
            }

        }

    }

    /// <summary>
    /// 释放数据库连接
    /// </summary>
    public void Dispose()
    {
        try
        {
            this.Dispose(true);
        }
        catch
        {

        }
        this.conn = null;
        this.sqlTran = null;

    }
}
