using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Warehouse.Settings;

namespace Warehouse.StoragePolicy
{
    public class RangePolicy : AbstractPolicy
    {
        /// <summary>
        /// 按键值范围分发存放的策略
        /// </summary>
        public RangePolicy()
        {
            throw new System.NotImplementedException();
        }

        public override void Storage(StoragePolicyConfigure config)
        {
            throw new NotImplementedException();
        }

        public override void Obtain(ObtainPolicyConfigure config)
        {
            throw new NotImplementedException();
        }
    }
}
