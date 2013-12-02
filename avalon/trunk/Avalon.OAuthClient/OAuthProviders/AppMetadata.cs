using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuthClient
{
    public class AppMetadata
    {
        public int ClientId
        {
            get;
            set;
        }

        public string ClientSecret
        {
            get;
            set;
        }

        public long PlatCode
        {
            get;
            set;
        }
    }
}
