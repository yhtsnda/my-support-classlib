using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.StoragePolicy
{
    public interface IPolicy
    {
        string StorageFlag
        {
            get;
            set;
        }

        /// <summary>
        /// 存储数据操作
        /// </summary>
        void Storage();

        /// <summary>
        /// 获取数据操作
        /// </summary>
        void Obtain();
    }
}
