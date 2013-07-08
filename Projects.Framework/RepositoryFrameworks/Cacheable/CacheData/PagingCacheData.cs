using Projects.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Projects.Framework
{
    /// <summary>
    /// 对象分页查询结果的缓存对象
    /// </summary>
    public class PagingCacheData : ListCacheData
    {
        protected PagingCacheData()
        { }

        public PagingCacheData(IEnumerable ids)
            : base(ids)
        {
        }

        public int TotalCount { get; set; }
    }
}
