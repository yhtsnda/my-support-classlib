using Castle.DynamicProxy;
using Projects.Tool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Projects.Framework
{
    internal class EntityInterceptor : IInterceptor
    {
        ILog log = LogManager.GetLogger<EntityInterceptor>();
        ClassDefineMetadata metadata;
        Hashtable caches = new Hashtable();

        public EntityInterceptor(ClassDefineMetadata metadata)
        {
            this.metadata = metadata;
        }

        public bool Inited
        {
            get;
            set;
        }

        public bool JoinChanged
        {
            get { return caches.Count > 0; }
        }

        public bool PropertyInited(string member)
        {
            return caches.ContainsKey(member);
        }

        public void Intercept(IInvocation invocation)
        {
            var entity = invocation.InvocationTarget;
            if (Inited)
            {
                var joinDefine = metadata.GetClassJoinDefineMetadata(invocation.Method);
                if (joinDefine != null)
                {
                    if (joinDefine.JoinType == MethodJoinType.PropertySet)
                    {
                        var data = invocation.Arguments[0];
                        if (data == null)
                            caches.Remove(joinDefine.JoinName);
                        else
                            caches[joinDefine.JoinName] = data;
                        invocation.Proceed();
                        return;
                    }
                    else if (joinDefine.JoinType == MethodJoinType.PropertyGet)
                    {
                        if (caches.ContainsKey(joinDefine.JoinName))
                        {
                            invocation.ReturnValue = caches[joinDefine.JoinName];
                            return;
                        }
                    }

                    ProfilerContext.Current.Trace("platform", String.Format("intercept method {0}.{1}", invocation.Method.DeclaringType.ToPrettyString(), invocation.Method.Name));

                    log.DebugFormat("intercept property {0}.{1}", invocation.Method.DeclaringType.ToPrettyString(), invocation.Method.Name);
                    invocation.ReturnValue = joinDefine.DataProcesser.Process(entity);

                    if (joinDefine.JoinType == MethodJoinType.PropertyGet)
                    {
                        caches[joinDefine.JoinName] = invocation.ReturnValue;
                    }
                    return;
                }
            }
            invocation.Proceed();
        }

        public static bool PropertyInited(object entity, string member)
        {
            var proxy = entity as IProxyTargetAccessor;
            if (proxy != null)
            {
                EntityInterceptor ei = (EntityInterceptor)proxy.GetInterceptors().FirstOrDefault(o => o.GetType() == typeof(EntityInterceptor));
                if (ei != null)
                    return ei.PropertyInited(member);
            }
            return true;
        }
    }
}
