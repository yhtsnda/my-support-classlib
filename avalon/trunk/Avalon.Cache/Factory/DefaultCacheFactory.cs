using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Avalon.Utility
{
    internal class DefaultCacheFactory : ICacheFactory
    {
        public ICache GetCacher(string name)
        {
            ICache cache = CacheUtil.GetDefaultCache(name);
            return cache;
        }

        public void Register(string name, ICache cache)
        {
            throw new NotImplementedException();
        }
    }
}
