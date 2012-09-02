using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

using Projects.Tool;

namespace Projects.Framework
{
    /// <summary>
    /// IBatis规约实现
    /// </summary>
    public class IBatisSpecification<T> : ISpecification<T>, IOrderedSpecification<T>, IConditionSpecification<T>
    {
        ISpecificationProvider mProvider;
        Expression<Func<T, bool>> mCriteriaExpression;
        IQueryable<T> query; 
        private string queryStatement;
        private string queryNumberStatement;
        private object searchObject;
        private int? mPageIndex = -1;
        private int? mPageSize = -1;

        internal IBatisSpecification(ISpecificationProvider provider)
            : this(provider, null)
        {
            
        }

        internal IBatisSpecification(ISpecificationProvider provider, Expression<Func<T, bool>> expr)
        {
            this.mProvider = provider;
            this.mCriteriaExpression = expr;
        }

        /// <summary>
        /// 规约表达式
        /// </summary>
        public Expression<Func<T, bool>> CriteriaExpression
        {
            get { return mCriteriaExpression; }
        }

        public ISpecificationProvider Provider
        {
            get { return mProvider; }
        }

        /// <summary>
        /// 查询的语句
        /// </summary>
        public string QueryStatement
        {
            get { return queryStatement; }
            set { queryStatement = value; }
        }

        /// <summary>
        /// 查询记录数量的语句
        /// </summary>
        public string QueryNumberStatement
        {
            get { return queryNumberStatement; }
            set { queryNumberStatement = value; }
        }

        /// <summary>
        /// 查询的条件
        /// </summary>
        public object SearchObject
        {
            get { return searchObject; }
            set { searchObject = value; }
        }

        /// <summary>
        /// 查询分页码
        /// </summary>
        public int? PageIndex
        {
            get { return mPageIndex; }
            set { mPageIndex = value; } }

        /// <summary>
        /// 查询分页数
        /// </summary>
        public int? PageSize
        {
            get { return mPageSize; }
            set { mPageSize = value; } }

        /// <summary>
        /// 查询记录数
        /// </summary>
        public int QueryRecordCount
        {
            get
            {
                if (String.IsNullOrEmpty(queryNumberStatement))
                    return 0;
                return SqlMapperManager.Instance.DefaultMapper.QueryForObject<int>(queryNumberStatement, searchObject);
            }
        }

        //将IBatis查询结果转化为Linq对象
        public IQueryable<T> Query
        {
            get
            {
                if (query == null)
                {
                    if (mCriteriaExpression == null)
                    {
                        if (mPageIndex == -1 && mPageSize == -1)
                        {
                            return SqlMapperManager
                                .Instance
                                .DefaultMapper
                                .QueryForList<T>(this.queryStatement, this.searchObject)
                                .AsQueryable<T>();
                        }
                        var skipSize1 = mPageSize*(mPageIndex - 1);
                        if (skipSize1 != null && mPageSize != null)
                        {
                                return SqlMapperManager
                                    .Instance
                                    .DefaultMapper
                                    .QueryForList<T>(this.queryStatement, this.searchObject, (int)skipSize1, (int)mPageSize)
                                    .AsQueryable<T>();
                        }
                    }

                    if (mPageIndex == -1 && mPageSize == -1)
                        return SqlMapperManager
                            .Instance
                            .DefaultMapper
                            .QueryForList<T>(this.queryStatement, this.searchObject)
                            .AsQueryable<T>().Where(mCriteriaExpression);
                    var skipSize2 = mPageSize * (mPageIndex - 1);
                    if (skipSize2 != null && mPageSize != null)
                    {
                        return SqlMapperManager
                            .Instance
                            .DefaultMapper
                            .QueryForList<T>(this.queryStatement, this.searchObject, (int)skipSize2, (int)mPageSize)
                            .AsQueryable<T>().Where(mCriteriaExpression);
                    }
                }
                return query;
            }
            set { query = value; }
        }
    }
}
