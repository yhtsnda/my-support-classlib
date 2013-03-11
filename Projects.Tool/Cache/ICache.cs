using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool
{
    public interface ICache
    {
        void Set<T>(string key, T value);

        void Set<T>(string key, T value, int secondsToLive);

        void Set<T>(string key, T value, DateTime expiredTime);

        void SetBatch<T>(IEnumerable<CacheItem<T>> items, int secondsToLive);

        void SetBatch<T>(IEnumerable<CacheItem<T>> items, DateTime expiredTime);

        T Get<T>(string key);

        IEnumerable<T> GetBatch<T>(IEnumerable<string> keys);

        IEnumerable<T> GetBatch<T>(IEnumerable<string> keys, out IEnumerable<string> missingKeys);

        void Remove(string key);

        bool Contains<T>(string key);
    }
}
