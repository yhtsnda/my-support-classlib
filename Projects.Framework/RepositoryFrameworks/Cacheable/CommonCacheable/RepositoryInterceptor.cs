using Castle.DynamicProxy;
using Projects.Tool;
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
            using (var tr = ProfilerContext.Profile(invocation.TargetType.FullName + "." + invocation.Method.Name))
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
                using (MonitorImpl.Repository(invocation.Method))
                    invocation.Proceed();

                ProxyEntity(invocation);
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
