using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    internal interface IEntityDefineSupport<T> where T : IRepository
    {
        IRepositoryCacheable<T> Depend(List<CacheRegion> regions, object entity);
    }
}
