using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    public interface IFetchable
    {
        void Fetch(object entity);

        void FetchList(IEnumerable entities);
    }
}
