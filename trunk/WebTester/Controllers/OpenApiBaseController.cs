using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebTester.Extensions;

using Projects.Tool.Util;
using Projects.Framework;
using Projects.Framework.Web;
using Projects.Framework.OAuth2;

namespace WebTester.Controllers
{
    [Compress]
    public class OpenApiBaseController : Controller
    {
        private int defaultCode = (int)ResultCode.ServerError;

        protected ActionResult Invoke<T>(Func<T> func)
        {
            if (func == null)
                throw new PlatformException("func不能为空");

            ResultWrapper<T> result = null;

            string validateMessage = GetValidateMessage();
            if (!string.IsNullOrEmpty(validateMessage))
            {
                result = new ResultWrapper<T>(defaultCode, validateMessage);
                return new InvokeActionResult<T>(result);
            }

            try
            {
                result = new ResultWrapper<T>(func());
            }
            catch (Exception ex)
            {
                result = new ResultWrapper<T>(defaultCode, ex.Message);
                if (ex is ApiException)
                {
                    result.Code = (int)((ApiException)ex).ResultCode;
                }
                //_log.Error(ex.ToString(), ex);
            }
            //TODO
            //return XmlResult(result);
            return new InvokeActionResult<T>(result);
        }

        private string GetValidateMessage()
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    if (modelState.Errors.Count > 0)
                    {
                        return modelState.Errors.First().ErrorMessage;
                    }
                }
            }
            return string.Empty;
        }
    }

    /// <summary>
    /// 由于mvc自带的序列化工具会自动对Html内容进行编码,故改用自定义的序列化工具。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InvokeActionResult<T> : ActionResult
    {

        public InvokeActionResult(ResultWrapper<T> resultWrapper)
        {
            this.resultWrapper = resultWrapper;
        }

        private ResultWrapper<T> resultWrapper;

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var callback = context.HttpContext.Request["callback"];
            var content = JsonConverter.ToJson(resultWrapper);
            if (!string.IsNullOrEmpty(callback))
                content = callback + "(" + JsonConverter.ToJson(resultWrapper) + ")";

            context.HttpContext.Response.ContentType = "application/json";
            HttpResponseBase response = context.HttpContext.Response;
            response.Write(content);
        }
    }
}
