using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Profiler
{
    public class RequestItem
    {
        public string Url { get; set; }

        public string Method { get; set; }

        public string Header { get; set; }

        public string Body { get; set; }
    }
}
