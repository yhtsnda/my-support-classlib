using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Warehouse.Settings;

namespace Warehouse.DataOperator
{
    public interface IPolicy
    {
        string StorageFlag
        {
            get;
            set;
        }

        string DbServerKey
        {
            get;
            set;
        }

        string TablePrefix
        {
            get;
            set;
        }

        /// <summary>
        /// 存储数据操作
        /// </summary>
        void Storage(StoragePolicyConfigure config);

        /// <summary>
        /// 获取数据操作
        /// </summary>
        void Obtain(ObtainPolicyConfigure config);
    }
}
