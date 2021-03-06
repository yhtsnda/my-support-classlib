﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class DesEncryptFunc: FunctionExpression
    {
        public DesEncryptFunc(List<IExpression> arguments)
            : base("DES_ENCRYPT", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new DesEncryptFunc(arguments);
        }
    }
}
