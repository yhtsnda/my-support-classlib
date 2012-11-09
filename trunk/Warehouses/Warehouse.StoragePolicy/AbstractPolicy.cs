using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Warehouse.Configure;

namespace Warehouse.StoragePolicy
{
    public abstract class AbstractPolicy : IPolicy
    {    
        public string StorageFlag
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
