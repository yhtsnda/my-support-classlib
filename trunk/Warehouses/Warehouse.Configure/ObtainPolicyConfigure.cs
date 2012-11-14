using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Warehouse.Access;

namespace Warehouse.Configure
{
    public class ObtainPolicyConfigure : PolicyConfigureBase
    {
        /// <summary>
        /// 是否使用缓存
        /// </summary>
        public bool UseCache { get; set; }

        public System.Data.CommandType ObtainCommandType
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
        /// 读取获取数据的策略
        /// </summary>
        public override void Load()
        {
            PolicyConfigAccessor.GetPolicyConfig<ObtainPolicyConfigure>(base.ConfigKey, ref this);
        }

        /// <summary>
        /// 保存存储数据的策略
        /// </summary>
        public override void Save()
        {
            PolicyConfigAccessor.SavePolicyCoinfig<ObtainPolicyConfigure>(this);
        }
    }
}
