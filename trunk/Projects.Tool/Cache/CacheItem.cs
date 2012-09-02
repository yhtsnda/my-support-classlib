using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool
{
    public class CacheItem<T>
    {
        public string Key { get; set; }
        public T Value { get; set; }

        public CacheItem(string key, T value)
        {
            Key = key;
            Value = value;
        }
    }
}
