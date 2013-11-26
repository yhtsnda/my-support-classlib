using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Avalon.Framework;
using Avalon.Utility;

namespace Avalon.Purviews
{
    public class ActionServices : IService
    {
        private IActionRepository repository;
        //操作的缓存
        private static CacheDomain<Action, string, string> actionCacheEx;

        public ActionServices(IActionRepository repositroy)
        {
            this.repository = repository;
            actionCacheEx = CacheDomain.CreatePairKey<Action, string, string>(o => o.Key, 
                GetActionByKeyInner, 
                null,
                cacheName: "actionEx", 
                cacheKeyFormat: "actionEx:{0}:{1}", 
                secondesToLive: 600);
        }

        internal Action GetActionByKeyInner(string domainKey, string actionKey)
        {
            var spec = repository.CreateSpecification()
                .Where(o => o.DomainKey == domainKey && o.Key == actionKey);
            return repository.FindOne(spec);
        }

        /// <summary>
        /// 获取动作
        /// </summary>
        /// <returns></returns>
        public Action GetAction(int id)
        {
            return repository.Get(id);
        }

        /// <summary>
        /// 查询动作 
        /// </summary>
        /// <param name="domainKey"></param>
        /// <param name="actionKey"></param>
        /// <returns></returns>
        public IList<Action> SearchActions(string domainKey, string actionKey)
        {
            var spec = repository.CreateSpecification()
                .Where(o => o.DomainKey == domainKey && o.Key == actionKey);
            return repository.FindAll(spec);
        }

        /// <summary>
        /// 获取动作根据名称
        /// </summary>
        /// <param name="domainKey"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public Action GetActionByKey(string domainKey, string actionKey)
        {
            var action = actionCacheEx.GetItem(domainKey, actionKey);
            return action;
        }

        /// <summary>
        /// 获取动作根据名称
        /// </summary>
        /// <param name="domainKey"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public Action GetActionByName(string domainKey, string actionName)
        {
            var spec = repository.CreateSpecification()
                    .Where(o => o.DomainKey == domainKey && o.Name == actionName);
            var action = repository.FindOne(spec);
            return action;
        }

        /// <summary>
        /// 获取域下的所有动作
        /// </summary>
        /// <param name="domainKey"></param>
        /// <returns></returns>
        public IList<Action> GetActions(string domainKey)
        {
            var spec = repository.CreateSpecification()
                .Where(o => o.DomainKey == domainKey);
            return repository.FindAll(spec);
        }

        /// <summary>
        /// 获取动作列表
        /// </summary>
        public PagingResult<Action> GetActionList(string domainKey, string actionName,
            int pageIndex = 1, int pageSize = 20)
        {
            var spec = repository.CreateSpecification()
                    .Where(o => o.DomainKey == domainKey && o.Name == actionName)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize);

            var result = repository.FindPaging(spec);
            return result;
        }

        public void SaveAction(Action entry)
        {
            Arguments.NotNull<Action>(entry, "entry");
            if (entry.AutoCode == 0)
            {
                repository.Create(entry);
            }
            else
            {
                repository.Update(entry);
                actionCacheEx.RemoveCache(entry.DomainKey, entry.Key);
            }
        }

        /// <summary>
        /// 判断域中是否有某个权限
        /// </summary>
        /// <param name="domainKey"></param>
        /// <param name="actionKey"></param>
        /// <returns></returns>
        public bool CheckAction(string domainKey, string actionKey)
        {
            var spec = repository.CreateSpecification()
                .Where(o => o.DomainKey == domainKey);
            return repository.Count(spec) > 0;
        }

        /// <summary>
        /// 判断域中是否有给定名称的权限
        /// </summary>
        /// <param name="domainKey"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public bool CheckActionName(string domainKey, string actionName)
        {
            var p = GetActionByName(domainKey, actionName);
            return p == null ? false : true;
        }

        /// <summary>
        /// 删除一个动作 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public void DeleteAction(Action action)
        {
            repository.Delete(action);
        }
    }
}
