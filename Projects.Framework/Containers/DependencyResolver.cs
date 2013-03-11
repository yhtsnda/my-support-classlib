using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    public static class DependencyResolver
    {
        private static IDependencyResolver _resolver;

        public static void SetResolver(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        public static T Resolve<T>()
        {
            return _resolver.Resolve<T>();
        }

        public static object Resolve(Type interfaceType)
        {
            return _resolver.Resolve(interfaceType);
        }

        public static T ResolveOptional<T>()
        {
            return _resolver.ResolveOptional<T>();
        }

        public static object ResolveOptional(Type interfaceType)
        {
            return _resolver.ResolveOptional(interfaceType);
        }
    }
}
