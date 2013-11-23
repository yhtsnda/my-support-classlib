using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    /// <summary>
    /// 抽象缓存域对象
    /// </summary>
    public abstract class AbstractCacheDomain
    {
        static object syncRoot;
        static Dictionary<string, object> syncObjects;
        ICache cache;

        static AbstractCacheDomain()
        {
            syncRoot = new object();
            syncObjects = new Dictionary<string, object>();
        }

        internal AbstractCacheDomain(CacheDomainOption option)
        {
            cache = CacheManager.GetCacher(option.GetCacheFullName());
        }

        public ICache Cache
        {
            get { return cache; }
        }

        /// <summary>
        /// 获取排它锁的对象
        /// </summary>
        static object GetSyncObject(string name)
        {
            object sync = syncObjects.TryGetValue(name);
            if (sync == null)
            {
                lock (syncRoot)
                {
                    sync = syncObjects.TryGetValue(name);
                    if (sync == null)
                    {
                        sync = new object();
                        syncObjects[name] = sync;
                    }
                }
            }
            return sync;
        }

        #region GetItem

        internal TEntity GetItem<TEntity>(ICache cache, CacheDomainOption<TEntity> option) where TEntity : class
        {
            var cacheKey = option.GetCacheKey();
            var cacheItem = cache.Get<TEntity>(cacheKey);

            if (cacheItem == null)
            {
                //根据缓存名获取锁对象
                object sync = GetSyncObject(option.GetCacheFullName());
                lock (sync)
                {
                    //再次从缓存读取
                    cacheItem = cache.Get<TEntity>(cacheKey);
                    if (cacheItem == null)
                    {
                        //从原始数据读取
                        cacheItem = option.MissingItemHandler();
                        if (cacheItem != null)
                            OnSetCache(cache, cacheKey, cacheItem, option.SecondesToLive);
                    }
                }
            }
            return cacheItem;
        }

        protected virtual void OnSetCache(ICache cache, string cacheKey, object entity, int secondesToLive)
        {
            cache.Set(cacheKey, entity, secondesToLive);
        }

        protected TEntity GetItem<TEntity, TKey>(ICache cache, TKey key, CacheDomainOption<TEntity, TKey> option) where TEntity : class
        {
            var cacheKey = option.GetCacheKey(key);
            var cacheItem = cache.Get<TEntity>(cacheKey);

            if (cacheItem == null)
            {
                //根据缓存名获取锁对象
                object sync = GetSyncObject(option.GetCacheFullName());
                lock (sync)
                {
                    //再次从缓存读取
                    cacheItem = cache.Get<TEntity>(cacheKey);
                    if (cacheItem == null)
                    {
                        //从原始数据读取
                        cacheItem = option.MissingItemHandler(key);
                        if (cacheItem != null)
                            OnSetCache(cache, cacheKey, cacheItem, option.SecondesToLive);
                    }
                }
            }
            return cacheItem;
        }

        protected TEntity GetItem<TEntity, TParam, TKey>(ICache cache, TParam param, TKey key, CacheDomainOption<TEntity, TParam, TKey> option) where TEntity : class
        {
            var cacheKey = option.GetCacheKey(param, key);
            var cacheItem = cache.Get<TEntity>(cacheKey);

            if (cacheItem == null)
            {
                //根据缓存名获取锁对象
                object sync = GetSyncObject(option.GetCacheFullName());
                lock (sync)
                {
                    //再次从缓存读取
                    cacheItem = cache.Get<TEntity>(cacheKey);
                    if (cacheItem == null)
                    {
                        //从原始数据读取
                        cacheItem = option.MissingItemHandler(param, key);
                        if (cacheItem != null)
                            OnSetCache(cache, cacheKey, cacheItem, option.SecondesToLive);
                    }
                }
            }
            return cacheItem;
        }

        protected TEntity GetItem<TEntity, TParam1, TParam2, TKey>(ICache cache, TParam1 param1, TParam2 param2, TKey key, CacheDomainOption<TEntity, TParam1, TParam2, TKey> option) where TEntity : class
        {
            var cacheKey = option.GetCacheKey(param1, param2, key);
            var cacheItem = cache.Get<TEntity>(cacheKey);

            if (cacheItem == null)
            {
                //根据缓存名获取锁对象
                object sync = GetSyncObject(option.GetCacheFullName());
                lock (sync)
                {
                    //再次从缓存读取
                    cacheItem = cache.Get<TEntity>(cacheKey);
                    if (cacheItem == null)
                    {
                        //从原始数据读取
                        cacheItem = option.MissingItemHandler(param1, param2, key);
                        if (cacheItem != null)
                            OnSetCache(cache, cacheKey, cacheItem, option.SecondesToLive);
                    }
                }
            }
            return cacheItem;
        }

        #endregion

        #region GetItems

        protected virtual void OnSetCacheBatch<TEntity>(ICache cache, IEnumerable<TEntity> items, Func<TEntity, string> keySelector, int secondsToLive)
        {
            cache.SetBatch(items, keySelector, secondsToLive);
        }

        protected IEnumerable<TEntity> GetItems<TEntity, TKey>(ICache cache, IEnumerable<TKey> keys, CacheDomainOption<TEntity, TKey> option)
        {
            //CacheKey 与属性键的映射（去重）
            IDictionary<string, TKey> keyDic = keys.Distinct().ToDictionary(key => option.GetCacheKey(key));

            //访问缓存并获取不在缓存中的 CacheKey 
            IEnumerable<string> missing;
            var cacheItems = cache.GetBatch<TEntity>(keyDic.Keys, out missing).ToList();

            if (missing.Count() > 0)
            {
                //根据缓存名获取锁对象
                object sync = GetSyncObject(option.GetCacheFullName());
                lock (sync)
                {
                    //二次查找缓存
                    IEnumerable<string> secondMissing;
                    var secondCacheItems = cache.GetBatch<TEntity>(missing, out secondMissing).ToList();
                    cacheItems.AddRange(secondCacheItems);

                    //从数据源获取
                    if (secondMissing.Count() > 0)
                    {
                        IEnumerable<TEntity> dbItems = option.GetMissingItems(secondMissing.Select(key => keyDic[key]).ToArray());
                        //过滤掉 null 项，并加入缓存
                        OnSetCacheBatch(cache, dbItems.Where(item => item != null), key => option.GetCacheKey(option.EntityKeySelector(key)), option.SecondesToLive);
                        cacheItems.AddRange(dbItems);
                    }
                }
            }

            //按目标序列输出
            return cacheItems.OrderBy(entity => option.EntityKeySelector(entity), keys);
        }

        protected IEnumerable<TEntity> GetItems<TEntity, TParam, TKey>(ICache cache, TParam param, IEnumerable<TKey> keys, CacheDomainOption<TEntity, TParam, TKey> option)
        {
            //CacheKey 与属性键的映射（去重）
            IDictionary<string, TKey> keyDic = keys.Distinct().ToDictionary(key => option.GetCacheKey(param, key));

            //访问缓存并获取不在缓存中的 CacheKey 
            IEnumerable<string> missing;
            var cacheItems = cache.GetBatch<TEntity>(keyDic.Keys, out missing).ToList();

            if (missing.Count() > 0)
            {
                //根据缓存名获取锁对象
                object sync = GetSyncObject(option.GetCacheFullName());
                lock (sync)
                {
                    //二次查找缓存
                    IEnumerable<string> secondMissing;
                    var secondCacheItems = cache.GetBatch<TEntity>(missing, out secondMissing).ToList();
                    cacheItems.AddRange(secondCacheItems);

                    //从数据源获取
                    if (secondMissing.Count() > 0)
                    {
                        IEnumerable<TEntity> dbItems = option.GetMissingItems(param, secondMissing.Select(key => keyDic[key]).ToArray());
                        //过滤掉 null 项，并加入缓存
                        OnSetCacheBatch(cache, dbItems.Where(item => item != null), key => option.GetCacheKey(param, option.EntityKeySelector(key)), option.SecondesToLive);
                        cacheItems.AddRange(dbItems);
                    }
                }
            }

            //按目标序列输出
            return cacheItems.OrderBy(entity => option.EntityKeySelector(entity), keys);
        }

        protected IEnumerable<TEntity> GetItems<TEntity, TParam1, TParam2, TKey>(ICache cache, TParam1 param1, TParam2 param2, IEnumerable<TKey> keys, CacheDomainOption<TEntity, TParam1, TParam2, TKey> option)
        {
            //CacheKey 与属性键的映射（去重）
            IDictionary<string, TKey> keyDic = keys.Distinct().ToDictionary(key => option.GetCacheKey(param1, param2, key));

            //访问缓存并获取不在缓存中的 CacheKey 
            IEnumerable<string> missing;
            var cacheItems = cache.GetBatch<TEntity>(keyDic.Keys, out missing).ToList();

            if (missing.Count() > 0)
            {
                //根据缓存名获取锁对象
                object sync = GetSyncObject(option.GetCacheFullName());
                lock (sync)
                {
                    //二次查找缓存
                    IEnumerable<string> secondMissing;
                    var secondCacheItems = cache.GetBatch<TEntity>(missing, out secondMissing).ToList();
                    cacheItems.AddRange(secondCacheItems);

                    //从数据源获取
                    if (secondMissing.Count() > 0)
                    {
                        IEnumerable<TEntity> dbItems = option.GetMissingItems(param1, param2, secondMissing.Select(key => keyDic[key]).ToArray());
                        //过滤掉 null 项，并加入缓存
                        OnSetCacheBatch(cache, dbItems.Where(item => item != null), key => option.GetCacheKey(param1, param2, option.EntityKeySelector(key)), option.SecondesToLive);
                        cacheItems.AddRange(dbItems);
                    }
                }
            }

            //按目标序列输出
            return cacheItems.OrderBy(entity => option.EntityKeySelector(entity), keys);
        }
        #endregion
    }
}
