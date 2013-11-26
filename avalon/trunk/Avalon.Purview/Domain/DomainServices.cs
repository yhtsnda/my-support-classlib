using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Avalon.Framework;

namespace Avalon.Purviews
{
    public class DomainServices : IService
    {
        private IDomainRepository repository;

        public DomainServices(IDomainRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// 根据键获取域
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Domain GetDomain(string key)
        {
            return repository.Get(key);
        }

        /// <summary>
        /// 根据名称获取域
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Domain GetDomainByName(string name)
        {
            var spec = repository.CreateSpecification()
                .Where(o => o.Name == name);
            return repository.FindOne(spec);
        }

        /// <summary>
        /// 获取所有域
        /// </summary>
        /// <returns></returns>
        public IList<Domain> GetDomainList()
        {
            var spec = repository.CreateSpecification();
            return repository.FindAll(spec);
        }

        /// <summary>
        /// 保存域
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public void SaveDomain(Domain entry)
        {
            var dm = GetDomain(entry.Key);
            if (dm == null)
                repository.Create(entry);
            repository.Update(entry);
        }

        /// <summary>
        /// 删除域
        /// </summary>
        public void DeleteDomain(Domain entry)
        {
            repository.Delete(entry);
        }
    }
}
