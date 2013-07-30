using System;
using System.Linq;
using System.Collections.Generic;
using Projects.Tool.Util;
using ServiceStack.Redis;
using ServiceStack.Text;

namespace Projects.Tool
{
    public class RedisCache : AbstractCache
    {
        ILog log = LogManager.GetLogger<RedisCache>();

        public string ConnName
        {
            get;
            private set;
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
            ValidParams(key);

            if (typeof(T).IsClass && EqualityComparer<T>.Default.Equals(value, default(T)))
                return;

            using (var client = CreateRedisClient())
            {
                var json = JsonSerializer.SerializeToString(value, value.GetType());
                client.SetEntryInHash(CacheName, key, JsonSerializer.SerializeToString(value, value.GetType()));
            }
        }

        protected override object GetInner(Type type, string key)
        {
            ValidParams(key);

            using (var client = CreateRedisClient())
            {
                var json = client.GetValueFromHash(CacheName, key);
                if (String.IsNullOrEmpty(json))
                    return null;

                try
                {
                    return JsonSerializer.DeserializeFromString(json, type);
                }
                catch (Exception ex)
                {
                    log.WarnFormat("反序列化数据 {0} 发生错误 {1}", key, ex.ToString());
                    return null;
                }
            }
        }

        protected override void RemoveInner(Type type, string key)
        {
            ValidParams(key);

            using (var client = CreateRedisClient())
            {
                client.RemoveEntryFromHash(CacheName, key);
            }
        }

        public override IEnumerable<CacheItemResult> GetBatchResult(Type type, IEnumerable<string> keys)
        {
            foreach (var key in keys)
                ValidParams(key);

            List<string> jsons;
            using (var client = CreateRedisClient())
            {
                jsons = client.GetValuesFromHash(CacheName, keys.ToArray());
            }

            List<CacheItemResult> outputs = keys.Select(o => new CacheItemResult() { Key = o }).ToList();
            int index = 0;
            foreach (var json in jsons)
            {
                var output = outputs[index];
                if (String.IsNullOrEmpty(json))
                {
                    output.IsMissing = true;
                }
                else
                {
                    try
                    {
                        output.Value = JsonSerializer.DeserializeFromString(json, type);
                    }
                    catch (Exception ex)
                    {
                        log.WarnFormat("反序列化数据 {0} 发生错误 {1}", output.Key, ex.ToString());
                        output.IsMissing = true;
                    }
                }
                index++;
            }
            TraceCache(keys, outputs.GetMissingKeys().Count());
            return outputs;
        }

        void ValidParams(string key)
        {
            if (String.IsNullOrEmpty(ConnName))
                throw new ArgumentNullException("ConnName");

            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
        }

        IRedisClient CreateRedisClient()
        {
            return RedisClientProvider.CreateRedisClient(ConnName);
        }

    }
}
