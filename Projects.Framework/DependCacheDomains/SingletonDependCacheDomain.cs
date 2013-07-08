using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Tool;

namespace Projects.Framework
{
    public class DependCacheDomain<TEntity> : CacheDomain<TEntity>
        where TEntity : IQueryTimestamp
    {
        Action<CacheDependDefine> dependAction;

        public DependCacheDomain(Func<TEntity> missingItemHandler, string cacheName = null, string cacheKeyFormat = null, int secondesToLive = 0, bool contextCacheEnabled = true)
            : base(missingItemHandler, cacheName, cacheKeyFormat, secondesToLive, contextCacheEnabled)
        {
        }

        public override TEntity GetItem()
        {
            var item = base.GetItem();
            if (CheckEntity(item))
                return item;

            return base.GetItem();
        }

        public override TEntity GetItemFromCache()
        {
            var item = base.GetItemFromCache();
            if (CheckEntity(item))
                return item;

            return default(TEntity);
        }

        public DependCacheDomain<TEntity> Depend(Action<CacheDependDefine> cdd)
        {
            dependAction = cdd;
            return this;
        }

        bool CheckEntity(IQueryTimestamp data)
        {
            if (data == null)
                return true;

            var cdd = new CacheDependDefine();
            dependAction(cdd);
            var keys = cdd.GetCacheKey();

            ICache dependCache = RepositoryFramework.GetCacherForCacheRegion();
            var values = dependCache.GetBatch<long>(keys);
            var isValid = values.Count() == 0 || values.All(o => o < data.Timestamp);
            if (!isValid)
                RemoveCache();

            return isValid;
        }
    }
}
