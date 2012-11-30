using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Accesses.MongoAccess;
using Projects.Tool.Pager;
using Projects.Framework;

using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace Projects.Repository
{
    public abstract class AbstractMongoRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public ISpecification<TEntity> CreateSpecification(object searchObj, int? pageIndex = -1, int? pageSize = 12)
        {
            throw new NotImplementedException();
        }

        public virtual ActionResult<ResultKey, TEntity> Create(TEntity entity)
        {
            MongoAccessor.Insert<TEntity>(entity);
            return new ActionResult<ResultKey, TEntity>();
        }

        public virtual ActionResult<ResultKey, object> Update(TEntity entity)
        {
            MongoAccessor.Update(entity);
            return new ActionResult<ResultKey, object>();
        }

        public virtual ActionResult<ResultKey, object> Delete(TEntity entity)
        {
            MongoAccessor.Delete(entity);
            return new ActionResult<ResultKey, object>();
        }

        public virtual TEntity Get(object id)
        {
            return MongoAccessor.GetCollection<TEntity>().FindOneByIdAs<TEntity>(BsonValue.Create(id));
        }

        public virtual IList<TEntity> GetList(IEnumerable<int> ids)
        {
            QueryComplete query = Query.In("_id", BsonArray.Create(ids));
            return MongoAccessor.GetCollection<TEntity>().FindAs<TEntity>(query).ToList();
        }

        public virtual TEntity FindOne(ISpecification<TEntity> spec)
        {
            return ((MongoSpecification<TEntity>)spec).Query.FirstOrDefault();
        }

        public virtual IList<TEntity> FindAll(ISpecification<TEntity> spec)
        {
            return ((MongoSpecification<TEntity>)spec).Query.ToList();
        }

        public PagedList<TEntity> FindPaging(ISpecification<TEntity> spec)
        {
            throw new NotImplementedException();
        }

        public int GetCount(ISpecification<TEntity> spec)
        {
            throw new NotImplementedException();
        }
    }
}
