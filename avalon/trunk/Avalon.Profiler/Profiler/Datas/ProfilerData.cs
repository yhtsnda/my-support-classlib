using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Profiler
{
    public class ProfilerData
    {
        public ProfilerData()
        {
            Traces = new List<TraceItem>();
            Watches = new List<WatchItem>();
            Request = new RequestItem();
        }

        public string Url { get; set; }

        public int Duration { get; set; }

        public DateTime RequestTime { get; set; }

        public List<TraceItem> Traces { get; set; }

        public List<WatchItem> Watches { get; set; }

        public RequestItem Request { get; set; }

    }

}
