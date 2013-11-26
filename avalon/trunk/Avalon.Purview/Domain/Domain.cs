using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Avalon.Purviews
{
    /// <summary>
    /// 权限系统中的域
    /// </summary>
    public class Domain
    {
        /// <summary>
        /// 控制域的键
        /// </summary>
        [Required]
        [RegularExpression("[a-z]+", ErrorMessage = "必须为小写字母")]
        public virtual string Key { get; set; }

        /// <summary>
        /// 控制域的名称
        /// </summary>
        [Required]
        public virtual string Name { get; set; }


        public Hashtable ToHashtable()
        {
            Hashtable table = new Hashtable();
            table.Add("Key", this.Key);
            table.Add("Name", this.Name);
            return table;
        }

        /// <summary>
        /// 从实例键中获取控制域
        /// </summary>
        public static string GetDomainKey(string instanceKey)
        {
            if (String.IsNullOrWhiteSpace(instanceKey))
                throw new ArgumentNullException("instanceKey");

            if (instanceKey.StartsWith("$") && instanceKey.EndsWith("$"))
                return instanceKey.Substring(1, instanceKey.Length - 2);

            int index = instanceKey.IndexOf(":");
            if (index == -1)
                throw new ArgumentException("无法获取到 domain key , 传入的instanceKey必须如下格式 {domainKey}:{...}");
            return instanceKey.Substring(0, index);
        }
    }
}
