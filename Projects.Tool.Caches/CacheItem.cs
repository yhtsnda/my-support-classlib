using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool
{
    public class CacheItem<T>
    {
        public CacheItem(string key, T value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public T Value { get; set; }
    }

    public class CacheItemResult
    {
        public CacheItemResult()
        {
        }

        public CacheItemResult(string key, object value)
        {
            Key = key;
            Value = value;
        }

        public CacheItemResult(string key)
        {
            Key = key;
            IsMissing = true;
        }

        public string Key { get; set; }

        public object Value { get; set; }

        public bool IsMissing { get; set; }
    }

    public static class CacheItemResultExtend
    {
        public static IEnumerable<CacheItemResult> GetMissingItems(this IEnumerable<CacheItemResult> items)
        {
            return items.Where(o => o.IsMissing);
        }

        public static IEnumerable<CacheItemResult> GetHitItems(this IEnumerable<CacheItemResult> items)
        {
            return items.Where(o => !o.IsMissing);
        }

        public static IEnumerable<string> GetMissingKeys(this IEnumerable<CacheItemResult> items)
        {
            return items.Where(o => o.IsMissing).Select(o => o.Key);
        }

        public static IEnumerable<CacheItem<object>> ToCacheItems(this IEnumerable<CacheItemResult> items)
        {
            return items.Select(o => new CacheItem<object>(o.Key, o.Value));
        }

        public static void Merge(this IEnumerable<CacheItemResult> items, IEnumerable<CacheItemResult> others)
        {
            var dic = items.ToDictionary(o => o.Key);
            foreach (var other in others)
            {
                var item = dic.TryGetValue(other.Key);
                if (item != null)
                {
                    item.Value = other.Value;
                    item.IsMissing = other.IsMissing;
                }
            }
        }
    }
}
