using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Projects.Framework.Specification
{
    /// <summary>
    /// 提供对数据类型已知的特定数据源的查询进行排序的功能。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OrderBy<T>
    {
        private IQueryable<T> _queryable;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="enumerable"></param>
        public OrderBy(IQueryable<T> queryable)
        {
            _queryable = queryable;
        }

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<T> Queryable
        {
            get { return _queryable; }
        }

        /// <summary>
        /// 根据键按升序对序列的元素排序。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public OrderBy<T> Asc<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            _queryable = _queryable
                .OrderBy(keySelector);
            return this;
        }

        /// <summary>
        /// 根据键按升序对序列的元素排序。
        /// </summary>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <param name="keySelector1"></param>
        /// <param name="keySelector2"></param>
        /// <returns></returns>
        public OrderBy<T> Asc<TKey1, TKey2>(Expression<Func<T, TKey1>> keySelector1,
                                              Expression<Func<T, TKey2>> keySelector2)
        {
            _queryable = _queryable
                .OrderBy(keySelector1)
                .OrderBy(keySelector2);
            return this;
        }

        /// <summary>
        /// 根据键按升序对序列的元素排序。
        /// </summary>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <typeparam name="TKey3"></typeparam>
        /// <param name="keySelector1"></param>
        /// <param name="keySelector2"></param>
        /// <param name="keySelector3"></param>
        /// <returns></returns>
        public OrderBy<T> Asc<TKey1, TKey2, TKey3>(Expression<Func<T, TKey1>> keySelector1,
                                                     Expression<Func<T, TKey2>> keySelector2,
                                                     Expression<Func<T, TKey3>> keySelector3)
        {
            _queryable = _queryable
                .OrderBy(keySelector1)
                .OrderBy(keySelector2)
                .OrderBy(keySelector3);
            return this;
        }

        /// <summary>
        /// 根据键按降序对序列的元素排序。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public OrderBy<T> Desc<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            _queryable = _queryable
                .OrderByDescending(keySelector);
            return this;
        }

        /// <summary>
        /// 根据键按降序对序列的元素排序。
        /// </summary>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <param name="keySelector1"></param>
        /// <param name="keySelector2"></param>
        /// <returns></returns>
        public OrderBy<T> Desc<TKey1, TKey2>(Expression<Func<T, TKey1>> keySelector1,
                                               Expression<Func<T, TKey2>> keySelector2)
        {
            _queryable = _queryable
                .OrderByDescending(keySelector1)
                .OrderByDescending(keySelector2);
            return this;
        }

        /// <summary>
        /// 根据键按降序对序列的元素排序。
        /// </summary>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <typeparam name="TKey3"></typeparam>
        /// <param name="keySelector1"></param>
        /// <param name="keySelector2"></param>
        /// <param name="keySelector3"></param>
        /// <returns></returns>
        public OrderBy<T> Desc<TKey1, TKey2, TKey3>(Expression<Func<T, TKey1>> keySelector1,
                                                      Expression<Func<T, TKey2>> keySelector2,
                                                      Expression<Func<T, TKey3>> keySelector3)
        {
            _queryable = _queryable
                .OrderByDescending(keySelector1)
                .OrderByDescending(keySelector2)
                .OrderByDescending(keySelector3);
            return this;
        }
    }
}
