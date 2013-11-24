using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Avalon.Framework.Configurations
{
    internal class CommandConfiguation
    {
        IList<Assembly> assemblies;
        IDependencyRegister register;

        public CommandConfiguation(IEnumerable<Assembly> assemblies, IDependencyRegister register)
        {
            this.assemblies = new List<Assembly>(assemblies);
            this.register = register;
        }

        protected void RegisterCommands()
        {
            var baseType = typeof(ICommandExecutor<>);
            foreach (var assembly in assemblies)
            {
                try
                {
                    if (assembly.IsDynamic) continue;

                    var types = assembly.GetExportedTypes();
                    foreach (var type in types)
                    {
                        foreach (var item in type.GetInterfaces())
                        {
                            if (item.IsInterface && item.IsGenericType && item.GetGenericTypeDefinition() == baseType)
                            {
                                register.Register(item, type);
                                break;
                            }
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void Load()
        {
            RegisterCommands();
            register.Register(typeof(ICommandBus), typeof(DefaultCommandBus));
        }
    }
}
