using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Projects.Framework
{
    public class ClassJoinDefineMetadata
    {
        public ClassJoinDefineMetadata()
        {
            CacheDepends = new List<ClassCacheRegionDefineMetadata>();
        }

        public MethodInfo Method
        {
            get;
            set;
        }

        internal IClassJoinProcesser Processer
        {
            get;
            set;
        }

        public bool IsCacheable
        {
            get;
            set;
        }

        public List<ClassCacheRegionDefineMetadata> CacheDepends
        {
            get;
            private set;
        }

        public List<CacheRegion> GetCacheRegions(object entity)
        {
            return CacheDepends.Select(o => CacheRegion.
                Create(o.RegionName, o.GetRegionCacheKey(entity))).ToList();
        }
    }
}
