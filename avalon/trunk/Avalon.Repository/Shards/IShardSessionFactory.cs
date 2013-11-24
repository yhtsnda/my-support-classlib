using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Avalon.Framework.Shards
{
    /// <summary>
    /// shard session工厂对象接口
    /// </summary>
    public interface IShardSessionFactory
    {
        ISpecificationProvider SpecificationProvider { get; }

        IShardSession<TEntity> OpenSession<TEntity>(ShardParams shardParams);

        IShardSession OpenSession(Type entityType, ShardParams shardParams);

        object GetConnectionManager();
    }
}
