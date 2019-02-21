using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Runtime.InteropServices;

using PayAPIInterface.Model.In;
using DataGridSort;
using PayAPIInterfaceHandle.Dareway.ShanDong;


namespace PayAPIInstance.Dareway.ShanDong.Dialog
{
    public partial class frm_FYSH : Form
    {
        public bool isOk = false;//用户是否取消操作

        public DarewayModel model;

        public int col = 0;

        public double upJE=0;//上传金额
        //double selJE=0;//指定自费金额
        //double kbJE = 0;//可报金额

        public frm_FYSH()
        {
            InitializeComponent();
        }

        public frm_FYSH(DarewayModel _model)
        {
            InitializeComponent();

           
            model = _model;

            dGV_FYSH.AutoGenerateColumns = false;//取消自动绑定列
            //dGV_FYSH.DataSource = model.InPayPara.Details;
            dGV_FYSH.DataSource = new BindingCollection<InNetworkUpDetail>(model.InPayPara.Details);  //datagridview绑定List,点列表题排序
            dGV_FYSH.Refresh();
        }

        public string str_Name = "";
        public string str_ZYH = "";
        public bool IsAutoEXE = false;

        private void frm_FYSH_Load(object sender, EventArgs e)
        {

            dGV_FYSH.CausesValidation = false;

            frm_FYSH_Resize(this, e);

            refresh_hj(); 
        }





        private void frm_FYSH_Resize(object sender, EventArgs e)
        {
            dGV_FYSH.Top = 3;
            dGV_FYSH.Left=3;
            dGV_FYSH.Width = this.Width-18;
            dGV_FYSH.Height = this.Height - groupBox1.Height-37;
            groupBox1.Top = dGV_FYSH.Height;
            groupBox1.Left = 3;
            groupBox1.Width = this.Width-12;
        }

       

        private void btn_UPload_Solo_Click(object sender, EventArgs e)
        {

            //-------------------------------
            lblts.Text = "正在上传费用，请稍候...";
            lblts.Visible = true;
            Application.DoEvents();

            model.ReimCancelItems(); //删除全部费用

            upJE = 0;
  

 

            SaveInItems(model.InPayPara.Details,model.InPayPara.PatInfo.MainDocCode==""?"001":model.InPayPara.PatInfo.MainDocCode,model.InPayPara.PatInfo.PatInHosCode);
            

            refresh_hj();

            //-------------------------------------
            lblts.Text = "上传费用完成！";
            Application.DoEvents();
            
           
             //重新加载数据，显示出医保返回值
            //dGV_FYSH.DataSource = dt;
            //dGV_FYSH.Refresh();
            
            
        }

        #region 保存住院费用明细

        public void SaveInItems(List<InNetworkUpDetail> items, string vysbm, string zyh)
        {

            //--------------------------------------
            lblts1.Text = "共 " + items.Count + " 条 已处理 0 条";
            lblts1.Visible = true;
            Application.DoEvents();
            //----------------------------------------
            int iRe = 0;

            string dateCur = Convert.ToDateTime(items[0].CreateTime).ToString("yyyy-MM-dd");

            for (int i = 0; i < items.Count; i++)
            {
                if (dateCur != Convert.ToDateTime(items[i].CreateTime).ToString("yyyy-MM-dd"))
                {
                    iRe = DarewayHandle.seiproxy.ExeFuncReInt("save_zy_script", new object[] { vysbm, dateCur });
                    if (iRe != 0)
                    {
                        throw new Exception("保存凭单出错,医保返回提示：" + DarewayHandle.seiproxy.ExeFuncReStr("get_errtext", null));
                    }
                    dateCur = Convert.ToDateTime(items[i].CreateTime).ToString("yyyy-MM-dd");

                    model.handelModel.InitZY(zyh);
                }

                DarewayHandle.seiproxy.ExeFuncReObj("new_zy_item", null);
                DarewayHandle.seiproxy.ExeFuncReObj("set_zy_item_string", new object[] { "yyxmbm", items[i].NetworkItemCode }); //医院项目编码
                DarewayHandle.seiproxy.ExeFuncReObj("set_zy_item_dec", new object[] { "dj", items[i].Price });                    //最小包装的单价
                DarewayHandle.seiproxy.ExeFuncReObj("set_zy_item_dec", new object[] { "sl", items[i].Quantity });                 //大包装数量
                DarewayHandle.seiproxy.ExeFuncReObj("set_zy_item_dec", new object[] { "bzsl", "1" });                   //  大包装的小包装数量
                DarewayHandle.seiproxy.ExeFuncReObj("set_zy_item_dec", new object[] { "zje", items[i].Amount });                  //总金额（zje=dj*sl*bzsl）
                DarewayHandle.seiproxy.ExeFuncReObj("set_zy_item_string", new object[] { "ksbm", items[i].DeptCode });           //科室编码
                DarewayHandle.seiproxy.ExeFuncReObj("set_zy_item_string", new object[] { "gg", items[i].Spec });                  //规格
                DarewayHandle.seiproxy.ExeFuncReObj("set_zy_item_string", new object[] { "zxksbm", "001" });            //*执行科室编码
                DarewayHandle.seiproxy.ExeFuncReObj("set_zy_item_string", new object[] { "kdksbm", "001" });            //*开单科室编码
                DarewayHandle.seiproxy.ExeFuncReObj("set_zy_item_dec", new object[] { "jyzfbl",0 });    //*自付比例items[i].SelfBurdenRatio
                iRe = DarewayHandle.seiproxy.ExeFuncReInt("save_zy_item", null);
                if (iRe != 0)
                {
                    throw new Exception("保存费用明细出错 项目编码为:" + items[i].ChargeCode.ToString() + "  项目名称为;" + items[i].ChargeName.ToString() + ",医保返回提示：" + DarewayHandle.seiproxy.ExeFuncReStr("get_errtext", null));
                }

                upJE = upJE + Convert.ToDouble(items[i].Amount.ToString());

                lblts1.Text = "共 " + items.Count + " 条 已上传 " + (i + 1) + " 条";
                Application.DoEvents();
                //----------------------------------------
            }

            iRe = DarewayHandle.seiproxy.ExeFuncReInt("save_zy_script", new object[] { vysbm, dateCur });
            if (iRe != 0)
            {
                throw new Exception("保存凭单出错,医保返回提示：" + DarewayHandle.seiproxy.ExeFuncReStr("get_errtext", null));
            }
        }
        #endregion
        

