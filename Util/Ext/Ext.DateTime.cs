
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Util.Ext;

namespace Util.Ext
{
    public static partial class Ext
    {



        public static string ToTimestamp(this DateTime dateTime)
        {

            var stamp = dateTime - TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            return stamp.TotalSeconds.ToString();
        }

        public static string ToTimeSpanStr(this TimeSpan timespan)
        {
            return timespan.ToString(@"hh\:mm\:ss");
        }
        public static TimeSpan TimeParse(this string input)
        {
            TimeSpan.TryParse(input, out TimeSpan res);
            return res;
        }
        public static bool TimeIsWork(TimeSpan timeStart, TimeSpan timeEnd, TimeSpan now)
        {

            if (timeEnd < timeStart)
            {
                timeEnd = timeEnd.Add(TimeSpan.FromDays(1));
            }
            var start = DateTime.Now.Date.Add(timeStart);
            var end = DateTime.Now.Date.Add(timeEnd);
            var iswork = DateTime.Now >= start && DateTime.Now < end;
            return iswork;
        }

        public static TimeSpan ToTimeSpan(this string input, bool utc = false)
        {
            TimeSpan timeSpan = new TimeSpan();
            Regex regex = new Regex(@"(?<time>\d{2}:\d{2}(:\d{2})?)((?<zone>[+-]\d{2}):\d{2})?");
            if (regex.IsMatch(input))
            {
                var match = regex.Match(input);
                timeSpan = TimeSpan.Parse(match.Groups["time"].Value);
                if (utc && !match.Groups["zone"].Value.isNull())
                {
                    var zone = match.Groups["zone"].Value.ToInt();
                    timeSpan = timeSpan.Add(new TimeSpan(-zone, 0, 0));
                }
            }
            return timeSpan;
        }

        public static int TimeSpanStrZone(this string input)
        {
            TimeSpan timeSpan = new TimeSpan();
            Regex regex = new Regex(@"(?<time>\d{2}:\d{2}:\d{2})((?<zone>[+-]\d{2}):\d{2})?");
            if (regex.IsMatch(input))
            {
                var match = regex.Match(input);
                timeSpan = TimeSpan.Parse(match.Groups["time"].Value);
                if (!match.Groups["zone"].Value.isNull())
                {
                    return match.Groups["zone"].ToInt();
                }
            }
            return 0;
        }
        /// <summary>
        /// 获取格式化字符串，带时分秒，格式："yyyy-MM-dd HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="isRemoveSecond">是否移除秒</param>
        public static string ToDateTimeString(this DateTime dateTime, bool isRemoveSecond = false)
        {
            if (isRemoveSecond)
                return dateTime.ToString("yyyy-MM-dd HH:mm");
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获取格式化字符串，带时分秒，格式："yyyy-MM-dd HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="isRemoveSecond">是否移除秒</param>
        public static string ToDateTimeString(this DateTime? dateTime, bool isRemoveSecond = false)
        {
            if (dateTime == null)
                return string.Empty;
            return ToDateTimeString(dateTime.Value, isRemoveSecond);
        }

        public static string ToDayTimeString(this DateTime? dateTime, bool isRemoveSecond = false)
        {
            if (dateTime == null)
                return string.Empty;
            var temp = ToDateTimeString(dateTime.Value, isRemoveSecond);
            return temp.Substring(5);

        }
        public static string ToDayTimeString(this DateTime dateTime, bool isRemoveSecond = false)
        {
            if (dateTime == null)
                return string.Empty;
            var temp = ToDateTimeString(dateTime, isRemoveSecond);
            return temp.Substring(5);

        }
        /// <summary>
        /// 获取下月几号
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="month">月数 1是下月 2是下下月 </param>
        /// <param name="day">日期</param>
        /// <returns></returns>
        public static DateTime GetNextMonth(this DateTime dateTime, int month = 1, int day = 1)
        {
            DateTime result = DateTime.Now;
            var next = dateTime.AddMonths(month);
            result = new DateTime(next.Year, next.Month, 1).AddDays(day - 1);
            return result;
        }


        /// <summary>
        /// 获取下周几的日期
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="weeks">周数 1是下周 2是下下周 </param>
        /// <param name="day">周几</param>
        /// <returns></returns>
        public static DateTime GetNextWeek(this DateTime dateTime, int weeks = 1, int day = 1)
        {
            DateTime result = DateTime.Now;

            var nowWeek = dateTime.GetDayOfWeek();
            var nextMondy = dateTime.AddDays(7 - nowWeek + 1);
            if (weeks > 1)
            {
                nextMondy = nextMondy.AddDays(7 * (weeks - 1));
            }
            result = nextMondy;
            if (day > 1)
            {
                result = result.AddDays(day - 1);
            }
            return result;
        }

        public static int GetDayOfWeek(this DateTime dt)
        {

            var dy = (int)dt.DayOfWeek;
            if (dy == 0)
                dy = 7;
            return dy;
        }
        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy-MM-dd"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy-MM-dd"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToDateString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy-MM-dd"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToDateString(this DateTime? dateTime)
        {
            if (dateTime == null)
                return string.Empty;
            return ToDateString(dateTime.Value);
        }

        /// <summary>
        /// 获取格式化字符串，不带年月日，格式："HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 获取格式化字符串，不带年月日，格式："HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToTimeString(this DateTime? dateTime)
        {
            if (dateTime == null)
                return string.Empty;
            return ToTimeString(dateTime.Value);
        }

        /// <summary>
        /// 获取格式化字符串，带毫秒，格式："yyyy-MM-dd HH:mm:ss.fff"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToMillisecondString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        /// 获取格式化字符串，带毫秒，格式："yyyy-MM-dd HH:mm:ss.fff"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToMillisecondString(this DateTime? dateTime)
        {
            if (dateTime == null)
                return string.Empty;
            return ToMillisecondString(dateTime.Value);
        }

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy年MM月dd日"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToChineseDateString(this DateTime dateTime)
        {
            return string.Format("{0}年{1}月{2}日", dateTime.Year, dateTime.Month, dateTime.Day);
        }

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy年MM月dd日"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToChineseDateString(this DateTime? dateTime)
        {
            if (dateTime == null)
                return string.Empty;
            return ToChineseDateString(dateTime.SafeValue());
        }

        /// <summary>
        /// 获取格式化字符串，带时分秒，格式："yyyy年MM月dd日 HH时mm分"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="isRemoveSecond">是否移除秒</param>
        public static string ToChineseDateTimeString(this DateTime dateTime, bool isRemoveSecond = false)
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat("{0}年{1}月{2}日", dateTime.Year, dateTime.Month, dateTime.Day);
            result.AppendFormat(" {0}时{1}分", dateTime.Hour, dateTime.Minute);
            if (isRemoveSecond == false)
                result.AppendFormat("{0}秒", dateTime.Second);
            return result.ToString();
        }

