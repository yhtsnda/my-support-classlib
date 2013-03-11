using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 通用状态
    /// </summary>
    public enum Status
    {
        ///// <summary>
        ///// 默认情况
        ///// </summary>
        //Default = 0,

        /// <summary>
        /// 可用的
        /// </summary>
        Enabled = 1,

        /// <summary>
        /// 禁用的
        /// </summary>
        Diabled = -1,

        /// <summary>
        /// 审核中
        /// </summary>
        Auditing = 0
    }

    public static partial class EnumHepler
    {
        public static string ToName(this Status status)
        {
            switch (status)
            {
                case Status.Enabled:
                    return "可用的";
                case Status.Diabled:
                    return "禁用的";
                case Status.Auditing:
                    return "审核中";
                default:
                    return "可用的";
            }
        }
    }
}
