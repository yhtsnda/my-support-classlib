using System;
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
        public static CacheDomain<TEntity> CreateSingleton<TEntity>(Func<TEntity> missingItemHandler, string cacheName = null, string cacheKeyFormat = null, int secondesToLive = 0, bool contextCacheEnabled = true)
        {
            return new CacheDomain<TEntity>(missingItemHandler, cacheName, cacheKeyFormat, secondesToLive, contextCacheEnabled);
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
        public static CacheDomain<TEntity, TKey> CreateSingleKey<TEntity, TKey>(Func<TEntity, TKey> entityKeySelector, Func<TKey, TEntity> missingItemHandler, Func<IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler, string cacheName = null, string cacheKeyFormat = null, int secondesToLive = 0, bool contextCacheEnabled = true)
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
        public static CacheDomain<TEntity, TParam, TKey> CreatePairKey<TEntity, TParam, TKey>(Func<TEntity, TKey> entityKeySelector, Func<TParam, TKey, TEntity> missingItemHandler, Func<TParam, IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler, string cacheName = null, string cacheKeyFormat = null, int secondesToLive = 0, bool contextCacheEnabled = true)
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
        public static CacheDomain<TEntity, TParam1, TParam2, TKey> CreateTripletKey<TEntity, TParam1, TParam2, TKey>(Func<TEntity, TKey> entityKeySelector, Func<TParam1, TParam2, TKey, TEntity> missingItemHandler, Func<TParam1, TParam2, IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler, string cacheName = null, string cacheKeyFormat = null, int secondesToLive = 0, bool contextCacheEnabled = true)
        {
            return new CacheDomain<TEntity, TParam1, TParam2, TKey>(entityKeySelector, missingItemHandler, missingItemsHandler, cacheName, cacheKeyFormat, secondesToLive, contextCacheEnabled);
        }

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
        [Obsolete("使用 CreateSingleton 方法")]
        public static CacheDomain<TEntity> Create<TEntity>(Func<TEntity> missingItemHandler, string cacheName = null, string cacheKeyFormat = null, int secondesToLive = 0, bool contextCacheEnabled = true)
        {
            return new CacheDomain<TEntity>(missingItemHandler, cacheName, cacheKeyFormat, secondesToLive, contextCacheEnabled);
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
        [Obsolete("使用 CreateSingleKey 方法")]
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
        [Obsolete("使用 CreatePairKey 方法")]
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
        [Obsolete("使用 CreateTripletKey 方法")]
        public static CacheDomain<TEntity, TParam1, TParam2, TKey> Create<TEntity, TParam1, TParam2, TKey>(Func<TEntity, TKey> entityKeySelector, Func<TParam1, TParam2, TKey, TEntity> missingItemHandler, Func<TParam1, TParam2, IEnumerable<TKey>, IEnumerable<TEntity>> missingItemsHandler, string cacheName = null, string cacheKeyFormat = null, int secondesToLive = 0, bool contextCacheEnabled = true)
        {
            return new CacheDomain<TEntity, TParam1, TParam2, TKey>(entityKeySelector, missingItemHandler, missingItemsHandler, cacheName, cacheKeyFormat, secondesToLive, contextCacheEnabled);
        }
    }
}
