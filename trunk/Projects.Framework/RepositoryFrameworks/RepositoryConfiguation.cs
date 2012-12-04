using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Projects.Tool.Reflection;
using Projects.Tool;
using Projects.Framework.Shards;

namespace Projects.Framework
{
    internal class RepositoryConfiguation
    {
        private object mSyncRoot = new object();
        private IShardSessionFactory mDefaultSessionFactory;
        private Type mDefaultStrategyType;
        private IDictionary<string, string> mDefaultAttr = new Dictionary<string, string>();
        //与仓储相关的程序集
        private List<Assembly> mAssembies = new List<Assembly>();
        private Dictionary<Type, IShardSessionFactory> mFactoryInstances = 
            new Dictionary<Type, IShardSessionFactory>();
        private Dictionary<Type, IShardStrategy> mStategies = new Dictionary<Type, IShardStrategy>();
        private Dictionary<Type, IShardSessionFactory> mSessionFactories = 
            new Dictionary<Type, IShardSessionFactory>();
        private Dictionary<Type, ClassDefineMetadata> mMetaDatas =
            new Dictionary<Type, ClassDefineMetadata>();
        private Dictionary<Type, List<IRepositoryFrameworkInterceptor>> mTypeInterceptors =
            new Dictionary<Type, List<IRepositoryFrameworkInterceptor>>();
        private List<IRepositoryFrameworkInterceptor> mInterceptors =
            new List<IRepositoryFrameworkInterceptor>();

        #region 属性
        public IEnumerable<Assembly> RepositoryAssemblies { get { return mAssembies; } }
        public ICacheKeyGenerator CacheKeyGenerator { get; set; }
        #endregion

        public RepositoryConfiguation()
        {
            CacheKeyGenerator = new DefaultCacheKeyGenerator();
            mInterceptors.Add(new DefaultRepositoryFrameworkInterceptor());
        }

        #region 公共方法 
        public void Configure()
        {
            var section = ToolSection.Instance;
            if (section == null)
                throw new ConfigurationException("缺少Projects.Tool配置节点信息");
            foreach (var node in section.TryGetNodes("repository/assembly"))
            {
                var assemblyName = node.Attributes.TryGetValue("name");
                try
                {
                    var assembly = Assembly.Load(assemblyName);
                    mAssembies.Add(assembly);
                }
                catch (Exception ex)
                {
                    throw new Exception("无法加载程序集" + assemblyName + ",Ex:" + ex.Message);
                }
            }
            LoadShardConfig(section);
            LoadClassDefines(section);
            LoadInterceptors(section);
        }

        public IShardSessionFactory GetSessionFactory(Type entityType)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IRepositoryFrameworkInterceptor> GetInterceptors(Type entityType)
        {
            throw new NotImplementedException();
        }

