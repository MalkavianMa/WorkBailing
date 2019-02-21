using System;
using System.Collections.Generic;
using System.Text;
using RIClientTestDemo.DAL;
using System.Data;
using System.Windows.Forms;
using Config;

namespace RIClientTestDemo.BLL
{
    public class RIClientTestBLL
    {
        public RIClientTestDAL RICDAL;
        public RIClientTestDALORCL RICDALORCL;
        DataTable DtResult;
        public RIClientTestBLL()
        {

        }
        public DataTable PatInHosCodeToPatInHosID(string PatInHosCode,string StrConn)
        {
            DtResult = new DataTable();
            if (CfgInfoStatic.DataType != "ORACLE")
            {
                RICDAL = new RIClientTestDAL();
                DtResult = RICDAL.PatHosCodeConvertToPatHosId(PatInHosCode, StrConn);
            } 
            else
            {
                RICDALORCL = new RIClientTestDALORCL();
                DtResult = RICDALORCL.PatHosCodeConvertToPatHosId(PatInHosCode, StrConn);
            }
            
            return DtResult;
        }
        public DataTable QueryChargeClassId(string StrConn) 
        {
            DtResult = new DataTable();
            if (CfgInfoStatic.DataType != "ORACLE")
            {
                RICDAL = new RIClientTestDAL();
                DtResult = RICDAL.QueryChargeClassID(StrConn);
            }
            else
            {
                RICDALORCL = new RIClientTestDALORCL();
                DtResult = RICDALORCL.QueryChargeClassID(StrConn);
            }
            
            return DtResult;
        }

        public DataTable ChargeClassIdConvertToNetworkPatClassId(string ChargeClassId, string StrConn)
        {
            DtResult = new DataTable();
            if (CfgInfoStatic.DataType != "ORACLE")
            {
                RICDAL = new RIClientTestDAL();
                DtResult = RICDAL.ChargeClassIdConvertToNetworkPatClassId(ChargeClassId, StrConn);
            }
            else
            {
                //RICDALORCL = new RIClientTestDALORCL();
                //DtResult = RICDALORCL.ChargeClassIdConvertToNetworkPatClassId(ChargeClassId, StrConn);
            }

            return DtResult;
        }

        public DataTable OutPatCodeToOutPatID(string OutPatCode, string StrConn)
        {
            DtResult = new DataTable();
            if (CfgInfoStatic.DataType != "ORACLE")
            {
                RICDAL = new RIClientTestDAL();
                DtResult = RICDAL.OutPatCodeConvertToOutPatID(OutPatCode, StrConn);
            }
            else
            {
                RICDALORCL = new RIClientTestDALORCL();
                DtResult = RICDALORCL.OutPatCodeConvertToOutPatID(OutPatCode, StrConn);
            }
            
            return DtResult;
        }
        public DataTable AutoIDFromOutPatCode(string OutPatCode, string StrConn)
        {
            DtResult = new DataTable();
            if (CfgInfoStatic.DataType != "ORACLE")
            {
                RICDAL = new RIClientTestDAL();
                DtResult = RICDAL.OutPatCodeQueryAutoID(OutPatCode, StrConn);
            }
            else
            {
                RICDALORCL = new RIClientTestDALORCL();
                DtResult = RICDALORCL.OutPatCodeQueryAutoID(OutPatCode, StrConn);
            }
            
            return DtResult;
        }
        public DataTable OutPatCodeToInvoice(string OutPatID,string StrConn,string StartTime,string EndTime)
        {
            DtResult = new DataTable();
            if (CfgInfoStatic.DataType != "ORACLE")
            {
                RICDAL = new RIClientTestDAL();
                DtResult = RICDAL.OutPatIdToInvoiceCode(OutPatID, StrConn, StartTime, EndTime);
            }
            else
            {
                RICDALORCL = new RIClientTestDALORCL();
                DtResult = RICDALORCL.OutPatIdToInvoiceCode(OutPatID, StrConn, StartTime, EndTime);
            }
            
            return DtResult;
        }
        public DataTable InvoiceIDToInvoiceDetail(string InvoiceIdList,string StrConn)
        {
            DtResult = new DataTable();
            if (CfgInfoStatic.DataType != "ORACLE")
            {
                RICDAL = new RIClientTestDAL();
                DtResult = RICDAL.GetInoviceDetails(InvoiceIdList, StrConn);
            }
            else
            {
                RICDALORCL = new RIClientTestDALORCL();
                DtResult = RICDALORCL.GetInoviceDetails(InvoiceIdList, StrConn);
            }
            
            return DtResult;
        }
        public DataTable OutPatNameToAllInfo(string OutPatName,string StrConn)
        {
            DtResult = new DataTable();
            if (CfgInfoStatic.DataType != "ORACLE")
            {
                RICDAL = new RIClientTestDAL();
                DtResult = RICDAL.GetOutPatAllInfo(OutPatName, StrConn);
            }
            else
            {
                RICDALORCL = new RIClientTestDALORCL();
                DtResult = RICDALORCL.GetOutPatAllInfo(OutPatName, StrConn);
            }
            
            return DtResult;
        }
        public DataTable OutPatIDToOutPatCode(string OutPatCode,string StrConn) 
        {
            DtResult = new DataTable();
            if (CfgInfoStatic.DataType != "ORACLE")
            {
                RICDAL = new RIClientTestDAL();
                DtResult = RICDAL.OutPatIDToOutPatCode(OutPatCode, StrConn);
            }
            else
            {
                RICDALORCL = new RIClientTestDALORCL();
                DtResult = RICDALORCL.OutPatIDToOutPatCode(OutPatCode, StrConn);
            }
            
            return DtResult;
        }
    }
}
