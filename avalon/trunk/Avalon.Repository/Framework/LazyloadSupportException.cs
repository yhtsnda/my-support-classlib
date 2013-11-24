using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    /// <summary>
    /// 当前方法支持延迟，但需要框架支持，无法直接调用。
    /// </summary>
    public class LazyloadSupportException : Exception
    {
        public LazyloadSupportException()
            : base("当前方法支持延迟，但需要框架支持，无法直接调用。")
        {
        }
    }
}
