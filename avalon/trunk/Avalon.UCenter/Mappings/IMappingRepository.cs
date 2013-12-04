using Avalon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public interface IMappingRepository : INoShardRepository<Mapping>
    {
    }

    public interface IMappingAppRepository : INoShardRepository<MappingApp>
    {
    }
}
