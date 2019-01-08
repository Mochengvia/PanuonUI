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
            if (string.IsNullOrEmpty(context))
                return 0;

            var result = 0;
            if (int.TryParse(context, out result))
                return result;
            else
                return 0;
        }

        /// <summary>
        /// 尝试将字符串转换为小数，若转换失败，则返回0。
        /// </summary>
        public static double ToDouble(this string context)
        {
            if (string.IsNullOrEmpty(context))
                return 0;

            var result = 0.0;
            if (double.TryParse(context, out result))
                return result;
            else
                return 0;
        }

        /// <summary>
        /// 尝试将字符串转换为布尔值，若转换失败，则返回False。
        /// </summary>
        public static bool ToBoolean(this string context)
        {
            if (string.IsNullOrEmpty(context))
                return false;

            var result = false;
            if (bool.TryParse(context, out result))
                return result;
            else
                return false;
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
        public static string ToString(this IList list, string spliter)
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
        public static IEnumerable<KeyValuePair<TKey, TValue>> GetValues<TKey, TValue>(this IDictionary<TKey, TValue> dic, IEnumerable<TKey> keys)
        {
            foreach (var key in keys)
            {
                yield return new KeyValuePair<TKey, TValue>(key, dic[key]);
            }
        }
        #endregion

        #region Others
        /// <summary>
        /// 比较两个对象的可写属性（具有非Private的Set访问器）是否完全相等。
        /// <para>对于无需比较的属性或集合型属性，您必须为其加上SkipCompare特性标签。集合型属性在比较时会因异常而返回False。</para>
        /// <para>若两个对象均为Null，则返回True；若只有一个为Null，则返回False。</para>
        /// </summary>
        /// <param name="value">要比较的另一个值。</param>
        public static bool IsEqual<T>(this T obj, T value)
        {
            try
            {
                if (obj == null && value == null)
                    return true;
                else if (obj == null || value == null)
                    return false;

                Type type = obj.GetType();
                if (type.IsValueType || type.FullName == typeof(string).FullName)
                    return obj.Equals(value);

                foreach (var propertyInfo in type.GetProperties())
                {
                    if (propertyInfo.CanWrite)
                    {
                        if (Attribute.IsDefined(propertyInfo, typeof(SkipCompareAttribute)))
                        {
                            continue;
                        }
                        var propType = propertyInfo.PropertyType;
                        var prop1 = propertyInfo.GetValue(obj, null);
                        var prop2 = propertyInfo.GetValue(value, null);

                        if (prop1 == null && prop2 == null)
                            continue;
                        else if (prop1 == null || prop2 == null)
                            return false;
                        else if (!propType.IsValueType && propType.FullName != typeof(string).FullName)
                        {
                            if (!prop1.IsEqual(prop2))
                            {
                                return false;
                            }
                            continue;
                        }
                        else if (!prop1.Equals(prop2))
                            return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Function
        #endregion
    }
}
