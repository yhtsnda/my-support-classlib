using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Projects.Framework
{
    public static class DependencyResolver
    {
        /// <summary>
        /// 解释器
        /// </summary>
        private static IDependencyResolver resolver;
        /// <summary>
        /// 注册器
        /// </summary>
        private static IDependencyRegister register;

        public static IDependencyResolver Current
        {
            get { return resolver; }
        }

        /// <summary>
        ///解释指定接口类型并获取实现实例
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            return resolver.Resolve<T>();
        }

        /// <summary>
        /// 解释指定接口类型并获取实现实例
        /// </summary>
        /// <param name="interfaceType">接口类型.</param>
        /// <returns></returns>
        public static object Resolve(Type interfaceType)
        {
            return resolver.Resolve(interfaceType);
        }

        /// <summary>
        /// 解释指定接口类型并获取所有实现实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> ResolveAll<T>()
        {
            return resolver.ResolveAll<T>();
        }

        /// <summary>
        /// 解释指定接口类型并获取所有实现实例
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <returns></returns>
        public static IEnumerable ResolveAll(Type interfaceType)
        {
            return resolver.ResolveAll(interfaceType);
        }

        /// <summary>
        /// 释放指定实例
        /// </summary>
        /// <param name="instance"></param>
        public static void Release(object instance)
        {
            resolver.Release(instance);
        }

        /// <summary>
        /// 注册接口类型及相应的实现类型
        /// </summary>
        /// <typeparam name="I">接口类型</typeparam>
        /// <typeparam name="T">实现该接口的类型</typeparam>
        public static void Register<I, T>() where T : I, new()
        {
            register.Register<I, T>();
        }

        /// <summary>
        /// 注册接口类型及相应的实现实例
        /// </summary>
        /// <typeparam name="I">接口类型</typeparam>
        /// <param name="instance">实现该接口的实例</param>
        public static void Register<I>(I instance)
        {
            register.Register<I>(instance);
        }

        /// <summary>
        ///  注册接口类型及相应的实现类型
        /// </summary>
        /// <param name="interfaceType">接口类型</param>
        /// <param name="instanceType">实现该接口的类型</param>
        public static void Register(Type interfaceType, Type instanceType)
        {
            register.Register(interfaceType, instanceType);
        }

        /// <summary>
        /// 注册接口类型及相应的实现实例
        /// </summary>
        /// <param name="interfaceType">接口类型</param>
        /// <param name="instance">实现该接口的实例</param>
        public static void Register(Type interfaceType, object instance)
        {
            register.Register(interfaceType, instance);
        }

        /// <summary>
        /// 设置解释器实例
        /// </summary>
        /// <param name="resolver"></param>
        public static void SetResolver(IDependencyResolver resolver)
        {
            if (resolver == null)
                throw new ArgumentNullException("resolver");

            DependencyResolver.resolver = resolver;
        }

        /// <summary>
        /// 设置注册器实例
        /// </summary>
        /// <param name="register"></param>
        public static void SetRegister(IDependencyRegister register)
        {
            if (register == null)
                throw new ArgumentNullException("register");
            DependencyResolver.register = register;
        }

        /// <summary>
        /// 执行自动搜索及注册
        /// </summary>
        /// <param name="interfaceAssemblies"></param>
        /// <param name="instanceAssemblies"></param>
        public static void RegisterBatch<I>(IEnumerable<string> interfaceAssemblies, IEnumerable<string> instanceAssemblies)
        {
            RegisterBatch(typeof(I), instanceAssemblies, instanceAssemblies);
        }

        public static void RegisterBatch(Type baseType, IEnumerable<string> interfaceAssemblies, IEnumerable<string> instanceAssemblies)
        {
            if (!baseType.IsInterface)
                throw new ArgumentException(baseType.FullName + " 不是接口。");

            List<Type> interfaces = new List<Type>();
            foreach (string assemblyString in interfaceAssemblies)
            {
                var assembly = Assembly.Load(assemblyString);
                var types = assembly.GetExportedTypes();

                interfaces.AddRange(types.Where(type => type.IsInterface && IsDeriveFrom(type, baseType)));
            }

            List<Type> instances = new List<Type>();
            foreach (string assemblyString in instanceAssemblies)
            {
                var assembly = Assembly.Load(assemblyString);
                var types = assembly.GetExportedTypes();

                instances.AddRange(types.Where(type => IsDeriveFrom(type, baseType) && type.IsClass && !type.IsAbstract));
            }

            instances.ForEach(instance =>
            {
                var bt = interfaces.FirstOrDefault(@interface => IsDeriveFrom(instance, @interface));
                if (bt != null)
                    Register(bt, instance);
            });
        }

        static bool IsDeriveFrom(Type type, Type baseType)
        {
            if (baseType.IsInterface)
                return type.GetInterface(baseType.FullName) != null;

            return type.IsSubclassOf(baseType);
        }
    }
}
