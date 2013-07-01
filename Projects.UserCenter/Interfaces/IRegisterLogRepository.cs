using Projects.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.UserCenter
{
    public interface IRegisterLogRepository : INoShardRepository<RegisterLog>
    {
    }
}
