using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using PayAPIClassLib;
using RIClientTestDemo.BLL;
using System.Collections;
using Config;
namespace RIClientTestDemo
{
    public partial class TestDemo : Form
    {
        public RIClientTestBLL RICBll;
        public string PatInHosID = "";
        public string WebApiAddress = "";
        public string OutPatID = "-1";
        public string PatInHosChargeClassID = "";
        private CfgInfo cInfo = new Config.CfgInfo();
        private ClientConfig cconfig = new Config.ClientConfig();


        public TestDemo()
        {
            InitializeComponent();
        }

        private void TestDemo_Load(object sender, EventArgs e)
        {
            int _nTimes = 0;
            if (cconfig.OpenConfigSet())
            {
                try
                {
                    cInfo = cconfig.ReadConfig();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("配置文件读取失败!" + ex.Message);
                    return;
                }
            }
            else
            {
                frmSetConfig frmSC = new frmSetConfig(cconfig.fileFullName);
                frmSC.ShowDialog();
                if (frmSC.DialogResult == DialogResult.OK)
                {
                    if (_nTimes < 2)
                    {
                        _nTimes++;
                        TestDemo_Load(this, new EventArgs());
                    }
                    return;
                }
                else
                {
                    this.Close();
                }
            }
            WebApiAddress = cInfo.strSvrIP;
            if (WebApiAddress == ".")
            {
                WebApiAddress = "http://localhost:81/";
            }
            else
            {
                WebApiAddress = "http://" + WebApiAddress + "/PayWebApi";
            }
            txtUserSysID.Text = cInfo.UserSysId;
            txtWebApiAddress.Text = WebApiAddress;
            //tbHosId.Text = cInfo.strHosID;
            //tbDeptId.Text = cInfo.strDeptID;
            FillChargeClassID();
            //dataTime_StartTime.Value = DateTime.Now;
            //dateTime_EndTime.Value = DateTime.Now;
            //this.cmb_QuerySelection.SelectedIndex = 0;
            dgrv_SelectName.Left = txt_QueryContent.Left;
        }

        /// <summary>
        /// 费别列表
        /// </summary>
        public void FillChargeClassID()
        {
            RICBll = new RIClientTestBLL();
            DataTable dt = RICBll.QueryChargeClassId(cInfo.strConn);
            ArrayList arry_2 = new ArrayList();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                arry_2.Add(new DictionaryEntry(dt.Rows[i]["CHARGE_CLASS_ID"].ToString(), dt.Rows[i]["CHARGE_CLASS_NAME"].ToString()));
            }
            cmbChargeClassId.DataSource = arry_2;
            cmbChargeClassId.DisplayMember = "Value";
            cmbChargeClassId.ValueMember = "Key";

        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Init_Click(object sender, EventArgs e)
        {
            PayAPIUtilities.Config.PayAPIConfig.DebugMode = chkDebugMode.Checked;

            PayAPIClassLib.ParaModel.ClientInitParameter clientInitParameter = new PayAPIClassLib.ParaModel.ClientInitParameter();
            clientInitParameter.TradeCode = "Init";
            clientInitParameter.TradeType = "";
            clientInitParameter.TradeId = -1;
            clientInitParameter.TradeSource = "";
            clientInitParameter.NetworkPatClassId = "-1";
            clientInitParameter.UserSysId = txtUserSysID.Text.Trim();
            clientInitParameter.WebApiAddress = txtWebApiAddress.Text.Trim();
            clientInitParameter.WorkStationId = "-1";
            string tradeInfo = PayAPIUtilities.WebAPI.JsonHelper.SerializeObject(clientInitParameter);
            //string tradeInfo = "{\"TradeCode\":\"Init\",\"TradeType\":\"\",\"TradeId\":-1,\"TradeSource\":\"\",\"NetworkPatClassId\":-1,\"UserSysId\":" + txtUserSysID.Text.Trim() + ",\"WebApiAddress\":\"" + txtWebApiAddress.Text.Trim() + "\",\"WorkStationId\":-1}";
            string strRe = ucPayInterfaceTest.ZYTrade(tradeInfo);
            MessageBox.Show(strRe);
        }

