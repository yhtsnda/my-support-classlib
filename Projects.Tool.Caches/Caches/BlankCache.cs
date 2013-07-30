using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool
{
    public class BlankCache : ICache
    {
        public void Set<T>(string key, T value) { }

        public void Set<T>(string key, T value, int secondsToLive) { }

        public void Set<T>(string key, T value, DateTime expiredTime) { }

        public void SetBatch<T>(IEnumerable<CacheItem<T>> items, int secondsToLive) { }

        public void SetBatch<T>(IEnumerable<CacheItem<T>> items, DateTime expiredTime) { }

        public T Get<T>(string key)
        {
            return default(T);
        }

        public IEnumerable<T> GetBatch<T>(IEnumerable<string> keys)
        {
            return new List<T>();
        }

        public IEnumerable<T> GetBatch<T>(IEnumerable<string> keys, out IEnumerable<string> missingKeys)
        {
            missingKeys = keys.ToList();
            return new List<T>();
        }

        public void Remove(Type type, string key) { }

        public bool Contains<T>(string key)
        {
            return false;
        }


        public object Get(Type type, string key)
        {
            return null;
        }


        public IEnumerable GetBatch(Type type, IEnumerable<string> keys)
        {
            return new ArrayList();
        }

        public IEnumerable GetBatch(Type type, IEnumerable<string> keys, out IEnumerable<string> missingKeys)
        {
            missingKeys = keys;
            return new ArrayList();
        }
    }
}
