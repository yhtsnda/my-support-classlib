﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Querys
{
    public class PropertyExpressionNode : ExpressionNode
    {
        public PropertyExpressionNode(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; set; }
    }
}
