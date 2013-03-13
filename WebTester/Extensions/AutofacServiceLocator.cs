using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;

using Autofac;
using Autofac.Integration.Mvc;

using WebTester;

namespace Projects.Framework.Web
{
    public class AutofacServiceLocator : IDependencyRegister, IDependencyResolver
    {
        private ContainerBuilder builder = new ContainerBuilder();
        private IContainer container;

        public IContainer Container
        {
            get
            {
                return container;
            }
        }

        public void RegisterRepository()
        {
            //初始化配置
            RepositoryFramework.Configure(this);
        }

        public void RegisterMvc()
        {
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();
        }

        public void Register(Type interfaceType, Type instanceType)
        {
            builder.RegisterType(instanceType).As(interfaceType).SingleInstance();
        }

        public void Register(Type interfaceType, object instance)
        {
            builder.RegisterInstance(instance).As(interfaceType);
        }

        public void RegisterType(Type implementationType)
        {
            builder.RegisterType(implementationType);
        }

        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return container.Resolve(type);
        }

        public T ResolveOptional<T>()
        {
            return (T)container.ResolveOptional(typeof(T));
        }

        public object ResolveOptional(Type type)
        {
            return container.ResolveOptional(type);
        }

        public void Build()
        {
            container = builder.Build();
            System.Web.Mvc.DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            Projects.Framework.DependencyResolver.SetResolver(this);
        }
    }
}
