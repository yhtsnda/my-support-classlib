using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Warehouse.Settings;
using Warehouse.DataOperator;

namespace Warehouse.StoragePolicy
{
    /// <summary>
    /// 存储/读取策略的抽象基类
    /// </summary>
    public abstract class AbstractPolicy : IPolicy
    {
        /// <summary>
        /// 数据读取和存储的标识位
        /// </summary>
        public string StorageFlag { get; set; }

        /// <summary>
        /// 数据库键
        /// </summary>
        public string DbServerKey { get; set; }

        /// <summary>
        /// 表前缀
        /// </summary>
        public string TablePrefix { get; set; }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="config">存储数据策略配置</param>
        public abstract void Storage(StoragePolicyConfigure config);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="config">获取数据策略配置</param>
        public abstract void Obtain(ObtainPolicyConfigure config);
    }
}
