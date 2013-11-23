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

using System.Collections.Concurrent;
using System.IO;
using Avalon.Utility;

namespace Avalon.Profiler
{
    public class ProfilerContext
    {
        const string ProfilerItemKey = "__ProfilerItemKey__";
        const int MaxItemCount = 500;

        ProfilerData data;
        Stopwatch watch;
        Stack<WatchItem> watchStack;
        int profilerCounter = 0;
        bool profilerEnabled = false;

        static ConcurrentQueue<ProfilerData> cache = new ConcurrentQueue<ProfilerData>();

        ProfilerContext()
        {
            data = new ProfilerData();

            var context = HttpContext.Current;
            if (context.IsAvailable())
            {
                data.Url = context.Request.Url.PathAndQuery;
            }

            watchStack = new Stack<WatchItem>();
        }

        public static ProfilerContext Current
        {
            get
            {
                var wb = Workbench.Current;
                ProfilerContext current = (ProfilerContext)wb.Items[ProfilerItemKey];
                if (current == null)
                {
                    current = new ProfilerContext();
                    wb.Items[ProfilerItemKey] = current;
                }
                return current;
            }
        }


        /// <summary>
        /// 开启一个监视块
        /// </summary>
        public static void BeginWatch(string message)
        {
            Current.InnerBeginWatch(message);
        }

        /// <summary>
        /// 结束上一个监视块
        /// </summary>
        public static void EndWatch()
        {
            Current.InnerEndWatch();
        }

        /// <summary>
        /// 获取一个监控块范围
        /// </summary>
        public static IDisposable Watch(string message)
        {
            return new WatchScope(message);
        }

        /// <summary>
        /// 获取 profile 数据
        /// </summary>
        public ProfilerData Data
        {
            get { return data; }
        }

        public Stack<WatchItem> WatchStack
        {
            get { return watchStack; }
        }

        /// <summary>
        /// 自请求开始来的经历的毫秒数
        /// </summary>
        public int Elapsed
        {
            get
            {
                if (watch == null)
                    return -1;
                return (int)watch.ElapsedMilliseconds;
            }
        }

        public bool Enabled
        {
            get { return profilerEnabled; }
        }

        public void Begin()
        {
            var serviceData = ProfilerService.ServiceData;
            if (serviceData.Setting.Enabled && (serviceData.Setting.Mode == ProfilerMode.Static || serviceData.Setting.Mode == ProfilerMode.Request && serviceData.UserProfileEnabled))
            {
                profilerEnabled = true;

                watch = Stopwatch.StartNew();
                data.RequestTime = DateTime.Now;
            }
        }

        public void End(HttpApplication app)
        {
            if (profilerEnabled && watch != null)
            {
                while (watchStack.Count > 0)
                    watchStack.Pop().Dispose();

                data.Duration = (int)watch.ElapsedMilliseconds;
                watch.Stop();

                var request = app.Request;
                data.Request.Url = request.Url.ToString();
                data.Request.Method = request.HttpMethod;
                data.Request.Header = String.Concat(request.Headers.AllKeys.Select(o => String.Format("{0}:{1}\r\n", o, request.Headers[o])));
                if (data.Request.Method == "POST")
                {
                    using (StreamReader sr = new StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        data.Request.Body = sr.ReadToEnd();
                    }
                }
            }
        }

        public void Trace(string type, string title)
        {
            if (profilerEnabled)
                Trace(type, title, ProfilerService.ServiceData.Setting.ShowDetail ? ProfilerUtil.FormatStackTrace(new StackTrace(true)) : "");
        }

        public void Trace(string type, string title, string content)
        {
            if (profilerEnabled && (data.Traces.Count < MaxItemCount || type == "nhibernate" || type == "mongo"))
            {
                TraceItem trace = new TraceItem() { Type = type, Title = watch.ElapsedMilliseconds.ToString() + " " + HttpUtility.HtmlEncode(title), Content = HttpUtility.HtmlEncode(content) };
                data.Traces.Add(trace);
            }
        }

        /// <summary>
        /// 开启一个监视块
        /// </summary>
        internal void InnerBeginWatch(string message)
        {
            if (profilerEnabled)
            {
                if (profilerCounter > MaxItemCount)
                    return;


                profilerCounter++;
                WatchItem profiler = new WatchItem(message);
                profiler.StackTrace = ProfilerService.ServiceData.Setting.ShowDetail ? ProfilerUtil.FormatStackTrace(new StackTrace(true)) : null;
                if (watchStack.Count > 0)
                    watchStack.Peek().Items.Add(profiler);
                else
                    data.Watches.Add(profiler);

                watchStack.Push(profiler);
            }
        }

        /// <summary>
        /// 结束上一个监视块
        /// </summary>
        internal void InnerEndWatch()
        {
            if (profilerEnabled)
            {
                if (watchStack.Count > 0)
                {
                    watchStack.Pop().Dispose();
                }
            }
        }

        class WatchScope : IDisposable
        {
            public WatchScope(string message)
            {
                ProfilerContext.BeginWatch(message);
            }

            void IDisposable.Dispose()
            {
                ProfilerContext.EndWatch();
            }
        }

        public static string GetSql()
        {
            return String.Join("\r\n", Current.Data.Traces.Where(o => o.Type == "nhibernate" || o.Type == "mongo").Select(o => o.Title).ToArray());
        }
    }
}
