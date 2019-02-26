using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using PayAPIInterface;
using PayAPIInterface.ParaModel;
using PayAPIUtilities.Config;
using PayAPIInterface.Model.Comm;
using PayAPITradeResolver.POS.ZIBO;

namespace PayAPITradeInstance.POS.ZIBO
{
    /// <summary>
    /// 淄博支付接口
    /// </summary>
    public class ZIBOPOSModel : ITradePayInterface
    {
        #region 参数
        /// <summary>
        /// 用来传给银行接口
        /// </summary>
        public string ZHAmount = "";

        /// <summary>
        /// 保存银行返回字符串
        /// </summary>
        public string B_resulet = "";

        /// <summary>
        /// POS返回金额
        /// </summary>
        PosReturn result = new PosReturn();

        /// <summary>
        /// 支付金额
        /// </summary>
        TradePayParameter tradePayPara = new TradePayParameter();
        #endregion 

        #region   中信银行扣款方法，使用posinf.dll
        /// <summary>
        /// 中信银行扣款方法，使用posinf.dll动态链接库
        /// </summary>
        /// <returns></returns>
        public string Bank_Pos_Deduct()
        {
            string Lrc = new Random().Next(100, 999).ToString();
            ZIBOPOSResolver handle = new ZIBOPOSResolver();
            handle.AddListInParas(handle.GetMac(), 8, new char[] { (' ') }, "L"); //POS机号
            handle.AddListInParas(PayAPIConfig.Operator.UserSysId, 8, new char[] { (' ') }, "L");//POS员工号
            handle.AddListInParas("00", 2, new char[] { (' ') }, "L");//交易类型
            handle.AddListInParas(ZHAmount, 12, new char[] { ('0') }, "R"); //交易金额      
            handle.AddListInParas("", 8, new char[] { (' ') }, "R");//原交易日期：退货时用，其他交易空格
            handle.AddListInParas("", 12, new char[] { (' ') }, "R");//原交易参考号：退货时用，其他交易空格
            handle.AddListInParas("", 6, new char[] { (' ') }, "R");//原交易凭证号：撤销时用，其他交易空格
            handle.AddListInParas(Lrc, 3, new char[] { (' ') }, "R");//LRC校验：3位随机数字
            handle.AddListInParas("", 100, new char[] { (' ') }, "R");//全民付(行业信息)

            StringBuilder Baninput = handle.CommInput();
            result = handle.CardTrans(Baninput);
            if (result.ReCode == "00")
            {
                string strresult = result.BankCode + "," + result.BankCardNo + "," + result.CertificateNo + "," + result.Amount + "," +
                    result.ErrMsg + "," + result.MerchantNo + "," + result.TerminalNo + "," + result.BatchNo + "," + result.TransDate + "," +
                    result.TransTime + "," + result.TransNo + "," + result.LicenseNo + "," + result.TallyDate + result.LRC + "," + Lrc;
                return strresult;
            }
            else
            {
                throw new Exception(result.ErrMsg);
            }
        }

        #endregion

