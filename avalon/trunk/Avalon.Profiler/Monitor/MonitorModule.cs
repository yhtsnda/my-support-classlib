using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace Avalon.Profiler
{
    public class MonitorModule : IWorkbenchModule
    {
        public void Init()
        {
            MonitorService.Init();
            RouteTable.Routes.Ignore(String.Format("{0}/{{*pathInfo}}", MonitorService.CommandService.BathPath));
        }

        public void BeginRequest(HttpApplication app)
        {
            MonitorService.RequestBegin(app);
        }

        public void EndRequest(HttpApplication app)
        {
            MonitorService.RequestEnd(app);
        }
    }
}
