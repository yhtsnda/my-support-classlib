using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class CaseWenOperatorExpression : PrimaryExpression
    {

        public CaseWenOperatorExpression(IExpression comparee, Dictionary<IExpression, IExpression> whenList,
                                      IExpression elseResult)
        {
            this.Comparee = comparee;
            this.WhenList = whenList;
            this.ElseResult = elseResult;
        }

        public IExpression Comparee { get; protected set; }
        public Dictionary<IExpression, IExpression> WhenList { get; protected set; }
        public IExpression ElseResult { get; protected set; }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<CaseWenOperatorExpression>(this);
        }
    }
}
