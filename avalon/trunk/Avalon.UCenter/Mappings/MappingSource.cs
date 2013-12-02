using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public enum MappingType
    {
        /// <summary>
        /// 空
        /// </summary>
        None = 0,

        /// <summary>
        /// 91通行证
        /// </summary>
        X91Passport = 1,

        /// <summary>
        /// 手机映射
        /// </summary>
        Mobile = -100,

        /// <summary>
        /// 新浪微博
        /// </summary>
        SinaWeibo = 10,

        /// <summary>
        /// 腾讯微博
        /// </summary>
        Tencent = 20,

        /// <summary>
        /// 中职教育svt
        /// </summary>
        Svt = 8
    }
}
