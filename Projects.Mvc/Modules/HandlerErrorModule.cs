using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Projects.Framework.Web
{
    /// <summary>
    /// 全局的异常捕捉类
    /// </summary>
    public abstract class HandlerErrorModule : BaseHttpModules
    {
        /// <summary>
        /// 验证异常是否需要被传递
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected abstract bool IsNeedHandle(Exception ex);

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        protected abstract void LogError(HttpContextBase context, Exception ex);

        /// <summary>
        /// 文件未找到的错误
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        protected abstract void NotFound(HttpContextBase context, HttpException ex);

        /// <summary>
        /// 系统异常
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        protected abstract void Error(HttpContextBase context, Exception ex);

        /// <summary>
        /// 发生错误时的处理
        /// </summary>
        /// <param name="context"></param>
        public override void OnError(HttpContextBase context)
        {
            var exception = context.Server.GetLastError();
            if (exception == null) return;
            if (!IsNeedHandle(exception)) return;

            var isHttpExecption = exception is HttpException;
            var statusCode = context.Response.StatusCode;

            //如果允许自定义的错误
            if (context.IsCustomErrorEnabled)
            {
                context.Server.ClearError();
                context.Response.Clear();
                if (isHttpExecption)
                {
                    var httpException = exception as HttpException;
                    statusCode = httpException.GetHttpCode();
                    //如果是页面没有找到的错误
                    if (statusCode == 404)
                    {
                        NotFound(context, httpException);
                        return;
                    }
                }
            }
            //如果是页面没有找到的错误
            if (statusCode != 404)
                LogError(context, exception);
            if (context.IsCustomErrorEnabled)
                Error(context, exception);
        }
    }
}
