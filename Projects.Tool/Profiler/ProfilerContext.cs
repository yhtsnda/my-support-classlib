using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Web;
using System.Reflection;
using System.Configuration;
using System.Globalization;
using System.Security;
using System.Collections;
using System.Web.Caching;

using Projects.Tool.Profiler;
using System.Collections.Concurrent;

namespace Projects.Tool
{
    public class ProfilerContext
    {
        const string ProfilerItemKey = "__ProfilerItemKey__";
        const string ProfilerValueKey = "__profiler__";
        const string StackTraceValueKey = "__stacktrace__";

        const int StaticMaxCount = 10;
        const int MaxItemCount = 500;

        ProfilerData data;
        Stopwatch watch;
        ProfileStatus status;
        Stack<ProfilerItem> profilerStack;
        int profilerCounter = 0;
        bool isStackTrace;

        static ConcurrentQueue<ProfilerData> cache = new ConcurrentQueue<ProfilerData>();

        ProfilerContext()
        {
            data = new ProfilerData();
            var configStatus = ConfigurationManager.AppSettings["nd.profiler.status"];
            status = ToProfileStatus(configStatus, false, false);

            var context = HttpContext.Current;
            if (context != null)
            {
                bool isProfilerPage = context.Request.Url.ToString().Contains("profiler.aspx");
                bool isSingleUserRequest = false;
                isStackTrace = false;

                var ckProfiler = context.Request.Cookies[ProfilerValueKey];
                if (ckProfiler != null)
                {
                    isSingleUserRequest = ckProfiler.Value == ((int)ProfileStatus.SingleUserRequest).ToString();
                    configStatus = ckProfiler.Value;
                }

                var ckStackTrace = context.Request.Cookies[StackTraceValueKey];
                if (ckStackTrace != null)
                    isStackTrace = ckStackTrace.Value == "true";

                status = ToProfileStatus(configStatus, isProfilerPage, isSingleUserRequest);

                var rtProfiler = context.Request.QueryString[ProfilerValueKey];
                var rtStackTrace = context.Request.QueryString[StackTraceValueKey];

                if (rtProfiler != null)
                {
                    if (rtProfiler == "request")
                    {
                        isSingleUserRequest = true;
                    }

                    status = ToProfileStatus(rtProfiler, isProfilerPage, isSingleUserRequest);
                    context.Response.Cookies.Add(new HttpCookie(ProfilerValueKey, status.ToString().ToLower()) { Expires = DateTime.Now.AddDays(1) });
                }

                if (rtStackTrace != null)
                {
                    isStackTrace = rtStackTrace == "true";
                    context.Response.Cookies.Add(new HttpCookie(StackTraceValueKey, isStackTrace.ToString().ToLower()) { Expires = DateTime.Now.AddDays(1) });
                }

                data.Url = HttpContext.Current.Request.Url.PathAndQuery;
            }

            profilerStack = new Stack<ProfilerItem>();
        }

        string GetCookieValue(HttpContext context, string key)
        {
            var cookie = context.Request.Cookies[key];
            if (cookie != null)
                return cookie.Value;

            return null;
        }

        public static ProfilerContext Current
        {
            get
            {
                ProfilerContext current = (ProfilerContext)Projects.Tool.Util.Workbench.Current.Items[ProfilerItemKey];
                if (current == null)
                {
                    current = new ProfilerContext();
                    Projects.Tool.Util.Workbench.Current.Items[ProfilerItemKey] = current;
                }
                return current;
            }
        }

        public static string GetSql()
        {
            return String.Join("\r\n", Current.Data.Traces.Where(o => o.Type == "nhibernate" || o.Type == "mongo").Select(o => o.Title).ToArray());
        }

        /// <summary>
        /// 开启一个监视块
        /// </summary>
        /// <param name="message"></param>
        public static void BeginProfile(string message)
        {
            Current.InnerBeginProfile(message);
        }

        /// <summary>
        /// 结束上一个监视块
        /// </summary>
        public static void EndProfile()
        {
            Current.InnerEndProfile();
        }

