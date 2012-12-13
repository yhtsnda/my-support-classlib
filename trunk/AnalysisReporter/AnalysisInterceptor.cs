using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Castle.DynamicProxy;

namespace AnalysisReporter
{
    internal class AnalysisInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var entity = invocation.InvocationTarget; ;
            if (entity != null)
            {
                Type entityType = entity.GetType();
                var arguments = invocation.Arguments;
                for (int count = 0; count < arguments.Length; count++)
                {
                    Type argType = arguments[count].GetType();
                    var attrs = argType.GetCustomAttributes(typeof(AnalysisAttribute), false);
                    //参数存在分析属性
                    if (attrs.Count == 0)
                    {

                    }
                }
            }
            invocation.Proceed();
        }
    }
}
