using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Config;

namespace RIClientTestDemo.DAL
{
    public class RIClientTestDALORCL
    {
        ORCLHelper sqlHelper;
        public RIClientTestDALORCL() 
        {
        
        }
        public DataTable PatHosCodeConvertToPatHosId(string PatInHosCode,string StrConn)
        {
            sqlHelper = new ORCLHelper(StrConn);
            DataTable dtResult = new DataTable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT PAT_IN_HOS_ID,PAT_AGAIN_IN_TIMES,CHARGE_CLASS_NAME,IN_PAT_NAME,CHARGE_CLASS_ID FROM ZYHIS.PAT_ALL_INFO_VIEW WHERE PAT_IN_HOS_CODE ='"+PatInHosCode+"'");
            dtResult = sqlHelper.ExecSqlReDs(strSql.ToString()).Tables[0];
            return dtResult;
        }
        public DataTable QueryChargeClassID(string StrConn)
        {
            sqlHelper = new ORCLHelper(StrConn);
            DataTable dtResult = new DataTable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT	 CHARGE_CLASS_ID ,CHARGE_CLASS_NAME ,NETWORKING_PAT_CLASS_ID FROM ZYDICT.CHARGE_CLASSES");
            dtResult = sqlHelper.ExecSqlReDs(strSql.ToString()).Tables[0];
            return dtResult;
        }
        public DataTable OutPatCodeConvertToOutPatID(string OutPatCode,string StrConn)
        {
            sqlHelper = new ORCLHelper(StrConn);
            DataTable dtResult = new DataTable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT OUT_PAT_NAME,OUT_PAT_ID,CHARGE_CLASS_ID FROM ZYDICT.OUT_PATS WHERE OUT_PAT_CODE ='"+OutPatCode+"'");
            dtResult = sqlHelper.ExecSqlReDs(strSql.ToString()).Tables[0];

            return dtResult;
        }
        public DataTable OutPatCodeQueryAutoID(string OutPatCode, string StrConn)
        {
            sqlHelper = new ORCLHelper(StrConn);
            DataTable dtResult = new DataTable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT 1 AS 是否收取该费用,b.AUTO_ID,c.CHARGE_NAME,b.AMOUNT AS 总金额,b.QUANTITY AS 数量,b.SPEC,b.PRICE AS 单价 FROM ZYDICT.OUT_PATS a INNER JOIN ZYHIS.OUT_ORDER_CHARGE_TMP_MZ b ON a.OUT_PAT_ID =b.PAT_ID INNER JOIN ZYCOMM.CHARGE_PRICE_ALL_VIEW c ON b.CHARGE_ID = c.CHARGE_ID  WHERE a.OUT_PAT_CODE='" + OutPatCode + "'");
            dtResult = sqlHelper.ExecSqlReDs(strSql.ToString()).Tables[0];
            return dtResult;
        }
        public DataTable OutPatIdToInvoiceCode(string OutPatID, string StrConn,string StartTime,string EndTime)
        {
            sqlHelper = new ORCLHelper(StrConn);
            DataTable dtResult = new DataTable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT 0 as 选择需要退费的发票号, INVOICE_CODE as 发票号,INVOICE_ID as 发票ID,AMOUNT as 发票金额 FROM ZYHIS.INVOICE_MAIN WHERE CREATE_TIME BETWEEN TO_DATE('" + StartTime + "', 'YYYY-MM-DD HH24:MI:SS') AND TO_DATE('" + EndTime + "', 'YYYY-MM-DD HH24:MI:SS') AND OUT_PAT_ID =" + OutPatID + " AND INVOICE_TYPE <10000 ");
            dtResult = sqlHelper.ExecSqlReDs(strSql.ToString()).Tables[0];
            return dtResult;

        }
        public DataTable GetInoviceDetails(string InvoiceList,string StrConn)
        {
            DataTable dtResult = new DataTable();
            sqlHelper = new ORCLHelper(StrConn);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT 0 as 选择需要退费的项目,b.CHARGE_NAME,a.QUANTITY AS 退费数量,a.QUANTITY AS 数量,a.AMOUNT AS 总额,(CASE WHEN b.CHARGE_TYPE >100 THEN 诊疗 WHEN b.CHARGE_TYPE<100 THEN 药品 END) AS 类型,a.INVOICE_DETAIL_ID AS 发票明细ID FROM");
            strSql.Append(" ZYHIS.INVOICE_DETAILS_VIEW a INNER JOIN ZYCOMM.CHARGE_PRICE_ALL_VIEW b ON a.CHARGE_ID = b.CHARGE_ID INNER JOIN ZYHIS.INVOICE_MAIN c ON a.INVOICE_ID =c.INVOICE_ID  WHERE A.INVOICE_ID in (" + InvoiceList + ") AND (c.INVOICE_TYPE >=1 AND c.INVOICE_TYPE <=10000)");
            dtResult = sqlHelper.ExecSqlReDs(strSql.ToString()).Tables[0];
            return dtResult;
        }
        public DataTable GetOutPatAllInfo(string name, string StrConn)
        {
            DataTable dtResult = new DataTable();
            sqlHelper = new ORCLHelper(StrConn);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT OUT_PAT_ID,OUT_PAT_CODE,CHARGE_CLASS_ID,OUT_PAT_NAME FROM ZYDICT.OUT_PATS WHERE OUT_PAT_NAME ='" + name + "' OR INPUT_CODE LIKE '" + name + "%'");
            dtResult = sqlHelper.ExecSqlReDs(strSql.ToString()).Tables[0];
            return dtResult;

        }
        public DataTable OutPatIDToOutPatCode(string OutPatID,string StrConn)
        {
            DataTable dtResult = new DataTable();
            sqlHelper = new ORCLHelper(StrConn);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT OUT_PAT_CODE FROM ZYDICT.OUT_PATS WHERE OUT_PAT_ID ='"+OutPatID+"'");
            dtResult = sqlHelper.ExecSqlReDs(strSql.ToString()).Tables[0];
            return dtResult;
        }
    }
   
}
