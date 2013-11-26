using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Avalon.Framework.Configurations
{
    internal class RepositoryConfiguation
    {
        IEnumerable<Assembly> assemblies;
        IDependencyRegister register;

        public RepositoryConfiguation(IEnumerable<Assembly> assemblies, IDependencyRegister register)
        {
            this.assemblies = assemblies;
            this.register = register;
        }

        public void Load()
        {
            var repositoryType = typeof(IRepository).FullName;
            var baseType = typeof(IRepository);

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(o => o.IsClass && !o.IsAbstract && !o.IsGenericTypeDefinition && o.GetInterface(repositoryType) != null);
                foreach (var type in types)
                {
                    RegisterRepository(type);
                }
            }
        }

        void RegisterRepository(Type type)
        {
            var baseType = typeof(IRepository);
            if (type.GetInterface(baseType.FullName) == null)
                throw new PlatformException("给定的类型 {0} 必须实现 {1} 接口", type.FullName, baseType.FullName);

            //验证方法virtual
            EntityUtil.CheckVirtualType(type);

            //使用继承的方式创建代理
            var proxy = ProxyProvider.Generator.CreateClassProxy(type, new RepositoryInterceptor());
            var interfaces = type.GetInterfaces().Where(o => o != baseType);
            foreach (var i in interfaces)
            {
                this.register.Register(i, proxy);
            }
        }
    }
}
