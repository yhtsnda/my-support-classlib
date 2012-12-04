using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    internal enum ClassJoinType
    {
        /// <summary>
        /// 一对N
        /// </summary>
        HasOne,
        /// <summary>
        /// 多对一
        /// </summary>
        HasMany
    }
}
