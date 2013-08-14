using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class GroupConcatFunc : FunctionExpression
    {
        public bool Distinct { get; protected set; }
        public IExpression OrderBy { get; protected set; }
        public bool IsDesc { get; protected set; }
        public List<IExpression> AppendedColumnNames { get; protected set; }
        public string Separator { get; protected set; }

        public GroupConcatFunc(bool distinct, List<IExpression> exprList, IExpression orderBy, bool isDesc,
                       List<IExpression> appendedColumnNames, string separator)
            : base("GROUP_CONCAT", arguments)
        {
            this.Distinct = distinct;
            this.OrderBy = orderBy;
            this.IsDesc = isDesc;
            this.AppendedColumnNames = appendedColumnNames;
            this.Separator = separator == null ? "," : separator;
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            throw new NotImplementedException("function of char has special arguments");
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<GroupConcatFunc>(this);
        }
    }
}
