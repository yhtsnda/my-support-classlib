using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.Profiler
{
    internal class MonitorHttpCommandService : CommandService
    {
        const string DetectSecretKey = "732CEDA25D944DC6A4DC91B019D10BB2}";

        public override string BathPath
        {
            get { return "monitor"; }
        }

        MonitorSetting Setting
        {
            get { return MonitorService.ServiceData.Setting; }
        }

        MonitorServiceData ServiceData
        {
            get { return MonitorService.ServiceData; }
        }

        public override string CommandHelpContent
        {
            get
            {
                return @"
[--help | --options | [
    --enabled | 
    --disabled | 
    --addmatch path pattern | 
    --delmatch path |
    --clearmatch
/monitor/view   显示本站点的请求统计数据
/monitor/data   数据接口

选项：
--help          显示命令的帮助信息
--options       显示当前 monitor 的选项设置
--enabled       启用或重置
--disabled      禁用（默认）
--addmatch      新增地址匹配项目
--delmatch      删除地址匹配项目
--clearmatch    清除地址匹配项目
";
            }
        }

        public override void InitCommands()
        {
            RegisterCommand("enabled", (value, app) =>
            {
                Setting.Enabled = true;
                ServiceData.RunningTime = DateTime.Now;
                ServiceData.Monitors.Clear();
            });
            RegisterCommand("disabled", (value, app) => Setting.Enabled = false);
            RegisterCommand("addmatch", (value, app) =>
            {
                var vs = value.ToLower().Split(' ');
                Setting.Matches.Add(new MonitorMatch() { Path = vs[0], Pattern = vs[1] });
            });
            RegisterCommand("delmatch", (value, app) =>
            {
                var v = value.ToLower();
                Setting.Matches.RemoveAll(o => o.Path == v);
            });
            RegisterCommand("clearmatch", (value, app) => Setting.Matches.Clear());
            RegisterCommand("detect", ProcessDetectCommand);
        }

        public override void InitRequests()
        {
            RegisterRequest("/data", ProcessDataRequest);
            RegisterRequest("/view", ProcessViewRequest);
        }

        protected override void ProcessCommandOptions(HttpApplication app)
        {
            WriteContent(ServiceData.ToString(), app);
        }

        protected override bool OnAuthorizeCommand(HttpApplication app, string[] cmds)
        {
            if (cmds.Length == 1 && cmds[0] == "detect")
                return true;

            return base.OnAuthorizeCommand(app, cmds);
        }

        protected override void OnProcessCommand(HttpApplication app, IList<string> cmds)
        {
            if (cmds.Count > 0)
                SettingProvider.Current.Save(Setting);
        }

        void ProcessDetectCommand(string value, HttpApplication app)
        {
            string[] names = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (names.Length < 1)
            {
                WriteContent("缺少参数.", app);
            }
            else
            {
                if (names[0] != DetectSecretKey)
                    WriteContent("校验错误.", app);
                else
                    WriteContent(ServerDetectorImpl.Detect(names.Skip(1).ToArray()), app);
            }
        }

        void ProcessDataRequest(HttpApplication app)
        {
            var scontain = app.Request.QueryString["contain"];
            var scount = app.Request.QueryString["count"];

            int count = 0;
            Int32.TryParse(scount, out count);

            var pages = ServiceData.Monitors.Values.ToList();
            var datas = pages.Where(o => o.Count >= count && (String.IsNullOrEmpty(scontain) || o.Name.Contains(scontain))).OrderByDescending(o => o.Spans / o.Count);

            var json = JsonConverter.ToJson(new
            {
                RunningTime = (long)(ServiceData.RunningTime.ToUnixTime() * 1000),
                CurrentTime = (long)(DateTime.Now.ToUnixTime() * 1000),
                StartTime = (long)(ServiceData.StartTime.ToUnixTime() * 1000),
                Enabled = Setting.Enabled,
                Datas = datas,
                Count = pages.Sum(o => o.Count)
            });
            app.Response.ContentType = "application/json";
            app.Response.Write(json);
        }

        void ProcessViewRequest(HttpApplication app)
        {
            var html = @"
<html>
<head>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
    <title>页面执行监控</title>
    <link type=""text/css"" rel=""stylesheet"" href=""http://s2.tianyuimg.com/addins/monitor/v1/monitor.css"" />
    <script type=""text/javascript"" src=""http://s2.tianyuimg.com/?f=addins/jquery/jquery-1.7.1.js,addins/ko/knockout-2.2.0.js,addins/ko/knockout.mapping-2.3.5.js,addins/utils/json2.js,addins/linqjs/v3.0.0/linq.min.js,addins/monitor/v1/monitor.js""></script>
</head>
<body>
    <p>
        当前监控状态：<span id=""lblStatus""></span>，监控开始时间：<span id=""lblTime""></span>，总次数：<span id=""lblCount""></span>，并发：<span id=""lblPreSecond""></span>
    </p>
    <p>
        最小请求数量：<input type=""text"" id=""txtCount"" value=""100"" />
        URL包含：<input type=""text"" id=""txtContain"" />
        <input id=""btnRefresh"" type=""button"" value=""刷新"" />
    </p>
    <table>
        <tr>
            <th>名称</th>
            <th style=""width: 70px;"">缓存/调用</th>
            <th style=""width: 60px;"">仓储</th>
            <th style=""width: 100px;"">平均耗时(ms)</th>
            <th style=""width: 100px;"">总耗时(ms)</th>
            <th style=""width: 70px;"">并发(s)</th>
        </tr>
        <!-- ko foreach: $root -->
        <tr class=""real"">
            <td data-bind=""text:Name""></td>
            <td data-bind=""html:format(Count)"" style=""text-align: right;""></td>
            <td data-bind=""html:Reps.length"" style=""text-align: right;""></td>
            <td data-bind=""html:format(Math.round(Spans/Count))"" style=""text-align: right;""></td>
            <td data-bind=""html:format(Spans)"" style=""text-align: right;""></td>
            <td data-bind=""html:Math.round(10*Count/(timespan/1000))/10"" style=""text-align: right;""></td>
        </tr>
        <!-- ko foreach: Reps -->
        <tr style=""color: #aaa;"" class=""inner"">
            <td data-bind=""text:Name"" style=""padding-left: 36px;""></td>
            <td data-bind=""html:format(CacheHits) + '/' + format(Count)"" style=""text-align: right;""></td>
            <td></td>
            <td data-bind=""html:format(Math.round(Spans/Count))"" style=""text-align: right;""></td>
            <td data-bind=""html:format(Spans)"" style=""text-align: right;""></td>
            <td></td>
        </tr>
        <!-- /ko -->
        <!-- /ko -->
    </table>
</body>
</html>
";
            app.Response.ContentType = "text/html; charset=UTF-8";
            app.Response.Write(html);
        }
    }
}
