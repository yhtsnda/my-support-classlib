using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Querys
{

    public enum JoinType
    {
        /// <summary>
        /// 一对一的内连接
        /// </summary>
        OneToOneInnerJoin,
        /// <summary>
        /// 内连接
        /// </summary>
        InnerJoin,
        /// <summary>
        /// 左连接
        /// </summary>
        LeftOuterJoin
    }
}
