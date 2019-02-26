using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PayAPIInterface.Model.Comm;
using PayAPIInterface.Model.Out;

namespace PayAPIInstance.Neusoft.Dialogs
{
    public partial class InfoTongChou1 : Form
    {
        private MSSQLHelper sqlHelperHis = new MSSQLHelper("Data Source=222.222.22.112;Initial Catalog=comm;Persist Security Info=True;User ID=power;Password=m@ssunsoft009");
        private bool isOut = false;

        public bool isOk = false;//是否存在结果
        public string TCTypeCode = "";  //统筹区号ID
        public string TCTypeName = "";  //统筹区号名称
        public NetworkPatInfo netPatInfo;  //医保个人信息 
        public OutNetworkRegisters outnetworkregisters;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isOut">true门诊false住院</param>
        public InfoTongChou1(bool IsOut, NetworkPatInfo patInfo, OutNetworkRegisters outnetworkregisters)
        {
            InitializeComponent();
            this.isOut = IsOut;
            this.netPatInfo = patInfo;
            this.outnetworkregisters = outnetworkregisters;
        }
        private void btnOk_Click(object sender, EventArgs e)
        {

            if (txtAreaName.Text == "")
            {
                MessageBox.Show("请选择统筹地区");
                return;
            }
            else
            {
                // TCTypeCode = txtAreaName.Text;  //把选中的统筹区号赋值给变量TCTypeCode
                this.Close();
            }

        }

        private void tbAreaName_TextChanged(object sender, EventArgs e)
        {
            if (txtAreaName.Focused == false)
            {
                dGV_ZD.Visible = false;
                return;
            }

            if (txtAreaName.Text == "")
            {
                dGV_ZD.Visible = false;
                return;
            }
            string strsql = @"SELECT 代码 as 编码 ,名称--,JianPin as 检索码,QuanPin as 全拼 
            FROM REPORT.dbo.AreaCode 
            WHERE  (JianPin LIKE '%" + txtAreaName.Text.Trim() + @"%' 
            OR 名称 LIKE '%" + txtAreaName.Text.Trim() + @"%' 
            OR 代码 LIKE '%" + txtAreaName.Text.Trim() + @"%'
            OR QuanPin LIKE '%" + txtAreaName.Text.Trim() + @"%'
            ) 
            ORDER BY CHARINDEX('" + txtAreaName.Text.Trim() + "',JianPin),LEN(JianPin),JianPin";
            DataSet ds = sqlHelperHis.ExecSqlReDs(strsql);

            if (ds.Tables[0].Rows.Count > 0)
            {
                dGV_ZD.Visible = true;
            }
            else
            {
                dGV_ZD.Visible = false;
            }
            dGV_ZD.DataSource = ds.Tables[0];//将datagirdview的AutoSizeColumnsMode属性改为AllCells根据内容自动调整宽度,但数据量大速度慢
            dGV_ZD.Refresh();
            dGV_ZD.Columns[0].Width = 80;
            dGV_ZD.Columns[1].Width = 150;
            dGV_ZD.Left = label1.Left + 20;
            dGV_ZD.Top = txtAreaName.Top + 25 + txtAreaName.Height;
        }

        private void tbAreaName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {

                dGV_ZD_DoubleClick(this, e);

            }
        }
        private void dGV_ZD_DoubleClick(object sender, EventArgs e)
        {
            if (dGV_ZD.CurrentRow == null)
            {
                return;
            }

            TCTypeCode = dGV_ZD.CurrentRow.Cells[0].Value.ToString();
            txtAreaName.Text = dGV_ZD.CurrentRow.Cells[1].Value.ToString();

            dGV_ZD.Visible = false;
            btnOk.Focus();

        }

        private void tbAreaName_KeyDown(object sender, KeyEventArgs e)
        {
            //上下箭头移动datagridview当前行

            if (e.KeyCode == Keys.Down)
            {
                System.Windows.Forms.SendKeys.Send("{END}");  //发送end键把光标挪到最后

                if (this.dGV_ZD.Visible == true)
                {
                    BindingManagerBase bm = dGV_ZD.BindingContext[dGV_ZD.DataSource];
                    bm.Position += 1;
                }
            }

            if (e.KeyCode == Keys.Up)
            {
                System.Windows.Forms.SendKeys.Send("{END}");
                if (this.dGV_ZD.Visible == true)
                {
                    BindingManagerBase bm = dGV_ZD.BindingContext[dGV_ZD.DataSource];
                    bm.Position -= 1;
                }
            }
        }
    }
}
