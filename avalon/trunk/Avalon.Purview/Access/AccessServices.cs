using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.Framework;
using Avalon.Utility;

namespace Avalon.Purviews
{
    public class AccessServices : IService
    {
        private IAccessRepository repository;
        private CacheDomain<Access, string, int> accessCache;

        public AccessServices(IAccessRepository repository)
        {
            this.repository = repository;

            accessCache = CacheDomain.CreatePairKey<Access, string, int>(
                            o => o.UserId,
                            GetAccessInner,
                            null,
                            cacheName: "access",
                            cacheKeyFormat: "access:{0}:{1}",
                            secondesToLive: 600);
        }

        Access GetAccessInner(string instanceKey, int userId)
        {
            var spec = repository.CreateSpecification()
                .Where(o => o.InstanceKey == instanceKey && o.UserId == userId);
            return repository.FindOne(spec);
        }

        /// <summary>
        /// 创建用户存取权限
        /// </summary>
        /// <param name="access"></param>
        /// <returns></returns>
        public void CreateUserAccess(Access access)
        {
            Arguments.NotNull<Access>(access, "access");
            repository.Create(access);
        }

        /// <summary>
        /// 删除用户已经分配过的角色
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        public void RemoveUserAccessRole(int accessKey)
        {
            if (accessKey <= 0)
                throw new AvalonException("accessKey为空");
            repository.DeleteAccessRole(accessKey);
        }

        /// <summary>
        /// 删除用户权限
        /// </summary>
        /// <param name="userKey"></param>
        /// <param name="instanceKey"></param>
        /// <returns></returns>
        public void DeleteUserAccess(int userKey, string instanceKey)
        {
            var spec = repository.CreateSpecification()
                .Where(o => o.UserId == userKey && o.InstanceKey == instanceKey);
            var userAccess = repository.FindOne(spec);
            if (userAccess == null)
                throw new ArgumentNullException("指定的用户权限不存在");
            repository.Delete(userAccess);
        }
        
        /// <summary>
        /// 获取一个用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Access GetUser(int userId, string instanceKey)
        {
            var access = accessCache.GetItem(instanceKey, userId);
            if (access == default(Access))
                return null;
            return access;
        }

        public bool CheckAction(string domainKey, string actionKey)
        {
            return false;
        }

        /// <summary>
        /// 检查用户在指定的域中是否有指定的操作
        /// </summary>
        public bool CheckAction(int userId, string instanceKey, string actionKey)
        {
            if (String.IsNullOrEmpty(instanceKey) || String.IsNullOrEmpty(actionKey))
                return false;

            Access user = GetUser(userId, instanceKey);
            if (user == null)
                return false;

            //判断用户在域实例中是否有权限
            bool canDoAction = user.CheckAction(instanceKey, actionKey);
            return canDoAction;
        }

        /// <summary>
        /// 检查用户在指定的域中是否有指定的操作以及资源
        /// </summary>
        public bool CheckAction(int userId, string instanceKey, string actionKey, string resourceKey)
        {
            return false;
        }
    }
}
