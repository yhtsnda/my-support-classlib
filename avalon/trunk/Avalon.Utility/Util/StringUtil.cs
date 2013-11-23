using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
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
