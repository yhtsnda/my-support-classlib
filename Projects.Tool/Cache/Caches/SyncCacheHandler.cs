using System;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Projects.Tool;
using System.Collections.Concurrent;

namespace Projects.Tool.Util
{
    /// <summary>
    /// 同步缓存接口
    /// </summary>
    public class SyncCacheHandler : IHttpHandler
    {
        static ConcurrentDictionary<string, int> counters = new ConcurrentDictionary<string, int>();

        /// <summary>
        /// HTTP请求
        /// </summary>
        private HttpRequest request;
        /// <summary>
        /// HTTP响应
        /// </summary>
        private HttpResponse response;

        public bool IsReusable
        {
            get { return true; }
        }

        internal static void Add(string name)
        {
            counters.AddOrUpdate(name, 1, (key, value) => value + 1);
        }

        public void ProcessRequest(HttpContext context)
        {
            request = context.Request;
            response = context.Response;

            DoAction();
        }

        private void DoAction()
        {
            if (request.QueryString["query"] == "true")
            {
                var sr = String.Join("<br />", counters.Select(o => String.Format("{0} @ {1}", o.Key, o.Value)));
                response.Write(sr);
            }
            else
            {
                string name = request.QueryString["name"];
                string key = request.QueryString["key"];

                if (string.IsNullOrEmpty(key))
                {
                    Error("缓存键名称不能为空");
                    return;
                }

                RemoveCache(name, key);
            }
        }

        private void RemoveCache(string name, string key)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    HttpRuntime.Cache.Remove(key);
                }
                else
                {
                    //ICache cache = CacheManager.GetCacher(name);
                    //cache.Remove(key);
                }
                Success();
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void Success()
        {
            response.Write("true");
        }

        private void Error(string message)
        {
            response.StatusCode = 400;
            response.Write(new JavaScriptSerializer().Serialize(new InvokeResult<bool>
            {
                Code = 400,
                Error = message
            }));
        }
    }
}
