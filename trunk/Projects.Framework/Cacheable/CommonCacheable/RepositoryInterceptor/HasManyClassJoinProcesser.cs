using Projects.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Projects.Framework
{
    internal class HasManyClassJoinProcesser<TEntity, TJoin> : IClassJoinDataProcesser where TJoin : class
    {
        Func<TEntity, ISpecification<TJoin>, ISpecification<TJoin>> specAction;
        ClassJoinDefineMetadata metadata;

        public HasManyClassJoinProcesser(Func<TEntity, ISpecification<TJoin>, ISpecification<TJoin>> specAction, ClassJoinDefineMetadata metadata)
        {
            this.specAction = specAction;
            this.metadata = metadata;
        }

        public IEnumerable<TJoin> Process(TEntity entity)
        {
            ISpecification<TJoin> specification = SpecificationFactory.Create<TJoin>();
            IRepository<TJoin> repository = DependencyResolver.Resolve<IRepository<TJoin>>();
            if (repository == null)
                throw new PlatformException("类型 {0} 未实现仓储。", typeof(TJoin).FullName);
            specification = specAction(entity, specification);

            // 查询缓存
            if (metadata.JoinCache.IsCacheable)
                return repository
                    .Cache()
                    .Depend(metadata.JoinCache.GetCacheRegions(entity))
                    .Proxy()
                    .FindAll(specification);

            return repository.FindAll(specification);
        }

        object IClassJoinDataProcesser.Process(object entity)
        {
            return Process((TEntity)entity);
        }
    }
}