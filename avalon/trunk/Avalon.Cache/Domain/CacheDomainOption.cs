using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    public abstract class CacheDomainOption
    {
        public string CacheName { get; set; }

        public string CacheKeyFormat { get; set; }

        public int SecondesToLive { get; set; }

        public virtual string GetCacheFullName()
        {
            return CacheName;
        }

        protected virtual string GetCacheKeyFormat()
        {
            return CacheKeyFormat;
        }
    }

    public class CacheDomainOption<TEntity> : CacheDomainOption
    {

        public Func<TEntity> MissingItemHandler { get; set; }

        public override string GetCacheFullName()
        {
            return CacheName ?? CacheDomainUtil.CreateCacheName<TEntity>();
        }

        protected override string GetCacheKeyFormat()
        {
            return CacheKeyFormat ?? CacheDomainUtil.CreateCacheKeyFormat<TEntity>();
        }

        public virtual string GetCacheKey()
        {
            return GetCacheFullName();
        }
    }

    public class CacheDomainOption<TEntity, TKey> : CacheDomainOption
    {
        public Func<TEntity, TKey> EntityKeySelector { get; set; }

        public Func<TKey, TEntity> MissingItemHandler { get; set; }

        public Func<IEnumerable<TKey>, IEnumerable<TEntity>> MissingItemsHandler { get; set; }

        public override string GetCacheFullName()
        {
            return CacheName ?? CacheDomainUtil.CreateCacheName<TEntity, TKey>();
        }

        protected override string GetCacheKeyFormat()
        {
            return CacheKeyFormat ?? CacheDomainUtil.CreateCacheKeyFormat<TEntity, TKey>();
        }

        public string GetCacheKey(TKey key)
        {
            return String.Format(GetCacheKeyFormat(), key);
        }

        public IEnumerable<TEntity> GetMissingItems(params TKey[] keys)
        {
            if (MissingItemHandler == null && MissingItemsHandler == null)
                throw new ArgumentNullException("MissingItemHandler 与 MissingItemsHandler 至少有一个不能为 Null.");

            if (MissingItemsHandler != null)
            {
                if (keys.Length == 1 && MissingItemHandler != null)
                    return new TEntity[] { MissingItemHandler(keys[0]) };
                return MissingItemsHandler(keys);
            }
            return keys.Select(o => MissingItemHandler(o));
        }
    }

    public class CacheDomainOption<TEntity, TParam, TKey> : CacheDomainOption
    {
        public Func<TEntity, TKey> EntityKeySelector { get; set; }

        public Func<TParam, TKey, TEntity> MissingItemHandler { get; set; }

        public Func<TParam, IEnumerable<TKey>, IEnumerable<TEntity>> MissingItemsHandler { get; set; }


        public override string GetCacheFullName()
        {
            return CacheName ?? CacheDomainUtil.CreateCacheName<TEntity, TParam, TKey>();
        }

        protected override string GetCacheKeyFormat()
        {
            return CacheKeyFormat ?? CacheDomainUtil.CreateCacheKeyFormat<TEntity, TParam, TKey>();
        }

        public string GetCacheKey(TParam param, TKey key)
        {
            return String.Format(GetCacheKeyFormat(), param, key);
        }

        public IEnumerable<TEntity> GetMissingItems(TParam param, params TKey[] keys)
        {
            if (MissingItemHandler == null && MissingItemsHandler == null)
                throw new ArgumentNullException("MissingItemHandler 与 MissingItemsHandler 至少有一个不能为 Null.");

            if (MissingItemsHandler != null)
            {
                if (keys.Length == 1 && MissingItemHandler != null)
                    return new TEntity[] { MissingItemHandler(param, keys[0]) };
                return MissingItemsHandler(param, keys);
            }
            return keys.Select(o => MissingItemHandler(param, o));
        }
    }

    public class CacheDomainOption<TEntity, TParam1, TParam2, TKey> : CacheDomainOption
    {
        public Func<TEntity, TKey> EntityKeySelector { get; set; }

        public Func<TParam1, TParam2, TKey, TEntity> MissingItemHandler { get; set; }

        public Func<TParam1, TParam2, IEnumerable<TKey>, IEnumerable<TEntity>> MissingItemsHandler { get; set; }

        public override string GetCacheFullName()
        {
            return CacheName ?? CacheDomainUtil.CreateCacheName<TEntity, TParam1, TParam2, TKey>();
        }

        protected override string GetCacheKeyFormat()
        {
            return CacheKeyFormat ?? CacheDomainUtil.CreateCacheKeyFormat<TEntity, TParam1, TParam2, TKey>();
        }

        public string GetCacheKey(TParam1 param1, TParam2 param2, TKey key)
        {
            return String.Format(GetCacheKeyFormat(), param1, param2, key);
        }

        public IEnumerable<TEntity> GetMissingItems(TParam1 param1, TParam2 param2, params TKey[] keys)
        {
            if (MissingItemHandler == null && MissingItemsHandler == null)
                throw new ArgumentNullException("MissingItemHandler 与 MissingItemsHandler 至少有一个不能为 Null.");

            if (MissingItemsHandler != null)
            {
                if (keys.Length == 1 && MissingItemHandler != null)
                    return new TEntity[] { MissingItemHandler(param1, param2, keys[0]) };
                return MissingItemsHandler(param1, param2, keys);
            }
            return keys.Select(o => MissingItemHandler(param1, param2, o));
        }
    }
}
