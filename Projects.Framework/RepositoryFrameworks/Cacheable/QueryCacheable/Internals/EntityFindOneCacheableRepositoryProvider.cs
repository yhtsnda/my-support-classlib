﻿using Castle.DynamicProxy;
using Projects.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Tool.Reflection;

namespace Projects.Framework
{
    internal class EntityFindOneCacheableRepositoryProvider<TEntity> : AbstractCacheableRepositoryProvider<TEntity>
        where TEntity : class
    {
        public override bool IsMatch()
        {
            return Invocation.Method.Name == "FindOne" && Invocation.Arguments.Length == 1 && Invocation.Arguments[0] is ISpecification<TEntity>;
        }

        public override IQueryTimestamp GetCacheData()
        {
            return RepositoryFramework.GetCacherForQuery(DefineMetadata).Get<ListCacheData>(CacheKey);
        }

        public override void ProcessCache(IQueryTimestamp cacheData)
        {
            var cd = (ListCacheData)cacheData;
            IRepository<TEntity> repository = (IRepository<TEntity>)Invocation.InvocationTarget;
            var ids = cd.CastIds(DefineMetadata.IdMember.PropertyType);
            var m = ids.GetEnumerator();
            m.MoveNext();
            Invocation.ReturnValue = repository.Get(cd.ShardParams, m.Current);
        }

        public override bool ProcessSource()
        {
            var metadata = DefineMetadata;
            Invocation.Proceed();
            ICache queryCache = RepositoryFramework.GetCacherForQuery(metadata);
            ICache cache = RepositoryFramework.GetCacher(metadata);

            //IList接口
            TEntity item = (TEntity)Invocation.ReturnValue;
            if (item != null)
            {
                var ta = TypeAccessor.GetAccessor(metadata.EntityType);
                var idGetter = ta.GetPropertyGetter(metadata.IdMember.Name);

                var spec = (ISpecification<TEntity>)Invocation.Arguments[0];
                if (spec == null)
                    throw new ArgumentException("第一个参数类型必须为 ISpecification<TEntity>");

                //加入缓存
                cache.Set(metadata.GetCacheKey(item), metadata.CloneEntity(item), 1200);
                queryCache.Set(CacheKey, new ListCacheData(new object[] { idGetter(item) })
                {
                    ShardParams = spec.ShardParams
                });
            }
            return item != null;
        }

    }
}
