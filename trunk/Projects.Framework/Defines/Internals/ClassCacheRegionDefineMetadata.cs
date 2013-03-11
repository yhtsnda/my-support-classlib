using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 对象缓存区域的定义
    /// </summary>
    public class ClassCacheRegionDefineMetadata
    {
        private ClassCacheRegionDefineMetadata()
        {
        }

        /// <summary>
        /// 实体类型
        /// </summary>
        public Type EntityType { get; private set; }

        /// <summary>
        /// 区域的名称
        /// </summary>
        public string RegionName { get; private set; }

        /// <summary>
        /// 获取区域参数的委托
        /// </summary>
        public Func<object, object> RegionValueFunc { get; private set; }

        /// <summary>
        /// 根据对象获取缓存区域的键
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string GetRegionCacheKey(object entity)
        {
            if (CacheKeyUtil.DefaultRegionName == RegionName)
                return CacheKeyUtil.GetRegionCacheKey(EntityType, RegionName, null);

            return CacheKeyUtil.GetRegionCacheKey(EntityType, RegionName, RegionValueFunc(entity));
        }

        public static ClassCacheRegionDefineMetadata Create<TRegionEntity>()
        {
            return new ClassCacheRegionDefineMetadata()
            {
                EntityType = typeof(TRegionEntity),
                RegionName = CacheKeyUtil.DefaultRegionName
            };
        }

        public static ClassCacheRegionDefineMetadata Create<TRegionEntity, TValueEntity>(string regionName, Func<TValueEntity, object> regionValueFunc)
        {
            if (String.IsNullOrEmpty(regionName))
                throw new ArgumentNullException("regionName");

            if (regionName != CacheKeyUtil.DefaultRegionName && regionValueFunc == null)
                throw new ArgumentNullException("regionValueFunc");

            var define = new ClassCacheRegionDefineMetadata()
            {
                EntityType = typeof(TRegionEntity),
                RegionName = regionName,
                RegionValueFunc = (o) => regionValueFunc((TValueEntity)o)
            };
            return define;
        }
    }
}
