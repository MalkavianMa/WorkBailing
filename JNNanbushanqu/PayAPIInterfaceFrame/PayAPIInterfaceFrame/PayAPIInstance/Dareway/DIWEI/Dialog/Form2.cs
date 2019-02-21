using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PayAPIInterface.Model.In;

namespace PayAPIInstance.Dareway.DIWEI.Dialog
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public DataTable dtsss = new DataTable();
        public List<InNetworkUpDetail> Details;
        public bool ispreCancle = false;
        private void Form2_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("自付比例");
            dt.Columns.Add("项目名称");
            dt.Columns.Add("Amount");
            dt.Columns.Add("AmountSelf");
            dt.Columns.Add("AmountSelfBurdern");
            dt.Columns.Add("AutoId");
            dt.Columns.Add("ChargeCode");
            dt.Columns.Add("ChargeId");
            //huhuan
            dt.Columns.Add("CreateTime");
            dt.Columns.Add("ChargeType");//huhuan
            dt.Columns.Add("DeptCode");
            dt.Columns.Add("DocCode");
            dt.Columns.Add("DrugFormName");
            dt.Columns.Add("HisItemName");
            dt.Columns.Add("MedAmount");
            dt.Columns.Add("Memo1");
            dt.Columns.Add("Memo2");
            dt.Columns.Add("Memo3");
            dt.Columns.Add("Memo4");
            dt.Columns.Add("Memo5");
            dt.Columns.Add("Memo6");
            dt.Columns.Add("Memo7");
            dt.Columns.Add("Memo8");
            dt.Columns.Add("NetworkItemClass");
            dt.Columns.Add("NetworkItemCode");
            dt.Columns.Add("NetworkItemProp");
            dt.Columns.Add("NetworkSettleId");
            dt.Columns.Add("OrderId");
            dt.Columns.Add("Price");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Spec");
            dt.Columns.Add("Unit");
            dt.Columns.Add("UpDetailId");
            dt.Columns.Add("UploadBackSerial");

            DataRow dr = dt.NewRow();

            for (int i = 0; i < Details.Count; i++)
            {
                dr["自付比例"] = Details[i].SelfBurdenRatio.ToString();
                dr["项目名称"] = Details[i].ChargeName.ToString();
                dr["Amount"] = Details[i].Amount.ToString();
                dr["AmountSelf"] = Details[i].AmountSelf.ToString();
                dr["AmountSelfBurdern"] = Details[i].AmountSelfBurdern.ToString();
                dr["AutoId"] = Details[i].AutoId.ToString();
                dr["ChargeCode"] = Details[i].ChargeCode.ToString();
                dr["ChargeId"] = Details[i].ChargeId.ToString();
                dr["CreateTime"] = Details[i].CreateTime.ToString();//huhuan
                dr["ChargeType"] = Details[i].ChargeType.ToString();
                //huhuan
                dr["DeptCode"] = Details[i].DeptCode.ToString();
                dr["DocCode"] = Details[i].DocCode.ToString();
                dr["DrugFormName"] = Details[i].DrugFormName.ToString();
                dr["HisItemName"] = Details[i].HisItemName.ToString();
                dr["MedAmount"] = Details[i].MedAmount.ToString();
                dr["Memo1"] = Details[i].Memo1.ToString();
                dr["Memo2"] = Details[i].Memo2.ToString();
                dr["Memo3"] = Details[i].Memo3.ToString();
                dr["Memo4"] = Details[i].Memo4.ToString();
                dr["Memo5"] = Details[i].Memo5.ToString();
                dr["Memo6"] = Details[i].Memo6.ToString();
                dr["Memo7"] = Details[i].Memo7.ToString();
                dr["Memo8"] = Details[i].Memo8.ToString();
                dr["NetworkItemClass"] = Details[i].NetworkItemClass.ToString();
                dr["NetworkItemCode"] = Details[i].NetworkItemCode.ToString();
                dr["NetworkItemProp"] = Details[i].NetworkItemProp.ToString();
                dr["NetworkSettleId"] = Details[i].NetworkSettleId.ToString();
                dr["OrderId"] = Details[i].OrderId.ToString();
                dr["Price"] = Details[i].Price.ToString();
                dr["Quantity"] = Details[i].Quantity.ToString();
                dr["Spec"] = Details[i].Spec.ToString();
                dr["Unit"] = Details[i].Unit.ToString();
                dr["UpDetailId"] = Details[i].UpDetailId.ToString();
                dr["UploadBackSerial"] = Details[i].UploadBackSerial.ToString();
                dt.Rows.Add(dr);
                dr = dt.NewRow();
            }

            DataTable dtable = dt.Copy();
            DataView dview = dtable.DefaultView;
            //dview.Sort = "CreateTime desc";
            dview.Sort = "ChargeCode asc,CreateTime desc";

            dtable = dview.ToTable();

            dataGridView1.DataSource = dtable;

            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }



        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dts = new DataTable();
            dts = (DataTable)dataGridView1.DataSource;
            for (int i = 0; i < Details.Count; i++)
            {
                //Details[i].Amount = Convert.ToDecimal("500");
                //string hg=dts.Rows[i][0].ToString();
                //if (i>=dts.Rows.Count)
                //{
                //   // Details.Remove(Details[i]);
                //    Details.RemoveAt(i);
                //}
                //else
                //{
                Details[i].SelfBurdenRatio = Convert.ToDecimal(dts.Rows[i][0].ToString());
                Details[i].ChargeName = dts.Rows[i][1].ToString();
                Details[i].Amount = Convert.ToDecimal(dts.Rows[i][2].ToString());
                Details[i].AmountSelf = Convert.ToDecimal(dts.Rows[i][3].ToString());
                Details[i].AmountSelfBurdern = Convert.ToDecimal(dts.Rows[i][4].ToString());
                Details[i].AutoId = Convert.ToDecimal(dts.Rows[i][5].ToString());
                Details[i].ChargeCode = dts.Rows[i][6].ToString();
                Details[i].ChargeId = Convert.ToDecimal(dts.Rows[i][7].ToString());
                Details[i].ChargeType = Convert.ToDecimal(dts.Rows[i][9].ToString());
                Details[i].CreateTime = Convert.ToDateTime(dts.Rows[i][8].ToString());
                Details[i].DeptCode = dts.Rows[i][10].ToString();
                Details[i].DocCode = dts.Rows[i][11].ToString();
                Details[i].DrugFormName = dts.Rows[i][12].ToString();
                Details[i].HisItemName = dts.Rows[i][13].ToString();
                Details[i].MedAmount = Convert.ToDecimal(dts.Rows[i][14].ToString());
                Details[i].Memo1 = dts.Rows[i][15].ToString();
                Details[i].Memo2 = dts.Rows[i][16].ToString();
                Details[i].Memo3 = dts.Rows[i][17].ToString();
                Details[i].Memo4 = dts.Rows[i][18].ToString();
                Details[i].Memo5 = dts.Rows[i][19].ToString();
                Details[i].Memo6 = dts.Rows[i][20].ToString();
                Details[i].Memo7 = dts.Rows[i][21].ToString();
                Details[i].Memo8 = dts.Rows[i][22].ToString();
                Details[i].NetworkItemClass = dts.Rows[i][23].ToString();
                Details[i].NetworkItemCode = dts.Rows[i][24].ToString();
                Details[i].NetworkItemProp = dts.Rows[i][25].ToString();
                Details[i].NetworkSettleId = Convert.ToDecimal(dts.Rows[i][26].ToString());
                Details[i].OrderId = Convert.ToDecimal(dts.Rows[i][27].ToString());
                Details[i].Price = Convert.ToDecimal(dts.Rows[i][28].ToString());
                Details[i].Quantity = Convert.ToDecimal(dts.Rows[i][29].ToString());
                Details[i].Spec = dts.Rows[i][30].ToString();
                Details[i].Unit = dts.Rows[i][31].ToString();
                Details[i].UpDetailId = Convert.ToDecimal(dts.Rows[i][32].ToString());
                Details[i].UploadBackSerial = dts.Rows[i][33].ToString();
                //}

            }
            //dtsss = dts;
            this.Close();
        }

        //string quicklyCellValuechange = "";
        //string quicklyCellValuechangeName = "";
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex == 0)
            //{
            //    string chargeID = "";
            //    string chargeName = "";
            //    string chargeZfbi = "";
            //    DataGridViewRow dgr = this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index];
            //    // this.dataGridView1.Rows[0].Cells[0].Value = "900";
            //    //取ID
            //    chargeID = dgr.Cells["ChargeId"].Value.ToString();

            //    chargeName = dgr.Cells["项目名称"].Value.ToString();

            //    //if (chargeName.Equals(quicklyCellValuechangeName) && chargeID.Equals(quicklyCellValuechange))
            //    //{
            //    //    return;
            //    //}




            //    chargeZfbi = dgr.Cells[0].Value.ToString();
            //    //取名称
            //    DataTable dts = new DataTable();
            //    dts = (DataTable)dataGridView1.DataSource;
            //    int itemNum = 0;

            //    foreach (DataRow item in dts.Rows)
            //    {
            //        //所有单元格赋值
            //        if (item["ChargeId"].ToString().Equals(chargeID) && item["项目名称"].ToString().Equals(chargeName))
            //        {

            //            this.dataGridView1.Rows[itemNum].Cells[0].Value = chargeZfbi;



            //        }

            //        itemNum++;

            //    }

            //    //quicklyCellValuechange = chargeID;
            //    //quicklyCellValuechangeName = chargeName;

            //}
        }

        int deleteRowindex = 0;
        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (e.Button==MouseButtons.Right)
            //{
            //    deleteRowindex = e.RowIndex;
            //    this.dataGridView1.Rows[e.RowIndex].Selected = true;
            //    this.dataGridView1.CurrentCell = this.dataGridView1.Rows[e.RowIndex].Cells[1];
            //    contextMenuStrip1.Show(Cursor.Position); 
            //}
        }

        private void 删除行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.dataGridView1.Rows[this.deleteRowindex].IsNewRow)
            {
                this.dataGridView1.Rows.RemoveAt(deleteRowindex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ispreCancle = true;
            this.Close();
        }



    }
}
