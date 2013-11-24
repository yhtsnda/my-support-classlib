using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    /// <summary>
    /// 表明该方法为数据的持久化方法（会变更数据的方法）
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PersistentMethodAttribute : Attribute
    {
    }
}
