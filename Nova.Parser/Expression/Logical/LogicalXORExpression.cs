using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class LogicalXORExpression : BinaryOperatorExpression
    {
        public LogicalXORExpression(IExpression left, IExpression right)
            : base(left, right, ExpressionPrecedence.LogicalXOR)
        {
        }

        public override string GetOperator()
        {
            return "XOR";
        }

        protected override object EvaluationInternal(IDictionary<object, object> parameters)
        {
            object left = LeftOprand.Evaluation(parameters);
            object right = RightOprand.Evaluation(parameters);
            if (left == null || right == null) return null;
            if (left == UnEvaluatable || right == UnEvaluatable) 
                return UnEvaluatable;
            bool b1 = false, b2 = false;
            if (Boolean.TryParse(left.ToString(), out b1) && Boolean.TryParse(right.ToString(), out b2))
                return b1 != b2 ? LiteralBoolean.TRUE : LiteralBoolean.FALSE;
            return LiteralBoolean.FALSE;
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<LogicalXORExpression>(this);
        }
    }
}
