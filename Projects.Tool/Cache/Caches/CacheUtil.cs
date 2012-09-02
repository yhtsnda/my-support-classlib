using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Tool.Reflection;

namespace Projects.Tool
{
    internal class CacheUtil
    {
        public static ICache GetDefaultCache(bool init = true)
        {
            SettingNode cacheNode = ToolSection.Instance.TryGetNode("cache");
            List<SettingNode> settingNodes = new List<SettingNode> { cacheNode };
            ICache cache = CacheUtil.TryCreateCache(cacheNode, "type");
            if (cache == null)
                cache = new AspnetCache();

            if (init)
                CacheUtil.InitCache(cache, CacheUtil.GetCacheSettingNode(settingNodes, cache));
            return cache;
        }

        /// <summary>
        /// 尝试创建缓存实例
        /// </summary>
        /// <param name="node">允许为空</param>
        /// <param name="typePath"></param>
        /// <returns></returns>
        public static ICache TryCreateCache(SettingNode node, string typePath)
        {
            if (node != null)
            {
                string typeName = node.TryGetValue(typePath);
                if (!String.IsNullOrEmpty(typeName))
                {
                    Type type = Type.GetType(typeName);
                    return (ICache)FastActivator.Create(type);
                }
            }
            return null;
        }

        public static ICache TryCreateCache(IEnumerable<SettingNode> nodes, string typePath)
        {
            string typeName = TryGetValue(nodes, typePath);
            if (!String.IsNullOrEmpty(typeName))
            {
                Type type = Type.GetType(typeName);
                return (ICache)FastActivator.Create(type);
            }
            return null;
        }

        public static void InitCache(ICache cache, IEnumerable<SettingNode> settingNodes)
        {
            if (cache == null)
                throw new ArgumentNullException("cache");

            if (cache is ICacheSettingable)
                ((ICacheSettingable)cache).InitSetting(settingNodes);
        }

        /// <summary>
        /// 获取缓存对象的设置项，可能返回空
        /// </summary>
        /// <param name="rootNode">允许为空</param>
        /// <param name="cache"></param>
        /// <returns></returns>
        public static IEnumerable<SettingNode> GetCacheSettingNode(IEnumerable<SettingNode> rootNodes, ICache cache)
        {
            if (cache == null)
                throw new ArgumentNullException("cache");

            string name = cache.GetType().Name;
            name = name.Substring(0, 1).ToLower() + name.Substring(1);

            rootNodes = rootNodes.Where(o => o != null);
            List<SettingNode> settingNodes = new List<SettingNode>();
            foreach (SettingNode rootNode in rootNodes)
            {
                SettingNode targetNode = rootNode.TryGetNode(name);
                settingNodes.Add(targetNode);
                settingNodes.Add(rootNode);
            }
            return settingNodes.Where(o => o != null);
        }

        public static IEnumerable<SettingNode> Combine(IEnumerable<SettingNode> nodes)
        {
            return nodes.Where(o => o != null);
        }

        public static IEnumerable<SettingNode> Combine(params SettingNode[] nodes)
        {
            return nodes.Where(o => o != null);
        }

        public static IEnumerable<SettingNode> Combine(SettingNode node, IEnumerable<SettingNode> nodes)
        {
            List<SettingNode> items = new List<SettingNode>();
            items.Add(node);
            items.AddRange(nodes);
            return items.Where(o => o != null);
        }

        public static string TryGetValue(IEnumerable<SettingNode> settingNodes, string path)
        {
            foreach (SettingNode node in settingNodes)
            {
                string v = node.TryGetValue(path);
                if (!String.IsNullOrEmpty(v))
                    return v;
            }
            return null;
        }

        public static SettingNode TryGetNode(IEnumerable<SettingNode> settingNodes, string path)
        {
            foreach (SettingNode node in settingNodes)
            {
                SettingNode targetNode = node.TryGetNode(path);
                if (targetNode != null)
                    return targetNode;
            }
            return null;
        }

        public static IEnumerable<SettingNode> TryGetNodes(IEnumerable<SettingNode> settingNodes, string path)
        {
            foreach (SettingNode node in settingNodes)
            {
                IEnumerable<SettingNode> nodes = node.TryGetNodes(path);
                if (nodes.Count() > 0)
                    return nodes;
            }
            return new List<SettingNode>();
        }
    }
}
