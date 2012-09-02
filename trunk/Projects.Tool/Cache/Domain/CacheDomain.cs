﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool
{
    /// <summary>提供用于创建缓存域对象的静态方法。</summary>
    /// <remarks>
    /// 	<para>CacheDomain 为工厂类，它提供了静态方法，用于创建不同类型组合的缓存域对象。</para>
    /// 	<para>CacheDomain 有 4 种类型：</para>
    /// 	<para>
    /// 		<strong>CacheDomain&lt;TEntity&gt;</strong>
    /// 	</para>
    /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
    /// 		<para>单例对象缓存域。具有全局唯一的特征，适合于全局对象的缓存，一般只能使用进程内缓存，如目录树对象、全局配置信息。</para>
    /// 	</blockquote>
    /// 	<para>
    /// 		<strong>CacheDomain&lt;TEntity, TKey&gt;</strong>
    /// 	</para>
    /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
    /// 		<para>单键对象缓存域，带有一个数据键参数 TKey。通过 TKey 作为数据键参数进行对象的缓存管理，用于单维业务的对象。</para>
    /// 	</blockquote>
    /// 	<para dir="ltr">
    /// 		<strong>CacheDomain&lt;TEntity, TParam, TKey&gt;</strong>
    /// 	</para>
    /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
    /// 		<para>两键对象缓存域，带有一个数据键参数 TKey，一个分组键参数 TParam。通过 TParam, TKey 作为键进行对象的缓存，用于二维业务的对象。</para>
    /// 	</blockquote>
    /// 	<para>
    /// 		<strong>CacheDomain&lt;TEntity, TParam1, TParam2, Tkey&gt;</strong>
    /// 	</para>
    /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
    /// 		<para>三键对象缓存域，带有一个数据键参数 TKey，两个分组键参数 TParam1、TParam2。通过 TParam1, TParam2, TKey 作为键进行对象的缓存，用于三维业务的对象。</para>
    /// 	</blockquote>
    /// </remarks>
    /// <seealso cref="!:domaincache"></seealso>
    public static class CacheDomain
    {
        /// <summary>
        /// 创建单例对象缓存域。
        /// </summary>
        /// <typeparam name="TEntity">缓存对象的类型。</typeparam>
        /// <param name="missingItemHandler">当无法在缓存中获取单个对象时从源读取数据的委托。</param>
        /// <param name="cacheName">(可选)缓存的名称。</param>
        /// <param name="cacheKeyFormat">(可选)用来生成缓存键的格式化字符串。</param>
        /// <param name="secondesToLive">(可选)存储的时长（秒）。</param>
        /// <param name="contextCacheEnabled">(可选)是否启用 HttpContext 进行缓存。默认值为 true。</param>
        /// <returns>
        /// 单例对象缓存域。
        /// </returns>
        public static CacheDomain<TEntity> Create<TEntity>(Func<TEntity> missingItemHandler, string cacheName = null, 
            string cacheKeyFormat = null, int secondesToLive = 0, bool contextCacheEnabled = true)
        {
            return new CacheDomain<TEntity>(missingItemHandler, cacheName, cacheKeyFormat, secondesToLive,
                                            contextCacheEnabled);
        }

        /// <summary>
        /// 创建单键对象缓存域，其中有一个数据键参数。
        /// </summary>
        /// <typeparam name="TEntity">缓存对象的类型。</typeparam>
        /// <typeparam name="TKey">数据键参数的类型。</typeparam>
        /// <param name="entityKeySelector">通过对象获取数据键参数的表达式。</param>
        /// <param name="missingItemHandler">当无法在缓存中获取单个对象时从源读取数据的委托。</param>
        /// <param name="missingItemsHandler">当无法在缓存中获取多个对象时从源读取数据的委托。</param>
        /// <param name="cacheName">(可选)缓存的名称。</param>
        /// <param name="cacheKeyFormat">(可选)用来生成缓存键的格式化字符串。</param>
        /// <param name="secondesToLive">(可选)存储的时长（秒）。</param>
        /// <param name="contextCacheEnabled">(可选)是否启用 HttpContext 进行缓存。默认值为 true。</param>
        /// <returns>
        /// 单键对象缓存域。
        /// </returns>
        public static CacheDomain<TEntity, TKey> Create<TEntity, TKey>(Func<TEntity, TKey> entityKeySelector, Func<TKey, TEntity> missingItemHandler, Func<IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler, string cacheName = null, string cacheKeyFormat = null, int secondesToLive = 0, bool contextCacheEnabled = true)
        {
            return new CacheDomain<TEntity, TKey>(entityKeySelector, missingItemHandler, missingItemsHandler, cacheName, cacheKeyFormat, secondesToLive, contextCacheEnabled);
        }

        /// <summary>
        /// 创建两键对象缓存域，其中有一个数据键参数，一个分组键参数。
        /// </summary>
        /// <typeparam name="TEntity">缓存对象的类型。</typeparam>
        /// <typeparam name="TParam">分组键参数的类型。</typeparam>
        /// <typeparam name="TKey">数据键参数的类型。</typeparam>
        /// <param name="entityKeySelector">通过对象获取数据键参数的表达式。</param>
        /// <param name="missingItemHandler">当无法在缓存中获取单个对象时从源读取数据的委托。</param>
        /// <param name="missingItemsHandler">当无法在缓存中获取多个对象时从源读取数据的委托。</param>
        /// <param name="cacheName">(可选)缓存的名称。</param>
        /// <param name="cacheKeyFormat">(可选)用来生成缓存键的格式化字符串。</param>
        /// <param name="secondesToLive">(可选)存储的时长（秒）。</param>
        /// <param name="contextCacheEnabled">(可选)是否启用 HttpContext 进行缓存。默认值为 true。</param>
        /// <returns>
        /// 两键对象缓存域。
        /// </returns>
        public static CacheDomain<TEntity, TParam, TKey> Create<TEntity, TParam, TKey>(Func<TEntity, TKey> entityKeySelector, Func<TParam, TKey, TEntity> missingItemHandler, Func<TParam, IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler, string cacheName = null, string cacheKeyFormat = null, int secondesToLive = 0, bool contextCacheEnabled = true)
        {
            return new CacheDomain<TEntity, TParam, TKey>(entityKeySelector, missingItemHandler, missingItemsHandler, cacheName, cacheKeyFormat, secondesToLive, contextCacheEnabled);
        }

        /// <summary>
        /// 创建三键对象缓存域，其中有一个数据键参数，二个分组键参数。
        /// </summary>
        /// <typeparam name="TEntity">缓存对象的类型。</typeparam>
        /// <typeparam name="TParam1">分组键参数一的类型。</typeparam>
        /// <typeparam name="TParam2">分组键参数二的类型。</typeparam>
        /// <typeparam name="TKey">数据键参数的类型。</typeparam>
        /// <param name="entityKeySelector">通过对象获取数据键参数的表达式。</param>
        /// <param name="missingItemHandler">当无法在缓存中获取单个对象时从源读取数据的委托。</param>
        /// <param name="missingItemsHandler">当无法在缓存中获取多个对象时从源读取数据的委托。</param>
        /// <param name="cacheName">(可选)缓存的名称。</param>
        /// <param name="cacheKeyFormat">(可选)用来生成缓存键的格式化字符串。</param>
        /// <param name="secondesToLive">(可选)存储的时长（秒）。</param>
        /// <param name="contextCacheEnabled">(可选)是否启用 HttpContext 进行缓存。默认值为 true。</param>
        /// <returns>
        /// 三键对象缓存域。
        /// </returns>
        public static CacheDomain<TEntity, TParam1, TParam2, TKey> Create<TEntity, TParam1, TParam2, TKey>(Func<TEntity, TKey> entityKeySelector, Func<TParam1, TParam2, TKey, TEntity> missingItemHandler, Func<TParam1, TParam2, IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler, string cacheName = null, string cacheKeyFormat = null, int secondesToLive = 0, bool contextCacheEnabled = true)
        {
            return new CacheDomain<TEntity, TParam1, TParam2, TKey>(entityKeySelector, missingItemHandler, missingItemsHandler, cacheName, cacheKeyFormat, secondesToLive, contextCacheEnabled);
        }
    }

    /// <summary>
    /// 单例对象缓存域。
    /// </summary>
    /// <typeparam name="TEntity">缓存对象的类型。</typeparam>
    public  class  CacheDomain<TEntity> : AbstractCacheDomain
    {
        private CacheDomainOption<TEntity> mOption;

        /// <summary>
        /// 初始化 <see cref="CacheDomain&lt;TEntity&gt;"/> 类的新实例。
        /// </summary>
        /// <param name="missingItemHandler">当无法在缓存中获取单个对象时从源读取数据的委托。</param>
        /// <param name="cacheName">(可选)缓存的名称。</param>
        /// <param name="cacheKeyFormat">(可选)用来生成缓存键的格式化字符串。</param>
        /// <param name="secondesToLive">(可选)存储的时长（秒）。</param>
        /// <param name="contextCacheEnabled">(可选)是否启用 HttpContext 进行缓存。默认值为 true。</param>
        public  CacheDomain(Func<TEntity> missingItemHandler, string cacheName= null, 
            string cacheKeyFormat=null,int secondesToLive=0,bool contextCacheEnabled=true ) 
            : this(new CacheDomainOption<TEntity>
            {
                CacheName = cacheName,
                CacheKeyFormat = cacheKeyFormat,
                SecondesToLive = secondesToLive,
                ContextCacheEnabled = contextCacheEnabled,
                MissingItemHandler = missingItemHandler
            })
        {
            
        }

        private CacheDomain(CacheDomainOption<TEntity> option)
            : base(option)
        {
            if(option.MissingItemHandler == null)
                throw new ArgumentNullException("MissingItemHandler");
            this.mOption = option;
        }

        /// <summary>
        /// 检索缓存域或数据源中的单例对象。
        /// </summary>
        /// <remarks>
        /// 首先在缓存域中检索，如果缓存不存在，则在数据源检索。
        /// 如果在数据源检索有获取到目标对象，则自动加入到缓存域。
        /// </remarks>
        /// <returns>
        /// 单例对象或 Null 值。
        /// </returns>
        public TEntity GetItem()
        {
            return GetItem(Cache, mOption);
        }

        /// <summary>
        /// 从缓存域中移除单例对象。
        /// </summary>
        public void RemoveCache()
        {
            Cache.Remove(mOption.GetCacheKey());
        }

        /// <summary>
        /// 将单例对象插入到缓存域，如果缓存域已存在该对象，则替换。
        /// </summary>
        /// <param name="entity">要插入缓存域的单例对象。</param>
        public void SetItemToCache(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Cache.Set(mOption.GetCacheKey(), entity);
        }

        /// <summary>
        /// 从缓存域中检索单例对象。
        /// </summary>
        /// <returns>检索到的缓存项，未找到时为 null。</returns>
        public TEntity GetItemFromCache()
        {
            return Cache.Get<TEntity>(mOption.GetCacheKey());
        }

        /// <summary>
        /// 返回一个值，该值指示在缓存域中是否存在单例对象。
        /// </summary>
        /// <returns>
        ///   如果在缓存域中存在目标单例对象，则为 true；否则为 false。 
        /// </returns>
        public bool Contains()
        {
            return Cache.Contains<TEntity>(mOption.GetCacheKey());
        }
    }

    /// <summary>
    /// 单键对象缓存域。
    /// </summary>
    /// <typeparam name="TEntity">缓存对象的类型。</typeparam>
    /// <typeparam name="TKey">数据键参数的类型。</typeparam>
    public class CacheDomain<TEntity, TKey> : AbstractCacheDomain
    {
        private CacheDomainOption<TEntity, TKey> mOption;

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

            this.mOption = option;
        }

        /// <summary>
        /// 检索缓存域或数据源的单键对象。
        /// </summary>
        /// <param name="key">数据键参数值。</param>
        /// <returns>单键对象或 Null 值。</returns>
        /// <remarks>
        /// 首先在缓存域中检索，如果缓存不存在，则在数据源检索。如果在数据源检索有获取到目标对象，则自动加入到缓存域。
        /// </remarks>
        public TEntity GetItem(TKey key)
        {
            return GetItem(Cache, key, mOption);
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
        public IEnumerable<TEntity> GetItems(IEnumerable<TKey> keys)
        {
            if (keys == null)
                throw new ArgumentNullException("keys");

            return GetItems(Cache, keys, mOption);
        }



        /// <summary>
        /// 从缓存域中移除单键对象。
        /// </summary>
        /// <param name="key">要移除单键对象对应数据键参数值。</param>
        public void RemoveCache(TKey key)
        {
            Cache.Remove(mOption.GetCacheKey(key));
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
                Cache.Remove(mOption.GetCacheKey(key));
        }

        /// <summary>
        /// 将单键对象插入到缓存域，如果缓存域已存在该对象，则替换。
        /// </summary>
        /// <param name="entity">要插入缓存域的单键对象。</param>
        public void SetItemToCache(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Cache.Set(mOption.GetCacheKey(mOption.EntityKeySelector(entity)), entity);
        }

        /// <summary>
        /// 从缓存域中检索单键对象。
        /// </summary>
        /// <param name="key">要检索的单键对象对应的数据键参数值。</param>
        /// <returns>检索到的缓存项，未找到时为 null。</returns>
        public TEntity GetItemFromCache(TKey key)
        {
            return Cache.Get<TEntity>(mOption.GetCacheKey(key));
        }

        /// <summary>
        /// 返回一个值，该值指示在缓存域中是否存在目标单键对象。
        /// </summary>
        /// <param name="key">要检索的单键对象对应的数据键参数值。</param>
        /// <returns>
        /// 如果在缓存域中存在目标单例对象，则为 true；否则为 false。
        /// </returns>
        public bool Contains(TKey key)
        {
            return Cache.Contains<TEntity>(mOption.GetCacheKey(key));
        }
    }

    /// <summary>
    /// 两键对象缓存域。
    /// </summary>
    /// <typeparam name="TEntity">缓存对象的类型。</typeparam>
    /// <typeparam name="TParam">分组键参数的类型。</typeparam>
    /// <typeparam name="TKey">数据键参数的类型。</typeparam>
    public class CacheDomain<TEntity, TParam, TKey> : AbstractCacheDomain
    {
        private CacheDomainOption<TEntity, TParam, TKey> mOption;

        /// <summary>
        /// 初始化 <see cref="CacheDomain&lt;TEntity, TParam, TKey&gt;"/> 类的新实例。
        /// </summary>
        /// <param name="entityKeySelector">通过对象获取数据键参数的表达式。</param>
        /// <param name="missingItemHandler">当无法在缓存中获取单个对象时从源读取数据的委托。</param>
        /// <param name="missingItemsHandler">当无法在缓存中获取多个对象时从源读取数据的委托。</param>
        /// <param name="cacheName">(可选)缓存的名称。</param>
        /// <param name="cacheKeyFormat">(可选)用来生成缓存键的格式化字符串。</param>
        /// <param name="secondesToLive">(可选)存储的时长（秒）。</param>
        /// <param name="contextCacheEnabled">(可选)是否启用 HttpContext 进行缓存。默认值为 true。</param>
        public CacheDomain(Func<TEntity, TKey> entityKeySelector, Func<TParam, TKey, TEntity> missingItemHandler, Func<TParam, IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler, string cacheName = null, string cacheKeyFormat = null, int secondesToLive = 0, bool contextCacheEnabled = true)
            : this(new CacheDomainOption<TEntity, TParam, TKey>
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

        private CacheDomain(CacheDomainOption<TEntity, TParam, TKey> option)
            : base(option)
        {
            if (option.EntityKeySelector == null)
                throw new ArgumentNullException("EntityKeySelector");

            if (option.MissingItemHandler == null && option.MissingItemsHandler == null)
                throw new ArgumentNullException("MissingItemHandler 与 MissingItemsHandler 至少有一个不能为 Null.");

            this.mOption = option;
        }

        /// <summary>
        /// 检索缓存域或数据源的两键对象。
        /// </summary>
        /// <param name="param">分组键参数值。</param>
        /// <param name="key">数据键参数值。</param>
        /// <returns>两键对象或 Null 值。</returns>
        public TEntity GetItem(TParam param, TKey key)
        {
            return GetItem(Cache, param, key, mOption);
        }

        /// <summary>
        /// 检索缓存域或数据源的多个两键对象。
        /// </summary>
        /// <param name="param">分组键参数值。</param>
        /// <param name="keys">多个数据键参数值。</param>
        /// <returns>
        /// 检索到多两键对象的 IEnumerable&lt;TEntity&gt;
        /// </returns>
        /// <remarks>
        /// 首先在缓存域中检索，如果缓存不存在，则在数据源检索。如果在数据源检索有获取到目标对象，则自动加入到缓存域。
        /// 返回结果中不会包含有 null 的项，并且与给定的 keys 同序。
        /// </remarks>
        public IEnumerable<TEntity> GetItems(TParam param, IEnumerable<TKey> keys)
        {
            if (keys == null)
                throw new ArgumentNullException("keys");

            return GetItems(Cache, param, keys, mOption);
        }

        /// <summary>
        /// 从缓存域中移除两键对象。
        /// </summary>
        /// <param name="param">要移除两键对象对应分组键参数值。</param>
        /// <param name="key">要移除两键对象对应数据键参数值。</param>
        public void RemoveCache(TParam param, TKey key)
        {
            Cache.Remove(mOption.GetCacheKey(param, key));
        }

        /// <summary>
        /// 从缓存域中移除多个两键对象。
        /// </summary>
        /// <param name="param">要移除两键对象对应分组键参数值。</param>
        /// <param name="keys">要移除的多个两键对象对应数据键参数值。</param>
        public void RemoveCache(TParam param, IEnumerable<TKey> keys)
        {
            if (keys == null)
                throw new ArgumentNullException("keys");

            foreach (TKey key in keys)
                Cache.Remove(mOption.GetCacheKey(param, key));
        }

        /// <summary>
        /// 将两键对象插入到缓存域，如果缓存域已存在该对象，则替换。
        /// </summary>
        /// <param name="param">两键对象对应的分组键参数值。</param>
        /// <param name="entity">要插入缓存域的两键对象。</param>
        public void SetItemToCache(TParam param, TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Cache.Set(mOption.GetCacheKey(param, mOption.EntityKeySelector(entity)), entity);
        }

        /// <summary>
        /// 从缓存域中检索两键对象。
        /// </summary>
        /// <param name="param">要检索的两键对象对应的分组键参数值。</param>
        /// <param name="key">要检索的两键对象对应的数据键参数值。</param>
        /// <returns>检索到的缓存项，未找到时为 null。</returns>
        public TEntity GetItemFromCache(TParam param, TKey key)
        {
            return Cache.Get<TEntity>(mOption.GetCacheKey(param, key));
        }

        /// <summary>
        /// 返回一个值，该值指示在缓存域中是否存在目标两键对象。
        /// </summary>
        /// <param name="param">要检索的两键对象对应的分组键参数值。</param>
        /// <param name="key">要检索的两键对象对应的数据键参数值。</param>
        /// <returns>
        /// 如果在缓存域中存在目标项，则为 true；否则为 false。
        /// </returns>
        public bool Contains(TParam param, TKey key)
        {
            return Cache.Contains<TEntity>(mOption.GetCacheKey(param, key));
        }
    }

    /// <summary>
    /// 三键对象缓存域
    /// </summary>
    /// <typeparam name="TEntity">缓存对象的类型。</typeparam>
    /// <typeparam name="TParam1">分组键参数一的类型。</typeparam>
    /// <typeparam name="TParam2">分组键参数二的类型。</typeparam>
    /// <typeparam name="TKey">数据键参数的类型。</typeparam>
    public class CacheDomain<TEntity, TParam1, TParam2, TKey> : AbstractCacheDomain
    {
        private CacheDomainOption<TEntity, TParam1, TParam2, TKey> mOption;

        /// <summary>
        /// 初始化 <see cref="CacheDomain&lt;TEntity, TParam1, TParam2, TKey&gt;"/> 类的新实例。
        /// </summary>
        /// <param name="entityKeySelector">通过对象获取数据键参数的表达式。</param>
        /// <param name="missingItemHandler">当无法在缓存中获取单个对象时从源读取数据的委托。</param>
        /// <param name="missingItemsHandler">当无法在缓存中获取多个对象时从源读取数据的委托。</param>
        /// <param name="cacheName">(可选)缓存的名称。</param>
        /// <param name="cacheKeyFormat">(可选)用来生成缓存键的格式化字符串。</param>
        /// <param name="secondesToLive">(可选)存储的时长（秒）。</param>
        /// <param name="contextCacheEnabled">(可选)是否启用 HttpContext 进行缓存。默认值为 true。</param>
        public CacheDomain(Func<TEntity, TKey> entityKeySelector, Func<TParam1, TParam2, TKey, TEntity> missingItemHandler, Func<TParam1, TParam2, IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler, string cacheName = null, string cacheKeyFormat = null, int secondesToLive = 0, bool contextCacheEnabled = true)
            : this(new CacheDomainOption<TEntity, TParam1, TParam2, TKey>
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

        private CacheDomain(CacheDomainOption<TEntity, TParam1, TParam2, TKey> option)
            : base(option)
        {
            if (option.EntityKeySelector == null)
                throw new ArgumentNullException("EntityKeySelector");

            if (option.MissingItemHandler == null && option.MissingItemsHandler == null)
                throw new ArgumentNullException("MissingItemHandler 与 MissingItemsHandler 至少有一个不能为 Null.");

            this.mOption = option;
        }

        /// <summary>
        /// 检索缓存域或数据源的三键对象。
        /// </summary>
        /// <param name="param1">分组键参数一值。</param>
        /// <param name="param2">分组键参数二值。</param>
        /// <param name="key">数据键参数值。</param>
        /// <returns>
        /// 三键对象或 Null 值。
        /// </returns>
        public TEntity GetItem(TParam1 param1, TParam2 param2, TKey key)
        {
            return GetItem(Cache, param1, param2, key, mOption);
        }

        /// <summary>
        /// 检索缓存域或数据源的多个三键对象。
        /// </summary>
        /// <param name="param1">分组键参数一值。</param>
        /// <param name="param2">分组键参数二值。</param>
        /// <param name="keys">多个数据键参数值。</param>
        /// <returns>
        /// 检索到多三键对象的 IEnumerable&lt;TEntity&gt;
        /// </returns>
        /// <remarks>
        /// 首先在缓存域中检索，如果缓存不存在，则在数据源检索。如果在数据源检索有获取到目标对象，则自动加入到缓存域。
        /// 返回结果中不会包含有 null 的项，并且与给定的 keys 同序。
        /// </remarks>
        public IEnumerable<TEntity> GetItems(TParam1 param1, TParam2 param2, IEnumerable<TKey> keys)
        {
            return GetItems(Cache, param1, param2, keys, mOption);
        }

        /// <summary>
        /// 从缓存域中移除三键对象。
        /// </summary>
        /// <param name="param1">要移除三键对象对应分组键参数一值。</param>
        /// <param name="param2">要移除三键对象对应分组键参数二值。</param>
        /// <param name="key">要移除三键对象对应数据键参数值。</param>
        public void RemoveCache(TParam1 param1, TParam2 param2, TKey key)
        {
            Cache.Remove(mOption.GetCacheKey(param1, param2, key));
        }

        /// <summary>
        /// 从缓存域中移除多个三键对象。
        /// </summary>
        /// <param name="param1">要移除三键对象对应分组键参数一值。</param>
        /// <param name="param2">要移除三键对象对应分组键参数二值。</param>
        /// <param name="keys">要移除的多三键对象对应数据键参数值。</param>
        public void RemoveCache(TParam1 param1, TParam2 param2, IEnumerable<TKey> keys)
        {
            if (keys == null)
                throw new ArgumentNullException("keys");

            foreach (TKey key in keys)
                Cache.Remove(mOption.GetCacheKey(param1, param2, key));
        }

        /// <summary>
        /// 将三键对象插入到缓存域，如果缓存域已存在该对象，则替换。
        /// </summary>
        /// <param name="param1">要移除三键对象对应分组键参数一值。</param>
        /// <param name="param2">要移除三键对象对应分组键参数二值。</param>
        /// <param name="entity">要插入缓存域的三键对象。</param>
        public void SetItemToCache(TParam1 param1, TParam2 param2, TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Cache.Set(mOption.GetCacheKey(param1, param2, mOption.EntityKeySelector(entity)), entity);
        }

        /// <summary>
        /// 从缓存域中检索三键对象。
        /// </summary>
        /// <param name="param1">要检索的三键对象对应的分组键参数一值。</param>
        /// <param name="param2">要检索的三键对象对应的分组键参数二值。</param>
        /// <param name="key">要检索的三键对象对应的数据键参数值。</param>
        /// <returns>
        /// 检索到的缓存项，未找到时为 null。
        /// </returns>
        public TEntity GetItemFromCache(TParam1 param1, TParam2 param2, TKey key)
        {
            return Cache.Get<TEntity>(mOption.GetCacheKey(param1, param2, key));
        }

        /// <summary>
        /// 返回一个值，该值指示在缓存域中是否存在目标三键对象。
        /// </summary>
        /// <param name="param1">要检索的三键对象对应的分组键参数一值。</param>
        /// <param name="param2">要检索的三键对象对应的分组键参数二值。</param>
        /// <param name="key">要检索的三键对象对应的数据键参数值。</param>
        /// <returns>
        /// 如果在缓存域中存在目标项，则为 true；否则为 false。
        /// </returns>
        public bool Contains(TParam1 param1, TParam2 param2, TKey key)
        {
            return Cache.Contains<TEntity>(mOption.GetCacheKey(param1, param2, key));
        }
    }
}
