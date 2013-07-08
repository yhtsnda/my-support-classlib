using Castle.DynamicProxy;
using Projects.Tool.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Projects.Framework.Proxys
{
    internal static class ProxyProvider
    {
        static ProxyGenerator generator;
        static Dictionary<Type, Func<IInterceptor[], object>> factoryCache = new Dictionary<Type, Func<IInterceptor[], object>>();

        static ProxyProvider()
        {

            generator = new ProxyGenerator();
        }

        public static ProxyGenerator Generator
        {
            get { return generator; }
        }

        public static void Fetch(object entity)
        {
            if (entity == null)
                return;

            var type = entity.GetType();
            var metadata = RepositoryFramework.GetDefineMetadata(type);
            if (metadata != null)
            {
                if (metadata.FetchableObject != null)
                {
                    metadata.FetchableObject.Fetch(entity);
                    Fetch(entity, metadata.FetchableObject.CheckFetchCancel);
                }
                else
                {
                    Fetch(entity, null);
                }
            }
        }

        static void Fetch(object entity, Func<object, bool> cancelFetchFunc)
        {
            if (entity == null)
                return;

            var type = entity.GetType();
            var metadata = RepositoryFramework.GetDefineMetadata(type);
            if (metadata != null)
            {
                if (metadata.FetchableObject != null)
                    (metadata.FetchableObject).Fetch(entity);

                var joins = metadata.ClassJoinDefines.Values.Cast<ClassJoinDefineMetadata>().Where(o => o.JoinType == MethodJoinType.PropertyGet);
                var ta = TypeAccessor.GetAccessor(metadata.EntityType);

                foreach (var join in joins)
                {
                    var value = ta.GetProperty(join.JoinName, entity);

                    if (cancelFetchFunc != null && cancelFetchFunc(value))
                        return;

                    var entityType = join.Method.ReturnType;

                    if (EntityUtil.IsGenericList(entityType))
                    {
                        var items = (IList)value;
                        for (var i = 0; i < items.Count; i++)
                        {
                            var v = items[i];
                            var sv = GetSource(v);
                            Fetch(sv, cancelFetchFunc);

                            if (sv != v)
                                items[i] = sv;
                        }
                    }
                    else
                    {
                        var sv = GetSource(value);
                        Fetch(sv, cancelFetchFunc);

                        if (value != sv)
                            ta.SetProperty(join.JoinName, entity, sv);
                    }
                }
            }
        }

        public static Func<IInterceptor[], object> GetCreateFunc(Type entityType)
        {
            Func<IInterceptor[], object> func;
            if (!(factoryCache.TryGetValue(entityType, out func)))
            {
                lock (factoryCache)
                {
                    if (!(factoryCache.TryGetValue(entityType, out func)))
                    {
                        ProxyGenerationOptions options = new ProxyGenerationOptions();
                        var proxyType = generator.ProxyBuilder.CreateClassProxyType(entityType, new Type[] { typeof(IProxy) }, options);
                        var construncor = proxyType.GetConstructor(new Type[] { typeof(IInterceptor[]) });
                        var interceptors = Expression.Parameter(typeof(IInterceptor[]), "interceptors");
                        var main = Expression.Convert(Expression.New(construncor, interceptors), typeof(object));

                        func = Expression.Lambda<Func<IInterceptor[], object>>(main, interceptors).Compile();
                        factoryCache[entityType] = func;
                    }
                }
            }
            return func;
        }

        public static bool IsProxy(object entity)
        {
            return entity is IProxy;
        }

        public static object GetSource(object entity)
        {
            if (IsProxy(entity))
                return ((IProxy)entity).GetSource();

            return entity;
        }

        internal static void ProxyJoins(object entity)
        {
            ClassDefineMetadata metadata = RepositoryFramework.GetDefineMetadataAndCheck(entity.GetType());

            var joins = metadata.ClassJoinDefines.Values.Cast<ClassJoinDefineMetadata>().Where(o => o.JoinType == MethodJoinType.PropertyGet);
            var ta = TypeAccessor.GetAccessor(metadata.EntityType);

            foreach (var join in joins)
            {
                var value = ta.GetProperty(join.JoinName, entity);

                //已经为代理对象，直接忽略
                if (IsProxy(value))
                {
                    ((IProxy)value).Reset();
                    continue;
                }

                var type = join.Method.ReturnType;

                if (EntityUtil.IsGenericList(type))
                {
                    var itemType = type.GetGenericArguments()[0];
                    var proxyType = typeof(ProxyCollection<>).MakeGenericType(itemType);
                    IProxyCollection proxy = (IProxyCollection)FastActivator.Create(proxyType);
                    proxy.Init(entity, join);

                    ta.SetProperty(join.JoinName, entity, proxy);
                }
                else
                {
                    var ei = new EntityProxyInterceptor(entity, join);
                    var proxy = ProxyProvider.GetCreateFunc(type)(new IInterceptor[] { ei });
                    ei.IsConstructed = true;
                    ta.SetProperty(join.JoinName, entity, proxy);
                }
            }
        }

        internal static void Proxy(object instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            var type = instance.GetType();

            if (EntityUtil.IsPagingResult(type))
            {
                var ta = TypeAccessor.GetAccessor(type);
                IEnumerable items = (IEnumerable)ta.GetProperty("Items", instance);
                foreach (var item in items)
                {
                    Proxy(item);
                }
            }
            else if (EntityUtil.IsGenericList(type))
            {
                IEnumerable items = (IEnumerable)instance;
                foreach (var item in items)
                {
                    Proxy(item);
                }
            }
            else if (RepositoryFramework.GetDefineMetadata(type) != null)
            {
                ProxyJoins(instance);
            }
        }
    }
}
