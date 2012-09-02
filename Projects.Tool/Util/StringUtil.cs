using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool.Util
{
    public class StringUtil
    {
        static Random random;
        const string numStr = "0123456789";
        const string upperStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowerStr = "abcdefghijklmnopqrstuvwxyz";
        const string markStr = @"`-=[];'\,./~!@#$%^&*()_+{}:""|<>?";

        static StringUtil()
        {
            random = new Random(Environment.TickCount);
        }

        public static string CreateRandomString(int length)
        {
            return CreateRandomString(length, true, true, true, false);
        }

        public static string CreateRandomString(int length, bool useNumChar, bool useLowerChar, bool useUpperChar, bool useMarkChar)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException("length");
            if (!useNumChar && !useLowerChar && !useUpperChar && !useMarkChar)
                throw new ArgumentException("至少需要指定一种字符类型");

            StringBuilder frameString = new StringBuilder();
            if (useNumChar)
                frameString.Append(numStr);
            if (useLowerChar)
                frameString.Append(lowerStr);
            if (useUpperChar)
                frameString.Append(upperStr);
            if (useMarkChar)
                frameString.Append(markStr);

            StringBuilder sb = new StringBuilder();
            int len = frameString.Length;
            for (int i = 0; i < length; i++)
            {
                sb.Append(frameString[random.Next(len)]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// MD5的加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Md5WithSalt(string str)
        {
            return Tools.StringUtil.MD5(str + "_509527").ToUpper();
        }

        public static DateTime GMTToLocal(string gmt)
        {
            DateTime dt = DateTime.MinValue;
            try
            {
                string pattern = "";
                if (gmt.IndexOf("+0", System.StringComparison.Ordinal) != -1)
                {
                    gmt = gmt.Replace("GMT", "");
                    pattern = "ddd MMM dd yyyy HH':'mm':'ss zzz";
                }
                if (gmt.ToUpper().IndexOf("GMT", System.StringComparison.Ordinal) != -1)
                {
                    pattern = "ddd MMM dd yyyy HH':'mm':'ss 'GMT'";
                }
                if (pattern != "")
                {
                    dt = DateTime.ParseExact(gmt, pattern, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal);
                    dt = dt.ToLocalTime();
                }
                else
                {
                    dt = Convert.ToDateTime(gmt);
                }
                return dt;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }  
    }
}
