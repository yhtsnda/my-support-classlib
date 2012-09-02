using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Projects.Purviews
{
    /// <summary>
    /// 表示一个资源
    /// </summary>
    public class Resource
    {
        /// <summary>
        /// 获取或设置资源的键名
        /// </summary>
        [Required]
        [RegularExpression("[a-z:]+", ErrorMessage = "必须为小写字母")]
        public virtual string Key { get; set; }

        /// <summary>
        /// 获取或设置资源的域键名
        /// </summary>
        [Required]
        [RegularExpression("[a-z]+", ErrorMessage = "必须为小写字母")]
        public virtual string DomainKey { get; set; }

        /// <summary>
        /// 获取或设置资源的名称
        /// </summary>
        [Required]
        public virtual string Name { get; set; }
    }
}
