/*==============================================================
*作者：ZEOUN
*时间：2018/10/10 15:20:15
*说明： 
*日志：2018/10/10 15:20:15 创建。
*==============================================================*/
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Panuon.UI.Utils
{
    public static class Extends
    {
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

        /// <summary>
        /// 将数字转换为中文大写文字。
        /// </summary>
        public static string ToChineseNumber(this int number)
        {
            var result = "";
            var str = "零一二三四五六七八九十";
            var convert = number.ToString();
            for(int i = 0; i < convert.Length; i++)
            {
                result += str[Int32.Parse(convert[i].ToString())];
            }
            return result;
        }

        /// <summary>
        /// 将时间转换成时间戳（精确到毫秒）。
        /// </summary>
        public static long ToTimeStamp(this DateTime date)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }

        /// <summary>
        /// 将时间戳转换成时间（精确到毫秒）。
        /// </summary>
        public static DateTime ToDate(this long timeStamp)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddMilliseconds((long)timeStamp);
        }
    }
}
