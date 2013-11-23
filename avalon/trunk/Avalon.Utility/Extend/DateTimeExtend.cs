using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    public static class DateTimeExtend
    {
        /// <summary>
        /// 获取时间的年周数
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int WeekOfDate(DateTime dateTime)
        {
            int days = dateTime.DayOfYear;
            int week = days / 7;
            if (days % 7 > 0) { ++week; }
            return week;
        }

        /// <summary>
        /// 将 Unix 时间戳（秒）转为 DateTime（本地时间）。
        /// </summary>
        public static DateTime FromUnixTime(double unixTime)
        {
            return NetworkTime.UnixEpochDateTime.AddSeconds(unixTime).ToLocalTime();
        }

        /// <summary>
        /// 将 DateTime 转为 Unix 时间戳（秒）。
        /// </summary>
        public static double ToUnixTime(this DateTime dateTime)
        {
            return (dateTime.ToUniversalTime() - NetworkTime.UnixEpochDateTime).TotalSeconds;
        }

        /// <summary>
        /// 返回两个 DateTime 较大的值。
        /// </summary>
        public static DateTime Max(DateTime time1, DateTime time2)
        {
            return (time1 > time2) ? time1 : time2;
        }

        /// <summary>
        /// 返回两个 DateTime 较小的值。
        /// </summary>
        public static DateTime Min(DateTime time1, DateTime time2)
        {
            return (time1 < time2) ? time1 : time2;
        }

        /// <summary>
        /// 判断当前的时间是否为 Unix 纪元时间。
        /// </summary>
        public static bool IsUnixEpoch(this DateTime date)
        {
            return (int)date.ToUnixTime() == 0;
        }
    }
}
