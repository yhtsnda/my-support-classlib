using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Avalon.Framework
{
    /// <summary>
    /// 使得 IRepository 能支持缓存及依赖区域定制
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepositoryCacheable<T> where T : IRepository
    {
        /// <summary>
        /// 返回 IRepository 的缓存实现代理接口 
        /// </summary>
        /// <returns></returns>
        T Proxy();

        /// <summary>
        /// 使得 IRepository 依赖于指定对象 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IRepositoryCacheable<T> Depend<TEntity>();

        /// <summary>
        /// 使得 IRepository 依赖于缓存名称及值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="regionName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IRepositoryCacheable<T> Depend<TEntity>(string regionName, object value);

        /// <summary>
        /// 使得 IRepository 依赖于缓存区域
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="regions"></param>
        /// <returns></returns>
        IRepositoryCacheable<T> Depend(IEnumerable<CacheRegion> regions);
    }
}
