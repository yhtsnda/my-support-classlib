using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using ServiceStack.Redis;

namespace Projects.Framework.RedisAccess
{
    public static class RedisManager
    {
        const string DefaultConn = "redisconn";

        static Dictionary<string, PooledRedisClientManager> clientManagerDic;
        static object rootSync = new object();

        static RedisManager()
        {
            clientManagerDic = new Dictionary<string, PooledRedisClientManager>();
            ParseConnectionString(DefaultConn);
        }

        public static IRedisClient GetRedisClient()
        {
            return CreateRedisClient(DefaultConn);
        }

        public static IRedisClient CreateRedisClient(string connName)
        {
            PooledRedisClientManager clientManager;
            if (!clientManagerDic.TryGetValue(connName, out clientManager))
            {
                lock (rootSync)
                {
                    if (!clientManagerDic.TryGetValue(connName, out clientManager))
                    {
                        clientManager = ParseConnectionString(connName);
                        clientManagerDic.Add(connName, clientManager);
                    }
                }
            }
            return clientManager.GetClient();
        }

        static PooledRedisClientManager ParseConnectionString(string connName)
        {
            var conn = ConfigurationManager.ConnectionStrings[connName];
            if (conn == null)
                throw new ConfigurationErrorsException(String.Format("缺少命名为“{0}”的连接字符串", connName));

            List<string> readWriteHosts = new List<string>();
            List<string> readOnlyHosts = new List<string>();
            RedisClientManagerConfig config = new RedisClientManagerConfig();

            string[] items = conn.ConnectionString.Split(';');
            foreach (var kv in items)
            {
                if (String.IsNullOrWhiteSpace(kv))
                    continue;

                var vs = kv.Split('=');
                if (vs.Length != 2)
                    throw new ArgumentException("Redis 的连接串在 " + kv + " 处存在错误");
                var key = vs[0].Trim().Replace(" ", "").ToLower();
                var value = vs[1].Trim();
                switch (key)
                {
                    case "readwriteserver":
                        readWriteHosts.Add(value);
                        break;
                    case "readserver":
                        readOnlyHosts.Add(value);
                        break;
                    case "maxreadpoolsize":
                        config.MaxReadPoolSize = Int32.Parse(value);
                        break;
                    case "maxwritepoolsize":
                        config.MaxWritePoolSize = Int32.Parse(value);
                        break;
                    case "autostart":
                        config.AutoStart = Boolean.Parse(value);
                        break;
                    case "defaultdb":
                        config.DefaultDb = Int32.Parse(value);
                        break;
                    default:
                        throw new ArgumentException("无法识别的 Redis 连接串属性，发生在 " + kv);
                }
            }
            return new PooledRedisClientManager(readWriteHosts, readOnlyHosts, config);
        }
    }
}
