using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Projects.Tool.Util
{
    /// <summary>
    /// 自定义Cookie加密解密
    /// </summary>
    public static class CookieAuthentication
    {
        private static readonly string AES_PASSWORD = Md5.Encrypt("CookieAuthenticationPassword");
        private static readonly string AES_KEY = Md5.Encrypt("CookieAuthenticationKey");

        /// <summary>
        /// 
        /// </summary>
        static CookieAuthentication()
        {
            if (!string.IsNullOrEmpty(FormsAuthentication.FormsCookieName))
                AES_PASSWORD = Md5.Encrypt(FormsAuthentication.FormsCookieName);
            if (!string.IsNullOrEmpty(FormsAuthentication.CookieDomain))
                AES_KEY = Md5.Encrypt(FormsAuthentication.CookieDomain);
        }

        /// <summary>
        /// 创建一个字符串，其中包含适用于 HTTP Cookie 的加密的 Forms 身份验证票证。
        /// </summary>
        /// <param name="ticket">用于创建加密的 Forms 身份验证票证的 System.Web.Security.FormsAuthenticationTicket 对象。</param>
        /// <returns>一个字符串，其中包含加密的 Forms 身份验证票证。</returns>
        public static string Encrypt(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket");

            try
            {
                var bytes = Serializer.Serialize(ticket);
                var data = AES.Encrypt(bytes, AES_PASSWORD, AES_KEY);
                return HttpServerUtility.UrlTokenEncode(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 创建一个 System.Web.Security.FormsAuthenticationTicket 对象，此对象将根据传递给该方法的加密的 Forms 身份验证票证而定。
        /// </summary>
        /// <param name="encryptedTicket">加密的身份验证票。</param>
        /// <returns>一个 System.Web.Security.FormsAuthenticationTicket 对象。如果 encryptedTicket 参数不是有效票证，则返回 null</returns>
        public static FormsAuthenticationTicket Decrypt(string encryptedTicket)
        {
            if (string.IsNullOrEmpty(encryptedTicket))
                throw new ArgumentNullException("encryptedTicket");

            try
            {
                var bytes = HttpServerUtility.UrlTokenDecode(encryptedTicket);
                var data = AES.Decrypt(bytes, AES_PASSWORD, AES_KEY);
                return Serializer.Deserialize(data, data.Length);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public static CookieCollection GetAllCookiesFromHeader(string header, string host)
        {
            var item = new List<string>();
            CookieCollection cc = new CookieCollection();
            if (header != string.Empty)
            {
                item = ConvertCookieHeaderToArrayList(header);
                cc = ConvertCookieArraysToCookieCollection(item, host);
            }
            return cc;
        }


        private static List<string> ConvertCookieHeaderToArrayList(string header)
        {
            header = header.Replace("\r", "");
            header = header.Replace("\n", "");
            var headers = header.Split(',');
            var result = new List<string>();
            var index = 0;
            while (index < headers.Length)
            {
                if (headers[index].IndexOf("expires=", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    result.Add(string.Concat(headers[index], ",", headers[index + 1]));
                    index = index + 1;
                }
                else
                {
                    result.Add(headers[index]);
                }
                index = index + 1;
            }
            return result;
        }


        private static CookieCollection ConvertCookieArraysToCookieCollection(IEnumerable<string> items, string host)
        {
            var cc = new CookieCollection();
            foreach (var item in items)
            {
                var parts = item.Split(';');
                var c = string.Empty;
                var p = string.Empty;
                var d = string.Empty;
                var nv = (string[])null;
                var cookie = new Cookie();

                for (int j = 0; j < parts.Length; j++)
                {
                    if (j == 0)
                    {
                        c = parts[j];
                        if (c != string.Empty)
                        {
                            var firstEqual = c.IndexOf("=");
                            var firstName = c.Substring(0, firstEqual);
                            var allValue = c.Substring(firstEqual + 1, c.Length - (firstEqual + 1));
                            cookie.Name = firstName;
                            cookie.Value = allValue;
                        }
                        continue;
                    }
                    if (parts[j].IndexOf("path", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        p = parts[j];
                        if (p != string.Empty)
                        {
                            nv = p.Split('=');
                            if (nv[1] != string.Empty)
                                cookie.Path = nv[1];
                            else
                                cookie.Path = "/";
                        }
                        continue;
                    }

                    if (parts[j].IndexOf("domain", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        p = parts[j];
                        if (p != string.Empty)
                        {
                            nv = p.Split('=');

                            if (nv[1] != string.Empty)
                                cookie.Domain = nv[1];
                            else
                                cookie.Domain = host;
                        }
                        continue;
                    }
                }

                if (cookie.Path == string.Empty)
                    cookie.Path = "/";
                if (cookie.Domain == string.Empty)
                    cookie.Domain = host;
                cc.Add(cookie);
            }
            return cc;
        }

        private static class AES
        {
            /// <summary>
            /// AES加密
            /// </summary>
            /// <param name="data">加密前的字符串</param>
            /// <param name="password">加密密码值</param>
            /// <param name="key">加密密钥</param>
            /// <returns>加密后的字符串</returns>
            public static byte[] Encrypt(byte[] bytes, string password, string key)
            {
                var salt = UTF8Encoding.UTF8.GetBytes(key);
                //AesManaged - 高级加密标准(AES) 对称算法的管理类
                var aes = new AesManaged();
                //Rfc2898DeriveBytes - 通过使用基于 HMACSHA1 的伪随机数生成器，实现基于密码的密钥派生功能 (PBKDF2 - 一种基于密码的密钥派生函数)
                //通过 密码 和 salt 派生密钥
                var rfc = new Rfc2898DeriveBytes(password, salt);
                /* 
                             * AesManaged.BlockSize - 加密操作的块大小（单位：bit）
                             * AesManaged.LegalBlockSizes - 对称算法支持的块大小（单位：bit）
                             * AesManaged.KeySize - 对称算法的密钥大小（单位：bit）
                             * AesManaged.LegalKeySizes - 对称算法支持的密钥大小（单位：bit）
                             * AesManaged.Key - 对称算法的密钥
                             * AesManaged.IV - 对称算法的密钥大小
                             * Rfc2898DeriveBytes.GetBytes(int 需要生成的伪随机密钥字节数) - 生成密钥
                            */
                aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
                aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                aes.Key = rfc.GetBytes(aes.KeySize / 8);
                aes.IV = rfc.GetBytes(aes.BlockSize / 8);
                //用当前的 Key 属性和初始化向量 IV 创建对称加密器对象
                var encryptTransform = aes.CreateEncryptor();
                //加密后的输出流
                var encryptStream = new MemoryStream();
                //将加密后的目标流（encryptStream）与加密转换（encryptTransform）相连接
                var encryptor = new CryptoStream(encryptStream, encryptTransform, CryptoStreamMode.Write);
                //将一个字节序列写入当前 CryptoStream （完成加密的过程）
                encryptor.Write(bytes, 0, bytes.Length);
                encryptor.Close();
                // 将加密后所得到的流转换成字节数组，再用Base64编码将其转换为字符串
                return encryptStream.ToArray();
            }

            /// <summary>
            /// AES解密
            /// </summary>
            /// <param name="value">加密前的字符串</param>
            /// <param name="password">加密密码值</param>
            /// <param name="key">加密密钥</param>
            /// <returns>加密前的字符串</returns>
            public static byte[] Decrypt(byte[] bytes, string password, string key)
            {
                var salt = Encoding.UTF8.GetBytes(key);
                var aes = new AesManaged();
                var rfc = new Rfc2898DeriveBytes(password, salt);
                aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
                aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                aes.Key = rfc.GetBytes(aes.KeySize / 8);
                aes.IV = rfc.GetBytes(aes.BlockSize / 8);
                //用当前的 Key 属性和初始化向量 IV 创建对称解密器对象
                var decryptTransform = aes.CreateDecryptor();
                //解密后的输出流
                var decryptStream = new MemoryStream();
                //将解密后的目标流（decryptStream）与解密转换（decryptTransform）相连接
                var decryptor = new CryptoStream(decryptStream, decryptTransform, CryptoStreamMode.Write);
                //将一个字节序列写入当前 CryptoStream （完成解密的过程）
                decryptor.Write(bytes, 0, bytes.Length);
                decryptor.Close();
                //将解密后所得到的流转换为字符串
                return decryptStream.ToArray();
            }
        }

        private static class Serializer
        {
            // Fields
            private const byte CURRENT_TICKET_SERIALIZED_VERSION = 1;

            // Methods
            public static FormsAuthenticationTicket Deserialize(byte[] serializedTicket, int serializedTicketLength)
            {
                FormsAuthenticationTicket ticket;
                try
                {
                    using (MemoryStream stream = new MemoryStream(serializedTicket))
                    {
                        using (SerializingBinaryReader reader = new SerializingBinaryReader(stream))
                        {
                            int num2;
                            DateTime time;
                            DateTime time2;
                            bool flag;
                            string str;
                            if (reader.ReadByte() == 1)
                            {
                                num2 = reader.ReadByte();
                                long ticks = reader.ReadInt64();
                                time = new DateTime(ticks, DateTimeKind.Utc);
                                time.ToLocalTime();
                                if (reader.ReadByte() != 0xfe)
                                {
                                    return null;
                                }
                                long num5 = reader.ReadInt64();
                                time2 = new DateTime(num5, DateTimeKind.Utc);
                                time2.ToLocalTime();
                                switch (reader.ReadByte())
                                {
                                    case 0:
                                        flag = false;
                                        goto Label_00A1;

                                    case 1:
                                        flag = true;
                                        goto Label_00A1;
                                }
                            }
                            return null;
                        Label_00A1:
                            str = reader.ReadBinaryString();
                            string userData = reader.ReadBinaryString();
                            string cookiePath = reader.ReadBinaryString();
                            if (reader.ReadByte() != 0xff)
                            {
                                return null;
                            }
                            if (stream.Position != serializedTicketLength)
                            {
                                return null;
                            }
                            ticket = new FormsAuthenticationTicket(num2, str, time.ToLocalTime(), time2.ToLocalTime(), flag, userData, cookiePath);

                        }
                    }
                }
                catch
                {
                    ticket = null;
                }
                return ticket;
            }

            public static byte[] Serialize(FormsAuthenticationTicket ticket)
            {
                byte[] buffer;
                using (MemoryStream stream = new MemoryStream())
                {
                    using (SerializingBinaryWriter writer = new SerializingBinaryWriter(stream))
                    {
                        writer.Write((byte)1);
                        writer.Write((byte)ticket.Version);
                        writer.Write(ticket.IssueDate.ToUniversalTime().Ticks);
                        writer.Write((byte)0xfe);
                        writer.Write(ticket.Expiration.ToUniversalTime().Ticks);
                        writer.Write(ticket.IsPersistent);
                        writer.WriteBinaryString(ticket.Name);
                        writer.WriteBinaryString(ticket.UserData);
                        writer.WriteBinaryString(ticket.CookiePath);
                        writer.Write((byte)0xff);
                        buffer = stream.ToArray();
                    }
                }
                return buffer;
            }

            // Nested Types
            private sealed class SerializingBinaryReader : BinaryReader
            {
                // Methods
                public SerializingBinaryReader(Stream input)
                    : base(input)
                {
                }

                public string ReadBinaryString()
                {
                    int num = base.Read7BitEncodedInt();
                    byte[] buffer = this.ReadBytes(num * 2);
                    char[] chArray = new char[num];
                    for (int i = 0; i < chArray.Length; i++)
                    {
                        chArray[i] = (char)(buffer[2 * i] | (buffer[(2 * i) + 1] << 8));
                    }
                    return new string(chArray);
                }

                public override string ReadString()
                {
                    throw new NotImplementedException();
                }
            }

            private sealed class SerializingBinaryWriter : BinaryWriter
            {
                // Methods
                public SerializingBinaryWriter(Stream output)
                    : base(output)
                {
                }

                public override void Write(string value)
                {
                    throw new NotImplementedException();
                }

                public void WriteBinaryString(string value)
                {
                    byte[] buffer = new byte[value.Length * 2];
                    for (int i = 0; i < value.Length; i++)
                    {
                        char ch = value[i];
                        buffer[2 * i] = (byte)ch;
                        buffer[(2 * i) + 1] = (byte)(ch >> 8);
                    }
                    base.Write7BitEncodedInt(value.Length);
                    this.Write(buffer);
                }
            }
        }

        private static class Md5
        {
            public static string Encrypt(string value)
            {
                HashAlgorithm algorithm = new MD5Cng();
                using (algorithm)
                {
                    return BinaryToHex(algorithm.ComputeHash(Encoding.UTF8.GetBytes(value)));
                }
            }

            private static string BinaryToHex(byte[] data)
            {
                if (data == null)
                {
                    return null;
                }
                char[] chArray = new char[data.Length * 2];
                for (int i = 0; i < data.Length; i++)
                {
                    byte num2 = data[i];
                    chArray[2 * i] = NibbleToHex((byte)(num2 >> 4));
                    chArray[(2 * i) + 1] = NibbleToHex((byte)(num2 & 15));
                }
                return new string(chArray);
            }

            private static char NibbleToHex(byte nibble)
            {
                return ((nibble < 10) ? ((char)(nibble + 0x30)) : ((char)((nibble - 10) + 0x41)));
            }
        }
    }
}
