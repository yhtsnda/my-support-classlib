using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Avalon.Profiler
{
    public class MonitorContext
    {
        const string MonitorItemKey = "__MonitorItemKey__";
        static Regex regex = new Regex(@"[\d]{2,}", RegexOptions.Compiled);


        bool monitorEnabled = false;
        Stopwatch watch;
        MonitorServiceData serviceData;
        string path;

        MonitorContext()
        {
            serviceData = MonitorService.ServiceData;
            Repositories = new List<RepositoryMonitor>();

            var context = HttpContext.Current;
            if (context.IsAvailable())
            {
                path = GetPagePath(context.Request.Url);
            }
        }

        public static MonitorContext Current
        {
            get
            {
                var wb = Workbench.Current;
                MonitorContext current = (MonitorContext)wb.Items[MonitorItemKey];
                if (current == null)
                {
                    current = new MonitorContext();
                    wb.Items[MonitorItemKey] = current;
                }
                return current;
            }
        }

        public static IDisposable Repository(MethodInfo method, bool cacheHit = false)
        {
            return new RepositoryScope(MonitorContext.Current, method, cacheHit);
        }

        public bool Enabled
        {
            get { return monitorEnabled; }
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

        public string Path
        {
            get { return path; }
        }

        public List<RepositoryMonitor> Repositories
        {
            get;
            private set;
        }

        public RepositoryMonitor CurrentRepositoryMonitor
        {
            get;
            set;
        }

        public void Begin()
        {
            if (serviceData.Setting.Enabled && !String.IsNullOrEmpty(path))
            {
                monitorEnabled = true;
                watch = Stopwatch.StartNew();
            }
        }

        public void End()
        {
            if (monitorEnabled && watch != null)
            {
                watch.Stop();

                var data = MonitorService.GetPageMonitorData(path);
                data.Append(this);

                Workbench.Current.Items.Remove(MonitorItemKey);
            }
        }

        string GetPagePath(Uri uri)
        {
            var url = uri.AbsolutePath.ToLower();

            string path;
            if (serviceData.Setting.IsMatch(url, out path))
                return path;

            url = regex.Replace(url, "*");
            return url;
        }

        class RepositoryScope : IDisposable
        {
            MonitorContext context;

            public RepositoryScope(MonitorContext context, MethodInfo method, bool cacheHit = false)
            {
                this.context = context;

                if (method.Name != "CreateSpecification" && context.Enabled)
                {
                    var repository = context.CurrentRepositoryMonitor;
                    if (repository == null)
                    {
                        repository = new RepositoryMonitor(method, cacheHit);
                        context.CurrentRepositoryMonitor = repository;
                    }
                    repository.Push();
                }
            }

            public void Dispose()
            {
                var repository = context.CurrentRepositoryMonitor;
                if (repository != null)
                {
                    if (repository.Pop())
                    {
                        repository.End();
                        context.Repositories.Add(repository);
                        context.CurrentRepositoryMonitor = null;
                    }
                }
            }
        }
    }
}
