using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    /// <summary>
    /// 常用的正则表达式模式
    /// </summary>
    public static class RegexPattern
    {
        /// <summary>
        /// 匹配常见的网址形适用于http，https，ftp等开头的网址形式
        /// </summary>
        public static readonly string Url = @"^((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?$";

        /// <summary>
        /// 匹配常见的E-mail地址
        /// </summary>
        public static readonly string Email = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";

        /// <summary>
        /// 匹配文件路径
        /// 不能匹配网络路径，如\\server\sharefolder\file1.txt
        /// 如果输入路径c:\windows\\system32\mfc.dll，路径匹配也会失败，所以在用表达式检测前需要把路径中的'\\'替换成'\'
        /// </summary>
        public static readonly string FilePath = @"^(?<path>(?:[a-zA-Z]:)?\\(?:[^\\\?\/\*\|<>:""]+\\)+)(?<filename>(?<name>[^\\\?\/\*\|<>:""]+?)\.(?<ext>[^.\\\?\/\*\|<>:""]+))$";

        /// <summary>
        /// 匹配文件名称
        /// </summary>
        public static readonly string FileName = @"^(?<filename>(?<name>[^\\\?\/\*\|<>:""]+?)\.(?<ext>[^.\\\?\/\*\|<>:""]+))$";

        /// <summary>
        /// 匹配图片文件名称，只支持扩展名为：jpg,gif,png,jpeg，扩展忽略大小写匹配
        /// </summary>
        public static readonly string ImageFileName = @"^(?<filename>(?<name>[^\\\?\/\*\|<>:""]+?)\.(?<ext>jpg|gif|png|jpeg|JPG|GIF|PNG|JPEG))$";

        /// <summary>
        /// 匹配QQ号
        /// </summary>
        public static readonly string QQ = @"^[1-9][0-9]{4,}$";

        /// <summary>
        /// 匹配电话或手机号码，支持手机号码，3-4位区号，7-8位直播号码，1－4位分机号
        /// </summary>
        public static readonly string PhoneNumber = @"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)";

        /// <summary>
        /// 匹配小写字母
        /// </summary>
        public static readonly string LowerLetter = @"^[a-z]+$";

        /// <summary>
        /// 匹配大写字母
        /// </summary>
        public static readonly string UpperLetter = @"^[A-Z]+$";
    }
}
