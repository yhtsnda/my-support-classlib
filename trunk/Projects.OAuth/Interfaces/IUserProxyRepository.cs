﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Framework;

namespace Projects.OAuth
{
    public interface IUserProxyRepository : INoShardRepository<UserProxy>
    {
        UserProxy GetUserProxy(string userName);
    }
}
