using Avalon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Avalon.Utility
{
    public static class EntityUtil
    {
        static Dictionary<Type, Action<object, object>> clones = new Dictionary<Type, Action<object, object>>();
        static object syncRoot = new object();

        static EntityUtil()
        {
            OriginalObjectProvider = new EmptyOriginalObjectProvider();
        }

        public static IOriginalObjectProvider OriginalObjectProvider { get; set; }

        internal static void CheckVirtualType(Type type)
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
                throw new AvalonException(String.Join("\r\n", novirtuals));
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

        public static object GetId(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var metadata = RepositoryFramework.GetDefineMetadataAndCheck(entity.GetType());
            var ta = TypeAccessor.GetAccessor(metadata.EntityType);
            return ta.GetProperty(metadata.IdMember.Name, entity);
        }
    }
}
