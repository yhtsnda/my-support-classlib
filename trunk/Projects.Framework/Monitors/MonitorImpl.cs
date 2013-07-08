using Projects.Tool.Util;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Projects.Framework
{
    public class MonitorImpl
    {
        const string PageMonitorKey = "__pagemonitor__";
        const string RepositoryMonitorKey = "__repositorymonitor__";

        static ConcurrentDictionary<string, PageMonitorData> monitors = new ConcurrentDictionary<string, PageMonitorData>();
        static bool enabled;
        static DateTime runningTime = DateTime.MinValue;
        static DateTime startTime = DateTime.Now;

        static MonitorImpl()
        {
            Enabled = System.Configuration.ConfigurationManager.AppSettings["nd.monitor.enabled"] == "true";
        }

        public static bool Enabled
        {
            get { return enabled; }
            set
            {
                if (enabled != value)
                {
                    enabled = value;
                    if (enabled)
                        runningTime = DateTime.Now;
                    else
                        monitors.Clear();
                }
            }
        }
        public static DateTime StartTime
        {
            get { return startTime; }
        }

        public static DateTime RunningTime
        {
            get { return runningTime; }
        }

        public static PageMonitor CurrentPage
        {
            get { return (PageMonitor)Workbench.Current.Items[PageMonitorKey]; }
        }

        public static void BeginPage(Uri url)
        {
            if (!enabled)
                return;

            var page = (PageMonitor)Workbench.Current.Items[PageMonitorKey];
            if (page == null)
            {
                page = new PageMonitor(url);
                Workbench.Current.Items[PageMonitorKey] = page;
            }
        }

        public static void BeginPage(HttpContext context)
        {
            BeginPage(context.Request.Url);
        }

        public static IDisposable Repository(MethodInfo method)
        {
            return new RepositoryScope(method);
        }

        static void BeginRepository(MethodInfo method)
        {
            if (!enabled)
                return;

            var repository = (RepositoryMonitor)Workbench.Current.Items[RepositoryMonitorKey];
            if (repository == null)
            {
                repository = new RepositoryMonitor(method);
                Workbench.Current.Items[RepositoryMonitorKey] = repository;
            }
            else
            {
                repository.Push();
            }
        }

        static void EndRepository()
        {
            if (!enabled)
                return;

            var repository = (RepositoryMonitor)Workbench.Current.Items[RepositoryMonitorKey];
            if (repository != null)
            {
                if (repository.Pop())
                {
                    repository.End();

                    var page = CurrentPage;
                    if (page != null)
                    {
                        page.Repositories.Add(repository);
                    }
                    Workbench.Current.Items.Remove(RepositoryMonitorKey);
                }
            }
        }

        public static void EndPage()
        {
            if (!enabled)
                return;

            var page = (PageMonitor)Workbench.Current.Items[PageMonitorKey];
            if (page != null)
            {
                page.End();

                var pageData = GetPageMonitorData(page.PagePath);
                pageData.Append(page);

                Workbench.Current.Items.Remove(PageMonitorKey);
            }
        }

        public static ICollection<PageMonitorData> GetMonitorDatas()
        {
            return monitors.Values;
        }

        static PageMonitorData GetPageMonitorData(string pagePath)
        {
            return monitors.GetOrAdd(pagePath, (page) =>
            {
                return new PageMonitorData(page);
            });
        }

        class RepositoryScope : IDisposable
        {
            public RepositoryScope(MethodInfo method)
            {
                if (method.Name != "CreateSpecification")
                {
                    BeginRepository(method);
                }
            }

            public void Dispose()
            {
                EndRepository();
            }
        }

    }
}
