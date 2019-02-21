using System;
using System.Collections.Generic;
using System.Text;

namespace PayAPIInstance.tools
{
    /// <summary>
    /// SdaCash参数类
    /// </summary>
    public class DibaoSdaCash
    {

        public DibaoSdaCash(string tbIDCardNo, string txt_YBZF, string tbName, string tbInsuranceID, string tbInsuranceType, string tbInsuranceTypeMC, string tbAidType, string tbAidTypeMC, string tbSerialNo, string tbGENDER, string cmb_mgbz,string cbxMGybText)
        {
            // TODO: Complete member initialization
            this.tbIDCardNo = tbIDCardNo;
            this.txt_YBZF = txt_YBZF;
            this.tbName = tbName;
            this.tbInsuranceID = tbInsuranceID;
            this.tbInsuranceType = tbInsuranceType;
            this.tbInsuranceTypeMC = tbInsuranceTypeMC;
            this.tbAidType = tbAidType;
            this.tbAidTypeMC = tbAidTypeMC;
            this.tbSerialNo = tbSerialNo;
            this.tbGENDER = tbGENDER;
            this.cbxMGyb = cmb_mgbz;
            this.cbxMGybText = cbxMGybText;
        }

        public DibaoSdaCash()
        {
            // TODO: Complete member initialization
        }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string tbIDCardNo { get; set; }
        /// <summary>
        /// 医保支付
        /// </summary>
        public string txt_YBZF { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string tbName { get; set; }
        /// <summary>
        /// 读取医保卡号
        /// </summary>
        public string tbInsuranceID { get; set; }
        /// <summary>
        /// 读取医保类型
        /// </summary>
        public string tbInsuranceType { get; set; }
        /// <summary>
        /// 医保类型说明
        /// </summary>
        public string tbInsuranceTypeMC { get; set; }
        /// <summary>
        /// 读取救助类型
        /// </summary>
        public string tbAidType { get; set; }
        /// <summary>
        /// 救助类型说明
        /// </summary>
        public string tbAidTypeMC { get; set; }
        /// <summary>
        /// 当前救助卡流水号
        /// </summary>
        public string tbSerialNo { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string tbGENDER { get; set; }
        /// <summary>
        /// 是否门规
        /// </summary>
        public string cbxMGyb { get; set; }
        /// <summary>
        /// 病种说明
        /// </summary>
        public string cbxMGybText { get; set; }

    }
}
