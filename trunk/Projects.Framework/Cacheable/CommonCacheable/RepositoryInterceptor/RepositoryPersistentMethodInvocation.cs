using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.DynamicProxy;

using Projects.Tool;
using Projects.Tool.Util;

namespace Projects.Framework
{
    internal class RepositoryPersistentMethodInvocation : IRepositoryMethodInvocation
    {
        private const string LOOP_DETECT_KEY = "_LoopDetectKey_";

        public bool IsMatch(IInvocation invocation)
        {
            var method = invocation.Method.Name;
            return method == "Create" || method == "Update" || method == "Delete";
        }

        public void Process(IInvocation invocation, ClassDefineMetadata metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException("metadata");

            var entity = invocation.Arguments[0];
            if (entity == null)
                throw new ArgumentNullException("entity");

            var interceptors = RepositoryFramework.GetInterceptors(metadata.EntityType);

            InvokePreEvent(interceptors, invocation, RepositoryFramework.ProxyEntity(entity), metadata);

            // 获取原生的对象
            var original = GetOriginalObject(entity);
            invocation.SetArgumentValue(0, original);
            invocation.Proceed();

            var pa = PropertyAccessorFactory.GetPropertyAccess(metadata.EntityType);
            pa.SetPropertyValues(entity, pa.GetPropertyValues(original));

            // 如果启用缓存，则通知过期
            if (metadata.IsCacheable)
            {
                var cacheKey = metadata.GetCacheKey(entity);
                ICache cache = RepositoryFramework.GetCacher(metadata);
                cache.Remove(cacheKey);
            }

            // 更新依赖缓存的时间戳
            var depends = metadata.GetCacheRegionKeys(invocation.Arguments[0]);
            if (depends.Count > 0)
            {
                var time = NetworkTime.Now.Ticks;
                ICache dependCache = RepositoryFramework.GetCacher(metadata, "cache:depend");
                dependCache.SetBatch(depends.Select(o => new CacheItem<long>(o, time)), DateTime.MaxValue);
            }

            //invoke post event
            InvokePostEvent(interceptors, invocation, RepositoryFramework.ProxyEntity(entity), metadata);
        }

        private object GetOriginalObject(object entity)
        {
            if (entity == null)
                return entity;

            var entityType = entity.GetType();
            if (entityType.IsValueType)
                return entity;

            var meta = RepositoryFramework.GetDefineMetadata(entityType);

            if (meta != null)
            {
                entityType = meta.EntityType;
                var original = Projects.Tool.Reflection.FastActivator.Create(entityType);
                var pa = PropertyAccessorFactory.GetPropertyAccess(entityType);
                var datas = pa.GetPropertyValues(entity);
                for (int i = 0; i < datas.Length; i++)
                {
                    datas[i] = GetOriginalObject(datas[i]);
                }
                pa.SetPropertyValues(original, datas);
                return original;
            }
            return entity;
        }

        private void InvokePreEvent(IEnumerable<IRepositoryFrameworkInterceptor> interceptors, 
            IInvocation invocation, object entity, ClassDefineMetadata metadata)
        {
            foreach (var interceptor in interceptors)
            {
                switch (invocation.Method.Name)
                {
                    case "Create":
                        interceptor.PreCreate(entity);
                        break;
                    case "Update":
                        using (var ld = new LoopDetect(interceptor, metadata, invocation.Method.Name, entity))
                        {
                            interceptor.PreUpdate(entity);
                        }
                        break;
                    case "Delete":
                        interceptor.PreDelete(entity);
                        break;
                }
            }
        }

        private void InvokePostEvent(IEnumerable<IRepositoryFrameworkInterceptor> interceptors, 
            IInvocation invocation, object entity, ClassDefineMetadata metadata)
        {
            foreach (var interceptor in interceptors)
            {
                switch (invocation.Method.Name)
                {
                    case "Create":
                        interceptor.PostCreate(entity);
                        break;
                    case "Update":
                        using (var ld = new LoopDetect(interceptor, metadata, invocation.Method.Name, entity))
                        {
                            interceptor.PostUpdate(entity);
                        }
                        break;
                    case "Delete":
                        interceptor.PostDelete(entity);
                        break;
                }
            }
        }
        
        /// <summary>
        /// 循环拦截的检测
        /// </summary>
        private class LoopDetect : IDisposable
        {
            string key;

            public LoopDetect(IRepositoryFrameworkInterceptor interceptor, 
                ClassDefineMetadata metadata, string operate, object entity)
            {
                var pa = PropertyAccessorFactory.GetPropertyAccess(entity.GetType());
                key = entity.GetType().FullName + ":" + 
                    pa.GetGetter(metadata.IdMember.Name).Get(entity).ToString() + ":" + operate;
                var v = WorkbenchUtil<string, bool>.GetValue(LOOP_DETECT_KEY, key);
                if (v)
                    throw new Exception(String.Format("拦截器存在循环拦截的过程，请检查拦截器中更新对象相关的代码。\r\ninterceptor: {0}", interceptor.GetType().FullName));

                WorkbenchUtil<string, bool>.SetValue(LOOP_DETECT_KEY, key, true);
            }

            public void Dispose()
            {
                WorkbenchUtil<string, bool>.SetValue(LOOP_DETECT_KEY, key, false);
            }
        }
    }
}
