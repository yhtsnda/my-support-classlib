using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Avalon.Purviews
{
    public class Access
    {
        public virtual int AutoCode { get; set; }

        public virtual int UserId { get; set; }

        public virtual string InstanceKey { get; set; }

        public virtual IList<Role> Roles { get; set; }

        public Access()
        {
           
        }

        /// <summary>
        /// 校验是否包含给定的权限
        /// </summary>
        public virtual bool CheckAction(string instanceKey, string actionKey)
        {
            if (this.InstanceKey != instanceKey)
                return false;

            bool result = false; //默认没有
            //用户包含的角色中只要有一个角色包含指定操作键即可
            foreach (var role in Roles)
            {
                result = role.CheckAction(actionKey);
                if (result)
                    break;
            }
            return result;
        }

        /// <summary>
        /// 校验是否包含给定的权限
        /// </summary>
        public virtual IList<bool> CheckAction(string instanceKey, IEnumerable<string> actionKeys)
        {
            return actionKeys.Select(actionKey => CheckAction(instanceKey, actionKey)).ToList();
        }

        /// <summary>
        /// 校验是否包含给定的资源
        /// </summary>
        public virtual bool CheckResource(string instanceKey, string resourceKey)
        {
            if (this.InstanceKey != instanceKey)
                return false;

            bool result = false; //默认没有
            //用户包含的角色中只要有一个角色包含指定操作键即可
            foreach (var role in Roles)
            {
                result = role.CheckResource(resourceKey);
                if (result)
                    break;
            }
            return result;
        }

        /// <summary>
        /// 校验是否包含给定的资源
        /// </summary>
        public virtual IList<bool> CheckResource(string instanceKey, IEnumerable<string> resourceKeys)
        {
            return resourceKeys.Select(resourceKey => CheckResource(instanceKey, resourceKey)).ToList();
        }

        /// <summary>
        /// 校验是否包含给定的权限及资源
        /// </summary>
        public virtual bool CheckAccess(string instanceKey, string actionKey, string resourceKey)
        {
            if (this.InstanceKey != instanceKey)
                return false;

            bool result = false; //默认没有
            //用户包含的角色中只要有一个角色包含指定操作键即可
            foreach (var role in Roles)
            {
                result = role.CheckAccess(actionKey, resourceKey);
                if (result)
                    break;
            }
            return result;
        }
    }
}
