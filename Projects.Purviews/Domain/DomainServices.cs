using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Framework;

namespace Projects.Purviews
{
    public class DomainServices
    {
        private static IDomainRepository repository = DependencyResolver.Resolve<IDomainRepository>();

        /// <summary>
        /// 根据键获取域
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Domain GetDomain(string key)
        {
            return repository.Get(key);
        }

        /// <summary>
        /// 根据名称获取域
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Domain GetDomainByName(string name)
        {
            return repository.FindOne(new Domain { Name = name });
        }

        /// <summary>
        /// 获取所有域
        /// </summary>
        /// <returns></returns>
        public static IList<Domain> GetDomainList()
        {
            return repository.FindAll(new Domain());
        }

        /// <summary>
        /// 保存域
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static ResultKey SaveDomain(Domain entry)
        {
            if (entry == null)
                return ResultKey.Failure;

            var dm = GetDomain(entry.Key);
            if (dm == null)
                return repository.Create(entry).Result;
            return repository.Update(entry);
        }

        /// <summary>
        /// 删除域
        /// </summary>
        public static ResultKey DeleteDomain(Domain entry)
        {
            if (entry == null)
                return ResultKey.Failure;

            return repository.Delete(entry);
        }
    }
}
