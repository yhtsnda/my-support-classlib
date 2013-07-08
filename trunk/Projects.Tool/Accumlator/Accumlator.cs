using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Projects.Tool.Accumlator
{
    public class Accumlator : IAccumlator
    {
        private static readonly Accumlator accumlator = new Accumlator();

        ConcurrentDictionary<string, AccumlatorDataInt32> int32Dic = new ConcurrentDictionary<string, AccumlatorDataInt32>();
        ConcurrentDictionary<string, AccumlatorDataInt64> int64Dic = new ConcurrentDictionary<string, AccumlatorDataInt64>();

        private Accumlator()
        { }

        public static Accumlator GetInstance()
        {
            return accumlator;
        }

        public void Increment(string name)
        {
            var data = GetData32(name);
            Interlocked.Increment(ref data.Value);
        }

        public void Decrement(string name)
        {
            var data = GetData32(name);
            Interlocked.Decrement(ref data.Value);
        }

        public void Add(string name, int value)
        {
            var data = GetData32(name);
            Interlocked.Add(ref data.Value, value);
        }

        public void SetValue(string name, int value, AccumlatorType type)
        {
            var data = GetData32(name);
            data.Value = value;
            data.Type = type;
        }

        public void IncrementInt64(string name)
        {
            var data = GetData64(name);
            Interlocked.Increment(ref data.Value);
        }

        public void DecrementInt64(string name)
        {
            var data = GetData64(name);
            Interlocked.Decrement(ref data.Value);
        }

        public void AddInt64(string name, long value)
        {
            var data = GetData64(name);
            Interlocked.Add(ref data.Value, value);
        }

        public void SetValueInt64(string name, long value, AccumlatorType type)
        {
            var data = GetData64(name);
            data.Value = value;
            data.Type = type;
        }

        public List<AccumlatorDataInt64> GetDatas()
        {
            var items = int32Dic.Values.Select(o => new AccumlatorDataInt64(o.Name)
            {
                Type = o.Type,
                Value = o.Value
            }).ToList();
            items.AddRange(int64Dic.Values);

            return items;
        }

        public void ResetAll()
        {
            int32Dic.Clear();
            int64Dic.Clear();
        }

        AccumlatorDataInt32 GetData32(string name)
        {
            return int32Dic.GetOrAdd(name, (n) => new AccumlatorDataInt32(n));
        }

        AccumlatorDataInt64 GetData64(string name)
        {
            return int64Dic.GetOrAdd(name, (n) => new AccumlatorDataInt64(n));
        }
    }
}
