using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.Framework;
using Avalon.Utility;

namespace Avalon.Purviews
{
    public class RoleServices : IService
    {
        private IRoleRepository roleRepository;
        private IAccessRepository accessRepository;

        //角色的缓存
        private CacheDomain<Role, int> roleCache;

        public RoleServices(IRoleRepository roleRepository, IAccessRepository accessRepository)
        {
            this.roleRepository = roleRepository;
            this.accessRepository = accessRepository;

            roleCache =
            new CacheDomain<Role, int>(o => o.Key,
                                        GeRoleInner,
                                        null,
                                        cacheName: "role",
                                        cacheKeyFormat: "role:{0}",
                                        secondesToLive: 600);
        }

        Role GeRoleInner(int roleKey)
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
        public IList<Role> GetRoleList(string instanceKey)
        {
            var spec = roleRepository.CreateSpecification()
                .Where(o => o.InstanceKey == instanceKey);
            var roles = roleRepository.FindAll(spec);
            return roles;
        }

        public IList<Role> GetRoleList(string instanceKey, IEnumerable<int> roleIds)
        {
            var spec = roleRepository.CreateSpecification()
                .Where(o => o.InstanceKey == instanceKey && roleIds.Contains(o.Key));
            var roles = roleRepository.FindAll(spec);
            return roles;
        }

        /// <summary>
        /// 根据角色的ID获取角色的信息
        /// </summary>
        /// <param name="roleKey"></param>
        /// <returns></returns>
        public Role GetRole(int roleKey)
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
        public void SaveRole(Role entry)
        {
            if (entry == null)
                throw new ArgumentNullException("entry");

            //创建的过程
            if (entry.Key == 0)
            {
                entry.SerialRolePurview();
                roleRepository.Create(entry);
            }
            //更新过程
            else
            {
                entry.SerialRolePurview();
                roleRepository.Update(entry);
                roleCache.RemoveCache(entry.Key);
            }
        }

        /// <summary>
        /// 移除角色
        /// </summary>
        /// <param name="roleKey"></param>
        /// <returns></returns>
        public void RemoveRole(int roleKey)
        {
            if (roleKey <= 0)
                throw new ArgumentNullException("roleKey");
            var role = roleRepository.Get(roleKey);
            if (role == null)
                throw new ArgumentNullException("role");

            roleRepository.Delete(role);
            roleCache.RemoveCache(roleKey);
        }

        /// <summary>
        /// 获取默认角色
        /// </summary>
        /// <returns></returns>
        public Role GetDefaultRole(string instanceKey)
        {
            var role = roleRepository.QueryDefaultRole(instanceKey);
            if (role == null)
                return new Role();
            return role;
        }
    }
}
