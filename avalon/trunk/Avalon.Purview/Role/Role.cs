using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Avalon.Utility;

namespace Avalon.Purviews
{
    /// <summary>
    /// 系统角色
    /// </summary>
    public class Role
    {
        /// <summary>
        /// 获取或设置角色的Key
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// 获取或设置角色的名称 
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置角色的域实例键名
        /// </summary>
        [Required]
        public string InstanceKey { get; set; }

        /// <summary>
        /// 是否为默认角色
        /// </summary>
        public bool IsDefault { get; set; }

        public string ActionKey { get; set; }

        public string ResourceKey { get; set; }

        /// <summary>
        /// 获取或设置角色的动作
        /// </summary>
        public virtual IList<string> ActionKeys { get; set; }

        /// <summary>
        /// 获取或设置角色的资源
        /// </summary>
        public virtual IList<string> ResourceKeys { get; set; }
        
        /// <summary>
        /// 表示一个角色
        /// </summary>
        public Role()
        {
            ActionKeys = new List<string>();
            ResourceKeys = new List<string>();
        }

        /// <summary>
        /// 检查是否包含给定的操作
        /// </summary>
        public virtual bool CheckAction(string actionKey)
        {
            if (this.ActionKeys.Count == 0 && this.ActionKey != null)
                DeserialRolePurview();

            return ActionKeys.Contains(actionKey);
        }

        /// <summary>
        /// 检查是否包含给定的操作(复数)
        /// </summary>
        public virtual IList<bool> CheckAction(IList<string> actionkeys)
        {
            if (this.ActionKeys.Count == 0 && this.ActionKey != null)
                DeserialRolePurview();

            return ActionKeys.Select(o => ActionKeys.Contains(o)).ToList();
        }

        /// <summary>
        /// 检查是否包含给定的资源
        /// </summary>
        public virtual bool CheckResource(string resKey)
        {
            if (this.ResourceKeys.Count == 0 && this.ActionKey != null)
                DeserialRolePurview();

            return ResourceKeys.Contains(resKey);
        }

        /// <summary>
        /// 检查是否包含给定的资源(复数)
        /// </summary>
        public virtual IList<bool> CheckResource(IList<string> reskeys)
        {
            if (this.ResourceKeys.Count == 0 && this.ActionKey != null)
                DeserialRolePurview();

            return ResourceKeys.Select(o => ActionKeys.Contains(o)).ToList();
        }

        /// <summary>
        /// 校验是否包含给定的动作及资源
        /// </summary>
        public virtual bool CheckAccess(string purviewKey, string resourceKey)
        {
            return (CheckResource(purviewKey) && CheckResource(resourceKey));
        }

        public virtual void SerialRolePurview()
        {
            this.ActionKey = JsonConverter.ToJson(this.ActionKeys);
            this.ResourceKey = JsonConverter.ToJson(this.ResourceKeys);
        }

        public virtual void DeserialRolePurview()
        {
            if(!String.IsNullOrEmpty(this.ActionKey))
                this.ActionKeys = JsonConverter.FromJson<List<string>>(this.ActionKey);
            if (!String.IsNullOrEmpty(this.ResourceKey))
                this.ResourceKeys = JsonConverter.FromJson<List<string>>(this.ResourceKey);
        }

    }
}
