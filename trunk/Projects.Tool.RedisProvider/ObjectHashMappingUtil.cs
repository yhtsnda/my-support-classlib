using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;
using Projects.Tool.Reflection;

namespace Projects.Tool.RedisProvider
{
    public static class ObjectHashMappingUtil
    {
        const string NullValue = "$<NULL>$";

        public static Dictionary<string, string> ToHash(object instance, string keyPrefix = "")
        {
            if (instance == null)
                throw new ArgumentNullException("entity");

            Dictionary<string, string> dic = new Dictionary<string, string>();
            TypeAccessor ta = TypeAccessor.GetAccessor(instance.GetType());
            if (ta.ReadWriteProperties.Count == 0)
                throw new NotSupportedException(String.Format("对象 {0} 无任何的属性，无法使用 RedisHashCache 请使用 RedisCache.", instance.GetType().FullName));

            var values = ta.GetReadWritePropertyValues(instance);
            int index = 0;
            foreach (var property in ta.ReadWriteProperties)
            {
                dic.Add(keyPrefix + property.Name, ToString(values[index], property.PropertyType));
                index++;
            }
            return dic;
        }

        public static string[] GetKeys(Type type, string keyPrefix)
        {
            TypeAccessor ta = TypeAccessor.GetAccessor(type);
            return ta.ReadWriteProperties.Select(o => keyPrefix + o.Name).ToArray();
        }

        public static object ToObject(Type type, string[] stringValues)
        {
            if (stringValues == null)
                throw new ArgumentException("stringValues");

            TypeAccessor ta = TypeAccessor.GetAccessor(type);
            if (stringValues.Length != ta.ReadWriteProperties.Count)
                throw new ArgumentException(String.Format("给定数据的个数 {0} 与类型 {1} 的读写属性个数 {2} 不一致", stringValues.Length, type.FullName, ta.ReadWriteProperties.Count));

            object[] values = new object[stringValues.Length];
            int index = 0;
            foreach (var property in ta.ReadWriteProperties)
            {
                values[index] = FromString(stringValues[index], property.PropertyType);
                index++;
            }

            object instance = ta.Create();
            ta.SetReadWritePropertyValues(instance, values);

            return instance;
        }

        public static object ToObject(Type type, Dictionary<string, string> dic, string keyPrefix = "")
        {
            if (dic == null)
                throw new ArgumentException("dic");

            if (dic.Count == 0)
                return null;

            TypeAccessor ta = TypeAccessor.GetAccessor(type);

            object[] values = new object[ta.ReadWriteProperties.Count];
            int index = 0;
            foreach (var property in ta.ReadWriteProperties)
            {
                string str;
                if (dic.TryGetValue(keyPrefix + property.Name, out str))
                {
                    values[index] = FromString(str, property.PropertyType);
                }
                index++;
            }

            object instance = ta.Create();
            ta.SetReadWritePropertyValues(instance, values);

            return instance;
        }

        static string ToString(object value, Type type)
        {
            var str = TypeSerializer.SerializeToString(value, type);
            if (str == null)
                str = NullValue;

            return str;
        }

        static object FromString(string value, Type type)
        {
            if (value == NullValue)
                return null;

            var v = TypeSerializer.DeserializeFromString(value, type);
            if (type == typeof(DateTime))
            {
                var dv = (DateTime)v;
                v = dv.ToLocalTime();
            }
            return v;
        }
    }
}
