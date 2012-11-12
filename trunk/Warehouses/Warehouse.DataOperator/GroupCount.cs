using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.DataOperator
{
    public class GroupCount<Key> : ICalculate<MappedKeyValuePair<Key, int>>
    {
        /// <summary>
        /// 分组计数计算
        /// </summary>
        public GroupCount()
        {
            throw new System.NotImplementedException();
        }

        public void Reduce(List<MappedKeyValuePair<Key, int>> collection)
        {
            throw new NotImplementedException();
        }

        public void Merge()
        {
            throw new NotImplementedException();
        }

        public List<MappedKeyValuePair<Key, int>> Map(Array collection)
        {
            throw new NotImplementedException();
        }
    }
}
