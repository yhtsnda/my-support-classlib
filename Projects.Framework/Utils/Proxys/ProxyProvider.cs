using Castle.DynamicProxy;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Projects.Framework
{
    internal static class ProxyProvider
    {
        static List<IProxyProvider> providers;
        static List<IProxyProvider> sourceProviders;
        static ProxyGenerator generator;
        static ConcurrentDictionary<Type, bool> dic = new ConcurrentDictionary<Type, bool>();
        static Dictionary<Type, Func<IInterceptor[], object>> factoryCache = new Dictionary<Type, Func<IInterceptor[], object>>();

        static ProxyProvider()
        {
            providers = new List<IProxyProvider>()
            {
                new EntityProxyProvider(),
                new GenericListProxyProvider(),
                new PagingResultProxyProvider()
            };
            sourceProviders = new List<IProxyProvider>()
            {
                new SourceEntityProxyProvider(),
                new GenericListProxyProvider(),
                new PagingResultProxyProvider()
            };
            generator = new ProxyGenerator();
        }

        public static ProxyGenerator Generator
        {
            get { return generator; }
        }

        internal static object GetProxy(object entity)
        {
            if (entity == null)
                return entity;

            var match = providers.FirstOrDefault(o => o.IsMatch(entity.GetType()));
            if (match != null)
                return match.Proxy(entity, GetProxy);

            return entity;
        }

        internal static object GetPoco(object entity)
        {
            if (entity == null)
                return entity;

            var match = providers.FirstOrDefault(o => o.IsMatch(entity.GetType()));
            if (match != null)
                return match.Poco(entity, GetPoco);

            return entity;
        }

        internal static void Fetch(object entity)
        {
            if (entity != null)
            {
                var match = providers.FirstOrDefault(o => o.IsMatch(entity.GetType()));
                if (match != null)
                    match.Fetch(entity, Fetch);
            }
        }

        internal static object GetSourceProxy(object entity, Action<object> entityProxyAction)
        {
            return new SourceProxyImpl(entityProxyAction).Proxy(entity);
        }

        public static object CreateEntityProxy(Type entityType)
        {
            ClassDefineMetadata metadata = RepositoryFramework.GetDefineMetadataAndCheck(entityType);
            return CreateProxy(entityType, metadata);

            //var ei = new EntityInterceptor(metadata);
            //ProxyGenerationOptions options = new ProxyGenerationOptions(new InnerHook(metadata));

            ////var t = generator.ProxyBuilder.CreateClassProxyType(entityType,new Type[0], options);
            ////
            //var proxy = generator.CreateClassProxy(entityType, options, ei);
            //ei.Inited = true;

            //return proxy;
        }

        static object CreateProxy(Type entityType, ClassDefineMetadata metadata)
        {
            Func<IInterceptor[], object> func;
            if (!(factoryCache.TryGetValue(entityType, out func)))
            {
                lock (factoryCache)
                {
                    if (!(factoryCache.TryGetValue(entityType, out func)))
                    {
                        ProxyGenerationOptions options = new ProxyGenerationOptions(new InnerHook(metadata));
                        var proxyType = generator.ProxyBuilder.CreateClassProxyType(entityType, new Type[0], options);
                        var construncor = proxyType.GetConstructor(new Type[] { typeof(IInterceptor[]) });
                        var interceptors = Expression.Parameter(typeof(IInterceptor[]), "interceptors");
                        var main = Expression.Convert(Expression.New(construncor, interceptors), typeof(object));

                        func = Expression.Lambda<Func<IInterceptor[], object>>(main, interceptors).Compile();
                        factoryCache[entityType] = func;
                    }
                }
            }
            var ei = new EntityInterceptor(metadata);
            var proxy = func(new IInterceptor[] { ei });
            ei.Inited = true;
            return proxy;
        }

        [Serializable]
        class InnerHook : IProxyGenerationHook
        {
            ClassDefineMetadata metadata;
            protected static readonly ICollection<Type> SkippedTypes = new Type[] { typeof(object), typeof(MarshalByRefObject), typeof(ContextBoundObject) };

            public InnerHook(ClassDefineMetadata metadata)
            {
                this.metadata = metadata;
            }

            public void MethodsInspected()
            {
            }

            public void NonProxyableMemberNotification(Type type, System.Reflection.MemberInfo memberInfo)
            {
            }

            // Methods
            public override bool Equals(object obj)
            {
                return ((obj != null) && (obj.GetType() == base.GetType()));
            }

            public override int GetHashCode()
            {
                return base.GetType().GetHashCode();
            }


            public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
            {

                return !SkippedTypes.Contains(methodInfo.DeclaringType) && metadata.GetClassJoinDefineMetadata(methodInfo) != null;
            }
        }


        public static bool IsProxy(object entity)
        {
            return entity != null && entity as IProxyTargetAccessor != null;
        }

        public static bool IsProxyChanged(object entity)
        {
            var m = entity as IProxyTargetAccessor;
            if (m == null)
                return false;

            return ((EntityInterceptor)m.GetInterceptors()[0]).JoinChanged;
        }

        class SourceProxyImpl
        {
            Action<object> entityProxyAction;

            public SourceProxyImpl(Action<object> entityProxyAction)
            {
                this.entityProxyAction = entityProxyAction;
            }

            public object Proxy(object entity)
            {
                if (entity == null)
                    return entity;

                var match = sourceProviders.FirstOrDefault(o => o.IsMatch(entity.GetType()));
                if (match != null)
                {
                    var proxy = match.Proxy(entity, Proxy);
                    if (match is SourceEntityProxyProvider)
                        entityProxyAction(proxy);

                    return proxy;
                }

                return entity;
            }
        }
    }
}
