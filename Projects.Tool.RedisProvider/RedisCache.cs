using System;
using System.Collections.Generic;
using Projects.Tool.Util;
using ServiceStack.Redis;

namespace Projects.Tool
{
    public class RedisCache : AbstractCache
    {
        public string ConnName
        {
            get;
            private set;
        }

        IRedisClient CreateRedisClient()
        {
            return RedisClientProvider.CreateRedisClient(ConnName);
        }

        public override void InitSetting(IEnumerable<SettingNode> settingNodes)
        {
            base.InitSetting(settingNodes);

            string connName = TryGetValue(settingNodes, "connectionName");
            if (String.IsNullOrEmpty(connName))
                throw new MissConfigurationException(settingNodes, "connectionName");

            ConnName = connName;
        }

        protected override void SetInner<T>(string key, T value, DateTime expiredTime)
        {
            //using (var client = CreateRedisClient())
            //    client.Set(key, value, expiredTime);

            //空值不缓存
            if (string.IsNullOrEmpty(CacheName) ||
                string.IsNullOrEmpty(key) ||
                EqualityComparer<T>.Default.Equals(value, default(T)))
            {
                return;
            }

            using (var client = CreateRedisClient())
            {
                client.SetEntryInHash(
                    CacheName,
                    key,
                    JsonConverter.ToJson(value)
                );
            }
        }

        protected override T GetInner<T>(string key)
        {
            //using (var client = CreateRedisClient())
            //    return client.Get<T>(key);

            if (string.IsNullOrEmpty(CacheName) ||
                string.IsNullOrEmpty(key))
            {
                return default(T);
            }

            using (var client = CreateRedisClient())
            {
                var value = client.GetValueFromHash(CacheName, key);
                if (string.IsNullOrEmpty(value))  // modiby by skypan 2013年2月5日14:39:37
                {
                    return default(T);
                }
                return JsonConverter.FromJson<T>(value);
            }
        }

        protected override void RemoveInner(string key)
        {
            //using (var client = CreateRedisClient())
            //    client.Remove(key);

            if (string.IsNullOrEmpty(CacheName) ||
                string.IsNullOrEmpty(key))
            {
                return;
            }

            using (var client = CreateRedisClient())
            {
                client.RemoveEntryFromHash(CacheName, key);
            }
        }
    }
}
