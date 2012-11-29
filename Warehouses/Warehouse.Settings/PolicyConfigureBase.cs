using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.Settings
{
    public abstract class PolicyConfigureBase : IConfigure
    {
        /// <summary>
        /// 唯一配置键
        /// </summary>
        public string ConfigKey
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 数据分区策略类型
        /// </summary>
        public PolicyType DistributePolicy
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// 数据在物理数据库上的分布类型
        /// </summary>
        public DataDistributeType DataDistribute
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// 数据存放的数据表名称
        /// </summary>
        public string Path
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// 读取配置信息
        /// </summary>
        public void Load()
        {
        }

        /// <summary>
        /// 保存配置信息
        /// </summary>
        public void Save()
        {
        }
    }
}
