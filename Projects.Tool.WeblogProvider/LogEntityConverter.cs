using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Projects.Tool.WeblogProvider
{
    /// <summary>
    /// 日志对象转换器
    /// </summary>
    internal class LogEntityConverter : JavaScriptConverter
    {
        /// <summary>
        /// 转换器支持类型
        /// </summary>
        public override IEnumerable<Type> SupportedTypes
        {
            get { return new List<Type> { typeof(LogEntity) }; }
        }

        /// <summary>
        /// 生成名称/值对的字典
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="serializer">负责序列化的对象</param>
        /// <returns>一个对象，包含表示该对象数据的键/值对</returns>
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var result = new Dictionary<string, object>();
            if (obj == null)
                return result;

            var entity = obj as LogEntity;
            if (entity != null)
            {
                result["AppId"] = entity.AppId;
                result["Exception"] = entity.Exception;
                result["Logger"] = entity.Logger;
                result["Message"] = entity.Message;
                result["Level"] = (int)entity.Level;
                result["StackTrace"] = entity.StackTrace;
                result["CreateTime"] = entity.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }

            return result;
        }

        /// <summary>
        /// 将所提供的字典转换为指定类型的对象
        /// </summary>
        /// <param name="dictionary">作为名称/值对存储的属性数据的 System.Collections.Generic.IDictionary实例</param>
        /// <param name="type">所生成对象的类型</param>
        /// <param name="serializer">System.Web.Script.Serialization.JavaScriptSerializer 实例</param>
        /// <returns>一个对象，包含表示该对象数据的键/值对</returns>
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
