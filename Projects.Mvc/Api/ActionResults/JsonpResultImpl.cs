using Projects.Tool;
using Projects.Tool.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Projects.Framework.Web
{
    internal class JsonpResultImpl
    {
        public static void Write(HttpResponseBase response, string callback, object data)
        {
            var content = JsonConverter.ToJson(data);
            response.ContentType = "application/x-javascript;charset=utf-8";
            response.Write(String.Format("{0}({1})", callback, content));
        }

        public static bool IsJsonpRequest(HttpRequestBase request, out string callback)
        {
            Arguments.NotNull(request, "request");

            callback = request.QueryString["callback"];
            if (String.IsNullOrEmpty(callback))
                callback = request.QueryString["jsoncallback"];
            if (String.IsNullOrEmpty(callback))
                callback = request.QueryString["jsonpcallback"];

            return !String.IsNullOrEmpty(callback);
        }
    }
}
