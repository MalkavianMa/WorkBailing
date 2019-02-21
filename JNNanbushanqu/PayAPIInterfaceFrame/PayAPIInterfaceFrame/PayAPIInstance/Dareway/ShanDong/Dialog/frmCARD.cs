using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PayAPIInstance.Dareway.ShanDong.Dialog
{
    public partial class frmCARD : Form
    {
        public bool isOk = false;//是否存在结果
        private DarewayModel model;
        public string p_yltclb = "";//医疗统筹类别
        public frmCARD(DarewayModel _model)
        {
            InitializeComponent();
            model=_model;
        }

        private void frmCARD_Load(object sender, EventArgs e)
        {
            FillCbType();
        }

        /// <summary>
        /// 填充医疗类别类别
        /// </summary>
        private void FillCbType()
        {
            DataSet ds = new DataSet();
            string strSql = "";
            if (model.isOut)   //数据库获取医疗类型下拉数据 true 代表门诊 1代表住院
            {
                strSql = "SELECT NET_TYPE_CODE TYPE_CODE,NET_TYPE_NAME TYPE_NAME FROM COMM.DICT.NETWORKING_NET_TYPE WHERE NETWORKING_PAT_CLASS_ID=27 AND TYPE_FLAG=0";
                ds = model.sqlHelperHis.ExecSqlReDs(strSql);
            }
            else
            {
                strSql = "SELECT NET_TYPE_CODE TYPE_CODE,NET_TYPE_NAME TYPE_NAME FROM COMM.DICT.NETWORKING_NET_TYPE WHERE NETWORKING_PAT_CLASS_ID=27 AND TYPE_FLAG=1";
                ds = model.sqlHelperHis.ExecSqlReDs(strSql);

            }
            cbType.ValueMember = "TYPE_CODE";
            cbType.DisplayMember = "TYPE_NAME";
            cbType.DataSource = ds.Tables[0]; //绑定数据库
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            p_yltclb = cbType.SelectedValue.ToString();
            isOk = true;
            this.Close();
        }

       
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