        /// <summary>
        /// 获取格式化字符串，带时分秒，格式："yyyy年MM月dd日 HH时mm分"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="isRemoveSecond">是否移除秒</param>
        public static string ToChineseDateTimeString(this DateTime? dateTime, bool isRemoveSecond = false)
        {
            if (dateTime == null)
                return string.Empty;
            return ToChineseDateTimeString(dateTime.Value);
        }



        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns> 
        public static DateTime StampToDateTime(this string timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dateTimeStart.Add(toNow);
        }

        public static DateTime StampToDateTime(this long timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dateTimeStart.AddSeconds(timeStamp / 1000);
        }
        public static DateTime StampToDateTime(this long? timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dateTimeStart.AddSeconds(timeStamp.ToBigInt() / 1000);
        }
        public static DateTime StampToDateTime(this int timeStamp)
        {

            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dateTimeStart.AddSeconds(timeStamp);
        }


        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int DateTimeToStamp(this System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 传递JAN ，FEB ，MAR  得到对应的月份  如  01  02   03
        /// </summary>
        /// <param name="monthAbbreviation"></param>
        /// <returns></returns>
        public static string GetMonthNumberStr(this string monthAbbreviation)
        {
            int yuefen = 0;
            string[] monthAbbreviations = new string[] { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };

            for (int i = 0; i < monthAbbreviations.Length; i++)
            {
                if (monthAbbreviations[i].Equals(monthAbbreviation, StringComparison.OrdinalIgnoreCase))
                {
                    yuefen = i + 1;
                }
            }
            return yuefen.ToString("D2");
        }

        /// <summary>
        /// 判断2个时间是否超过一年
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static bool IsMoreThanAYearApart(DateTime date1, DateTime date2)
        {
            int totalDaysDiff = (date2 - date1).Days;

            if (totalDaysDiff > 365)
            {
                return true;
            }

            return false;


            // 计算两个日期之间的总天数差值


        }
    }
}
