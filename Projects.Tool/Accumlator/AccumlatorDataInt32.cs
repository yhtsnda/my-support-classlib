using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool.Accumlator
{
    [Serializable]
    public class AccumlatorDataInt32
    {
        public AccumlatorDataInt32(string name)
        {
            Name = name;
            Type = AccumlatorType.Counter;
        }

        public string Name { get; set; }

        public int Value;

        public AccumlatorType Type { get; set; }
    }
}
