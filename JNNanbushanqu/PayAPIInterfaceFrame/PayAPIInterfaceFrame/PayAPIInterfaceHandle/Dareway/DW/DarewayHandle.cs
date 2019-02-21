using PayAPIInterfaceHandle.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using PayAPIUtilities.Log;

namespace PayAPIInterfaceHandle.Dareway.DW
{
    /// <summary>
    /// 地纬处理类
    /// </summary>
    public class DarewayHandle
    { 
        /// <summary>
        /// 反射接口类
        /// </summary>
        public RefCOM SeiProxy;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DarewayHandle()
        {
            SeiProxy = new RefCOM("embeded_interface"); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public string ExeFuncReStr(string p1, object[] p2)
        {
            return SeiProxy.ExeFuncReStr(p1, p2);
        }

        public object ExeFuncReObj(string FuncName, Object[] ObjList)
        {
            return SeiProxy.ExeFuncReObj(FuncName, ObjList);
        }
        public int ExeFuncReInt(string FuncName, Object[] ObjList)
        {
            return SeiProxy.ExeFuncReInt( FuncName,ObjList);
        }
        public decimal ExeFuncReDec(string FuncName, Object[] ObjList)
        {
            return SeiProxy.ExeFuncReDec( FuncName,ObjList);
        }

     
        /// <summary>
        /// 释放COM对象
        /// </summary>
        public void ReleaseComObj()
        {
            try
            {
                if (SeiProxy != null)
                {
                    //MessageBox.Show("1");
                    SeiProxy.Dispose();
                    SeiProxy = new RefCOM("embeded_interface");
                    //MessageBox.Show("2");
                }
            }
            catch (Exception desEx)
            {
                try
                {
                    LogManager.Info("DisposeSelf::" + desEx.Message + "  " + desEx.StackTrace + " " + desEx.Source);
                }
                catch (Exception desEx1)
                {
                    LogManager.Info("DisposeSelf_err::" + desEx1.Message + "  ");
                }
            }
        }



    }
}
