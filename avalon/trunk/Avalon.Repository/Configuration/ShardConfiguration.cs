using Avalon.Framework.Shards;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Configurations
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
        /// <summary>
        /// 默认的分区策略类型
        /// </summary>
        public Type DefaultStrategyType { get; set; }
        /// <summary>
        /// 默认的分区标识
        /// </summary>
        public ShardId DefaultShardId { get; set; }
        /// <summary>
        /// 默认的会话工厂
        /// </summary>
        public Type DefaultShardSessionFactoryType { get; set; }

        public void Load()
        {
            var shardNode = section.TryGetNode("shard");
            if (shardNode != null)
            {
                this.TrySetSetting(shardNode, "defaultStrategy", 
                    o => o.DefaultStrategyType, v => GetType(v));
                this.TrySetSetting(shardNode, "defaultShard", 
                    o => o.DefaultShardId, v => new ShardId(v));
                this.TrySetSetting(shardNode, "defaultSessionFactory", 
                    o => o.DefaultShardSessionFactoryType, v => GetType(v));

                if (DefaultShardId != null)
                    defaultAttr.Add("shard", DefaultShardId.Id);

                foreach (var item in shardNode.TryGetNodes("entities/entity"))
                {
                    var entityType = Type.GetType(item.TryGetValue("type"));
                    if (entityType == null)
                        throw new ArgumentNullException("entityType");

                    //注册分区策略
                    var stategyString = item.TryGetValue("strategy");
                    Type strategyType = DefaultStrategyType;
                    if (!String.IsNullOrEmpty(stategyString))
                        strategyType = Type.GetType(stategyString);

                    if (strategyType == null)
                        throw new AvalonException("类型 {0} 没有定义分区策略", entityType.FullName);

                    RegisterShardStragety(entityType, strategyType, item.Attributes);

                    //注册工厂
                    var factoryType = DefaultShardSessionFactoryType;
                    var factoryString = item.TryGetValue("sessionFactory");
                    if (!String.IsNullOrEmpty(factoryString))
                        factoryType = GetType(factoryString);
                    if (factoryType == null)
                        throw new AvalonException("类型 {0} 没有定义数据会话工厂或没有定义默认的数据会话工厂", entityType.FullName);

                    RegisterShardSessionFactory(entityType, factoryType);
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
            if (factory == null)
                factory = CreateShardSessionFactory(DefaultShardSessionFactoryType);

            if (valid && factory == null)
                throw new AvalonException("无法获取类型 " + entityType.FullName + " 的 IShardSessionFactory 对象。");

            return factory;
        }

        Type GetType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type == null)
                throw new AvalonException("无法获取值为 {0} 的类型", typeName);
            return type;
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

        internal IShardSessionFactory CreateShardSessionFactory(Type factoryType)
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
