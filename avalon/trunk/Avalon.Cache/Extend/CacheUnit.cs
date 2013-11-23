using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    public class CacheUnit
    {
        public static CacheUnit<TEntity> Create<TEntity>(string cacheKeyFormat, int secodesToLive, Func<TEntity> missingItemHandler = null)
        {
            return new CacheUnit<TEntity>(cacheKeyFormat, secodesToLive, missingItemHandler);
        }

        public static CacheUnit<TEntity> Create<TEntity>(string cacheName, string cacheKeyFormat, int secodesToLive, Func<TEntity> missingItemHandler = null)
        {
            return new CacheUnit<TEntity>(cacheName, cacheKeyFormat, secodesToLive, missingItemHandler);
        }

        public static CacheUnit<TEntity, TKey> Create<TEntity, TKey>(string cacheKeyFormat, int secodesToLive, Func<TEntity, TKey> entityKeySelector, Func<TKey, TEntity> missingItemHandler = null, Func<IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler = null, Func<TKey, string> formatingHandler = null)
        {
            return new CacheUnit<TEntity, TKey>(cacheKeyFormat, secodesToLive, entityKeySelector, missingItemHandler, missingItemsHandler, formatingHandler);
        }

        public static CacheUnit<TEntity, TParam, TKey> Create<TEntity, TParam, TKey>(string cacheKeyFormat, int secodesToLive, Func<TEntity, TKey> entityKeySelector, Func<TParam, TKey, TEntity> missingItemHandler = null, Func<TParam, IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler = null)
        {
            return new CacheUnit<TEntity, TParam, TKey>(cacheKeyFormat, secodesToLive, entityKeySelector, missingItemHandler, missingItemsHandler);
        }

        public static CacheUnit<TEntity, TParam1, TParam2, TKey> Create<TEntity, TParam1, TParam2, TKey>(string cacheKeyFormat, int secodesToLive, Func<TEntity, TKey> entityKeySelector, Func<TParam1, TParam2, TKey, TEntity> missingItemHandler = null, Func<TParam1, TParam2, IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler = null)
        {
            return new CacheUnit<TEntity, TParam1, TParam2, TKey>(cacheKeyFormat, secodesToLive, entityKeySelector, missingItemHandler, missingItemsHandler);
        }
    }

    public class AbstractCacheUnit<TEntity>
    {
        protected ICache cache;
        protected string cacheKeyFormat;
        protected int secodesToLive;

        protected AbstractCacheUnit(string cacheName, string cacheKeyFormat, int secodesToLive)
        {
            this.cacheKeyFormat = cacheKeyFormat;
            this.secodesToLive = secodesToLive;
            cache = CacheManager.GetCacher(cacheName);
        }

        protected AbstractCacheUnit(string cacheKeyFormat, int secodesToLive)
        {
            this.cacheKeyFormat = cacheKeyFormat;
            this.secodesToLive = secodesToLive;
            cache = CacheManager.GetCacher<TEntity>();
        }

        public ICache Cache { get { return cache; } }

        protected string GetCacheKey()
        {
            return cacheKeyFormat;
        }

        protected string GetCacheKey<TParam>(TParam param)
        {
            return GetCacheKey(param, null);
        }

        protected string GetCacheKey<TParam>(TParam param, Func<TParam, string> formateSelector)
        {
            if (formateSelector != null)
                return formateSelector(param);
            return cacheKeyFormat.Format(param);
        }

        protected string GetCacheKey<TParam1, TParam2>(TParam1 param1, TParam2 param2)
        {
            return cacheKeyFormat.Format(param1, param2);
        }

        protected string GetCacheKey<TParam1, TParam2, TParam3>(TParam1 param1, TParam2 param2, TParam3 param3)
        {
            return cacheKeyFormat.Format(param1, param2, param3);
        }

        protected static bool IsDefault<T>(T value)
        {
            return EqualityComparer<T>.Default.Equals(value, default(T));
        }
    }

    public class CacheUnit<TEntity> : AbstractCacheUnit<TEntity>
    {
        Func<TEntity> missingItemHandler;

        public CacheUnit(string cacheKeyFormat, int secodesToLive, Func<TEntity> missingItemHandler = null)
            : base(cacheKeyFormat, secodesToLive)
        {
            this.missingItemHandler = missingItemHandler;
        }

        public CacheUnit(string cacheName, string cacheKeyFormat, int secodesToLive, Func<TEntity> missingItemHandler = null)
            : base(cacheName, cacheKeyFormat, secodesToLive)
        {
            this.missingItemHandler = missingItemHandler;
        }

        public TEntity GetItem()
        {
            return cache.GetItem(cacheKeyFormat, secodesToLive, missingItemHandler);
        }

        public void RemoveCache()
        {
            cache.Remove(typeof(TEntity), GetCacheKey());
        }

        public void RemoveCache(Predicate<TEntity> selector)
        {
            TEntity entity = GetItemFromCache();
            if (!IsDefault(entity))
            {
                if (selector(entity))
                    RemoveCache();
            }
        }

        public void SetItemToCache(TEntity entity)
        {
            cache.Set(GetCacheKey(), entity);
        }

        public TEntity GetItemFromCache()
        {
            return cache.Get<TEntity>(GetCacheKey());
        }
    }

    public class CacheUnit<TEntity, TKey> : AbstractCacheUnit<TEntity>
    {
        Func<TEntity, TKey> entityKeySelector;
        Func<TKey, TEntity> missingItemHandler;
        Func<IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler;
        Func<TKey, string> formatingHandler;

        public CacheUnit(string cacheKeyFormat, int secodesToLive, Func<TEntity, TKey> entityKeySelector = null, Func<TKey, TEntity> missingItemHandler = null, Func<IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler = null, Func<TKey, string> formatingHandler = null)
            : base(cacheKeyFormat, secodesToLive)
        {
            this.entityKeySelector = entityKeySelector;
            this.missingItemHandler = missingItemHandler;
            this.missingItemsHandler = missingItemsHandler;
            this.formatingHandler = formatingHandler;

            if (missingItemHandler == null && missingItemsHandler == null)
                throw new ArgumentNullException("missingItemHandler and missingItemsHandler is null");

            if (missingItemHandler == null)
                this.missingItemHandler = key => missingItemsHandler(new TKey[] { key }).FirstOrDefault();
            if (missingItemsHandler == null)
                this.missingItemsHandler = keys => keys.Select(key => missingItemHandler(key));
        }

        public TEntity GetItem(TKey key)
        {
            return cache.GetItem(key, InnerGetCacheKey(key), secodesToLive, missingItemHandler);
        }

        public IEnumerable<TEntity> GetItems(IEnumerable<TKey> keys)
        {
            return cache.GetItems(keys, InnerGetCacheKey, secodesToLive, entityKeySelector, missingItemsHandler);
        }

        public void RemoveCache(TKey key)
        {
            cache.Remove(typeof(TEntity), InnerGetCacheKey(key));
        }

        public void RemoveCache(IEnumerable<TKey> keys)
        {
            foreach (TKey key in keys)
                cache.Remove(typeof(TEntity), InnerGetCacheKey(key));
        }

        public void RemoveCache(TKey key, Predicate<TEntity> selector)
        {
            TEntity entity = GetItemFromCache(key);
            if (!IsDefault(entity))
            {
                if (selector(entity))
                    RemoveCache(key);
            }
        }

        public void SetItemToCache(TEntity entity)
        {
            cache.Set(InnerGetCacheKey(entityKeySelector(entity)), entity);
        }

        public void SetItemToCache(TEntity entity, TKey key)
        {
            cache.Set(InnerGetCacheKey(key), entity);
        }

        public TEntity GetItemFromCache(TKey key)
        {
            return cache.Get<TEntity>(InnerGetCacheKey(key));
        }

        internal string InnerGetCacheKey(TKey key)
        {
            return GetCacheKey(key, formatingHandler);
        }
    }

    public class CacheUnit<TEntity, TParam, TKey> : AbstractCacheUnit<TEntity>
    {
        Func<TEntity, TKey> entityKeySelector;
        Func<TParam, TKey, TEntity> missingItemHandler;
        Func<TParam, IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler;

        public CacheUnit(string cacheKeyFormat, int secodesToLive, Func<TEntity, TKey> entityKeySelector, Func<TParam, TKey, TEntity> missingItemHandler = null, Func<TParam, IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler = null)
            : base(cacheKeyFormat, secodesToLive)
        {
            this.entityKeySelector = entityKeySelector;
            this.missingItemHandler = missingItemHandler;
            this.missingItemsHandler = missingItemsHandler;

            if (missingItemHandler == null && missingItemsHandler == null)
                throw new ArgumentNullException("missingItemHandler and missingItemsHandler is null");

            if (missingItemHandler == null)
                this.missingItemHandler = (param, key) => missingItemsHandler(param, new TKey[] { key }).FirstOrDefault();
            if (missingItemsHandler == null)
                this.missingItemsHandler = (param, keys) => keys.Select(key => missingItemHandler(param, key));
        }

        public TEntity GetItem(TParam param, TKey key)
        {
            return cache.GetItem(param, key, cacheKeyFormat, secodesToLive, missingItemHandler);
        }

        public IEnumerable<TEntity> GetItems(TParam param, IEnumerable<TKey> keys)
        {
            return cache.GetItem(param, keys, cacheKeyFormat, secodesToLive, missingItemsHandler);
        }

        public void RemoveCache(TParam param, TKey key)
        {
            cache.Remove(typeof(TEntity), GetCacheKey(param, key));
        }

        public void RemoveCache(TParam param, TKey key, Predicate<TEntity> selector)
        {
            TEntity entity = GetItemFromCache(param, key);
            if (!IsDefault(entity))
            {
                if (selector(entity))
                    RemoveCache(param, key);
            }
        }

        public void SetItemToCache(TParam param, TEntity entity)
        {
            cache.Set(GetCacheKey(param, entityKeySelector(entity)), entity);
        }

        public TEntity GetItemFromCache(TParam param, TKey key)
        {
            return cache.Get<TEntity>(GetCacheKey(param, key));
        }
    }

    public class CacheUnit<TEntity, TParam1, TParam2, TKey> : AbstractCacheUnit<TEntity>
    {
        Func<TEntity, TKey> entityKeySelector;
        Func<TParam1, TParam2, TKey, TEntity> missingItemHandler;
        Func<TParam1, TParam2, IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler;

        public CacheUnit(string cacheKeyFormat, int secodesToLive, Func<TEntity, TKey> entityKeySelector, Func<TParam1, TParam2, TKey, TEntity> missingItemHandler = null, Func<TParam1, TParam2, IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler = null)
            : base(cacheKeyFormat, secodesToLive)
        {
            this.entityKeySelector = entityKeySelector;
            this.missingItemHandler = missingItemHandler;
            this.missingItemsHandler = missingItemsHandler;

            if (missingItemHandler == null && missingItemsHandler == null)
                throw new ArgumentNullException("missingItemHandler and missingItemsHandler is null");

            if (missingItemHandler == null)
                this.missingItemHandler = (param1, param2, key) => missingItemsHandler(param1, param2, new TKey[] { key }).FirstOrDefault();
            if (missingItemsHandler == null)
                this.missingItemsHandler = (param1, param2, keys) => keys.Select(key => missingItemHandler(param1, param2, key));
        }

        public TEntity GetItem(TParam1 param1, TParam2 param2, TKey key)
        {
            return cache.GetItem(param1, param2, key, cacheKeyFormat, secodesToLive, missingItemHandler);
        }

        public IEnumerable<TEntity> GetItems(TParam1 param1, TParam2 param2, IEnumerable<TKey> keys)
        {
            return cache.GetItem(param1, param2, keys, cacheKeyFormat, secodesToLive, missingItemsHandler);
        }

        public void RemoveCache(TParam1 param1, TParam2 param2, TKey key)
        {
            cache.Remove(typeof(TEntity), GetCacheKey(param1, param2, key));
        }

        public void RemoveCache(TParam1 param1, TParam2 param2, TKey key, Predicate<TEntity> selector)
        {
            TEntity entity = GetItemFromCache(param1, param2, key);
            if (!IsDefault(entity))
            {
                if (selector(entity))
                    RemoveCache(param1, param2, key);
            }
        }

        public void SetItemToCache(TParam1 param1, TParam2 param2, TEntity entity)
        {
            cache.Set(GetCacheKey(param1, param2, entityKeySelector(entity)), entity);
        }

        public TEntity GetItemFromCache(TParam1 param1, TParam2 param2, TKey key)
        {
            return cache.Get<TEntity>(GetCacheKey(param1, param2, key));
        }
    }
}
