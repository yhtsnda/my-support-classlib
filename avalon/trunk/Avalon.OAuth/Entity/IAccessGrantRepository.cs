using Avalon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuth
{
    public interface IAccessGrantRepository : INoShardRepository<AccessGrant>
    {
    }
}
