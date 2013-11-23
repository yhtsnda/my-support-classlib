using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    /// <summary>
    /// 所有缓存对象接口
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 是否支持空值缓存
        /// </summary>
        bool EmptySupport { get; set; }
        /// <summary>
        /// 缓存名称
        /// </summary>
        string CacheName { get; set; }
        /// <summary>
        /// 根据类型及键获取对象
        /// </summary>
        object Get(Type type, string key);
        /// <summary>
        /// 根据类型及键获取对象
        /// </summary>
        T Get<T>(string key);
        /// <summary>
        /// 将给定的数据添加到缓存
        /// </summary>
        void Set(string key, object value);
        /// <summary>
        /// 将给定的数据添加到缓存,并设置过期时间(秒数)
        /// </summary>
        void Set(string key, object value, int secondsToLive);
        /// <summary>
        /// 将给定的数据添加到缓存,并设置过期时间
        /// </summary>
        void Set(string key, object value, DateTime expiredTime);
        /// <summary>
        /// 
        /// </summary>
        IEnumerable GetBatch(Type type, IEnumerable<string> keys);
        /// <summary>
        /// 
        /// </summary>
        IList<CacheItemResult> GetBatchResult(Type type, IEnumerable<string> keys);
        /// <summary>
        /// 
        /// </summary>
        CacheItemResult GetResult(Type type, string key);
        /// <summary>
        /// 
        /// </summary>
        IEnumerable<T> GetBatch<T>(IEnumerable<string> keys);
        /// <summary>
        /// 
        /// </summary>
        void SetBatch(IEnumerable<CacheItem> items);
        /// <summary>
        /// 
        /// </summary>
        void SetBatch(IEnumerable<CacheItem> items, int secondsToLive);
        /// <summary>
        /// 
        /// </summary>
        void SetBatch(IEnumerable<CacheItem> items, DateTime expiredTime);
        /// <summary>
        /// 
        /// </summary>
        void Remove(Type type, string key);
        /// <summary>
        /// 
        /// </summary>
        bool Contains(Type type, string key);
    }
}
