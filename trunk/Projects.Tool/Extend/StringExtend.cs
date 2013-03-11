using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Linq
{
    /// <summary>
    /// 字符扩展
    /// </summary>
    public static class StringExtend
    {
        /// <summary>
        /// 指示指定的字符串是 null、空还是仅由空白字符组成。
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return String.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 指示指定的字符串是 null 还是 System.String.Empty 字符串。
        /// </summary>
        public static bool IsNullOrEmpty(this string value)
        {
            return String.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 将指定字符串中的格式项替换为指定数组中相应对象的字符串表示形式。
        /// </summary>
        public static string Format(this string value, params object[] args)
        {
            if (String.IsNullOrEmpty(value))
                return value;
            return String.Format(value, args);
        }

        /// <summary>
        /// 将给定的字符串转为 int。
        /// </summary>
        public static int ToInt32(this string value)
        {
            return Int32.Parse(value);
        }

        /// <summary>
        /// 对字符串进行汉字以2字节计算
        /// </summary>
        public static string SubBytesString(this string value, int length)
        {
            if (Encoding.Default.GetByteCount(value) <= length)
                return value;

            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = String.Empty;
            byte[] s = ascii.GetBytes(value);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;

                if (tempLen > length) break;
                tempString += value.Substring(i, 1);

            }

            if (Encoding.Default.GetByteCount(value) > length)
                tempString += "...";

            return tempString;
        }

        /// <summary>
        /// 对给定的字符串进行指定长度的省略处理。
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="length">The length.</param>
        /// <param name="ellipsis">是否显示省略号。</param>
        /// <returns></returns>
        static string Overflow(this string value, int length, bool ellipsis)
        {
            return null;
        }
    }
}
