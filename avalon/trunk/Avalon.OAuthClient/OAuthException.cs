using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuthClient
{
    public class OAuthException : AvalonException
    {
        public OAuthException(string message)
            : base(message)
        {

        }
    }
}
