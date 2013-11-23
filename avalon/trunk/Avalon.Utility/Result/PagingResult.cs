using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    public class PagingResult<TEntity>
    {
        IList<TEntity> items;

        public PagingResult()
        {

        }

        public PagingResult(int totalCount)
        {
            TotalCount = totalCount;
            items = new List<TEntity>();
        }

        public PagingResult(int totalCount, IEnumerable<TEntity> items)
        {
            TotalCount = totalCount;
            if (items == null)
                this.items = new List<TEntity>();
            else
                this.items = new List<TEntity>(items);
        }

        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 实体集合
        /// </summary>
        public IList<TEntity> Items
        {
            get { return items; }
            set { items = value; }
        }

        public void AddRange(IEnumerable<TEntity> items)
        {
            this.items.AddRange(items);
        }
    }

    public static class PagingResult
    {
        /// <summary>
        /// 对数据进行分页
        /// </summary>
        /// <typeparam name="TEntity">目标类型</typeparam>
        /// <param name="countQuery">目标总数数的查询，不带最终的结果</param>
        /// <param name="selectQuery">目标数据的查询，不带分页的参数</param>
        /// <param name="pageIndex">分页的页索引，从0开始</param>
        /// <param name="pageSize">分页的大小。如果值为0,则表示不分页；如果为负值，表示不返回总数。</param>
        /// <returns></returns>
        public static PagingResult<TEntity> Create<TEntity>(IQueryable<TEntity> countQuery, IQueryable<TEntity> selectQuery, int pageIndex, int pageSize, bool ignoreTotalCount = false)
        {
            int totalCount = -1;
            if (pageSize > 0 && pageSize != Int32.MaxValue)
                totalCount = countQuery.Count();

            PagingResult<TEntity> paging = new PagingResult<TEntity>(totalCount);
            pageSize = Math.Abs(pageSize);

            IEnumerable<TEntity> result;
            if (pageSize == 0 || pageSize == Int32.MaxValue)
                result = selectQuery.ToArray();
            else
                result = selectQuery.Skip(pageIndex * pageSize).Take(pageSize).ToArray();
            paging.AddRange(result);

            #region [zhangcl 2011-08-03　Add]处理取所有记录返回总数的问题
            if (paging.TotalCount == -1)
            {
                paging.TotalCount = paging.Items.Count;
            }
            #endregion

            return paging;
        }

        public static PagingResult<Tuple<TEntity, TJoin>> TupleJoin<TEntity, TJoin, TKey>(this PagingResult<TEntity> paging, IEnumerable<TJoin> joins, Func<TEntity, TKey> entityKeySelector, Func<TJoin, TKey> joinKeySelector)
        {
            PagingResult<Tuple<TEntity, TJoin>> output = new PagingResult<Tuple<TEntity, TJoin>>(paging.TotalCount);
            output.AddRange(paging.Items.TupleJoin<TEntity, TJoin, TKey>(joins, entityKeySelector, joinKeySelector));
            return output;
        }
    }
}
