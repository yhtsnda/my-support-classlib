using Projects.Tool;
using Projects.Tool.Shards;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    internal class LinqShardSession<TEntity> : IShardSession<TEntity>
    {
        IGetter idGetter;

        public LinqShardSession()
        {
            var metadata = RepositoryFramework.GetDefineMetadataAndCheck(typeof(TEntity));
            var accessor = PropertyAccessorFactory.GetPropertyAccess(typeof(TEntity));
            idGetter = accessor.GetGetter(metadata.IdMember.Name);
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
            var id = idGetter.Get(entity);
            Data.RemoveAll(o => idGetter.Get(o) == id);
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
            return Data.FirstOrDefault(o => idGetter.Get(o) == id);
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
