using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Tool;

namespace Projects.Framework
{
    public class DependCacheDomain<TEntity, TParam1, TParam2, TKey> : CacheDomain<TEntity, TParam1, TParam2, TKey>
          where TEntity : IQueryTimestamp
    {
        Action<CacheDependDefine, TParam1, TParam2, TKey> dependAction;

        public DependCacheDomain(Func<TEntity, TKey> entityKeySelector, Func<TParam1, TParam2, TKey, TEntity> missingItemHandler, Func<TParam1, TParam2, IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler, string cacheName = null, string cacheKeyFormat = null, int secondesToLive = 0, bool contextCacheEnabled = true)
            : base(entityKeySelector, missingItemHandler, missingItemsHandler, cacheName, cacheKeyFormat, secondesToLive, contextCacheEnabled)
        {
        }

        public override TEntity GetItem(TParam1 param1, TParam2 param2, TKey key)
        {
            var item = base.GetItem(param1, param2, key);
            if (CheckEntity(param1, param2, key, item))
                return item;

            return base.GetItem(param1, param2, key);
        }

        public override IEnumerable<TEntity> GetItems(TParam1 param1, TParam2 param2, IEnumerable<TKey> keys)
        {
            var items = base.GetItems(param1, param2, keys);
            List<TKey> invalidKeys = new List<TKey>();
            List<TEntity> validItems = new List<TEntity>();
            foreach (var item in items)
            {
                var key = Option.EntityKeySelector(item);
                if (CheckEntity(param1, param2, key, item))
                    validItems.Add(item);
                else
                    invalidKeys.Add(key);
            }
            if (invalidKeys.Count > 0)
            {
                var newItems = base.GetItems(param1, param2, invalidKeys);
                validItems.AddRange(newItems);
            }
            return validItems.OrderBy(o => Option.EntityKeySelector(o), keys).ToList();
        }

        public override TEntity GetItemFromCache(TParam1 param1, TParam2 param2, TKey key)
        {
            var item = base.GetItemFromCache(param1, param2, key);
            if (CheckEntity(param1, param2, key, item))
                return item;

            return default(TEntity);
        }

        public override bool Contains(TParam1 param1, TParam2 param2, TKey key)
        {
            return GetItemFromCache(param1, param2, key) != null;
        }

        public DependCacheDomain<TEntity, TParam1, TParam2, TKey> Depend(Action<CacheDependDefine, TParam1, TParam2, TKey> cdd)
        {
            dependAction = cdd;
            return this;
        }

        bool CheckEntity(TParam1 param1, TParam2 param2, TKey key, IQueryTimestamp data)
        {
            if (data == null)
                return true;

            var cdd = new CacheDependDefine();
            dependAction(cdd, param1, param2, key);
            var keys = cdd.GetCacheKey();

            ICache dependCache = RepositoryFramework.GetCacherForCacheRegion();
            var values = dependCache.GetBatch<long>(keys);
            var isValid = values.Count() == 0 || values.All(o => o < data.Timestamp);
            if (!isValid)
                RemoveCache(param1, param2, key);

            return isValid;
        }
    }
}
