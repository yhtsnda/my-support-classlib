using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Event.Default;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using NHibernate.Proxy;

namespace Projects.Framework.NHibernateAccess
{
    /// <summary>
    /// 对 Session 进行扩展
    /// </summary>
    internal static class SessionExtend
    {
        /// <summary>
        /// 使用主键批量获取对象集合。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static IList<T> GetList<T>(this ISession session, IEnumerable ids)
        {
            CacheLoader loader = new CacheLoader();
            var entityPersister = session.GetSessionImplementation().GetEntityPersister(typeof(T).FullName, null);
            var entityName = typeof(T).FullName;
            var eventSource = (IEventSource)session;
            var loadType = LoadEventListener.Get;

            List<T> output = new List<T>();
            ArrayList missings = new ArrayList();
            foreach (object id in ids)
            {
                EntityKey entityKey = new EntityKey(id, entityPersister, EntityMode.Poco);
                LoadEvent loadEvent = new LoadEvent(id, entityName, false, eventSource);

                var entity = loader.LoadFromSessionCache(loadEvent, entityKey, loadType);
                if (entity == null)
                    entity = loader.LoadFromSecondLevelCache(loadEvent, entityPersister, loadType);

                if (entity != null)
                    output.Add((T)entity);
                else
                    missings.Add(id);
            }

            if (missings.Count > 0)
            {
                //对组合键进行处理
                if (entityPersister.IdentifierType.IsComponentType)
                {
                    ComponentType idType = (ComponentType)entityPersister.IdentifierType;

                    List<HashSet<object>> props = new List<HashSet<object>>();
                    foreach (string name in idType.PropertyNames)
                        props.Add(new HashSet<object>());

                    foreach (var missing in missings)
                    {
                        var values = idType.GetPropertyValues(missing, EntityMode.Poco);
                        for (int i = 0; i < values.Length; i++)
                            props[i].Add(values[i]);
                    }

                    var cr = session.CreateCriteria(entityName);

                    //合并为查询
                    int index = 0;
                    foreach (HashSet<object> set in props)
                    {
                        string propName = entityPersister.IdentifierPropertyName + "." + idType.PropertyNames[index];
                        if (set.Count == 1)
                            cr = cr.Add(Expression.Eq(propName, set.First()));
                        else
                            cr = cr.Add(Expression.In(propName, set.ToArray()));
                        index++;
                    }

                    output.AddRange(cr.List<T>());
                }
                else
                {
                    output.AddRange(session.CreateCriteria(entityName)
                        .Add(Expression.In(entityPersister.IdentifierPropertyName, missings))
                        .List<T>());
                }
            }

            return output;
        }


        public static bool IsDirtyEntity(this ISession session, object entity)
        {
            ISessionImplementor sessionImpl = session.GetSessionImplementation();
            IPersistenceContext persistenceContext = sessionImpl.PersistenceContext;
            EntityEntry oldEntry = persistenceContext.GetEntry(entity);

            string className = oldEntry.EntityName;
            IEntityPersister persister = sessionImpl.Factory.GetEntityPersister(className);

            if ((oldEntry == null) && (entity is INHibernateProxy))
            {
                INHibernateProxy proxy = entity as INHibernateProxy;
                object obj = sessionImpl.PersistenceContext.Unproxy(proxy);
                oldEntry = sessionImpl.PersistenceContext.GetEntry(obj);
            }

            object[] oldState = oldEntry.LoadedState;
            object[] currentState = persister.GetPropertyValues(entity, sessionImpl.EntityMode);

            int[] dirtyProps = oldState.Select((o, i) => (oldState[i] == currentState[i]) ? -1 : i).Where(x => x >= 0).ToArray();
            return (dirtyProps != null);
        }

        public static bool IsDirtyProperty(this ISession session, object entity, string propertyName)
        {
            ISessionImplementor sessionImpl = session.GetSessionImplementation();
            IPersistenceContext persistenceContext = sessionImpl.PersistenceContext;
            EntityEntry oldEntry = persistenceContext.GetEntry(entity);
            string className = oldEntry.EntityName;

            IEntityPersister persister = sessionImpl.Factory.GetEntityPersister(className);
            if ((oldEntry == null) && (entity is INHibernateProxy))
            {
                INHibernateProxy proxy = entity as INHibernateProxy;
                object obj = sessionImpl.PersistenceContext.Unproxy(proxy);
                oldEntry = sessionImpl.PersistenceContext.GetEntry(obj);
            }

            object[] oldState = oldEntry.LoadedState;
            object[] currentState = persister.GetPropertyValues(entity, sessionImpl.EntityMode);
            int[] dirtyProps = persister.FindDirty(currentState, oldState, entity, sessionImpl);
            int index = Array.IndexOf(persister.PropertyNames, propertyName);

            bool isDirty = (dirtyProps != null) ? (Array.IndexOf(dirtyProps, index) != -1) : false;
            return (isDirty);
        }

        public static object GetOriginalEntityProperty(this ISession session, object entity, string propertyName)
        {
            ISessionImplementor sessionImpl = session.GetSessionImplementation();
            IPersistenceContext persistenceContext = sessionImpl.PersistenceContext;
            EntityEntry oldEntry = persistenceContext.GetEntry(entity);
            string className = oldEntry.EntityName;
            IEntityPersister persister = sessionImpl.Factory.GetEntityPersister(className);

            if ((oldEntry == null) && (entity is INHibernateProxy))
            {
                INHibernateProxy proxy = entity as INHibernateProxy;
                object obj = sessionImpl.PersistenceContext.Unproxy(proxy);
                oldEntry = sessionImpl.PersistenceContext.GetEntry(obj);
            }

            object[] oldState = oldEntry.LoadedState;
            object[] currentState = persister.GetPropertyValues(entity, sessionImpl.EntityMode);
            int[] dirtyProps = persister.FindDirty(currentState, oldState, entity, sessionImpl);
            int index = Array.IndexOf(persister.PropertyNames, propertyName);
            bool isDirty = (dirtyProps != null) ? (Array.IndexOf(dirtyProps, index) != -1) : false;

            return ((isDirty == true) ? oldState[index] : currentState[index]);
        }

        private class CacheLoader : DefaultLoadEventListener
        {
            public new object LoadFromSessionCache(LoadEvent @event, EntityKey keyToLoad, LoadType options)
            {
                return base.LoadFromSessionCache(@event, keyToLoad, options);
            }

            public new object LoadFromSecondLevelCache(LoadEvent @event, IEntityPersister persister, LoadType options)
            {
                return base.LoadFromSecondLevelCache(@event, persister, options);
            }
        }
    }
}
