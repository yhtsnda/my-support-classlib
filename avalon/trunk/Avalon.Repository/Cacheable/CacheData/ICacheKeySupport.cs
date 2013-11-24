using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    /// <summary>
    /// 对象支持转化为缓存键的接口
    /// </summary>
    public interface ICacheKeySupport
    {
        IDictionary<string, Object> Serialize();
    }
}
