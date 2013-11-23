using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    /// <summary>
    /// 缓存工厂接口
    /// </summary>
    public interface ICacheFactory
    {
        /// <summary>
        /// 根据缓存对象名称获取缓存者对象
        /// </summary>
        ICache GetCacher(string name);
        /// <summary>
        /// 注册缓存者
        /// </summary>
        void Register(string name, ICache cache);
    }
}
