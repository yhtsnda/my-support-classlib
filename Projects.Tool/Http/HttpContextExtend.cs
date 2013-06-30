using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Tool.Reflection;

namespace System.Web
{
    public static class HttpContextExtend
    {
        static Func<object, object> handler;

        static HttpContextExtend()
        {
            TypeAccessor ta = TypeAccessor.GetAccessor(typeof(HttpContext));
            handler = ta.GetFieldGetter("HideRequestResponse");
        }

        /// <summary>
        /// 获取一个bool值,表示当前上下文是否可用
        /// </summary>
        /// <param name="context">Http上下文</param>
        /// <returns>当前上下文是否可用</returns>
        public static bool IsAvailable(this HttpContext context)
        {
            return context != null && ((bool)handler(context));
        }
    }
}
