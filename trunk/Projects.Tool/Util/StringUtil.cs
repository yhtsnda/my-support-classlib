using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
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
        public static string GetUniqueIdentifier(int length)
        {
            int maxSize = length;
            char[] chars = new char[62];
            string a;
            a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }
            // Unique identifiers cannot begin with 0-9
            if (result[0] >= '0' && result[0] <= '9')
            {
                return GetUniqueIdentifier(length);
            }
            return result.ToString();
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

        public static string ToHex(byte[] bytes)
        {
            char[] c = new char[bytes.Length * 2];

            byte b;

            for (int bx = 0, cx = 0; bx < bytes.Length; ++bx, ++cx)
            {
                b = ((byte)(bytes[bx] >> 4));
                c[cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);

                b = ((byte)(bytes[bx] & 0x0F));
                c[++cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);
            }

            return new string(c);
        }

        public static byte[] HexToBytes(string str)
        {
            if (str.Length == 0 || str.Length % 2 != 0)
                return new byte[0];

            byte[] buffer = new byte[str.Length / 2];
            char c;
            for (int bx = 0, sx = 0; bx < buffer.Length; ++bx, ++sx)
            {
                // Convert first half of byte
                c = str[sx];
                buffer[bx] = (byte)((c > '9' ? (c > 'Z' ? (c - 'a' + 10) : (c - 'A' + 10)) : (c - '0')) << 4);

                // Convert second half of byte
                c = str[++sx];
                buffer[bx] |= (byte)(c > '9' ? (c > 'Z' ? (c - 'a' + 10) : (c - 'A' + 10)) : (c - '0'));
            }

            return buffer;
        }
    }
}
