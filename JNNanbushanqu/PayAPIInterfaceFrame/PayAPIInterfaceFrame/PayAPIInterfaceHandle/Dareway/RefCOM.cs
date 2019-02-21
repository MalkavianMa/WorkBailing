using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace PayAPIInterfaceHandle.Dareway
{
    /// <summary>
    /// 利用反射 动态加载COM
    /// </summary>
    public class RefCOM:IDisposable
    {
        private Type tp;
        private Object COMObj;
        private bool _isInit = false;
        private bool disposed = false;


        public RefCOM(string strProgID)
        {
            try
            {
                tp = Type.GetTypeFromProgID(strProgID); //根据ProgID获取类名
                COMObj = Activator.CreateInstance(tp, null);  //建立实例
                _isInit = true;
                //MessageBox.Show("已初始化");
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void DisposeSelf() 
        {
            if (COMObj != null)
            {
                try
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(COMObj);
                }catch(Exception ex){
                    //MessageBox.Show("未能释放对象");
                    throw ex;
                }
            }
            //MessageBox.Show("tp");
            tp = null;
            //MessageBox.Show("COMObj");
            COMObj = null;
            //MessageBox.Show("_isInit");
            _isInit = false;
            //MessageBox.Show("GC");
            _isInit = false;
            //MessageBox.Show("GC");
            GC.Collect();
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        public void Dispose() 
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 执行对象中的方法
        /// </summary>
        /// <param name="fieldName">方法名</param>
        /// <returns></returns>
        public object GetMemberReObj(string fieldName)
        {
            return tp.InvokeMember(fieldName, BindingFlags.GetProperty, null, COMObj, null);
        }

        /// <summary>
        /// 获取对象中属性值
        /// </summary>
        /// <param name="fieldName">方法名</param>
        /// <returns></returns>
        public object GetMemberReObj(string fieldName,object objCom)
        {
            return tp.InvokeMember(fieldName, BindingFlags.GetProperty, null, objCom, null);
        }

        /// <summary>
        /// 执行对象中的方法
        /// </summary>
        /// <param name="FuncName">方法名</param>
        /// <param name="ObjList">参数列表</param>
        /// <returns></returns>
        public object ExeFuncReObj(string FuncName, Object[] ObjList)
        {
            return tp.InvokeMember(FuncName, BindingFlags.InvokeMethod, null, COMObj, ObjList);
        }

        /// <summary>
        /// 执行对象中的方法
        /// </summary>
        /// <param name="FuncName">方法名</param>
        /// <param name="ObjList">参数列表</param>
        /// <returns></returns>
        public object ExeFuncReObj(string FuncName, Object[] ObjList,object objCom)
        {
            return tp.InvokeMember(FuncName, BindingFlags.InvokeMethod, null, objCom, ObjList);
        }

        /// <summary>
        /// 执行对象中的方法
        /// </summary>
        /// <param name="FuncName">方法名</param>
        /// <param name="ObjList">参数列表</param>
        /// <returns></returns>
        public int ExeFuncReInt(string FuncName, Object[] ObjList)
        {
            return Convert.ToInt32(tp.InvokeMember(FuncName, BindingFlags.InvokeMethod, null, COMObj, ObjList));
        }

        /// <summary>
        /// 执行对象中的方法
        /// </summary>
        /// <param name="FuncName">方法名</param>
        /// <param name="ObjList">参数列表</param>
        /// <returns></returns>
        public string ExeFuncReStr(string FuncName, Object[] ObjList)
        {
            return Convert.ToString(tp.InvokeMember(FuncName, BindingFlags.InvokeMethod, null, COMObj, ObjList));
        }

        /// <summary>
        /// 执行对象中的方法
        /// </summary>
        /// <param name="FuncName">方法名</param>
        /// <param name="ObjList">参数列表</param>
        /// <returns></returns>
        public decimal ExeFuncReDec(string FuncName, Object[] ObjList)
        {
            return Convert.ToDecimal(tp.InvokeMember(FuncName, BindingFlags.InvokeMethod, null, COMObj, ObjList));
        }


        /// <summary>
        /// 非密封类修饰用protected virtual
        /// 密封类修饰用private
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                // 清理托管资源
                if (tp != null)
                {
                    tp = null;
                }
            }
            // 清理非托管资源
            System.Runtime.InteropServices.Marshal.ReleaseComObject(COMObj);
            COMObj = null;
            //让类型知道自己已经被释放
            disposed = true;
        }
    }
}
