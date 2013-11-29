using Avalon.Framework;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuth
{
    public interface IClientAuthorizationRepository : INoShardRepository<ClientAuthorization>
    {
        PagingResult<ClientAuthorization> GetClientAuthorizationList(ClientAuthorizationFilter filter);
    }
}
