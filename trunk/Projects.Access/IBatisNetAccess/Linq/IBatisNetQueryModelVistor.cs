using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Collections;
using Remotion.Linq.Clauses.ResultOperators;

namespace Projects.Accesses.IBatisNetAccess.Linq
{
    internal class IBatisNetQueryModelVistor : QueryModelVisitorBase
    {
        private IBatisNetQueryData mQueryData = new IBatisNetQueryData();

        public override void VisitOrderByClause(OrderByClause orderByClause, QueryModel queryModel, int index)
        {
            base.VisitOrderByClause(orderByClause, queryModel, index);
        }

        public override void VisitGroupJoinClause(GroupJoinClause groupJoinClause, QueryModel queryModel, 
            int index)
        {
            base.VisitGroupJoinClause(groupJoinClause, queryModel, index);
        }

        public override void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index)
        {
            base.VisitWhereClause(whereClause, queryModel, index);
        }

        public override void VisitSelectClause(SelectClause selectClause, QueryModel queryModel)
        {
            base.VisitSelectClause(selectClause, queryModel);
        }

        public override void VisitResultOperator(ResultOperatorBase resultOperator, 
            QueryModel queryModel, int index)
        {
            if (resultOperator is SkipResultOperator)
                mQueryData.SkipValue = ((SkipResultOperator)resultOperator).GetConstantCount();
            else if (resultOperator is TakeResultOperator)
                mQueryData.TakeValue = ((TakeResultOperator)resultOperator).GetConstantCount();
            else
                throw new NotSupportedException();
            base.VisitResultOperator(resultOperator, queryModel, index);
        }
    }
}
