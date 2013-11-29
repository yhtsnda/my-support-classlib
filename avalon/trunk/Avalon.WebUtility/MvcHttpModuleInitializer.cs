using Avalon.Utility;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace Avalon.WebUtility
{
    /// <summary>
    /// 在应用初始化是进行httpmodule的动态注册
    /// </summary>
    public class MvcHttpModuleInitializer
    {
        public static void Setup()
        {
            DynamicModuleUtility.RegisterModule(typeof(WorkbenchModule));

            WorkbenchModule.Register(typeof(Avalon.Profiler.ProfilerModule));
            WorkbenchModule.Register(typeof(Avalon.Profiler.MonitorModule));
            WorkbenchModule.Register(typeof(Avalon.Profiler.CmdCommandService));
            WorkbenchModule.Register(typeof(Avalon.Profiler.StatCommandService));
        }
    }
}
