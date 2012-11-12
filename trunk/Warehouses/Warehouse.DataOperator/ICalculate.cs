using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.DataOperator
{
    public interface ICalculate<Key, Value>
    {
        void Reduce(List<MappedKeyValuePair<Key, Value>> collection);

        void Merge();

        List<MappedKeyValuePair<Key, Value>> Map(Func<object, Key> converter, 
            ArrayList collection);
    }
}
