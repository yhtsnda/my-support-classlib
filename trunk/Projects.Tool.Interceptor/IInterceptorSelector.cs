using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Projects.Tool.Interceptor
{
    public interface IInterceptorSelector
    {
        IInterceptor[] SelectInterceptors(Type type, MethodInfo method);
    }
}
