using System;
using System.Linq;
using System.Collections.Generic;
using ServiceStack.Redis;
using ServiceStack.Text;
using Avalon.Access;

namespace Avalon.Utility
{
    public class RedisCache : AbstractCache
    {
        ILog log = LogManager.GetLogger<RedisCache>();

        protected override bool IsLocal
        {
            get { return false; }
        }

        public string ConnectionName
        {
            get;
            set;
        }

        public override bool EmptySupport
        {
            get
            {
                return false;
            }
            set
            {
                if (value)
                    throw new NotSupportedException();
                base.EmptySupport = value;
            }
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

        protected override IList<CacheItemResult> GetBatchResultInner(Type type, IEnumerable<string> keys)
        {
            foreach (var key in keys)
                ValidParams(key);

            List<string> jsons;
            using (var client = CreateRedisClient())
            {
                jsons = client.GetValuesFromHash(CacheName, keys.ToArray());
            }

            var outputs = keys.For((o, i) => new CacheItemResult(o, GetValue(type, jsons[i])));
            return outputs;
        }

        protected override void SetBatchInner(IEnumerable<CacheItem> items, DateTime expiredTime)
        {
            var range = items.Select(o => new KeyValuePair<string, string>(o.Key, JsonSerializer.SerializeToString(o.Value, o.Value.GetType())));

            using (var client = CreateRedisClient())
            {
                client.SetRangeInHash(CacheName, range);
            }
        }

        protected override bool ContainsInner(Type type, string key)
        {
            return base.ContainsByResult(type, key);
        }

        protected override void RemoveInner(Type type, string key)
        {
            ValidParams(key);

            using (var client = CreateRedisClient())
            {
                client.RemoveEntryFromHash(CacheName, key);
            }
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

        object GetValue(Type type, string json)
        {
            if (String.IsNullOrEmpty(json))
                return null;
            try
            {
                return JsonSerializer.DeserializeFromString(json, type);
            }
            catch (Exception ex)
            {
                log.WarnFormat("反序列化数据 {0} 发生错误 {1}", json, ex.ToString());
            }
            return null;
        }
    }
}