        private void btn_YJS_Click(object sender, EventArgs e) //继续预结算
        {
            DataSet dst = new DataSet();
            string strS = "SELECT TOTAL_COSTS FROM ZY.[IN].PAT_IN_HOS_EXTENDED WHERE PAT_IN_HOS_ID='" + model.InPayPara.PatInfo.PatInHosId + "'";
            dst = model.sqlHelperHis.ExecSqlReDs(strS);
            if (dst.Tables[0].Rows.Count > 0)
            {
                if (Math.Round(upJE, 2) != Math.Round(Convert.ToDouble(dst.Tables[0].Rows[0]["TOTAL_COSTS"].ToString()), 2))
                {
                    MessageBox.Show("上传费用[" + upJE.ToString() + "]与HIS发生的费用总额[" + Convert.ToDouble(dst.Tables[0].Rows[0]["TOTAL_COSTS"]).ToString() + "]不符！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                isOk = true;
                this.Close();
            }
           
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            isOk = false;
            this.Close();
        }

        private void dGV_FYSH_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {

                if ((e.ColumnIndex == this.dGV_FYSH.Columns["类别"].Index) && e.Value != null && e.Value.ToString() != "" && dGV_FYSH.Rows[e.RowIndex].Cells["类别"].Value.ToString() != "")
                {
                    if (dGV_FYSH.Rows[e.RowIndex].Cells["类别"].Value.ToString() == "0")
                    {
                        dGV_FYSH.Rows[e.RowIndex].Cells["类别"].Value = "药品";
                    }
                    else if (dGV_FYSH.Rows[e.RowIndex].Cells["类别"].Value.ToString() == "1")
                    {
                        dGV_FYSH.Rows[e.RowIndex].Cells["类别"].Value = "医疗";
                    }
                }
               
            }
            catch (Exception ex)
            {
            }

        }


        

        private void frm_FYSH_Shown(object sender, EventArgs e)
        {
          
        }





        private void refresh_hj()
        {
            DataSet dst = new DataSet();
            string strS = "SELECT TOTAL_COSTS FROM ZY.[IN].PAT_IN_HOS_EXTENDED WHERE PAT_IN_HOS_ID='" + model.InPayPara.PatInfo.PatInHosId + "'";
            dst = model.sqlHelperHis.ExecSqlReDs(strS);
            if (dst.Tables[0].Rows.Count > 0)
            {
                this.Text = "费用审核上传  总金额:" + Convert.ToDouble(dst.Tables[0].Rows[0]["TOTAL_COSTS"]).ToString() + "  上传金额:" + upJE.ToString();
            }
            
        }

        








  
        ////////////////////////////////////////////////////////////////////////////////////
        

    }
}
