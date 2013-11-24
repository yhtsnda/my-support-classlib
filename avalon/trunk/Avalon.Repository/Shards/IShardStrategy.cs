using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Shards
{
    /// <summary>
    /// 定义分区分表策略接口
    /// </summary>
    public interface IShardStrategy
    {
        /// <summary>
        /// 获取分区标识
        /// </summary>
        ShardId GetShardId(ShardParams shardParams);

        /// <summary>
        /// 获取分表标识
        /// </summary>
        /// <param name="shardParams"></param>
        /// <returns></returns>
        PartitionId GetPartitionId(ShardParams shardParams);


        //IEnumerable<PartitionId> GetPartitionIdList(ShardId shardId);
    }
}
