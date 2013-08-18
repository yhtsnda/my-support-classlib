using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ArithmeticAddExpression : ArithmeticBinaryOperatorExpression
    {
        public override string GetOperator()
        {
            return "+";
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<ArithmeticAddExpression>(this);
        }
    }
}
