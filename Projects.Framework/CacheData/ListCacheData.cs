using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nd.Tool;

namespace Projects.Framework
{
    /// <summary>
    /// 对象查询结果的缓存对象
    /// </summary>
    public class ListCacheData : IQueryTimestamp
    {
        public ListCacheData()
        {
            Timestamp = NetworkTime.Now.Ticks;
        }

        public ShardParams ShardParams { get; set; }

        /// <summary>
        /// TODO 类型转换？
        /// </summary>
        public object[] Ids { get; set; }

        public long Timestamp { get; set; }
    }
}
