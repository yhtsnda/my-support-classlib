﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool
{
    /// <summary>
    /// 单键对象缓存域。
    /// </summary>
    /// <typeparam name="TEntity">缓存对象的类型。</typeparam>
    /// <typeparam name="TKey">数据键参数的类型。</typeparam>
    public class CacheDomain<TEntity, TKey> : AbstractCacheDomain
    {
        CacheDomainOption<TEntity, TKey> option;

        /// <summary>
        /// 初始化 <see cref="CacheDomain&lt;TEntity, TKey&gt;"/> 类的新实例。
        /// </summary>
        /// <param name="entityKeySelector">通过对象获取数据键参数的表达式。</param>
        /// <param name="missingItemHandler">当无法在缓存中获取单个对象时从源读取数据的委托。</param>
        /// <param name="missingItemsHandler">当无法在缓存中获取多个对象时从源读取数据的委托。</param>
        /// <param name="cacheName">(可选)缓存的名称。</param>
        /// <param name="cacheKeyFormat">(可选)用来生成缓存键的格式化字符串。</param>
        /// <param name="secondesToLive">(可选)存储的时长（秒）。</param>
        /// <param name="contextCacheEnabled">(可选)是否启用 HttpContext 进行缓存。默认值为 true。</param>
        public CacheDomain(Func<TEntity, TKey> entityKeySelector, Func<TKey, TEntity> missingItemHandler, Func<IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler, string cacheName = null, string cacheKeyFormat = null, int secondesToLive = 0, bool contextCacheEnabled = true)
            : this(new CacheDomainOption<TEntity, TKey>
            {
                CacheName = cacheName,
                CacheKeyFormat = cacheKeyFormat,
                SecondesToLive = secondesToLive,
                ContextCacheEnabled = contextCacheEnabled,
                EntityKeySelector = entityKeySelector,
                MissingItemHandler = missingItemHandler,
                MissingItemsHandler = missingItemsHandler
            })
        {
        }

        private CacheDomain(CacheDomainOption<TEntity, TKey> option)
            : base(option)
        {
            if (option.EntityKeySelector == null)
                throw new ArgumentNullException("EntityKeySelector");

            if (option.MissingItemHandler == null && option.MissingItemsHandler == null)
                throw new ArgumentNullException("MissingItemHandler 与 MissingItemsHandler 至少有一个不能为 Null.");

            this.option = option;
        }

        public CacheDomainOption<TEntity, TKey> Option
        {
            get { return option; }
        }

        /// <summary>
        /// 检索缓存域或数据源的单键对象。
        /// </summary>
        /// <param name="key">数据键参数值。</param>
        /// <returns>单键对象或 Null 值。</returns>
        /// <remarks>
        /// 首先在缓存域中检索，如果缓存不存在，则在数据源检索。如果在数据源检索有获取到目标对象，则自动加入到缓存域。
        /// </remarks>
        public virtual TEntity GetItem(TKey key)
        {
            return GetItem(Cache, key, option);
        }

        /// <summary>
        /// 检索缓存域或数据源的多个单键对象。
        /// </summary>
        /// <param name="keys">多个数据键参数值。</param>
        /// <remarks>
        /// 首先在缓存域中检索，如果缓存不存在，则在数据源检索。如果在数据源检索有获取到目标对象，则自动加入到缓存域。
        /// 返回结果中不会包含有 null 的项，并且与给定的 keys 同序。
        /// </remarks>
        /// <returns>检索到多单键对象的 IEnumerable&lt;TEntity&gt;</returns>
        public virtual IEnumerable<TEntity> GetItems(IEnumerable<TKey> keys)
        {
            if (keys == null)
                throw new ArgumentNullException("keys");

            return GetItems(Cache, keys, option);
        }

        /// <summary>
        /// 从缓存域中移除单键对象。
        /// </summary>
        /// <param name="key">要移除单键对象对应数据键参数值。</param>
        public void RemoveCache(TKey key)
        {
            Cache.Remove(typeof(TEntity), option.GetCacheKey(key));
        }

        /// <summary>
        /// 从缓存域中移除多个单键对象。
        /// </summary>
        /// <param name="keys">要移除的多单键对象对应数据键参数值。</param>
        public void RemoveCache(IEnumerable<TKey> keys)
        {
            if (keys == null)
                throw new ArgumentNullException("keys");

            foreach (TKey key in keys)
                Cache.Remove(typeof(TEntity), option.GetCacheKey(key));
        }

        /// <summary>
        /// 将单键对象插入到缓存域，如果缓存域已存在该对象，则替换。
        /// </summary>
        /// <param name="entity">要插入缓存域的单键对象。</param>
        public void SetItemToCache(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Cache.Set(option.GetCacheKey(option.EntityKeySelector(entity)), entity);
        }

        /// <summary>
        /// 从缓存域中检索单键对象。
        /// </summary>
        /// <param name="key">要检索的单键对象对应的数据键参数值。</param>
        /// <returns>检索到的缓存项，未找到时为 null。</returns>
        public virtual TEntity GetItemFromCache(TKey key)
        {
            return Cache.Get<TEntity>(option.GetCacheKey(key));
        }

        /// <summary>
        /// 返回一个值，该值指示在缓存域中是否存在目标单键对象。
        /// </summary>
        /// <param name="key">要检索的单键对象对应的数据键参数值。</param>
        /// <returns>
        /// 如果在缓存域中存在目标单例对象，则为 true；否则为 false。
        /// </returns>
        public virtual bool Contains(TKey key)
        {
            return Cache.Contains<TEntity>(option.GetCacheKey(key));
        }
    }
}
