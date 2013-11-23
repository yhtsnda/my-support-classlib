using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    /// <summary>
    /// 缓存扩展
    /// </summary>
    public static class CacheExtend
    {
        static object syncRoot;
        static Dictionary<string, object> syncObjects;

        static CacheExtend()
        {
            syncRoot = new object();
            syncObjects = new Dictionary<string, object>();
        }

        public static IEnumerable GetBatch(this ICache cache, Type type, IEnumerable<string> keys, out IEnumerable<string> missingKeys)
        {
            var results = cache.GetBatchResult(type, keys);
            missingKeys = results.GetMissingKeys();
            return results.Where(o => o.HasData).Select(o => o.GetValue());
        }


        public static IEnumerable<TEntity> GetBatch<TEntity>(this ICache cache, IEnumerable<string> keys, out IEnumerable<string> missingKeys)
        {
            var results = cache.GetBatchResult(typeof(TEntity), keys);
            missingKeys = results.GetMissingKeys();
            return results.Where(o => o.HasData).Select(o => o.GetValue<TEntity>());
        }

        /// <summary>
        /// 批量将对象添加到缓存
        /// </summary>
        public static void SetBatch<T>(this ICache cache, IEnumerable<T> items, Func<T, string> keySelector, int secondsToLive)
        {
            var batches = from i in items
                          select new CacheItem(keySelector(i), i);
            cache.SetBatch(batches, secondsToLive);
        }

        /// <summary>
        /// 批量将对象添加到缓存
        /// </summary>
        public static void SetBatch<T>(this ICache cache, IEnumerable<T> items, Func<T, string> keySelector, DateTime expiredTime)
        {
            var batches = from i in items
                          select new CacheItem(keySelector(i), i);
            cache.SetBatch(batches, expiredTime);
        }

        #region GetItems

        /// <summary>
        /// 封装了批量缓存及数据源读取的逻辑
        /// </summary>
        /// <typeparam name="TEntity">目标类型</typeparam>
        /// <typeparam name="TKey">目标类型键的类型</typeparam>
        /// <param name="cache">缓存对象</param>
        /// <param name="keys">要获取的键值集合</param>
        /// <param name="cacheKeyFormat">用于缓存键的格式文本。使用String.Format(cacheKeyFormat,key)实现</param>
        /// <param name="secodesToLive">缓存的秒数</param>
        /// <param name="entityKeySelector">从目标类型获取键值</param>
        /// <param name="missingSelector">当数据在缓存不存在时从数据源获取</param>
        /// <returns></returns>
        public static IEnumerable<TEntity> GetItems<TEntity, TKey>(
            this ICache cache,
            IEnumerable<TKey> keys,
            string cacheKeyFormat,
            int secodesToLive,
            Func<TEntity, TKey> entityKeySelector,
            Func<IEnumerable<TKey>, IEnumerable<TEntity>> missingSelector)
        {
            object sync = GetSyncObject(cacheKeyFormat);

            Dictionary<string, TKey> keyHash = CreateKeyDictionary(keys, cacheKeyFormat);

            IEnumerable<string> missing;
            var cacheItems = cache.GetBatch<TEntity>(keyHash.Keys, out missing).ToList();
            if (missing.Count() > 0)
            {
                var distinctMissing = missing.Distinct();

                IEnumerable<TEntity> dbItems = missingSelector(GetMissingKeys(keyHash, distinctMissing));
                cache.SetBatch(dbItems, entity => cacheKeyFormat.Format(entityKeySelector(entity)), secodesToLive);

                var dic = dbItems.ToDictionary(o => cacheKeyFormat.Format(entityKeySelector(o)));
                cacheItems.AddRange(missing.Select(o => dic.TryGetValue(o)));
            }

            //按目标序列输出
            return cacheItems.OrderBy(entity => entityKeySelector(entity), keys);
        }

        /// <summary>
        /// 从缓存获取对象
        /// </summary>
        public static IEnumerable<TEntity> GetItems<TEntity, TKey>(
            this ICache cache,
            IEnumerable<TKey> keys,
            Func<TKey, string> cacheKeyFormating,
            int secodesToLive,
            Func<TEntity, TKey> entityKeySelector,
            Func<IEnumerable<TKey>, IEnumerable<TEntity>> missingSelector)
        {
            Dictionary<string, TKey> keyHash = CreateKeyDictionary(keys, cacheKeyFormating);

            IEnumerable<string> missing;
            var cacheItems = cache.GetBatch<TEntity>(keyHash.Keys, out missing).ToList();
            if (missing.Count() > 0)
            {
                var distinctMissing = missing.Distinct();

                IEnumerable<TEntity> dbItems = missingSelector(GetMissingKeys(keyHash, distinctMissing));
                cache.SetBatch(dbItems, entity => cacheKeyFormating(entityKeySelector(entity)), secodesToLive);

                var dic = dbItems.ToDictionary(o => cacheKeyFormating(entityKeySelector(o)));
                cacheItems.AddRange(missing.Select(o => dic.TryGetValue(o)));
            }

            //按目标序列输出
            return cacheItems.OrderBy(entity => entityKeySelector(entity), keys);
        }

        /// <summary>
        /// 从缓存获取对象
        /// </summary>
        public static IEnumerable<TEntity> GetItems<TEntity, TKey1, TKey2>(
            this ICache cache,
            TKey1 key1,
            IEnumerable<TKey2> key2s,
            string cacheKeyFormat,
            int secodesToLive,
            Func<TEntity, TKey2> entityKeySelector,
            Func<TKey1, IEnumerable<TKey2>, IEnumerable<TEntity>> missingSelector)
        {
            Dictionary<string, TKey2> keyHash = CreateKeyDictionary(key2s, cacheKeyFormat);

            IEnumerable<string> missing;
            var cacheItems = cache.GetBatch<TEntity>(keyHash.Keys, out missing).ToList();
            if (missing.Count() > 0)
            {
                var distinctMissing = missing.Distinct();

                IEnumerable<TEntity> dbItems = missingSelector(key1, GetMissingKeys(keyHash, distinctMissing));
                cache.SetBatch(dbItems, entity => cacheKeyFormat.Format(key1, entityKeySelector(entity)), secodesToLive);

                var dic = dbItems.ToDictionary(o => cacheKeyFormat.Format(key1, entityKeySelector(o)));
                cacheItems.AddRange(missing.Select(o => dic.TryGetValue(o)));
            }

            //按目标序列输出
            return cacheItems.OrderBy(o => entityKeySelector(o), key2s);
        }

        /// <summary>
        /// 从缓存获取对象
        /// </summary>
        public static IEnumerable<TEntity> GetItems<TEntity, TKey1, TKey2, TKey3>(
            this ICache cache,
            TKey1 key1,
            TKey2 key2,
            IEnumerable<TKey3> key3s,
            string cacheKeyFormat,
            int secodesToLive,
            Func<TEntity, TKey3> entityKeySelector,
            Func<TKey1, TKey2, IEnumerable<TKey3>, IEnumerable<TEntity>> missingSelector)
        {
            Dictionary<string, TKey3> keyHash = CreateKeyDictionary(key3s, cacheKeyFormat);

            IEnumerable<string> missing;
            var cacheItems = cache.GetBatch<TEntity>(keyHash.Keys, out missing).ToList();
            if (missing.Count() > 0)
            {
                var distinctMissing = missing.Distinct();

                IEnumerable<TEntity> dbItems = missingSelector(key1, key2, GetMissingKeys(keyHash, distinctMissing));
                cache.SetBatch(dbItems, entity => cacheKeyFormat.Format(key1, key2, entityKeySelector(entity)), secodesToLive);

                var dic = dbItems.ToDictionary(o => cacheKeyFormat.Format(key1, key2, entityKeySelector(o)));
                cacheItems.AddRange(missing.Select(o => dic.TryGetValue(o)));
            }

            //按目标序列输出
            return cacheItems.OrderBy(entity => entityKeySelector(entity), key3s);
        }

        #endregion

        #region GetItem

        /// <summary>
        /// 从缓存获取对象
        /// </summary>
        public static TEntity GetItem<TEntity>(
            this ICache cache,
            string cacheKeyFormat,
            int secodesToLive,
            Func<TEntity> missingSelector)
        {
            var cacheItem = cache.Get<TEntity>(cacheKeyFormat);
            if (cacheItem == null)
            {
                cacheItem = missingSelector();
                if (cacheItem != null)
                    cache.Set(cacheKeyFormat, cacheItem);
            }

            return cacheItem;
        }

        /// <summary>
        /// 从缓存获取对象
        /// </summary>
        public static TEntity GetItem<TEntity, TKey>(
            this ICache cache,
            TKey key,
            string cacheKeyFormat,
            int secodesToLive,
            Func<TKey, TEntity> missingSelector)
        {
            string cacheKey = cacheKeyFormat.Format(key);
            var cacheItem = cache.Get<TEntity>(cacheKey);
            if (cacheItem == null)
            {
                cacheItem = missingSelector(key);
                if (cacheItem != null)
                    cache.Set(cacheKey, cacheItem);
            }

            return cacheItem;
        }

        /// <summary>
        /// 从缓存获取对象
        /// </summary>
        public static TEntity GetItem<TEntity, TKey1, TKey2>(
            this ICache cache,
            TKey1 key1,
            TKey2 key2,
            string cacheKeyFormat,
            int secodesToLive,
            Func<TKey1, TKey2, TEntity> missingSelector)
        {
            string cacheKey = cacheKeyFormat.Format(key1, key2);
            var cacheItem = cache.Get<TEntity>(cacheKey);
            if (cacheItem == null)
            {
                cacheItem = missingSelector(key1, key2);
                if (cacheItem != null)
                    cache.Set(cacheKey, cacheItem);
            }

            return cacheItem;
        }

        /// <summary>
        /// 从缓存获取对象
        /// </summary>
        public static TEntity GetItem<TEntity, TKey1, TKey2, TKey3>(
            this ICache cache,
            TKey1 key1,
            TKey2 key2,
            TKey3 key3,
            string cacheKeyFormat,
            int secodesToLive,
            Func<TKey1, TKey2, TKey3, TEntity> missingSelector)
        {
            string cacheKey = cacheKeyFormat.Format(key1, key2, key3);
            var cacheItem = cache.Get<TEntity>(cacheKey);
            if (cacheItem == null)
            {
                cacheItem = missingSelector(key1, key2, key3);
                if (cacheItem != null)
                    cache.Set(cacheKey, cacheItem);
            }

            return cacheItem;
        }

        #endregion

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

        static Dictionary<string, TKey> CreateKeyDictionary<TKey>(IEnumerable<TKey> keys, Func<TKey, string> cacheKeyFormating)
        {
            return keys.Distinct().ToDictionary(o => cacheKeyFormating(o), o => o);
        }

        static Dictionary<string, TKey> CreateKeyDictionary<TKey>(IEnumerable<TKey> keys, string cacheKeyFormat)
        {
            return keys.Distinct().ToDictionary(o => cacheKeyFormat.Format(o), o => o);
        }

        static IEnumerable<TKey> GetMissingKeys<TKey>(Dictionary<string, TKey> keyHash, IEnumerable<string> missing)
        {
            List<TKey> missingKeys = new List<TKey>();
            foreach (string key in missing)
                missingKeys.Add(keyHash[key]);
            return missingKeys;
        }
    }
}
