using Projects.Tool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    internal class SecondLevelPersister : AbstractPersister
    {
        ClassDefineMetadata metadata;
        ICache cache;

        public SecondLevelPersister(ClassDefineMetadata metadata, ICache cache, IPersister innerPersister)
            : base(innerPersister)
        {
            this.metadata = metadata;
            this.cache = cache;
        }

        public override void Set(string cacheKey, object entity)
        {
            if (metadata.IsCacheable)
                cache.Set(cacheKey, CacheData.FromEntity(entity));
        }

        public override void SetBatch(IEnumerable<DataItem> items)
        {
            if (metadata.IsCacheable)
                cache.SetBatch<CacheData>(items.Select(o => new CacheItem<CacheData>(o.Key, CacheData.FromEntity(o.Data))), 1200);
        }

        protected override object GetInner(string cacheKey)
        {
            if (!metadata.IsCacheable)
                return null;

            return cache.Get<CacheData>(cacheKey).GetOrDefault(o => o.Convert(metadata.EntityType));
        }

        protected override IEnumerable GetListInner(DataWrapper wrapper)
        {
            if (!metadata.IsCacheable)
                return EmptyEnumerable;

            var keys = wrapper.GetMissKeys();
            return cache.GetBatch<CacheData>(keys).Select(o => o.Convert(metadata.EntityType)).ToList();
        }
    }
}
