﻿using Castle.DynamicProxy;
using Projects.Tool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Tool.Reflection;

namespace Projects.Framework
{
    internal class EntityPagingCacheableRepositoryProvider<TEntity> : AbstractCacheableRepositoryProvider<TEntity>
         where TEntity : class
    {
        public override bool IsMatch()
        {
            return Invocation.Method.Name == "FindPaging" && Invocation.Arguments.Length == 1 && Invocation.Arguments[0] is ISpecification<TEntity>;
        }

        public override IQueryTimestamp GetCacheData()
        {
            return RepositoryFramework.GetCacherForQuery(DefineMetadata).Get<PagingCacheData>(CacheKey);
        }

        public override void ProcessCache(IQueryTimestamp cacheData)
        {
            var cd = (PagingCacheData)cacheData;
            IRepository<TEntity> repository = (IRepository<TEntity>)Invocation.InvocationTarget;
            var ids = cd.CastIds(DefineMetadata.IdMember.PropertyType);
            Invocation.ReturnValue = new PagingResult<TEntity>(cd.TotalCount, repository.GetList(cd.ShardParams, ids));
        }

        public override bool ProcessSource()
        {
            var metadata = DefineMetadata;
            Invocation.Proceed();
            ICache queryCache = RepositoryFramework.GetCacherForQuery(metadata);
            ICache cache = RepositoryFramework.GetCacher(metadata);

            //IList接口
            PagingResult<TEntity> paging = (PagingResult<TEntity>)Invocation.ReturnValue;

            var ta = TypeAccessor.GetAccessor(metadata.EntityType);
            var idGetter = ta.GetPropertyGetter(metadata.IdMember.Name);

            var spec = (ISpecification<TEntity>)Invocation.Arguments[0];
            if (spec == null)
                throw new ArgumentException("第一个参数类型必须为 ISpecification<TEntity>");

            //加入缓存
            cache.SetBatch(paging.Items.Cast<object>().Select(o => new CacheItem<object>(metadata.GetCacheKey(o), metadata.CloneEntity(o))), 1200);
            queryCache.Set(CacheKey, new PagingCacheData(paging.Items.Select(o => idGetter(o)))
            {
                ShardParams = spec.ShardParams,
                TotalCount = paging.TotalCount
            });
            return true;
        }
    }
}
