using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    public static class RepositoryExtend
    {
        /// <summary>
        /// 使得给定的 IRepository 方法支持缓存及依赖区域定义。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository"></param>
        /// <returns></returns>
        public static IRepositoryCacheable<T> Cache<T>(this T repository)
            where T : class, IRepository
        {
            return new DefaultRepositoryCacheable<T>(repository);
        }

        /// <summary>
        /// 使得给定的 IRepository 方法支持缓存及依赖区域定义。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository"></param>
        /// <returns></returns>
        public static IRepositoryCacheable<T> Cache<T>(this T repository, string cacheFormat, params object[] args)
            where T : class, IRepository
        {
            return new DefaultRepositoryCacheable<T>(repository, String.Format(cacheFormat, args));
        }
    }
}
