using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    public class DefaultDependency : IDependencyRegister
    {
        public void Register(Type interfaceType, Type instanceType)
        {
        }

        public void Register(Type interfaceType, object instance)
        {
        }

        public void RegisterType(Type implementationType)
        {
        }
    }
}
