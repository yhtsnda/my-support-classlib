using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Projects.Framework
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
        /// 使得 IRepository 依赖于置顶对象属性及值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IRepositoryCacheable<T> Depend<TEntity>(Expression<Func<TEntity, object>> property, object value);
    }
}
