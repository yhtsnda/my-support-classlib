using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.UserCenter
{
    /// <summary>
    /// 映射类型
    /// </summary>
    public enum MappingType
    {
        /// <summary>
        /// 无映射
        /// </summary>
        None = 0,

        /// <summary>
        /// 通过手机号码映射
        /// </summary>
        Mobile = 1,

        /// <summary>
        /// 使用身份证映射
        /// </summary>
        IdCard = 2,

        /// <summary>
        /// 新浪微博映射
        /// </summary>
        SinaWeibo = 10,

        /// <summary>
        /// 使用腾讯映射
        /// </summary>
        Tencent = 20
    }
}
