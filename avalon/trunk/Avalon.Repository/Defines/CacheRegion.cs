using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Avalon.Framework
{
    /// <summary>
    /// 缓存区域的数据
    /// </summary>
    public class CacheRegion
    {
        private CacheRegion() { }

        /// <summary>
        /// 区域名称
        /// </summary>
        public string RegionName { get; private set; }

        /// <summary>
        /// 缓存区域的值
        /// </summary>
        public string CacheKey { get; private set; }

        public Type EntityType { get; private set; }

        /// <summary>
        /// 创建缓存区域定义对象
        /// </summary>
        /// <typeparam name="TRegionEntity"></typeparam>
        /// <returns></returns>
        public static CacheRegion Create<TRegionEntity>()
        {
            return Create<TRegionEntity>(null, null);
        }

        /// <summary>
        /// 创建缓存区域定义对象
        /// </summary>
        public static CacheRegion Create<TRegionEntity>(string regionName, object value)
        {
            return Create(typeof(TRegionEntity), regionName, value);
        }

        /// <summary>
        /// 创建缓存区域定义对象
        /// </summary>
        public static CacheRegion Create(Type entityType, string regionName, object value)
        {
            if (String.IsNullOrEmpty(regionName))
                regionName = CacheKeyUtil.DefaultRegionName;

            return new CacheRegion()
            {
                RegionName = regionName,
                CacheKey = CacheKeyUtil.GetRegionCacheKey(entityType, regionName, value),
                EntityType = entityType
            };
        }

        /// <summary>
        /// 创建缓存区域定义对象
        /// </summary>
        public static CacheRegion CreateByCacheKey<TRegionEntity>(string regionName, string cacheKey)
        {
            return CreateByCacheKey(typeof(TRegionEntity), regionName, cacheKey);
        }

        public static CacheRegion CreateByCacheKey(Type entityType, string regionName, string cacheKey)
        {
            if (String.IsNullOrEmpty(regionName))
                throw new ArgumentNullException("regionName");

            if (String.IsNullOrEmpty(cacheKey))
                throw new ArgumentNullException("cacheKey");

            return new CacheRegion()
            {
                RegionName = regionName,
                CacheKey = cacheKey,
                EntityType = entityType
            };
        }

    }
}
