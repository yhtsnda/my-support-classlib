using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 支持时间戳的查询数据
    /// </summary>
    public interface IQueryTimestamp
    {
        long Timestamp { get; }
    }
}
