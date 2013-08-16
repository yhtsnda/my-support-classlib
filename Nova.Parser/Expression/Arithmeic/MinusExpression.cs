using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class MinusExpression : UnaryOperatorExpression, IUnaryOperandCalculator
    {
        public override string GetOperator()
        {
            throw new NotImplementedException();
        }

        public object Calculate(int num)
        {
            throw new NotImplementedException();
        }

        public object Calculate(long num)
        {
            throw new NotImplementedException();
        }

        public object Calculate(System.Numerics.BigInteger num)
        {
            throw new NotImplementedException();
        }

        public object Calculate(decimal num)
        {
            throw new NotImplementedException();
        }
    }
}
