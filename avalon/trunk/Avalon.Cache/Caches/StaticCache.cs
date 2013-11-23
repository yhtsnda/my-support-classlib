using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Avalon.Utility
{
    public class StaticCache : AbstractCache
    {
        static Hashtable staticDic = Hashtable.Synchronized(new Hashtable());

        protected override IList<CacheItemResult> GetBatchResultInner(Type type, IEnumerable<string> keys)
        {
            return keys.Select(o => new CacheItemResult(o, staticDic[o].DeepClone())).ToList();
        }

        protected override void SetBatchInner(IEnumerable<CacheItem> items, DateTime expiredTime)
        {
            foreach (var item in items)
                staticDic[item.Key] = item.Value.DeepClone();
        }

        protected override void RemoveInner(Type type, string key)
        {
            staticDic.Remove(key);
        }

        protected override bool ContainsInner(Type type, string key)
        {
            return staticDic.Contains(key);
        }
    }
}
