using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;

namespace Config
{
    public class LogManagerConfig
    { 
        //根目录
        private static string lExceptionPath = "\\Exception.txt";
        static object objWriteFileLock = new object();
        public LogManagerConfig()
        {
        }

        /// <summary>
        /// 本程序的异常文件记录，记录在txt中
        /// </summary>
        /// <param name="Path">放置异常文件的路径</param>
        /// <param name="strException">异常内容</param>
        /// <param name="strPosition">异常产生位置</param>
        static public void RecordException(string Path,string strException, string strPosition)
        {
            lExceptionPath = Path + "\\Exception.txt";
            try
            {
                lock (objWriteFileLock)
                {
                    string str ="\r\n\n********************************************************************* \r\n\r\n";
                    str += string.Format("[{0}][{1}]{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), strPosition, strException);

                    StreamWriter stream = new StreamWriter(lExceptionPath, true);
                    stream.WriteLine(str);
                    stream.Close();
                    //日期中日为3的整数倍且时为零，则调用备份
                    if (DateTime.Now.Day % 3 == 0 && DateTime.Now.Hour == 10)
                        BackUpException();
                }
            }
            catch
            {
                return;
            }
        }
        /// <summary>
        /// 异常记录文件存在且大于300k则备份且删除当前文件
        /// </summary>
        static public void BackUpException()
        {
            try
            {
                string strBackUpPath = "..\\BackUpEp" + DateTime.Now.ToString("yyyyMMddHHmm") + ".txt";
                if (!File.Exists(lExceptionPath))
                    return;
                if (new FileInfo(lExceptionPath).Length > 309200)
                {
                    File.Copy(lExceptionPath, strBackUpPath);
                    File.Delete(lExceptionPath);
                }
            }
            catch
            {
                return;
            }
        }
    }
}
