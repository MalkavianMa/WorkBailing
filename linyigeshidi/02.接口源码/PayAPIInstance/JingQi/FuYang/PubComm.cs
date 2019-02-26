using System;
using System.Collections.Generic;
using System.Text;

namespace PayAPIInstance.JingQi.FuYang
{
    /// <summary>
    /// 公共固定变量
    /// </summary>
    public class PubComm
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string ConnStr
        {
            get
            {
                return PayAPIUtilities.Config.PayAPIConfig.InstitutionDict[2].Memo;
            }
        }

        /// <summary>
        /// 农合的医疗机构编码
        /// </summary>
        public static string NHInstitutionCode
        {
            get
            {
                return PayAPIUtilities.Config.PayAPIConfig.InstitutionDict[2].InstitutionCode;
            }
        }

        /// <summary>
        /// 医保的医疗机构编码
        /// </summary>
        public static string YBInstitutionCode
        {
            get
            {
                return PayAPIUtilities.Config.PayAPIConfig.InstitutionDict[1].InstitutionCode;
            }
        }
    }
}
