﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class TimediffFunc : FunctionExpression
    {
        public TimediffFunc(List<IExpression> arguments)
            : base("TIMEDIFF", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new TimediffFunc(arguments);
        }
    }
}
