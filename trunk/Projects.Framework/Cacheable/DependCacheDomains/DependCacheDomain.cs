using Projects.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{

    public static class DependCacheDomain
    {
        public static DependCacheDomain<TEntity, TKey> CreateSingleKey<TEntity, TKey>(Func<TEntity, TKey> entityKeySelector, Func<TKey, TEntity> missingItemHandler, Func<IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler, string cacheName = null, string cacheKeyFormat = null, int secondesToLive = 0, bool contextCacheEnabled = true)
            where TEntity : IQueryTimestamp
        {
            return new DependCacheDomain<TEntity, TKey>(entityKeySelector, missingItemHandler, missingItemsHandler, cacheName, cacheKeyFormat, secondesToLive, contextCacheEnabled);
        }

        public static DependCacheDomain<TEntity, TParam, TKey> CreatePairKey<TEntity, TParam, TKey>(Func<TEntity, TKey> entityKeySelector, Func<TParam, TKey, TEntity> missingItemHandler, Func<TParam, IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler, string cacheName = null, string cacheKeyFormat = null, int secondesToLive = 0, bool contextCacheEnabled = true)
           where TEntity : IQueryTimestamp
        {
            return new DependCacheDomain<TEntity, TParam, TKey>(entityKeySelector, missingItemHandler, missingItemsHandler, cacheName, cacheKeyFormat, secondesToLive, contextCacheEnabled);
        }
    }




}
