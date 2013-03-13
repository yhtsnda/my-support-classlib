using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Projects.Framework;
using Projects.Framework.OAuth2;

namespace WebTester.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CountRestrictAttribute : ActionFilterAttribute
    {
        private static readonly string mPageSizeParameter = "pageSize";
        private static int mDefaultCode = (int)ResultCode.ServerError;
        private static int mMaxRequestCount = 50;

        public CountRestrictAttribute() { }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            //1.list列表的个数限制
            var actionParams = filterContext.ActionParameters;
            foreach (var actionParam in actionParams)
            {
                var objValue = actionParam.Value;
                if (objValue == null)
                    continue;

                var objType = objValue.GetType();
                if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    if ((objValue as IList).Count > mMaxRequestCount)
                    {
                        ThrowPlatFormException(string.Format("列表个数不能大于{0}", mMaxRequestCount));
                    }

                }

            }

            //分页大小的限制
            if (request[mPageSizeParameter] != null)
            {
                var pageSize = Convert.ToInt32(request[mPageSizeParameter]);
                if (pageSize > mMaxRequestCount)
                {
                    ThrowPlatFormException(string.Format("分页大小不能超过{0}", mMaxRequestCount));
                }
            }

            base.OnActionExecuting(filterContext);
        }

        private void ThrowPlatFormException(string message)
        {
            var exception = new PlatformException(message);
            exception.Code = mDefaultCode;
            throw exception;
        }
    }
}