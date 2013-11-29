using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.WebUtility
{
    public class PlatformHttpUnauthorizedException : Exception
    {
        public PlatformHttpUnauthorizedException()
            : base()
        { }

        public PlatformHttpUnauthorizedException(string message)
            : base(message)
        { }

        public PlatformHttpUnauthorizedException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
