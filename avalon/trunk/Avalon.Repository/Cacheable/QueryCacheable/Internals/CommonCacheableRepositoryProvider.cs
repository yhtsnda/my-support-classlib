using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
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
            return RepositoryFramework.GetCacherForQuery(DefineMetadata).Get<QueryCacheData>(CacheKey);
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

            ICache queryCache = RepositoryFramework.GetCacherForQuery(metadata);
            ICache cache = RepositoryFramework.GetCacher(DefineMetadata);

            object data = Invocation.ReturnValue;

            //if (data != null)
            //{
            //    cache.Set(CacheKey, new QueryCacheData()
            //    {
            //        Data = data
            //    });
            //}
            //return data != null;

            queryCache.Set(CacheKey, new QueryCacheData()
            {
                Data = data
            });
            return true;
        }
    }
}
