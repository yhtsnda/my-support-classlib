using Castle.DynamicProxy;
using Projects.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    internal class RepositoryGetMethodInvocation : IRepositoryMethodInvocation
    {
        public bool IsMatch(IInvocation invocation)
        {
            var method = invocation.Method.Name;
            return method == "Get" && invocation.Arguments.Length == 2 && invocation.Method.GetParameters()[0].ParameterType == typeof(ShardParams);
        }

        public void Process(IInvocation invocation, ClassDefineMetadata metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException("metadata");

            //var persister = PersisterImpl.CreatePersister(invocation, metadata);
            //var cacheKey = metadata.GetCacheKeyById(invocation.Arguments[1]);
            //invocation.ReturnValue = persister.Get(cacheKey);

            if (metadata.IsCacheable)
            {
                var cacheKey = metadata.GetCacheKeyById(invocation.Arguments[1]);
                ICache cache = RepositoryFramework.GetCacher(metadata);
                CacheData value = cache.Get<CacheData>(cacheKey);
                if (value == null)
                {
                    // 调用源接口，并将结果写入缓存
                    invocation.Proceed();
                    var entity = invocation.ReturnValue;
                    if (entity != null)
                        cache.Set(cacheKey, CacheData.FromEntity(entity));

                    if (ProfilerContext.Current.Enabled)
                        ProfilerContext.Current.Trace("platform", String.Format("missing get{1}\r\n{0}", cacheKey, entity == null ? " for null" : ""));
                }
                else
                {
                    //if (ProfilerContext.Current.Enabled)
                    //    ProfilerContext.Current.Trace("platform", String.Format("hit get\r\n{0}", cacheKey));
                    invocation.ReturnValue = value.Convert(metadata.EntityType);
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
    }

}
