using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class NegativeValueExpression : UnaryOperatorExpression
    {
        public NegativeValueExpression(IExpression operand)
            : base(operand, ExpressionPrecedence.UnaryOp)
        {

        }

        public override string GetOperator()
        {
            return "!";
        }

        protected override object EvaluationInternal(IDictionary<object, object> parameters)
        {
            object operand = Operand.Evaluation(parameters);
            if (operand == null)
                return null;
            if (operand == UnEvaluatable)
                return UnEvaluatable;
            bool boolVal = false;
            if (Boolean.TryParse(operand.ToString(), out boolVal))
            {
                return boolVal ? LiteralBoolean.FALSE : LiteralBoolean.TRUE;
            }
            return LiteralBoolean.FALSE;
        }
    }
}
