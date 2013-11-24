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

        public object Execute(Type queryType, MethodInfo method, Expression expression)
        {
            if (method == countMethod)
                return ExecuteCount(queryType, expression);

            throw new NotSupportedException("不支持的方法 " + method.Name);
        }

        public object ExecuteItems<TItem>(Type queryType, MethodInfo method, Expression expression)
        {
            if (method == listMethod)
                return ExecuteList<TItem>(queryType, expression);

            if (method == pagingMethod)
                return ExecutePaging<TItem>(queryType, expression);

            throw new NotSupportedException("不支持的方法 " + method.Name);
        }

        int ExecuteCount(Type queryType, Expression expression)
        {
            LinqToSqlVisitor vistor = new LinqToSqlVisitor(new QueryEntityManager(queryType), expression, SqlMode.Count);
            var sql = vistor.SqlBuilder.GetQuerySql();

            var session = (INHibernateShardSession)RepositoryFramework.GetSessionFactory(queryType).OpenSession(queryType, ShardParams.Empty);
            var query = session.InnerSession.CreateSQLQuery(sql).AddScalar("c", NHibernateUtil.Int32);
            return (int)query.UniqueResult();
        }

        IList<TItem> ExecuteList<TItem>(Type queryType, Expression expression)
        {
            var resultType = typeof(TItem);
            LinqToSqlVisitor vistor = new LinqToSqlVisitor(new QueryEntityManager(queryType, resultType), expression, SqlMode.Query);
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

        PagingResult<TItem> ExecutePaging<TItem>(Type queryType, Expression expression)
        {
            int count = ExecuteCount(queryType, expression);
            var paging = new PagingResult<TItem>(count);
            if (count > 0)
            {
                var items = ExecuteList<TItem>(queryType, expression);
                paging.AddRange(items);
            }
            return paging;
        }
    }
}
