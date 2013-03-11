using Castle.DynamicProxy;
using Castle.DynamicProxy.Internal;
using Projects.Tool.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    public class EntityProxy
    {
        public static object Create(ClassDefineMetadata matadata)
        {
            throw new NotImplementedException();
            //return ProxyProvider.Generator.CreateClassProxyWithTarget(matadata.EntityType, target, new EntityProxyInterceptor());
        }

        public static void SetJoinProperties(ClassDefineMetadata metadata, object entity)
        {
            var pa = PropertyAccessorFactory.GetPropertyAccess(metadata.EntityType);
            foreach (var property in metadata.CascadeProperties)
            {
                var joinMetadata = metadata.GetClassJoinDefineMetadata(property.GetGetMethod(true));
                var target = FastActivator.Create(property.PropertyType);
                var proxy = ProxyProvider.Generator.CreateClassProxyWithTarget(
                    metadata.EntityType,
                    target,
                    new EntityProxyInterceptor(entity, joinMetadata)
                    );
                pa.GetSetter(property.Name).Set(entity, proxy);
            }
        }
    }

    public class EntityProxyInterceptor : IInterceptor
    {
        object entity;
        ClassJoinDefineMetadata joinMetadata;
        bool inited;

        public EntityProxyInterceptor(object entity, ClassJoinDefineMetadata joinMetadata)
        {
            this.entity = entity;
            this.joinMetadata = joinMetadata;
        }

        public void Intercept(IInvocation invocation)
        {
            if (!inited)
            {
                var target = InitEntity(invocation);

            }
            else
            {
                invocation.Proceed();
            }
        }

        object InitEntity(IInvocation invocation)
        {
            var proxy = invocation.Proxy;
            var target = joinMetadata.DataProcesser.Process(entity);
            EntityProxy.SetJoinProperties(RepositoryFramework.GetDefineMetadataAndCheck(target.GetType()), target);

            TargetAccessImpl.GetAcess(proxy.GetType()).Set(proxy, target);
            //set invocation target field
            inited = true;
            return target;
        }
    }
}
