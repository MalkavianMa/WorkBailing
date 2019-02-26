using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using PayAPIInstance.JingQi.Dialogs;

namespace PayAPIInstance.JingQi.Dialogs
{
    public partial class Form1 : Form
    {
        public MSSQLHelper SQLHelper = new MSSQLHelper("Data Source=222.222.22.112;Initial Catalog=comm;Persist Security Info=True;User ID=power;Password=m@ssunsoft009");

        public  string DJLX;  //登记类型
        public  string JBBM;
        public  string JBMC;
 
        //患者信息
        string[] a3;
        public Form1()
        {
            InitializeComponent();
        }
        public Form1(string [] a3)
        {
            

            
            this.a3 = a3;
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = a3[0];
            textBox5.Text = a3[1];
            textBox3.Text = a3[3];
            textBox2.Text = a3[8];
            textBox4.Text = a3[7];
            
            textBox9.Text = a3[6];
           // ExpenseType = a3[15];
            switch (a3[15])
            {
                case "": textBox6.Text = "非贫困人口";
                    break;
                case "0": textBox6.Text = "非贫困人口";
                    break;
                case "1": textBox6.Text = "建档立卡人员";
                    break;
                case "2": textBox6.Text = "脱贫不脱政策";

                    break;

            }
            // <summary> 
            /// 填充医疗类别
            /// </summary>

            ArrayList arr2 = new ArrayList();

            arr2.Add(new DictionaryEntry("2", "入院"));
            arr2.Add(new DictionaryEntry("9", "入院冲销"));
            arr2.Add(new DictionaryEntry("3", "出院"));
            arr2.Add(new DictionaryEntry("0", "入院修改"));
            arr2.Add(new DictionaryEntry("11", "普通慢性病门诊"));
            arr2.Add(new DictionaryEntry("12", "专科慢性病登记"));
            arr2.Add(new DictionaryEntry("13", "特殊慢性门诊登记"));
            arr2.Add(new DictionaryEntry("100", "单病种"));



            comboBox1.DataSource = arr2;
            comboBox1.DisplayMember = "Value";
            comboBox1.ValueMember = "Key";
            comboBox1.SelectedIndex = 0; //默认空

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //全局变量获取疾病编码
            JBBM =  textBox8.Text;
            JBMC = textBox7.Text;

            DJLX = comboBox1.SelectedValue.ToString();
            this.Close();


        }

        private void button1_Click(object sender, EventArgs e)
        {
           
               
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

             

        
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.Text.ToString().Trim() == "")
            {
                dataGridView1.Visible = false;
                textBox7.Text = "";
                return;
            }
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

                textBox7.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                textBox8.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
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
            textBox7.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox8.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            dataGridView1.Visible = false;
                
               
                
            
        }
        /// <summary>
        /// 文本框回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {

           
                if (e.KeyCode == Keys.Enter)
                {
                  
                    StringBuilder strSql = new StringBuilder();
                    //strSql.Append(" SELECT 病种编码,病种名称,CASE WHEN 病种类别='01' THEN '普通病种' WHEN 病种类别='02' THEN '门诊慢性病' WHEN 病种类别='03' THEN '特殊病种' WHEN 病种类别='01' THEN '' WHEN 病种类别='04' THEN '生育病种' WHEN 病种类别='09' THEN '其他病种' END 病种类别 FROM YBDR.dbo.YB_BZML  ");
                    //strSql.Append(" WHERE 病种类别 NOT IN('01','07') AND (病种编码 LIKE '" + textBox_Zdmc.Text.ToString().Trim() + "%' OR 病种名称 LIKE  '" + textBox_Zdmc.Text.ToString().Trim() + "%' OR 拼音助记码 LIKE  '" + textBox_Zdmc.Text.ToString().Trim() + "%') ");
                    strSql.Append("select CENTER_DIAGNOSIS_CODE as 病种编码, CENTER_DIAGNOSIS_NAME as 病种名称  FROM COMM.DICT.NETWORKING_DIAGNOSIS_DICT WHERE ( INPUT_CODE LIKE '" + textBox7.Text.ToString().Trim() + "%' OR CENTER_DIAGNOSIS_NAME   LIKE '" + textBox7.Text.ToString().Trim() + "%') ");
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
            
            
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
            throw new Exception("取消结算");

        }
    }
}
