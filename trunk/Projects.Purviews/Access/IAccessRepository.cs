using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Framework;

namespace Projects.Purviews
{
    public interface IAccessRepository : ISimpleRepository<Access, Access>
    {
        ResultKey DeleteAccessRole(int accessKey);
    }
}
