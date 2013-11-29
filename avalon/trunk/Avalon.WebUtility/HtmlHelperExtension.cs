using Avalon.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace System.Web.Mvc
{
    /// <summary>
    /// HtmlHelper扩展
    /// </summary>
    public static class HtmlHelperExtension
    {
        private static JavaScriptSerializer _serializer = new JavaScriptSerializer();

        /// <summary>
        /// 静态资源引用
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="collector"></param>
        /// <returns></returns>
        public static IHtmlString Include(this HtmlHelper htmlHelper, Action<IResourceCollector> collector)
        {
            return htmlHelper.Raw(Avalon.Resource.Resource.Include(collector));
        }

        /// <summary>
        /// 包装静态资源文件地址
        /// 如传入：gwytraining/logo.gif
        /// 返回：{static_site_url}/gwytraining/logo.gif?version={version}
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="staticFileUrl">静态资源地址</param>
        /// <returns></returns>
        public static string WrapperUrl(this HtmlHelper htmlHelper, string staticFileUrl)
        {
            return Avalon.Resource.Resource.WrapperUrl(staticFileUrl);
        }

        /// <summary>
        /// 将对象转化成JSON输出
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IHtmlString ToJson(this HtmlHelper htmlHelper, object value)
        {
            return htmlHelper.Raw(_serializer.Serialize(value));
            //return htmlHelper.Raw(JsonConverter.ToJson(value));
        }

        /// <summary>
        /// 编码字符串
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string EncryptString(this HtmlHelper htmlHelper, string inputString)
        {
            byte[] bInput = Encoding.Default.GetBytes(inputString);
            return Convert.ToBase64String(bInput);
        }

        //解码字符串
        public static string DecryptString(this HtmlHelper htmlHelper, string inputString)
        {
            byte[] bInput = Convert.FromBase64String(inputString);
            return Encoding.Default.GetString(bInput);
        }
    }
}
