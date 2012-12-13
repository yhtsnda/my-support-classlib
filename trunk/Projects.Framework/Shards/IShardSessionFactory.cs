using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Framework.Specification;

namespace Projects.Framework.Shards
{
    public interface IShardSessionFactory
    {
        ISpecificationProvider SpecificationProvider { get; }
        IShardSession<TEntity> OpenSession<TEntity>(ShardParams shardParams);
    }
}
