using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections; 
using System.Runtime.InteropServices;
using System.Xml; 
using PayAPIInterface.ParaModel;
using PayAPIInterfaceHandle.AHFYCityWebReference;

namespace PayAPIInstance.JingQi.FuYang.Dialogs
{
    public partial class MzInvoiceInput : Form
    {

        public MSSQLHelper sqlHelper = new MSSQLHelper(PubComm.ConnStr);

        IJQCenterWebServiceservice serr = new IJQCenterWebServiceservice();

        public string sHospitalCode = "5AB552AAF3DB47E055E06177CF51A5C4";   //医疗机构编号

        private FYNHCityInterfaceModel model;
        public  InPayParameter inReimPara;//住院患者信息
        public  OutPayParameter outReimPara;
        public  NetPatInfo netPatInfo;

        public string sAreaCode = "";        //地区代码,根据操作员选择进行赋值



        public MzInvoiceInput()
        {
            InitializeComponent();
        }

        public MzInvoiceInput(InPayParameter para, NetPatInfo patInfo, string AreaCode)
        {
            InitializeComponent();
            this.model = new FYNHCityInterfaceModel();
            this.inReimPara = para;
            this.netPatInfo = patInfo;
            this.sAreaCode = AreaCode;

        }



        private void tbInvoiceCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbInvoiceCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {


                if (this.tbInvoiceCode.Text.Trim().Length == 0)
                {
                    MessageBox.Show("请输入正确的发票号码！");
                }


                StringBuilder strsql = new StringBuilder();
                strsql.Append("SELECT B.CREATE_TIME 记账时间,CHARGE_NAME 项目名称,B.PRICE 单价,QUANTITY 数量,MEASURE_UNIT_NAME 单位,B.AMOUNT 金额, ");
                strsql.Append("INVOICE_NAME 项目归属,E.DEPT_NAME 开单科室,UESR_NAME 开单医生,INVOICE_CODE ,A.AMOUNT 总金额 ");
                strsql.Append("FROM MZ.OUT.INVOICE_MAIN A INNER JOIN MZ.OUT.INVOICE_DETAILS_CHARGE B ON A.INVOICE_ID = B.INVOICE_ID ");
                strsql.Append("INNER JOIN COMM.COMM.CHARGE_PRICE_ALL_VIEW C ON B.CHARGE_ID=C.CHARGE_ID ");
                strsql.Append("INNER JOIN COMM.COMM.CHARGE_INVOICES D ON C.INVOICE_ITEM_ID = D.INVOICE_ITEM_ID ");
                strsql.Append("INNER JOIN COMM.COMM.DEPTS E ON B.BILL_DEPT_ID=E.DEPT_ID ");
                strsql.Append("INNER JOIN COMM.COMM.USERINFO_VIEW F ON DOC_SYS_ID=F.USER_SYS_ID ");
                strsql.Append("WHERE INVOICE_CODE='" + this.tbInvoiceCode.Text.Trim() + "'");



                try
                {

                    //获取家庭成员信息
                    DataSet ds = sqlHelper.ExecSqlReDs(strsql.ToString());


                    if (ds.Tables[0].Rows.Count != 0)
                    {

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            object[] row;
                            row = new object[] { ds.Tables[0].Rows[i]["项目名称"].ToString(), ds.Tables[0].Rows[i]["单价"].ToString(), ds.Tables[0].Rows[i]["数量"].ToString(), ds.Tables[0].Rows[i]["单位"].ToString(), ds.Tables[0].Rows[i]["金额"].ToString() 
                            ,ds.Tables[0].Rows[i]["项目归属"].ToString(),ds.Tables[0].Rows[i]["开单科室"].ToString(),ds.Tables[0].Rows[i]["开单医生"].ToString(),ds.Tables[0].Rows[i]["记账时间"].ToString()};
                            this.dgvInvoiceDetail.Rows.Add(row);
                        }

                    }

                    this.dgvInvoiceDetail.Focus();
                }
                catch (System.Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        private void btnFysc_Click(object sender, EventArgs e)
        {


            StringBuilder strsql = new StringBuilder();
            strsql.Append("SELECT B.CREATE_TIME 记账时间,CHARGE_CODE HIS编码,CHARGE_NAME 项目名称,B.PRICE 单价,QUANTITY 数量,MEASURE_UNIT_NAME 单位,B.AMOUNT 金额, ");
            strsql.Append("INVOICE_NAME 项目归属,E.DEPT_NAME 开单科室,UESR_NAME 开单医生,INVOICE_CODE ,A.AMOUNT 总金额,NETWORK_ITEM_CODE 中心编码,NETWORK_ITEM_NAME 中心名称,NETWORK_ITEM_CHARGE_CLASS 中心类别 ");
            strsql.Append("FROM MZ.OUT.INVOICE_MAIN A INNER JOIN MZ.OUT.INVOICE_DETAILS_CHARGE B ON A.INVOICE_ID = B.INVOICE_ID ");
            strsql.Append("INNER JOIN COMM.COMM.CHARGE_PRICE C ON B.CHARGE_ID=C.CHARGE_ID ");
            strsql.Append("INNER JOIN COMM.COMM.CHARGE_INVOICES D ON C.INVOICE_ITEM_ID = D.INVOICE_ITEM_ID ");
            strsql.Append("INNER JOIN COMM.COMM.DEPTS E ON B.BILL_DEPT_ID=E.DEPT_ID ");
            strsql.Append("INNER JOIN COMM.COMM.USERINFO_VIEW F ON DOC_SYS_ID=F.USER_SYS_ID ");

            strsql.Append("LEFT JOIN COMM.DICT.CHARGE_CLASSES H ON  H.CHARGE_CLASS_ID='" + sAreaCode + "'");

            strsql.Append("LEFT JOIN (SELECT NETWORKING_PAT_CLASS_ID,ITEM_PROP,HIS_ITEM_CODE,HIS_ITEM_NAME,NETWORK_ITEM_CODE,NETWORK_ITEM_PROP,MAX(NETWORK_ITEM_NAME) NETWORK_ITEM_NAME, NETWORK_ITEM_CHARGE_CLASS,min ");
            strsql.Append("(self_burden_ratio)self_burden_ratio FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WITH ( NOLOCK )  group by NETWORKING_PAT_CLASS_ID,ITEM_PROP,HIS_ITEM_CODE, ");
            strsql.Append("HIS_ITEM_NAME,NETWORK_ITEM_CODE,NETWORK_ITEM_PROP, NETWORK_ITEM_CHARGE_CLASS) G ON C.CHARGE_CODE=G.HIS_ITEM_CODE  AND G.NETWORKING_PAT_CLASS_ID = H.NETWORKING_PAT_CLASS_ID ");

            strsql.Append("WHERE INVOICE_CODE='" + this.tbInvoiceCode.Text.Trim() + "' AND B.FLAG_SUPER=0  ");

            strsql.Append("UNION ALL ");

            strsql.Append("SELECT B.CREATE_TIME 记账时间,J.CHARGE_CODE HIS编码,J.CHARGE_NAME 项目名称,J.PRICE 单价,QUANTITY*I.CHILD_AMOUNT 数量,J.MEASURE_UNIT_NAME 单位,CHILD_PRICE*QUANTITY*I.CHILD_AMOUNT 金额, ");
            strsql.Append("INVOICE_NAME 项目归属,E.DEPT_NAME 开单科室,UESR_NAME 开单医生,INVOICE_CODE ,A.AMOUNT 总金额,ISNULL(NETWORK_ITEM_CODE,'') 中心编码,NETWORK_ITEM_NAME 中心名称,NETWORK_ITEM_CHARGE_CLASS 中心类别 ");
            strsql.Append("FROM MZ.OUT.INVOICE_MAIN A INNER JOIN MZ.OUT.INVOICE_DETAILS_CHARGE B ON A.INVOICE_ID = B.INVOICE_ID ");
            strsql.Append("INNER JOIN COMM.COMM.CHARGE_PRICE C ON B.CHARGE_ID=C.CHARGE_ID ");
            strsql.Append("INNER JOIN comm.COMM.CHARGE_GROUP I ON C.CHARGE_ID=I.CHARGE_PRICE_ID ");
            strsql.Append("INNER JOIN COMM.COMM.CHARGE_PRICE J ON I.CHILD_PRICE_ID=J.CHARGE_ID ");
            strsql.Append("INNER JOIN COMM.COMM.CHARGE_INVOICES D ON J.INVOICE_ITEM_ID = D.INVOICE_ITEM_ID ");
            strsql.Append("INNER JOIN COMM.COMM.DEPTS E ON B.BILL_DEPT_ID=E.DEPT_ID ");
            strsql.Append("INNER JOIN COMM.COMM.USERINFO_VIEW F ON DOC_SYS_ID=F.USER_SYS_ID ");
            strsql.Append("LEFT JOIN COMM.DICT.CHARGE_CLASSES H ON  H.CHARGE_CLASS_ID='" + sAreaCode + "'");
            strsql.Append("LEFT JOIN (SELECT NETWORKING_PAT_CLASS_ID,ITEM_PROP,HIS_ITEM_CODE,HIS_ITEM_NAME,NETWORK_ITEM_CODE,NETWORK_ITEM_PROP,MAX(NETWORK_ITEM_NAME) NETWORK_ITEM_NAME, NETWORK_ITEM_CHARGE_CLASS,min ");
            strsql.Append("(self_burden_ratio)self_burden_ratio FROM COMM.COMM.NETWORKING_ITEM_VS_HIS WITH ( NOLOCK )  group by NETWORKING_PAT_CLASS_ID,ITEM_PROP,HIS_ITEM_CODE, ");
            strsql.Append("HIS_ITEM_NAME,NETWORK_ITEM_CODE,NETWORK_ITEM_PROP, NETWORK_ITEM_CHARGE_CLASS) G ON J.CHARGE_CODE=G.HIS_ITEM_CODE AND G.NETWORKING_PAT_CLASS_ID = H.NETWORKING_PAT_CLASS_ID ");
            
            strsql.Append("WHERE INVOICE_CODE='" + this.tbInvoiceCode.Text.Trim() + "' AND B.FLAG_SUPER=1 ");

            DataTable InvoiceDetail = sqlHelper.ExecSqlReDs(strsql.ToString()).Tables[0];

            #region 如果费用明细里有未和农合对应的情况，则抛出异常终止操作
            DataRowCollection drInUpdateTable = InvoiceDetail.Rows;
            string notMatchedCharge = "";
            foreach (DataRow dr in drInUpdateTable)
            {
                if (dr["中心编码"].ToString().Trim().Length == 0)
                {
                    notMatchedCharge += "编码:" + dr["HIS编码"] + "," + "名称:" + dr["项目名称"] + "；";
                }
            }
            if (notMatchedCharge.Trim().Length > 0)
            {
                if (MessageBox.Show("有以下项目未对应：\n" + notMatchedCharge + "\n是否继续？选择”是“将按自费项目进行收费报销。否则，取消本次收费报销！", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    throw new Exception("取消上传费用");
                }
            }
            #endregion


            string sMessage = ""; string sResult = ""; float kbxje = 0;

            for (int i = 0; i < InvoiceDetail.Rows.Count; i++)
            {

                int res = serr.InpatientAddFee(sAreaCode, inReimPara.RegInfo.NetRegSerial, sHospitalCode, "0035", InvoiceDetail.Rows[i]["中心编码"].ToString(), InvoiceDetail.Rows[i]["中心名称"].ToString(), InvoiceDetail.Rows[i]["数量"].ToString(), InvoiceDetail.Rows[i]["单位"].ToString(), InvoiceDetail.Rows[i]["单价"].ToString(), InvoiceDetail.Rows[i]["金额"].ToString(), inReimPara.PatInfo.InDeptName, inReimPara.PatInfo.InNetWorkDeptCode, inReimPara.PatInfo.DoctorName, "", Convert.ToDateTime(InvoiceDetail.Rows[i]["记账时间"]).ToString("yyyy-MM-dd hh:mm:ss"), inReimPara.PatInfo.DoctorName, "", "", "", "", "", ref sResult, ref sMessage);
                if (res == 0)
                {
                    throw new Exception(sMessage);
                }

                XmlDocument ReResulst = new XmlDocument();
                string result = sResult;                                                           //进行医保业务
                ReResulst.LoadXml(result);
                int rowCount = int.Parse(ReResulst.SelectSingleNode("/JQWebService/RowCount").InnerText);
                if (rowCount != 0)
                {
                    float m = float.Parse(ReResulst.SelectSingleNode("/JQWebService/Row1/sApply").InnerText);

                    kbxje = kbxje + m;
                }
            }

            MessageBox.Show("上传成功，可报销金额为：" + kbxje.ToString());

        }


        private void btnquery_Click(object sender, EventArgs e)
        {
            if (this.tbInvoiceCode.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入正确的发票号码！");
            }



            StringBuilder strsql = new StringBuilder();
            strsql.Append("SELECT B.CREATE_TIME 记账时间,CHARGE_NAME 项目名称,B.PRICE 单价,QUANTITY 数量,MEASURE_UNIT_NAME 单位,B.AMOUNT 金额, ");
            strsql.Append("INVOICE_NAME 项目归属,E.DEPT_NAME 开单科室,UESR_NAME 开单医生,INVOICE_CODE ,A.AMOUNT 总金额 ");
            strsql.Append("FROM MZ.OUT.INVOICE_MAIN A INNER JOIN MZ.OUT.INVOICE_DETAILS_CHARGE B ON A.INVOICE_ID = B.INVOICE_ID ");
            strsql.Append("INNER JOIN COMM.COMM.CHARGE_PRICE C ON B.CHARGE_ID=C.CHARGE_ID ");
            strsql.Append("INNER JOIN COMM.COMM.CHARGE_INVOICES D ON C.INVOICE_ITEM_ID = D.INVOICE_ITEM_ID ");
            strsql.Append("INNER JOIN COMM.COMM.DEPTS E ON B.BILL_DEPT_ID=E.DEPT_ID ");
            strsql.Append("INNER JOIN COMM.COMM.USERINFO_VIEW F ON DOC_SYS_ID=F.USER_SYS_ID ");
            strsql.Append("WHERE INVOICE_CODE='" + this.tbInvoiceCode.Text.Trim() + "' ");


            try
            {

                //获取家庭成员信息
                DataSet ds = sqlHelper.ExecSqlReDs(strsql.ToString());


                if (ds.Tables[0].Rows.Count != 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        object[] row;
                        row = new object[] { ds.Tables[0].Rows[i]["项目名称"].ToString(), ds.Tables[0].Rows[i]["单价"].ToString(), ds.Tables[0].Rows[i]["数量"].ToString(), ds.Tables[0].Rows[i]["单位"].ToString(), ds.Tables[0].Rows[i]["金额"].ToString() 
                            ,ds.Tables[0].Rows[i]["项目归属"].ToString(),ds.Tables[0].Rows[i]["开单科室"].ToString(),ds.Tables[0].Rows[i]["开单医生"].ToString(),ds.Tables[0].Rows[i]["记账时间"].ToString()};
                        this.dgvInvoiceDetail.Rows.Add(row);
                    }
                    tbAmount.Text = ds.Tables[0].Rows[0]["总金额"].ToString();
                }

                this.dgvInvoiceDetail.Focus();
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }





    }
}
