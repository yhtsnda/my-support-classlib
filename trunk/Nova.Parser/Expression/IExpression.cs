using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public interface IExpression : IASTNode
    {
        object UnEvaluatable { get; }

        ExpressionPrecedence GetPrecedence();

        IExpression SetCacheEvalRst(bool cacheEvalRst);

        Object Evaluation(IDictionary<object, object> parameters);
    }
}
