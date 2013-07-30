using Projects.Tool.RedisProvider;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.Tool
{
    public class RedisHashCache : AbstractCache
    {
        ILog log = LogManager.GetLogger<RedisHashCache>();

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

            using (var client = CreateRedisClient())
            {
                var keyPrefix = GetKeyPrefix(key);
                var kvs = ObjectHashMappingUtil.ToHash(value, keyPrefix);
                client.SetRangeInHash(CacheName, kvs);
            }
        }

        protected override object GetInner(Type type, string key)
        {
            ValidParams(key);
            Projects.Tool.Accumlator.Accumlator.GetInstance().Increment("RedisHash" + type.FullName);

            using (var client = CreateRedisClient())
            {
                var keyPrefix = GetKeyPrefix(key);
                var keys = ObjectHashMappingUtil.GetKeys(type, keyPrefix);
                var values = client.GetValuesFromHash(CacheName, keys).ToArray();
                if (values.All(o => String.IsNullOrEmpty(o)))
                    return null;

                try
                {
                    return ObjectHashMappingUtil.ToObject(type, values);
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
                var keyPrefix = GetKeyPrefix(key);
                var keys = ObjectHashMappingUtil.GetKeys(type, keyPrefix);
                client.SetRangeInHash(CacheName, keys.Select(o => new KeyValuePair<string, string>(o, "")));
            }
        }

        public override IEnumerable<CacheItemResult> GetBatchResult(Type type, IEnumerable<string> keys)
        {
            //产生 field keys
            List<BatchItem> items = new List<BatchItem>();
            foreach (var key in keys)
            {
                ValidParams(key);
                var item = new BatchItem()
                {
                    SourceKey = key,
                    FieldKeys = ObjectHashMappingUtil.GetKeys(type, GetKeyPrefix(key))
                };
                items.Add(item);
            }

            //批量获取数据
            string[] allValues;
            using (var client = CreateRedisClient())
            {
                var allKeys = items.SelectMany(o => o.FieldKeys).ToArray();
                allValues = client.GetValuesFromHash(CacheName, allKeys).ToArray();
            }

            //转换数据为对象
            List<CacheItemResult> outputs = keys.Select(o => new CacheItemResult()).ToList();
            int index = 0;
            foreach (var item in items)
            {
                var output = outputs[index];
                output.Key = item.SourceKey;

                var values = new string[item.FieldKeys.Length];
                Array.Copy(allValues, index * item.FieldKeys.Length, values, 0, values.Length);

                //判断是否命中
                if (values.All(o => String.IsNullOrEmpty(o)))
                {
                    output.IsMissing = true;
                }
                else
                {
                    try
                    {
                        output.Value = ObjectHashMappingUtil.ToObject(type, values); ;
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

        public override void SetBatch<T>(IEnumerable<CacheItem<T>> items, DateTime expiredTime)
        {
            List<KeyValuePair<string, string>> datas = new List<KeyValuePair<string, string>>();
            foreach (var item in items)
            {
                var keyPrefix = GetKeyPrefix(item.Key);
                var kvs = ObjectHashMappingUtil.ToHash(item.Value, keyPrefix);
                datas.AddRange(kvs);
            }

            using (var client = CreateRedisClient())
            {
                client.SetRangeInHash(CacheName, datas);
            }
        }

        void ValidParams(string key)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
        }

        IRedisClient CreateRedisClient()
        {
            return RedisClientProvider.CreateRedisClient(ConnName);
        }

        string GetKeyPrefix(string key)
        {
            return key.Replace(CacheName + ":", "") + ":";
        }

        class BatchItem
        {
            public string SourceKey { get; set; }

            public string[] FieldKeys { get; set; }
        }
    }
}
