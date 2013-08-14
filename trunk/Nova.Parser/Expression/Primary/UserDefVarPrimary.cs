using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class UserDefVarPrimary : PrimaryExpression
    {
        public string VarText { get; protected set; }

        public UserDefVarPrimary(string varText)
        {
            this.VarText = varText;
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<UserDefVarPrimary>(this);
        }
    }
}
