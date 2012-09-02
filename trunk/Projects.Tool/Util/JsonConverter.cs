using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool.Util
{
    public class JsonConverter
    {
        static IJsonConverterProvider provider;

        static JsonConverter()
        {
            provider = ToolSection.Instance.TryGetInstance<IJsonConverterProvider>("util/jsonProvider");
            if (provider == null)
                throw new ConfigurationException("缺少 util/jsonProvider 配置。");
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
