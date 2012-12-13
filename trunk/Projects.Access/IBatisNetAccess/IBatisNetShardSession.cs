using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IBatisNet.DataMapper;

using Projects.Framework;
using Projects.Framework.Shards;

namespace Projects.Accesses.IBatisNetAccess
{
    public class IBatisNetShardSession<TEntity> : IShardSession<TEntity>
    {
        ISqlMapSession mSession;

        public ISqlMapSession InnerSession
        {
            get { return mSession; }
        }

        public void Create(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void SessionEvict(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool SessionContains(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(object id)
        {
            throw new NotImplementedException();
        }

        public IList<TEntity> GetList(System.Collections.IEnumerable ids)
        {
            throw new NotImplementedException();
        }

        public TEntity FindOne(Framework.Specification.ISpecification<TEntity> spec)
        {
            throw new NotImplementedException();
        }

        public IList<TEntity> FindAll(Framework.Specification.ISpecification<TEntity> spec)
        {
            throw new NotImplementedException();
        }

        public Tool.Pager.PagedList<TEntity> FindPaging(Framework.Specification.ISpecification<TEntity> spec)
        {
            throw new NotImplementedException();
        }

        public int Count(Framework.Specification.ISpecification<TEntity> spec)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
