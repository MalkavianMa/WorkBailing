using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DW_YBBX.ZX_Business;
namespace DW_YBBX
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());//登录界面
            //Form1
            Application.Run(new Login());//登录界面

        }
    }
}
