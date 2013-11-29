using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Avalon.WebUtility
{
    public static class MutilApiImpl
    {
        public static List<string> Process(string[] urls)
        {
            Arguments.NotNull(urls, "urls");
            Arguments.That(urls.Length <= 10, "urls", "urls 的个数不能超过10.");

            string host = "http://" + HttpContext.Current.Request.Url.Host;

            List<string> outputs = new List<string>();
            foreach (var url in urls)
            {
                var urlPath = UriPath.Combine(host, url);
                var qi = url.IndexOf("?");
                var urlQuery = qi == -1 ? "" : url.Substring(qi + 1);

                var request = new HttpRequest("", urlPath, urlQuery);
                request.RequestType = "GET";
                var response = new HttpResponse(new StringWriter());
                var contextWrapper = new HttpContextWrapper(new HttpContext(request, response));
                var routeData = RouteTable.Routes.GetRouteData(contextWrapper);

                var requestContext = new RequestContext(contextWrapper, routeData);
                //生成一个controller
                var controller = ControllerBuilder.Current.GetControllerFactory()
                    .CreateController(requestContext, routeData.GetRequiredString("controller"));
                controller.Execute(requestContext);
                //将执行结果添加到 outputs
                outputs.Add(response.Output.ToString());
            }
            return outputs;
        }

        public static string ProcessResult(string[] urls)
        {
            var outputs = Process(urls);
            return String.Format(@"{{""Code"": 0,""Message"": """",""Data"": [{0}]}}", String.Join(",", outputs));
        }
    }
}
