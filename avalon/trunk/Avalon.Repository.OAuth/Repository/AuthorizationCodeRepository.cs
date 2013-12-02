using Avalon.Framework;
using Avalon.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Repository.OAuth.Repository
{
    public class AuthorizationCodeRepository : AbstractNoShardRepository<AuthorizationCode>, IAuthorizationCodeRepository
    {
    }
}
