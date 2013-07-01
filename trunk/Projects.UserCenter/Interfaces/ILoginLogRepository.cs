using Projects.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.UserCenter
{
    public interface ILoginLogRepository : IShardedRepositoy<LoginLog, DateTime>
    {

    }
}
