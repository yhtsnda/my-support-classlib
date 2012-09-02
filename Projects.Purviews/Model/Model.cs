using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using Projects.Tool.Util;

namespace Projects.Purviews
{
    /// <summary>
    /// 表示特定的模块
    /// </summary>
    public class Model : ICloneable
    {
        /// <summary>
        /// 获取或设置模块的键
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// 获取或设置模块的名称
        /// </summary>
        [Required(ErrorMessage = "模块名称不能为空")]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置父模块的Key
        /// </summary>
        public int ParentKey { get;set; }

        /// <summary>
        /// 获取设置模块的权限键名
        /// </summary>
        [RegularExpression(@"[a-z:\$]+", ErrorMessage = "必须为小写字母")]
        public string ActionKey { get; set; }

        /// <summary>
        /// 获取或设置模块的域实例键名
        /// </summary>
        [RegularExpression(@"[a-z:\$\d]+", ErrorMessage = "必须为小写字母、数字、$、:组成")]
        public string InstanceKey { get; set; }

        /// <summary>
        /// 获取或设置模块的排序值
        /// </summary>
        [Required(ErrorMessage = "模块排序值不能为空")]
        [RegularExpression(@"\d+", ErrorMessage = "必须为数字")]
        public int SortNumber { get; set; }

        /// <summary>
        /// 获取或设置模块的URL
        /// </summary>
        [StringLength(200, ErrorMessage = "URL长度不能超过200")]
        public string Url { get; set; }

        private IList<Model> mChilds = new List<Model>();
        
        /// <summary>
        /// 子模块集合
        /// </summary>
        [JsonProperty]
        public IList<Model> Childs
        {
            get { return mChilds; }
        }

        public Model()
        {
            
        }

        /// <summary>
        /// 根模块
        /// </summary>
        public Model(string instanceKey, string name)
            : this()
        {
            InstanceKey = instanceKey;
            Name = name;
        }

        /// <summary>
        /// 结构性模块
        /// </summary>
        public Model(string instanceKey, string name, int parentKey)
            : this()
        {
            InstanceKey = instanceKey;
            Name = name;
            ParentKey = parentKey;
        }

        /// <summary>
        /// 普通模块
        /// </summary>
        public Model(string instanceKey, string name, int parentKey, string url)
            : this()
        {
            InstanceKey = instanceKey;
            Name = name;
            ParentKey = parentKey;
            Url = url;
        }

        public Model Clone()
        {
            return new Model()
            {
                Key = Key,
                InstanceKey = InstanceKey,
                Name = Name,
                ParentKey = ParentKey,
                ActionKey = ActionKey,
                SortNumber = SortNumber,
                Url = Url
            };
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public override string ToString()
        {
            return Name + "[" + ActionKey + "], url:" + Url;
        }
    }
}
