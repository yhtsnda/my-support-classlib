using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Collections;
using Remotion.Linq.Clauses.ResultOperators;

using MongoDB.Bson.Serialization;

namespace Projects.Accesses.MongoAccess
{
    internal class MongoQueryModelVisitor : QueryModelVisitorBase
    {
        QueryData queryData = new QueryData();

        public static QueryData GetQueryData(QueryModel queryModel)
        {
            var vistor = new MongoQueryModelVisitor();
            vistor.VisitQueryModel(queryModel);
            return vistor.queryData;
        }

        public override void VisitMainFromClause(MainFromClause fromClause, QueryModel queryModel)
        {
            queryData.EntityType = fromClause.ItemType;
            base.VisitMainFromClause(fromClause, queryModel);
        }

        public override void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index)
        {
            queryData.Query = MongoExpressionTreeVisitor.GetQueryComplete(whereClause.Predicate);
            base.VisitWhereClause(whereClause, queryModel, index);
        }

        public override void VisitResultOperator(ResultOperatorBase resultOperator, QueryModel queryModel, int index)
        {
            if (resultOperator is CountResultOperator)
            {
                queryData.Operate = OperateType.Count;
            }
            else if (resultOperator is SkipResultOperator)
            {
                queryData.SkipValue = ((SkipResultOperator)resultOperator).GetConstantCount();
            }
            else if (resultOperator is TakeResultOperator)
            {
                queryData.TakeValue = ((TakeResultOperator)resultOperator).GetConstantCount();
            }
            else if (resultOperator is SingleResultOperator)
            {
                queryData.Operate = OperateType.Single;
            }
            else if (resultOperator is FirstResultOperator)
            {
                queryData.Operate = OperateType.First;
            }
            else
            {
                throw new NotSupportedException();
            }
            base.VisitResultOperator(resultOperator, queryModel, index);
        }

        public override void VisitOrdering(Ordering ordering, QueryModel queryModel, OrderByClause orderByClause, int index)
        {
            string name = GetMemberName(ordering.Expression);
            if (ordering.OrderingDirection == OrderingDirection.Asc)
                queryData.Sorts.Ascending(name);
            else
                queryData.Sorts.Descending(name);

            base.VisitOrdering(ordering, queryModel, orderByClause, index);
        }

        public override void VisitSelectClause(SelectClause selectClause, QueryModel queryModel)
        {
            MongoExpressionTreeVisitor.GetQueryComplete(selectClause.Selector);
            //todo
            base.VisitSelectClause(selectClause, queryModel);
        }

        public static string GetMemberName(Expression expression)
        {
            if (expression.NodeType != ExpressionType.MemberAccess)
                throw new ArgumentException();

            var member = ((MemberExpression)expression).Member;

            var bcm = BsonClassMap.LookupClassMap(member.ReflectedType);
            if (bcm != null && bcm.IdMemberMap != null && bcm.IdMemberMap.MemberInfo == member)
            {
                return "_id";
            }
            return ((MemberExpression)expression).Member.Name;
        }
    }
}
