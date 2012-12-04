using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Tool;
using Projects.Framework.Shards;

namespace Projects.Framework
{
    /// <summary>
    /// 对象分页查询结果的缓存对象
    /// </summary>
    public class PagingCacheData : IQueryTimestamp
    {
        public PagingCacheData()
        {
            Timestamp = NetworkTime.Now.Ticks;
        }

        public ShardParams ShardParams { get; set; }

        public int TotalCount { get; set; }

        public object[] Ids { get; set; }

        public long Timestamp { get; set; }
    }
}
