using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.DataOperator
{
    public class GroupCount<Key> : ICalculate<Key, int> 
        where Key : new()
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

        /// <summary>
        /// 用于在主控进程执行合并操作的函数,将Map函数执行的结果合并为Key-List-Value Pair
        /// </summary>
        /// <param name="mapResult">Map函数的执行结果</param>
        /// <returns>Key-List-Value Pair(作为Reduce函数的输入)</returns>
        public List<MappedKeyValuePair<Key, List<int>>> Merge(List<MappedKeyValuePair<Key, int>> mapResult)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 进行分组计数的Map函数
        /// </summary>
        /// <param name="converter">将集合中的项转换为键值的函数</param>
        /// <param name="collection">需进行分组的数据组</param>
        /// <returns>以出现在数组中的项为Key,以出现次数为值的数组</returns>
        public List<MappedKeyValuePair<Key, int>> Map(Func<object, Key> converter,
            Array collection)
        {
            var result = new List<MappedKeyValuePair<Key, int>>();
            //将集合中每个对象循环的送入转换函数中进行处理
            foreach (object obj in collection)
            {
                var key = converter(obj);
                //如果在result中找不到这样的Key值
                if (result.Any(item => item.Key.Equals(key)))
                {
                    var newPair = new MappedKeyValuePair<Key, int> { Key = key, Value = 1 };
                    result.Add(newPair);
                }
                //如果在Result中找到这样的Key值
                else
                {
                    result.First(item => item.Key.Equals(key)).Value++;
                }
            }
            return result;
        }


        public void Merge()
        {
            throw new NotImplementedException();
        }

        public List<MappedKeyValuePair<Key, int>> Map(Func<object, Key> converter, ArrayList collection)
        {
            throw new NotImplementedException();
        }
    }
}
