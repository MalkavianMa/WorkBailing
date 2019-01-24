using System;
using System.Collections.Generic;
using System.Text;

namespace DW_YBBX
{
    #region 医保目录下载所用类
    public class YBMLModel
    {
        public string resultcode { get; set; }
        public string resulttext { get; set; }
        public List<YLXM> ylxm_ds { get; set; }
        public List<ZFBL> sxzfbl_ds { get; set; }
    }
    //医疗项目类
    public class YLXM
    {
        public string ylxmbm { get; set; }
        public string ylxmbzmc { get; set; }
        public string py { get; set; }
        public string jsxmbh { get; set; }
        public string mldj { get; set; }
        public string ypbz { get; set; }
        public string qsrq { get; set; } /////
        public string zzrq { get; set; }/////
        public string gg { get; set; }
        public string dw { get; set; }
        public string jxm { get; set; }
        public string sm { get; set; }
        public string dffbz { get; set; }
        public string mztckfbx { get; set; }
        public string zxbz { get; set; }
        public string scqy { get; set; }
        public string cfybz { get; set; }
        public string gxsj { get; set; }////
    }
    //首先自付比例类
    public class ZFBL
    {
        public string ylxmbm { get; set; }
        public string qsrq { get; set; }//
        public string zzrq { get; set; }//
        public string sxzfbl { get; set; }
        public string xj { get; set; }
    }
    #endregion

    #region 中心疾病下载所用类
    //医保疾病
    public class YBJBModel
    {
        public string resultcode { get; set; }
        public string resulttext { get; set; }
        public List<YBJB> ybjb_ds { get; set; }
       
    }
    //医保疾病类
    public class YBJB
    {
        public string jbbm { get; set; }
        public string jbmc { get; set; }
        public string py { get; set; }
        public string mzdblb { get; set; }
        public string sbjgbh { get; set; }
        public string xzfw { get; set; }
        public string zxbz { get; set; }
        public string bz { get; set; }
        public string qsrq { get; set; }
        public string zzrq { get; set; }
    }
    #endregion

    #region 项目对照信息下载所用类
    public class YYXMModel
    {
        public string resultcode { get; set; }
        public string resulttext { get; set; }
        public List<YYXM> yyxm_ds { get; set; }        
        public List<Cxzd> code_ds { get; set; }
    }
    //医疗项目信息类
    public class YYXM
    {
        public string yyxmbm { get; set; }
        public string yyxmmc { get; set; }
        public string ylxmbm { get; set; }
        public string ylxmmc { get; set; }
        public string ypbz { get; set; }
        public string jsxmbh { get; set; }
        public string gg { get; set; }
        public string dw { get; set; }
        public string jxm { get; set; }
        public string qsrq { get; set; }/////////
        public string zzrq { get; set; }//////////
        public string gxsj { get; set; }///////////
        public string spbz { get; set; }
        public string spsj { get; set; }/////////


    }

    //查询字典
    public class Cxzd
    {
    
        public string code { get; set; }/////////
        public string content { get; set; }////////

    }

    //待遇封锁
    public class dyfs
    {
        public string fsyy { get; set; }/////////
        public string fslx { get; set; }/////////
        public string qsrq { get; set; }/////////
        public string zzrq { get; set; }/////////
    }

    //审批查询
    public class Hlcx
    {

        public string hlzl { get; set; }/////////
        public string spbz { get; set; }/////////
        public string spyj { get; set; }/////////
        public string qsrq { get; set; }/////////
    }

    //靶向药审批信息查询
    public class Bxysp
    {

        public string yybm { get; set; }/////////
        public string yymc { get; set; }/////////
        public string ydbm { get; set; }/////////
        public string ydmc { get; set; }/////////
        public string qsrq { get; set; }/////////
        public string zzrq { get; set; }/////////
        public string ylxmbm { get; set; }/////////
        public string ylxmmc { get; set; }/////////
        public string ysbh { get; set; }/////////
        public string ysxm { get; set; }/////////
    }


    //靶向药审批信息查询
    public class jsxx
    {
        public string grbh { get; set; }/////////
        public string xm { get; set; }/////////
        public string brjsrq { get; set; }/////////
        public string jshid { get; set; }/////////
        public string yltclb { get; set; }/////////
        public string yllb { get; set; }/////////
        public string rqlb { get; set; }/////////
        public string zje { get; set; }/////////
        public string ybfdje { get; set; }/////////
        public string grzhzf { get; set; }/////////
        public string xjzf { get; set; }/////////
        public string jsbz { get; set; }/////////
        public string tczf { get; set; }/////////
        public string dezf { get; set; }/////////
        public string gwybz { get; set; }/////////

        public string bcsbdblpzf { get; set; }/////////
        public string yljmje { get; set; }/////////
        public string fprymzjzje { get; set; }/////////
        public string fpthbxje { get; set; }/////////
        public string dbjzje { get; set; }/////////
        public string ztdjbzf { get; set; }/////////
    }


    #endregion
}
