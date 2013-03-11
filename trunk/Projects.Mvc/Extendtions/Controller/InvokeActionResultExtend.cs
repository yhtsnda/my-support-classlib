using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;

using Projects.Tool;
using Projects.Tool.Util;

namespace System.Web.Mvc
{
    /// <summary>
    /// 对 Controller 进行扩展使其能符合“API接口开发规范”的返回结果
    /// </summary>
    public static class InvokeActionResultExtend
    {
        /// <summary>
        /// 执行一个调用，并且该调用有一个返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static ActionResult Invoke<T>(this Controller controller, Func<T> func)
        {
            InvokeResult<T> result = new InvokeResult<T>();
            try
            {
                result.Data = func();
            }
            catch (ArgumentException ex)
            {
                result.Code = ResultCode.BadRequest;
                result.Error = ex.Message;
                result.Exception = ex;
            }
            catch (Exception ex)
            {
                result.Code = ResultCode.InternalServerError;
                result.Error = ex.Message;
                result.Exception = ex;
            }
            return new InvokeActionResult<T>(result);
        }

        /// <summary>
        /// 执行一个调用，该调用没有返回值
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ActionResult Invoke(this Controller controller, Action action)
        {
            InvokeResult<VoidValue> result = new InvokeResult<VoidValue>();
            try
            {
                action();
            }
            catch (ArgumentException ex)
            {
                result.Code = ResultCode.BadRequest;
                result.Error = ex.Message;
                result.Exception = ex;
            }
            catch (Exception ex)
            {
                result.Code = ResultCode.InternalServerError;
                result.Error = ex.Message;
                result.Exception = ex;
            }
            return new InvokeActionResult<VoidValue>(result);
        }
    }

    /// <summary>
    /// 封装标准 API 接口的返回结果，接口规范见
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InvokeActionResult<T> : ActionResult
    {
        private static JavaScriptSerializer serializer = new JavaScriptSerializer();

        InvokeResult<T> invokeResult;

        public InvokeActionResult(InvokeResult<T> invokeResult)
        {
            if (invokeResult == null)
                throw new ArgumentNullException("invokeResult");

            this.invokeResult = invokeResult;
        }

        public InvokeResult<T> InvokeResult
        {
            get { return invokeResult; }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            HttpResponseBase response = context.HttpContext.Response;
            //response.Clear();
            response.StatusCode = invokeResult.Code / 1000;
            response.ContentType = "application/json";
            if (!invokeResult.IsSuccess())
            {
                response.StatusDescription = invokeResult.Error;

                var data = new
                {
                    code = invokeResult.Code,
                    error = invokeResult.Error,
                    exception = invokeResult.Exception == null ? string.Empty : invokeResult.Exception.ToString()
                };
                response.Write(serializer.Serialize(data));
                response.End();
            }
            else
            {
                response.Write(serializer.Serialize(invokeResult.Data));
            }
        }

        private string Serialize<T>(T value)
        {
            string result = "";
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(value.GetType());
                serializer.WriteObject(stream, value);
                stream.Flush();
                result = Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Position);
            }
            return result;
        }
    }
}
