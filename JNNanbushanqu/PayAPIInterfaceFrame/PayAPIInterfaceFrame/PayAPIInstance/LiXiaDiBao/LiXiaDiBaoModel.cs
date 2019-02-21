using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using PayAPIInstance.LiXiaDiBao.Dialog;
using PayAPIInterface;
using PayAPIInterface.Model.Comm;
using PayAPIUtilities.Config;
using PayAPIInterface.ParaModel;
using PayAPIInterface.Model.Out;
using PayAPIInterface.Model.In;
using PayAPIUtilities.Log;
using System.Data;
using PayAPIInterfaceHandle.Dareway.JiNan;

namespace PayAPIInstance.LiXiaDiBao
{
    public class LiXiaDiBaoModel : IPayCompanyInterface
    {

        public MSSQLHelper sqlHelper = new MSSQLHelper("Data Source=172.22.29.6;Initial Catalog=COMM;Persist Security Info=True;User ID=power;Password=m@ssunsoft009");
        public NetworkPatInfo networkPatInfo = new NetworkPatInfo();
        /// <summary>
        /// 门诊入参
        /// </summary>
        public OutPayParameter outReimPara;

        //-----------------------------------为了DarewayHandle中定义的门诊费用审核
        public static DarewayHandle handelModel;
        public bool isInit = false;  //是否初始化
        /// <summary>
        /// 初始化
        /// </summary>
        public void InterfaceInit()
        {
            if (!isInit)
            {
                handelModel = new DarewayHandle();
                isInit = true;
            }
        }
        //-----------------------------------
        public LiXiaDiBaoModel()
        {
            //指定IE目录
            //System.Environment.CurrentDirectory = System.Environment.CommandLine.Substring(0, System.Environment.CommandLine.LastIndexOf("\\")).Replace("\"", "");
        }


        public NetworkPatInfo NetworkReadCard()
        {
            frmCARD frmcard = new frmCARD(this);
            frmcard.TopMost = true;
            frmcard.ShowDialog();
            if (frmcard.isCancel)
            {
                throw new Exception("取消操作");
            }
            return networkPatInfo;
            
        }

        public void OutNetworkRegister(OutPayParameter para)
        {
            outReimPara = para;

            NetworkReadCard();

            //当姓名不一致时提示
            if (outReimPara.PatInfo.PatName != networkPatInfo.PatName)
            {
                if (MessageBox.Show(" 低保卡姓名为：【" + networkPatInfo.PatName.ToString() + "】     HIS患者姓名为：【" + outReimPara.PatInfo.PatName + "】 是否继续 ", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                {
                    throw new Exception("姓名不一致，操作员取消操作！");
                }
            }
            outReimPara.RegInfo.CardNo = networkPatInfo.ICNo;                   
            outReimPara.RegInfo.NetPatType = networkPatInfo.MedicalType;             
            outReimPara.RegInfo.CantonCode ="";            
            outReimPara.RegInfo.MemberNo = networkPatInfo.MedicalNo;
            outReimPara.RegInfo.CompanyName = networkPatInfo.CompanyName;               
            outReimPara.RegInfo.PatAddress = networkPatInfo.CompanyNo;
            outReimPara.RegInfo.IdNo = networkPatInfo.IDNo;                   
            //outReimPara.RegInfo.Balance =0 ;
            outReimPara.RegInfo.NetPatName =networkPatInfo.PatName;                  
            outReimPara.RegInfo.NetDiagnosCode = "";                        
            outReimPara.RegInfo.NetType = "";
        }

        public void OutNetworkPreSettle(OutPayParameter para)
        {
            outReimPara = para;

            string strSql = "";
            DataSet ds = new DataSet();
            strSql = "SELECT id FROM REPORT.dbo.yb_MzXlb WHERE NETWORKING_PAT_CLASS_ID='" + outReimPara.CommPara.NetworkPatClassId + "'"+
                     " UNION ALL "+
                     "SELECT id FROM REPORT.dbo.yb_MzXeb WHERE NETWORKING_PAT_CLASS_ID='" + outReimPara.CommPara.NetworkPatClassId + "'";
            ds = sqlHelper.ExecSqlReDs(strSql);
            if (ds.Tables[0].Rows.Count >0)
            {
                InterfaceInit();
                //outReimPara.CommPara.NetworkPatClassId;
                //判断费用限额
                string reStr = handelModel.MZfysh(outReimPara.RegInfo.IdNo, outReimPara.CommPara.NetworkPatClassId, outReimPara.CommPara.TradeId.ToString());
                if (reStr != "1")
                {
                    throw new Exception(reStr);
                }

            }
        }

        public void OutNetworkSettle(OutPayParameter para)
        {
            outReimPara = para;
            frmJS frmjs = new frmJS(this);
            frmjs.TopMost = true;
            frmjs.ShowDialog();
        }

        public void CancelOutNetworkSettle(OutPayParameter para)
        {
            outReimPara = para;
            frmQXJS frmqxjs = new frmQXJS(this);
            frmqxjs.TopMost = true;
            frmqxjs.ShowDialog();
            
        }


        #region 住院(不需要)
        public void CancelInNetworkRegister(InPayParameter para)
        {
            throw new NotImplementedException();
        }

        public void CancelInNetworkSettle(InPayParameter para)
        {
            throw new NotImplementedException();
        }

        public void InNetworkPreSettle(InPayParameter para)
        {
            throw new NotImplementedException();
        }

        public void InNetworkRegister(InPayParameter para)
        {
            throw new NotImplementedException();
        }

        public void InNetworkSettle(InPayParameter para)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
