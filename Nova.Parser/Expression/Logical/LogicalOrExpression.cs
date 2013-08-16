using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class LogicalOrExpression : PolyadicOperatorExpression
    {
        public LogicalOrExpression()
            : base(ExpressionPrecedence.LogicalOR)
        {
        }

        public override string GetOperator()
        {
            return "OR";
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
                    if (boolVal)
                        return LiteralBoolean.TRUE;
                }
            }
            return LiteralBoolean.FALSE;
        }

        public override void Accept(IASTVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}
