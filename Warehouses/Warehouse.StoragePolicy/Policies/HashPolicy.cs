using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Warehouse.Settings;

namespace Warehouse.StoragePolicy
{
    public class HashPolicy : AbstractPolicy
    {
        /// <summary>
        /// 根据键值的Hash值进行分布的策略
        /// </summary>
        public HashPolicy()
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
