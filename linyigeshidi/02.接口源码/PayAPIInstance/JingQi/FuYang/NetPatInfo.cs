using System;
using System.Collections.Generic;
using System.Text;

namespace PayAPIInstance.JingQi.FuYang
{
    /// <summary>
    /// 联网报销患者信息
    /// </summary>
    public class NetPatInfo
    {
        /// <summary>
        /// 医保个人编号
        /// </summary>
        public string medicalNo { get; set; }

        /// <summary>
        /// IC卡号/医疗卡号
        /// </summary>
        public string ICNo { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string patName { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDNo { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string sex { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime birthday { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public float age { get; set; }

        /// <summary>
        /// 医疗人员类别
        /// </summary>
        public string medicalType { get; set; }

        /// <summary>
        /// 医疗人员类别名称
        /// </summary>
        public string medicalTypeName { get; set; }

        /// <summary>
        /// 个人帐户余额
        /// </summary>
        public decimal ICAmount { get; set; }

        /// <summary>
        /// 单位编号
        /// </summary>
        public string companyNo { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string companyName { get; set; }

        /// <summary>
        /// 门诊登记号码
        /// </summary>
        public string outRegNo { get; set; }

        /// <summary>
        /// 住院登记号码/住院流水号
        /// </summary>
        public string inRegNo { get; set; }
        /// <summary>
        /// 住址
        /// </summary>
        public string patAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 人员类别(如一般农民，五保农民，一般职工等)
        /// </summary>
        public string PersonClass
        {
            get;
            set;
        }

        /// <summary>
        /// 区域
        /// </summary>
        public string Canton
        {
            get;
            set;
        }
        
        /// <summary>
        /// 就诊类型
        /// </summary>
        public string strCureId {get;set;}
        /// <summary>
        /// 来院状态
        /// </summary>
        public string strInHosId { get; set; }
        /// <summary>
        /// 入院科室编码
        /// </summary>
        public string strInOfficeId { get; set; }
        /// <summary>
        /// 入院诊断编码
        /// </summary>
        public string strInDiagnoCode { get; set; }
        /// <summary>
        /// 入院诊断名称
        /// </summary>
        public string strInDiagnoName { get; set; }
        /// <summary>
        /// 出院科室编码
        /// </summary>
        public string strOutOfficeId { get; set; }
        /// <summary>
        /// 入院科室名称
        /// </summary>
        public string strInOfficeName { get; set; }
        /// <summary>
        /// 出院科室名称
        /// </summary>
        public string strOutOfficeName { get; set; }
        /// <summary>
        /// 出院状态
        /// </summary>
        public string strOutHosId { get; set; }
        /// <summary>
        /// 出院诊断
        /// </summary>
        public string strOutDiagnoCode { get; set; }
        /// <summary>
        /// 第二诊断名称
        /// </summary>
        public string strSecondDiagno { get; set; }
        /// <summary>
        /// 第二诊断编码
        /// </summary>
        public string strSecondDiaCode { get; set; }
        /// <summary>
        /// 身高
        /// </summary>
        public string strStature { get; set; }
        /// <summary>
        /// 手术编码
        /// </summary>
        public string strOpsId { get; set; }
        /// <summary>
        /// 治疗方式
        /// </summary>
        public string strTreatCode { get; set; }
        /// <summary>
        /// 体重
        /// </summary>
        public string strWeight { get; set; }
        /// <summary>
        /// 经治医生
        /// </summary>
        public string strCureDoctor { get; set; }
        /// <summary>
        /// 入院病区
        /// </summary>
        public string strSectionNo { get; set; }
        /// <summary>
        /// 床号
        /// </summary>
        public string strBedNo { get; set; }
        /// <summary>
        /// 民政通知书
        /// </summary>
        public string strMinisterNotice { get; set; }
        /// <summary>
        /// 生育证号
        /// </summary>
        public string strProcreateNotice { get; set; }
        /// <summary>
        /// 并发症
        /// </summary>
        public string strComplication { get; set; }
        /// <summary>
        /// 成员编码
        /// </summary>
        public string strMemberNo { get; set; }
        /// <summary>
        /// 家庭编码
        /// </summary>
        public string strFamilySysno { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string strTelNo { get; set; }
        /// <summary>
        /// 补偿 类型
        /// </summary>
        public string strRedeemNo { get; set; }
    }
}
