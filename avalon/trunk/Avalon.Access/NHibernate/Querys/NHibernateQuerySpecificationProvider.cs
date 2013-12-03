using Avalon.Utility;
using NHibernate;
using NHibernate.Transform;
using NHibernate.Type;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Avalon.Framework.Querys;
using Avalon.Framework;

namespace Avalon.NHibernateAccess
{
    public class NHibernateQuerySpecificationProvider : IQuerySpecificationProvider
    {
        static MethodInfo countMethod, pagingMethod, listMethod;

        static NHibernateQuerySpecificationProvider()
        {
            var type = typeof(QuerySpecificationExtend);
            countMethod = type.GetMethod("Count");
            pagingMethod = type.GetMethod("ToPaging");
            listMethod = type.GetMethod("ToList");
        }

        public IQuerySpecification<T> CreateSpecification<T>()
        {
            return new NHibernateQuerySpecification<T>(this);
        }

        public IQuerySpecification<T> CreateSpecification<T>(Expression expression)
        {
            return new NHibernateQuerySpecification<T>(this, expression);
        }

        public object Execute(Type queryType, MethodInfo method, IQuerySpecification spec)
        {
            if (method == countMethod)
                return ExecuteCount(queryType, spec);

            throw new NotSupportedException("不支持的方法 " + method.Name);
        }

        public object ExecuteItems<TItem>(Type queryType, MethodInfo method, IQuerySpecification spec)
        {
            if (method == listMethod)
                return ExecuteList<TItem>(queryType, spec);

            if (method == pagingMethod)
                return ExecutePaging<TItem>(queryType, spec);

            throw new NotSupportedException("不支持的方法 " + method.Name);
        }

        int ExecuteCount(Type queryType, IQuerySpecification spec)
        {
            LinqToSqlVisitor vistor = new LinqToSqlVisitor(new QueryEntityManager(queryType), spec.Expression, SqlMode.Count);
            var sql = vistor.SqlBuilder.GetQuerySql();

            var session = (INHibernateShardSession)RepositoryFramework.GetSessionFactory(queryType).OpenSession(queryType, ShardParams.Empty);
            var query = session.InnerSession.CreateSQLQuery(sql).AddScalar("c", NHibernateUtil.Int32);
            return (int)query.UniqueResult();
        }

        IList<TItem> ExecuteList<TItem>(Type queryType, IQuerySpecification spec)
        {
            var resultType = typeof(TItem);
            LinqToSqlVisitor vistor = new LinqToSqlVisitor(new QueryEntityManager(queryType, resultType), spec.Expression, SqlMode.Query);
            var sql = vistor.SqlBuilder.GetQuerySql();

            var session = (INHibernateShardSession)RepositoryFramework.GetSessionFactory(queryType).OpenSession(queryType, ShardParams.Empty);
            var query = session.InnerSession.CreateSQLQuery(sql);
            if (QueryMetadataProvider.SourceProvider.IsSource(typeof(TItem)))
                query = query.AddEntity(typeof(TItem));
            else
                query = (ISQLQuery)query.SetResultTransformer(Transformers.AliasToBean<TItem>());
            var items = query.List<TItem>();
            return items;
        }

        PagingResult<TItem> ExecutePaging<TItem>(Type queryType, IQuerySpecification spec)
        {
            int count = ExecuteCount(queryType, spec);
            var paging = new PagingResult<TItem>(count);
            if (count > 0)
            {
                var items = ExecuteList<TItem>(queryType, spec);
                paging.AddRange(items);
            }
            return paging;
        }
    }
}
