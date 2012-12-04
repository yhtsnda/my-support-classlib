using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Projects.Framework
{
    internal class HasManyClassJoinProcesser<TEntity, TJoin> : IClassJoinProcesser where TJoin : class
    {
        private Func<TEntity, ISpecification<TJoin>, ISpecification<TJoin>> mSpecAction;
        private ClassJoinDefineMetadata mMetadata;

        public HasManyClassJoinProcesser(Func<TEntity, ISpecification<TJoin>, ISpecification<TJoin>> specAction, ClassJoinDefineMetadata metadata)
        {
            this.mSpecAction = specAction;
            this.mMetadata = metadata;
        }

        public IEnumerable<TJoin> Process(TEntity entity)
        {
            ISpecification<TJoin> specification = SpecificationFactory.Create<TJoin>();
            IRepository<TJoin> repository = DependencyResolver.Resolve<IRepository<TJoin>>();
            specification = mSpecAction(entity, specification);

            // 查询缓存
            if (mMetadata.IsCacheable)
                return repository
                    .Cache()
                    .Depend(mMetadata.GetCacheRegions(entity))
                    .Proxy()
                    .FindAll(specification);

            return repository.FindAll(specification);
        }

        object IClassJoinProcesser.Process(object entity)
        {
            return Process((TEntity)entity);
        }
    }
}
