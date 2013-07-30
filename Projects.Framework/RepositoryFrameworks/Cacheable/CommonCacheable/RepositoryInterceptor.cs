using Castle.DynamicProxy;
using Projects.Tool;
using Projects.Tool.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    internal class RepositoryInterceptor : IInterceptor
    {
        ILog log = LogManager.GetLogger<RepositoryInterceptor>();

        static List<IRepositoryMethodInvocation> invocations;

        static RepositoryInterceptor()
        {
            invocations = new List<IRepositoryMethodInvocation>()
            {
                new RepositoryPersistentMethodInvocation(),
                new RepositoryGetMethodInvocation(),
                new RepositoryGetListMethodInvocation()
            };
        }

        public void Intercept(IInvocation invocation)
        {
            var methodName = invocation.Method.Name;
            bool skip = methodName == "OpenSession" || methodName == "GetShardParams" || methodName == "CreateSpecification";
            if (!skip && (methodName == "GetList" || methodName == "Get"))
            {
                var arguments = invocation.Method.GetParameters();
                skip = arguments.Length != 2 || arguments[0].ParameterType != typeof(ShardParams);
            }
            if (!skip)
                ProfilerContext.BeginWatch(invocation.TargetType.FullName + "." + invocation.Method.Name);

            try
            {
                var entityType = ReflectionHelper.GetEntityTypeFromRepositoryType(invocation.TargetType);
                var metadata = RepositoryFramework.GetDefineMetadata(entityType);

                if (metadata != null)
                {
                    var mi = invocations.FirstOrDefault(o => o.IsMatch(invocation));
                    if (mi != null)
                    {
                        mi.Process(invocation, metadata);
                        ProxyEntity(invocation);
                        return;
                    }
                }
                if (skip)
                {
                    invocation.Proceed();
                }
                else
                {
                    using (MonitorContext.Repository(invocation.Method))
                        invocation.Proceed();
                }

                ProxyEntity(invocation);
            }
            finally
            {
                if (!skip)
                    ProfilerContext.EndWatch();
            }
        }


        /// <summary>
        /// 对返回对象进行代理处理
        /// </summary>
        void ProxyEntity(IInvocation invocation)
        {
            var data = invocation.ReturnValue;
            if (data != null)
                ProxyProvider.Proxy(data);
        }
    }
}
