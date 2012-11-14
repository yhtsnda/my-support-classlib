using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.StoragePolicy
{
    public class ModulusPolicy : AbstractPolicy
    {
        /// <summary>
        /// 取模操作的策略
        /// </summary>
        public ModulusPolicy(string configKey, string parameters)
        {
            throw new System.NotImplementedException();
        }

        public ModulusPolicy()
        {
            throw new System.NotImplementedException();
        }

        public override void Storage(Configure.StoragePolicyConfigure config)
        {
            throw new NotImplementedException();
        }

        public override void Obtain(Configure.ObtainPolicyConfigure config)
        {
            throw new NotImplementedException();
        }
    }
}
