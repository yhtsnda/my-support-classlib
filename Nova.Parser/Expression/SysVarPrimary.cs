using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class SysVarPrimary : VariableExpression
    {
        public SysVarPrimary(VariableScope scope, string varText, string varTextUp)
        {
            this.Scope = scope;
            this.VarText = varText;
            this.VarTextUp = varTextUp;
        }

        public VariableScope Scope { get; protected set; }
        public string VarText { get; protected set; }
        public string VarTextUp { get; protected set; }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<SysVarPrimary>(this);
        }
    }
}
