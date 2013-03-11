using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Projects.Tool;

namespace Projects.Framework.Web
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class OpenApiAttribute : AuthorizeAttribute, IExceptionFilter
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool result = false;
            //从HttpRequest的Header中获取用户名,密码和AppKey
            string userName = httpContext.Request.Headers["User"];
            string password = httpContext.Request.Headers["Password"];
            string appKey = httpContext.Request.Headers["AppKey"];
            string checkSum = httpContext.Request.Headers["CheckSum"];
            if (userName == null || password == null || appKey == null || checkSum == null)
                result = false;

            //获取IP地址
            string requestIp = Projects.Tool.Util.IpAddress.GetIP();
            //如果返回为String.Empty,表示不是网络环境,不传入IP进行验证
            if (requestIp == String.Empty)
            {
                result = ServiceUserAuth.Instance.Check(userName, password, appKey, checkSum);
            }
            //否则就是网络环境
            else
            {
                result = ServiceUserAuth.Instance.Check(userName, password, appKey, checkSum, requestIp);
            }
            //判断检查结果
            if (!result)
                httpContext.Response.StatusCode = 403;
            return result;
        }

        protected int GetResponseCode(ExceptionContext filterContext)
        {
            var code = ResultCode.InternalServerError;
            if (filterContext.Exception is ArgumentException)
                code = ResultCode.BadRequest;

            var ex = filterContext.Exception as ProjectBaseException; ;
            if (ex != null)
            {
                code = ex.Code;
                LogManager.GetLogger("building_log").Error("系统出现:" + ex.Code.ToString() + "错误~", ex);
            }

            return code;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.HttpContext.Response.StatusCode == 403)
            {
                filterContext.Result = new OpenApiDataResult(new Exception("未通过的验证信息"), 403);
            }
        }

        public void OnException(ExceptionContext filterContext)
        {
            filterContext.Result = new OpenApiDataResult(filterContext.Exception, GetResponseCode(filterContext));
            filterContext.ExceptionHandled = true;
        }
    }
}
