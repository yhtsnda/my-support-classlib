using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.Framework;

namespace Avalon.UCenter
{
    public interface IRegisterLogRepository : INoShardRepository<RegisterLog>
    {
    }

    public interface ILoginLogRepository : IShardedRepository<LoginLog, DateTime>
    {
        bool HasLog(long userId, long platCode, int ip);

        void SetLog(long userId, long platCode, int ip);
    }

    public interface ILastLoginLogRepository : INoShardRepository<LastLoginLog>
    {
    }

    public interface IActiveLogRepository : INoShardRepository<ActiveLog>
    {
    }


}
