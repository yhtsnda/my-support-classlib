using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    /// <summary>
    /// 来源平台类型
    /// </summary>
    public enum PlatType
    {
        /// <summary>
        /// 通用Web平台
        /// </summary>
        CommonWeb = 1001,
        /// <summary>
        /// 移动Web平台
        /// </summary>
        MobileWeb = 1002,
        /// <summary>
        /// Android手机
        /// </summary>
        AndroidPhone = 3001,
        /// <summary>
        /// Android平板
        /// </summary>
        AndroidPad = 3002,
        /// <summary>
        /// 土豪金
        /// </summary>
        IPhone = 6001,
        /// <summary>
        /// 艾派德
        /// </summary>
        IPad = 6002,
        /// <summary>
        /// Windows Phone
        /// </summary>
        WindowsPhone = 5001,
        /// <summary>
        /// WIndows Pad(Surface)
        /// </summary>
        WindowsPad = 5002,
        /// <summary>
        /// 其他平台
        /// </summary>
        Other = 9999
    }
}
