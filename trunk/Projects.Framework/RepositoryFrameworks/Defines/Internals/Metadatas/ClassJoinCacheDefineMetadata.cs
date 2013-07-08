using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    public class ClassJoinCacheDefineMetadata
    {
        internal ClassJoinCacheDefineMetadata()
        {
            CacheDepends = new List<ClassJoinCacheDependDefineMetadata>();
        }

        public bool IsCacheable { get; set; }

        public List<ClassJoinCacheDependDefineMetadata> CacheDepends
        {
            get;
            private set;
        }


        public List<CacheRegion> GetCacheRegions(object entity)
        {
            return CacheDepends.Select(o => CacheRegion.CreateByCacheKey(o.EntityType, o.RegionName, o.GetRegionCacheKey(entity))).ToList();
        }
    }
}