        public IShardStrategy GetShardStrategy(Type entityType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 注册IshardSessionFactory实例
        /// </summary>
        public void RegisterShardSessionFactory(Type entityType, Type shardSessionFactoryType)
        {
        }

        /// <summary>
        /// 注册元数据定义
        /// </summary>
        public void RegisterDefineMetaData(Assembly assembly)
        {
        }

        /// <summary>
        /// 注册指定类型的仓储拦截器
        /// </summary>
        /// <param name="entityType">拦截器类型</param>
        /// <param name="interceptor">拦截器</param>
        public void RegisterInterceptor(Type entityType, IRepositoryFrameworkInterceptor interceptor)
        {
        }

        /// <summary>
        /// 注册仓储拦截器
        /// </summary>
        /// <param name="interceptor">拦截器</param>
        public void RegisterInterceptor(IRepositoryFrameworkInterceptor interceptor)
        {
        }

        /// <summary>
        /// 注册类型的分区策略
        /// </summary>
        /// <returns></returns>
        public IShardStrategy RegisterShardStragety(Type entityType, Type shardStragegyType,
            IDictionary<string, string> attributes = null)
        {
            if (entityType == null)
                throw new ArgumentNullException("entityType");
            if (shardStragegyType == null)
                throw new ArgumentNullException("shardStragegyType");

            IShardStrategy strategy = (IShardStrategy)FastActivator.Create(shardStragegyType);
            if (strategy == null)
                throw new ArgumentNullException("strategy");

            if (strategy is AbstractShardRepository && attributes != null)
            {
                ((AbstractShardRepository)strategy).Init(attributes);
            }
            if (mStategies.ContainsKey(entityType))
                mStategies[entityType] = strategy;
            else
                mStategies.Add(entityType, strategy);
            return strategy;
        }

        #endregion

        #region 内部方法
        internal IShardSessionFactory GetSessionFactory(Type entityType, bool valid)
        {
            var factory = mSessionFactories.TryGetValue(entityType);
            while (factory == null && entityType != null)
            {
                entityType = entityType.BaseType;
                if (entityType != null)
                    factory = mSessionFactories.TryGetValue(entityType);
            }
            factory = factory ?? mDefaultSessionFactory;

            if (valid && factory == null)
                throw new Exception(String.Format("无法获取类型{0}的IShardSessionFactory对象"));
            return factory;
        }

        internal ClassDefineMetadata GetDefineMetadata(Type entityType)
        {
            if (entityType == null)
                throw new ArgumentNullException("entityType");

            var define = mMetaDatas.TryGetValue(entityType);
            if (define != null)
                return define;

            foreach (var cdm in mMetaDatas.Values)
            {
                if (cdm.EntityType.IsAssignableFrom(entityType))
                    return cdm;
            }
            return null;
        }

        #endregion

        #region 私有方法
        private void LoadShardConfig(ToolSection section)
        {
            var shardNode = section.TryGetNode("shard");
            if (shardNode != null)
            {
                var dst = shardNode.TryGetValue("defaultStrategy");
                if (!String.IsNullOrEmpty(dst))
                    mDefaultStrategyType = Type.GetType(dst);

                mDefaultAttr.Add("shard", shardNode.TryGetValue("defaultShard"));

                mDefaultSessionFactory = CreateShardSessionFactory(shardNode, "defaultSessionFactory");

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
                        strategyType = mDefaultStrategyType;

                    if (strategyType == null)
                        throw new Exception(String.Format("类型 {0} 没有定义分区策略", entityType.FullName));

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

        private void LoadClassDefines(ToolSection section)
        {
            mAssembies.ForEach(item => RegisterDefineMetaData(item));
        }

        private void LoadInterceptors(ToolSection section)
        {
            var interceptorNodes = section.TryGetNodes("interceptors/interceptor");
            foreach (var node in interceptorNodes)
            {
                var entityAttr = node.Attributes.TryGetValue("entity");
                var typeAttr = node.Attributes.TryGetValue("type");
                try
                {
                    if (String.IsNullOrEmpty(entityAttr))
                        RegisterInterceptor((IRepositoryFrameworkInterceptor)FastActivator.Create(typeAttr));
                    else
                        RegisterInterceptor(Type.GetType(entityAttr),
                            (IRepositoryFrameworkInterceptor)FastActivator.Create(typeAttr));
                }
                catch (Exception ex)
                {
                    throw new Exception(String.Format("LOAD INTERCEPTOR ERROR! EX:{0}",ex.Message));
                }
            }
        }

        private IShardStrategy CreateShardStategy(SettingNode node, string path)
        {
            var type = node.TryGetValue(path);
            if (String.IsNullOrEmpty(type))
                return null;

            IShardStrategy strategy = (IShardStrategy)FastActivator.Create(type);
            if (strategy != null && strategy is AbstractShardStrategy)
            {
                ((AbstractShardStrategy)strategy).Init(node.Attributes);
            }
            return strategy;
        }

        private IShardSessionFactory CreateShardSessionFactory(SettingNode node, string path)
        {
            var factoryTypeString = node.TryGetValue(path);
            if (String.IsNullOrEmpty(factoryTypeString))
                return null;

            Type factoryType = Type.GetType(factoryTypeString);
            return CreateShardSessionFactory(factoryType);
        }

        private IShardSessionFactory CreateShardSessionFactory(Type factoryType)
        {
            var factory = mFactoryInstances.TryGetValue(factoryType);
            if (factory == null)
            {
                factory = (IShardSessionFactory)FastActivator.Create(factoryType);
                if (factory == null)
                    throw new ArgumentNullException("factory");
                mFactoryInstances.Add(factoryType, factory);
            }
            return factory;
        }
        #endregion
    }
}
