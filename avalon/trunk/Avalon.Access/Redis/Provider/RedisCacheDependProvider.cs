using Avalon.Access;
using Avalon.Profiler;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    /// <summary>
    /// Redis 缓存依赖提供者
    /// </summary>
    public class RedisCacheDependProvider : ICacheDependProvider
    {
        public RedisCacheDependProvider()
        {
        }

        public RedisCacheDependProvider(string connectionName, string hashName)
        {
            ConnectionName = connectionName;
            HashName = hashName;
        }

        public string ConnectionName
        {
            get;
            set;
        }

        public string HashName
        {
            get;
            set;
        }

        public void InitSetting(IEnumerable<SettingNode> nodes)
        {
            this.TrySetSetting(nodes, ConfigurationName, "connectionName", o => o.ConnectionName);
            this.TrySetSetting(nodes, ConfigurationName, "hashName", o => o.HashName);
        }

        public void Init()
        {
            if (String.IsNullOrEmpty(ConnectionName))
                throw new ArgumentNullException("ConnectionName");

            if (String.IsNullOrEmpty(HashName))
                throw new ArgumentNullException("HashName");
        }

        public IList<CacheDependResult> GetBatchDependResult(IEnumerable<string> keys)
        {
            Init();

            using (var client = CreateRedisClient())
            {
                var ks = keys.ToArray();
                var vs = client.GetValuesFromHash(HashName, ks);

                var results = new List<CacheDependResult>();
                for (var i = 0; i < ks.Length; i++)
                {
                    var v = vs[i];
                    if (String.IsNullOrEmpty(v))
                        results.Add(new CacheDependResult(ks[i]));
                    else
                        results.Add(new CacheDependResult(ks[i], Int64.Parse(v)));
                }
                ProfilerContext.Current.Trace("dependcache",
                       String.Format("[{0}] @{1} {2}/{3}\r\n {4} ", GetType().Name, HashName, vs.Count, keys.Count(), String.Join(",", keys)));
                return results;
            }
        }

        IRedisClient CreateRedisClient()
        {
            return RedisManager.Instance.CreateRedisClient(ConnectionName);
        }

        string ConfigurationName
        {
            get
            {
                var name = GetType().Name;
                return name.Substring(0, 1).ToLower() + name.Substring(1);
            }
        }


    }
}
