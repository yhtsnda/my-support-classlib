using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Tool.Pager;
using Projects.Tool;
using Projects.Framework;

namespace Projects.Purviews
{
    public static class AccessServices
    {
        private static IAccessRepository repository = DependencyResolver.Resolve<IAccessRepository>();
        private static CacheDomain<Access, string, int> accessCache = new CacheDomain<Access, string, int>(
            o => o.UserId,
            GetAccessInner,
            null,
            cacheName: "access",
            cacheKeyFormat: "access:{0}:{1}",
            secondesToLive: 600);


        private static Access GetAccessInner(string instanceKey, int userId)
        {
            if (userId <= 0)
                return new Access();
            if (String.IsNullOrEmpty(instanceKey))
                return new Access();

            Access user = repository.FindOne(new Access { UserId = userId, InstanceKey = instanceKey });
            return user;
        }

        /// <summary>
        /// 创建用户存取权限
        /// </summary>
        /// <param name="access"></param>
        /// <returns></returns>
        public static ResultKey CreateUserAccess(Access access)
        {
            if (access == null)
                return ResultKey.Failure;

            var result = repository.Create(access);
            if (result.Result == ResultKey.OK)
                accessCache.RemoveCache(access.InstanceKey, access.UserId);
            return result.Result;
        }

        /// <summary>
        /// 删除用户已经分配过的角色
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        public static ResultKey RemoveUserAccessRole(int accessKey)
        {
            if (accessKey <= 0)
                return ResultKey.Failure;

            return repository.DeleteAccessRole(accessKey);
        }

        public static ResultKey DeleteUserAccess(int userKey, string instanceKey)
        {
            if (userKey <= 0)
                return ResultKey.Failure;
            if (String.IsNullOrEmpty(instanceKey))
                return ResultKey.Failure;

            var result = repository.Delete(new Access { UserId = userKey, InstanceKey = instanceKey });
            if (result == ResultKey.OK)
                accessCache.RemoveCache(instanceKey, userKey);
            return result;
        }
        /// <summary>
        /// 获取一个用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Access GetUser(int userId, string instanceKey)
        {
            var access = accessCache.GetItem(instanceKey, userId);
            if (access == default(Access))
                return null;
            return access;
        }

        public static bool CheckAction(string domainKey, string actionKey)
        {
            return false;
        }

        /// <summary>
        /// 检查用户在指定的域中是否有指定的操作
        /// </summary>
        public static bool CheckAction(int userId, string instanceKey, string actionKey)
        {
            if (String.IsNullOrEmpty(instanceKey) || String.IsNullOrEmpty(actionKey))
                return false;

            Access user = AccessServices.GetUser(userId, instanceKey);
            if (user == null)
                return false;

            //判断用户在域实例中是否有权限
            bool canDoAction = user.CheckAction(instanceKey, actionKey);
            return canDoAction;
        }

        /// <summary>
        /// 检查用户在指定的域中是否有指定的操作以及资源
        /// </summary>
        public static bool CheckAction(int userId, string instanceKey, string actionKey, string resourceKey)
        {
            return false;
        }
    }
}
