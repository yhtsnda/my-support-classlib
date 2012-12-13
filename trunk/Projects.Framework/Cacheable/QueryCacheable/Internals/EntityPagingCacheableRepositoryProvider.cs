﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.DynamicProxy;
using Projects.Tool;
using Projects.Tool.Pager;
using Projects.Framework.Specification;

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
            return CacheManager.GetCacher(CacheMetadata.EntityType).Get<PagingCacheData>(CacheKey);
        }

        public override void ProcessCache(IQueryTimestamp cacheData)
        {
            var cd = (PagingCacheData)cacheData;
            IRepository<TEntity> repository = (IRepository<TEntity>)Invocation.InvocationTarget;
            Invocation.ReturnValue = new PagedList<TEntity>(null, 0, 0);
        }

        public override bool ProcessSource()
        {
            var metadata = CacheMetadata;
            Invocation.Proceed();
            ICache cache = CacheManager.GetCacher(metadata.EntityType);

            //IList接口
            PagedList<TEntity> paging = (PagedList<TEntity>)Invocation.ReturnValue;
            var pa = PropertyAccessorFactory.GetPropertyAccess(metadata.EntityType);
            var getter = pa.GetGetter(metadata.IdMember.Name);

            var spec = (ISpecification<TEntity>)Invocation.Arguments[0];
            if (spec == null)
                throw new ArgumentException("第一个参数类型必须为 ISpecification<TEntity>");

            //加入缓存
            cache.SetBatch(paging.Cast<object>()
                .Select(o => new CacheItem<CacheData>(metadata.GetCacheKey(o),
                CacheData.FromEntity(o))), 1200);
            cache.Set(CacheKey, new PagingCacheData()
            {
                ShardParams = spec.ShardParams,
                TotalCount = paging.TotalItemCount,
                Ids = paging.Cast<object>().Select(o => getter.Get(o)).ToArray()
            });
            return true;
        }
    }
}
