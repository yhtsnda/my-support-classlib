using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using Projects.Tool.Reflection;

namespace Projects.Tool.Http
{
    /// <summary>
    /// 标准URI的构建类
    /// </summary>
    public class UriPathBuilder
    {
        public static readonly char PathSeparatorChar = '/';

        private StringBuilder InnerBuilder =  new StringBuilder();
        private string BaseUrl = String.Empty;

        protected UriPathBuilder()
        {
            
        }

        /// <summary>
        /// 标准URI的构建类
        /// </summary>
        /// <param name="baseUrl"></param>
        public UriPathBuilder(string baseUrl)
        {
            Arguments.NotNull(baseUrl, "baseUrl");
            this.BaseUrl = baseUrl;
            if (BaseUrl.IndexOf("?") == -1)
                BaseUrl += "?";
        }

        /// <summary>
        /// 添加一个参数值对
        /// </summary>
        /// <param name="key">建值</param>
        /// <param name="value">数据</param>
        /// <returns></returns>
        public UriPathBuilder Append(string key, string value)
        {
            Arguments.NotNull(key, "key");
            InnerBuilder.AppendFormat("{0}={1}&", key, HttpUtility.UrlEncode(value));
            return this;
        }

        /// <summary>
        /// 添加一个参数值对
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">数据</param>
        /// <returns></returns>
        public UriPathBuilder Append(string key, object value)
        {
            string v = (String)Convert.ChangeType(value, typeof(String));
            return Append(key, v);
        }

        /// <summary>
        /// 添加多个参数值,使用同样的Key
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="items">对象数据</param>
        /// <returns></returns>
        public UriPathBuilder AppendMany(string key, IEnumerable items)
        {
            Arguments.NotNull(key, "key");

            foreach (var item in items)
            {
                string value = (string)Convert.ChangeType(item, typeof(string));
                InnerBuilder.AppendFormat("{0}={1}&", key, HttpUtility.UrlEncode(value));
            }
            return this;
        }

        /// <summary>
        /// 添加多个参数
        /// </summary>
        /// <param name="data">参数数据</param>
        /// <returns></returns>
        public UriPathBuilder AppendMany(NameValueCollection data)
        {
            if (data != null && data.Count > 0)
            {
                List<string> items = new List<string>();
                foreach (string name in data)
                {
                    var values = data.GetValues(name);
                    if (values != null)
                        items.AddRange(values.Select(o => String.Concat(name, "=", HttpUtility.UrlEncode(o))));
                }
                InnerBuilder.Append(String.Join("&", items));
            }
            return this;
        }

        /// <summary>
        /// 获取组装的URL
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result = InnerBuilder.ToString();
            //如果没有参数
            if (String.IsNullOrEmpty(result))
                return BaseUrl.TrimEnd('?');
            return String.Format("{0}{1}", BaseUrl, result.TrimEnd('&'));
        }

        /// <summary>
        /// 将实体对象中的属性转换为NameValue的集合
        /// </summary>
        /// <param name="values">对象类</param>
        /// <returns>转换后的NameValue集合</returns>
        public static NameValueCollection ToNameValueCollection(object values)
        {
            NameValueCollection data = new NameValueCollection();
            if (values != null)
            {
                var va = TypeAccessor.GetAccessor(values.GetType());
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(values))
                {
                    var value = va.GetPropertyGetter(descriptor.Name)(values);
                    string tmp = String.Empty;
                    if (descriptor.PropertyType == typeof(Guid))
                        tmp = value.ToString();
                    else
                        tmp = (String)Convert.ChangeType(value, typeof(String));

                    data.Add(descriptor.Name, tmp);
                }
            }
            return data;
        }

        /// <summary>
        /// 合并路径
        /// </summary>
        /// <param name="paths">路径参数</param>
        /// <returns>合并结果</returns>
        public static string Combine(params string[] paths)
        {
            Arguments.NotNull(paths, "paths");

            List<string> items = new List<string>();
            for (var i = 0; i < paths.Length; i++)
            {
                var path = paths[i];
                Arguments.NotNull(path, "paths");

                path = path.Trim().Trim(new char[] { PathSeparatorChar });
                if (path.Length > 0)
                    items.Add(path);
            }
            return String.Join(PathSeparatorChar.ToString(), items);
        }
    }
}
