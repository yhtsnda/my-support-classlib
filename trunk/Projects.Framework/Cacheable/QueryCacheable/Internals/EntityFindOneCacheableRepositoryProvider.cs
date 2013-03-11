using Castle.DynamicProxy;
using Projects.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            return RepositoryFramework.GetCacher(DefineMetadata).Get<ListCacheData>(CacheKey);
        }

        public override void ProcessCache(IQueryTimestamp cacheData)
        {
            var cd = (ListCacheData)cacheData;
            IRepository<TEntity> repository = (IRepository<TEntity>)Invocation.InvocationTarget;
            Invocation.ReturnValue = repository.Get(cd.ShardParams, cd.Ids[0]);
        }

        public override bool ProcessSource()
        {
            var metadata = DefineMetadata;
            Invocation.Proceed();
            ICache cache = RepositoryFramework.GetCacher(metadata);

            //IList接口
            TEntity item = (TEntity)Invocation.ReturnValue;
            if (item != null)
            {
                var pa = PropertyAccessorFactory.GetPropertyAccess(metadata.EntityType);
                var getter = pa.GetGetter(metadata.IdMember.Name);

                var spec = (ISpecification<TEntity>)Invocation.Arguments[0];
                if (spec == null)
                    throw new ArgumentException("第一个参数类型必须为 ISpecification<TEntity>");

                //加入缓存
                cache.Set(metadata.GetCacheKey(item), CacheData.FromEntity(item), 1200);
                cache.Set(CacheKey, new ListCacheData()
                {
                    ShardParams = spec.ShardParams,
                    Ids = new object[] { getter.Get(item) }
                });
            }
            return item != null;
        }

    }
}
