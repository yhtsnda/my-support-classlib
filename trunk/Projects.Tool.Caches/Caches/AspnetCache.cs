using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Web;
using Projects.Tool.Reflection;

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
            if (value == null)
                return;

            T clone = (T)value.DeepClone();
            InnerCache.Insert(key, clone, null, expiredTime, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
        }

        protected override void RemoveInner(Type type, string key)
        {
            InnerCache.Remove(key);
        }

        protected override object GetInner(Type type, string key)
        {
            var value = InnerCache.Get(key);
            return value.DeepClone();
        }
    }
}
