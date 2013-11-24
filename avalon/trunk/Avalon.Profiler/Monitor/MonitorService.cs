using Avalon.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.Profiler
{
    public class MonitorService
    {
        static MonitorHttpCommandService commandService = new MonitorHttpCommandService();
        static MonitorServiceData serviceData = new MonitorServiceData();
        static bool settingInited = false;

        static MonitorService()
        {
            commandService = new MonitorHttpCommandService();
            commandService.InitCommands();
            commandService.InitRequests();

            serviceData.StartTime = DateTime.Now;
        }

        public static CommandService CommandService
        {
            get { return commandService; }
        }

        internal static void Init()
        {
            var setting = SettingProvider.Current.Load<MonitorSetting>(serviceData.Setting.Id);
            if (setting != null)
                serviceData.Setting = setting;

            if (serviceData.Setting.Enabled)
                serviceData.RunningTime = DateTime.Now;
        }

        public static void RequestBegin(HttpApplication app)
        {
            if (!commandService.Process(app))
            {
                MonitorContext.Current.Begin();
            }
        }

        public static void RequestEnd(HttpApplication app)
        {
            MonitorContext.Current.End();
        }

        public static PageMonitorData GetPageMonitorData(string pagePath)
        {
            return ServiceData.Monitors.GetOrAdd(pagePath, (page) =>
            {
                return new PageMonitorData(page);
            });
        }

        internal static MonitorServiceData ServiceData
        {
            get { return serviceData; }
        }
    }
}
