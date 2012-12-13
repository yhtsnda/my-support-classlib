using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

using MongoDB.Driver.Builders;
using MongoDB.Bson;
using MongoDB.Driver;

using Remotion.Linq;
using Remotion.Linq.Parsing;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ResultOperators;

namespace Projects.Accesses.MongoAccess
{
    internal class MongoExpressionTreeVisitor : ThrowingExpressionTreeVisitor
    {
        string name;
        BsonValue value;
        bool? isNot;
        Stack<IMongoQuery> queryStack = new Stack<IMongoQuery>();

        public static IMongoQuery GetQueryComplete(Expression expression)
        {
            var vistor = new MongoExpressionTreeVisitor();
            vistor.VisitExpression(expression);
            return vistor.GetQueryComplete();
        }

        protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
        {
            throw new NotImplementedException();
        }

        protected override Expression VisitUnaryExpression(UnaryExpression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Not:
                    isNot = true;
                    VisitExpression(expression.Operand);
                    if (isNot != null)
                        return base.VisitUnaryExpression(expression);

                    return expression;
                case ExpressionType.Convert:
                    return expression;
            }
            return base.VisitUnaryExpression(expression);
        }

        protected override Expression VisitBinaryExpression(BinaryExpression expression)
        {
            VisitExpression(expression.Left);
            VisitExpression(expression.Right);

            IMongoQuery query = null;
            switch (expression.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    var right1 = queryStack.Pop();
                    var left1 = queryStack.Pop();
                    query = Query.And(left1, right1);
                    break;

                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    var right2 = queryStack.Pop();
                    var left2 = queryStack.Pop();
                    query = Query.Or(left2, right2);
                    break;

                case ExpressionType.Equal:
                    query = Query.EQ(name, value);
                    break;
                case ExpressionType.NotEqual:
                    query = Query.NE(name, value);
                    break;
                case ExpressionType.GreaterThan:
                    query = Query.GT(name, value);
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    query = Query.GTE(name, value);
                    break;
                case ExpressionType.LessThan:
                    query = Query.LT(name, value);
                    break;
                case ExpressionType.LessThanOrEqual:
                    query = Query.LTE(name, value);
                    break;
                default:
                    break;
            }
            queryStack.Push(query);

            return expression;
        }

        protected override Expression VisitMemberExpression(MemberExpression expression)
        {
            VisitExpression(expression.Expression);
            name = MongoQueryModelVisitor.GetMemberName(expression);
            return expression;
        }

        protected override Expression VisitQuerySourceReferenceExpression(QuerySourceReferenceExpression expression)
        {
            var name = expression.ReferencedQuerySource.ItemName;
            return expression;
        }

        protected override Expression VisitConstantExpression(ConstantExpression expression)
        {
            var v = expression.Value;
            if (v is Array)
            {
                BsonArray array = new BsonArray();
                foreach (var vi in (Array)v)
                    array.Add(BsonValue.Create(vi));
                value = array;
            }
            else
            {
                value = BsonValue.Create(v);
            }

            return expression;
        }

        protected override Expression VisitSubQueryExpression(SubQueryExpression expression)
        {
            if (expression.QueryModel.MainFromClause.FromExpression.NodeType == ExpressionType.Constant && expression.QueryModel.ResultOperators.Count == 1)
            {
                var resultOperateor = expression.QueryModel.ResultOperators[0];
                if (resultOperateor is ContainsResultOperator)
                {
                    ContainsResultOperator containsOperateor = (ContainsResultOperator)resultOperateor;
                    VisitExpression(containsOperateor.Item);
                    VisitExpression(expression.QueryModel.MainFromClause.FromExpression);

                    if (isNot.HasValue && isNot.Value)
                        queryStack.Push(Query.NotIn(name, (BsonArray)value));
                    else
                        queryStack.Push(Query.In(name, (BsonArray)value));

                    isNot = null;

                    return expression;
                }
            }
            return base.VisitSubQueryExpression(expression);
        }

        public IMongoQuery GetQueryComplete()
        {
            if (queryStack.Count > 0)
                return queryStack.Pop();

            return null;
        }
    }
}
