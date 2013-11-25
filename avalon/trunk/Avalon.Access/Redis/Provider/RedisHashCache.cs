using Avalon.Access;
using Avalon.RedisProvider;
using ServiceStack.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    /// <summary>
    /// 使用 RedisHash 数据结构缓存数据。
    /// </summary>
    /// <remarks>
    /// 分3种情况存储数据
    /// 1、空值：
    ///     key     #{id}:{ValueName}
    ///     value   {NullValue}
    /// 2、简单对象：
    ///     key     #{id}:{ValueName}
    ///     value   {value}
    /// 3、复杂对象
    ///     key     #{id}:{property}...
    ///     value   {value}...
    /// 所有的类型都包含值，该值可用于依赖缓存或被对象缓存的管理
    ///     key     {key}
    ///     value   {timestamp}
    /// </remarks>
    public class RedisHashCache : AbstractCache
    {
        RedisCacheDependProvider dependProvider;
        ILog log = LogManager.GetLogger<RedisHashCache>();

        protected override bool IsLocal
        {
            get { return false; }
        }

        public string ConnectionName
        {
            get;
            set;
        }

        protected override void InitSetting(IEnumerable<SettingNode> settingNodes)
        {
            base.InitSetting(settingNodes);

            this.TrySetSetting(settingNodes, ConfigurationName, "connectionName", o => o.ConnectionName);
        }

        protected override void InitCache()
        {
            base.InitCache();

            if (String.IsNullOrEmpty(ConnectionName))
                throw new ArgumentNullException("ConnectionName");
        }

        protected override void RemoveInner(Type type, string key)
        {
            ValidParams(key);

            using (var client = CreateRedisClient())
            {
                var keys = new List<string>(GetKeys(type, key));
                keys.Add(key);
                client.RemoveEntryFromHash(CacheName, keys.ToArray());
            }
        }

        protected override IList<CacheItemResult> GetBatchResultInner(Type type, IEnumerable<string> keys)
        {
            //产生 field keys
            List<BatchItem> items = keys.Select(o =>
            {
                ValidParams(o);
                return new BatchItem(o, GetKeys(type, o));
            }).ToList();

            //批量获取数据
            string[] allValues = null;
            using (var client = CreateRedisClient())
            {
                var allKeys = items.SelectMany(o => o.FieldKeys).ToArray();
                allValues = client.GetValuesFromHash(CacheName, allKeys).ToArray();
            }

            var clearKeys = new List<string>();
            var outputs = items.For((o, i) =>
            {
                var values = new string[o.FieldKeys.Length];
                Array.Copy(allValues, i * o.FieldKeys.Length, values, 0, values.Length);

                object value = null;

                try
                {
                    value = ObjectHashMappingUtil.ToObject(type, values);
                }
                catch (Exception ex)
                {
                    clearKeys.AddRange(o.FieldKeys);
                    log.WarnFormat("反序列化数据 {0} 发生错误 {1}", String.Join("\r\n", values), ex.ToString());
                }
                return new CacheItemResult(o.SourceKey, value);
            });

            //清除无效的数据
            if (clearKeys.Count > 0)
            {
                using (var client = CreateRedisClient())
                {
                    client.RemoveEntryFromHash(CacheName, clearKeys.ToArray());
                }
            }
            return outputs;
        }

        protected override void SetBatchInner(IEnumerable<CacheItem> items, DateTime expiredTime)
        {
            List<KeyValuePair<string, string>> datas = new List<KeyValuePair<string, string>>();
            var tick = NetworkTime.Now.Ticks.ToString();
            foreach (var item in items)
            {
                var keyPrefix = GetKeyPrefix(item.Key);
                var kvs = ObjectHashMappingUtil.ToHash(item.Value, keyPrefix);

                // add timestamp for depend and manager
                kvs.Add(item.Key, tick);
                datas.AddRange(kvs);
            }

            using (var client = CreateRedisClient())
            {
                client.SetRangeInHash(CacheName, datas);
            }
        }

        protected override bool ContainsInner(Type type, string key)
        {
            return base.ContainsByResult(type, key);
        }

        object GetObject(Type type, string[] values)
        {
            if (values.All(o => String.IsNullOrEmpty(o)))
                return null;
            try
            {
                return ObjectHashMappingUtil.ToObject(type, values);
            }
            catch (Exception ex)
            {

                log.WarnFormat("反序列化数据 {0} 发生错误 {1}", String.Join("\r\n", values), ex.ToString());
            }
            return null;
        }

        string[] GetKeys(Type type, string key)
        {
            var keyPrefix = GetKeyPrefix(key);
            return ObjectHashMappingUtil.GetKeys(type, keyPrefix);
        }

        void ValidParams(string key)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
        }

        IRedisClient CreateRedisClient()
        {
            return RedisManager.Instance.CreateRedisClient(ConnectionName);
        }

        string GetKeyPrefix(string key)
        {
            return key.Replace(CacheName + ":", "") + ":";
        }


        class BatchItem
        {
            public BatchItem(string sourceKey, string[] fieldKeys)
            {
                SourceKey = sourceKey;
                FieldKeys = fieldKeys;
            }

            public string SourceKey { get; private set; }

            public string[] FieldKeys { get; private set; }
        }
    }
}
