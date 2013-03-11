using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Engine;

using Projects.Framework;
using Projects.Framework.NHibernateAccess;
using Projects.Framework.Shards;
using Projects.Tool;
using Projects.Framework;

namespace Projects.Repository
{
    public static class NHibernateRepositoryExtend
    {
        public static ISession GetSession<TEntity>(this AbstractRepository<TEntity> repository, ShardParams shardParams) where
            TEntity : class
        {
            var nr = (AbstractRepository<TEntity>)repository;
            return ((NHibernateShardSession<TEntity>)nr.OpenSession(shardParams)).InnerSession;
        }

        public static ISession GetSession<TEntity>(this AbstractShardRepository<TEntity> repository) where
           TEntity : class
        {
            var nr = (AbstractRepository<TEntity>)repository;
            return ((NHibernateShardSession<TEntity>)nr.OpenSession(ShardParams.Empty)).InnerSession;
        }

        public static ISession GetSession<TEntity>(this AbstractUserShardRepository<TEntity> repository, long userId) where
           TEntity : class
        {
            var nr = (AbstractRepository<TEntity>)repository;
            return ((NHibernateShardSession<TEntity>)nr.OpenSession(ShardParams.Form(userId))).InnerSession;
        }

        //public static void Evict<TEntity>(this AbstractUserShardRepository<TEntity> repository, TEntity entity) where
        //   TEntity : class
        //{
        //    var session = ((NHibernateShardSession<TEntity>)repository.OpenSession(entity)).InnerSession;

        //    var impl = session.GetSessionImplementation();
        //    var entityPersister = impl.GetEntityPersister(typeof(TEntity).FullName, entity);
        //    var id = entityPersister.GetIdentifier(entity, impl.EntityMode);

        //    EntityKey key = new EntityKey(id, entityPersister, impl.EntityMode);
        //    if (impl.PersistenceContext.ContainsEntity(key))
        //        impl.PersistenceContext.RemoveEntity(key);
        //}


    }
}