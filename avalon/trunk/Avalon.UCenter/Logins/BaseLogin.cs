using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public abstract class BaseLogin
    {
        public string AppCode { get; set; }

        public string IpAddress { get; set; }

        public string Browser { get; set; }

        public string ExtendField { get; set; }
    }
}
