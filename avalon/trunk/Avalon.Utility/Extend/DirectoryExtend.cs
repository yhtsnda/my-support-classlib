using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    public static class DirectoryExtend
    {
        /// <summary>
        /// 如果指定的键尚不存在，则将键/值对添加到字典中
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dic">The dic.</param>
        /// <param name="key">The key.</param>
        /// <param name="valueFactory">The value factory.</param>
        /// <returns></returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, Func<TKey, TValue> valueFactory)
        {
            if (dic == null)
                throw new ArgumentNullException("dic");
            if (key == null)
                throw new ArgumentNullException("key");
            if (valueFactory == null)
                throw new ArgumentNullException("valueFactory");

            TValue value;
            if (!dic.TryGetValue(key, out value))
            {
                value = valueFactory(key);
                dic.Add(key, value);
            }
            return value;
        }

        /// <summary>
        /// 如果指定的键尚不存在，则将键/值对添加到字典中；如果指定的键已存在，则更新字典中的键/值对。 
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dic">The dic.</param>
        /// <param name="key">The key.</param>
        /// <param name="addValueFactory">The add value factory.</param>
        /// <param name="updateValueFactory">The update value factory.</param>
        /// <returns></returns>
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
        {
            if (dic == null)
                throw new ArgumentNullException("dic");
            if (key == null)
                throw new ArgumentNullException("key");
            if (addValueFactory == null)
                throw new ArgumentNullException("addValueFactory");
            if (updateValueFactory == null)
                throw new ArgumentNullException("updateValueFactory");


            TValue value;
            TValue local;
            if (!dic.TryGetValue(key, out local))
            {
                value = addValueFactory(key);
                dic.Add(key, value);
            }
            else
            {
                value = updateValueFactory(key, local);
                dic[key] = value;
            }
            return value;
        }
    }
}
