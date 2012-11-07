using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;

namespace Projects.Tool.Interceptor
{
    public interface IInnerInvocation
    {
        bool IsMatch(IInvocation invocation);
        void Process(IInvocation invocation);
    }
}
