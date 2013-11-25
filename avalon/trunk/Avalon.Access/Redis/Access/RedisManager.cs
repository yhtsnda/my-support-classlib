using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using ServiceStack.Redis;
using Avalon.Framework;
using Avalon.Framework.Shards;

namespace Avalon.Access
{
    public class RedisManager : AbstractConnectionManager<PooledRedisClientManager>
    {
        static RedisManager instance = new RedisManager();

        const string DefaultConn = "redisconn";

        public static RedisManager Instance
        {
            get { return instance; }
        }

        public static IRedisClient GetRedisClient()
        {
            return Instance.CreateRedisClient();
        }

        public IRedisClient CreateRedisClient()
        {
            return GetConnection(DefaultConn).GetClient();
        }

        public IRedisClient CreateRedisClient(string connName)
        {
            return GetConnection(connName).GetClient();
        }

        public IRedisClient CreateRedisClient(ShardId shardId)
        {
            return GetConnection(shardId).GetClient();
        }

        protected override PooledRedisClientManager CreateConnection(string connStr)
        {
            List<string> readWriteHosts = new List<string>();
            List<string> readOnlyHosts = new List<string>();
            RedisClientManagerConfig config = new RedisClientManagerConfig();

            string[] items = connStr.Split(';');
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
