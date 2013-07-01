using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using Projects.Tool.Util;

namespace Projects.UserCenter
{
    public static class UserCenterUtility
    {
        /// <summary>
        /// 使用MD5获取的加密后的密码
        /// </summary>
        /// <param name="password">原始密码</param>
        /// <returns></returns>
        public static string EncryptPassword(string password)
        {
            string appkey = "fdjf,jkgfkl"; //加一特殊的字符后再加密，这样更安全些

            MD5 MD5 = new MD5CryptoServiceProvider();
            byte[] a = Encoding.Default.GetBytes(appkey);
            byte[] datSource = Encoding.Default.GetBytes(password);
            byte[] b = new byte[a.Length + 4 + datSource.Length];

            int i;
            for (i = 0; i < datSource.Length; i++)
            {
                b[i] = datSource[i];
            }

            b[i++] = 163;
            b[i++] = 172;
            b[i++] = 161;
            b[i++] = 163;

            for (int k = 0; k < a.Length; k++)
            {
                b[i] = a[k];
                i++;
            }

            byte[] newSource = MD5.ComputeHash(b);
            return StringUtil.ToHex(newSource);
        }
    }
}
