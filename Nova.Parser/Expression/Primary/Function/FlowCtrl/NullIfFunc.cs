﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class NullIfFunc : FunctionExpression
    {
        public NullIfFunc(List<IExpression> arguments)
            : base("NULLIF", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new NullIfFunc(arguments);
        }
    }
}
