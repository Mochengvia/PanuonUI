using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Panuon.UI.Utils
{
    public static class Extends
    {

        #region Interger
        /// <summary>
        /// 将0~10的数字转换为文字。
        /// </summary>
        public static char ToSingleChineseNumber(this int number)
        {
            var array = "零一二三四五六七八九十";
            return array[number];
        }

        public static string ToSizeString(this int byteSize)
        {
            if (byteSize > 1024 * 1024 * 1024)
            {
                return (byteSize * 1.0 / 1024 / 1024 / 1024).ToString("f2") + "GB";
            }
            else if (byteSize > 1024 * 1024)
            {
                return (byteSize * 1.0 / 1024 / 1024).ToString("f2") + "MB";
            }
            else if (byteSize > 1024)
            {
                return (byteSize * 1.0 / 1024).ToString("f2") + "KB";
            }
            else
            {
                return byteSize + "B";
            }
        }
        #endregion

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
            return date1.Date == date2.Date;
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

        public static string ToSizeString(this long byteSize)
        {
            if (byteSize > 1024 * 1024 * 1024)
            {
                return (byteSize * 1.0 / 1024 / 1024 / 1024).ToString("f2") + "GB";
            }
            else if (byteSize > 1024 * 1024)
            {
                return (byteSize * 1.0 / 1024 / 1024).ToString("f2") + "MB";
            }
            else if (byteSize > 1024)
            {
                return (byteSize * 1.0 / 1024).ToString("f2") + "KB";
            }
            else
            {
                return byteSize + "B";
            }
        }
        #endregion

        #region List
        /// <summary>
        /// 将列表中的每个元素拼接成一段字符串。
        /// </summary>
        /// <param name="spliter">分隔符。</param>
        public static string ToString(this IList list,string spliter)
        {
            return String.Join(spliter, list);
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IList<T> list)
        {
            return new ObservableCollection<T>(list);
        }

        #endregion

        #region Dictionary
        /// <summary>
        /// 将字典中指定键的键值对返回到一个枚举集合中。
        /// </summary>
        /// <typeparam name="TKey">唯一键。</typeparam>
        /// <typeparam name="TValue">数据值。</typeparam>
        /// <param name="keys">要查找的键集合。</param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<TKey,TValue>> GetValues<TKey,TValue>(this IDictionary<TKey,TValue> dic, IEnumerable<TKey> keys)
        {
            foreach(var key in keys)
            {
                yield return new KeyValuePair<TKey, TValue>(key, dic[key]);
            }
        }
        #endregion

        #region Others
        #endregion

        #region Function
        private static bool IsCanCompare(Type t)
        {
            if (t.IsValueType)
                return true;
            else
            {
                if (t.FullName == typeof(String).FullName)
                {
                    return true;
                }
                return false;
            }
        }
        #endregion


    }
}
