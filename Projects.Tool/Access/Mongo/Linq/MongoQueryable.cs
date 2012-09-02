using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Projects.Tool.MongoAccess
{
    public class MongoQueryable<TElement> : IQueryable<TElement>, IOrderedQueryable<TElement>, IQueryProvider
    {
        Expression expression;
        internal MongoQueryable()
            : this(null)
        {
        }

        internal MongoQueryable(Expression expression)
        {
            this.expression = expression ?? Expression.Constant(this);
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return Execute(expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Execute(expression).GetEnumerator();
        }

        public Type ElementType
        {
            get { return typeof(TElement); }
        }

        public Expression Expression
        {
            get { return expression; }
        }

        public IQueryProvider Provider
        {
            get { return this; }
        }

        IEnumerable<TElement> Execute(Expression expression)
        {
            MongoExpressionParser parser = new MongoExpressionParser(expression);
            return (IEnumerable<TElement>)parser.Execute();
        }


        IQueryable<T> IQueryProvider.CreateQuery<T>(Expression expression)
        {
            return new MongoQueryable<T>(expression);
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            Type elementType = expression.Type;
            return (IQueryable)Activator.CreateInstance(typeof(MongoQueryable<>).MakeGenericType(elementType), new object[] { expression });
        }

        TResult IQueryProvider.Execute<TResult>(Expression expression)
        {
            MongoExpressionParser parser = new MongoExpressionParser(expression);
            return (TResult)parser.Execute();
        }

        object IQueryProvider.Execute(Expression expression)
        {
            MongoExpressionParser parser = new MongoExpressionParser(expression);
            return parser.Execute();
        }
    }
}
