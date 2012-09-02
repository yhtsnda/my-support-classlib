using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Web;

namespace Projects.Tool
{
    public class AspnetCache : AbstractCache
    {
        protected override DateTime CurrentTime
        {
            get
            {
                return DateTime.Now;
            }
        }

        Cache InnerCache
        {
            get { return HttpRuntime.Cache; }
        }

        protected override void SetInner<T>(string key, T value, DateTime expiredTime)
        {
            InnerCache.Insert(key, value, null, expiredTime, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
        }

        protected override T GetInner<T>(string key)
        {
            return (T)InnerCache.Get(key);
        }

        protected override void RemoveInner(string key)
        {
            InnerCache.Remove(key);
        }

    }
}
