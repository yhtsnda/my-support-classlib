using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 自定义类型定义基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class ClassDefine<TEntity> : ICacheMetadataProvider
    {
        private const string DEFAULT_REGION_NAME = "_default_";
        private ClassDefineMetadata mMetadata = new ClassDefineMetadata(typeof(TEntity));

        public void Id(Expression<Func<TEntity, object>> memberExpression)
        {
            mMetadata.IdMember = ReflectionHelper.GetMember(memberExpression.Body);
        }

        public void Cache()
        {
            mMetadata.IsCacheable = true;
        }

        public void CacheRegion()
        {
            CacheRegion(null, null);
        }

        public void Table(string table)
        {
            mMetadata.Table = table;
        }

        /// <summary>
        /// 定义对象属性为缓存区域
        /// </summary>
        /// <param name="dependProperty"></param>
        public void CacheRegion(string regionName, Func<TEntity, object> regionValueFunc)
        {
            regionName = regionName ?? DEFAULT_REGION_NAME;

            if (mMetadata.CacheRegionDefines.ContainsKey(regionName))
                throw new ArgumentException("已经定义了命名为 {0} 的缓存区域。", regionName);

            mMetadata.CacheRegionDefines.Add(regionName, ClassCacheRegionDefineMetadata.Create<TEntity, TEntity>(regionName, regionValueFunc));
        }

        /// <summary>
        /// 定义单一对象的对象关系。
        /// </summary>
        /// <typeparam name="TJoin"></typeparam>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
        public ClassJoinDefine<TEntity, TJoin> JoinOne<TJoin>(
            Expression<Func<TEntity, TJoin>> memberExpression) where TJoin : class
        {
            var method = ReflectionHelper.GetMethod(memberExpression.Body);
            if (method == null)
                throw new ArgumentNullException("method");

            return new ClassJoinDefine<TEntity, TJoin>(ClassJoinType.HasOne, method, mMetadata);
        }

        /// <summary>
        /// 定义集合对象的对象关系。
        /// </summary>
        /// <typeparam name="TJoin"></typeparam>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
        public ClassJoinDefine<TEntity, TJoin> JoinMany<TJoin>(
            Expression<Func<TEntity, IEnumerable<TJoin>>> memberExpression) where TJoin : class
        {
            var method = ReflectionHelper.GetMethod(memberExpression.Body);
            if (method == null)
                throw new ArgumentNullException("method");

            return new ClassJoinDefine<TEntity, TJoin>(ClassJoinType.HasMany, method, mMetadata);
        }

        ClassDefineMetadata ICacheMetadataProvider.GetCacheMetadata()
        {
            if (mMetadata.IsCacheable && mMetadata.IdMember == null)
                throw new ArgumentException("当使用对象缓存必须指定主键属性。");

            return mMetadata;
        }
    }
}
