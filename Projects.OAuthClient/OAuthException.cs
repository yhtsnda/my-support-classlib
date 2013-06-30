using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.OAuthClient
{
    public class OAuthException : Exception
    {
        public OAuthException(string message)
            : base(message)
        {

        }
    }
}
