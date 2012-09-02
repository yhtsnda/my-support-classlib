using System.Reflection;

using Newtonsoft.Json.Serialization;

namespace Projects.Tool
{
    public class PublicContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, Newtonsoft.Json.MemberSerialization memberSerialization)
        {
            JsonProperty jp = base.CreateProperty(member, memberSerialization);
            PropertyInfo pinfo = (PropertyInfo)member;
            //仅可写及公开
            jp.Ignored = !pinfo.CanWrite || pinfo.GetSetMethod() == null;
            return jp;
        }
    }
}
