using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Avalon.Framework.Configurations
{
    internal class ServiceConfiguation
    {
        IEnumerable<Assembly> assemblies;
        IDependencyRegister register;

        public ServiceConfiguation(IEnumerable<Assembly> assemblies, IDependencyRegister register)
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
                }
            }
        }
    }
}
