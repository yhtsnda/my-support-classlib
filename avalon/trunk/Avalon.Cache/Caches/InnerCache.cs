using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.Utility
{
    public static class InnerCache
    {
        const string ContextDataKey = "__cachecontext__";

        public static string[] ContextKeys
        {
            get { return WorkbenchUtil<string, object>.GetDictionary(ContextDataKey).Keys.ToArray(); }
        }

        public static string[] GetAspnetKeys(string word = null)
        {
            var keys = HttpRuntime.Cache.Cast<DictionaryEntry>().Select(o => o.Key.ToString());
            if (!String.IsNullOrEmpty(word))
                keys = keys.Where(o => o.Contains(word));
            return keys.ToArray();
        }

        public static int GetAspnetKeysCount(string word = null)
        {
            var keys = HttpRuntime.Cache.Cast<DictionaryEntry>().Select(o => o.Key.ToString());
            if (!String.IsNullOrEmpty(word))
                return keys.Where(o => o.Contains(word)).Count();
            return keys.Count();
        }

        public static IList<CacheItemResult> GetContext(this ICache cache, Type type, IEnumerable<string> keys)
        {
            List<CacheItemResult> items = new List<CacheItemResult>();
            foreach (var key in keys)
            {
                object value = WorkbenchUtil<string, object>.GetValue(ContextDataKey, cache.GetContextKey(type, key));
                if (value == null && cache.EmptySupport)
                    value = WorkbenchUtil<string, object>.GetValue(ContextDataKey, cache.GetContextKey(typeof(EmptyData), key));

                var item = new CacheItemResult(key, value);
                item.IsContextHit = item.IsHit;
                items.Add(item);
            }
            return items;
        }

        public static void SetContext(this ICache cache, IEnumerable<CacheItem> items)
        {
            foreach (var item in items)
            {
                WorkbenchUtil<string, object>.SetValue(ContextDataKey, cache.GetContextKey(item.Value.GetType(), item.Key), item.Value);
            }
        }

        public static void RemoveContext(this ICache cache, Type type, string key)
        {
            WorkbenchUtil<string, object>.GetDictionary(ContextDataKey).Remove(cache.GetContextKey(type, key));
        }

        public static bool ContainsContext(this ICache cache, Type type, string key)
        {
            return WorkbenchUtil<string, object>.GetDictionary(ContextDataKey).ContainsKey(cache.GetContextKey(type, key));
        }

        public static string GetContextKey(this ICache cache, Type type, string key)
        {
            return cache.CacheName + ":" + ToPrettyString(type) + ":" + key;
        }

        static string ToPrettyString(Type type)
        {
            if (!type.IsGenericType)
                return type.Name;

            StringBuilder sb = new StringBuilder();
            var gtype = type.GetGenericTypeDefinition();
            sb.Append(gtype.Name.Remove(gtype.Name.IndexOf("`")) + "<");

            bool flag = false;
            foreach (var stype in type.GetGenericArguments())
            {
                if (flag)
                    sb.Append(",");
                sb.Append(ToPrettyString(stype));
                flag = true;
            }
            sb.Append(">");
            return sb.ToString(); ;
        }
    }
}
