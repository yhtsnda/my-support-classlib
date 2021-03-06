﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Framework;

namespace Projects.Purviews
{
    public interface IRoleRepository : ISimpleRepository<Role, Role>
    {
        Role QueryDefaultRole(string instanceKey);
    }
}
