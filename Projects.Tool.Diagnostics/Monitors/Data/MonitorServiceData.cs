using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool.Diagnostics
{
    internal class MonitorServiceData
    {
        static ConcurrentDictionary<string, PageMonitorData> monitors = new ConcurrentDictionary<string, PageMonitorData>();

        public MonitorServiceData()
        {
            Setting = new MonitorSetting();
        }

        public MonitorSetting Setting { get; set; }

        public ConcurrentDictionary<string, PageMonitorData> Monitors
        {
            get { return monitors; }
        }

        public DateTime StartTime
        {
            get;
            set;
        }

        public DateTime RunningTime
        {
            get;
            set;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("id             {0}\r\n", Setting.Id);
            sb.AppendFormat("enabled        {0}\r\n", Setting.Enabled);
            sb.AppendFormat("startTime      {0}\r\n", StartTime);
            sb.AppendFormat("runningTime    {0}\r\n", RunningTime);
            sb.AppendFormat("matches\r\n");
            foreach (var match in Setting.Matches)
            {
                sb.AppendFormat("   {0}  {1}  {2}\r\n", match.Path, match.Pattern, match.ValidPattern());
            }
            return sb.ToString();
        }
    }
}
