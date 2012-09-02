using System;

using Projects.Tool.Util;

namespace Projects.Tool
{
    public class JsonConverterProvider : JsonConverter.IJsonConverterProvider
    {
        static Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings()
        {
            ContractResolver = new PublicContractResolver()
        };

        public string ToJson(object entity)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(entity, Newtonsoft.Json.Formatting.None, settings);
        }

        public T FromJson<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public object FromJson(string json, Type type)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(json, type);
        }
    }
}
