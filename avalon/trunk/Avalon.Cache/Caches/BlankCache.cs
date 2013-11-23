using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    public class BlankCache : AbstractCache
    {
        protected override IList<CacheItemResult> GetBatchResultInner(Type type, IEnumerable<string> keys)
        {
            return keys.Select(o => new CacheItemResult(o)).ToList();
        }

        protected override void SetBatchInner(IEnumerable<CacheItem> items, DateTime expiredTime)
        {
        }

        protected override void RemoveInner(Type type, string key)
        {
        }

        protected override bool ContainsInner(Type type, string key)
        {
            return false;
        }
    }
}
