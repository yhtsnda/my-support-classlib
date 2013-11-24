using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    internal class HasManyByForeignKeyClassJoinProcessor<TEntity, TJoin> : IClassJoinDataProcessor where TJoin : class
    {
        ClassJoinDefineMetadata metadata;
        Action<TEntity, HasManyByForeignKeyDefine> foreignKeyDefine;

        public HasManyByForeignKeyClassJoinProcessor(Action<TEntity, HasManyByForeignKeyDefine> foreignKeyDefine, ClassJoinDefineMetadata metadata)
        {
            this.foreignKeyDefine = foreignKeyDefine;
            this.metadata = metadata;
        }

        public IList<TJoin> Process(TEntity entity)
        {
            IRepository<TJoin> repository = DependencyResolver.Resolve<IRepository<TJoin>>();
            if (repository == null)
                throw new AvalonException("类型 {0} 未实现仓储。", typeof(TJoin).FullName);

            HasManyByForeignKeyDefine define = new HasManyByForeignKeyDefine();
            foreignKeyDefine(entity, define);

            return repository.GetList(define.ShardParams, define.Ids);
        }

        object IClassJoinDataProcessor.Process(object entity)
        {
            return Process((TEntity)entity);
        }
    }
}
