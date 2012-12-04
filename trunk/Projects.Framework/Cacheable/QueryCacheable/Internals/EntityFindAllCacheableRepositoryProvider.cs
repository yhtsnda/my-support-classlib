using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.DynamicProxy;
using Projects.Tool;

namespace Projects.Framework
{
    internal class EntityFindAllCacheableRepositoryProvider<TEntity> : AbstractCacheableRepositoryProvider<TEntity>
        where TEntity : class
    {
        public override bool IsMatch()
        {
            return Invocation.Method.Name == "FindAll" && Invocation.Arguments.Length == 1 && Invocation.Arguments[0] is ISpecification<TEntity>;
        }

        public override IQueryTimestamp GetCacheData()
        {
            return CacheManager.GetCacher(CacheMetadata.EntityType).Get<ListCacheData>(CacheKey);
        }

        public override void ProcessCache(IQueryTimestamp cacheData)
        {
            var cd = (ListCacheData)cacheData;
            IRepository<TEntity> repository = (IRepository<TEntity>)Invocation.InvocationTarget;
            Invocation.ReturnValue = repository.GetList(cd.ShardParams, cd.Ids.Cast<int>().ToArray());
        }

        public override bool ProcessSource()
        {
            var metadata = CacheMetadata;
            Invocation.Proceed();
            ICache cache = CacheManager.GetCacher(CacheMetadata.EntityType);

            //IList接口
            IList items = (IList)Invocation.ReturnValue;
            var pa = PropertyAccessorFactory.GetPropertyAccess(metadata.EntityType);
            var getter = pa.GetGetter(metadata.IdMember.Name);

            var spec = (ISpecification<TEntity>)Invocation.Arguments[0];
            if (spec == null)
                throw new ArgumentException("第一个参数类型必须为 ISpecification<TEntity>");

            //加入缓存
            cache.SetBatch(items.Cast<object>().Select(o => new CacheItem<CacheData>(metadata.GetCacheKey(o), CacheData.FromEntity(o))), 1200);
            cache.Set(CacheKey, new ListCacheData()
            {
                ShardParams = spec.ShardParams,
                Ids = items.Cast<object>().Select(o => getter.Get(o)).ToArray()
            });
            return true;
        }
    }
}
