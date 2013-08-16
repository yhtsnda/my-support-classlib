using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Nova.Parser
{
    public interface IUnaryOperandCalculator
    {
        object Calculate(int num);
        object Calculate(long num);
        object Calculate(BigInteger num);
        object Calculate(decimal num);
    }
}
