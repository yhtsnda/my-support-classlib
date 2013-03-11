using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    public class CacheDependDefine
    {
        List<CacheRegion> regions = new List<CacheRegion>();

        public CacheDependDefine Depend<TRegionEntity>()
        {
            regions.Add(CacheRegion.Create<TRegionEntity>());
            return this;
        }

        public CacheDependDefine Depend<TRegionEntity>(string regionName, object value)
        {
            regions.Add(CacheRegion.Create<TRegionEntity>(regionName, value));
            return this;
        }

        internal IEnumerable<string> GetCacheKey()
        {
            return regions.Select(o => o.CacheKey);
        }
    }
}
