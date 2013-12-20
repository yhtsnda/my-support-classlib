using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Avalon.Utility
{
    public static class Base34Encoding
    {
        private const string Digits = "123456789abcdefghijkmnopqrstuvwxyz";

        public static string Encode(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            // Decode byte[] to BigInteger
            BigInteger intData = 0;
            for (int i = 0; i < data.Length; i++)
            {
                intData = intData * 256 + data[i];
            }

            // Encode BigInteger to Base34 string
            string result = "";
            while (intData > 0)
            {
                int remainder = (int)(intData % 34);
                intData /= 34;
                result = Digits[remainder] + result;
            }

            // Append `1` for each leading 0 byte
            for (int i = 0; i < data.Length && data[i] == 0; i++)
            {
                result = '1' + result;
            }
            return result;
        }

        public static string EncodeGuid(Guid guid)
        {
            return Encode(guid.ToByteArray());
        }

        public static string CreateEncodeGuid()
        {
            return EncodeGuid(Guid.NewGuid());
        }

        public static byte[] Decode(string s)
        {
            if (s == null)
                throw new ArgumentNullException("s");

            // Decode Base34 string to BigInteger 
            BigInteger intData = 0;
            for (int i = 0; i < s.Length; i++)
            {
                int digit = Digits.IndexOf(s[i]); //Slow
                if (digit < 0)
                    throw new FormatException(string.Format("Invalid Base34 character `{0}` at position {1}", s[i], i));
                intData = intData * 34 + digit;
            }

            // Encode BigInteger to byte[]
            // Leading zero bytes get encoded as leading `1` characters
            int leadingZeroCount = s.TakeWhile(c => c == '1').Count();
            var leadingZeros = Enumerable.Repeat((byte)0, leadingZeroCount);
            var bytesWithoutLeadingZeros =
                intData.ToByteArray()
                .Reverse()// to big endian
                .SkipWhile(b => b == 0);//strip sign byte
            var result = leadingZeros.Concat(bytesWithoutLeadingZeros).ToArray();
            return result;
        }

        public static Guid DecodeGuid(string s)
        {
            return new Guid(Decode(s));
        }
    }
}
