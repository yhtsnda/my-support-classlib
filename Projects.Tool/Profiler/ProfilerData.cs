using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool.Profiler
{
    public class ProfilerData
    {
        IList<TraceItem> traces = new List<TraceItem>();
        IList<ProfilerItem> profilters = new List<ProfilerItem>();

        public string Url { get; set; }

        public int Duration { get; set; }

        public DateTime RequestTime { get; set; }

        public IList<TraceItem> Traces
        {
            get { return traces; }
        }

        public IList<ProfilerItem> Profilers
        {
            get { return profilters; }
        }
    }
}
