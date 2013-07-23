using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public enum StatementType
    {
        /// <summary>
        /// 查询语句
        /// </summary>
        Select = 1,
        /// <summary>
        /// 删除语句
        /// </summary>
        Delete = 2,
        /// <summary>
        /// 插入语句
        /// </summary>
        Insert = 3,
        /// <summary>
        /// 更新语句
        /// </summary>
        Update = 4,
        /// <summary>
        /// 替换语句
        /// </summary>
        Replace = 5,
    }
}
