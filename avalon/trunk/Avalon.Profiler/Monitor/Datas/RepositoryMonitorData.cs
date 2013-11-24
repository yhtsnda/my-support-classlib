using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Profiler
{
    public class RepositoryMonitorData
    {
        public string Name { get; set; }

        public int Count;

        public int CacheHits;

        public int Spans;
    }

}
