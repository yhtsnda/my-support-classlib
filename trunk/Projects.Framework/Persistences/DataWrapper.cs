using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    internal class DataItem
    {
        public string Key;

        public object Id;

        public bool Loaded;

        public object Data;
    }

    internal class DataWrapper
    {
        public ClassDefineMetadata Metadata;

        List<DataItem> Datas;

        public DataWrapper(ClassDefineMetadata metadata, IEnumerable ids)
        {
            Metadata = metadata;
            Datas = ids.Cast<object>().Select(o => new DataItem() { Id = o, Key = metadata.GetCacheKeyById(o) }).ToList();
        }

        public IEnumerable<string> GetMissKeys()
        {
            return Datas.Select(o => o.Key);
        }

        public IEnumerable<object> GetMissIds()
        {
            return Datas.Select(o => o.Id);
        }

        public bool HasMiss
        {
            get { return Datas.Any(o => !o.Loaded); }
        }

        public IEnumerable<DataItem> Update(IEnumerable items)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (var item in items)
            {
                string key = Metadata.GetCacheKey(item);
                dic[key] = item;
            }
            List<DataItem> newItems = new List<DataItem>();
            foreach (var data in Datas)
            {
                object di;
                if (dic.TryGetValue(data.Key, out di))
                {
                    data.Loaded = true;
                    data.Data = di;

                    newItems.Add(data);
                }
            }
            return newItems;
        }
    }
}
