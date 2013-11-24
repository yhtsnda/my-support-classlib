using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Avalon.Framework
{
    /// <summary>
    /// 进行对象间关联的缓存定义
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TJoin"></typeparam>
    public class ClassJoinCacheDefine<TEntity, TJoin> where TJoin : class
    {
        ClassJoinCacheDefineMetadata metadata;

        internal ClassJoinCacheDefine(ClassJoinCacheDefineMetadata metadata)
        {
            this.metadata = metadata;
        }

        /// <summary>
        /// 表示当前缓存依赖于给定的类型
        /// </summary>
        /// <typeparam name="TRegionEntity"></typeparam>
        /// <returns></returns>
        public ClassJoinCacheDefine<TEntity, TJoin> Depend<TRegionEntity>()
        {
            metadata.CacheDepends.Add(ClassJoinCacheDependDefineMetadata.Create<TRegionEntity>());
            return this;
        }

        /// <summary>
        /// 表示当前缓存依赖于给定类型的属性
        /// </summary>
        /// <typeparam name="TRegionEntity"></typeparam>
        /// <param name="property"></param>
        /// <param name="valueFunc"></param>
        /// <returns></returns>
        public ClassJoinCacheDefine<TEntity, TJoin> Depend<TRegionEntity>(string regionName, Func<TEntity, object> valueFunc)
        {
            metadata.CacheDepends.Add(ClassJoinCacheDependDefineMetadata.Create<TRegionEntity, TEntity>(regionName, valueFunc));
            return this;
        }
    }
}
