using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using PayAPIInstance;
using PayAPIInterfaceHandle.Neusoft;
using PayAPIInterface.ParaModel;
using PayAPIUtilities.Log;

namespace PayAPIInstance.Dareway.Neusoft.Dialog
{
    public partial class WinReadCard : Form
    {
        /// <summary>
        ///     医保接口
        /// </summary>
        private readonly LiYiNeusoftHandle _liYiNeusoftHandle = new LiYiNeusoftHandle();

        /// <summary>
        ///     住院或门诊标志
        /// </summary>
        private readonly string _outOrIn;

        /// <summary>
        ///     住院
        /// </summary>
        public InPayParameter InPara;

        /// <summary>
        ///     门诊
        /// </summary>
        public OutPayParameter OutPara;//问题在这里

        /// <summary>
        ///     医保交易返回值
        /// </summary>
        public Dictionary<string, string> PersonInfoDic;

        /// <summary>
        ///  二级代码
        /// </summary>
        public DataTable SecLevelDt;

        /// <summary>
        ///     构造方法
        /// </summary>
        /// <param name="outOrIn">门诊或住院标示</param>
        public WinReadCard(string outOrIn)
        {
            _outOrIn = outOrIn;
            InitializeComponent();
        }

        /// <summary>
        ///     窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WinReadCard_Load(object sender, EventArgs e)
        {
            LoadBasicInfo();
            LoadSecLevelCode();
        }

        /// <summary>
        ///     加载基础信息
        /// </summary>
        private void LoadBasicInfo()
        {
            //加载医疗类别
            var arryMedKind = new ArrayList();
            if (_outOrIn.ToUpper().Equals("OUT"))
            {
                arryMedKind.Add(new DictionaryEntry("11", "普通门诊"));
                arryMedKind.Add(new DictionaryEntry("13", "门诊慢性病"));
                arryMedKind.Add(new DictionaryEntry("18", "特殊疾病门诊"));
                //if (OutPara.PatInfo.PatChargeClassId == 10004)
                //if (true)
                //{
                //    rdbNoCard.Checked = true;
                //    rdbHaveCard.Checked = false;
                //}
                //else 
                //{
                if (OutPara!=null)
                {
                    rdbNoCard.Checked = true;
                    rdbHaveCard.Checked = false;
                    rdbNoCard.Enabled = true;
                    rdbHaveCard.Enabled = true;
                    //if (OutPara.PatInfo.PatChargeClassId == 10004)
                    //{
                    //    rdbNoCard.Checked = true;
                    //    rdbHaveCard.Checked = false;

                    //    rdbNoCard.Enabled = false;
                    //    rdbHaveCard.Enabled = false;
                    //}
                    //else
                    //{
                    //    rdbNoCard.Checked = false;
                    //    rdbHaveCard.Checked = true;
                    //    rdbNoCard.Enabled = false;
                    //    rdbHaveCard.Enabled = false;
                    //    //if (rdbHaveCard.Checked)
                    //    //{
                    //    //    btnReadCard_Click(sender, e);
                    //    //}
                    //}
                }
                else
                {
                    LogManager.Debug("获取OutPara.PatInfo.PatChargeClassId == 10004 失败");

                    rdbNoCard.Checked = true;
                    rdbHaveCard.Checked = false;
                }

                //}
            }
            if (_outOrIn.ToUpper().Equals("IN"))
            {
                arryMedKind.Add(new DictionaryEntry("21", "普通住院"));
                arryMedKind.Add(new DictionaryEntry("29", "无责任人意外伤害"));
                arryMedKind.Add(new DictionaryEntry("52", "生育住院"));
                arryMedKind.Add(new DictionaryEntry("22", "按病种住院"));
                arryMedKind.Add(new DictionaryEntry("28", "单病种住院"));

                rdbNoCard.Enabled = true;
                rdbHaveCard.Enabled = true;

            }
            cbxMedKind.DataSource = arryMedKind;
            cbxMedKind.DisplayMember = "Value";
            cbxMedKind.ValueMember = "Key";
            cbxMedKind.SelectedIndex = 0;

        }

