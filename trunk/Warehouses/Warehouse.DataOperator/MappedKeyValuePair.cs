using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.DataOperator
{
    /// <summary>
    /// 映射的键值对,作为Map函数的返回
    /// </summary>
    /// <typeparam name="K">Key对象</typeparam>
    /// <typeparam name="V">Value对象</typeparam>
    public class MappedKeyValuePair<K, V> 
        where K : new() 
        where V : new()
    {
        public K Key { get; set; }
        public V Value { get; set; }
    }
}
