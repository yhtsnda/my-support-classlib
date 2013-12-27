using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.Utility;

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

        public static bool IsAvailable(this HttpContext context)
        {
            return context != null && !((bool)handler(context));
        }

        public static bool IsLargeRequest(this HttpContext context)
        {
            if (IsAvailable(context))
            {
                var request = context.Request;
                if(request.HttpMethod == "POST")
                    return request.ContentType.StartsWith("multipart/form-data") || request.ContentType == "application/offset+octet-stream" || request.ContentLength > 2048;
            }
            return false;
        }
    }
}
