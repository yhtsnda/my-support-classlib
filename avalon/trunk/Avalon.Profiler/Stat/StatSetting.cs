using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.Utility;

namespace Avalon.Profiler
{
    public class StatSetting : ISetting
    {
        public StatSetting()
        {
            Id = "stat:" + SettingProvider.SiteIdentity;
            EnabledGroups = new List<string>();
        }

        public string Id { get; set; }

        public bool AutoEnabled { get; set; }

        public List<string> EnabledGroups { get; set; }
    }
}
