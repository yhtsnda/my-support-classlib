using Projects.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Projects.Framework
{
    public class MonitorHttpModule : IHttpModule
    {
        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(OnContextBeginRequest);
            context.EndRequest += new EventHandler(OnContextEndRequest);
        }

        void OnContextBeginRequest(object sender, EventArgs e)
        {
            try
            {
                var app = (HttpApplication)sender;
                if (app.Request.Url.AbsolutePath == "/pagemonitor")
                {
                    var enabled = app.Request.QueryString["enabled"];
                    if (!String.IsNullOrEmpty(enabled))
                    {
                        MonitorImpl.Enabled = enabled == "true";
                        app.Response.Write(MonitorImpl.Enabled);
                        app.Response.End();
                        return;
                    }

                    var scontain = app.Request.QueryString["contain"];
                    var scount = app.Request.QueryString["count"];

                    int count = 0;
                    Int32.TryParse(scount, out count);

                    var pages = MonitorImpl.GetMonitorDatas().Cast<PageMonitorData>();
                    var datas = pages.Where(o => o.Count >= count && (String.IsNullOrEmpty(scontain) || o.Name.Contains(scontain))).OrderByDescending(o => o.Spans / o.Count);

                    var json = Projects.Tool.Util.JsonConverter.ToJson(new
                    {
                        RunningTime = (long)(MonitorImpl.RunningTime.ToUnixTime() * 1000),
                        CurrentTime = (long)(DateTime.Now.ToUnixTime() * 1000),
                        StartTime = (long)(MonitorImpl.StartTime.ToUnixTime() * 1000),
                        Enabled = MonitorImpl.Enabled,
                        Datas = datas,
                        Count = pages.Sum(o => o.Count)
                    });
                    app.Response.Write(json);
                    app.Response.End();
                    return;
                }
                if (MonitorImpl.Enabled)
                    MonitorImpl.BeginPage(app.Context);
            }
            catch { }
        }

        void OnContextEndRequest(object sender, EventArgs e)
        {
            if (MonitorImpl.Enabled)
                MonitorImpl.EndPage();
        }
    }
}
