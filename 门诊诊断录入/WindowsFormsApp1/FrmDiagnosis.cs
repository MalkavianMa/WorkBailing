using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class FrmDiagnosis : Form
    {

        public string ReDiagnosName = "";//诊断名称
        public string ReDiagnosCode = "";//诊断编号
        private bool bLoadingDia = false;//载入默认诊断时
                                         // public MSSQLHelper sqlHelper = new MSSQLHelper("Data Source=172.22.25.6;Initial Catalog=COMM;User ID=power;Password=m@ssuns0ft009");
        public MSSQLHelper sqlHelper = new MSSQLHelper("Data Source=.;Initial Catalog=COMM;User ID=sa;Password=1989789mjw");

        public FrmDiagnosis()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Test");
        }

        private void FrmDiagnosis_KeyDown(object sender, KeyEventArgs e)
        {
            int n = -1;
            if (dataGv.Rows.Count > 0)
                n = dataGv.SelectedRows[0].Index;
            if (e.KeyCode == Keys.Down && n < dataGv.Rows.Count - 1)
            {
                dataGv.Rows[n + 1].Selected = true;
                dataGv.FirstDisplayedScrollingRowIndex = n + 1;
            }
            if (e.KeyCode == Keys.Up && n > 0)
            {
                dataGv.Rows[n - 1].Selected = true;
                dataGv.FirstDisplayedScrollingRowIndex = n - 1;
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (dataGv.SelectedRows.Count > 0)
                {
                    tbDiagnosCode.Text = dataGv.SelectedRows[0].Cells[0].Value.ToString();
                    tbDignosis.Text = dataGv.SelectedRows[0].Cells[1].Value.ToString();
                }
                dataGv.Visible = false;
                button1.Focus();
            }
        }

        private void dataGv_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dataGv.SelectedRows.Count != 1)
                return;
            tbDiagnosCode.Text = dataGv.SelectedRows[0].Cells[0].Value.ToString();
            tbDignosis.Text = dataGv.SelectedRows[0].Cells[1].Value.ToString();
            ReDiagnosCode = tbDiagnosCode.Text.Trim();
            ReDiagnosName = tbDignosis.Text.Trim();

            dataGv.Visible = false;
            button1.Focus();
        }

        private void dataGv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (dataGv.SelectedRows.Count != 1)
                    return;
                tbDiagnosCode.Text = dataGv.SelectedRows[0].Cells[0].Value.ToString();
                tbDignosis.Text = dataGv.SelectedRows[0].Cells[1].Value.ToString();
                ReDiagnosCode = tbDiagnosCode.Text.Trim();
                ReDiagnosName = tbDignosis.Text.Trim();

                dataGv.Visible = false;
                button1.Focus();
            }
        }

        private void FrmDiagnosis_Load(object sender, EventArgs e)
        {
            if (ReDiagnosCode != "")
            {


                bLoadingDia = false;
            }

            if (tbDignosis.Text.Trim() != "")
                button1.Focus();
            else
                button1.Focus();

           
        }

        private void tbDignosis_TextChanged(object sender, EventArgs e)
        {
            if (bLoadingDia)
                return;
            if (tbDignosis.Text.Trim() == "")
            {
                dataGv.Visible = false;
                tbDiagnosCode.Text = "";
                return;
            }
            #region 本地查询诊断

            //NetworkQueryDALSQLite query = new NetworkQueryDALSQLite();
            //DataSet ds = query.QueryDignosis(tbDignosis.Text.Trim(), 1);

            // string strsql1 = "SELECT top 100 CENTER_DIAGNOSIS_CODE,CENTER_DIAGNOSIS_NAME FROM COMM.DICT.NETWORKING_DIAGNOSIS_DICT WHERE FLAG_INVALID=0  AND INPUT_CODE LIKE '%" + tbDignosis.Text.Trim() + "%' AND NETWORKING_PAT_CLASS_ID='370100'  ORDER BY CHARINDEX('" + tbDignosis.Text.Trim() + "',INPUT_CODE),LEN(INPUT_CODE) ASC";
            //SELECT top 100 CENTER_DIAGNOSIS_CODE,CENTER_DIAGNOSIS_NAME FROM COMM.DICT.NETWORKING_DIAGNOSIS_DICT WHERE FLAG_INVALID=0  AND (INPUT_CODE LIKE '%感冒%'  OR  CENTER_DIAGNOSIS_NAME  LIKE '%感冒%' ) AND NETWORKING_PAT_CLASS_ID='370100'        ORDER BY CHARINDEX('感冒',INPUT_CODE),LEN(INPUT_CODE) ASC    
            //  string strsql1 = "SELECT top 100 CENTER_DIAGNOSIS_CODE,CENTER_DIAGNOSIS_NAME FROM COMM.DICT.NETWORKING_DIAGNOSIS_DICT WHERE FLAG_INVALID=0  AND (INPUT_CODE LIKE '%感冒%'  OR  CENTER_DIAGNOSIS_NAME  LIKE '%" + tbDignosis.Text.Trim() + "%' ) AND NETWORKING_PAT_CLASS_ID='370100'        ORDER BY CHARINDEX('" + tbDignosis.Text.Trim() + "',INPUT_CODE),LEN(INPUT_CODE) ASC ";
            StringBuilder strsql1 = new StringBuilder();

            strsql1.AppendLine("    SELECT*");
            strsql1.AppendLine("  FROM(SELECT TOP 500");
            strsql1.AppendLine("          CENTER_DIAGNOSIS_CODE,");
            strsql1.AppendLine("             CENTER_DIAGNOSIS_NAME");
            strsql1.AppendLine("     FROM      COMM.DICT.NETWORKING_DIAGNOSIS_DICT");
            strsql1.AppendLine("     WHERE     FLAG_INVALID = 0");
            strsql1.AppendLine("            AND(INPUT_CODE LIKE  '%" + tbDignosis.Text.Trim() + "%'");
            strsql1.AppendLine("                OR CENTER_DIAGNOSIS_NAME LIKE  '%" + tbDignosis.Text.Trim() + "%'");
            strsql1.AppendLine("              )");
            strsql1.AppendLine("       AND NETWORKING_PAT_CLASS_ID = '370100'");
            strsql1.AppendLine("     ORDER BY  CHARINDEX( '" + tbDignosis.Text.Trim() + "', INPUT_CODE),");
            strsql1.AppendLine("                LEN(INPUT_CODE) ASC");
            strsql1.AppendLine("     ) AS H");
            strsql1.AppendLine("  UNION");
            strsql1.AppendLine("  SELECT *");
            strsql1.AppendLine("  FROM(SELECT TOP 500");
            strsql1.AppendLine("                 CENTER_DIAGNOSIS_CODE,");
            strsql1.AppendLine("                  CENTER_DIAGNOSIS_NAME");
            strsql1.AppendLine("        FROM      COMM.DICT.NETWORKING_DIAGNOSIS_DICT");
            strsql1.AppendLine("      WHERE     FLAG_INVALID = 0");
            strsql1.AppendLine("               AND INPUT_CODE LIKE  '%" + tbDignosis.Text.Trim() + "%'");
            strsql1.AppendLine("               AND NETWORKING_PAT_CLASS_ID = '370100'");
            strsql1.AppendLine("       ORDER BY  CHARINDEX( '" + tbDignosis.Text.Trim() + "', INPUT_CODE),");
            strsql1.AppendLine("                 LEN(INPUT_CODE) ASC");
            strsql1.AppendLine("      ) AS N");




            DataSet ds = sqlHelper.ExecSqlReDs(strsql1.ToString());
            if (ds.Tables[0].Rows.Count > 0)
                dataGv.Visible = true;
            ds.Tables[0].TableName = "Disgnos";
            dataGv.DataSource = ds;
            dataGv.DataMember = "Disgnos";
            #endregion
           // dataGv.Focus();

            //dataGv.Left = label2.Left + groupBox2.Left;
            //dataGv.Top = tbDignosis.Top + groupBox2.Top + tbDignosis.Height;
        }

        private void FrmDiagnosis_Activated(object sender, EventArgs e)
        {
            tbDignosis.Focus();
        }
    }
}
