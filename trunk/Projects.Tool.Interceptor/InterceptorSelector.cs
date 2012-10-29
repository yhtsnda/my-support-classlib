using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Projects.Tool;
using Castle.DynamicProxy;

namespace Projects.Tool.Interceptor
{
    public sealed class InterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var interceptorTypes = InterceptorManager.Interceptors;
            List<IInterceptor> selectedInterceptors = new List<IInterceptor>();

            if (interceptorTypes == null)
            {
                if (type.BaseType != null && type.BaseType != typeof(Object))
                {
                    Type baseType = type.BaseType;
                    MethodInfo methodInfoBase = null;
                    while (baseType != null && type.BaseType != typeof(Object))
                    {
                        methodInfoBase = GetMethodInfoBase(baseType, method);
                        if (methodInfoBase != null)
                            break;
                        baseType = baseType.BaseType;
                    }
                    if (baseType != null && methodInfoBase != null)
                    {
                        ;
                    }
                }
                if (interceptorTypes == null)
                {
                    var intfTypes = type.GetInterfaces();
                    if (intfTypes != null && intfTypes.Count() > 0)
                    {
                        foreach (var intfType in intfTypes)
                        {
                            var methodInfoBase = GetMethodInfoBase(intfType, method);
                            if (methodInfoBase != null)
                            {
                                ;
                            }
                            if (interceptorTypes != null)
                                break;
                        }
                    }
                }
            }

            if (interceptorTypes != null && interceptorTypes.Count() > 0)
            {
                foreach (var interceptor in interceptors)
                {
                    if (interceptorTypes.Any(item => interceptor.GetType().AssemblyQualifiedName.Equals(item)))
                    {
                        selectedInterceptors.Add(interceptor);
                    }
                }
            }
            //返回拦截器的列表
            return selectedInterceptors.ToArray();
        }

        private MethodInfo GetMethodInfoBase(Type baseType, MethodInfo thisMethod)
        {
            MethodInfo[] methods = baseType.GetMethods();
            var methodQuery = methods.Where(method =>
            {
                var retval = method.Name == thisMethod.Name &&
                    method.IsGenericMethod == thisMethod.IsGenericMethod &&
                    ((method.GetParameters() == null && thisMethod.GetParameters() == null) ||
                    (method.GetParameters().Length == thisMethod.GetParameters().Length));
                if (!retval)
                    return false;

                var thisMethodParameters = thisMethod.GetParameters();
                var pMethodParameters = method.GetParameters();
                for (int i = 0; i < thisMethodParameters.Length; i++)
                {
                    retval &= pMethodParameters[i].ParameterType == thisMethodParameters[i].ParameterType;
                }
                return retval;
            });

            if (methodQuery != null && methodQuery.Count() > 0)
                return methodQuery.Single();
            return null;
        }
    }
}
