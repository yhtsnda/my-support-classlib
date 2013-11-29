using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.WebUtility
{
    /// <summary>
    /// 全局异常捕获类
    /// </summary>
    public abstract class HandlerErrorModule : BaseHttpModule
    {
        public override void OnError(HttpContextBase context)
        {
            var exception = context.Server.GetLastError();

            if (exception == null) return;

            if (!IsNeedHandle(exception)) return;

            var isHttpException = exception is HttpException;
            var statusCode = context.Response.StatusCode;

            if (context.IsCustomErrorEnabled)
            {
                context.Server.ClearError();
                context.Response.Clear();
                if (isHttpException)
                {
                    var httpException = exception as HttpException;
                    statusCode = httpException.GetHttpCode();
                    if (statusCode == 404)
                    {
                        NotFound(context, httpException);
                        return;
                    }
                }
            }
            if (statusCode != 404)
            {
                LogError(context, exception);
            }
            if (context.IsCustomErrorEnabled)
            {
                Error(context, exception);
            }
        }

        /// <summary>
        /// 验证该类型的异常是否需要处理
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        protected abstract bool IsNeedHandle(Exception exception);

        /// <summary>
        /// 记录异常详情
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        protected abstract void LogError(HttpContextBase context, Exception exception);

        /// <summary>
        /// 文件不存在
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        protected abstract void NotFound(HttpContextBase context, HttpException exception);

        /// <summary>
        /// 系统异常
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        protected abstract void Error(HttpContextBase context, Exception exception);
    }

}
