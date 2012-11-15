using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Warehouse.Configure
{
    internal class ObtainPolicyConfigure : PolicyConfigureBase
    {
        /// <summary>
        /// 是否使用缓存
        /// </summary>
        public bool UseCache { get; set; }

        /// <summary>
        /// 获取记录的方式
        /// </summary>
        public CommandType ObtainCommandType { get; set; }
    }
}
