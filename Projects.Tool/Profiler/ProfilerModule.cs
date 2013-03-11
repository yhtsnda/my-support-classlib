using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Net;
using System.Web.SessionState;

namespace Projects.Tool.Profiler
{
    public class ProfilerModule : IHttpModule, IRequiresSessionState
    {
        const string ProfilerBegin = "/* --tracebeging--";
        const string ProfilerEnd = "--traceend-- */";

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
            if (ProfilerContext.Current.Enabled)
                ProfilerContext.Current.Begin();
        }

        void OnContextEndRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            if (app.Response.StatusCode == (int)HttpStatusCode.OK)
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                bool isProfilerPage = HttpContext.Current.Request.Url.ToString().Contains("profiler.aspx");
                if (ProfilerContext.Current.Enabled)
                {
                    ProfilerContext.Current.End();
                    if (ProfilerContext.Current.Status == ProfileStatus.Request
                        || ProfilerContext.Current.Status == ProfileStatus.SingleUserRequest)
                    {
                        string json = jss.Serialize(ProfilerContext.Current.Data);

                        if (app.Response.ContentType == "application/json")
                        {
                            app.Response.Write("\r" + ProfilerBegin + json + ProfilerEnd);
                        }
                        else if (app.Response.ContentType == "text/html" && !isProfilerPage)
                        {
                            app.Response.Write("<script type=\"text/javascript\">if(typeof($profiler) != 'undefined'){$profiler.add(" + json + ");}</script>");
                        }
                    }
                }
                else if (ProfilerContext.Current.Status == ProfileStatus.StaticPageRequest)
                {
                    string writeString = "<script type=\"text/javascript\">if(typeof($profiler) != 'undefined'){";
                    foreach (var item in ProfilerContext.Current.StaticCache.ToArray())
                    {
                        string cacheJson = jss.Serialize(item);
                        writeString += "$profiler.add(" + cacheJson + ");";
                    }
                    writeString += "}</script>";
                    app.Response.Write(writeString);
                }
            }
        }
    }
}
