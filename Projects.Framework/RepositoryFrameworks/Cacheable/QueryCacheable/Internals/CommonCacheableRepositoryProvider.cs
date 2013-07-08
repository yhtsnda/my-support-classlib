using Projects.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            return RepositoryFramework.GetCacher(DefineMetadata).Get<QueryCacheData>(CacheKey);
        }

        public override void ProcessCache(IQueryTimestamp cacheData)
        {
            var cd = (QueryCacheData)cacheData;
            Invocation.ReturnValue = cd.Data;
        }

        public override bool ProcessSource()
        {
            var metadata = DefineMetadata;
            Invocation.Proceed();
            ICache cache = RepositoryFramework.GetCacher(DefineMetadata);

            object data = Invocation.ReturnValue;
            if (data != null)
            {
                cache.Set(CacheKey, new QueryCacheData()
                {
                    Data = data
                });
            }
            return data != null;
        }
    }
}
