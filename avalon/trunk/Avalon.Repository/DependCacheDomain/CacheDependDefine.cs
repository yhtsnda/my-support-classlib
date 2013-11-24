using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    public class CacheDependDefine
    {
        List<CacheRegion> regions = new List<CacheRegion>();

        public CacheDependDefine Depend<TRegionEntity>()
        {
            var region = CacheRegion.Create<TRegionEntity>();
            CheckCacheRegion<TRegionEntity>(region);
            regions.Add(region);
            return this;
        }

        public CacheDependDefine Depend<TRegionEntity>(string regionName, object value)
        {
            var region = CacheRegion.Create<TRegionEntity>(regionName, value);
            CheckCacheRegion<TRegionEntity>(region);
            regions.Add(region);
            return this;
        }

        internal IEnumerable<string> GetCacheKey()
        {
            return regions.Select(o => o.CacheKey);
        }

        void CheckCacheRegion<TRegionEntity>(CacheRegion region)
        {
            var entityType = typeof(TRegionEntity);
            var metadata = RepositoryFramework.GetDefineMetadataAndCheck(entityType);
            metadata.CheckCacheRegions(new CacheRegion[] { region });
        }
    }
}
