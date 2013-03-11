using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    internal class SourceEntityProxyProvider : IProxyProvider
    {
        public bool IsMatch(Type type)
        {
            return RepositoryFramework.GetDefineMetadata(type) != null;
        }

        public object Proxy(object entity, Func<object, object> subProxyHandler)
        {
            var metadata = RepositoryFramework.GetDefineMetadata(entity.GetType());

            var proxy = entity;
            //if (!ProxyProvider.IsProxy(entity) && metadata.ClassJoinDefines.Count > 0)
            if (!ProxyProvider.IsProxy(entity))
            {
                proxy = ProxyProvider.CreateEntityProxy(metadata.EntityType);
                metadata.MergeData(entity, proxy);
            }
            return proxy;
        }


        public object Poco(object entity, Func<object, object> subPocoHandler)
        {
            throw new NotImplementedException();
        }


        public void Fetch(object entity, Action<object> subFetchHandler)
        {
            throw new NotImplementedException();
        }
    }
}
