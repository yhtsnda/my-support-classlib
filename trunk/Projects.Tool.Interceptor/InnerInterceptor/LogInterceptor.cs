using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.DynamicProxy;

namespace Projects.Tool.Interceptor
{
    public class LogInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var entity = invocation.TargetType;
            if (entity != null)
            {
                
            }
            invocation.Proceed();
        }
    }
}
