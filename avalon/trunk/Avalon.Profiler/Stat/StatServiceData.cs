using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Profiler
{
    public class StatServiceData
    {
        ConcurrentDictionary<string, StatGroup> groups;

        public StatServiceData()
        {
            Setting = new StatSetting();
            groups = new ConcurrentDictionary<string, StatGroup>();
        }

        public StatSetting Setting
        {
            get;
            internal set;
        }

        public IList<StatGroup> Groups
        {
            get { return groups.Values.ToList(); }
        }

        public StatGroup GetOrAddGroup(string group)
        {
            var g = groups.GetOrAdd(group, (n) => new StatGroup(n));
            if (Setting.AutoEnabled)
                g.Enabled = true;
            return g;
        }

        public StatGroup TryGetGroup(string group)
        {
            return groups.TryGetValue(group);
        }

        public IList<string> GroupKeys
        {
            get { return groups.Keys.ToList(); }
        }

        public int GroupCount
        {
            get { return groups.Count; }
        }

        public bool Contains(string group)
        {
            return groups.ContainsKey(group);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("id: {0}\r\n", Setting.Id);
            sb.AppendFormat("auto: {0}\r\n", Setting.AutoEnabled);
            sb.AppendLine("enabled_groups:");
            foreach (var g in Setting.EnabledGroups)
                sb.AppendLine("  " + g);
            return sb.ToString();
        }
    }
}
