using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 实现动态属性Get方法的接口
    /// </summary>
    public interface IGetter
    {
        object Get(object target);

        string PropertyName { get; }
    }
}
