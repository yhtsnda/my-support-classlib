﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public interface IASTVisitor
    {
        void Visit<T>(T node) where T : IASTNode;
    }
}
