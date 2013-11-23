using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.Utility
{
    public class HttpContextCache : AbstractCache
    {
        protected override IList<CacheItemResult> GetBatchResultInner(Type type, IEnumerable<string> keys)
        {
            return keys.Select(o => new CacheItemResult(o, null)).ToList();
        }

        protected override void SetBatchInner(IEnumerable<CacheItem> items, DateTime expiredTime)
        {  
        }

        protected override void RemoveInner(Type type, string key)
        {
        }

        protected override bool ContainsInner(Type type, string key)
        {
            return base.ContainsByResult(type, key);
        }
    }
}
