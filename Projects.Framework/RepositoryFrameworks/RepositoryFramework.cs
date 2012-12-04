using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Castle.DynamicProxy;

using Projects.Tool;
using Projects.Tool.Util;
using Projects.Framework.Shards;

namespace Projects.Framework
{
    public static class RepositoryFramework
    {
        private static object mSyncRoot = new object();
        static RepositoryConfiguation mmShardConfiguration;

        internal static RepositoryConfiguation mShardConfiguration
        {
            get { return mmShardConfiguration; }
        }

        /// <summary>
        /// 方法调用的缓存键生成器
        /// </summary>
        public static ICacheKeyGenerator CacheKeyGenerator
        {
            get { return mmShardConfiguration.CacheKeyGenerator; }
            set { mmShardConfiguration.CacheKeyGenerator = value; }
        }

        static RepositoryFramework()
        {
            mmShardConfiguration = new RepositoryConfiguation();
        }

        /// <summary>
        /// 进行配置的初始化
        /// </summary>
        public static void Configure()
        {
            mShardConfiguration.Configure();
        }

        /// <summary>
        /// 获取指定类型的 IShardSessionFactory 实例
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static IShardSessionFactory GetSessionFactory(Type entityType)
        {
            return mShardConfiguration.GetSessionFactory(entityType);
        }

        /// <summary>
        /// 注册类型的 IShardSessionFactory 实例
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="shardSessionFactoryType"></param>
        public static void RegisterShardSessionFactory(Type entityType, Type shardSessionFactoryType)
        {
            mShardConfiguration.RegisterShardSessionFactory(entityType, shardSessionFactoryType);
        }

