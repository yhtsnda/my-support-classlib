using Projects.Tool.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    internal class SessionCache
    {
        const string SessionCacheKey = "_SessionCacheKey_";
        Dictionary<string, object> datas = new Dictionary<string, object>();

        public static SessionCache Current
        {
            get
            {
                var wb = Workbench.Current;
                var cache = (SessionCache)wb.Items[SessionCacheKey];
                if (cache == null)
                {
                    cache = new SessionCache();
                    wb.Items[SessionCacheKey] = cache;
                }
                return cache;
            }
        }

        public bool TryGet(string key, out object entity)
        {
            return datas.TryGetValue(key, out entity);
        }

        public object Get(string key)
        {
            return datas.TryGetValue(key);
        }

        public T Get<T>(string key)
        {
            return (T)datas.TryGetValue(key);
        }

        public void Set(string key, object entity)
        {
            datas[key] = entity;
        }

        public IList GetList(IEnumerable<string> keys)
        {
            List<object> items = new List<object>();
            foreach (var key in keys)
            {
                object entity;
                if (TryGet(key, out entity))
                    items.Add(entity);
            }
            return items;
        }

        public IEnumerable<T> GetList<T>(IEnumerable<string> keys)
        {
            List<T> items = new List<T>();
            foreach (var key in keys)
            {
                items.Add(Get<T>(key));
            }
            return items;
        }

        public void Remove(string key)
        {
            datas.Remove(key);
        }

        public bool Contains(string key)
        {
            return datas.ContainsKey(key);
        }
    }
}
