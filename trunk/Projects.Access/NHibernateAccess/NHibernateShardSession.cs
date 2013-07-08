using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Linq;
using NHibernate.Engine;

using Projects.Tool;
using Projects.Tool.Shards;

namespace Projects.Framework.NHibernateAccess
{
    public class NHibernateShardSession<TEntity> : IShardSession<TEntity>
    {
        ISession session;

        public NHibernateShardSession(ISession session)
        {
            this.session = session;
        }

        public ISession InnerSession
        {
            get { return session; }
        }

        public void Create(TEntity entity)
        {
            session.Save(entity);
            session.Flush();
        }

        public void Update(TEntity entity)
        {
            var merge = MergeEntity(session, entity);
            session.Update(merge);
            session.Flush();
        }
        public void Delete(TEntity entity)
        {
            var merge = MergeEntity(session, entity);
            session.Delete(merge);
            session.Flush();
        }

        public void SessionEvict(TEntity entity)
        {
            session.Evict(entity);
        }

        public bool SessionContains(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(object id)
        {
            return session.Get<TEntity>(id);
        }

        public IList<TEntity> GetList(IEnumerable ids)
        {
            return session.GetList<TEntity>(ids);
        }

        public TEntity FindOne(ISpecification<TEntity> spec)
        {
            return ((NHibernateSpecification<TEntity>)spec).Query.FirstOrDefault();
        }

        public IList<TEntity> FindAll(ISpecification<TEntity> spec)
        {
            return ((NHibernateSpecification<TEntity>)spec).Query.ToList();
        }

        public PagingResult<TEntity> FindPaging(ISpecification<TEntity> spec)
        {
            NHibernateSpecification<TEntity> nhSpec = (NHibernateSpecification<TEntity>)spec;

            var cq = nhSpec.GetSession().Query<TEntity>();
            if (nhSpec.CriteriaExpression != null)
                cq = cq.Where(nhSpec.CriteriaExpression);

            PagingResult<TEntity> result = new PagingResult<TEntity>(cq.Count());
            result.AddRange(nhSpec.Query.ToList());
            return result;
        }

        public void Dispose()
        {
            session.Dispose();
        }

        object MergeEntity(ISession session, object entity)
        {
            if (!session.Contains(entity))
            {
                var impl = session.GetSessionImplementation();
                var entityPersister = impl.GetEntityPersister(null, entity);
                var id = entityPersister.GetIdentifier(entity, impl.EntityMode);

                EntityKey key = new EntityKey(id, entityPersister, impl.EntityMode);
                if (impl.PersistenceContext.ContainsEntity(key))
                    entity = (TEntity)session.Merge(entity);
            }
            return entity;
        }

        void ClearSession()
        {
            session.Clear();
        }

        void RemoveEntityFromSession<TEntity>(ISession session, object id)
        {
            var impl = session.GetSessionImplementation();

            var entityPersister = impl.Factory.GetEntityPersister(typeof(TEntity).FullName);
            EntityKey key = new EntityKey(id, entityPersister, impl.EntityMode);
            if (impl.PersistenceContext.ContainsEntity(key))
                impl.PersistenceContext.RemoveEntity(key);
        }

        public int Count(ISpecification<TEntity> spec)
        {
            NHibernateSpecification<TEntity> nhSpec = (NHibernateSpecification<TEntity>)spec;

            var cq = nhSpec.GetSession().Query<TEntity>();
            if (nhSpec.CriteriaExpression != null)
                cq = cq.Where(nhSpec.CriteriaExpression);

            return cq.Count();
        }
    }
}
