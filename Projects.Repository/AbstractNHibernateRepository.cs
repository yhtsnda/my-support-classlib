using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Framework;

namespace Projects.Repository
{
    /// <summary>
    /// 使用NHibernate作为仓储操作的抽象类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class AbstractNHibernateRepository<TEntity> 
        : IRepository<TEntity> where TEntity :class
    {

        public ISpecification<TEntity> CreateSpecification(object searchObj, 
            int? pageIndex = -1, int? pageSize = 12)
        {
            throw new NotImplementedException();
        }

        public ActionResult<ResultKey, TEntity> Create(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public ActionResult<ResultKey, object> Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public ActionResult<ResultKey, object> Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(object id)
        {
            throw new NotImplementedException();
        }

        public IList<TEntity> GetList(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public TEntity FindOne(ISpecification<TEntity> spec)
        {
            throw new NotImplementedException();
        }

        public IList<TEntity> FindAll(ISpecification<TEntity> spec)
        {
            throw new NotImplementedException();
        }

        public Tool.Pager.PagedList<TEntity> FindPaging(ISpecification<TEntity> spec)
        {
            throw new NotImplementedException();
        }

        public int GetCount(ISpecification<TEntity> spec)
        {
            throw new NotImplementedException();
        }
    }
}
