using Castle.DynamicProxy;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.Profiler;

namespace Avalon.Framework
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

            if (metadata.IsCacheable)
            {
                var cacheKey = metadata.GetCacheKeyById(invocation.Arguments[1]);
                ICache cache = RepositoryFramework.GetCacher(metadata);
                var value = cache.Get(metadata.EntityType, cacheKey);
                if (value == null)
                {
                    // 调用源接口，并将结果写入缓存
                    using (MonitorContext.Repository(invocation.Method))
                        invocation.Proceed();

                    var entity = invocation.ReturnValue;
                    if (entity != null)
                        cache.Set(cacheKey, metadata.CloneEntity(entity));

                    if (ProfilerContext.Current.Enabled)
                        ProfilerContext.Current.Trace("platform", String.Format("missing get{1}\r\n{0}", cacheKey, entity == null ? " for null" : ""));
                }
                else
                {
                    //if (ProfilerContext.Current.Enabled)
                    //    ProfilerContext.Current.Trace("platform", String.Format("hit get\r\n{0}", cacheKey));
                    //invocation.ReturnValue = value.Convert(metadata.EntityType);
                    using (MonitorContext.Repository(invocation.Method, true)) ;
                    invocation.ReturnValue = value;
                }
            }
            else
            {
                using (MonitorContext.Repository(invocation.Method))
                    invocation.Proceed();
            }
        }
    }

}
