using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    public static class CacheUtil
    {
        public static ICache GetDefaultCache(string cacheName, bool init = true)
        {
            List<SettingNode> settingNodes = new List<SettingNode>();

            SettingNode cacheNode = ToolSection.Instance.TryGetNode("cache");
            if (cacheNode != null)
                settingNodes.Add(cacheNode);

            ICache cache = ToolSettingUtil.TryCreateInstance<ICache>(settingNodes, "cache", "type");

            if (cache == null)
                cache = new AspnetCache();
            cache.CacheName = cacheName;

            if (init)
                CacheUtil.InitSetting(cache, ToolSettingUtil.GetInstanceSettingNodes(settingNodes, cache));
            return cache;
        }

        public static void InitSetting(this ICache cache, IEnumerable<SettingNode> settingNodes)
        {
            if (cache == null)
                throw new ArgumentNullException("cache");

            if (cache is ICacheImplementor)
                ((ICacheImplementor)cache).InitSetting(settingNodes);
        }
    }
}
