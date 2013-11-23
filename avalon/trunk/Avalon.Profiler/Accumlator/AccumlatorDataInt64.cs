using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Profiler
{
    [Serializable]
    public class AccumlatorDataInt64
    {
        public AccumlatorDataInt64(string name)
        {
            Name = name;
            Type = AccumlatorType.Counter;
        }

        public string Name { get; set; }

        public Int64 Value;

        public AccumlatorType Type { get; set; }
    }
}
