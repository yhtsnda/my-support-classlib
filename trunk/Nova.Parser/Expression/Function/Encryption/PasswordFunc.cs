﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class PasswordFunc: FunctionExpression
    {
        public PasswordFunc(List<IExpression> arguments)
            : base("PASSWORD", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new PasswordFunc(arguments);
        }
    }
}
