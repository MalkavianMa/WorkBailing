using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace RIClientTestDemo.DAL
{
    public class RIClientTestDAL
    {
        MSSQLHelpers sqlHelper;
        public RIClientTestDAL() 
        {
        
        }
        public DataTable PatHosCodeConvertToPatHosId(string PatInHosCode,string StrConn)
        {
            sqlHelper = new MSSQLHelpers(StrConn);
            DataTable dtResult = new DataTable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT PAT_IN_HOS_ID,PAT_AGAIN_IN_TIMES,CHARGE_CLASS_NAME,IN_PAT_NAME,CHARGE_CLASS_ID FROM ZY.[IN].PAT_ALL_INFO_VIEW WHERE PAT_IN_HOS_CODE ='"+PatInHosCode+"'");
            dtResult = sqlHelper.ExecSqlReDs(strSql.ToString()).Tables[0];
            return dtResult;
        }
        public DataTable QueryChargeClassID(string StrConn)
        {
            sqlHelper = new MSSQLHelpers(StrConn);
            DataTable dtResult = new DataTable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT	 CHARGE_CLASS_ID ,CHARGE_CLASS_NAME ,NETWORKING_PAT_CLASS_ID FROM COMM.DICT.CHARGE_CLASSES");
            dtResult = sqlHelper.ExecSqlReDs(strSql.ToString()).Tables[0];
            return dtResult;
        }

        public DataTable ChargeClassIdConvertToNetworkPatClassId(string ChargeClassId, string StrConn)
        {
            sqlHelper = new MSSQLHelpers(StrConn);
            DataTable dtResult = new DataTable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT NETWORKING_PAT_CLASS_ID FROM COMM.DICT.CHARGE_CLASSES WHERE CHARGE_CLASS_ID ='" + ChargeClassId + "'");
            dtResult = sqlHelper.ExecSqlReDs(strSql.ToString()).Tables[0];

            return dtResult;
        }

        public DataTable OutPatCodeConvertToOutPatID(string OutPatCode,string StrConn)
        {
            sqlHelper = new MSSQLHelpers(StrConn);
            DataTable dtResult = new DataTable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT OUT_PAT_NAME,OUT_PAT_ID,CHARGE_CLASS_ID FROM COMM.DICT.OUT_PATS WHERE OUT_PAT_CODE ='"+OutPatCode+"'");
            dtResult = sqlHelper.ExecSqlReDs(strSql.ToString()).Tables[0];

            return dtResult;
        }
        public DataTable OutPatCodeQueryAutoID(string OutPatCode, string StrConn)
        {
            sqlHelper = new MSSQLHelpers(StrConn);
            DataTable dtResult = new DataTable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT CAST(1 AS BIT) AS 是否收取该费用,b.AUTO_ID,c.CHARGE_NAME,b.AMOUNT AS 总金额,b.QUANTITY AS 数量,b.SPEC,b.PRICE AS 单价 FROM COMM.DICT.OUT_PATS a INNER JOIN MZ.OUT.OUT_ORDER_CHARGE_TMP b ON a.OUT_PAT_ID =b.PAT_ID INNER JOIN COMM.COMM.CHARGE_PRICE_ALL_VIEW c ON b.CHARGE_ID = c.CHARGE_ID  WHERE a.OUT_PAT_CODE='" + OutPatCode + "'");
            dtResult = sqlHelper.ExecSqlReDs(strSql.ToString()).Tables[0];
            return dtResult;
        }
        public DataTable OutPatIdToInvoiceCode(string OutPatID, string StrConn,string StartTime,string EndTime)
        {
            sqlHelper = new MSSQLHelpers(StrConn);
            DataTable dtResult = new DataTable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT CAST(0 AS BIT) as 选择需要退费的发票号, INVOICE_CODE as 发票号,INVOICE_ID as 发票ID,AMOUNT as 发票金额 FROM MZ.OUT.INVOICE_MAIN WHERE CREATE_TIME BETWEEN '" + StartTime + "' AND '" + EndTime + "' AND OUT_PAT_ID ='" + OutPatID + "'AND INVOICE_TYPE <10000 ");
            dtResult = sqlHelper.ExecSqlReDs(strSql.ToString()).Tables[0];
            return dtResult;

        }
        public DataTable GetInoviceDetails(string InvoiceList,string StrConn)
        {
            DataTable dtResult = new DataTable();
            sqlHelper = new MSSQLHelpers(StrConn);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT CAST(0 AS BIT) as 选择需要退费的项目,b.CHARGE_NAME,a.QUANTITY AS 退费数量,a.QUANTITY AS 数量,a.AMOUNT AS 总额,(CASE WHEN b.CHARGE_TYPE >100 THEN '诊疗' WHEN b.CHARGE_TYPE<100 THEN '药品' END) AS 类型,a.INVOICE_DETAIL_ID AS 发票明细ID FROM");
            strSql.Append(" MZ.OUT.INVOICE_DETAILS_VIEW a INNER JOIN COMM.COMM.CHARGE_PRICE_ALL_VIEW b ON a.CHARGE_ID = b.CHARGE_ID INNER JOIN MZ.OUT.INVOICE_MAIN c ON a.INVOICE_ID =c.INVOICE_ID  WHERE A.INVOICE_ID in (" + InvoiceList + ") AND (c.INVOICE_TYPE >=1 AND c.INVOICE_TYPE <=10000)");
            dtResult = sqlHelper.ExecSqlReDs(strSql.ToString()).Tables[0];
            return dtResult;
        }
        public DataTable GetOutPatAllInfo(string name, string StrConn)
        {
            DataTable dtResult = new DataTable();
            sqlHelper = new MSSQLHelpers(StrConn);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT OUT_PAT_ID,OUT_PAT_CODE,CHARGE_CLASS_ID,OUT_PAT_NAME FROM COMM.DICT.OUT_PATS WHERE OUT_PAT_NAME ='" + name + "' OR INPUT_CODE LIKE '" + name + "%'");
            dtResult = sqlHelper.ExecSqlReDs(strSql.ToString()).Tables[0];
            return dtResult;

        }
        public DataTable OutPatIDToOutPatCode(string OutPatID,string StrConn)
        {
            DataTable dtResult = new DataTable();
            sqlHelper = new MSSQLHelpers(StrConn);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT OUT_PAT_CODE FROM COMM.DICT.OUT_PATS WHERE OUT_PAT_ID ='"+OutPatID+"'");
            dtResult = sqlHelper.ExecSqlReDs(strSql.ToString()).Tables[0];
            return dtResult;
        }
    }
   
}
