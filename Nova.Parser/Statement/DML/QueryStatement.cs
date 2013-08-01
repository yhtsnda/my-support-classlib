using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public abstract class QueryStatement : Statement, IQueryExpression
    {
        public abstract void Accept(IASTVisitor visitor);

        private object unEvaluatable = new object();

        public object UnEvaluatable
        {
            get { return unEvaluatable; }
        }

        public ExpressionPrecedence GetPrecedence()
        {
            return ExpressionPrecedence.Query;
        }

        public IExpression SetCacheEvalRst()
        {
            return this;
        }

        public object Evaluation<K, V>(IDictionary<K, V> parameters)
        {
            return UnEvaluatable;
        }
    }
}
