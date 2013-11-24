using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Avalon.Framework
{
    /// <summary>
    /// 进行类型扩展定义的定义基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class ClassDefine<TEntity> : ICacheMetadataProvider
    {
        const string DefaultRegionName = "_default_";

        ClassDefineMetadata metadata = new ClassDefineMetadata(typeof(TEntity));

        public ClassDefine()
        {
            metadata.FetchableObject = this as IFetchable;
        }

        public void Id(Expression<Func<TEntity, object>> memberExpression)
        {
            metadata.IdMember = ReflectionHelper.GetProperty(memberExpression.Body);
            Map(memberExpression);
        }

        /// <summary>
        /// 表示对象支持缓存
        /// </summary>
        public void Cache()
        {
            metadata.IsCacheable = true;
        }

        public void Cache(ICache cache)
        {
            Cache();
        }

        public void DisableContextCache()
        {
            metadata.IsContextCacheable = false;
        }

        /// <summary>
        /// 定义对象为缓存区域
        /// </summary>
        public void CacheRegion()
        {
            CacheRegion(null, null);
        }

        public void Table(string table)
        {
            metadata.Table = table;
        }

        /// <summary>
        /// 定义对象属性为缓存区域
        /// </summary>
        /// <param name="dependProperty"></param>
        public void CacheRegion(string regionName, Func<TEntity, object> regionValueFunc)
        {
            regionName = regionName ?? DefaultRegionName;

            if (metadata.CacheRegionDefines.ContainsKey(regionName))
                throw new ArgumentException("已经定义了命名为 {0} 的缓存区域。", regionName);

            metadata.CacheRegionDefines.Add(regionName, ClassCacheRegionDefineMetadata.Create<TEntity, TEntity>(regionName, regionValueFunc));
        }

        /// <summary>
        /// 定义数据属性
        /// </summary>
        /// <param name="memberExpr"></param>
        public void Map(Expression<Func<TEntity, object>> memberExpr)
        {
            var property = ReflectionHelper.GetProperty(memberExpr.Body);
            if (metadata.DataProperties.Contains(property))
                throw new AvalonException("类型 {1} 属性 {0} 已经映射。", property.Name, typeof(TEntity));
            if (!property.CanRead || !property.CanWrite)
                throw new AvalonException("领域对象的数据属性 {0}.{1} 必须是可读可写，但可以使用protected/internal。", typeof(TEntity).FullName, property.Name);

            metadata.DataProperties.Add(property);
        }

        /// <summary>
        /// 定义单一对象的对象关系。
        /// </summary>
        /// <typeparam name="TJoin"></typeparam>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
        public ClassJoinOneDefine<TEntity, TJoin> JoinOne<TJoin>(
            Expression<Func<TEntity, TJoin>> memberExpr,
            Func<TEntity, ISpecification<TJoin>, ISpecification<TJoin>> joinExpr) where TJoin : class
        {
            var member = GetMember(memberExpr);

            return new ClassJoinOneDefine<TEntity, TJoin>(metadata, member, joinExpr);
        }

        /// <summary>
        /// 定义单一对象的对象关系。(使用外键）
        /// </summary>
        public ClassJoinOneDefine<TEntity, TJoin> JoinForeign<TJoin>(
            Expression<Func<TEntity, TJoin>> memberExpr,
            Action<TEntity, HasOneByForeignKeyDefine> foreignKeyDefine) where TJoin : class
        {
            var member = GetMember(memberExpr);

            return new ClassJoinOneDefine<TEntity, TJoin>(metadata, member, foreignKeyDefine);
        }

        public ClassJoinManyDefine<TEntity, TJoin> JoinForeignMany<TJoin>(
            Expression<Func<TEntity, IEnumerable<TJoin>>> memberExpr,
            Action<TEntity, HasManyByForeignKeyDefine> foreignKeyDefine) where TJoin : class
        {
            var member = GetMember(memberExpr);

            return new ClassJoinManyDefine<TEntity, TJoin>(metadata, member, foreignKeyDefine);
        }

        /// <summary>
        /// 定义集合对象的对象关系。
        /// </summary>
        /// <typeparam name="TJoin"></typeparam>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
        public ClassJoinManyDefine<TEntity, TJoin> JoinMany<TJoin>(
            Expression<Func<TEntity, IEnumerable<TJoin>>> memberExpr,
            Func<TEntity, ISpecification<TJoin>, ISpecification<TJoin>> joinExpr
            ) where TJoin : class
        {
            var member = GetMember(memberExpr);
            return new ClassJoinManyDefine<TEntity, TJoin>(metadata, member, joinExpr);
        }

        MemberInfo GetMember(LambdaExpression memberExpr)
        {
            var member = (MemberInfo)ReflectionHelper.GetProperty(memberExpr.Body);
            if (member == null)
                member = (MemberInfo)ReflectionHelper.GetMethod(memberExpr.Body);

            if (member == null)
                throw new ArgumentNullException("member");

            return member;
        }

        ClassDefineMetadata ICacheMetadataProvider.GetCacheMetadata()
        {
            if (metadata.IsCacheable && metadata.IdMember == null)
                throw new ArgumentException("当使用对象缓存必须指定主键属性。");

            return metadata;
        }
    }
}
