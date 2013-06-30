﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Tool;
using System.Reflection;
using Projects.Tool.Reflection;
using Projects.Tool.Shards;
using Projects.Framework.Shards;
using Castle.DynamicProxy;
using Projects.Tool.Util;

namespace Projects.Framework
{
    /// <summary>
    /// 仓储框架入口（静态）
    /// </summary>
    public static class RepositoryFramework
    {
        static FrameworkConfiguation shardConfiguation;

        static RepositoryFramework()
        {
            shardConfiguation = new FrameworkConfiguation();
        }

        internal static FrameworkConfiguation ShardConfiguation
        {
            get { return shardConfiguation; }
        }

        /// <summary>
        /// 方法调用的缓存键生成器
        /// </summary>
        public static ICacheKeyGenerator CacheKeyGenerator
        {
            get { return shardConfiguation.CacheKeyGenerator; }
            set { shardConfiguation.CacheKeyGenerator = value; }
        }

        /// <summary>
        /// 仓储相关的程序集
        /// </summary>
        public static IEnumerable<Assembly> RepositoryAssemblies
        {
            get { return shardConfiguation.RepositoryAssemblies; }
        }

        /// <summary>
        /// 进行配置的初始化
        /// </summary>
        public static void Configure(IDependencyRegister register)
        {
            shardConfiguation.Configure(register);
        }

        /// <summary>
        /// 获取指定类型的 IShardSessionFactory 实例
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static IShardSessionFactory GetSessionFactory(Type entityType)
        {
            return shardConfiguation.GetSessionFactory(entityType);
        }

        /// <summary>
        /// 注册类型的 IShardSessionFactory 实例
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="shardSessionFactoryType"></param>
        public static void RegisterShardSessionFactory(Type entityType, Type shardSessionFactoryType)
        {
            shardConfiguation.RegisterShardSessionFactory(entityType, shardSessionFactoryType);
        }

        /// <summary>
        /// 注册元数据定义
        /// </summary>
        /// <param name="assembly"></param>
        public static void RegisterDefineMetadata(Assembly assembly)
        {
            shardConfiguation.RegisterDefineMetadata(assembly);
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
            return shardConfiguation.GetDefineMetadata(entityType);
        }

        internal static ClassDefineMetadata GetDefineMetadataAndCheck(Type entityType)
        {
            var metadata = GetDefineMetadata(entityType);
            if (metadata == null)
                throw new PlatformException("无法获取对象的定义 {0}", entityType.FullName);

            return metadata;
        }

        /// <summary>
        /// 获取指定类型的的仓储拦截器
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static IEnumerable<IRepositoryFrameworkInterceptor> GetInterceptors(Type entityType)
        {
            return shardConfiguation.GetInterceptors(entityType);
        }

        /// <summary>
        /// 注册指定类型的仓储拦截器
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="interceptor"></param>
        public static void RegisterInterceptor(Type entityType, IRepositoryFrameworkInterceptor interceptor)
        {
            shardConfiguation.RegisterInterceptor(entityType, interceptor);
        }

        /// <summary>
        /// 注册仓储拦截器
        /// </summary>
        /// <param name="interceptor"></param>
        public static void RegisterInterceptor(IRepositoryFrameworkInterceptor interceptor)
        {
            shardConfiguation.RegisterInterceptor(interceptor);
        }

        /// <summary>
        /// 获取指定类型的分区策略
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static IShardStrategy GetShardStrategy(Type entityType)
        {
            return shardConfiguation.GetShardStrategy(entityType);
        }

        /// <summary>
        /// 注册类型的分区策略
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="shardStragegyType"></param>
        /// <param name="attributes"></param>
        public static void RegisterShardStragety(Type entityType, Type shardStragegyType, IDictionary<string, string> attributes = null)
        {
            shardConfiguation.RegisterShardStragety(entityType, shardStragegyType, attributes);
        }

        /// <summary>
        /// 创建指定类型的规约对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static ISpecification<TEntity> CreateSpecification<TEntity>()
        {
            return shardConfiguation.GetSessionFactory(typeof(TEntity), true).SpecificationProvider.CreateSpecification<TEntity>();
        }

        /// <summary>
        /// 打开一个 IShardSession 实例
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="shardParams"></param>
        /// <returns></returns>
        public static IShardSession<TEntity> OpenSession<TEntity>(ShardParams shardParams)
        {
            return shardConfiguation.GetSessionFactory(typeof(TEntity), true).OpenSession<TEntity>(shardParams);
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
                throw new PlatformException("无法获取对象的定义 {0}", entity.GetType().FullName);

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

        public static T GetProxy<T>(T entity) where T : class
        {
            return (T)ProxyProvider.GetProxy(entity);
        }

        public static void FetchObject<T>(T entity) where T : class
        {
            ProxyProvider.Fetch(entity);
        }

        internal static object Raise(object entity, RaiseType raiseType, UniqueRaise ur = null)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            var metadata = GetDefineMetadata(entity.GetType());
            if (metadata == null)
                throw new PlatformException("无法获取对象的定义 {0}", entity.GetType().FullName);

            var interceptors = RepositoryFramework.GetInterceptors(metadata.EntityType);

            entity = ProxyProvider.GetProxy(entity);

            if (ur == null)
                ur = new UniqueRaise(raiseType, metadata, entity, true);

            if (ur.NotRaised)
            {
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

            if (ur.NotRaised && ur.AutoDisposed)
                ur.Dispose();

            return entity;
        }

        internal class UniqueRaise : IDisposable
        {
            const string UniqueRaiseKey = "_UniqueRaiseKey_";
            string key = null;

            public bool NotRaised
            {
                get;
                private set;
            }

            public bool AutoDisposed
            {
                get;
                private set;
            }

            public UniqueRaise(RaiseType raiseType, ClassDefineMetadata metadata, object entity, bool autoDisposed)
            {
                var pa = PropertyAccessorFactory.GetPropertyAccess(metadata.EntityType);
                key = String.Format("{0}:{1}:{2}", metadata.EntityType.FullName, pa.GetGetter(metadata.IdMember.Name).Get(entity), raiseType);
                NotRaised = !WorkbenchUtil<string, bool>.GetValue(UniqueRaiseKey, key);
                if (NotRaised)
                    WorkbenchUtil<string, bool>.SetValue(UniqueRaiseKey, key, true);

                AutoDisposed = autoDisposed;
            }

            public void Dispose()
            {
                if (NotRaised)
                    WorkbenchUtil<string, bool>.SetValue(UniqueRaiseKey, key, false);
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

        internal static ICache GetCacher(string name)
        {
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

        internal enum RaiseType
        {
            Unknown,
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