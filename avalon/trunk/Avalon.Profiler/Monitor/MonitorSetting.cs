using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Avalon.Profiler
{
    public class MonitorSetting : ISetting
    {
        public MonitorSetting()
        {
            Id = "monitor:" + SettingProvider.SiteIdentity;
            Matches = new List<MonitorMatch>();
        }

        public string Id { get; set; }

        public bool Enabled { get; set; }

        public List<MonitorMatch> Matches { get; set; }

        public bool IsMatch(string url, out string path)
        {
            foreach (var m in Matches)
            {
                if (m.IsMatch(url, out path))
                    return true;
            }
            path = null;
            return false;
        }
    }

    public class MonitorMatch
    {
        string pattern;
        Regex regex = null;

        public string Path { get; set; }

        public string Pattern
        {
            get { return pattern; }
            set
            {
                pattern = value;
                try
                {
                    regex = new Regex(value, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                }
                catch { }
            }
        }

        public bool ValidPattern()
        {
            return regex != null;
        }

        public bool IsMatch(string url, out string path)
        {
            path = null;
            if (regex != null)
            {
                var flag = regex.IsMatch(url);
                if (flag)
                    path = regex.Replace(url, Path);
                return flag;
            }

            return false;
        }
    }
}
