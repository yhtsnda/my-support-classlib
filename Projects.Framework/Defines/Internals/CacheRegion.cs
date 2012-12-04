using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 缓存区的数据
    /// </summary>
    public class CacheRegion
    {
        /// <summary>
        /// 区域名称
        /// </summary>
        public string RegionName { get; private set; }

        /// <summary>
        /// 缓存区域的键值
        /// </summary>
        public string CacheKey { get; private set; }

        private CacheRegion()
        {
        }

        public static CacheRegion Create<TRegionEntity>()
        {
            return Create<TRegionEntity>(null, null);
        }

        public static CacheRegion Create<TRegionEntity>(string regionName, object value)
        {
            if (String.IsNullOrEmpty(regionName))
                regionName = CacheKeyUtil.DefaultRegionName;

            return new CacheRegion
            {
                RegionName = regionName,
                CacheKey = CacheKeyUtil.GetRegionCacheKey(typeof(TRegionEntity), regionName, value)
            };
        }

        public static CacheRegion Create(string regionName, string cacheKey)
        {
            if (String.IsNullOrEmpty(regionName))
                throw new ArgumentNullException("regionName");
            if (String.IsNullOrEmpty(cacheKey))
                throw new ArgumentNullException("cacheKey");

            return new CacheRegion
            {
                RegionName = regionName,
                CacheKey = cacheKey
            };
        }
    }
}
