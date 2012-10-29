using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool
{
    public abstract class AbstractCacheDomain
    {
        private static object mSyncRoot;
        private static Dictionary<string, object> mSyncObjects;
        private ICache mCache;

        public  ICache Cache
        {
            get { return mCache; }
        }

        static  AbstractCacheDomain()
        {
            mSyncRoot = new object();
            mSyncObjects = new Dictionary<string, object>();
        }

        internal  AbstractCacheDomain(CacheDomainOption option)
        {
            mCache = CacheManager.GetCacher(option.GetCacheFullName());
            //使用缓存代理处理
            if(option.ContextCacheEnabled && !(mCache is AspnetCache))
                mCache = new CacheProxy(mCache);
        }

        /// <summary>
        /// 获取排它锁对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static  object GetSyncObject(string name)
        {
            object sync = mSyncObjects.TryGetValue(name);
            if(sync == null)
            {
                lock (mSyncRoot)
                {
                    sync = mSyncObjects.TryGetValue(name);
                    if(sync == null)
                    {
                        sync = new object();
                        mSyncObjects[name] = sync;
                    }
                }
            }
            return sync;
        }

        internal static TEntity GetItem<TEntity>(ICache cache, CacheDomainOption<TEntity> option)
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
                        if (!AbstractCache.IsDefault(cacheItem))
                            cache.Set(cacheKey, cacheItem, option.SecondesToLive);
                    }
                }
            }
            return cacheItem;
        }

        internal static TEntity GetItem<TEntity, TKey>(ICache cache, TKey key, CacheDomainOption<TEntity, TKey> option)
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
                        if (!AbstractCache.IsDefault(cacheItem))
                            cache.Set(cacheKey, cacheItem, option.SecondesToLive);
                    }
                }
            }
            return cacheItem;
        }

        internal static TEntity GetItem<TEntity, TParam, TKey>(ICache cache, TParam param, TKey key, CacheDomainOption<TEntity, TParam, TKey> option)
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
                        if (!AbstractCache.IsDefault(cacheItem))
                            cache.Set(cacheKey, cacheItem, option.SecondesToLive);
                    }
                }
            }
            return cacheItem;
        }

        internal static TEntity GetItem<TEntity, TParam1, TParam2, TKey>(ICache cache, TParam1 param1, TParam2 param2, 
            TKey key, CacheDomainOption<TEntity, TParam1, TParam2, TKey> option)
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
                        if (!AbstractCache.IsDefault(cacheItem))
                            cache.Set(cacheKey, cacheItem, option.SecondesToLive);
                    }
                }
            }
            return cacheItem;
        }

        internal static IEnumerable<TEntity> GetItems<TEntity, TKey>(ICache cache, IEnumerable<TKey> keys, CacheDomainOption<TEntity, TKey> option)
        {
            //CacheKey 与属性键的映射（去重）
            IDictionary<string, TKey> keyDic = keys.Distinct().ToDictionary(key => option.GetCacheKey(key));

            //访问缓存并获取不在缓存中的 CacheKey 
            IEnumerable<string> missing;
            var cacheItems = cache.GetBatch<TEntity>(keyDic.Keys, out missing).ToList();

            if (missing.Any())
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
                    if (secondMissing.Any())
                    {
                        IEnumerable<TEntity> dbItems = option.GetMissingItems(secondMissing.Select(key => keyDic[key]).ToArray());
                        //过滤掉 null 项，并加入缓存
                        cache.SetBatch(dbItems.Where(item => !AbstractCache.IsDefault(item)), 
                            key => option.GetCacheKey(option.EntityKeySelector(key)),
                            option.SecondesToLive);
                        cacheItems.AddRange(dbItems);
                    }
                }
            }

            //按目标序列输出
            return cacheItems.OrderBy(entity => option.EntityKeySelector(entity), keys);
        }

        internal static IEnumerable<TEntity> GetItems<TEntity, TParam, TKey>(ICache cache, TParam param, 
            IEnumerable<TKey> keys, CacheDomainOption<TEntity, TParam, TKey> option)
        {
            //CacheKey 与属性键的映射（去重）
            IDictionary<string, TKey> keyDic = keys.Distinct().ToDictionary(key => option.GetCacheKey(param, key));

            //访问缓存并获取不在缓存中的 CacheKey 
            IEnumerable<string> missing;
            var cacheItems = cache.GetBatch<TEntity>(keyDic.Keys, out missing).ToList();

            if (missing.Any())
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
                    if (secondMissing.Any())
                    {
                        IEnumerable<TEntity> dbItems = option.GetMissingItems(param, secondMissing.Select(key => keyDic[key]).ToArray());
                        //过滤掉 null 项，并加入缓存
                        cache.SetBatch(dbItems.Where(item => !AbstractCache.IsDefault(item)), key => option.GetCacheKey(param, option.EntityKeySelector(key)), option.SecondesToLive);
                        cacheItems.AddRange(dbItems);
                    }
                }
            }

            //按目标序列输出
            return cacheItems.OrderBy(entity => option.EntityKeySelector(entity), keys);
        }

        internal static IEnumerable<TEntity> GetItems<TEntity, TParam1, TParam2, TKey>(ICache cache, TParam1 param1, 
            TParam2 param2, IEnumerable<TKey> keys, CacheDomainOption<TEntity, TParam1, TParam2, TKey> option)
        {
            //CacheKey 与属性键的映射（去重）
            IDictionary<string, TKey> keyDic = keys.Distinct().ToDictionary(key => option.GetCacheKey(param1, param2, key));

            //访问缓存并获取不在缓存中的 CacheKey 
            IEnumerable<string> missing;
            var cacheItems = cache.GetBatch<TEntity>(keyDic.Keys, out missing).ToList();

            if (missing.Any())
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
                    if (secondMissing.Any())
                    {
                        IEnumerable<TEntity> dbItems = option.GetMissingItems(param1, param2, secondMissing.Select(key => keyDic[key]).ToArray());
                        //过滤掉 null 项，并加入缓存
                        cache.SetBatch(dbItems.Where(item => !AbstractCache.IsDefault(item)), 
                            key => option.GetCacheKey(param1, param2, option.EntityKeySelector(key)), 
                            option.SecondesToLive);
                        cacheItems.AddRange(dbItems);
                    }
                }
            }

            //按目标序列输出
            return cacheItems.OrderBy(entity => option.EntityKeySelector(entity), keys);
        }
    }
}
