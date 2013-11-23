using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;

namespace Avalon.Utility
{
    public class JsonConverter
    {
        static IJsonConverterProvider provider;

        static JsonConverter()
        {
            try
            {
                var type = ToolSection.Instance.TryGetValue("util/jsonProvider");
                if (!String.IsNullOrEmpty(type))
                {
                    provider = ToolSection.Instance.TryGetInstance<IJsonConverterProvider>("util/jsonProvider");
                    if (provider == null)
                        throw new ConfigurationException(String.Format("无法加载类型 {0}.", type));
                }
            }
            catch (Exception)
            {
                provider = new DefaultJsonConverterProvider();
            }
        }

        public static string ToJson(object entity)
        {
            //DetectLoop(null, entity, new HashSet<object>());
            return provider.ToJson(entity);
        }

        static void DetectLoop(string identity, object instance, HashSet<object> hash)
        {
            if (instance != null)
            {
                var type = instance.GetType();
                if (type.IsClass && type != typeof(string))
                {
                    if (!hash.Add(instance))
                        throw new ArgumentException(String.Format("对象存在循环引用，无法进行序列化。发生在类型 {0}", identity));

                    var ta = TypeAccessor.GetAccessor(type);
                    if (instance is IEnumerable)
                    {
                        foreach (var v in (IEnumerable)instance)
                        {
                            DetectLoop(type.FullName + "[]", v, hash);
                        }
                    }
                    else
                    {
                        var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                        foreach (var property in properties)
                        {
                            if (property.GetIndexParameters().Length == 0)
                            {
                                var getter = ta.GetPropertyGetter(property.Name);
                                var v = getter(instance);
                                DetectLoop(type.FullName + "." + property.Name, v, hash);
                            }
                        }
                    }
                }
            }
        }

        public static T FromJson<T>(string json)
        {
            return provider.FromJson<T>(json);
        }

        public static object FromJson(string json, Type type)
        {
            return provider.FromJson(json, type);
        }

        public interface IJsonConverterProvider
        {
            string ToJson(object entity);

            T FromJson<T>(string json);

            object FromJson(string json, Type type);
        }

        private class DefaultJsonConverterProvider : IJsonConverterProvider
        {
            private static JavaScriptSerializer _serializer = new JavaScriptSerializer();

            public string ToJson(object entity)
            {
                return _serializer.Serialize(entity);
                //var serializer = new DataContractJsonSerializer(entity.GetType());
                //using (var ms = new MemoryStream())
                //{
                //    serializer.WriteObject(ms, entity);
                //    return Encoding.UTF8.GetString(ms.ToArray());
                //}
            }

            public T FromJson<T>(string json)
            {
                if (String.IsNullOrEmpty(json))
                    return default(T);

                return _serializer.Deserialize<T>(json);
                //return (T)FromJson(json, typeof(T));
            }

            public object FromJson(string json, Type type)
            {
                if (String.IsNullOrEmpty(json))
                    return FastActivator.Create(type);

                return _serializer.Deserialize(json, type);
                //using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                //{
                //    var serializer = new DataContractJsonSerializer(type);
                //    return serializer.ReadObject(ms);
                //}
            }
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class JsonIgnoreAttribute : Attribute
    {
    }

    /// <summary>
    /// 表示当前的属性或字段需要进行 JSON 序列化。
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class JsonPropertyAttribute : Attribute
    {
        string propertyName;

        public JsonPropertyAttribute()
        {
        }

        public JsonPropertyAttribute(string propertyName)
        {
            this.propertyName = propertyName;
        }

        public string PropertyName
        {
            get { return propertyName; }
        }
    }
}
