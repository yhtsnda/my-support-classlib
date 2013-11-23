using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Avalon.Utility;

namespace Avalon.Profiler
{
    public static class ProfilerService
    {
        static ProfilerCommandService commandService;
        static ProfilerServiceData serviceData;
        static JavaScriptSerializer serializer;
        static bool settingInited = false;

        const string ProfilerRequestKey = "__profiler__";
        const string ProfilerBegin = "/* --tracebeging--";
        const string ProfilerEnd = "--traceend-- */";

        static ProfilerService()
        {
            commandService = new ProfilerCommandService();
            commandService.InitCommands();
            commandService.InitRequests();

            serviceData = new ProfilerServiceData();
            serializer = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        }

        internal static void Init()
        {
            var setting = SettingProvider.Current.Load<ProfilerSetting>(serviceData.Setting.Id);
            if (setting != null)
                serviceData.Setting = setting;

            var node = ToolSection.Instance.TryGetNode("profiler");
            if (node != null)
            {
                var type = node.TryGetValue("serializer");
                IProfilerSerializer profilerSerializer = (IProfilerSerializer)FastActivator.Create(type);
                profilerSerializer.Init(node);
                ProfilerSerializer = profilerSerializer;
            }
            else
            {
                ProfilerSerializer = new DefaultProfilerSerializer();
            }
        }

        public static IProfilerSerializer ProfilerSerializer { get; set; }

        public static CommandService CommandService { get { return commandService; } }

        internal static ProfilerServiceData ServiceData
        {
            get { return ProfilerService.serviceData; }
        }

        internal static bool UserProfileEnabled
        {
            get
            {
                return HttpContext.Current.Request.Cookies[ProfilerRequestKey].GetOrDefault(o => o.Value) == "True";
            }
            set
            {
                HttpContext.Current.Response.Cookies.Add(new HttpCookie(ProfilerRequestKey, value.ToString()) { Expires = DateTime.Now.AddDays(1) });
            }
        }

        public static void RequestBegin(HttpApplication app)
        {
            if (!commandService.Process(app))
            {
                var request = app.Request;
                var path = request.Url.AbsolutePath.ToLower();
                if (path == "/favicon.ico" || path == "/synccache.ashx" || path == "/pagemonitor")
                    return;

                //url filter
                if (!String.IsNullOrEmpty(serviceData.Setting.UrlFilter) && !request.Url.AbsolutePath.ToLower().Contains(serviceData.Setting.UrlFilter.ToLower()))
                    return;

                //ip filter
                if (!String.IsNullOrEmpty(serviceData.Setting.IPFilter) && !IpAddress.GetIP().Contains(serviceData.Setting.IPFilter))
                    return;

                //no url filter
                if (!String.IsNullOrEmpty(serviceData.Setting.NoUrlFilter))
                {
                    var noUrls = serviceData.Setting.NoUrlFilter.ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (noUrls.Any(o => path.Contains(o)))
                        return;
                }

                ProfilerContext.Current.Begin();
            }
        }

        public static void RequestEnd(HttpApplication app)
        {
            var context = ProfilerContext.Current;
            context.End(app);
            if (context.Enabled)
            {
                switch (serviceData.Setting.Mode)
                {
                    case ProfilerMode.Request:
                        ProcessModeForRequest(app, context);
                        break;
                    case ProfilerMode.Static:
                        ProcessModeForStatic(app, context);
                        break;
                }

                if (serviceData.Setting.IsSlow(context.Data))
                    ProfilerSerializer.Save(context.Data);

                PushRequest(context);
            }
        }

        static void PushRequest(ProfilerContext context)
        {
            if (!serviceData.Setting.RequestEnabled)
                return;

            RequestData rd = new RequestData()
            {
                Time = NetworkTime.Now,
                Url = context.Data.Request.Url,
                HttpMethod = context.Data.Request.Method,
                Header = context.Data.Request.Header,
                Body = context.Data.Request.Body
            };

            serviceData.RequestDatas.Enqueue(rd);
            while (serviceData.RequestDatas.Count > serviceData.Setting.RequestCount)
            {
                RequestData ord;
                serviceData.RequestDatas.TryDequeue(out ord);
            }
        }

        static void ProcessModeForStatic(HttpApplication app, ProfilerContext context)
        {
            serviceData.BufferDatas.Enqueue(context.Data);
            while (serviceData.BufferDatas.Count > serviceData.Setting.StaticCount)
            {
                ProfilerData pd;
                serviceData.BufferDatas.TryDequeue(out pd);
            }
        }

        static void ProcessModeForRequest(HttpApplication app, ProfilerContext context)
        {
            if (app.Response.StatusCode == (int)HttpStatusCode.OK)
            {
                string json = serializer.Serialize(context.Data);

                if (app.Response.ContentType.Contains("application/json"))
                {
                    app.Response.Write("\r" + ProfilerBegin + json + ProfilerEnd);
                }
                else if (app.Response.ContentType.Contains("text/html"))
                {
                    app.Response.Write("<script type=\"text/javascript\">if(typeof($profiler) != 'undefined'){$profiler.add(" + json + ");}</script>");
                }
            }
        }
    }
}
