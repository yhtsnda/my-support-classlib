using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;

namespace Projects.Tool.Interceptor
{
    public class LoggerInvocation : IInnerInvocation
    {
        public bool IsMatch(IInvocation invocation)
        {
            throw new NotImplementedException();
        }

        public void Process(IInvocation invocation)
        {
            throw new NotImplementedException();
        }
    }
}
