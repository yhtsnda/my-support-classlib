using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Web;

namespace Avalon.Utility
{
    public class AspnetCache : AbstractCache
    {
        protected override DateTime CurrentTime
        {
            get { return DateTime.Now; }
        }

        Cache InnerCache
        {
            get { return HttpRuntime.Cache; }
        }

        protected override IList<CacheItemResult> GetBatchResultInner(Type type, IEnumerable<string> keys)
        {
            return keys.Select(o => new CacheItemResult(o, GetObject(type, o))).ToList();
        }

        object GetObject(Type type, string key)
        {
            var v = InnerCache.Get(key);
            if (v != null && v.GetType() == type)
                return v.DeepClone();
            return null;
        }

        protected override void SetBatchInner(IEnumerable<CacheItem> items, DateTime expiredTime)
        {
            foreach (var item in items)
            {
                var value = item.Value.DeepClone();
                InnerCache.Insert(item.Key, value, null, expiredTime, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }
        }

        protected override void RemoveInner(Type type, string key)
        {
            InnerCache.Remove(key);
        }

        protected override bool ContainsInner(Type type, string key)
        {
            return InnerCache.Get(key) != null;
        }
    }
}
