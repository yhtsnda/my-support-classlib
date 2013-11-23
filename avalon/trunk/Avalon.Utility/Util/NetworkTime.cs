using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Avalon.Utility
{
    /// <summary>
    /// 对当前应用程序域的网络时间进行管理
    /// </summary>
    public static class NetworkTime
    {
        /// <summary>
        /// 获取 Unix 时间戳的纪元时间
        /// </summary>
        public static readonly DateTime UnixEpochDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        static ILog log = LogManager.GetLogger(typeof(NetworkTime));
        static TimeSpan timeDiff = new TimeSpan(0);

        /// <summary>
        /// 获取远程服务器时间的耗时
        /// </summary>
        static long getGlobalServertimeDiff = 0;

        /// <summary>
        /// 是否正在发送请求获取远程服务器时间
        /// </summary>
        static bool isSending = false;

        static object syncLocker = new object();

        static NetworkTime()
        {
            GetRemoteTime();
        }

        /// <summary>
        /// 获取纪元时间，可以作为数据库日期字段的默认值(1970-1-1 12:00:00)
        /// </summary>
        public static DateTime Null
        {
            get { return UnixEpochDateTime.AddHours(12); }
        }

        /// <summary>
        /// 获取当前程序域的网络时间
        /// </summary>
        public static DateTime Now
        {
            get
            {
                if (getGlobalServertimeDiff > 500)
                    GetRemoteTime();
                return DateTime.Now.Add(timeDiff);
            }
        }

        /// <summary>
        /// 获取本地时间与网络时间测差值
        /// </summary>
        public static TimeSpan TimeDiff
        {
            get { return timeDiff; }
        }

        /// <summary>
        /// 设置当前程序域网络时间
        /// </summary>
        public static void SetNetworkTime(DateTime networkTime)
        {
            SetNetworkTime(networkTime, false);
        }

        /// <summary>
        /// 设置当前程序域网络时间，是否同步本地时间为可选
        /// </summary>
        /// <param name="networkTime"></param>
        /// <param name="syncLocalTime">是否同步本地时间</param>
        public static void SetNetworkTime(DateTime networkTime, bool syncLocalTime)
        {
            timeDiff = networkTime - DateTime.Now;

            if (syncLocalTime)
            {
                int i = SetLocalSystemTime(networkTime);

                if (i == 1) //系统时间修改成功
                    timeDiff = new TimeSpan(0); //重新设置偏移量
                else
                    log.Error("系统时间修改失败");
            }
        }

        /// <summary>
        /// 设置本地操作系统的时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int SetLocalSystemTime(DateTime dateTime)
        {
            // And then set up a structure with the required properties and call the api from code: 
            SystemTime systNew = new SystemTime();

            // 设置属性 
            systNew.wDay = (ushort)dateTime.Day;
            systNew.wMonth = (ushort)dateTime.Month;
            systNew.wYear = (ushort)dateTime.Year;
            systNew.wHour = (ushort)dateTime.Hour;
            systNew.wMinute = (ushort)dateTime.Minute;
            systNew.wSecond = (ushort)dateTime.Second;
            systNew.wMilliseconds = (ushort)dateTime.Millisecond;

            // 调用API，更新系统时间 
            return SetLocalTime(ref systNew);
        }

        /// <summary>
        /// 获取远程服务器时间
        /// </summary>
        private static void GetRemoteTime()
        {
            if (isSending)
                return;

            lock (syncLocker)
            {
                if (isSending)
                    return;

                isSending = true;
            }
            try
            {
                var timeUrl = ToolSection.Instance.TryGetValue("time/url");
                if (!String.IsNullOrEmpty(timeUrl))
                {
                    ////TimeSpan diff = DateTime.Now.ToUniversalTime() - NetworkTime.UnixEpochDateTime;
                    ////var tm = (long)Math.Floor(diff.TotalMilliseconds);

                    var beginTimestamp = GetUnixTimestamp();
                    WebClient client = new WebClient();
                    string v = client.DownloadString(timeUrl + "?clientTick=" + beginTimestamp.ToString());
                    var endTimestamp = GetUnixTimestamp();

                    //设置获取远程服务器时间耗时
                    getGlobalServertimeDiff = endTimestamp - beginTimestamp;

                    string[] vs = v.Trim(']', '[').Split(',');
                    ////diff = DateTime.Now.ToUniversalTime() - NetworkTime.UnixEpochDateTime;
                    ////tm = (long)Math.Floor(diff.TotalMilliseconds);
                    var remoteTimestamp = Int64.Parse(vs[1]);
                    timeDiff = TimeSpan.FromMilliseconds(remoteTimestamp - ((endTimestamp + beginTimestamp) / 2));
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                lock (syncLocker)
                {
                    isSending = false;
                }
            }
        }

        /// <summary>
        /// 获取当前服务器的Unix时间戳
        /// </summary>
        /// <returns></returns>
        private static long GetUnixTimestamp()
        {
            TimeSpan diff = DateTime.Now.ToUniversalTime() - NetworkTime.UnixEpochDateTime;
            return (long)Math.Floor(diff.TotalMilliseconds);
        }

        #region kernel32.dll 静态方法，修改系统时间

        [DllImport("kernel32.dll", SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "WINAPI")]
        static extern int SetLocalTime(ref SystemTime lpSystemTime);

        //struct for date/time apis 
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "WINAPI")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "WINAPI")]
        private struct SystemTime
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }

        #endregion
    }
}
