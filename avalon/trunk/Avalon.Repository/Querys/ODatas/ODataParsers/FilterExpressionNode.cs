﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Querys
{
    public class FilterExpressionNode : ExpressionNode
    {
        public ExpressionNode Body
        {
            get { return Expressions.First(); }
        }
    }
}