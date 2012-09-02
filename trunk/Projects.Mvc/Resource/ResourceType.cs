using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingSiteCheck.Resource
{
    /// <summary>
    /// 资源类型
    /// </summary>
    public enum ResourceType
    {
        /// <summary>
        /// 未定义，自动识别
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// JavaScript资源
        /// </summary>
        JS,

        /// <summary>
        /// CSS资源
        /// </summary>
        CSS
    }
}
