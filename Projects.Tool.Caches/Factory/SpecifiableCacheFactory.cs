﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Tool.Reflection;

namespace Projects.Tool
{
    /// <summary>
    /// 可指定缓存类型的工厂
    /// </summary>
    public class SpecifiableCacheFactory : ICacheFactory
    {
        IDictionary<string, SettingNode> cacheNodes;
        IDictionary<string, ICache> cacheDic = new Dictionary<string, ICache>();

        public SpecifiableCacheFactory()
        {
            cacheNodes = new Dictionary<string, SettingNode>();
            var nodes = ToolSection.Instance.TryGetNodes("cache/specifiableFactory/add");
            foreach (var node in nodes)
            {
                cacheNodes.Add(node.TryGetValue("key"), node);
            }
        }

        public virtual ICache GetCacher(string name)
        {
            ICache cache = cacheDic.TryGetValue(name);
            if (cache == null)
            {
                SettingNode node = cacheNodes.TryGetValue(name);

                if (node == null)
                {
                    cache = CacheUtil.GetDefaultCache();
                }
                else
                {
                    cache = CacheUtil.TryCreateCache(node, "type");
                    List<SettingNode> settingNodes = new List<SettingNode>() { node };
                    settingNodes.Add(ToolSection.Instance.TryGetNode("cache/specifiableFactory"));
                    settingNodes.Add(ToolSection.Instance.TryGetNode("cache"));

                    CacheUtil.InitCache(cache, CacheUtil.GetCacheSettingNode(settingNodes, cache));
                }
                if (cache is AbstractCache)
                    ((AbstractCache)cache).CacheName = name;

                cacheDic[name] = cache;
            }
            return cache;
        }

        public ICache GetCacher(Type type)
        {
            return GetCacher(type.FullName);
        }
    }
}
