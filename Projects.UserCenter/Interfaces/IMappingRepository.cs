using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Framework;

namespace Projects.UserCenter
{
    public interface IMappingRepository : INoShardRepository<Mapping>
    {
    }
}