        public static IDisposable Profile(string message)
        {
            return new ProfileScope(message);
        }

        public ProfileStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        public bool IsStackTrace
        {
            get { return isStackTrace; }
        }

        public bool Enabled
        {
            get
            {
                return status != ProfileStatus.Disable && status != ProfileStatus.StaticPageRequest;
            }
        }

        ProfileStatus ToProfileStatus(string name, bool isStaticPage, bool isSingleUser)
        {
            name = name.ToLower();
            if (isStaticPage && name == "static")
                return ProfileStatus.StaticPageRequest;
            if (isSingleUser && name == "request")
                return ProfileStatus.SingleUserRequest;

            switch (name)
            {
                case "request":
                    return ProfileStatus.Request;
                case "static":
                    return ProfileStatus.Static;
                case "disabled":
                    return ProfileStatus.Disable;
                case "singleuserrequest":
                    return ProfileStatus.SingleUserRequest;
                case "staticpagerequest":
                    return ProfileStatus.StaticPageRequest;
                default:
                    return ProfileStatus.Disable;
            }
        }

        public int Elapsed
        {
            get
            {
                if (watch == null)
                    return -1;
                return (int)watch.ElapsedMilliseconds;
            }
        }

        public ProfilerData Data
        {
            get { return data; }
        }

        public ConcurrentQueue<ProfilerData> StaticCache
        {
            get
            {
                return cache;
            }
        }

        public void Begin()
        {
            if (Enabled)
            {
                watch = Stopwatch.StartNew();
                data.RequestTime = DateTime.Now;
            }
        }

        public void End()
        {
            if (Enabled && watch != null)
            {
                while (profilerStack.Count > 0)
                    profilerStack.Pop().Dispose();
                data.Duration = (int)watch.ElapsedMilliseconds;
                watch.Stop();
                ProfilerData _item;
                while (cache.Count > 10)
                    cache.TryDequeue(out _item);
                cache.Enqueue(data);
            }
        }

        public void Trace(string type, string title)
        {
            if (Enabled)
                Trace(type, title, isStackTrace ? ProfilerUtil.FormatStackTrace(new StackTrace(true)) : "");
        }

        public void Trace(string type, string title, string content)
        {
            if (Enabled && (data.Traces.Count < MaxItemCount || type == "nhibernate" || type == "mongo"))
            {
                TraceItem trace = new TraceItem() { Type = type, Title = watch.ElapsedMilliseconds.ToString() + " " + HttpUtility.HtmlEncode(title), Content = HttpUtility.HtmlEncode(content) };
                data.Traces.Add(trace);
            }
        }

        internal void InnerBeginProfile(string message)
        {
            if (Enabled)
            {
                if (profilerCounter > MaxItemCount)
                    return;
                if (status == ProfileStatus.Static && profilerCounter > StaticMaxCount)
                    return;
                profilerCounter++;
                ProfilerItem profiler = new ProfilerItem(message);
                profiler.StackTrace = isStackTrace ? ProfilerUtil.FormatStackTrace(new StackTrace(true)) : null;
                if (profilerStack.Count > 0)
                    profilerStack.Peek().Items.Add(profiler);
                else
                    data.Profilers.Add(profiler);

                profilerStack.Push(profiler);

                //static cache
                //if (status == ProfileStatus.Static)
                //{
                //cache.Profilers.Add(profiler);
                //if (staticDic.Count > 10)
                //    staticDic.Remove();
                //}
            }
        }

        internal void InnerEndProfile()
        {
            if (Enabled)
            {
                if (profilerStack.Count > 0)
                {
                    profilerStack.Pop().Dispose();
                }
            }
        }

        class ProfileScope : IDisposable
        {
            public ProfileScope(string message)
            {
                ProfilerContext.BeginProfile(message);
            }

            void IDisposable.Dispose()
            {
                ProfilerContext.EndProfile();
            }
        }

    }
}
