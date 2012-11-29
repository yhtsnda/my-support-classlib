using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.Settings
{
    public enum PolicyType
    {
        /// <summary>
        /// 值区间策略
        /// </summary>
        Range = 1,
        /// <summary>
        /// 取模数策略
        /// </summary>
        Modulus = 2,
        /// <summary>
        /// Hash值策略
        /// </summary>
        Hash = 3,
    }
}
