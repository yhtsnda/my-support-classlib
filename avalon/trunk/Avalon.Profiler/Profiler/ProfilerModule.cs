using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Net;
using System.Web.SessionState;
using System.Web.Routing;
using Avalon.Utility;

namespace Avalon.Profiler
{
    public class ProfilerModule : IWorkbenchModule
    {
        public void Init()
        {
            ProfilerService.Init();
            //var url = String.Format("{0}/{{*pathInfo}}");
            RouteTable.Routes.Ignore(String.Format("{0}/{{*pathInfo}}", ProfilerService.CommandService.BathPath));
        }

        public void BeginRequest(HttpApplication app)
        {
            ProfilerService.RequestBegin(app);
        }

        public void EndRequest(HttpApplication app)
        {
            ProfilerService.RequestEnd(app);
        }
    }
}
