using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Framework;

namespace Projects.OAuth
{
    public class OAuthException : PlatformException
    {
        public OAuthException(string errorCode, string message, int statusCode, Exception ex = null)
            : base(String.Format("{0}.{1}", errorCode, message), ex)
        {
            base.Code = statusCode;
        }
    }
}
