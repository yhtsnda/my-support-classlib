using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Projects.Tool.Util
{
    /// <summary>
    /// 表示线程安全的键和值的集合
    /// </summary>
    /// <typeparam name="TKey">字典中的键的类型</typeparam>
    /// <typeparam name="TValue">字典中的值的类型</typeparam>
    public class SynchronizedDictionary<TKey, TValue> :
        IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        /// 内部字典
        /// </summary>
        private Dictionary<TKey, TValue> _innerDictionary;
        /// <summary>
        /// 线程读写锁
        /// </summary>
        private ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();

        /// <summary>
        /// 
        /// </summary>
        public SynchronizedDictionary()
        {
            _innerDictionary = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        public SynchronizedDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _innerDictionary = new Dictionary<TKey, TValue>(dictionary);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparer"></param>
        public SynchronizedDictionary(IEqualityComparer<TKey> comparer)
        {
            _innerDictionary = new Dictionary<TKey, TValue>(comparer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        public SynchronizedDictionary(int capacity)
        {
            _innerDictionary = new Dictionary<TKey, TValue>(capacity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="comparer"></param>
        public SynchronizedDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            _innerDictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="comparer"></param>
        public SynchronizedDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            _innerDictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            using (new AcquireWriteLock(_locker))
            {
                _innerDictionary.Add(key, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            using (new AcquireReadLock(_locker))
            {
                return _innerDictionary.ContainsKey(key);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<TKey> Keys
        {
            get
            {
                using (new AcquireReadLock(_locker))
                {
                    return _innerDictionary.Keys;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(TKey key)
        {
            using (new AcquireWriteLock(_locker))
            {
                return _innerDictionary.Remove(key);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            using (new AcquireReadLock(_locker))
            {
                return _innerDictionary.TryGetValue(key, out value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<TValue> Values
        {
            get
            {
                using (new AcquireReadLock(_locker))
                {
                    return _innerDictionary.Values;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get
            {
                using (new AcquireReadLock(_locker))
                {
                    return _innerDictionary[key];
                }
            }
            set
            {
                using (new AcquireWriteLock(_locker))
                {
                    _innerDictionary[key] = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            using (new AcquireWriteLock(_locker))
            {
                _innerDictionary.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            using (new AcquireReadLock(_locker))
            {
                return _innerDictionary.Contains<KeyValuePair<TKey, TValue>>(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            using (new AcquireReadLock(_locker))
            {
                _innerDictionary.ToArray<KeyValuePair<TKey, TValue>>().CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                using (new AcquireReadLock(_locker))
                {
                    return _innerDictionary.Count;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            using (new AcquireReadLock(_locker))
            {
                return _innerDictionary.GetEnumerator();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            using (new AcquireReadLock(_locker))
            {
                return _innerDictionary.GetEnumerator();
            }
        }
    }
}
