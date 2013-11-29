using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Avalon.Utility;

namespace Avalon.Profiler
{
    public class StatCommandService : CommandService, IWorkbenchModule
    {
        public StatServiceData Data
        {
            get { return StatService.Data; }
        }

        public override string BathPath
        {
            get { return "stat"; }
        }

        public override string CommandHelpContent
        {
            get
            {
                return @"
auto [true/false]   自动启动新加入的组
keys [group]        获取所有组列表或指定组的键列表
len [group]         获取组个数或指定组的键个数
enabled [group]     启用所有组或指定组，返回操作生效的数量
disabled [group]    禁用所有组或指定组，返回操作生效的数量
info [group]        获取系统信息或指定组的信息
data group [regex]  获取指定组的数据，可用正则进行键名的匹配
";
            }
        }

        public override void InitCommands()
        {
            RegisterCommand("auto", (v, app) => Data.Setting.AutoEnabled = "true".Equals(v, StringComparison.CurrentCultureIgnoreCase));
            RegisterCommand("keys", (v, app) => Process(v, app, g => WriteContent(g.Keys, app), () =>
            {
                if (String.IsNullOrEmpty(v))
                    WriteContent(Data.GroupKeys, app);
                else
                    WriteContent("(nil)", app);
            }));
            RegisterCommand("len", (v, app) => Process(v, app, g => WriteContent(g.Count, app), () =>
            {
                if (String.IsNullOrEmpty(v))
                    WriteContent(Data.GroupCount, app);
                else
                    WriteContent("(nil)", app);
            }));
            RegisterCommand("data", (v, app) =>
            {
                var gname = v;
                var vs = v.Split(' ');
                if (vs.Length > 1)
                    gname = vs[0];
                Process(gname, app, g =>
                {
                    var seconds = (int)(NetworkTime.Now - g.StatTime).TotalSeconds;
                    var datas = g.Datas;

                    if (vs.Length > 1)
                    {
                        try
                        {
                            var regex = new Regex(vs[1], RegexOptions.IgnoreCase);
                            datas = datas.Where(o => regex.IsMatch(o.Key)).ToList();
                        }
                        catch { }
                    }
                    WriteContent(datas.ToDictionary(o => o.Key, o => o.Value.ToString("N0").PadLeft(12) + ((o.Value * 100 / seconds) / 100F).ToString("N2").PadLeft(12)), app);
                });
            });

            RegisterCommand("enabled", (v, app) => WriteContent(StatService.Enabled(v), app));
            RegisterCommand("disabled", (v, app) => WriteContent(StatService.Disabled(v), app));
            RegisterCommand("info", ProcessInfoCommand);
        }

        public override void InitRequests()
        {

        }

        protected override void OnProcessCommand(HttpApplication app, IList<string> cmds)
        {
            if (cmds.Contains("auto") || cmds.Contains("enabled") || cmds.Contains("disabled"))
                SettingProvider.Current.Save(Data.Setting);
        }

        protected override void ProcessCommandOptions(HttpApplication app)
        {
            WriteContent(Data.ToString(), app);
        }

        void ProcessInfoCommand(string v, HttpApplication app)
        {
            if (String.IsNullOrEmpty(v))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("count: {0}\r\n", Data.GroupCount);
                sb.AppendFormat("enabled_count: {0}\r\n", Data.Groups.Count(o => o.Enabled));
                sb.AppendFormat("disabled_count: {0}\r\n", Data.Groups.Count(o => !o.Enabled));
                WriteContent(sb.ToString(), app);
            }
            else
            {
                var g = Data.TryGetGroup(v);
                if (g == null)
                {
                    WriteContent("(nil)", app);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("name: {0}\r\n", g.Name);
                    sb.AppendFormat("enabled: {0}\r\n", g.Enabled);
                    sb.AppendFormat("stat_time: {0}\r\n", g.StatTime);
                    sb.AppendFormat("past_seconds: {0}\r\n", (int)(NetworkTime.Now - g.StatTime).TotalSeconds);
                    sb.AppendFormat("count: {0}\r\n", g.Count);
                    WriteContent(sb.ToString(), app);
                }
            }
        }

        void Process(string group, HttpApplication app, Action<StatGroup> process, Action nilProcess = null)
        {
            var statGroup = Data.TryGetGroup(group);
            if (statGroup == null)
            {
                if (nilProcess != null)
                    nilProcess();
                else
                    WriteContent("(nil)", app);
            }
            else
                process(statGroup);
        }

        public void Init()
        {
            var setting = SettingProvider.Current.Load<StatSetting>(Data.Setting.Id);
            if (setting != null)
            {
                Data.Setting = setting;
                foreach (var group in setting.EnabledGroups)
                {
                    var g = Data.GetOrAddGroup(group);
                    g.Enabled = true;
                }
            }

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
