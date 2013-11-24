using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    public class QueryCacheData : IQueryTimestamp
    {
        public QueryCacheData()
        {
            Timestamp = NetworkTime.Now.Ticks;
        }

        /// <summary>
        /// 缓存的对象
        /// </summary>
        public object Data { get; set; }

        public long Timestamp { get; set; }
    }
}
