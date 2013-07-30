using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool
{
    /// <summary>
    /// 单例对象缓存域。
    /// </summary>
    /// <typeparam name="TEntity">缓存对象的类型。</typeparam>
    public class CacheDomain<TEntity> : AbstractCacheDomain
    {
        CacheDomainOption<TEntity> option;

        /// <summary>
        /// 初始化 <see cref="CacheDomain&lt;TEntity&gt;"/> 类的新实例。
        /// </summary>
        /// <param name="missingItemHandler">当无法在缓存中获取单个对象时从源读取数据的委托。</param>
        /// <param name="cacheName">(可选)缓存的名称。</param>
        /// <param name="cacheKeyFormat">(可选)用来生成缓存键的格式化字符串。</param>
        /// <param name="secondesToLive">(可选)存储的时长（秒）。</param>
        /// <param name="contextCacheEnabled">(可选)是否启用 HttpContext 进行缓存。默认值为 true。</param>
        public CacheDomain(Func<TEntity> missingItemHandler, string cacheName = null, string cacheKeyFormat = null, int secondesToLive = 0, bool contextCacheEnabled = true)
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
            if (option.MissingItemHandler == null)
                throw new ArgumentNullException("MissingItemHandler");

            this.option = option;
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
        public virtual TEntity GetItem()
        {
            return GetItem(Cache, option);
        }

        /// <summary>
        /// 从缓存域中移除单例对象。
        /// </summary>
        public void RemoveCache()
        {
            Cache.Remove(typeof(TEntity), option.GetCacheKey());
        }

        /// <summary>
        /// 将单例对象插入到缓存域，如果缓存域已存在该对象，则替换。
        /// </summary>
        /// <param name="entity">要插入缓存域的单例对象。</param>
        public void SetItemToCache(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Cache.Set(option.GetCacheKey(), entity);
        }

        /// <summary>
        /// 从缓存域中检索单例对象。
        /// </summary>
        /// <returns>检索到的缓存项，未找到时为 null。</returns>
        public virtual TEntity GetItemFromCache()
        {
            return Cache.Get<TEntity>(option.GetCacheKey());
        }

        /// <summary>
        /// 返回一个值，该值指示在缓存域中是否存在单例对象。
        /// </summary>
        /// <returns>
        ///   如果在缓存域中存在目标单例对象，则为 true；否则为 false。 
        /// </returns>
        public virtual bool Contains()
        {
            return Cache.Contains<TEntity>(option.GetCacheKey());
        }
    }
}
