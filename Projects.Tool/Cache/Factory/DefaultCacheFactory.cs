using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool
{
    internal class DefaultCacheFactory : ICacheFactory
    {
        public ICache GetCacher(string name)
        {
            ICache cache = CacheUtil.GetDefaultCache();
            if (cache is AbstractCache)
                ((AbstractCache)cache).CacheName = name;
            return cache;
        }

        public ICache GetCacher(Type type)
        {
            return GetCacher(type.FullName);
        }
    }
}
