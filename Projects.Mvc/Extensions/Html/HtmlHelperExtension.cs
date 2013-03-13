using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

using Projects.Framework.Resource;

namespace System.Web.Mvc
{
    public static class HtmlHelperExtension
    {
        private static JavaScriptSerializer _serializer = new JavaScriptSerializer();

        /// <summary>
        /// 静态资源的引用
        /// </summary>
        public static IHtmlString Include(this HtmlHelper htmlHelper, Action<IResourceCollector> collector)
        {
            return htmlHelper.Raw(Projects.Framework.Resource.Resource.Include(collector));
        }

        /// <summary>
        /// 包装静态资源文件路径
        /// </summary>
        /// <remarks>
        /// 如传入：gwytraining/logo.gif
        /// 返回：{static_site_url}/gwytraining/logo.gif?version={version}
        /// </remarks>
        public static string WrapperUrl(this HtmlHelper htmlHelper, string staticFileUrl)
        {
            return Projects.Framework.Resource.Resource.WrapperUrl(staticFileUrl);
        }

        /// <summary>
        /// 将对象转化成JSON输出
        /// </summary>
        public static IHtmlString ToJson(this HtmlHelper htmlHelper, object value)
        {
            return htmlHelper.Raw(_serializer.Serialize(value));
        }

        /// <summary>
        /// 编码字符串
        /// </summary>
        public static string EncryptString(this HtmlHelper htmlHelper, string inputString)
        {
            byte[] bInput = Encoding.Default.GetBytes(inputString);
            return Convert.ToBase64String(bInput);
        }

        /// <summary>
        /// 解码字符串
        /// </summary>
        public static string DecryptString(this HtmlHelper htmlHelper, string inputString)
        {
            byte[] bInput = Convert.FromBase64String(inputString);
            return Encoding.Default.GetString(bInput);
        }
    }
}
