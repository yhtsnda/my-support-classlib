using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Projects.Tool.Interceptor
{
    public interface IInvocation
    {
        //方法
        object GetArgumentValue(int index);
        MethodInfo GetConcreteMethod();
        MethodInfo GetConcreteMethodInvocationTarget();
        void Proceed();
        void SetArgumentValue(int index, object value);

        //属性
        object[] Arguments { get; }
        Type[] GenericArguments { get; }
        object InvocationTarget { get; }
        MethodInfo Method { get; }
        MethodInfo MethodInvocationTarget { get; }
        object Proxy { get; }
        object ReturnValue { get; set; }
        Type TargetType { get; }
    }
}
