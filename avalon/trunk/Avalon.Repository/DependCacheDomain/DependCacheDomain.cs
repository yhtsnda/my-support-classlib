using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{

    public static class DependCacheDomain
    {
        public static DependCacheDomain<TEntity> CreateSingleton<TEntity>(Func<TEntity> missingItemHandler, string cacheName = null, string cacheKeyFormat = null, int secondesToLive = 0, bool contextCacheEnabled = true)
            where TEntity : class, IQueryTimestamp
        {
            return new DependCacheDomain<TEntity>(missingItemHandler, cacheName, cacheKeyFormat, secondesToLive);
        }

        public static DependCacheDomain<TEntity, TKey> CreateSingleKey<TEntity, TKey>(Func<TEntity, TKey> entityKeySelector, Func<TKey, TEntity> missingItemHandler, Func<IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler, string cacheName = null, string cacheKeyFormat = null, int secondesToLive = 0)
            where TEntity : class, IQueryTimestamp
        {
            return new DependCacheDomain<TEntity, TKey>(entityKeySelector, missingItemHandler, missingItemsHandler, cacheName, cacheKeyFormat, secondesToLive);
        }

        public static DependCacheDomain<TEntity, TParam, TKey> CreatePairKey<TEntity, TParam, TKey>(Func<TEntity, TKey> entityKeySelector, Func<TParam, TKey, TEntity> missingItemHandler, Func<TParam, IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler, string cacheName = null, string cacheKeyFormat = null, int secondesToLive = 0)
           where TEntity : class,  IQueryTimestamp
        {
            return new DependCacheDomain<TEntity, TParam, TKey>(entityKeySelector, missingItemHandler, missingItemsHandler, cacheName, cacheKeyFormat, secondesToLive);
        }

        public static DependCacheDomain<TEntity, TParam1, TParam2, TKey> CreateTripletKey<TEntity, TParam1, TParam2, TKey>(Func<TEntity, TKey> entityKeySelector, Func<TParam1, TParam2, TKey, TEntity> missingItemHandler, Func<TParam1, TParam2, IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler, string cacheName = null, string cacheKeyFormat = null, int secondesToLive = 0)
          where TEntity : class, IQueryTimestamp
        {
            return new DependCacheDomain<TEntity, TParam1, TParam2, TKey>(entityKeySelector, missingItemHandler, missingItemsHandler, cacheName, cacheKeyFormat, secondesToLive);
        }
    }




}
