using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Avalon.Framework;

namespace Avalon.Purviews
{
    public interface IAccessRepository : INoShardRepository<Access>
    {
        void DeleteAccessRole(int accessKey);
    }
}
