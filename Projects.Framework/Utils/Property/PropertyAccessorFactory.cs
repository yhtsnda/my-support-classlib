using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 提升对象属性访问的性能
    /// </summary>
    public static class PropertyAccessorFactory
    {
        static Dictionary<Type, IPropertyAccessor> accessors = new Dictionary<Type, IPropertyAccessor>();
        static object syncRoot = new object();

        public static IPropertyAccessor GetPropertyAccess(Type type)
        {
            IPropertyAccessor accessor;
            if (!accessors.TryGetValue(type, out accessor))
            {
                lock (syncRoot)
                {
                    if (!accessors.TryGetValue(type, out accessor))
                    {
                        accessor = new DefaultPropertyAccessor(type);
                        accessors.Add(type, accessor);
                    }
                }
            }
            return accessor;
        }

        public static void SetId(object entity, object value)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var entityType = entity.GetType();
            var metadata = RepositoryFramework.GetDefineMetadata(entityType);
            if (metadata == null)
                throw new ArgumentNullException("metadata");

            var pa = GetPropertyAccess(metadata.EntityType);
            pa.GetSetter(metadata.IdMember.Name).Set(entity, value);
        }

        public static object GetId(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var entityType = entity.GetType();
            var metadata = RepositoryFramework.GetDefineMetadata(entityType);
            if (metadata == null)
                throw new ArgumentNullException("metadata");

            var pa = GetPropertyAccess(metadata.EntityType);
            return pa.GetGetter(metadata.IdMember.Name).Get(entity);
        }
    }
}
