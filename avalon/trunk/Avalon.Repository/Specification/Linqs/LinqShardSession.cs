using Avalon.Framework.Shards;
using Avalon.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    internal class LinqShardSession<TEntity> : IShardSession<TEntity>
    {
        public LinqShardSession()
        {
            var metadata = RepositoryFramework.GetDefineMetadataAndCheck(typeof(TEntity));
        }

        List<TEntity> Data
        {
            get { return LinqDatabase.Instance.GetData<TEntity>(); }
        }

        public void Create(TEntity entity)
        {
            Data.Add(entity);
        }

        public void Update(TEntity entity)
        {
            Delete(entity);
            Data.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            var id = EntityUtil.GetId(entity);
            Data.RemoveAll(o => EntityUtil.GetId(o) == id);
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
            return Data.FirstOrDefault(o => EntityUtil.GetId(o) == id);
        }

        public IList<TEntity> GetList(IEnumerable ids)
        {
            List<TEntity> items = new List<TEntity>();
            foreach (var id in ids)
            {
                var entity = Get(id);
                if (entity != null)
                    items.Add(entity);
            }
            return items;
        }

        public TEntity FindOne(ISpecification<TEntity> spec)
        {
            LinqSpecification<TEntity> linqSpec = (LinqSpecification<TEntity>)spec;
            return linqSpec.Query.FirstOrDefault();
        }

        public IList<TEntity> FindAll(ISpecification<TEntity> spec)
        {
            LinqSpecification<TEntity> linqSpec = (LinqSpecification<TEntity>)spec;
            return linqSpec.Query.ToList();
        }

        public PagingResult<TEntity> FindPaging(ISpecification<TEntity> spec)
        {
            return null;
        }

        public int Count(ISpecification<TEntity> spec)
        {
            LinqSpecification<TEntity> linqSpec = (LinqSpecification<TEntity>)spec;
            return linqSpec.Query.Count();
        }

        public void Dispose()
        {
        }
    }
}