        #region 中信银行扣款撤销方法，使用posinf.dll 
        /// <summary>
        /// 中信银行扣款方法，使用posinf.dll
        /// </summary>
        /// <param name="isCancelAll">是否全部撤销</param>
        /// <returns></returns>
        public string Bank_Pos_back(bool isCancelAll)
        {
            string Lrc = new Random().Next(100, 999).ToString();
            /************************************************************************/
            /* 通过CREATE_TIME与当前的日期进行对比，判断是当日撤销还是隔日退货，因为
             * 不同的日期需调用银行不同的方法来进行撤销银行交易*/
            /************************************************************************/
            string Insert_time = Convert.ToDateTime(tradePayPara.SettleInfo.CreateTime).ToString("yyyyMMdd");
            //新增的银联POS功能
            string bank_amount = "0";
            if (isCancelAll)
            {
                bank_amount = Convert.ToString(Convert.ToInt32(Convert.ToDecimal(tradePayPara.SettleInfo.AmountPos) * 100));
            }
            else
            {
                bank_amount = Convert.ToString(Convert.ToInt32(Math.Abs(Convert.ToDecimal(tradePayPara.CommPara.RefundAmount)) * 100));
            }

            /************************************************************************/
            /* 将本地存放的SETTLE_BACK_NO的字符串进行分割成数组，为撤销交易提供参数                                                                     */
            /************************************************************************/
            string[] arrResult = tradePayPara.SettleInfo.SettleBackNo.ToString().Split(',');
            ZIBOPOSResolver handle = new ZIBOPOSResolver();

            //交易参数
            if (Insert_time != DateTime.Now.ToString("yyyyMMdd") || !isCancelAll)
            {
                //如果交易时间与当前日期不一致，使用银行隔日退货
                handle.AddListInParas(handle.GetMac(), 8, new char[] { (' ') }, "L"); //POS机号
                handle.AddListInParas(PayAPIConfig.Operator.UserSysId, 8, new char[] { (' ') }, "L");//POS员工号
                handle.AddListInParas("02", 2, new char[] { (' ') }, "L");//交易类型
                handle.AddListInParas(bank_amount, 12, new char[] { ('0') }, "R"); //交易金额      
                handle.AddListInParas(arrResult[8], 8, new char[] { (' ') }, "R");//原交易日期：退货时用，其他交易空格
                handle.AddListInParas(arrResult[10], 12, new char[] { (' ') }, "R");//原交易参考号：退货时用，其他交易空格
                handle.AddListInParas("", 6, new char[] { (' ') }, "R");//原交易凭证号：撤销时用，其他交易空格
                handle.AddListInParas(Lrc, 3, new char[] { (' ') }, "R");//LRC校验：3位随机数字
                handle.AddListInParas("", 100, new char[] { (' ') }, "R");//全民付(行业信息)
            }
            else
            {
                //交易日期与当前日期一致，就直接使用银行撤销
                handle.AddListInParas(handle.GetMac(), 8, new char[] { (' ') }, "L"); //POS机号
                handle.AddListInParas(PayAPIConfig.Operator.UserSysId, 8, new char[] { (' ') }, "L");//POS员工号
                handle.AddListInParas("01", 2, new char[] { (' ') }, "L");//交易类型
                handle.AddListInParas(bank_amount, 12, new char[] { ('0') }, "R"); //交易金额      
                handle.AddListInParas("", 8, new char[] { (' ') }, "R");//原交易日期：退货时用，其他交易空格
                handle.AddListInParas("", 12, new char[] { (' ') }, "R");//原交易参考号：退货时用，其他交易空格
                handle.AddListInParas(arrResult[2], 6, new char[] { (' ') }, "R");//原交易凭证号：撤销时用，其他交易空格
                handle.AddListInParas(Lrc, 3, new char[] { (' ') }, "R");//LRC校验：3位随机数字
                handle.AddListInParas("", 100, new char[] { (' ') }, "R");//全民付(行业信息)
            }


            StringBuilder Baninput = handle.CommInput();
            //调用银行撤销方法
            result = handle.CardTrans(Baninput);
            if (result.ReCode == "00")
            {
                string strresult = result.BankCode + "," + result.BankCardNo + "," + result.CertificateNo + "," + result.Amount + "," +
                    result.ErrMsg + "," + result.MerchantNo + "," + result.TerminalNo + "," + result.BatchNo + "," + result.TransDate + "," +
                    result.TransTime + "," + result.TransNo + "," + result.LicenseNo + "," + result.TallyDate + result.LRC + "," + Lrc;
                return strresult;
            }
            else
            {
                throw new Exception(result.ErrMsg);
            }
        } 
        #endregion

        /// <summary>
        /// 交易支付
        /// </summary>
        /// <param name="para"></param>
        public void TradePay(TradePayParameter para)
        {
            tradePayPara = para;  
            ZHAmount = Convert.ToInt32(Convert.ToDecimal(tradePayPara.CommPara.TradeAmount) * 100).ToString();
            B_resulet = Bank_Pos_Deduct();

            SaveSettleMain(result);

            tradePayPara.CommPara.NetCardNo = result.BankCardNo;
        }

        /// <summary>
        /// 保存POS结算信息
        /// </summary>
        public void SaveSettleMain(PosReturn PosResult)
        {
            NetworkSettleMain networkSettleMain = tradePayPara.SettleInfo; 
            networkSettleMain.SettleNo = PosResult.TransNo;                                // 原系统参考号
            networkSettleMain.Amount = Convert.ToDecimal(tradePayPara.CommPara.TradeAmount);       //本次POS金额
            networkSettleMain.GetAmount = Convert.ToDecimal(tradePayPara.CommPara.TradeAmount) - Convert.ToDecimal(PosResult.Amount) / 100;    //本次现金支出
            networkSettleMain.SettleBackNo = B_resulet; //交易信息
            networkSettleMain.SettleType = "pos";
            networkSettleMain.AmountPos = Convert.ToDecimal(PosResult.Amount) / 100;

            //添加银联支付方式
            tradePayPara.AddPayType(3, "银联", networkSettleMain.AmountPos);
        }

        /// <summary>
        /// 支付撤销
        /// </summary>
        /// <param name="para"></param>
        public void TradeCancel(TradePayParameter para)
        {
            tradePayPara = para;

            string RestrPos = Bank_Pos_back(true);
            //保存撤销结算信息
            tradePayPara.SettleInfo.SettleBackNo = RestrPos;
            tradePayPara.SettleInfo.SettleNo = result.TransNo;
        }

        /// <summary>
        /// 交易退费（可部分退费）
        /// </summary>
        /// <param name="para"></param>
        public void TradeRefund(TradePayParameter para)
        {
            tradePayPara = para;

            string RestrPos = Bank_Pos_back(false);
            //保存撤销结算信息
            tradePayPara.SettleInfo.SettleBackNo = RestrPos;
            tradePayPara.SettleInfo.SettleNo = result.TransNo;
            //更新退费金额
            tradePayPara.SettleInfo.Amount = tradePayPara.CommPara.RefundAmount;
            tradePayPara.SettleInfo.AmountPos = tradePayPara.CommPara.RefundAmount;

            tradePayPara.CommPara.NetCardNo = result.BankCardNo;
        }

        /// <summary>
        /// 交易查询
        /// </summary>
        /// <param name="para"></param>
        public void TradeQuery(TradePayParameter para)
        {
            throw new NotImplementedException();
        }
    }
}
