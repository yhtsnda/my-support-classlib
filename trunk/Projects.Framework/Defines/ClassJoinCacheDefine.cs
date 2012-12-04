using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 进行对象间关联的缓存定义
    /// </summary>
    public class ClassJoinCacheDefine<TEntity, TJoin> where TJoin : class
    {
        private ClassJoinDefineMetadata mMetadata;

        internal ClassJoinCacheDefine(ClassJoinDefineMetadata metadata)
        {
            this.mMetadata = metadata;
        }

        /// <summary>
        /// 标识属性支持缓存
        /// </summary>
        public ClassJoinCacheDefine<TEntity, TJoin> Cache()
        {
            mMetadata.IsCacheable = true;
            return this;
        }

        /// <summary>
        /// 表示当前缓存依赖于给定的类型
        /// </summary>
        public ClassJoinCacheDefine<TEntity, TJoin> Depend<TRegionEntity>()
        {
            mMetadata.CacheDepends.Add(ClassCacheRegionDefineMetadata.Create<TEntity>());
            return this;
        }

        /// <summary>
        /// 表示当前缓存依赖于给定类型的属性
        /// </summary>
        public ClassJoinCacheDefine<TEntity, TJoin> Depend<TRegionEntity>(
            string regionName, Func<TEntity, object> valueFunc)
        {
            mMetadata.CacheDepends.Add(ClassCacheRegionDefineMetadata.Create<TRegionEntity, TEntity>
                (regionName, valueFunc));
            return this;
        }
    }
}
