using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    internal enum ClassJoinType
    {
        /// <summary>
        /// 通过外键一对N
        /// </summary>
        HasOneByForeignKey,
        /// <summary>
        /// 通过外键多对一
        /// </summary>
        HasManyByForeignKey,
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
