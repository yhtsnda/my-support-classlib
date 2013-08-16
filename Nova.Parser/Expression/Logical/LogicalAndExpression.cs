using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class LogicalAndExpression : PolyadicOperatorExpression
    {
        public LogicalAndExpression()
            : base(ExpressionPrecedence.LogicalAnd)
        {
        }

        public override string GetOperator()
        {
            return "AND";
        }

        protected override object EvaluationInternal(IDictionary<object, object> parameters)
        {
            foreach (IExpression operand in operands)
            {
                object val = operand.Evaluation(parameters);
                if (val == null)
                    return null;
                if (val == UnEvaluatable)
                    return UnEvaluatable;
                bool boolVal = false;
                if (Boolean.TryParse(val.ToString(), out boolVal))
                {
                    if (!boolVal)
                        return LiteralBoolean.FALSE;
                }
            }
            return LiteralBoolean.TRUE;
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<LogicalAndExpression>(this);
        }
    }
}
