using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Projects.Framework.Configurations
{
    internal class ServiceConfiguation
    {
        IList<Assembly> assemblies;
        IDependencyRegister register;

        public ServiceConfiguation(IList<Assembly> assemblies, IDependencyRegister register)
        {
            this.assemblies = assemblies;
            this.register = register;
        }

        public void Load()
        {
            var baseType = typeof(IService);
            var serviceType = baseType.FullName;

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(o => o.IsClass && !o.IsAbstract && !o.IsGenericTypeDefinition && o.GetInterface(serviceType) != null);
                foreach (var type in types)
                {
                    this.register.RegisterType(type);
                    //var interfaces = type.GetInterfaces().Where(o => o != baseType);
                    //foreach (var i in interfaces)
                    //{
                    //    this.register.Register(i, baseType);
                    //    //ServiceLocator.Current.Register(i, baseType);
                    //    //Container.Register(i, baseType);
                    //}
                }
            }
        }
    }
}
