using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    public interface IFetchable
    {
        void Fetch(object entity);

        bool CheckFetchCancel(object entity);
    }
}
