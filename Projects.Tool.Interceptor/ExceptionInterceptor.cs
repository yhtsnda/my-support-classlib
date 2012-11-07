using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Tool.Exceptions;
using Castle.DynamicProxy;

namespace Projects.Tool.Interceptor
{
    public class ExceptionInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                invocation.ReturnValue = GetReturnValueByType(invocation.Method.ReturnType);
                bool handled = ExceptionManager.HandleException(ex);
                if (!handled)
                    throw;
            }
        }

        private object GetReturnValueByType(Type type)
        {
            if (type.IsClass || type.IsInterface)
                return null;
            if (type == typeof(void))
                return null;

            return Activator.CreateInstance(type);
        }
    }
}
