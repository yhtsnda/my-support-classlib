using Projects.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    public class DependCacheDomain<TEntity, TParam, TKey> : CacheDomain<TEntity, TParam, TKey>
      where TEntity : IQueryTimestamp
    {
        Action<CacheDependDefine, TParam, TKey> dependAction;

        public DependCacheDomain(Func<TEntity, TKey> entityKeySelector, Func<TParam, TKey, TEntity> missingItemHandler, Func<TParam, IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler, string cacheName = null, string cacheKeyFormat = null, int secondesToLive = 0, bool contextCacheEnabled = true)
            : base(entityKeySelector, missingItemHandler, missingItemsHandler, cacheName, cacheKeyFormat, secondesToLive, contextCacheEnabled)
        {
        }

        public override TEntity GetItem(TParam param, TKey key)
        {
            var item = base.GetItem(param, key);
            if (!CheckEntity(param, key, item))
                return item;

            return base.GetItem(param, key);
        }

        public override IEnumerable<TEntity> GetItems(TParam param, IEnumerable<TKey> keys)
        {
            var items = base.GetItems(param, keys);
            List<TKey> invalidKeys = new List<TKey>();
            List<TEntity> validItems = new List<TEntity>();
            foreach (var item in items)
            {
                var key = Option.EntityKeySelector(item);
                if (!CheckEntity(param, key, item))
                    invalidKeys.Add(key);
                else
                    validItems.Add(item);
            }
            if (invalidKeys.Count > 0)
            {
                var newItems = base.GetItems(param, invalidKeys);
                validItems.AddRange(newItems);
            }
            return validItems.OrderBy(o => Option.EntityKeySelector(o), keys).ToList();
        }

        public override TEntity GetItemFromCache(TParam param, TKey key)
        {
            var item = base.GetItemFromCache(param, key);
            if (!CheckEntity(param, key, item))
                return default(TEntity);

            return item;
        }

        public override bool Contains(TParam param, TKey key)
        {
            return GetItemFromCache(param, key) != null;
        }

        public void Depend(Action<CacheDependDefine, TParam, TKey> cdd)
        {
            dependAction = cdd;
        }

        bool CheckEntity(TParam param, TKey key, IQueryTimestamp data)
        {
            if (data == null)
                return true;

            var cdd = new CacheDependDefine();
            dependAction(cdd, param, key);
            var keys = cdd.GetCacheKey();

            ICache dependCache = RepositoryFramework.GetCacher("cache:region");
            var values = dependCache.GetBatch<long>(keys);
            var isValid = values.Count() == 0 || values.All(o => o < data.Timestamp);
            if (!isValid)
                RemoveCache(param, key);

            return isValid;
        }
    }
}