        /// <summary>
        /// 注册元数据定义
        /// </summary>
        /// <param name="assembly"></param>
        public static void RegisterDefineMetadata(Assembly assembly)
        {
            mShardConfiguration.RegisterDefineMetaData(assembly);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyStrings"></param>
        public static void RegisterDefineMetadata(IEnumerable<string> assemblyStrings)
        {
            foreach (var assemblyString in assemblyStrings)
            {
                try
                {
                    var assembly = Assembly.Load(assemblyString);
                    RegisterDefineMetadata(assembly);
                }
                catch (Exception ex)
                {
                }
            }
        }

        /// <summary>
        /// 获取对象的元数据定义
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static ClassDefineMetadata GetDefineMetadata(Type entityType)
        {
            return mShardConfiguration.GetDefineMetadata(entityType);
        }

        /// <summary>
        /// 获取指定类型的的仓储拦截器
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static IEnumerable<IRepositoryFrameworkInterceptor> GetInterceptors(Type entityType)
        {
            return mShardConfiguration.GetInterceptors(entityType);
        }

        /// <summary>
        /// 注册指定类型的仓储拦截器
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="interceptor"></param>
        public static void RegisterInterceptor(Type entityType, IRepositoryFrameworkInterceptor interceptor)
        {
            mShardConfiguration.RegisterInterceptor(entityType, interceptor);
        }

        /// <summary>
        /// 注册仓储拦截器
        /// </summary>
        /// <param name="interceptor"></param>
        public static void RegisterInterceptor(IRepositoryFrameworkInterceptor interceptor)
        {
            mShardConfiguration.RegisterInterceptor(interceptor);
        }

        /// <summary>
        /// 获取指定类型的分区策略
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static IShardStrategy GetShardStrategy(Type entityType)
        {
            return mShardConfiguration.GetShardStrategy(entityType);
        }

        /// <summary>
        /// 注册类型的分区策略
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="shardStragegyType"></param>
        /// <param name="attributes"></param>
        public static void RegisterShardStragety(Type entityType, Type shardStragegyType, IDictionary<string, string> attributes = null)
        {
            mShardConfiguration.RegisterShardStragety(entityType, shardStragegyType, attributes);
        }

        /// <summary>
        /// 创建指定类型的规约对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static ISpecification<TEntity> CreateSpecification<TEntity>()
        {
            return mShardConfiguration.GetSessionFactory(typeof(TEntity), true).SpecificationProvider.CreateSpecification<TEntity>();
        }

        /// <summary>
        /// 打开一个 IShardSession 实例
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="shardParams"></param>
        /// <returns></returns>
        public static IShardSession<TEntity> OpenSession<TEntity>(ShardParams shardParams)
        {
            return mShardConfiguration.GetSessionFactory(typeof(TEntity), true).OpenSession<TEntity>(shardParams);
        }

        /// <summary>
        /// 引发对象的 PreCreate 事件
        /// </summary>
        public static void RaisePreCreate(object entity)
        {
            Raise(entity, RaiseType.PreCreate);
        }

        /// <summary>
        /// 引发对象的 PreUpdate 事件
        /// </summary>
        public static void RaisePreUpdate(object entity)
        {
            Raise(entity, RaiseType.PreUpdate);
        }

        /// <summary>
        /// 引发对象的 PreDelete 事件
        /// </summary>
        public static void RaisePreDelete(object entity)
        {
            Raise(entity, RaiseType.PreDelete);
        }

        /// <summary>
        /// 引发对象的 PostCreate 事件
        /// </summary>
        public static void RaisePostCreate(object entity)
        {
            Raise(entity, RaiseType.PostCreate);
        }

        /// <summary>
        /// 引发对象的 PostUpdate 事件
        /// </summary>
        public static void RaisePostUpdate(object entity)
        {
            Raise(entity, RaiseType.PostUpdate);
        }

        /// <summary>
        /// 引发对象的 PostDelere 事件
        /// </summary>
        public static void RaisePostDelere(object entity)
        {
            Raise(entity, RaiseType.PostDelete);
        }

        /// <summary>
        /// 引发对象的 PostLoad 事件
        /// </summary>
        public static void RaisePostLoad(object entity)
        {
            Raise(entity, RaiseType.PostLoad);
        }

        /// <summary>
        /// 引发数据变更的操作，通过改操作可以进行缓存及缓存依赖的通知
        /// </summary>
        /// <param name="entity"></param>
        public static void RaisePersistent(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            var metadata = GetDefineMetadata(entity.GetType());
            if (metadata == null)
                throw new Exception(String.Format("无法获取对象的定义 {0}", entity.GetType().FullName));

            // 如果启用缓存，则通知过期
            if (metadata.IsCacheable)
            {
                var cacheKey = metadata.GetCacheKey(entity);
                ICache cache = GetCacher(metadata);
                cache.Remove(cacheKey);
            }

            // 更新依赖缓存的时间戳
            var depends = metadata.GetCacheRegionKeys(entity);
            if (depends.Count > 0)
            {
                var time = NetworkTime.Now.Ticks;
                ICache dependCache = RepositoryFramework.GetCacher(metadata, "cache:depend");
                dependCache.SetBatch(depends.Select(o => new CacheItem<long>(o, time)), DateTime.MaxValue);
            }
        }

        public static T ProxyEntity<T>(T entity)
        {
            if (entity as IProxyTargetAccessor != null)
                return entity;

            T proxyObject = entity;
            var metadata = GetDefineMetadata(entity.GetType());
            if (metadata != null && metadata.ClassJoinDefines.Count > 0 && metadata.EntityType == entity.GetType())
            {
                proxyObject = (T)ProxyGeneratorUtil.Instance.CreateClassProxy(metadata.EntityType, new EntityInterceptor());

                var pa = PropertyAccessorFactory.GetPropertyAccess(metadata.EntityType);
                var datas = pa.GetPropertyValues(entity);
                pa.SetPropertyValues(proxyObject, datas);
            }
            return proxyObject;
        }

        static void Raise(object entity, RaiseType raiseType)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            var metadata = GetDefineMetadata(entity.GetType());
            if (metadata == null)
                throw new Exception(String.Format("无法获取对象的定义 {0}", entity.GetType().FullName));

            var interceptors = RepositoryFramework.GetInterceptors(metadata.EntityType);

            foreach (var interceptor in interceptors)
            {
                switch (raiseType)
                {
                    case RaiseType.PreCreate:
                        interceptor.PreCreate(entity);
                        break;
                    case RaiseType.PreUpdate:
                        interceptor.PreUpdate(entity);
                        break;
                    case RaiseType.PreDelete:
                        interceptor.PreDelete(entity);
                        break;
                    case RaiseType.PostCreate:
                        interceptor.PostCreate(entity);
                        break;
                    case RaiseType.PostUpdate:
                        interceptor.PostUpdate(entity);
                        break;
                    case RaiseType.PostDelete:
                        interceptor.PostDelete(entity);
                        break;
                    case RaiseType.PostLoad:
                        interceptor.PostLoad(entity);
                        break;
                }
            }
        }

        internal static ICache GetCacher(ClassDefineMetadata metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException("metadata");

            var cache = CacheManager.GetCacher(metadata.EntityType);
            if (cache is HttpContextCache)
                return cache;

            return new SecondaryCacheEx(cache);
        }

        internal static ICache GetCacher(ClassDefineMetadata metadata, string name)
        {
            if (metadata == null)
                throw new ArgumentNullException("metadata");
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            var cache = CacheManager.GetCacher(name);
            if (cache is HttpContextCache)
                return cache;

            return new SecondaryCacheEx(cache);
        }

        class SecondaryCacheEx : SecondaryCache
        {
            public SecondaryCacheEx(ICache cache)
            {
                FirstCache = new HttpContextCache();
                SecondCache = cache;
            }

            protected override T GetInner<T>(string key)
            {
                return base.GetInner<T>(key);
            }
        }

        enum RaiseType
        {
            PreCreate,
            PreUpdate,
            PreDelete,
            PostCreate,
            PostUpdate,
            PostDelete,
            PostLoad
        }
    }
}
