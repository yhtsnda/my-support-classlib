using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Projects.Purviews
{
    /// <summary>
    /// 动作项
    /// </summary>
    public class Action
    {
        /// <summary>
        /// 权限的自动编号
        /// </summary>
        public int AutoCode { get; set; }

        /// <summary>
        /// 获取或设置动作项的键名
        /// </summary>
        [Required]
        [RegularExpression("[a-z:]+", ErrorMessage = "必须为小写字母与冒号")]
        public string Key { get; set; }

        /// <summary>
        /// 获取或设置动作项所属的域键名
        /// </summary>
        [Required]
        [RegularExpression("[a-z]+", ErrorMessage = "必须为小写字母")]
        public string DomainKey { get; set; }

        /// <summary>
        /// 获取或设置动作项的名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 将对象转化为Hashtable
        /// </summary>
        /// <returns></returns>
        public Hashtable ToHashtable()
        {
            Hashtable table = new Hashtable();
            table.Add("Key", this.Key);
            table.Add("DomainKey", this.DomainKey);
            table.Add("Name", this.Name);
            return table;
        }
    }
}
