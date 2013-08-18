using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Nova.Parser
{
    public interface IBinaryOperandCalculator
    {
        TNumber Calculate<TNumber>(TNumber number1, TNumber number2);
    }
}
