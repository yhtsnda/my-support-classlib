using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 使用方法及调用的参数生成缓存键的生成器
    /// </summary>
    public interface ICacheKeyGenerator
    {
        string CreateCacheKey(MethodBase method, object[] inputs);
    }

}
