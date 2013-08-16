using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ArithmeticModExpression : ArithmeticBinaryOperatorExpression
    {
        public override string GetOperator()
        {
            throw new NotImplementedException();
        }

        public override void Accept(IASTVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}
