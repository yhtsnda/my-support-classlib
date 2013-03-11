using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate.Event;
using Projects.Framework;

namespace Projects.Framework
{
    internal class RaiseEventListener :
        IPreInsertEventListener,
        IPreUpdateEventListener,
        IPreDeleteEventListener,
        IPostInsertEventListener,
        IPostUpdateEventListener,
        IPostDeleteEventListener,
        IPostLoadEventListener
    {
        public bool OnPreInsert(PreInsertEvent @event)
        {
            RepositoryFramework.RaisePreCreate(@event.Entity);
            return false;
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            RepositoryFramework.RaisePreUpdate(@event.Entity);
            return false;
        }

        public bool OnPreDelete(PreDeleteEvent @event)
        {
            RepositoryFramework.RaisePreDelete(@event.Entity);
            return false;
        }

        public void OnPostInsert(PostInsertEvent @event)
        {
            RepositoryFramework.RaisePostCreate(@event.Entity);
        }

        public void OnPostUpdate(PostUpdateEvent @event)
        {
            RepositoryFramework.RaisePostUpdate(@event.Entity);
        }

        public void OnPostDelete(PostDeleteEvent @event)
        {
            RepositoryFramework.RaisePostDelere(@event.Entity);
        }

        public void OnPostLoad(PostLoadEvent @event)
        {
            //RepositoryFramework.RaisePostLoad(@event.Entity);
        }
    }
}
