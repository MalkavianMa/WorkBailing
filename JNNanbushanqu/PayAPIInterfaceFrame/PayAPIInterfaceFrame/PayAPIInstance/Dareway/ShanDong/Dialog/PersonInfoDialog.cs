using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace PayAPIInstance.Dareway.ShanDong.Dialog
{
    public partial class PersonInfoDialog : Form
    {
        //疾病编码
        public string strDiagnosCode = "";
        //疾病名称
        public string strDiagnosName = "";

        private Dictionary<string, string> dicPerInfo;

        private bool isHaveDiagnos = false;
        //是否取消
        public bool isCancel = true;

        public PersonInfoDialog()
        {
            InitializeComponent();
        }

        public PersonInfoDialog(Dictionary<string,string> patInfo)
        {
            dicPerInfo = patInfo;
            InitializeComponent();
            ShowInfo();
        }

        private void ShowInfo() 
        {
            txtName.Text = dicPerInfo["xm"];
            txtSex.Text = dicPerInfo["xb"] == "1" ? "男" : "女";  //性别
            txtMemNo.Text = dicPerInfo["ylzbh"];
            txtIDNO.Text = dicPerInfo["shbzhm"];
            txtInvalid.Text = dicPerInfo["zfbz"];
            txtInvalidReason.Text = dicPerInfo["zfsm"];
            txtPersonType.Text = dicPerInfo["ylrylb"];
            txtCompany.Text = dicPerInfo["dwmc"];
            txtBalance.Text = dicPerInfo["ye"];
            txtIns.Text = dicPerInfo["sbjglx"] == "A" ? "职工" : (dicPerInfo["sbjglx"] == "B" ? "居民" : "");
            txtMzdbbz.Text = dicPerInfo["mzdbbz"];
            string strDiagnos = dicPerInfo["mzdbjbs"];
            string[] arrDiagnos = strDiagnos.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            ArrayList arrList = new ArrayList();
            
			
            for (int i = 0; i < arrDiagnos.Length; i++)
            {
                if(arrDiagnos[i].Trim().Length!=0){
                    string[] arrDia = arrDiagnos[i].Split(new char[] { '#', 'm' }, StringSplitOptions.RemoveEmptyEntries);
                    arrList.Add(new DictionaryEntry(arrDia[1], arrDia[0]));
                }
            }
            if (arrList.Count > 0)
            {
                isHaveDiagnos = true;
                cbxDiagnos.DataSource = arrList;
                cbxDiagnos.DisplayMember = "Value";
                cbxDiagnos.ValueMember = "Key";
                cbxDiagnos.SelectedIndex = 0;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            strDiagnosCode = isHaveDiagnos ? cbxDiagnos.SelectedValue.ToString() : "";
            strDiagnosName = cbxDiagnos.Text;
            isCancel = false;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isCancel = true;
            this.Close();
        }
    }
}
