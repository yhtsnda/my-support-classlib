using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool.Interceptor
{
    public interface IInterceptor
    {
        //方法
        void Intercept(IInvocation invocation);
    }
}
