using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.DynamicProxy;

using Projects.Tool;

namespace Projects.Framework
{
    internal class EntityInterceptor : IInterceptor
    {
        ILog mLog = LogManager.GetLogger<EntityInterceptor>();

        public void Intercept(IInvocation invocation)
        {
            var entity = invocation.InvocationTarget;
            if (entity != null)
            {
                Type entityType = entity.GetType();
                ClassDefineMetadata metadata = RepositoryFramework.GetDefineMetadata(entityType);
                if (metadata != null)
                {
                    var joinDefine = metadata.GetClassJoinDefineMetadata(invocation.Method);
                    if (joinDefine != null)
                    {
                        mLog.DebugFormat("framework", String.Format("Intercept Property {0}.{1}",
                            invocation.Method.DeclaringType.ToPrettyString(),
                            invocation.Method.Name));
                        invocation.ReturnValue = joinDefine.Processer.Process(entity);
                        return;
                    }
                }
            }
        }
    }
}
