using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuth
{
    public class OAuthException : AvalonException
    {
        public OAuthException(string errorCode, string message, int code, Exception ex = null)
            : base(String.Format("{0}: {1}", errorCode, message), ex)
        {
            base.Code = code;
        }
    }
}
