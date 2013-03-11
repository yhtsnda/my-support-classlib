using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    internal interface IPersister
    {
        object Get(string cacheKey);

        IEnumerable<DataItem> GetList(DataWrapper wrapper);

        void Set(string cacheKey, object entity);

        void SetBatch(IEnumerable<DataItem> items);
    }
}
