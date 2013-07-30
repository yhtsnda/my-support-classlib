using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool.Diagnostics
{
    public class RequestData
    {
        public DateTime Time { get; set; }

        public string HttpMethod { get; set; }

        public string Url { get; set; }

        public string Body { get; set; }

        public string Header { get; set; }
    }
}
