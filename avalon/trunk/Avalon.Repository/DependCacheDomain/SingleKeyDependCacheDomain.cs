using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    public class DependCacheDomain<TEntity, TKey> : CacheDomain<TEntity, TKey>
       where TEntity : class, IQueryTimestamp
    {
        Action<CacheDependDefine, TKey> dependAction;

        public DependCacheDomain(Func<TEntity, TKey> entityKeySelector, Func<TKey, TEntity> missingItemHandler, Func<IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler, string cacheName = null, string cacheKeyFormat = null, int secondesToLive = 0)
            : base(entityKeySelector, missingItemHandler, missingItemsHandler, cacheName, cacheKeyFormat, secondesToLive)
        {
        }

        public override TEntity GetItem(TKey key)
        {
            var item = base.GetItem(key);
            if (CheckEntity(key, item))
                return item;

            return base.GetItem(key);
        }

        public override IEnumerable<TEntity> GetItems(IEnumerable<TKey> keys)
        {
            var items = base.GetItems(keys);
            List<TKey> invalidKeys = new List<TKey>();
            List<TEntity> validItems = new List<TEntity>();
            foreach (var item in items)
            {
                var key = Option.EntityKeySelector(item);
                if (CheckEntity(key, item))
                    validItems.Add(item);
                else
                    invalidKeys.Add(key);
            }
            if (invalidKeys.Count > 0)
            {
                var newItems = base.GetItems(invalidKeys);
                validItems.AddRange(newItems);
            }
            return validItems.OrderBy(o => Option.EntityKeySelector(o), keys).ToList();
        }

        public override TEntity GetItemFromCache(TKey key)
        {
            var item = base.GetItemFromCache(key);
            if (CheckEntity(key, item))
                return item;

            return default(TEntity);
        }

        public override bool Contains(TKey key)
        {
            return GetItemFromCache(key) != null;
        }

        public DependCacheDomain<TEntity, TKey> Depend(Action<CacheDependDefine, TKey> cdd)
        {
            dependAction = cdd;
            return this;
        }

        bool CheckEntity(TKey key, IQueryTimestamp data)
        {
            if (data == null)
                return true;

            var cdd = new CacheDependDefine();
            dependAction(cdd, key);
            var keys = cdd.GetCacheKey();

            ICache dependCache = RepositoryFramework.GetCacherForCacheRegion();
            var values = dependCache.GetBatch<long>(keys);
            var isValid = values.Count() == 0 || values.All(o => o < data.Timestamp);
            if (!isValid)
                RemoveCache(key);

            return isValid;
        }
    }
}
