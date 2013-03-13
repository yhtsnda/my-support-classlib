using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using Projects.Tool.Util;

namespace Projects.Tool
{
    public class PublicContractResolver : DefaultContractResolver
    {
        public PublicContractResolver()
        {
            DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
        }

        protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, Newtonsoft.Json.MemberSerialization memberSerialization)
        {
            JsonProperty jp = base.CreateProperty(member, memberSerialization);

            if (GetAttribute<JsonIgnoreAttribute>(member) != null)
            {
                jp.Ignored = true;
            }
            else
            {
                JsonPropertyAttribute jsonProp = GetAttribute<JsonPropertyAttribute>(member);
                if (jsonProp != null)
                {
                    if (!String.IsNullOrEmpty(jsonProp.PropertyName))
                        jp.PropertyName = jsonProp.PropertyName;
                }
                else
                {
                    if (member.MemberType == MemberTypes.Property)
                    {
                        PropertyInfo pi = (PropertyInfo)member;
                        //仅可写及公开
                        jp.Ignored = pi.GetGetMethod() == null || pi.GetSetMethod() == null;
                    }
                    else if (member.MemberType == MemberTypes.Field)
                    {
                        FieldInfo fi = (FieldInfo)member;
                        //如果是匿名类型，就不忽略 add by skypan 2012年4月10日12:19:13
                        if (fi.DeclaringType.Name.IndexOf("<>f__AnonymousType") > -1)
                        {
                            jp.Ignored = false;
                            jp.PropertyName = fi.Name.Substring(1, fi.Name.IndexOf(">") - 1);
                        }
                        else
                        {
                            jp.Ignored = !fi.IsPublic;
                        }

                    }
                }
            }
            return jp;
        }

        private static T GetAttribute<T>(MemberInfo memberInfo) where T : Attribute
        {
            return (T)Attribute.GetCustomAttribute(memberInfo, typeof(T));
        }
    }
}
