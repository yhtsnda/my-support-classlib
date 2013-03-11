using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    internal class HasOneByForeignKeyClassJoinProcesser<TEntity, TJoin> : IClassJoinDataProcesser where TJoin : class
    {
        ClassJoinDefineMetadata metadata;
        Action<TEntity, HasOneByForeignKeyDefine> foreignKeyDefine;

        public HasOneByForeignKeyClassJoinProcesser(Action<TEntity, HasOneByForeignKeyDefine> foreignKeyDefine, ClassJoinDefineMetadata metadata)
        {
            this.foreignKeyDefine = foreignKeyDefine;
            this.metadata = metadata;
        }

        public TJoin Process(TEntity entity)
        {
            IRepository<TJoin> repository = DependencyResolver.Resolve<IRepository<TJoin>>();
            if (repository == null)
                throw new PlatformException("类型 {0} 未实现仓储。", typeof(TJoin).FullName);

            HasOneByForeignKeyDefine define = new HasOneByForeignKeyDefine();
            foreignKeyDefine(entity, define);

            return repository.Get(define.ShardParams, define.Id);
        }

        object IClassJoinDataProcesser.Process(object entity)
        {
            return Process((TEntity)entity);
        }
    }
}
