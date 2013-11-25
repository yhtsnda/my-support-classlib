using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    /// <summary>
    /// 支持二级缓存的 Redis 缓存。 一级缓存使用 AspnetCache 并且依赖于二级缓存。
    /// </summary>
    public class RedisHashSecondaryCache : SecondaryCache
    {
        RedisHashCache redisHashCache;
        RedisCacheDependProvider dependProvider;

        public RedisHashSecondaryCache()
        {
            redisHashCache = new RedisHashCache();
            SecondCache = redisHashCache;
            dependProvider = new RedisCacheDependProvider();

            FirstCache = new DependableCache(new AspnetCache(), dependProvider);
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
            redisHashCache.ConnectionName = ConnectionName;

            dependProvider.HashName = CacheName;
            dependProvider.ConnectionName = ConnectionName;
            base.InitCache();
        }
    }
}
