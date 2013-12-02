using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Avalon.UCenter
{
    public static class UserUtil
    {
        static Regex ipMatchRegex = new Regex(@"(((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))\.){3}((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))", RegexOptions.Compiled);
        /// <summary>
        /// 默认使用gb2312进行编码
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Md5(string content)
        {
            return Md5(content, "gb2312");
        }

        public static string Md5(string content, Encoding encoding)
        {
            byte[] buffer = MD5.Create().ComputeHash(encoding.GetBytes(content));
            return StringUtil.ToHex(buffer);
        }

        public static string Md5(string content, string encode)
        {
            Encoding encoding = Encoding.GetEncoding(encode);
            return Md5(content, encoding);
        }

        public static string Md5Strings(params string[] values)
        {
            var content = String.Concat(values);
            return Md5(content);
        }

        public static string Encrypt_PHP(string source, string key)
        {
            string str = Md5("128");
            int startIndex = 0;

            byte[] b_source = System.Text.Encoding.GetEncoding("gb2312").GetBytes(source);

            byte[] b_destiation = new byte[b_source.Length * 2];

            for (int i = 0; i < b_source.Length; i++)
            {
                if (startIndex == str.Length)
                {
                    startIndex = 0;
                }

                b_destiation[i * 2] = System.Text.Encoding.GetEncoding("gb2312").GetBytes(str.Substring(startIndex, 1))[0];

                b_destiation[i * 2 + 1] = (byte)(b_source[i] ^ b_destiation[i * 2]);

                startIndex++;

            }

            return EncodingToBase64(Keyed(b_destiation, key));
        }

        /// <summary>
        /// 将字节数组转化B64字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string EncodingToBase64(byte[] str)
        {
            return System.Convert.ToBase64String(str);
        }

        private static byte[] Keyed(byte[] source, string key)
        {
            string str = Md5(key, "gb2312");
            int startIndex = 0;

            for (int i = 0; i < source.Length; i++)
            {
                if (str.Length == startIndex)
                {
                    startIndex = 0;
                }

                int num = source[i] ^ Encoding.GetEncoding("gb2312").GetBytes(str.Substring(startIndex, 1))[0];

                source[i] = (byte)num;

                startIndex++;
            }

            return source;
        }
       
        /// <summary>
        /// 获取IP的Int格式
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns>输入IP的整型格式</returns>
        public static int GetIpInt(string ip)
        {
            try
            {
                if (!CheckIPAddress(ip)) return -1;

                if (ip.Split('.').Length == 3)
                    ip = ip + ".0";
                string[] strArray = ip.Split('.');
                long num2 = ((long.Parse(strArray[0]) * 0x100L) * 0x100L) * 0x100L;
                long num3 = (long.Parse(strArray[1]) * 0x100L) * 0x100L;
                long num4 = long.Parse(strArray[2]) * 0x100L;
                long num5 = long.Parse(strArray[3]);
                return (int)(((num2 + num3) + num4) + num5);
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 检查传入的数据是否为IP
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns>检查结果</returns>
        private static bool CheckIPAddress(string ip)
        {
            return ipMatchRegex.IsMatch(ip);
        }

        public static int GetProductCode(long platCode)
        {
            var pc = platCode.ToString();
            if (pc.Length == 12)
                return Int32.Parse(pc.Substring(0, 4));
            return 0;
        }

        public static int GetTerminalCode(long platCode)
        {
            var pc = platCode.ToString();
            if (pc.Length == 12)
                return Int32.Parse(pc.Substring(4, 4));
            return 0;
        }

        public static int GetOriginCode(long platCode)
        {
            var pc = platCode.ToString();
            if (pc.Length == 12)
                return Int32.Parse(pc.Substring(8, 4));
            return 0;
        }
    }
}
