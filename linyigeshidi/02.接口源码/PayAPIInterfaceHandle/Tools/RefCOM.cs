using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace PayAPIInterfaceHandle.Tools
{
    /// <summary>
    /// 利用反射 动态加载COM
    /// </summary>
    public class RefCom : IDisposable
    {
        private Type _tp;
        private Object _comObj;
        //private bool _isInit = false;
        private bool disposed = false;

        public RefCom(string strProgId)
        {
            try
            {
                _tp = Type.GetTypeFromProgID(strProgId); //根据ProgID获取类名
                _comObj = Activator.CreateInstance(_tp, null); //建立实例
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 执行对象中的方法
        /// </summary>
        /// <param name="fieldName">方法名</param>
        /// <returns></returns>
        public object GetMemberReObj(string fieldName)
        {
            return _tp.InvokeMember(fieldName, BindingFlags.GetProperty, null, _comObj, null);
        }

        /// <summary>
        /// 获取对象中属性值
        /// </summary>
        /// <param name="fieldName">方法名</param>
        /// <param name="objCom"></param>
        /// <returns></returns>
        public object GetMemberReObj(string fieldName, object objCom)
        {
            return _tp.InvokeMember(fieldName, BindingFlags.GetProperty, null, objCom, null);
        }

        /// <summary>
        /// 执行对象中的方法
        /// </summary>
        /// <param name="funcName">方法名</param>
        /// <param name="objList">参数列表</param>
        /// <returns></returns>
        public object ExeFuncReObj(string funcName, Object[] objList)
        {
            return _tp.InvokeMember(funcName, BindingFlags.InvokeMethod, null, _comObj, objList);
        }

        /// <summary>
        /// 执行对象中的方法
        /// </summary>
        /// <param name="funcName">方法名</param>
        /// <param name="objList">参数列表</param>
        /// <param name="objCom"></param>
        /// <returns></returns>
        public object ExeFuncReObj(string funcName, Object[] objList, object objCom)
        {
            return _tp.InvokeMember(funcName, BindingFlags.InvokeMethod, null, objCom, objList);
        }

        /// <summary>
        /// 执行对象中的方法
        /// </summary>
        /// <param name="funcName">方法名</param>
        /// <param name="objList">参数列表</param>
        /// <returns></returns>
        public int ExeFuncReInt(string funcName, Object[] objList)
        {
            //MethodInfo ine = _tp.GetMethod(funcName, BindingFlags.Static|BindingFlags.Public);
            return Convert.ToInt32(_tp.InvokeMember(funcName, BindingFlags.InvokeMethod, null, _comObj, objList));
        }


        /// <summary>
        /// 执行对象中的方法
        /// </summary>
        /// <param name="funcName">方法名</param>
        /// <param name="objList">参数列表</param>
        /// <param name="refArr"></param>
        /// <returns></returns>
        public int ExeFuncReInt(string funcName, Object[] objList, bool[] refArr)
        {
            int refLen = refArr.Length;
            ParameterModifier[] paraModiArr = new ParameterModifier[1];
            paraModiArr[0] = new ParameterModifier(refLen);
            for (int i = 0; i < refLen; i++)
            {
                paraModiArr[0][i] = refArr[i];
            }
          //  MethodInfo nn = _tp.GetMethod(funcName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);
           // return Convert.ToInt32(_tp.InvokeMember(funcName, BindingFlags.Default | BindingFlags.InvokeMethod, null, _comObj, objList, paraModiArr, System.Globalization.CultureInfo.CreateSpecificCulture("zh-CN"), null));
         //return Convert.ToInt32(_tp.InvokeMember(funcName, BindingFlags.Default | BindingFlags.InvokeMethod, null, _comObj, objList, paraModiArr, System.Globalization.CultureInfo.CreateSpecificCulture("zh-CN"), null));
            return Convert.ToInt32(_tp.InvokeMember(funcName, BindingFlags.Default | BindingFlags.InvokeMethod, null, _comObj, objList, paraModiArr, System.Globalization.CultureInfo.CreateSpecificCulture("zh-CN"), null));

        }
        public int ExeFuncReInt1(string funcName, Object[] objList, bool[] refArr)
        {
            int refLen = refArr.Length;
            ParameterModifier[] paraModiArr = new ParameterModifier[1];
            paraModiArr[0] = new ParameterModifier(refLen);
            for (int i = 0; i < refLen; i++)
            {
                paraModiArr[0][i] = refArr[i];
            }
            // return Convert.ToInt32(_tp.InvokeMember(funcName, BindingFlags.Default | BindingFlags.InvokeMethod, null, _comObj, objList, paraModiArr, System.Globalization.CultureInfo.CreateSpecificCulture("zh-CN"), null));
            return Convert.ToInt32(_tp.InvokeMember(funcName, BindingFlags.Default | BindingFlags.InvokeMethod, null, _comObj, objList, paraModiArr, System.Globalization.CultureInfo.CreateSpecificCulture("zh-CN"), null));

        }
        /// <summary>
        /// 执行对象中的方法
        /// </summary>
        /// <param name="funcName">方法名</param>
        /// <param name="objList">参数列表</param>
        /// <param name="refArr"></param>
        /// <returns></returns>
        public int ExeFuncReIntFrm(string funcName, Object[] objList, bool[] refArr)
        {
            int ret;
            int refLen = refArr.Length;
            ParameterModifier[] paraModiArr = new ParameterModifier[1];
            paraModiArr[0] = new ParameterModifier(refLen);
            for (int i = 0; i < refLen; i++)
            {
                paraModiArr[0][i] = refArr[i];
            }
            ret = Convert.ToInt32(_tp.InvokeMember(funcName, BindingFlags.Default | BindingFlags.InvokeMethod, null, _comObj, objList, paraModiArr, System.Globalization.CultureInfo.CreateSpecificCulture("zh-CN"), null));
            return ret;
        }

        /// <summary>
        /// 执行对象中的方法
        /// </summary>
        /// <param name="funcName">方法名</param>
        /// <param name="objList">参数列表</param>
        /// <returns></returns>
        public string ExeFuncReStr(string funcName, Object[] objList)
        {
            return Convert.ToString(_tp.InvokeMember(funcName, BindingFlags.InvokeMethod, null, _comObj, objList));
        }

        /// <summary>
        /// 执行对象中的方法
        /// </summary>
        /// <param name="funcName">方法名</param>
        /// <param name="objList">参数列表</param>
        /// <returns></returns>
        public decimal ExeFuncReDec(string funcName, Object[] objList)
        {
            return Convert.ToDecimal(_tp.InvokeMember(funcName, BindingFlags.InvokeMethod, null, _comObj, objList));
        }

        /// <summary>
        ///  释放对象
        /// </summary>
        public void DisposeSelf()
        {
            if (_comObj != null)
            {
                try
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(_comObj);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            _tp = null;
            _comObj = null;
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
                if (_tp == null)
                {
                    _tp = null;
                }
            }
            // 清理非托管资源
            System.Runtime.InteropServices.Marshal.ReleaseComObject(_comObj);
            _comObj = null;
            //让类型知道自己已经被释放
            disposed = true;
        }
    }
}
