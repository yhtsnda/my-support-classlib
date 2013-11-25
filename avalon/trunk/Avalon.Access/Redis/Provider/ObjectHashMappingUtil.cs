using Avalon.Utility;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Avalon.RedisProvider
{
    internal static class ObjectHashMappingUtil
    {
        const string NullValue = "$<NULL>$";
        const string ValueName = "$<VALUE>$";

        public static Dictionary<string, string> ToHash(object instance, string keyPrefix = "")
        {
            if (instance == null)
                throw new ArgumentNullException("entity");

            Type type = instance.GetType();
            Dictionary<string, string> dic = new Dictionary<string, string>();

            if (instance is EmptyData)
            {
                dic.Add(keyPrefix + ValueName, NullValue);
            }
            else
            {
                TypeAccessor ta = TypeAccessor.GetAccessor(instance.GetType());
                if (ta.ReadWriteProperties.Count == 0)
                {
                    dic.Add(keyPrefix + ValueName, ToString(instance, type));
                }
                else
                {
                    var values = ta.GetReadWritePropertyValues(instance);
                    int index = 0;
                    foreach (var property in ta.ReadWriteProperties)
                    {
                        dic.Add(keyPrefix + property.Name, ToString(values[index], property.PropertyType));
                        index++;
                    }
                }
            }
            return dic;
        }

        public static string[] GetKeys(Type type, string keyPrefix)
        {
            TypeAccessor ta = TypeAccessor.GetAccessor(type);
            List<string> keys = new List<string>();
            keys.Add(keyPrefix + ValueName);
            keys.AddRange(ta.ReadWriteProperties.Select(o => keyPrefix + o.Name));
            return keys.ToArray();
        }

        /// <summary>
        /// 将 HASH 数据转为对象，注意第一个指向的为 ValueName 的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="stringValues"></param>
        /// <returns></returns>
        public static object ToObject(Type type, string[] stringValues)
        {
            if (stringValues == null || stringValues.Length == 0)
                throw new ArgumentException("stringValues");

            if (stringValues.All(o => String.IsNullOrEmpty(o)))
                return null;

            if (stringValues[0] == NullValue)
                return EmptyData.Value;

            if (stringValues.Length == 1)
                return FromString(stringValues[0], type);

            TypeAccessor ta = TypeAccessor.GetAccessor(type);

            object[] values = new object[stringValues.Length - 1];
            int index = 0;
            foreach (var property in ta.ReadWriteProperties)
            {
                values[index] = FromString(stringValues[index + 1], property.PropertyType);
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
            if (v != null && type == typeof(DateTime))
            {
                var dv = (DateTime)v;
                v = dv.ToLocalTime();
            }
            return v;
        }
    }
}
