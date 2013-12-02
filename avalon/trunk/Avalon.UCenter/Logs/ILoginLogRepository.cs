using Avalon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public interface ILoginLogRepository : IShardedRepository<LoginLog, DateTime>
    {
        bool HasLog(long userId, long platCode, int ip);

        void SetLog(long userId, long platCode, int ip);
    }
}
