using Projects.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Projects.Tool.Shards
{
    /// <summary>
    /// shard session工厂对象接口
    /// </summary>
    public interface IShardSessionFactory
    {
        ISpecificationProvider SpecificationProvider { get; }

        IShardSession<TEntity> OpenSession<TEntity>(ShardParams shardParams);
    }
}
