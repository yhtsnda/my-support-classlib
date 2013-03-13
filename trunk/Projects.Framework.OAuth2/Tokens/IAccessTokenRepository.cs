using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Tool;
using Projects.Framework;

namespace Projects.Framework.OAuth2
{
    public interface IAccessTokenRepository : IShardRepository<AccessToken>
    {
    }
}
