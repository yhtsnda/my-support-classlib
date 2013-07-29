using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool.Diagnostics
{
    public enum ProfileStatus
    {
        /// <summary>
        /// 正常请求，开启Profiler
        /// </summary>
        Request = 1,
        /// <summary>
        /// 静态请求（通过特定页面查看，仅保留最后10个请求的记录，存在服务器内存中）
        /// </summary>
        Static = 2,
        /// <summary>
        /// 关闭Profiler
        /// </summary>
        Disable = 3,
        /// <summary>
        /// 单一用户请求
        /// </summary>
        SingleUserRequest = 4,
        /// <summary>
        /// 
        /// </summary>
        StaticPageRequest = 5
    }
}
