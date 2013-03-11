using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;

namespace Projects.Tool.Util
{
    public class JsonConverter
    {
        static IJsonConverterProvider provider;

        static JsonConverter()
        {
            try
            {
                provider = ToolSection.Instance.TryGetInstance<IJsonConverterProvider>("util/jsonProvider");
                //if (provider == null)
                //    throw new ConfigurationException("缺少 util/jsonProvider 配置。");
            }
            catch (Exception ex)
            {
                provider = new DefaultJsonConverterProvider();
            }
        }

        public static string ToJson(object entity)
        {
            return provider.ToJson(entity);
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
            //private static JavaScriptSerializer _serializer = new JavaScriptSerializer();

            public string ToJson(object entity)
            {
                var serializer = new DataContractJsonSerializer(entity.GetType());
                using (var ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, entity);
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }

            public T FromJson<T>(string json)
            {
                return (T)FromJson(json, typeof(T));
            }

            public object FromJson(string json, Type type)
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    var serializer = new DataContractJsonSerializer(type);
                    return serializer.ReadObject(ms);
                }
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
