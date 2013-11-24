using Avalon.Utility;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    internal static class CascadeImpl
    {
        public static void OnCreate(object entity)
        {
            var processors = GetCascadeProcessors(entity.GetType());
            processors.ForEach(o => o.OnCreate(entity));
        }

        public static void OnUpdate(object entity)
        {
            // var processors = GetCascadeProcessorsForUpdate(entity);
            var processors = GetCascadeProcessors(entity.GetType());
            processors.ForEach(o => o.OnUpdate(entity));
        }

        public static void OnDelete(object entity)
        {
            var processors = GetCascadeProcessors(entity.GetType());
            processors.ForEach(o => o.OnDelete(entity));
        }

        static IEnumerable<IClassJoinCascadeProcessor> GetCascadeProcessors(Type type)
        {
            var metadata = RepositoryFramework.GetDefineMetadata(type);
            if (metadata == null)
                throw new ArgumentNullException("metadata");

            return metadata.ClassJoinDefines.Where(o => o.Value.JoinType == MethodJoinType.PropertyGet && o.Value.JoinCascade.CascataType != CascadeType.None)
                .Select(o => o.Value.JoinCascade.Processor);
        }

        //static IEnumerable<IClassJoinCascadeProcessor> GetCascadeProcessorsForUpdate(object entity)
        //{
        //    var proxy = entity as IProxyTargetAccessor;
        //    if (proxy != null)
        //    {
        //        EntityInterceptor ei = (EntityInterceptor)proxy.GetInterceptors().FirstOrDefault(o => o.GetType() == typeof(EntityInterceptor));
        //        if (ei != null)
        //        {
        //            var metadata = RepositoryFramework.GetDefineMetadata(entity.GetType());
        //            if (metadata == null)
        //                throw new ArgumentNullException("metadata");

        //            return metadata.ClassJoinDefines.Where(o => o.Value.JoinType == MethodJoinType.PropertyGet && o.Value.JoinCascade.CascataType != CascadeType.None && ei.PropertyInited(o.Value.JoinName))
        //                .Select(o => o.Value.JoinCascade.Processor);
        //        }
        //    }
        //    return GetCascadeProcessors(entity.GetType());
        //}

        internal static bool IsDefault(object value)
        {
            if (value == null)
                return true;

            var type = value.GetType();
            if (type.IsValueType)
                return value.Equals(FastActivator.Create(type));

            return false;
        }
    }
}
