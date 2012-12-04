using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.DynamicProxy;

namespace Projects.Framework
{
    /// <summary>
    /// 使IRepository能够支持缓存
    /// </summary>
    public class CacheDependencyResolver : DefaultDependencyResolver
    {
        public List<Type> entityTypes = new List<Type>();

        public override void Register(Type interfaceType, object instance)
        {
            if (instance is IRepository)
            {
                var entityType = ReflectionHelper.GetEntityTypeFromRepositoryType(instance.GetType());
                entityTypes.Add(entityType);

                var metadata = RepositoryFramework.GetDefineMetadata(entityType);
                if (metadata != null)
                {
                    //使用继承的方式创建代理
                    var proxy = ProxyGeneratorUtil.Instance.CreateClassProxy(instance.GetType(),
                        new RepositoryInterceptor());
                    //var proxy = ProxyGeneratorUtil.Instance.CreateInterfaceProxyWithTarget(interfaceType, instance, new RepositoryInterceptor());
                    base.Register(interfaceType, proxy);
                    return;
                }
                else
                {
                    throw new NotSupportedException(String.Format("类型 {0} 未进行定义，因此无法实现仓储扩展。", entityType.FullName));
                }
            }
            base.Register(interfaceType, instance);
        }
    }
}
