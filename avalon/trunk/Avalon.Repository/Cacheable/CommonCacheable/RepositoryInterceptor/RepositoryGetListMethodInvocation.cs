using Castle.DynamicProxy;
using Avalon.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.Profiler;

namespace Avalon.Framework
{
    internal class RepositoryGetListMethodInvocation : IRepositoryMethodInvocation
    {
        public bool IsMatch(IInvocation invocation)
        {
            var method = invocation.Method.Name;
            return method == "GetList" && invocation.Arguments.Count() == 2 && invocation.Method.GetParameters()[0].ParameterType == typeof(ShardParams);
        }

        public void Process(IInvocation invocation, ClassDefineMetadata metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException("metadata");

            if (metadata.IsCacheable)
            {
                IEnumerable ids = (IEnumerable)invocation.Arguments[1];

                var cache = RepositoryFramework.GetCacher(metadata);

                var idKeys = ids.Cast<object>().Distinct().Select(o => new IdKey(o, metadata.GetCacheKeyById(o)));

                //访问缓存并获取不在缓存中的 CacheKey 
                IEnumerable<string> missing;

                var cacheItems = cache.GetBatch(metadata.EntityType, idKeys.Select(o => o.Key), out missing).Cast<object>().ToList();

                if (missing.Count() > 0)
                {
                    var missIds = missing.Select(o => idKeys.First(ik => ik.Key == o).Id);
                    invocation.SetArgumentValue(1, missIds);

                    using (MonitorContext.Repository(invocation.Method))
                        invocation.Proceed();

                    var sourceItems = ((IEnumerable)invocation.ReturnValue).Cast<object>();

                    cacheItems.AddRange(sourceItems);

                    if (sourceItems.Count() > 0)
                    {
                        //加入缓存
                        cache.SetBatch(sourceItems.Select(o => new CacheItem(metadata.GetCacheKey(o), metadata.CloneEntity(o))), 1200);
                    }
                    else
                    {
                        if (ProfilerContext.Current.Enabled)
                            ProfilerContext.Current.Trace("platform", String.Format("missing get list for null\r\n{0}", String.Join(",", missing)));
                    }
                }
                else
                {
                    using (MonitorContext.Repository(invocation.Method, true)) ;
                }

                var ta = TypeAccessor.GetAccessor(metadata.EntityType);
                var idGetter = ta.GetPropertyGetter(metadata.IdMember.Name);

                //按ID排序并进行类型转化
                invocation.ReturnValue = cacheItems
                    .OrderBy(entity => idGetter(entity), ids.Cast<object>())
                    .Cast(metadata.EntityType);
            }
            else
            {
                using (MonitorContext.Repository(invocation.Method))
                    invocation.Proceed();
            }
        }

        class IdKey
        {
            public IdKey(object id, string key)
            {
                Id = id;
                Key = key;
            }
            public object Id;
            public string Key;
        }
    }
}
