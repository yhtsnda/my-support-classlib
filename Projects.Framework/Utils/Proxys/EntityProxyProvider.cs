using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    public class EntityProxyProvider : IProxyProvider
    {
        public bool IsMatch(Type type)
        {
            return RepositoryFramework.GetDefineMetadata(type) != null;
        }

        public object Proxy(object entity, Func<object, object> subProxyHandler)
        {
            var metadata = RepositoryFramework.GetDefineMetadata(entity.GetType());

            var proxy = entity;
            var proxyed = false;
            if (!ProxyProvider.IsProxy(entity) && metadata.ClassJoinDefines.Count > 0)
            {
                proxy = ProxyProvider.CreateEntityProxy(metadata.EntityType);
                metadata.MergeData(entity, proxy);
                proxyed = true;
            }

            var pa = PropertyAccessorFactory.GetPropertyAccess(metadata.EntityType);
            foreach (var property in metadata.CascadeProperties)
            {
                if (EntityInterceptor.PropertyInited(entity, property.Name))
                {
                    var value = pa.GetGetter(property.Name).Get(entity);
                    if (value != null)
                    {
                        var valueProxy = subProxyHandler(value);
                        if (valueProxy != value || proxyed)
                            pa.GetSetter(property.Name).Set(proxy, valueProxy);
                    }
                }
            }

            return proxy;
        }

        public object Poco(object entity, Func<object, object> subPocoHandler)
        {
            var metadata = RepositoryFramework.GetDefineMetadata(entity.GetType());
            var pa = PropertyAccessorFactory.GetPropertyAccess(metadata.EntityType);

            var poco = entity;
            var pocoed = false;
            if (ProxyProvider.IsProxy(entity) && metadata.ClassJoinDefines.Count > 0)
            {
                poco = pa.CreateInstance();
                metadata.MergeData(entity, poco);
                pocoed = true;
            }

            foreach (var property in metadata.CascadeProperties)
            {
                var value = pa.GetGetter(property.Name).Get(entity);
                if (value != null)
                {
                    var valuePoco = subPocoHandler(value);
                    if (valuePoco != value || pocoed)
                        pa.GetSetter(property.Name).Set(poco, valuePoco);
                }
            }

            return poco;
        }

        public void Fetch(object entity, Action<object> subFetchHandler)
        {
            var metadata = RepositoryFramework.GetDefineMetadata(entity.GetType());
            var pa = PropertyAccessorFactory.GetPropertyAccess(metadata.EntityType);

            foreach (var property in metadata.CascadeProperties)
            {
                var value = pa.GetGetter(property.Name).Get(entity);
                if (value != null)
                    subFetchHandler(value);
            }
        }
    }
}
