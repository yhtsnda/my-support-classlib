using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Tool;

namespace Projects.Framework
{
    internal class CommonCacheableRepositoryProvider<TEntity> : AbstractCacheableRepositoryProvider<TEntity>
         where TEntity : class
    {
        public override bool IsMatch()
        {
            return true;
        }

        public override IQueryTimestamp GetCacheData()
        {
            return CacheManager.GetCacher(CacheMetadata.EntityType).Get<QueryCacheData>(CacheKey);
        }

        public override void ProcessCache(IQueryTimestamp cacheData)
        {
            var cd = (QueryCacheData)cacheData;
            Invocation.ReturnValue = cd.Data;
        }

        public override bool ProcessSource()
        {
            var metadata = CacheMetadata;
            Invocation.Proceed();
            ICache cache = CacheManager.GetCacher(CacheMetadata.EntityType);

            object data = Invocation.ReturnValue;
            if (data != null)
            {
                cache.Set(CacheKey, new QueryCacheData()
                {
                    Data = data
                });
            }
            return true;
        }
    }
}
