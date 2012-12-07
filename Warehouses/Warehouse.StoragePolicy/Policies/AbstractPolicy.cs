using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Warehouse.Utility;
using Warehouse.Settings;
using Warehouse.DataOperator;

namespace Warehouse.StoragePolicy
{
    /// <summary>
    /// 存储/读取策略的抽象基类
    /// </summary>
    public abstract class AbstractPolicy<TEntity> : IPolicy, IPolicy<TEntity>
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

        public PolicyConfig GetPolicyConfig()
        {
            throw new NotImplementedException();
        }

        public ResultKey Storage(DataTable datas)
        {
            throw new NotImplementedException();
        }

        public DataTable Obtain()
        {
            throw new NotImplementedException();
        }

        public ResultKey Storage(List<TEntity> datas)
        {
            throw new NotImplementedException();
        }

        public IList<TEntity> Obtain<TEntity>()
        {
            throw new NotImplementedException();
        }
    }
}
