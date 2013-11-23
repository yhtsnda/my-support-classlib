using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Avalon.Utility;

namespace Avalon.Profiler
{
    internal class ProfilerCommandService : CommandService
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };

        ProfilerSetting Setting
        {
            get { return ProfilerService.ServiceData.Setting; }
        }

        ProfilerServiceData ServiceData
        {
            get { return ProfilerService.ServiceData; }
        }

        public override string BathPath
        {
            get { return "profiler"; }
        }

        public override string CommandHelpContent
        {
            get
            {
                return @"
/profiler/static    显示服务端记录的 profiler 信息
/profiler/request   显示服务端记录的请求信息
/profiler/slow      显示服务器记录的慢请求信息

[--help | --options | [
    --mode [request|static] | 
    --enabled | 
    --disabled | 
    --detail [true|false] | 
    --staticcount [num] |
    --clearstatic |
    --url [url] |
    --nourl [nourl] |
    --ip [ip] |
    --request [true|false] |
    --requestcount [num] |
    --clearrequest
    --slow [true|false]
    --slow-msec [num]
    --slow-db [num]
    --slow-cache  [num]
] ]

选项：
--help          显示命令的帮助信息
--options       显示当前 profiler 的选项设置
--mode          模式，默认为request。request: profile信息附加在响应内容里面，static: profile信息在服务端存储。  
--enabled       启用（默认）
--disabled      禁用
--detail        显示详细的堆栈，默认false
--staticcount   记录profile的最大数量，默认为20
--clearstatic   清除服务端存储profile的数据
--url           URL过滤器，仅包含指定的url的地址才会产生profile信息
--nourl         URL禁止过滤器，包含的url的地址会被过滤掉
--ip            IP过滤器，仅包含指定ip的请求才会产生profile信息
--request       记录请求信息
--requestcount  记录请求信息的最大数量，默认为20
--clearrequest  清除当前记录的请求信息
--slow          是否记录慢处理的请求
--slow-msec     记录为慢处理的最小处理毫秒数
--slow-db       记录为慢处理的最少数据库请求数
--slow-cache    记录为慢处理的最少查询请求数
";
            }
        }

        protected override void ProcessCommandOptions(HttpApplication app)
        {
            WriteContent(ProfilerService.ServiceData.ToString(), app);
        }

        public override void InitCommands()
        {
            RegisterCommand("mode", ProcessCommandMode);
            RegisterCommand("disabled", (value, app) =>
            {
                Setting.Enabled = false;
                ProfilerService.UserProfileEnabled = false;
            });
            RegisterCommand("enabled", (value, app) =>
            {
                Setting.Enabled = true;
                ProfilerService.UserProfileEnabled = true;
            });
            RegisterCommand("detail", (value, app) => Setting.ShowDetail = "true".Equals(value));
            RegisterCommand("staticcount", (value, app) => Setting.StaticCount = Int32.Parse(value));
            RegisterCommand("clearstatic", (value, app) => ServiceData.BufferDatas = new ConcurrentQueue<ProfilerData>());

            RegisterCommand("url", (value, app) => Setting.UrlFilter = value);
            RegisterCommand("nourl", (value, app) => Setting.NoUrlFilter = value);
            RegisterCommand("ip", (value, app) => Setting.IPFilter = value);

            RegisterCommand("request", (value, app) => Setting.RequestEnabled = "true".Equals(value));
            RegisterCommand("requestcount", (value, app) => Setting.RequestCount = Int32.Parse(value));
            RegisterCommand("clearrequest", (value, app) => ServiceData.RequestDatas = new ConcurrentQueue<RequestData>());

            RegisterCommand("slow", (value, app) => Setting.SlowEnabled = "true".Equals(value));
            RegisterCommand("slow-msec", (value, app) => Setting.SlowMilliSecond = Int32.Parse(value));
            RegisterCommand("slow-db", (value, app) => Setting.SlowDbCount = Int32.Parse(value));
            RegisterCommand("slow-cache", (value, app) => Setting.SlowCacheCount = Int32.Parse(value));
        }

        public override void InitRequests()
        {
            RegisterRequest("/static", ProcessStaticRequest);
            RegisterRequest("/request", ProcessRequestRequest);
            RegisterRequest("/slow", ProcessSlowRequest);
            RegisterRequest("/mysql", ProcessMysqlRequest);
        }

        protected override void OnProcessCommand(HttpApplication app, IList<string> cmds)
        {
            if (cmds.Count > 0)
            {
                SettingProvider.Current.Save(Setting);
            }
        }

        void ProcessCommandMode(string value, HttpApplication app)
        {
            Setting.Mode = "static".Equals(value) ? ProfilerMode.Static : ProfilerMode.Request;
            if (Setting.Mode == ProfilerMode.Request)
                ProfilerService.UserProfileEnabled = true;
        }

        void ProcessStaticRequest(HttpApplication app)
        {
            ResponseProfilerData(app, ServiceData.BufferDatas.ToArray().Reverse());
        }

        void ProcessRequestRequest(HttpApplication app)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in ServiceData.RequestDatas.ToArray().Reverse())
            {
                sb.AppendLine(item.Time.ToString());
                sb.AppendLine(item.HttpMethod + " " + item.Url);
                sb.AppendLine(item.Header);
                sb.AppendLine(item.Body);
                sb.AppendLine();
                sb.AppendLine();
            }
            WriteContent(sb.ToString(), app);
        }

        void ProcessSlowRequest(HttpApplication app)
        {
            var request = app.Request;
            DateTime date = DateTime.Parse(request.QueryString["date"] ?? DateTime.Now.Date.ToString());
            int index = Int32.Parse(request.QueryString["index"] ?? "0");
            int length = Int32.Parse(request.QueryString["length"] ?? "20");

            var items = ProfilerService.ProfilerSerializer.Load(date, index, length);
            ResponseProfilerData(app, items);
        }

        void ProcessMysqlRequest(HttpApplication app)
        {
            WriteContent(MySqlTraceListener.GetContent(), app);
        }

        void ResponseProfilerData(HttpApplication app, IEnumerable<ProfilerData> datas)
        {
            var response = app.Response;
            response.ContentType = "text/html; charset=UTF-8";

            response.Write(@"
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <link type=""text/css"" rel=""stylesheet"" href=""http://s2.tianyuimg.com/platform/css/mod.css"" />
    <link type=""text/css"" rel=""stylesheet"" href=""http://s2.tianyuimg.com/addins/profilter/v1/profilter.css"" />
    <script src=""http://s2.tianyuimg.com/addins/jquery/jquery-1.7.1.min.js""></script>
    <script src=""http://s2.tianyuimg.com/addins/profilter/v1/profiler.js""></script>
");

            response.Write("    <script type=\"text/javascript\">\r\n");
            response.Write("        if(typeof($profiler) != 'undefined'){\r\n");

            foreach (var item in datas)
            {
                string json = serializer.Serialize(item);
                response.Write("        $profiler.add(" + json + ");\r\n");
            }
            response.Write("    }\r\n   </script>\r\n</head>");
            response.Write("<body>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;SiteIdentity: " + SettingProvider.SiteIdentity + "</body>");
            response.Write("\r\n</html>");
        }
    }
}
