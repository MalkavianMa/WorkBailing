﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
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
            //Application.Run(new ZX_Business.FrmCeneter());//登录界面
            Application.Run(new Login());//登录界面

        }
    }
}
