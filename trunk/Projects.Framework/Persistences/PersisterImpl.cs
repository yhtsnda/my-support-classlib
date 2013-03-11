using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    internal class PersisterImpl
    {
        public static IPersister CreatePersister(IInvocation invocation, ClassDefineMetadata metadata)
        {
            DataSourcePersister dataSource = new DataSourcePersister(invocation, null);
            SecondLevelPersister secondLevel = new SecondLevelPersister(metadata, RepositoryFramework.GetCacher(metadata), dataSource);
            SessionPersister session = new SessionPersister(SessionCache.Current, secondLevel);
            return session;
        }
    }
}
