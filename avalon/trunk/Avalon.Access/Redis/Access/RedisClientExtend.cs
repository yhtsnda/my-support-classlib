using ServiceStack.Common;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Avalon.Access
{
    public static class RedisClientExtend
    {
        static Func<RedisNativeClient, byte[][], int> sendExpectIntFunc;

        static RedisClientExtend()
        {
            var type = typeof(RedisNativeClient);
            var method = type.GetMethod("SendExpectInt", BindingFlags.Instance | BindingFlags.NonPublic);

            var expClient = Expression.Parameter(typeof(RedisNativeClient), "client");
            var expArgs = Expression.Parameter(typeof(byte[][]), "args");
            var expCall = Expression.Call(expClient, method, expArgs);
            sendExpectIntFunc = (Func<RedisNativeClient, byte[][], int>)Expression.Lambda(typeof(Func<RedisNativeClient, byte[][], int>), expCall, expClient, expArgs).Compile();
        }

        /// <summary>
        /// 批量删除 Hash 中的键
        /// </summary>
        public static int RemoveEntryFromHash(this IRedisClient client, string hashId, params string[] keys)
        {
            return HDel(client, hashId, keys);
        }

        static int HDel(this IRedisClient client, string hashId, params string[] keys)
        {
            if (hashId == null)
            {
                throw new ArgumentNullException("hashId");
            }
            if (keys.Length == 0)
            {
                throw new ArgumentNullException("keys");
            }
            List<string> list = new List<string>();
            list.Add(hashId);
            list.AddRange(keys);
            byte[][] cmdWithBinaryArgs = MergeCommandWithArgs(ServiceStack.Redis.Commands.HDel, list.ToArray());
            return sendExpectIntFunc((RedisNativeClient)client, cmdWithBinaryArgs);

        }

        static byte[][] MergeCommandWithArgs(byte[] cmd, params string[] args)
        {
            byte[][] bufferArray = ToMultiByteArray(args);
            return MergeCommandWithArgs(cmd, bufferArray);
        }

        static byte[][] MergeCommandWithArgs(byte[] cmd, params byte[][] args)
        {
            byte[][] bufferArray = new byte[1 + args.Length][];
            bufferArray[0] = cmd;
            for (int i = 0; i < args.Length; i++)
            {
                bufferArray[i + 1] = args[i];
            }
            return bufferArray;
        }

        public static byte[][] ToMultiByteArray(this string[] args)
        {
            byte[][] bufferArray = new byte[args.Length][];
            for (int i = 0; i < args.Length; i++)
            {
                bufferArray[i] = ServiceStack.Text.StringExtensions.ToUtf8Bytes(args[i]);
            }
            return bufferArray;
        }

    }
}
