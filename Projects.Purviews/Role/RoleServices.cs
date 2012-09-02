using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Tool;
using Projects.Framework;

namespace Projects.Purviews
{
    public static class RoleServices
    {
        private static IRoleRepository roleRepository = DependencyResolver.Resolve<IRoleRepository>();
        private static IAccessRepository accessRepository = DependencyResolver.Resolve<IAccessRepository>();

        //角色的缓存
        private static CacheDomain<Role, int> roleCache = new CacheDomain<Role, int>(  o => o.Key,
                                                                                                        GeRoleInner,
                                                                                                        null,
                                                                                                        cacheName: "role",
                                                                                                        cacheKeyFormat: "role:{0}",
                                                                                                        secondesToLive: 600);

        internal static Role GeRoleInner(int roleKey)
        {
            var role = roleRepository.Get(roleKey);
            if (role == null)
                return new Role();
            return role;
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="instanceKey"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public static IList<Role> GetRoleList(string instanceKey)
        {
            var roles = roleRepository.FindAll(new Role { InstanceKey = instanceKey });
            if (!roles.Any())
                return null;
            return roles;
        }

        public static IList<Role> GetRoleList(string instanceKey, IEnumerable<int> roleIds)
        {
            var roles = roleRepository.FindAll(new Role { InstanceKey = instanceKey });
            if (!roles.Any())
                return null;

            return roles.Where(o => roleIds.Contains(o.Key)).ToList();
        }

        /// <summary>
        /// 根据角色的ID获取角色的信息
        /// </summary>
        /// <param name="roleKey"></param>
        /// <returns></returns>
        public static Role GetRole(int roleKey)
        {
            var role = roleCache.GetItem(roleKey);
            if (role == default(Role))
                return new Role();
            role.DeserialRolePurview();
            return role;
        }

        /// <summary>
        /// 保存角色信息
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static ResultKey SaveRole(Role entry)
        {
            if (entry == null)
                return ResultKey.Failure;

            //创建的过程
            if (entry.Key == 0)
            {
                entry.SerialRolePurview();
                return roleRepository.Create(entry).Result;
            }
            //更新过程
            else
            {
                entry.SerialRolePurview();
                var result = roleRepository.Update(entry);
                if (result == ResultKey.OK)
                    roleCache.RemoveCache(entry.Key);
                return result;
            }
        }

        /// <summary>
        /// 移除角色
        /// </summary>
        /// <param name="roleKey"></param>
        /// <returns></returns>
        public static ResultKey RemoveRole(int roleKey)
        {
            if (roleKey <= 0)
                return ResultKey.Failure;
            var result = roleRepository.Delete(new Role { Key = roleKey });
            if (result == ResultKey.OK)
                roleCache.RemoveCache(roleKey);
            return result;
        }

    }
}
