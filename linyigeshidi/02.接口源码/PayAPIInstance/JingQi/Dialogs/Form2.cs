using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PayAPIInterface.ParaModel;
using System.Collections;

namespace PayAPIInstance.JingQi.Dialogs
{
    public partial class Form2 : Form
    {
        public string Cyzdmc;//出院诊断名称
        public string Cyzdbm;//出院诊断编码
        public MSSQLHelper SQLHelper = new MSSQLHelper("Data Source=222.222.22.112;Initial Catalog=comm;Persist Security Info=True;User ID=power;Password=m@ssunsoft009");
        public string Jslx;//结算类型
        InPayParameter InPayPara = new InPayParameter();//调用his信息
        public Form2(InPayParameter InPayPara)
        {
            this.InPayPara = InPayPara;
            InitializeComponent();
        }
        /// <summary>
        /// 诊断检索后回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                // dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

                textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                dataGridView1.Visible = false;



            }
        }
        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            dataGridView1.Visible = false;




        }
        /// <summary>
        /// 文本框回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {


            if (e.KeyCode == Keys.Enter)
            {

                StringBuilder strSql = new StringBuilder();
                //strSql.Append(" SELECT 病种编码,病种名称,CASE WHEN 病种类别='01' THEN '普通病种' WHEN 病种类别='02' THEN '门诊慢性病' WHEN 病种类别='03' THEN '特殊病种' WHEN 病种类别='01' THEN '' WHEN 病种类别='04' THEN '生育病种' WHEN 病种类别='09' THEN '其他病种' END 病种类别 FROM YBDR.dbo.YB_BZML  ");
                //strSql.Append(" WHERE 病种类别 NOT IN('01','07') AND (病种编码 LIKE '" + textBox_Zdmc.Text.ToString().Trim() + "%' OR 病种名称 LIKE  '" + textBox_Zdmc.Text.ToString().Trim() + "%' OR 拼音助记码 LIKE  '" + textBox_Zdmc.Text.ToString().Trim() + "%') ");
                strSql.Append("select CENTER_DIAGNOSIS_CODE as 病种编码, CENTER_DIAGNOSIS_NAME as 病种名称  FROM COMM.DICT.NETWORKING_DIAGNOSIS_DICT WHERE ( INPUT_CODE LIKE '" + textBox2.Text.ToString().Trim() + "%' OR CENTER_DIAGNOSIS_NAME   LIKE '" + textBox2.Text.ToString().Trim() + "%') ");
                DataTable dt = SQLHelper.ExecSqlReDs(strSql.ToString()).Tables[0];

                dataGridView1.DataSource = null;

                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                    dataGridView1.Visible = true;
                    if (e.KeyCode == Keys.Enter)
                    {
                        dataGridView1.Focus();
                    }
                }
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox6.Text = InPayPara.RegInfo.NetPatName;
            textBox3.Text = InPayPara.RegInfo.IdNo;
            textBox5.Text = InPayPara.RegInfo.CardNo;

            // <summary> 
            /// 填充医疗类别
            /// </summary>

            ArrayList arr2 = new ArrayList();

            arr2.Add(new DictionaryEntry("1", "正常出院结算"));
            arr2.Add(new DictionaryEntry("3", "住院平产"));
            arr2.Add(new DictionaryEntry("4", "住院剖腹产"));
            arr2.Add(new DictionaryEntry("9", "慢病结算"));




            comboBox1.DataSource = arr2;
            comboBox1.DisplayMember = "Value";
            comboBox1.ValueMember = "Key";
            comboBox1.SelectedIndex = 0; //默认空

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.ToString().Trim() == "")
            {
                dataGridView1.Visible = false;
                textBox2.Text = "";
                return;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Jslx = comboBox1.SelectedValue.ToString();
            Cyzdmc = textBox2.Text.ToString();
            Cyzdbm = textBox1.Text.ToString();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            throw new  Exception("取消结算");
        }
    }
}
