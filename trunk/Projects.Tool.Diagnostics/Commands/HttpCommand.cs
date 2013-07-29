using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace Projects.Tool.Diagnostics
{
    public abstract class HttpCommand
    {
        public const string COMMAND_NAME = "command";
        public const string COMMAND_COOKIE = "C7834E515708C07CBD18E0156B631225";

        private Dictionary<string, Action<string, HttpApplication>> commands = 
            new Dictionary<string, Action<string, HttpApplication>>();
        private Dictionary<string, Action<HttpApplication>> request = new Dictionary<string, Action<HttpApplication>>();

        public abstract string BathPath { get; }

        public abstract string HelpContent { get; }

        public HttpCommand()
        {
            commands.Add("help", (value, app) => ProcessCommandHelp(app));
            commands.Add("options", (value, app) => ProcessCommandOptions(app));
        }

        public abstract void InitCommands();
        public abstract void InitRequests();

        public virtual bool Process(HttpApplication app)
        {
            var path = app.Request.Url.AbsolutePath.ToLower().TrimStart('/');
            if (!path.StartsWith(BathPath, StringComparison.CurrentCultureIgnoreCase))
                return false;

            app.CompleteRequest();
            if (OnAuthorize(app))
            {
                if (path.Equals(BathPath, StringComparison.CurrentCultureIgnoreCase))
                    ProcessCommand(app);
                else
                    ProcessRequest(app);
            }
            return true;
        }

        protected virtual bool OnAuthorize(HttpApplication app)
        {
#if DEBUG
            return true;
#else
            var cookie = app.Request.Cookies[COMMAND_NAME];
            if (cookie == null || cookie.Value != COMMAND_COOKIE)
            {
                WriteContent("login need.", app);
                return false;
            }
            else
            {
                return true;
            }
#endif
        }

        protected virtual void ProcessCommand(HttpApplication app)
        {
            var cmdString = app.Server.UrlDecode(app.Request.Url.Query.TrimStart('?'));
            var cmds = ParseCommands(cmdString);
            var keys = cmds.AllKeys;

            if (keys.Length == 0)
            {
                ProcessCommandOptions(app);
            }
            else
            {
                foreach (var key in keys)
                {
                    var value = cmds[key];
                    var handler = commands.TryGetValue(key);
                    if (handler == null)
                    {
                        WriteContent(String.Format("不支持的命令: {0}，请使用 --help 获取帮助信息。", key), app);
                        return;
                    }
                    handler(value, app);
                }
            }
        }

        protected virtual void ProcessRequest(HttpApplication app)
        {
            var path = app.Request.Url.AbsolutePath.ToLower().TrimStart('/').Substring(BathPath.Length);
            var handler = request.TryGetValue(path);
            if(handler == null)
                WriteContent(String.Format("不支持的请求路径: {0}，请使用 --help 获取帮助信息。", path), app);
            handler(app);
        }

        protected virtual void ProcessCommandHelp(HttpApplication app)
        {
            WriteContent(HelpContent, app);
        }

        protected abstract void ProcessCommandOptions(HttpApplication app);

        protected void RegisterCommand(string command, Action<string, HttpApplication> handler)
        {
            this.commands.Add(command, handler);
        }

        protected void RegisterRequest(string requestPath, Action<HttpApplication> handler)
        {
            this.request.Add(requestPath, handler);
        }

        protected void WriteContent(string content, HttpApplication app)
        {
            var raw = app.Request.AcceptTypes.Contains("raw");
            if (!raw)
            {
                content = content.Replace("\r\n", "<br/>");
                content = content.Replace(" ", "&nbsp;");
            }
            app.Response.ContentType = "text/html; charset=UTF-8";
            app.Response.Write(content);
        }

        private NameValueCollection ParseCommands(string command)
        {
            NameValueCollection data = new NameValueCollection();
            var cmds = command.Split(new string[] { "--" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in cmds)
            {
                var bi = item.IndexOf(" ");
                if (bi > -1)
                    data.Add(item.Substring(0, bi).Trim().ToLower(), item.Substring(bi + 1).Trim());
                else
                    data.Add(item.Trim().ToLower(), "");
            }
            return data;
        }
    }
}
