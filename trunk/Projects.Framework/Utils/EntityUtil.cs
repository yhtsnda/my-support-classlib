using Projects.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Projects.Framework
{
    public static class EntityUtil
    {
        public static object[] GetValuesForCache(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var cache = GetCache((object)entity);
            var metadata = RepositoryFramework.GetDefineMetadata(cache.GetType());
            if (metadata != null)
                return metadata.GetData(entity);

            var pa = PropertyAccessorFactory.GetPropertyAccess(cache.GetType());
            return pa.GetDatasHandler(entity);
        }

        public static object SetValueForCache(Type type, object[] values)
        {
            var metadata = RepositoryFramework.GetDefineMetadata(type);
            if (metadata != null)
                return metadata.CreateInstanceFormDatas(values);

            var pa = PropertyAccessorFactory.GetPropertyAccess(type);
            var entity = pa.CreateInstance();
            pa.SetDatasHandler(entity, values);

            return entity;
        }

        public static void CheckVirtualType(Type type)
        {
            List<string> novirtuals = new List<string>();

            foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (!method.IsConstructor
                    && (method.IsPublic || method.IsAssembly || method.IsFamily || method.IsFamilyOrAssembly)
                    && method.Name != "GetType" && method.Name != "MemberwiseClone"
                    && !method.IsVirtual)
                {
                    novirtuals.Add(String.Format("类型 {0} 方法 {1} 必须为 virtual。", type.ToPrettyString(), method.Name));
                }
            }

            if (novirtuals.Count > 0)
                throw new PlatformException(String.Join("\r\n", novirtuals));
        }

        public static bool IsGenericList(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>) || type.GetInterface(typeof(IList<>).FullName) != null;
        }

        public static bool IsPagingResult(Type type)
        {
            if (type == null)
                return false;

            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(PagingResult<>) || IsPagingResult(type.BaseType);
        }

        public static object GetCache(object entity)
        {
            if (entity == null)
                return entity;

            var metadata = RepositoryFramework.GetDefineMetadata(entity.GetType());
            if (metadata == null)
                return entity;

            var pa = PropertyAccessorFactory.GetPropertyAccess(metadata.EntityType);
            var poco = pa.CreateInstance();
            pa.MergeData(entity, poco);

            return poco;
        }

        public static object GetPersistent(object entity)
        {
            return GetCache(entity);
        }

        public static void MergeObject(object source, object destination)
        {
            if (destination == source || destination == null || source == null)
                return;

            var metadata = RepositoryFramework.GetDefineMetadata(destination.GetType());
            if (metadata != null)
                metadata.MergeData(source, destination);
            else
            {
                var pa = PropertyAccessorFactory.GetPropertyAccess(destination.GetType());
                pa.MergeData(source, destination);
            }
        }

        public static void Persistent<TEntity>(TEntity entity, Action<TEntity> action) where TEntity : class
        {
            var persistent = EntityUtil.GetPersistent(entity) as TEntity;
            action(persistent);
            EntityUtil.MergeObject(persistent, entity);
        }
    }
}
