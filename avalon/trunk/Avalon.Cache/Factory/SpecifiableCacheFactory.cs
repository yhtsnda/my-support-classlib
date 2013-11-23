using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    /// <summary>
    /// 可指定缓存类型的工厂
    /// </summary>
    public class SpecifiableCacheFactory : ICacheFactory
    {
        IDictionary<string, SettingNode> cacheNodes;
        List<MatchCacheData> matchCacheNodes;

        IDictionary<string, ICache> cacheDic = new Dictionary<string, ICache>();
        object syscRoot = new object();

        public SpecifiableCacheFactory()
        {
            ReloadConfig();
            ToolSection.ToolSectionChanged += ToolSection_ToolSectionChanged;
        }

        public virtual ICache GetCacher(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            ICache cache = cacheDic.TryGetValue(name);
            if (cache == null)
            {
                lock (syscRoot)
                {
                    cache = cacheDic.TryGetValue(name);
                    if (cache == null)
                    {
                        SettingNode node = cacheNodes.TryGetValue(name);

                        if (node != null)
                        {
                            cache = CreateCache(name, node);
                        }
                        else
                        {
                            var matchCache = matchCacheNodes.FirstOrDefault(o => o.IsMatch(name));
                            if (matchCache != null)
                                cache = CreateCache(name, matchCache.Node);
                            else
                                cache = CacheUtil.GetDefaultCache(name);
                        }

                        if (cache == null)
                            throw new ArgumentNullException("cache", node == null ? "node is null" : (" path: " + node.Path + " type:" + node.TryGetValue("type")));

                        cacheDic.Add(name, cache);
                    }
                }
            }
            return cache;
        }


        public void Register(string name, ICache cache)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            if (cache == null)
                throw new ArgumentNullException("cache");

            lock (syscRoot)
            {
                cacheDic[name] = cache;
                cache.CacheName = name;
            }
        }

        public void ReloadConfig()
        {
            lock (syscRoot)
            {
                cacheNodes = new Dictionary<string, SettingNode>();
                matchCacheNodes = new List<MatchCacheData>();
                cacheDic.Clear();

                var nodes = ToolSection.Instance.TryGetNodes("cache/specifiableFactory/add");
                foreach (var node in nodes)
                {
                    var key = node.TryGetValue("key");

                    if (key.Contains("*"))
                        matchCacheNodes.Add(new MatchCacheData(key, node));
                    else
                        cacheNodes.Add(key, node);
                }
            }
        }

        void ToolSection_ToolSectionChanged(object sender, EventArgs e)
        {
            ReloadConfig();
        }

        ICache CreateCache(string cacheName, SettingNode node)
        {
            ICache cache = ToolSettingUtil.TryCreateInstance<ICache>(node, "type");
            cache.CacheName = cacheName;

            List<SettingNode> settingNodes = new List<SettingNode>() { node };
            settingNodes.Add(ToolSection.Instance.TryGetNode("cache/specifiableFactory"));
            settingNodes.Add(ToolSection.Instance.TryGetNode("cache"));

            CacheUtil.InitSetting(cache, ToolSettingUtil.GetInstanceSettingNodes(settingNodes, cache));

            return cache;
        }

        class MatchCacheData
        {
            public MatchCacheData(string key, SettingNode node)
            {
                bool s = key.StartsWith("*");
                bool e = key.EndsWith("*");
                if (s && e)
                {
                    MatchType = MatchType.Contains;
                    Word = key.Substring(1, key.Length - 2);
                }
                else if (s)
                {
                    MatchType = MatchType.EndWidth;
                    Word = key.Substring(1);
                }
                else
                {
                    MatchType = MatchType.StartWith;
                    Word = key.Substring(0, key.Length - 1);
                }
                Node = node;
            }

            public string Word { get; set; }

            public MatchType MatchType { get; set; }

            public SettingNode Node { get; set; }

            public bool IsMatch(string key)
            {
                switch (MatchType)
                {
                    case MatchType.StartWith:
                        return key.StartsWith(Word);
                    case MatchType.EndWidth:
                        return key.EndsWith(Word);
                }
                return key.Contains(Word);
            }
        }

        enum MatchType
        {
            StartWith,
            EndWidth,
            Contains
        }
    }
}
