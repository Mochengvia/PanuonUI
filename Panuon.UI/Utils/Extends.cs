using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Panuon.UI.Utils
{
    public static class Extends
    {
        #region String
        /// <summary>
        /// 尝试将字符串转换为整数，若转换失败，则返回0。
        /// </summary>
        public static int ToInt(this string context)
        {
            var result = 0;
            if (Int32.TryParse(context, out result))
                return result;
            else
                return 0;
        }

        /// <summary>
        /// 尝试将字符串转换为小数，若转换失败，则返回0。
        /// </summary>
        public static double ToDouble(this string context)
        {
            var result = 0.0;
            if (Double.TryParse(context, out result))
                return result;
            else
                return 0;
        }
        #endregion

        #region DateTime
        /// <summary>
        /// 将时间转换成时间戳（精确到毫秒）。
        /// </summary>
        public static long ToTimeStamp(this DateTime date)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }

        /// <summary>
        /// 与另一个日期的年月日进行比较，若相同，则返回True。
        /// </summary>
        public static bool CompareYearMonthDay(this DateTime date1, DateTime date2)
        {
            return date1.Year == date2.Year && date1.Month == date2.Month && date1.Day == date2.Day;
        }

        #endregion

        #region Long
        /// <summary>
        /// 将时间戳转换成时间（精确到毫秒）。
        /// </summary>
        public static DateTime ToDate(this long timeStamp)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddMilliseconds((long)timeStamp);
        }
        #endregion

        #region List
        /// <summary>
        /// 将列表中的每个元素拼接成一段字符串。
        /// </summary>
        /// <param name="spliter">分隔符。</param>
        public static string ToString<T>(this IList<T> list,string spliter)
        {
            return String.Join(spliter, list);
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IList<T> list)
        {
            return new ObservableCollection<T>(list);
        }
        #endregion
    }
}
