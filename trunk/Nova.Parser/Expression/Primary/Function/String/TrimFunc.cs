using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class TrimFunc : FunctionExpression
    {
        public enum Direction
        {
            Default,
            Both,
            Leading,
            Trailing
        }

        public TrimFunc.Direction Direc { get; protected set; }

        public TrimFunc(TrimFunc.Direction direction, IExpression remstr, IExpression str)
            : base("TRIM", WrapList(str, remstr))
        {
            this.Direc = direction;
        }

        public IExpression GetString()
        {
            return Arguments.FirstOrDefault();
        }

        public IExpression GetRemainString()
        {
            List<IExpression> args = Arguments;
            if (args.Count < 2)
                return null;
            return Arguments.ElementAt(1);
        }
        
        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            throw new NotImplementedException("function of trim has special arguments");
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<TrimFunc>(this);
        }

        private static List<IExpression> WrapList(IExpression str, IExpression remstr)
        {
            if (str == null)
                throw new ArgumentException("str is null");
            List<IExpression> list = remstr != null ?
                new List<IExpression>(2) :
                new List<IExpression>(1);
            list.Add(str);
            if (remstr != null)
                list.Add(remstr);
            return list;
        }
    }
}
