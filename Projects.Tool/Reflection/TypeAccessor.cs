using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Web.Compilation;

namespace Projects.Tool.Reflection
{
    public class TypeAccessor
    {
        static Dictionary<Type, TypeAccessor> accessors = new Dictionary<Type, TypeAccessor>();

        Type type;
        Dictionary<string, Func<object, object>> propertyGetterDic;
        Dictionary<string, Action<object, object>> propertySetterDic;

        IList<PropertyInfo> readWriteProperties;
        IList<Action<object, object>> readWriterPropertyCloners;
        IList<Func<object, object>> readWritePropertyGetters;
        IList<Action<object, object>> readWritePropertySetters;

        Dictionary<string, Func<object, object>> fieldGetterDic;
        Dictionary<string, Action<object, object>> fieldSetterDic;

        Dictionary<string, Action<object, object>> clonePropertyDic;
        Dictionary<string, Action<object, object>> cloneFieldDic;
        IList<Action<object, object>> fieldCloners;

        private TypeAccessor(Type type)
        {
            this.type = type;

            propertyGetterDic = new Dictionary<string, Func<object, object>>();
            propertySetterDic = new Dictionary<string, Action<object, object>>();

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(o => o.GetIndexParameters().Length == 0).ToList();
            propertyGetterDic = properties.Where(o => o.CanRead).ToDictionary(o => o.Name, o => DelegateAccessor.CreatePropertyGetter(o));
            propertySetterDic = properties.Where(o => o.CanWrite).ToDictionary(o => o.Name, o => DelegateAccessor.CreatePropertySetter(o));

            readWriteProperties = properties.Where(o => o.CanRead && o.CanWrite).ToList();
            readWritePropertyGetters = readWriteProperties.Select(o => propertyGetterDic[o.Name]).ToList();
            readWritePropertySetters = readWriteProperties.Select(o => propertySetterDic[o.Name]).ToList();

            clonePropertyDic = readWriteProperties.ToDictionary(o => o.Name, o => DelegateAccessor.CreatePropertyCloner(o));

            readWriterPropertyCloners = readWriteProperties.Select(o => clonePropertyDic[o.Name]).ToList();

            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
            fieldGetterDic = fields.ToDictionary(o => o.Name, o => DelegateAccessor.CreateFieldGetter(o));
            fieldSetterDic = fields.Where(o => !o.IsInitOnly).ToDictionary(o => o.Name, o => DelegateAccessor.CreateFieldSetter(o));

            cloneFieldDic = fields.Where(o => !o.IsInitOnly).ToDictionary(o => o.Name, o => DelegateAccessor.CreateFieldCloner(o));
            fieldCloners = fields.Where(o => !o.IsInitOnly).Select(o => cloneFieldDic[o.Name]).ToList();
        }

        public static TypeAccessor GetAccessor(Type type)
        {
            TypeAccessor accessor;
            if (!accessors.TryGetValue(type, out accessor))
            {
                lock (accessors)
                {
                    if (!accessors.TryGetValue(type, out accessor))
                    {
                        accessor = new TypeAccessor(type);
                        accessors.Add(type, accessor);
                    }
                }
            }
            return accessor;
        }

        public Type Type
        {
            get { return type; }
        }

        public object Create()
        {
            return FastActivator.Create(type);
        }

        public Action<object, object> GetPropertyClone(string propertyName)
        {
            return clonePropertyDic.TryGetValue(propertyName);
        }

        public Action<object, object> GetFieldClone(string fieldName)
        {
            return cloneFieldDic.TryGetValue(fieldName);
        }

        public Func<object, object> GetPropertyGetter(string propertyName)
        {
            return propertyGetterDic.TryGetValue(propertyName);
        }

        public Action<object, object> GetPropertySetter(string propertyName)
        {
            return propertySetterDic.TryGetValue(propertyName);
        }

        public Func<object, object> GetFieldGetter(string propertyName)
        {
            return fieldGetterDic.TryGetValue(propertyName);
        }

        public Action<object, object> GetFieldSetter(string propertyName)
        {
            return fieldSetterDic.TryGetValue(propertyName);
        }

        public object GetProperty(string propertyName, object instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            Func<object, object> getter;
            if (propertyGetterDic.TryGetValue(propertyName, out getter))
            {
                return getter(instance);
            }
            throw new ArgumentOutOfRangeException("propertyName", String.Format("对象 {0} 没有命名为 {1} 的属性", instance.GetType().FullName, propertyName));
        }

        public void SetProperty(string propertyName, object instance, object value)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            Action<object, object> setter;
            if (propertySetterDic.TryGetValue(propertyName, out setter))
            {
                setter(instance, value);
                return;
            }
            throw new ArgumentOutOfRangeException("propertyName", String.Format("对象 {0} 没有命名为 {1} 的属性", instance.GetType().FullName, propertyName));
        }

        public object GetField(string fieldName, object instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            Func<object, object> getter;
            if (fieldGetterDic.TryGetValue(fieldName, out getter))
            {
                return getter(instance);
            }
            throw new ArgumentOutOfRangeException("fieldName", String.Format("对象 {0} 没有命名为 {1} 的字段", instance.GetType().FullName, fieldName));
        }

        public void SetField(string fieldName, object instance, object value)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            Action<object, object> setter;
            if (fieldSetterDic.TryGetValue(fieldName, out setter))
            {
                setter(instance, value);
            }
            throw new ArgumentOutOfRangeException("fieldName", String.Format("对象 {0} 没有命名为 {1} 的属性", instance.GetType().FullName, fieldName));
        }

        public IList<PropertyInfo> ReadWriteProperties
        {
            get { return readWriteProperties; }
        }

        public object[] GetReadWritePropertyValues(object entity)
        {
            return readWritePropertyGetters.Select(o => o(entity)).ToArray();
        }

        public void SetReadWritePropertyValues(object entity, object[] values)
        {
            if (values.Length != readWriteProperties.Count)
                throw new ArgumentException(String.Format("给定的值个数 {0} 与属性的个数 {1} 不一致，对象类型为 {2}。", values.Length, readWriteProperties.Count, type.FullName));
            for (var i = 0; i < values.Length; i++)
                readWritePropertySetters[i](entity, values[i]);
        }

        public object CloneFromProperty(object source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var target = Create();
            readWriterPropertyCloners.ForEach(o => o(source, target));
            return target;
        }

        public object CloneFromField(object source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var target = Create();
            CloneFromField(source, target);
            return target;
        }

        public void CloneFromField(object source, object target)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                throw new ArgumentNullException("target");

            fieldCloners.ForEach(o => o(source, target));
        }

        public IDictionary<string, object> GetFieldValueDictionary(object instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");
            return fieldGetterDic.ToDictionary(o => o.Key, o => o.Value(instance));
        }
    }
}
