using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    public class CacheItem
    {
        public CacheItem(string key, object value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public object Value { get; set; }
    }

    public class CacheItemResult
    {

        bool isHit;

        public CacheItemResult(string key)
            : this(key, null)
        {
        }

        public CacheItemResult(string key, object value)
        {
            Key = key;
            Value = value;
            isHit = value != null;
        }

        /// <summary>
        /// 缓存的键
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// 缓存的值
        /// </summary>
        public object Value { get; internal set; }

        /// <summary>
        /// 是否不存在于缓存中，如果支持空值缓存(Empty）并存在,该值为 false
        /// </summary>
        public bool IsMissing
        {
            get { return !isHit; }
        }

        /// <summary>
        /// 是否存在于缓存中，如果支持空值缓存(Empty）并存在,该值为 true
        /// </summary>
        public bool IsHit
        {
            get { return isHit; }
        }

        internal bool IsContextHit
        {
            get;
            set;
        }

        public bool HasData
        {
            get { return isHit && Value != EmptyData.Value; }
        }

        /// <summary>
        /// 是否为空值，即数据在数据源中不存在
        /// </summary>
        public bool IsEmpty
        {
            get { return Value == null || Value == EmptyData.Value; }
        }

        public object GetValue()
        {
            if (IsMissing || IsEmpty)
                return null;
            return Value;
        }

        public T GetValue<T>()
        {
            if (IsMissing || IsEmpty)
                return default(T);
            return (T)Value;
        }

        internal void SetNull()
        {
            isHit = false;
            Value = null;
        }

        internal void Merge(CacheItemResult result)
        {
            Value = result.Value;
            isHit = result.isHit;
        }

        internal void SetEmpty()
        {
            Value = EmptyData.Value;
            isHit = true;
        }
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

        public static IEnumerable<string> GetHitKeys(this IEnumerable<CacheItemResult> items)
        {
            return items.Where(o => !o.IsMissing).Select(o => o.Key);
        }

        public static IEnumerable<CacheItem> ToCacheItems(this IEnumerable<CacheItemResult> items)
        {
            return items.Select(o => new CacheItem(o.Key, o.Value));
        }

        public static void Merge(this IList<CacheItemResult> items, IList<CacheItemResult> others)
        {
            var dic = items.ToDictionary(o => o.Key);
            foreach (var other in others)
            {
                var item = dic.TryGetValue(other.Key);
                if (item != null)
                {
                    item.Merge(other);
                }
            }
        }
    }
}
