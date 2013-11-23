using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Avalon.Utility
{
    /// <summary>
    /// 对象深度克隆
    /// </summary>
    public static class DeepCloneExtend
    {
        static readonly string CloneableInterface = typeof(ICloneable).FullName;
        static HashSet<Type> noCloneHash;
        static object syncRoot;

        static DeepCloneExtend()
        {
            syncRoot = new object();

            noCloneHash = new HashSet<Type>(new[]{
                typeof(bool),
                typeof(byte),
                typeof(sbyte),
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(IntPtr),
                typeof(UIntPtr),
                typeof(char),
                typeof(double),
                typeof(Single),
                typeof(string),
                typeof(DateTime),
                typeof(TimeSpan),
                typeof(Guid)
            });
        }

        public static object DeepClone(this object instance)
        {
            if (instance == null)
                return null;

            var visited = new VisitedGraph();
            return Clone(instance, visited);
        }

        public static object DeepClone(this object instance, object clone)
        {
            if (instance == null)
                return null;

            if (clone == null)
                throw new ArgumentNullException("The clone instance cannot be null");

            Clone(instance, new VisitedGraph(), clone);
            return clone;
        }

        static object Clone(object instance, VisitedGraph visited)
        {
            if (instance == null)
                return null;

            Type instanceType = instance.GetType();
            if (noCloneHash.Contains(instanceType))
                return instance;

            if (instanceType.IsEnum || instanceType.IsPointer || instanceType == typeof(Pointer))
            {
                lock (syncRoot)
                {
                    noCloneHash.Add(instanceType);
                    return instance;
                }
            }

            object clone = visited.TryGetValue(instance);
            if (clone != null)
                return clone;

            if (!instanceType.IsArray && instance is ICloneable)
            {
                clone = ((ICloneable)instance).Clone();
                visited.Add(instance, clone);
                return clone;
            }

            if (typeof(Type).IsAssignableFrom(instanceType) || typeof(MemberInfo).IsAssignableFrom(instanceType) || typeof(ParameterInfo).IsAssignableFrom(instanceType))
            {
                visited.Add(instance, instance);
                return instance;
            }

            if (instanceType.IsArray)
            {
                var array = (Array)instance;
                int length = array.Length;
                Array copied = (Array)Activator.CreateInstance(instanceType, length);
                visited.Add(instance, copied);
                for (int i = 0; i < length; ++i)
                {
                    var c = Clone(array.GetValue(i), visited);
                    copied.SetValue(c, i);
                }
                return copied;
            }

            clone = CreateInstance(instanceType);
            visited.Add(instance, clone);
            Clone(instance, visited, clone);

            return clone;
        }

        static void Clone(object instance, VisitedGraph visited, object clone)
        {
            Type type = instance.GetType();

            while (type != null)
            {
                var ta = TypeAccessor.GetAccessor(type);
                ta.CloneByFields(instance, clone, (v) => Clone(v, visited));
                type = type.BaseType;
            }
        }

        static object CreateInstance(Type instanceType)
        {
            try
            {
                return FormatterServices.GetUninitializedObject(instanceType);
            }
            catch
            {
                throw new ArgumentException(string.Format("Object of type {0} cannot be cloned because an uninitialized object could not be created.", instanceType.FullName));
            }
        }

        class VisitedGraph : Dictionary<object, object>
        {
            public new bool ContainsKey(object key)
            {
                if (key == null)
                    return true;
                return base.ContainsKey(key);
            }

            public new object this[object key]
            {
                get
                {
                    if (key == null)
                        return null;
                    return base[key];
                }
            }
        }
    }
}
