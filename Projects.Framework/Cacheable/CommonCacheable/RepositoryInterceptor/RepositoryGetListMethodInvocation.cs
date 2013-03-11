using Castle.DynamicProxy;
using Projects.Tool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
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

            //var persister = PersisterImpl.CreatePersister(invocation, metadata);
            //IEnumerable ids = (IEnumerable)invocation.Arguments[1];
            //DataWrapper wrapper = new DataWrapper(metadata, ids);

            //var items = persister.GetList(wrapper).Select(o => o.Data).Cast(metadata.EntityType);
            //invocation.ReturnValue = items;

            if (metadata.IsCacheable)
            {
                IEnumerable ids = (IEnumerable)invocation.Arguments[1];

                var cache = RepositoryFramework.GetCacher(metadata);

                var idKeys = ids.Cast<object>().Distinct().Select(o => new IdKey(o, metadata.GetCacheKeyById(o)));

                //访问缓存并获取不在缓存中的 CacheKey 
                IEnumerable<string> missing;
                var cacheItems = cache.GetBatch<CacheData>(idKeys.Select(o => o.Key), out missing).Select(o => o.Convert(metadata.EntityType)).ToList();

                if (missing.Count() > 0)
                {
                    var missIds = missing.Select(o => idKeys.First(ik => ik.Key == o).Id);
                    invocation.SetArgumentValue(1, missIds);
                    invocation.Proceed();
                    var sourceItems = ((IEnumerable)invocation.ReturnValue).Cast<object>();

                    cacheItems.AddRange(sourceItems);

                    if (sourceItems.Count() > 0)
                    {
                        //加入缓存
                        cache.SetBatch(sourceItems.Select(o => new CacheItem<CacheData>(metadata.GetCacheKey(o), CacheData.FromEntity(o))), 1200);
                    }
                    else
                    {
                        if (ProfilerContext.Current.Enabled)
                            ProfilerContext.Current.Trace("platform", String.Format("missing get list for null\r\n{0}", String.Join(",", missing)));
                    }
                }
                var pa = PropertyAccessorFactory.GetPropertyAccess(metadata.EntityType);
                var getter = pa.GetGetter(metadata.IdMember.Name);

                //按ID排序并进行类型转化
                invocation.ReturnValue = cacheItems
                    .OrderBy(entity => getter.Get(entity), ids.Cast<object>())
                    .Cast(metadata.EntityType);
            }
            else
            {
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
