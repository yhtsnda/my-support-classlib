using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool.Accumlator
{
    public interface IAccumlator
    {
        void Increment(string name);
        void Decrement(string name);
        void Add(string name, int value);
        void SetValue(string name, int value, AccumlatorType type);

        void IncrementInt64(string name);
        void DecrementInt64(string name);
        void AddInt64(string name, long value);
        void SetValueInt64(string name, long value, AccumlatorType type);
        void ResetAll();
        List<AccumlatorDataInt64> GetDatas();
    }
}