        private void tbPatInHosCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                RICBll = new RIClientTestBLL();
                DataTable dt = RICBll.PatInHosCodeToPatInHosID(tbPatInHosCode.Text, cInfo.strConn);
                ArrayList arry_1 = new ArrayList();
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("输入的住院号不正确");
                }
                else
                {
                    txt_PatInChargeClassID.Text = dt.Rows[0]["CHARGE_CLASS_NAME"].ToString();
                    txt_pat_in_name.Text = dt.Rows[0]["IN_PAT_NAME"].ToString();
                    PatInHosChargeClassID = dt.Rows[0]["CHARGE_CLASS_ID"].ToString();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        arry_1.Add(new DictionaryEntry(dt.Rows[i]["PAT_IN_HOS_ID"], dt.Rows[i]["PAT_AGAIN_IN_TIMES"]));
                    }
                    Cmb_In_Times.DataSource = arry_1;
                    Cmb_In_Times.ValueMember = "Key";
                    Cmb_In_Times.DisplayMember = "Value";
                }
            }
        }

        /// <summary>
        /// 住院交易
        /// </summary>
        /// <param name="TradeCode"></param>
        private void InTrade(string TradeCode, string networkPatClassId = "-1")
        {
            PatInHosID = Cmb_In_Times.SelectedValue.ToString();

            if (PatInHosID == "")
            {
                MessageBox.Show("请先查询住院病人信息再操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string chargeClassId = PatInHosChargeClassID;

            if (networkPatClassId == "-1")
            {
                DataTable dt = RICBll.ChargeClassIdConvertToNetworkPatClassId(chargeClassId, cInfo.strConn);
                if (dt.Rows.Count > 0)
                {
                    networkPatClassId = dt.Rows[0]["NETWORKING_PAT_CLASS_ID"].ToString();
                }
            }

            PayAPIClassLib.ParaModel.ClientInParameter clientInParameter = new PayAPIClassLib.ParaModel.ClientInParameter();
            clientInParameter.TradeCode = TradeCode;
            clientInParameter.TradeType = "";
            clientInParameter.TradeId = -1;
            clientInParameter.TradeSource = "";
            clientInParameter.NetBillNo = tbSettleNo.Text;
            clientInParameter.NetworkPatClassId = networkPatClassId;
            clientInParameter.PatInHosId = Convert.ToDecimal(PatInHosID);
            clientInParameter.TradeAmount = 0.01M;

            string tradeInfo = PayAPIUtilities.WebAPI.JsonHelper.SerializeObject(clientInParameter);
            string strRe = ucPayInterfaceTest.ZYTrade(tradeInfo);
            MessageBox.Show(strRe);
        }

        /// <summary>
        /// 住院登记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInRegister_Click(object sender, EventArgs e)
        {
            InTrade("ZYReg");
        }

        /// <summary>
        /// 取消住院登记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelInRegister_Click(object sender, EventArgs e)
        {
            InTrade("ZYRegCancel");
        }

        /// <summary>
        /// 住院预结算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInPreSettle_Click(object sender, EventArgs e)
        {
            InTrade("ZYPreSettle");
        }

        /// <summary>
        /// 住院结算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInSettle_Click(object sender, EventArgs e)
        {
            InTrade("ZYSettle");
        }

        /// <summary>
        /// 取消住院结算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelInSettle_Click(object sender, EventArgs e)
        {
            InTrade("ZYSettleCancel");
        }

        //-----------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            PayAPIClassLib.ParaModel.ClientOutParameter clientOutParameter = new PayAPIClassLib.ParaModel.ClientOutParameter();
            clientOutParameter.TradeCode = "MZCash";
            clientOutParameter.TradeType = "";
            clientOutParameter.TradeId = 11;
            clientOutParameter.TradeSource = "";
            clientOutParameter.NetworkPatClassId = "1";
            clientOutParameter.OutPatId = 311;

            PayAPIUtilities.Config.PayAPIConfig.DebugMode = true;

            string tradeInfo = PayAPIUtilities.WebAPI.JsonHelper.SerializeObject(clientOutParameter);

            string strRe = ucPayInterfaceTest.ZYTrade(tradeInfo);
            MessageBox.Show(strRe);
        }
        //-------------------------------------------------------------
        private void txt_QueryContent_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar != 13)
            {
                return;
            }
            if (dgrv_SelectName.Visible == true)
            {
                dgrv_SelectName_DoubleClick(this, e);
            }

            if (cmb_QuerySelection.Text == "OUT_PAT_ID")
            {
                RICBll = new RIClientTestBLL();
                DataTable dt = new DataTable();
                dt = RICBll.OutPatIDToOutPatCode(txt_QueryContent.Text, cInfo.strConn);
                if (dt.Rows.Count != 0)
                {
                    txtOutPatCode.Text = dt.Rows[0]["OUT_PAT_CODE"].ToString();

                }
                else
                {
                    return;

                }

            }
            if (cmb_QuerySelection.Text == "就诊号")
            {
                txtOutPatCode.Text = txt_QueryContent.Text;

            }

            qurey();
        }
        private void txt_QueryContent_TextChanged(object sender, EventArgs e)
        {
            if (cmb_QuerySelection.Text == "姓名")
            {

                if (txt_QueryContent.Focused == false)
                {
                    dgrv_SelectName.Visible = false;
                    return;
                }

                if (txt_QueryContent.Text == "")
                {
                    dgrv_SelectName.Visible = false;
                    return;
                }
                //筛选
                RICBll = new RIClientTestBLL();
                DataTable dt = new DataTable();
                dt = RICBll.OutPatNameToAllInfo(txt_QueryContent.Text, cInfo.strConn);
                if (dt.Rows.Count > 0)
                {
                    dgrv_SelectName.Visible = true;
                }
                else
                {
                    dgrv_SelectName.Visible = false;
                }
                dgrv_SelectName.DataSource = dt;  //将datagirdview的AutoSizeColumnsMode属性改为AllCells根据内容自动调整宽度,但数据量大速度慢
                dgrv_SelectName.Refresh();
            }

        }

        private void txt_QueryContent_KeyDown(object sender, KeyEventArgs e)
        {
            //上下箭头移动datagridview当前行

            if (e.KeyCode == Keys.Down)
            {
                System.Windows.Forms.SendKeys.Send("{END}");  //发送end键把光标挪到最后

                if (this.dgrv_SelectName.Visible == true)
                {
                    BindingManagerBase bm = dgrv_SelectName.BindingContext[dgrv_SelectName.DataSource];
                    bm.Position += 1;
                }
            }

            if (e.KeyCode == Keys.Up)
            {
                System.Windows.Forms.SendKeys.Send("{END}");
                if (this.dgrv_SelectName.Visible == true)
                {
                    BindingManagerBase bm = dgrv_SelectName.BindingContext[dgrv_SelectName.DataSource];
                    bm.Position -= 1;
                }
            }
        }

        private void dgrv_SelectName_DoubleClick(object sender, EventArgs e)
        {
            if (dgrv_SelectName.CurrentRow == null)
            {
                return;
            }



            txtOutPatCode.Text = dgrv_SelectName.CurrentRow.Cells[1].Value.ToString();
            cmbChargeClassId.SelectedValue = dgrv_SelectName.CurrentRow.Cells[2].Value.ToString();
            txt_QueryContent.Text = dgrv_SelectName.CurrentRow.Cells[3].Value.ToString();
            txt_OutPatName.Text = txt_QueryContent.Text;


            dgrv_SelectName.Visible = false;
            qurey();
        }
        //-------------------------------------------------------------


        private void txtOutPatCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                qurey();
            }
        }

        private void qurey()
        {
            RICBll = new RIClientTestBLL();
            DataTable dt = RICBll.OutPatCodeToOutPatID(txtOutPatCode.Text, cInfo.strConn);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("输入的就诊号:   " + txtOutPatCode.Text + "   错误，查无此病人");
                txt_OutPatName.Text = "";
            }
            else
            {
                cmbChargeClassId.SelectedValue = dt.Rows[0]["CHARGE_CLASS_ID"].ToString();
                OutPatID = dt.Rows[0]["OUT_PAT_ID"].ToString();
                txt_OutPatName.Text = dt.Rows[0]["OUT_PAT_NAME"].ToString();
                //txtAutoIds_Click(this, new EventArgs());
            }
            dt = new DataTable();
            //dt = RICBll.OutPatCodeToInvoice(OutPatID, cInfo.strConn, dataTime_StartTime.Value.ToString("yyyy-MM-dd 00:00:00"), dateTime_EndTime.Value.ToString("yyyy-MM-dd 23:59:59"));
            if (dt.Rows.Count == 0)
            {
                return;
            }
            else
            {
                //dgrv_Invoice.DataSource = dt;
            }
        }

        /// <summary>
        /// 医保读卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadCard_Click(object sender, EventArgs e)
        {

            string chargeClassId = cmbChargeClassId.SelectedValue.ToString();

            string networkPatClassId = "-1";

            DataTable dt = RICBll.ChargeClassIdConvertToNetworkPatClassId(chargeClassId, cInfo.strConn);
            if (dt.Rows.Count > 0)
            {
                networkPatClassId = dt.Rows[0]["NETWORKING_PAT_CLASS_ID"].ToString();
            }

            PayAPIClassLib.ParaModel.ClientParameter clientParameter = new PayAPIClassLib.ParaModel.ClientParameter();
            clientParameter.TradeCode = "ReadCard";
            clientParameter.TradeType = "";
            clientParameter.TradeId = -1;
            clientParameter.TradeSource = "";
            clientParameter.NetworkPatClassId = networkPatClassId;



            string tradeInfo = PayAPIUtilities.WebAPI.JsonHelper.SerializeObject(clientParameter);
            string strRe = ucPayInterfaceTest.ZYTrade(tradeInfo);
            MessageBox.Show(strRe);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TradeCode"></param>
        /// <param name="TradeId"></param>
        /// <param name="chargeIds"></param>
        private void OutTrade(string TradeCode, string TradeId, string chargeIds = "", string networkBillNo = "", string networkPatClassId = "")
        {

            if (OutPatID == "")
            {
                MessageBox.Show("请先查询门诊病人信息再操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string chargeClassId = cmbChargeClassId.SelectedValue.ToString();

            if (networkPatClassId.Length == 0)
            {
                DataTable dt = RICBll.ChargeClassIdConvertToNetworkPatClassId(chargeClassId, cInfo.strConn);
                if (dt.Rows.Count > 0)
                {
                    networkPatClassId = dt.Rows[0]["NETWORKING_PAT_CLASS_ID"].ToString();
                }
            }

            PayAPIClassLib.ParaModel.ClientOutParameter clientOutParameter = new PayAPIClassLib.ParaModel.ClientOutParameter();
            clientOutParameter.TradeCode = TradeCode;
            clientOutParameter.TradeType = "YB";
            clientOutParameter.TradeId = Convert.ToDecimal(TradeId);
            clientOutParameter.TradeSource = "";
            clientOutParameter.NetworkPatClassId = networkPatClassId;
            clientOutParameter.RegisterChargeIds = chargeIds;
            clientOutParameter.OutPatId = Convert.ToDecimal(OutPatID);
            clientOutParameter.NetWorkBillNo = networkBillNo;
            clientOutParameter.TradeAmount = 10;

            string tradeInfo = PayAPIUtilities.WebAPI.JsonHelper.SerializeObject(clientOutParameter);

            string strRe = ucPayInterfaceTest.ZYTrade(tradeInfo);
            MessageBox.Show(strRe);
        }

        /// <summary>
        /// 门诊结算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOutSettle_Click(object sender, EventArgs e)
        {
            OutTrade("MZCash", "6676");
        }

        private void btnOutRefund_Click(object sender, EventArgs e)
        {
            OutTrade("MZCashRefund", "56");
            //

        }

        private void chkDebugMode_CheckedChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 挂号费报销
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegSettle_Click(object sender, EventArgs e)
        {
            OutTrade("MZReg", "-1", "8669,9461");
        }

        private void btnRegSettleCancel_Click(object sender, EventArgs e)
        {
            OutTrade("MZRegRefund", "-1", "", "MZ140.0");
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            OutTrade("PayOrder", "6", "", "", "2");
        }

        private void btnPayCancel_Click(object sender, EventArgs e)
        {
            OutTrade("PayOrderRefund", "5", "", "", "2");
        }

        private void btnCancelTrade_Click(object sender, EventArgs e)
        {
            OutTrade("MZCashCancel", "2");
        }

        private void btnInPay_Click(object sender, EventArgs e)
        {
            InTrade("InPayOrder","2");
        }

        private void btnInPayCancel_Click(object sender, EventArgs e)
        {
            InTrade("InPayOrderCancel", "2");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //dWInterfaceModel.LoadXML();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //dWInterfaceModel.LoadXML2();

        }

        private void button4_Click(object sender, EventArgs e)
        {

            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Form1 fr = new Form1("5", "10000005");
            //fr.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        private void txtWebApiAddress_TextChanged(object sender, EventArgs e)
        {

        }
        //-------------------------
    }
}
