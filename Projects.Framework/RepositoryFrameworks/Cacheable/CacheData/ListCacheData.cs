using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Tool;

namespace Projects.Framework
{
    /// <summary>
    /// 对象查询结果的缓存对象
    /// </summary>
    public class ListCacheData : IQueryTimestamp
    {
        protected ListCacheData()
        {
        }

        public ListCacheData(IEnumerable ids)
        {
            Timestamp = NetworkTime.Now.Ticks;
            Ids = String.Join(",", ids.Cast<object>().ToArray());
        }

        public ShardParams ShardParams { get; set; }

        /// <summary>
        /// 该标识需要进行类型转换 HHB 2013-4-26 update for redis hash
        /// </summary>
        public string Ids { get; set; }

        public long Timestamp { get; set; }

        public IEnumerable CastIds(Type type)
        {
            if (!String.IsNullOrEmpty(Ids))
            {
                var ids = Ids.Split(',');
                return ids.Select(o => Convert.ChangeType(o, type)).ToList();
            }
            return new ArrayList();
        }
    }
}