        /// <summary>
        ///  取二级代码字典
        /// </summary>
        private void LoadSecLevelCode()
        {
            SecLevelDt = _liYiNeusoftHandle.GetSecLevelCodeDs();
        }

        /// <summary>
        ///  取二级代码值
        /// </summary>
        /// <param name="secClass"></param>
        /// <param name="secCode"></param>
        /// <returns></returns>
        private string GetSecValue(string secClass, string secCode)
        {
            return
                SecLevelDt.Select("SEC_CLASS='" + secClass + "' AND SEC_CODE='" + secCode + "'")[0]["SEC_VALUE"]
                    .ToString();
        }

        /// <summary>
        ///  读卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadCard_Click(object sender, EventArgs e)
        {
            string impStr;
            string isHaveOrNoTrans;
            if (rdbHaveCard.Checked)
            {
                // 有卡读卡 2100 
                impStr = _liYiNeusoftHandle.GetImpString("2100", "1", "",true,"");
                isHaveOrNoTrans = "true";
            }
            else
            {
                //无卡读卡 1400
                var personNo = txtPersonNo.Text;
                var idNo = txtIdNo.Text;
                if (string.IsNullOrEmpty(personNo.Trim()) && string.IsNullOrEmpty(idNo.Trim()))
                {
                    MessageBox.Show("无卡交易时,请输入个人编号或身份证号中至少一个!");
                    return;
                }
                impStr = _liYiNeusoftHandle.GetImpString("1400", "1", personNo + "|" + idNo,true,"");
                //impStr = _liYiNeusoftHandle.GetImpString("1400", "1", personNo + idNo + "|");

                isHaveOrNoTrans = "false";
            }
            // impStr = "1400^1321100105^00001000^0105-00001000-20180629964^0105-20180629142316-001^0000^371321200807285342|371321200807285342^1^";

            //  impStr = "1400^1321100105^00001000^0105-00001000-20180629964^1321-20180629141021-114^0000^371321200807285342|371321200807285342^1^";
            //114
            // impStr = "1400^1321100105^00001000^0105-00001000-20180508423^1321-20180629103630-001^0000^|371321200807285342^1^";
            // impStr = "1400^1321100105^00001000^0105-00001000-20180629-964^1321-20180629115650-001^0000^371321200807285342|371321200807285342^1^";
            // "1400^1321100105^00001000^0105-00001000-20180629964^1321-20180629115650-001^0000^371321200807285342|371321200807285342^1^";
            //"1400^1321100105^00001000^0105-00001000-20180629964^1321-20180629112039-001^0000^371321200807285342^1^";

            PersonInfoDic = _liYiNeusoftHandle.ReadCard(impStr);
            PersonInfoDic.Add("有无卡", isHaveOrNoTrans);
            //显示人员信息
            txtRPersonNo.Text = PersonInfoDic["个人编号"];
            txtRIdNo.Text = PersonInfoDic["身份证号"];
            txtRName.Text = PersonInfoDic["姓名"];
            //txtRSex.Text = PersonInfoDic["性别"];
            txtRSex.Text = GetSecValue("性别", PersonInfoDic["性别"]);
            //txtRPersonClass.Text = PersonInfoDic["人员类别"];
            txtRPersonClass.Text = GetSecValue("医疗待遇类别", PersonInfoDic["人员类别"]);
            //txtRPersonStatus.Text = PersonInfoDic["人员状态"];
            //if (!string.IsNullOrEmpty(PersonInfoDic["人员状态"]))
            //{
            //    txtRPersonStatus.Text = GetSecValue("人员状态", PersonInfoDic["人员状态"]);
            //}
            if (!string.IsNullOrEmpty(PersonInfoDic["参保状态"]))
            {
                txtRPersonStatus.Text = GetSecValue("人员状态", PersonInfoDic["参保状态"]);
            }
            txtDeBalance.Text = PersonInfoDic["帐户余额"];
            txtTotalTc.Text = PersonInfoDic["本年统筹支出累计"];
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (rdbNoCard.Checked)
            {
                if (this.txtIdNo.Text.ToString().Trim().Length != 18)
                {
                    MessageBox.Show("身份证号长度无效!");
                    this.txtIdNo.Focus();
                    return;
                }
            }

            if (txtRName.Text.Trim() == "")
            {
                MessageBox.Show("请先读卡！");
                return;
            }
            DictionaryEntry de = (DictionaryEntry)cbxMedKind.SelectedItem;
            string strTypeCode = de.Key.ToString();
            if (_outOrIn.ToUpper().Equals("IN"))
            {
                if (txtDiagnosisName.Text.Trim() == "")
                {
                    MessageBox.Show("住院诊断名称不能为空！");
                    return;
                }
            }

            PersonInfoDic.Add("医疗类别", cbxMedKind.SelectedValue.ToString());
            PersonInfoDic.Add("诊断编码", txtDiagnosisCode.Text);
            PersonInfoDic.Add("诊断名称", txtDiagnosisName.Text);
            DialogResult = DialogResult.OK;
            Close();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        ///     录入诊断事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDiagnosisName_TextChanged(object sender, EventArgs e)
        {
            if (txtDiagnosisName.Text.Trim() == "")
            {
                dataGv.Visible = false;
                txtDiagnosisCode.Text = "";
                return;
            }
            DictionaryEntry de = (DictionaryEntry)cbxMedKind.SelectedItem;
            string strTypeCode = de.Key.ToString();

            string bztype = "";
            if (strTypeCode == "11")
            {
                bztype = "01";
            }
            else if (strTypeCode == "13")
            {
                bztype = "02";
            }
            else
            {
                bztype = "03";
            }
            //按病种

            var ds = _liYiNeusoftHandle.QueryDignosis(txtDiagnosisName.Text.Trim(), bztype);
            if (ds.Tables[0].Rows.Count > 0)
                dataGv.Visible = true;
            ds.Tables[0].TableName = "Disgnos";
            dataGv.DataSource = ds;
            dataGv.DataMember = "Disgnos";
            dataGv.Left = label10.Left;
            dataGv.Width = label10.Width + txtDiagnosisName.Width + txtDiagnosisName.Width;
        }

        /// <summary>
        ///     诊断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDiagnosisName_KeyDown(object sender, KeyEventArgs e)
        {
            var n = -1;
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
                    txtDiagnosisCode.Text = dataGv.SelectedRows[0].Cells[0].Value.ToString();
                    txtDiagnosisName.Text = dataGv.SelectedRows[0].Cells[1].Value.ToString();
                }
                dataGv.Visible = false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGv_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dataGv.SelectedRows.Count != 1)
                return;
            txtDiagnosisCode.Text = dataGv.SelectedRows[0].Cells[0].Value.ToString();
            txtDiagnosisName.Text = dataGv.SelectedRows[0].Cells[1].Value.ToString();
            dataGv.Visible = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (dataGv.SelectedRows.Count != 1)
                    return;
                txtDiagnosisCode.Text = dataGv.SelectedRows[0].Cells[0].Value.ToString();
                txtDiagnosisName.Text = dataGv.SelectedRows[0].Cells[1].Value.ToString();
                dataGv.Visible = false;
            }
        }

        /// <summary>
        ///  点击无卡操作时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbNoCard_CheckedChanged(object sender, EventArgs e)
        {
            if (_outOrIn == "IN" && !InPara.RegInfo.IsReg)
            {
                this.txtIdNo.Text = InPara.PatInfo.IDNo;
            }
        }

        /// <summary>
        ///  点击有卡操作时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbHaveCard_CheckedChanged(object sender, EventArgs e)
        {
            this.txtIdNo.Text = "";
         
        }
        private void txtIdNo_TextChanged(object sender, EventArgs e)
        {
            if (txtIdNo.Text.Trim().Length == 18)
            {
                btnReadCard_Click(sender, e);
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void rdbHaveCard_click(object sender, EventArgs e)
        {
            if (rdbHaveCard.Checked)
            {
                btnReadCard_Click(sender, e);
            }
        }
    }
}