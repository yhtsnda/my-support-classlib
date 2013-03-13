using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Framework.Shards;
using Projects.Tool;
using Projects.Tool.Reflection;
using Projects.Tool.Shards;

namespace Projects.Framework.Configurations
{
    internal class ShardConfiguration
    {
        ToolSection section;
        IShardSessionFactory defaultSessionFactory;
        Type defaultStrategyType;
        IDictionary<string, string> defaultAttr = new Dictionary<string, string>();

        Dictionary<Type, IShardSessionFactory> factoryInstances = new Dictionary<Type, IShardSessionFactory>();
        Dictionary<Type, IShardStrategy> stategies = new Dictionary<Type, IShardStrategy>();
        Dictionary<Type, IShardSessionFactory> sessionFactories = new Dictionary<Type, IShardSessionFactory>();

        public ShardConfiguration(ToolSection section)
        {
            this.section = section;
        }

        public void Load()
        {
            var shardNode = section.TryGetNode("shard");
            if (shardNode != null)
            {
                var dst = shardNode.TryGetValue("defaultStrategy");
                if (!String.IsNullOrEmpty(dst))
                    defaultStrategyType = Type.GetType(dst);

                defaultAttr.Add("shard", shardNode.TryGetValue("defaultShard"));

                defaultSessionFactory = CreateShardSessionFactory(shardNode, "defaultSessionFactory");

                foreach (var item in shardNode.TryGetNodes("entities/entity"))
                {
                    var entityType = Type.GetType(item.TryGetValue("type"));
                    if (entityType == null)
                        throw new ArgumentNullException("entityType");

                    var stategyString = item.TryGetValue("strategy");
                    Type strategyType = null;
                    if (!String.IsNullOrEmpty(stategyString))
                        strategyType = Type.GetType(stategyString);
                    if (strategyType == null)
                        strategyType = defaultStrategyType;

                    if (strategyType == null)
                        throw new PlatformException("类型 {0} 没有定义分区策略", entityType.FullName);

                    RegisterShardStragety(entityType, strategyType, item.Attributes);

                    var factoryString = item.TryGetValue("sessionFactory");
                    if (!String.IsNullOrEmpty(factoryString))
                    {
                        Type factoryType = Type.GetType(factoryString);
                        RegisterShardSessionFactory(entityType, factoryType);
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定类型的分区策略
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public IShardStrategy GetShardStrategy(Type entityType)
        {
            if (entityType == null)
                throw new ArgumentNullException("entityType");

            var bt = entityType;
            var strategy = stategies.TryGetValue(bt);
            while (strategy == null && bt != null)
            {
                bt = bt.BaseType;
                if (bt != null)
                    strategy = stategies.TryGetValue(bt);
            }

            if (strategy == null)
                return RegisterShardStragety(entityType, typeof(NoShardStrategy), defaultAttr);

            return strategy;
        }

        /// <summary>
        /// 注册类型的分区策略
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="shardStragegyType"></param>
        /// <param name="attributes"></param>
        public IShardStrategy RegisterShardStragety(Type entityType, Type shardStragegyType, IDictionary<string, string> attributes = null)
        {
            if (entityType == null)
                throw new ArgumentNullException("entityType");
            if (shardStragegyType == null)
                throw new ArgumentNullException("shardStragegyType");

            IShardStrategy strategy = (IShardStrategy)FastActivator.Create(shardStragegyType);

            if (strategy == null)
                throw new ArgumentNullException("strategy");

            if (strategy is AbstractShardStrategy && attributes != null)
                ((AbstractShardStrategy)strategy).Init(attributes);

            if (stategies.ContainsKey(entityType))
                stategies[entityType] = strategy;
            else
                stategies.Add(entityType, strategy);
            return strategy;
        }

        /// <summary>
        /// 获取指定类型的 IShardSessionFactory 实例
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public IShardSessionFactory GetSessionFactory(Type entityType)
        {
            if (entityType == null)
                throw new ArgumentNullException("entityType");

            return GetSessionFactory(entityType, false);
        }

        /// <summary>
        /// 注册类型的 IShardSessionFactory 实例
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="shardSessionFactoryType"></param>
        public void RegisterShardSessionFactory(Type entityType, Type shardSessionFactoryType)
        {
            if (entityType == null)
                throw new ArgumentNullException("entityType");

            if (shardSessionFactoryType == null)
                throw new ArgumentNullException("shardSessionFactoryType");

            var factory = CreateShardSessionFactory(shardSessionFactoryType);
            if (factory != null)
                sessionFactories.Add(entityType, factory);
        }

        public IShardSessionFactory GetSessionFactory(Type entityType, bool valid)
        {
            var factory = sessionFactories.TryGetValue(entityType);
            while (factory == null && entityType != null)
            {
                entityType = entityType.BaseType;
                if (entityType != null)
                    factory = sessionFactories.TryGetValue(entityType);
            }
            factory = factory ?? defaultSessionFactory;

            if (valid && factory == null)
                throw new PlatformException("无法获取类型 " + entityType.FullName + " 的 IShardSessionFactory 对象。");

            return factory;
        }

        IShardStrategy CreateShardStrategy(SettingNode node, string path)
        {
            var type = node.TryGetValue(path);
            if (String.IsNullOrEmpty(type))
                return null;

            IShardStrategy strategy = (IShardStrategy)FastActivator.Create(type);
            if (strategy != null && strategy is AbstractShardStrategy)
                ((AbstractShardStrategy)strategy).Init(node.Attributes);

            return strategy;
        }

        IShardSessionFactory CreateShardSessionFactory(SettingNode node, string path)
        {
            var factoryTypeString = node.TryGetValue(path);
            if (String.IsNullOrEmpty(factoryTypeString))
                return null;

            Type factoryType = Type.GetType(factoryTypeString);
            return CreateShardSessionFactory(factoryType);
        }

        IShardSessionFactory CreateShardSessionFactory(Type factoryType)
        {
            var factory = factoryInstances.TryGetValue(factoryType);
            if (factory == null)
            {
                factory = (IShardSessionFactory)FastActivator.Create(factoryType);
                if (factory == null)
                    throw new ArgumentNullException("factory");

                factoryInstances.Add(factoryType, factory);
            }
            return factory;
        }
    }
}
