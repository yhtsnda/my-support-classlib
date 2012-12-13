using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Accesses.IBatisNetAccess.Linq
{
    /// <summary>
    /// IBatis的查询数据
    /// </summary>
    internal class IBatisNetQueryData
    {
        /// <summary>
        /// 查询数据的类型
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// 查询的语句
        /// </summary>
        public string StatementValue { get; }

        /// <summary>
        /// 需要跳过的数量
        /// </summary>
        public int? SkipValue { get; set; }

        /// <summary>
        /// 需要获取的数量
        /// </summary>
        public int? TakeValue { get; set; }
    }
}
