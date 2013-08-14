using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class MasterPosWaitFunc : FunctionExpression
    {
        public MasterPosWaitFunc(List<IExpression> args)
            : base("MASTER_POS_WAIT", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new MasterPosWaitFunc(arguments);
        }
    }
}
