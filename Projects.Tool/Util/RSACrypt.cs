using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Projects.Tool.Util
{
    /// <summary>
    /// 
    /// </summary>
    public class RSACrypt
    {
        /// <summary>
        /// 生成公私钥
        /// </summary>
        /// <returns></returns>
        public static Tuple<string,string> GenerateKey()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(512);
            string publicKey = rsa.ToXmlString(false);
            string privateKey = rsa.ToXmlString(true);
            return new Tuple<string, string>(publicKey, privateKey);
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        public static string SignData(string data, string privateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);
            RSAPKCS1SignatureFormatter format = new RSAPKCS1SignatureFormatter(rsa);
            format.SetHashAlgorithm("SHA1");
            byte[] source = UTF8Encoding.ASCII.GetBytes(data);
            SHA1Managed sha = new SHA1Managed();
            byte[] result = sha.ComputeHash(source);
            byte[] b = format.CreateSignature(result);

            return Convert.ToBase64String(b);
        }
        
        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="signedData">签名后的数据</param>
        /// <param name="data">原始数据</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public static bool ValidateSignData(string signedData, string data, string publicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);
            RSAPKCS1SignatureDeformatter deformat = new RSAPKCS1SignatureDeformatter(rsa);
            deformat.SetHashAlgorithm("SHA1");
            byte[] key = Convert.FromBase64String(signedData);
            SHA1Managed sha = new SHA1Managed();
            byte[] orginData = sha.ComputeHash(UTF8Encoding.ASCII.GetBytes(data));
            return deformat.VerifySignature(orginData, key);
        }

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public static string RSAEncrypt(string data, string publicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] crpherbytes;
            rsa.FromXmlString(publicKey);
            crpherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(data), false);
            return Convert.ToBase64String(crpherbytes);
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="encryptData">加密后的数据</param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        public static string RSADecrypt(string encryptData, string privateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] crpherbytes;
            rsa.FromXmlString(privateKey);
            crpherbytes = rsa.Decrypt(Convert.FromBase64String(encryptData), false);
            return Encoding.UTF8.GetString(crpherbytes);
        }
    }
}
