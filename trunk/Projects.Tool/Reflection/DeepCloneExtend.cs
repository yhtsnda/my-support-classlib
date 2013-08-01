using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Projects.Tool.Reflection
{
    /// <summary>
    /// 对象深度克隆
    /// </summary>
    public static class DeepCloneExtend
    {
        static readonly string CloneableInterface = typeof(ICloneable).FullName;

        public static object DeepClone(this object instance)
        {
            if (instance == null)
                return null;

            return Clone(instance, new VisitedGraph());
        }

        public static object DeepClone(this object instance, object clone)
        {
            if (instance == null)
                return null;

            if (clone == null)
                throw new ArgumentNullException("The clone instance cannot be null");

            return Clone(instance, new VisitedGraph(), clone);
        }

        static object Clone(object instance, VisitedGraph visited)
        {
            if (instance == null)
                return null;

            Type instanceType = instance.GetType();

            if (instanceType.IsPointer || instanceType == typeof(Pointer) || instanceType.IsPrimitive || instanceType == typeof(string))
                return instance; // Pointers, primitive types and strings are considered immutable

            if (instanceType.GetInterface(CloneableInterface) != null)
                return ((ICloneable)instance).Clone();

            if (typeof(Type).IsAssignableFrom(instanceType) || typeof(MemberInfo).IsAssignableFrom(instanceType) || typeof(ParameterInfo).IsAssignableFrom(instanceType))
                return instance;

            if (instanceType.IsArray)
            {
                var array = (Array)instance;
                int length = array.Length;
                Array copied = (Array)Activator.CreateInstance(instanceType, length);
                visited.Add(instance, copied);
                for (int i = 0; i < length; ++i)
                {
                    var clone = Clone(array.GetValue(i), visited);
                    copied.SetValue(clone, i);
                }
                return copied;
            }

            return Clone(instance, visited, CreateInstance(instanceType));
        }

        static object Clone(object instance, VisitedGraph visited, object clone)
        {
            if (visited.ContainsKey(instance))
                return visited[instance];

            visited.Add(instance, clone);

            Type type = instance.GetType();

            while (type != null)
            {
                var ta = TypeAccessor.GetAccessor(type);
                foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    object value = ta.GetField(field.Name, instance);
                    object cloneValue = visited.ContainsKey(value) ? visited[value] : Clone(value, visited);
                    ta.SetField(field.Name, clone, cloneValue);
                }

                type = type.BaseType;
            }
            return clone;
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
