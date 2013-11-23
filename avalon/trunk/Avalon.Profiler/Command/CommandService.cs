using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.Profiler
{
    public abstract class CommandService
    {
        public const string CommandCookieName = "cmd";
        public const string CommandCookieValue = "6F54FAA39BAA48D0A074A8E5A6626A47}";

        Dictionary<string, Action<string, HttpApplication>> commands = new Dictionary<string, Action<string, HttpApplication>>();
        Dictionary<string, Action<HttpApplication>> requests = new Dictionary<string, Action<HttpApplication>>();

        public abstract string BathPath { get; }

        public abstract string CommandHelpContent { get; }

        public CommandService()
        {
            commands.Add("help", (value, app) => ProcessCommandHelp(app));
            commands.Add("options", (value, app) => ProcessCommandOptions(app));
        }

        public abstract void InitCommands();

        public abstract void InitRequests();

        public virtual bool Process(HttpApplication app)
        {
            try
            {
                var path = app.Request.Url.AbsolutePath.ToLower().TrimStart('/');
                if (!path.StartsWith(BathPath, StringComparison.CurrentCultureIgnoreCase))
                    return false;

                app.CompleteRequest();

                if (path.Equals(BathPath, StringComparison.CurrentCultureIgnoreCase))
                    ProcessCommand(app);
                else
                    ProcessRequest(app);
            }
            catch (Exception ex)
            {
                WriteContent("<pre style=\"color:red;\">error: " + ex.Message + "</pre>" + ex.ToString(), app);
            }
            return true;
        }


        protected virtual bool OnAuthorizeRequest(HttpApplication app, string path)
        {
#if DEBUG
            return true;
#else
            var cookie = app.Request.Cookies[CommandCookieName];
            if (cookie == null || cookie.Value != CommandCookieValue)
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
                keys = new string[] { "options" };
            }

            if (OnAuthorizeCommand(app, keys))
            {
                List<string> cmdNames = new List<string>();
                foreach (var key in keys)
                {
                    var value = cmds[key];
                    var handler = commands.TryGetValue(key);
                    if (handler == null)
                    {
                        OnProcessCommand(app, cmdNames);
                        WriteContent(String.Format("不支持的命令: {0}，请使用 --help 获取帮助信息。", key), app);
                        return;
                    }
                    handler(value, app);
                    cmdNames.Add(key);
                }
                OnProcessCommand(app, cmdNames);
            }
        }

        protected virtual void OnProcessCommand(HttpApplication app, IList<string> cmds)
        {
        }

        protected virtual bool OnAuthorizeCommand(HttpApplication app, string[] cmds)
        {
            return OnAuthorizeRequest(app, null);
        }

        protected virtual void ProcessRequest(HttpApplication app)
        {
            var path = app.Request.Url.AbsolutePath.ToLower().TrimStart('/').Substring(BathPath.Length);
            if (OnAuthorizeRequest(app, path))
            {
                var handler = requests.TryGetValue(path);
                if (handler == null)
                    WriteContent(String.Format("不支持的请求路径: {0}，请使用 --help 获取帮助信息。", path), app);

                handler(app);
            }
        }

        protected virtual void ProcessCommandHelp(HttpApplication app)
        {
            WriteContent(CommandHelpContent, app);
        }

        protected abstract void ProcessCommandOptions(HttpApplication app);

        protected void RegisterCommand(string commandName, Action<string, HttpApplication> commandHandler)
        {
            commands[commandName] = commandHandler;
        }

        protected void RegisterRequest(string requestPath, Action<HttpApplication> requestHandler)
        {
            requests[requestPath] = requestHandler;
        }

        protected void WriteNil(HttpApplication app)
        {
            WriteContent("(nil)", app);
        }

        protected void WriteContent(string content, HttpApplication app)
        {
            if (app.Request.AcceptTypes != null && app.Request.AcceptTypes.Length > 0)
            {
                var raw = app.Request.AcceptTypes.Contains("raw");
                if (!raw)
                {
                    content = content.Replace("\r\n", "<br/>");
                    content = content.Replace(" ", "&nbsp;");
                }
            }
            app.Response.ContentType = "text/html; charset=UTF-8";
            app.Response.Write(content);
        }

        protected void WriteContent(int value, HttpApplication app)
        {
            WriteContent(value.ToString(), app);
        }

        protected void WriteContent(IList<string> list, HttpApplication app)
        {
            WriteContent(ListToString(list), app);
        }

        protected void WriteContent(IDictionary<string, string> dic, HttpApplication app)
        {
            WriteContent(DictionaryToString(dic), app);
        }

        protected string ListToString(IList<string> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list == null || list.Count == 0)
            {
                sb.Append("(empty list or set)");
            }
            else
            {
                list = list.OrderBy(o => o).ToList();
                var count = list.Count;
                var pad = count.ToString().Length;
                for (var i = 0; i < count; i++)
                {
                    if (i != 0)
                        sb.AppendLine();
                    sb.AppendFormat("{0}) {1}", (i + 1).ToString().PadLeft(pad), HttpUtility.HtmlEncode(list[i]));
                }
            }
            return sb.ToString();
        }

        protected string DictionaryToString(IDictionary<string, string> dic)
        {
            if (dic == null || dic.Count == 0)
                return "(empty list or set)";

            var keyPad = dic.Keys.Max(o => o.Length);
            var numPad = dic.Count.ToString().Length;
            StringBuilder sb = new StringBuilder();

            var index = 0;
            foreach (var item in dic.OrderBy(o => o.Key))
            {
                if (index > 0)
                    sb.AppendLine();
                sb.AppendFormat("{0}) {1}   {2}", (index + 1).ToString().PadLeft(numPad), item.Key.PadRight(keyPad), HttpUtility.HtmlEncode(item.Value));
                index++;
            }
            return sb.ToString();
        }


        NameValueCollection ParseCommands(string cmdString)
        {
            NameValueCollection data = new NameValueCollection();
            var cmds = cmdString.Split(new string[] { "--" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var cmd in cmds)
            {
                var bi = cmd.IndexOf(" ");
                if (bi > -1)
                    data.Add(cmd.Substring(0, bi).Trim().ToLower(), cmd.Substring(bi + 1).Trim());
                else
                    data.Add(cmd.Trim().ToLower(), "");
            }

            return data;
        }
    }
}
