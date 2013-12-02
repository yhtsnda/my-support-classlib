using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuthClient
{
    internal class AccessGrantSerializer
    {
        public static AccessGrant Deserialize(string data)
        {
            var buffer = Convert.FromBase64String(data);
            return new AccessGrant()
            {
                AccessToken = ToHexString(buffer, 0, 16),
                RefreshToken = ToHexString(buffer, 16, 16),
                CreateTime = DateTimeExtend.FromUnixTime(BitConverter.ToInt32(buffer, 32)),
                ExpireTime = DateTimeExtend.FromUnixTime(BitConverter.ToInt32(buffer, 36)),
                ClientId = BitConverter.ToInt32(buffer, 40),
                UserId = BitConverter.ToInt64(buffer, 44)
            };
        }

        public static string Serialize(AccessGrant accessGrant)
        {
            var buffer = new byte[52];

            AppendBytes(buffer, 0, accessGrant.AccessToken);
            AppendBytes(buffer, 16, accessGrant.RefreshToken);
            AppendBytes(buffer, 32, accessGrant.CreateTime);
            AppendBytes(buffer, 36, accessGrant.ExpireTime);
            AppendBytes(buffer, 40, accessGrant.ClientId);
            AppendBytes(buffer, 44, BitConverter.GetBytes(accessGrant.UserId));

            return Convert.ToBase64String(buffer);
        }

        static void AppendBytes(byte[] buffer, int index, byte[] data)
        {
            Array.Copy(data, 0, buffer, index, data.Length);
        }

        static void AppendBytes(byte[] buffer, int index, string hexString)
        {
            var data = StringUtil.HexToBytes(hexString);
            AppendBytes(buffer, index, data);
        }

        static void AppendBytes(byte[] buffer, int index, DateTime date)
        {
            var data = (int)date.ToUnixTime();
            AppendBytes(buffer, index, data);
        }

        static void AppendBytes(byte[] buffer, int index, int value)
        {
            var data = BitConverter.GetBytes(value);
            AppendBytes(buffer, index, data);
        }

        static string ToHexString(byte[] buffer, int index, int length)
        {
            if (buffer.Length < index + length)
                throw new ArgumentOutOfRangeException();

            byte[] output = new byte[length];
            Array.Copy(buffer, index, output, 0, length);

            return StringUtil.ToHex(output);
        }
    }
}
