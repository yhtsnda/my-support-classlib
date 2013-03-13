using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Projects.Framework;

namespace Projects.Framework.OAuth2
{
    public interface IOAuthClientRepository : IShardRepository<OAuthClient>
    {
    }
}
