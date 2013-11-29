using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuth
{
    public class ClientAuthorizationFilter
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string Name { get; set; }
    }
}
