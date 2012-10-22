using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Serialization;

namespace Projects.Tool.WeblogProvider
{
    public class LogUtil
    {
        /// <summary>
        /// 序列化器
        /// </summary>
        private static readonly JavaScriptSerializer serializer = new JavaScriptSerializer();

        /// <summary>
        /// 获取当前HTTP的堆栈信息
        /// </summary>
        /// <returns>在非HTTP环境下，返回null</returns>
        public static string GetStackTrace()
        {
            if (!HostingEnvironment.IsHosted)
                return Environment.StackTrace;

            var context = HttpContext.Current;
            var stackTrace = new StackTrace();
            stackTrace.AbsolutePath = context.Request.Url.AbsolutePath;
            stackTrace.UrlReferrer = context.Request.UrlReferrer == null
                ? string.Empty
                : context.Request.UrlReferrer.ToString();
            stackTrace.Query = context.Request.Url.Query;
            stackTrace.Form = ConvertNameValueCollection(context.Request.Form);

            var user = new StackTrace.UserIdentity()
            {
                IsAuthenticated = context.User.Identity.IsAuthenticated,
                Name = context.User.Identity.Name
            };
            stackTrace.User = user;
            return serializer.Serialize(stackTrace);
        }

        /// <summary>
        /// 根据键值对集合获取指定格式字符串
        /// </summary>
        /// <param name="value">键值对集合</param>
        /// <returns>格式如:key1=value1&key2=value2的字符串</returns>
        private static string ConvertNameValueCollection(NameValueCollection nvc)
        {
            var list = new List<string>();
            foreach (var key in nvc.AllKeys)
            {
                list.Add(string.Concat(key, "=", nvc[key]));
            }
            return string.Join("&", list);
        }

        /// <summary>
        /// 发送HTTP请求数据
        /// </summary>
        /// <param name="url">发送地址(url)</param>
        /// <param name="value">要发送的数据</param>
        /// <returns>响应字符串</returns>
        public static string GetPostResponse(string url, string value)
        {
            var data = Encoding.UTF8.GetBytes(value);
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8";
            request.ContentLength = data.Length;
            request.Timeout = 20000;
            var response = (HttpWebResponse)null;
            try
            {
                var stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                response = (HttpWebResponse)request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
        }
    } 
}
