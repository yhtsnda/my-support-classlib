﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework.Shards
{
    public interface IShardSessionFactory
    {
        ISpecificationProvider SpecificationProvider { get; }
        IShardSession<TEntity> OpenSession<TEntity>(ShardParams shardParams);
    }
}
