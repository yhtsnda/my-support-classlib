using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Profiler
{
    /// <summary>
    /// 接口的TPS为 300W
    /// </summary>
    public class StatService
    {
        static StatServiceData serviceData;

        static StatService()
        {
            serviceData = new StatServiceData();
        }

        public static StatServiceData Data
        {
            get { return serviceData; }
        }

        public static void Increment(string group, string name)
        {
            GetAccumlator(group, a => a.Increment(name));
        }

        public static void Decrement(string group, string name)
        {
            GetAccumlator(group, a => a.Decrement(name));
        }

        public static void SetValue(string group, string name, int value)
        {
            GetAccumlator(group, a => a.SetValue(name, value));
        }

        public static void IncrementInt64(string group, string name)
        {
            GetAccumlator(group, a => a.IncrementInt64(name));
        }

        public static void Add(string group, string name, int value)
        {
            GetAccumlator(group, a => a.Add(name, value));
        }

        public static void AddInt64(string group, string name, int value)
        {
            GetAccumlator(group, a => a.AddInt64(name, value));
        }

        public static void DecrementInt64(string group, string name)
        {
            GetAccumlator(group, a => a.DecrementInt64(name));
        }

        public static void SetValueInt64(string group, string name, long value)
        {
            GetAccumlator(group, a => a.SetValueInt64(name, value));
        }

        public static bool CheckEnabled(string group)
        {
            var g = serviceData.GetOrAddGroup(group);
            return g.Enabled;
        }

        static void GetAccumlator(string group, Action<Accumlator> action)
        {
            var g = serviceData.GetOrAddGroup(group);
            if (g.Enabled)
            {
                action(g.Accumlator);
            }
        }

        public static int Reset(string group = null)
        {
            if (String.IsNullOrEmpty(group))
            {
                serviceData.Groups.ForEach(o => o.Reset());
                return serviceData.Groups.Count;
            }
            var g = serviceData.TryGetGroup(group);
            if (g == null)
                return 0;
            g.Reset();
            return 1;
        }


        public static int Enabled(string group = null)
        {
            if (String.IsNullOrEmpty(group))
            {
                var groups = serviceData.Groups.Where(o => !o.Enabled).ToList();
                groups.ForEach(o => o.Enabled = true);
                return groups.Count;
            }
            var g = serviceData.TryGetGroup(group);
            if (g == null || g.Enabled)
                return 0;
            g.Enabled = true;
            return 1;
        }

        public static int Disabled(string group = null)
        {
            if (String.IsNullOrEmpty(group))
            {
                var groups = serviceData.Groups.Where(o => o.Enabled).ToList();
                groups.ForEach(o => o.Enabled = false);
                return groups.Count;
            }
            var g = serviceData.TryGetGroup(group);
            if (g == null || !g.Enabled)
                return 0;
            g.Enabled = false;
            return 1;
        }
    }
}
