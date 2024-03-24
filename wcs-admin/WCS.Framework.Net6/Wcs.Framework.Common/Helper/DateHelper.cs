using System;

namespace Wcs.Framework.Common.Helper
{
    public class DateHelper
    {
        /// <summary>
        /// 获取 本周、本月、本季度、本年 的开始时间
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeType">WEEK\MONTH\SEASON\YEAR</param>
        /// <returns></returns>
        public static DateTime GetTimeStartByType(DateTime time, string timeType)
        {
            switch (timeType)
            {
                case "WEEK":
                    return time.AddDays(-(int)time.DayOfWeek + 1);
                case "MONTH":
                    return time.AddDays(-time.Day + 1);
                case "SEASON":
                    var _time = time.AddMonths(0 - ((time.Month - 1) % 3));
                    return _time.AddDays(-_time.Day + 1);
                case "YEAR":
                    return time.AddDays(-time.DayOfYear + 1);
                default: return time;
            }
        }

        /// <summary>
        /// 获取 本周、本月、本季度、本年 的结束时间
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeType">WEEK\MONTH\SEASON\YEAR</param>
        /// <returns></returns>
        public static DateTime GetTimeEndByType(DateTime time, string timeType)
        {
            switch (timeType)
            {
                case "WEEK":
                    return time.AddDays(7 - (int)time.DayOfWeek);
                case "MONTH":
                    return time.AddMonths(1).AddDays(-time.AddMonths(1).Day + 1).AddDays(-1);
                case "SEASON":
                    {
                        var _time = time.AddMonths(3 - ((time.Month - 1) % 3));
                        return _time.AddMonths(1).AddDays(-_time.AddMonths(1).Day + 1).AddDays(-1);
                    }
                case "YEAR":
                    {
                        var _time = time.AddYears(1);
                        return _time.AddDays(-_time.DayOfYear);
                    }
                default: return time;
            }
        }

        public static DateTime StampToDateTime(string time)
        {
            time = time.Substring(0, 10);
            double timestamp = Convert.ToInt64(time);
            System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();
            return dateTime;
        }

        public static string TimeSubTract(DateTime time1, DateTime time2)
        {
            TimeSpan subTract = time1.Subtract(time2);
            return $"{subTract.Days} 天 {subTract.Hours} 时 {subTract.Minutes} 分 ";
        }
        /// <summary>
        ///  时间戳转本地时间-时间戳精确到秒
        /// </summary> 
        public static DateTime ToLocalTimeDateBySeconds(long unix)
        {
            var dto = DateTimeOffset.FromUnixTimeSeconds(unix);
            return dto.ToLocalTime().DateTime;
        }

        /// <summary>
        ///  时间转时间戳Unix-时间戳精确到秒
        /// </summary> 
        public static long ToUnixTimestampBySeconds(DateTime dt)
        {
            DateTimeOffset dto = new DateTimeOffset(dt);
            return dto.ToUnixTimeSeconds();
        }


        /// <summary>
        ///  时间戳转本地时间-时间戳精确到毫秒
        /// </summary> 
        public static DateTime ToLocalTimeDateByMilliseconds(long unix)
        {
            var dto = DateTimeOffset.FromUnixTimeMilliseconds(unix);
            return dto.ToLocalTime().DateTime;
        }

        /// <summary>
        ///  时间转时间戳Unix-时间戳精确到毫秒
        /// </summary> 
        public static long ToUnixTimestampByMilliseconds(DateTime dt)
        {
            DateTimeOffset dto = new DateTimeOffset(dt);
            return dto.ToUnixTimeMilliseconds();
        }
    }
}
