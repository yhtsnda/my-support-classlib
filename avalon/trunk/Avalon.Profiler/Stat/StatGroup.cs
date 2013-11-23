using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.Utility;

namespace Avalon.Profiler
{
    public class StatGroup
    {
        Accumlator accumlator;
        bool enabled;

        public StatGroup(string name)
        {
            Name = name;
            accumlator = new Accumlator();
        }

        public string Name { get; private set; }

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (enabled != value)
                {
                    enabled = value;
                    if (enabled)
                        Reset();
                }
            }
        }

        public DateTime StatTime { get; set; }

        public Accumlator Accumlator
        {
            get { return accumlator; }
        }

        public IList<string> Keys
        {
            get { return accumlator.GetKeys(); }
        }

        public int Count
        {
            get { return accumlator.Count; }
        }

        public IList<KeyValuePair<string, long>> Datas
        {
            get { return accumlator.GetKeyValues(); }
        }

        public void Reset()
        {
            StatTime = NetworkTime.Now;
            accumlator.Reset();
        }
    }
}
