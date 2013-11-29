using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace Avalon.Profiler
{
    public class CmdCommandService : CommandService, IWorkbenchModule
    {
        public override string BathPath
        {
            get { return "cmd"; }
        }

        public override string CommandHelpContent
        {
            get { return ""; }
        }

        protected override bool OnAuthorizeRequest(HttpApplication app, string path)
        {
            return true;
        }

        public override void InitCommands()
        {
            RegisterCommand("login", (value, app) =>
            {
                if (value == CommandCookieValue)
                {
                    var cookie = new HttpCookie(CommandCookieName, CommandCookieValue);
                    cookie.Expires = DateTime.Now.AddDays(7);
                    app.Response.AppendCookie(cookie);
                    WriteContent("success", app);
                }
                else
                {
                    WriteContent("wrong token.", app);
                }
            });
        }


        public override void InitRequests()
        {
        }

        protected override void ProcessCommandOptions(HttpApplication app)
        {
            var host = "s2.tianyuimg.com";
            var html = String.Format(@"
<html>
<head>
    <title>console</title>
    <link type=""text/css"" rel=""stylesheet"" href=""http://{0}/addins/cmds/v1/cmd.css"" />
    <script src=""http://{0}/addins/jquery/jquery-1.7.1.min.js""></script>
    <script type=""text/javascript"" src=""http://{0}/addins/cmds/v1/jquery.console.js""></script>
    <script type=""text/javascript"" src=""http://{0}/addins/cmds/v1/cmd.js""></script>
</head>
<body>
</body>
</html>
", host);
            app.Response.ContentType = "text/html; charset=UTF-8";
            app.Response.Write(html);
        }

        public void Init()
        {
            RouteTable.Routes.Ignore(String.Format("{0}/{{*pathInfo}}", BathPath));
            InitCommands();
            InitRequests();
        }

        public void BeginRequest(HttpApplication app)
        {
            Process(app);
        }

        public void EndRequest(HttpApplication app)
        {
        }
    }
}
