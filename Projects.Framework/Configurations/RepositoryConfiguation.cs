using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Projects.Framework.Configurations
{
    internal class RepositoryConfiguation
    {
        IList<Assembly> assemblies;
        IDependencyRegister register;

        public RepositoryConfiguation(IList<Assembly> assemblies, IDependencyRegister register)
        {
            this.assemblies = assemblies;
            this.register = register;
        }

        public void Load()
        {
            var repositoryType = typeof(IRepository<>).FullName;
            var baseType = typeof(IRepository);

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(o => o.IsClass && !o.IsAbstract && !o.IsGenericTypeDefinition && o.GetInterface(repositoryType) != null);
                foreach (var type in types)
                {
                    //验证方法virtual
                    EntityUtil.CheckVirtualType(type);

                    var entityType = ReflectionHelper.GetEntityTypeFromRepositoryType(type);
                    var metadata = RepositoryFramework.GetDefineMetadataAndCheck(entityType);

                    //使用继承的方式创建代理
                    var proxy = ProxyProvider.Generator.CreateClassProxy(type, new RepositoryInterceptor());
                    var interfaces = type.GetInterfaces().Where(o => o != baseType);
                    foreach (var i in interfaces)
                    {
                        this.register.Register(i, proxy);
                        //ServiceLocator.Current.Register(i, proxy);
                        //Container.Register(i, proxy);
                    }
                }
            }
        }
    }
}
