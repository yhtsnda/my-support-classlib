using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Tool;
using Projects.Framework;

namespace Projects.OAuth
{
    public interface IAuthorizationCodeRepository : INoShardRepository<AuthorizationCode>
    {
    }
}
