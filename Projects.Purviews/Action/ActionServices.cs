using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Tool.Pager;
using Projects.Tool;
using Projects.Framework;

namespace Projects.Purviews
{
    public class ActionServices
    {
        private static IActionRepository repository = DependencyResolver.Resolve<IActionRepository>();
        //操作的缓存
        private static CacheDomain<Action, int> actionCache = new CacheDomain<Action, int>(o => o.AutoCode,
                                                                                                        GeActionInner,
                                                                                                        null,
                                                                                                        cacheName: "action",
                                                                                                        cacheKeyFormat: "action:{0}",
                                                                                                        secondesToLive: 600);

        private static CacheDomain<Action, string, string> actionCacheEx = new CacheDomain<Action, string, string>(o => o.Key, GetActionByKeyInner, null, cacheName: "actionEx", cacheKeyFormat: "actionEx:{0}:{1}", secondesToLive: 600);

        internal static Action GetActionByKeyInner(string domainKey, string actionKey)
        {
            var action = repository.FindOne(new Action { DomainKey = domainKey, Key = actionKey });
            if (action == default(Action))
                return null;
            return action;
        }

        internal static Action GeActionInner(int id)
        {
            var action = repository.FindOne(new Action { AutoCode = id });
            if (action == null)
                return new Action();
            return action;
        }

        /// <summary>
        /// 获取动作
        /// </summary>
        /// <returns></returns>
        public static Action GetAction(int id)
        {
            var action = actionCache.GetItem(id);
            if (action == default(Action))
                return null;
            return action;
        }

        /// <summary>
        /// 查询动作 
        /// </summary>
        /// <param name="domainKey"></param>
        /// <param name="actionKey"></param>
        /// <returns></returns>
        public static IList<Action> SearchActions(string domainKey, string actionKey)
        {
            var actionList = repository.FindAll(new Action { DomainKey = domainKey });
            return actionList.Where(o => o.Key.Contains(actionKey)).ToList();
        }

        /// <summary>
        /// 获取动作根据名称
        /// </summary>
        /// <param name="domainKey"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public static Action GetActionByKey(string domainKey, string actionKey)
        {
            var action = actionCacheEx.GetItem(domainKey, actionKey);
            if (action == default(Action))
                return null;
            return action;
        }

        /// <summary>
        /// 获取动作根据名称
        /// </summary>
        /// <param name="domainKey"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public static Action GetActionByName(string domainKey, string actionName)
        {
            var action = repository.FindOne(new Action { DomainKey = domainKey, Name = actionName });
            if (action == default(Action))
                return null;
            return action;
        }

        /// <summary>
        /// 获取域下的所有动作
        /// </summary>
        /// <param name="domainKey"></param>
        /// <returns></returns>
        public static IList<Action> GetActions(string domainKey)
        {
            return repository.FindAll(new Action { DomainKey = domainKey });
        }

        public static IList<Action> GetActionList(string domainKey)
        {
            Action conditon = new Action();
            if (!String.IsNullOrEmpty(domainKey))
                conditon.DomainKey = domainKey;
            var result = repository.FindAll(conditon);
            return result;
        }

        /// <summary>
        /// 获取动作列表
        /// </summary>
        public static PagedList<Action> GetActionList(string domainKey, string actionName, int pageIndex = 1)
        {
            Action conditon = new Action();
            if (!String.IsNullOrEmpty(domainKey))
                conditon.DomainKey = domainKey;
            if (!String.IsNullOrEmpty(actionName))
                conditon.Name = actionName;

            var result = repository.FindPaging(conditon, pageIndex);
            return result;
        }

        public static ResultKey SaveAction(Action entry)
        {
            if (entry == null)
                return ResultKey.Failure;

            if (entry.AutoCode == 0)
            {
                return repository.Create(entry).Result;
            }
            var result = repository.Update(entry);
            if (result == ResultKey.OK)
            {
                actionCache.RemoveCache(entry.AutoCode);
                actionCacheEx.RemoveCache(entry.DomainKey, entry.Key);
            }
            return result;
        }

        /// <summary>
        /// 判断域中是否有某个权限
        /// </summary>
        /// <param name="domainKey"></param>
        /// <param name="actionKey"></param>
        /// <returns></returns>
        public static bool CheckAction(string domainKey, string actionKey)
        {
            var p = repository.FindOne(new Action { DomainKey = domainKey, Key = actionKey });
            return p == null ? false : true;
        }

        /// <summary>
        /// 判断域中是否有给定名称的权限
        /// </summary>
        /// <param name="domainKey"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public static bool CheckActionName(string domainKey, string actionName)
        {
            var p = GetActionByName(domainKey, actionName);
            return p == null ? false : true;
        }

        /// <summary>
        /// 删除一个动作 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ResultKey DeleteAction(Action action)
        {
            if (action == null)
                return ResultKey.Failure;
            action = GetAction(action.AutoCode);

            var result = repository.Delete(action);
            if (result == ResultKey.OK)
            {
                actionCache.RemoveCache(action.AutoCode);
                actionCacheEx.RemoveCache(action.DomainKey, action.Key);
            }
                
            return result;
        }
    }
}
