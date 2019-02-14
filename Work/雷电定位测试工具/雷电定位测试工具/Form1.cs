﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace 雷电定位测试工具
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //雷电定位测试工具.LDService.GetFlashReadService LDService = new LDService.GetFlashReadService();

            //雷电定位测试工具.FaultRecordService.FaultRecordServiceService FaultRecordServiceService = new FaultRecordService.FaultRecordServiceService();
            //FaultRecordServiceService.getFaultRecordByDeviceOneName()
        }
        private void writeLog(string log, string logtime)
        {
            try
            {
                this.listBox1.Items.Add(log + "  " + logtime);
                string des = Assembly.GetExecutingAssembly().Location;
                des = des.Substring(0, des.LastIndexOf(@"\")) + "\\Log";
                if (!Directory.Exists(des))
                {
                    Directory.CreateDirectory(des);
                }
                string filename = des + "\\leidian_log " + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                FileStream myFileStream = new FileStream(filename, FileMode.Append);
                StreamWriter sw = new StreamWriter(myFileStream, System.Text.Encoding.Default);
                sw.WriteLine(log + "  " + logtime);
                myFileStream.Flush();
                sw.Close();
                myFileStream.Close();
            }
            catch (System.Exception ex)
            {
                this.listBox1.Items.Add(ex.Message.ToString() + "  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
            }
        }

        private Stopwatch stw = new Stopwatch();

        private void btntest_Click(object sender, EventArgs e)
        {
            stw.Start();
            writeLog("采集数据开始leftLocation =" + tbxleftLocation.Text + "rightLocation=" + tbxRightLocation.Text + "startTime =" + tbxstarttime.Text + "endTime =" + tbxEndtime.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

            雷电定位测试工具.LDService.GetFlashReadService LDService = new LDService.GetFlashReadService();
            XmlDocument doc = new XmlDocument();
            string result = "";
            // string getFlash = "";

            if (true)
            {
                //result = LDService.getFlashByRect(tbxleftLocation.Text, tbxRightLocation.Text, tbxstarttime.Text, tbxEndtime.Text, tbxStartline.Text, tbxLineCount.Text);

                // getFlash =  LDService.getFlashByRect(tbxleftLocation.Text, tbxRightLocation.Text, tbxstarttime.Text, tbxEndtime.Text, tbxStartline.Text, tbxLineCount.Text);
                // XmlNode root = doc.DocumentElement;
                // doc.LoadXml(getFlash);
                try
                {
                    // 雷电定位测试工具.QueryService queryservice = new 雷电定位测试工具.QueryService.QueryService();

                    doc.LoadXml(LDService.getFlashByRect(tbxleftLocation.Text, tbxRightLocation.Text, tbxstarttime.Text, tbxEndtime.Text, tbxStartline.Text, tbxLineCount.Text));
                    result = doc.InnerXml.ToString();

                    writeLog("返回" + result, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
                }
                catch (Exception ex)
                {
                    writeLog("异常" + ex.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
                }
            }

            //   private Stopwatch stw = new Stopwatch();
            stw.Stop();
            writeLog("采集数据结束，耗时 " + stw.Elapsed.Minutes + "分 " + stw.Elapsed.Seconds + "秒" + stw.Elapsed.Milliseconds + "毫秒。", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));




        }

        private void btnLineQuery_Click(object sender, EventArgs e)
        {
            int FRrtn = -1;
            string FRerror = "";

            try
            {
                雷电定位测试工具.FaultRecordService.FaultRecordServiceService FRService = new FaultRecordService.FaultRecordServiceService();
                雷电定位测试工具.FaultRecordService.time startTime = new FaultRecordService.time();// tbxWaveStartTime.Text;
                雷电定位测试工具.FaultRecordService.time endTime = new FaultRecordService.time();// tbxWaveStartTime.Text;
                雷电定位测试工具.FaultRecordService.faultRecordRtn FRfaultRecordRtn = new FaultRecordService.faultRecordRtn();

                DateTime startTime1 = Convert.ToDateTime(tbxWaveStartTime.Text);
                DateTime endTime1 = Convert.ToDateTime(tbxWaveEndTime.Text);
                writeLog("datetime类型 startTime1" + startTime1.ToString() + "endTime1" + endTime1.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                try
                {
                    ConvertTime(startTime, startTime1);
                    ConvertTime(endTime, endTime1);
                }
                catch (Exception ex)
                {

                    writeLog("异常" + ex.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                }

                writeLog("***time类型 startTime" + startTime.ToString() + "endtime" + endTime.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                FRfaultRecordRtn = FRService.getFaultRecordByStationName(tbxdeviceOneName.Text, startTime, endTime);
                //FRService.getFaultRecordByDeviceOneName(tbxdeviceOneName.Text, startTime, endTime);
                // FRService.getFaultRecordByStationName(tbxdeviceOneName.Text, startTime, endTime);

                //FRrtn = FRService.getFaultRecordByStationName(tbxdeviceOneName.Text, startTime, endTime).rtn;
                //FRerror = FRService.getFaultRecordByStationName(tbxdeviceOneName.Text, startTime, endTime).error;
                //FRfaultRecordRtn.faultRecords = FRService.getFaultRecordByStationName(tbxdeviceOneName.Text, startTime, endTime).faultRecords;

                FRrtn = FRfaultRecordRtn.rtn;
                FRerror = FRfaultRecordRtn.error;
                writeLog(FRrtn.ToString() + FRerror.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));


                writeLog(FRfaultRecordRtn.faultRecords.Length.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                for (int i = 0; i < FRfaultRecordRtn.faultRecords.Length; i++)
                {
                    writeLog("返回rtn:" + FRrtn.ToString() + "返回error:" + FRerror + "返回faultRecordRtn" + "编号" + FRfaultRecordRtn.faultRecords[i].id.ToString() + "一次设备名" + FRfaultRecordRtn.faultRecords[i].deviceOneName.ToString() + "故障时间毫秒值" + FRfaultRecordRtn.faultRecords[i].faultTime.month.ToString() + "主站名" + FRfaultRecordRtn.faultRecords[i].stationName.ToString() + "录波器名" + FRfaultRecordRtn.faultRecords[i].recorderName.ToString() + "故障相别" + FRfaultRecordRtn.faultRecords[i].phase.ToString() + "故障测距" + FRfaultRecordRtn.faultRecords[i].location.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                }
            }
            catch (Exception ex)
            {

                writeLog("异常" + ex.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
            }
            // FRerror = FRService.getFaultRecordByDeviceOneName(tbxdeviceOneName.Text,,).error;
            //FaultRecordServiceService.getFaultRecordByDeviceOneName()
        }

        private static void ConvertTime(FaultRecordService.time startTime, DateTime startTime1)
        {

            startTime.day = startTime1.Day;
            startTime.hour = startTime1.Hour;
            startTime.minute = startTime1.Minute;
            startTime.month = startTime1.Month;
            startTime.msecond = startTime1.Millisecond;
            startTime.second = startTime1.Second;
            startTime.year = startTime1.Year;

        }

        private void btnWave_Click(object sender, EventArgs e)
        {
            long waveId = Convert.ToInt64(tbxWaveId.Text);// Convert.ToUInt32(tbxWaveId);
            int rtn = -1;
            byte[] bytes = new byte[] { };
            string error = "";
            try
            {
                雷电定位测试工具.FaultRecordService.FaultRecordServiceService FRService = new FaultRecordService.FaultRecordServiceService();

                //rtn = FRService.getFile(waveId, tbxWaveExtension.Text).rtn;
                //bytes = FRService.getFile(waveId, tbxWaveExtension.Text).bytes;

                //FileRtn fr = new FileRtn();
                //var fileRtn1 = FRService.getFile(waveId, tbxWaveExtension.Text);
                //fr = GetFileRtn1(fileRtn1);
                雷电定位测试工具.FaultRecordService.fileRtn fr = new FaultRecordService.fileRtn();
                fr = FRService.getFile(waveId, tbxWaveExtension.Text);
                rtn = fr.rtn;
                writeLog("返回wenjian** 返回rtn:" + rtn.ToString() + "返回error：" + error, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                bytes = fr.bytes;
                error = fr.error;
                writeLog("返回wenjian**" + bytes.Length.ToString() + "返回rtn:" + rtn.ToString() + "返回error：" + error, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                if (bytes.Length != 0)
                {
                    // FileStream opBytes
                    waveDownload(bytes);
                };
                //error = FRService.getFile(waveId, tbxWaveExtension.Text).error;
                //  writeLog("返回rtn:" + rtn + "返回ByteS:" + bytes + "返回error：" + error, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                writeLog("返回rtn:" + rtn.ToString() + "返回error：" + error, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
            }
            catch (Exception ex)
            {
                writeLog("异常" + ex.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                // throw;
            }


        }



        private void waveDownload(byte[] bytes)
        {
            string des = Assembly.GetExecutingAssembly().Location;
            des = des.Substring(0, des.LastIndexOf(@"\")) + "\\Log";
            if (!Directory.Exists(des))
            {
                Directory.CreateDirectory(des);
            }
            string filename = des + "\\download " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff") + "." + tbxWaveExtension.Text;
            System.IO.File.WriteAllBytes(filename, bytes);

        }

        private void btnCalculator_Click(object sender, EventArgs e)
        {
            PositionModel po = new PositionModel();
            Double poWave = Convert.ToDouble(tbxleftLocation.Text.Split('_')[0]);
            Double pwWave = Convert.ToDouble(tbxleftLocation.Text.Split('_')[1]);
            po = DistanceHelper.FindNeighPosition(poWave, pwWave, Convert.ToDouble(tbxRange.Text));//(Convert.ToDouble(tbxleftLocation.Text),Convert.ToDouble(tbxRightLocation.Text), rangWave);
            richTextBox1.Text = "返回" + po.MaxLat + "|" + po.MaxLng + "|" + po.MinLat + "|" + po.MinLng;
            writeLog("返回MAX " + po.MaxLat + "|" + po.MaxLng + "|MIN" + po.MinLat + "|" + po.MinLng, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
        }

        private void btnKMcal_Click(object sender, EventArgs e)
        {
            double result = DistanceHelper.GetDistance(Convert.ToDouble(tbxlat.Text), Convert.ToDouble(tbxlng.Text), Convert.ToDouble(tbxlat2.Text), Convert.ToDouble(tbxlng2.Text));
            writeLog("返回计算距离" + result, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

        }


        //参数（起点坐标，角度，斜边长（距离）） 这是一个基本的三角函数应用
        //private double[] getNewPoint(double[] point, double angle, double bevel)
        //{
        //    //在Flash中顺时针角度为正，逆时针角度为负
        //    //换算过程中先将角度转为弧度
        //    var radian = angle * Math.PI / 180;
        //    var xMargin = Math.Cos(radian) * bevel;
        //    var yMargin = Math.Sin(radian) * bevel;
        //    return new double[] { point[0] + xMargin, point[1] + yMargin };

        //}



        private void button1_Click(object sender, EventArgs e)
        { //
            if (!string.IsNullOrEmpty(textBox2.Text))
            {

                double poWave = Convert.ToSingle(textBox1.Text.Split('_')[0]);
                double pwWave = Convert.ToSingle(textBox1.Text.Split('_')[1]);
                float prWave = Convert.ToSingle(textBox2.Text);

                PositionModel bto = new 雷电定位测试工具.PositionModel();
                bto = DistanceHelper.FindNeighPosition(poWave, pwWave, prWave);

                richTextBox2.Text = "左下:经度\r" + bto.MinLng + "\r纬度\r" + bto.MinLat + "\r" + "右上:经度\r" + bto.MaxLng + "\r纬度:\r" + bto.MaxLat;


                // stw.Start();
                //writeLog("采集数据开始(leftLocation =rightLocation =startTime =endTime =)...",DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss.ffffff"));




                //stw.Stop(); 
                /// writeLog("采集数据结束，耗时 " + stw.Elapsed.Minutes + "分 " +stw.Elapsed.Seconds+"秒"+stw.Elapsed.Milliseconds+"毫秒。")
                //      Double poWave = Convert.ToDouble(tbxleftLocation.Text.Split('_')[0]);
                //     Double pwWave = Convert.ToDouble(tbxleftLocation.Text.Split('_')[1]);
                //  double[] point = { poWave, pwWave };
                //  double bevel = 5 * Math.Sqrt(2);
                ////double [] result=   getNewPoint(point, 45, bevel);
                //PointF fo = new PointF();
                //fo.X =poWave;
                //fo.Y = pwWave;
                //richTextBox2.Text = getNewPoint(fo, 45, bevel).X.ToString()+"\n" + getNewPoint(fo, 45, bevel).Y.ToString();
                // richTextBox2.Text = result[0].ToString() +"\n"+ result[1].ToString (); 
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void tbxWaveEndTime_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbxWaveStartTime_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbxdeviceOneName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnZhuzhan_Click(object sender, EventArgs e)
        {
            int FRrtn = -1;
            string FRerror = "";

            try
            {
                雷电定位测试工具.FaultRecordService.FaultRecordServiceService FRService = new FaultRecordService.FaultRecordServiceService();
                雷电定位测试工具.FaultRecordService.time startTime = new FaultRecordService.time();// tbxWaveStartTime.Text;
                雷电定位测试工具.FaultRecordService.time endTime = new FaultRecordService.time();// tbxWaveStartTime.Text;
                雷电定位测试工具.FaultRecordService.faultRecordRtn FRfaultRecordRtn = new FaultRecordService.faultRecordRtn();

                DateTime startTime1 = Convert.ToDateTime(tbxZhuzhanSta.Text);
                DateTime endTime1 = Convert.ToDateTime(tbxZhuzhanStop.Text);
                writeLog("datetime类型 startTime1" + startTime1.ToString() + "endTime1" + endTime1.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                try
                {
                    ConvertTime(startTime, startTime1);
                    ConvertTime(endTime, endTime1);
                }
                catch (Exception ex)
                {

                    writeLog("异常" + ex.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                }

                writeLog("zhuzhantime类型 startTime" + startTime.ToString() + "endtime" + endTime.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                FRfaultRecordRtn = FRService.getFaultRecordByMainstationName(tbxZhuzhan.Text, startTime, endTime);
                //FRService.getFaultRecordByDeviceOneName(tbxdeviceOneName.Text, startTime, endTime);
                // FRService.getFaultRecordByStationName(tbxdeviceOneName.Text, startTime, endTime);

                //FRrtn = FRService.getFaultRecordByStationName(tbxdeviceOneName.Text, startTime, endTime).rtn;
                //FRerror = FRService.getFaultRecordByStationName(tbxdeviceOneName.Text, startTime, endTime).error;
                //FRfaultRecordRtn.faultRecords = FRService.getFaultRecordByStationName(tbxdeviceOneName.Text, startTime, endTime).faultRecords;

                FRrtn = FRfaultRecordRtn.rtn;
                FRerror = FRfaultRecordRtn.error;
                writeLog(FRrtn.ToString() + FRerror.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
                writeLog(FRfaultRecordRtn.faultRecords.Length.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));


                writeLog(FRfaultRecordRtn.faultRecords.Length.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                for (int i = 0; i < FRfaultRecordRtn.faultRecords.Length; i++)
                {
                    writeLog("返回rtn:" + FRrtn.ToString() + "返回error:" + FRerror + "返回faultRecordRtn" + "编号" + FRfaultRecordRtn.faultRecords[i].id.ToString() + "一次设备名" + FRfaultRecordRtn.faultRecords[i].deviceOneName.ToString() + "故障时间毫秒值" + FRfaultRecordRtn.faultRecords[i].faultTime.month.ToString() + "主站名" + FRfaultRecordRtn.faultRecords[i].stationName.ToString() + "录波器名" + FRfaultRecordRtn.faultRecords[i].recorderName.ToString() + "故障相别" + FRfaultRecordRtn.faultRecords[i].phase.ToString() + "故障测距" + FRfaultRecordRtn.faultRecords[i].location.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                }
            }
            catch (Exception ex)
            {

                writeLog("异常" + ex.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
            }
        }

        private void btnYci_Click(object sender, EventArgs e)
        {
            int FRrtn = -1;
            string FRerror = "";

            try
            {
                雷电定位测试工具.FaultRecordService.FaultRecordServiceService FRService = new FaultRecordService.FaultRecordServiceService();
                雷电定位测试工具.FaultRecordService.time startTime = new FaultRecordService.time();// tbxWaveStartTime.Text;
                雷电定位测试工具.FaultRecordService.time endTime = new FaultRecordService.time();// tbxWaveStartTime.Text;
                雷电定位测试工具.FaultRecordService.faultRecordRtn FRfaultRecordRtn = new FaultRecordService.faultRecordRtn();

                DateTime startTime1 = Convert.ToDateTime(tbxYsta.Text);
                DateTime endTime1 = Convert.ToDateTime(tbxYstop.Text);
                writeLog("datetime类型 startTime1" + startTime1.ToString() + "endTime1" + endTime1.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                try
                {
                    ConvertTime(startTime, startTime1);
                    ConvertTime(endTime, endTime1);
                }
                catch (Exception ex)
                {

                    writeLog("异常" + ex.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                }

                writeLog("*yici**time类型 startTime" + startTime.ToString() + "endtime" + endTime.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                FRfaultRecordRtn = FRService.getFaultRecordByDeviceOneName(tbxYiciName.Text, startTime, endTime);
                //FRService.getFaultRecordByDeviceOneName(tbxdeviceOneName.Text, startTime, endTime);
                // FRService.getFaultRecordByStationName(tbxdeviceOneName.Text, startTime, endTime);

                //FRrtn = FRService.getFaultRecordByStationName(tbxdeviceOneName.Text, startTime, endTime).rtn;
                //FRerror = FRService.getFaultRecordByStationName(tbxdeviceOneName.Text, startTime, endTime).error;
                //FRfaultRecordRtn.faultRecords = FRService.getFaultRecordByStationName(tbxdeviceOneName.Text, startTime, endTime).faultRecords;

                FRrtn = FRfaultRecordRtn.rtn;
                FRerror = FRfaultRecordRtn.error;
                writeLog(FRrtn.ToString() + FRerror.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));


                writeLog(FRfaultRecordRtn.faultRecords.Length.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                for (int i = 0; i < FRfaultRecordRtn.faultRecords.Length; i++)
                {
                    writeLog("返回rtn:" + FRrtn.ToString() + "返回error:" + FRerror + "返回faultRecordRtn" + "编号" + FRfaultRecordRtn.faultRecords[i].id.ToString() + "一次设备名" + FRfaultRecordRtn.faultRecords[i].deviceOneName.ToString() + "故障时间毫秒值" + FRfaultRecordRtn.faultRecords[i].faultTime.month.ToString() + "主站名" + FRfaultRecordRtn.faultRecords[i].stationName.ToString() + "录波器名" + FRfaultRecordRtn.faultRecords[i].recorderName.ToString() + "故障相别" + FRfaultRecordRtn.faultRecords[i].phase.ToString() + "故障测距" + FRfaultRecordRtn.faultRecords[i].location.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                }
            }
            catch (Exception ex)
            {

                writeLog("异常" + ex.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int FRrtn = -1;
            string FRerror = "";

            try
            {
                雷电定位测试工具.FaultRecordService.FaultRecordServiceService FRService = new FaultRecordService.FaultRecordServiceService();
                雷电定位测试工具.FaultRecordService.time startTime = new FaultRecordService.time();// tbxWaveStartTime.Text;
                雷电定位测试工具.FaultRecordService.time endTime = new FaultRecordService.time();// tbxWaveStartTime.Text;
                雷电定位测试工具.FaultRecordService.faultRecordRtn FRfaultRecordRtn = new FaultRecordService.faultRecordRtn();

                DateTime startTime1 = Convert.ToDateTime(tbxRsta.Text);
                DateTime endTime1 = Convert.ToDateTime(tbxRstop.Text);
                writeLog("datetime类型 startTime1" + startTime1.ToString() + "endTime1" + endTime1.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                try
                {
                    ConvertTime(startTime, startTime1);
                    ConvertTime(endTime, endTime1);
                }
                catch (Exception ex)
                {

                    writeLog("异常" + ex.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                }

                writeLog("luboming***time类型 startTime" + startTime.ToString() + "endtime" + endTime.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                FRfaultRecordRtn = FRService.getFaultRecordByRecorderName(tbxRecorderName.Text, startTime, endTime);
                //FRService.getFaultRecordByDeviceOneName(tbxdeviceOneName.Text, startTime, endTime);
                // FRService.getFaultRecordByStationName(tbxdeviceOneName.Text, startTime, endTime);

                //FRrtn = FRService.getFaultRecordByStationName(tbxdeviceOneName.Text, startTime, endTime).rtn;
                //FRerror = FRService.getFaultRecordByStationName(tbxdeviceOneName.Text, startTime, endTime).error;
                //FRfaultRecordRtn.faultRecords = FRService.getFaultRecordByStationName(tbxdeviceOneName.Text, startTime, endTime).faultRecords;

                FRrtn = FRfaultRecordRtn.rtn;
                FRerror = FRfaultRecordRtn.error;
                writeLog(FRrtn.ToString() + FRerror.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));


                writeLog(FRfaultRecordRtn.faultRecords.Length.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                for (int i = 0; i < FRfaultRecordRtn.faultRecords.Length; i++)
                {
                    writeLog("返回rtn:" + FRrtn.ToString() + "返回error:" + FRerror + "返回faultRecordRtn" + "编号" + FRfaultRecordRtn.faultRecords[i].id.ToString() + "一次设备名" + FRfaultRecordRtn.faultRecords[i].deviceOneName.ToString() + "故障时间毫秒值" + FRfaultRecordRtn.faultRecords[i].faultTime.month.ToString() + "主站名" + FRfaultRecordRtn.faultRecords[i].stationName.ToString() + "录波器名" + FRfaultRecordRtn.faultRecords[i].recorderName.ToString() + "故障相别" + FRfaultRecordRtn.faultRecords[i].phase.ToString() + "故障测距" + FRfaultRecordRtn.faultRecords[i].location.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                }
            }
            catch (Exception ex)
            {

                writeLog("异常" + ex.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
            }
        }


        // string DrawLargeRectangle = "";
        PositionModel DrawLargeRectangle = new 雷电定位测试工具.PositionModel();

        private void btnDrawALargeRectangle_Click(object sender, EventArgs e)
        {
            float poWave = Convert.ToSingle(tbxCenterPoint.Text.Split('_')[0]);
            float pwWave = Convert.ToSingle(tbxCenterPoint.Text.Split('_')[1]);
            float prWave = Convert.ToSingle(tbxAcquisitionRange.Text);

            PositionModel bto = new 雷电定位测试工具.PositionModel();

            DrawLargeRectangle = DistanceHelper.FindNeighPosition(poWave, pwWave, prWave);
            bto = DistanceHelper.FindNeighPosition(poWave, pwWave, prWave);
            // DrawLargeRectangle= "左下:经度\r" + bto.MinLng + "\r纬度\r" + bto.MinLat + "\r" + "右上:经度\r" + bto.MaxLng + "\r纬度:\r" + bto.MaxLat;
            richTextBox2.Text = "左下:经度\r" + bto.MinLng + "\r纬度\r" + bto.MinLat + "\r" + "右上:经度\r" + bto.MaxLng + "\r纬度:\r" + bto.MaxLat;

        }

        private void btnDrawASmallRectangle_Click(object sender, EventArgs e)
        {
            // if (!string.IsNullOrEmpty(DrawLargeRectangle))
            {
                string RectangleStart, RectangleStartTwo, RectangleStartThree, RectangleStop = "";
                RectangleStart = DrawLargeRectangle.MinLng.ToString() + "_" + DrawLargeRectangle.MinLat.ToString();
                RectangleStartTwo = DrawLargeRectangle.MaxLng.ToString() + "_" + DrawLargeRectangle.MaxLat.ToString();
                RectangleStartThree = DrawLargeRectangle.MinLng.ToString() + "_" + DrawLargeRectangle.MinLat.ToString();
                RectangleStop = DrawLargeRectangle.MaxLng.ToString() + "_" + DrawLargeRectangle.MinLat.ToString();
                DrawASmall(RectangleStart, RectangleStop, DrawLargeRectangle.MaxLng, DrawLargeRectangle.MinLat);

            }
        }

        
        private void DrawASmall(string RectangleStart, string RectangleStop,double   RectangleStartTwoLng,double RectangleStartThreeLat)
        {
             double btnLng;
            //小矩形暂定2km的长和宽//中心点变为左上,依次推左下 和右上//实际画取1kM的正方形
            double poWave = Convert.ToSingle(RectangleStart.Split('_')[0]);
            double pwWave = Convert.ToSingle(RectangleStart.Split('_')[1]);
            float prWave = 2;

            PositionModel bta= new 雷电定位测试工具.PositionModel();
           bta = DistanceHelper.FindNeighPosition(poWave, pwWave, prWave);
          //btnLng = btm.MaxLng;

            // for (; btnLng < RectangleStartTwoLng; DrawASmall(RectangleStart, "", RectangleStartTwoLng, RectangleStartThreeLat))//经度缩减 纬度不变

            {
               

                //writeLog("返回小矩形的左下经度" + poWave + "纬度" + btm.MinLat + "右上经度:" + btm.MaxLng + "右上纬度" + pwWave, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
              //  RectangleStart = btm.MaxLng.ToString() + "_" + pwWave.ToString();

                for (double  i = DrawASmallLat(poWave, bta.MinLat, RectangleStartThreeLat).MinLat;  RectangleStartThreeLat<i; bta.MinLat = DrawASmallLat(poWave, i, RectangleStartThreeLat).MinLat)////--纬度追加递减
                {
                    // PositionModel btnLat=new 雷电定位测试工具.PositionModel ();
                    // btnLat = DrawASmallLat(poWave, btm.MinLat, RectangleStartThreeLat):
                   
                   
                }                //DrawASmall(RectangleStart, "", RectangleStartTwoLng);
            }
        }
       
        private PositionModel DrawASmallLat(double poWave, double minLat, double rectangleStartThreeLat)
        {
            double btnLat;

            float prWave = 2;//暂定2km

            PositionModel btm = new 雷电定位测试工具.PositionModel();
            btm = DistanceHelper.FindNeighPosition(poWave, minLat, prWave);
            btnLat = btm.MinLat;
            writeLog("返回小矩形的左下经度" + poWave + "纬度" + btm.MinLat + "右上经度:" + btm.MaxLng + "右上纬度" + minLat, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

            return btm;
            // throw new NotImplementedException();
        }





        //参数（起点坐标，角度，斜边长（距离）） 这是一个基本的三角函数应用
        //private PointF getNewPoint(PointF point, double angle, double bevel)
        //{
        //    //在Flash中顺时针角度为正，逆时针角度为负
        //    //换算过程中先将角度转为弧度
        //    var radian = angle * Math.PI / 180;
        //    var xMargin = Math.Cos(radian) * bevel;
        //    var yMargin = Math.Sin(radian) * bevel;
        //    return new PointF(point.X + (float)xMargin, point.Y + (float)yMargin);

        //}






        ///// <remarks/>
        //[System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://www.sgcc.com.cn/sggis/service/gisservice", ResponseNamespace = "http://www.sgcc.com.cn/sggis/service/gisservice", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        //[return: System.Xml.Serialization.XmlElementAttribute("out")]
        //public string queryPSR(string inputXML)
        //{
        //    object[] results = this.Invoke("queryPSR", new object[] {
        //                inputXML});
        //    return ((string)(results[0]));
        //}
    }
}
