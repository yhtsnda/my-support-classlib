using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Nova.Parser
{
    public abstract class ArithmeticBinaryOperatorExpression : BinaryOperatorExpression, IBinaryOperandCalculator
    {
        protected ArithmeticBinaryOperatorExpression(IExpression leftOprand, IExpression rightOprand,
            ExpressionPrecedence precedence)
            : base(leftOprand, rightOprand, precedence)
        {
        }

        public abstract TNumber Calculate<TNumber>(TNumber number1, TNumber number2);

        protected override object EvaluationInternal(IDictionary<object, object> parameters)
        {
            object left = LeftOprand.Evaluation(parameters);
            object right = RightOprand.Evaluation(parameters);

            if (left == null || right == null)
                return null;
            if (left == UnEvaluatable || right == UnEvaluatable)
                return UnEvaluatable;
            return null;
        }

        
    }
}
